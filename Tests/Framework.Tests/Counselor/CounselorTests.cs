namespace Framework.Tests.Counselor
{
    using System.IO;
    using VM.Platform.TestAutomationFramework.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for CounselorTests
    /// </summary>
    [TestClass]
    public class CounselorTests
    {
        private static TestAutomation testAutomation;
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            const string repositoryLocation = "https://tfs.ext.VMddc.net/tfs/VM";
            const string projectName = "MSI New Business";
            const string targetBrowser = "Chrome";
            const string testPlan = "CYCLE NEW BUSINESS";
            const string controlMapSource = "388774";//388774

            testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, targetBrowser, controlMapSource);

            if (File.Exists("Execution.log"))
            {
                File.Delete("Execution.log");
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            testAutomation.TestRunCleanup();
        }

        [TestMethod]
        [TestCategory("endtoend")]
        public void Counselor_446266()
        {
            //testAutomation.Execute(446266, "COUNSELOR_DEV");
        }

        [TestMethod]
        [TestCategory("endtoend")]
        public void Counselor_485626()
        {
            //testAutomation.Execute(309300, "COUNSELOR_USER");
        }

        [TestMethod]
        [TestCategory("endtoend")]
        public void Counselor_486508()
        {
            //testAutomation.Execute(486508, "COUNSELOR_USER");
        }

        [TestMethod]
        [TestCategory("endtoend")]
        public void Cycle_452902()
        {
            //testAutomation.Execute(452902, "COUNSELOR_DEV");
        }

        [TestMethod]
        [TestCategory("endtoend")]
        public void Cycle_485623()
        {
            // testAutomation.Execute(485623, "COUNSELOR_DEV");
        }
        [TestMethod]
        [TestCategory("endtoend")]
        public void Cycle_486578()
        {
            //testAutomation.Execute(486578, "COUNSELOR_SI");
        }
    }
}
