using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportAnalyzeResFulfilment : RESPage
    {
        private string sessionResultsKey = "ReportAnalyzeResFulfilment";

        const string All = "Всички";

        string militaryUnitId = "";
        string militaryCommandIds = "";
        string militaryCategoryKey = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZERESFULFILMENT") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnitId"]))
                {
                    militaryUnitId = Request.Params["MilitaryUnitId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCommandIds"]))
                {
                    militaryCommandIds = Request.Params["MilitaryCommandIds"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCategoryKey"]))
                {
                    militaryCategoryKey = Request.Params["MilitaryCategoryKey"];
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportAnalyzeResFulfilment.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        private string GetHeaderForExport(int columnsSpan, string militaryUnitName, string militaryCommandName, string militaryCategoryName)
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
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H4""><Data ss:Type=""String"">Анализ на комплектуването</Data></Cell>
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
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Категория: <B>" + militaryCategoryName + @"</B></Data></Cell>
                    </Row>
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""" + mergeAccross + @""" ss:StyleID=""H3""/>
                    </Row>";

            return xml;
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            ReportAnalyzeResFulfilmentFilter filter = new ReportAnalyzeResFulfilmentFilter()
            {
                MilitaryUnitId = int.Parse(militaryUnitId),
                MilitaryCommandIds = militaryCommandIds,
                MilitaryCategoryKey = militaryCategoryKey
            };

            ReportAnalyzeResFulfilmentResult reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportAnalyzeResFulfilmentResult)Session[sessionResultsKey];
            else
                reportResult = ReportAnalyzeResFulfilmentUtil.GetReportAnalyzeResFulfilment(filter, CurrentUser);


            string militaryUnitName = "";
            MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(filter.MilitaryUnitId, CurrentUser);

            if (militaryUnit != null)
                militaryUnitName = militaryUnit.DisplayTextForSelection;


            string militaryCommandName = "";
            List<MilitaryCommand> militaryCommands = MilitaryCommandUtil.GetAllMilitaryCommandsByIDs(CurrentUser, militaryCommandIds);
            foreach (MilitaryCommand militaryCommand in militaryCommands)
            {
                militaryCommandName += (militaryCommandName == "" ? "" : ", ") + militaryCommand.DisplayTextForSelection;
            }

            string militaryCategoryName = "";
            MilitaryCategoryForReports militaryCategory = MilitaryCategoryForReportsUtil.GetMilitaryCategoryByKey(filter.MilitaryCategoryKey);

            if (militaryCategory != null)
                militaryCategoryName = militaryCategory.CategoryName;


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
                                  <Style ss:ID=""C3"" ss:Parent=""N"">
                                    <Alignment ss:Horizontal=""Right"" ss:Vertical=""Bottom"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                    <Borders>
                                        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                    </Borders>
                                    <Font ss:FontName=""Arial"" ss:Size=""10""/>
                                    <NumberFormat ss:Format=""0.0%""/>
                                  </Style>
                           
                           </Styles>
");

            bool anyData = false;

            if (reportResult.ByCategoryResult != null && reportResult.ByCategoryResult.Count > 0)
            {
                anyData = true;

                html.Append(@"
                    <Worksheet ss:Name=""Разпределение по категории"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""40"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""200"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    " + GetHeaderForExport(8, militaryUnitName, militaryCommandName, militaryCategoryName) + @"
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""8"" ss:StyleID=""H5""><Data ss:Type=""String"">Разпределението на личния състав от запаса по команди е както следва:</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">№ по ред</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">№ на команда</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Военно формирование</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">ВСИЧКО</Data></Cell>
                    </Row>");

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByCategoryBlock block in reportResult.ByCategoryResult)
                {
                    counter++;

                    html.Append(@"
                    <Row ss:AutoFitHeight=""1"">
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + counter.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryCommand + @"</Data></Cell>
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryUnit + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSoldiers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledTotal.ToString() + @"</Data></Cell>
                    </Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.ByMRSResult != null && reportResult.ByMRSResult.Count > 0)
            {
                anyData = true;

                html.Append(@"
                    <Worksheet ss:Name=""Разпределение по ВОС"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""40"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""70"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    " + GetHeaderForExport(7, militaryUnitName, militaryCommandName, militaryCategoryName) + @"
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H5""><Data ss:Type=""String"">Получили мобилизационно назначение по военноотчетни специалности:</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">№ по ред</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">№ на команда</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">По щат</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">По основна ВОС</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">%</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Заменени</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">%</Data></Cell>
                    </Row>");

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByMRSBlock block in reportResult.ByMRSResult)
                {
                    counter++;

                    html.Append(@"
                    <Row ss:AutoFitHeight=""1"">
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + counter.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryCommand + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.CountByStaff.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledByMRS.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C3""><Data ss:Type=""Number"">" + ((decimal)(block.FulfiledByMRSPerc / 100)).ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesCnt.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C3""><Data ss:Type=""Number"">" + ((decimal)(block.ChangesPerc / 100)).ToString() + @"</Data></Cell>
                    </Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.ByCommandResult != null && reportResult.ByCommandResult.Count > 0)
            {
                anyData = true;

                html.Append(@"
                    <Worksheet ss:Name=""Разпределение по команди"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""40"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""200"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""85"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    " + GetHeaderForExport(6, militaryUnitName, militaryCommandName, militaryCategoryName) + @"
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""8"" ss:StyleID=""H5""><Data ss:Type=""String"">Разпределение на личният състав по команди:</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">№ по ред</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">№ на команда</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Военно формирование</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">По щат</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Подадени от ВО</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">% на комплектоване</Data></Cell>
                    </Row>");

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByCommandBlock block in reportResult.ByCommandResult)
                {
                    counter++;

                    html.Append(@"
                    <Row ss:AutoFitHeight=""1"">
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + counter.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryCommand + @"</Data></Cell>
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryUnit + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.CountByStaff.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledByMilitaryDepartment.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C3""><Data ss:Type=""Number"">" + ((decimal)(block.FulfiledPerc / 100)).ToString() + @"</Data></Cell>
                    </Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.ByAgeResult != null && reportResult.ByAgeResult.Count > 0)
            {
                anyData = true;

                html.Append(@"
                    <Worksheet ss:Name=""Разпределение по възраст"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""40"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    " + GetHeaderForExport(5, militaryUnitName, militaryCommandName, militaryCategoryName) + @"
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""8"" ss:StyleID=""H5""><Data ss:Type=""String"">Комплектоването на длъжностите по възраст:</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ по ред</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ на команда</Data></Cell>
                        <Cell ss:MergeAcross=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">Възраст</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:Index=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">До 35 г.</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">До 45 г.</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Над 45 г.</Data></Cell>
                    </Row>");

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByAgeBlock block in reportResult.ByAgeResult)
                {
                    counter++;

                    html.Append(@"
                    <Row ss:AutoFitHeight=""1"">
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + counter.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryCommand + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledAgeUnder35.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledAgeUnder45.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledAgeAbove45.ToString() + @"</Data></Cell>
                    </Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.ByEdu_Officers_Result != null && reportResult.ByEdu_Officers_Result.Count > 0 ||
                reportResult.ByEdu_OffCand_Result != null && reportResult.ByEdu_OffCand_Result.Count > 0 ||
                reportResult.ByEdu_Sergeants_Result != null && reportResult.ByEdu_Sergeants_Result.Count > 0 ||
                reportResult.ByEdu_Soldiers_Result != null && reportResult.ByEdu_Soldiers_Result.Count > 0)
            {
                anyData = true;

                html.Append(@"
                    <Worksheet ss:Name=""Разпределение по образование"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""40"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    " + GetHeaderForExport(7, militaryUnitName, militaryCommandName, militaryCategoryName) + @"
                    ");

                if (reportResult.ByEdu_Officers_Result != null && reportResult.ByEdu_Officers_Result.Count > 0)
                {
                    html.Append(@"
                        <Row ss:Height=""20.25"">                        
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""H5""><Data ss:Type=""String"">По притежавано образование комплектуването е:</Data></Cell>
                        </Row>
                        <Row>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ по ред</Data></Cell>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ на команда</Data></Cell>
                            <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Военно образование</Data></Cell>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">ВСИЧКО</Data></Cell>
                        </Row>
                        <Row ss:Height=""25.5"">
                            <Cell ss:Index=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">ВА (ГЩФ)</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Военно училище</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Школи</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Без военно образование</Data></Cell>
                        </Row>");

                    int counter = 0;
                    foreach (ReportAnalyzeResFulfilmentByEdu_Officers_Block block in reportResult.ByEdu_Officers_Result)
                    {
                        counter++;

                        html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + counter.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryCommand + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledVA.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledVU.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSZHO.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledNoMilEdu.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledTotal.ToString() + @"</Data></Cell>
                        </Row>");
                    }

                    html.Append(@"<Row><Cell/></Row>");
                }

                if (reportResult.ByEdu_OffCand_Result != null && reportResult.ByEdu_OffCand_Result.Count > 0)
                {
                    html.Append(@"
                        <Row ss:Height=""20.25"">                        
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""H5""><Data ss:Type=""String"">По притежавано образование комплектуването е:</Data></Cell>
                        </Row>
                        <Row>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ по ред</Data></Cell>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ на команда</Data></Cell>
                            <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Военно образование</Data></Cell>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">ВСИЧКО</Data></Cell>
                        </Row>
                        <Row ss:Height=""25.5"">
                            <Cell ss:Index=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Военно училище</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Колеж</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">........</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Без военно образование</Data></Cell>
                        </Row>");

                    int counter = 0;
                    foreach (ReportAnalyzeResFulfilmentByEdu_OffCand_Block block in reportResult.ByEdu_OffCand_Result)
                    {
                        counter++;

                        html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + counter.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryCommand + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledVU.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledCollege.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""String""></Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledNoMilEdu.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledTotal.ToString() + @"</Data></Cell>
                        </Row>");
                    }

                    html.Append(@"<Row><Cell/></Row>");
                }

                if (reportResult.ByEdu_Sergeants_Result != null && reportResult.ByEdu_Sergeants_Result.Count > 0)
                {
                    html.Append(@"
                        <Row ss:Height=""20.25"">                        
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""H5""><Data ss:Type=""String"">По притежавано образование комплектуването е:</Data></Cell>
                        </Row>
                        <Row>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ по ред</Data></Cell>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ на команда</Data></Cell>
                            <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Военно образование</Data></Cell>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">ВСИЧКО</Data></Cell>
                        </Row>
                        <Row ss:Height=""25.5"">
                            <Cell ss:Index=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Военно училище</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Колеж</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">........</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Без военно образование</Data></Cell>
                        </Row>");

                    int counter = 0;
                    foreach (ReportAnalyzeResFulfilmentByEdu_Sergeants_Block block in reportResult.ByEdu_Sergeants_Result)
                    {
                        counter++;

                        html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + counter.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryCommand + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledVU.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledCollege.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""String""></Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledNoMilEdu.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledTotal.ToString() + @"</Data></Cell>
                        </Row>");
                    }

                    html.Append(@"<Row><Cell/></Row>");
                }

                if (reportResult.ByEdu_Soldiers_Result != null && reportResult.ByEdu_Soldiers_Result.Count > 0)
                {
                    html.Append(@"
                        <Row ss:Height=""20.25"">                        
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""H5""><Data ss:Type=""String"">По притежавано образование комплектуването е:</Data></Cell>
                        </Row>
                        <Row>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ по ред</Data></Cell>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">№ на команда</Data></Cell>
                            <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Притежавано образование</Data></Cell>
                            <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">ВСИЧКО</Data></Cell>
                        </Row>
                        <Row ss:Height=""25.5"">
                            <Cell ss:Index=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Висше</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Средно</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Основно</Data></Cell>
                            <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Без образование</Data></Cell>
                        </Row>");

                    int counter = 0;
                    foreach (ReportAnalyzeResFulfilmentByEdu_Soldiers_Block block in reportResult.ByEdu_Soldiers_Result)
                    {
                        counter++;

                        html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + counter.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilitaryCommand + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledVisshe.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSredno.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOsnovno.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledNoEdu.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledTotal.ToString() + @"</Data></Cell>
                        </Row>");
                    }

                    html.Append(@"<Row><Cell/></Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (!anyData)
            {
                html.Append(@"<Worksheet ss:Name=""Няма данни"">
                                <Table ss:StyleID=""N"">
                                    <Column ss:StyleID=""N"" ss:Width=""120"" />
                                    <Row>
                                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">Няма намерени данни</Data></Cell>
                                    </Row>
                                </Table>
                                <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                                    <DoNotDisplayGridlines/>
                                </WorksheetOptions>
                              </Worksheet>");
            }

            html.Append("</Workbook>");

            return html.ToString();
        }
    }
}
