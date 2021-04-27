using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ManageReservists : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;
        private bool loadedMaxPage = false;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_HUMANRES";
            }
        }

        private DateTime? postBackStart = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSWriteBenchmarkLog")
            {
                JSWriteBenchmarkLog();
                return;
            }

            DateTime? pageStart = null;
            if(!IsPostBack)
                pageStart = BenchmarkLog.WriteStart("Отваряне на екран 'Списък на водените на военен отчет'", CurrentUser, Request);

            if (IsPostBack)
                postBackStart = BenchmarkLog.WriteStart("PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request);

            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("HumanResources", "ManageReservists");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            lblWorkCompany_UnifiedIdentityCode.Text = CommonFunctions.GetLabelText("UnifiedIdentityCode") + ":";

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate page header title
                SetPageTitle();

                //Populate any drop-downs and list-boxes
                DateTime listsStart = BenchmarkLog.WriteStart("\tНачало на зареждане на класификаторите", CurrentUser, Request);
                PopulateLists();
                BenchmarkLog.WriteEnd("\tКрай на зареждане на класификаторите", CurrentUser, Request, listsStart);

                if (Session["ManageReservists_Filter"] != null)
                {
                    LoadFilterFromSession();
                }

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());
            }

            //setup AddNewReservist button
            this.SetBtnNew();

            lblMessage.Text = "";
            lblGridMessage.Text = "";

            if (!IsPostBack && IsThereFilter())
            {
                btnRefresh_Click(Page, new EventArgs());
            }

            if (pageStart.HasValue)
                BenchmarkLog.WriteEnd("Край на зареждане на екран 'Списък на водените на военен отчет'", CurrentUser, Request, pageStart.Value);
        }

        private void RefreshMaxPage()
        {
            if (!loadedMaxPage)
            {
                DateTime start = BenchmarkLog.WriteStart("\tНачало на изчисляване на общия брой страници", CurrentUser, Request);

                //Collect the filter information to be able to pull the number of rows for this specific filter
                ReservistManageFilter filter = CollectFilterData();

                int allRows = ReservistUtil.GetAllReservistManageBlocksCount(filter, CurrentUser);
                //Get the number of rows and calculate the number of pages in the grid
                maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

                loadedMaxPage = true;

                BenchmarkLog.WriteEnd("\tКрай на изчисляване на общия брой страници", CurrentUser, Request, start);
            }
        }

        private void SetPageTitle()
        {
            lblHeaderTitle.InnerHtml = "Списък на водените на военен отчет";
        }

        // Setup AddNewReservist button according to rights of user's role
        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            DateTime start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Категория'", CurrentUser, Request);
            PopulateMilitaryCategory();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Категория'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Състояние по отчета'", CurrentUser, Request);
            PopulateMilitaryReportStatus();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Състояние по отчета'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'ВО'", CurrentUser, Request);
            PopulateMilitaryDepartment();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'ВО'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Назначен на тип ВОС'", CurrentUser, Request);
            PopulateAppointedMilRepSpecType();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Назначен на тип ВОС'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Тип ВОС'", CurrentUser, Request);
            PopulateMilRepSpecType();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Тип ВОС'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Длъжност'", CurrentUser, Request);
            PopulatePositionTitle();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Длъжност'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Работил/служил в'", CurrentUser, Request);
            PopulateAdministration();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Работил/служил в'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Чужд език'", CurrentUser, Request);
            PopulateLanguage();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Чужд език'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Образование'", CurrentUser, Request);
            PopulateEducation();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Образование'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Гражданска специалност'", CurrentUser, Request);
            PopulateCivilSpeciality();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Гражданска специалност'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Област'", CurrentUser, Request);
            PopulateRegions();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Област'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало списък 'Връчено МН'", CurrentUser, Request);
            PopulateAppointmentIsDelivered();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Връчено МН'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало списък 'Подходящ за МН'", CurrentUser, Request);
            PopulateSuitableForMobAppointment();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Подходящ за МН'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало списък 'Вид резерв'", CurrentUser, Request);
            PopulateReadiness();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Вид резерв'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало списък 'Професия'", CurrentUser, Request);
            PopulateProfessions();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Професия'", CurrentUser, Request, start);        
        }

        //Populate ddMilRepSpecType
        private void PopulateMilitaryCategory()
        {
            ddMilitaryCategory.DataSource = MilitaryCategoryUtil.GetAllMilitaryCategories(CurrentUser);
            ddMilitaryCategory.DataTextField = "CategoryName";
            ddMilitaryCategory.DataValueField = "CategoryId";
            ddMilitaryCategory.DataBind();
            ddMilitaryCategory.Items.Insert(0, ListItems.GetOptionAll());

            //initialize ddMilitaryRank with blannk value
            ddMilitaryRank.Items.Clear();
            ddMilitaryRank.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddMilitaryRank
        protected void ddMilitaryCategory_Changed(object sender, EventArgs e)
        {
            if (ddMilitaryCategory.SelectedValue != "-1")
            {
                ddMilitaryRank.DataSource = MilitaryRankUtil.GetAllMilitaryRanksByCategory(ddMilitaryCategory.SelectedValue, CurrentUser);
                ddMilitaryRank.DataTextField = "LongName";
                ddMilitaryRank.DataValueField = "MilitaryRankId";
                ddMilitaryRank.DataBind();
                ddMilitaryRank.Items.Insert(0, ListItems.GetOptionAll());
            }
            else
            {
                ddMilitaryRank.Items.Clear();
                ddMilitaryRank.Items.Insert(0, ListItems.GetOptionChooseOne());
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Populate ddMilitaryReportStatus
        private void PopulateMilitaryReportStatus()
        {
            ddMilitaryReportStatus.DataSource = MilitaryReportStatusUtil.GetAllMilitaryReportStatuses(CurrentUser);
            ddMilitaryReportStatus.DataTextField = "MilitaryReportStatusName";
            ddMilitaryReportStatus.DataValueField = "MilitaryReportStatusId";
            ddMilitaryReportStatus.DataBind();
            ddMilitaryReportStatus.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddMilitaryDepartment
        private void PopulateMilitaryDepartment()
        {
            ddMilitaryDepartment.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            ddMilitaryDepartment.DataTextField = "MilitaryDepartmentName";
            ddMilitaryDepartment.DataValueField = "MilitaryDepartmentId";
            ddMilitaryDepartment.DataBind();
            ddMilitaryDepartment.Items.Insert(0, ListItems.GetOptionAll());

            CommonFunctions.SetDropDownTooltip(ddMilitaryDepartment);
        }

        //Populate ddAppointedMilRepSpecType
        private void PopulateAppointedMilRepSpecType()
        {
            ddAppointedMilRepSpecType.DataSource = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(CurrentUser);
            ddAppointedMilRepSpecType.DataTextField = "TypeName";
            ddAppointedMilRepSpecType.DataValueField = "Type";
            ddAppointedMilRepSpecType.DataBind();
            ddAppointedMilRepSpecType.Items.Insert(0, ListItems.GetOptionAll());

            // initialize ddAppointedMilRepSpec with blank value
            ddAppointedMilRepSpec.Items.Clear();
            ddAppointedMilRepSpec.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddAppointedMilRepSpec
        protected void ddAppointedMilRepSpecType_Changed(object sender, EventArgs e)
        {
            if (ddAppointedMilRepSpecType.SelectedValue != "-1")
            {
                int type = int.Parse(ddAppointedMilRepSpecType.SelectedValue);

                ddAppointedMilRepSpec.DataSource = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialitiesByType(CurrentUser, type);
                ddAppointedMilRepSpec.DataTextField = "CodeAndName";
                ddAppointedMilRepSpec.DataValueField = "MilReportSpecialityId";
                ddAppointedMilRepSpec.DataBind();
                ddAppointedMilRepSpec.Items.Insert(0, ListItems.GetOptionAll());

                CommonFunctions.SetDropDownTooltip(ddAppointedMilRepSpec);
            }
            else
            {
                ddAppointedMilRepSpec.Items.Clear();
                ddAppointedMilRepSpec.Items.Insert(0, ListItems.GetOptionChooseOne());
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Populate ddMilRepSpecType
        private void PopulateMilRepSpecType()
        {
            ddMilRepSpecType.DataSource = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(CurrentUser);
            ddMilRepSpecType.DataTextField = "TypeName";
            ddMilRepSpecType.DataValueField = "Type";
            ddMilRepSpecType.DataBind();
            ddMilRepSpecType.Items.Insert(0, ListItems.GetOptionAll());

            // initialize ddMilRepSpec with blank value
            ddMilRepSpec.Items.Clear();
            ddMilRepSpec.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddMilRepSpec
        protected void ddMilRepSpecType_Changed(object sender, EventArgs e)
        {
            if (ddMilRepSpecType.SelectedValue != "-1")
            {
                int type = int.Parse(ddMilRepSpecType.SelectedValue);

                ddMilRepSpec.DataSource = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialitiesByType(CurrentUser, type);
                ddMilRepSpec.DataTextField = "CodeAndName";
                ddMilRepSpec.DataValueField = "MilReportSpecialityId";
                ddMilRepSpec.DataBind();
                ddMilRepSpec.Items.Insert(0, ListItems.GetOptionAll());

                CommonFunctions.SetDropDownTooltip(ddMilRepSpec);
            }
            else
            {
                ddMilRepSpec.Items.Clear();
                ddMilRepSpec.Items.Insert(0, ListItems.GetOptionChooseOne());
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Populate ddPositionTitle
        private void PopulatePositionTitle()
        {
            ddPositionTitle.DataSource = PositionTitleUtil.GetAllPositionTitles(CurrentUser);
            ddPositionTitle.DataTextField = "PositionTitleName";
            ddPositionTitle.DataValueField = "PositionTitleId";
            ddPositionTitle.DataBind();
            ddPositionTitle.Items.Insert(0, ListItems.GetOptionAll());

            CommonFunctions.SetDropDownTooltip(ddPositionTitle);
        }

        //Populate ddAdministration
        private void PopulateAdministration()
        {
            ddAdministration.DataSource = AdministrationUtil.GetAllAdministrations(CurrentUser);
            ddAdministration.DataTextField = "AdministrationName";
            ddAdministration.DataValueField = "AdministrationId";
            ddAdministration.DataBind();
            ddAdministration.Items.Insert(0, ListItems.GetOptionAll());

            CommonFunctions.SetDropDownTooltip(ddAdministration);
        }

        //Populate ddLanguage
        private void PopulateLanguage()
        {
            ddLanguage.DataSource = PersonLanguageUtil.GetAllPersonLanguages(CurrentUser);
            ddLanguage.DataTextField = "PersonLanguageName";
            ddLanguage.DataValueField = "PersonLanguageCode";
            ddLanguage.DataBind();
            ddLanguage.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddEducation
        private void PopulateEducation()
        {
            ddEducation.DataSource = PersonEducationUtil.GetAllPersonEducations(CurrentUser);
            ddEducation.DataTextField = "PersonEducationName";
            ddEducation.DataValueField = "PersonEducationCode";
            ddEducation.DataBind();
            ddEducation.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddCivilSpeciality
        private void PopulateCivilSpeciality()
        {
            ddCivilSpeciality.DataSource = PersonSchoolSubjectUtil.GetAllPersonSchoolSubjects(CurrentUser);
            ddCivilSpeciality.DataTextField = "PersonSchoolSubjectName";
            ddCivilSpeciality.DataValueField = "PersonSchoolSubjectCode";
            ddCivilSpeciality.DataBind();
            ddCivilSpeciality.Items.Insert(0, ListItems.GetOptionAll());

            CommonFunctions.SetDropDownTooltip(ddCivilSpeciality);
        }

        //Populate ddRegion
        private void PopulateRegions()
        {
            ddRegion.DataSource = RegionUtil.GetRegions(CurrentUser);
            ddRegion.DataTextField = "RegionName";
            ddRegion.DataValueField = "RegionId";
            ddRegion.DataBind();
            ddRegion.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddRegion_Changed(object sender, EventArgs e)
        {
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();

            if (ddRegion.SelectedValue != "-1")
            {
                int regionId = int.Parse(ddRegion.SelectedValue);

                PopulateMunicipalities(regionId);
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Populate ddMuniciplaity
        private void PopulateMunicipalities(int regionID)
        {
            ddMuniciplaity.DataSource = MunicipalityUtil.GetMunicipalities(regionID, CurrentUser); ;
            ddMuniciplaity.DataTextField = "MunicipalityName";
            ddMuniciplaity.DataValueField = "MunicipalityId";
            ddMuniciplaity.DataBind();
            ddMuniciplaity.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddMuniciplaity_Changed(object sender, EventArgs e)
        {
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();

            if (ddMuniciplaity.SelectedValue != "-1")
            {
                int municiplaityId = int.Parse(ddMuniciplaity.SelectedValue);

                PopulateCities(municiplaityId);
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Populate ddCity
        private void PopulateCities(int municipalityID)
        {
            ddCity.DataSource = CityUtil.GetCities(municipalityID, CurrentUser);
            ddCity.DataTextField = "CityName";
            ddCity.DataValueField = "CityId";
            ddCity.DataBind();
            ddCity.Items.Insert(0, ListItems.GetOptionAll());

            // Initialize ddDistrict with blank value
            ddDistrict.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddDistrict
        protected void ddCity_Changed(object sender, EventArgs e)
        {
            ddDistrict.Items.Clear();

            if (ddCity.SelectedValue != "-1")
            {
                int cityId = int.Parse(ddCity.SelectedValue);

                ddDistrict.DataSource = DistrictUtil.GetDistricts(cityId, CurrentUser);
                ddDistrict.DataTextField = "DistrictName";
                ddDistrict.DataValueField = "DistrictId";
                ddDistrict.DataBind();

                if (ddDistrict.Items.Count > 0)
                    ddDistrict.Items.Insert(0, ListItems.GetOptionAll());
                else
                    ddDistrict.Items.Insert(0, ListItems.GetOptionChooseOne());
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Populate ddAppointmentIsDelivered
        private void PopulateAppointmentIsDelivered()
        {
            ddAppointmentIsDelivered.Items.Add(ListItems.GetOptionAll());
            ddAppointmentIsDelivered.Items.Add(ListItems.GetOptionYes());
            ddAppointmentIsDelivered.Items.Add(ListItems.GetOptionNo());
        }

        //Populate suitableForMobAppointmentDropDownList
        private void PopulateSuitableForMobAppointment()
        {
            suitableForMobAppointmentDropDownList.Items.Add(ListItems.GetOptionAll());
            suitableForMobAppointmentDropDownList.Items.Add(ListItems.GetOptionYes());
            suitableForMobAppointmentDropDownList.Items.Add(ListItems.GetOptionNo());
        }

        //Populate ddReadiness
        private void PopulateReadiness()
        {
            int baseReadiness = 1;
            int additionalReadiness = 2;

            ddReadiness.Items.Clear();

            ddReadiness.Items.Insert(0, ListItems.GetOptionAll());
            ddReadiness.Items.Add(new ListItem(ReadinessUtil.ReadinessName(baseReadiness), baseReadiness.ToString()));
            ddReadiness.Items.Add(new ListItem(ReadinessUtil.ReadinessName(additionalReadiness), additionalReadiness.ToString()));
        }
       
        //Populate ddPrfoession
        private void PopulateProfessions()
        {
            ddProfession.DataSource = ProfessionUtil.GetAllProfessions(CurrentUser);
            ddProfession.DataTextField = "ProfessionName";
            ddProfession.DataValueField = "ProfessionId";
            ddProfession.DataBind();
            ddProfession.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddSpeciality
        private void PopulateSpecialities(int professionId)
        {
            ddSpeciality.DataSource = SpecialityUtil.GetSpecialities(professionId, CurrentUser); ;
            ddSpeciality.DataTextField = "SpecialityName";
            ddSpeciality.DataValueField = "SpecialityId";
            ddSpeciality.DataBind();
            ddSpeciality.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddProfession_Changed(object sender, EventArgs e)
        {
            ddSpeciality.Items.Clear();

            if (ddProfession.SelectedValue != "-1")
            {
                int professionId = int.Parse(ddProfession.SelectedValue);
                PopulateSpecialities(professionId);
            }
        }
  
        //Setup some styling on the page
        private void SetupStyle()
        {

        }

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";

            if (!isDataValid)
            {
                lblMessage.CssClass = "ErrorText";
                lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Refresh the data grid
        private void RefreshReservists()
        {
            RefreshMaxPage();

            DateTime start = BenchmarkLog.WriteStart("\tНачало на зареждане на записите за избраната страница", CurrentUser, Request);

            pnlPaging.Visible = true;

            string html = "";

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReservistManageFilter filter = CollectFilterData();

            StoreFilterIntoSession(filter);

            //Get the list of records according to the specified filters, order and paging
            DateTime startSQL = BenchmarkLog.WriteStart("\t\tНачало извличане на данните от базата данни", CurrentUser, Request);
            List<ReservistManageBlock> reservists = ReservistUtil.GetAllReservistManageBlocks(filter, pageLength, CurrentUser);
            BenchmarkLog.WriteEnd("\t\tКрай извличане на данните от базата данни", CurrentUser, Request, startSQL);
            
            //No data found
            if (reservists.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllReservists.Visible = false;
                this.btnExportAllReservists.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintAllReservists.Visible = true;
                this.btnExportAllReservists.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Име и презиме" + arrOrderCol[0] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(10);'>Фамилия" + arrOrderCol[9] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>ЕГН" + arrOrderCol[1] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Звание" + arrOrderCol[2] + @"</th>
                               <th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(9);'>ВОС" + arrOrderCol[8] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Населено място" + arrOrderCol[3] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>На отчет в" + arrOrderCol[4] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Категория" + arrOrderCol[5] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Състояние по отчета" + arrOrderCol[6] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(8);'>Команда" + arrOrderCol[7] + @"</th>     
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(11);'>Подходяща длъжност" + arrOrderCol[10] + @"</th>      
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReservistManageBlock reservist in reservists)
                {
                    string cellStyle = "vertical-align: top;";

                    string chooseHTML = "";

                    if (GetUIItemAccessLevel("RES_HUMANRES") != UIAccessLevel.Hidden && GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") != UIAccessLevel.Hidden)
                        chooseHTML = @"onmouseover=""this.className = 'SelectionItem';"" onmouseout=""this.className = '" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"';"" onclick=""ChooseReservist(" + reservist.ReservistID + @");"" title='Избери'";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' " + chooseHTML + @" >
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.FirstAndSurName + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.FamilyName + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.IdentNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.MilitaryRankName + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.MilReportingSpecialityCode + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.RegionMuniciplaityAndCity + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.MilitaryDepartment + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.MilitaryCategory + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.MilitaryReportStatus + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.PositionTitle + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlDataGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Заявката беше изтрита успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }

            BenchmarkLog.WriteEnd("\tКрай на зареждане на записите за избраната страница", CurrentUser, Request, start);
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlSearchHint.Visible = false;

            if (ValidateData())
            {
                RefreshMaxPage();

                if (sender != Page ||
                   (!String.IsNullOrEmpty(hdnPageIdx.Value) && int.Parse(hdnPageIdx.Value) > maxPage))
                    hdnPageIdx.Value = "1";

                RefreshReservists();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshReservists();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page > 1)
                {
                    page--;
                    hdnPageIdx.Value = page.ToString();

                    RefreshReservists();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                RefreshMaxPage();

                int page = int.Parse(hdnPageIdx.Value);

                if (page < maxPage)
                {
                    page++;
                    hdnPageIdx.Value = page.ToString();

                    RefreshReservists();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                RefreshMaxPage();

                hdnPageIdx.Value = maxPage.ToString();
                RefreshReservists();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                RefreshMaxPage();

                int gotoPage;
                if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
                {
                    hdnPageIdx.Value = gotoPage.ToString();
                    RefreshReservists();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        //Refresh the paging image buttons
        private void SetImgBtns()
        {
            RefreshMaxPage();

            int page = int.Parse(hdnPageIdx.Value);

            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirst.ImageUrl = "../Images/ButtonFirst.png";
            btnPrev.ImageUrl = "../Images/ButtonPrev.png";
            btnLast.ImageUrl = "../Images/ButtonLast.png";
            btnNext.ImageUrl = "../Images/ButtonNext.png";

            if (page == 1)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                btnFirst.ImageUrl = "../Images/ButtonFirstDisabled.png";
                btnPrev.ImageUrl = "../Images/ButtonPrevDisabled.png";
            }

            if (page == maxPage)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }
        }

        //Navigate back to the previous screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);

            Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Go to create a new record
        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);

            Response.Redirect("~/ContentPages/AddEditReservist.aspx");
        }

        //Collect the filet information from the page
        private ReservistManageFilter CollectFilterData()
        {
            ReservistManageFilter filter = new ReservistManageFilter();

            this.hdnFirstAndSurName.Value = txtFirstAndSurName.Text;
            this.hdnFamilyName.Value = txtFamilyName.Text;
            this.hdnInitials.Value = txtInitials.Text;
            this.hdnIdentNumber.Value = txtIdentNumber.Text;

            this.hdnMilitaryCommand.Value = txtMilitaryCommand.Text;
            this.hdnPostCode.Value = txtPostCode.Text;
            this.hdnPosition.Value = txtPosition.Text;
            this.hdnAddress.Value = txtAddress.Text;

            string militaryCategory = "";

            if (ddMilitaryCategory.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryCategory = ddMilitaryCategory.SelectedValue;
                this.hdnMilitaryCategoryId.Value = this.ddMilitaryCategory.SelectedValue;
            }
            else
            {
                this.hdnMilitaryCategoryId.Value = "";
            }

            string militaryRank = "";

            if (ddMilitaryRank.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryRank = ddMilitaryRank.SelectedValue;
                this.hdnMilitaryRankId.Value = this.ddMilitaryRank.SelectedValue;
            }
            else
            {
                this.hdnMilitaryRankId.Value = "";
            }

            string militaryReportStatus = "";

            if (ddMilitaryReportStatus.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryReportStatus = ddMilitaryReportStatus.SelectedValue;
                this.hdnMilitaryReportStatusId.Value = this.ddMilitaryReportStatus.SelectedValue;
            }
            else
            {
                this.hdnMilitaryReportStatusId.Value = "";
            }

            string militaryDepartment = "";

            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryDepartment = ddMilitaryDepartment.SelectedValue;
                this.hdnMilitaryDepartmentId.Value = this.ddMilitaryDepartment.SelectedValue;
            }
            else
            {
                this.hdnMilitaryDepartmentId.Value = "";
            }

            string milAppointedRepSpecType = "";
            string milAppointedRepSpec = "";

            if (ddAppointedMilRepSpecType.SelectedValue != ListItems.GetOptionAll().Value)
            {
                milAppointedRepSpecType = ddAppointedMilRepSpecType.SelectedValue;
                this.hdnMilAppointedRepSpecTypeId.Value = this.ddAppointedMilRepSpecType.SelectedValue;

                if (ddAppointedMilRepSpec.SelectedValue != ListItems.GetOptionAll().Value)
                {
                    milAppointedRepSpec = ddAppointedMilRepSpec.SelectedValue;
                    this.hdnMilAppointedRepSpecId.Value = this.ddAppointedMilRepSpec.SelectedValue;
                }
                else
                {
                    this.hdnMilAppointedRepSpecId.Value = "";
                }
            }
            else
            {
                this.hdnMilAppointedRepSpecTypeId.Value = "";
                this.hdnMilAppointedRepSpecId.Value = "";
            }

            string milRepSpecType = "";
            string milRepSpec = "";

            if (ddMilRepSpecType.SelectedValue != ListItems.GetOptionAll().Value)
            {
                milRepSpecType = ddMilRepSpecType.SelectedValue;
                this.hdnMilRepSpecTypeId.Value = this.ddMilRepSpecType.SelectedValue;

                if (ddMilRepSpec.SelectedValue != ListItems.GetOptionAll().Value)
                {
                    milRepSpec = ddMilRepSpec.SelectedValue;
                    this.hdnMilRepSpecId.Value = this.ddMilRepSpec.SelectedValue;
                }
                else
                {
                    this.hdnMilRepSpecId.Value = "";
                }
            }
            else
            {
                this.hdnMilRepSpecTypeId.Value = "";
                this.hdnMilRepSpecId.Value = "";
            }

            string positionTitle = "";

            if (ddPositionTitle.SelectedValue != ListItems.GetOptionAll().Value)
            {
                positionTitle = ddPositionTitle.SelectedValue;
                this.hdnPositionTitleId.Value = this.ddPositionTitle.SelectedValue;
            }
            else
            {
                this.hdnPositionTitleId.Value = "";
            }

            this.hdnIsPrimaryPositionTitle.Value = chkIsPrimaryPositionTitle.Checked ? "1" : "0";


            string administration = "";

            if (ddAdministration.SelectedValue != ListItems.GetOptionAll().Value)
            {
                administration = ddAdministration.SelectedValue;
                this.hdnAdministrationId.Value = this.ddAdministration.SelectedValue;
            }
            else
            {
                this.hdnAdministrationId.Value = "";
            }

            string language = "";

            if (ddLanguage.SelectedValue != ListItems.GetOptionAll().Value)
            {
                language = ddLanguage.SelectedValue;
                this.hdnLanguageId.Value = this.ddLanguage.SelectedValue;
            }
            else
            {
                this.hdnLanguageId.Value = "";
            }

            string education = "";

            if (ddEducation.SelectedValue != ListItems.GetOptionAll().Value)
            {
                education = ddEducation.SelectedValue;
                this.hdnEducationId.Value = this.ddEducation.SelectedValue;
            }
            else
            {
                this.hdnEducationId.Value = "";
            }

            string civilSpeciality = "";

            if (ddCivilSpeciality.SelectedValue != ListItems.GetOptionAll().Value)
            {
                civilSpeciality = ddCivilSpeciality.SelectedValue;
                hdnCivilSpecialityId.Value = this.ddCivilSpeciality.SelectedValue;
            }
            else
            {
                hdnCivilSpecialityId.Value = "";
            }

            bool isPermAddress;

            if (rblAddress.SelectedValue == "2")
                isPermAddress = true;
            else
                isPermAddress = false;

            this.hdnIsPermAddress.Value = (isPermAddress ? "1" : "0");

            string region = "";

            if (ddRegion.SelectedValue != ListItems.GetOptionAll().Value)
            {
                region = ddRegion.SelectedValue;
                this.hdnRegionId.Value = this.ddRegion.SelectedValue;
            }
            else
            {
                this.hdnRegionId.Value = "";
            }

            string municiplaity = "";

            if (ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                municiplaity = ddMuniciplaity.SelectedValue;
                this.hdnMunicipalityId.Value = this.ddMuniciplaity.SelectedValue;
            }
            else
            {
                this.hdnMunicipalityId.Value = "";
            }

            string city = "";

            if (ddCity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                city = ddCity.SelectedValue;
                this.hdnCityId.Value = this.ddCity.SelectedValue;
            }
            else
            {
                this.hdnCityId.Value = "";
            }

            string district = "";

            if (ddDistrict.SelectedValue != ListItems.GetOptionAll().Value)
            {
                district = ddDistrict.SelectedValue;
                this.hdnDistrictId.Value = this.ddDistrict.SelectedValue;
            }
            else
            {
                this.hdnDistrictId.Value = "";
            }

            hdnWorkCompany_UnifiedIdentityCode.Value = txtWorkCompany_UnifiedIdentityCode.Text;
            hdnWorkCompany_Name.Value = txtWorkCompany_Name.Text;
            hdnHasBeenOnMission.Value = chkHasBeenOnMission.Checked ? "1" : "0";

            string appointmentIsDelivered = "";

            if (ddAppointmentIsDelivered.SelectedValue != ListItems.GetOptionAll().Value)
            {
                appointmentIsDelivered = ddAppointmentIsDelivered.SelectedValue;
                this.hdnAppointmentIsDelivered.Value = this.ddAppointmentIsDelivered.SelectedValue;
            }
            else
            {
                this.hdnAppointmentIsDelivered.Value = "";
            }


            string isSuitableForMobAppointment = "";

            if (suitableForMobAppointmentDropDownList.SelectedValue != ListItems.GetOptionAll().Value)
            {
                isSuitableForMobAppointment = suitableForMobAppointmentDropDownList.SelectedValue;
                this.hdnSuitableForMobAppointment.Value = this.suitableForMobAppointmentDropDownList.SelectedValue;
            }
            else
            {
                this.hdnSuitableForMobAppointment.Value = "";
            }

            string readiness = "";

            if (ddReadiness.SelectedValue != ListItems.GetOptionAll().Value)
            {
                readiness = ddReadiness.SelectedValue;
                this.hdnReadiness.Value = this.ddReadiness.SelectedValue;
            }
            else
            {
                this.hdnReadiness.Value = "";
            }
           
            string professionId = "";
            if (this.ddProfession.SelectedValue != ListItems.GetOptionAll().Value)
            {
                professionId = this.ddProfession.SelectedValue;
                this.hdnProfessionId.Value = this.ddProfession.SelectedValue;
            }
            else
            {
                this.hdnProfessionId.Value = "";
            }

            string specialityId = "";
            if (this.ddSpeciality.SelectedValue != ListItems.GetOptionAll().Value)
            {
                specialityId = this.ddSpeciality.SelectedValue;
                this.hdnSpecialityId.Value = this.ddSpeciality.SelectedValue;
            }
            else
            {
                this.hdnSpecialityId.Value = "";
            }
         
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.FirstAndSurName = txtFirstAndSurName.Text;
            filter.FamilyName = txtFamilyName.Text;
            filter.Initials = txtInitials.Text;
            filter.IdentNumber = txtIdentNumber.Text;
            filter.MilitaryCategory = militaryCategory;
            filter.MilitaryRank = militaryRank;
            filter.MilitaryReportStatus = militaryReportStatus;
            filter.MilitaryCommand = txtMilitaryCommand.Text;
            filter.MilitaryDepartment = militaryDepartment;
            filter.Position = txtPosition.Text;
            filter.MilAppointedRepSpecType = milAppointedRepSpecType;
            filter.MilAppointedRepSpec = milAppointedRepSpec;
            filter.MilRepSpecType = milRepSpecType;
            filter.MilRepSpec = milRepSpec;
            filter.IsPrimaryMilRepSpec = chkIsPrimaryMilRepSpec.Checked;
            filter.PositionTitle = positionTitle;
            filter.IsPrimaryPositionTitle = chkIsPrimaryPositionTitle.Checked;
            filter.Administration = administration;
            filter.Language = language;
            filter.Education = education;
            filter.CivilSpeciality = civilSpeciality;
            filter.IsPermAddress = isPermAddress;
            filter.PostCode = txtPostCode.Text;
            filter.Region = region;
            filter.Municipality = municiplaity;
            filter.City = city;
            filter.District = district;
            filter.Address = txtAddress.Text;
            filter.WorkUnifiedIdentityCode = txtWorkCompany_UnifiedIdentityCode.Text;
            filter.WorkCompanyName = txtWorkCompany_Name.Text;
            filter.HasBeenOnMission = chkHasBeenOnMission.Checked;
            filter.AppointmentIsDelivered = appointmentIsDelivered;
            filter.IsSuitableForMobAppointment = isSuitableForMobAppointment;
            filter.Readiness = readiness;
            filter.ProfessionId = professionId;
            filter.SpecialityId = specialityId;
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFirstAndSurName.Text = "";
            txtFamilyName.Text = "";
            txtInitials.Text = "";
            txtIdentNumber.Text = "";
            txtMilitaryCommand.Text = "";
            txtPostCode.Text = "";
            txtPosition.Text = "";
            txtAddress.Text = "";
            ddMilitaryCategory.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryRank.Items.Clear();
            ddMilitaryReportStatus.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryDepartment.SelectedValue = ListItems.GetOptionAll().Value;
            ddAppointedMilRepSpecType.SelectedValue = ListItems.GetOptionAll().Value;
            ddAppointedMilRepSpec.Items.Clear();
            ddMilRepSpecType.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilRepSpec.Items.Clear();
            ddPositionTitle.SelectedValue = ListItems.GetOptionAll().Value;
            chkIsPrimaryPositionTitle.Checked = false;
            ddAdministration.SelectedValue = ListItems.GetOptionAll().Value;
            ddLanguage.SelectedValue = ListItems.GetOptionAll().Value;
            ddEducation.SelectedValue = ListItems.GetOptionAll().Value;
            ddCivilSpeciality.SelectedValue = ListItems.GetOptionAll().Value;
            rblAddress.SelectedValue = "1";
            ddRegion.SelectedValue = ListItems.GetOptionAll().Value;
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();
            txtWorkCompany_Name.Text = "";
            txtWorkCompany_UnifiedIdentityCode.Text = "";
            chkHasBeenOnMission.Checked = false;
            chkIsPrimaryMilRepSpec.Checked = false;
            ddAppointmentIsDelivered.SelectedValue = ListItems.GetOptionAll().Value;
            suitableForMobAppointmentDropDownList.SelectedValue = ListItems.GetOptionAll().Value;
            ddReadiness.SelectedValue = ListItems.GetOptionAll().Value;
            ddProfession.SelectedValue = ListItems.GetOptionAll().Value;
            ddProfession_Changed(sender, e);
            
            pnlPaging.Visible = false;
            pnlDataGrid.InnerHtml = "";
            pnlSearchHint.Visible = true;

            ClearFilterFromSession();

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Списък на водените на военен отчет'", CurrentUser, Request, postBackStart.Value);
        }

        private void LoadFilterFromSession()
        {
            if (Session["ManageReservists_Filter"] is ReservistManageFilter)
            {
                DateTime start = BenchmarkLog.WriteStart("\tНачало зареждане на филтъра от сесията", CurrentUser, Request);
                SetFilterData((ReservistManageFilter)Session["ManageReservists_Filter"]);
                BenchmarkLog.WriteEnd("\tКрай зареждане на филтъра от сесията", CurrentUser, Request, start);
            }
        }

        private void StoreFilterIntoSession(ReservistManageFilter filter)
        {
            Session["ManageReservists_Filter"] = filter;
        }

        private void ClearFilterFromSession()
        {
            Session["ManageReservists_Filter"] = null;
        }

        //Set the filter information into the page
        private void SetFilterData(ReservistManageFilter filter)
        {
            //Read the info from the filter object
            txtFirstAndSurName.Text = filter.FirstAndSurName;
            txtFamilyName.Text = filter.FamilyName;
            txtInitials.Text = filter.Initials;
            txtIdentNumber.Text = filter.IdentNumber;

            if (!String.IsNullOrEmpty(filter.MilitaryCategory))
            {
                ddMilitaryCategory.SelectedValue = filter.MilitaryCategory;
                ddMilitaryCategory_Changed(ddMilitaryCategory, new EventArgs());
            }

            if(!String.IsNullOrEmpty(filter.MilitaryRank))
            {
                ddMilitaryRank.SelectedValue = filter.MilitaryRank;
            }

            if (!String.IsNullOrEmpty(filter.MilitaryReportStatus))
            {
                ddMilitaryReportStatus.SelectedValue = filter.MilitaryReportStatus;
            }

            txtMilitaryCommand.Text = filter.MilitaryCommand;

            if (!String.IsNullOrEmpty(filter.MilitaryDepartment))
            {
                ddMilitaryDepartment.SelectedValue = filter.MilitaryDepartment;
            }

            txtPosition.Text = filter.Position;

            if (!String.IsNullOrEmpty(filter.MilAppointedRepSpecType))
            {
                ddAppointedMilRepSpecType.SelectedValue = filter.MilAppointedRepSpecType;
                ddAppointedMilRepSpecType_Changed(ddAppointedMilRepSpec, new EventArgs());
            }

            if (!String.IsNullOrEmpty(filter.MilAppointedRepSpec))
            {
                ddAppointedMilRepSpec.SelectedValue = filter.MilAppointedRepSpec;
            }

            if(!String.IsNullOrEmpty(filter.MilRepSpecType))
            {
                ddMilRepSpecType.SelectedValue = filter.MilRepSpecType;
                ddMilRepSpecType_Changed(ddMilRepSpecType, new EventArgs());
            }

            if (!String.IsNullOrEmpty(filter.MilRepSpec))
            {
                ddMilRepSpec.SelectedValue = filter.MilRepSpec;
            }

            chkIsPrimaryMilRepSpec.Checked = filter.IsPrimaryMilRepSpec;

            if (!String.IsNullOrEmpty(filter.PositionTitle))
            {
                ddPositionTitle.SelectedValue = filter.PositionTitle;
            }

            chkIsPrimaryPositionTitle.Checked = filter.IsPrimaryPositionTitle;

            if (!String.IsNullOrEmpty(filter.Administration))
            {
                ddAdministration.SelectedValue = filter.Administration;
            }

            if (!String.IsNullOrEmpty(filter.Language))
            {
                ddLanguage.SelectedValue = filter.Language;
            }

            if (!String.IsNullOrEmpty(filter.Education))
            {
                ddEducation.SelectedValue = filter.Education;
            }

            if (!String.IsNullOrEmpty(filter.CivilSpeciality))
            {
                ddCivilSpeciality.SelectedValue = filter.CivilSpeciality;
            }

            rblAddress.SelectedValue = filter.IsPermAddress ? "2" : "1";

            txtPostCode.Text = filter.PostCode;

            if (!String.IsNullOrEmpty(filter.Region))
            {
                ddRegion.SelectedValue = filter.Region;
                ddRegion_Changed(ddRegion, new EventArgs());
            }

            if (!String.IsNullOrEmpty(filter.Municipality))
            {
                ddMuniciplaity.SelectedValue = filter.Municipality;
                ddMuniciplaity_Changed(ddMuniciplaity, new EventArgs());
            }

            if (!String.IsNullOrEmpty(filter.City))
            {
                ddCity.SelectedValue = filter.City;
                ddCity_Changed(ddCity, new EventArgs());
            }

            if (!String.IsNullOrEmpty(filter.District))
            {
                ddDistrict.SelectedValue = filter.District;
            }

            txtAddress.Text = filter.Address;
            txtWorkCompany_UnifiedIdentityCode.Text = filter.WorkUnifiedIdentityCode;
            txtWorkCompany_Name.Text = filter.WorkCompanyName;
            chkHasBeenOnMission.Checked = filter.HasBeenOnMission;

            if (!String.IsNullOrEmpty(filter.AppointmentIsDelivered))
            {
                ddAppointmentIsDelivered.SelectedValue = filter.AppointmentIsDelivered;
            }

            if (!String.IsNullOrEmpty(filter.IsSuitableForMobAppointment))
            {
                 suitableForMobAppointmentDropDownList.SelectedValue = filter.IsSuitableForMobAppointment;
            }

            if (!String.IsNullOrEmpty(filter.Readiness))
            {
                ddReadiness.SelectedValue = filter.Readiness;
            }
            
            if (!String.IsNullOrEmpty(filter.ProfessionId.ToString()))
            {
                ddProfession.SelectedValue = filter.ProfessionId.ToString();
                ddProfession_Changed(new Object(), new EventArgs());
            }

            if (!String.IsNullOrEmpty(filter.SpecialityId.ToString()))
            {
                ddSpeciality.SelectedValue = filter.SpecialityId;
            }
         
            hdnSortBy.Value = filter.OrderBy.ToString();
            hdnPageIdx.Value = filter.PageIdx.ToString();

            //Move the info from the inputs and put it into the hidden fields (like into the CollectFilterData() method)
            this.hdnFirstAndSurName.Value = txtFirstAndSurName.Text;
            this.hdnFamilyName.Value = txtFamilyName.Text;
            this.hdnInitials.Value = txtInitials.Text;
            this.hdnIdentNumber.Value = txtIdentNumber.Text;

            this.hdnMilitaryCommand.Value = txtMilitaryCommand.Text;
            this.hdnPostCode.Value = txtPostCode.Text;
            this.hdnPosition.Value = txtPosition.Text;
            this.hdnAddress.Value = txtAddress.Text;

            if (ddMilitaryCategory.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnMilitaryCategoryId.Value = this.ddMilitaryCategory.SelectedValue;
            }
            else
            {
                this.hdnMilitaryCategoryId.Value = "";
            }

            if (ddMilitaryRank.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnMilitaryRankId.Value = this.ddMilitaryRank.SelectedValue;
            }
            else
            {
                this.hdnMilitaryRankId.Value = "";
            }

            if (ddMilitaryReportStatus.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnMilitaryReportStatusId.Value = this.ddMilitaryReportStatus.SelectedValue;
            }
            else
            {
                this.hdnMilitaryReportStatusId.Value = "";
            }

            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnMilitaryDepartmentId.Value = this.ddMilitaryDepartment.SelectedValue;
            }
            else
            {
                this.hdnMilitaryDepartmentId.Value = "";
            }

            if (ddAppointedMilRepSpecType.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnMilAppointedRepSpecTypeId.Value = this.ddAppointedMilRepSpecType.SelectedValue;

                if (ddAppointedMilRepSpec.SelectedValue != ListItems.GetOptionAll().Value)
                {
                    this.hdnMilAppointedRepSpecId.Value = this.ddAppointedMilRepSpec.SelectedValue;
                }
                else
                {
                    this.hdnMilAppointedRepSpecId.Value = "";
                }
            }
            else
            {
                this.hdnMilAppointedRepSpecTypeId.Value = "";
                this.hdnMilAppointedRepSpecId.Value = "";
            }

            if (ddMilRepSpecType.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnMilRepSpecTypeId.Value = this.ddMilRepSpecType.SelectedValue;

                if (ddMilRepSpec.SelectedValue != ListItems.GetOptionAll().Value)
                {
                    this.hdnMilRepSpecId.Value = this.ddMilRepSpec.SelectedValue;
                }
                else
                {
                    this.hdnMilRepSpecId.Value = "";
                }
            }
            else
            {
                this.hdnMilRepSpecTypeId.Value = "";
                this.hdnMilRepSpecId.Value = "";
            }

            if (ddAdministration.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnAdministrationId.Value = this.ddAdministration.SelectedValue;
            }
            else
            {
                this.hdnAdministrationId.Value = "";
            }

            if (ddLanguage.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnLanguageId.Value = this.ddLanguage.SelectedValue;
            }
            else
            {
                this.hdnLanguageId.Value = "";
            }

            if (ddEducation.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnEducationId.Value = this.ddEducation.SelectedValue;
            }
            else
            {
                this.hdnEducationId.Value = "";
            }

            if (ddCivilSpeciality.SelectedValue != ListItems.GetOptionAll().Value)
            {
                hdnCivilSpecialityId.Value = this.ddCivilSpeciality.SelectedValue;
            }
            else
            {
                hdnCivilSpecialityId.Value = "";
            }

            bool isPermAddress;

            if (rblAddress.SelectedValue == "2")
                isPermAddress = true;
            else
                isPermAddress = false;

            this.hdnIsPermAddress.Value = (isPermAddress ? "1" : "0");


            if (ddRegion.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnRegionId.Value = this.ddRegion.SelectedValue;
            }
            else
            {
                this.hdnRegionId.Value = "";
            }

            if (ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnMunicipalityId.Value = this.ddMuniciplaity.SelectedValue;
            }
            else
            {
                this.hdnMunicipalityId.Value = "";
            }

            if (ddCity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnCityId.Value = this.ddCity.SelectedValue;
            }
            else
            {
                this.hdnCityId.Value = "";
            }

            if (ddDistrict.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnDistrictId.Value = this.ddDistrict.SelectedValue;
            }
            else
            {
                this.hdnDistrictId.Value = "";
            }

            hdnWorkCompany_UnifiedIdentityCode.Value = txtWorkCompany_UnifiedIdentityCode.Text;
            hdnWorkCompany_Name.Value = txtWorkCompany_Name.Text;
            hdnHasBeenOnMission.Value = chkHasBeenOnMission.Checked ? "1" : "0";

            if (ddAppointmentIsDelivered.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnAppointmentIsDelivered.Value = this.ddAppointmentIsDelivered.SelectedValue;
            }
            else
            {
                this.hdnAppointmentIsDelivered.Value = "";
            }

            if ( suitableForMobAppointmentDropDownList.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnSuitableForMobAppointment.Value = this.suitableForMobAppointmentDropDownList.SelectedValue;
            }
            else
            {
                this.hdnSuitableForMobAppointment.Value = "";
            }

            if (ddReadiness.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnReadiness.Value = this.ddReadiness.SelectedValue;
            }
            else
            {
                this.hdnReadiness.Value = "";
            }

            if (ddProfession.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnProfessionId.Value = this.ddProfession.SelectedValue;
            }
            else
            {
                this.hdnProfessionId.Value = "";
            }

            if (ddSpeciality.SelectedValue != ListItems.GetOptionAll().Value)
            {
                this.hdnSpecialityId.Value = this.ddSpeciality.SelectedValue;
            }
            else
            {
                this.hdnSpecialityId.Value = "";
            }            
        }

        private void JSWriteBenchmarkLog()
        {
            string stat = "";
            string response = "";

            try
            {
                string msg = Request.Form["Message"];

                BenchmarkLog.Write(msg, CurrentUser, Request);

                response = @"<response>OK</response>";

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        private bool IsThereFilter()
        {
            bool isThereFilter = false;

            if (!isThereFilter && !String.IsNullOrEmpty(txtFirstAndSurName.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtFamilyName.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtInitials.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtIdentNumber.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtMilitaryCommand.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtPostCode.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtPosition.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtAddress.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && ddMilitaryCategory.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddMilitaryCategory.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddMilitaryRank.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddMilitaryRank.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddMilitaryReportStatus.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddMilitaryReportStatus.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddMilitaryDepartment.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddAppointedMilRepSpecType.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddAppointedMilRepSpecType.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddAppointedMilRepSpec.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddAppointedMilRepSpec.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddMilRepSpecType.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddMilRepSpecType.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddMilRepSpec.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddMilRepSpec.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddPositionTitle.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddPositionTitle.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddAdministration.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddAdministration.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddLanguage.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddLanguage.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddEducation.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddEducation.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddCivilSpeciality.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddCivilSpeciality.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddRegion.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddRegion.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddMuniciplaity.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddCity.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddCity.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddDistrict.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddDistrict.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtWorkCompany_UnifiedIdentityCode.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && !String.IsNullOrEmpty(txtWorkCompany_Name.Text.Trim()))
                isThereFilter = true;

            if (!isThereFilter && chkHasBeenOnMission.Checked)
                isThereFilter = true;

            if (!isThereFilter && ddAppointmentIsDelivered.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddAppointmentIsDelivered.SelectedValue))
                isThereFilter = true;

            if (!isThereFilter && ddReadiness.SelectedValue != ListItems.GetOptionAll().Value &&
                !String.IsNullOrEmpty(ddReadiness.SelectedValue))
                isThereFilter = true;

            return isThereFilter;
        }
    }
}
