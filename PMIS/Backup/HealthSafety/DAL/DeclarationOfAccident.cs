using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Text;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    //This class represents all fileds from AddEditDeclarationOfAccident.aspx page and ManageDeclarationOfAccident.aspx pages

    public class DeclarationOfAccident : BaseDbObject
    {
        private int declarationId;
        public int DeclarationId
        {
            get { return declarationId; }
            set { declarationId = value; }
        }

        private string declarationSubmitting = "";
        public string DeclarationSubmitting
        {
            get { return declarationSubmitting; }
            set { declarationSubmitting = value; }
        }

        private DeclarationOfAccidentHeader declarationOfAccidentHeader;
        public DeclarationOfAccidentHeader DeclarationOfAccidentHeader
        {
            get { return declarationOfAccidentHeader; }
            set { declarationOfAccidentHeader = value; }
        }

        private DeclarationOfAccidentFooter declarationOfAccidentFooter;
        public DeclarationOfAccidentFooter DeclarationOfAccidentFooter
        {
            get { return declarationOfAccidentFooter; }
            set { declarationOfAccidentFooter = value; }
        }

        //This class represents all fileds for Tab Employer
        private Employer employer;
        public Employer Employer
        {
            get
            {
                if (employer == null)
                {
                    employer = EmployerUtil.GetEmployerForDeclarationId(declarationId, CurrentUser);
                }

                return employer;

            }
            set { employer = value; }
        }

        private DeclarationOfAccidentWorker declarationOfAccidentWorker;
        public DeclarationOfAccidentWorker DeclarationOfAccidentWorker
        {
            get { return declarationOfAccidentWorker; }
            set { declarationOfAccidentWorker = value; }
        }

        private DeclarationOfAccidentAcc declarationOfAccidentAcc;
        public DeclarationOfAccidentAcc DeclarationOfAccidentAcc
        {
            get { return declarationOfAccidentAcc; }
            set { declarationOfAccidentAcc = value; }
        }

        private DeclarationOfAccidentHarm declarationOfAccidentHarm;
        public DeclarationOfAccidentHarm DeclarationOfAccidentHarm
        {
            get { return declarationOfAccidentHarm; }
            set { declarationOfAccidentHarm = value; }
        }

        private DeclarationOfAccidentWith declarationOfAccidentWith;
        public DeclarationOfAccidentWith DeclarationOfAccidentWith
        {
            get { return declarationOfAccidentWith; }
            set { declarationOfAccidentWith = value; }
        }

        private DeclarationOfAccidentHeir declarationOfAccidentHeir;
        public DeclarationOfAccidentHeir DeclarationOfAccidentHeir
        {
            get { return declarationOfAccidentHeir; }
            set { declarationOfAccidentHeir = value; }
        }

        public DeclarationOfAccident(int _declarationId, User currentUser)
            : base(currentUser)
        {
            declarationId = _declarationId;
            declarationOfAccidentHeader = new DeclarationOfAccidentHeader();
            declarationOfAccidentFooter = new DeclarationOfAccidentFooter();
            declarationOfAccidentWorker = new DeclarationOfAccidentWorker(currentUser);
            declarationOfAccidentAcc = new DeclarationOfAccidentAcc(currentUser);
            declarationOfAccidentHarm = new DeclarationOfAccidentHarm();
            declarationOfAccidentWith = new DeclarationOfAccidentWith(currentUser);
            declarationOfAccidentHeir = new DeclarationOfAccidentHeir(currentUser);
        }

        public DeclarationOfAccident(User currentUser)
            : base(currentUser)
        {
            declarationId = 0;
            declarationOfAccidentHeader = new DeclarationOfAccidentHeader();
            declarationOfAccidentFooter = new DeclarationOfAccidentFooter();
            declarationOfAccidentWorker = new DeclarationOfAccidentWorker(currentUser);
            declarationOfAccidentAcc = new DeclarationOfAccidentAcc(currentUser);
            declarationOfAccidentHarm = new DeclarationOfAccidentHarm();
            declarationOfAccidentWith = new DeclarationOfAccidentWith(currentUser);
            declarationOfAccidentHeir = new DeclarationOfAccidentHeir(currentUser);
        }
    }

    public class DeclarationOfAccidentHeader
    {
        private string declarationNumber = "";
        public string DeclarationNumber
        {
            get { return declarationNumber; }
            set { declarationNumber = value; }
        }

        private DateTime? declarationDate;
        public DateTime? DeclarationDate
        {
            get { return declarationDate; }
            set { declarationDate = value; }
        }

        private string referenceNumber = "";
        public string ReferenceNumber
        {
            get { return referenceNumber; }
            set { referenceNumber = value; }
        }

        private DateTime? referenceDate;
        public DateTime? ReferenceDate
        {
            get { return referenceDate; }
            set { referenceDate = value; }
        }

        private string fileNumber = "";
        public string FileNumber
        {
            get { return fileNumber; }
            set { fileNumber = value; }
        }
    }

    public class DeclarationOfAccidentFooter
    {
        private int applicantType = 0;
        public int ApplicantType
        {
            get { return applicantType; }
            set { applicantType = value; }
        }

        private string aplicantPosition = "";
        public string AplicantPosition
        {
            get { return aplicantPosition; }
            set { aplicantPosition = value; }
        }

        private string aplicantName = "";
        public string AplicantName
        {
            get { return aplicantName; }
            set { aplicantName = value; }
        }
    }

    //This class represents all fileds for Tab Worker
    public class DeclarationOfAccidentWorker : BaseDbObject
    {
        private string workerFullName = "";

        public string WorkerFullName
        {
            get { return workerFullName; }
            set { workerFullName = value; }
        }
        private string workerEgn = "";

        public string WorkerEgn
        {
            get { return workerEgn; }
            set { workerEgn = value; }
        }
        private int wCityId;

        public int WCityId
        {
            set { wCityId = value; }
        }

        private City city;

        public City City
        {
            get
            {
                if (city == null)
                {
                    city = CityUtil.GetCity(wCityId, base.CurrentUser);
                }
                return city;

            }
            set { city = value; }
        }

        private string wStreet = "";

        public string WStreet
        {
            get { return wStreet; }
            set { wStreet = value; }
        }
        private string wStreetNum = "";

        public string WStreetNum
        {
            get { return wStreetNum; }
            set { wStreetNum = value; }
        }
        private string wDistrict = "";

        public string WDistrict
        {
            get { return wDistrict; }
            set { wDistrict = value; }
        }
        private string wBlock = "";

        public string WBlock
        {
            get { return wBlock; }
            set { wBlock = value; }
        }
        private string wEntrance = "";

        public string WEntrance
        {
            get { return wEntrance; }
            set { wEntrance = value; }
        }
        private string wFloor = "";

        public string WFloor
        {
            get { return wFloor; }
            set { wFloor = value; }
        }
        private string wApt = "";

        public string WApt
        {
            get { return wApt; }
            set { wApt = value; }
        }
        private string wPhone = "";

        public string WPhone
        {
            get { return wPhone; }
            set { wPhone = value; }
        }
        private string wFax = "";

        public string WFax
        {
            get { return wFax; }
            set { wFax = value; }
        }
        private string wEmail = "";

        public string WEmail
        {
            get { return wEmail; }
            set { wEmail = value; }
        }

        private DateTime? wBirthDate;
        public DateTime? WBirthDate
        {
            get { return wBirthDate; }
            set { wBirthDate = value; }
        }

        private int? wGender;
        public int? WGender
        {
            get { return wGender; }
            set { wGender = value; }
        }
        private string wCitizenship = "";

        public string WCitizenship
        {
            get { return wCitizenship; }
            set { wCitizenship = value; }
        }
        private int? wHireType;

        public int? WHireType
        {
            get { return wHireType; }
            set { wHireType = value; }
        }
        private int? wWorkTime;

        public int? WWorkTime
        {
            get { return wWorkTime; }
            set { wWorkTime = value; }
        }
        private DateTime? wHireDate;

        public DateTime? WHireDate
        {
            get { return wHireDate; }
            set { wHireDate = value; }
        }
        private string wJobTitle = "";

        public string WJobTitle
        {
            get { return wJobTitle; }
            set { wJobTitle = value; }
        }
        private string wJobCode = "";

        public string WJobCode
        {
            get { return wJobCode; }
            set { wJobCode = value; }
        }
        private int? wJobCategory;

        public int? WJobCategory
        {
            get { return wJobCategory; }
            set { wJobCategory = value; }
        }
        private int? wYearsOnService;

        public int? WYearsOnService
        {
            get { return wYearsOnService; }
            set { wYearsOnService = value; }
        }

        private int? wCurrentJobYearsOnService;
        public int? WCurrentJobYearsOnService
        {
            get { return wCurrentJobYearsOnService; }
            set { wCurrentJobYearsOnService = value; }
        }

        private string wBranch = "";

        public string WBranch
        {
            get { return wBranch; }
            set { wBranch = value; }
        }

        public DeclarationOfAccidentWorker(User currentUser)
            : base(currentUser)
        {
            //Define Initial state of ddl
            // this.wGender = 1;
            // this.wHireType = 1;
            // this.wJobCategory = 1;
            // this.wWorkTime = 1;
            wCityId = 0;
        }
        public DeclarationOfAccidentWorker(int cityId, User currentUser)
            : base(currentUser)
        {
            //Define Initial state of ddl
            // this.wGender = 1;
            // this.wHireType = 1;
            // this.wJobCategory = 1;
            // this.wWorkTime = 1;
            wCityId = cityId;

        }

    }

    //This class represents all fileds for Tab Accident
    public class DeclarationOfAccidentAcc : BaseDbObject
    {
        private DateTime? accDateTime;

        public DateTime? AccDateTime
        {
            get { return accDateTime; }
            set { accDateTime = value; }
        }
        private int? accWorkFromHour1;

        public int? AccWorkFromHour1
        {
            get { return accWorkFromHour1; }
            set { accWorkFromHour1 = value; }
        }
        private int? accWorkFromMin1;

        public int? AccWorkFromMin1
        {
            get { return accWorkFromMin1; }
            set { accWorkFromMin1 = value; }
        }
        private int? accWorkToHour1;

        public int? AccWorkToHour1
        {
            get { return accWorkToHour1; }
            set { accWorkToHour1 = value; }
        }
        private int? accWorkToMin1;

        public int? AccWorkToMin1
        {
            get { return accWorkToMin1; }
            set { accWorkToMin1 = value; }
        }
        private int? accWorkFromHour2;

        public int? AccWorkFromHour2
        {
            get { return accWorkFromHour2; }
            set { accWorkFromHour2 = value; }
        }
        private int? accWorkFromMin2;

        public int? AccWorkFromMin2
        {
            get { return accWorkFromMin2; }
            set { accWorkFromMin2 = value; }
        }
        private int? accWorkToHour2;

        public int? AccWorkToHour2
        {
            get { return accWorkToHour2; }
            set { accWorkToHour2 = value; }
        }
        private int? accWorkToMin2;

        public int? AccWorkToMin2
        {
            get { return accWorkToMin2; }
            set { accWorkToMin2 = value; }
        }

        private string accPlace = "";

        public string AccPlace
        {
            get { return accPlace; }
            set { accPlace = value; }
        }

        private string accCountry = "";
        public string AccCountry
        {
            get { return accCountry; }
            set { accCountry = value; }
        }

        private int accCityId;

        public int AccCityId
        {
            set { accCityId = value; }
        }

        private City city;

        public City City
        {
            get
            {
                if (city == null)
                {
                    city = CityUtil.GetCity(accCityId, base.CurrentUser);
                }
                return city;

            }
            set { city = value; }
        }

        private string accStreet = "";

        public string AccStreet
        {
            get { return accStreet; }
            set { accStreet = value; }
        }
        private string accStreetNum = "";

        public string AccStreetNum
        {
            get { return accStreetNum; }
            set { accStreetNum = value; }
        }

        private string accDistrict = "";
        public string AccDistrict
        {
            get { return accDistrict; }
            set { accDistrict = value; }
        }

        private string accBlock = "";
        public string AccBlock
        {
            get { return accBlock; }
            set { accBlock = value; }
        }

        private string accEntrance = "";
        public string AccEntrance
        {
            get { return accEntrance; }
            set { accEntrance = value; }
        }

        private string accFloor = "";
        public string AccFloor
        {
            get { return accFloor; }
            set { accFloor = value; }
        }

        private string accApt = "";
        public string AccApt
        {
            get { return accApt; }
            set { accApt = value; }
        }

        private string accPhone = "";
        public string AccPhone
        {
            get { return accPhone; }
            set { accPhone = value; }
        }

        private string accFax = "";
        public string AccFax
        {
            get { return accFax; }
            set { accFax = value; }
        }

        private string accEmail = "";
        public string AccEmail
        {
            get { return accEmail; }
            set { accEmail = value; }
        }

        private int? accHappenedAt;

        public int? AccHappenedAt
        {
            get { return accHappenedAt; }
            set { accHappenedAt = value; }
        }
        private string accHappenedOther = "";

        public string AccHappenedOther
        {
            get { return accHappenedOther; }
            set { accHappenedOther = value; }
        }
        private string accJobType = "";

        public string AccJobType
        {
            get { return accJobType; }
            set { accJobType = value; }
        }
        private string accTaskType = "";

        public string AccTaskType
        {
            get { return accTaskType; }
            set { accTaskType = value; }
        }
        private string accDeviationFromTask = "";

        public string AccDeviationFromTask
        {
            get { return accDeviationFromTask; }
            set { accDeviationFromTask = value; }
        }
        private string accInjurDesc = "";

        public string AccInjurDesc
        {
            get { return accInjurDesc; }
            set { accInjurDesc = value; }
        }
        private int? accInjHasRights;

        public int? AccInjHasRights
        {
            get { return accInjHasRights; }
            set { accInjHasRights = value; }
        }
        private int? accLegalRef;

        public int? AccLegalRef
        {
            get { return accLegalRef; }
            set { accLegalRef = value; }
        }
        private string accPlannedActions = "";

        public string AccPlannedActions
        {
            get { return accPlannedActions; }
            set { accPlannedActions = value; }
        }
        private int? accLostDays;

        public int? AccLostDays
        {
            get { return accLostDays; }
            set { accLostDays = value; }
        }

        public DeclarationOfAccidentAcc(User currentUser)
            : base(currentUser)
        {
            //Define Initial state of ddl
            //this.accHappenedAt = 1;
            //this.accInjHasRights = 1;
            //this.accLegalRef = 1;

            accCityId = 0;
        }
        public DeclarationOfAccidentAcc(int cityId, User currentUser)
            : base(currentUser)
        {
            //Define Initial state of ddl
            //this.accHappenedAt = 1;
            //this.accInjHasRights = 1;
            //this.accLegalRef = 1;

            accCityId = cityId;

        }

    }

    //This class represents all fileds for Tab Harm
    public class DeclarationOfAccidentHarm
    {
        private string harmType = "";

        public string HarmType
        {
            get { return harmType; }
            set { harmType = value; }
        }
        private string harmBodyParts = "";

        public string HarmBodyParts
        {
            get { return harmBodyParts; }
            set { harmBodyParts = value; }
        }
        private int? harmResult;

        public int? HarmResult
        {
            get { return harmResult; }
            set { harmResult = value; }
        }

        public DeclarationOfAccidentHarm()
        {
            //Define Initial state of ddl
            // this.HarmResult = 3;
        }
    }

    //This class represents all fileds for Tab Withiness
    public class DeclarationOfAccidentWith : BaseDbObject
    {
        private string witnessFullName = "";
        public string WitnessFullName
        {
            get { return witnessFullName; }
            set { witnessFullName = value; }
        }

        private int witCityId;
        public int WitCityId
        {
            set { witCityId = value; }
        }

        private City city;
        public City City
        {
            get
            {
                if (city == null)
                {
                    city = CityUtil.GetCity(witCityId, base.CurrentUser);
                }
                return city;
            }
            set { city = value; }
        }

        private string witStreet = "";

        public string WitStreet
        {
            get { return witStreet; }
            set { witStreet = value; }
        }
        private string witStreetNum = "";

        public string WitStreetNum
        {
            get { return witStreetNum; }
            set { witStreetNum = value; }
        }
        private string witDistrict = "";

        public string WitDistrict
        {
            get { return witDistrict; }
            set { witDistrict = value; }
        }
        private string witBlock = "";

        public string WitBlock
        {
            get { return witBlock; }
            set { witBlock = value; }
        }
        private string witEntrance = "";

        public string WitEntrance
        {
            get { return witEntrance; }
            set { witEntrance = value; }
        }
        private string witFloor = "";

        public string WitFloor
        {
            get { return witFloor; }
            set { witFloor = value; }
        }
        private string witApt = "";

        public string WitApt
        {
            get { return witApt; }
            set { witApt = value; }
        }
        private string witPhone = "";

        public string WitPhone
        {
            get { return witPhone; }
            set { witPhone = value; }
        }
        private string witFax = "";

        public string WitFax
        {
            get { return witFax; }
            set { witFax = value; }
        }
        private string witEmail = "";

        public string WitEmail
        {
            get { return witEmail; }
            set { witEmail = value; }
        }

        public DeclarationOfAccidentWith(User currentUser)
            : base(currentUser)
        {
            witCityId = 0;
        }
        public DeclarationOfAccidentWith(int cityId, User currentUser)
            : base(currentUser)
        {
            witCityId = cityId;
        }
    }

    //This class represents all fileds for Tab Heir
    public class DeclarationOfAccidentHeir : BaseDbObject
    {
        private string heirFullName = "";

        public string HeirFullName
        {
            get { return heirFullName; }
            set { heirFullName = value; }
        }
        private string heirEgn = "";

        public string HeirEgn
        {
            get { return heirEgn; }
            set { heirEgn = value; }
        }
        private int heirCityId;
        public int HeirCityId
        {
            set { heirCityId = value; }
        }
        private City city;
        public City City
        {
            get
            {
                if (city == null)
                {
                    city = CityUtil.GetCity(heirCityId, base.CurrentUser);
                }
                return city;
            }
            set { city = value; }
        }

        private string heirStreetNum = "";

        public string HeirStreetNum
        {
            get { return heirStreetNum; }
            set { heirStreetNum = value; }
        }
        private string heirDistrict = "";

        public string HeirDistrict
        {
            get { return heirDistrict; }
            set { heirDistrict = value; }
        }
        private string heirBlock = "";

        public string HeirBlock
        {
            get { return heirBlock; }
            set { heirBlock = value; }
        }
        private string heirEntrance = "";

        public string HeirEntrance
        {
            get { return heirEntrance; }
            set { heirEntrance = value; }
        }
        private string heirFloor = "";

        public string HeirFloor
        {
            get { return heirFloor; }
            set { heirFloor = value; }
        }
        private string heirApt = "";

        public string HeirApt
        {
            get { return heirApt; }
            set { heirApt = value; }
        }
        private string heirPhone = "";

        public string HeirPhone
        {
            get { return heirPhone; }
            set { heirPhone = value; }
        }
        private string heirFax = "";

        public string HeirFax
        {
            get { return heirFax; }
            set { heirFax = value; }
        }
        private string heirEmail = "";

        public string HeirEmail
        {
            get { return heirEmail; }
            set { heirEmail = value; }
        }

        private string heirStreet = "";

        public string HeirStreet
        {
            get { return heirStreet; }
            set { heirStreet = value; }
        }

        public DeclarationOfAccidentHeir(User currentUser)
            : base(currentUser)
        {
            heirCityId = 0;
        }
        public DeclarationOfAccidentHeir(int cityId, User currentUser)
            : base(currentUser)
        {
            heirCityId = cityId;
        }

    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class DeclarationOfAccidentFilter
    {
        string declarationNumber;
        public string DeclarationNumber
        {
            get { return declarationNumber; }
            set { declarationNumber = value; }
        }

        DateTime? declarationDateFrom;
        public DateTime? DeclarationDateFrom
        {
            get { return declarationDateFrom; }
            set { declarationDateFrom = value; }
        }

        DateTime? declarationDateTo;
        public DateTime? DeclarationDateTo
        {
            get { return declarationDateTo; }
            set { declarationDateTo = value; }
        }

        string workerFullName;
        public string WorkerFullName
        {
            get { return workerFullName; }
            set { workerFullName = value; }
        }

        private int orderBy;
        public int OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        private int pageIndex;
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        private int pageCount;
        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }
    }

    //This class stored methods for looking and changes DeclarationOfAccident
    public static class DeclarationOfAccidentUtil
    {
        //Create cheked string for string radiobuttons
        public static string GetChekedStatus(int rbObject, int? dbValue)
        {
            string htmlReturn = "";
            if (dbValue.HasValue)
            {
                if (rbObject == dbValue)
                {
                    htmlReturn = "checked='checked'";
                }
            }

            return htmlReturn;

        }
        //Create cheked string for string radiobuttons
        public static string GetVisibilityStatus(string statusRadioButton)
        {
            string htmlReturn = "hidden";
            if (statusRadioButton == "checked='checked'")
            {
                htmlReturn = "visible";
            }
            return htmlReturn;
        }

        //Get RegionId, MunisipalityId and CityId for selected tab
        public static List<int> GetListCode(int declarationId, string selectedTab, User currentUser)
        {
            List<int> listResult = new List<int>();

            if ((declarationId == 0) || (selectedTab == "btnTabHarm")) return listResult;

            string cityName = "";

            switch (selectedTab)
            {
                //Create Select statement for this tab

                case "btnTabEmpl":
                    cityName = "EmplCityId";
                    break;
                case "btnTabWorker":
                    cityName = "WCityId";
                    break;
                case "btnTabAcc":
                    cityName = "AccCityId";
                    break;
                case "btnTabWith":
                    cityName = "WitCityId";
                    break;
                case "btnTabHeir":
                    cityName = "HeirCityid";
                    break;
                default:
                    break;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT  Kod_Obl, Kod_Obs, Kod_Nma  FROM UKAZ_OWNER.KL_NMA 
                                WHERE kod_nma =(SELECT " + cityName +
                                @" FROM PMIS_HS.DECLARATIONSOFACCIDENT
                                where DECLARATIONID=:declarationId)";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "declarationId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = declarationId;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    listResult.Add(DBCommon.GetInt(dr["Kod_Obl"]));
                    listResult.Add(DBCommon.GetInt(dr["Kod_Obs"]));
                    listResult.Add(DBCommon.GetInt(dr["Kod_Nma"]));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listResult;
        }
        //Get PostCode for selected City
        public static int? GetPostCode(int cityId, User currentUser)
        {
            int? postCode = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.PK FROM UKAZ_OWNER.KL_NMA a
where a.Kod_Nma = :CityId";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "CityId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = cityId;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read() && (DBCommon.GetInt(dr["Pk"]) != -1))
                {
                    postCode = DBCommon.GetInt(dr["Pk"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postCode;
        }
        //Get RegionId, MunisipalityId and PostCode for SelectedEmployeerCityId 
        public static List<int> GetListForEmployerCityId(int cityId, User currentUser)
        {
            List<int> listResult = new List<int>();
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT  Kod_Obl, Kod_Obs, Pk FROM UKAZ_OWNER.KL_NMA 
                                WHERE kod_nma = :CityId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "CityId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = cityId;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    listResult.Add(DBCommon.GetInt(dr["Kod_Obl"]));
                    listResult.Add(DBCommon.GetInt(dr["Kod_Obs"]));
                    listResult.Add(DBCommon.GetInt(dr["Pk"]));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listResult;
        }
        //Get RegionId, MunisipalityId and CityId for postCode 
        public static List<int> GetListForPostCode(int postCode, User currentUser)
        {
            List<int> listResult = new List<int>();
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string SQL = @"SELECT  KOD_OBL, KOD_OBS, KOD_NMA  FROM UKAZ_OWNER.KL_NMA 
                               WHERE PK = :postCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "postCode";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = postCode;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    listResult.Add(DBCommon.GetInt(dr["KOD_OBL"]));
                    listResult.Add(DBCommon.GetInt(dr["KOD_OBS"]));
                    listResult.Add(DBCommon.GetInt(dr["KOD_NMA"]));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listResult;
        }
        //Get Object for fill UI
        public static DeclarationOfAccident GetDeclarationOfAccident(int declarationId, User currentUser, string selectedTab)
        {
            DeclarationOfAccident declarationOfAccident = null;

            DeclarationOfAccidentHeader declarationOfAccidentHeader = null;
            DeclarationOfAccidentFooter declarationOfAccidentFooter = null;
            Employer employer = null;
            DeclarationOfAccidentWorker declarationOfAccidentWorker = null;
            DeclarationOfAccidentAcc declarationOfAccidentAcc = null;
            DeclarationOfAccidentHarm declarationOfAccidentHarm = null;
            DeclarationOfAccidentWith declarationOfAccidentWith = null;
            DeclarationOfAccidentHeir declarationOfAccidentHeir = null;

            string SQL = "";

            if (declarationId == 0)
            {
                return declarationOfAccident;
            }

            declarationOfAccident = new DeclarationOfAccident(declarationId, currentUser);

            switch (selectedTab)
            {
                //Create Select statement for this tab
                case "btnTabEmpl":
                    SQL = @"SELECT 

                            a.declarationNumber,
                            a.declarationDate,
                            a.referenceNumber,
                            a.referenceDate,
                            a.fileNumber,
                            a.AplicantType,
                            a.AplicantPosition,
                            a.AplicantName,

                            a.employerId,
                            b.employerName,
                            a.emplEik,
                            a.emplCityId,
                            a.emplStreet,
                            a.emplStreetNum,
                            a.emplDistrict,
                            a.emplBlock,
                            a.emplEntrance,
                            a.emplFloor,
                            a.emplApt,
                            a.emplPhone,
                            a.emplFax,
                            a.emplEmail,
                            a.emplNumberOfEmployees,
                            a.emplFemaleEmployees,
                            a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate

                        FROM PMIS_HS.DeclarationsOfAccident a
                        LEFT OUTER JOIN  PMIS_HS.EMPLOYERS b 
                        on a.employerId = b.employerId ";
                    break;
                case "btnTabWorker":
                    SQL = @"SELECT 

                            WorkerFullName,
                            WorkerEgn,
                            WCityId,
                            WStreet,
                            WStreetNum,
                            WDistrict,
                            WBlock,
                            WEntrance,
                            WFloor,
                            WApt,
                            WPhone,
                            WFax,
                            WEmail,
                            WBirthDate,
                            WGender,
                            WCitizenship,
                            WHireType,
                            WWorkTime,
                            WHireDate,
                            WJobTitle,
                            WJobCode,
                            WJobCategory,
                            WYearsOnService,
                            WCurrentJobYearsOnService,
                            WBranch,
                            CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate
 
                        FROM PMIS_HS.DeclarationsOfAccident a ";

                    break;
                case "btnTabAcc":
                    SQL = @"SELECT 

                            AccDateTime,
                            AccWorkFromHour1,
                            AccWorkFromMin1,
                            AccWorkToHour1,
                            AccWorkToMin1,
                            AccWorkFromHour2,
                            AccWorkFromMin2,
                            AccWorkToHour2,
                            AccWorkToMin2,
                            AccPlace,
                            AccCountry,
                            AccCityId,
                            AccStreet,
                            AccStreetNum,
                            AccDistrict,
                            AccBlock,
                            AccEntrance,
                            AccFloor,
                            AccApt,
                            AccPhone,
                            AccFax,
                            AccEmail,
                            AccHappenedAt,
                            AccHappenedOther,
                            AccJobType,
                            AccTaskType,
                            AccDeviationFromTask,
                            AccInjurDesc,
                            AccInjHasRights,
                            AccLegalRef,
                            AccPlannedActions,
                            AccLostDays,
                            CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate 

                        FROM PMIS_HS.DeclarationsOfAccident a ";

                    break;
                case "btnTabHarm":
                    SQL = @"SELECT 

                            HarmType,
                            HarmBodyParts,
                            HarmResult,
                            CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate

                        FROM PMIS_HS.DeclarationsOfAccident a ";


                    break;
                case "btnTabWith":
                    SQL = @"SELECT 

                            WitnessFullName,
                            WitCityId,
                            WitStreet,
                            WitStreetNum,
                            WitDistrict,
                            WitBlock,
                            WitEntrance,
                            WitFloor,
                            WitApt,
                            WitPhone,
                            WitFax,
                            WitEmail,
                            CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate


                        FROM PMIS_HS.DeclarationsOfAccident a ";

                    break;
                case "btnTabHeir":

                    SQL = @"SELECT 

                            HeirFullName,
                            HeirEgn,
                            HeirCityid,
                            HeirStreet,
                            HeirStreetNum,
                            HeirDistrict,
                            HeirBlock,
                            HeirEntrance,
                            HeirFloor,
                            HeirApt,
                            HeirPhone,
                            HeirFax,
                            HeirEmail,
                            CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate

                        FROM PMIS_HS.DeclarationsOfAccident a ";


                    break;
                default:
                    break;
            }

            string where = "";

            //Restric the user to access only his own records if this is set for the particular role
            UIItem uiItem = UIItemUtil.GetUIItems("HS_DECLARATIONACC", currentUser, false, currentUser.Role.RoleId, null)[0];
            if (uiItem.AccessOnlyOwnData)
            {
                where += " AND a.CreatedBy = " + currentUser.UserId.ToString();
            }

            SQL += "WHERE a.declarationId = :declarationId " + where;

            SQL = DBCommon.FixNewLines(SQL);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            try
            {
                conn.Open();

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "declarationId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = declarationId;

                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    switch (selectedTab)
                    {
                        case "btnTabEmpl": //fill Header and this tab UI

                            //declarationOfAccidentHeader
                            declarationOfAccidentHeader = new DeclarationOfAccidentHeader();

                            declarationOfAccidentHeader.DeclarationNumber = dr["declarationNumber"].ToString();
                            declarationOfAccidentHeader.ReferenceNumber = dr["referenceNumber"].ToString();
                            declarationOfAccidentHeader.FileNumber = dr["fileNumber"].ToString();

                            if (dr["declarationDate"] is DateTime) declarationOfAccidentHeader.DeclarationDate = (DateTime)dr["DeclarationDate"];
                            if (dr["referenceDate"] is DateTime) declarationOfAccidentHeader.ReferenceDate = (DateTime)dr["ReferenceDate"];
                            //Fill parent Object
                            declarationOfAccident.DeclarationOfAccidentHeader = declarationOfAccidentHeader;

                            //Tab Emplyeer
                            employer = new Employer(currentUser);

                            employer.EmployerName = dr["employerName"].ToString();
                            employer.EmplEik = dr["emplEik"].ToString();
                            employer.EmplStreet = dr["emplStreet"].ToString();
                            employer.EmplStreetNum = dr["emplStreetNum"].ToString();
                            employer.EmplDistrict = dr["emplDistrict"].ToString();
                            employer.EmplBlock = dr["emplBlock"].ToString();
                            employer.EmplEntrance = dr["emplEntrance"].ToString();
                            employer.EmplFloor = dr["emplFloor"].ToString();
                            employer.EmplApt = dr["emplApt"].ToString();
                            employer.EmplPhone = dr["emplPhone"].ToString();
                            employer.EmplFax = dr["emplFax"].ToString();
                            employer.EmplEmail = dr["emplEmail"].ToString();

                            if (DBCommon.IsInt(dr["employerId"])) employer.EmployerId = DBCommon.GetInt(dr["employerId"]);
                            if (DBCommon.IsInt(dr["emplNumberOfEmployees"])) employer.EmplNumberOfEmployees = DBCommon.GetInt(dr["emplNumberOfEmployees"]);
                            if (DBCommon.IsInt(dr["emplFemaleEmployees"])) employer.EmplFemaleEmployees = DBCommon.GetInt(dr["emplFemaleEmployees"]);

                            City city = new City(currentUser);
                            if (DBCommon.IsInt(dr["emplCityId"]))
                            {
                                int cityId = DBCommon.GetInt(dr["emplCityId"]);
                                city = CityUtil.GetCity(cityId, currentUser);
                                city.CityName = city.CityName;
                                city.CityId = cityId;
                            }
                            //fill parent object
                            employer.City = city;

                            //Fill parent Object
                            declarationOfAccident.Employer = employer;

                            //declarationOfAccidentFooter
                            declarationOfAccidentFooter = new DeclarationOfAccidentFooter();

                            if (DBCommon.IsInt(dr["AplicantType"]))
                            {
                                declarationOfAccidentFooter.ApplicantType = DBCommon.GetInt(dr["AplicantType"]);
                                if (declarationOfAccidentFooter.ApplicantType == 1)
                                {
                                    declarationOfAccidentFooter.AplicantPosition = dr["AplicantPosition"].ToString();
                                    declarationOfAccidentFooter.AplicantName = dr["AplicantName"].ToString();
                                }
                            }
                            declarationOfAccident.DeclarationOfAccidentFooter = declarationOfAccidentFooter;

                            if (dr["declarationDate"] is DateTime) declarationOfAccidentHeader.DeclarationDate = (DateTime)dr["DeclarationDate"];
                            if (dr["referenceDate"] is DateTime) declarationOfAccidentHeader.ReferenceDate = (DateTime)dr["ReferenceDate"];
                            //Fill parent Object
                            declarationOfAccident.DeclarationOfAccidentHeader = declarationOfAccidentHeader;

                            break;

                        case "btnTabWorker":
                            //Tab Worker
                            declarationOfAccidentWorker = new DeclarationOfAccidentWorker(currentUser);

                            declarationOfAccidentWorker.WorkerFullName = dr["WorkerFullName"].ToString();
                            declarationOfAccidentWorker.WorkerEgn = dr["WorkerEgn"].ToString();
                            declarationOfAccidentWorker.WStreet = dr["WStreet"].ToString();
                            declarationOfAccidentWorker.WStreetNum = dr["WStreetNum"].ToString();
                            declarationOfAccidentWorker.WDistrict = dr["WDistrict"].ToString();
                            declarationOfAccidentWorker.WBlock = dr["WBlock"].ToString();
                            declarationOfAccidentWorker.WEntrance = dr["WEntrance"].ToString();
                            declarationOfAccidentWorker.WFloor = dr["WFloor"].ToString();
                            declarationOfAccidentWorker.WApt = dr["WApt"].ToString();
                            declarationOfAccidentWorker.WPhone = dr["WPhone"].ToString();
                            declarationOfAccidentWorker.WFax = dr["WFax"].ToString();
                            declarationOfAccidentWorker.WEmail = dr["WEmail"].ToString();
                            declarationOfAccidentWorker.WCitizenship = dr["WCitizenship"].ToString();
                            declarationOfAccidentWorker.WJobTitle = dr["WJobTitle"].ToString();
                            declarationOfAccidentWorker.WJobCode = dr["WJobCode"].ToString();
                            declarationOfAccidentWorker.WBranch = dr["WBranch"].ToString();

                            if (DBCommon.IsInt(dr["WCityId"])) declarationOfAccidentWorker.WCityId = DBCommon.GetInt(dr["WCityId"]);
                            if (DBCommon.IsInt(dr["WGender"])) declarationOfAccidentWorker.WGender = DBCommon.GetInt(dr["WGender"]);
                            if (DBCommon.IsInt(dr["WHireType"])) declarationOfAccidentWorker.WHireType = DBCommon.GetInt(dr["WHireType"]);
                            if (DBCommon.IsInt(dr["WWorkTime"])) declarationOfAccidentWorker.WWorkTime = DBCommon.GetInt(dr["WWorkTime"]);
                            if (DBCommon.IsInt(dr["WJobCategory"])) declarationOfAccidentWorker.WJobCategory = DBCommon.GetInt(dr["WJobCategory"]);
                            if (DBCommon.IsInt(dr["WYearsOnService"])) declarationOfAccidentWorker.WYearsOnService = DBCommon.GetInt(dr["WYearsOnService"]);
                            if (DBCommon.IsInt(dr["WCurrentJobYearsOnService"])) declarationOfAccidentWorker.WCurrentJobYearsOnService = DBCommon.GetInt(dr["WCurrentJobYearsOnService"]);

                            if (dr["WBirthDate"] is DateTime) declarationOfAccidentWorker.WBirthDate = (DateTime)dr["WBirthDate"];
                            if (dr["WHireDate"] is DateTime) declarationOfAccidentWorker.WHireDate = (DateTime)dr["WHireDate"];
                            //Fill parent Object
                            declarationOfAccident.DeclarationOfAccidentWorker = declarationOfAccidentWorker;

                            break;

                        case "btnTabAcc":
                            //create obect
                            declarationOfAccidentAcc = new DeclarationOfAccidentAcc(currentUser);

                            declarationOfAccidentAcc.AccPlace = dr["AccPlace"].ToString();
                            declarationOfAccidentAcc.AccCountry = dr["AccCountry"].ToString();
                            declarationOfAccidentAcc.AccStreet = dr["AccStreet"].ToString();
                            declarationOfAccidentAcc.AccStreetNum = dr["AccStreetNum"].ToString();
                            declarationOfAccidentAcc.AccDistrict = dr["AccDistrict"].ToString();
                            declarationOfAccidentAcc.AccBlock = dr["AccBlock"].ToString();
                            declarationOfAccidentAcc.AccEntrance = dr["AccEntrance"].ToString();
                            declarationOfAccidentAcc.AccFloor = dr["AccFloor"].ToString();
                            declarationOfAccidentAcc.AccApt = dr["AccApt"].ToString();
                            declarationOfAccidentAcc.AccPhone = dr["AccPhone"].ToString();
                            declarationOfAccidentAcc.AccFax = dr["AccFax"].ToString();
                            declarationOfAccidentAcc.AccEmail = dr["AccEmail"].ToString();
                            declarationOfAccidentAcc.AccHappenedOther = dr["AccHappenedOther"].ToString();
                            declarationOfAccidentAcc.AccJobType = dr["AccJobType"].ToString();
                            declarationOfAccidentAcc.AccTaskType = dr["AccTaskType"].ToString();
                            declarationOfAccidentAcc.AccDeviationFromTask = dr["AccDeviationFromTask"].ToString();
                            declarationOfAccidentAcc.AccInjurDesc = dr["AccInjurDesc"].ToString();
                            declarationOfAccidentAcc.AccPlannedActions = dr["AccPlannedActions"].ToString();

                            if (dr["AccDateTime"] is DateTime) declarationOfAccidentAcc.AccDateTime = (DateTime)dr["AccDateTime"];

                            if (DBCommon.IsInt(dr["AccWorkFromHour1"])) declarationOfAccidentAcc.AccWorkFromHour1 = DBCommon.GetInt(dr["AccWorkFromHour1"]);
                            if (DBCommon.IsInt(dr["AccWorkFromMin1"])) declarationOfAccidentAcc.AccWorkFromMin1 = DBCommon.GetInt(dr["AccWorkFromMin1"]);
                            if (DBCommon.IsInt(dr["AccWorkToHour1"])) declarationOfAccidentAcc.AccWorkToHour1 = DBCommon.GetInt(dr["AccWorkToHour1"]);
                            if (DBCommon.IsInt(dr["AccWorkToMin1"])) declarationOfAccidentAcc.AccWorkToMin1 = DBCommon.GetInt(dr["AccWorkToMin1"]);
                            if (DBCommon.IsInt(dr["AccWorkFromHour2"])) declarationOfAccidentAcc.AccWorkFromHour2 = DBCommon.GetInt(dr["AccWorkFromHour2"]);
                            if (DBCommon.IsInt(dr["AccWorkFromMin2"])) declarationOfAccidentAcc.AccWorkFromMin2 = DBCommon.GetInt(dr["AccWorkFromMin2"]);
                            if (DBCommon.IsInt(dr["AccWorkToHour2"])) declarationOfAccidentAcc.AccWorkToHour2 = DBCommon.GetInt(dr["AccWorkToHour2"]);
                            if (DBCommon.IsInt(dr["AccWorkToMin2"])) declarationOfAccidentAcc.AccWorkToMin2 = DBCommon.GetInt(dr["AccWorkToMin2"]);
                            if (DBCommon.IsInt(dr["AccCityId"])) declarationOfAccidentAcc.AccCityId = DBCommon.GetInt(dr["AccCityId"]);
                            if (DBCommon.IsInt(dr["AccHappenedAt"])) declarationOfAccidentAcc.AccHappenedAt = DBCommon.GetInt(dr["AccHappenedAt"]);
                            if (DBCommon.IsInt(dr["AccInjHasRights"])) declarationOfAccidentAcc.AccInjHasRights = DBCommon.GetInt(dr["AccInjHasRights"]);
                            if (DBCommon.IsInt(dr["AccLegalRef"])) declarationOfAccidentAcc.AccLegalRef = DBCommon.GetInt(dr["AccLegalRef"]);
                            if (DBCommon.IsInt(dr["AccLostDays"])) declarationOfAccidentAcc.AccLostDays = DBCommon.GetInt(dr["AccLostDays"]);
                            //Fill parent Object
                            declarationOfAccident.DeclarationOfAccidentAcc = declarationOfAccidentAcc;

                            break;
                        case "btnTabHarm":
                            declarationOfAccidentHarm = new DeclarationOfAccidentHarm();

                            declarationOfAccidentHarm.HarmType = dr["HarmType"].ToString();
                            declarationOfAccidentHarm.HarmBodyParts = dr["HarmBodyParts"].ToString();

                            if (DBCommon.IsInt(dr["HarmResult"])) declarationOfAccidentHarm.HarmResult = DBCommon.GetInt(dr["HarmResult"]);

                            //Fill parent Object
                            declarationOfAccident.DeclarationOfAccidentHarm = declarationOfAccidentHarm;

                            break;


                        case "btnTabWith":
                            //Create Object
                            declarationOfAccidentWith = new DeclarationOfAccidentWith(currentUser);

                            declarationOfAccidentWith.WitnessFullName = dr["WitnessFullName"].ToString();
                            declarationOfAccidentWith.WitStreet = dr["WitStreet"].ToString();
                            declarationOfAccidentWith.WitStreetNum = dr["WitStreetNum"].ToString();
                            declarationOfAccidentWith.WitDistrict = dr["WitDistrict"].ToString();
                            declarationOfAccidentWith.WitBlock = dr["WitBlock"].ToString();
                            declarationOfAccidentWith.WitEntrance = dr["WitEntrance"].ToString();
                            declarationOfAccidentWith.WitFloor = dr["WitFloor"].ToString();
                            declarationOfAccidentWith.WitApt = dr["WitApt"].ToString();
                            declarationOfAccidentWith.WitPhone = dr["WitPhone"].ToString();
                            declarationOfAccidentWith.WitFax = dr["WitFax"].ToString();
                            declarationOfAccidentWith.WitEmail = dr["WitEmail"].ToString();

                            if (DBCommon.IsInt(dr["WitCityId"])) declarationOfAccidentWith.WitCityId = DBCommon.GetInt(dr["WitCityId"]);
                            //Fill parent Object
                            declarationOfAccident.DeclarationOfAccidentWith = declarationOfAccidentWith;
                            break;
                        case "btnTabHeir":
                            //Create Object
                            declarationOfAccidentHeir = new DeclarationOfAccidentHeir(currentUser);

                            declarationOfAccidentHeir.HeirFullName = dr["HeirFullName"].ToString();
                            declarationOfAccidentHeir.HeirEgn = dr["HeirEgn"].ToString();
                            declarationOfAccidentHeir.HeirStreet = dr["HeirStreet"].ToString();
                            declarationOfAccidentHeir.HeirStreetNum = dr["HeirStreetNum"].ToString();
                            declarationOfAccidentHeir.HeirDistrict = dr["HeirDistrict"].ToString();
                            declarationOfAccidentHeir.HeirBlock = dr["HeirBlock"].ToString();
                            declarationOfAccidentHeir.HeirEntrance = dr["HeirEntrance"].ToString();
                            declarationOfAccidentHeir.HeirFloor = dr["HeirFloor"].ToString();
                            declarationOfAccidentHeir.HeirApt = dr["HeirApt"].ToString();
                            declarationOfAccidentHeir.HeirPhone = dr["HeirPhone"].ToString();
                            declarationOfAccidentHeir.HeirFax = dr["HeirFax"].ToString();
                            declarationOfAccidentHeir.HeirEmail = dr["HeirEmail"].ToString();

                            if (DBCommon.IsInt(dr["HeirCityid"])) declarationOfAccidentHeir.HeirCityId = DBCommon.GetInt(dr["HeirCityid"]);
                            //Fill parent Object
                            declarationOfAccident.DeclarationOfAccidentHeir = declarationOfAccidentHeir;

                            break;
                        default:
                            break;
                    }

                    //Extrat created and last modified fields from a data reader. Coomon for all objects
                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, declarationOfAccident);

                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return declarationOfAccident;
        }
        //Save Method
        public static bool SaveDeclarationOfAccident(DeclarationOfAccident declarationOfAccident, User currentUser, Change changeEntry)
        {
            List<string> columnNameList = new List<string>();
            List<OracleParameter> paramCollection = new List<OracleParameter>();
            string columnName = "";
            string columnParameter = "";

            ChangeEvent changeEvent;
            string SQL = "";
            bool result = false;

            //Bind List with Column and corresponded Parameter

            //add ListItems for Haeder and TabEmpl by default

            AddColumnNameParameterList(declarationOfAccident, "Header", columnNameList, paramCollection, currentUser);
            AddColumnNameParameterList(declarationOfAccident, "Footer", columnNameList, paramCollection, currentUser);
            AddColumnNameParameterList(declarationOfAccident, "tabEmpl", columnNameList, paramCollection, currentUser);

            if (declarationOfAccident.DeclarationOfAccidentWorker != null)
            {
                AddColumnNameParameterList(declarationOfAccident, "tabWorker", columnNameList, paramCollection, currentUser);
            }
            if (declarationOfAccident.DeclarationOfAccidentAcc != null)
            {
                AddColumnNameParameterList(declarationOfAccident, "tabAcc", columnNameList, paramCollection, currentUser);
            }
            if (declarationOfAccident.DeclarationOfAccidentHarm != null)
            {
                AddColumnNameParameterList(declarationOfAccident, "tabHarm", columnNameList, paramCollection, currentUser);
            }
            if (declarationOfAccident.DeclarationOfAccidentWith != null)
            {
                AddColumnNameParameterList(declarationOfAccident, "tabWith", columnNameList, paramCollection, currentUser);
            }
            if (declarationOfAccident.DeclarationOfAccidentHeir != null)
            {
                AddColumnNameParameterList(declarationOfAccident, "tabHeir", columnNameList, paramCollection, currentUser);
            }

            if (columnNameList.Count > 0)
            {

                for (int i = 0; i <= columnNameList.Count - 1; i++)
                {
                    columnName += columnNameList[i];
                    if (i < columnNameList.Count - 1)
                    {
                        columnName += ", ";
                    }
                }

                for (int i = 0; i <= columnNameList.Count - 1; i++)
                {
                    columnParameter += ":" + columnNameList[i];
                    if (i < columnNameList.Count - 1)
                    {
                        columnParameter += ", ";
                    }
                }
            }
            else // We have no Column to update/insert
            {
                return false;
            }

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (declarationOfAccident.DeclarationId == 0) //INSERT
                {
                    string comma = "";
                    if (columnName != "") comma = ", ";

                    columnName += comma + "CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate";
                    columnParameter += comma + ":CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate";

                    SQL += @"INSERT INTO PMIS_HS.DeclarationsOfAccident (" + columnName + @")
                             VALUES (" + columnParameter + @");
                       
                             SELECT PMIS_HS.DeclarationsOfAccident_ID_SEQ.currval INTO :declarationId FROM dual; ";

                    //Create obect using log for INSERT records
                    changeEvent = new ChangeEvent("HS_DeclAcc_AddDecl", "", null, null, currentUser);
                }
                else //UPDATE
                {
                    string columnNameValue = "";

                    if (columnNameList.Count > 0)
                    {

                        for (int i = 0; i <= columnNameList.Count - 1; i++)
                        {
                            columnNameValue += columnNameList[i] + " = :" + columnNameList[i];
                            if (i < columnNameList.Count - 1)
                            {
                                columnNameValue += ", ";
                            }
                        }
                    }

                    if (columnNameValue != "")
                    {
                        columnNameValue += ", ";

                        columnNameValue += "LastModifiedBy = :LastModifiedBy, ";
                        columnNameValue += "LastModifiedDate = :LastModifiedDate";
                    }

                    SQL += @" UPDATE PMIS_HS.DeclarationsOfAccident  SET " +
                              columnNameValue +
                          @"  WHERE declarationId = :declarationId ;";

                    //Create obect using log for UPDATE records
                    changeEvent = new ChangeEvent("HS_DeclAcc_EditDecl", "", null, null, currentUser);
                }

                //Fill object with data for Insert or Update
                AddChangeEventDetails(declarationOfAccident, changeEvent, currentUser);

                SQL += @" END;";

                SQL = DBCommon.FixNewLines(SQL);

                //Create command object
                OracleCommand cmd = new OracleCommand(SQL, conn);

                //Fill object with parameters
                for (int i = 0; i <= paramCollection.Count - 1; i++)
                {
                    cmd.Parameters.Add(paramCollection[i]);
                }

                //Set additional special paramaters for log
                if (declarationOfAccident.DeclarationId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);


                //Add Special Parameter using to hold New declarationId for Insert 
                //or using in Where clause for Update

                OracleParameter paramDeclarationId = new OracleParameter();
                paramDeclarationId.ParameterName = "declarationId";
                paramDeclarationId.OracleType = OracleType.Number;

                if (declarationOfAccident.DeclarationId != 0) // we have Update
                {
                    paramDeclarationId.Direction = ParameterDirection.InputOutput;
                    paramDeclarationId.Value = declarationOfAccident.DeclarationId;
                }
                else
                {
                    paramDeclarationId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramDeclarationId);

                //Set Comand text
                cmd.CommandText = SQL;

                //Run Query
                result = Convert.ToBoolean(cmd.ExecuteNonQuery());

                //Set outputparameter
                declarationOfAccident.DeclarationId = DBCommon.GetInt(paramDeclarationId.Value);

                if (changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                }

            }

            finally
            {
                conn.Close();
            }
            return result;
        }
        //Bind parameter list for Oracle Query
        private static void AddColumnNameParameterList(DeclarationOfAccident declarationOfAccident, String tabName, List<string> columnNameList, List<OracleParameter> paramCollection, User currentUser)
        {
            //Create instance of OracleParameter object
            OracleParameter param;

            switch (tabName)
            {
                case "Header":

                    columnNameList.Add("declarationNumber");
                    param = new OracleParameter();
                    param.ParameterName = "declarationNumber";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber))
                    {
                        param.Value = declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("declarationDate");
                    param = new OracleParameter();
                    param.ParameterName = "declarationDate";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    if (declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.HasValue)
                    {
                        param.Value = declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("referenceNumber");
                    param = new OracleParameter();
                    param.ParameterName = "referenceNumber";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccident.DeclarationOfAccidentHeader.ReferenceNumber))
                    {
                        param.Value = declarationOfAccident.DeclarationOfAccidentHeader.ReferenceNumber;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("referenceDate");
                    param = new OracleParameter();
                    param.ParameterName = "referenceDate";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    if (declarationOfAccident.DeclarationOfAccidentHeader.ReferenceDate.HasValue)
                    {
                        param.Value = declarationOfAccident.DeclarationOfAccidentHeader.ReferenceDate;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("fileNumber");
                    param = new OracleParameter();
                    param.ParameterName = "fileNumber";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccident.DeclarationOfAccidentHeader.FileNumber))
                    {
                        param.Value = declarationOfAccident.DeclarationOfAccidentHeader.FileNumber;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    break;


                case "Footer":

                    columnNameList.Add("AplicantType");
                    param = new OracleParameter();
                    param.ParameterName = "AplicantType";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccident.DeclarationOfAccidentFooter != null
                        && declarationOfAccident.DeclarationOfAccidentFooter.ApplicantType > 0)
                    {
                        param.Value = declarationOfAccident.DeclarationOfAccidentFooter.ApplicantType;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);

                    columnNameList.Add("AplicantPosition");
                    param = new OracleParameter();
                    param.ParameterName = "AplicantPosition";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccident.DeclarationOfAccidentFooter.AplicantPosition))
                    {
                        param.Value = declarationOfAccident.DeclarationOfAccidentFooter.AplicantPosition;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);

                    columnNameList.Add("AplicantName");
                    param = new OracleParameter();
                    param.ParameterName = "AplicantName";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccident.DeclarationOfAccidentFooter.AplicantName))
                    {
                        param.Value = declarationOfAccident.DeclarationOfAccidentFooter.AplicantName;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);

                    break;


                case "tabEmpl":
                    //Create helper object
                    Employer employer = new Employer(currentUser);
                    //fill helper Object
                    employer = declarationOfAccident.Employer;


                    columnNameList.Add("employerId");
                    param = new OracleParameter();
                    param.ParameterName = "employerId";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (employer.EmployerId > 0)
                    {
                        param.Value = employer.EmployerId;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("emplEik");
                    param = new OracleParameter();
                    param.ParameterName = "emplEik";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplEik))
                    {
                        param.Value = employer.EmplEik;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplCityId");
                    param = new OracleParameter();
                    param.ParameterName = "emplCityId";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (employer.City.CityId > 0)
                    {
                        param.Value = employer.City.CityId;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplStreet");
                    param = new OracleParameter();
                    param.ParameterName = "emplStreet";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplStreet))
                    {
                        param.Value = employer.EmplStreet;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplStreetNum");
                    param = new OracleParameter();
                    param.ParameterName = "emplStreetNum";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplStreetNum))
                    {
                        param.Value = employer.EmplStreetNum;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplDistrict");
                    param = new OracleParameter();
                    param.ParameterName = "emplDistrict";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplDistrict))
                    {
                        param.Value = employer.EmplDistrict;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplBlock");
                    param = new OracleParameter();
                    param.ParameterName = "emplBlock";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplBlock))
                    {
                        param.Value = employer.EmplBlock;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplEntrance");
                    param = new OracleParameter();
                    param.ParameterName = "emplEntrance";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplEntrance))
                    {
                        param.Value = employer.EmplEntrance;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplFloor");
                    param = new OracleParameter();
                    param.ParameterName = "emplFloor";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplFloor))
                    {
                        param.Value = employer.EmplFloor;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplApt");
                    param = new OracleParameter();
                    param.ParameterName = "emplApt";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplApt))
                    {
                        param.Value = employer.EmplApt;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplPhone");
                    param = new OracleParameter();
                    param.ParameterName = "emplPhone";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplPhone))
                    {
                        param.Value = employer.EmplPhone;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplFax");
                    param = new OracleParameter();
                    param.ParameterName = "emplFax";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplFax))
                    {
                        param.Value = employer.EmplFax;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplEmail");
                    param = new OracleParameter();
                    param.ParameterName = "emplEmail";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(employer.EmplEmail))
                    {
                        param.Value = employer.EmplEmail;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("emplNumberOfEmployees");
                    param = new OracleParameter();
                    param.ParameterName = "emplNumberOfEmployees";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (employer.EmplNumberOfEmployees.HasValue)
                    {
                        param.Value = employer.EmplNumberOfEmployees;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("emplFemaleEmployees");
                    param = new OracleParameter();
                    param.ParameterName = "emplFemaleEmployees";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (employer.EmplFemaleEmployees.HasValue)
                    {
                        param.Value = employer.EmplFemaleEmployees;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    break;

                case "tabWorker":

                    //Create helper object
                    DeclarationOfAccidentWorker declarationOfAccidentWorker = new DeclarationOfAccidentWorker(currentUser);
                    //fill helper Object
                    declarationOfAccidentWorker = declarationOfAccident.DeclarationOfAccidentWorker;


                    columnNameList.Add("WorkerFullName");
                    param = new OracleParameter();
                    param.ParameterName = "WorkerFullName";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WorkerFullName))
                    {
                        param.Value = declarationOfAccidentWorker.WorkerFullName;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WorkerEgn");
                    param = new OracleParameter();
                    param.ParameterName = "WorkerEgn";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WorkerEgn))
                    {
                        param.Value = declarationOfAccidentWorker.WorkerEgn;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WCityId");
                    param = new OracleParameter();
                    param.ParameterName = "WCityId";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentWorker.City.CityId > 0)
                    {
                        param.Value = declarationOfAccidentWorker.City.CityId;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);




                    columnNameList.Add("WStreet");
                    param = new OracleParameter();
                    param.ParameterName = "WStreet";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WStreet))
                    {
                        param.Value = declarationOfAccidentWorker.WStreet;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WStreetNum");
                    param = new OracleParameter();
                    param.ParameterName = "WStreetNum";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WStreetNum))
                    {
                        param.Value = declarationOfAccidentWorker.WStreetNum;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WDistrict");
                    param = new OracleParameter();
                    param.ParameterName = "WDistrict";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WDistrict))
                    {
                        param.Value = declarationOfAccidentWorker.WDistrict;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WBlock");
                    param = new OracleParameter();
                    param.ParameterName = "WBlock";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WBlock))
                    {
                        param.Value = declarationOfAccidentWorker.WBlock;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WEntrance");
                    param = new OracleParameter();
                    param.ParameterName = "WEntrance";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WEntrance))
                    {
                        param.Value = declarationOfAccidentWorker.WEntrance;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WFloor");
                    param = new OracleParameter();
                    param.ParameterName = "WFloor";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WFloor))
                    {
                        param.Value = declarationOfAccidentWorker.WFloor;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WApt");
                    param = new OracleParameter();
                    param.ParameterName = "WApt";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WApt))
                    {
                        param.Value = declarationOfAccidentWorker.WApt;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WPhone");
                    param = new OracleParameter();
                    param.ParameterName = "WPhone";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WPhone))
                    {
                        param.Value = declarationOfAccidentWorker.WPhone;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WFax");
                    param = new OracleParameter();
                    param.ParameterName = "WFax";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WFax))
                    {
                        param.Value = declarationOfAccidentWorker.WFax;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WEmail");
                    param = new OracleParameter();
                    param.ParameterName = "WEmail";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WEmail))
                    {
                        param.Value = declarationOfAccidentWorker.WEmail;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WBirthDate");
                    param = new OracleParameter();
                    param.ParameterName = "WBirthDate";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    if (declarationOfAccidentWorker.WBirthDate.HasValue)
                    {
                        param.Value = declarationOfAccidentWorker.WBirthDate;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WGender");
                    param = new OracleParameter();
                    param.ParameterName = "WGender";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentWorker.WGender.HasValue)
                    {
                        param.Value = declarationOfAccidentWorker.WGender;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WCitizenship");
                    param = new OracleParameter();
                    param.ParameterName = "WCitizenship";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WCitizenship))
                    {
                        param.Value = declarationOfAccidentWorker.WCitizenship;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WHireType");
                    param = new OracleParameter();
                    param.ParameterName = "WHireType";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentWorker.WHireType.HasValue)
                    {
                        param.Value = declarationOfAccidentWorker.WHireType;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);

                    columnNameList.Add("WWorkTime");
                    param = new OracleParameter();
                    param.ParameterName = "WWorkTime";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentWorker.WWorkTime.HasValue)
                    {
                        param.Value = declarationOfAccidentWorker.WWorkTime;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WHireDate");
                    param = new OracleParameter();
                    param.ParameterName = "WHireDate";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    if (declarationOfAccidentWorker.WHireDate.HasValue)
                    {
                        param.Value = declarationOfAccidentWorker.WHireDate;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WJobTitle");
                    param = new OracleParameter();
                    param.ParameterName = "WJobTitle";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WJobTitle))
                    {
                        param.Value = declarationOfAccidentWorker.WJobTitle;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WJobCode");
                    param = new OracleParameter();
                    param.ParameterName = "WJobCode";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WJobCode))
                    {
                        param.Value = declarationOfAccidentWorker.WJobCode;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WJobCategory");
                    param = new OracleParameter();
                    param.ParameterName = "WJobCategory";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentWorker.WJobCategory.HasValue)
                    {
                        param.Value = declarationOfAccidentWorker.WJobCategory;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }

                    paramCollection.Add(param);


                    columnNameList.Add("WYearsOnService");
                    param = new OracleParameter();
                    param.ParameterName = "WYearsOnService";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentWorker.WYearsOnService.HasValue)
                    {
                        param.Value = declarationOfAccidentWorker.WYearsOnService;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);

                    columnNameList.Add("WCurrentJobYearsOnService");
                    param = new OracleParameter();
                    param.ParameterName = "WCurrentJobYearsOnService";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentWorker.WCurrentJobYearsOnService.HasValue)
                    {
                        param.Value = declarationOfAccidentWorker.WCurrentJobYearsOnService;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WBranch");
                    param = new OracleParameter();
                    param.ParameterName = "WBranch";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWorker.WBranch))
                    {
                        param.Value = declarationOfAccidentWorker.WBranch;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    break;
                case "tabAcc":
                    //Create helper object
                    DeclarationOfAccidentAcc declarationOfAccidentAcc = new DeclarationOfAccidentAcc(currentUser);
                    //fill helper Object
                    declarationOfAccidentAcc = declarationOfAccident.DeclarationOfAccidentAcc;

                    columnNameList.Add("AccDateTime");
                    param = new OracleParameter();
                    param.ParameterName = "AccDateTime";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    if (declarationOfAccidentAcc.AccDateTime.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccDateTime;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccWorkFromHour1");
                    param = new OracleParameter();
                    param.ParameterName = "AccWorkFromHour1";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccWorkFromHour1.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccWorkFromHour1;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccWorkFromMin1");
                    param = new OracleParameter();
                    param.ParameterName = "AccWorkFromMin1";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccWorkFromMin1.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccWorkFromMin1;
                    }
                    else
                    {
                        if (declarationOfAccidentAcc.AccWorkFromHour1.HasValue)
                        {
                            param.Value = 0;
                        }
                        else
                        {
                            param.Value = DBNull.Value;
                        }
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccWorkToHour1");
                    param = new OracleParameter();
                    param.ParameterName = "AccWorkToHour1";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccWorkToHour1.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccWorkToHour1;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccWorkToMin1");
                    param = new OracleParameter();
                    param.ParameterName = "AccWorkToMin1";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccWorkToMin1.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccWorkToMin1;
                    }
                    else
                    {
                        if (declarationOfAccidentAcc.AccWorkToHour1.HasValue)
                        {
                            param.Value = 0;
                        }
                        else
                        {
                            param.Value = DBNull.Value;
                        }
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccWorkFromHour2");
                    param = new OracleParameter();
                    param.ParameterName = "AccWorkFromHour2";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccWorkFromHour2.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccWorkFromHour2;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccWorkFromMin2");
                    param = new OracleParameter();
                    param.ParameterName = "AccWorkFromMin2";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccWorkFromMin2.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccWorkFromMin2;
                    }
                    else
                    {
                        if (declarationOfAccidentAcc.AccWorkFromHour2.HasValue)
                        {
                            param.Value = 0;
                        }
                        else
                        {
                            param.Value = DBNull.Value;
                        }
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccWorkToHour2");
                    param = new OracleParameter();
                    param.ParameterName = "AccWorkToHour2";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccWorkToHour2.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccWorkToHour2;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccWorkToMin2");
                    param = new OracleParameter();
                    param.ParameterName = "AccWorkToMin2";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccWorkToMin2.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccWorkToMin2;
                    }
                    else
                    {
                        if (declarationOfAccidentAcc.AccWorkToHour2.HasValue)
                        {
                            param.Value = 0;
                        }
                        else
                        {
                            param.Value = DBNull.Value;
                        }
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccPlace");
                    param = new OracleParameter();
                    param.ParameterName = "AccPlace";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccPlace))
                    {
                        param.Value = declarationOfAccidentAcc.AccPlace;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccCountry");
                    param = new OracleParameter();
                    param.ParameterName = "AccCountry";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccCountry))
                    {
                        param.Value = declarationOfAccidentAcc.AccCountry;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccCityId");
                    param = new OracleParameter();
                    param.ParameterName = "AccCityId";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.City.CityId > 0)
                    {
                        param.Value = declarationOfAccidentAcc.City.CityId;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);




                    columnNameList.Add("AccStreet");
                    param = new OracleParameter();
                    param.ParameterName = "AccStreet";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccStreet))
                    {
                        param.Value = declarationOfAccidentAcc.AccStreet;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccStreetNum");
                    param = new OracleParameter();
                    param.ParameterName = "AccStreetNum";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccStreetNum))
                    {
                        param.Value = declarationOfAccidentAcc.AccStreetNum;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccDistrict");
                    param = new OracleParameter();
                    param.ParameterName = "AccDistrict";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccDistrict))
                    {
                        param.Value = declarationOfAccidentAcc.AccDistrict;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccBlock");
                    param = new OracleParameter();
                    param.ParameterName = "AccBlock";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccBlock))
                    {
                        param.Value = declarationOfAccidentAcc.AccBlock;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccEntrance");
                    param = new OracleParameter();
                    param.ParameterName = "AccEntrance";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccEntrance))
                    {
                        param.Value = declarationOfAccidentAcc.AccEntrance;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccFloor");
                    param = new OracleParameter();
                    param.ParameterName = "AccFloor";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccFloor))
                    {
                        param.Value = declarationOfAccidentAcc.AccFloor;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccApt");
                    param = new OracleParameter();
                    param.ParameterName = "AccApt";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccApt))
                    {
                        param.Value = declarationOfAccidentAcc.AccApt;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccPhone");
                    param = new OracleParameter();
                    param.ParameterName = "AccPhone";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccPhone))
                    {
                        param.Value = declarationOfAccidentAcc.AccPhone;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccFax");
                    param = new OracleParameter();
                    param.ParameterName = "AccFax";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccFax))
                    {
                        param.Value = declarationOfAccidentAcc.AccFax;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccEmail");
                    param = new OracleParameter();
                    param.ParameterName = "AccEmail";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccEmail))
                    {
                        param.Value = declarationOfAccidentAcc.AccEmail;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccHappenedAt");
                    param = new OracleParameter();
                    param.ParameterName = "AccHappenedAt";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccHappenedAt.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccHappenedAt;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);

                    columnNameList.Add("AccHappenedOther");
                    param = new OracleParameter();
                    param.ParameterName = "AccHappenedOther";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccHappenedOther))
                    {
                        param.Value = declarationOfAccidentAcc.AccHappenedOther;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("AccJobType");
                    param = new OracleParameter();
                    param.ParameterName = "AccJobType";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccJobType))
                    {
                        param.Value = declarationOfAccidentAcc.AccJobType;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccTaskType");
                    param = new OracleParameter();
                    param.ParameterName = "AccTaskType";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccTaskType))
                    {
                        param.Value = declarationOfAccidentAcc.AccTaskType;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccDeviationFromTask");
                    param = new OracleParameter();
                    param.ParameterName = "AccDeviationFromTask";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccDeviationFromTask))
                    {
                        param.Value = declarationOfAccidentAcc.AccDeviationFromTask;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccInjurDesc");
                    param = new OracleParameter();
                    param.ParameterName = "AccInjurDesc";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccInjurDesc))
                    {
                        param.Value = declarationOfAccidentAcc.AccInjurDesc;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccInjHasRights");
                    param = new OracleParameter();
                    param.ParameterName = "AccInjHasRights";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccInjHasRights.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccInjHasRights;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }

                    paramCollection.Add(param);

                    columnNameList.Add("AccLegalRef");
                    param = new OracleParameter();
                    param.ParameterName = "AccLegalRef";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccLegalRef.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccLegalRef;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccPlannedActions");
                    param = new OracleParameter();
                    param.ParameterName = "AccPlannedActions";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentAcc.AccPlannedActions))
                    {
                        param.Value = declarationOfAccidentAcc.AccPlannedActions;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("AccLostDays");
                    param = new OracleParameter();
                    param.ParameterName = "AccLostDays";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentAcc.AccLostDays.HasValue)
                    {
                        param.Value = declarationOfAccidentAcc.AccLostDays;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    break;
                case "tabHarm":
                    //Create helper object
                    DeclarationOfAccidentHarm declarationOfAccidentHarm = new DeclarationOfAccidentHarm();
                    //Fill helper Object
                    declarationOfAccidentHarm = declarationOfAccident.DeclarationOfAccidentHarm;


                    columnNameList.Add("HarmType");
                    param = new OracleParameter();
                    param.ParameterName = "HarmType";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHarm.HarmType))
                    {
                        param.Value = declarationOfAccidentHarm.HarmType;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HarmBodyParts");
                    param = new OracleParameter();
                    param.ParameterName = "HarmBodyParts";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHarm.HarmBodyParts))
                    {
                        param.Value = declarationOfAccidentHarm.HarmBodyParts;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("HarmResult");
                    param = new OracleParameter();
                    param.ParameterName = "HarmResult";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentHarm.HarmResult.HasValue)
                    {
                        param.Value = declarationOfAccidentHarm.HarmResult;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    break;
                case "tabWith":
                    //Create helper object
                    DeclarationOfAccidentWith declarationOfAccidentWith = new DeclarationOfAccidentWith(currentUser);
                    //Fill helper Object
                    declarationOfAccidentWith = declarationOfAccident.DeclarationOfAccidentWith;


                    columnNameList.Add("WitnessFullName");
                    param = new OracleParameter();
                    param.ParameterName = "WitnessFullName";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitnessFullName))
                    {
                        param.Value = declarationOfAccidentWith.WitnessFullName;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitCityId");
                    param = new OracleParameter();
                    param.ParameterName = "WitCityId";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentWith.City.CityId > 0)
                    {
                        param.Value = declarationOfAccidentWith.City.CityId;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }

                    paramCollection.Add(param);



                    columnNameList.Add("WitStreet");
                    param = new OracleParameter();
                    param.ParameterName = "WitStreet";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitStreet))
                    {
                        param.Value = declarationOfAccidentWith.WitStreet;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitStreetNum");
                    param = new OracleParameter();
                    param.ParameterName = "WitStreetNum";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitStreetNum))
                    {
                        param.Value = declarationOfAccidentWith.WitStreetNum;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("WitDistrict");
                    param = new OracleParameter();
                    param.ParameterName = "WitDistrict";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitDistrict))
                    {
                        param.Value = declarationOfAccidentWith.WitDistrict;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitBlock");
                    param = new OracleParameter();
                    param.ParameterName = "WitBlock";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitBlock))
                    {
                        param.Value = declarationOfAccidentWith.WitBlock;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitEntrance");
                    param = new OracleParameter();
                    param.ParameterName = "WitEntrance";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitEntrance))
                    {
                        param.Value = declarationOfAccidentWith.WitEntrance;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitFloor");
                    param = new OracleParameter();
                    param.ParameterName = "WitFloor";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitFloor))
                    {
                        param.Value = declarationOfAccidentWith.WitFloor;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitApt");
                    param = new OracleParameter();
                    param.ParameterName = "WitApt";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitApt))
                    {
                        param.Value = declarationOfAccidentWith.WitApt;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitPhone");
                    param = new OracleParameter();
                    param.ParameterName = "WitPhone";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitPhone))
                    {
                        param.Value = declarationOfAccidentWith.WitPhone;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitFax");
                    param = new OracleParameter();
                    param.ParameterName = "WitFax";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitFax))
                    {
                        param.Value = declarationOfAccidentWith.WitFax;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("WitEmail");
                    param = new OracleParameter();
                    param.ParameterName = "WitEmail";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentWith.WitEmail))
                    {
                        param.Value = declarationOfAccidentWith.WitEmail;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);

                    break;
                case "tabHeir":
                    //Create helper object
                    DeclarationOfAccidentHeir declarationOfAccidentHeir = new DeclarationOfAccidentHeir(currentUser);
                    //Fill helper Object
                    declarationOfAccidentHeir = declarationOfAccident.DeclarationOfAccidentHeir;


                    columnNameList.Add("HeirFullName");
                    param = new OracleParameter();
                    param.ParameterName = "HeirFullName";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirFullName))
                    {
                        param.Value = declarationOfAccidentHeir.HeirFullName;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirEgn");
                    param = new OracleParameter();
                    param.ParameterName = "HeirEgn";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirEgn))
                    {
                        param.Value = declarationOfAccidentHeir.HeirEgn;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirCityId");
                    param = new OracleParameter();
                    param.ParameterName = "HeirCityId";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.Number;
                    if (declarationOfAccidentHeir.City.CityId > 0)
                    {
                        param.Value = declarationOfAccidentHeir.City.CityId;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("HeirStreet");
                    param = new OracleParameter();
                    param.ParameterName = "HeirStreet";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirStreet))
                    {
                        param.Value = declarationOfAccidentHeir.HeirStreet;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirStreetNum");
                    param = new OracleParameter();
                    param.ParameterName = "HeirStreetNum";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirStreetNum))
                    {
                        param.Value = declarationOfAccidentHeir.HeirStreetNum;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);



                    columnNameList.Add("HeirDistrict");
                    param = new OracleParameter();
                    param.ParameterName = "HeirDistrict";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirDistrict))
                    {
                        param.Value = declarationOfAccidentHeir.HeirDistrict;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirBlock");
                    param = new OracleParameter();
                    param.ParameterName = "HeirBlock";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirBlock))
                    {
                        param.Value = declarationOfAccidentHeir.HeirBlock;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirEntrance");
                    param = new OracleParameter();
                    param.ParameterName = "HeirEntrance";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirEntrance))
                    {
                        param.Value = declarationOfAccidentHeir.HeirEntrance;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirFloor");
                    param = new OracleParameter();
                    param.ParameterName = "HeirFloor";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirFloor))
                    {
                        param.Value = declarationOfAccidentHeir.HeirFloor;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirApt");
                    param = new OracleParameter();
                    param.ParameterName = "HeirApt";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirApt))
                    {
                        param.Value = declarationOfAccidentHeir.HeirApt;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirPhone");
                    param = new OracleParameter();
                    param.ParameterName = "HeirPhone";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirPhone))
                    {
                        param.Value = declarationOfAccidentHeir.HeirPhone;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirFax");
                    param = new OracleParameter();
                    param.ParameterName = "HeirFax";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirFax))
                    {
                        param.Value = declarationOfAccidentHeir.HeirFax;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    columnNameList.Add("HeirEmail");
                    param = new OracleParameter();
                    param.ParameterName = "HeirEmail";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    if (!string.IsNullOrEmpty(declarationOfAccidentHeir.HeirEmail))
                    {
                        param.Value = declarationOfAccidentHeir.HeirEmail;
                    }
                    else
                    {
                        param.Value = DBNull.Value;
                    }
                    paramCollection.Add(param);


                    break;

                default:

                    break;
            }

        }
        //Bind list for tarcking changes Insert/Update
        public static void AddChangeEventDetails(DeclarationOfAccident declarationOfAccident, ChangeEvent changeEvent, User currentUser)
        {
            DeclarationOfAccidentHeader declarationOfAccidentHeader;
            DeclarationOfAccidentFooter declarationOfAccidentFooter;
            Employer employer;
            DeclarationOfAccidentWorker declarationOfAccidentWorker;
            DeclarationOfAccidentAcc declarationOfAccidentAcc;
            DeclarationOfAccidentHarm declarationOfAccidentHarm;
            DeclarationOfAccidentWith declarationOfAccidentWith;
            DeclarationOfAccidentHeir declarationOfAccidentHeir;

            declarationOfAccidentHeader = declarationOfAccident.DeclarationOfAccidentHeader;
            declarationOfAccidentFooter = declarationOfAccident.DeclarationOfAccidentFooter;
            employer = declarationOfAccident.Employer;
            declarationOfAccidentWorker = declarationOfAccident.DeclarationOfAccidentWorker;
            declarationOfAccidentAcc = declarationOfAccident.DeclarationOfAccidentAcc;
            declarationOfAccidentHarm = declarationOfAccident.DeclarationOfAccidentHarm;
            declarationOfAccidentWith = declarationOfAccident.DeclarationOfAccidentWith;
            declarationOfAccidentHeir = declarationOfAccident.DeclarationOfAccidentHeir;

            if (declarationOfAccident.DeclarationId == 0) //INSERT
            {
                //TabHeader - 5 fields
                if (declarationOfAccidentHeader != null)
                {
                    if (!String.IsNullOrEmpty(declarationOfAccidentHeader.DeclarationNumber))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_DeclarationNumber", "", declarationOfAccidentHeader.DeclarationNumber, currentUser));
                    }

                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_DeclarationDate", "", CommonFunctions.FormatDate(declarationOfAccidentHeader.DeclarationDate.ToString()), currentUser));

                    if (!String.IsNullOrEmpty(declarationOfAccidentHeader.ReferenceNumber))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_ReferenceNumber", "", declarationOfAccidentHeader.ReferenceNumber, currentUser));
                    }

                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_ReferenceDate", "", CommonFunctions.FormatDate(declarationOfAccidentHeader.ReferenceDate.ToString()), currentUser));

                    if (!String.IsNullOrEmpty(declarationOfAccidentHeader.FileNumber))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_FileNumber", "", declarationOfAccidentHeader.FileNumber, currentUser));
                    }
                }

                //TabFooter - 3 fields
                if (declarationOfAccidentFooter != null)
                {
                    if (declarationOfAccidentFooter.ApplicantType > 0)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantType", "", declarationOfAccidentFooter.ApplicantType.ToString(), currentUser));
                    }

                    if (!String.IsNullOrEmpty(declarationOfAccidentFooter.AplicantPosition))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantPosition", "", declarationOfAccidentFooter.AplicantPosition, currentUser));
                    }

                    if (!String.IsNullOrEmpty(declarationOfAccidentFooter.AplicantName))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantName", "", declarationOfAccidentFooter.AplicantName, currentUser));
                    }
                }

                //TabEmpl - 15 fields
                //if (declarationOfAccidentEmpl != null)
                //{
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmployerId", "", declarationOfAccidentEmpl.EmployerId.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEik", "", declarationOfAccidentEmpl.EmplEik, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplCityId", "", declarationOfAccidentEmpl.EmplCityId.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplStreet", "", declarationOfAccidentEmpl.EmplStreet, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplStreetNum", "", declarationOfAccidentEmpl.EmplStreetNum, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplDistrict", "", declarationOfAccidentEmpl.EmplDistrict, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplBlock", "", declarationOfAccidentEmpl.EmplBlock, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEntrance", "", declarationOfAccidentEmpl.EmplEntrance, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFloor", "", declarationOfAccidentEmpl.EmplFloor, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplApt", "", declarationOfAccidentEmpl.EmplApt, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplPhone", "", declarationOfAccidentEmpl.EmplPhone, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFax", "", declarationOfAccidentEmpl.EmplFax, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEmail", "", declarationOfAccidentEmpl.EmplEmail, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplNumberOfEmployees", "", declarationOfAccidentEmpl.EmplNumberOfEmployees.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFemaleEmployees", "", declarationOfAccidentEmpl.EmplFemaleEmployees.ToString(), currentUser));
                //}
                //TabWorker - 24 fields
                if (declarationOfAccidentWorker != null)
                {
                    if (!String.IsNullOrEmpty(declarationOfAccidentWorker.WorkerFullName))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WorkerFullName", "", declarationOfAccidentWorker.WorkerFullName, currentUser));
                    }
                    if (!String.IsNullOrEmpty(declarationOfAccidentWorker.WorkerEgn))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WorkerEgn", "", declarationOfAccidentWorker.WorkerEgn, currentUser));
                    }

                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WCityId", "", declarationOfAccidentWorker.WCityId.ToString(), currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WStreet", "", declarationOfAccidentWorker.WStreet, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WStreetNum", "", declarationOfAccidentWorker.WStreetNum, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WDistrict", "", declarationOfAccidentWorker.WDistrict, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBlock", "", declarationOfAccidentWorker.WBlock, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WEntrance", "", declarationOfAccidentWorker.WEntrance, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WFloor", "", declarationOfAccidentWorker.WFloor, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WApt", "", declarationOfAccidentWorker.WApt, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WPhone", "", declarationOfAccidentWorker.WPhone, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WFax", "", declarationOfAccidentWorker.WFax, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WEmail", "", declarationOfAccidentWorker.WEmail, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBirthDate", "", CommonFunctions.FormatDate(declarationOfAccidentWorker.WBirthDate.ToString()), currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WGender", "", declarationOfAccidentWorker.WGender.ToString(), currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WCitizenship", "", declarationOfAccidentWorker.WCitizenship, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WHireType", "", declarationOfAccidentWorker.WHireType.ToString(), currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WWorkTime", "", declarationOfAccidentWorker.WWorkTime.ToString(), currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WHireDate", "", CommonFunctions.FormatDate(declarationOfAccidentWorker.WHireDate.ToString()), currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobTitle", "", declarationOfAccidentWorker.WJobTitle, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobCode", "", declarationOfAccidentWorker.WJobCode, currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobCategory", "", declarationOfAccidentWorker.WJobCategory.ToString(), currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WYearsOnService", "", declarationOfAccidentWorker.WYearsOnService.ToString(), currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBranch", "", declarationOfAccidentWorker.WBranch, currentUser));
                }
                //TabAcc - 23 fields
                //if (declarationOfAccidentAcc != null)
                //{
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccDateTime", "", CommonFunctions.FormatDate(declarationOfAccidentAcc.AccDateTime), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkFromHour1", "", declarationOfAccidentAcc.AccWorkFromHour1.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkFromMin1", "", declarationOfAccidentAcc.AccWorkFromMin1.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkToHour1", "", declarationOfAccidentAcc.AccWorkToHour1.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkToMin1", "", declarationOfAccidentAcc.AccWorkToMin1.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkFromHour2", "", declarationOfAccidentAcc.AccWorkFromHour2.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkFromMin2", "", declarationOfAccidentAcc.AccWorkFromMin2.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkToHour2", "", declarationOfAccidentAcc.AccWorkToHour2.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkToMin2", "", declarationOfAccidentAcc.AccWorkToMin2.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPlace", "", declarationOfAccidentAcc.AccPlace, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccCityId", "", declarationOfAccidentAcc.AccCityId.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccStreet", "", declarationOfAccidentAcc.AccStreet, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccStreetNum", "", declarationOfAccidentAcc.AccStreetNum, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPhone", "", declarationOfAccidentAcc.AccPhone, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccHappenedAt", "", declarationOfAccidentAcc.AccHappenedAt.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccJobType", "", declarationOfAccidentAcc.AccJobType, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccTaskType", "", declarationOfAccidentAcc.AccTaskType, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccDeviationFromTask", "", declarationOfAccidentAcc.AccDeviationFromTask, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccInjurDesc", "", declarationOfAccidentAcc.AccInjurDesc, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccInjHasRights", "", declarationOfAccidentAcc.AccInjHasRights.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccLegalRef", "", declarationOfAccidentAcc.AccLegalRef.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPlannedActions", "", declarationOfAccidentAcc.AccPlannedActions, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccLostDays", "", declarationOfAccidentAcc.AccLostDays.ToString(), currentUser));
                //}
                ////TabHarm - 3 fields
                //if (declarationOfAccidentHarm != null)
                //{
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmType", "", declarationOfAccidentHarm.HarmType, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmBodyParts", "", declarationOfAccidentHarm.HarmBodyParts, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmResult", "", declarationOfAccidentHarm.HarmResult.ToString(), currentUser));
                //}
                ////TabWith - 12 fields
                //if (declarationOfAccidentWith != null)
                //{
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitnessFullName", "", declarationOfAccidentWith.WitnessFullName, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitCityId", "", declarationOfAccidentWith.WitCityId.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitStreet", "", declarationOfAccidentWith.WitStreet, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitStreetNum", "", declarationOfAccidentWith.WitStreetNum, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitDistrict", "", declarationOfAccidentWith.WitDistrict, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitBlock", "", declarationOfAccidentWith.WitBlock, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitEntrance", "", declarationOfAccidentWith.WitEntrance, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitFloor", "", declarationOfAccidentWith.WitFloor, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitApt", "", declarationOfAccidentWith.WitApt, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitPhone", "", declarationOfAccidentWith.WitPhone, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitFax", "", declarationOfAccidentWith.WitFax, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitEmail", "", declarationOfAccidentWith.WitEmail, currentUser));
                //}
                ////TabHeir - 13 fields
                //if (declarationOfAccidentHeir != null)
                //{
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFullName", "", declarationOfAccidentHeir.HeirFullName, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEgn", "", declarationOfAccidentHeir.HeirEgn, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirCityid", "", declarationOfAccidentHeir.HeirCityId.ToString(), currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirStreet", "", declarationOfAccidentHeir.HeirStreet, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirStreetNum", "", declarationOfAccidentHeir.HeirStreetNum, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirDistrict", "", declarationOfAccidentHeir.HeirDistrict, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirBlock", "", declarationOfAccidentHeir.HeirBlock, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEntrance", "", declarationOfAccidentHeir.HeirEntrance, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFloor", "", declarationOfAccidentHeir.HeirFloor, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirApt", "", declarationOfAccidentHeir.HeirApt, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirPhone", "", declarationOfAccidentHeir.HeirPhone, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFax", "", declarationOfAccidentHeir.HeirFax, currentUser));
                //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEmail", "", declarationOfAccidentHeir.HeirEmail, currentUser));
                //}
            }
            else //UPDATE
            {
                //Define local object
                DeclarationOfAccidentHeader oldDeclarationOfAccidentHeader;
                DeclarationOfAccidentFooter oldDeclarationOfAccidentFooter;
                Employer oldEmployer;
                DeclarationOfAccidentWorker oldDeclarationOfAccidentWorker;
                DeclarationOfAccidentAcc oldDeclarationOfAccidentAcc;
                DeclarationOfAccidentHarm oldDeclarationOfAccidentHarm;
                DeclarationOfAccidentWith oldDeclarationOfAccidentWith;
                DeclarationOfAccidentHeir oldDeclarationOfAccidentHeir;

                DeclarationOfAccident oldDeclarationOfAccident;

                //Create old object

                oldDeclarationOfAccident = GetDeclarationOfAccident(declarationOfAccident.DeclarationId, currentUser, "btnTabEmpl");

                oldDeclarationOfAccidentHeader = oldDeclarationOfAccident.DeclarationOfAccidentHeader;
                oldDeclarationOfAccidentFooter = oldDeclarationOfAccident.DeclarationOfAccidentFooter;


                //TabHeader
                if (declarationOfAccidentHeader != null)
                {
                    if (oldDeclarationOfAccidentHeader.DeclarationNumber.Trim() !=
                              declarationOfAccidentHeader.DeclarationNumber.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_DeclarationNumber",
                                      oldDeclarationOfAccidentHeader.DeclarationNumber,
                                         declarationOfAccidentHeader.DeclarationNumber, currentUser));
                    }
                    if (oldDeclarationOfAccidentHeader.DeclarationDate !=
                             declarationOfAccidentHeader.DeclarationDate)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_DeclarationDate",
                                      CommonFunctions.FormatDate(oldDeclarationOfAccidentHeader.DeclarationDate),
                                         CommonFunctions.FormatDate(declarationOfAccidentHeader.DeclarationDate), currentUser));
                    }
                    if (oldDeclarationOfAccidentHeader.ReferenceNumber.Trim() !=
                             declarationOfAccidentHeader.ReferenceNumber.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_ReferenceNumber",
                                      oldDeclarationOfAccidentHeader.ReferenceNumber,
                                         declarationOfAccidentHeader.ReferenceNumber, currentUser));
                    }
                    //if (oldDeclarationOfAccidentHeader.ReferenceDate !=
                    //        declarationOfAccidentHeader.ReferenceDate)
                    //{
                    //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_ReferenceDate",
                    //                  CommonFunctions.FormatDate(oldDeclarationOfAccidentHeader.ReferenceDate),
                    //                     CommonFunctions.FormatDate(declarationOfAccidentHeader.ReferenceDate), currentUser));
                    //}
                    if (oldDeclarationOfAccidentHeader.FileNumber.Trim() !=
                            declarationOfAccidentHeader.FileNumber.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_FileNumber",
                                      oldDeclarationOfAccidentHeader.FileNumber,
                                         declarationOfAccidentHeader.FileNumber, currentUser));
                    }
                }

                //TabFooter
                if (declarationOfAccidentFooter != null)
                {
                    if (oldDeclarationOfAccidentFooter.ApplicantType !=
                           declarationOfAccidentFooter.ApplicantType)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantType",
                                      oldDeclarationOfAccidentFooter.ApplicantType.ToString(),
                                          declarationOfAccidentFooter.ApplicantType.ToString(), currentUser));
                    }

                    if (oldDeclarationOfAccidentFooter.AplicantPosition.Trim() !=
                              declarationOfAccidentFooter.AplicantPosition.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantPosition",
                                      oldDeclarationOfAccidentFooter.AplicantPosition,
                                         declarationOfAccidentFooter.AplicantPosition, currentUser));
                    }

                    if (oldDeclarationOfAccidentFooter.AplicantName.Trim() !=
                             declarationOfAccidentFooter.AplicantName.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantName",
                                      oldDeclarationOfAccidentFooter.AplicantName,
                                         declarationOfAccidentFooter.AplicantName, currentUser));
                    }
                }

                //TabEmpl

                oldEmployer = oldDeclarationOfAccident.Employer;

                if (oldEmployer != null)
                {
                    if (oldEmployer.EmployerId !=
                           employer.EmployerId)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmployerId",
                                      oldEmployer.EmployerName,
                                          employer.EmployerName, currentUser));
                    }
                    if (oldEmployer.EmplEik.Trim() !=
                            employer.EmplEik.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEik",
                                      oldEmployer.EmplEik,
                                         employer.EmplEik, currentUser));
                    }
                    if (oldEmployer.City.CityId !=
                           employer.City.CityId)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplCityId",
                                      oldEmployer.City.CityName,
                                          employer.City.CityName, currentUser));
                    }
                    if (oldEmployer.EmplStreet.Trim() !=
                            employer.EmplStreet.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplStreet",
                                      oldEmployer.EmplStreet,
                                        employer.EmplStreet, currentUser));
                    }
                    if (oldEmployer.EmplStreetNum.Trim() !=
                            employer.EmplStreetNum.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplStreetNum",
                                      oldEmployer.EmplStreetNum,
                                         employer.EmplStreetNum, currentUser));
                    }
                    if (oldEmployer.EmplDistrict.Trim() !=
                           employer.EmplDistrict.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplDistrict",
                                      oldEmployer.EmplDistrict,
                                         employer.EmplDistrict, currentUser));
                    }
                    if (oldEmployer.EmplBlock.Trim() !=
                           employer.EmplBlock.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplBlock",
                                      oldEmployer.EmplBlock,
                                         employer.EmplBlock, currentUser));
                    }
                    if (oldEmployer.EmplEntrance.Trim() !=
                          employer.EmplEntrance.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEntrance",
                                      oldEmployer.EmplEntrance,
                                         employer.EmplEntrance, currentUser));
                    }
                    if (oldEmployer.EmplFloor.Trim() !=
                         employer.EmplFloor.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFloor",
                                      oldEmployer.EmplFloor,
                                         employer.EmplFloor, currentUser));
                    }
                    if (oldEmployer.EmplApt.Trim() !=
                        employer.EmplApt.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplApt",
                                      oldEmployer.EmplApt,
                                         employer.EmplApt, currentUser));
                    }
                    if (oldEmployer.EmplPhone.Trim() !=
                        employer.EmplPhone.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplPhone",
                                      oldEmployer.EmplPhone,
                                         employer.EmplPhone, currentUser));
                    }
                    if (oldEmployer.EmplFax.Trim() !=
                        employer.EmplFax.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFax",
                                      oldEmployer.EmplFax,
                                         employer.EmplFax, currentUser));
                    }
                    if (oldEmployer.EmplEmail.Trim() !=
                       employer.EmplEmail.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEmail",
                                      oldEmployer.EmplEmail,
                                         employer.EmplEmail, currentUser));
                    }
                    if (oldEmployer.EmplNumberOfEmployees !=
                      employer.EmplNumberOfEmployees)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplNumberOfEmployees",
                                      oldEmployer.EmplNumberOfEmployees.ToString(),
                                         employer.EmplNumberOfEmployees.ToString(), currentUser));
                    }
                    if (oldEmployer.EmplFemaleEmployees !=
                     employer.EmplFemaleEmployees)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFemaleEmployees",
                                      oldEmployer.EmplFemaleEmployees.ToString(),
                                         employer.EmplFemaleEmployees.ToString(), currentUser));
                    }
                }
                //TabWorker

                //Create old object
                oldDeclarationOfAccident = GetDeclarationOfAccident(declarationOfAccident.DeclarationId, currentUser, "btnTabWorker");
                oldDeclarationOfAccidentWorker = oldDeclarationOfAccident.DeclarationOfAccidentWorker;

                if (declarationOfAccidentWorker != null)
                {
                    if (oldDeclarationOfAccidentWorker.WorkerFullName.Trim() !=
                           declarationOfAccidentWorker.WorkerFullName.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WorkerFullName",
                                      oldDeclarationOfAccidentWorker.WorkerFullName,
                                         declarationOfAccidentWorker.WorkerFullName, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WorkerEgn.Trim() !=
                           declarationOfAccidentWorker.WorkerEgn.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WorkerEgn",
                                      oldDeclarationOfAccidentWorker.WorkerEgn,
                                         declarationOfAccidentWorker.WorkerEgn, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.City.CityId !=
                           declarationOfAccidentWorker.City.CityId)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WCityId",
                                      oldDeclarationOfAccidentWorker.City.CityName,
                                         declarationOfAccidentWorker.City.CityName, currentUser));
                    }

                    if (oldDeclarationOfAccidentWorker.WStreet.Trim() !=
        declarationOfAccidentWorker.WStreet.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WStreet",
                                      oldDeclarationOfAccidentWorker.WStreet,
                                        declarationOfAccidentWorker.WStreet, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WStreetNum.Trim() !=
                            declarationOfAccidentWorker.WStreetNum.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WStreetNum",
                                      oldDeclarationOfAccidentWorker.WStreetNum,
                                         declarationOfAccidentWorker.WStreetNum, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WDistrict.Trim() !=
                           declarationOfAccidentWorker.WDistrict.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WDistrict",
                                      oldDeclarationOfAccidentWorker.WDistrict,
                                         declarationOfAccidentWorker.WDistrict, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WBlock.Trim() !=
                           declarationOfAccidentWorker.WBlock.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBlock",
                                      oldDeclarationOfAccidentWorker.WBlock,
                                         declarationOfAccidentWorker.WBlock, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WEntrance.Trim() !=
                          declarationOfAccidentWorker.WEntrance.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WEntrance",
                                      oldDeclarationOfAccidentWorker.WEntrance,
                                         declarationOfAccidentWorker.WEntrance, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WFloor.Trim() !=
                         declarationOfAccidentWorker.WFloor.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WFloor",
                                      oldDeclarationOfAccidentWorker.WFloor,
                                         declarationOfAccidentWorker.WFloor, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WApt.Trim() !=
                        declarationOfAccidentWorker.WApt.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WApt",
                                      oldDeclarationOfAccidentWorker.WApt,
                                         declarationOfAccidentWorker.WApt, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WPhone.Trim() !=
                        declarationOfAccidentWorker.WPhone.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WPhone",
                                      oldDeclarationOfAccidentWorker.WPhone,
                                         declarationOfAccidentWorker.WPhone, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WFax.Trim() !=
                        declarationOfAccidentWorker.WFax.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WFax",
                                      oldDeclarationOfAccidentWorker.WFax,
                                         declarationOfAccidentWorker.WFax, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WEmail.Trim() !=
                       declarationOfAccidentWorker.WEmail.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WEmail",
                                      oldDeclarationOfAccidentWorker.WEmail,
                                         declarationOfAccidentWorker.WEmail, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WBirthDate !=
                       declarationOfAccidentWorker.WBirthDate)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBirthDate",
                                    CommonFunctions.FormatDate(oldDeclarationOfAccidentWorker.WBirthDate),
                                         CommonFunctions.FormatDate(declarationOfAccidentWorker.WBirthDate), currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WGender !=
                      declarationOfAccidentWorker.WGender)
                    {
                        string newGender = "";
                        string oldGender = "";

                        string s1 = "мъж";
                        string s2 = "жена";

                        switch (oldDeclarationOfAccidentWorker.WGender)
                        {
                            case 1:
                                oldGender = s1;
                                break;
                            case 2:
                                oldGender = s2;
                                break;
                            default:
                                break;
                        }

                        switch (declarationOfAccidentWorker.WGender)
                        {
                            case 1:
                                newGender = s1;
                                break;
                            case 2:
                                newGender = s2;
                                break;
                            default:
                                break;
                        }


                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WGender",
                                         oldGender,
                                         newGender, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WCitizenship.Trim() !=
                      declarationOfAccidentWorker.WCitizenship.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WCitizenship",
                                      oldDeclarationOfAccidentWorker.WCitizenship,
                                         declarationOfAccidentWorker.WCitizenship, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WHireType !=
                     declarationOfAccidentWorker.WHireType)
                    {
                        string newWHireType = "";
                        string oldWHireType = "";

                        string s1 = "пълно работно време";
                        string s2 = "непълно работно време";

                        switch (oldDeclarationOfAccidentWorker.WHireType)
                        {
                            case 1:
                                oldWHireType = s1;
                                break;
                            case 2:
                                oldWHireType = s2;
                                break;
                            default:
                                break;
                        }

                        switch (declarationOfAccidentWorker.WHireType)
                        {
                            case 1:
                                newWHireType = s1;
                                break;
                            case 2:
                                newWHireType = s2;
                                break;
                            default:
                                break;
                        }


                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WHireType",
                                         oldWHireType,
                                         newWHireType, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WWorkTime !=
                     declarationOfAccidentWorker.WWorkTime)
                    {
                        string newWWorkTime = "";
                        string oldWWorkTime = "";

                        string s1 = "пълно работно време";
                        string s2 = "непълно работно време";

                        switch (oldDeclarationOfAccidentWorker.WWorkTime)
                        {
                            case 1:
                                oldWWorkTime = s1;
                                break;
                            case 2:
                                oldWWorkTime = s2;
                                break;
                            default:
                                break;
                        }

                        switch (declarationOfAccidentWorker.WWorkTime)
                        {
                            case 1:
                                newWWorkTime = s1;
                                break;
                            case 2:
                                newWWorkTime = s2;
                                break;
                            default:
                                break;
                        }

                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WWorkTime",
                                      oldWWorkTime,
                                      newWWorkTime, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WHireDate !=
                     declarationOfAccidentWorker.WHireDate)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WHireDate",
                                      oldDeclarationOfAccidentWorker.WHireDate.ToString(),
                                         declarationOfAccidentWorker.WHireDate.ToString(), currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WJobTitle.Trim() !=
                     declarationOfAccidentWorker.WJobTitle.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobTitle",
                                      oldDeclarationOfAccidentWorker.WJobTitle,
                                         declarationOfAccidentWorker.WJobTitle, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WJobCode.Trim() !=
                    declarationOfAccidentWorker.WJobCode.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobCode",
                                      oldDeclarationOfAccidentWorker.WJobCode,
                                         declarationOfAccidentWorker.WJobCode, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WJobCategory !=
                   declarationOfAccidentWorker.WJobCategory)
                    {
                        string newWJobCategory = "";
                        string oldWJobCategory = "";

                        string s1 = "първа";
                        string s2 = "втора";
                        string s3 = "трета";

                        switch (oldDeclarationOfAccidentWorker.WJobCategory)
                        {
                            case 1:
                                oldWJobCategory = s1;
                                break;
                            case 2:
                                oldWJobCategory = s2;
                                break;
                            case 3:
                                oldWJobCategory = s3;
                                break;
                            default:
                                break;
                        }

                        switch (declarationOfAccidentWorker.WJobCategory)
                        {
                            case 1:
                                newWJobCategory = s1;
                                break;
                            case 2:
                                newWJobCategory = s2;
                                break;
                            case 3:
                                newWJobCategory = s3;
                                break;
                            default:
                                break;
                        }

                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobCategory",
                                      oldWJobCategory,
                                      newWJobCategory, currentUser));
                    }
                    if (oldDeclarationOfAccidentWorker.WYearsOnService !=
                     declarationOfAccidentWorker.WYearsOnService)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WYearsOnService",
                                      oldDeclarationOfAccidentWorker.WYearsOnService.ToString(),
                                         declarationOfAccidentWorker.WYearsOnService.ToString(), currentUser));
                    }

                    if (oldDeclarationOfAccidentWorker.WCurrentJobYearsOnService !=
                     declarationOfAccidentWorker.WCurrentJobYearsOnService)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WCurrentJobYearsOnService",
                                      oldDeclarationOfAccidentWorker.WCurrentJobYearsOnService.ToString(),
                                         declarationOfAccidentWorker.WCurrentJobYearsOnService.ToString(), currentUser));
                    }

                    if (oldDeclarationOfAccidentWorker.WBranch.Trim() !=
                   declarationOfAccidentWorker.WBranch.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBranch",
                                      oldDeclarationOfAccidentWorker.WBranch,
                                         declarationOfAccidentWorker.WBranch, currentUser));
                    }
                }

                //TabAcc
                oldDeclarationOfAccident = GetDeclarationOfAccident(declarationOfAccident.DeclarationId, currentUser, "btnTabAcc");
                oldDeclarationOfAccidentAcc = oldDeclarationOfAccident.DeclarationOfAccidentAcc;

                if (declarationOfAccidentAcc != null)
                {
                    string min1;
                    string min2;
                    string time1;
                    string time2;

                    if (oldDeclarationOfAccidentAcc.AccDateTime !=
                                         declarationOfAccidentAcc.AccDateTime)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccDateTime",
                                    PMIS.Common.CommonFunctions.FormatDateTimeShort(oldDeclarationOfAccidentAcc.AccDateTime),
                                        PMIS.Common.CommonFunctions.FormatDateTimeShort(declarationOfAccidentAcc.AccDateTime), currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccWorkFromHour1 != declarationOfAccidentAcc.AccWorkFromHour1
                     || oldDeclarationOfAccidentAcc.AccWorkFromMin1 != declarationOfAccidentAcc.AccWorkFromMin1)
                    {
                        bool doRecords = true;

                        if (!oldDeclarationOfAccidentAcc.AccWorkFromMin1.HasValue && declarationOfAccidentAcc.AccWorkFromMin1.Value == 0)
                        {
                            doRecords = false;
                        }
                        if (!declarationOfAccidentAcc.AccWorkFromMin1.HasValue && oldDeclarationOfAccidentAcc.AccWorkFromMin1.Value == 0)
                        {
                            doRecords = false;
                        }

                        if (doRecords)
                        {
                            min1 = oldDeclarationOfAccidentAcc.AccWorkFromMin1 != null ? oldDeclarationOfAccidentAcc.AccWorkFromMin1.ToString() : "00";
                            min2 = declarationOfAccidentAcc.AccWorkFromMin1 != null ? declarationOfAccidentAcc.AccWorkFromMin1.ToString() : "00";

                            min1 = min1.Length != 1 ? min1 : "0" + min1;
                            min2 = min2.Length != 1 ? min2 : "0" + min2;

                            min1 = oldDeclarationOfAccidentAcc.AccWorkFromHour1 != null ? min1 : "";
                            min2 = declarationOfAccidentAcc.AccWorkFromHour1 != null ? min2 : "";

                            time1 = oldDeclarationOfAccidentAcc.AccWorkFromHour1.ToString() + ":" + min1;
                            time2 = declarationOfAccidentAcc.AccWorkFromHour1.ToString() + ":" + min2;

                            time1 = time1 == ":" ? "" : time1;
                            time2 = time2 == ":" ? "" : time2;

                            changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkTime1From", time1, time2, currentUser));
                        }
                    }

                    if (oldDeclarationOfAccidentAcc.AccWorkToHour1 != declarationOfAccidentAcc.AccWorkToHour1
                     || oldDeclarationOfAccidentAcc.AccWorkToMin1 != declarationOfAccidentAcc.AccWorkToMin1)
                    {
                        bool doRecords = true;

                        if (!oldDeclarationOfAccidentAcc.AccWorkToMin1.HasValue && declarationOfAccidentAcc.AccWorkToMin1.Value == 0)
                        {
                            doRecords = false;
                        }
                        if (!declarationOfAccidentAcc.AccWorkToMin1.HasValue && oldDeclarationOfAccidentAcc.AccWorkToMin1.Value == 0)
                        {
                            doRecords = false;
                        }

                        if (doRecords)
                        {
                            min1 = oldDeclarationOfAccidentAcc.AccWorkToMin1 != null ? oldDeclarationOfAccidentAcc.AccWorkToMin1.ToString() : "00";
                            min2 = declarationOfAccidentAcc.AccWorkToMin1 != null ? declarationOfAccidentAcc.AccWorkToMin1.ToString() : "00";

                            min1 = min1.Length != 1 ? min1 : "0" + min1;
                            min2 = min2.Length != 1 ? min2 : "0" + min2;

                            min1 = oldDeclarationOfAccidentAcc.AccWorkToHour1 != null ? min1 : "";
                            min2 = declarationOfAccidentAcc.AccWorkToHour1 != null ? min2 : "";

                            time1 = oldDeclarationOfAccidentAcc.AccWorkToHour1.ToString() + ":" + min1;
                            time2 = declarationOfAccidentAcc.AccWorkToHour1.ToString() + ":" + min2;

                            time1 = time1 == ":" ? "" : time1;
                            time2 = time2 == ":" ? "" : time2;

                            changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkTime1To", time1, time2, currentUser));
                        }
                    }


                    if (oldDeclarationOfAccidentAcc.AccWorkFromHour2 != declarationOfAccidentAcc.AccWorkFromHour2
                     || oldDeclarationOfAccidentAcc.AccWorkFromMin2 != declarationOfAccidentAcc.AccWorkFromMin2)
                    {
                        bool doRecords = true;

                        if (!oldDeclarationOfAccidentAcc.AccWorkFromMin2.HasValue && declarationOfAccidentAcc.AccWorkFromMin2.Value == 0)
                        {
                            doRecords = false;
                        }
                        if (!declarationOfAccidentAcc.AccWorkFromMin2.HasValue && oldDeclarationOfAccidentAcc.AccWorkFromMin2.Value == 0)
                        {
                            doRecords = false;
                        }

                        if (doRecords)
                        {
                            min1 = oldDeclarationOfAccidentAcc.AccWorkFromMin2 != null ? oldDeclarationOfAccidentAcc.AccWorkFromMin2.ToString() : "00";
                            min2 = declarationOfAccidentAcc.AccWorkFromMin2 != null ? declarationOfAccidentAcc.AccWorkFromMin2.ToString() : "00";

                            min1 = min1.Length != 1 ? min1 : "0" + min1;
                            min2 = min2.Length != 1 ? min2 : "0" + min2;

                            min1 = oldDeclarationOfAccidentAcc.AccWorkFromHour2 != null ? min1 : "";
                            min2 = declarationOfAccidentAcc.AccWorkFromHour2 != null ? min2 : "";

                            time1 = oldDeclarationOfAccidentAcc.AccWorkFromHour2.ToString() + ":" + min1;
                            time2 = declarationOfAccidentAcc.AccWorkFromHour2.ToString() + ":" + min2;

                            time1 = time1 == ":" ? "" : time1;
                            time2 = time2 == ":" ? "" : time2;

                            changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkTime2From", time1, time2, currentUser));
                        }
                    }

                    if (oldDeclarationOfAccidentAcc.AccWorkToHour2 != declarationOfAccidentAcc.AccWorkToHour2
                    || oldDeclarationOfAccidentAcc.AccWorkToMin2 != declarationOfAccidentAcc.AccWorkToMin2)
                    {
                        bool doRecords = true;

                        if (!oldDeclarationOfAccidentAcc.AccWorkToMin2.HasValue && declarationOfAccidentAcc.AccWorkToMin2.Value == 0)
                        {
                            doRecords = false;
                        }
                        if (!declarationOfAccidentAcc.AccWorkToMin2.HasValue && oldDeclarationOfAccidentAcc.AccWorkToMin2.Value == 0)
                        {
                            doRecords = false;
                        }

                        if (doRecords)
                        {
                            min1 = oldDeclarationOfAccidentAcc.AccWorkToMin2 != null ? oldDeclarationOfAccidentAcc.AccWorkToMin2.ToString() : "00";
                            min2 = declarationOfAccidentAcc.AccWorkToMin2 != null ? declarationOfAccidentAcc.AccWorkToMin2.ToString() : "00";

                            min1 = min1.Length != 1 ? min1 : "0" + min1;
                            min2 = min2.Length != 1 ? min2 : "0" + min2;

                            min1 = oldDeclarationOfAccidentAcc.AccWorkToHour2 != null ? min1 : "";
                            min2 = declarationOfAccidentAcc.AccWorkToHour2 != null ? min2 : "";

                            time1 = oldDeclarationOfAccidentAcc.AccWorkToHour2.ToString() + ":" + min1;
                            time2 = declarationOfAccidentAcc.AccWorkToHour2.ToString() + ":" + min2;

                            time1 = time1 == ":" ? "" : time1;
                            time2 = time2 == ":" ? "" : time2;

                            changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkTime2To", time1, time2, currentUser));
                        }
                    }

                    if (oldDeclarationOfAccidentAcc.AccPlace.Trim() !=
                                                          declarationOfAccidentAcc.AccPlace.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPlace",
                                      oldDeclarationOfAccidentAcc.AccPlace,
                                         declarationOfAccidentAcc.AccPlace, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccCountry.Trim() !=
                                                          declarationOfAccidentAcc.AccCountry.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccCountry",
                                      oldDeclarationOfAccidentAcc.AccCountry,
                                         declarationOfAccidentAcc.AccCountry, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.City.CityId !=
                                                          declarationOfAccidentAcc.City.CityId)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccCityId",
                                      oldDeclarationOfAccidentAcc.City.CityName,
                                         declarationOfAccidentAcc.City.CityName, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccStreet.Trim() !=
                                                          declarationOfAccidentAcc.AccStreet.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccStreet",
                                      oldDeclarationOfAccidentAcc.AccStreet,
                                         declarationOfAccidentAcc.AccStreet, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccStreetNum.Trim() !=
                                                          declarationOfAccidentAcc.AccStreetNum.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccStreetNum",
                                      oldDeclarationOfAccidentAcc.AccStreetNum,
                                         declarationOfAccidentAcc.AccStreetNum, currentUser));
                    }

                    if (oldDeclarationOfAccidentAcc.AccDistrict.Trim() !=
                           declarationOfAccidentAcc.AccDistrict.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccDistrict",
                                      oldDeclarationOfAccidentAcc.AccDistrict,
                                         declarationOfAccidentAcc.AccDistrict, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccBlock.Trim() !=
                           declarationOfAccidentAcc.AccBlock.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccBlock",
                                      oldDeclarationOfAccidentAcc.AccBlock,
                                         declarationOfAccidentAcc.AccBlock, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccEntrance.Trim() !=
                          declarationOfAccidentAcc.AccEntrance.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccEntrance",
                                      oldDeclarationOfAccidentAcc.AccEntrance,
                                         declarationOfAccidentAcc.AccEntrance, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccFloor.Trim() !=
                         declarationOfAccidentAcc.AccFloor.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccFloor",
                                      oldDeclarationOfAccidentAcc.AccFloor,
                                         declarationOfAccidentAcc.AccFloor, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccApt.Trim() !=
                        declarationOfAccidentAcc.AccApt.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccApt",
                                      oldDeclarationOfAccidentAcc.AccApt,
                                         declarationOfAccidentAcc.AccApt, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccPhone.Trim() !=
                        declarationOfAccidentAcc.AccPhone.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPhone",
                                      oldDeclarationOfAccidentAcc.AccPhone,
                                         declarationOfAccidentAcc.AccPhone, currentUser));
                    }

                    if (oldDeclarationOfAccidentAcc.AccFax.Trim() !=
                        declarationOfAccidentAcc.AccFax.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccFax",
                                      oldDeclarationOfAccidentAcc.AccFax,
                                         declarationOfAccidentAcc.AccFax, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccEmail.Trim() !=
                       declarationOfAccidentAcc.AccEmail.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccEmail",
                                      oldDeclarationOfAccidentAcc.AccEmail,
                                         declarationOfAccidentAcc.AccEmail, currentUser));
                    }

                    string newAccHappenedAt = "";
                    string oldAccHappenedAt = "";

                    if (oldDeclarationOfAccidentAcc.AccHappenedAt != declarationOfAccidentAcc.AccHappenedAt)
                    {
                        string s1 = "обичайното стационарно работно място";
                        string s2 = "случайно, нестационарно (мобилно), временно работно място";

                        switch (oldDeclarationOfAccidentAcc.AccHappenedAt)
                        {
                            case 1:
                                oldAccHappenedAt = s1;
                                break;
                            case 2:
                                oldAccHappenedAt = s2;
                                break;
                            case 3:
                                oldAccHappenedAt = oldDeclarationOfAccidentAcc.AccHappenedOther;
                                break;
                            default:
                                break;
                        }
                        switch (declarationOfAccidentAcc.AccHappenedAt)
                        {
                            case 1:
                                newAccHappenedAt = s1;
                                break;
                            case 2:
                                newAccHappenedAt = s2;
                                break;
                            case 3:
                                newAccHappenedAt = declarationOfAccidentAcc.AccHappenedOther;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {  
                        oldAccHappenedAt = oldDeclarationOfAccidentAcc.AccHappenedOther;
                        newAccHappenedAt = declarationOfAccidentAcc.AccHappenedOther;
                    }
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccHappenedAt", oldAccHappenedAt, newAccHappenedAt, currentUser));


                    if (oldDeclarationOfAccidentAcc.AccJobType.Trim() != declarationOfAccidentAcc.AccJobType.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccJobType", oldDeclarationOfAccidentAcc.AccJobType, declarationOfAccidentAcc.AccJobType, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccTaskType.Trim() != declarationOfAccidentAcc.AccTaskType.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccTaskType", oldDeclarationOfAccidentAcc.AccTaskType, declarationOfAccidentAcc.AccTaskType, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccDeviationFromTask.Trim() != declarationOfAccidentAcc.AccDeviationFromTask.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccDeviationFromTask", oldDeclarationOfAccidentAcc.AccDeviationFromTask, declarationOfAccidentAcc.AccDeviationFromTask, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccInjurDesc.Trim() != declarationOfAccidentAcc.AccInjurDesc.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccInjurDesc", oldDeclarationOfAccidentAcc.AccInjurDesc, declarationOfAccidentAcc.AccInjurDesc, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccInjHasRights != declarationOfAccidentAcc.AccInjHasRights)
                    {

                        string newAccInjHasRights = "";
                        string oldAccInjHasRights = "";

                        string s1 = "да";
                        string s2 = "не";
                        string s3 = "не се изисква";

                        switch (oldDeclarationOfAccidentAcc.AccInjHasRights)
                        {
                            case 1:
                                oldAccInjHasRights = s1;
                                break;
                            case 2:
                                oldAccInjHasRights = s2;
                                break;
                            case 3:
                                oldAccInjHasRights = s3;
                                break;
                            default:
                                break;
                        }

                        switch (declarationOfAccidentAcc.AccInjHasRights)
                        {
                            case 1:
                                newAccInjHasRights = s1;
                                break;
                            case 2:
                                newAccInjHasRights = s2;
                                break;
                            case 3:
                                newAccInjHasRights = s3;
                                break;
                            default:
                                break;
                        }


                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccInjHasRights", oldAccInjHasRights, newAccInjHasRights, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccLegalRef != declarationOfAccidentAcc.AccLegalRef)
                    {
                        string newAccLegalRef = "";
                        string oldAccLegalRef = "";

                        string s1 = "по чл. 55, ал.1 от КСО";
                        string s2 = "по чл. 55, ал.2 от КСО";

                        switch (oldDeclarationOfAccidentAcc.AccLegalRef)
                        {
                            case 1:
                                oldAccLegalRef = s1;
                                break;
                            case 2:
                                oldAccLegalRef = s2;
                                break;
                            default:
                                break;
                        }

                        switch (declarationOfAccidentAcc.AccLegalRef)
                        {
                            case 1:
                                newAccLegalRef = s1;
                                break;
                            case 2:
                                newAccLegalRef = s2;
                                break;
                            default:
                                break;
                        }
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccLegalRef", oldAccLegalRef, newAccLegalRef, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccPlannedActions.Trim() != declarationOfAccidentAcc.AccPlannedActions.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPlannedActions", oldDeclarationOfAccidentAcc.AccPlannedActions, declarationOfAccidentAcc.AccPlannedActions, currentUser));
                    }
                    if (oldDeclarationOfAccidentAcc.AccLostDays != declarationOfAccidentAcc.AccLostDays)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccLostDays", oldDeclarationOfAccidentAcc.AccLostDays.ToString(), declarationOfAccidentAcc.AccLostDays.ToString(), currentUser));
                    }

                }

                //TabHarm
                oldDeclarationOfAccident = GetDeclarationOfAccident(declarationOfAccident.DeclarationId, currentUser, "btnTabHarm");
                oldDeclarationOfAccidentHarm = oldDeclarationOfAccident.DeclarationOfAccidentHarm;

                if (declarationOfAccidentHarm != null)
                {
                    if (oldDeclarationOfAccidentHarm.HarmType.Trim() != declarationOfAccidentHarm.HarmType.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmType", oldDeclarationOfAccidentHarm.HarmType, declarationOfAccidentHarm.HarmType, currentUser));
                    }
                    if (oldDeclarationOfAccidentHarm.HarmBodyParts.Trim() != declarationOfAccidentHarm.HarmBodyParts.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmBodyParts", oldDeclarationOfAccidentHarm.HarmBodyParts, declarationOfAccidentHarm.HarmBodyParts, currentUser));
                    }
                    if (oldDeclarationOfAccidentHarm.HarmResult != declarationOfAccidentHarm.HarmResult)
                    {

                        string newHarmResult = "";
                        string oldHarmResult = "";

                        string s1 = "смърт";
                        string s2 = "вероятна инвалидност ";
                        string s3 = "временна неработоспособност";

                        switch (oldDeclarationOfAccidentHarm.HarmResult)
                        {
                            case 1:
                                oldHarmResult = s1;
                                break;
                            case 2:
                                oldHarmResult = s2;
                                break;
                            case 3:
                                oldHarmResult = s3;
                                break;
                            default:
                                break;
                        }

                        switch (declarationOfAccidentHarm.HarmResult)
                        {
                            case 1:
                                newHarmResult = s1;
                                break;
                            case 2:
                                newHarmResult = s2;
                                break;
                            case 3:
                                newHarmResult = s3;
                                break;
                            default:
                                break;
                        }

                        changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmResult", oldHarmResult, newHarmResult, currentUser));
                    }
                }

                //TabWith
                oldDeclarationOfAccident = GetDeclarationOfAccident(declarationOfAccident.DeclarationId, currentUser, "btnTabWith");
                oldDeclarationOfAccidentWith = oldDeclarationOfAccident.DeclarationOfAccidentWith;

                if (declarationOfAccidentWith != null)
                {
                    if (oldDeclarationOfAccidentWith.WitnessFullName.Trim() != declarationOfAccidentWith.WitnessFullName.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitnessFullName", oldDeclarationOfAccidentWith.WitnessFullName, declarationOfAccidentWith.WitnessFullName, currentUser)); }
                    if (oldDeclarationOfAccidentWith.City.CityId != declarationOfAccidentWith.City.CityId) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitCityId", oldDeclarationOfAccidentWith.City.CityName, declarationOfAccidentWith.City.CityName, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitStreet.Trim() != declarationOfAccidentWith.WitStreet.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitStreet", oldDeclarationOfAccidentWith.WitStreet, declarationOfAccidentWith.WitStreet, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitStreetNum.Trim() != declarationOfAccidentWith.WitStreetNum.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitStreetNum", oldDeclarationOfAccidentWith.WitStreetNum, declarationOfAccidentWith.WitStreetNum, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitDistrict.Trim() != declarationOfAccidentWith.WitDistrict.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitDistrict", oldDeclarationOfAccidentWith.WitDistrict, declarationOfAccidentWith.WitDistrict, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitBlock.Trim() != declarationOfAccidentWith.WitBlock.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitBlock", oldDeclarationOfAccidentWith.WitBlock, declarationOfAccidentWith.WitBlock, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitEntrance.Trim() != declarationOfAccidentWith.WitEntrance.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitEntrance", oldDeclarationOfAccidentWith.WitEntrance, declarationOfAccidentWith.WitEntrance, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitFloor.Trim() != declarationOfAccidentWith.WitFloor.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitFloor", oldDeclarationOfAccidentWith.WitFloor, declarationOfAccidentWith.WitFloor, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitApt.Trim() != declarationOfAccidentWith.WitApt.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitApt", oldDeclarationOfAccidentWith.WitApt, declarationOfAccidentWith.WitApt, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitPhone.Trim() != declarationOfAccidentWith.WitPhone.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitPhone", oldDeclarationOfAccidentWith.WitPhone, declarationOfAccidentWith.WitPhone, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitFax.Trim() != declarationOfAccidentWith.WitFax.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitFax", oldDeclarationOfAccidentWith.WitFax, declarationOfAccidentWith.WitFax, currentUser)); }
                    if (oldDeclarationOfAccidentWith.WitEmail.Trim() != declarationOfAccidentWith.WitEmail.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitEmail", oldDeclarationOfAccidentWith.WitEmail, declarationOfAccidentWith.WitEmail, currentUser)); }
                }

                //TabHeir
                oldDeclarationOfAccident = GetDeclarationOfAccident(declarationOfAccident.DeclarationId, currentUser, "btnTabHeir");
                oldDeclarationOfAccidentHeir = oldDeclarationOfAccident.DeclarationOfAccidentHeir;

                if (declarationOfAccidentHeir != null)
                {
                    if (oldDeclarationOfAccidentHeir.HeirFullName.Trim() != declarationOfAccidentHeir.HeirFullName.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFullName", oldDeclarationOfAccidentHeir.HeirFullName, declarationOfAccidentHeir.HeirFullName, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirEgn.Trim() != declarationOfAccidentHeir.HeirEgn.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEgn", oldDeclarationOfAccidentHeir.HeirEgn, declarationOfAccidentHeir.HeirEgn, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.City.CityId != declarationOfAccidentHeir.City.CityId) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirCityid", oldDeclarationOfAccidentHeir.City.CityName, declarationOfAccidentHeir.City.CityName, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirStreet.Trim() != declarationOfAccidentHeir.HeirStreet.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirStreet", oldDeclarationOfAccidentHeir.HeirStreet, declarationOfAccidentHeir.HeirStreet, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirStreetNum.Trim() != declarationOfAccidentHeir.HeirStreetNum.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirStreetNum", oldDeclarationOfAccidentHeir.HeirStreetNum, declarationOfAccidentHeir.HeirStreetNum, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirDistrict.Trim() != declarationOfAccidentHeir.HeirDistrict.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirDistrict", oldDeclarationOfAccidentHeir.HeirDistrict, declarationOfAccidentHeir.HeirDistrict, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirBlock.Trim() != declarationOfAccidentHeir.HeirBlock.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirBlock", oldDeclarationOfAccidentHeir.HeirBlock, declarationOfAccidentHeir.HeirBlock, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirEntrance.Trim() != declarationOfAccidentHeir.HeirEntrance.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEntrance", oldDeclarationOfAccidentHeir.HeirEntrance, declarationOfAccidentHeir.HeirEntrance, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirFloor.Trim() != declarationOfAccidentHeir.HeirFloor.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFloor", oldDeclarationOfAccidentHeir.HeirFloor, declarationOfAccidentHeir.HeirFloor, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirApt.Trim() != declarationOfAccidentHeir.HeirApt.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirApt", oldDeclarationOfAccidentHeir.HeirApt, declarationOfAccidentHeir.HeirApt, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirPhone.Trim() != declarationOfAccidentHeir.HeirPhone.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirPhone", oldDeclarationOfAccidentHeir.HeirPhone, declarationOfAccidentHeir.HeirPhone, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirFax.Trim() != declarationOfAccidentHeir.HeirFax.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFax", oldDeclarationOfAccidentHeir.HeirFax, declarationOfAccidentHeir.HeirFax, currentUser)); }
                    if (oldDeclarationOfAccidentHeir.HeirEmail.Trim() != declarationOfAccidentHeir.HeirEmail.Trim()) { changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEmail", oldDeclarationOfAccidentHeir.HeirEmail, declarationOfAccidentHeir.HeirEmail, currentUser)); }
                }

            }

        }
        //Delete Method
        public static bool DeleteDeclarationOfAccident(int declarationId, User currentUser, Change changeEntry)
        {
            //Det details for Tracking Change for Delete

            DeclarationOfAccident declarationOfAccident;
            DeclarationOfAccident oldDeclarationOfAccident = new DeclarationOfAccident(currentUser);

            DeclarationOfAccidentHeader declarationOfAccidentHeader = new DeclarationOfAccidentHeader();
            Employer employer = new Employer(currentUser);
            declarationOfAccident = GetDeclarationOfAccident(declarationId, currentUser, "btnTabEmpl");

            declarationOfAccidentHeader = declarationOfAccident.DeclarationOfAccidentHeader;
            employer = declarationOfAccident.Employer;

            DeclarationOfAccidentWorker declarationOfAccidentWorker = new DeclarationOfAccidentWorker(currentUser);
            declarationOfAccident = GetDeclarationOfAccident(declarationId, currentUser, "btnTabWorker");
            declarationOfAccidentWorker = declarationOfAccident.DeclarationOfAccidentWorker;

            oldDeclarationOfAccident.DeclarationOfAccidentHeader = declarationOfAccidentHeader;
            oldDeclarationOfAccident.Employer = employer;
            oldDeclarationOfAccident.DeclarationOfAccidentWorker = declarationOfAccidentWorker;

            string SQL = "";
            bool isDeleted = false;
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();
            try
            {
                SQL = @"DELETE FROM PMIS_HS.DeclarationsOfAccident WHERE DeclarationId = :DeclarationId";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "DeclarationId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = declarationId;
                cmd.Parameters.Add(param);

                isDeleted = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (isDeleted) // Succes Delete Operation
            {
                //Create obect using log for DELETE operation
                ChangeEvent changeEvent = new ChangeEvent("HS_DeclAcc_DeleteDecl", "", null, null, currentUser);

                AddChangeEventDetailsForDelete(oldDeclarationOfAccident, changeEvent, currentUser);

                changeEntry.AddEvent(changeEvent);
            }
            return isDeleted;

        }
        //Select list of object
        public static List<DeclarationOfAccident> GetAllDeclarationOfAccident(DeclarationOfAccidentFilter filter, User currentUser)
        {
            //Create list of DeclarationOfAccident obect
            List<DeclarationOfAccident> listDeclarationOfAccident = new List<DeclarationOfAccident>();

            //Create command object
            OracleCommand cmd = new OracleCommand();

            //Create parameter object
            OracleParameter param = new OracleParameter();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {    //Binding Filter in Query

                // 1. User
                string SqlUserWhere = "";
                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_DECLARATIONACC", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    SqlUserWhere += " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }
                // 2. Filter
                string SqlFilterWhere = "";
                if (!String.IsNullOrEmpty(filter.DeclarationNumber))
                {
                    SqlFilterWhere += " AND UPPER(a.DeclarationNumber) LIKE :DeclarationNumber";

                    param = new OracleParameter();
                    param.ParameterName = "DeclarationNumber";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    param.Value = "%" + filter.DeclarationNumber.ToUpper() + "%";
                    cmd.Parameters.Add(param);
                }

                if ((filter.DeclarationDateFrom.HasValue) || (filter.DeclarationDateTo.HasValue))
                {
                    if ((filter.DeclarationDateFrom.HasValue) && (filter.DeclarationDateTo.HasValue))
                    {
                        SqlFilterWhere += " AND a.DeclarationDate >= :DeclarationDateFrom AND  a.DeclarationDate < :DeclarationDateTo ";
                    }
                    else
                    {
                        if (filter.DeclarationDateFrom.HasValue)
                        {
                            SqlFilterWhere += " AND a.DeclarationDate >= :DeclarationDateFrom ";
                        }

                        if (filter.DeclarationDateTo.HasValue)
                        {
                            SqlFilterWhere += " AND a.DeclarationDate < :DeclarationDateTo";
                        }
                    }
                }


                if (filter.DeclarationDateFrom.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "DeclarationDateFrom";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.DeclarationDateFrom;
                    cmd.Parameters.Add(param);
                }

                if (filter.DeclarationDateTo.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "DeclarationDateTo";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.DeclarationDateTo.Value.AddDays(1);
                    cmd.Parameters.Add(param);
                }

                if (!String.IsNullOrEmpty(filter.WorkerFullName))
                {
                    SqlFilterWhere += " AND UPPER(a.WorkerFullName) LIKE :WorkerFullName";

                    param = new OracleParameter();
                    param.ParameterName = "WorkerFullName";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    param.Value = "%" + filter.WorkerFullName.ToUpper() + "%";
                    cmd.Parameters.Add(param);
                }


                string sqlOrderBy = "";
                string orderByDir = "ASC";

                int filterOrderBy = filter.OrderBy;

                if (filter.OrderBy > 100)
                {
                    filterOrderBy -= 100;
                    orderByDir = "DESC";
                }

                //Configure orderBy filter
                switch (filterOrderBy)
                {
                    case 1:
                        sqlOrderBy += "a.DeclarationNumber";
                        break;
                    case 2:
                        sqlOrderBy += "a.DeclarationDate";
                        break;
                    case 3:
                        sqlOrderBy += "a.WorkerFullName";
                        break;
                    default:
                        sqlOrderBy += "a.DeclarationNumber";
                        break;
                }

                sqlOrderBy += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                //Configure SQL statement
                string SQL = @" SELECT tmp.DeclarationId, 
                                       tmp.DeclarationNumber, 
                                       tmp.WorkerFullName,  
                                       tmp.DeclarationDate, 
                                       tmp.AccDateTime,
                                       tmp.CreatedBy, tmp.CreatedDate, tmp.LastModifiedBy, tmp.LastModifiedDate, 
                                       tmp.RowNumber as RowNumber  
                                  FROM (
                                  SELECT a.DeclarationId, 
                                         a.DeclarationNumber, 
                                         a.WorkerFullName, 
                                         a.DeclarationDate,
                                         a.AccDateTime,
                                         a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,      
                                  RANK() OVER (ORDER BY " + sqlOrderBy + @" , a.DeclarationId) as RowNumber 
                                  FROM PMIS_HS.DECLARATIONSOFACCIDENT a WHERE 1=1 " + SqlFilterWhere + " " + SqlUserWhere + " " +
                                    @" ) tmp";


                //Binding page filter if need
                if (filter.PageIndex > 0 && filter.PageCount > 0)
                {
                    string sqlPageWhere = "";
                    sqlPageWhere = " WHERE tmp.RowNumber BETWEEN (" + filter.PageIndex.ToString() + @" - 1) * " + filter.PageCount.ToString() + @" + 1 AND " + filter.PageIndex.ToString() + @" * " + filter.PageCount.ToString() + @" ";
                    SQL += sqlPageWhere;
                }

                SQL = DBCommon.FixNewLines(SQL);

                //Set connection and command text 
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = SQL;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DeclarationOfAccident declarationOfAccident = new DeclarationOfAccident(Convert.ToInt32(dr["DeclarationId"]), currentUser);

                    //Select Column to display

                    declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber = dr["DeclarationNumber"].ToString();
                    declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName = dr["WorkerFullName"].ToString();

                    if (dr["DeclarationDate"] is DateTime) declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate = Convert.ToDateTime(dr["DeclarationDate"]);

                    if (dr["AccDateTime"] is DateTime) declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime = Convert.ToDateTime(dr["AccDateTime"]);

                    //Add to list
                    listDeclarationOfAccident.Add(declarationOfAccident);

                    //Extrat created and last modified fields from a data reader. Coomon for all objects
                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, declarationOfAccident);
                }

                dr.Close();

            }
            finally
            {
                conn.Close();
            }

            return listDeclarationOfAccident;
        }
        //Get Count of object
        public static int CountDeclarationOfAccident(DeclarationOfAccidentFilter filter, User currentUser)
        {

            int count = 0;
            string SQL = "";
            string SQLWHERE = "";

            //Create connection object
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            try
            {
                SQL = @" SELECT COUNT(*)  FROM PMIS_HS.DECLARATIONSOFACCIDENT a WHERE 1=1 ";

                //Create command object
                OracleCommand cmd = new OracleCommand();
                //Create parameter object
                OracleParameter param = new OracleParameter();

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_DECLARATIONACC", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    SQLWHERE += " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(filter.DeclarationNumber))
                {
                    SQLWHERE += " AND UPPER(a.DeclarationNumber) LIKE :DeclarationNumber";

                    param = new OracleParameter();
                    param.ParameterName = "DeclarationNumber";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    param.Value = "%" + filter.DeclarationNumber.ToUpper() + "%";
                    cmd.Parameters.Add(param);
                }

                if ((filter.DeclarationDateFrom.HasValue) || (filter.DeclarationDateTo.HasValue))
                {
                    if ((filter.DeclarationDateFrom.HasValue) && (filter.DeclarationDateTo.HasValue))
                    {
                        SQLWHERE += " AND a.DeclarationDate >= :DeclarationDateFrom AND  a.DeclarationDate < :DeclarationDateTo ";
                    }
                    else
                    {
                        if (filter.DeclarationDateFrom.HasValue)
                        {
                            SQLWHERE += " AND a.DeclarationDate >= :DeclarationDateFrom ";
                        }

                        if (filter.DeclarationDateTo.HasValue)
                        {
                            SQLWHERE += " AND a.DeclarationDate < :DeclarationDateTo";
                        }
                    }
                }


                if (filter.DeclarationDateFrom.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "DeclarationDateFrom";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.DeclarationDateFrom;
                    cmd.Parameters.Add(param);
                }

                if (filter.DeclarationDateTo.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "DeclarationDateTo";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.DeclarationDateTo.Value.AddDays(1);
                    cmd.Parameters.Add(param);
                }

                if (!String.IsNullOrEmpty(filter.WorkerFullName))
                {
                    SQLWHERE += " AND UPPER(a.WorkerFullName) LIKE :WorkerFullName";

                    param = new OracleParameter();
                    param.ParameterName = "WorkerFullName";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    param.Value = "%" + filter.WorkerFullName.ToUpper() + "%";
                    cmd.Parameters.Add(param);
                }

                SQL += SQLWHERE;

                SQL = DBCommon.FixNewLines(SQL);

                //Set connection and comand text to command object
                cmd.Connection = conn;
                cmd.CommandText = SQL;

                //Execute command and getnumber of row
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
        //Bind list for tarcking changes Delete
        public static void AddChangeEventDetailsForDelete(DeclarationOfAccident oldDeclarationOfAccident, ChangeEvent changeEvent, User currentUser)
        {

            DeclarationOfAccidentHeader declarationOfAccidentHeader;
            DeclarationOfAccidentFooter declarationOfAccidentFooter;
            Employer employer;
            DeclarationOfAccidentWorker declarationOfAccidentWorker;
            //DeclarationOfAccidentAcc declarationOfAccidentAcc;
            //DeclarationOfAccidentHarm declarationOfAccidentHarm;
            //DeclarationOfAccidentWith declarationOfAccidentWith;
            //DeclarationOfAccidentHeir declarationOfAccidentHeir;

            declarationOfAccidentHeader = oldDeclarationOfAccident.DeclarationOfAccidentHeader;
            declarationOfAccidentFooter = oldDeclarationOfAccident.DeclarationOfAccidentFooter;
            employer = oldDeclarationOfAccident.Employer;
            declarationOfAccidentWorker = oldDeclarationOfAccident.DeclarationOfAccidentWorker;
            // declarationOfAccidentAcc = declarationOfAccident.DeclarationOfAccidentAcc;
            //declarationOfAccidentHarm = declarationOfAccident.DeclarationOfAccidentHarm;
            //declarationOfAccidentWith = declarationOfAccident.DeclarationOfAccidentWith;
            //declarationOfAccidentHeir = declarationOfAccident.DeclarationOfAccidentHeir;


            //TabHeader - 5 fields
            if (declarationOfAccidentHeader != null)
            {
                if (!String.IsNullOrEmpty(declarationOfAccidentHeader.DeclarationNumber))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_DeclarationNumber", declarationOfAccidentHeader.DeclarationNumber, "", currentUser));
                }

                changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_DeclarationDate", CommonFunctions.FormatDate(declarationOfAccidentHeader.DeclarationDate.ToString()), "", currentUser));

                if (!String.IsNullOrEmpty(declarationOfAccidentHeader.ReferenceNumber))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_ReferenceNumber", "", declarationOfAccidentHeader.ReferenceNumber, currentUser));
                }

                //   changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_ReferenceDate", CommonFunctions.FormatDate(declarationOfAccidentHeader.ReferenceDate.ToString()), "", currentUser));

                if (!String.IsNullOrEmpty(declarationOfAccidentHeader.FileNumber))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_FileNumber", declarationOfAccidentHeader.FileNumber, "", currentUser));
                }
            }

            //TabFooter - 3 fields
            if (declarationOfAccidentFooter != null)
            {
                if (declarationOfAccidentFooter.ApplicantType > 0)
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantType", declarationOfAccidentFooter.ApplicantType.ToString(), "", currentUser));
                }

                if (!String.IsNullOrEmpty(declarationOfAccidentFooter.AplicantPosition))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantPosition", "", declarationOfAccidentFooter.AplicantPosition, currentUser));
                }

                if (!String.IsNullOrEmpty(declarationOfAccidentFooter.AplicantName))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AplicantName", declarationOfAccidentFooter.AplicantName, "", currentUser));
                }
            }

            ////  TabEmpl - 15 fields
            //if (employer != null)
            //{
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmployerId", employer.EmployerName, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEik", employer.EmplEik, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplCityId", employer.EmplCityId.ToString(), "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplStreet", employer.EmplStreet, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplStreetNum", employer.EmplStreetNum, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplDistrict", employer.EmplDistrict, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplBlock", employer.EmplBlock, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEntrance", employer.EmplEntrance, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFloor", employer.EmplFloor, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplApt", employer.EmplApt, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplPhone", employer.EmplPhone, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFax", employer.EmplFax, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplEmail", employer.EmplEmail, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplNumberOfEmployees", employer.EmplNumberOfEmployees.ToString(), "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_EmplFemaleEmployees", employer.EmplFemaleEmployees.ToString(), "", currentUser));
            //}
            //  TabWorker - 24 fields
            if (declarationOfAccidentWorker != null)
            {
                if (!String.IsNullOrEmpty(declarationOfAccidentWorker.WorkerFullName))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WorkerFullName", declarationOfAccidentWorker.WorkerFullName, "", currentUser));
                }
                if (!String.IsNullOrEmpty(declarationOfAccidentWorker.WorkerEgn))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WorkerEgn", declarationOfAccidentWorker.WorkerEgn, "", currentUser));
                }

                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WCityId",  declarationOfAccidentWorker.WCityId.ToString(), "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WStreet",  declarationOfAccidentWorker.WStreet, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WStreetNum",  declarationOfAccidentWorker.WStreetNum, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WDistrict",  declarationOfAccidentWorker.WDistrict, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBlock",  declarationOfAccidentWorker.WBlock, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WEntrance",  declarationOfAccidentWorker.WEntrance, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WFloor",  declarationOfAccidentWorker.WFloor, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WApt",  declarationOfAccidentWorker.WApt, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WPhone",  declarationOfAccidentWorker.WPhone, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WFax",  declarationOfAccidentWorker.WFax, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WEmail",  declarationOfAccidentWorker.WEmail, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBirthDate",  CommonFunctions.FormatDate(declarationOfAccidentWorker.WBirthDate.ToString()), "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WGender",  declarationOfAccidentWorker.WGender.ToString(), "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WCitizenship",  declarationOfAccidentWorker.WCitizenship, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WHireType",  declarationOfAccidentWorker.WHireType.ToString(), "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WWorkTime",  declarationOfAccidentWorker.WWorkTime.ToString(), "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WHireDate",  CommonFunctions.FormatDate(declarationOfAccidentWorker.WHireDate.ToString()), "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobTitle",  declarationOfAccidentWorker.WJobTitle, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobCode",  declarationOfAccidentWorker.WJobCode, "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WJobCategory",  declarationOfAccidentWorker.WJobCategory.ToString(), "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WYearsOnService",  declarationOfAccidentWorker.WYearsOnService.ToString(), "", currentUser));
                //changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WBranch",  declarationOfAccidentWorker.WBranch, "", currentUser));
            }
            //// TabAcc - 23 fields
            // if (declarationOfAccidentAcc != null)
            // {
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccDateTime",  CommonFunctions.FormatDate(declarationOfAccidentAcc.AccDateTime), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkFromHour1",  declarationOfAccidentAcc.AccWorkFromHour1.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkFromMin1",  declarationOfAccidentAcc.AccWorkFromMin1.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkToHour1",  declarationOfAccidentAcc.AccWorkToHour1.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkToMin1",  declarationOfAccidentAcc.AccWorkToMin1.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkFromHour2",  declarationOfAccidentAcc.AccWorkFromHour2.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkFromMin2",  declarationOfAccidentAcc.AccWorkFromMin2.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkToHour2",  declarationOfAccidentAcc.AccWorkToHour2.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccWorkToMin2",  declarationOfAccidentAcc.AccWorkToMin2.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPlace",  declarationOfAccidentAcc.AccPlace, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccCityId",  declarationOfAccidentAcc.AccCityId.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccStreet",  declarationOfAccidentAcc.AccStreet, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccStreetNum",  declarationOfAccidentAcc.AccStreetNum, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPhone",  declarationOfAccidentAcc.AccPhone, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccHappenedAt",  declarationOfAccidentAcc.AccHappenedAt.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccJobType",  declarationOfAccidentAcc.AccJobType, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccTaskType",  declarationOfAccidentAcc.AccTaskType, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccDeviationFromTask",  declarationOfAccidentAcc.AccDeviationFromTask, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccInjurDesc",  declarationOfAccidentAcc.AccInjurDesc, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccInjHasRights",  declarationOfAccidentAcc.AccInjHasRights.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccLegalRef",  declarationOfAccidentAcc.AccLegalRef.ToString(), "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccPlannedActions",  declarationOfAccidentAcc.AccPlannedActions, "", currentUser));
            //     changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_AccLostDays",  declarationOfAccidentAcc.AccLostDays.ToString(), "", currentUser));
            // }
            ////TabHarm - 3 fields
            //if (declarationOfAccidentHarm != null)
            //{
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmType",  declarationOfAccidentHarm.HarmType, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmBodyParts",  declarationOfAccidentHarm.HarmBodyParts, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HarmResult",  declarationOfAccidentHarm.HarmResult.ToString(), "", currentUser));
            //}
            ////TabWith - 12 fields
            //if (declarationOfAccidentWith != null)
            //{
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitnessFullName",  declarationOfAccidentWith.WitnessFullName, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitCityId",  declarationOfAccidentWith.WitCityId.ToString(), "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitStreet",  declarationOfAccidentWith.WitStreet, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitStreetNum",  declarationOfAccidentWith.WitStreetNum, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitDistrict",  declarationOfAccidentWith.WitDistrict, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitBlock",  declarationOfAccidentWith.WitBlock, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitEntrance",  declarationOfAccidentWith.WitEntrance, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitFloor",  declarationOfAccidentWith.WitFloor, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitApt",  declarationOfAccidentWith.WitApt, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitPhone",  declarationOfAccidentWith.WitPhone, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitFax",  declarationOfAccidentWith.WitFax, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_WitEmail",  declarationOfAccidentWith.WitEmail, "", currentUser));
            //}
            ////TabHeir - 13 fields
            //if (declarationOfAccidentHeir != null)
            //{
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFullName",  declarationOfAccidentHeir.HeirFullName, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEgn",  declarationOfAccidentHeir.HeirEgn, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirCityid",  declarationOfAccidentHeir.HeirCityId.ToString(), "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirStreet",  declarationOfAccidentHeir.HeirStreet, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirStreetNum",  declarationOfAccidentHeir.HeirStreetNum, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirDistrict",  declarationOfAccidentHeir.HeirDistrict, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirBlock",  declarationOfAccidentHeir.HeirBlock, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEntrance",  declarationOfAccidentHeir.HeirEntrance, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFloor",  declarationOfAccidentHeir.HeirFloor, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirApt",  declarationOfAccidentHeir.HeirApt, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirPhone",  declarationOfAccidentHeir.HeirPhone, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirFax",  declarationOfAccidentHeir.HeirFax, "", currentUser));
            //    changeEvent.AddDetail(new ChangeEventDetail("HS_DeclAcc_HeirEmail",  declarationOfAccidentHeir.HeirEmail, "", currentUser));
            //}



        }
        //Get Object for Investigation Protocol - use in AddEditInvestigationProtocol.aspx
        public static DeclarationOfAccident GetDeclarationOfAccidentForInvProtocol(int? declarationId, User currentUser)
        {
            DeclarationOfAccident declarationOfAccident = null;

            string SQL = "";
            if (!declarationId.HasValue) return declarationOfAccident;
            if (declarationId == 0) return declarationOfAccident;

            declarationOfAccident = new DeclarationOfAccident(currentUser);

            string sqlUserWhere = "";

            //Restric the user to access only his own records if this is set for the particular role
            UIItem uiItem = UIItemUtil.GetUIItems("HS_DECLARATIONACC", currentUser, false, currentUser.Role.RoleId, null)[0];
            if (uiItem.AccessOnlyOwnData)
            {
                sqlUserWhere = " AND a.CreatedBy = " + currentUser.UserId.ToString();

            }

            SQL = @"SELECT AccDateTime, WorkerFullName, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate
                    FROM PMIS_HS.DeclarationsOfAccident a 
                    WHERE a.declarationId = :declarationId " + sqlUserWhere;

            SQL = DBCommon.FixNewLines(SQL);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            try
            {
                conn.Open();

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "declarationId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = declarationId;

                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr["AccDateTime"] is DateTime) declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime = (DateTime)dr["AccDateTime"];
                    declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName = dr["WorkerFullName"].ToString();

                    //Extrat created and last modified fields from a data reader. Coomon for all objects
                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, declarationOfAccident);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return declarationOfAccident;
        }
    }
}

