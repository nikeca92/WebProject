using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintAllReservists : RESPage
    {
        const string All = "Всички";

        string firstAndSurName = "";
        string familyName = "";
        string initials = "";
        string identNumber = "";
        string militaryCategoryId = "";
        string militaryRankId = "";
        string militaryReportStatusId = "";
        string militaryCommand = "";
        string militaryDepartmentId = "";
        string position = "";
        string milAppointedRepSpecTypeId = "";
        string milAppointedRepSpecId = "";
        string milRepSpecTypeId = "";
        string milRepSpecId = "";
        string positionTitleId = "";
        bool isPrimaryPositionTitle = false;
        string administrationId = "";
        string languageId = "";
        string educationId = "";
        string civilSpecialityId = "";
        bool isPermAddress = true;
        string postCode = "";
        string regionId = "";
        string municipalityId = "";
        string cityId = "";
        string districtId = "";
        string address = "";
        string workCompany_UnifiedIdentityCode = "";
        string workCompany_Name = "";
        bool hasBeenOnMission = false;
        string appointmentIsDelivered = "";
        string isSuitableForMobAppointment = "";
        string readiness = "";
        string professionID = "";
        string specialityID = "";

        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "900";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "55";
        }

        private DateTime? postBackStart = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime? pageStart = null;
            if (!IsPostBack)
                pageStart = BenchmarkLog.WriteStart("Отваряне на екран 'ПЕЧАТ: Списък на водените на военен отчет'", CurrentUser, Request);

            if (IsPostBack)
                postBackStart = BenchmarkLog.WriteStart("PostBack в екран 'ПЕЧАТ: Списък на водените на военен отчет'", CurrentUser, Request);

            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("RES_HUMANRES") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["FirstAndSurName"]))
                {
                    firstAndSurName = Request.Params["FirstAndSurName"];
                }

                if (!String.IsNullOrEmpty(Request.Params["FamilyName"]))
                {
                    familyName = Request.Params["FamilyName"];
                }

                if (!String.IsNullOrEmpty(Request.Params["Initials"]))
                {
                    initials = Request.Params["Initials"];
                }

                if (!String.IsNullOrEmpty(Request.Params["IdentNumber"]))
                {
                    identNumber = Request.Params["IdentNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCategoryId"]))
                {
                    militaryCategoryId = Request.Params["MilitaryCategoryId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryRankId"]))
                {
                    militaryRankId = Request.Params["MilitaryRankId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryReportStatusId"]))
                {
                    militaryReportStatusId = Request.Params["MilitaryReportStatusId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCommand"]))
                {
                    militaryCommand = Request.Params["MilitaryCommand"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["Position"]))
                {
                    position = Request.Params["Position"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilAppointedRepSpecTypeId"]))
                {
                    milAppointedRepSpecTypeId = Request.Params["MilAppointedRepSpecTypeId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilAppointedRepSpecId"]))
                {
                    milAppointedRepSpecId = Request.Params["MilAppointedRepSpecId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilRepSpecTypeId"]))
                {
                    milRepSpecTypeId = Request.Params["MilRepSpecTypeId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilRepSpecId"]))
                {
                    milRepSpecId = Request.Params["MilRepSpecId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["PositionTitleId"]))
                {
                    positionTitleId = Request.Params["PositionTitleId"];
                }

                isPrimaryPositionTitle = Request.Params["IsPrimaryPositionTitle"] == "1";

                if (!String.IsNullOrEmpty(Request.Params["AdministrationId"]))
                {
                    administrationId = Request.Params["AdministrationId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["LanguageId"]))
                {
                    languageId = Request.Params["LanguageId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["EducationId"]))
                {
                    educationId = Request.Params["EducationId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["CivilSpecialityId"]))
                {
                    civilSpecialityId = Request.Params["CivilSpecialityId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["IsPermAddress"]))
                {
                    isPermAddress = Request.Params["IsPermAddress"] == "1";
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

                if (!String.IsNullOrEmpty(Request.Params["WorkCompany_UnifiedIdentityCode"]))
                {
                    workCompany_UnifiedIdentityCode = Request.Params["WorkCompany_UnifiedIdentityCode"];
                }

                if (!String.IsNullOrEmpty(Request.Params["WorkCompany_Name"]))
                {
                    workCompany_Name = Request.Params["WorkCompany_Name"];
                }

                if (!String.IsNullOrEmpty(Request.Params["HasBeenOnMission"]))
                {
                    hasBeenOnMission = Request.Params["HasBeenOnMission"] == "1";
                }

                if (!String.IsNullOrEmpty(Request.Params["AppointmentIsDelivered"]))
                {
                    appointmentIsDelivered = Request.Params["AppointmentIsDelivered"];
                }

                if (!String.IsNullOrEmpty(Request.Params["hdnSuitableForMobAppointment"]))
                {
                    isSuitableForMobAppointment = Request.Params["hdnSuitableForMobAppointment"];
                }

                if (!String.IsNullOrEmpty(Request.Params["Readiness"]))
                {
                    readiness = Request.Params["Readiness"];
                }

                if (!String.IsNullOrEmpty(Request.Params["ProfessionId"]))
                {
                    professionID = Request.Params["ProfessionId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["SpecialityId"]))
                {
                    specialityID = Request.Params["SpecialityId"];
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

            if (pageStart.HasValue)
                BenchmarkLog.WriteEnd("Край на зареждане на екран 'ПЕЧАТ: Списък на водените на военен отчет'", CurrentUser, Request, pageStart.Value);
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
            ReservistManageFilter filter = new ReservistManageFilter()
            {
                FirstAndSurName = firstAndSurName,
                FamilyName = familyName,
                Initials = initials,
                IdentNumber = identNumber,
                MilitaryCategory = militaryCategoryId,
                MilitaryRank = militaryRankId,
                MilitaryReportStatus = militaryReportStatusId,
                MilitaryCommand = militaryCommand,
                MilitaryDepartment = militaryDepartmentId,
                Position = position,
                MilAppointedRepSpecType = milAppointedRepSpecTypeId,
                MilAppointedRepSpec = milAppointedRepSpecId,
                MilRepSpecType = milRepSpecTypeId,
                MilRepSpec = milRepSpecId,
                PositionTitle = positionTitleId,
                IsPrimaryPositionTitle = isPrimaryPositionTitle,
                Administration = administrationId,
                Language = languageId,
                Education = educationId,
                CivilSpeciality = civilSpecialityId,
                IsPermAddress = isPermAddress,
                PostCode = postCode,          
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                Address = address,
                WorkUnifiedIdentityCode = workCompany_UnifiedIdentityCode,
                WorkCompanyName = workCompany_Name,
                HasBeenOnMission = hasBeenOnMission,
                AppointmentIsDelivered = appointmentIsDelivered,
                IsSuitableForMobAppointment = isSuitableForMobAppointment,
                Readiness = readiness,
                ProfessionId = professionID,
                SpecialityId = specialityID,
                OrderBy = sortBy,
                PageIdx = 0
            };

            DateTime start = BenchmarkLog.WriteStart("\tНачало на зареждане на всички записи от базата данни (HTML)", CurrentUser, Request);

            //Get the list of records according to the specified filters and order
            List<ReservistManageBlock> reservists = ReservistUtil.GetAllReservistManageBlocks(filter, 0, CurrentUser);

            BenchmarkLog.WriteEnd(String.Format("\tКрай на зареждане на всички записи от базата данни. Брой записи: {0}.", reservists.Count.ToString()), CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\tНачало на зареждане на филтрите", CurrentUser, Request);

            MilitaryCategory militaryCategory = null;
            if (!String.IsNullOrEmpty(militaryCategoryId))
            {
                militaryCategory = MilitaryCategoryUtil.GetMilitaryCategory(int.Parse(militaryCategoryId), CurrentUser);
            }

            MilitaryRank militaryRank = null;
            if (!String.IsNullOrEmpty(militaryRankId))
            {
                militaryRank = MilitaryRankUtil.GetMilitaryRank(militaryRankId, CurrentUser);
            }

            MilitaryReportStatus militaryReportStatus = null;
            if (!String.IsNullOrEmpty(militaryReportStatusId))
            {
                militaryReportStatus = MilitaryReportStatusUtil.GetMilitaryReportStatus(int.Parse(militaryReportStatusId), CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (!String.IsNullOrEmpty(militaryDepartmentId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(militaryDepartmentId), CurrentUser);
            }

            MilitaryReportSpecialityType milAppointedRepSpecType = null;
            if (!String.IsNullOrEmpty(milAppointedRepSpecTypeId))
            {
                milAppointedRepSpecType = MilitaryReportSpecialityTypeUtil.GetMilitaryReportSpecialityType(int.Parse(milAppointedRepSpecTypeId), CurrentUser);
            }

            MilitaryReportSpeciality milAppointedRepSpec = null;
            if (!String.IsNullOrEmpty(milAppointedRepSpecId))
            {
                milAppointedRepSpec = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(int.Parse(milAppointedRepSpecId), CurrentUser);
            }

            MilitaryReportSpecialityType milRepSpecType = null;
            if (!String.IsNullOrEmpty(milRepSpecTypeId))
            {
                milRepSpecType = MilitaryReportSpecialityTypeUtil.GetMilitaryReportSpecialityType(int.Parse(milRepSpecTypeId), CurrentUser);
            }

            MilitaryReportSpeciality milRepSpec = null;
            if (!String.IsNullOrEmpty(milRepSpecId))
            {
                milRepSpec = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(int.Parse(milRepSpecId), CurrentUser);
            }

            PositionTitle positionTitle = null;
            if (!String.IsNullOrEmpty(positionTitleId))
            {
                positionTitle = PositionTitleUtil.GetPositionTitle(int.Parse(positionTitleId), CurrentUser);
            }

            Administration administration = null;
            if (!String.IsNullOrEmpty(administrationId))
            {
                administration = AdministrationUtil.GetAdministration(int.Parse(administrationId), CurrentUser);
            }

            PersonLanguage personLanguage = null;
            if (!String.IsNullOrEmpty(languageId))
            {
                personLanguage = PersonLanguageUtil.GetPersonLanguage(languageId, CurrentUser);
            }

            PersonEducation personEducation = null;
            if (!String.IsNullOrEmpty(educationId))
            {
                personEducation = PersonEducationUtil.GetPersonEducation(educationId, CurrentUser);
            }

            PersonSchoolSubject civilSpeciality = null;
            if (!String.IsNullOrEmpty(civilSpecialityId))
            {
                civilSpeciality = PersonSchoolSubjectUtil.GetPersonSchoolSubject(civilSpecialityId, CurrentUser);
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

            BenchmarkLog.WriteEnd("\tКрай на зареждане на филтрите", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\tНачало на построяване на справката", CurrentUser, Request);

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 240px;'>
                                    <span class='Label'>Име и презиме:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                   <span class='ValueLabel'>" + firstAndSurName + @"</span>&nbsp;&nbsp;
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>ЕГН:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 285px;'>
                                    <span class='ValueLabel'>" + identNumber + @"</span>
                                </td>
                             </tr>
                             <tr>
                                 <td align='right' style=''>
                                   <span class='Label'>Фамилия:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + familyName + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Инициали:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + initials + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Категория:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (militaryCategory != null ? militaryCategory.CategoryName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Военно звание:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (militaryRank != null ? militaryRank.LongName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Състояние по отчета:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (militaryReportStatus != null ? militaryReportStatus.MilitaryReportStatusName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Команда:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + militaryCommand + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>На отчет в:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Длъжност:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + position + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Назначен на тип ВОС:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (milAppointedRepSpecType != null ? milAppointedRepSpecType.TypeName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Назначен на ВОС:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (milAppointedRepSpec != null ? milAppointedRepSpec.CodeAndName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Тип ВОС:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (milRepSpecType != null ? milRepSpecType.TypeName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>ВОС:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (milRepSpec != null ? milRepSpec.CodeAndName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Подходяща длъжност:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (positionTitle != null ? positionTitle.PositionTitleName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Основна длъжност:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (isPrimaryPositionTitle ? "Да" : "Не") + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Работил/служил в:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (administration != null ? administration.AdministrationName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Чужд език:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (personLanguage != null ? personLanguage.PersonLanguageName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Образование:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (personEducation != null ? personEducation.PersonEducationName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Гражданска специалност:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (civilSpeciality != null ? civilSpeciality.PersonSchoolSubjectName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Месторабота " + CommonFunctions.GetLabelText("UnifiedIdentityCode") + @":&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + workCompany_UnifiedIdentityCode + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Име на фирмата:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + workCompany_Name + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Бил на мисия:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (hasBeenOnMission ? "Да" : "Не") + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Връчено МН:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (appointmentIsDelivered != "" ? (appointmentIsDelivered == ListItems.GetOptionYes().Value ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Вид резерв:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (readiness != "" ? ReadinessUtil.ReadinessName(int.Parse(readiness)) : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Подходящ за МН:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (isSuitableForMobAppointment != "" ? (isSuitableForMobAppointment == ListItems.GetOptionYes().Value ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) : All) + @"</span>
                                </td>
                             </tr>


                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Професия:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (professionID != "" ? ProfessionUtil.GetProfession(int.Parse(professionID), CurrentUser).ProfessionName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Специалност:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (specialityID != "" ? SpecialityUtil.GetSpeciality(int.Parse(specialityID), CurrentUser).SpecialityName : All) + @"</span>
                                </td>
                             </tr>


                             <tr>
                                <td colspan='4' align='left'>
                                    &nbsp;
                                </td>
                             </tr>
                             <tr>
                                <td colspan='4' align='left'>
                                    <span class='Label'>" + (isPermAddress ? "Постоянен адрес" : "Настоящ адрес") + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Пощенски код:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + postCode + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Област:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (region != null ? region.RegionName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Населено място:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (city != null ? city.CityName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Община:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (municipality != null ? municipality.MunicipalityName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Адрес:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + address + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Район:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (district != null ? district.DistrictName : All) + @"</span>
                                </td>
                             </tr>");

            if (reservists.Count() > 0)
            {
                html.Append(@"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 15px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 85px;'>Име и презиме</th>
                                <th style='width: 85px;'>Фамилия</th>
                                <th style='width: 70px;'>ЕГН</th>
                                <th style='width: 70px;'>Звание</th>
                                <th style='width: 70px;'>ВОС</th>
                                <th style='width: 80px;'>Населено място</th>
                                <th style='width: 90px;'>На отчет в</th>
                                <th style='width: 90px;'>Категория</th>
                                <th style='width: 100px;'>Състояние по отчета</th>
                                <th style='width: 90px; border-right: 1px solid #000000;'>Команда</th>
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            foreach (ReservistManageBlock рeservistManageBlock in reservists)
            {
                html.Append(@"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + рeservistManageBlock.FirstAndSurName + @"</td>
                            <td align='left'>" + рeservistManageBlock.FamilyName + @"</td>
                            <td align='left'>" + рeservistManageBlock.IdentNumber + @"</td>
                            <td align='left'>" + рeservistManageBlock.MilitaryRankName + @"</td>
                            <td align='left'>" + рeservistManageBlock.MilReportingSpecialityCode + @"</td>
                            <td align='left'>" + рeservistManageBlock.RegionMuniciplaityAndCity + @"</td>
                            <td align='left'>" + рeservistManageBlock.MilitaryDepartment + @"</td>
                            <td align='left'>" + рeservistManageBlock.MilitaryCategory + @"</td>
                            <td align='left'>" + рeservistManageBlock.MilitaryReportStatus + @"</td>
                            <td align='left'>" + рeservistManageBlock.MilitaryCommand + @"</td>
                          </tr>");
                counter++;
            }

            if (reservists.Count() > 0)
            {
                html.Append("</tbody></table></td></tr>");
            }

            html.Append("</table>");

            BenchmarkLog.WriteEnd("\tКрай на построяване на справката", CurrentUser, Request, start);

            return html.ToString();
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_HUMANRES") != UIAccessLevel.Hidden)
            {
                string result = this.GeneratePageContent(true);
                Response.Clear();
                Response.AppendHeader("content-disposition", "attachment; filename=Reservists.xls");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(result);

                if (postBackStart.HasValue)
                    BenchmarkLog.WriteEnd("Край на PostBack в екран 'ПЕЧАТ: Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);

                Response.End();
            }
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            ReservistManageFilter filter = new ReservistManageFilter()
            {
                FirstAndSurName = firstAndSurName,
                FamilyName = familyName,
                Initials = initials,
                IdentNumber = identNumber,
                MilitaryCategory = militaryCategoryId,
                MilitaryRank = militaryRankId,
                MilitaryReportStatus = militaryReportStatusId,
                MilitaryCommand = militaryCommand,
                MilitaryDepartment = militaryDepartmentId,
                Position = position,
                MilAppointedRepSpecType = milAppointedRepSpecTypeId,
                MilAppointedRepSpec = milAppointedRepSpecId,
                MilRepSpecType = milRepSpecTypeId,
                MilRepSpec = milRepSpecId,
                PositionTitle = positionTitleId,
                IsPrimaryPositionTitle = isPrimaryPositionTitle,
                Administration = administrationId,
                Language = languageId,
                Education = educationId,
                CivilSpeciality = civilSpecialityId,
                IsPermAddress = isPermAddress,
                PostCode = postCode,
                Region = regionId,
                Municipality = municipalityId,
                City = cityId,
                District = districtId,
                Address = address,
                WorkUnifiedIdentityCode = workCompany_UnifiedIdentityCode,
                WorkCompanyName = workCompany_Name,
                HasBeenOnMission = hasBeenOnMission,
                AppointmentIsDelivered = appointmentIsDelivered,
                IsSuitableForMobAppointment=isSuitableForMobAppointment,
                Readiness = readiness,
                ProfessionId = professionID,
                SpecialityId = specialityID,
                OrderBy = sortBy,
                PageIdx = 0
            };

            DateTime start = BenchmarkLog.WriteStart("\tНачало на зареждане на всички записи от базата данни (Excel)", CurrentUser, Request);

            //Get the list of records according to the specified filters and order
            List<ReservistManageBlock> reservists = ReservistUtil.GetAllReservistManageBlocks(filter, 0, CurrentUser);

            BenchmarkLog.WriteEnd(String.Format("\tКрай на зареждане на всички записи от базата данни. Брой записи: {0}.", reservists.Count.ToString()), CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\tНачало на зареждане на филтрите", CurrentUser, Request);

            MilitaryCategory militaryCategory = null;
            if (!String.IsNullOrEmpty(militaryCategoryId))
            {
                militaryCategory = MilitaryCategoryUtil.GetMilitaryCategory(int.Parse(militaryCategoryId), CurrentUser);
            }

            MilitaryRank militaryRank = null;
            if (!String.IsNullOrEmpty(militaryRankId))
            {
                militaryRank = MilitaryRankUtil.GetMilitaryRank(militaryRankId, CurrentUser);
            }

            MilitaryReportStatus militaryReportStatus = null;
            if (!String.IsNullOrEmpty(militaryReportStatusId))
            {
                militaryReportStatus = MilitaryReportStatusUtil.GetMilitaryReportStatus(int.Parse(militaryReportStatusId), CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (!String.IsNullOrEmpty(militaryDepartmentId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(militaryDepartmentId), CurrentUser);
            }

            MilitaryReportSpecialityType milAppointedRepSpecType = null;
            if (!String.IsNullOrEmpty(milAppointedRepSpecTypeId))
            {
                milAppointedRepSpecType = MilitaryReportSpecialityTypeUtil.GetMilitaryReportSpecialityType(int.Parse(milAppointedRepSpecTypeId), CurrentUser);
            }

            MilitaryReportSpeciality milAppointedRepSpec = null;
            if (!String.IsNullOrEmpty(milAppointedRepSpecId))
            {
                milAppointedRepSpec = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(int.Parse(milAppointedRepSpecId), CurrentUser);
            }

            MilitaryReportSpecialityType milRepSpecType = null;
            if (!String.IsNullOrEmpty(milRepSpecTypeId))
            {
                milAppointedRepSpecType = MilitaryReportSpecialityTypeUtil.GetMilitaryReportSpecialityType(int.Parse(milRepSpecTypeId), CurrentUser);
            }

            MilitaryReportSpeciality milRepSpec = null;
            if (!String.IsNullOrEmpty(milRepSpecId))
            {
                milRepSpec = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(int.Parse(milRepSpecId), CurrentUser);
            }

            PositionTitle positionTitle = null;
            if (!String.IsNullOrEmpty(positionTitleId))
            {
                positionTitle = PositionTitleUtil.GetPositionTitle(int.Parse(positionTitleId), CurrentUser);
            }

            Administration administration = null;
            if (!String.IsNullOrEmpty(administrationId))
            {
                administration = AdministrationUtil.GetAdministration(int.Parse(administrationId), CurrentUser);
            }

            PersonLanguage personLanguage = null;
            if (!String.IsNullOrEmpty(languageId))
            {
                personLanguage = PersonLanguageUtil.GetPersonLanguage(languageId, CurrentUser);
            }

            PersonEducation personEducation = null;
            if (!String.IsNullOrEmpty(educationId))
            {
                personEducation = PersonEducationUtil.GetPersonEducation(educationId, CurrentUser);
            }

            PersonSchoolSubject civilSpeciality = null;
            if (!String.IsNullOrEmpty(civilSpecialityId))
            {
                civilSpeciality = PersonSchoolSubjectUtil.GetPersonSchoolSubject(civilSpecialityId, CurrentUser);
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

            BenchmarkLog.WriteEnd("\tКрай на зареждане на филтрите", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\tНачало на построяване на справката", CurrentUser, Request);

            StringBuilder html = new StringBuilder();

            html.Append(@"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                                <style>
                                    .dc
                                    {
                                       border: 1px solid black;
                                       text-align: left;
                                    }
                                </style>
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='11' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='11' style='font-weight: bold; font-size: 2em;'>Резервисти</td></tr>
                                    <tr><td colspan='11'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='11' style='font-weight: bold; font-size: 1.3em;'>Списък на водените на военен отчет</td></tr>
                                    <tr><td colspan='11'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Име и презиме:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + firstAndSurName + @"</span>
                                        </td>
                                        
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>ЕГН:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + identNumber + @"</span>
                                        </td>
                                    
                                    </tr>

                                    
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Фамилия:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + familyName + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Инициали:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + initials + @"</span>
                                        </td>  
                                    </tr>
                                        
                        

                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Категория:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (militaryCategory != null ? militaryCategory.CategoryName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Военно звание:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (militaryRank != null ? militaryRank.LongName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Състояние по отчета:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (militaryReportStatus != null ? militaryReportStatus.MilitaryReportStatusName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Команда:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + militaryCommand + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>На отчет в:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Длъжност:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + position + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Назначен на тип ВОС:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (milAppointedRepSpecType != null ? milAppointedRepSpecType.TypeName: All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Назначен на ВОС:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (milAppointedRepSpec != null ? milAppointedRepSpec.CodeAndName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Тип ВОС:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (milRepSpecType != null ? milRepSpecType.TypeName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>ВОС:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (milRepSpec != null ? milRepSpec.CodeAndName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Подходяща длъжност:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (positionTitle != null ? positionTitle.PositionTitleName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Основна длъжност:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (isPrimaryPositionTitle ? "Да" : "Не") + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Работил/служил в:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (administration != null ? administration.AdministrationName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Чужд език:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (personLanguage != null ? personLanguage.PersonLanguageName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Образование:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (personEducation != null ? personEducation.PersonEducationName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Гражданска специалност:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (civilSpeciality != null ? civilSpeciality.PersonSchoolSubjectName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Месторабота " + CommonFunctions.GetLabelText("UnifiedIdentityCode") + @":&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + workCompany_UnifiedIdentityCode + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Име на фирмата:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + workCompany_Name + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Бил на мисия:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (hasBeenOnMission ? "Да" : "Не") + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Връчено МН:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (appointmentIsDelivered != "" ? (appointmentIsDelivered == ListItems.GetOptionYes().Value ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Вид резерв:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (readiness != "" ? ReadinessUtil.ReadinessName(int.Parse(readiness)) : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Подходящ за МН:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: bold;'>" + (isSuitableForMobAppointment != "" ? (isSuitableForMobAppointment == ListItems.GetOptionYes().Value ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) : All) + @"</span>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align='right'  colspan='3'>
                                            <span class='Label'>Професия:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span class='ValueLabel' style='font-weight: bold;'>" + (professionID != "" ? ProfessionUtil.GetProfession(int.Parse(professionID), CurrentUser).ProfessionName : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='2'>
                                            <span class='Label'>Специалност:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span class='ValueLabel' style='font-weight: bold;'>" + (specialityID != "" ? SpecialityUtil.GetSpeciality(int.Parse(specialityID), CurrentUser).SpecialityName : All) + @"</span>
                                        </td>
                                     </tr>

                                    <tr>
                                        <td colspan='11'>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>" + (isPermAddress ? "Постоянен адрес" : "Настоящ адрес") + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
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
                                        <td align='right' colspan='3'>
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
                                        <td align='right' colspan='3'>
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
                                        <td style='width: 30px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                    </tr>
                                </table>");


            if (reservists.Count() > 0)
            {
                html.Append(@"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Име и презиме</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Фамилия</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Звание</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ВОС</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Населено място</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>На отчет в</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Категория</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Състояние по отчета</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Команда</th>
                            </tr>
                        </thead><tbody>");

                int counter = 1;

                foreach (ReservistManageBlock reservistManageBlock in reservists)
                {
                    html.Append(@"
<tr>
<td class='dc'>" + counter + @"</td>
<td class='dc'>" + reservistManageBlock.FirstAndSurName + @"</td>
<td class='dc'>" + reservistManageBlock.FamilyName + @"</td>
<td class='dc'>" + reservistManageBlock.IdentNumber + @"</td>
<td class='dc'>" + reservistManageBlock.MilitaryRankName + @"</td>
<td class='dc'>" + reservistManageBlock.MilReportingSpecialityCode + @"</td>
<td class='dc'>" + reservistManageBlock.RegionMuniciplaityAndCity + @"</td>
<td class='dc'>" + reservistManageBlock.MilitaryDepartment + @"</td>
<td class='dc'>" + reservistManageBlock.MilitaryCategory + @"</td>
<td class='dc'>" + reservistManageBlock.MilitaryReportStatus + @"</td>
<td class='dc'>" + reservistManageBlock.MilitaryCommand + @"</td>
</tr>");
                    counter++;
                }

                html.Append("</tbody></table>");
            }

            html.Append("</body></html>");

            BenchmarkLog.WriteEnd("\tКрай на построяване на справката", CurrentUser, Request, start);

            return html.ToString();
        }
    }
}
