using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
//using Microsoft.TeamFoundation.TestManagement.Client;
using VM.Platform.TestAutomationFramework.Core;
using LinqToExcel;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using LinqToExcel.Domain;

namespace UIDesign
{    
    public partial class v3Migration : Form
    {

        TfsTeamProjectCollection projColl;
        FuncLib objLib = new FuncLib();
        bool repositioryFlag = true;
        //int testcaseId;

        private Label lbl_tfsurl = new Label();
        private Label lbl_username = new Label();
        private Label lbl_password = new Label();
        private Label lbl_teamproj = new Label();
        private Label lbl_testsuiteId = new Label();
        private Label lbl_pagetitle = new Label();
        private Label lbl_migrationstatus = new Label();

        private TextBox txt_tfsurl = new TextBox();
        private TextBox txt_username = new TextBox();
        private TextBox txt_teamproj = new TextBox();
        private TextBox txt_testsuiteId = new TextBox();
        private TextBox txt_password = new TextBox();

        private Button btn_connect = new Button();
        private Button btn_migrate = new Button();

        private PictureBox pic_gecko = new PictureBox();                
        private FlowLayoutPanel flp_migration = new FlowLayoutPanel();
        private TableLayoutPanel tlp_migration = new TableLayoutPanel();
        private Panel panel2 = new Panel();

