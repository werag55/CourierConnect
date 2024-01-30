using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Utility
{
    public static class SD //all the constans 
    {
        public const string Role_User_Admin = "Admin";
        public const string Role_User_Worker = "Worker";
        public const string Role_User_Courier = "Courier";
        public const string Role_User_Client = "Client";

        public const string ApiKeySectionName = "Authentication:ApiKey";
        public const string SpecialApiKeySectionName = "Authentication:SpecialApiKey";
        public const string ApiUrlSectionName = "ServiceUrls:API";

        public const string CourierHubApiKeySectionName = "Authentication:CourierHubApiKey";
        public const string CourierHubApiUrlSectionName = "ServiceUrls:CourierHub";

        public const string TokenUrlSectionName = "Authentication:TokenUrl";
        public const string CurrierGrandTypeSectionName = "Authentication:GrandType";
        public const string CurrierClientIdSectionName = "Authentication:ClientId";
        public const string CurrierClientSecretSectionName = "Authentication:ClientSecret";
        public const string CurrierScopeSectionName = "Authentication:Scope";
        public const string CurrierApiUrlSectionName = "ServiceUrls:Currier";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
