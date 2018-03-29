# Bitdefender

C# client library for Bitdefender Control Center API.

This project was a part of internal research that was closed. So this library is **unsupported**. You can use it as-is for what-you-want.

## Getting Started

The project could be built using Visual Studio 2017.

Library interface is equal to original API structure,
so [API documentation](Bitdefender_ControlCenter_API-Guide_enUS.pdf) can be used for better understanding.

Usage example:

``` C#

var client = new BitdefenderClient("PUT-HERE-BITDEFENDER-API-KEY");

// Create new company
var id = await client.Companies.CreateCompany(CompanyType.Customer, "ABC Ltd.");

// Delete this company
await client.Companies.DeleteCompany(id);

```

## Design Concepts

This project is based on code generation. The main idea:
- Parse [API documentation](Bitdefender_ControlCenter_API-Guide_enUS.pdf) file and extract all API metadata.
- Join generated metadata with manual-prepared metadata.
- Generate controllers. Separate controller for each API.
- Compile the library.

## Running the Tests

Environment variable 'BITDEFENDER_API_KEY' should be configured before running xUnit tests.

## Lisence

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
