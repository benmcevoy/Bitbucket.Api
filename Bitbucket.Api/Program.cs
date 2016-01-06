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
            var logger = new ConsoleLogger();

            var branchRepository = new BranchRepository(account, logger);
            var branches = branchRepository.Get();

            var commitRepository = new CommitRepository(account, logger);
            var commits = commitRepository.Get(setttings.BranchName);

            var prRepository = new PullRequestRepository(account, logger);
            var pullRequests = prRepository.Get();

            var sql = new SqlRepository(setttings.ConnectionString);

            sql.LoadTable(branches);
            sql.LoadTable(commits);
            sql.LoadTable(pullRequests);



        }
    }
}
