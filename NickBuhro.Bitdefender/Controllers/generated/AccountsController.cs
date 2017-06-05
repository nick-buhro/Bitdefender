//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NickBuhro.Bitdefender.Models;

namespace NickBuhro.Bitdefender.Controllers
{
	
    /// <summary>
    /// The Accounts API includes several methods allowing the management of user
    /// accounts:
    /// * getAccountsList : lists existing user accounts.
    /// * deleteAccount : deletes a user account.
    /// * createAccount : creates a user account.
    /// * updateAccount : updates a user account.
    /// * configureNotificationsSettings : configures the user notification
    /// settings.
    /// * getNotificationsSettings : returns the notifications settings.
    /// API url: CONTROL_CENTER_APIs_ACCESS_URL/v1.0/jsonrpc/accounts
    /// </summary>	
    public sealed partial class AccountsController : Controller
    {		
        public AccountsController(HttpClient http)
            : base(http, "/v1.0/jsonrpc/accounts") { }


        /// <summary>	
        /// This method creates a user account with password.
        /// </summary>
        /// <param name="email"> The email address for the new account. profile Object No An object containing profile information: fullName, timezone and language. timezone and language are optional. </param>
        /// <param name="password"> The password for the new account. If this value is omitted a password will be created and sent by email to the user. The password should be at least 6 characters in length and must contain at least one digit, one upper case, one lower case and one special character. </param>
        /// <param name="companyId"> The company ID used for linking the user account to a specific company. If not specified, the account will be linked to the company that holds the API key. </param>
        /// <param name="role"> The role of the new account. The default value is 1 - Company Administrator. These are the available roles: * 1 - Company Administrator. * 2 - Network Administrator. * 3 - Reporter. * 4 - Partner. * 5 - Custom. For this role, rights must be specified. rights Object Yes An object containing the rights of a user account. This object should be set only when role parameter has the value 5 - Custom. When set for other roles, the values will be ignored and replaced with the rights specific to that role. The available rights are: * manageCompanies * manageNetworks Setting this to true implies manageReports right to true * manageUsers * manageReports * companyManager Each option has two possible values: true, where the user is granted the right, or false otherwise. Omitted values from the request are automatically set to false. targetIds Array Yes A list of IDs representing the targets to be managed by the user account. For more information, refer to the relation between companyId and targetIds. </param>
        /// <returns>
        /// The ID of the created user account.
        /// </returns>
        public Task<string> CreateAccount (
            string email,
            string password = null,
            string companyId = null,
            int? role = null,
            CancellationToken ct = default(CancellationToken))
        {

            if (email == null)
                throw new ArgumentNullException("email");

            var p = new JObject();
            p["email"] = email;
            if (password != null)
                p["password"] = password;
            if (companyId != null)
                p["companyId"] = companyId;
            if (role != null)
                p["role"] = role.Value;
                        
            return Send<JObject, string>("createAccount", p, ct);
        }

        /// <summary>	
        /// This method updates a user account identified through the account ID.
        /// </summary>
        /// <param name="accountId"> The ID of the user account to be updated. </param>
        /// <param name="email"> The email address for the account. </param>
        /// <param name="password"> The password for the account.The password should be at least 6 characters in length and must contain at least one digit, one upper case, one lower case and one special character.. profile Object No An object containing profile information: fullName, timezone and language. timezone and language are optional. </param>
        /// <param name="role"> The new role of the user. These are the available roles: * 1 - Company Administrator. * 2 - Network Administrator. * 3 - Reporter. * 4 - Partner. * 5 - Custom. For this role, rights must be specified. rights Object Yes An object containing the rights of a user account. This object should be set only when role parameter has the value 5 - Custom. When set for other roles, the values will be ignored and replaced with the rights specific to that role. The available rights are: * manageCompanies * manageNetworks Setting this to True implies manageReports right to true * manageUsers * manageReports * companyManager Each option has two possible values: true, where the user is granted the right, or false otherwise. Omitted values from the request are automatically set to false. targetIds Array Yes A list of IDs representing the targets to be managed by the user account. </param>
        /// <returns>
        /// True when the user account has been successfully
        /// </returns>
        public Task<bool> UpdateAccount (
            string accountId,
            string email = null,
            string password = null,
            int? role = null,
            CancellationToken ct = default(CancellationToken))
        {

            if (accountId == null)
                throw new ArgumentNullException("accountId");

            var p = new JObject();
            p["accountId"] = accountId;
            if (email != null)
                p["email"] = email;
            if (password != null)
                p["password"] = password;
            if (role != null)
                p["role"] = role.Value;
                        
            return Send<JObject, bool>("updateAccount", p, ct);
        }

        /// <summary>	
        /// This method deletes a user account identified through the account ID.
        /// Note
        /// The account that was used to create the API key cannot be deleted by using the API.
        /// </summary>
        /// <param name="accountId"> The ID of the user account to be deleted. </param>
        public Task DeleteAccount (
            string accountId,
            CancellationToken ct = default(CancellationToken))
        {

            if (accountId == null)
                throw new ArgumentNullException("accountId");

            var p = new JObject();
            p["accountId"] = accountId;
                        
            return Send<JObject, object>("deleteAccount", p, ct);
        }
    }
}
