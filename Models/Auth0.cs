using System;

namespace Spd.Console.Models
{
    public class AuthVerification
    {
        public AuthVerification(string code, string verifier)
        {
            this.code = code;
            code_verifier = verifier;
            client_id = Constants.Auth0_ClientID;
            grant_type = "authorization_code";
            redirect_uri = $"{Constants.API_Uri}/auth";
        }

        public string code { get; private set; }
        public string code_verifier { get; private set; }
        public string client_id { get; private set; }
        public string grant_type { get; private set; }
        public string redirect_uri { get; private set; }
    }

    public class AuthToken
    {
        public AuthToken()
        {
            timeStamp = DateTime.Now;
        }

        public DateTime timeStamp { get; private set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string token_type { get; set; }
        public string user_name { get; set; }
    }

    public class AuthUser
    {
        public bool email_verified { get; set; }
        public string email { get; set; }
        public string clientID { get; set; }
        public DateTime updated_at { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
        public string user_id { get; set; }
        public string nickname { get; set; }
        public AuthIdentities[] identities { get; set; }
        public DateTime created_at { get; set; }
        public string sub { get; set; }
    }

    public class AuthIdentities
    {
        public string user_id { get; set; }
        public string provider { get; set; }
        public string connection { get; set; }
        public bool isSocial { get; set; }
    }
}
