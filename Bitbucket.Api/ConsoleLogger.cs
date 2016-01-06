using System;

namespace Bitbucket.Api
{
    public class ConsoleLogger : Ilogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public  interface Ilogger
    {
        void Log(string message);
    }
}
