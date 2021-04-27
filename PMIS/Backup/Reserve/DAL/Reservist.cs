using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Reservist into the system
    public class Reservist : BaseDbObject
    {
        private int reservistId;
        private int personId;
        private Person person;
        private string groupManagementSection;        
        private string section;        
        private string deliverer;        
        private int? punktID;        
        private RequestCommandPunkt punkt;        
        private ReservistMilRepStatus currResMilRepStatus;

        public int ReservistId
        {
            get
            {
                return reservistId;
            }
            set
            {
                reservistId = value;
            }
        }

        public int PersonId
        {
            get
            {
                return personId;
            }
            set
            {
                personId = value;
            }
        }

        public Person Person
        {
            get
            {
                //Lazy initialization
                if (person == null)
                    person = PersonUtil.GetPerson(PersonId, CurrentUser);

                return person;
            }
            set
            {
                person = value;
            }
        }

        public string GroupManagementSection
        {
            get { return groupManagementSection; }
            set { groupManagementSection = value; }
        }

        public string Section
        {
            get { return section; }
            set { section = value; }
        }

        public string Deliverer
        {
            get { return deliverer; }
            set { deliverer = value; }
        }

        public int? PunktID
        {
            get { return punktID; }
            set { punktID = value; }
        }

        public RequestCommandPunkt Punkt
        {
            get 
            {
                if (punkt == null && punktID.HasValue)
                    punkt = RequestCommandPunktUtil.GetRequestCommandPunkt(punktID.Value, CurrentUser);

                return punkt; 
            }
            set { punkt = value; }
        }

        public ReservistMilRepStatus CurrResMilRepStatus
        {
            get
            {
                //Lazy initialization
                if (currResMilRepStatus == null)
                    currResMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(ReservistId, CurrentUser);

                return currResMilRepStatus;
            }
            set
            {
                currResMilRepStatus = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public bool CanEdit
        {
            get
            {
                bool canEdit = true;

                if (Person != null)
                {
                    if (Config.GetWebSetting("KOD_KZV_Check_Reservist").ToLower() == "true" &&
                        CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Reservist")) &&
                        !CommonFunctions.IsKeyInList(person.CategoryCode, Config.GetWebSetting("KOD_KAT_Restricted_Reservist_Exceptions")))
                    {
                        canEdit = false;
                    }
                }

                return canEdit;
            }
        }

        public bool CanAccessMilitaryDepartment(User currentUser)
        {
            bool canAccess = true;

            if (this != null)
            {
                if(this.CurrResMilRepStatus != null &&
                   this.CurrResMilRepStatus.SourceMilDepartment != null)
                {
                    canAccess = false;

                    List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsPerUser(currentUser, currentUser);

                    foreach(MilitaryDepartment militaryDepartment in militaryDepartments)
                    {
                        if(militaryDepartment.MilitaryDepartmentId == this.CurrResMilRepStatus.SourceMilDepartment.MilitaryDepartmentId)
                        {
                            canAccess = true;
                            break;
                        }
                    }
                }
            }

            return canAccess;
        }

        public Reservist(User user)
            : base(user)
        {

        }
    }

    public class ReservistSearchFilter
    {
        string firstAndSurName;
        string familyName;
        string initials;        
        string identNumber;        
        string mobilAppointmentPosition;
        string milRepSpecType;        
        string milRepSpec;
        bool isPrimaryMilRepSpec;
        string positionTitle;
        bool isPrimaryPositionTitle;
        string militaryRank;        
        string civilSpeciality;        
        int? age;        
        string language;        
        string region;        
        string municipality;        
        string city;        
        string workPosition;        
        string education;        
        string militaryTraining;
        string workUnifiedIdentityCode;
        string workCompanyName;
        string isSuitableForMobAppointment;

        int orderBy;        
        int pageIdx;        

        public string FirstAndSurName
        {
            get { return firstAndSurName; }
            set { firstAndSurName = value; }
        }

        public string FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }

        public string Initials
        {
            get { return initials; }
            set { initials = value; }
        }

        public string IdentNumber
        {
            get { return identNumber; }
            set { identNumber = value; }
        }

        public string MobilAppointmentPosition
        {
            get { return mobilAppointmentPosition; }
            set { mobilAppointmentPosition = value; }
        }

        public string MilRepSpecType
        {
            get { return milRepSpecType; }
            set { milRepSpecType = value; }
        }

        public string MilRepSpec
        {
            get { return milRepSpec; }
            set { milRepSpec = value; }
        }

        public bool IsPrimaryMilRepSpec
        {
            get { return isPrimaryMilRepSpec; }
            set { isPrimaryMilRepSpec = value; }
        }

        public string PositionTitle
        {
            get { return positionTitle; }
            set { positionTitle = value; }
        }

        public bool IsPrimaryPositionTitle
        {
            get { return isPrimaryPositionTitle; }
            set { isPrimaryPositionTitle = value; }
        }

        public string MilitaryRank
        {
            get { return militaryRank; }
            set { militaryRank = value; }
        }

        public string CivilSpeciality
        {
            get { return civilSpeciality; }
            set { civilSpeciality = value; }
        }

        public int? Age
        {
            get { return age; }
            set { age = value; }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string Municipality
        {
            get { return municipality; }
            set { municipality = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string WorkPosition
        {
            get { return workPosition; }
            set { workPosition = value; }
        }

        public string Education
        {
            get { return education; }
            set { education = value; }
        }

        public string MilitaryTraining
        {
            get { return militaryTraining; }
            set { militaryTraining = value; }
        }

        public string WorkUnifiedIdentityCode
        {
            get { return workUnifiedIdentityCode; }
            set { workUnifiedIdentityCode = value; }
        }

        public string WorkCompanyName
        {
            get { return workCompanyName; }
            set { workCompanyName = value; }
        }

        public int OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int PageIdx
        {
            get { return pageIdx; }
            set { pageIdx = value; }
        }

        public string IsSuitableForMobAppointment
        {
            get { return isSuitableForMobAppointment; }
            set { isSuitableForMobAppointment = value; }
        }
    }

    public class ReservistManageFilter
    {
        string firstAndSurName;
        string familyName;
        string initials;
        string identNumber;
        string militaryCategory;        
        string militaryRank;
        string militaryReportStatus;
        string militaryCommand;
        string militaryDepartment;
        string position;
        string milAppointedRepSpecType;        
        string milAppointedRepSpec;        
        string milRepSpecType;
        string milRepSpec;
        bool isPrimaryMilRepSpec;
        string positionTitle;
        bool isPrimaryPositionTitle;
        string administration;        
        string language;
        string education;        
        string civilSpeciality;
        bool isPermAddress;               
        string region;
        string municipality;
        string city;
        string district;        
        string postCode;
        string address;
        string workUnifiedIdentityCode;
        string workCompanyName;
        bool hasBeenOnMission;
        string appointmentIsDelivered;
        string isSuitableForMobAppointment;

        
        string readiness;

        int orderBy;
        int pageIdx;

        public string FirstAndSurName
        {
            get { return firstAndSurName; }
            set { firstAndSurName = value; }
        }

        public string FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }

        public string Initials
        {
            get { return initials; }
            set { initials = value; }
        }

        public string IdentNumber
        {
            get { return identNumber; }
            set { identNumber = value; }
        }

        public string MilitaryCategory
        {
            get { return militaryCategory; }
            set { militaryCategory = value; }
        }

        public string MilitaryRank
        {
            get { return militaryRank; }
            set { militaryRank = value; }
        }

        public string MilitaryReportStatus
        {
            get { return militaryReportStatus; }
            set { militaryReportStatus = value; }
        }

        public string MilitaryCommand
        {
            get { return militaryCommand; }
            set { militaryCommand = value; }
        }

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        public string MilAppointedRepSpecType
        {
            get { return milAppointedRepSpecType; }
            set { milAppointedRepSpecType = value; }
        }

        public string MilAppointedRepSpec
        {
            get { return milAppointedRepSpec; }
            set { milAppointedRepSpec = value; }
        }

        public string MilRepSpecType
        {
            get { return milRepSpecType; }
            set { milRepSpecType = value; }
        }

        public string MilRepSpec
        {
            get { return milRepSpec; }
            set { milRepSpec = value; }
        }

        public bool IsPrimaryMilRepSpec
        {
            get { return isPrimaryMilRepSpec; }
            set { isPrimaryMilRepSpec = value; }
        }

        public string PositionTitle
        {
            get { return positionTitle; }
            set { positionTitle = value; }
        }

        public bool IsPrimaryPositionTitle
        {
            get { return isPrimaryPositionTitle; }
            set { isPrimaryPositionTitle = value; }
        }

        public string Administration
        {
            get { return administration; }
            set { administration = value; }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public string Education
        {
            get { return education; }
            set { education = value; }
        }        

        public string CivilSpeciality
        {
            get { return civilSpeciality; }
            set { civilSpeciality = value; }
        }

        public bool IsPermAddress
        {
            get { return isPermAddress; }
            set { isPermAddress = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string Municipality
        {
            get { return municipality; }
            set { municipality = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string District
        {
            get { return district; }
            set { district = value; }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string WorkUnifiedIdentityCode
        {
            get { return workUnifiedIdentityCode; }
            set { workUnifiedIdentityCode = value; }
        }

        public string WorkCompanyName
        {
            get { return workCompanyName; }
            set { workCompanyName = value; }
        }

        public bool HasBeenOnMission
        {
            get { return hasBeenOnMission; }
            set { hasBeenOnMission = value; }
        }

        public string AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }

        public string IsSuitableForMobAppointment
        {
            get { return isSuitableForMobAppointment; }
            set { isSuitableForMobAppointment = value; }
        }

        public string Readiness
        {
            get { return readiness; }
            set { readiness = value; }
        }
      
        public string ProfessionId { get; set; }
        public string SpecialityId { get; set; }
        
        public int OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int PageIdx
        {
            get { return pageIdx; }
            set { pageIdx = value; }
        }
    }

    public class ReservistGroupTakingDownFilter
    {
        int? age;
        string todate;
        string gender;
        string militaryCategory;
        string militaryRank;
        string administration;
        string militaryDepartment;
        string milRepSpecType;
        string milRepSpec;
        bool isPrimaryMilRepSpec;
        bool onlyMobileAppointed = false;        

        public int? Age
        {
            get { return age; }
            set { age = value; }
        }

        public string ToDate
        {
            get { return todate; }
            set { todate = value; }
        }

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public string MilitaryCategory
        {
            get { return militaryCategory; }
            set { militaryCategory = value; }
        }

        public string MilitaryRank
        {
            get { return militaryRank; }
            set { militaryRank = value; }
        }

        public string Administration
        {
            get { return administration; }
            set { administration = value; }
        }

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }

        public string MilRepSpecType
        {
            get { return milRepSpecType; }
            set { milRepSpecType = value; }
        }

        public string MilRepSpec
        {
            get { return milRepSpec; }
            set { milRepSpec = value; }
        }

        public bool IsPrimaryMilRepSpec
        {
            get { return isPrimaryMilRepSpec; }
            set { isPrimaryMilRepSpec = value; }
        }

        public bool OnlyMobileAppointed
        {
            get { return onlyMobileAppointed; }
            set { onlyMobileAppointed = value; }
        }

        int orderBy;
        int pageIdx;

        public int OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int PageIdx
        {
            get { return pageIdx; }
            set { pageIdx = value; }
        }
    }

    public class ReservistSearchBlock
    {
        private int reservistID;        
        private string identNumber;
        private string firstAndSurName;
        private string familyName;        
        private string milRepSpecHTML;        
        private string militaryRankName;        
        private string education;        
        private int age;        
        private string regionMunicipalityAndCity;        
        private string languages;
        private string positionTitle;
        private string specialities;
        private int? militaryTraining;
        //private string isSuitableForMobAppointment;

        public int ReservistID
        {
            get { return reservistID; }
            set { reservistID = value; }
        }

        public string IdentNumber
        {
            get { return identNumber; }
            set { identNumber = value; }
        }

        public string FirstAndSurName
        {
            get { return firstAndSurName; }
            set { firstAndSurName = value; }
        }

        public string FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }

        public string MilRepSpecHTML
        {
            get { return milRepSpecHTML; }
            set { milRepSpecHTML = value; }
        }

        public string MilitaryRankName
        {
            get { return militaryRankName; }
            set { militaryRankName = value; }
        }

        public string Education
        {
            get { return education; }
            set { education = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public string RegionMunicipalityAndCity
        {
            get { return regionMunicipalityAndCity; }
            set { regionMunicipalityAndCity = value; }
        }

        public string Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string PositionTitle
        {
            get { return positionTitle; }
            set { positionTitle = value; }
        }

        public string Specialities
        {
            get { return specialities; }
            set { specialities = value; }
        }

        public int? MilitaryTraining
        {
            get { return militaryTraining; }
            set { militaryTraining = value; }
        }

        public string MilitaryTrainingText
        {
            get
            {
                if (MilitaryTraining.HasValue)
                {
                    switch (MilitaryTraining.Value)
                    {
                        case 1:
                            return "С военна подготовка";
                        case 2:
                            return "Без военна подготовка";
                        default:
                            return "";
                    }
                }
                else
                    return "";
            }
        }

        //public string IsSuitableForMobAppointment
        //{
        //    get { return isSuitableForMobAppointment; }
        //    set { isSuitableForMobAppointment = value; }
        //}
    }

    public class ReservistManageBlock
    {
        private int reservistID;        
        private string firstAndSurName;
        private string familyName;
        private string identNumber;        
        private string militaryRankName;        
        private string regionMuniciplaityAndCity;        
        private string militaryDepartment;  
        private string militaryCategory;     
        private string militaryReportStatus;
        private string militaryCommand;
        private string milReportingSpecialityCode;
        private string positionTitle;

        public int ReservistID
        {
            get { return reservistID; }
            set { reservistID = value; }
        }

        public string FirstAndSurName
        {
            get { return firstAndSurName; }
            set { firstAndSurName = value; }
        }

        public string FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }

        public string IdentNumber
        {
            get { return identNumber; }
            set { identNumber = value; }
        }

        public string MilitaryRankName
        {
            get { return militaryRankName; }
            set { militaryRankName = value; }
        }

        public string RegionMuniciplaityAndCity
        {
            get { return regionMuniciplaityAndCity; }
            set { regionMuniciplaityAndCity = value; }
        }

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }

        public string MilitaryCategory
        {
            get { return militaryCategory; }
            set { militaryCategory = value; }
        }

        public string MilitaryReportStatus
        {
            get { return militaryReportStatus; }
            set { militaryReportStatus = value; }
        }

        public string MilitaryCommand
        {
            get { return militaryCommand; }
            set { militaryCommand = value; }
        }

        public string MilReportingSpecialityCode
        {
            get { return milReportingSpecialityCode; }
            set { milReportingSpecialityCode = value; }
        }

        public string PositionTitle
        {
            get { return positionTitle; }
            set { positionTitle = value; }
        }
    }

    public class ReservistGroupTakingDownBlock
    {
        private int reservistID;
        private string fullName;
        private string identNumber;
        private string gender;
        private string administration;       
        private string militaryReportStatus;
        private string militaryCommand;

        public int ReservistID
        {
            get { return reservistID; }
            set { reservistID = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string IdentNumber
        {
            get { return identNumber; }
            set { identNumber = value; }
        }

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public string Administration
        {
            get { return administration; }
            set { administration = value; }
        }
       
        public string MilitaryReportStatus
        {
            get { return militaryReportStatus; }
            set { militaryReportStatus = value; }
        }

        public string MilitaryCommand
        {
            get { return militaryCommand; }
            set { militaryCommand = value; }
        }
    }

    public static class ReservistUtil
    {
        //This method creates and returns a FillReservistsRequest object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static Reservist ExtractReservist(OracleDataReader dr, User currentUser)
        {
            Reservist reservist = new Reservist(currentUser);

            reservist.ReservistId = DBCommon.GetInt(dr["ReservistID"]);
            reservist.PersonId = DBCommon.GetInt(dr["PersonID"]);
            reservist.GroupManagementSection = dr["GroupManagementSection"].ToString();
            reservist.Section = dr["Section"].ToString();
            reservist.Deliverer = dr["Deliverer"].ToString();
            reservist.PunktID = DBCommon.IsInt(dr["PunktID"]) ? DBCommon.GetInt(dr["PunktID"]) : (int?)null;

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, reservist);

            return reservist;
        }

        //Get a particular object by its ID
        public static Reservist GetReservist(int reservistId, User currentUser)
        {
            Reservist reservist = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.ReservistID, a.PersonID, a.GroupManagementSection, a.Section, a.Deliverer, a.PunktID,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_RES.Reservists a
                               WHERE a.ReservistID = :ReservistID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistID", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reservist = ExtractReservist(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservist;
        }

        //Get a particular reservist by its IdentNumber
        public static Reservist GetReservistByIdentNumber(string identNumber, User currentUser)
        {
            Reservist reservist = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                string SQL = @"SELECT a.ReservistID, a.PersonID, a.GroupManagementSection, a.Section, a.Deliverer, a.PunktID,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_RES.Reservists a
                               INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                               WHERE b.EGN = :IdentNumber " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentNumber", OracleType.VarChar).Value = identNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reservist = ExtractReservist(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservist;
        }

        public static List<ReservistSearchBlock> GetAllReservistSearchBlocks(ReservistSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<ReservistSearchBlock> reservistSearchBlocks = new List<ReservistSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_VS_LS = false;
                bool join_ReservistMilRepStatuses = false;
                bool join_MilitaryReportStatuses = false;
                bool join_Persons = false;
                bool join_KLV_ZVA = false;
                bool join_KLV_OBR = false;
                bool join_KL_NMA = false;
                bool join_KL_OBS = false;
                bool join_KL_OBL = false;
                bool join_Companies = false;
                bool join_NKPD = false;
                bool join_PositionTitle = false;

                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.IdentNumber))
                {
                    if (filter.IdentNumber.Length == 10)
                        where += (where == "" ? "" : " AND ") +
                             " b.EGN = '" + filter.IdentNumber.Replace("'", "''") + "' ";
                    else
                        where += (where == "" ? "" : " AND ") +
                                 " b.EGN LIKE '" + filter.IdentNumber.Replace("'", "''") + "%' ";

                    join_VS_LS = true;
                }
                else
                {


                    if (!string.IsNullOrEmpty(filter.FirstAndSurName))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " Lower(b.IME) LIKE '%" + filter.FirstAndSurName.Replace("'", "''").ToLower() + "%' ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.FamilyName))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " Lower(b.FAM) LIKE '%" + filter.FamilyName.Replace("'", "''").ToLower() + "%' ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Initials))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " c.Initials LIKE '%" + filter.Initials.Replace("'", "''") + "%' ";

                        join_Persons = true;
                    }

                    if (!string.IsNullOrEmpty(filter.MobilAppointmentPosition))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" a.ReservistID IN (SELECT a.ReservistID 
                                                  FROM PMIS_RES.ReservistAppointments a
                                                  WHERE a.Position LIKE '%" + filter.MobilAppointmentPosition.Replace("'", "''") + "%') ";
                    }

                    string isPrimaryMilRepSpecFilter = "";

                    if (filter.IsPrimaryMilRepSpec)
                    {
                        isPrimaryMilRepSpecFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                    }

                    if (!string.IsNullOrEmpty(filter.MilRepSpecType))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a
                                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                                                WHERE  " + (filter.MilRepSpecType == "-2" ? " b.Active = 0 " : " b.Type IN (" + filter.MilRepSpecType + ") ") + isPrimaryMilRepSpecFilter + @" ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.IsSuitableForMobAppointment))
                    {
                        where += (where == "" ? "" : " AND ") +
                        @"NVL(c.IsSuitableForMobAppointment, 0) = " + ((filter.IsSuitableForMobAppointment == ListItems.GetOptionNo().Value) ? "0" : "1");

                        join_Persons = true;
                    }

                    if (!string.IsNullOrEmpty(filter.MilRepSpec))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a                                                
                                                WHERE  a.MilReportSpecialityID IN (" + filter.MilRepSpec + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                    }

                    string isPrimaryPositionTitleFilter = "";

                    if (filter.IsPrimaryPositionTitle)
                    {
                        isPrimaryPositionTitleFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                    }

                    if (!string.IsNullOrEmpty(filter.PositionTitle))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonPositionTitles a                                                
                                                WHERE  a.PositionTitleID IN (" + filter.PositionTitle + ") " + isPrimaryPositionTitleFilter + @" ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.MilitaryRank))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_ZVA IN ( " + filter.MilitaryRank + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.CivilSpeciality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.EGN IN ( SELECT a.OBRG_EGNLS 
                                           FROM VS_OWNER.VS_OBRG a                                           
                                           WHERE  a.OBRG_SPEKOD IN (" + filter.CivilSpeciality + ") ) ";

                        join_VS_LS = true;
                    }

                    if (filter.Age.HasValue)
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) <= " + filter.Age.Value + " ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Language))
                    {
                        where += (where == "" ? "" : " AND ") +
                                @" b.EGN IN ( SELECT a.EZIK_EGNLS 
                                           FROM VS_OWNER.VS_EZIK a                                           
                                           WHERE  a.EZIK_EZKKOD IN ('" + filter.Language.Replace("'", "''").Replace(",", "','") + "') ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( " + filter.City + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.WorkPosition))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " (nkpd.NKPDCode LIKE '%" + filter.WorkPosition.Replace("'", "''") + "%' OR UPPER(nkpd.NKPDName) LIKE UPPER('%" + filter.WorkPosition.Replace("'", "''") + "%')) ";

                        join_NKPD = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Education))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " c.EducationCode = '" + filter.Education + "' ";

                        join_Persons = true;
                    }

                    if (!string.IsNullOrEmpty(filter.MilitaryTraining))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " c.MilitaryTraining = " + filter.MilitaryTraining + " ";

                        join_Persons = true;
                    }

                    if (!string.IsNullOrEmpty(filter.WorkUnifiedIdentityCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" UPPER(wc.UnifiedIdentityCode) LIKE UPPER('%" + filter.WorkUnifiedIdentityCode.Replace("'", "''") + "%') ";

                        join_Companies = true;
                    }

                    if (!string.IsNullOrEmpty(filter.WorkCompanyName))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" UPPER(wc.CompanyName) LIKE UPPER('%" + filter.WorkCompanyName.Replace("'", "''") + "%') ";

                        join_Companies = true;
                    }
                }
                
                where += (where == "" ? "" : " AND ") +
                             @" a.ReservistID NOT IN ( SELECT ReservistID 
                                                       FROM PMIS_RES.FillReservistsRequest 
                                                       WHERE RequestCommandPositionID = " + requestCommandPositionID + @" AND
                                                             MilitaryDepartmentID = " + militaryDepartmentID + " ) ";
                 
                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filter.OrderBy > 100)
                {
                    filter.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filter.OrderBy)
                {
                    case 1:
                        orderBySQL = "b.EGN";
                        join_VS_LS = true;
                        break;
                    case 2:
                        orderBySQL = "b.IME";
                        join_VS_LS = true;
                        break;
                    case 3:
                        orderBySQL = "PMIS_ADM.COMMONFUNCTIONS.GetMRSPerPersonHTML(a.PersonID, 20)";
                        break;
                    case 4:
                        orderBySQL = "d.ZVA_IME";
                        join_KLV_ZVA = true;
                        break;
                    case 5:
                        orderBySQL = "e.OBR_IME";
                        join_KLV_OBR = true;
                        break;
                    case 6:
                        orderBySQL = "PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN)";
                        join_VS_LS = true;
                        break;
                    case 7:
                        orderBySQL = "h.IME_OBL || ' ' || g.IME_OBS || ' ' || f.IME_NMA";
                        join_KL_NMA = true;
                        join_KL_OBS = true;
                        join_KL_OBL = true;
                        break;
                    case 8:
                        orderBySQL = "PMIS_ADM.COMMONFUNCTIONS.GetLanguagesPerPerson(a.PersonID)"; 
                        break;
                    case 9:
                        orderBySQL = "PMIS_ADM.COMMONFUNCTIONS.GetSpecialitiesPerPerson(a.PersonID)";
                        break;
                    case 10:
                        orderBySQL = "c.MilitaryTraining";
                        join_Persons = true;
                        break;
                    case 11:
                        orderBySQL = " b.FAM";
                        join_VS_LS = true;
                        break;
                    case 12:
                        orderBySQL = "pt.PositionTitle";
                        join_PositionTitle = true;
                        break;
                    default:
                        orderBySQL = "b.EGN";
                        join_VS_LS = true;
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                join_MilitaryReportStatuses = true;

                if (join_MilitaryReportStatuses)
                {
                    join_ReservistMilRepStatuses = true;
                }

                if (join_KLV_ZVA)
                {
                    join_VS_LS = true;
                }

                if (join_KLV_OBR)
                {
                    join_Persons = true;
                }

                if (join_KL_NMA)
                {
                    join_VS_LS = true;
                }

                if (join_KL_OBS)
                {
                    join_VS_LS = true;
                    join_KL_NMA = true;
                }

                if (join_KL_OBL)
                {
                    join_VS_LS = true;
                    join_KL_NMA = true;
                }

                if (join_Companies)
                {
                    join_Persons = true;
                }

                if (join_NKPD)
                {
                    join_Persons = true;
                }

                string SQL = @"SELECT tmp.*,
                                      a.PersonID, a.GroupManagementSection, a.Section, a.Deliverer, a.PunktID,
                                      b.IME as FirstAndSurName, 
                                      b.FAM as FamilyName, 
                                      b.EGN as PersonIdentNumber,
                                      PMIS_ADM.COMMONFUNCTIONS.GetMRSPerPersonHTML(a.PersonID, 20) as MilRepSpecHTML,
                                      d.ZVA_IME as MilitaryRankName,
                                      e.OBR_IME as Education,
                                      PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age,
                                      f.IME_NMA as CityName,
                                      g.IME_OBS as MunicipalityName,
                                      h.IME_OBL as RegionName,
                                      PMIS_ADM.COMMONFUNCTIONS.GetLanguagesPerPerson(a.PersonID) as Languages,
                                      PMIS_ADM.COMMONFUNCTIONS.GetSpecialitiesPerPerson(a.PersonID) as Specialities,
                                      c.MilitaryTraining,
                                      pt.PositionTitle
                               FROM ( SELECT a.ReservistID, 
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.ReservistID) as RowNumber 
                                      FROM PMIS_RES.Reservists a                                                                
                   " + (join_VS_LS ? "INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID" : "") + @"
 " + (join_ReservistMilRepStatuses ? "INNER JOIN PMIS_RES.ReservistMilRepStatuses rs ON a.ReservistID = rs.ReservistID AND rs.IsCurrent = 1 AND rs.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"" : "") + @"
 " + (join_MilitaryReportStatuses ? @"INNER JOIN PMIS_RES.MilitaryReportStatuses s ON rs.MilitaryReportStatusID = s.MilitaryReportStatusID AND 
                                                                                 s.MilitaryReportStatusKey IN (" + ReservistUtil.SearchResMilRepStatuses() + @")" : "") + @"
                 " + (join_Persons ? "LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID" : "") + @"
                 " + (join_KLV_ZVA ? "LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON b.KOD_ZVA = d.ZVA_KOD" : "") + @"
                 " + (join_KLV_OBR ? "LEFT OUTER JOIN VS_OWNER.KLV_OBR e ON c.EducationCode = e.OBR_KOD" : "") + @"
                  " + (join_KL_NMA ? "LEFT OUTER JOIN UKAZ_OWNER.KL_NMA f ON b.KOD_NMA_MJ = f.KOD_NMA" : "") + @"
                  " + (join_KL_OBS ? "LEFT OUTER JOIN UKAZ_OWNER.KL_OBS g ON f.KOD_OBS = g.KOD_OBS" : "") + @"
                  " + (join_KL_OBL ? "LEFT OUTER JOIN UKAZ_OWNER.KL_OBL h ON f.KOD_OBL = h.KOD_OBL" : "") + @"
               " + (join_Companies ? "LEFT OUTER JOIN PMIS_ADM.Companies wc ON c.WorkCompanyID = wc.CompanyID" : "") + @"
                    " + (join_NKPD ? "LEFT OUTER JOIN PMIS_ADM.NKPD nkpd ON c.WorkPositionNKPDID = nkpd.NKPDID" : "") + @"
           " + (join_PositionTitle ? "LEFT OUTER JOIN PMIS_ADM.PersonPositionTitles ppt ON ppt.PersonID = a.PersonID AND ppt.IsPrimary = 1 LEFT OUTER JOIN PMIS_ADM.PositionTitles pt ON pt.PositionTitleID = ppt.PositionTitleID" : "") + @"
                                     " + where + @"
                                    ) tmp
                               INNER JOIN PMIS_RES.Reservists a ON tmp.ReservistID = a.ReservistID
                               INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON b.KOD_ZVA = d.ZVA_KOD
                               LEFT OUTER JOIN VS_OWNER.KLV_OBR e ON c.EducationCode = e.OBR_KOD
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA f ON b.KOD_NMA_MJ = f.KOD_NMA
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS g ON f.KOD_OBS = g.KOD_OBS
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL h ON f.KOD_OBL = h.KOD_OBL
                               LEFT OUTER JOIN PMIS_ADM.PersonPositionTitles ppt ON ppt.PersonID = b.PersonID AND ppt.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.PositionTitles pt ON ppt.PositionTitleID = pt.PositionTitleID
                               " + pageWhere + @"
                               ORDER BY RowNumber";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReservistSearchBlock reservistSearchBlock = new ReservistSearchBlock();

                    reservistSearchBlock.ReservistID = DBCommon.GetInt(dr["ReservistID"]);
                    reservistSearchBlock.FirstAndSurName = dr["FirstAndSurName"].ToString();
                    reservistSearchBlock.FamilyName = dr["FamilyName"].ToString();
                    reservistSearchBlock.IdentNumber = dr["PersonIdentNumber"].ToString();
                    reservistSearchBlock.MilRepSpecHTML = dr["MilRepSpecHTML"].ToString();
                    reservistSearchBlock.MilitaryRankName = dr["MilitaryRankName"].ToString();
                    reservistSearchBlock.Education = dr["Education"].ToString();
                    reservistSearchBlock.Age = DBCommon.IsInt(dr["Age"]) ? DBCommon.GetInt(dr["Age"]) : 0;
                    reservistSearchBlock.RegionMunicipalityAndCity = dr["RegionName"].ToString() + ", " + dr["MunicipalityName"].ToString() + ", " + dr["CityName"].ToString();
                    reservistSearchBlock.Languages = dr["Languages"].ToString();
                    reservistSearchBlock.Specialities = dr["Specialities"].ToString();
                    reservistSearchBlock.MilitaryTraining = DBCommon.IsInt(dr["MilitaryTraining"]) ? (int?)DBCommon.GetInt(dr["MilitaryTraining"]) : null;
                    reservistSearchBlock.PositionTitle = dr["PositionTitle"].ToString();
                    

                    reservistSearchBlocks.Add(reservistSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistSearchBlocks;
        }

        public static int GetAllReservistSearchBlocksCount(ReservistSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int reservistSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_VS_LS = false;
                bool join_ReservistMilRepStatuses = false;
                bool join_MilitaryReportStatuses = false;
                bool join_Persons = false;
                bool join_KLV_ZVA = false;
                bool join_KLV_OBR = false;
                bool join_KL_NMA = false;
                bool join_KL_OBS = false;
                bool join_KL_OBL = false;
                bool join_Companies = false;
                bool join_NKPD = false;

                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.IdentNumber))
                {
                    if (filter.IdentNumber.Length == 10)
                        where += (where == "" ? "" : " AND ") +
                             " b.EGN = '" + filter.IdentNumber.Replace("'", "''") + "' ";
                    else
                        where += (where == "" ? "" : " AND ") +
                                 " b.EGN LIKE '" + filter.IdentNumber.Replace("'", "''") + "%' ";

                    join_VS_LS = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.FirstAndSurName))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " Lower(b.IME) LIKE '%" + filter.FirstAndSurName.Replace("'", "''").ToLower() + "%' ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.FamilyName))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " Lower(b.FAM) LIKE '%" + filter.FamilyName.Replace("'", "''").ToLower() + "%' ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Initials))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " c.Initials LIKE '%" + filter.Initials.Replace("'", "''") + "%' ";

                        join_Persons = true;
                    }

                    if (!string.IsNullOrEmpty(filter.MobilAppointmentPosition))
                    {
                        where += (where == "" ? "" : " AND ") +
                                @" a.ReservistID IN (SELECT a.ReservistID 
                                                  FROM PMIS_RES.ReservistAppointments a
                                                  WHERE a.Position LIKE '%" + filter.MobilAppointmentPosition.Replace("'", "''") + "%') ";
                    }

                    string isPrimaryMilRepSpecFilter = "";

                    if (filter.IsPrimaryMilRepSpec)
                    {
                        isPrimaryMilRepSpecFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                    }

                    if (!string.IsNullOrEmpty(filter.MilRepSpecType))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a
                                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                                                WHERE  b.Type IN (" + filter.MilRepSpecType + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.IsSuitableForMobAppointment))
                    {
                        where += (where == "" ? "" : " AND ") +
                        @"NVL(c.IsSuitableForMobAppointment, 0) = " + ((filter.IsSuitableForMobAppointment == ListItems.GetOptionNo().Value) ? "0" : "1");
                        join_Persons = true;
                    }

                    if (!string.IsNullOrEmpty(filter.MilRepSpec))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a                                                
                                                WHERE  a.MilReportSpecialityID IN (" + filter.MilRepSpec + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                    }

                    string isPrimaryPositionTitleFilter = "";

                    if (filter.IsPrimaryPositionTitle)
                    {
                        isPrimaryPositionTitleFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                    }

                    if (!string.IsNullOrEmpty(filter.PositionTitle))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonPositionTitles a                                                
                                                WHERE  a.PositionTitleID IN (" + filter.PositionTitle + ") " + isPrimaryPositionTitleFilter + @" ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.MilitaryRank))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_ZVA IN ( " + filter.MilitaryRank + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.CivilSpeciality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.EGN IN ( SELECT a.OBRG_EGNLS 
                                           FROM VS_OWNER.VS_OBRG a                                           
                                           WHERE  a.OBRG_SPEKOD IN (" + filter.CivilSpeciality + ") ) ";

                        join_VS_LS = true;
                    }

                    if (filter.Age.HasValue)
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) <= " + filter.Age.Value + " ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Language))
                    {
                        where += (where == "" ? "" : " AND ") +
                                @" b.EGN IN ( SELECT a.EZIK_EGNLS 
                                           FROM VS_OWNER.VS_EZIK a                                           
                                           WHERE  a.EZIK_EZKKOD IN ('" + filter.Language.Replace("'", "''").Replace(",", "','") + "') ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( " + filter.City + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.WorkPosition))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " (nkpd.NKPDCode LIKE '%" + filter.WorkPosition.Replace("'", "''") + "%' OR UPPER(nkpd.NKPDName) LIKE UPPER('%" + filter.WorkPosition.Replace("'", "''") + "%')) ";

                        join_NKPD = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Education))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " c.EducationCode = '" + filter.Education + "' ";

                        join_Persons = true;
                    }

                    if (!string.IsNullOrEmpty(filter.MilitaryTraining))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " c.MilitaryTraining = " + filter.MilitaryTraining + " ";

                        join_Persons = true;
                    }

                    if (!string.IsNullOrEmpty(filter.WorkUnifiedIdentityCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" UPPER(wc.UnifiedIdentityCode) LIKE UPPER('%" + filter.WorkUnifiedIdentityCode.Replace("'", "''") + "%') ";

                        join_Companies = true;
                    }

                    if (!string.IsNullOrEmpty(filter.WorkCompanyName))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" UPPER(wc.CompanyName) LIKE UPPER('%" + filter.WorkCompanyName.Replace("'", "''") + "%') ";

                        join_Companies = true;
                    }
                }

                where += (where == "" ? "" : " AND ") +
                             @" a.ReservistID NOT IN ( SELECT ReservistID 
                                                       FROM PMIS_RES.FillReservistsRequest 
                                                       WHERE RequestCommandPositionID = " + requestCommandPositionID + @" AND
                                                             MilitaryDepartmentID = " + militaryDepartmentID + " ) ";

                join_MilitaryReportStatuses = true;

                if (join_MilitaryReportStatuses)
                {
                    join_ReservistMilRepStatuses = true;
                }

                if (join_KLV_ZVA)
                {
                    join_VS_LS = true;
                }

                if (join_KLV_OBR)
                {
                    join_Persons = true;
                }

                if (join_KL_NMA)
                {
                    join_VS_LS = true;
                }

                if (join_KL_OBS)
                {
                    join_VS_LS = true;
                    join_KL_NMA = true;
                }

                if (join_KL_OBL)
                {
                    join_VS_LS = true;
                    join_KL_NMA = true;
                }

                if (join_Companies)
                {
                    join_Persons = true;
                }

                if (join_NKPD)
                {
                    join_Persons = true;
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"      SELECT COUNT(*) as Cnt
                                     FROM PMIS_RES.Reservists a                                                                
                  " + (join_VS_LS ? "INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID" : "") + @"
" + (join_ReservistMilRepStatuses ? "INNER JOIN PMIS_RES.ReservistMilRepStatuses rs ON a.ReservistID = rs.ReservistID AND rs.IsCurrent = 1 AND rs.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"" : "") + @"
" + (join_MilitaryReportStatuses ? @"INNER JOIN PMIS_RES.MilitaryReportStatuses s ON rs.MilitaryReportStatusID = s.MilitaryReportStatusID AND 
                                                                                 s.MilitaryReportStatusKey IN (" + ReservistUtil.SearchResMilRepStatuses() + @")" : "") + @"
                " + (join_Persons ? "LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID" : "") + @"
                " + (join_KLV_ZVA ? "LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON b.KOD_ZVA = d.ZVA_KOD" : "") + @"
                " + (join_KLV_OBR ? "LEFT OUTER JOIN VS_OWNER.KLV_OBR e ON c.EducationCode = e.OBR_KOD" : "") + @"
                 " + (join_KL_NMA ? "LEFT OUTER JOIN UKAZ_OWNER.KL_NMA f ON b.KOD_NMA_MJ = f.KOD_NMA" : "") + @"
                 " + (join_KL_OBS ? "LEFT OUTER JOIN UKAZ_OWNER.KL_OBS g ON f.KOD_OBS = g.KOD_OBS" : "") + @"
                 " + (join_KL_OBL ? "LEFT OUTER JOIN UKAZ_OWNER.KL_OBL h ON f.KOD_OBL = h.KOD_OBL" : "") + @"
              " + (join_Companies ? "LEFT OUTER JOIN PMIS_ADM.Companies wc ON c.WorkCompanyID = wc.CompanyID" : "") + @"
                   " + (join_NKPD ? "LEFT OUTER JOIN PMIS_ADM.NKPD nkpd ON c.WorkPositionNKPDID = nkpd.NKPDID" : "") + @"
                                  " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        reservistSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistSearchBlocksCnt;
        }

        public static List<ReservistManageBlock> GetAllReservistManageBlocks(ReservistManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<ReservistManageBlock> reservistManageBlocks = new List<ReservistManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_VS_LS = false;
                bool join_Persons = false;
                bool join_KLV_ZVA = false;
                bool join_KLV_KAT = false;
                bool join_KLV_OBR = false;
                bool join_KL_NMA = false;
                bool join_KL_OBS = false;
                bool join_KL_OBL = false;
                bool join_ReservistMilRepStatuses = false;
                bool join_MilitaryDepartments = false;
                bool join_MilitaryReportStatuses = false;
                bool join_ReservistAppointments = false;
                bool join_MilitaryReportSpecialities = false;
                bool join_Companies = false;
                bool join_MRS = false;
                bool join_PositionTitle = false;
                bool join_FillReservistsRequest = false;

                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.FirstAndSurName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Lower(b.IME) LIKE '%" + filter.FirstAndSurName.Replace("'", "''").ToLower() + "%' ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.FamilyName))
                {
                    where += (where == "" ? "" : " AND ") +
                             "Lower(b.FAM) LIKE '%" + filter.FamilyName.Replace("'", "''").ToLower() + "%' ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.Initials))
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.Initials LIKE '%" + filter.Initials.Replace("'", "''") + "%' ";

                    join_Persons = true;
                }

                if (!string.IsNullOrEmpty(filter.IdentNumber))
                {
                    if (filter.IdentNumber.Length == 10)
                        where += (where == "" ? "" : " AND ") +
                             " b.EGN = '" + filter.IdentNumber.Replace("'", "''") + "' ";
                    else
                        where += (where == "" ? "" : " AND ") +
                                 " b.EGN LIKE '" + filter.IdentNumber.Replace("'", "''") + "%' ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryCategory))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" d.ZVA_KAT_KOD IN ( " + filter.MilitaryCategory + ") ";

                    join_KLV_ZVA = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryRank))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.KOD_ZVA IN ( " + filter.MilitaryRank + ") ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryReportStatus))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.MilitaryReportStatusID IN ( " + filter.MilitaryReportStatus + ") ";

                    join_ReservistMilRepStatuses = true;
                }
                else
                {
                    // Ако е избран за статус Всички, да взема всички без Изключени
                    MilitaryReportStatus removed = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("REMOVED", currentUser);

                    where += (where == "" ? "" : " AND ") +
                             @" NVL(i.MilitaryReportStatusID,0) <> " + removed.MilitaryReportStatusId.ToString() + " ";

                    join_ReservistMilRepStatuses = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryCommand))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.MilitaryCommandName LIKE '%" + filter.MilitaryCommand.Replace("'", "''") + "%' ";

                    join_ReservistAppointments = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";

                    join_ReservistMilRepStatuses = true;
                }

                if (!string.IsNullOrEmpty(filter.Position))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.Position LIKE '%" + filter.Position.Replace("'", "''") + "%' ";

                    join_ReservistAppointments = true;
                }

                if (!string.IsNullOrEmpty(filter.MilAppointedRepSpecType))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" m.Type IN (" + filter.MilAppointedRepSpecType + ") ";

                    join_MilitaryReportSpecialities = true;
                }

                if (!string.IsNullOrEmpty(filter.MilAppointedRepSpec))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.MilReportSpecialityID IN (" + filter.MilAppointedRepSpec + ") ";

                    join_ReservistAppointments = true;
                }

                string isPrimaryMilRepSpecFilter = "";

                if (filter.IsPrimaryMilRepSpec)
                {
                    isPrimaryMilRepSpecFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpecType))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a
                                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                                                WHERE  b.Type IN (" + filter.MilRepSpecType + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpec))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a                                                
                                                WHERE  a.MilReportSpecialityID IN (" + filter.MilRepSpec + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                string isPrimaryPositionTitleFilter = "";

                if (filter.IsPrimaryPositionTitle)
                {
                    isPrimaryPositionTitleFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                }

                if (!string.IsNullOrEmpty(filter.PositionTitle))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonPositionTitles a                                                
                                                WHERE  a.PositionTitleID IN (" + filter.PositionTitle + ") " + isPrimaryPositionTitleFilter + @" ) ";
                }

                if (!string.IsNullOrEmpty(filter.Administration))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" c.AdministrationID IN ( " + filter.Administration + ") ";

                    join_Persons = true;
                }

                if (!string.IsNullOrEmpty(filter.Language))
                {
                    where += (where == "" ? "" : " AND ") +
                            @" b.EGN IN ( SELECT a.EZIK_EGNLS 
                                           FROM VS_OWNER.VS_EZIK a                                           
                                           WHERE  a.EZIK_EZKKOD IN ('" + filter.Language.Replace("'", "''").Replace(",", "','") + "') ) ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.Education))
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.EducationCode = '" + filter.Education + "' ";

                    join_Persons = true;
                }                

                if (!string.IsNullOrEmpty(filter.CivilSpeciality))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.EGN IN ( SELECT a.OBRG_EGNLS 
                                           FROM VS_OWNER.VS_OBRG a                                           
                                           WHERE  a.OBRG_SPEKOD IN (" + filter.CivilSpeciality + ") ) ";

                    join_VS_LS = true;
                }
               
                if (filter.IsPermAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( " + filter.City + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.PermAddrDistrictID IN ( " + filter.District + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ADRES) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.PermSecondPostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";

                        join_VS_LS = true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.CurrAddrCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.CurrAddrCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.CurrAddrCityID IN ( " + filter.City + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.CurrAddrDistrictID IN ( " + filter.District + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.CurrAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.PresSecondPostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";

                        join_VS_LS = true;
                    }
                }

                if (!string.IsNullOrEmpty(filter.WorkUnifiedIdentityCode))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" UPPER(wc.UnifiedIdentityCode) LIKE UPPER('%" + filter.WorkUnifiedIdentityCode.Replace("'", "''") + "%') ";

                    join_Companies = true;
                }

                if (!string.IsNullOrEmpty(filter.WorkCompanyName))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" UPPER(wc.CompanyName) LIKE UPPER('%" + filter.WorkCompanyName.Replace("'", "''") + "%') ";

                    join_Companies = true;
                }

                if (filter.HasBeenOnMission)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.EGN IN ( SELECT a.ARDLO_EGNLS 
                                           FROM VS_OWNER.VS_AR_DLO a                                           
                                           WHERE  NVL(a.ARDLO_MISIA, '0') = '1' OR NVL(a.ARDLO_MISIA, '0') = 'Y') ";

                    join_VS_LS = true;
                }

                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IS NULL OR i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs_ListOfValues + ")) ";

                join_ReservistMilRepStatuses = true;


                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.AppointmentIsDelivered))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (fr.ReservistID IS NULL OR (fr.ReservistID IS NOT NULL AND NVL(fr.AppointmentIsDelivered, 0) = " + (filter.AppointmentIsDelivered == ListItems.GetOptionYes().Value ? "1" : "0") + ")) ";

                    join_FillReservistsRequest = true;
                }

                if (!string.IsNullOrEmpty(filter.IsSuitableForMobAppointment))
                {
                    where += (where == "" ? "" : " AND ") +
                        @"NVL(c.IsSuitableForMobAppointment, 0) = " + ((filter.IsSuitableForMobAppointment == ListItems.GetOptionNo().Value) ? "0" : "1");

                    join_Persons = true;
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.Readiness))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (fr.ReservistID IS NULL OR (fr.ReservistID IS NOT NULL AND fr.ReservistReadinessID = " + int.Parse(filter.Readiness).ToString() + ")) ";

                    join_FillReservistsRequest = true;
                }
                               
                if (!string.IsNullOrEmpty(filter.ProfessionId))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" a.PersonID IN (SELECT PersonID FROM PMIS_ADM.PersonSpecialities WHERE ProfessionID = " + filter.ProfessionId + @")
                            ";
                }

                if (!string.IsNullOrEmpty(filter.SpecialityId))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" a.PersonID IN (SELECT PersonID FROM PMIS_ADM.PersonSpecialities WHERE SpecialityID = " + filter.SpecialityId + @")
                            ";
                }
              
                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filter.OrderBy > 100)
                {
                    filter.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filter.OrderBy)
                {
                    case 1:
                        orderBySQL = "b.IME";
                        join_VS_LS = true;
                        break;
                    case 2:
                        orderBySQL = "b.EGN";
                        join_VS_LS = true;
                        break;
                    case 3:
                        orderBySQL = "d.ZVA_IME";
                        join_KLV_ZVA = true;
                        break;
                    case 4:
                        orderBySQL = "h.IME_OBL || ' ' || g.IME_OBS || ' ' || f.IME_NMA";
                        join_KL_NMA = true;
                        join_KL_OBS = true;
                        join_KL_OBL = true;
                        break;
                    case 5:
                        orderBySQL = "j.MilitaryDepartmentName";
                        join_MilitaryDepartments = true;
                        break;
                    case 6:
                        orderBySQL = "d2.KAT_IME";
                        join_KLV_KAT = true;
                        break;
                    case 7:
                        orderBySQL = "k.MilitaryReportStatusName";
                        join_MilitaryReportStatuses = true;
                        break;
                    case 8:
                        orderBySQL = "l.MilitaryCommandName";
                        join_ReservistAppointments = true;
                        break;
                    case 9:
                        orderBySQL = "mrs.MilReportingSpecialityCode";
                        join_MRS = true;
                        break;
                    case 10:
                        orderBySQL = "b.FAM";
                        join_VS_LS = true;
                        break;
                    case 11:
                        orderBySQL = "pt.PositionTitle";
                        join_PositionTitle = true;
                        break;
                    default:
                        orderBySQL = "b.EGN";
                        join_VS_LS = true;
                        break;
                }

                if (join_KLV_ZVA)
                {
                    join_VS_LS = true;
                }

                if (join_KLV_KAT)
                {
                    join_VS_LS = true;
                    join_KLV_ZVA = true;
                }

                if (join_KLV_OBR)
                {
                    join_Persons = true;
                }

                if (join_KL_NMA)
                {
                    join_VS_LS = true;
                }

                if (join_KL_OBS)
                {
                    join_VS_LS = true;
                    join_KL_NMA = true;
                }

                if (join_KL_OBL)
                {
                    join_VS_LS = true;
                    join_KL_NMA = true;
                }

                if (join_MilitaryDepartments)
                {
                    join_ReservistMilRepStatuses = true;
                }

                if (join_MilitaryReportStatuses)
                {
                    join_ReservistMilRepStatuses = true;
                }

                if (join_MilitaryReportSpecialities)
                {
                    join_ReservistAppointments = true;
                }

                if (join_Companies)
                {
                    join_Persons = true;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.*,
                                      a.PersonID, a.GroupManagementSection, a.Section, a.Deliverer, a.PunktID,
                                      b.IME as FirstAndSurName, 
                                      b.FAM as FamilyName,
                                      b.EGN as PersonIdentNumber,                                       
                                      d.ZVA_IME as MilitaryRankName,
                                      j.MilitaryDepartmentName,
                                      d2.KAT_IME as MilitaryCategory,
                                      k.MilitaryReportStatusName,
                                      f.IME_NMA as CityName,
                                      g.IME_OBS as MunicipalityName,
                                      h.IME_OBL as RegionName,
                                      l.MilitaryCommandName as MilitaryCommandName,
                                      mrs.MilReportingSpecialityCode,
                                      pt.PositionTitle
                                FROM (   SELECT a.ReservistID, 
                                                RANK() OVER (ORDER BY " + orderBySQL + @", a.ReservistID) as RowNumber 
                                         FROM PMIS_RES.Reservists a                                                                
                      " + (join_VS_LS ? "INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID" : "") + @"
                    " + (join_Persons ? "LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID" : "") + @"
                    " + (join_KLV_ZVA ? "LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON b.KOD_ZVA = d.ZVA_KOD" : "") + @"
                    " + (join_KLV_KAT ? "LEFT OUTER JOIN VS_OWNER.KLV_KAT d2 ON d.ZVA_KAT_KOD = d2.KAT_KOD" : "") + @"
                    " + (join_KLV_OBR ? "LEFT OUTER JOIN VS_OWNER.KLV_OBR e ON c.EducationCode = e.OBR_KOD" : "") + @"
                     " + (join_KL_NMA ? "LEFT OUTER JOIN UKAZ_OWNER.KL_NMA f ON b.KOD_NMA_MJ = f.KOD_NMA" : "") + @"
                     " + (join_KL_OBS ? "LEFT OUTER JOIN UKAZ_OWNER.KL_OBS g ON f.KOD_OBS = g.KOD_OBS" : "") + @"
                     " + (join_KL_OBL ? "LEFT OUTER JOIN UKAZ_OWNER.KL_OBL h ON f.KOD_OBL = h.KOD_OBL" : "") + @"
    " + (join_ReservistMilRepStatuses ? "LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1 " : "") + @"
        " + (join_MilitaryDepartments ? "LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments j ON i.SourceMilDepartmentID = j.MilitaryDepartmentID" : "") + @"
     " + (join_MilitaryReportStatuses ? "LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses k ON i.MilitaryReportStatusID = k.MilitaryReportStatusID" : "") + @"
      " + (join_ReservistAppointments ? "LEFT OUTER JOIN PMIS_RES.ReservistAppointments l ON a.ReservistID = l.ReservistID AND l.IsCurrent = 1" : "") + @"
 " + (join_MilitaryReportSpecialities ? "LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities m ON l.MilReportSpecialityID = m.MilReportSpecialityID" : "") + @"
                  " + (join_Companies ? "LEFT OUTER JOIN PMIS_ADM.Companies wc ON c.WorkCompanyID = wc.CompanyID" : "") + @"
                        " + (join_MRS ? "LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec pmrs ON pmrs.PersonID = a.PersonID AND pmrs.IsPrimary = 1 LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities mrs ON mrs.MilreportSpecialityID = pmrs.MilreportSpecialityID" : "") + @"
              " + (join_PositionTitle ? "LEFT OUTER JOIN PMIS_ADM.PersonPositionTitles ppt ON ppt.PersonID = a.PersonID AND ppt.IsPrimary = 1 LEFT OUTER JOIN PMIS_ADM.PositionTitles pt ON pt.PositionTitleID = ppt.PositionTitleID" : "") + @"
      " + (join_FillReservistsRequest ? "LEFT OUTER JOIN PMIS_RES.FillReservistsRequest fr ON a.ReservistID = fr.ReservistID" : "") + @"
                                     " + where + @"    
                                     ) tmp
                                INNER JOIN PMIS_RES.Reservists a ON tmp.ReservistID = a.ReservistID
                                INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON b.KOD_ZVA = d.ZVA_KOD
                                LEFT OUTER JOIN VS_OWNER.KLV_KAT d2 ON d.ZVA_KAT_KOD = d2.KAT_KOD
                                LEFT OUTER JOIN UKAZ_OWNER.KL_NMA f ON b.KOD_NMA_MJ = f.KOD_NMA
                                LEFT OUTER JOIN UKAZ_OWNER.KL_OBS g ON f.KOD_OBS = g.KOD_OBS
                                LEFT OUTER JOIN UKAZ_OWNER.KL_OBL h ON f.KOD_OBL = h.KOD_OBL
                                LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments j ON i.SourceMilDepartmentID = j.MilitaryDepartmentID
                                LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses k ON i.MilitaryReportStatusID = k.MilitaryReportStatusID
                                LEFT OUTER JOIN PMIS_RES.ReservistAppointments l ON a.ReservistID = l.ReservistID AND l.IsCurrent = 1
                                LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec pmrs ON pmrs.PersonID = b.PersonID AND pmrs.IsPrimary = 1
                                LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities mrs ON mrs.MilreportSpecialityID = pmrs.MilreportSpecialityID
                                LEFT OUTER JOIN PMIS_ADM.PersonPositionTitles ppt ON ppt.PersonID = b.PersonID AND ppt.IsPrimary = 1
                                LEFT OUTER JOIN PMIS_ADM.PositionTitles pt ON ppt.PositionTitleID = pt.PositionTitleID
                                " + pageWhere + @"
                                ORDER BY RowNumber";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReservistManageBlock reservistManageBlock = new ReservistManageBlock();

                    reservistManageBlock.ReservistID = DBCommon.GetInt(dr["ReservistID"]);
                    reservistManageBlock.FirstAndSurName = dr["FirstAndSurName"].ToString();
                    reservistManageBlock.FamilyName = dr["FamilyName"].ToString();
                    reservistManageBlock.IdentNumber = dr["PersonIdentNumber"].ToString();                   
                    reservistManageBlock.MilitaryRankName = dr["MilitaryRankName"].ToString();
                    reservistManageBlock.RegionMuniciplaityAndCity = dr["RegionName"].ToString() + (string.IsNullOrEmpty(dr["RegionName"].ToString()) ? "" : ", ") + dr["MunicipalityName"].ToString() + (string.IsNullOrEmpty(dr["MunicipalityName"].ToString()) ? "" : ", ") + dr["CityName"].ToString();
                    reservistManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    reservistManageBlock.MilitaryCategory = dr["MilitaryCategory"].ToString();
                    reservistManageBlock.MilitaryReportStatus = dr["MilitaryReportStatusName"].ToString();
                    reservistManageBlock.MilitaryCommand = dr["MilitaryCommandName"].ToString();
                    reservistManageBlock.MilReportingSpecialityCode = dr["MilReportingSpecialityCode"].ToString();
                    reservistManageBlock.PositionTitle = dr["PositionTitle"].ToString(); 
                    

                    reservistManageBlocks.Add(reservistManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistManageBlocks;
        }

        public static int GetAllReservistManageBlocksCount(ReservistManageFilter filter, User currentUser)
        {
            int reservistSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_VS_LS = false;
                bool join_Persons = false;
                bool join_KLV_ZVA = false;
                bool join_KLV_KAT = false;
                bool join_KLV_OBR = false;
                bool join_KL_NMA = false;
                bool join_KL_OBS = false;
                bool join_KL_OBL = false;
                bool join_ReservistMilRepStatuses = false;
                bool join_MilitaryDepartments = false;
                bool join_MilitaryReportStatuses = false;
                bool join_ReservistAppointments = false;
                bool join_MilitaryReportSpecialities = false;
                bool join_Companies = false;
                bool join_FillReservistsRequest = false;
 
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.FirstAndSurName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Lower(b.IME) LIKE '%" + filter.FirstAndSurName.Replace("'", "''").ToLower() + "%' ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.FamilyName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Lower(b.FAM) LIKE '%" + filter.FamilyName.Replace("'", "''").ToLower() + "%' ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.Initials))
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.Initials LIKE '%" + filter.Initials.Replace("'", "''") + "%' ";

                    join_Persons = true;
                }

                if (!string.IsNullOrEmpty(filter.IdentNumber))
                {
                    if (filter.IdentNumber.Length == 10)
                        where += (where == "" ? "" : " AND ") +
                             " b.EGN = '" + filter.IdentNumber.Replace("'", "''") + "' ";
                    else
                        where += (where == "" ? "" : " AND ") +
                                 " b.EGN LIKE '" + filter.IdentNumber.Replace("'", "''") + "%' ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryCategory))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" d.ZVA_KAT_KOD IN ( " + filter.MilitaryCategory + ") ";

                    join_KLV_ZVA = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryRank))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.KOD_ZVA IN ( " + filter.MilitaryRank + ") ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryReportStatus))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.MilitaryReportStatusID IN ( " + filter.MilitaryReportStatus + ") ";

                    join_ReservistMilRepStatuses = true;
                }
                else
                {
                    // Ако е избран за статус Всички, да взема всички без Изключени
                    MilitaryReportStatus removed = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("REMOVED", currentUser);

                    where += (where == "" ? "" : " AND ") +
                             @" NVL(i.MilitaryReportStatusID,0) <> " + removed.MilitaryReportStatusId.ToString() + " ";

                    join_ReservistMilRepStatuses = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryCommand))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.MilitaryCommandName LIKE '%" + filter.MilitaryCommand.Replace("'", "''") + "%' ";

                    join_ReservistAppointments = true;
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";

                    join_ReservistMilRepStatuses = true;
                }

                if (!string.IsNullOrEmpty(filter.Position))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.Position LIKE '%" + filter.Position.Replace("'", "''") + "%' ";

                    join_ReservistAppointments = true;
                }

                if (!string.IsNullOrEmpty(filter.MilAppointedRepSpecType))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" m.Type IN (" + filter.MilAppointedRepSpecType + ") ";

                    join_MilitaryReportSpecialities = true;
                }

                if (!string.IsNullOrEmpty(filter.MilAppointedRepSpec))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.MilReportSpecialityID IN (" + filter.MilAppointedRepSpec + ") ";

                    join_ReservistAppointments = true;
                }

                string isPrimaryMilRepSpecFilter = "";

                if (filter.IsPrimaryMilRepSpec)
                {
                    isPrimaryMilRepSpecFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpecType))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a
                                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                                                WHERE  b.Type IN (" + filter.MilRepSpecType + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpec))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a                                                
                                                WHERE  a.MilReportSpecialityID IN (" + filter.MilRepSpec + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                string isPrimaryPositionTitleFilter = "";

                if (filter.IsPrimaryPositionTitle)
                {
                    isPrimaryPositionTitleFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                }

                if (!string.IsNullOrEmpty(filter.PositionTitle))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonPositionTitles a                                                
                                                WHERE  a.PositionTitleID IN (" + filter.PositionTitle + ") " + isPrimaryPositionTitleFilter + @" ) ";
                }

                if (!string.IsNullOrEmpty(filter.Administration))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" c.AdministrationID IN ( " + filter.Administration + ") ";

                    join_Persons = true;
                }

                if (!string.IsNullOrEmpty(filter.Language))
                {
                    where += (where == "" ? "" : " AND ") +
                            @" b.EGN IN ( SELECT a.EZIK_EGNLS 
                                           FROM VS_OWNER.VS_EZIK a                                           
                                           WHERE  a.EZIK_EZKKOD IN ('" + filter.Language.Replace("'", "''").Replace(",", "','") + "') ) ";

                    join_VS_LS = true;
                }

                if (!string.IsNullOrEmpty(filter.Education))
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.EducationCode = '" + filter.Education + "' ";

                    join_Persons = true;
                }

                if (!string.IsNullOrEmpty(filter.CivilSpeciality))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.EGN IN ( SELECT a.OBRG_EGNLS 
                                           FROM VS_OWNER.VS_OBRG a                                           
                                           WHERE  a.OBRG_SPEKOD IN (" + filter.CivilSpeciality + ") ) ";

                    join_VS_LS = true;
                }

                if (filter.IsPermAddress)
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.KOD_NMA_MJ IN ( " + filter.City + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.PermAddrDistrictID IN ( " + filter.District + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ADRES) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.PermSecondPostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";

                        join_VS_LS = true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.CurrAddrCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.CurrAddrCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.CurrAddrCityID IN ( " + filter.City + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.CurrAddrDistrictID IN ( " + filter.District + ") ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.CurrAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";

                        join_VS_LS = true;
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.PresSecondPostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";

                        join_VS_LS = true;
                    }
                }

                if (!string.IsNullOrEmpty(filter.WorkUnifiedIdentityCode))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" UPPER(wc.UnifiedIdentityCode) LIKE UPPER('%" + filter.WorkUnifiedIdentityCode.Replace("'", "''") + "%') ";

                    join_Companies = true;
                }

                if (!string.IsNullOrEmpty(filter.WorkCompanyName))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" UPPER(wc.CompanyName) LIKE UPPER('%" + filter.WorkCompanyName.Replace("'", "''") + "%') ";

                    join_Companies = true;
                }

                if (filter.HasBeenOnMission)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.EGN IN ( SELECT a.ARDLO_EGNLS 
                                           FROM VS_OWNER.VS_AR_DLO a                                           
                                           WHERE  NVL(a.ARDLO_MISIA, '0') = '1' OR NVL(a.ARDLO_MISIA, '0') = 'Y') ";

                    join_VS_LS = true;
                }

                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IS NULL OR i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs_ListOfValues + ")) ";

                join_ReservistMilRepStatuses = true;


                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.AppointmentIsDelivered))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (fr.ReservistID IS NULL OR (fr.ReservistID IS NOT NULL AND NVL(fr.AppointmentIsDelivered, 0) = " + (filter.AppointmentIsDelivered == ListItems.GetOptionYes().Value ? "1" : "0") + ")) ";

                    join_FillReservistsRequest = true;
                }

                if (!string.IsNullOrEmpty(filter.IsSuitableForMobAppointment))
                {
                    where += (where == "" ? "" : " AND ") +
                       @"NVL(c.IsSuitableForMobAppointment, 0) = " + ((filter.IsSuitableForMobAppointment == ListItems.GetOptionNo().Value) ? "0" : "1");

                    join_Persons = true;
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.Readiness))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (fr.ReservistID IS NULL OR (fr.ReservistID IS NOT NULL AND fr.ReservistReadinessID = " + int.Parse(filter.Readiness).ToString() + "))";

                    join_FillReservistsRequest = true;
                }

                if (!string.IsNullOrEmpty(filter.ProfessionId))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" a.PersonID IN (SELECT PersonID FROM PMIS_ADM.PersonSpecialities WHERE ProfessionID = " + filter.ProfessionId + @")
                            ";
                }

                if (!string.IsNullOrEmpty(filter.SpecialityId))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" a.PersonID IN (SELECT PersonID FROM PMIS_ADM.PersonSpecialities WHERE SpecialityID = " + filter.SpecialityId + @")
                            ";
                }

                if (join_KLV_ZVA)
                {
                    join_VS_LS = true;
                }

                if (join_KLV_KAT)
                {
                    join_VS_LS = true;
                    join_KLV_ZVA = true;
                }

                if (join_KLV_OBR)
                {
                    join_Persons = true;
                }

                if (join_KL_NMA)
                {
                    join_VS_LS = true;
                }

                if (join_KL_OBS)
                {
                    join_VS_LS = true;
                    join_KL_NMA = true;
                }

                if (join_KL_OBL)
                {
                    join_VS_LS = true;
                    join_KL_NMA = true;
                }

                if (join_MilitaryDepartments)
                {
                    join_ReservistMilRepStatuses = true;
                }

                if (join_MilitaryReportStatuses)
                {
                    join_ReservistMilRepStatuses = true;
                }

                if (join_MilitaryReportSpecialities)
                {
                    join_ReservistAppointments = true;
                }

                if (join_Companies)
                {
                    join_Persons = true;
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.Reservists a                                                                
             " + (join_VS_LS ? "INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID" : "") + @"
           " + (join_Persons ? "LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID" : "") + @"
           " + (join_KLV_ZVA ? "LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON b.KOD_ZVA = d.ZVA_KOD" : "") + @"
           " + (join_KLV_KAT ? "LEFT OUTER JOIN VS_OWNER.KLV_KAT d2 ON d.ZVA_KAT_KOD = d2.KAT_KOD" : "") + @"
           " + (join_KLV_OBR ? "LEFT OUTER JOIN VS_OWNER.KLV_OBR e ON c.EducationCode = e.OBR_KOD" : "") + @"
            " + (join_KL_NMA ? "LEFT OUTER JOIN UKAZ_OWNER.KL_NMA f ON b.KOD_NMA_MJ = f.KOD_NMA" : "") + @"
            " + (join_KL_OBS ? "LEFT OUTER JOIN UKAZ_OWNER.KL_OBS g ON f.KOD_OBS = g.KOD_OBS" : "") + @"
            " + (join_KL_OBL ? "LEFT OUTER JOIN UKAZ_OWNER.KL_OBL h ON f.KOD_OBL = h.KOD_OBL" : "") + @"
    " + (join_ReservistMilRepStatuses ? "LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1 " : "") + @"
        " + (join_MilitaryDepartments ? "LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments j ON i.SourceMilDepartmentID = j.MilitaryDepartmentID" : "") + @"
     " + (join_MilitaryReportStatuses ? "LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses k ON i.MilitaryReportStatusID = k.MilitaryReportStatusID" : "") + @"
      " + (join_ReservistAppointments ? "LEFT OUTER JOIN PMIS_RES.ReservistAppointments l ON a.ReservistID = l.ReservistID AND l.IsCurrent = 1" : "") + @"
 " + (join_MilitaryReportSpecialities ? "LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities m ON l.MilReportSpecialityID = m.MilReportSpecialityID" : "") + @"
                  " + (join_Companies ? "LEFT OUTER JOIN PMIS_ADM.Companies wc ON c.WorkCompanyID = wc.CompanyID" : "") + @"
      " + (join_FillReservistsRequest ? "LEFT OUTER JOIN PMIS_RES.FillReservistsRequest fr ON a.ReservistID = fr.ReservistID" : "") + @"
                            " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        reservistSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistSearchBlocksCnt;
        }

        public static List<ReservistGroupTakingDownBlock> GetAllReservistGroupTakingDownBlocks(ReservistGroupTakingDownFilter filter, int rowsPerPage, User currentUser)
        {
            List<ReservistGroupTakingDownBlock> reservistGroupTakingDownBlocks = new List<ReservistGroupTakingDownBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                MilitaryReportStatus RemovedStatus = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("REMOVED", currentUser);

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.Gender))
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.GenderID IN ( " + filter.Gender + ") ";
                }

                if (filter.Age.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGNbyDate(b.EGN, :ToDate) = " + filter.Age.Value + " ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryCategory))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" d1.ZVA_KAT_KOD IN ( " + filter.MilitaryCategory + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryRank))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.KOD_ZVA IN ( " + filter.MilitaryRank + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Administration))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" c.AdministrationID IN ( " + filter.Administration + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";
                }

                string isPrimaryMilRepSpecFilter = "";

                if (filter.IsPrimaryMilRepSpec)
                {
                    isPrimaryMilRepSpecFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpecType))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a
                                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                                                WHERE  b.Type IN (" + filter.MilRepSpecType + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpec))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a                                                
                                                WHERE  a.MilReportSpecialityID IN (" + filter.MilRepSpec + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (filter.OnlyMobileAppointed)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.ReservistAppointmentID IS NOT NULL ";   
                }

                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";
               
                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filter.OrderBy > 100)
                {
                    filter.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filter.OrderBy)
                {
                    case 1:
                        orderBySQL = "b.IME || ' ' || b.FAM";
                        break;
                    case 2:
                        orderBySQL = "b.EGN";
                        break;
                    case 3:
                        orderBySQL = "d.GenderName";
                        break;
                    case 4:
                        orderBySQL = "e.AdministrationName";
                        break;
                    case 5:
                        orderBySQL = "k.MilitaryReportStatusName";
                        break;
                    case 6:
                        orderBySQL = "l.MilitaryCommandName";
                        break;
                    default:
                        orderBySQL = "b.EGN";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.ReservistID, 
                                       a.PersonID, 
                                       b.IME || ' ' || b.FAM as PersonName, 
                                       b.EGN as PersonIdentNumber,            
                                       d.GenderName as Gender,              
                                       e.AdministrationName as Administration,                                                    
                                       k.MilitaryReportStatusName,
                                       l.MilitaryCommandName as MilitaryCommandName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.ReservistID) as RowNumber 
                                FROM PMIS_RES.Reservists a                                                                
                                INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                LEFT OUTER JOIN VS_OWNER.KLV_ZVA d1 ON b.KOD_ZVA = d1.ZVA_KOD
                                LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                                LEFT OUTER JOIN PMIS_ADM.Gender d ON c.GenderID = d.GenderID
                                LEFT OUTER JOIN PMIS_ADM.Administrations e ON e.AdministrationID = c.AdministrationID
                                INNER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1 AND i.MilitaryReportStatusID <> " + RemovedStatus.MilitaryReportStatusId.ToString() + @"
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments j ON i.SourceMilDepartmentID = j.MilitaryDepartmentID   
                                LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses k ON i.MilitaryReportStatusID = k.MilitaryReportStatusID                          
                                LEFT OUTER JOIN PMIS_RES.ReservistAppointments l ON a.ReservistID = l.ReservistID AND l.IsCurrent = 1     
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", a.ReservistID
                               ) tmp
                               " + pageWhere;
                OracleCommand cmd = new OracleCommand(SQL, conn);
                if (filter.Age.HasValue)
                {
                    if (CommonFunctions.TryParseDate(filter.ToDate))
                    {
                        cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = CommonFunctions.ParseDate(filter.ToDate);
                    }
                    else
                    {
                        cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = DateTime.Now;
                    }
                }
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReservistGroupTakingDownBlock reservistGroupTakingDownBlock = new ReservistGroupTakingDownBlock();

                    reservistGroupTakingDownBlock.ReservistID = DBCommon.GetInt(dr["ReservistID"]);
                    reservistGroupTakingDownBlock.FullName = dr["PersonName"].ToString();
                    reservistGroupTakingDownBlock.IdentNumber = dr["PersonIdentNumber"].ToString();
                    reservistGroupTakingDownBlock.Gender = dr["Gender"].ToString();
                    reservistGroupTakingDownBlock.Administration = dr["Administration"].ToString();
                    reservistGroupTakingDownBlock.MilitaryReportStatus = dr["MilitaryReportStatusName"].ToString();
                    reservistGroupTakingDownBlock.MilitaryCommand = dr["MilitaryCommandName"].ToString(); ;

                    reservistGroupTakingDownBlocks.Add(reservistGroupTakingDownBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistGroupTakingDownBlocks;
        }

        public static int GetAllReservistGroupTakingDownBlocksCount(ReservistGroupTakingDownFilter filter, User currentUser)
        {
            int Cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                MilitaryReportStatus RemovedStatus = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("REMOVED", currentUser);

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.Gender))
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.GenderID IN ( " + filter.Gender + ") ";
                }

                if (filter.Age.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGNByDate(b.EGN, :ToDate) = " + filter.Age.Value + " ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryCategory))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" d1.ZVA_KAT_KOD IN ( " + filter.MilitaryCategory + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryRank))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.KOD_ZVA IN ( " + filter.MilitaryRank + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Administration))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" c.AdministrationID IN ( " + filter.Administration + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";
                }

                string isPrimaryMilRepSpecFilter = "";

                if (filter.IsPrimaryMilRepSpec)
                {
                    isPrimaryMilRepSpecFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpecType))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a
                                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                                                WHERE  b.Type IN (" + filter.MilRepSpecType + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpec))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a                                                
                                                WHERE  a.MilReportSpecialityID IN (" + filter.MilRepSpec + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (filter.OnlyMobileAppointed)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.ReservistAppointmentID IS NOT NULL ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.Reservists a                                                                
                                INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                LEFT OUTER JOIN VS_OWNER.KLV_ZVA d1 ON b.KOD_ZVA = d1.ZVA_KOD
                                LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                                LEFT OUTER JOIN PMIS_ADM.Gender d ON c.GenderID = d.GenderID
                                LEFT OUTER JOIN PMIS_ADM.Administrations e ON e.AdministrationID = c.AdministrationID
                                INNER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1 AND i.MilitaryReportStatusID <> " + RemovedStatus.MilitaryReportStatusId.ToString() + @" 
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments j ON i.SourceMilDepartmentID = j.MilitaryDepartmentID   
                                LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses k ON i.MilitaryReportStatusID = k.MilitaryReportStatusID                          
                                LEFT OUTER JOIN PMIS_RES.ReservistAppointments l ON a.ReservistID = l.ReservistID AND l.IsCurrent = 1     
                                 " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);
                if (filter.Age.HasValue)
                {
                    if (CommonFunctions.TryParseDate(filter.ToDate))
                    {
                        cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = CommonFunctions.ParseDate(filter.ToDate);
                    }
                    else
                    {
                        cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = DateTime.Now;
                    }
                }

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        Cnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return Cnt;
        }

        public static bool CheckMobApptMilRepStatus(ReservistGroupTakingDownFilter filter, User currentUser)
        {
            int Cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.Gender))
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.GenderID IN ( " + filter.Gender + ") ";
                }

                if (filter.Age.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGNbyDate(b.EGN, :ToDate) = " + filter.Age.Value + " ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryCategory))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" d1.ZVA_KAT_KOD IN ( " + filter.MilitaryCategory + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryRank))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.KOD_ZVA IN ( " + filter.MilitaryRank + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Administration))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" c.AdministrationID IN ( " + filter.Administration + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";
                }

                string isPrimaryMilRepSpecFilter = "";

                if (filter.IsPrimaryMilRepSpec)
                {
                    isPrimaryMilRepSpecFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpecType))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a
                                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                                                WHERE  b.Type IN (" + filter.MilRepSpecType + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpec))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a                                                
                                                WHERE  a.MilReportSpecialityID IN (" + filter.MilRepSpec + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (filter.OnlyMobileAppointed)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" l.ReservistAppointmentID IS NOT NULL ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                where += (where == "" ? "" : " AND ") +
                             @" i.MilitaryReportStatusID = (SELECT MilitaryReportStatusID FROM PMIS_RES.MilitaryReportStatuses WHERE MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT') ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.Reservists a                                                                
                                INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                LEFT OUTER JOIN VS_OWNER.KLV_ZVA d1 ON b.KOD_ZVA = d1.ZVA_KOD
                                LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                                LEFT OUTER JOIN PMIS_ADM.Gender d ON c.GenderID = d.GenderID
                                LEFT OUTER JOIN PMIS_ADM.Administrations e ON e.AdministrationID = c.AdministrationID
                                LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1  
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments j ON i.SourceMilDepartmentID = j.MilitaryDepartmentID   
                                LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses k ON i.MilitaryReportStatusID = k.MilitaryReportStatusID                          
                                LEFT OUTER JOIN PMIS_RES.ReservistAppointments l ON a.ReservistID = l.ReservistID AND l.IsCurrent = 1     
                                  " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);
                if (filter.Age.HasValue)
                {
                    if (CommonFunctions.TryParseDate(filter.ToDate))
                    {
                        cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = CommonFunctions.ParseDate(filter.ToDate);
                    }
                    else
                    {
                        cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = DateTime.Now;
                    }
                }
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        Cnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return Cnt == 0;
        }

        public static void SetGroupDischargedMilRepStatus(ReservistGroupTakingDownFilter filter, string TakeDownDate, string OrderNumber, string OrderDate, string OrderSignedBy, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                MilitaryReportStatus RemovedStatus = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("REMOVED", currentUser);

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_HUMANRES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.Gender))
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.GenderID IN ( " + filter.Gender + ") ";
                }

                if (filter.Age.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             @" PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGNbyDate(b.EGN, :ToDate) = " + filter.Age.Value + " ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryCategory))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" d1.ZVA_KAT_KOD IN ( " + filter.MilitaryCategory + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryRank))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.KOD_ZVA IN ( " + filter.MilitaryRank + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Administration))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" c.AdministrationID IN ( " + filter.Administration + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";
                }

                string isPrimaryMilRepSpecFilter = "";

                if (filter.IsPrimaryMilRepSpec)
                {
                    isPrimaryMilRepSpecFilter = " AND NVL(a.IsPrimary, 0) = 1 ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpecType))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a
                                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                                                WHERE  b.Type IN (" + filter.MilRepSpecType + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                if (!string.IsNullOrEmpty(filter.MilRepSpec))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.PersonID IN ( SELECT a.PersonID 
                                                FROM PMIS_ADM.PersonMilRepSpec a                                                
                                                WHERE  a.MilReportSpecialityID IN (" + filter.MilRepSpec + ") " + isPrimaryMilRepSpecFilter + @" ) ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                
                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"DECLARE
                                  DischaredStatusID number;
                                  RemovedReasonID number;
                                  ChangeID PMIS_ADM.ChangesLog.ChangeID%TYPE;
                                  ChangeEventTypeID PMIS_ADM.ChangeEvents.ChangeEventTypeID%TYPE;
                               BEGIN
                            
                               SELECT MilitaryReportStatusID INTO DischaredStatusID FROM PMIS_RES.MilitaryReportStatuses WHERE MilitaryReportStatusKey = 'REMOVED';
                               SELECT TableKey INTO RemovedReasonID FROM PMIS_RES.GTable WHERE TableName = 'MilRepStat_RemovedReasons' AND TableValue = 'Пределна възраст';



                               INSERT INTO PMIS_ADM.ChangesLog (UserID, ChangeDate, ChangeTypeID, LoginLogID)
                               VALUES (" + currentUser.UserId.ToString() + @", 
                                        SYSDATE,                                      
                                       (SELECT ChangeTypeID FROM PMIS_ADM.ChangeTypes WHERE ChangeTypeKey = 'RES_Reservists'), 
                                       " + currentUser.LoginLogId.ToString() + @");

                               SELECT PMIS_ADM.CHANGESLOG_ID_SEQ.currval INTO ChangeID FROM dual;

                               SELECT ChangeEventTypeID INTO ChangeEventTypeID FROM PMIS_ADM.ChangeEventTypes WHERE ChangeEventTypeKey = 'RES_Reservist_GroupTakeDown';

                               INSERT INTO PMIS_ADM.ChangeEvents (ChangeID, ChangeEventTypeID, ObjectDesc, MilitaryUnitID, PersonID)
                               SELECT ChangeID, ChangeEventTypeID, 'ВО: ' || j.MilitaryDepartmentName, NULL, a.PersonID
                               FROM PMIS_RES.Reservists a                                                                
                               INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA d1 ON b.KOD_ZVA = d1.ZVA_KOD
                               LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                               INNER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1 AND i.MilitaryReportStatusID <> " + RemovedStatus.MilitaryReportStatusId.ToString() + @"
                               LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments j ON i.SourceMilDepartmentID = j.MilitaryDepartmentID   
                               " + where + @"; 
                               


                               /*UPDATE VS_OWNER.VS_LS SET KOD_KZV = 'О', KOD_PSO = 'C', V_PODELENIE = NULL, V_KOMANDA = NULL 
                               WHERE PersonID IN (SELECT PersonID 
                                                  FROM PMIS_RES.Reservists a                                                                
                                                  INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                                  LEFT OUTER JOIN VS_OWNER.KLV_ZVA d1 ON b.KOD_ZVA = d1.ZVA_KOD
                                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                                                  INNER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1 AND i.MilitaryReportStatusID <> " + RemovedStatus.MilitaryReportStatusId.ToString() + @"
                                                   " + where + @"); */


                               INSERT INTO PMIS_RES.ReservistMilRepStatuses (ReservistID, 
                                                                             IsCurrent, 
                                                                             MilitaryReportStatusID, 
                                                                             Removed_Date,
                                                                             Removed_ReasonID,
                                                                             Removed_AgeLimit_Order,
                                                                             Removed_AgeLimit_Date,
                                                                             Removed_AgeLimit_SignedBy, 
                                                                             SourceMilDepartmentID, 
                                                                             CreatedBy, 
                                                                             CreatedDate, 
                                                                             LastModifiedBy, 
                                                                             LastModifiedDate)
                               SELECT a.ReservistID, 
                                      NULL, 
                                      DischaredStatusID, 
                                      :TakeDownDate, 
                                      RemovedReasonID,
                                      :OrderNumber,
                                      :OrderDate,
                                      :OrderSignedBy,
                                      a.SourceMilDepartmentID, 
                                      :CreatedBy,
                                      :CreatedDate,
                                      :LastModifiedBy,
                                      :LastModifiedDate 
                                FROM PMIS_RES.ReservistMilRepStatuses a
                                WHERE IsCurrent = 1 AND a.ReservistID IN (SELECT ReservistID 
                                                                          FROM PMIS_RES.Reservists a                                                                
                                                                          INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                                                          LEFT OUTER JOIN VS_OWNER.KLV_ZVA d1 ON b.KOD_ZVA = d1.ZVA_KOD
                                                                          LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                                                                          INNER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1 AND i.MilitaryReportStatusID <> " + RemovedStatus.MilitaryReportStatusId.ToString() + @"
                                                                          " + where + @");


                               UPDATE PMIS_RES.ReservistMilRepStatuses SET IsCurrent = 0
                               WHERE IsCurrent = 1 AND ReservistID IN (SELECT ReservistID 
                                                                       FROM PMIS_RES.Reservists a                                                                
                                                                       INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                                                       LEFT OUTER JOIN VS_OWNER.KLV_ZVA d1 ON b.KOD_ZVA = d1.ZVA_KOD
                                                                       LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                                                                       INNER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent = 1 AND i.MilitaryReportStatusID <> " + RemovedStatus.MilitaryReportStatusId.ToString() + @"
                                                                       " + where + @");
                                
                               UPDATE PMIS_RES.ReservistMilRepStatuses SET IsCurrent = 1
                               WHERE IsCurrent IS NULL AND ReservistID IN (SELECT ReservistID 
                                                                           FROM PMIS_RES.Reservists a                                                                
                                                                           INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                                                           LEFT OUTER JOIN VS_OWNER.KLV_ZVA d1 ON b.KOD_ZVA = d1.ZVA_KOD
                                                                           LEFT OUTER JOIN PMIS_ADM.Persons c ON a.PersonID = c.PersonID 
                                                                           INNER JOIN PMIS_RES.ReservistMilRepStatuses i ON a.ReservistID = i.ReservistID AND i.IsCurrent IS NULL AND i.MilitaryReportStatusID = " + RemovedStatus.MilitaryReportStatusId.ToString() + @"
                                                                           " + where + @");
                               
                               END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);
                if (filter.Age.HasValue)
                {
                    if (!string.IsNullOrEmpty(filter.ToDate) && CommonFunctions.TryParseDate(filter.ToDate))
                    {
                        cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = CommonFunctions.ParseDate(filter.ToDate);
                    }
                    else
                    {
                        cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = DateTime.Now;
                    }
                }
                if (string.IsNullOrEmpty(TakeDownDate))
                {
                    cmd.Parameters.Add("TakeDownDate", OracleType.DateTime).Value = DateTime.Now;
                }
                else
                {
                    if (CommonFunctions.TryParseDate(TakeDownDate))
                    {
                        cmd.Parameters.Add("TakeDownDate", OracleType.DateTime).Value = CommonFunctions.ParseDate(TakeDownDate);
                    }
                    else
                    {
                        cmd.Parameters.Add("TakeDownDate", OracleType.DateTime).Value = DateTime.Now;
                    }
                }

                OracleParameter param = new OracleParameter();
                param = new OracleParameter();
                param.ParameterName = "OrderNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(OrderNumber))
                    param.Value = OrderNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OrderDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (CommonFunctions.TryParseDate(OrderDate))
                    param.Value = CommonFunctions.ParseDate(OrderDate);
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OrderSignedBy";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(OrderSignedBy))
                    param.Value = OrderSignedBy;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();               
            }
            finally
            {
                conn.Close();
            }
        }

        //Save a particular Reservist into the DB
        public static bool AddReservist(Reservist reservist, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            string logDescription = "Име: " + reservist.Person.FullName + "; " +
                                    "ЕГН: " + reservist.Person.IdentNumber;

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_AddReservist", logDescription, null, reservist.Person, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"BEGIN
                            INSERT INTO PMIS_RES.Reservists(PersonID,
                               CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:PersonID,
                               :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_RES.Reservists_ID_SEQ.currval INTO :ReservistID FROM dual;
                         END;
                        ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramReservistID = new OracleParameter();
                paramReservistID.ParameterName = "ReservistID";
                paramReservistID.OracleType = OracleType.Number;
                paramReservistID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramReservistID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservist.Person.PersonId;
                cmd.Parameters.Add(param);                

                BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);
                
                cmd.ExecuteNonQuery();

                reservist.ReservistId = DBCommon.GetInt(paramReservistID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        //Save a particular Reservist into the DB
        public static bool SaveReservist_WhenEditingMilitaryReportTab(Reservist reservist, User currentUser, ChangeEvent changeEvent)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"UPDATE PMIS_RES.Reservists SET
                               GroupManagementSection = :GroupManagementSection,
                               Section = :Section,
                               Deliverer = :Deliverer,
                               PunktID = :PunktID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END                              
                             WHERE ReservistID = :ReservistID
                        ";

                Reservist oldReservist = ReservistUtil.GetReservist(reservist.ReservistId, currentUser);

                if (oldReservist.GroupManagementSection.Trim() != reservist.GroupManagementSection.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_GroupManagementSection", oldReservist.GroupManagementSection, reservist.GroupManagementSection, currentUser));

                if (oldReservist.Section.Trim() != reservist.Section.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_Section", oldReservist.Section, reservist.Section, currentUser));

                if (oldReservist.Deliverer.Trim() != reservist.Deliverer.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_Deliverer", oldReservist.Deliverer, reservist.Deliverer, currentUser));

                if (oldReservist.PunktID != reservist.PunktID)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_Punkt", oldReservist.Punkt != null ? oldReservist.Punkt.Text() : "", reservist.Punkt != null ? reservist.Punkt.Text() : "", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservist.ReservistId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "GroupManagementSection";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservist.GroupManagementSection))
                    param.Value = reservist.GroupManagementSection;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Section";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservist.Section))
                    param.Value = reservist.Section;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Deliverer";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(reservist.Deliverer))
                    param.Value = reservist.Deliverer;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PunktID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservist.PunktID.HasValue)
                    param.Value = reservist.PunktID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                PersonUtil.SetPersonModified(reservist.Person.PersonId, currentUser);
            }

            return result;
        }

        public static bool SaveReservist_SetNeedCourse(int reservistId, int needCourse, User currentUser, ChangeEvent changeEvent)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"UPDATE PMIS_RES.FillReservistsRequest SET
                               NeedCourse = :NeedCourse 
                             WHERE ReservistID = :ReservistID
                        ";

                List<FillReservistsRequest> allRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByReservist(reservistId, currentUser);
                FillReservistsRequest oldRequest = null;

                if (allRequests != null && allRequests.Count > 0)
                    oldRequest = allRequests[0];

                if (oldRequest.NeedCourse != (needCourse == 1))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_NeedCourse", oldRequest.NeedCourse ? "Да" : "Не", needCourse == 1 ? "Да" : "Не", currentUser));
            
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NeedCourse";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = needCourse;                
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static bool SaveReservist_SetAppointmentIsDelivered(int reservistId, int appointmentIsDelivered, User currentUser, ChangeEvent changeEvent)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"UPDATE PMIS_RES.FillReservistsRequest SET
                               AppointmentIsDelivered = :AppointmentIsDelivered 
                             WHERE ReservistID = :ReservistID
                        ";

                List<FillReservistsRequest> allRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByReservist(reservistId, currentUser);
                FillReservistsRequest oldRequest = null;

                if (allRequests != null && allRequests.Count > 0)
                    oldRequest = allRequests[0];

                if (oldRequest.AppointmentIsDelivered != (appointmentIsDelivered == 1))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_AppointmentIsDelivered", oldRequest.AppointmentIsDelivered ? "Да" : "Не", appointmentIsDelivered == 1 ? "Да" : "Не", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentIsDelivered";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = appointmentIsDelivered;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static bool SaveReservist_SetIsSuitableForMobAppointment(int personId, int isSuitableForMobAppointment, User currentUser, ChangeEvent changeEvent)
        {
            bool result = false;
            int oldIsSuitableForMobAppointment = 0;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"UPDATE PMIS_ADM.Persons SET
                               IsSuitableForMobAppointment = :IsSuitableForMobAppointment 
                         WHERE PersonID = :PersonId";

                Person getPerson = PersonUtil.GetPerson(personId,currentUser);

                if (getPerson.IsSuitableForMobAppointment)
                {
                    oldIsSuitableForMobAppointment = 1;
                }
                else
                {
                    oldIsSuitableForMobAppointment = 0;
                }

                if (oldIsSuitableForMobAppointment != isSuitableForMobAppointment)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_IsSuitableForMobAppointment", oldIsSuitableForMobAppointment.ToString(), isSuitableForMobAppointment.ToString(), currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PersonId";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsSuitableForMobAppointment";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = isSuitableForMobAppointment;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static void SetReservistModified(int reservistId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_RES.Reservists SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE ReservistID = :ReservistID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistID", OracleType.Number).Value = reservistId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public static string SearchResMilRepStatuses()
        {
            string statuses = Config.GetWebSetting("SearchFulfilReservistMilRepStatuses");
            string[] arrStatues = statuses.Split(',');

            statuses = "";
            foreach (string status in arrStatues)
            {
                statuses += (statuses == "" ? "" : ",") +
                             "'" + status.Replace("'", "''") + "'";
            }

            return statuses;
        }

        public static void ImportMilitaryReportSpecialityFromVitosha(Reservist pReservist, User pCurrentUser)
        {
            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"BEGIN                  
                    /*Get the code of thr PrimeMilitaryReportSpeciality for the person*/
                    SELECT VS_OWNER.VS_SPR.GET_UP_LS_VOS(:EGN) 
					INTO :PrimeMilRepSpecCode
                    FROM VS_OWNER.VS_LS
                    WHERE PersonID = :PersonID;     
                    
                    /*Insert all MilitaryReportSpeciality from Vitosha that have mach in PMIS_ADM.MilitaryReportSpecialities*/
                    INSERT INTO PMIS_ADM.PersonMilRepSpec(PersonID, MilReportSpecialityID, IsPrimary)
                    SELECT :PersonID, 
                           a.MilReportSpecialityID, 
                           CASE WHEN a.MilRepSpecCode = :PrimeMilRepSpecCode 
                                THEN 1
                                ELSE 0
                           END as IsPrime                     
                    FROM (
                      SELECT MIN(a.MilReportSpecialityID) as MilReportSpecialityID,
                             a.MilReportingSpecialityCode as MilRepSpecCode
                      FROM PMIS_ADM.MilitaryReportSpecialities a
                      INNER JOIN (
                          SELECT a.MilReportingSpecialityCode,
                                 MIN(a.type) as Type
                          FROM  PMIS_ADM.MilitaryReportSpecialities  a
                          INNER JOIN (
                              SELECT d.VSO_KOD,
                                     d.VSO_TYPE
                              FROM VS_OWNER.VS_LS a
                              INNER JOIN VS_OWNER.KLV_VSO d ON d.vso_kod = a.KOD_VSO
                              WHERE a.EGN = :EGN
                              
                              UNION 
                              
                              SELECT d.VSO_KOD,
                                     d.VSO_TYPE
                              FROM VS_OWNER.VS_LS a  
                              INNER JOIN VS_OWNER.KLV_VSO d ON d.vso_kod = a.KOD_VSS
                              WHERE a.EGN = :EGN
                              
                              UNION 
                              
                              SELECT d.VSO_KOD,
                                     d.VSO_TYPE
                              FROM VS_OWNER.VS_LS a  
                              INNER JOIN VS_OWNER.KLV_VSO d ON d.vso_kod = a.KOD_VSV
                              WHERE a.EGN = :EGN
                              
                              UNION 
                              
                              SELECT d.VSO_KOD,
                                     d.VSO_TYPE
                              FROM VS_OWNER.VS_LS a 
                              INNER JOIN VS_OWNER.KLV_VSO d ON d.vso_kod = a.KOD_VSA
                              WHERE a.EGN = :EGN
                              
                              UNION 
                              
                              SELECT b.VSO_KOD,
                                     b.VSO_TYPE
                              FROM VS_OWNER.VS_KURS a
                              INNER JOIN VS_OWNER.KLV_VSO b ON b.vso_kod = a.KURS_VOS
                              WHERE a.kurs_egnls = :EGN
                              
                              UNION 
                              
                              SELECT b.VSO_KOD,
                                     b.VSO_TYPE
                              FROM VS_OWNER.VS_OBRV a 
                              INNER JOIN VS_OWNER.KLV_VSO b ON b.vso_kod = a.OBRV_VOS
                              WHERE a.OBRV_egnls = :EGN
                              
                              UNION 
                              
                              SELECT b.VSO_KOD,
                                     b.VSO_TYPE
                              FROM VS_OWNER.VS_OBRVA a
                              INNER JOIN VS_OWNER.KLV_VSO b ON b.vso_kod = a.OBRVA_VOS
                              WHERE a.obrva_egnls = :EGN
                          ) b ON a.milreportingspecialitycode = b.VSO_KOD AND
                                 a.Type = CASE WHEN b.VSO_TYPE = 8 THEN 1 
                                               WHEN b.VSO_TYPE = 7 THEN 2 
                                               WHEN b.VSO_TYPE = 6 THEN 3 
                                               WHEN b.VSO_TYPE = 5 THEN 4 
                                               ELSE a.Type 
                                          END
                          GROUP BY a.milreportingspecialitycode
                       ) b ON a.MilReportingSpecialityCode = b.MilReportingSpecialityCode  
                              AND a.Type = b.Type              
                    GROUP BY a.MilReportingSpecialityCode
                    )  a
                    LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec p ON p.PersonID = :PersonID AND
                                                       p.MilReportSpecialityID = a.MilReportSpecialityID
                    WHERE p.MilReportSpecialityID IS NULL;

                    /*In case that there are no prime records for the person. Set the first one*/
                    UPDATE PMIS_ADM.PersonMilRepSpec
                    SET IsPrimary = 1
                    WHERE PersonMilRepSpecID = (SELECT MIN(PersonMilRepSpecID)
                                                FROM PMIS_ADM.PersonMilRepSpec
                                                WHERE PersonID = :PersonID
                                                GROUP BY PersonID
                                                HAVING SUM(IsPrimary) < 1);
                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "EGN";
                param.OracleType = OracleType.NVarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = pReservist.Person.IdentNumber;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pReservist.Person.PersonId;
                cmd.Parameters.Add(param);
                
                param = new OracleParameter();
                param.ParameterName = "PrimeMilRepSpecCode";
                param.Size = 10;
                param.OracleType = OracleType.NVarChar;
                param.Direction = ParameterDirection.InputOutput;
                param.Value = "";
                cmd.Parameters.Add(param);
                      
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public static string GetPersonStatus(Person person, User currentUser)
        {
            string status = "";
            Reservist reservist = ReservistUtil.GetReservistByIdentNumber(person.IdentNumber, currentUser);

            if (CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_VoluntaryReserve")))
            {
                status = Config.GetWebSetting("Status_VoluntaryReserve_Label");
            }
            else if (CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_RegularService")) &&
                    CommonFunctions.IsKeyInList(person.CategoryCode, Config.GetWebSetting("KOD_KAT_Civilian")))
            {
                status = Config.GetWebSetting("Status_RegularService_Civilian_Label");
            }
            else if (CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_RegularService")) &&
                    !CommonFunctions.IsKeyInList(person.CategoryCode, Config.GetWebSetting("KOD_KAT_Civilian")))
            {
                status = Config.GetWebSetting("Status_RegularService_Military_Label");
            }
            else if (reservist != null && reservist.CurrResMilRepStatus != null)
            {
                if (reservist.CurrResMilRepStatus.MilitaryReportStatus.MilitaryReportStatusKey == "REMOVED")
                {
                    status = Config.GetWebSetting("Status_Removed_Label");
                }
                else 
                {
                    status = Config.GetWebSetting("Status_Reserve_Label");
                }
            }
            else
            {
                status = Config.GetWebSetting("Status_NA_Label");
            }

            return status;
        }
    }
}