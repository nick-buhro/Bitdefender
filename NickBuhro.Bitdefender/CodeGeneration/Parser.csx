#r "Libs\\itextsharp.dll"
#load "Models.csx"
#load "TypeHelper.csx"

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

public sealed class Parser
{
    private const string ChapterName = "Reference";

    private readonly Regex _rxPageFooter = new Regex(
        @"^\s*([a-zA-Z0-9 \t]*[a-zA-Z0-9])\s+(\d+)\s*$",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

    private readonly Regex _rxHeadingController = new Regex(
        @"^\s*(2\.\d{1,2}\.)\s+(\w+)\s*$",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

    private readonly Regex _rxHeadingMethod = new Regex(
        @"^\s*(2\.\d{1,2}\.\d{1,2}\.)\s+(\w+)\s*$",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

    private readonly Regex _rxHeadingMethodSection = new Regex(
        @"^\s*((Parameters)|(Return value)|(Example))\s*$",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

    private readonly Regex _rxApiPath = new Regex(
        @"CONTROL_CENTER_APIs_ACCESS_URL([a-zA-Z0-9./_-]+)\s*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly Regex _rxParameterTableHeader = new Regex(
        @"^\s*Parameter\s+Type\s+Optional\s+Description\s*$",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

    private readonly Regex _rxParameterTableLine = new Regex(
        @"^\s*([a-zA-Z0-9_]+)\s+(Number|String|Boolean)\s+(Yes|No)\s+(\S.*)$",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

    private readonly string _pdfPath;


    public Parser(string pdfPath)
    {
        _pdfPath = pdfPath;
    }


    public void FillControllers(ControllerObject[] template)
    {
        var lines = GetChapterLines();
        VisitSubchapters(lines, _rxHeadingController, template, ControllerVisitor);

        Trace.Assert(template.All(m => m.Visited));
    }

    private void ControllerVisitor(List<string> lines, Match match, ControllerObject[] controllerCollection)
    {
        var paragraph = match.Groups[1].Value;
        var name = match.Groups[2].Value;

        var controller = controllerCollection.FirstOrDefault(c => c.Name == name);
        if (controller == null) return;

        controller.Visited = true;
        controller.Paragraph = paragraph;
        controller.SourceText = GetSourceText(lines, match);

        var firstMethodIndex = VisitSubchapters(lines, _rxHeadingMethod, controller, MethodVisitor);
        if (firstMethodIndex < 0)
            throw new Exception($"{name} doesn't contain subchapters.");

        var descr = string.Join(Environment.NewLine, lines.Take(firstMethodIndex));
        controller.Description = controller.Description ?? descr;

        var pathMatch = _rxApiPath.Match(descr);
        if (!pathMatch.Success)
            throw new Exception($"{name} API url not found.");
        controller.Path = pathMatch.Groups[1].Value;

        Trace.Assert(controller.Methods.All(m => m.Visited));
    }

    private void MethodVisitor(List<string> lines, Match match, ControllerObject controller)
    {
        var paragraph = match.Groups[1].Value;
        var name = match.Groups[2].Value;

        var method = controller.Methods.FirstOrDefault(m => m.Name == name);
        if (method == null) return;

        method.Visited = true;
        method.Paragraph = paragraph;
        method.SourceText = GetSourceText(lines, match);

        if (method.Parameters == null)
            method.Parameters = new List<ParameterObject>();

        var firstSubsectionIndex = VisitSubchapters(lines, _rxHeadingMethodSection, method, MethodSectionVisitor);

        var descr = string.Join(Environment.NewLine, lines.Take(firstSubsectionIndex));
        method.Description = method.Description ?? descr;

        Trace.Assert(method.Parameters.All(p => p.Visited));
    }

    private void MethodSectionVisitor(List<string> lines, Match match, MethodObject method)
    {
        if (match.Groups[2].Success)
        {
            UpdateParameters(lines, method);
        }
        else if (match.Groups[3].Success)
        {
            UpdateReturnValue(lines, method);
        }
    }

    private void UpdateParameters(List<string> lines, MethodObject method)
    {
        for (var i = lines.Count - 1; i >= 0; i--)
        {
            if (_rxParameterTableHeader.IsMatch(lines[i]))
                lines.RemoveAt(i);
        }

        VisitSubchapters(lines, _rxParameterTableLine, method, ParameterVisitor);
    }

    private void ParameterVisitor(List<string> lines, Match match, MethodObject method)
    {
        var name = match.Groups[1].Value;
        var type = match.Groups[2].Value;
        var opt = match.Groups[3].Value;
        var descr = match.Groups[4].Value.TrimEnd();

        var param = method.Parameters.FirstOrDefault(p => p.Name == name);
        if (param == null)
        {
            param = new ParameterObject() { Name = name };
        }
        else
        {
            method.Parameters.Remove(param);
        }

        method.Parameters.Add(param);
        param.Visited = true;
        param.SourceText = GetSourceText(lines, match);
        param.Optional = (opt == "Yes");

        if (param.Type == null)
            param.Type = ResolveType(type);
        
        if (param.Description == null)
            param.Description = descr + string.Join(null, lines.Select(l => " " + l));
    }

    private void UpdateReturnValue(List<string> lines, MethodObject method)
    {
        if (lines.Count == 0) return;
        if (method.ReturnType != null)
        {
            method.ReturnDescription = method.ReturnDescription ?? string.Join(Environment.NewLine, lines);
            return;
        }

        // "This method does not return any value."
        if (lines[0] == "This method does not return any value.")
        {
            method.ReturnType = "void";
            return;
        }

        // "This method returns a String: the ID of the newly-created company."
        var match = Regex.Match(lines[0], "^This method returns a (String|Number|Boolean): ");
        if (match.Success)
        {
            var descr = lines[0].Substring(match.Value.Length);
            descr = descr.Substring(0, 1).ToUpper() + descr.Substring(1);

            method.ReturnType = ResolveType(match.Groups[1].Value);
            method.ReturnDescription = descr.Trim();
            return;
        }
    }

    private List<string> GetChapterLines(string chapterName = ChapterName)
    {
        var lineSeparator = new[] { "\r\n", "\n" };
        var targetName = chapterName.ToUpperInvariant();

        var stage = 0;
        var result = new List<string>();
        foreach (var page in GetPdfPages())
        {
            var lines = page.Split(lineSeparator, StringSplitOptions.RemoveEmptyEntries);
            var match = _rxPageFooter.Match(lines[lines.Length - 1]);
            var name = match.Success
                ? match.Groups[1].Value.Trim().ToUpperInvariant()
                : null;

            // First pages before any chapter
            if (stage == 0)
            {
                if (match.Success)
                    stage = 1;
            }

            // Chapters before target
            if (stage == 1)
            {
                if (!match.Success)
                    throw new Exception("Can't recognize page chapter.");
                if (name == targetName)
                    stage = 2;
            }

            // Target chapter
            if (stage == 2)
            {
                if (!match.Success)
                    throw new Exception("Can't recognize page chapter.");
                if (name == targetName)
                {
                    var limit = lines.Length - 1;
                    for (var i = 0; i < limit; i++)
                    {
                        var l = lines[i].Trim();
                        if (!string.IsNullOrEmpty(l))
                            result.Add(l);
                    }
                }
                else
                {
                    stage = 3;
                }
            }

            // Chapters after target
            if (stage == 3)
            {
                if (!match.Success)
                    throw new Exception("Can't recognize page chapter.");
                if (name == targetName)
                    throw new Exception("Invalid chapter order.");
            }
        }

        return result;
    }

    private IEnumerable<string> GetPdfPages()
    {
        using (var reader = new PdfReader(_pdfPath))
        {
            for (var i = 1; i <= reader.NumberOfPages; i++)
            {
                var strategy = new MyTextExtractionStrategy();
                yield return PdfTextExtractor.GetTextFromPage(reader, i, strategy);
            }
        }
    }


    private static string GetSourceText(List<string> lines, Match match)
    {
        if (lines.Count == 0) return match.Value;
        return match.Value + Environment.NewLine + string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Split lines to subchapters and invoke visitor for each.
    /// Returns index of the first subchapter (-1 if not possible).
    /// </summary>    
    private static int VisitSubchapters<TArgument>(
        List<string> lines,
        Regex headingRegex,
        TArgument arg,
        Action<List<string>, Match, TArgument> visitor)
    {
        var result = -1;
        Match match = null;
        var headingIndex = -1;
        for (var i = 0; true; i++)
        {
            var m = (lines.Count > i)
                ? headingRegex.Match(lines[i])
                : null;

            // Process previous section
            if ((match != null) && ((m == null) || m.Success))
            {
                var sublines = lines.GetRange(headingIndex + 1, i - headingIndex - 1);
                visitor(sublines, match, arg);
            }

            // Check exit
            if (m == null) break;

            // Prepare new section
            if (m.Success)
            {
                if (match == null)
                    result = i;

                match = m;
                headingIndex = i;
            }
        }
        return result;
    }

    private sealed class MyTextExtractionStrategy : LocationTextExtractionStrategy
    {
        protected override bool IsChunkAtWordBoundary(TextChunk chunk, TextChunk previousChunk)
        {
            const float threshold = 1;

            var dist = chunk.DistanceFromEndOf(previousChunk);
            return (dist > threshold);
        }
    }
}
