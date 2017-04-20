using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spd.Console.Models;

namespace Spd.Console.Extensions
{
    public interface IAuth {
        Task<AuthToken> Login(string jwt);
    }

    public class Auth0 : IAuth
    {
        public Auth0(IWebService service = null)
        {
            _webservice = service ?? new WebService();
        }

        public async Task<AuthToken> Login(string jwt)
        {
            var spinner = new Animations.Spinner(0, 0) { Message = "Please login." };
            spinner.Start();
            try
            {
                Init();
                System.Console.Clear();

                var code = await GetCode(_state);
                var authVerification = new AuthVerification(code, _verifier);
                var token = await _webservice.Request<AuthToken>(RequestType.Post, $"{Constants.Auth0_Domain}oauth/token", authVerification);
                var authUser = await _webservice.Request<AuthUser>(RequestType.Get, $"{Constants.Auth0_Domain}userinfo", token: token.access_token);
                var response = await _webservice.Request<string>(RequestType.Post, $"{Constants.API_Uri}/member", authUser, token.id_token);
                System.Console.WriteLine(response);
                token.user_name = authUser.nickname;
                return token;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to login: {ex}");
                return null;
            }
            finally
            {
                spinner.Stop();
            }
        }

        private void Init()
        {
            var random = new Random();

            var verifier = new byte[32];
            random.NextBytes(verifier);
            var state = new byte[32];
            random.NextBytes(state);

            _state = ToBase64UrlEncoding(state);
            _verifier = ToBase64UrlEncoding(verifier);
            _challenge = ToBase64UrlEncoding(Sha256(_verifier));
        }

        private async Task<string> GetCode(string state)
        {
            var url = $"{Constants.Auth0_Domain}authorize?response_type=code&scope=openid&client_id={Constants.Auth0_ClientID}&redirect_uri={Constants.API_Uri}/auth&code_challenge={_challenge}&code_challenge_method=S256&state={_state}";
            var process = Process.Start("IEXPLORE.EXE", "-nomerge " + url);

            var results = "";
            for (var i = 0; i < 10; i++) //we only try 10 times
            {
                Thread.Sleep(5000); //trying again in 5 seconds.

                results = await _webservice.Request<string>(RequestType.Get, $"{Constants.API_Uri}/auth/{state}/code");
                if (!string.IsNullOrEmpty(results)) break;
            }

            if (string.IsNullOrEmpty(results))
                throw new UnauthorizedAccessException(Constants.Error_Login);

            process?.Kill();
            return results;
        }

        private byte[] Sha256(string password)
        {
            var crypt = new SHA256Managed();
            return crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        }

        public string ToBase64UrlEncoding(byte[] bytes)
        {
            var padding = new[] { '=' };
            return Convert.ToBase64String(bytes).TrimEnd(padding).Replace('+', '-').Replace('/', '_').Replace("=", "");
        }

        private static string _state;
        private static string _verifier;
        private static string _challenge;
        private readonly IWebService _webservice;
    }
}