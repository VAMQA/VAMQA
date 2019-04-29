using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VM.WebServices.TestAutomationFramework
{
    /// <summary>
    /// Summary description for NonTimeTravelVMTestCases
    /// </summary>
    [TestClass]
    //[DeploymentItem(@".\TestCases", @".\Testcases")]
    public class NonTimeTravelVMTestCases
    {
        public NonTimeTravelVMTestCases()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void NonTimeTravelVMTestCases_AddfunctioninNACHAdatabase_349056()
        {
            WebServicesFrameworkDriver.FrameworkDriver("349056", ConfigurationManager.AppSettings["Environment"].ToString().Trim(), Constants.Scrum.PaymentsVM);
        }
        [TestMethod]
        public void NonTimeTravelVMTestCases_EditfunctioninNACHAdatabase_349063()
        {
            WebServicesFrameworkDriver.FrameworkDriver("349063", ConfigurationManager.AppSettings["Environment"].ToString().Trim(), Constants.Scrum.PaymentsVM);
        }

    }
}
