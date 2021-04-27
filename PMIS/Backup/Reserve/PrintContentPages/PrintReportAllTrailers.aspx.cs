﻿using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportAllTrailers : RESPage
    {
        const string All = "Всички";

        string technicsTypeKey = "";
        string regNumber = "";
        string inventoryNumber = "";
        string technicsCategoryId = "";
        string trailerKindId = "";
        string trailerTypeId = "";
        string militaryReportStatusId = "";
        string militaryDepartmentId = "";
        string ownershipNumber = "";
        string ownershipName = "";
        bool isOwnershipAddress = true;
        string postCode = "";
        string regionId = "";
        string municipalityId = "";
        string cityId = "";
        string districtId = "";
        string address = "";
        string normativeTechnicsId = "";
        string appointmentIsDelivered = "";
        string readiness = "";

        TechnicsType technicsType = null;

        int sortBy = 1; // 1 - Default

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
                this.GetUIItemAccessLevel("RES_REPORTS_TRAILERS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["TechnicsTypeKey"]))
                {
                    technicsTypeKey = Request.Params["TechnicsTypeKey"];

                    technicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeKey, CurrentUser);
                }

                lblHeaderTitle.InnerHtml = "Списък на техниката на военен отчет - " + (technicsType != null ? technicsType.TypeName : "");
                this.Title = lblHeaderTitle.InnerHtml;

                if (!String.IsNullOrEmpty(Request.Params["RegNumber"]))
                {
                    regNumber = Request.Params["RegNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["InventoryNumber"]))
                {
                    inventoryNumber = Request.Params["InventoryNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]))
                {
                    technicsCategoryId = Request.Params["TechnicsCategoryId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["TrailerKindId"]))
                {
                    trailerKindId = Request.Params["TrailerKindId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["TrailerTypeId"]))
                {
                    trailerTypeId = Request.Params["TrailerTypeId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryReportStatusId"]))
                {
                    militaryReportStatusId = Request.Params["MilitaryReportStatusId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["OwnershipNumber"]))
                {
                    ownershipNumber = Request.Params["OwnershipNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["OwnershipName"]))
                {
                    ownershipName = Request.Params["OwnershipName"];
                }

                if (!String.IsNullOrEmpty(Request.Params["IsOwnershipAddress"]))
                {
                    isOwnershipAddress = Request.Params["IsOwnershipAddress"] == "1";
                }

                if (!String.IsNullOrEmpty(Request.Params["PostCode"]))
                {
                    postCode = Request.Params["PostCode"];
                }

                if (!String.IsNullOrEmpty(Request.Params["RegionId"]))
                {
                    regionId = Request.Params["RegionId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MunicipalityId"]))
                {
                    municipalityId = Request.Params["MunicipalityId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["CityId"]))
                {
                    cityId = Request.Params["CityId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["DistrictId"]))
                {
                    districtId = Request.Params["DistrictId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["Address"]))
                {
                    address = Request.Params["Address"];
                }

                if (!String.IsNullOrEmpty(Request.Params["NormativeTechnicsId"]))
                {
                    normativeTechnicsId = Request.Params["NormativeTechnicsId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["AppointmentIsDelivered"]))
                {
                    appointmentIsDelivered = Request.Params["AppointmentIsDelivered"];
                }

                if (!String.IsNullOrEmpty(Request.Params["Readiness"]))
                {
                    readiness = Request.Params["Readiness"];
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
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
            bool isNormativeTechnicsHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool isRegNumberHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_REGNUMBER") == UIAccessLevel.Hidden;
            bool isInventoryNumberHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_INVENTORYNUMBER") == UIAccessLevel.Hidden;
            bool isTechnicsCategoryHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_TECHNICSCATEGORY") == UIAccessLevel.Hidden;
            bool isTrailerKindHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_TRAILERKIND") == UIAccessLevel.Hidden;
            bool isBodyTypeHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_BODYTYPE") == UIAccessLevel.Hidden;
            bool isTrailerTypeHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_TRAILERTYPE") == UIAccessLevel.Hidden;
            bool isMilDepartmentHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_MILDEPARTMENT") == UIAccessLevel.Hidden;
            bool isMilRepStatusHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_MILREPSTATUS") == UIAccessLevel.Hidden;
            bool isOwnershipHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_OWNERSHIP") == UIAccessLevel.Hidden;
            bool isAddressHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_ADDRESS") == UIAccessLevel.Hidden;


            ReportTrailerManageFilter filter = new ReportTrailerManageFilter()
            {
                RegNumber = regNumber,
                InventoryNumber = inventoryNumber,
                TechnicsCategoryId = technicsCategoryId,
                TrailerKindId = trailerKindId,
                TrailerTypeId = trailerTypeId,
                MilitaryReportStatus = militaryReportStatusId,
                MilitaryDepartment = militaryDepartmentId,
                OwnershipNumber = ownershipNumber,
                OwnershipName = ownershipName,
                IsOwnershipAddress = isOwnershipAddress,
                PostCode = postCode,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                Address = address,
                NormativeTechnics = normativeTechnicsId,
                AppointmentIsDelivered = appointmentIsDelivered,
                Readiness = readiness,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportTrailerManageBlock> reportTrailerManageBlocks = ReportTrailerUtil.GetAllReportTrailerManageBlocks(filter, 0, CurrentUser);

            TechnicsCategory technicsCategory = null;
            int techCategoryId = 0;
            if (int.TryParse(technicsCategoryId, out techCategoryId))
            {
                technicsCategory = TechnicsCategoryUtil.GetTechnicsCategory(techCategoryId, CurrentUser);   
            }

            GTableItem trailerKind = null;
            int trailKindId = 0;
            if (int.TryParse(trailerKindId, out trailKindId))
            {
                trailerKind = GTableItemUtil.GetTableItem("TrailerKind", trailKindId, ModuleUtil.RES(), CurrentUser);
            }

            GTableItem trailerType = null;
            int trailTypeId = 0;
            if (int.TryParse(trailerTypeId, out trailTypeId))
            {
                trailerType = GTableItemUtil.GetTableItem("TrailerType", trailTypeId, ModuleUtil.RES(), CurrentUser);
            }

            TechMilitaryReportStatus techMilitaryReportStatus = null;
            int techMilitaryReportStatusId = 0;
            if (int.TryParse(militaryReportStatusId, out techMilitaryReportStatusId))
            {
                techMilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatus(techMilitaryReportStatusId, CurrentUser);
            }

            MilitaryDepartment milDepartment = null;
            int milDepartmentId = 0;
            if (int.TryParse(militaryDepartmentId, out milDepartmentId))
            {
                milDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(milDepartmentId, CurrentUser);
            }

            Region region = null;
            if (!String.IsNullOrEmpty(regionId))
            {
                region = RegionUtil.GetRegion(int.Parse(regionId), CurrentUser);
            }

            Municipality municipality = null;
            if (!String.IsNullOrEmpty(municipalityId))
            {
                municipality = MunicipalityUtil.GetMunicipality(int.Parse(municipalityId), CurrentUser);
            }

            City city = null;
            if (!String.IsNullOrEmpty(cityId))
            {
                city = CityUtil.GetCity(int.Parse(cityId), CurrentUser);
            }

            District district = null;
            if (!String.IsNullOrEmpty(districtId))
            {
                district = DistrictUtil.GetDistrict(int.Parse(districtId), CurrentUser);
            }

            NormativeTechnics normativeTechnics = null;
            if (!String.IsNullOrEmpty(normativeTechnicsId))
            {
                normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsObj(CurrentUser, int.Parse(normativeTechnicsId));
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px; width: 1024px;'>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Регистрационен номер:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 205px;'>
                                   <span class='ValueLabel'>" + regNumber + @"</span>&nbsp;&nbsp;
                                </td>
                                <td align='right' style='width: 150px;'>
                                    <span class='Label'>Инвентарен номер:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 205px;'>
                                    <span class='ValueLabel'>" + inventoryNumber + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Категория:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 205px;'>
                                    <span class='ValueLabel'>" + (technicsCategory != null ? technicsCategory.CategoryName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 150px;'>
                                    <span class='Label'>Вид:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 205px;'>
                                    <span class='ValueLabel'>" + (trailerKind != null ? trailerKind.TableValue : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Тип:&nbsp;</span>
                                </td>
                                <td colspan='3' align='left'>
                                    <span class='ValueLabel'>" + (trailerType != null ? trailerType.TableValue : All) + @"</span>
                                </td>
                             </tr>
                              <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Състояние по отчета:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 145px;'>
                                    <span class='ValueLabel'>" + (techMilitaryReportStatus != null ? techMilitaryReportStatus.TechMilitaryReportStatusName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>На отчет в:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                    <span class='ValueLabel'>" + (milDepartment != null ? milDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Нормативна категория:&nbsp;</span>
                                </td>
                                <td align='left' colspan='3'>
                                    <span class='ValueLabel'>" + (normativeTechnics != null ? normativeTechnics.CodeAndText : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right'>
                                    <span class='Label'>Връчено МН:&nbsp;</span>
                                </td>
                                <td align='left'>
                                    <span class='ValueLabel'>" + (appointmentIsDelivered != "" ? (appointmentIsDelivered == ListItems.GetOptionYes().Value ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) : All) + @"</span>
                                </td>
                                <td align='right'>
                                    <span class='Label'>Вид резерв:&nbsp;</span>
                                </td>
                                <td align='left'>
                                    <span class='ValueLabel'>" + (readiness != "" ? ReadinessUtil.ReadinessName(int.Parse(readiness)) : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Собственик ЕГН/" + CommonFunctions.GetLabelText("UnifiedIdentityCode") + @":&nbsp;</span>
                                </td>
                                <td align='left' style='width: 145px;'>
                                    <span class='ValueLabel'>" + ownershipNumber + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>Трите имена/Име на фирмата:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                    <span class='ValueLabel'>" + ownershipName + @"</span>
                                </td>
                             </tr><tr>
                                <td colspan='4' align='left'>
                                    &nbsp;
                                </td>
                             </tr>
                             <tr>
                                <td colspan='4' align='left'>
                                    <span class='Label'>" + (isOwnershipAddress ? "Адрес на собственик" : "Адрес по местодомуване") + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Пощенски код:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                    <span class='ValueLabel'>" + postCode + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>Област:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                    <span class='ValueLabel'>" + (region != null ? region.RegionName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Населено място:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                    <span class='ValueLabel'>" + (city != null ? city.CityName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>Община:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                    <span class='ValueLabel'>" + (municipality != null ? municipality.MunicipalityName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Адрес:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                    <span class='ValueLabel'>" + address + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>Район:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                    <span class='ValueLabel'>" + (district != null ? district.DistrictName : All) + @"</span>
                                </td>
                             </tr>");

            if (reportTrailerManageBlocks.Count() > 0)
            {
                html.Append(@"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 15px; border-left: 1px solid #000000;'>№</th>" +
            (!isNormativeTechnicsHidden ? "<th style='width: 80px;'>Нормативна категория</th>" : "") +
            (!isRegNumberHidden ? "<th style='width: 130px;'>Регистрационен номер</th>" : "") +
            (!isInventoryNumberHidden ? "<th style='width: 120px;'>Инвентарен номер</th>" : "") +
            (!isTechnicsCategoryHidden ? "<th style='width: 150px;'>Категория</th>" : "") +
            (!isTrailerKindHidden ? "<th style='width: 150px;'>Вид</th>" : "") +
            (!isBodyTypeHidden ? "<th style='width: 150px;'>Каросерия</th>" : "") +
            (!isTrailerTypeHidden ? "<th style='width: 150px;'>Тип</th>" : "") +
            (!isMilDepartmentHidden ? "<th style='width: 120px;'>На отчет в</th>" : "") +
            (!isMilRepStatusHidden ? "<th style='width: 120px;'>Състояние по отчета</th>" : "") +
            (!isOwnershipHidden ? "<th style='width: 120px;'>Собственик</th>" : "") +
            (!isAddressHidden ? "<th style='width: 120px; border-right: 1px solid #000000;'>Адрес</th>" : "") + @"
            
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            foreach (ReportTrailerManageBlock reportTrailerManageBlock in reportTrailerManageBlocks)
            {
                html.Append(@"
                          <tr>
                            <td align='center'>" + counter + @"</td>" +
            (!isNormativeTechnicsHidden ? "<td align='left'>" + reportTrailerManageBlock.NormativeTechnicsCode + @"</td>" : "") +
            (!isRegNumberHidden ? "<td align='left'>" + reportTrailerManageBlock.RegNumber + @"</td>" : "") +
            (!isInventoryNumberHidden ? "<td align='left'>" + reportTrailerManageBlock.InventoryNumber + @"</td>" : "") +
            (!isTechnicsCategoryHidden ? "<td align='left'>" + reportTrailerManageBlock.TechnicsCategory + @"</td>" : "") +
            (!isTrailerKindHidden ? "<td align='left'>" + reportTrailerManageBlock.TrailerKind + @"</td>" : "") +
            (!isBodyTypeHidden ? "<td align='left'>" + reportTrailerManageBlock.BodyType + @"</td>" : "") +
            (!isTrailerTypeHidden ? "<td align='left'>" + reportTrailerManageBlock.TrailerType + @"</td>" : "") +
            (!isMilDepartmentHidden ? "<td align='left'>" + reportTrailerManageBlock.MilitaryDepartment + @"</td>" : "") +
            (!isMilRepStatusHidden ? "<td align='left'>" + reportTrailerManageBlock.MilitaryReportStatus + @"</td>" : "") +
            (!isOwnershipHidden ? "<td align='left'>" + reportTrailerManageBlock.Ownership + @"</td>" : "") +
            (!isAddressHidden ? "<td align='left'>" + reportTrailerManageBlock.Address + @"</td>" : "") + @"
                          </tr>");
                counter++;
            }

            if (reportTrailerManageBlocks.Count() > 0)
            {
                html.Append("</tbody></table></td></tr>");
            }

            html.Append("</table>");

            return html.ToString();
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportAllTrailers.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            bool isNormativeTechnicsHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool isRegNumberHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_REGNUMBER") == UIAccessLevel.Hidden;
            bool isInventoryNumberHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_INVENTORYNUMBER") == UIAccessLevel.Hidden;
            bool isTechnicsCategoryHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_TECHNICSCATEGORY") == UIAccessLevel.Hidden;
            bool isTrailerKindHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_TRAILERKIND") == UIAccessLevel.Hidden;
            bool isBodyTypeHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_BODYTYPE") == UIAccessLevel.Hidden;
            bool isTrailerTypeHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_TRAILERTYPE") == UIAccessLevel.Hidden;
            bool isMilDepartmentHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_MILDEPARTMENT") == UIAccessLevel.Hidden;
            bool isMilRepStatusHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_MILREPSTATUS") == UIAccessLevel.Hidden;
            bool isOwnershipHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_OWNERSHIP") == UIAccessLevel.Hidden;
            bool isAddressHidden = GetUIItemAccessLevel("RES_REPORTS_TRAILERS_ADDRESS") == UIAccessLevel.Hidden;

            int visibleColumnsCount = 1;
            if (!isNormativeTechnicsHidden)
                visibleColumnsCount++;
            if (!isRegNumberHidden)
                visibleColumnsCount++;
            if (!isInventoryNumberHidden)
                visibleColumnsCount++;
            if (!isTechnicsCategoryHidden)
                visibleColumnsCount++;
            if (!isTrailerKindHidden)
                visibleColumnsCount++;
            if (!isBodyTypeHidden)
                visibleColumnsCount++;
            if (!isTrailerTypeHidden)
                visibleColumnsCount++;
            if (!isMilDepartmentHidden)
                visibleColumnsCount++;
            if (!isMilRepStatusHidden)
                visibleColumnsCount++;
            if (!isOwnershipHidden)
                visibleColumnsCount++;
            if (!isAddressHidden)
                visibleColumnsCount++;

            ReportTrailerManageFilter filter = new ReportTrailerManageFilter()
            {
                RegNumber = regNumber,
                InventoryNumber = inventoryNumber,
                TechnicsCategoryId = technicsCategoryId,
                TrailerKindId = trailerKindId,
                TrailerTypeId = trailerTypeId,
                MilitaryReportStatus = militaryReportStatusId,
                MilitaryDepartment = militaryDepartmentId,
                OwnershipNumber = ownershipNumber,
                OwnershipName = ownershipName,
                IsOwnershipAddress = isOwnershipAddress,
                PostCode = postCode,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                Address = address,
                NormativeTechnics = normativeTechnicsId,
                AppointmentIsDelivered = appointmentIsDelivered,
                Readiness = readiness,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportTrailerManageBlock> reportTrailerManageBlocks = ReportTrailerUtil.GetAllReportTrailerManageBlocks(filter, 0, CurrentUser);

            TechnicsCategory technicsCategory = null;
            int techCategoryId = 0;
            if (int.TryParse(technicsCategoryId, out techCategoryId))
            {
                technicsCategory = TechnicsCategoryUtil.GetTechnicsCategory(techCategoryId, CurrentUser);
            }

            GTableItem trailerKind = null;
            int trailKindId = 0;
            if (int.TryParse(trailerKindId, out trailKindId))
            {
                trailerKind = GTableItemUtil.GetTableItem("TrailerKind", trailKindId, ModuleUtil.RES(), CurrentUser);
            }

            GTableItem trailerType = null;
            int trailTypeId = 0;
            if (int.TryParse(trailerTypeId, out trailTypeId))
            {
                trailerType = GTableItemUtil.GetTableItem("TrailerType", trailTypeId, ModuleUtil.RES(), CurrentUser);
            }

            TechMilitaryReportStatus techMilitaryReportStatus = null;
            int techMilitaryReportStatusId = 0;
            if (int.TryParse(militaryReportStatusId, out techMilitaryReportStatusId))
            {
                techMilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatus(techMilitaryReportStatusId, CurrentUser);
            }

            MilitaryDepartment milDepartment = null;
            int milDepartmentId = 0;
            if (int.TryParse(militaryDepartmentId, out milDepartmentId))
            {
                milDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(milDepartmentId, CurrentUser);
            }

            Region region = null;
            if (!String.IsNullOrEmpty(regionId))
            {
                region = RegionUtil.GetRegion(int.Parse(regionId), CurrentUser);
            }

            Municipality municipality = null;
            if (!String.IsNullOrEmpty(municipalityId))
            {
                municipality = MunicipalityUtil.GetMunicipality(int.Parse(municipalityId), CurrentUser);
            }

            City city = null;
            if (!String.IsNullOrEmpty(cityId))
            {
                city = CityUtil.GetCity(int.Parse(cityId), CurrentUser);
            }

            District district = null;
            if (!String.IsNullOrEmpty(districtId))
            {
                district = DistrictUtil.GetDistrict(int.Parse(districtId), CurrentUser);
            }

            NormativeTechnics normativeTechnics = null;
            if (!String.IsNullOrEmpty(normativeTechnicsId))
            {
                normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsObj(CurrentUser, int.Parse(normativeTechnicsId));
            }

            int headerLeftColspan = visibleColumnsCount;

            if (visibleColumnsCount > 1)
                headerLeftColspan = visibleColumnsCount / 2;

            StringBuilder html = new StringBuilder();
            html.Append(@"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 2em;'>Отчет на ресурсите от резерва</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Списък на техниката на военен отчет - " + (technicsType != null ? technicsType.TypeName : "") + @"</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Регистрационен номер:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + regNumber + @"</span>
                                        </td>
                                        <td align='right' colspan='1'>
                                            <span style='font-weight: normal;'>Инвентарен номер:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + inventoryNumber + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Категория:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (technicsCategory != null ? technicsCategory.CategoryName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='1'>
                                            <span style='font-weight: normal;'>Вид:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (trailerKind != null ? trailerKind.TableValue : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Тип:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (trailerType != null ? trailerType.TableValue : All) + @"</span>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Състояние по отчета:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (techMilitaryReportStatus != null ? techMilitaryReportStatus.TechMilitaryReportStatusName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='1'>
                                            <span style='font-weight: normal;'>На отчет в:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (milDepartment != null ? milDepartment.MilitaryDepartmentName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Нормативна категория:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='6'>
                                            <span style='font-weight: bold;'>" + (normativeTechnics != null ? normativeTechnics.CodeAndText : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Връчено МН:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (appointmentIsDelivered != "" ? (appointmentIsDelivered == ListItems.GetOptionYes().Value ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='1'>
                                            <span style='font-weight: normal;'>Вид резерв:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (readiness != "" ? ReadinessUtil.ReadinessName(int.Parse(readiness)) : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Собственик ЕГН/" + CommonFunctions.GetLabelText("UnifiedIdentityCode") + @":&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + ownershipNumber + @"</span>
                                        </td>
                                        <td align='right' colspan='1'>
                                            <span style='font-weight: normal;'>Трите имена/Име на фирмата:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + ownershipName + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='" + visibleColumnsCount + @"'>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>" + (isOwnershipAddress ? "Адрес на собственик" : "Адрес по местодомуване") + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Пощенски код:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + postCode + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Област:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (region != null ? region.RegionName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Населено място:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (city != null ? city.CityName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Община:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (municipality != null ? municipality.MunicipalityName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Адрес:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + address + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Район:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (district != null ? district.DistrictName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='" + visibleColumnsCount + @"'>&nbsp;</td>
                                    </tr>
                                </table>");


            if (reportTrailerManageBlocks.Count() > 0)
            {
                html.Append(@"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>" +
                                (!isNormativeTechnicsHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Нормативна категория</th>" : "") +
                                (!isRegNumberHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Регистрационен номер</th>" : "") +
                                (!isInventoryNumberHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Инвентарен номер</th>" : "") +
                                (!isTechnicsCategoryHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Категория</th>" : "") +
                                (!isTrailerKindHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Вид</th>" : "") +
                                (!isBodyTypeHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Каросерия</th>" : "") +
                                (!isTrailerTypeHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Тип</th>" : "") +
                                (!isMilDepartmentHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>На отчет в</th>" : "") +
                                (!isMilRepStatusHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Състояние по отчета</th>" : "") +
                                (!isOwnershipHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Собственик</th>" : "") +
                                (!isAddressHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Адрес</th>" : "") +
                            @"</tr>
                        </thead><tbody>");

                int counter = 1;

                foreach (ReportTrailerManageBlock reportTrailerManageBlock in reportTrailerManageBlocks)
                {
                    html.Append(@"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>" +
                                (!isNormativeTechnicsHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.NormativeTechnicsCode + "</td>") : "") + 
                                (!isRegNumberHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.RegNumber + @"</td>") : "") + 
                                (!isInventoryNumberHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.InventoryNumber + @"</td>") : "") + 
                                (!isTechnicsCategoryHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.TechnicsCategory + @"</td>") : "") +
                                (!isTrailerKindHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.TrailerKind + @"</td>") : "") +
                                (!isBodyTypeHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.BodyType + @"</td>") : "") + 
                                (!isTrailerTypeHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.TrailerType + @"</td>") : "") + 
                                (!isMilDepartmentHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.MilitaryDepartment + "</td>") : "") +
                                (!isMilRepStatusHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.MilitaryReportStatus + "</td>") : "") +
                                (!isOwnershipHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.Ownership + "</td>") : "") +
                                (!isAddressHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportTrailerManageBlock.Address + "</td>") : "") + @"
                              </tr>");
                    counter++;
                }

                html.Append("</tbody></table>");
            }

            html.Append("</body></html>");

            return html.ToString();
        }
    }
}
