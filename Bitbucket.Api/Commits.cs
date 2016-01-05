using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Bitbucket.Api
{
    public class CommitRepository
    {
        private readonly AccountRepository _account;

        public CommitRepository(AccountRepository account)
        {
            _account = account;
        }

        public IEnumerable<Commit> Get(string branch)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _account.BasicAuthToken();

                var response = client.GetAsync(
                    $"https://api.bitbucket.org/2.0/repositories/{_account.Account}/{_account.Repository}/commits/{branch}")
                    .Result
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                var results = new List<Commit>();

                foreach (dynamic commit in result.values)
                {
                    results.Add(new Commit
                    {
                        Date = DateTime.Parse(commit.date.ToString()),
                        Message = commit.message,
                        Author = commit.author.user == null ? commit.author.raw : commit.author.user.username,
                        Branch = branch,
                        Hash = commit.hash
                    });
                }

                return results;
            }
        }

    }

    public class Commit
    {
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Hash { get; set; }
        public string Branch { get; set; }
    }
}
