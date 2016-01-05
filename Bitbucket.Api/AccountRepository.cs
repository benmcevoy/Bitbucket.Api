using System;
using System.Net.Http.Headers;
using System.Text;

namespace Bitbucket.Api
{
    public class AccountRepository
    {
        public AuthenticationHeaderValue BasicAuthToken()
        {
            var byteArray = Encoding.ASCII.GetBytes($"{UserName}:{Password}");

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public string Repository { get; set; }
        public string Account { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}