        public v3Migration()
        {
            InitializeComponent();

            #region testcloner_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 4 + 150 , System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height-50);
            this.AutoScroll = true;
            this.Text = "Migration";

            //Flow Layout Panel Settings            
            flp_migration.FlowDirection = FlowDirection.LeftToRight;
            flp_migration.SetFlowBreak(pic_gecko, true);
            flp_migration.Dock = DockStyle.Top;
            flp_migration.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_migration.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_migration.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_migration.AutoSize = true;
            tlp_migration.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 170;
            pic_gecko.Width = 220;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Username Label settings            
            lbl_tfsurl.Text = "TFS URL :*";
            lbl_tfsurl.Name = "lbl_tfsurl";
            lbl_tfsurl.TextAlign = ContentAlignment.BottomLeft;
            lbl_tfsurl.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_tfsurl.Height = 20;
            lbl_tfsurl.Width = 100;

            //Username Label settings            
            lbl_username.Text = "User ID :*";
            lbl_username.Name = "lbl_username";
            lbl_username.TextAlign = ContentAlignment.BottomLeft;
            lbl_username.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_username.Height = 20;
            lbl_username.Width = 100;

            //Username Label settings            
            //lbl_migrationstatus.Text = "";
            //lbl_migrationstatus.Name = "lbl_migrationstatus";
            //lbl_migrationstatus.TextAlign = ContentAlignment.BottomLeft;
            //lbl_migrationstatus.Font = new Font("Calibri", 11F, FontStyle.Bold);
            //lbl_migrationstatus.Height = 20;
            //lbl_migrationstatus.Width = 100;

            //Error Message Label settings           
            lbl_migrationstatus.Height = 200;
            lbl_migrationstatus.Width = 300;
            lbl_migrationstatus.Name = "lbl_migrationstatus";
            lbl_migrationstatus.TextAlign = ContentAlignment.TopLeft;
            lbl_migrationstatus.Font = new Font("Calibri", 10F, FontStyle.Regular);
            //lbl_migrationstatus.Text = "Please Wait.....Migration is In-Process..";
            lbl_migrationstatus.ForeColor = Color.Red;
            //lbl_migrationstatus.Visible = false;

            //Username Label settings            
            lbl_teamproj.Text = "Team Project :*";
            lbl_teamproj.Name = "lbl_teamproj";
            lbl_teamproj.TextAlign = ContentAlignment.BottomLeft;
            lbl_teamproj.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_teamproj.Height = 20;
            lbl_teamproj.Width = 100;
            lbl_teamproj.Visible = false;

            //Username Label settings            
            lbl_testsuiteId.Text = "Test SuiteId :*";
            lbl_testsuiteId.Name = "lbl_testsuiteId";
            lbl_testsuiteId.TextAlign = ContentAlignment.BottomLeft;
            lbl_testsuiteId.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_testsuiteId.Height = 20;
            lbl_testsuiteId.Width = 100;
            lbl_testsuiteId.Visible = false;

            //Password Label settings            
            lbl_password.Text = "Password :*";
            lbl_password.Name = "lbl_password";
            lbl_password.TextAlign = ContentAlignment.BottomLeft;
            lbl_password.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_password.Height = 20;
            lbl_password.Width = 100;

            txt_tfsurl.Text = "https://tfs.ext.VMddc.net/tfs/VM";
            txt_tfsurl.Name = "txt_tfsurl";
            txt_tfsurl.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_tfsurl.Height = 24;
            txt_tfsurl.Width = 300;

            //txt_username.Text = "dev\\u1hc409";
            txt_username.Name = "txt_username";
            txt_username.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_username.Height = 24;
            txt_username.Width = 300;

            //txt_password.Text = "Rock@123!";
            txt_password.Name = "txt_password";
            txt_password.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_password.Height = 24;
            txt_password.Width = 300;
            txt_password.PasswordChar = '*';

            txt_teamproj.Text = "MSI New Business";
            txt_teamproj.Name = "txt_teamproj";
            txt_teamproj.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_teamproj.Height = 24;
            txt_teamproj.Width = 300;
            txt_teamproj.Visible = false;

            //txt_testsuiteId.Text = "ANBC - DECEMBER FTRACK Mini Regression_11242016";
            txt_testsuiteId.Name = "txt_testsuiteId";
            txt_testsuiteId.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testsuiteId.Height = 24;
            txt_testsuiteId.Width = 300;
            txt_testsuiteId.Visible = false;
            

            //Connect button settings            
            btn_connect.Text = "Connect";
            btn_connect.Name = "btn_connect";
            btn_connect.Font = new Font("Calibri", 10F, FontStyle.Regular);
            btn_connect.TextAlign = ContentAlignment.MiddleCenter;
            btn_connect.Height = 24;
            btn_connect.Width = 70;

            //Connect button settings            
            btn_migrate.Text = "Migrate";
            btn_migrate.Name = "btn_migrate";
            btn_migrate.Font = new Font("Calibri", 10F, FontStyle.Regular);
            btn_migrate.TextAlign = ContentAlignment.MiddleCenter;
            btn_migrate.Height = 24;
            btn_migrate.Width = 70;
            btn_migrate.Visible = false;

            //PageTitle Label settings           
            lbl_pagetitle.Height = 30;
            lbl_pagetitle.Width = 450;
            lbl_pagetitle.Text = "v3 Migration";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.MiddleCenter;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;

            tlp_migration.Controls.Add(lbl_pagetitle, 0, 1);
            tlp_migration.SetColumnSpan(lbl_pagetitle, 10);

            tlp_migration.Controls.Add(lbl_tfsurl, 1, 2);
            tlp_migration.Controls.Add(txt_tfsurl, 3, 2);
            tlp_migration.Controls.Add(lbl_username, 1, 4);
            tlp_migration.Controls.Add(txt_username, 3, 4);
            tlp_migration.Controls.Add(lbl_password, 1, 6);
            tlp_migration.Controls.Add(txt_password, 3, 6);
            tlp_migration.Controls.Add(btn_connect, 3, 7);
            tlp_migration.SetColumnSpan(btn_connect, 3);

            tlp_migration.Controls.Add(lbl_teamproj, 1, 9);
            tlp_migration.Controls.Add(txt_teamproj, 3, 9);
            tlp_migration.Controls.Add(lbl_testsuiteId, 1, 10);
            tlp_migration.Controls.Add(txt_testsuiteId, 3, 10);
            tlp_migration.Controls.Add(btn_migrate, 3, 11);
            tlp_migration.SetColumnSpan(btn_migrate, 11);
            tlp_migration.Controls.Add(lbl_migrationstatus, 1, 12);
            tlp_migration.SetColumnSpan(lbl_migrationstatus, 3);

            //Adding Controls to Flow Layout Panel
            flp_migration.Controls.AddRange(new Control[] { pic_gecko, tlp_migration });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_migration });
            this.Load += new System.EventHandler(v3Migration_Load);

