using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spd.Console.Models;

namespace Spd.Console.Extensions
{
    public class Auth0
    {
        public static async Task<Auth0Token> Login(string jwt)
        {
            Init();
            System.Console.Clear();
            var spinner = new Animations.Spinner(0, 0) { Message = "Please login." };
            spinner.Start();

            var code = await GetCode(_state);

            spinner.Message = "Validating, please wait...";

            var content = new Auth0Verification(code, _verifier);
            var uri = $"{Constants.Auth0_Domain}oauth/token";
            var token = await WebService.Request<Auth0Token>(RequestType.Post, uri, content);


            var user = await WebService.Request<Auth0User>(RequestType.Get, $"{Constants.Auth0_Domain}userinfo", token : token.access_token);
            token.user_name = user.nickname;
            spinner.Stop();
            System.Console.WriteLine($"Welcome {token.user_name}!");
            return token;
        }

        private static void Init()
        {
            var random = new Random();

            var verifier = new byte[32];
            random.NextBytes(verifier);
            var state = new byte[32];
            random.NextBytes(state);

            _state = state.ToBase64UrlEncoding();
            _verifier = verifier.ToBase64UrlEncoding();
            _challenge = Sha256(_verifier).ToBase64UrlEncoding();
        }

        private static async Task<string> GetCode(string state)
        {
            var url = $"{Constants.Auth0_Domain}authorize?response_type=code&scope=openid&client_id={Constants.Auth0_ClientID}&redirect_uri={Constants.API_Uri}/auth&code_challenge={_challenge}&code_challenge_method=S256&state={_state}";
            var process = Process.Start("IEXPLORE.EXE", "-nomerge " + url);

            var results = "";
            for (var i = 0; i < 10; i++) //we only try 10 times
            {
                Thread.Sleep(5000); //trying again in 5 seconds.

                results = await WebService.Request<string>(RequestType.Get, $"{Constants.API_Uri}/auth/{state}/code");
                if (!string.IsNullOrEmpty(results)) break;
            }

            if (string.IsNullOrEmpty(results))
                throw new UnauthorizedAccessException(Constants.Error_Login);

            process?.Kill();
            return results;
        }

        private static byte[] Sha256(string password)
        {
            var crypt = new SHA256Managed();
            return crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        }

        private static string _state;
        private static string _verifier;
        private static string _challenge;
    }
}