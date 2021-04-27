using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.IO;


namespace PMIS.Reserve.ContentPages
{
    public partial class ImportPersonsData : RESPage
    {
        int MAX_NUMBER_OF_LINES = 1000;

        string redirectBack = "";

        public override string PageUIKey
        {
            get
            {
                return "TODO";
            }
        }       

        //This is a flag field that says if the screen is opened from the Home screen
        //This is used to navigate the user back to the home screen when using the Back button
        private int FromHome
        {
            get
            {
                int fh = 0;
                if (String.IsNullOrEmpty(this.hdnFromHome.Value) || this.hdnFromHome.Value == "0")
                {
                    if (Request.Params["fh"] != null)
                        int.TryParse(Request.Params["fh"].ToString(), out fh);

                    this.hdnFromHome.Value = hdnFromHome.ToString();
                }
                else
                {
                    Int32.TryParse(this.hdnFromHome.Value, out fh);
                }

                return fh;
            }

            set
            {
                this.hdnFromHome.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied - TO DO change UI keys
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden 
                || GetUIItemAccessLevel("RES_HUMANRES_MILITARYREPORTPERSON") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }           

            //Hilight the correct item in the menu
            HighlightMenuItems("Reservists", "ImportPersonsData");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);           

            if (Request.Params["fh"] == "1")
                redirectBack = "~/ContentPages/Home.aspx";            

            if (!IsPostBack)
            {
                string header = "Импорт на нови за АСУ ВОЛ";
                lblHeaderTitle.InnerHtml = header;
                this.Title = header;

                //Populate the drop-downs
                PopulateDropdowns();
            }
        }

        //Populate the drop-downs
        private void PopulateDropdowns()
        {
            this.PopulateMilitaryDepartments();
        }

        //Populate the MilitaryDepartments drop-down
        private void PopulateMilitaryDepartments()
        {
            this.ddlMilitaryDepartments.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsWithoutRestrictions(CurrentUser);
            this.ddlMilitaryDepartments.DataTextField = "MilitaryDepartmentName";
            this.ddlMilitaryDepartments.DataValueField = "MilitaryDepartmentId";
            this.ddlMilitaryDepartments.DataBind();
            this.ddlMilitaryDepartments.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(redirectBack);
        }

        protected void btnImportPersonsData_Click(object sender, EventArgs e)
        {
            spanInfo.InnerHtml = "";

            if (ddlMilitaryDepartments.SelectedValue == ListItems.GetOptionChooseOne().Value)
            {
                spanInfo.Attributes["class"] = "ErrorText";
                spanInfo.InnerHtml += CommonFunctions.GetErrorMessageMandatory("Военно окръжие") + "<br />";
            }

            if (!fuFileToImport.HasFile)
            {
                spanInfo.Attributes["class"] = "ErrorText";
                spanInfo.InnerHtml += CommonFunctions.GetErrorMessageMandatory("Файл за импортиране");
            }

            if (fuFileToImport.HasFile &&
                ddlMilitaryDepartments.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                ReservistPersonsDataImporter reservistPersonsDataImporter = new ReservistPersonsDataImporter(CurrentUser, fuFileToImport.FileContent, int.Parse(ddlMilitaryDepartments.SelectedValue));

                if (reservistPersonsDataImporter.LinesCount > MAX_NUMBER_OF_LINES)
                {
                    spanInfo.Attributes["class"] = "ErrorText";
                    spanInfo.InnerHtml += "Максималният допустим брой редове в един файл е " + MAX_NUMBER_OF_LINES.ToString() + ", а в този файл има " + reservistPersonsDataImporter.LinesCount.ToString() + " реда";
                }
                else
                {
                    reservistPersonsDataImporter.ParseImportFile();

                    Change change = new Change(CurrentUser, "RES_MilitaryReportPersons");
                    string logDescription = @"Файл: " + fuFileToImport.FileName +
                                            @";Военно окръжие: " + MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(ddlMilitaryDepartments.SelectedValue), CurrentUser).MilitaryDepartmentName +
                                            @";Обработени записи: " + reservistPersonsDataImporter.AllLines +
                                            @";Успешно импортирани записи: " + reservistPersonsDataImporter.Persons.Count +
                                            @";Добавени специалности: " + reservistPersonsDataImporter.ProfessionAndSpecialityRecordsImported +
                                            @";Грешни записи: " + reservistPersonsDataImporter.ErrorLines.ToString() +
                                            @";Изключения: " + reservistPersonsDataImporter.Exceptions.Length.ToString();
                    ChangeEvent changeEvent = new ChangeEvent("RES_ImportMilitaryReportPersons", logDescription, null, null, CurrentUser);
                    change.AddEvent(changeEvent);
                    change.WriteLog();

                    hdnImportExceptionsFile.Value = string.Join("\r\n", reservistPersonsDataImporter.Exceptions);

                    spanResult.Attributes["class"] = "SuccessText";
                    spanResult.InnerHtml = "Файлът <b>" + fuFileToImport.FileName + "</b> е успешно импортиран";
                    spanResult.InnerHtml += "<br />Обработени са " + reservistPersonsDataImporter.AllLines + " записа";
                    spanResult.InnerHtml += "<br />Успешно импортирани са " + reservistPersonsDataImporter.Persons.Count + " записа";
                    spanResult.InnerHtml += "<br />За обработените записи са добавени " + reservistPersonsDataImporter.ProfessionAndSpecialityRecordsImported + " специалности";
                    //if (reservistPersonsDataImporter.ErrorLines > 0)
                    //    spanResult.InnerHtml += @"<br /><div class=""ErrorText"" style='padding-top: 8px; text-align: left;'>Грешните записи във файла са " + reservistPersonsDataImporter.ErrorLines.ToString() + " записа</div>";
                    if (reservistPersonsDataImporter.NoChangesCnt > 0)
                        spanResult.InnerHtml += "<br />Пропуснати са (вече съществуват в системата) " + reservistPersonsDataImporter.NoChangesCnt + " записа";
                    
                    if (reservistPersonsDataImporter.Exceptions.Length > 0)
                        spanResult.InnerHtml += @"<br /><div class=""ErrorText"" style='padding-top: 8px; text-align: left;'>Изключенията са " + reservistPersonsDataImporter.Exceptions.Length + @" записа и са отбелязани във файла <a href=""#"" onclick=""GetImportExceptionsFile(); return false;"">ImportExceptions.txt</a></div>";

                    contentDiv.Visible = false;
                    resultDiv.Visible = true;
                }
            }
           
        }

        protected void btnNewImport_Click(object sender, EventArgs e)
        {
            contentDiv.Visible = true;
            resultDiv.Visible = false;

            hdnImportExceptionsFile.Value = "";
            spanInfo.InnerHtml = "";
        }

        protected void hdnBtnGetImportExceptionsFile_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=\"ImportExceptions.txt\"");
            Response.ContentType = "text/plain";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetBytes(hdnImportExceptionsFile.Value));
            Response.End();
        }

        protected void lnkSampleImportDataFile_Click(object sender, EventArgs e)
        {
            string sampleImportDataFilePath = Config.GetWebSetting("SampleImportDataFilePath");
            string sampleImportDataFileName = Path.GetFileName(sampleImportDataFilePath);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=\"" + sampleImportDataFileName + "\"");
            Response.ContentType = "text";
            Response.WriteFile(sampleImportDataFilePath);
            Response.End();
        }
    }
}
