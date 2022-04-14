using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Domain
{
    public class TestService : ITestService
    {
        public int TestCount { get; set; }
        public string getTestContent()
        {
            return "This is test content from TestService";
        }

        public void setTestCount(int count)
        {
            TestCount = count;
        }

        public int getTestCount()
        {
            return TestCount;
        }
    }

    public interface ITestService
    {
        string getTestContent();
        void setTestCount(int count);
        int getTestCount();
    }
}
