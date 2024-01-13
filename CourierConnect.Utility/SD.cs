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

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
