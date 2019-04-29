namespace Framework.Tests.Auto
{
    using System;
    using System.IO;
    using System.Globalization;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.DependencyManagement;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VM.Platform.TestAutomationFramework.UIAutomation;
    using System.Collections.Generic;
    using Framework.Tests.ConsolidatedHTMLReport;



    //[TestClass]
    public class AutoCustomerTests
    {
        private static TestAutomation testAutomation;
        public TestContext TestContext { get; set; }        
        
        public static void ClassInitialize(TestContext testContext)
        {
            //Local Execution
            const string repositoryLocation = "https://local";
            const string projectName = null;
            const string testPlan = null;
            const string controlMapSource = null;            

            var targetBrowser = SupportedBrowser.Chrome.ToString();            
            testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, targetBrowser, controlMapSource);            
            if (File.Exists("Execution.log"))
            {
                File.Delete("Execution.log");
            }
        }

        public void Initialize(string browserType,string projId)
        {
            //DB Execution
            const string repositoryLocation = "https://local";
            const string projectName = null;
            string testPlan = projId;
            const string controlMapSource = null;            
            
            testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, browserType, controlMapSource);
            if (File.Exists("Execution.log"))
            {
                File.Delete("Execution.log");
            }
        }

        public void Cleanup()
        {
            testAutomation.TestRunCleanup();
        }  
        
        public static void ClassCleanup()
        {
           testAutomation.TestRunCleanup();
        }        

        //[TestMethod]
        //public void Execute(int id, string strEnv, TestAutomation test, int Retry)
        //{
        //    var testRunContext = test.Execute(id, Retry, strEnv);
        //    Report(testRunContext);
        //}            

        public class ConsolidatedTestResults
        {
            public string testCaseId;
            public string autoPolicyNbr;
            public string clmPolicyNbr;
            public string testResult;
            public List<TestFinding> TestFindings = new List<TestFinding>();

            public static ConsolidatedTestResults ConsolTestResults(TestRunContext testRunContext)
            {
                ConsolidatedTestResults consTestResults = new ConsolidatedTestResults();
                consTestResults.testCaseId = Convert.ToString(testRunContext.TestCaseId);
                consTestResults.testResult = Convert.ToString(testRunContext.TestResult);

                foreach (var testfinding in testRunContext.TestFindings)
                {
                    if ((testfinding.Time.ToString() == "1/1/0001 12:00:00 AM") || (testfinding.Time.ToString() == "{1/1/0001 12:00:00 AM}"))
                    {
                        testfinding.Time = DateTime.Now;
                    }

                    if (testfinding.Action.Contains("{PolicyNumber}"))
                    {
                        consTestResults.autoPolicyNbr = testfinding.Value;
                    }
                    else if (testfinding.Action.Contains("Claim_Number_Validation") || testfinding.Action.Contains("Claim_Number"))
                    {
                        consTestResults.clmPolicyNbr = string.IsNullOrEmpty(Convert.ToString(testfinding.ActualResult))?string.Empty:testfinding.ActualResult.ToString();
                    }
                }
                consTestResults.TestFindings = testRunContext.TestFindings;
                return consTestResults;
            }
        }

        public void Report(TestRunContext testRunContext)
        {
            //TestRunContext testRunContext = new TestRunContext();
            var conResults = ConsolidatedTestResults.ConsolTestResults(testRunContext);
            Framework.Tests.ConsolidatedHTMLReport.ListConsolidatedResults.ConsolidatedTestCaseResults.Add(conResults);
            //ConsolidatedHTMLReportGenerator.GenerateHTMLReport(ListConsolidatedResults.ConsolidatedTestCaseResults);
            //if (testRunContext.TestResult == TestResult.Fail) { Assert.Fail("Test Failed"); }
        }
        public void ConsolidateReport(TimeSpan exeTime)
        {
            ConsolidatedHTMLReportGenerator.GenerateHTMLReport(ListConsolidatedResults.ConsolidatedTestCaseResults, exeTime);
        }

            
            
    }
}
