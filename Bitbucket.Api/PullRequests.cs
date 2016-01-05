using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Bitbucket.Api
{
    public class PullRequestRepository
    {
        private readonly AccountRepository _account;

        public PullRequestRepository(AccountRepository account)
        {
            _account = account;
        }

        public IEnumerable<PullRequest> Get()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = _account.BasicAuthToken();

                var response = client.GetAsync(
                    $"https://api.bitbucket.org/2.0/repositories/{_account.Account}/{_account.Repository}/pullrequests?state=[OPEN, MERGED, DECLINED]")
                    .Result
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                var results = new List<PullRequest>();

                foreach (dynamic pr in result.values)
                {
                    results.Add(new PullRequest
                    {
                        LastUpdateDateTime = DateTime.Parse(pr.updated_on.ToString()),
                        Description = pr.description,
                        Title = pr.title,
                        Author = pr.author.username,
                        Branch = pr.destination.branch.name,
                        State = pr.state
                    });
                }

                return results;
            }
        }
    }

    public class PullRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string State { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string Branch { get; set; }
    }
}
