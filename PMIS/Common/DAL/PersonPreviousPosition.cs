using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonPreviousPosition : BaseDbObject
    {

        public int PersonPreviousPositionId { get; set; }

        //1  Код длъжност   ARDLO_DLOKOD   VARCHAR2(13 BYTE)   Nulable  No
        public string PersonPreviousPositionCode { get; set; }

        //2  Длъжност (име)   ARDLO_TEXTDL   VARCHAR2(1024 BYTE)   Nulable  No
        public string PersonPreviousPositionPositionName { get; set; }

        //3  Заемал длъжността като   ARDLO_VRIKOD   VARCHAR2(1 BYTE)   Nulable  No
        public string PersonPreviousPositionTypeKey { get; set; }
        private PreviousPositionType personPreviousPositionType;
        public PreviousPositionType PersonPreviousPositionType
        {
            get
            {
                if (personPreviousPositionType == null && !string.IsNullOrEmpty(PersonPreviousPositionTypeKey))
                {
                    personPreviousPositionType = PreviousPositionTypeUtil.GetPreviousPositionType(PersonPreviousPositionTypeKey, CurrentUser);
                }
                return personPreviousPositionType;
            }
        }

        //4  Вид  ARDLO_KZVKOD   VARCHAR2(1 BYTE)   Nulable  Yes
        public string PersonPreviousPositionKindKey { get; set; }
        private PreviousPositionKind personPreviousPositionKind;
        public PreviousPositionKind PersonPreviousPositionKind
        {
            get
            {
                if (personPreviousPositionKind == null && !string.IsNullOrEmpty(PersonPreviousPositionKindKey))
                {
                    personPreviousPositionKind = PreviousPositionKindUtil.GetPreviousPositionKind(PersonPreviousPositionKindKey, CurrentUser);
                }
                return personPreviousPositionKind;
            }
        }

        //5  Категория  ARDLO_KATKOD   VARCHAR2(2 BYTE)   Nulable  Yes
        public string PersonPreviousPositionMilitaryCategoryId { get; set; }
        private MilitaryCategory personPreviousPositionMilitaryCategory;
        public MilitaryCategory PersonPreviousPositionMilitaryCategory
        {
            get
            {
                if (personPreviousPositionMilitaryCategory == null && !string.IsNullOrEmpty(this.PersonPreviousPositionMilitaryCategoryId))
                {
                    int CategoryId = 0;
                    int.TryParse(PersonPreviousPositionMilitaryCategoryId, out CategoryId);

                    personPreviousPositionMilitaryCategory = MilitaryCategoryUtil.GetMilitaryCategory(CategoryId, CurrentUser);
                }
                return personPreviousPositionMilitaryCategory;
            }
        }

        //6  ВОС  ARDLO_VSOKOD   VARCHAR2(4 BYTE)   Nulable  Yes
        public string PersonPreviousPositionMilReportingSpecialityCode { get; set; }
        private MilitaryReportSpeciality militaryReportSpeciality;
        public MilitaryReportSpeciality MilitaryReportSpeciality
        {
            get
            {
                if (militaryReportSpeciality == null && !string.IsNullOrEmpty(this.PersonPreviousPositionMilReportingSpecialityCode))
                {
                    //int personPreviousPositionMilReportingSpecialityCodeInt = 0;
                    //int.TryParse(PersonPreviousPositionMilReportingSpecialityCode, out personPreviousPositionMilReportingSpecialityCodeInt);
                    militaryReportSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialityByCode(this.PersonPreviousPositionMilReportingSpecialityCode, CurrentUser);
                }
                return militaryReportSpeciality;
            }
        }


        //7  Мисия  ARDLO_MISIA   VARCHAR2(1 BYTE)   Nulable  No
        public bool PersonPreviousPositionMission { get; set; }

        //8  Заповед Номер  ARDLO_ZPVD   VARCHAR2(20 BYTE)   Nulable  No
        public string PersonPreviousPositionVaccAnnNum { get; set; }

        //9  Заповед Дата  ARDLO_IZD   DATE   Nulable  No
        public DateTime PersonPreviousPositionVaccAnnDateVacAnn { get; set; }

        //10  В сила от   ARDLO_KOGA   DATE   Nulable  No
        public DateTime PersonPreviousPositionVaccAnnDateWhen { get; set; }

        //11  Подписал заповедта  ARDLO_SPZKOD   VARCHAR2(1 BYTE)   Nulable  Yes
        public string PersonPreviousPositionMilitaryCommanderRankCode { get; set; }
        private MilitaryCommanderRank personPreviousPositionMilitaryCommanderRank;
        public MilitaryCommanderRank PersonPreviousPositionMilitaryCommanderRank
        {
            get
            {
                if (personPreviousPositionMilitaryCommanderRank == null && !string.IsNullOrEmpty(this.PersonPreviousPositionMilitaryCommanderRankCode))
                {
                    personPreviousPositionMilitaryCommanderRank = MilitaryCommanderRankUtil.GetMilitaryCommanderRank(PersonPreviousPositionMilitaryCommanderRankCode, CurrentUser);
                }
                return personPreviousPositionMilitaryCommanderRank;
            }
        }

        //12  Поделение  ARDLO_VPN   VARCHAR2(5 BYTE)   Nulable  No
        //13  Име на поделението  ARDLO_IMEPOD   VARCHAR2(20 BYTE)   Nulable  No
        //Get this Fields from   PersonMilitaryUnit object
        //In Light Box use Item selector
        public int PersonPreviousPositionMilitaryUnitId { get; set; }
        private MilitaryUnit personPreviousPositionMilitaryUnit;
        public MilitaryUnit PersonPreviousPositionMilitaryUnit
        {
            get
            {
                if (personPreviousPositionMilitaryUnit == null && this.PersonPreviousPositionMilitaryUnitId > 0)
                {
                    personPreviousPositionMilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit(PersonPreviousPositionMilitaryUnitId, CurrentUser);
                }
                return personPreviousPositionMilitaryUnit;
            }
            set
            {
                personPreviousPositionMilitaryUnit = value;
            }
        }


        //14  Организационна еденица  ARDLO_IMEOE   VARCHAR2(25 BYTE)   Nulable  No
        public string PersonPreviousPositionOrganisationUnit { get; set; }

        //15  Гарнизон  ARDLO_NMAKOD   NUMBER(5,0)   Nulable  No
        public int PersonPreviousPositionGarrisonCityId { get; set; }
        private string personPreviousPositionGarrisonName;
        public string PersonPreviousPositionGarrisonName
        {
            get
            {
                if (string.IsNullOrEmpty(personPreviousPositionGarrisonName) && this.PersonPreviousPositionGarrisonCityId > 0)
                {

                    City city = CityUtil.GetCity(PersonPreviousPositionGarrisonCityId, CurrentUser);
                    personPreviousPositionGarrisonName = city.RegionMunicipalityAndCity;
                }
                return personPreviousPositionGarrisonName;
            }
        }

        //16    ARDLO_MILITARYDEPARTMENTID   NUMBER   Nulable  Yes
        //using this to set update/delete logic
        public int? PersonPreviousPositionMilitaryDepartmentId { get; set; }

        //17  До дата   ARDLO_ENDDATE   DATE   Nulable  Yes
        public DateTime? PersonPreviousPositionVaccAnnDateEnd { get; set; }

        private bool canDelete;
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonPreviousPosition(User user)
            : base(user)
        {
            this.PersonPreviousPositionTypeKey = "";
            this.PersonPreviousPositionKindKey = "";
            this.PersonPreviousPositionMilitaryCommanderRankCode = "";
            this.PersonPreviousPositionMilReportingSpecialityCode = "";
            this.PersonPreviousPositionMilitaryCategoryId = "";
            this.PersonPreviousPositionPositionName = "";
            this.PersonPreviousPositionCode = "";
        }
    }

    public class PersonPreviousPositionUtil
    {
        private static PersonPreviousPosition ExtractPersonPreviousPositionFromDR(OracleDataReader dr, User currentUser)
        {
            PersonPreviousPosition personPreviousPosition = new PersonPreviousPosition(currentUser);

            personPreviousPosition.PersonPreviousPositionId = DBCommon.GetInt(dr["PersonPreviousPositionId"]);
            personPreviousPosition.PersonPreviousPositionCode = dr["Code"].ToString();
            personPreviousPosition.PersonPreviousPositionPositionName = dr["PositionName"].ToString();
            personPreviousPosition.PersonPreviousPositionTypeKey = dr["TypeKey"].ToString();
            personPreviousPosition.PersonPreviousPositionKindKey = dr["KindKey"].ToString();
            personPreviousPosition.PersonPreviousPositionMilitaryCategoryId = dr["MilitaryCategoryId"].ToString();
            personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode = dr["MilReportingSpecialityCode"].ToString();
            personPreviousPosition.PersonPreviousPositionMission = (dr["Mission"].ToString() == "Y" || dr["Mission"].ToString() == "1");
            personPreviousPosition.PersonPreviousPositionVaccAnnNum = dr["VaccAnnNum"].ToString();
            personPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn = (DateTime)(dr["VaccAnnDateVacAnn"]);
            personPreviousPosition.PersonPreviousPositionVaccAnnDateWhen = (DateTime)(dr["VaccAnnDateWhen"]);
            personPreviousPosition.PersonPreviousPositionMilitaryCommanderRankCode = dr["MilitaryCommanderRankCode"].ToString();

            personPreviousPosition.PersonPreviousPositionMilitaryUnit = new MilitaryUnit(currentUser);

            personPreviousPosition.PersonPreviousPositionMilitaryUnit.VPN = dr["MilitaryUnitVpn"].ToString();
            personPreviousPosition.PersonPreviousPositionMilitaryUnit.ShortName = dr["MilitaryUnitName"].ToString();

            personPreviousPosition.PersonPreviousPositionMilitaryUnitId = MilitaryUnitUtil.GetMilitaryUnitsId(personPreviousPosition.PersonPreviousPositionMilitaryUnit.VPN, personPreviousPosition.PersonPreviousPositionMilitaryUnit.ShortName, currentUser);

            personPreviousPosition.PersonPreviousPositionOrganisationUnit = dr["OrganisationUnit"].ToString();
            personPreviousPosition.PersonPreviousPositionGarrisonCityId = DBCommon.GetInt(dr["GarrisonCityId"]);
            personPreviousPosition.PersonPreviousPositionMilitaryDepartmentId = (dr["MilitaryDepartmentId"] is int ? DBCommon.GetInt(dr["MilitaryDepartmentId"]) : (int?)null);
            personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd = (dr["VaccAnnDateEnd"] is DateTime ? (DateTime)(dr["VaccAnnDateEnd"]) : (DateTime?)null);

            return personPreviousPosition;
        }

        public static PersonPreviousPosition GetPersonPreviousPosition(int personPreviousPositionId, User currentUser)
        {
            PersonPreviousPosition personPreviousPosition = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT
                                ARDLOID        AS PersonPreviousPositionId,
                                ARDLO_DLOKOD   AS Code,
                                ARDLO_TEXTDL   AS PositionName,
                                ARDLO_VRIKOD   AS TypeKey,
                                ARDLO_KZVKOD   AS KindKey,
                                ARDLO_KATKOD   AS MilitaryCategoryId,
                                ARDLO_VSOKOD   AS MilReportingSpecialityCode,
                                ARDLO_MISIA    AS Mission,
                                ARDLO_ZPVD     AS VaccAnnNum,
                                ARDLO_IZD      AS VaccAnnDateVacAnn,
                                ARDLO_KOGA     As VaccAnnDateWhen,
                                ARDLO_SPZKOD   AS MilitaryCommanderRankCode,
                                ARDLO_VPN      AS MilitaryUnitVpn,
                                ARDLO_IMEPOD   AS MilitaryUnitName,
                                ARDLO_IMEOE    AS OrganisationUnit,
                                ARDLO_NMAKOD   AS GarrisonCityId,
                                ARDLO_MILITARYDEPARTMENTID AS MilitaryDepartmentId,
                                ARDLO_ENDDATE  AS VaccAnnDateEnd
                          
                              FROM VS_OWNER.VS_AR_DLO a

                              WHERE a.ARDLOID = :PersonPreviousPositionId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonPreviousPositionId", OracleType.Number).Value = personPreviousPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personPreviousPosition = ExtractPersonPreviousPositionFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPreviousPosition;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 3 parameters
        public static PersonPreviousPosition GetPersonPreviousPosition(string identityNumber, DateTime vaccAnnDateWhen, string vaccAnnNum, User currentUser)
        {
            PersonPreviousPosition personPreviousPosition = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT
                                ARDLOID        AS PersonPreviousPositionId,
                                ARDLO_DLOKOD   AS Code,
                                ARDLO_TEXTDL   AS PositionName,
                                ARDLO_VRIKOD   AS TypeKey,
                                ARDLO_KZVKOD   AS KindKey,
                                ARDLO_KATKOD   AS MilitaryCategoryId,
                                ARDLO_VSOKOD   AS MilReportingSpecialityCode,
                                ARDLO_MISIA    AS Mission,
                                ARDLO_ZPVD     AS VaccAnnNum,
                                ARDLO_IZD      AS VaccAnnDateVacAnn,
                                ARDLO_KOGA     As VaccAnnDateWhen,
                                ARDLO_SPZKOD   AS MilitaryCommanderRankCode,
                                ARDLO_VPN      AS MilitaryUnitVpn,
                                ARDLO_IMEPOD   AS MilitaryUnitName,
                                ARDLO_IMEOE    AS OrganisationUnit,
                                ARDLO_NMAKOD   AS GarrisonCityId,
                                ARDLO_MILITARYDEPARTMENTID AS MilitaryDepartmentId,
                                ARDLO_ENDDATE  AS VaccAnnDateEnd
                          
                              FROM VS_OWNER.VS_AR_DLO a

                              WHERE a.ARDLO_EGNLS = :IdentityNumber
                              AND   a.ARDLO_KOGA = :VaccAnnDateWhen
                              AND   a.ARDLO_ZPVD = :VaccAnnNum";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("VaccAnnDateWhen", OracleType.DateTime).Value = vaccAnnDateWhen;
                cmd.Parameters.Add("VaccAnnNum", OracleType.VarChar).Value = vaccAnnNum;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personPreviousPosition = ExtractPersonPreviousPositionFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPreviousPosition;
        }

        public static List<PersonPreviousPosition> GetAllPersonPreviousPositionByPersonID(int personId, User currentUser)
        {
            List<PersonPreviousPosition> listPersonPreviousPosition = new List<PersonPreviousPosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                                ARDLOID        AS PersonPreviousPositionId,
                                ARDLO_DLOKOD   AS Code,
                                ARDLO_TEXTDL   AS PositionName,
                                ARDLO_VRIKOD   AS TypeKey,
                                ARDLO_KZVKOD   AS KindKey,
                                ARDLO_KATKOD   AS MilitaryCategoryId,
                                ARDLO_VSOKOD   AS MilReportingSpecialityCode,
                                ARDLO_MISIA    AS Mission,
                                ARDLO_ZPVD     AS VaccAnnNum,
                                ARDLO_IZD      AS VaccAnnDateVacAnn,
                                ARDLO_KOGA     As VaccAnnDateWhen,
                                ARDLO_SPZKOD   AS MilitaryCommanderRankCode,
                                ARDLO_VPN      AS MilitaryUnitVpn,
                                ARDLO_IMEPOD   AS MilitaryUnitName,
                                ARDLO_IMEOE    AS OrganisationUnit,
                                ARDLO_NMAKOD   AS GarrisonCityId,
                                ARDLO_MILITARYDEPARTMENTID AS MilitaryDepartmentId,
                                ARDLO_ENDDATE  AS VaccAnnDateEnd                                

                               FROM VS_OWNER.VS_AR_DLO a 
                               INNER JOIN VS_OWNER.VS_LS c ON a.ARDLO_EGNLS = c.EGN
                               WHERE c.PersonID = :PersonID
                               ORDER BY a.ARDLO_IZD, a.ARDLO_ZPVD";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonPreviousPosition.Add(ExtractPersonPreviousPositionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonPreviousPosition;
        }

        public static bool SavePersonPreviousPosition(PersonPreviousPosition personPreviousPosition, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (personPreviousPosition.PersonPreviousPositionId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_AR_DLO (
                                    ARDLO_EGNLS,
                                    ARDLO_DLOKOD,
                                    ARDLO_TEXTDL,
                                    ARDLO_VRIKOD,
                                    ARDLO_KZVKOD,
                                    ARDLO_KATKOD,
                                    ARDLO_VSOKOD,
                                    ARDLO_MISIA,
                                    ARDLO_ZPVD,
                                    ARDLO_IZD,
                                    ARDLO_KOGA,
                                    ARDLO_SPZKOD,
                                    ARDLO_VPN,
                                    ARDLO_IMEPOD,
                                    ARDLO_IMEOE,
                                    ARDLO_NMAKOD,
                                    ARDLO_MILITARYDEPARTMENTID,
                                    ARDLO_ENDDATE
                                    )

                            VALUES (
                                    :IdentityNumber, 
                                    :Code,
                                    :PositionName,
                                    :TypeKey,
                                    :KindKey,
                                    :MilitaryCategoryId,
                                    :MilReportingSpecialityCode,
                                    :Mission,
                                    :VaccAnnNum,
                                    :VaccAnnDateVacAnn,
                                    :VaccAnnDateWhen,
                                    :MilitaryCommanderRankCode,
                                    :MilitaryUnitVpn,
                                    :MilitaryUnitName,
                                    :OrganisationUnit,
                                    :GarrisonCityId,
                                    :MilitaryDepartmentId,
                                    :VaccAnnDateEnd
                                    );

                           SELECT VS_OWNER.VS_AR_DLO_ARDLOID_SEQ.currval INTO :PersonPreviousPositionId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_AddPreviousPosition", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_PositionCode", "", personPreviousPosition.PersonPreviousPositionCode, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_PositionName", "", personPreviousPosition.PersonPreviousPositionPositionName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_TypeKey", "", personPreviousPosition.PersonPreviousPositionType.PreviousPositionTypeName, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_KindKey", "", personPreviousPosition.PersonPreviousPositionKind != null ? personPreviousPosition.PersonPreviousPositionKind.PreviousPositionKindName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_Category", "", personPreviousPosition.PersonPreviousPositionMilitaryCategory != null ? personPreviousPosition.PersonPreviousPositionMilitaryCategory.CategoryName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_MilRepSpec", "", !string.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode) ? personPreviousPosition.MilitaryReportSpeciality.CodeAndName : "", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_Mission", "", personPreviousPosition.PersonPreviousPositionMission ? "1" : "0", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnNum", "", personPreviousPosition.PersonPreviousPositionVaccAnnNum, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateVacAnn", "", CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn), currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateWhen", "", CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateWhen), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateEnd", "", CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_CommanderRank", "", personPreviousPosition.PersonPreviousPositionMilitaryCommanderRank != null ? personPreviousPosition.PersonPreviousPositionMilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_MilitaryUnit", "", personPreviousPosition.PersonPreviousPositionMilitaryUnit.DisplayTextForSelection, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_OrganisationUnit", "", personPreviousPosition.PersonPreviousPositionOrganisationUnit, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_GarrisonCity", "", personPreviousPosition.PersonPreviousPositionGarrisonName, currentUser));

                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_AR_DLO SET

                                    ARDLO_DLOKOD   = :Code,
                                    ARDLO_TEXTDL   = :PositionName,
                                    ARDLO_VRIKOD   = :TypeKey,
                                    ARDLO_KZVKOD   = :KindKey,
                                    ARDLO_KATKOD   = :MilitaryCategoryId,
                                    ARDLO_VSOKOD   = :MilReportingSpecialityCode,
                                    ARDLO_MISIA    = :Mission,
                                    ARDLO_ZPVD     = :VaccAnnNum,
                                    ARDLO_IZD      = :VaccAnnDateVacAnn,
                                    ARDLO_KOGA     = :VaccAnnDateWhen,
                                    ARDLO_SPZKOD   = :MilitaryCommanderRankCode,
                                    ARDLO_VPN      = :MilitaryUnitVpn,
                                    ARDLO_IMEPOD   = :MilitaryUnitName,
                                    ARDLO_IMEOE    = :OrganisationUnit,
                                    ARDLO_NMAKOD   = :GarrisonCityId,
                                    ARDLO_MILITARYDEPARTMENTID = :MilitaryDepartmentId,
                                    ARDLO_ENDDATE = :VaccAnnDateEnd


                              WHERE ARDLOID = :PersonPreviousPositionId ; ";


                    PersonPreviousPosition oldPersonPreviousPosition = GetPersonPreviousPosition(personPreviousPosition.PersonPreviousPositionId, currentUser);

                    string logDescription = "Заповед номер: " + oldPersonPreviousPosition.PersonPreviousPositionVaccAnnNum + "; " +
                                            "В сила от: " + CommonFunctions.FormatDate(oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateWhen);

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_EditPreviousPosition", logDescription, null, person, currentUser);

                    if (oldPersonPreviousPosition.PersonPreviousPositionCode != personPreviousPosition.PersonPreviousPositionCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_PositionCode", oldPersonPreviousPosition.PersonPreviousPositionCode, personPreviousPosition.PersonPreviousPositionCode, currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionPositionName != personPreviousPosition.PersonPreviousPositionPositionName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_PositionName", oldPersonPreviousPosition.PersonPreviousPositionPositionName, personPreviousPosition.PersonPreviousPositionPositionName, currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionTypeKey != personPreviousPosition.PersonPreviousPositionTypeKey)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_TypeKey", oldPersonPreviousPosition.PersonPreviousPositionType.PreviousPositionTypeName, personPreviousPosition.PersonPreviousPositionType.PreviousPositionTypeName, currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionKindKey != personPreviousPosition.PersonPreviousPositionKindKey)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_KindKey", oldPersonPreviousPosition.PersonPreviousPositionKind != null ? oldPersonPreviousPosition.PersonPreviousPositionKind.PreviousPositionKindName : "", personPreviousPosition.PersonPreviousPositionKind != null ? personPreviousPosition.PersonPreviousPositionKind.PreviousPositionKindName : "", currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionMilitaryCategoryId != personPreviousPosition.PersonPreviousPositionMilitaryCategoryId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_Category", oldPersonPreviousPosition.PersonPreviousPositionMilitaryCategory != null ? oldPersonPreviousPosition.PersonPreviousPositionMilitaryCategory.CategoryName : "", personPreviousPosition.PersonPreviousPositionMilitaryCategory != null ? personPreviousPosition.PersonPreviousPositionMilitaryCategory.CategoryName : "", currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode != personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_MilRepSpec", oldPersonPreviousPosition.MilitaryReportSpeciality != null ? oldPersonPreviousPosition.MilitaryReportSpeciality.CodeAndName : "", personPreviousPosition.MilitaryReportSpeciality != null ? personPreviousPosition.MilitaryReportSpeciality.CodeAndName : "", currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionMission != personPreviousPosition.PersonPreviousPositionMission)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_Mission", oldPersonPreviousPosition.PersonPreviousPositionMission ? "1" : "0", personPreviousPosition.PersonPreviousPositionMission ? "1" : "0", currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionVaccAnnNum != personPreviousPosition.PersonPreviousPositionVaccAnnNum)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnNum", oldPersonPreviousPosition.PersonPreviousPositionVaccAnnNum, personPreviousPosition.PersonPreviousPositionVaccAnnNum, currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn != personPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateVacAnn", CommonFunctions.FormatDate(oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn), CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn), currentUser));


                    if (oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateWhen != personPreviousPosition.PersonPreviousPositionVaccAnnDateWhen)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateWhen", CommonFunctions.FormatDate(oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateWhen), CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateWhen), currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateEnd != personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateEnd", CommonFunctions.FormatDate(oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateEnd), CommonFunctions.FormatDate(personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd), currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionMilitaryCommanderRankCode != personPreviousPosition.PersonPreviousPositionMilitaryCommanderRankCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_CommanderRank", oldPersonPreviousPosition.PersonPreviousPositionMilitaryCommanderRank != null ? oldPersonPreviousPosition.PersonPreviousPositionMilitaryCommanderRank.MilitaryCommanderRankName : "", personPreviousPosition.PersonPreviousPositionMilitaryCommanderRank != null ? personPreviousPosition.PersonPreviousPositionMilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionMilitaryUnitId != personPreviousPosition.PersonPreviousPositionMilitaryUnitId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_MilitaryUnit", oldPersonPreviousPosition.PersonPreviousPositionMilitaryUnit.DisplayTextForSelection, personPreviousPosition.PersonPreviousPositionMilitaryUnit.DisplayTextForSelection, currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionOrganisationUnit != personPreviousPosition.PersonPreviousPositionOrganisationUnit)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_OrganisationUnit", oldPersonPreviousPosition.PersonPreviousPositionOrganisationUnit, personPreviousPosition.PersonPreviousPositionOrganisationUnit, currentUser));

                    if (oldPersonPreviousPosition.PersonPreviousPositionGarrisonCityId != personPreviousPosition.PersonPreviousPositionGarrisonCityId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_GarrisonCity", oldPersonPreviousPosition.PersonPreviousPositionGarrisonName, personPreviousPosition.PersonPreviousPositionGarrisonName, currentUser));



                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPreviousPositionId = new OracleParameter();
                paramPreviousPositionId.ParameterName = "PersonPreviousPositionId";
                paramPreviousPositionId.OracleType = OracleType.Number;

                if (personPreviousPosition.PersonPreviousPositionId != 0)
                {
                    paramPreviousPositionId.Direction = ParameterDirection.Input;
                    paramPreviousPositionId.Value = personPreviousPosition.PersonPreviousPositionId;
                }
                else
                {
                    paramPreviousPositionId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPreviousPositionId);

                OracleParameter param = null;

                if (personPreviousPosition.PersonPreviousPositionId == 0)
                {
                    //Insert
                    param = new OracleParameter();
                    param.ParameterName = "IdentityNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.IdentNumber;
                    cmd.Parameters.Add(param);
                }
                else
                {
                    //Update
                    param = new OracleParameter();
                    param.ParameterName = "PersonPreviousPositionId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personPreviousPosition.PersonPreviousPositionId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "Code";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PositionName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionPositionName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TypeKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionTypeKey;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "KindKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionKindKey))
                {
                    param.Value = personPreviousPosition.PersonPreviousPositionKindKey;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "MilitaryCategoryId";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionMilitaryCategoryId))
                {
                    param.Value = personPreviousPosition.PersonPreviousPositionMilitaryCategoryId;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //VOS
                param = new OracleParameter();
                param.ParameterName = "MilReportingSpecialityCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode))
                {
                    param.Value = personPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "Mission";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionMission ? "1" : "0";
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VaccAnnNum";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionVaccAnnNum;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VaccAnnDateVacAnn";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VaccAnnDateWhen";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionVaccAnnDateWhen;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommanderRankCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personPreviousPosition.PersonPreviousPositionMilitaryCommanderRankCode))
                {
                    param.Value = personPreviousPosition.PersonPreviousPositionMilitaryCommanderRankCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitVpn";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionMilitaryUnit.VPN;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionMilitaryUnit.ShortName;
                cmd.Parameters.Add(param);



                param = new OracleParameter();
                param.ParameterName = "OrganisationUnit";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionOrganisationUnit;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "GarrisonCityId";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personPreviousPosition.PersonPreviousPositionGarrisonCityId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryDepartmentId";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (personPreviousPosition.PersonPreviousPositionMilitaryDepartmentId.HasValue)
                {
                    param.Value = personPreviousPosition.PersonPreviousPositionMilitaryDepartmentId.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "VaccAnnDateEnd";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd.HasValue)
                {
                    param.Value = personPreviousPosition.PersonPreviousPositionVaccAnnDateEnd;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personPreviousPosition.PersonPreviousPositionId == 0)
                    personPreviousPosition.PersonPreviousPositionId = DBCommon.GetInt(paramPreviousPositionId.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                    PersonUtil.SetPersonModified(person.PersonId, currentUser);
                }
            }

            return result;
        }

        public static bool DeletePersonPreviousPosition(int personPreviousPositionId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonPreviousPosition oldPersonPreviousPosition = GetPersonPreviousPosition(personPreviousPositionId, currentUser);

            //Bind MilitaryReportSpeciality obect to get MilitaryReportSpecialityType
            //int personPreviousPositionMilReportingSpecialityCode = 0;
            MilitaryReportSpeciality militaryReportSpeciality = new MilitaryReportSpeciality(currentUser);
            if (!string.IsNullOrEmpty(oldPersonPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode))
            {
                //int.TryParse(oldPersonPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode, out personPreviousPositionMilReportingSpecialityCode);
                //if (personPreviousPositionMilReportingSpecialityCode > 0)
                //{
                //    militaryReportSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(personPreviousPositionMilReportingSpecialityCode, currentUser);
                //}

                militaryReportSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialityByCode(oldPersonPreviousPosition.PersonPreviousPositionMilReportingSpecialityCode, currentUser);
            }

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_MilServ_DeletePreviousPosition", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_PositionCode", oldPersonPreviousPosition.PersonPreviousPositionCode, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_PositionName", oldPersonPreviousPosition.PersonPreviousPositionPositionName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_TypeKey", oldPersonPreviousPosition.PersonPreviousPositionType.PreviousPositionTypeName, "", currentUser));

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_KindKey", oldPersonPreviousPosition.PersonPreviousPositionKind != null ? oldPersonPreviousPosition.PersonPreviousPositionKind.PreviousPositionKindName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_Category", oldPersonPreviousPosition.PersonPreviousPositionMilitaryCategory != null ? oldPersonPreviousPosition.PersonPreviousPositionMilitaryCategory.CategoryName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_MilRepSpec", oldPersonPreviousPosition.MilitaryReportSpeciality != null ? oldPersonPreviousPosition.MilitaryReportSpeciality.MilReportingSpecialityCode + " " + oldPersonPreviousPosition.MilitaryReportSpeciality.MilReportingSpecialityName : "", "", currentUser));

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_Mission", oldPersonPreviousPosition.PersonPreviousPositionMission ? "1" : "0", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnNum", oldPersonPreviousPosition.PersonPreviousPositionVaccAnnNum, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateVacAnn", CommonFunctions.FormatDate(oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateVacAnn), "", currentUser));

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateWhen", CommonFunctions.FormatDate(oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateWhen), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_VaccAnnDateEnd", CommonFunctions.FormatDate(oldPersonPreviousPosition.PersonPreviousPositionVaccAnnDateEnd), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_CommanderRank", oldPersonPreviousPosition.PersonPreviousPositionMilitaryCommanderRank != null ? oldPersonPreviousPosition.PersonPreviousPositionMilitaryCommanderRank.MilitaryCommanderRankName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_MilitaryUnit", oldPersonPreviousPosition.PersonPreviousPositionMilitaryUnit.DisplayTextForSelection, "", currentUser));

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_OrganisationUnit", oldPersonPreviousPosition.PersonPreviousPositionOrganisationUnit, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_PreviousPosition_GarrisonCity", oldPersonPreviousPosition.PersonPreviousPositionGarrisonName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_AR_DLO WHERE ARDLOID = :PersonPreviousPositionId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonPreviousPositionId", OracleType.Number).Value = personPreviousPositionId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);
                PersonUtil.SetPersonModified(person.PersonId, currentUser);
            }

            return result;
        }

    }
}
