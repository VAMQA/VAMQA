using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VM.WebServices.TestAutomationFramework.Common;
using System.Xml;
using System.IO;
using System.Data;
using System.Xml.Linq;
using System.Net.Http;
using ClosedXML.Excel;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Configuration;


namespace VM.WebServices.TestAutomationFramework
{
    [TestClass]
    public class WebServicesFrameworkDriver : FWLogger
    {
        [TestMethod]
        public static void FrameworkDriver(string currentTC, string strEnvironment = "", Constants.Scrum eScrum = Constants.Scrum.Default)
        {
            try
            {
                Logger.Info(Constants.ENTERINGMETHOD + System.Reflection.MethodBase.GetCurrentMethod().Name);
                //Logger.Info(Constants.ENTERINGMETHOD);
                string testCasesExcelFile = "";
                string FetchPolicynumbers = "";
                //string FindPolicynumbers = "";

                switch (eScrum)
                {
                    case Constants.Scrum.CoreADC:
                        testCasesExcelFile = @".\" + Constants.TESTCOREADC_ASEEXCELFILE;
                        break;
                    case Constants.Scrum.CoreVM:
                        testCasesExcelFile = @".\" + Constants.TESTCOREVM_ASEEXCELFILE;
                        break;
                    case Constants.Scrum.PaymentsADC:
                        testCasesExcelFile = @".\" + Constants.TESTPAYMENTADC_ASEEXCELFILE;
                        break;

                    case Constants.Scrum.DisbursementsADC:
                        testCasesExcelFile = @".\" + Constants.TESTDISBURSEMENTADC_ASEEXCELFILE;
                        break;
                    case Constants.Scrum.PaymentsVM:
                        testCasesExcelFile = @".\" + Constants.TESTPAYMENTVM_ASEEXCELFILE;
                        break;

                    case Constants.Scrum.Smoke:
                        testCasesExcelFile = @".\" + Constants.SMOKETEST_ASEEXCELFILE;
                        break;
                    case Constants.Scrum.Default:
                        //testCasesExcelFile = Library.GetSolutionPath() + @"\UIDesign\UIDesign" + Constants.TESTCASEEXCELFILE;
                        testCasesExcelFile = Directory.GetCurrentDirectory() + Constants.TESTCASEEXCELFILE;
                        break;
                    default:
                        testCasesExcelFile = @".\" + Constants.TESTCASEEXCELFILE;
                        break;
                }

                //FindPolicynumbers = Library.GetSolutionPath() + @"\UIDesign\UIDesign" + Constants.TESTPolicynumberEXCELFILE;
                //FindPolicynumbers = Directory.GetCurrentDirectory() + Constants.TESTPolicynumberEXCELFILE;

                DataSet All = Library.ReadExcel(testCasesExcelFile);
                //DataSet DBsetPolicies = Library.ReadExcel(FindPolicynumbers);
                DataTable dtTestSuite = All.Tables[Constants.TESTSUITE];
                //DataTable dtPolicies = DBsetPolicies.Tables[Constants.POLICIES];
                TestSuite currentTS = new TestSuite(dtTestSuite);
                DataTable dtOutputXPath = All.Tables[Constants.OUTPUTXPATH];
                DataTable dtEnvironments = All.Tables[Constants.ENVIRONMENT];
                DataTable dtXPathRepository = All.Tables[Constants.XPATHREPOSITORY];
                DataTable dtCustomHeader = All.Tables[Constants.CUSTOMHEADER];
                //var PolicynumberRow = from myRow in dtPolicies.AsEnumerable()
                //              where myRow.Field<string>("TestcaseId ") == currentTC.ToString()
                //                   select myRow.Field<string>("Policynumbers").ToString();
                //foreach (var row in PolicynumberRow)
                //{
                //    FetchPolicynumbers = row.ToString();
                //}            
                Logger.Info(Constants.LOGFILEHEADER);
                Logger.Info(Constants.LOGSTARTTESTCASEHEADER);
                Logger.Info(Constants.LOGFILEHEADER);
                Logger.Info(Constants.TESTCASEID + currentTC);
                XDocument reqXML = null;
                XDocument resXML = null;
                string CurrentXML = null;
                bool Success = false;
                bool bSkipTestCase = false;
                Library.Keywords.Clear();
                string sLOB = null;
                string sActualcurrentTC = null;
                System.Threading.Thread.Sleep(5000);

                //sLOB = ConfigurationManager.AppSettings["LOB"].ToString().Trim();
                //sLOB = (sLOB.ToUpper() == "AUTO" ? "" : sLOB);
                //if (sLOB != "")
                //{
                //    DataRow[] ChkFilteredTableForProductSelection = dtTestSuite.DefaultView.ToTable(true, new string[] { "User Story", "Test Case" }).Select("[Test Case] Like '" + currentTC + "%'");

                //    if (ChkFilteredTableForProductSelection.Length > 2)
                //    {
                //        bSkipTestCase = true;
                //        Logger.Info("Duplicate Test Case Identified. Test Case # : " + currentTC);
                //    }
                //    else if (ChkFilteredTableForProductSelection.Length == 2)
                //    {
                //        DataRow[] FilteredTableForProductSelection = dtTestSuite.DefaultView.ToTable(true, new string[] { "User Story", "Test Case" }).Select("[Test Case] = '" + currentTC + "_" + sLOB + "'");
                //        if (FilteredTableForProductSelection.Length == 1)
                //        {
                //            sActualcurrentTC = currentTC;
                //            currentTC = currentTC + "_" + sLOB;
                //        }
                //    }
                //}                
                string environment = string.Empty;
                foreach (TestCase currentTCDetails in currentTS.TestCases[currentTC])
                {

                    if ((currentTCDetails.Execute == Constants.Yes) && (bSkipTestCase == false))
                    {
                        currentTCDetails.StartTime = DateTime.Now;
                        Logger.Info(Constants.LOGFILEHEADER);
                        Logger.Info(Constants.LOGSTARTTESTSTEPHEADER);
                        Logger.Info(Constants.LOGFILEHEADER);

                        if ((currentTCDetails.Action != Constants.VALIDATE) && (currentTCDetails.Action != Constants.NOVALIDATE))
                        {
                            string strUriEnv = null;
                            if (strEnvironment == "NA")
                            {
                                strUriEnv = currentTCDetails.URI;
                            }
                            else
                            {
                                strUriEnv = currentTCDetails.URI + (strEnvironment == "" ? currentTCDetails.Environment : strEnvironment);
                            }
                            string strFilter = string.Format("[{0}]='{1}'", Constants.environment, strUriEnv);
                            if ((!currentTCDetails.Action.Equals("~~URI1-Request")) & (!currentTCDetails.Action.Equals("~~URI2-Request")) &
                                (!currentTCDetails.Action.Equals("GET-~~URI1-Negative Request")) & (!currentTCDetails.Action.Equals("GET-~~URI1-Request"))
                                & (!currentTCDetails.Action.Equals("~~URI3-Request")) & (!currentTCDetails.Action.Equals("~~URI4-Request"))
                                & (!currentTCDetails.Action.Equals("~~URI5-Request")) & (!currentTCDetails.Action.Equals("~~URI6-Request"))
                                & (!currentTCDetails.Action.Equals("~~URI7-Request")) & (!currentTCDetails.Action.Equals("~~URI8-Request"))
                                & (!currentTCDetails.Action.Equals("~~URI-Request")))
                                environment = dtEnvironments.Select(strFilter)[0].ItemArray[1].ToString().Replace("AfterIssuePolicyNumber", FetchPolicynumbers); ;
                            if (!currentTCDetails.Action.Contains("Request"))
                            {
                                environment = Library.ReplaceURIwithGivenKeyword(currentTCDetails, environment);

                            }
                            if (currentTCDetails.XMLFile.Contains("JSonTemplt"))
                            {
                                string[] sheetNXmlFile = currentTCDetails.XMLFile.Split('-');
                                DataTable dtInputXPath = All.Tables[sheetNXmlFile[0].ToString()];
                                currentTCDetails.strRequest = Library.BuildJSonRequestFromExcel(dtInputXPath, currentTC);
                            }
                            else if (currentTCDetails.XMLFile.Contains("Tmplt"))
                            {
                                DataTable dtInputXPath = All.Tables[currentTCDetails.XMLFile];
                                //string XMLFilePath = Library.GetSolutionPath() + @"\UIDesign\UIDesign" + Constants.XMLTEMPLATEPATH + currentTCDetails.XMLFile;
                                string XMLFilePath = Directory.GetCurrentDirectory() + Constants.XMLTEMPLATEPATH + currentTCDetails.XMLFile;


                                Logger.Info("Tmplt XML File Path # : " + XMLFilePath);
                                currentTCDetails.RequestXML = Library.BuildXMLFromXPath(XMLFilePath, currentTC, currentTCDetails.XMLFile, dtInputXPath, dtXPathRepository, FetchPolicynumbers);

                            }
                            else if (currentTCDetails.Action.Contains("Request") && !currentTCDetails.XMLFile.Contains("NA"))
                            {
                                string[] sheetNXmlFile = currentTCDetails.XMLFile.Split('-');
                                DataTable dtInputXPath = All.Tables[sheetNXmlFile[0].ToString()];

                                if (sActualcurrentTC == null)
                                {
                                    sActualcurrentTC = currentTC;
                                }

                                DirectoryInfo dirTestCases = new System.IO.DirectoryInfo(@".\" + Constants.TESTDATAXMLS + (sActualcurrentTC != currentTC ? sActualcurrentTC : currentTCDetails.TCNbr));
                                IEnumerable<FileInfo> xmlFileList = dirTestCases.GetFiles("*.xml", System.IO.SearchOption.AllDirectories);
                                foreach (FileInfo xmlFile in xmlFileList)
                                {
                                    string xmlFileName = xmlFile.Name.Replace(".Rq.xml", "");
                                    if (xmlFileName == sheetNXmlFile[1].ToString())
                                    {
                                        Logger.Info("Request XML File Path # : " + xmlFile.FullName);
                                        currentTCDetails.RequestXML = Library.BuildXMLFromXPath(xmlFile.FullName, currentTC, (sActualcurrentTC != currentTC ? sActualcurrentTC : currentTCDetails.TCNbr), dtInputXPath, dtXPathRepository);
                                    }
                                }
                            }
                            if (currentTCDetails.Action.Contains("Request"))
                            {
                                //WebService objWebService = new WebService();
                                WebService objWebService = new WebService(dtEnvironments, strFilter);
                                string strResponse = "";
                                if (currentTCDetails.Action.Contains("POST JSON"))
                                {
                                    strResponse = objWebService.MakeServiceCall(HttpMethod.Post, environment, (currentTCDetails.strRequest == null ? "" : currentTCDetails.strRequest), dtCustomHeader, currentTC, out Success, ((currentTCDetails.RequestHeader.Trim() == "" || currentTCDetails.RequestHeader.Trim().ToLower().Contains("optional")) ? Constants.sDefaultContentType : currentTCDetails.RequestHeader));
                                }
                                else if (currentTCDetails.Action.Contains("GET"))
                                {
                                    strResponse = objWebService.MakeServiceCall(HttpMethod.Get, environment, (currentTCDetails.RequestXML == null ? "" : currentTCDetails.RequestXML.ToString()), dtCustomHeader, currentTC, out Success, ((currentTCDetails.RequestHeader.Trim() == "" || currentTCDetails.RequestHeader.Trim().ToLower().Contains("optional")) ? Constants.sDefaultContentType : currentTCDetails.RequestHeader));
                                }
                                else
                                {
                                    System.Net.ServicePointManager.Expect100Continue = false;
                                    strResponse = objWebService.MakeServiceCall(HttpMethod.Post, environment, (currentTCDetails.RequestXML == null ? "" : currentTCDetails.RequestXML.ToString()), dtCustomHeader, currentTC, out Success, ((currentTCDetails.RequestHeader.Trim() == "" || currentTCDetails.RequestHeader.Trim().ToLower().Contains("optional")) ? Constants.sDefaultContentType : currentTCDetails.RequestHeader));
                                }

                                if (((Success) && currentTCDetails.Action.Contains("-Action Request") != true) || (currentTCDetails.Action.Contains("-Negative Request") && (Success == false)))
                                {
                                    //if (!strResponse.ToLower().StartsWith("{"))
                                    //{ currentTCDetails.ResponseXML = XDocument.Parse(strResponse); }
                                    //else
                                    { //currentTCDetails.ResponseXML = JsonConvert.DeserializeXmlNode(strResponse.ToString(), "root");
                                        //currentTCDetails.ResponseXML = JsonConvert.DeserializeXmlNode("{\"Row\":" + strResponse + "}", "root");
                                        currentTCDetails.ResponseXML = JsonConvert.DeserializeXNode("{\"Row\":" + strResponse + "}", "root");
                                    }

                                    Library.ReplaceXMLFromXPath(currentTCDetails, currentTC, dtOutputXPath, dtXPathRepository);
                                    reqXML = currentTCDetails.RequestXML;
                                    resXML = currentTCDetails.ResponseXML;
                                }
                                else if ((Success) && currentTCDetails.Action.Contains("-Action Request"))
                                {

                                }
                                else
                                {
                                    currentTCDetails.Result = Constants.FAIL;
                                    bSkipTestCase = true;
                                    Logger.Info("Service is not responded properly" + "Request : " + (currentTCDetails.RequestXML == null ? "" : currentTCDetails.RequestXML.ToString()) + "\n" + " Response : " + strResponse);
                                    throw new AssertFailedException("Service is not responded properly" + "Request : " + (currentTCDetails.RequestXML == null ? "" : currentTCDetails.RequestXML.ToString()) + "\n" + " Response : " + strResponse);
                                }
                            }

                            if ((Success) || (currentTCDetails.Action.Contains("-Negative Request") && (Success == false)))
                            {
                                currentTCDetails.Result = Constants.PASS;

                                switch (currentTCDetails.XMLFile.ToLower())
                                {
                                    case "policy.tmplt.rq.xml":
                                    case "policy.cycle.tmplt.rq.xml":
                                    case "policy-rpm.tmplt.rq.xml":
                                    case "cycle-rpm.tmplt.rq.xml":
                                    case "cycle.tmplt.rq.xml":
                                    case "umbrella.tmplt.rq.xml":
                                        Library.ValidatePolicyXMLFromXPath(currentTCDetails, "000000", dtOutputXPath, dtXPathRepository);

                                        if (currentTCDetails.Results.Count > 0)
                                        {
                                            var Fail = currentTCDetails.Results.FindAll(v => v.Result == Constants.FAIL);
                                            var Pass = currentTCDetails.Results.FindAll(v => v.Result == Constants.PASS);
                                            if (Pass.Count > 0)
                                            {
                                                currentTCDetails.Result = Constants.PASS;
                                                Library.BuildDictonaryObjectFromXML(currentTCDetails, "000000", dtOutputXPath, dtXPathRepository);
                                            }
                                            if (Fail.Count > 0)
                                            {
                                                currentTCDetails.Result = Constants.FAIL;
                                                bSkipTestCase = true;
                                                Logger.Info("Policy is not Issued Properly");
                                                throw new AssertFailedException("Policy is not Issued Properly");
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else if (currentTCDetails.Action.Contains("~ISSUEDPOLICYNUMBER"))
                            {
                            }
                            else
                            {
                                currentTCDetails.Result = Constants.FAIL;
                                bSkipTestCase = true;
                                Logger.Info("Service is not responded properly");
                                throw new AssertFailedException("Service is not responded properly");
                            }

                            CurrentXML = currentTCDetails.XMLFile;
                        }
                        else if (currentTCDetails.Action == Constants.NOVALIDATE)
                        {
                            currentTCDetails.Result = Constants.PASS;
                        }
                        else
                        {
                            currentTCDetails.RequestXML = reqXML;
                            currentTCDetails.ResponseXML = resXML;
                            currentTCDetails.XMLFile = CurrentXML;
                            Library.ValidateXMLFromXPath(currentTCDetails, currentTC, dtOutputXPath, dtXPathRepository);
                            if (currentTCDetails.Results.Count > 0)
                            {
                                var Fail = currentTCDetails.Results.FindAll(v => v.Result == Constants.FAIL);
                                var Pass = currentTCDetails.Results.FindAll(v => v.Result == Constants.PASS);
                                if (Pass.Count > 0)
                                    currentTCDetails.Result = Constants.PASS;
                                if (Fail.Count > 0)
                                {
                                    currentTCDetails.Result = Constants.FAIL;
                                    //throw new AssertFailedException("Test Validation Failed");
                                }
                            }
                        }
                        currentTCDetails.EndTime = DateTime.Now;
                        Logger.Info(Constants.cStep + currentTCDetails.Steps);
                        Logger.Info(Constants.cURI + currentTCDetails.URI);
                        Logger.Info(Constants.cAction + currentTCDetails.Action);
                        Logger.Info(Constants.cEnv + (strEnvironment == "" ? currentTCDetails.Environment : strEnvironment));
                        Logger.Info(Constants.cUserStory + currentTCDetails.UserStory);
                        Logger.Info(Constants.LOGFILEHEADER);
                        Logger.Info(Constants.LOGENDTESTSTEPHEADER);
                        Logger.Info(Constants.LOGFILEHEADER);
                    }
                }
                Logger.Info(Constants.LOGFILEHEADER);
                Logger.Info(Constants.LOGENDTESTCASEHEADER);
                Logger.Info(Constants.LOGFILEHEADER);
                Library.GenerateHTMLReport(currentTS, currentTC);
                Logger.Info(Constants.EXITINGMETHOD + System.Reflection.MethodBase.GetCurrentMethod().Name);
                //Logger.Info(Constants.EXITINGMETHOD);
            }
            catch (Exception ex)
            {
                Logger.Info("webserviceframworkdriver : " + (ex.InnerException == null ? ex.StackTrace.ToString() : ex.InnerException.ToString()));
                throw new AssertFailedException(ex.Message);
            }
        }

    }
}
