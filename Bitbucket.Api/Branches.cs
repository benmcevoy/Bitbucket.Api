using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Bitbucket.Api
{
    public class BranchRepository
    {
        private readonly AccountRepository _account;

        public BranchRepository(AccountRepository account)
        {
            _account = account;
        }

        public IEnumerable<Branch> Get()
        {
            var response = GetJson();

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

            var results = new List<Branch>();

            foreach (JProperty branch in result)
            {
                var bd = branch.First() as dynamic;

                results.Add(new Branch
                {
                    Name = branch.Name,
                    Author = bd.author,
                    LastModifiedDateTime = DateTime.Parse(bd.timestamp.ToString()),
                    LastModifiedMessage = bd.message
                });
            }

            return results;
        }

        private string GetJson()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _account.BasicAuthToken();

                var response = client.GetAsync(
                    $"https://bitbucket.org/api/1.0/repositories/{_account.Account}/{_account.Repository}/branches")
                    .Result
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                return
                    response;
            }
        }

        private string GetJsonMock()
        {
            return @"{""master"": {""node"": ""0b64d6000dad"",""utctimestamp"": ""2012-05-07 22:35:02+00:00"",""author"": ""doklovic_atlassian"",""timestamp"": ""2012-05-08 00:35:02"",""branch"": ""master"",""message"": ""[maven-release-plugin] prepare for next development iteration\n""}}";
        }
    }

    public class Branch
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public string LastModifiedMessage { get; set; }
    }
}