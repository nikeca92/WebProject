using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportAllVehicles : RESPage
    {
        const string All = "Всички";

        string technicsTypeKey = "";
        string regNumber = "";
        string inventoryNumber = "";
        string technicsCategoryId = "";
        string vehicleKindId = "";

        //string vehicleMakeId = "";
        //string vehicleModelId = "";

        string vehicleMakeName = "";
        string vehicleModelName = "";

        string vehicleBodyTypeId = "";
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
                this.GetUIItemAccessLevel("RES_REPORTS_VEHICLES") != UIAccessLevel.Hidden)
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

                if (!String.IsNullOrEmpty(Request.Params["VehicleKindId"]))
                {
                    vehicleKindId = Request.Params["VehicleKindId"];
                }

                //if (!String.IsNullOrEmpty(Request.Params["VehicleMakeId"]))
                //{
                //    vehicleMakeId = Request.Params["VehicleMakeId"];
                //}

                //if (!String.IsNullOrEmpty(Request.Params["VehicleModelId"]))
                //{
                //    vehicleModelId = Request.Params["VehicleModelId"];
                //}

                if (!String.IsNullOrEmpty(Request.Params["VehicleMakeName"]))
                {
                    vehicleMakeName = Request.Params["VehicleMakeName"];
                }

                if (!String.IsNullOrEmpty(Request.Params["VehicleModelName"]))
                {
                    vehicleModelName = Request.Params["VehicleModelName"];
                }

                if (!String.IsNullOrEmpty(Request.Params["VehicleBodyTypeId"]))
                {
                    vehicleBodyTypeId = Request.Params["VehicleBodyTypeId"];
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
            bool isNormativeTechnicsHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool isRegNumberHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_REGNUMBER") == UIAccessLevel.Hidden;
            bool isInventoryNumberHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_INVENTORYNUMBER") == UIAccessLevel.Hidden;
            bool isTechnicsCategoryHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_TECHNICSCATEGORY") == UIAccessLevel.Hidden;
            bool isVehicleKindHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_VEHICLEKIND") == UIAccessLevel.Hidden;
            bool isBodyTypeHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_BODYTYPE") == UIAccessLevel.Hidden;
            bool isVehicleMakeHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_VEHICLEMAKE") == UIAccessLevel.Hidden;
            bool isVehicleModelHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_VEHICLEMODEL") == UIAccessLevel.Hidden;
            bool isMilDepartmentHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_MILDEPARTMENT") == UIAccessLevel.Hidden;
            bool isMilRepStatusHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_MILREPSTATUS") == UIAccessLevel.Hidden;
            bool isOwnershipHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_OWNERSHIP") == UIAccessLevel.Hidden;
            bool isAddressHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_ADDRESS") == UIAccessLevel.Hidden;

            ReportVehicleManageFilter filter = new ReportVehicleManageFilter()
            {
                RegNumber = regNumber,
                InventoryNumber = inventoryNumber,
                TechnicsCategoryId = technicsCategoryId,
                VehicleKindId = vehicleKindId,

                //VehicleMakeId = vehicleMakeId,
                //VehicleModelId = vehicleModelId,

                VehicleMakeName = vehicleMakeName,
                VehicleModelName = vehicleModelName,

                VehicleBodyTypeId = vehicleBodyTypeId,
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
            List<ReportVehicleManageBlock> reportVehicleManageBlocks = ReportVehicleUtil.GetAllReportVehicleManageBlocks(filter, 0, CurrentUser);

            TechnicsCategory technicsCategory = null;
            int techCategoryId = 0;
            if (int.TryParse(technicsCategoryId, out techCategoryId))
            {
                technicsCategory = TechnicsCategoryUtil.GetTechnicsCategory(techCategoryId, CurrentUser);   
            }

            GTableItem vehicleKind = null;
            int vehKindId = 0;
            if (int.TryParse(vehicleKindId, out vehKindId))
            {
                vehicleKind = GTableItemUtil.GetTableItem("VehicleKind", vehKindId, ModuleUtil.RES(), CurrentUser);
            }

            //VehicleMake vehicleMake = null;
            //int vehMakeId = 0;
            //if (int.TryParse(vehicleMakeId, out vehMakeId))
            //{
            //    vehicleMake = VehicleMakeUtil.GetVehicleMake(vehMakeId, CurrentUser);
            //}

            //VehicleModel vehicleModel = null;
            //int vehModelId = 0;
            //if (int.TryParse(vehicleModelId, out vehModelId))
            //{
            //    vehicleModel = VehicleModelUtil.GetVehicleModel(vehModelId, CurrentUser);
            //}

            GTableItem vehicleBodyType = null;
            int vehBodyTypeId = 0;
            if (int.TryParse(vehicleBodyTypeId, out vehBodyTypeId))
            {
                vehicleBodyType = GTableItemUtil.GetTableItem("VehicleBodyType", vehBodyTypeId, ModuleUtil.RES(), CurrentUser);
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
                                <td align='left' style='width: 145px;'>
                                   <span class='ValueLabel'>" + regNumber + @"</span>&nbsp;&nbsp;
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>Инвентарен номер:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                    <span class='ValueLabel'>" + inventoryNumber + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Категория:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 145px;'>
                                    <span class='ValueLabel'>" + (technicsCategory != null ? technicsCategory.CategoryName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>Вид:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                    <span class='ValueLabel'>" + (vehicleKind != null ? vehicleKind.TableValue : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Марка:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 145px;'>
                                    <span class='ValueLabel'>" + vehicleMakeName + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>Модел:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                    <span class='ValueLabel'>" + vehicleModelName + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Каросерия:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 145px;'>
                                    <span class='ValueLabel'>" + (vehicleBodyType != null ? vehicleBodyType.TableValue : All) + @"</span>
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

            if (reportVehicleManageBlocks.Count() > 0)
            {
                html.Append(@"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 15px; border-left: 1px solid #000000;'>№</th>" +
    (!isNormativeTechnicsHidden ? "<th style='width: 80px;'>Нормативна категория</th>" : "") +
            (!isRegNumberHidden ? "<th style='width: 120px;'>Регистрационен номер</th>" : "") +
            (!isInventoryNumberHidden ? "<th style='width: 100px;'>Инвентарен номер</th>" : "") +
            (!isTechnicsCategoryHidden ? "<th style='width: 120px;'>Категория</th>" : "") +
            (!isVehicleKindHidden ? "<th style='width: 120px;'>Вид</th>" : "") +
            (!isBodyTypeHidden    ? "<th style='width: 120px;'>Каросерия</th>" : "") +
            (!isVehicleMakeHidden ? "<th style='width: 120px;'>Марка</th>" : "") +
            (!isVehicleModelHidden ? "<th style='width: 120px;'>Модел</th>" : "") +
           (!isMilDepartmentHidden ? "<th style='width: 120px;'>На отчет в</th>" : "") +
            (!isMilRepStatusHidden ? "<th style='width: 120px;'>Състояние по отчета</th>" : "") +
               (!isOwnershipHidden ? "<th style='width: 120px;'>Собственик</th>" : "") +
               (!isAddressHidden   ? "<th style='width: 120px; border-right: 1px solid #000000;'>Адрес</th>" : "") + @"
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            foreach (ReportVehicleManageBlock reportVehicleManageBlock in reportVehicleManageBlocks)
            {
                html.Append(@"
                          <tr>
                            <td align='center'>" + counter + @"</td>" +
            (!isNormativeTechnicsHidden ? "<td align='left'>" + reportVehicleManageBlock.NormativeTechnicsCode + @"</td>" : "") +
            (!isRegNumberHidden ? "<td align='left'>" + reportVehicleManageBlock.RegNumber + @"</td>" : "") +
            (!isInventoryNumberHidden ? "<td align='left'>" + reportVehicleManageBlock.InventoryNumber + @"</td>" : "") +
            (!isTechnicsCategoryHidden ? "<td align='left'>" + reportVehicleManageBlock.TechnicsCategory + @"</td>" : "") +
            (!isVehicleKindHidden ? "<td align='left'>" + reportVehicleManageBlock.VehicleKind + @"</td>" : "") +
            (!isBodyTypeHidden ? "<td align='left'>" + reportVehicleManageBlock.BodyType + @"</td>" : "") +
            (!isVehicleMakeHidden ? "<td align='left'>" + reportVehicleManageBlock.VehicleMake + @"</td>" : "") +
            (!isVehicleModelHidden ? "<td align='left'>" + reportVehicleManageBlock.VehicleModel + @"</td>" : "") +
            (!isMilDepartmentHidden ? "<td align='left'>" + reportVehicleManageBlock.MilitaryDepartment + @"</td>" : "") +
            (!isMilRepStatusHidden ? "<td align='left'>" + reportVehicleManageBlock.MilitaryReportStatus + @"</td>" : "") +
            (!isOwnershipHidden ? "<td align='left'>" + reportVehicleManageBlock.Ownership + @"</td>" : "") +
            (!isAddressHidden ? "<td align='left'>" + reportVehicleManageBlock.Address + @"</td>" : "") + @"
                          </tr>");
                counter++;
            }

            if (reportVehicleManageBlocks.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportAllVehicles.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            bool isNormativeTechnicsHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool isRegNumberHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_REGNUMBER") == UIAccessLevel.Hidden;
            bool isInventoryNumberHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_INVENTORYNUMBER") == UIAccessLevel.Hidden;
            bool isTechnicsCategoryHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_TECHNICSCATEGORY") == UIAccessLevel.Hidden;
            bool isVehicleKindHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_VEHICLEKIND") == UIAccessLevel.Hidden;
            bool isBodyTypeHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_BODYTYPE") == UIAccessLevel.Hidden;
            bool isVehicleMakeHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_VEHICLEMAKE") == UIAccessLevel.Hidden;
            bool isVehicleModelHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_VEHICLEMODEL") == UIAccessLevel.Hidden;
            bool isMilDepartmentHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_MILDEPARTMENT") == UIAccessLevel.Hidden;
            bool isMilRepStatusHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_MILREPSTATUS") == UIAccessLevel.Hidden;
            bool isOwnershipHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_OWNERSHIP") == UIAccessLevel.Hidden;
            bool isAddressHidden = GetUIItemAccessLevel("RES_REPORTS_VEHICLES_ADDRESS") == UIAccessLevel.Hidden;

            int visibleColumnsCount = 1;
            if (!isNormativeTechnicsHidden)
                visibleColumnsCount++;
            if (!isRegNumberHidden)
                visibleColumnsCount++;
            if (!isInventoryNumberHidden)
                visibleColumnsCount++;
            if (!isTechnicsCategoryHidden)
                visibleColumnsCount++;
            if (!isVehicleKindHidden)
                visibleColumnsCount++;
            if (!isBodyTypeHidden)
                visibleColumnsCount++;
            if (!isVehicleMakeHidden)
                visibleColumnsCount++;
            if (!isVehicleModelHidden)
                visibleColumnsCount++;
            if (!isMilDepartmentHidden)
                visibleColumnsCount++;
            if (!isMilRepStatusHidden)
                visibleColumnsCount++;
            if (!isOwnershipHidden)
                visibleColumnsCount++;
            if (!isAddressHidden)
                visibleColumnsCount++;

            ReportVehicleManageFilter filter = new ReportVehicleManageFilter()
            {
                RegNumber = regNumber,
                InventoryNumber = inventoryNumber,
                TechnicsCategoryId = technicsCategoryId,
                VehicleKindId = vehicleKindId,

                //VehicleMakeId = vehicleMakeId,
                //VehicleModelId = vehicleModelId,

                VehicleMakeName = vehicleMakeName,
                VehicleModelName = vehicleModelName,

                VehicleBodyTypeId = vehicleBodyTypeId,
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
            List<ReportVehicleManageBlock> reportVehicleManageBlocks = ReportVehicleUtil.GetAllReportVehicleManageBlocks(filter, 0, CurrentUser);

            TechnicsCategory technicsCategory = null;
            int techCategoryId = 0;
            if (int.TryParse(technicsCategoryId, out techCategoryId))
            {
                technicsCategory = TechnicsCategoryUtil.GetTechnicsCategory(techCategoryId, CurrentUser);
            }

            GTableItem vehicleKind = null;
            int vehKindId = 0;
            if (int.TryParse(vehicleKindId, out vehKindId))
            {
                vehicleKind = GTableItemUtil.GetTableItem("VehicleKind", vehKindId, ModuleUtil.RES(), CurrentUser);
            }

            //VehicleMake vehicleMake = null;
            //int vehMakeId = 0;
            //if (int.TryParse(vehicleMakeId, out vehMakeId))
            //{
            //    vehicleMake = VehicleMakeUtil.GetVehicleMake(vehMakeId, CurrentUser);
            //}

            //VehicleModel vehicleModel = null;
            //int vehModelId = 0;
            //if (int.TryParse(vehicleModelId, out vehModelId))
            //{
            //    vehicleModel = VehicleModelUtil.GetVehicleModel(vehModelId, CurrentUser);
            //}

            GTableItem vehicleBodyType = null;
            int vehBodyTypeId = 0;
            if (int.TryParse(vehicleBodyTypeId, out vehBodyTypeId))
            {
                vehicleBodyType = GTableItemUtil.GetTableItem("VehicleBodyType", vehBodyTypeId, ModuleUtil.RES(), CurrentUser);
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
                                            <span style='font-weight: bold;'>" + (vehicleKind != null ? vehicleKind.TableValue : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Марка:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + vehicleMakeName + @"</span>
                                        </td>
                                        <td align='right' colspan='1'>
                                            <span style='font-weight: normal;'>Модел:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + vehicleModelName + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='" + headerLeftColspan + @"'>
                                            <span style='font-weight: normal;'>Каросерия:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (vehicleBodyType != null ? vehicleBodyType.TableValue : All) + @"</span>
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


            if (reportVehicleManageBlocks.Count() > 0)
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
                                (!isVehicleKindHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Вид</th>" : "") +
                                (!isBodyTypeHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Каросерия</th>" : "") +
                                (!isVehicleMakeHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Марка</th>" : "") +
                                (!isVehicleModelHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Модел</th>" : "") +
                                (!isMilDepartmentHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>На отчет в</th>" : "") +
                                (!isMilRepStatusHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Състояние по отчета</th>" : "") +
                                (!isOwnershipHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Собственик</th>" : "") +
                                (!isAddressHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Адрес</th>" : "") +
                            @"</tr>
                        </thead><tbody>");

                int counter = 1;

                foreach (ReportVehicleManageBlock reportVehicleManageBlock in reportVehicleManageBlocks)
                {
                    html.Append(@"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>" +
                                (!isNormativeTechnicsHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.NormativeTechnicsCode + "</td>") : "") + 
                                (!isRegNumberHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.RegNumber + "</td>") : "") + 
                                (!isInventoryNumberHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.InventoryNumber + "</td>") : "") + 
                                (!isTechnicsCategoryHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.TechnicsCategory + "</td>") : "") +
                                (!isVehicleKindHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.VehicleKind + "</td>") : "") +
                                (!isBodyTypeHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.BodyType + "</td>") : "") + 
                                (!isVehicleMakeHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.VehicleMake + "</td>") : "") + 
                                (!isVehicleModelHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.VehicleModel + "</td>") : "") +
                                (!isMilDepartmentHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.MilitaryDepartment + "</td>") : "") +
                                (!isMilRepStatusHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.MilitaryReportStatus + "</td>") : "") +
                                (!isOwnershipHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.Ownership + "</td>") : "") +
                                (!isAddressHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportVehicleManageBlock.Address + "</td>") : "") + @"
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
