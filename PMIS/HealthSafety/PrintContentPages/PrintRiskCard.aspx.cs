using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintRiskCard : HSPage
    {
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");
        Position position = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnGenerateWord.Style.Add("display", "none");

            int positionId = 0;

            if (int.TryParse(Request.Params["positionID"], out positionId))
            {
                position = PositionUtil.GetPosition(positionId, CurrentUser);

                // Check visibility right for the print screen
                bool screenHidden = (this.GetUIItemAccessLevel("HS_RISKCARD") == UIAccessLevel.Hidden)
                                        || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden);


                if (position != null && !screenHidden)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td rowspan=\"2\">" + this.GenerateRiskCardHtml() + "</td>");
                    sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 180, false, true) + "</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, false, true) + "</td></tr>");
                    sb.Append("</table>");

                    this.divResults.InnerHtml = sb.ToString(); 
                }
                else
                {
                    this.divResults.InnerHtml = "";
                }
            }
        }

        // Generates html content related to contextual risk card
        private string GenerateRiskCardHtml()
        {
            List<RiskCardItem> riskCardItems = RiskCardItemUtil.GetAllRiskCardItemsByPosition(position.PositionId, CurrentUser);

            bool IsRiskCardHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD") == UIAccessLevel.Hidden;
            bool IsProbabilityHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_PROBABILITY") == UIAccessLevel.Hidden;
            bool IsExposureHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EXPOSURE") == UIAccessLevel.Hidden;
            bool IsEffectWeightHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EFFECTWEIGHT") == UIAccessLevel.Hidden;
            bool IsHazardHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_HAZARD") == UIAccessLevel.Hidden;

            int maxRiskValue = 0;
            string maxRiskRank = "";

            int columnNumber = 7;

            if (IsProbabilityHidden)
                columnNumber--;

            if (IsExposureHidden)
                columnNumber--;

            if (IsEffectWeightHidden)
                columnNumber--;

            if (IsHazardHidden)
                columnNumber--;

            List<RiskFactorType> riskFactorTypes = RiskFactorTypeUtil.GetAllRiskFactorTypes(CurrentUser);

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<table style='padding: 5px;'>
                             <tr>
                                <td align='left' style='width: 100%;'>
                                    <span class='Label'>" + this.MilitaryUnitLabel + @":&nbsp;</span>
                                   <span class='ValueLabel'>" + position.Subdivision.MilitaryUnit.DisplayTextForSelection + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='left' style='width: 100%;'>
                                    <span class='Label'>Подразделение/обект:&nbsp;</span>
                                   <span class='ValueLabel'>" + position.Subdivision.SubdivisionName + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='left' style='width: 100%;'>
                                    <span class='Label'>Длъжност:&nbsp;</span>
                                   <span class='ValueLabel'>" + position.PositionName + @"</span>
                                </td>
                             </tr>
<tr style='height: 17px;'></tr>");

            string headerStyle = "vertical-align: middle;";

            sb.Append(@"<tr><td align='center'>");

            sb.Append(@"<table class='CommonHeaderTable' style='text-align: center;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;" + headerStyle + @"'>№ по ред</th>
                               <th style='width: 200px;" + headerStyle + @"'>Вид опасност</th>

            " + (!IsProbabilityHidden ? @"<th style='width: 30px;" + headerStyle + @"'>Вероятност<br />(B)</th>" : "") + @"
            " + (!IsExposureHidden ? @"<th style='width: 30px;" + headerStyle + @"'>Експозиция<br />(E)</th>" : "") + @"
            " + (!IsEffectWeightHidden ? @"<th style='width: 30px;" + headerStyle + @"'>Тежест<br />(T)</th>" : "") + @"
            " + (!IsHazardHidden ? @"<th style='width: 30px;" + headerStyle + @"'>Риск<br />(B*E*T)</th>" : "") + @"

                               <th style='width: 30px;" + headerStyle + @"'>Степен</th>
                               <th style='width: 80px;" + headerStyle + @"'>Класификация</th>
                            </tr>
                         </thead>");

            int riskFactorTypeCounter = 1;

            foreach (RiskFactorType riskFactorType in riskFactorTypes)
            {
                var subRiskCardItems = riskCardItems.FindAll(a => a.RiskFactorTypeId == riskFactorType.RiskFactorTypeId).OrderBy(a => a.RiskFactorId).ThenBy(a => a.RiskFactorSeq).ThenBy(a => a.HazardSeq).ThenBy(a => a.RiskCardItemId);

                string cellStyle = "vertical-align: top;";

                sb.Append(@"<tr style='background-color: #F8F8F8;'>
                                <td style='" + cellStyle + @"  text-align: left; padding-left: 3px;'>" + riskFactorTypeCounter + @".</td>                                                 
                                <td style='" + cellStyle + @" text-align: left; font-size: 1.25em;' colspan='" + columnNumber + "'>" + CommonFunctions.HtmlEncoding(riskFactorType.RiskFactorTypeName) + @"</td>
                            </tr>");

                if (!IsRiskCardHidden)
                {
                    int riskFactorCounter = 0;
                    int riskFactorId = 0;
                    int riskCardItemCounter = 1;

                    foreach (RiskCardItem riskCardItem in subRiskCardItems)
                    {
                        if (maxRiskValue < riskCardItem.HazardValue)
                        {
                            maxRiskValue = riskCardItem.HazardValue;
                            maxRiskRank = riskCardItem.RiskRank;
                        }

                        if (riskFactorId != riskCardItem.RiskFactorId)
                        {
                            riskFactorCounter++;
                            riskCardItemCounter = 1;
                            riskFactorId = riskCardItem.RiskFactorId;

                            RiskFactor riskFactor = RiskFactorUtil.GetRiskFactor(riskCardItem.RiskFactorId, CurrentUser);

                            sb.Append(@"<tr style='background-color: #F8F8F8;'>
                                        <td style='" + cellStyle + @"  text-align: left; padding-left: 3px;'>" + riskFactorTypeCounter + "." + riskFactorCounter + @".</td>                                                 
                                        <td style='" + cellStyle + @" text-align: left; font-size: 1.0em;' colspan='" + columnNumber + "'>" + CommonFunctions.HtmlEncoding(riskFactor.RiskFactorName) + @"</td>
                                    </tr>");
                        }

                        sb.Append(@"<tr>
                                        <td style='" + cellStyle + @"'>" + riskFactorTypeCounter + "." + riskFactorCounter + "." + riskCardItemCounter + @".</td>");

                        if (riskCardItem.HazardId.HasValue)
                        {
                            sb.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + riskCardItem.HazardName + @"</td>");
                        }
                        else
                        {
                            sb.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + CommonFunctions.ReplaceNewLinesInString(riskCardItem.OtherHazard) + @"</td>");
                        }

                        sb.Append(@"                                    
                        " + (!IsProbabilityHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.ProbabilityFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsExposureHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.ExposureFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsEffectWeightHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.EffectWeightFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsHazardHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.HazardValue.ToString() + @"</td>" : "") + @"

                                    <td style='" + cellStyle + @"'>" + riskCardItem.RiskRank + @"</td>
                                    <td style='" + cellStyle + @" text-align: left;'>" + CommonFunctions.HtmlEncoding(riskCardItem.RiskRankName) + @"</td>
                                </tr>");

                        riskCardItemCounter++;
                    }
                }

                riskFactorTypeCounter++;
            }

            sb.Append(@"</table><br />");
            sb.Append(@"<span id='lblMaxHazardValue' class='Label' style='float: right; margin-right: 15px;'>Максимална степен на риска за длъжност " + position.PositionName + ": <b>" + maxRiskRank + "</b></span>");
            sb.Append(@"</td></tr></table><br />");
            sb.Append(@"<table style='width: 100%; margin-bottom: 15px;'>
                            <tr>
                                <td colspan='2' style='width: 50%; text-align: left; padding-left: 150px;'>
                                    <span class='ValueLabel'>Работна група</span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 50%; text-align: left;'>
                                    <span class='Label'>Отпечатано в 2 екз</span>
                                </td>
                                <td style='text-align: left;'>
                                    <span class='Label'>&nbsp;Председател: ........................./......................../</span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 50%; text-align: left;'>
                                    <span class='Label'>Екз. №1 : за под..................... – гр......................</span>
                                </td>
                                <td style='text-align: left;'>
                                    <span class='Label'>&nbsp;&nbsp;Членове: д-р ........................./......................../</span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 50%; text-align: left;'>
                                    <span class='Label'>Екз. №2 : за ЦВЕХ ОПМ – гр.........................</span>
                                </td>
                                <td style='text-align: left;'>
                                    <span class='Label'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ........................./......................../</span>
                                </td>
                            </tr>
                            <tr>
                                <td style='width: 50%; text-align: left;'>
                                    <span class='Label'>Отпечатал документа:................................</span>
                                </td>
                                <td style='text-align: left;'>
                                    <span class='Label'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ........................./......................../</span>
                                </td>
                            </tr>
                        </table>");

            return sb.ToString();
        }

        // Generates html content for Word export related to contextual risk card
        private string GenerateWordExport()
        {
            List<RiskCardItem> riskCardItems = RiskCardItemUtil.GetAllRiskCardItemsByPosition(position.PositionId, CurrentUser);

            bool IsRiskCardHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD") == UIAccessLevel.Hidden;
            bool IsProbabilityHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_PROBABILITY") == UIAccessLevel.Hidden;
            bool IsExposureHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EXPOSURE") == UIAccessLevel.Hidden;
            bool IsEffectWeightHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_EFFECTWEIGHT") == UIAccessLevel.Hidden;
            bool IsHazardHidden = this.GetUIItemAccessLevel("HS_RISKCARD_HAZARD_HAZARD") == UIAccessLevel.Hidden;

            int maxRiskValue = 0;
            string maxRiskRank = "";

            int columnNumber = 7;

            if (IsProbabilityHidden)
                columnNumber--;

            if (IsExposureHidden)
                columnNumber--;

            if (IsEffectWeightHidden)
                columnNumber--;

            if (IsHazardHidden)
                columnNumber--;

            List<RiskFactorType> riskFactorTypes = RiskFactorTypeUtil.GetAllRiskFactorTypes(CurrentUser);

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<html xmlns:v=""urn:schemas-microsoft-com:vml""
                        xmlns:o=""urn:schemas-microsoft-com:office:office""
                        xmlns:w=""urn:schemas-microsoft-com:office:word"" 
                        charset=""UTF-8"" >
                        <head>
   <style>
     @page Section1
	 {   
         size: 841.9pt 595.3pt;
	     margin: 0.98in 0.64in 0.98in 0.98in;
         mso-page-orientation:landscape;
     }

     div.Section1
	 {page:Section1;}

      body
      {
         font-family: ""Times New Roman"";
         font-size: 12pt;
      }

      p
      {
         margin-top: 0px;
         margin-bottom: 0px;
      }

      .Header1
      {
         font-size: 15pt;
         font-weight: bold;
         text-align: center;
         margin-top: 6pt;
         letter-spacing: 1.2pt;
      }

      .Header2
      {
         font-size: 15pt;
         font-weight: bold;
         text-align: center;
         margin-bottom: 6pt;
      }

.Label
{
	text-align: right;
	color: black;
	font-weight: normal;
}

.ValueLabel
{
	text-align: left;
	color: black;
	font-weight: bold;
}

.CommonHeaderTable
{
	border: 1px solid #000000; 
	border-collapse:collapse;
	vertical-align: middle;
}

.CommonHeaderTable thead
{
	vertical-align: middle;
    border-top: 2px solid #000000;
    border-bottom: 2px solid #000000;
}

.CommonHeaderTable thead tr
{
	border-left: solid 1px #000000;
}

.CommonHeaderTable tr td
{
	border: solid 1px #000000;
}

.CommonHeaderTable thead tr th
{
	border: solid 1px #000000;
    text-align: center;
}

   </style>   
                           <xml>
                              <w:WordDocument>
                                 <w:View>Print</w:View>
                                 <w:Zoom>100</w:Zoom>
                              </w:WordDocument>
                           </xml>
                        </head>
                        <body lang=BG>");

            sb.Append(@"<div class=""Section1"">

                                <p class='Header1'>Карта за оценка на риска за здравето</p>

                                <p class='Header2'>и безопасността на работещите</p>
                                <p style='margin-top: 16pt;'>
                                    <span class='Label'>" + this.MilitaryUnitLabel + @":&nbsp;</span>
                                    <span class='ValueLabel'>" + position.Subdivision.MilitaryUnit.DisplayTextForSelection + @"</span>
                                </p>
                                <p>
                                   <span class='Label'>Подразделение/обект:&nbsp;</span>
                                   <span class='ValueLabel'>" + position.Subdivision.SubdivisionName + @"</span>
                                </p>
                                <p style='margin-bottom: 16pt;'>
                                   <span class='Label'>Длъжност:&nbsp;</span>
                                   <span class='ValueLabel'>" + position.PositionName + @"</span>
                                </p>
                             ");

            string headerStyle = "vertical-align: middle;";

            sb.Append(@"<p style='margin-top: 16pt;'>");

            sb.Append(@"<table class='CommonHeaderTable' style='text-align: center;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;" + headerStyle + @"'>№ по ред</th>
                               <th style='width: 30px;" + headerStyle + @"'>Вид опасност</th>

            " + (!IsProbabilityHidden ? @"<th style='width: 30px;" + headerStyle + @"'>Вероятност<br />(B)</th>" : "") + @"
            " + (!IsExposureHidden ? @"<th style='width: 30px;" + headerStyle + @"'>Експозиция<br />(E)</th>" : "") + @"
            " + (!IsEffectWeightHidden ? @"<th style='width: 30px;" + headerStyle + @"'>Тежест<br />(T)</th>" : "") + @"
            " + (!IsHazardHidden ? @"<th style='width: 30px;" + headerStyle + @"'>Риск<br />(B*E*T)</th>" : "") + @"

                               <th style='width: 30px;" + headerStyle + @"'>Степен</th>
                               <th style='width: 80px;" + headerStyle + @"'>Класификация</th>
                            </tr>
                         </thead>");

            int riskFactorTypeCounter = 1;

            foreach (RiskFactorType riskFactorType in riskFactorTypes)
            {
                var subRiskCardItems = riskCardItems.FindAll(a => a.RiskFactorTypeId == riskFactorType.RiskFactorTypeId).OrderBy(a => a.RiskFactorId).ThenBy(a => a.RiskFactorSeq).ThenBy(a => a.HazardSeq).ThenBy(a => a.RiskCardItemId);

                string cellStyle = "vertical-align: top;";

                sb.Append(@"<tr>
                                <td style='" + cellStyle + @"  text-align: left; padding-left: 3px;'>" + riskFactorTypeCounter + @".</td>                                                 
                                <td style='" + cellStyle + @" text-align: left;' colspan='" + columnNumber + "'>" + CommonFunctions.HtmlEncoding(riskFactorType.RiskFactorTypeName) + @"</td>
                            </tr>");

                if (!IsRiskCardHidden)
                {
                    int riskFactorCounter = 0;
                    int riskFactorId = 0;
                    int riskCardItemCounter = 1;

                    foreach (RiskCardItem riskCardItem in subRiskCardItems)
                    {
                        if (maxRiskValue < riskCardItem.HazardValue)
                        {
                            maxRiskValue = riskCardItem.HazardValue;
                            maxRiskRank = riskCardItem.RiskRank;
                        }

                        if (riskFactorId != riskCardItem.RiskFactorId)
                        {
                            riskFactorCounter++;
                            riskCardItemCounter = 1;
                            riskFactorId = riskCardItem.RiskFactorId;

                            RiskFactor riskFactor = RiskFactorUtil.GetRiskFactor(riskCardItem.RiskFactorId, CurrentUser);

                            sb.Append(@"<tr>
                                        <td style='" + cellStyle + @"  text-align: left; padding-left: 3px;'>" + riskFactorTypeCounter + "." + riskFactorCounter + @".</td>                                                 
                                        <td style='" + cellStyle + @" text-align: left;' colspan='" + columnNumber + "'>" + CommonFunctions.HtmlEncoding(riskFactor.RiskFactorName) + @"</td>
                                    </tr>");
                        }

                        sb.Append(@"<tr>
                                        <td style='" + cellStyle + @"'>" + riskFactorTypeCounter + "." + riskFactorCounter + "." + riskCardItemCounter + @".</td>");

                        if (riskCardItem.HazardId.HasValue)
                        {
                            sb.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + riskCardItem.HazardName + @"</td>");
                        }
                        else
                        {
                            sb.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + CommonFunctions.ReplaceNewLinesInString(riskCardItem.OtherHazard) + @"</td>");
                        }

                        sb.Append(@"                                    
                        " + (!IsProbabilityHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.ProbabilityFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsExposureHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.ExposureFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsEffectWeightHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.EffectWeightFactor.ToString() + @"</td>" : "") + @"
                        " + (!IsHazardHidden ? @"<td style='" + cellStyle + @"'>" + riskCardItem.HazardValue.ToString() + @"</td>" : "") + @"

                                    <td style='" + cellStyle + @"'>" + riskCardItem.RiskRank + @"</td>
                                    <td style='" + cellStyle + @" text-align: left;'>" + CommonFunctions.HtmlEncoding(riskCardItem.RiskRankName) + @"</td>
                                </tr>");

                        riskCardItemCounter++;
                    }
                }

                riskFactorTypeCounter++;
            }

            sb.Append(@"</table><br />");
            sb.Append(@"</p>");
            sb.Append(@"<p style='text-align: right;'><span id='lblMaxHazardValue' class='Label' style='float: right; margin-right: 15px;'>Максимална степен на риска за длъжност " + position.PositionName + ": <b>" + maxRiskRank + "</b></span></p>");

            sb.Append(@"<p style='margin-top: 20px;'>
                            <span class='ValueLabel'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Работна група</span>
                        </p>
                        <p>
                            <span class='Label'>Отпечатано в 2 екз&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                            <span class='Label'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Председател: ........................./......................../</span>
                        </p>
                        <p>
                            <span class='Label'>Екз. №1 : за под..................... – гр......................</span>
                            <span class='Label'>&nbsp;&nbsp;Членове: д-р ........................./......................../</span>
                        </p>
                        <p>
                            <span class='Label'>Екз. №2 : за ЦВЕХ ОПМ – гр.........................</span>
                            <span class='Label'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ........................./......................../</span>
                        </p>
                        <p>
                            <span class='Label'>Отпечатал документа:........................................</span>
                            <span class='Label'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ........................./......................../</span>
                        </p>
                     
                     <div class=""Section1"">");

            sb.Append(@"</body></html>");

            return sb.ToString();
        }

        protected void btnGenerateWord_Click(object sender, EventArgs e)
        {
            string result = GenerateWordExport();

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=RiskCard.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }
    }
}