            #endregion

            #region migration_methods            
            
            btn_connect.Click += new System.EventHandler(btn_connect_Click);
            //btn_migrate.Click += new System.EventHandler(btn_migrate_Click);

            #endregion
        }
        private void v3Migration_Load(object sender, EventArgs e)
        {

        }
        //private void btn_migrate_Click(object sender, EventArgs e)
        //{
        //    ITestCaseCollection tccoll = null;
        //    if((!string.IsNullOrEmpty(txt_teamproj.Text))&(!string.IsNullOrEmpty(txt_testsuiteId.Text)))
        //    {
        //        try
        //        {
        //            ITestManagementService tstMgtService = (ITestManagementService)projColl.GetService(typeof(ITestManagementService));
        //            ITestManagementTeamProject tfsProject = tstMgtService.GetTeamProject(txt_teamproj.Text);
        //            ITestSuiteCollection tstsuite = tfsProject.TestSuites.Query("SELECT * FROM TestSuite where Title = '" + txt_testsuiteId.Text + "'");
        //            List<ITestCaseResultCollection> tcresultColl = new List<ITestCaseResultCollection>();
        //            tccoll = tstsuite[0].AllTestCases;
        //            DialogResult res= MessageBox.Show("Test Suite Name: " + txt_testsuiteId.Text + "\n\n\nTest Cases Count : " +tccoll.Where(x=>x.Title.StartsWith("ATC:")).Count().ToString() + "\n\n\nDo You Want To Migrate ?","Migration",MessageBoxButtons.YesNo);
        //            //tccoll.Count.ToString()
        //            if(DialogResult.Yes==res)
        //            {
        //                txt_teamproj.Enabled = false;
        //                txt_testsuiteId.Enabled = false;
        //                btn_migrate.Enabled = false;                        
        //                startMigration(tccoll);
        //            }                
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("ERROR : " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("ERROR : Please provide Team Project & Test Suite");
        //    }
        //}        
        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                if((!string.IsNullOrEmpty(txt_tfsurl.Text))&&(!string.IsNullOrEmpty(txt_username.Text))&&(!string.IsNullOrEmpty(txt_password.Text)))
                {
                    if (txt_username.Text.Contains('\\'))
                    {
                        System.Net.NetworkCredential tfsCredentials = new System.Net.NetworkCredential(txt_username.Text.Split('\\')[1], txt_password.Text, txt_username.Text.Split('\\')[0]);
                        projColl = new TfsTeamProjectCollection(new Uri(txt_tfsurl.Text.Trim()), tfsCredentials);
                        projColl.EnsureAuthenticated();
                        txt_tfsurl.Enabled = false;
                        txt_username.Enabled = false;
                        txt_password.Enabled = false;
                        btn_connect.Enabled = false;
                        lbl_teamproj.Visible = true;
                        txt_teamproj.Visible = true;
                        lbl_testsuiteId.Visible = true;
                        txt_testsuiteId.Visible = true;
                        btn_migrate.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("ERROR : User ID should be Doamin\\UserId format.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR : "+ex.Message);
            } 
        }
        //private void startMigration(ITestCaseCollection tcColl)
        //{
        //    lbl_migrationstatus.Text = "Please Wait.....\n\nMigration is In-Process..";
        //   try
        //   {
        //       if (tcColl != null)
        //       {
        //           //lbl_migrationstatus.Text = "Please Wait.....Migration is In-Process..";
        //           foreach (ITestCase testcase in tcColl)
        //           {
        //               if ((repositioryFlag) && (testcase.Title.StartsWith("ATC:")))
        //               {
        //                   //lbl_migrationstatus.Text = "Please Wait.....Migration is In-Process..";
        //                   ImportObjectRepositiory(testcase);
        //                   repositioryFlag = false;
        //                   ImportTestCaseandData(testcase);
        //               }
        //               else if (testcase.Title.StartsWith("ATC:"))
        //               {
        //                   //lbl_migrationstatus.Text = "Please Wait.....Migration is In-Process..";
        //                   ImportTestCaseandData(testcase);
        //               }
        //           }                   
        //           lbl_migrationstatus.Text = "Migration Completed Successfully.";
        //           txt_teamproj.Enabled = true;
        //           txt_testsuiteId.Enabled = true;
        //           btn_migrate.Enabled = true;
        //           //MessageBox.Show("Migration Completed", "Migration");
        //           //this.Close();
        //       }
        //   }
        //   catch(Exception ex)
        //   {
        //       txt_teamproj.Enabled = true;
        //       txt_testsuiteId.Enabled = true;
        //       btn_migrate.Enabled = true;
        //       lbl_migrationstatus.Text = "Migration Failed. \n\nERROR : "+ex.Message;
        //      // MessageBox.Show("Migration Failed.\n\n ERROR : "+ex.Message, "Migration");
        //   }
        //}
        //private void ImportObjectRepositiory(ITestCase testcase)
        //{
        //    var repositioryStep = (from a in testcase.Actions
        //                         where a is ISharedStepReference
        //                         let s = ((ISharedStepReference)a).FindSharedStep()
        //                         select s).SingleOrDefault();
        //    GetSharedStepDto(repositioryStep);
        //}
        //private void GetSharedStepDto(ISharedStep tfsSharedStep)
        //{            
        //    if (tfsSharedStep != null)
        //    {
        //        var sharedStepAction = tfsSharedStep.Actions;
        //        ITestStep repStep = (ITestStep)sharedStepAction[0];
        //        IEnumerable<ITestAttachment> ORfile = repStep.Attachments;

        //        if (ORfile.Count() == 1)
        //        {
        //            foreach (var att in ORfile)
        //            {
        //                att.DownloadToFile(".\\MASTEROR_" + tfsSharedStep.Id + ".xlsx");
        //                ImportMasterOR(tfsSharedStep.Id);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Ambigiuty Due to Multiple Attachments. Test Case ID :" + tfsSharedStep.Id, "OR Migration");
        //        }
        //    }

        //}
        private void ImportMasterOR(int testcaseId)
        {
            var xlFile = new ExcelQueryFactory(@".\\MASTEROR_" + Convert.ToString(testcaseId) + ".xlsx");
            var sheetnames = ImportSheetNames(testcaseId);
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> pagerepo = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
            foreach (var sheetname in sheetnames)
            {
                var xldata = from a in xlFile.Worksheet(sheetname) select a;
                var xlcolnames = xlFile.GetColumnNames(sheetname).ToArray();
                Dictionary<string, Dictionary<string, string>> labels = new Dictionary<string, Dictionary<string, string>>();
                foreach (var item in xldata)
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    foreach (var col in xlcolnames)
                    {
                        temp.Add(col.ToLower(), string.IsNullOrEmpty(item[col].Value.ToString()) ? null : item[col].Value.ToString());
                    }
                    if(temp["label"]!=null)
                        labels.Add(temp["label"], temp);
                }
                pagerepo.Add(sheetname, labels);
            }
            if (File.Exists(@".\\MASTEROR_" + Convert.ToString(testcaseId) + ".xlsx"))
                File.Delete(@".\\MASTEROR_" + Convert.ToString(testcaseId) + ".xlsx");
            objLib.InsertMasterOR(pagerepo);
        }
        private string[] ImportSheetNames(int id)
        {
            var xlFile = new ExcelQueryFactory(@".\\MASTEROR_" + Convert.ToString(id) + ".xlsx");
            var pagetitles = xlFile.GetWorksheetNames().ToArray();
            objLib.InsertPageNames(pagetitles);            
            return pagetitles;
        }
        //private void ImportTestCaseandData(ITestCase testcase)
        //{
        //    importtestcases(testcase);
        //    importtestdata(testcase);
        //}
        //private void importtestcases(ITestCase tc)
        //{
        //    objLib.ImportActionflow(tc);        
        //}
        //private void importtestdata(ITestCase tc)
        //{
        //    TestActionCollection tstActions = tc.Actions;
        //    ITestStep TestDataStep = (ITestStep)tstActions[1];
        //    IEnumerable<ITestAttachment> tdAttachment = TestDataStep.Attachments;

        //    if (tdAttachment.Count() == 1)
        //    {
        //        foreach (var att in tdAttachment)
        //        {
        //            att.DownloadToFile(".\\TESTDATA_" + tc.Id + ".xlsx");
        //            ImportData(tc);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Ambigiuty Due to Multiple Attachments. Test Case ID :" + tc.Id, "TestData Migration");
        //    }            
        //}
        //private void ImportData(ITestCase testcase)
        //{
        //    var xlFile = new ExcelQueryFactory(@".\\TESTDATA_" + testcase.Id + ".xlsx");
        //    xlFile.DatabaseEngine = DatabaseEngine.Ace;
        //    var sheetnames = xlFile.GetWorksheetNames().ToArray();
        //    var directiveMap = new Dictionary<string, IEnumerable<TestDataDirective>>();
        //    foreach (var sheetname in sheetnames)
        //    {
        //        var rows = from a in xlFile.Worksheet(sheetname) select a;
        //        var xlcolnames = xlFile.GetColumnNames(sheetname).ToArray();
        //        var directives = new List<TestDataDirective>();
        //        foreach (var row in rows)
        //        {
        //            var directive = new TestDataDirective();
        //            foreach (var key in row.ColumnNames.Where(IsImportantColumn))
        //            {
        //                if (string.IsNullOrWhiteSpace(row[key])) continue;
        //                switch (key.Replace(" ", string.Empty).ToLower())
        //                {
        //                    case "execute":
        //                        directive.ShouldExecute = IsPositive(row[key]);
        //                        break;
        //                    case "tc#":
        //                    case "tc #":
        //                        directive.TestCaseNumber = row[key];
        //                        break;
        //                    case "flowidentifier":
        //                        directive.FlowIdentifier = int.Parse(row[key]);
        //                        break;
        //                    case "dataidentifier":
        //                        directive.DataIdentifier = int.Parse(row[key]);
        //                        break;
        //                    case "indicator":
        //                        directive.Indicator = row[key];
        //                        break;
        //                    default:
        //                        directive.Interactions.Add(new TestDataInteraction { LogicalFieldName = key, Value = row[key] });
        //                        break;
        //                }
        //            }
        //            directives.Add(directive);
        //        }
        //        directiveMap.Add(sheetname.ToLower(), directives);
        //    }
        //    var tcdirective = objLib.GetTestCaseDirective();
        //    if (File.Exists(@".\\TESTDATA_" + testcase.Id + ".xlsx"))
        //        File.Delete(@".\\TESTDATA_" + testcase.Id + ".xlsx");
        //    objLib.InsertTestData(tcdirective, directiveMap, testcase.Id);            
        //}
        private bool IsPositive(Cell cell)
        {
            return string.IsNullOrWhiteSpace(cell.Value.ToString())
                || new[] { "yes", "true" }.Contains(cell.Value.ToString(), StringComparer.OrdinalIgnoreCase);
        }
        private bool IsImportantColumn(string columnName)
        {
            var ignorableColumnPatterns = new[]
            {
                @"^Comments?\s*\d*$",
                @"^Scenario\s*Id$",
                @"^Scenario\s*Description$",
                @"^States?$",
                @"^Rule\s*Description$",
                @"^TC#$",
                @"^Validations?$",
            };

            return ignorableColumnPatterns.All(
                    p => !Regex.IsMatch(columnName, p, RegexOptions.IgnoreCase));
        }
    }
    public class TestCaseFlowInteraction
    {
        public int PageId { get; set; }
        public int FlowIdentifier { get; set; }
    }
}
