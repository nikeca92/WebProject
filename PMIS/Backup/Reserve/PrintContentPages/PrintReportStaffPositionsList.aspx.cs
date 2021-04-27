using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class ReportStaffPositionsList : RESPage
    {
        private string sessionResultsKey = "ReportStaffPositionsList";

        const string All = "Всички";

        string militaryUnitId = "";
        string militaryCommandId = "";
        string subMilitaryCommandSuffix = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTSTAFFPOSITIONLIST") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnitId"]))
                {
                    militaryUnitId = Request.Params["MilitaryUnitId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCommandId"]))
                {
                    militaryCommandId = Request.Params["MilitaryCommandId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["SubMilitaryCommandSuffix"]))
                {
                    subMilitaryCommandSuffix = Request.Params["SubMilitaryCommandSuffix"];
                }

                if (!IsPostBack)
                {
                    if (Request.Params["Export"] != null && Request.Params["Export"].ToLower() == "true")
                    {
                        btnGenerateExcel_Click(this, new EventArgs());
                    }
                }
            }
        }

        // Generate the page content's html
        private string GeneratePageContent(bool isExport)
        {
            string contentPage = "";

            if (!isExport)
            {
                
            }
            else
            {
                contentPage = this.GenerateAllRecordsForExport();
            }

            return contentPage;
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportStaffPositionsList.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        private string GetHeaderForExport(int columnsSpan, string militaryUnitName, string militaryCommandName, string subMilitaryCommandName)
        {
            string mergeAccross = ((int)(columnsSpan - 1)).ToString();
            string xml = @"
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H4""><Data ss:Type=""String"">Щатно-длъжностен списък</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">" + CommonFunctions.GetLabelText("MilitaryUnit") + @": <B>" + militaryUnitName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + subMilitaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H3""/>
                    </Row>";

            return xml;
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            ReportStaffPositionsListFilter filter = new ReportStaffPositionsListFilter()
            {
                MilitaryUnitId = int.Parse(militaryUnitId),
                MilitaryCommandId = int.Parse(militaryCommandId),
                SubMilitaryCommandSuffix = subMilitaryCommandSuffix,
                PageSize = -1,
                PageIdx = -1
            };

            ReportStaffPositionsListResult reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportStaffPositionsListResult)Session[sessionResultsKey];
            else
                reportResult = ReportStaffPositionsListUtil.GetReportStaffPositionsList(filter, CurrentUser);


            string militaryUnitName = "";
            MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(filter.MilitaryUnitId, CurrentUser);

            if (militaryUnit != null)
                militaryUnitName = militaryUnit.DisplayTextForSelection;

            string militaryCommandName = All;
            MilitaryCommand militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(filter.MilitaryCommandId, CurrentUser);
            if (militaryCommand != null)
                militaryCommandName = militaryCommand.DisplayTextForSelection;

            string subMilitaryCommandName = All;
            if (militaryCommand != null && !string.IsNullOrEmpty(filter.SubMilitaryCommandSuffix) && filter.SubMilitaryCommandSuffix != "-1")
                subMilitaryCommandName = militaryCommand.CommandNumber + " " + filter.SubMilitaryCommandSuffix + " " + militaryCommand.ShortName;

            StringBuilder html = new StringBuilder();
            html.Append(@"<?xml version=""1.0""?> 

						   <Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""
									 xmlns:o=""urn:schemas-microsoft-com:office:office""
									 xmlns:x=""urn:schemas-microsoft-com:office:excel""
									 xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""
									 xmlns:html=""http://www.w3.org/TR/REC-html40"">

							<Styles>
                                <Style ss:ID=""N"" ss:Name=""Normal"">
                                    <Alignment ss:Horizontal=""Right"" ss:Vertical=""Bottom""/>
                                    <Borders/>
                                    <Font ss:FontName=""Arial""/>
                                    <Interior/>
                                    <NumberFormat/>                            
                                </Style>

                                 <Style ss:ID=""H"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""16"" ss:Bold=""1""/>                         
                                 </Style>
                                 <Style ss:ID=""H2"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""20"" ss:Bold=""1""/>                         
                                 </Style>
                                <Style ss:ID=""H3"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" />                                   
                                 </Style>
                                 <Style ss:ID=""H4"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""13"" ss:Bold=""1""/>                         
                                 </Style>
                                 <Style ss:ID=""H5"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Left"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""13"" ss:Bold=""1""/>                         
                                 </Style>
                                 <Style ss:ID=""H6"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""10""/>                         
                                 </Style>
                                <Style ss:ID=""H7"" ss:Parent=""N"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                    <Borders>
                                        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                    </Borders>
                                    <Font ss:FontName=""Arial"" ss:Size=""10"" ss:Bold=""1""/>                                    
                                </Style>                               
                                 
                                 
                                  <Style ss:ID=""C1"" ss:Parent=""N"">
                                    <Alignment ss:Horizontal=""Right"" ss:Vertical=""Bottom"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                    <Borders>
                                        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                    </Borders>
                                    <Font ss:FontName=""Arial"" ss:Size=""10""/>
                                  </Style>
                                  <Style ss:ID=""C2"" ss:Parent=""N"">
                                    <Alignment ss:Horizontal=""Left"" ss:Vertical=""Bottom"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                    <Borders>
                                        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                    </Borders>
                                    <Font ss:FontName=""Arial"" ss:Size=""10""/>
                                  </Style>
                           
                           </Styles>
");

            
            html.Append(@"
                <Worksheet ss:Name=""Щатно-длъжностен списък"">
                <Table ss:StyleID=""N"">
                <Column ss:StyleID=""N"" ss:Width=""30"" />
                <Column ss:StyleID=""N"" ss:Width=""180"" />
                <Column ss:StyleID=""N"" ss:Width=""70"" />
                <Column ss:StyleID=""N"" ss:Width=""100"" />
                <Column ss:StyleID=""N"" ss:Width=""70"" />
                <Column ss:StyleID=""N"" ss:Width=""170"" />
                <Column ss:StyleID=""N"" ss:Width=""100"" />
                <Column ss:StyleID=""N"" ss:Width=""60"" />
                <Column ss:StyleID=""N"" ss:Width=""200"" />
                <Column ss:StyleID=""N"" ss:Width=""100"" />
                <Column ss:StyleID=""N"" ss:Width=""90"" />
                <Column ss:StyleID=""N"" ss:Width=""80"" />
                <Column ss:StyleID=""N"" ss:Width=""60"" />
                " + GetHeaderForExport(13, militaryUnitName, militaryCommandName, subMilitaryCommandName) + @"
                <Row ss:Height=""25.5"">
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">№ по ред</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeAcross=""3""><Data ss:Type=""String"">Данни по щат</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">Звание, име, презиме, фамилия</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">ВОС</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">ЕГН</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">Постоянен адрес</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">Образование</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">От кое военно окръжие е получил назначението</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">Полагащо се по щат оръжие</Data></Cell>
                    <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">Дата на приемане</Data></Cell>
                </Row>
                <Row ss:Height=""25.5"">
                    <Cell ss:Index=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">Наименование на длъжността</Data></Cell>
                    <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Звание</Data></Cell>
                    <Cell ss:StyleID=""H7""><Data ss:Type=""String"">ВОС</Data></Cell>
                    <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Код на длъжността</Data></Cell>
                </Row>");

            foreach (ReportStaffPositionsListBlock block in reportResult.Result)
            {
                html.Append(@"
                <Row ss:AutoFitHeight=""1"">
                    <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RowIndex.ToString() + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.PositionName + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryRankName + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.StaffMRS + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.PositionCode + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.PersonFullNameAndRank + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.PersonMRS + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.PersonIdentityNumber + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.PersonPermAddress + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.PersonEducation + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryDepartment + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.StaffWeapon + @"</Data></Cell>
                    <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + CommonFunctions.FormatDate(block.AppointmentDate) + @"</Data></Cell>
                </Row>");
            }

          
            html.Append(@"</Table>
                    <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                        <DoNotDisplayGridlines/>
                    </WorksheetOptions></Worksheet>");

            html.Append("</Workbook>");

            return html.ToString();
        }
    }
}
