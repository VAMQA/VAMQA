namespace Framework.Tests.Counselor
{
    using System.IO;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.DependencyManagement;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for CounselorTests
    /// </summary>
    //[TestClass]
    public class CycleTests
    {
        private static TestAutomation testAutomation;
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            const string repositoryLocation = "https://tfs.ext.VMddc.net/tfs/VM";
            const string projectName = "MSI New Business";
            var targetBrowser = SupportedBrowser.Chrome.ToString();
            const string testPlan = "CYCLE NEW BUSINESS";
            const string controlMapSource = "311142";

            testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, targetBrowser, controlMapSource);

            if (File.Exists("Execution.log"))
            {
                File.Delete("Execution.log");
            }
        }

        //[TestMethod]
        //[TestCategory("endtoend")]
        //public void Cycle_488424()
        //{
        //    //testAutomation.Execute(488424, "CNBNLF_USER");
        //}


    }
}
