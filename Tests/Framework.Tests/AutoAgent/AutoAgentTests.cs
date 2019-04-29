namespace Framework.Tests.Auto
{
    using System.IO;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.DependencyManagement;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NUnit.Framework;

    //[TestClass]
    public class AutoAgentTests
    {

    }

    [TestFixture]
    [Parallelizable]
    public class ANBC_562079
    {
        private static TestAutomation testAutomation;
        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext { get; set; }

        [Test]
        public void AutoCust_562079(string rep, string pname, string testp, string browserType, string src, int id, string env, int retry)
        {
            string repositoryLocation = rep;
            string projectName = pname;
            string testPlan = testp;
            string controlMapSource = src;
            var targetBrowser = browserType;
            testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, targetBrowser, controlMapSource);
            testAutomation.Execute(id, retry, env);
        }
    }


    [TestFixture]
    [Parallelizable]
    public class ANBC_562078
    {
        private static TestAutomation testAutomation;
        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext { get; set; }

        [Test]
        public void AutoCust_562078(string rep, string pname, string testp, string browserType, string src, int id, string env, int retry)
        {
            string repositoryLocation = rep;
            string projectName = pname;
            string testPlan = testp;
            string controlMapSource = src;
            var targetBrowser = browserType;
            testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, targetBrowser, controlMapSource);
            testAutomation.Execute(id, retry, env);
        }
    }
}
