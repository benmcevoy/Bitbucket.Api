using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radio7.ConfigReader.ConfigReaders;

namespace Bitbucket.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            var account = Radio7.ConfigReader.ConfigFactory.Instance.Resolve<AccountRepository>();
            var setttings = Radio7.ConfigReader.ConfigFactory.Instance.Resolve<Settings>();

            var branchRepository = new BranchRepository(account);
            var branches = branchRepository.Get();

            var commitRepository = new CommitRepository(account);
            var commits = commitRepository.Get(setttings.BranchName);

            var prRepository = new PullRequestRepository(account);
            var pullRequests = prRepository.Get();

            var sql = new SqlRepository(setttings.SqlConnectionString);

            sql.LoadTable(branches);
            sql.LoadTable(commits);
            sql.LoadTable(pullRequests);



        }
    }
}
