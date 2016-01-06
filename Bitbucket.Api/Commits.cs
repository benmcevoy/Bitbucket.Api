using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace Bitbucket.Api
{
    public class CommitRepository
    {
        private readonly AccountRepository _account;
        private readonly Ilogger _logger;

        public CommitRepository(AccountRepository account, Ilogger logger)
        {
            _account = account;
            _logger = logger;
        }

        public IEnumerable<Commit> Get(string branch)
        {
            return GetImpl(branch, $"https://api.bitbucket.org/2.0/repositories/{_account.Account}/{_account.Repository}/commits/{branch}");
        }

        private IEnumerable<Commit> GetImpl(string branch, string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _account.BasicAuthToken();

                _logger.Log("GET: " + url);

                var response = client.GetAsync(url)
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

                if (result.next != null)
                {
                    // throttle
                    Thread.Sleep(1000);
                    results.AddRange(GetImpl(branch, result.next.ToString()));
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
