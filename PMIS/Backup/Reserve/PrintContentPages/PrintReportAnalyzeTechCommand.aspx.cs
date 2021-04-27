using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportAnalyzeTechCommand : RESPage
    {
        private string sessionResultsKey = "ReportAnalyzeTechCommand";

        const string All = "Всички";

        string militaryDepartmentId = "";
        string militaryReadinessID = "";
        string militaryCommandId = "";
        string militaryCommandSuffix = "";

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "1024";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "255";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTSV1") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCommandId"]))
                {
                    militaryCommandId = Request.Params["MilitaryCommandId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryReadinessID"]))
                {
                    militaryReadinessID = Request.Params["MilitaryReadinessID"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCommandSuffix"]))
                {
                    militaryCommandSuffix = Request.Params["MilitaryCommandSuffix"];
                }

                if (!IsPostBack)
                {
                    if (Request.Params["Export"] != null && Request.Params["Export"].ToLower() == "true")
                    {
                        btnGenerateExcel_Click(this, new EventArgs());
                    }
                    else
                    {
                        this.divResults.InnerHtml = GeneratePageContent(false);
                    }
                }
            }
            else
            {
                this.divResults.InnerHtml = "";
            }
        }

        // Generate the page content's html
        private string GeneratePageContent(bool isExport)
        {
            string contentPage = "";

            if (!isExport)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");
                sb.Append("<tr>");
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllRecordsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllRecordsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllRecordsHtml()
        {
            ReportAnalyzeTechCommandFilter filter = new ReportAnalyzeTechCommandFilter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                MilitaryCommandId = int.Parse(militaryCommandId),
                MilitaryReadinessID = int.Parse(militaryReadinessID),
                MilitaryCommandSuffix = militaryCommandSuffix
            };

            ReportAnalyzeTechCommandResult reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportAnalyzeTechCommandResult)Session[sessionResultsKey];
            else
                reportResult = ReportAnalyzeTechCommandUtil.GetReportAnalyzeTechCommand(filter, CurrentUser);


            string militaryDepartmentName = "";
            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByIDs(CurrentUser, militaryDepartmentId);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                militaryDepartmentName += (militaryDepartmentName == "" ? "" : ", ") + militaryDepartment.MilitaryDepartmentName;
            }

            if(militaryDepartmentName == "")
            {
                militaryDepartmentName = All;
            }

            string militaryReadinessName = All;
            MilitaryReadiness militaryReadinesses = MilitaryReadinessUtil.GetMilitaryReadiness(filter.MilitaryReadinessID, CurrentUser);

            if (militaryReadinesses != null)
                militaryReadinessName = militaryReadinesses.MilReadinessName;  

            string militaryCommandName = All;
            MilitaryCommand militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(filter.MilitaryCommandId, CurrentUser);
            if (militaryCommand != null)
                militaryCommandName = militaryCommand.DisplayTextForSelection;

            string militarySubCommand = All;
            if (militaryCommand != null && !string.IsNullOrEmpty(militaryCommandSuffix) && militaryCommandSuffix != "-1")
                militarySubCommand = militaryCommand.CommandNumber + " " + militaryCommandSuffix + " " + militaryCommand.ShortName;

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px; width: 1024px;'>
                             <tr>
                                <td align='left'>
                                    <span class='Label'>Военно окръжие:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryDepartmentName + @"</span>                                  
                                </td>
                             </tr>
                             <tr>
                                <td align='left'>
                                    <span class='Label'>Готовност:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryReadinessName + @"</span>                                  
                                </td>
                             </tr>
                             <tr>
                                <td align='left'>                                   
                                    <span class='Label'>Команда:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryCommandName + @"</span>                                    
                                </td>
                             </tr>
                             <tr>
                                <td align='left'>                                  
                                    <span class='Label'>Подкоманда:&nbsp;</span>
                                    <span class='ValueLabel'>" + militarySubCommand + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td>&nbsp;</td>
                             </tr>
                            ");

            if (reportResult.OverallResult != null)
            {
                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='left'>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'></div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>                               
                               <th style='width: 480px;' colspan='2'>Заявка от формированието</th>
                               <th style='width: 200px;' colspan='2'>Фактическо изпълнение</th>
                               <th style='width: 100px;' rowspan='2'>Процент резерв с МН /ед.т./</th>                                                              
                            </tr> 
                            <tr>
                               <th style='width: 400px;'>Вид и тип на техниката</th>
                               <th style='width: 80px;'>Брой</th>
                               <th style='width: 80px;'>Брой</th>
                               <th style='width: 120px;'>От тях по заменки бр.</th>
                            </tr>
                         </thead>");

                int counter = 0;
                foreach (ReportOverallAnalyzeTechCommandResultBlock block in reportResult.OverallResult)
                {
                    counter++;
                    html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                                
                                     <td style='" + cellStyle + @" text-align: left;'>" + block.NormativeName.ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: right;'>" + block.RequestedPos.ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: right;'>" + block.FilledPos.ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: right;'>" + block.ReplacedPos.ToString() + @"</td>
                                     <td style='" + cellStyle + @" text-align: right;'>" + block.ReservePos.ToString() + @"</td>
                                  </tr>");
                }
                
                html.Append(@"</table>
                          </td></tr>");
            }

            if (reportResult.PositionDeliveryResult != null && reportResult.PositionDeliveryResult.Count > 0)
            {
                var technicsTypes = TechnicsTypeUtil.GetAllTechnicsTypes(CurrentUser);
                var vehicleKinds = GTableItemUtil.GetAllGTableItemsByTableName("VehicleKind", ModuleKey, 1, 0, 0, CurrentUser).OrderBy(x => x.TableSeq).ToList();
                var readinessTypeColspan = technicsTypes.Count + vehicleKinds.Count;
                var readinessTypes = new int[] { 1, 2 };

                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='center'>
                         <div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>Анализ по доставяне</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>                        
                               <th style='width: 240px;' rowspan='3'>Община</th>
                               <th style='width: 240px;' rowspan='3'>Район</th>
                               <th style='width: 120px;' colspan='" + readinessTypeColspan + @"'>Основно попълнение</th>
                               <th style='width: 120px;' colspan='" + readinessTypeColspan + @"'>Резерв</th>                                                            
                            </tr>");

                foreach (var readiness in readinessTypes)
                {
                    foreach (var technicsType in technicsTypes)
                    {
                        if (technicsType.TypeKey == "VEHICLES")
                            html.Append("<th colspan='" + vehicleKinds.Count + @"'>" + technicsType.TypeName + "</th>");
                        else
                            html.Append("<th style='width: 120px;' rowspan='2'>" + technicsType.TypeName + "</th>");
                    }
                    html.Append("<th style='width: 120px;' rowspan='2'>Всичко</th>");
                }

                html.Append(@"</tr>
                              <tr>");

                foreach (var readiness in readinessTypes)
                {
                    foreach (var vehicleKind in vehicleKinds)
                    {
                        html.Append("<th style='width: 120px;'>" + vehicleKind.TableValue + "</th>");
                    }
                }
                html.Append("</tr>");

                html.Append("</thead>");

                int counter = 0;
                int cellValue = 0;

                foreach (var block in reportResult.PositionDeliveryResult.GroupBy(x => new { x.MuniciplaityName, x.DistrictName }))
                {
                    counter++;

                    html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                    <td style='" + cellStyle + @" text-align: left;'>" + block.Key.MuniciplaityName + @"</td>
                                    <td style='" + cellStyle + @" text-align: left;'>" + block.Key.DistrictName + @"</td>
                                 ");

                    foreach (var readiness in readinessTypes)
                    {
                        foreach (var technicsType in technicsTypes)
                        {
                            if (technicsType.TypeKey == "VEHICLES")
                            {
                                foreach (var vehicleKind in vehicleKinds)
                                {
                                    var vehichleKindRec = block.Where(x => x.VehicleKindID == vehicleKind.TableKey).SingleOrDefault();
                                    cellValue = (vehichleKindRec == null ? 0 : (readiness == 1 ? vehichleKindRec.Fulfiled : vehichleKindRec.Reserve));
                                    html.Append("<td style='" + cellStyle + @" text-align: right;'>" + cellValue.ToString() + @"</td>");
                                }
                            }
                            else
                            {
                                var technicsTypeRec = block.Where(x => x.TechnicsTypeID == technicsType.TechnicsTypeId).SingleOrDefault();
                                cellValue = (technicsTypeRec == null ? 0 : (readiness == 1 ? technicsTypeRec.Fulfiled : technicsTypeRec.Reserve));
                                html.Append("<td style='" + cellStyle + @" text-align: right;'>" + cellValue.ToString() + @"</td>");
                            }
                        }
                        cellValue = block.Sum(x => (readiness == 1 ? x.Fulfiled : x.Reserve));
                        html.Append("<td style='" + cellStyle + @" text-align: right;'>" + cellValue.ToString() + @"</td>");
                    }

                    html.Append(@"</tr>");
                }

                html.Append(@"</table>
                          </td></tr>");
            }

            
            html.Append("</table>");

            return html.ToString();
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", @"attachment; filename=""PrintReportAnalyzeTechCommand.xls""");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            ReportAnalyzeTechCommandFilter filter = new ReportAnalyzeTechCommandFilter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                MilitaryCommandId = int.Parse(militaryCommandId),
                MilitaryReadinessID = int.Parse(militaryReadinessID),
                MilitaryCommandSuffix = militaryCommandSuffix,
            };

            ReportAnalyzeTechCommandResult reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportAnalyzeTechCommandResult)Session[sessionResultsKey];
            else
                reportResult = ReportAnalyzeTechCommandUtil.GetReportAnalyzeTechCommand(filter, CurrentUser);


            string militaryDepartmentName = "";
            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByIDs(CurrentUser, militaryDepartmentId);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                militaryDepartmentName += (militaryDepartmentName == "" ? "" : ", ") + militaryDepartment.MilitaryDepartmentName;
            }

            if (militaryDepartmentName == "")
            {
                militaryDepartmentName = All;
            }

            string militaryReadinessName = All;
            MilitaryReadiness militaryReadinesses = MilitaryReadinessUtil.GetMilitaryReadiness(filter.MilitaryReadinessID, CurrentUser);

            if (militaryReadinesses != null)
                militaryReadinessName = militaryReadinesses.MilReadinessName;

            string militaryCommandName = All;
            MilitaryCommand militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(filter.MilitaryCommandId, CurrentUser);
            if (militaryCommand != null)
                militaryCommandName = militaryCommand.DisplayTextForSelection;

            string militarySubCommand = All;
            if (militaryCommand != null && !string.IsNullOrEmpty(militaryCommandSuffix) && militaryCommandSuffix != "-1")
                militarySubCommand = militaryCommand.CommandNumber + " " + militaryCommandSuffix + " " + militaryCommand.ShortName;

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
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
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

            if (reportResult.OverallResult != null)
            {
                html.Append(@"
                    <Worksheet ss:Name=""ОБЩ БРОЙ"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""250"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение-анализ за комплектуване на команда с техника-запас</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""4"" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">Заявка от формированието</Data></Cell>
                        <Cell ss:MergeAcross=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">Фактическо изпълнение</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">процент резерв с МН/ед.т./</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Вид и тип на техниката</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Брой</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Брой</Data></Cell>                             
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">От тях по заменки бр.</Data></Cell>                             
                    </Row>
                    ");

                foreach (ReportOverallAnalyzeTechCommandResultBlock block in reportResult.OverallResult)
                {
                    html.Append(@"
                            <Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""C1""><Data ss:Type=""String"">" + block.NormativeName.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestedPos.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FilledPos.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReplacedPos.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReservePos.ToString() + @"</Data></Cell>
                        </Row>");
                }


                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }


            if (reportResult.PositionDeliveryResult != null && reportResult.PositionDeliveryResult.Count > 0)
            {
                var technicsTypes = TechnicsTypeUtil.GetAllTechnicsTypes(CurrentUser);
                var vehicleKinds = GTableItemUtil.GetAllGTableItemsByTableName("VehicleKind", ModuleKey, 1, 0, 0, CurrentUser).OrderBy(x => x.TableSeq).ToList();
                var readinessTypeColspan = technicsTypes.Count + vehicleKinds.Count - 1;
                var readinessTypes = new int[] { 1, 2 };

                html.Append(@"
                    <Worksheet ss:Name=""Анализ по доставяне"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""180"" />
                    <Column ss:StyleID=""N"" ss:Width=""180"" />");

                var numberOfValueColumns = readinessTypes.Count() * (readinessTypeColspan + 1);
                for (int valueColumnCounter = 1; valueColumnCounter <= numberOfValueColumns; valueColumnCounter++)
                    html.Append(@"<Column ss:StyleID=""N"" ss:Width=""70"" />");
                    
                html.Append(@"
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение - анализ за комплектуването на команда</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H5""><Data ss:Type=""String"">Мобилизационните ресурсите се доставят от:</Data></Cell>
                    </Row>                
                    <Row ss:Height=""25.5"">                      
                        <Cell ss:StyleID=""H7"" ss:MergeDown=""2""><Data ss:Type=""String"">Община</Data></Cell>
                        <Cell ss:StyleID=""H7"" ss:MergeDown=""2""><Data ss:Type=""String"">Район</Data></Cell>
                        <Cell ss:StyleID=""H7"" ss:MergeAcross=""" + readinessTypeColspan + @"""><Data ss:Type=""String"">Основно попълнение</Data></Cell>
                        <Cell ss:StyleID=""H7"" ss:MergeAcross=""" + readinessTypeColspan + @"""><Data ss:Type=""String"">Резерв</Data></Cell>                  
                    </Row>
                    <Row ss:Height=""25.5"">   ");

                foreach (var readiness in readinessTypes)
                {
                    foreach (var technicsType in technicsTypes)
                    {
                        bool isFirstTechnicsTypeInFirstReadiness = readinessTypes.First() == readiness && technicsTypes.First() == technicsType;

                        if (technicsType.TypeKey == "VEHICLES")
                            html.Append(@"<Cell " + (isFirstTechnicsTypeInFirstReadiness ? @"ss:Index=""3""" : "") + @" ss:StyleID=""H7"" ss:MergeAcross=""" + (vehicleKinds.Count - 1) + @"""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
");
                        else
                            html.Append(@"<Cell " + (isFirstTechnicsTypeInFirstReadiness ? @"ss:Index=""3""" : "") + @" ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
");
                    }
                    html.Append(@"<Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">Всичко</Data></Cell>
");
                }

                html.Append(@"</Row>
                             <Row ss:Height=""25.5"">
");

                foreach (var readiness in readinessTypes)
                {
                    foreach (var vehicleKind in vehicleKinds)
                    {
                        int columnIndex = 2 + (readiness == 2 ? technicsTypes.Count + vehicleKinds.Count : 0) + 1 + technicsTypes.FindIndex(a => a.TypeKey == "VEHICLES");
                        bool isFirstVehicleKind = vehicleKinds.First() == vehicleKind;

                        html.Append(@"<Cell " + (isFirstVehicleKind ? @"ss:Index=""" + columnIndex + @"""" : "") + @" ss:StyleID=""H7""><Data ss:Type=""String"">" + vehicleKind.TableValue + @"</Data></Cell>
");
                    }
                }


                html.Append(@"</Row>
");

                int cellValue = 0;

                foreach (var block in reportResult.PositionDeliveryResult.GroupBy(x => new { x.MuniciplaityName, x.DistrictName }))
                {
                    html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                            <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.Key.MuniciplaityName + @"</Data></Cell>
                            <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.Key.DistrictName + @"</Data></Cell>
");

                    foreach (var readiness in readinessTypes)
                    {
                        foreach (var technicsType in technicsTypes)
                        {
                            if (technicsType.TypeKey == "VEHICLES")
                            {
                                foreach (var vehicleKind in vehicleKinds)
                                {
                                    var vehichleKindRec = block.Where(x => x.VehicleKindID == vehicleKind.TableKey).SingleOrDefault();
                                    cellValue = (vehichleKindRec == null ? 0 : (readiness == 1 ? vehichleKindRec.Fulfiled : vehichleKindRec.Reserve));
                                    html.Append(@"<Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + cellValue.ToString() + @"</Data></Cell>
");
                                }
                            }
                            else
                            {
                                var technicsTypeRec = block.Where(x => x.TechnicsTypeID == technicsType.TechnicsTypeId).SingleOrDefault();
                                cellValue = (technicsTypeRec == null ? 0 : (readiness == 1 ? technicsTypeRec.Fulfiled : technicsTypeRec.Reserve));
                                html.Append(@"<Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + cellValue.ToString() + @"</Data></Cell>
");
                            }
                        }
                        cellValue = block.Sum(x => (readiness == 1 ? x.Fulfiled : x.Reserve));
                        html.Append(@"<Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + cellValue.ToString() + @"</Data></Cell>
");
                    }


                    html.Append(@"</Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }


            html.Append("</Workbook>");

            return html.ToString();
        }
    }
}
