using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintAllPotencialApplicants : APPLPage
    {
        const string All = "Всички";

        int? militaryDepartmentId = null;
        string drivingLicense = "";
        string comment = "";
        string serviceType = "";
        string lastApperianceFrom = "";
        string lastApperianceTo = "";
        string idNumber = "";

        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "870";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentID"]) && Request.Params["MilitaryDepartmentID"] != "-1")
                {
                    int mDepartmentId = 0;
                    int.TryParse(Request.Params["MilitaryDepartmentID"], out mDepartmentId);
                    if (mDepartmentId > 0)
                    {
                        militaryDepartmentId = mDepartmentId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["DrivingLicense"]))
                {
                    drivingLicense = Request.Params["DrivingLicense"].ToString();
                }

                if (!String.IsNullOrEmpty(Request.Params["Comment"]))
                {
                    comment = Request.Params["Comment"].ToString();
                }

                if (!String.IsNullOrEmpty(Request.Params["ServiceType"]))
                {
                    serviceType = Request.Params["ServiceType"].ToString();
                }

                if (!String.IsNullOrEmpty(Request.Params["LastApperianceFrom"]))
                {
                    lastApperianceFrom = Request.Params["LastApperianceFrom"].ToString();
                }

                if (!String.IsNullOrEmpty(Request.Params["LastApperianceTo"]))
                {
                    lastApperianceTo = Request.Params["LastApperianceTo"].ToString();
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
                }

                if (!String.IsNullOrEmpty(Request.Params["IdentityNumber"]))
                {
                    idNumber = Request.Params["IdentityNumber"];
                }

                this.divResults.InnerHtml = GeneratePageContent(false);
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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllPotentialApplicantsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllPotentialApplicantsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllPotentialApplicantsHtml()
        {
            PotencialApplicantsFilter filter = new PotencialApplicantsFilter()
            {
                MilitaryDepartmentId = militaryDepartmentId,
                DrivingLicense = drivingLicense,
                Comment = comment,
                ServiceType = serviceType,
                LastApperianceFrom = (CommonFunctions.TryParseDate(lastApperianceFrom) ? CommonFunctions.ParseDate(lastApperianceFrom) : (DateTime?)null),
                LastApperianceTo = (CommonFunctions.TryParseDate(lastApperianceTo) ? CommonFunctions.ParseDate(lastApperianceTo) : (DateTime?)null),
                IdentityNumber = idNumber,
                OrderBy = sortBy,
                PageIdx = 0
            };
           
            string DrivingLicenseName = "";
            List<DrivingLicenseCategory> drivingLicenseNames = DrivingLicenseCategoryUtil.GetDrivingLicenseCategoryByCategoryId(drivingLicense, CurrentUser);
            foreach (DrivingLicenseCategory drivingLicenseName in drivingLicenseNames)
            {
                DrivingLicenseName += (DrivingLicenseName == "" ? "" : ", ") + drivingLicenseName.DrivingLicenseCategoryName;
            }
                      

            string Comment = comment;

            string LastApperianceFrom = (filter.LastApperianceFrom == null ? "" : CommonFunctions.FormatDate(filter.LastApperianceFrom));
            string LastApperianceTo = (filter.LastApperianceTo == null ? "" : CommonFunctions.FormatDate(filter.LastApperianceTo));


            string ServiceTypeName = "";
            List<ServiceType> serviceTypeNames = ServiceTypeUtil.GetServiceTypeNamesByServiceTypeIDs(serviceType, CurrentUser);
            foreach (ServiceType serviceTypeName in serviceTypeNames)
            {
                ServiceTypeName += (ServiceTypeName == "" ? "" : ", ") + serviceTypeName.ServiceTypeName;
            }
                      
          
            //Get the list of potential applicants according to the specified filters and order
            List<PotencialApplicant> potencialApplicants = PotencialApplicantUtil.GetAllPotencialApplicants(filter, 0, CurrentUser);

            MilitaryDepartment militaryDepartment = null;
            if (militaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)militaryDepartmentId, CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 290px;'>
                                    <span class='Label'>Място на регистрация:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 230px;'>
                                    <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 155px;'>
                                     <span class='Label'>Шофьорска книжка:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                    <span class='ValueLabel'>" + DrivingLicenseName + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Коментар:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + Comment + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Вид служба:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + ServiceTypeName + @"</span>
                                </td>
                             </tr>
                            <tr>
                                <td align='right'>
                                     <span class='Label'>Дата на последно явяване от:&nbsp;</span>
                                </td>
                                <td>
                                     <span class='ValueLabel'>" + LastApperianceFrom + @"</span>
                                     <span class='Label'>&nbsp;до:&nbsp;</span>
                                     <span class='ValueLabel'>" + LastApperianceTo + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>ЕГН:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + idNumber + @"</span>
                                </td>
                            </tr>

";

            if (potencialApplicants.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 130px;'>Трите имена</th>
                                <th style='width: 80px;'>ЕГН</th>
                                <th style='width: 140px;'>Място на регистрация</th>
                                <th style='width: 60px;'>Последна актуализация</th>
                                <th style='width: 60px;'>Последнo явяванe</th>
                                <th style='width: 60px;'>Шофьорска книжка</th>
                                <th style='width: 140px;'>Вид служба</th>
                                <th style='width: 60px; border-right: 1px solid #000000;'>Коментар</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (PotencialApplicant potencialApplicant in potencialApplicants)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + potencialApplicant.Person.FullName + @"</td>
                            <td align='left'>" + potencialApplicant.Person.IdentNumber + @"</td>
                            <td align='left'>" + potencialApplicant.MilitaryDepartment.MilitaryDepartmentName + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(potencialApplicant.Person.LastModifiedDate) + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(potencialApplicant.LastAppearance) + @"</td>
                            <td align='left'>" + potencialApplicant.Person.DrivingLicenseCategoriesString + @"</td>
                            <td align='left'>" + potencialApplicant.ServiceTypesString + @"</td>
                            <td align='left'>" + potencialApplicant.Comments + @"</td>
                          </tr>";
                counter++;
            }

            if (potencialApplicants.Count() > 0)
            {
                html += "</tbody></table></td></tr>";
            }

            html += "</table>";

            return html;
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=PotentialAppilcants.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllPotentialApplicantsForExport()
        {
            PotencialApplicantsFilter filter = new PotencialApplicantsFilter()
            {
                MilitaryDepartmentId = militaryDepartmentId,
                DrivingLicense = drivingLicense,
                Comment = comment,
                ServiceType = serviceType,
                LastApperianceFrom = (CommonFunctions.TryParseDate(lastApperianceFrom) ? CommonFunctions.ParseDate(lastApperianceFrom) : (DateTime?)null),
                LastApperianceTo = (CommonFunctions.TryParseDate(lastApperianceTo) ? CommonFunctions.ParseDate(lastApperianceTo) : (DateTime?)null),
                IdentityNumber = idNumber,
                OrderBy = sortBy,
                PageIdx = 0
            };

            string DrivingLicenseName = "";
            List<DrivingLicenseCategory> drivingLicenseNames = DrivingLicenseCategoryUtil.GetDrivingLicenseCategoryByCategoryId(drivingLicense, CurrentUser);
            foreach (DrivingLicenseCategory drivingLicenseName in drivingLicenseNames)
            {
                DrivingLicenseName += (DrivingLicenseName == "" ? "" : ", ") + drivingLicenseName.DrivingLicenseCategoryName;
            }
                 

            string Comment = comment;

            string LastApperianceFrom = (filter.LastApperianceFrom == null ? "" : CommonFunctions.FormatDate(filter.LastApperianceFrom));
            string LastApperianceTo = (filter.LastApperianceTo == null ? "" : CommonFunctions.FormatDate(filter.LastApperianceTo));


            string ServiceTypeName = "";
            List<ServiceType> serviceTypeNames = ServiceTypeUtil.GetServiceTypeNamesByServiceTypeIDs(serviceType, CurrentUser);
            foreach (ServiceType serviceTypeName in serviceTypeNames)
            {
                ServiceTypeName += (ServiceTypeName == "" ? "" : ", ") + serviceTypeName.ServiceTypeName;
            }
                       

            //Get the list of potencial applicants according to the specified filters and order
            List<PotencialApplicant> potencialApplicants = PotencialApplicantUtil.GetAllPotencialApplicants(filter, 0, CurrentUser);

            MilitaryDepartment militaryDepartment = null;
            if (militaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)militaryDepartmentId, CurrentUser);
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='9' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='9' style='font-weight: bold; font-size: 2em;'>Кандидати</td></tr>
                                    <tr><td colspan='9'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='9' style='font-weight: bold; font-size: 1.3em;'>Списък на потенциалните кандидати</td></tr>
                                    <tr><td colspan='9'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right'  colspan='4'>
                                            <span>Място на регистрация:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;' colspan='2'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                        </td>
                                        <td align='right' style='width: 125px;'>
                                             <span >Шофьорска книжка:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 125px;' colspan='2'>
                                            <span style='font-weight: bold;' >" + DrivingLicenseName + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='4' style='vertical-align: top;'>
                                            <span>Коментар:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + Comment + @"</span>
                                        </td>
                                        <td align='right' style='vertical-align: top;'>
                                            <span>Вид служба:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 125px;' colspan='2'>
                                            <span style='font-weight: bold;'>" + ServiceTypeName + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='4'>
                                             <span >Дата на последно явяване от:&nbsp;</span>
                                        </td>
                                        <td colspan='2'>
                                             <span style='font-weight: bold;'>" + LastApperianceFrom + @"</span>
                                             <span >&nbsp;до:&nbsp;</span>
                                             <span style='font-weight: bold;'>" + LastApperianceTo + @"</span>
                                        </td>
                                        <td align='right' style='vertical-align: top;'>
                                            <span>ЕГН:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 125px;' colspan='2'>
                                            <span style='font-weight: bold;'>" + idNumber + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                        <td style='width: 250px;'>&nbsp;</td>
                                        <td style='width: 80px;'>&nbsp;</td>
                                        <td style='width: 250px;'>&nbsp;</td>
                                        <td style='width: 100px;'>&nbsp;</td>
                                        <td style='width: 100px;'>&nbsp;</td>
                                        <td style='width: 100px;'>&nbsp;</td>
                                        <td style='width: 100px;'>&nbsp;</td>
                                        <td style='width: 100px;'>&nbsp;</td>
                                    </tr>
                                </table>";


            if (potencialApplicants.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Трите имена</th>
                                <th style='width: 80px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>
                                <th style='width: 250px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Място на регистрация</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Последна актуализация</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Последно явяване</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Шофьорска книжка</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Вид служба</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Коментар</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (PotencialApplicant potencialApplicant in potencialApplicants)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + potencialApplicant.Person.FullName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + potencialApplicant.Person.IdentNumber + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + potencialApplicant.MilitaryDepartment.MilitaryDepartmentName + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(potencialApplicant.Person.LastModifiedDate) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(potencialApplicant.LastAppearance) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + potencialApplicant.Person.DrivingLicenseCategoriesString + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + potencialApplicant.ServiceTypesString + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + potencialApplicant.Comments + @"</td>
                              </tr>";
                    counter++;
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
