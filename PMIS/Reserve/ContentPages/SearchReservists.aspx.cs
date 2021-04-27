using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class SearchReservists : RESPage
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
                return "RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL";
            }
        }

        //This property represents the ID of the EquipmentReservistsRequest object that is loaded on the screen
        //It is stored in a hidden field on the page
        private int EquipmentReservistsRequestId
        {
            get
            {
                int equipmentReservistsRequestId = 0;
                if (String.IsNullOrEmpty(this.hfEquipmentReservistsRequestID.Value)
                    || this.hfEquipmentReservistsRequestID.Value == "0")
                {
                    if (Request.Params["EquipmentReservistsRequestId"] != null)
                        int.TryParse(Request.Params["EquipmentReservistsRequestId"].ToString(), out equipmentReservistsRequestId);

                    this.hfEquipmentReservistsRequestID.Value = equipmentReservistsRequestId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfEquipmentReservistsRequestID.Value, out equipmentReservistsRequestId);
                }

                return equipmentReservistsRequestId;
            }

            set
            {
                this.hfEquipmentReservistsRequestID.Value = value.ToString();
            }
        }

        //This property represents the ID of the MilitaryDepartment object that is loaded on the screen
        //It is stored in a hidden field on the page
        private int MilitaryDepartmentId
        {
            get
            {
                int militaryDepartmentId = 0;
                if (String.IsNullOrEmpty(this.hfMilitaryDepartmentID.Value)
                    || this.hfMilitaryDepartmentID.Value == "0")
                {
                    if (Request.Params["MilitaryDepartmentId"] != null)
                        int.TryParse(Request.Params["MilitaryDepartmentId"].ToString(), out militaryDepartmentId);

                    this.hfMilitaryDepartmentID.Value = militaryDepartmentId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfMilitaryDepartmentID.Value, out militaryDepartmentId);
                }

                return militaryDepartmentId;
            }

            set
            {
                this.hfMilitaryDepartmentID.Value = value.ToString();
            }
        }

        //This property represents the ID of the MilitaryCommand object for which is the selection
        //It is stored in a hidden field on the page
        private int MilitaryCommandID
        {
            get
            {
                int militaryCommandID = 0;
                if (String.IsNullOrEmpty(this.hfMilitaryCommandID.Value)
                    || this.hfMilitaryCommandID.Value == "0")
                {
                    if (Request.Params["MilitaryCommandID"] != null)
                        int.TryParse(Request.Params["MilitaryCommandID"].ToString(), out militaryCommandID);

                    this.hfMilitaryCommandID.Value = militaryCommandID.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfMilitaryCommandID.Value, out militaryCommandID);
                }

                return militaryCommandID;
            }

            set
            {
                this.hfMilitaryCommandID.Value = value.ToString();
            }
        }

        //This property represents the ID of the RequestCommandPositionID object that is loaded on the screen
        //It is stored in a hidden field on the page
        private int RequestCommandPositionID
        {
            get
            {
                int requestCommandPositionId = 0;
                if (String.IsNullOrEmpty(this.hfRequestCommandPositionID.Value)
                    || this.hfRequestCommandPositionID.Value == "0")
                {
                    if (Request.Params["RequestCommandPositionID"] != null)
                        int.TryParse(Request.Params["RequestCommandPositionID"].ToString(), out requestCommandPositionId);

                    this.hfRequestCommandPositionID.Value = requestCommandPositionId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfRequestCommandPositionID.Value, out requestCommandPositionId);
                }

                return requestCommandPositionId;
            }

            set
            {
                this.hfRequestCommandPositionID.Value = value.ToString();
            }
        }

        //This property represents the Readiness(base or additional) of reservist to be choosen
        //It is stored in a hidden field on the page
        private int Readiness
        {
            get
            {
                int readiness = 0;
                if (String.IsNullOrEmpty(this.hfReadiness.Value)
                    || this.hfReadiness.Value == "0")
                {
                    if (Request.Params["Readiness"] != null)
                        int.TryParse(Request.Params["Readiness"].ToString(), out readiness);

                    this.hfReadiness.Value = readiness.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfReadiness.Value, out readiness);
                }

                return readiness;
            }

            set
            {
                this.hfReadiness.Value = value.ToString();
            }
        }

        //This property represents the FromFulfilByCommand flag indicating that the opener screen is By Command
        //It is stored in a hidden field on the page
        private int FromFulfilByCommand
        {
            get
            {
                int fromFulfilByCommand = 0;
                if (String.IsNullOrEmpty(this.hfFromFulfilByCommand.Value)
                    || this.hfFromFulfilByCommand.Value == "0")
                {
                    if (Request.Params["FromFulfilByCommand"] != null)
                        int.TryParse(Request.Params["FromFulfilByCommand"].ToString(), out fromFulfilByCommand);

                    this.hfFromFulfilByCommand.Value = fromFulfilByCommand.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfFromFulfilByCommand.Value, out fromFulfilByCommand);
                }

                return fromFulfilByCommand;
            }

            set
            {
                this.hfFromFulfilByCommand.Value = value.ToString();
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
            if (!IsPostBack)
                pageStart = BenchmarkLog.WriteStart("Отваряне на екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request);

            if (IsPostBack)
                postBackStart = BenchmarkLog.WriteStart("PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request);

            //Redirect if the access is denied
            if ((GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") != UIAccessLevel.Enabled) ||
                (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH") != UIAccessLevel.Enabled))
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSChooseReservist")
            {
                JSChooseReservist();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Equipment");            

            //Setup some basic styles on the screen
            SetupStyle();

            lblWorkCompany_UnifiedIdentityCode.Text = CommonFunctions.GetLabelText("UnifiedIdentityCode") + ":";

            //Setup hidden fields
            EquipmentReservistsRequestId = EquipmentReservistsRequestId;
            MilitaryDepartmentId = MilitaryDepartmentId;
            MilitaryCommandID = MilitaryCommandID;
            RequestCommandPositionID = RequestCommandPositionID;
            Readiness = Readiness;

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate page header title
                SetPageTitle();

                //Populate any drop-downs and list-boxes
                DateTime listsStart = BenchmarkLog.WriteStart("\tНачало на зареждане на класификаторите", CurrentUser, Request);
                PopulateLists();
                BenchmarkLog.WriteEnd("\tКрай на зареждане на класификаторите", CurrentUser, Request, listsStart);

                //Pre-fill the filter based on the selected request command position
                PreFillFilter();
                PopulateSelectMilRepSpec();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());
            }
            else
            {
                //Re-load the options for this list (used in the light-box) each time when a post-back happens. Othwersie ASP.NET doesn't remember the options attributes in ViewStage.
                //As a result, the option attributes like colors and titles are lost...
                PopulateSelectMilRepSpec();
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

            if (pageStart.HasValue)
                BenchmarkLog.WriteEnd("Край на зареждане на екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, pageStart.Value);
        }

        private void RefreshMaxPage()
        {
            if (!loadedMaxPage)
            {
                DateTime start = BenchmarkLog.WriteStart("\tНачало на изчисляване на общия брой страници", CurrentUser, Request);

                //Collect the filter information to be able to pull the number of rows for this specific filter
                ReservistSearchFilter filter = CollectFilterData();

                int allRows = ReservistUtil.GetAllReservistSearchBlocksCount(filter, RequestCommandPositionID, MilitaryDepartmentId, CurrentUser);
                //Get the number of rows and calculate the number of pages in the grid
                maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

                loadedMaxPage = true;

                BenchmarkLog.WriteEnd("\tКрай на изчисляване на общия брой страници", CurrentUser, Request, start);
            }
        }

        private void SetPageTitle()
        {
            lblHeaderTitle.InnerHtml = "Търсене на резервисти за попълване на заявка №";

            EquipmentReservistsRequest request = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(EquipmentReservistsRequestId, CurrentUser);

            if (request != null)
            {
                lblHeaderTitle.InnerHtml += request.RequestNumber + " / " + CommonFunctions.FormatDate(request.RequestDate);
            }

            RequestCommandPosition position = RequestCommandPositionUtil.GetRequestCommandPosition(CurrentUser, RequestCommandPositionID);

            if (position != null)
            {
                lblSubHeaderTitle.InnerHtml = "Длъжност '" + position.Position + "' (" + ReadinessUtil.ReadinessName(Readiness) + ")";
            }
        }

        private void PreFillFilter()
        {
            RequestCommandPosition position = RequestCommandPositionUtil.GetRequestCommandPosition(CurrentUser, RequestCommandPositionID);

            if (position != null)
            {
                if (position.MilitaryReportSpecialities.Count > 0)
                {
                    MilitaryReportSpeciality milRepSpeciality = position.MilitaryReportSpecialities[0].Speciality;

                    ddMilRepSpecType.SelectedValue = milRepSpeciality.MilReportSpecialityTypeId.ToString();
                    ddMilRepSpecType_Changed(Page, new EventArgs());
                    ddMilRepSpec.SelectedValue = milRepSpeciality.MilReportSpecialityId.ToString();
                }

                //TODO pre-populate
                if (position.MilitaryRanks != null && position.MilitaryRanks.Count >= 0)
                {
                    ddMilitaryRank.SelectedValue = position.MilitaryRanks[0].Rank.MilitaryRankId.ToString();
                }
            }
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            DateTime start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Тип ВОС'", CurrentUser, Request);
            PopulateMilRepSpecType();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Тип ВОС'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Длъжност'", CurrentUser, Request);
            PopulatePositionTitle();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Длъжност'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Военно звание'", CurrentUser, Request);
            PopulateMilitaryRank();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Военно звание'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Гражданска специалност'", CurrentUser, Request);
            PopulateCivilSpeciality();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Гражданска специалност'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Чужд език'", CurrentUser, Request);
            PopulateLanguage();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Чужд език'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Област'", CurrentUser, Request);
            PopulateRegions();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Област'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Образование'", CurrentUser, Request);
            PopulateEducation();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Образование'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Военна подготовка'", CurrentUser, Request);
            PopulateMilitaryTraining();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Военна подготовка'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало списък 'Подходящ за МН'", CurrentUser, Request);
            PopulateIsSuitableForMobAppointment();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Подходящ за МН'", CurrentUser, Request, start);
        }

        //Populate ddMilRepSpecType
        private void PopulateMilRepSpecType()
        {            
            ddMilRepSpecType.DataSource = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(CurrentUser);
            ddMilRepSpecType.DataTextField = "TypeName";
            ddMilRepSpecType.DataValueField = "Type";
            ddMilRepSpecType.DataBind();
            ddMilRepSpecType.Items.Insert(0, new ListItem("Неактивни", "-2"));
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
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

        //Populate ddMilitaryRank
        private void PopulateMilitaryRank()
        {
            string currentVal = ddMilitaryRank.SelectedValue;

            var milRanks = MilitaryRankUtil.GetAllMilitaryRanks(CurrentUser);

            if (!chkShowAllMilitaryRanks.Checked)
            {
                RequestCommandPosition position = RequestCommandPositionUtil.GetRequestCommandPosition(CurrentUser, RequestCommandPositionID);
                if (position != null && position.MilitaryRanks != null && position.MilitaryRanks.Count >= 0)
                    milRanks = milRanks.Where(x => position.MilitaryRanks.Any(y => y.Rank.MilitaryRankId == x.MilitaryRankId)).ToList();
            }

            ddMilitaryRank.DataSource = milRanks;
            ddMilitaryRank.DataTextField = "LongName";
            ddMilitaryRank.DataValueField = "MilitaryRankId";
            ddMilitaryRank.DataBind();
            ddMilitaryRank.Items.Insert(0, ListItems.GetOptionAll());

            if (milRanks.Any(x => x.MilitaryRankId == currentVal))
                ddMilitaryRank.SelectedValue = currentVal;
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

        //Populate ddLanguage
        private void PopulateLanguage()
        {
            ddLanguage.DataSource = PersonLanguageUtil.GetAllPersonLanguages(CurrentUser);
            ddLanguage.DataTextField = "PersonLanguageName";
            ddLanguage.DataValueField = "PersonLanguageCode";
            ddLanguage.DataBind();
            ddLanguage.Items.Insert(0, ListItems.GetOptionAll());
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

        //Populate ddSelectMilRepSpec
        private void PopulateSelectMilRepSpec()
        {
            ddSelectMilRepSpec.Items.Clear();
            RequestCommandPosition position = RequestCommandPositionUtil.GetRequestCommandPosition(CurrentUser, RequestCommandPositionID);

            foreach (CommandPositionMilitaryReportSpeciality speciality in position.MilitaryReportSpecialities)
            {
                ListItem li = new ListItem();
                li.Text = speciality.Speciality.CodeAndName;
                li.Value = speciality.Speciality.MilReportSpecialityId.ToString();
                li.Attributes.Add("class", speciality.IsPrimary ? "PrimaryMRSOption" : "NonPrimaryMRSOption");

                ddSelectMilRepSpec.Items.Add(li);
            }

            if(position.MilitaryReportSpecialities.Count == 0)
                ddSelectMilRepSpec.Items.Insert(0, ListItems.GetOptionChooseOne());

            CommonFunctions.SetDropDownTooltip(ddSelectMilRepSpec);
        }

        //Populate isSuitableForMobAppoitmentDropDownList
        private void PopulateIsSuitableForMobAppointment()
        {
            isSuitableForMobAppoitmentDropDownList.Items.Add(ListItems.GetOptionAll());
            isSuitableForMobAppoitmentDropDownList.Items.Add(ListItems.GetOptionYes());
            isSuitableForMobAppoitmentDropDownList.Items.Add(ListItems.GetOptionNo());
        }

        protected void ddRegion_Changed(object sender, EventArgs e)
        {
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();

            if (ddRegion.SelectedValue != "-1")
            {
                int regionId = int.Parse(ddRegion.SelectedValue);

                PopulateMunicipalities(regionId);
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
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

            if (ddMuniciplaity.SelectedValue != "-1")
            {
                int municiplaityId = int.Parse(ddMuniciplaity.SelectedValue);

                PopulateCities(municiplaityId);
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
        }

        //Populate ddCity
        private void PopulateCities(int municipalityID)
        {
            ddCity.DataSource = CityUtil.GetCities(municipalityID, CurrentUser);
            ddCity.DataTextField = "CityName";
            ddCity.DataValueField = "CityId";
            ddCity.DataBind();
            ddCity.Items.Insert(0, ListItems.GetOptionAll());
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

        //Populate ddMilitaryTraining
        private void PopulateMilitaryTraining()
        {
            ddMilitaryTraining.Items.Add(new ListItem("С военна подготовка", "1"));
            ddMilitaryTraining.Items.Add(new ListItem("Без военна подготовка", "2"));
            
            ddMilitaryTraining.Items.Insert(0, ListItems.GetOptionAll());
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

            if (txtAge.Text != "" && !CommonFunctions.IsValidInt(txtAge.Text))
            {
                isDataValid = false;
                errMsg = CommonFunctions.GetErrorMessageNumber("Възраст до");
            }

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

            //pnlPaging.Visible = true;

            string html = "";

            bool IsMilRepSpecHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_MILREPSPEC") == UIAccessLevel.Hidden;
            bool IsMilRankHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_MILRANK") == UIAccessLevel.Hidden;
            bool IsEducationHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_EDUCATION") == UIAccessLevel.Hidden;
            bool IsAgeHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_AGE") == UIAccessLevel.Hidden;
            bool IsCityHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_CITY") == UIAccessLevel.Hidden;
            bool IsLanguageHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_LANGUAGE") == UIAccessLevel.Hidden;
            bool IsPositionTitleHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_POSITIONTITLE") == UIAccessLevel.Hidden;
            bool IsSpecialityHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_SPECIALITY") == UIAccessLevel.Hidden;
            bool IsMilTrainHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH_MILTRAIN") == UIAccessLevel.Hidden;            

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReservistSearchFilter filter = CollectFilterData();            

            //Get the list of records according to the specified filters, order and paging
            DateTime startSQL = BenchmarkLog.WriteStart("\t\tНачало извличане на данните от базата данни", CurrentUser, Request);
            List<ReservistSearchBlock> reservists = ReservistUtil.GetAllReservistSearchBlocks(filter, RequestCommandPositionID, MilitaryDepartmentId, pageLength, CurrentUser);
            BenchmarkLog.WriteEnd("\t\tКрай извличане на данните от базата данни", CurrentUser, Request, startSQL);

            //No data found
            if (reservists.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.pnlPaging.Attributes["class"] = "";

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "", "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>ЕГН" + arrOrderCol[0] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Име и презиме" + arrOrderCol[1] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(11);'>Фамилия" + arrOrderCol[10] + @"</th>" +
      (!IsMilRepSpecHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>ВОС" + arrOrderCol[2] + @"</th>" : "") +
         (!IsMilRankHidden ? @"<th style='width: 250px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Военно звание" + arrOrderCol[3] + @"</th>" : "") +
       (!IsEducationHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Образование" + arrOrderCol[4] + @"</th>" : "") +
             (!IsAgeHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Възраст" + arrOrderCol[5] + @"</th>" : "") +
            (!IsCityHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Населено място" + arrOrderCol[6] + @"</th>" : "") +
        /*Because there is not enought place for all columns on the screen, that is why we are replacing the Languages column with the new PositionTitle one*/
        /*(!IsLanguageHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(8);'>Чужди езици" + arrOrderCol[7] + @"</th>" : "") +*/
   (!IsPositionTitleHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(12);'>Подходяща длъжност" + arrOrderCol[11] + @"</th>" : "") +
      (!IsSpecialityHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(9);'>Специалност" + arrOrderCol[8] + @"</th>" : "") +
        (!IsMilTrainHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(10);'>Военна подготовка" + arrOrderCol[9] + @"</th>" : "") +
                            @"</tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReservistSearchBlock reservist in reservists)
                {
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' onmouseover=""this.className = 'SelectionItem';"" onmouseout=""this.className = '" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"';"" title='Избери'>
                                 <td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/user.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewReservist(" + reservist.ReservistID.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.IdentNumber + @"</td>
                                 <td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.FirstAndSurName + @"</td>
                                 <td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.FamilyName + @"</td>" +
        (!IsMilRepSpecHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.MilRepSpecHTML + @"</td>" : "") +
           (!IsMilRankHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.MilitaryRankName + @"</td>" : "") +
         (!IsEducationHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.Education + @"</td>" : "") +
               (!IsAgeHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.Age + @"</td>" : "") +
              (!IsCityHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.RegionMunicipalityAndCity + @"</td>" : "") +
          /*(!IsLanguageHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.Languages + @"</td>" : "") +*/
     (!IsPositionTitleHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.PositionTitle + @"</td>" : "") +
        (!IsSpecialityHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.Specialities + @"</td>" : "") +
          (!IsMilTrainHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowSelectMilRepSpecLightBox(" + reservist.ReservistID + @");"">" + reservist.MilitaryTrainingText + @"</td>" : "") +
                              @"</tr>";

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

        protected void chkShowAllMilitaryRanks_Check(object sender, EventArgs e)
        {
            PopulateMilitaryRank();
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.pnlSearchHint.Attributes["class"] = "HiddenPnl";

            if (ValidateData())
            {
                RefreshMaxPage();

                hdnPageIdx.Value = "1";
                RefreshReservists();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Търсене на резервисти за попълване на заявка'", CurrentUser, Request, postBackStart.Value);

            if (FromFulfilByCommand == 1)
            {
                Response.Redirect("~/ContentPages/FulfilReservistsMilCommand.aspx?MilitaryDepartmentId=" + MilitaryDepartmentId +
                                                                                "&MilitaryCommandID=" + MilitaryCommandID);
            }
            else
            {
                Response.Redirect("~/ContentPages/FulfilEquipmentReservistsRequest.aspx?EquipmentReservistsRequestId=" + EquipmentReservistsRequestId +
                                                                                        "&MilitaryDepartmentId=" + MilitaryDepartmentId +
                                                                                        "&MilitaryCommandID=" + MilitaryCommandID);
            }
        }

        //Collect the filet information from the page
        private ReservistSearchFilter CollectFilterData()
        {
            ReservistSearchFilter filter = new ReservistSearchFilter();

            string milRepSpecType = "";
            string milRepSpec = "";

            if (ddMilRepSpecType.SelectedValue != ListItems.GetOptionAll().Value)
            {
                milRepSpecType = ddMilRepSpecType.SelectedValue;

                if (ddMilRepSpec.SelectedValue != ListItems.GetOptionAll().Value)
                    milRepSpec = ddMilRepSpec.SelectedValue;
            }

            string positionTitle = "";

            if (ddPositionTitle.SelectedValue != ListItems.GetOptionAll().Value)
            {
                positionTitle = ddPositionTitle.SelectedValue;
            }

            string militaryRank = "";

            if (ddMilitaryRank.SelectedValue != ListItems.GetOptionAll().Value)
                militaryRank = ddMilitaryRank.SelectedValue;

            string civilSpeciality = "";

            if (ddCivilSpeciality.SelectedValue != ListItems.GetOptionAll().Value)
                civilSpeciality = ddCivilSpeciality.SelectedValue;

            int ageFake;
            int? age;

            if (int.TryParse(txtAge.Text, out ageFake))
                age = ageFake;
            else
                age = null;

            string language = "";

            if (ddLanguage.SelectedValue != ListItems.GetOptionAll().Value)
                language = ddLanguage.SelectedValue;

            string region = "";

            if (ddRegion.SelectedValue != ListItems.GetOptionAll().Value)
                region = ddRegion.SelectedValue;

            string municiplaity = "";

            if (ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value)
                municiplaity = ddMuniciplaity.SelectedValue;

            string city = "";

            if (ddCity.SelectedValue != ListItems.GetOptionAll().Value)
                city = ddCity.SelectedValue;

            string education = "";

            if (ddEducation.SelectedValue != ListItems.GetOptionAll().Value)
                education = ddEducation.SelectedValue;

            string militaryTraining = "";

            if (ddMilitaryTraining.SelectedValue != ListItems.GetOptionAll().Value)
                militaryTraining = ddMilitaryTraining.SelectedValue;

            string isSuitableForMobAppointment = "";

            if (isSuitableForMobAppoitmentDropDownList.SelectedValue != ListItems.GetOptionAll().Value)
            {
                isSuitableForMobAppointment = isSuitableForMobAppoitmentDropDownList.SelectedValue;
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
            filter.MilRepSpecType = milRepSpecType;
            filter.MilRepSpec = milRepSpec;
            filter.IsPrimaryMilRepSpec = chkIsPrimaryMilRepSpec.Checked;
            filter.PositionTitle = positionTitle;
            filter.IsPrimaryPositionTitle = chkIsPrimaryPositionTitle.Checked;
            filter.MobilAppointmentPosition = txtMobilAppointmentPosition.Text;
            filter.MilitaryRank = militaryRank;
            filter.CivilSpeciality = civilSpeciality;
            filter.Age = age;
            filter.Language = language;
            filter.Region = region;
            filter.Municipality = municiplaity;
            filter.City = city;
            filter.WorkPosition = txtWorkPosition.Text;
            filter.Education = education;
            filter.MilitaryTraining = militaryTraining;
            filter.WorkUnifiedIdentityCode = txtWorkCompany_UnifiedIdentityCode.Text;
            filter.WorkCompanyName = txtWorkCompany_Name.Text;
            filter.OrderBy = orderBy;
            filter.IsSuitableForMobAppointment = isSuitableForMobAppointment;
            filter.PageIdx = pageIdx;

            return filter;
        }

        private void JSChooseReservist()
        {
            string stat = "";
            string response = "";
            
            int reservistID = int.Parse(Request.Form["ReservistID"]);
            int requestCommandPositionID = int.Parse(Request.Form["RequestCommandPositionID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);
            int readiness = int.Parse(Request.Form["Readiness"]);
            int milReportSpecialityID = int.Parse(Request.Form["MilReportSpecialityID"]);
            bool needCourse = int.Parse(Request.Form["NeedCourse"]) == 1;

            FillReservistsRequest fillReservistsRequest = new FillReservistsRequest(CurrentUser);
            fillReservistsRequest.ReservistID = reservistID;
            fillReservistsRequest.RequestCommandPositionID = requestCommandPositionID;
            fillReservistsRequest.MilitaryDepartmentID = militaryDepartmentID;
            fillReservistsRequest.ReservistReadinessID = readiness;
            fillReservistsRequest.MilReportSpecialityID = milReportSpecialityID;
            fillReservistsRequest.NeedCourse = needCourse;

            try
            {
                List<FillReservistsRequest> existingFulfillments = FillReservistsRequestUtil.GetAllFillReservistsRequestByReservist(reservistID, CurrentUser);

                if (existingFulfillments.Count == 0)
                {
                    Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                    FillReservistsRequestUtil.SaveRequestCommandReservist(fillReservistsRequest, CurrentUser, change);

                    //Change the current Military Reporting Status of the chosen reservist
                    ReservistMilRepStatusUtil.SetMilRepStatusTo_COMPULSORY_RESERVE_MOB_APPOINTMENT(reservistID, CurrentUser, change);

                    //Add a new ReservistAppointment for the chosen reservist
                    RequestCommandPosition requestCommandPosition = RequestCommandPositionUtil.GetRequestCommandPosition(CurrentUser, fillReservistsRequest.RequestCommandPositionID);

                    /*
                    MilitaryReportSpeciality firstMRS = null;

                    if (requestCommandPosition.MilitaryReportSpecialities.Count > 0)
                        firstMRS = requestCommandPosition.MilitaryReportSpecialities[0];
                    */

                    MilitaryReportSpeciality fulfilMRS = fillReservistsRequest.MilitaryReportSpeciality;

                    ReservistAppointment reservistAppointment = new ReservistAppointment(CurrentUser);

                    reservistAppointment.ReservistId = fillReservistsRequest.ReservistID;
                    reservistAppointment.IsCurrent = true;
                    reservistAppointment.ReqOrderNumber = requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.RequestNumber;
                    reservistAppointment.EquipmentReservistsRequestId = requestCommandPosition.RequestsCommand.EquipmentReservistsRequestId;
                    reservistAppointment.ReceiptAppointmentDate = DateTime.Now; //TODO? Should we change this?
                    reservistAppointment.MilitaryCommandName = requestCommandPosition.RequestsCommand.MilitaryCommand.DisplayTextForSelection;
                    reservistAppointment.MilitaryCommandSuffix = requestCommandPosition.RequestsCommand.MilitaryCommandSuffix;
                    reservistAppointment.MilitaryCommandId = requestCommandPosition.RequestsCommand.MilitaryCommand.MilitaryCommandId;
                    reservistAppointment.ReservistReadinessId = fillReservistsRequest.ReservistReadinessID;
                    reservistAppointment.MilitaryRankName = (requestCommandPosition.MilitaryRanks != null && requestCommandPosition.MilitaryRanks.Count > 0 ? requestCommandPosition.MilitaryRanks[0].Rank.LongName : "");
                    reservistAppointment.MilitaryRankId = (requestCommandPosition.MilitaryRanks != null && requestCommandPosition.MilitaryRanks.Count > 0 ? requestCommandPosition.MilitaryRanks[0].Rank.MilitaryRankId : "");
                    reservistAppointment.MilReportingSpecialityName = (fulfilMRS != null ? fulfilMRS.MilReportingSpecialityName : "");
                    reservistAppointment.MilReportingSpecialityCode = (fulfilMRS != null ? fulfilMRS.MilReportingSpecialityCode : "");
                    reservistAppointment.MilReportSpecialityId = (fulfilMRS != null ? fulfilMRS.MilReportSpecialityId : (int?)null);
                    reservistAppointment.Position = requestCommandPosition.Position;
                    reservistAppointment.AppointmentTime = requestCommandPosition.RequestsCommand.AppointmentTime;
                    reservistAppointment.AppointmentPlace = ""; //TODO Currently we do not support this
                    reservistAppointment.FillReservistsRequestId = fillReservistsRequest.FillReservistsRequestID;

                    ReservistAppointmentUtil.AddReservistAppointment(reservistAppointment, CurrentUser, change);

                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>";
                }
                else
                {
                    stat = AJAXTools.OK;
                    response = @"<status>EXISTINGFULFIL</status>
                                 <message>Не може да бъде издадено МН, защото вече има такова за избрания резервист.</message>";
                }
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
    }
}
