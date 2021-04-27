using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //The Person object represents a particular person and him personal iformation in the system
    //The Person details are stored in two main tables: VS_LS and Persons. The relation is 1:1
    public class Person : BaseDbObject
    {
        private int personId;
        private string firstName="";
        private string lastName="";
        private string identNumber="";
        private string militaryRankId;
        private MilitaryRank militaryRank;
        private Gender gender;
        private List<DrivingLicenseCategory> drivingLicenseCategories = null;
        private int? permCityId;
        private City permCity;
        private int? permDistrictId;
        private string permSecondPostCode;
        private District permDistrict;
        private string permAddress="";
        private int? presCityId;
        private City presCity;
        private int? presDistrictId;
        private string presSecondPostCode;
        private District presDistrict;
        private string presAddress="";
        private Address contactAddress;
        private long? homePhone;
        private string mobilePhone = "";
        private string businessPhone = "";
        private string email = "";
        private int? hasMilitaryService;
        private int? militaryUnitId;
        private MilitaryUnit militaryUnit;
        private string initials;
        private string idCardNumber;
        private string idCardIssuedBy;
        private DateTime? idCardIssueDate;
        private Country birthCountry;
        private int? birthCityId;
        private City birthCity;
        private string birthCityIfAbroad;
        private int? militaryTraining;
        private string recordOfServiceSeries;
        private string recordOfServiceNumber;
        private DateTime? recordOfServiceDate;
        private bool recordOfServiceCopy;
        private MaritalStatus maritalStatus;
        private string parentsContact;
        private int? childCount;
        private int? sizeClothingId;
        private GTableItem sizeClothing;
        private int? sizeHatId;
        private GTableItem sizeHat;
        private int? sizeShoesId;
        private GTableItem sizeShoes;
        private int? personHeight;
        private string personTypeCode; //KOD_KZV
        private string categoryCode;   //KOD_KAT
        private bool isAbroad;
        private string abroadCountryId;
        private Country abroadCountry;
        private DateTime? abroadSince;
        private int? abroadPeriod;
        private string otherInfo;
        private int? administrationID;        
        private Administration administration;        
        private int? clInformationAccLevelBgID;        
        private DateTime? clInformationAccLevelBgExpDate;
        private int? workPositionNKPDId;
        private NKPD workPositionNKPD;
        private int? workCompanyId;
        private Company workCompany;
        private bool isSuitableForMobAppointment;

        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string IdentNumber
        {
            get { return identNumber; }
            set { identNumber = value; }
        }

        public string MilitaryRankId
        {
            get { return militaryRankId; }
            set { militaryRankId = value; }
        }

        public MilitaryRank MilitaryRank
        {
            get
            {
                if (militaryRank == null)
                    militaryRank = MilitaryRankUtil.GetMilitaryRank(militaryRankId, CurrentUser);

                return militaryRank;
            }
            set { militaryRank = value; }
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public List<DrivingLicenseCategory> DrivingLicenseCategories
        {
            get
            {
                if (drivingLicenseCategories == null)
                    drivingLicenseCategories = DrivingLicenseCategoryUtil.GetPersonDrivingLicenseCategories(personId, CurrentUser);

                return drivingLicenseCategories;
            }
            set { drivingLicenseCategories = value; }
        }

        public string DrivingLicenseCategoriesString
        {
            get
            {
                string drivingLicenseCategories = "";

                foreach (DrivingLicenseCategory category in DrivingLicenseCategories)
                {
                    drivingLicenseCategories += (drivingLicenseCategories == "" ? "" : ", ") +
                        category.DrivingLicenseCategoryName.ToString();
                }

                return drivingLicenseCategories;
            }
        }

        public int? PermCityId
        {
            get { return permCityId; }
            set { permCityId = value; }
        }

        public City PermCity
        {
            get
            {
                if (permCity == null && permCityId != null)
                {
                    permCity = CityUtil.GetCity((int)permCityId, base.CurrentUser);
                }

                return permCity;
            }
            set { permCity = value; }
        }

        public int? PermDistrictId
        {
            get { return permDistrictId; }
            set { permDistrictId = value; }
        }

        public District PermDistrict
        {
            get
            {
                if (permDistrict == null && permDistrictId != null)
                {
                    permDistrict = DistrictUtil.GetDistrict((int)permDistrictId, base.CurrentUser);
                }
                return permDistrict;

            }
            set { permDistrict = value; }
        }

        public string PermSecondPostCode
        {
            get { return permSecondPostCode; }
            set { permSecondPostCode = value; }
        }

        public string PermAddress
        {
            get { return permAddress; }
            set { permAddress = value; }
        }

        public int? PresCityId
        {
            get { return presCityId; }
            set { presCityId = value; }
        }

        public City PresCity
        {
            get
            {
                if (presCity == null && presCityId != null)
                {
                    presCity = CityUtil.GetCity((int)presCityId, base.CurrentUser);
                }

                return presCity;
            }
            set { presCity = value; }
        }

        public int? PresDistrictId
        {
            get { return presDistrictId; }
            set { presDistrictId = value; }
        }

        public District PresDistrict
        {
            get
            {
                if (presDistrict == null && presDistrictId != null)
                {
                    presDistrict = DistrictUtil.GetDistrict((int)presDistrictId, base.CurrentUser);
                }
                return presDistrict;

            }
            set { presDistrict = value; }
        }

        public string PresSecondPostCode
        {
            get { return presSecondPostCode; }
            set { presSecondPostCode = value; }
        }

        public string PresAddress
        {
            get { return presAddress; }
            set { presAddress = value; }
        }

        public Address ContactAddress 
        { 
            get { return contactAddress; }
            set { contactAddress = value; }
        }

        public long? HomePhone
        {
            get { return homePhone; }
            set { homePhone = value; }
        }

        public string MobilePhone
        {
            get { return mobilePhone; }
            set { mobilePhone = value; }
        }

        public string BusinessPhone
        {
            get { return businessPhone; }
            set { businessPhone = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public int? HasMilitaryService
        {
            get { return hasMilitaryService; }
            set { hasMilitaryService = value; }
        }

        public int? MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get
            {
                if (militaryUnit == null && militaryUnitId != null)
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);

                return militaryUnit;
            }
            set { militaryUnit = value; }
        }

        public string Initials
        {
            get
            {
                return initials;
            }
            set
            {
                initials = value;
            }
        }

        public string IDCardNumber
        {
            get
            {
                return idCardNumber;
            }
            set
            {
                idCardNumber = value;
            }
        }

        public string IDCardIssuedBy
        {
            get
            {
                return idCardIssuedBy;
            }
            set
            {
                idCardIssuedBy = value;
            }
        }

        public DateTime? IDCardIssueDate
        {
            get
            {
                return idCardIssueDate;
            }
            set
            {
                idCardIssueDate = value;
            }
        }

        public Country BirthCountry
        {
            get
            {
                if (birthCountry == null &&
                   birthCityId.HasValue)
                {
                    birthCountry = CountryUtil.GetCountryBulgaria(CurrentUser);
                }

                return birthCountry;
            }
            set
            {
                birthCountry = value;
            }
        }

        public int? BirthCityId
        {
            get
            {
                return birthCityId;
            }
            set
            {
                birthCityId = value;
            }
        }

        public City BirthCity
        {
            get
            {
                if (birthCity == null && birthCityId.HasValue)
                {
                    birthCity = CityUtil.GetCity(birthCityId.Value, CurrentUser);
                }

                return birthCity;
            }
            set
            {
                birthCity = value;
            }
        }

        public string BirthCityIfAbroad
        {
            get
            {
                return birthCityIfAbroad;
            }
            set
            {
                birthCityIfAbroad = value;
            }
        }

        public int? MilitaryTraining
        {
            get
            {
                return militaryTraining;
            }
            set
            {
                militaryTraining = value;
            }
        }

        public string MilitaryTrainingString
        {
            get
            {
                string milTraining = "";

                if (MilitaryTraining.HasValue)
                {
                    if (MilitaryTraining.Value == 1)
                        milTraining = "Има";
                    else if (MilitaryTraining.Value == 2)
                        milTraining = "Няма";
                }

                return milTraining;
            }
        }

        public string RecordOfServiceSeries
        {
            get
            {
                return recordOfServiceSeries;
            }
            set
            {
                recordOfServiceSeries = value;
            }
        }

        public string RecordOfServiceNumber
        {
            get
            {
                return recordOfServiceNumber;
            }
            set
            {
                recordOfServiceNumber = value;
            }
        }

        public DateTime? RecordOfServiceDate
        {
            get
            {
                return recordOfServiceDate;
            }
            set
            {
                recordOfServiceDate = value;
            }
        }

        public bool RecordOfServiceCopy
        {
            get
            {
                return recordOfServiceCopy;
            }
            set
            {
                recordOfServiceCopy = value;
            }
        }

        public MaritalStatus MaritalStatus
        {
            get
            {
                return maritalStatus;
            }
            set
            {
                maritalStatus = value;
            }
        }

        public string ParentsContact
        {
            get
            {
                return parentsContact;
            }
            set
            {
                parentsContact = value;
            }
        }

        public int? ChildCount
        {
            get
            {
                return childCount;
            }
            set
            {
                childCount = value;
            }
        }

        public int? SizeClothingId
        {
            get
            {
                return sizeClothingId;
            }
            set
            {
                sizeClothingId = value;
            }
        }

        public GTableItem SizeClothing
        {
            get
            {
                if (sizeClothing == null && sizeClothingId.HasValue)
                {
                    sizeClothing = GTableItemUtil.GetTableItem("SizeClothing", sizeClothingId.Value, ModuleUtil.RES(), CurrentUser);
                }

                return sizeClothing;
            }
            set
            {
                sizeClothing = value;
            }
        }

        public int? SizeHatId
        {
            get
            {
                return sizeHatId;
            }
            set
            {
                sizeHatId = value;
            }
        }

        public GTableItem SizeHat
        {
            get
            {
                if (sizeHat == null && sizeHatId.HasValue)
                {
                    sizeHat = GTableItemUtil.GetTableItem("SizeHat", sizeHatId.Value, ModuleUtil.RES(), CurrentUser);
                }

                return sizeHat;
            }
            set
            {
                sizeHat = value;
            }
        }

        public int? SizeShoesId
        {
            get
            {
                return sizeShoesId;
            }
            set
            {
                sizeShoesId = value;
            }
        }


        public GTableItem SizeShoes
        {
            get
            {
                if (sizeShoes == null && sizeShoesId.HasValue)
                {
                    sizeShoes = GTableItemUtil.GetTableItem("SizeShoes", sizeShoesId.Value, ModuleUtil.RES(), CurrentUser);
                }

                return sizeShoes;
            }
            set
            {
                sizeShoes = value;
            }
        }


        public int? PersonHeight
        {
            get
            {
                return personHeight;
            }
            set
            {
                personHeight = value;
            }
        }

        public string PersonTypeCode
        {
            get
            {
                return personTypeCode;
            }
            set
            {
                personTypeCode = value;
            }
        }

        public string CategoryCode
        {
            get
            {
                return categoryCode;
            }
            set
            {
                categoryCode = value;
            }
        }

        public bool IsAbroad
        {
            get
            {
                return isAbroad;
            }
            set
            {
                isAbroad = value;
            }
        }

        public string AbroadCountryId
        {
            get
            {
                return abroadCountryId;
            }
            set
            {
                abroadCountryId = value;
            }
        }

        public Country AbroadCountry
        {
            get
            {
                //Lazy initialization
                if (abroadCountry == null && !String.IsNullOrEmpty(abroadCountryId))
                    abroadCountry = CountryUtil.GetCountry(abroadCountryId, CurrentUser);
                
                return abroadCountry;
            }
            set
            {
                abroadCountry = value;
            }
        }

        public DateTime? AbroadSince
        {
            get
            {
                return abroadSince;
            }
            set
            {
                abroadSince = value;
            }
        }

        public int? AbroadPeriod
        {
            get
            {
                return abroadPeriod;
            }
            set
            {
                abroadPeriod = value;
            }
        }

        public string OtherInfo
        {
            get { return otherInfo; }
            set { otherInfo = value; }
        }

        public int? AdministrationID
        {
            get { return administrationID; }
            set { administrationID = value; }

        }

        public Administration Administration
        {
            get
            {
                if (administration == null && administrationID.HasValue)
                    administration = AdministrationUtil.GetAdministration(administrationID.Value, CurrentUser);

                return administration;
            }
            set { administration = value; }

        }

        public int? ClInformationAccLevelBgID
        {
            get { return clInformationAccLevelBgID; }
            set { clInformationAccLevelBgID = value; }
        }

        public string ClInformationAccLevelBg
        {
            get
            {
                string clInformationAccLevelBg = "";

                if (ClInformationAccLevelBgID.HasValue)
                    clInformationAccLevelBg = ClInformationUtil.GetClInformationBG(ClInformationAccLevelBgID.Value.ToString(), CurrentUser).ClInfoName;

                return clInformationAccLevelBg;
            }
        }

        public DateTime? ClInformationAccLevelBgExpDate
        {
            get { return clInformationAccLevelBgExpDate; }
            set { clInformationAccLevelBgExpDate = value; }
        }

        public int? WorkPositionNKPDId
        {
            get { return workPositionNKPDId; }
            set { workPositionNKPDId = value; }
        }

        public NKPD WorkPositionNKPD
        {
            get
            {
                if (workPositionNKPD == null && workPositionNKPDId.HasValue)
                    workPositionNKPD = NKPDUtil.GetNKPD(workPositionNKPDId.Value, CurrentUser);

                return workPositionNKPD;
            }
            set { workPositionNKPD = value; }
        }

        public int? WorkCompanyId
        {
            get { return workCompanyId; }
            set { workCompanyId = value; }
        }

        public Company WorkCompany
        {
            get
            {
                if (workCompany == null && workCompanyId.HasValue)
                    workCompany = CompanyUtil.GetCompany(workCompanyId.Value, CurrentUser);

                return workCompany;
            }
            set { workCompany = value; }
        }

        public bool IsSuitableForMobAppointment
        {
            get { return isSuitableForMobAppointment; }
            set { isSuitableForMobAppointment = value; }
        }

        public Person(User user)
            : base(user)
        {
        }
    }

    public class PersonStatus
    {
        public int PersonID { get; set; }
        public string Status { get; set; }
        public Dictionary<string, string> Details {get; set;}

        public PersonStatus()
        {
            this.PersonID = 0;
            this.Status = "Нов";
            this.Details = new Dictionary<string, string>();
        }
    }
   
    //This is an "util" class that provides methods for working with Person objects
    public static class PersonUtil
    {
        //Extracts a new Person object from a particualr data reader
        //Use this method when pulling person records from the DB
        public static Person ExtractPersonFromDR(User currentUser, OracleDataReader dr)
        {
            Person person = new Person(currentUser);

            person.PersonId = (DBCommon.IsInt(dr["PersonID"]) ? DBCommon.GetInt(dr["PersonID"]) : 0);
            person.MilitaryRankId = dr["MilitaryRankID"].ToString();
            person.FirstName = dr["FirstName"].ToString();
            person.LastName = dr["LastName"].ToString();
            person.IdentNumber = dr["IdentNumber"].ToString();
            person.Gender = GenderUtil.ExtractGenderFromDR(currentUser, dr);
            person.PermCityId = (DBCommon.IsInt(dr["PermCityID"]) ? DBCommon.GetInt(dr["PermCityID"]) : (int?)null);
            person.PermDistrictId = (DBCommon.IsInt(dr["PermDistrictID"]) && person.PermCityId.HasValue ? DBCommon.GetInt(dr["PermDistrictID"]) : (int?)null);

            if (!string.IsNullOrEmpty(dr["PermSecondPostCode"].ToString()))
            {
                person.PermSecondPostCode = dr["PermSecondPostCode"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["PermAddress"].ToString()))
            {
                person.PermAddress = dr["PermAddress"].ToString();
            }

            long homePhone = 0;
            if (long.TryParse(dr["HomePhone"].ToString(), out homePhone))
            {
                person.HomePhone = homePhone;
            }
            else
            {
                person.HomePhone = (long?)null;
            }

            person.LastModifiedDate = (dr["LastModifiedDate"] is DateTime) ? (DateTime)dr["LastModifiedDate"] : (DateTime?)null;
            person.PresCityId = (DBCommon.IsInt(dr["PresCityID"]) ? DBCommon.GetInt(dr["PresCityID"]) : (int?)null);
            person.PresDistrictId = (DBCommon.IsInt(dr["PresDistrictID"]) && person.PresCityId.HasValue ? DBCommon.GetInt(dr["PresDistrictID"]) : (int?)null);

            if (!string.IsNullOrEmpty(dr["PresSecondPostCode"].ToString()))
            {
                person.PresSecondPostCode = dr["PresSecondPostCode"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["PresAddress"].ToString()))
            {
                person.PresAddress = dr["PresAddress"].ToString();
            }

            person.ContactAddress = AddressUtil.ExtractAddressFromDR("Contact", currentUser, dr);

            if (!string.IsNullOrEmpty(dr["MobilePhone"].ToString()))
            {
                person.MobilePhone = dr["MobilePhone"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["BusinessPhone"].ToString()))
            {
                person.BusinessPhone = dr["BusinessPhone"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["Email"].ToString()))
            {
                person.Email = dr["Email"].ToString();
            }

            person.HasMilitaryService = (DBCommon.IsInt(dr["MilitaryService"]) ? DBCommon.GetInt(dr["MilitaryService"]) : (int?)null);

            if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                person.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);

            person.Initials = dr["Initials"].ToString();
            person.IDCardNumber = dr["IDCardNumber"].ToString();
            person.IDCardIssuedBy = dr["IDCardIssuedBy"].ToString();
            person.IDCardIssueDate = (dr["IDCardIssueDate"] is DateTime) ? (DateTime)dr["IDCardIssueDate"] : (DateTime?)null;

            if (DBCommon.IsInt(dr["BirthCountryID"]))
            {
                person.BirthCountry = new Country(currentUser);
                person.BirthCountry.CountryId = dr["BirthCountryID"].ToString();
                person.BirthCountry.CountryName = dr["BirthCountryName"].ToString();
            }

            person.BirthCityId = (DBCommon.IsInt(dr["BirthCityID"]) ? DBCommon.GetInt(dr["BirthCityID"]) : (int?)null);
            person.BirthCityIfAbroad = dr["BirthCityIfAbroad"].ToString();
            person.MilitaryTraining = (DBCommon.IsInt(dr["MilitaryTraining"]) ? DBCommon.GetInt(dr["MilitaryTraining"]) : (int?)null);
            person.RecordOfServiceSeries = dr["RecordOfServiceSeries"].ToString();
            person.RecordOfServiceNumber = dr["RecordOfServiceNumber"].ToString();
            person.RecordOfServiceDate = (dr["RecordOfServiceDate"] is DateTime) ? (DateTime)dr["RecordOfServiceDate"] : (DateTime?)null;
            person.RecordOfServiceCopy = (DBCommon.IsInt(dr["RecordOfServiceCopy"]) && DBCommon.GetInt(dr["RecordOfServiceCopy"]) == 1);

            if (!String.IsNullOrEmpty(dr["MaritalStatusKey"].ToString()))
            {
                person.MaritalStatus = new MaritalStatus();
                person.MaritalStatus.MaritalStatusKey = dr["MaritalStatusKey"].ToString();
                person.MaritalStatus.MaritalStatusName = dr["MaritalStatusName"].ToString();
            }

            person.ParentsContact = dr["ParentsContact"].ToString();
            person.ChildCount = (DBCommon.IsInt(dr["ChildCount"]) ? DBCommon.GetInt(dr["ChildCount"]) : (int?)null);
            person.SizeClothingId = (DBCommon.IsInt(dr["SizeClothingID"]) ? DBCommon.GetInt(dr["SizeClothingID"]) : (int?)null);
            person.SizeHatId = (DBCommon.IsInt(dr["SizeHatID"]) ? DBCommon.GetInt(dr["SizeHatID"]) : (int?)null);
            person.SizeShoesId = (DBCommon.IsInt(dr["SizeShoesID"]) ? DBCommon.GetInt(dr["SizeShoesID"]) : (int?)null);
            person.PersonHeight = (DBCommon.IsInt(dr["PersonHeight"]) ? DBCommon.GetInt(dr["PersonHeight"]) : (int?)null);
            person.PersonTypeCode = dr["PersonTypeCode"].ToString();
            person.CategoryCode = dr["CategoryCode"].ToString();
            person.IsAbroad = (DBCommon.IsInt(dr["IsAbroad"]) && DBCommon.GetInt(dr["IsAbroad"]) == 1);
            person.AbroadCountryId = dr["AbroadCountryID"].ToString();
            person.AbroadSince = (dr["AbroadSince"] is DateTime) ? (DateTime)dr["AbroadSince"] : (DateTime?)null;
            person.AbroadPeriod = (DBCommon.IsInt(dr["AbroadPeriod"]) ? DBCommon.GetInt(dr["AbroadPeriod"]) : (int?)null);
            person.OtherInfo = dr["OtherInfo"].ToString();

            person.AdministrationID = (DBCommon.IsInt(dr["AdministrationID"]) ? DBCommon.GetInt(dr["AdministrationID"]) : (int?)null);
            person.ClInformationAccLevelBgID = (DBCommon.IsInt(dr["ClInformationAccLevelBg"]) ? DBCommon.GetInt(dr["ClInformationAccLevelBg"]) : (int?)null);
            person.ClInformationAccLevelBgExpDate = (dr["ClInformationAccLevelBgExpDate"] is DateTime) ? (DateTime)dr["ClInformationAccLevelBgExpDate"] : (DateTime?)null;

            person.WorkPositionNKPDId = (DBCommon.IsInt(dr["WorkPositionNKPDID"]) ? DBCommon.GetInt(dr["WorkPositionNKPDID"]) : (int?)null);
            person.WorkCompanyId = (DBCommon.IsInt(dr["WorkCompanyID"]) ? DBCommon.GetInt(dr["WorkCompanyID"]) : (int?)null);
            person.IsSuitableForMobAppointment = (DBCommon.IsInt(dr["IsSuitableForMobAppointment"]) && DBCommon.GetInt(dr["IsSuitableForMobAppointment"]) == 1);

            return person;
        }

        //Get a single Person object by PersonID
        public static Person GetPerson(int personId, User currentUser)
        {
            Person person = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonID as PersonID, 
                                      a.IME as FirstName, 
                                      a.FAM as LastName, 
                                      a.EGN as IdentNumber, 
                                      a.KOD_KZV as PersonTypeCode,
                                      a.KOD_KAT as CategoryCode,
                                      a.KOD_ZVA as MilitaryRankID,
                                      a.KOD_NMA_MJ as PermCityID,
                                      a.PermAddrDistrictID as PermDistrictID,
                                      a.ADRES as PermAddress,
                                      a.TEL as HomePhone,
                                      a.V_PODELENIE as MilitaryUnitID,
                                      b.GenderID, c.GenderName,
                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                      a.CurrAddrCityID as PresCityID, a.CurrAddrDistrictID as PresDistrictID, a.CurrAddress as PresAddress, 
                                      b.MobilePhone, b.BusinessPhone, b.Email, b.MilitaryService, b.MilitaryService,
                                      b.Initials, b.IDCardNumber, b.IDCardIssuedBy, b.IDCardIssueDate,
                                      d.DJJ_KOD as BirthCountryID, d.DJJ_IME as BirthCountryName,
                                      a.KOD_NMA_MR as BirthCityID, b.BirthCityIfAbroad,
                                      b.MilitaryTraining, 
                                      b.RecordOfServiceSeries, b.RecordOfServiceNumber, b.RecordOfServiceDate, b.RecordOfServiceCopy,
                                      e.MaritalStatusKey, e.MaritalStatusName, b.ParentsContact, b.ChildCount,
                                      b.SizeClothingID, b.SizeHatID, b.SizeShoesID, b.PersonHeight,
                                      b.IsAbroad, b.AbroadCountryID, b.AbroadSince, b.AbroadPeriod, a.TXT as OtherInfo, b.AdministrationID,
                                      b.ClInformationAccLevelBg, b.ClInformationAccLevelBgExpDate,
                                      b.WorkPositionNKPDID, b.WorkCompanyID,
                                      a.PermSecondPostCode, a.PresSecondPostCode, b.IsSuitableForMobAppointment,
                                      g.CityID as ContactCityID, g.DistrictID as ContactDistrictID, g.PostCode as ContactPostCode, g.AddressText as ContactAddressText
                               FROM VS_OWNER.VS_LS a
                               LEFT OUTER JOIN PMIS_ADM.Persons b ON a.PersonID = b.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Gender c ON b.GenderID = c.GenderID
                               LEFT OUTER JOIN VS_OWNER.KLV_DJJ d ON b.BirthCountryID = d.DJJ_KOD
                               LEFT OUTER JOIN PMIS_ADM.MaritalStatuses e ON a.KOD_SPO = e.MaritalStatusKey
                               LEFT OUTER JOIN PMIS_ADM.PersonAddresses f ON a.PersonID = f.PersonID AND f.AddressType = 'ADR_CONTACT'
                               LEFT OUTER JOIN PMIS_ADM.Addresses g ON f.AddressID = g.AddressID
                               WHERE a.PersonID = :PersonID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    person = ExtractPersonFromDR(currentUser, dr);
                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, person);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return person;
        }

        //Get a person by its IdentNumber
        public static Person GetPersonByIdentNumber(string identNumber, User currentUser)
        {
            Person person = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonID as PersonID, 
                                      a.IME as FirstName, 
                                      a.FAM as LastName, 
                                      a.EGN as IdentNumber, 
                                      a.KOD_KZV as PersonTypeCode,
                                      a.KOD_KAT as CategoryCode,
                                      a.KOD_ZVA as MilitaryRankID,
                                      a.KOD_NMA_MJ as PermCityID,
                                      a.PermAddrDistrictID as PermDistrictID,
                                      a.ADRES as PermAddress,
                                      a.TEL as HomePhone,
                                      a.V_PODELENIE as MilitaryUnitID,
                                      b.GenderID, c.GenderName,
                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                      a.CurrAddrCityID as PresCityID, a.CurrAddrDistrictID as PresDistrictID, a.CurrAddress as PresAddress, 
                                      b.MobilePhone, b.BusinessPhone, b.Email, b.MilitaryService, b.MilitaryService,
                                      b.Initials, b.IDCardNumber, b.IDCardIssuedBy, b.IDCardIssueDate,
                                      d.DJJ_KOD as BirthCountryID, d.DJJ_IME as BirthCountryName,
                                      a.KOD_NMA_MR as BirthCityID, b.BirthCityIfAbroad,
                                      b.MilitaryTraining, 
                                      b.RecordOfServiceSeries, b.RecordOfServiceNumber, b.RecordOfServiceDate, b.RecordOfServiceCopy,
                                      e.MaritalStatusKey, e.MaritalStatusName, b.ParentsContact, b.ChildCount,
                                      b.SizeClothingID, b.SizeHatID, b.SizeShoesID, b.PersonHeight,
                                      b.IsAbroad, b.AbroadCountryID, b.AbroadSince, b.AbroadPeriod, a.TXT as OtherInfo, b.AdministrationID,
                                      b.ClInformationAccLevelBg, b.ClInformationAccLevelBgExpDate,
                                      b.WorkPositionNKPDID, b.WorkCompanyID,
                                      a.PermSecondPostCode, a.PresSecondPostCode, b.IsSuitableForMobAppointment,
                                      g.CityID as ContactCityID, g.DistrictID as ContactDistrictID, g.PostCode as ContactPostCode, g.AddressText as ContactAddressText                                      
                               FROM VS_OWNER.VS_LS a
                               LEFT OUTER JOIN PMIS_ADM.Persons b ON a.PersonID = b.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Gender c ON b.GenderID = c.GenderID
                               LEFT OUTER JOIN VS_OWNER.KLV_DJJ d ON b.BirthCountryID = d.DJJ_KOD
                               LEFT OUTER JOIN PMIS_ADM.MaritalStatuses e ON a.KOD_SPO = e.MaritalStatusKey
                               LEFT OUTER JOIN PMIS_ADM.PersonAddresses f ON a.PersonID = f.PersonID AND f.AddressType = 'ADR_CONTACT'
                               LEFT OUTER JOIN PMIS_ADM.Addresses g ON f.AddressID = g.AddressID
                               WHERE a.EGN = :IdentNumber";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentNumber", OracleType.VarChar).Value = identNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    person = ExtractPersonFromDR(currentUser, dr);
                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, person);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return person;
        }

        //Get a list of all Persons in the system
        public static List<Person> GetAllPersons(User currentUser)
        {
            List<Person> persons = new List<Person>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonID as PersonID, 
                                      a.IME as FirstName, 
                                      a.FAM as LastName, 
                                      a.EGN as IdentNumber, 
                                      a.KOD_KZV as PersonTypeCode,
                                      a.KOD_KAT as CategoryCode,
                                      a.KOD_ZVA as MilitaryRankID,
                                      a.KOD_NMA_MJ as PermCityID,
                                      a.PermAddrDistrictID as PermDistrictID,
                                      a.ADRES as PermAddress,
                                      a.TEL as HomePhone,
                                      a.V_PODELENIE as MilitaryUnitID,
                                      b.GenderID, c.GenderName,
                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                      a.CurrAddrCityID as PresCityID, a.CurrAddrDistrictID as PresDistrictID, a.CurrAddress as PresAddress, 
                                      b.MobilePhone, b.BusinessPhone, b.Email, b.MilitaryService, b.MilitaryService,
                                      b.Initials, b.IDCardNumber, b.IDCardIssuedBy, b.IDCardIssueDate,
                                      d.DJJ_KOD as BirthCountryID, d.DJJ_IME as BirthCountryName,
                                      a.KOD_NMA_MR as BirthCityID, b.BirthCityIfAbroad,
                                      b.MilitaryTraining, 
                                      b.RecordOfServiceSeries, b.RecordOfServiceNumber, b.RecordOfServiceDate, b.RecordOfServiceCopy,
                                      e.MaritalStatusKey, e.MaritalStatusName, b.ParentsContact, b.ChildCount,
                                      b.SizeClothingID, b.SizeHatID, b.SizeShoesID, b.PersonHeight,
                                      b.IsAbroad, b.AbroadCountryID, b.AbroadSince, b.AbroadPeriod, a.TXT as OtherInfo, b.AdministrationID,
                                      b.ClInformationAccLevelBg, b.ClInformationAccLevelBgExpDate,
                                      b.WorkPositionNKPDID, b.WorkCompanyID,
                                      a.PermSecondPostCode, a.PresSecondPostCode, b.IsSuitableForMobAppointment,
                                      g.CityID as ContactCityID, g.DistrictID as ContactDistrictID, g.PostCode as ContactPostCode, g.AddressText as ContactAddressText                                      
                               FROM VS_OWNER.VS_LS a
                               LEFT OUTER JOIN PMIS_ADM.Persons b ON a.PersonID = b.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Gender c ON b.GenderID = c.GenderID
                               LEFT OUTER JOIN VS_OWNER.KLV_DJJ d ON b.BirthCountryID = d.DJJ_KOD
                               LEFT OUTER JOIN PMIS_ADM.MaritalStatuses e ON a.KOD_SPO = e.MaritalStatusKey
                               LEFT OUTER JOIN PMIS_ADM.PersonAddresses f ON a.PersonID = f.PersonID AND f.AddressType = 'ADR_CONTACT'
                               LEFT OUTER JOIN PMIS_ADM.Addresses g ON f.AddressID = g.AddressID
                               ORDER BY PersonID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonID"]))
                    {
                        Person person = ExtractPersonFromDR(currentUser, dr);
                        BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, person);
                        persons.Add(person);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return persons;
        }

        public static List<Person> GetAllPersons(string identNumber, string firstName, string lastName, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<Person> persons = new List<Person>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(identNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EGN LIKE '%" + identNumber.Replace("'", "''") + @"%' ";
                }

                if (!String.IsNullOrEmpty(firstName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.IME) LIKE '%" + firstName.Replace("'", "''").ToUpper() + @"%' ";
                }

                if (!String.IsNullOrEmpty(lastName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.FAM) LIKE '%" + lastName.Replace("'", "''").ToUpper() + @"%' ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "a.EGN";
                        break;
                    case 2:
                        orderBySQL = "a.IME " + orderByDir;
                        orderBySQL += ", a.FAM";
                        break;
                    case 3:
                        orderBySQL = "c.ZVA_IME";
                        break;
                    default:
                        orderBySQL = "a.EGN";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.PersonID as PersonID, tmp.RowNumber as RowNumber  FROM (
                                  SELECT a.PersonID as PersonID,                                         
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.PersonID) as RowNumber, b.IsSuitableForMobAppoitment
                                  FROM VS_OWNER.VS_LS a
                                  LEFT OUTER JOIN PMIS_ADM.PERSONS b ON a.PersonID = b.PersonID                                                      
                                  LEFT OUTER JOIN VS_OWNER.KLV_ZVA c ON a.KOD_ZVA = c.ZVA_KOD           
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", PersonID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonID"]))
                        persons.Add(PersonUtil.GetPerson(DBCommon.GetInt(dr["PersonID"]), currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return persons;
        }

        public static int GetAllPersonsCount(string identNumber, string firstName, string lastName, User currentUser)
        {
            int personsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(identNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EGN LIKE '%" + identNumber.Replace("'", "''") + @"%' ";
                }

                if (!String.IsNullOrEmpty(firstName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.IME) LIKE '%" + firstName.Replace("'", "''").ToUpper() + @"%' ";
                }

                if (!String.IsNullOrEmpty(lastName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.FAM) LIKE '%" + lastName.Replace("'", "''").ToUpper() + @"%' ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                string SQL = @"SELECT COUNT(*) as Cnt
                                  FROM VS_OWNER.VS_LS a
                                  LEFT OUTER JOIN PMIS_ADM.PERSONS b ON a.PersonID = b.PersonID                                                      
                                  LEFT OUTER JOIN VS_OWNER.KLV_ZVA c ON a.KOD_ZVA = c.ZVA_KOD           
                                  " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        personsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personsCnt;
        }

        //This function returns the PersonID for a specific IdentNumber
        public static int? GetPersonIdByIdentNumber(string identNumber, User currentUser)
        {
            int? personId = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT PersonID
                               FROM VS_OWNER.VS_LS
                               WHERE EGN = :IdentNumber";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentNumber", OracleType.VarChar).Value = identNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonID"]))
                        personId = DBCommon.GetInt(dr["PersonID"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personId;
        }

        //Save a particular Person into the DB
        //Use this method when updating a Person from the Add new Applicant process
        public static bool SavePerson_WhenAddingNewApplicant(Person person, string changeEventType, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"DECLARE
                           ExistingPersonsCnt number;                         

                        BEGIN
                        
                       ";

                if (person.PersonId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_LS(EGN, FAM, IME, KOD_KZV, KOD_KAT, 
                                  NLD, ACK, KOD_NMA_MR, KOD_NMA_MJ, ADRES, TEL, /* KOD_NAR, */
                                  /* KOD_SPO, */ S_UD, N_UD, D_UD, V_BA, KOD_VIS,
                                  KOD_TIT, KOD_ZVA, Z_Z, Z_ZKOGA, KOD_SPZZ,
                                  /* KOD_JIL, KOD_KRG, */ DOPUSK, KOD_VSO, KOD_VSS,
                                  GDAT, ATES, ATES_TXT, KOD_UVO, PSTG, PSTM,
                                  PSTD, OFZV, KOD_PSO, /*OBR_SHZO, OBR_SHPZO,*/
                                  /* OBR_VU, OBR_VA, OBR_VISHE, OBR_SR, */ V_PODELENIE,
                                  V_KOMANDA, /* ZVR_SNET,  ZOTSR, ZOGVS, */ ZKOD_DLO,
                                  ZKOD_DLS, ZPR_DL, ZPDLO, ZPDLS, ZPPR_DL,
                                  KOD_KATV, ZKURS_DL, ZKURS_MES, ZVVO, ZDATA_OTCH,
                                  OT_UD, GR_UD, OSTG, OSTM, OSTD, TXT,
                                  DOPUSK_DO, Z_ZIZD, KOD_VSV, KOD_VSA,
                                  CurrAddrCityID, CurrAddress,
                                  PermAddrDistrictID, CurrAddrDistrictID,
                                PermSecondPostCode, PresSecondPostCode)
                             VALUES (:IdentNumber, :LastName, :FirstName, '" + Config.GetWebSetting("KOD_KZV_Applicant") + @"', " + Config.GetWebSetting("KOD_KAT_Applicant") + @" ,
                                  NULL, NULL, :BirthCityID, :PermCityID, :PermAddress, :HomePhone, /* 'Б',*/
                                  /* 'Н', */ NULL, NULL, NULL, SYSDATE, NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  /* 'БЖ', 'А1+', */ NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, /* 'N', 'N', */
                                  /* 'N', 'N', 'Y', 'Y', */ '" + Config.GetWebSetting("V_PODELENIE_Applicant") + @"',
                                  '" + Config.GetWebSetting("V_KOMANDA_Applicant") + @"',/* 'N', 'N', 'N' ,*/ NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL,
                                  :PresCityID, :PresAddress, 
                                  :PermAddrDistrictID, :CurrAddrDistrictID,
                                  :PermSecondPostCode, :PresSecondPostCode);

                             SELECT VS_OWNER.VS_LS_PersonID_SEQ.currval INTO :PersonID FROM dual;

                             INSERT INTO PMIS_ADM.Persons (PersonID, GenderID,
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate,
                                IDCardNumber, IDCardIssuedBy, IDCardIssueDate,
                                MobilePhone, Email, MilitaryService, MilitaryTraining,
                                BirthCountryID, BirthCityIfAbroad)
                             VALUES (:PersonID, :GenderID,
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate,
                                :IDCardNumber, :IDCardIssuedBy, :IDCardIssueDate,
                                :MobilePhone, :Email, :HasMilitaryService, :MilitaryTraining,
                                :BirthCountryID, :BirthCityIfAbroad);

                             UPDATE PMIS_ADM.Persons SET Initials = (SELECT PMIS_ADM.CommonFunctions.GetInitials(IME || ' ' || FAM) 
                                                                     FROM VS_OWNER.VS_LS 
                                                                     WHERE VS_OWNER.VS_LS.PersonID = :PersonID)
                             WHERE PMIS_ADM.Persons.PersonID = :PersonID;          

                             INSERT INTO PMIS_ADM.Addresses (CityID, DistrictID, PostCode, AddressText)
                             VALUES (:ContactCityID, :ContactDistrictID, :ContactPostCode, :ContactAddressText);

                             SELECT PMIS_ADM.Addresses_ID_SEQ.currval INTO :AddressID FROM dual;  
 
                             INSERT INTO PMIS_ADM.PersonAddresses (PersonID, AddressID, AddressType)
                             VALUES (:PersonID, :AddressID, 'ADR_CONTACT');
                            ";

                    changeEvent = GetChangeEvents(changeEventType, null, person, currentUser);
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_LS SET
                               IME = :FirstName, 
                               FAM = :LastName,
                               ADRES = :PermAddress,
                               TEL = :HomePhone,
                               KOD_NMA_MJ = :PermCityID,
                               CurrAddrCityID = :PresCityID,
                               CurrAddress = :PresAddress,
                               PermAddrDistrictID = :PermAddrDistrictID,
                               CurrAddrDistrictID = :CurrAddrDistrictID,
                               PermSecondPostCode = :PermSecondPostCode, 
                               PresSecondPostCode = :PresSecondPostCode
                               /*,KOD_KZV = '" + Config.GetWebSetting("KOD_KZV_Applicant") + @"' */
                             WHERE PersonID = :PersonID;

                             SELECT COUNT(*) INTO ExistingPersonsCnt
                             FROM PMIS_ADM.Persons a
                             WHERE a.PersonID = :PersonID;

                             IF (NVL(ExistingPersonsCnt, 0) = 0) THEN
                                INSERT INTO PMIS_ADM.Persons (PersonID)
                                VALUES (:PersonID);
                             END IF;

                             UPDATE PMIS_ADM.Persons SET
                               GenderID = :GenderID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END,
                               IDCardNumber = :IDCardNumber, 
                               IDCardIssuedBy = :IDCardIssuedBy, 
                               IDCardIssueDate = :IDCardIssueDate,
                               MobilePhone = :MobilePhone,
                               Email = :Email,
                               MilitaryService = :HasMilitaryService,
                               MilitaryTraining = :MilitaryTraining
                             WHERE PersonID = :PersonID;

                             UPDATE PMIS_ADM.Persons SET Initials = (SELECT PMIS_ADM.CommonFunctions.GetInitials(IME || ' ' || FAM) 
                                                                     FROM VS_OWNER.VS_LS 
                                                                     WHERE VS_OWNER.VS_LS.PersonID = :PersonID)
                             WHERE PMIS_ADM.Persons.PersonID = :PersonID;

                             DELETE FROM PMIS_ADM.PersonDrivingLicenseCategories 
                             WHERE PersonID = :PersonID;
                            ";

                    if (person.ContactAddress.AddressId > 0)
                    {
                        SQL += @" UPDATE PMIS_ADM.Addresses SET
                                      CityID = :ContactCityID,
                                      DistrictID = :ContactDistrictID,
                                      PostCode = :ContactPostCode,
                                      AddressText = :ContactAddressText
                                  WHERE AddressID = :AddressID;
                                ";
                    }
                    else
                    {
                        SQL += @" INSERT INTO PMIS_ADM.Addresses (CityID, DistrictID, PostCode, AddressText)
                                  VALUES (:ContactCityID, :ContactDistrictID, :ContactPostCode, :ContactAddressText);

                                  SELECT PMIS_ADM.Addresses_ID_SEQ.currval INTO :AddressID FROM dual;  
     
                                  INSERT INTO PMIS_ADM.PersonAddresses (PersonID, AddressID, AddressType)
                                  VALUES (:PersonID, :AddressID, 'ADR_CONTACT');                                  
                                ";
                    }

                    Person oldPerson = GetPerson(person.PersonId, currentUser);

                    changeEvent = GetChangeEvents(changeEventType, oldPerson, person, currentUser);

                }

                if (person.DrivingLicenseCategories.Count > 0)
                {
                    //Add insert statement with value to sql statement
                    foreach (DrivingLicenseCategory drivingLicenseCategory in person.DrivingLicenseCategories)
                    {
                        SQL += " INSERT INTO PMIS_ADM.PersonDrivingLicenseCategories (PersonID, DrivingLicenseCategoryId) VALUES (:PersonID, " + drivingLicenseCategory.DrivingLicenseCategoryId + "); ";
                    }
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonID = new OracleParameter();
                paramPersonID.ParameterName = "PersonID";
                paramPersonID.OracleType = OracleType.Number;

                if (person.PersonId != 0)
                {
                    paramPersonID.Direction = ParameterDirection.Input;
                    paramPersonID.Value = person.PersonId;
                }
                else
                {
                    paramPersonID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonID);

                OracleParameter paramAddressID = new OracleParameter();
                paramAddressID.ParameterName = "AddressID";
                paramAddressID.OracleType = OracleType.Number;

                if (person.ContactAddress.AddressId != 0)
                {
                    paramAddressID.Direction = ParameterDirection.Input;
                    paramAddressID.Value = person.ContactAddress.AddressId;
                }
                else
                {
                    paramAddressID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramAddressID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "FirstName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.FirstName))
                    param.Value = person.FirstName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.LastName))
                    param.Value = person.LastName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "GenderID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.Gender != null)
                    param.Value = person.Gender.GenderId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                if (person.PersonId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (!String.IsNullOrEmpty(person.IdentNumber))
                        param.Value = person.IdentNumber;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                }

                param = new OracleParameter();
                param.ParameterName = "PermCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermCityId != null)
                    param.Value = person.PermCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermAddress))
                    param.Value = person.PermAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresCityId != null)
                    param.Value = person.PresCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresAddress))
                    param.Value = person.PresAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.ContactAddress.CityId != null)
                    param.Value = person.ContactAddress.CityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.ContactAddress.DistrictId != null)
                    param.Value = person.ContactAddress.DistrictId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.ContactAddress.PostCode))
                    param.Value = person.ContactAddress.PostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactAddressText";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.ContactAddress.AddressText))
                    param.Value = person.ContactAddress.AddressText;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.IDCardNumber))
                    param.Value = person.IDCardNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardIssuedBy";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.IDCardIssuedBy))
                    param.Value = person.IDCardIssuedBy;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardIssueDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (person.IDCardIssueDate.HasValue)
                    param.Value = person.IDCardIssueDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HomePhone";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.HomePhone != null)
                    param.Value = person.HomePhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MobilePhone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.MobilePhone))
                    param.Value = person.MobilePhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Email";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.Email))
                    param.Value = person.Email;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HasMilitaryService";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.HasMilitaryService != null)
                    param.Value = person.HasMilitaryService;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryTraining";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.MilitaryTraining != null)
                    param.Value = person.MilitaryTraining;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermDistrictId.HasValue)
                    param.Value = person.PermDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CurrAddrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresDistrictId.HasValue)
                    param.Value = person.PresDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermSecondPostCode))
                    param.Value = person.PermSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresSecondPostCode))
                    param.Value = person.PresSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (person.PersonId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "BirthCityID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    if (person.BirthCityId.HasValue)
                        param.Value = person.BirthCityId.Value;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "BirthCountryID";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (person.BirthCountry != null)
                        param.Value = person.BirthCountry.CountryId;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "BirthCityIfAbroad";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (!string.IsNullOrEmpty(person.BirthCityIfAbroad))
                        param.Value = person.BirthCityIfAbroad;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);
                }
              
                if (person.PersonId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (person.PersonId == 0)
                    person.PersonId = DBCommon.GetInt(paramPersonID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        //Save a particular Person into the DB
        //Use this method when updating a Person from the Add new Potential Applicant process
        public static bool SavePerson_WhenAddingNewPotentialApplicant(Person person, string changeEventType, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"DECLARE
                           ExistingPersonsCnt number;

                        BEGIN
                        
                       ";

                if (person.PersonId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_LS(EGN, FAM, IME, KOD_KZV, KOD_KAT, 
                                  NLD, ACK, KOD_NMA_MR, KOD_NMA_MJ, ADRES, TEL, /* KOD_NAR, */
                                  /* KOD_SPO, */ S_UD, N_UD, D_UD, V_BA, KOD_VIS,
                                  KOD_TIT, KOD_ZVA, Z_Z, Z_ZKOGA, KOD_SPZZ,
                                  /* KOD_JIL, KOD_KRG, */ DOPUSK, KOD_VSO, KOD_VSS,
                                  GDAT, ATES, ATES_TXT, KOD_UVO, PSTG, PSTM,
                                  PSTD, OFZV, KOD_PSO, /*OBR_SHZO, OBR_SHPZO,*/
                                  /* OBR_VU, OBR_VA, OBR_VISHE, OBR_SR, */ V_PODELENIE,
                                  V_KOMANDA, /* ZVR_SNET,  ZOTSR, ZOGVS, */ ZKOD_DLO,
                                  ZKOD_DLS, ZPR_DL, ZPDLO, ZPDLS, ZPPR_DL,
                                  KOD_KATV, ZKURS_DL, ZKURS_MES, ZVVO, ZDATA_OTCH,
                                  OT_UD, GR_UD, OSTG, OSTM, OSTD, TXT,
                                  DOPUSK_DO, Z_ZIZD, KOD_VSV, KOD_VSA,
                                  CurrAddrCityID, CurrAddress, 
                                  PermAddrDistrictID, CurrAddrDistrictID,
                                  PermSecondPostCode, PresSecondPostCode)
                             VALUES (:IdentNumber, :LastName, :FirstName, '" + Config.GetWebSetting("KOD_KZV_PotentialApplicant") + @"', " + Config.GetWebSetting("KOD_KAT_PotentialApplicant") + @" ,
                                  NULL, NULL, :BirthCityID, :PermCityID, :PermAddress, :HomePhone, /* 'Б',*/
                                  /* 'Н', */ NULL, NULL, NULL, SYSDATE, NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  /* 'БЖ', 'А1+', */ NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, /* 'N', 'N', */
                                  /* 'N', 'N', 'Y', 'Y', */ '" + Config.GetWebSetting("V_PODELENIE_PotentialApplicant") + @"',
                                  '" + Config.GetWebSetting("V_KOMANDA_PotentialApplicant") + @"',/* 'N', 'N', 'N' ,*/ NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL,
                                  :PresCityID, :PresAddress, 
                                  :PermAddrDistrictID, :CurrAddrDistrictID,
                                  :PermSecondPostCode, :PresSecondPostCode);

                             SELECT VS_OWNER.VS_LS_PersonID_SEQ.currval INTO :PersonID FROM dual;

                             INSERT INTO PMIS_ADM.Persons (PersonID, GenderID,
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate,
                                IDCardNumber, IDCardIssuedBy, IDCardIssueDate,
                                MobilePhone, Email, MilitaryService, MilitaryTraining,
                                BirthCountryID, BirthCityIfAbroad)
                             VALUES (:PersonID, :GenderID,
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate,
                                :IDCardNumber, :IDCardIssuedBy, :IDCardIssueDate,
                                :MobilePhone, :Email, :HasMilitaryService, :MilitaryTraining,
                                :BirthCountryID, :BirthCityIfAbroad);

                             UPDATE PMIS_ADM.Persons SET Initials = (SELECT PMIS_ADM.CommonFunctions.GetInitials(IME || ' ' || FAM) 
                                                                     FROM VS_OWNER.VS_LS 
                                                                     WHERE VS_OWNER.VS_LS.PersonID = :PersonID)
                             WHERE PMIS_ADM.Persons.PersonID = :PersonID;

                             INSERT INTO PMIS_ADM.Addresses (CityID, DistrictID, PostCode, AddressText)
                             VALUES (:ContactCityID, :ContactDistrictID, :ContactPostCode, :ContactAddressText);

                             SELECT PMIS_ADM.Addresses_ID_SEQ.currval INTO :AddressID FROM dual;  
 
                             INSERT INTO PMIS_ADM.PersonAddresses (PersonID, AddressID, AddressType)
                             VALUES (:PersonID, :AddressID, 'ADR_CONTACT');
                            ";

                    changeEvent = GetChangeEvents(changeEventType, null, person, currentUser);
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_LS SET
                               IME = :FirstName, 
                               FAM = :LastName,
                               ADRES = :PermAddress,
                               TEL = :HomePhone,
                               KOD_NMA_MJ = :PermCityID,
                               CurrAddrCityID = :PresCityID,
                               CurrAddress = :PresAddress,
                               PermAddrDistrictID = :PermAddrDistrictID,
                               CurrAddrDistrictID = :CurrAddrDistrictID,
                               PermSecondPostCode = :PermSecondPostCode, 
                               PresSecondPostCode = :PresSecondPostCode
                               /*,KOD_KZV = '" + Config.GetWebSetting("KOD_KZV_PotentialApplicant") + @"' */
                             WHERE PersonID = :PersonID;

                             SELECT COUNT(*) INTO ExistingPersonsCnt
                             FROM PMIS_ADM.Persons a
                             WHERE a.PersonID = :PersonID;

                             IF (NVL(ExistingPersonsCnt, 0) = 0) THEN
                                INSERT INTO PMIS_ADM.Persons (PersonID)
                                VALUES (:PersonID);
                             END IF;

                             UPDATE PMIS_ADM.Persons SET
                               GenderID = :GenderID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END,
                               IDCardNumber = :IDCardNumber, 
                               IDCardIssuedBy = :IDCardIssuedBy, 
                               IDCardIssueDate = :IDCardIssueDate,
                               MobilePhone = :MobilePhone,
                               Email = :Email,
                               MilitaryService = :HasMilitaryService,
                               MilitaryTraining = :MilitaryTraining
                             WHERE PersonID = :PersonID;

                             UPDATE PMIS_ADM.Persons SET Initials = (SELECT PMIS_ADM.CommonFunctions.GetInitials(IME || ' ' || FAM) 
                                                                     FROM VS_OWNER.VS_LS 
                                                                     WHERE VS_OWNER.VS_LS.PersonID = :PersonID)
                             WHERE PMIS_ADM.Persons.PersonID = :PersonID;

                              DELETE FROM PMIS_ADM.PersonDrivingLicenseCategories 
                             WHERE PersonID = :PersonID;
                            ";

                    if (person.ContactAddress.AddressId > 0)
                    {
                        SQL += @" UPDATE PMIS_ADM.Addresses SET
                                      CityID = :ContactCityID,
                                      DistrictID = :ContactDistrictID,
                                      PostCode = :ContactPostCode,
                                      AddressText = :ContactAddressText
                                  WHERE AddressID = :AddressID;
                                ";
                    }
                    else
                    {
                        SQL += @" INSERT INTO PMIS_ADM.Addresses (CityID, DistrictID, PostCode, AddressText)
                                  VALUES (:ContactCityID, :ContactDistrictID, :ContactPostCode, :ContactAddressText);

                                  SELECT PMIS_ADM.Addresses_ID_SEQ.currval INTO :AddressID FROM dual;  
     
                                  INSERT INTO PMIS_ADM.PersonAddresses (PersonID, AddressID, AddressType)
                                  VALUES (:PersonID, :AddressID, 'ADR_CONTACT');                                  
                                ";
                    }

                    Person oldPerson = GetPerson(person.PersonId, currentUser);

                    changeEvent = GetChangeEvents(changeEventType, oldPerson, person, currentUser);

                }

                if (person.DrivingLicenseCategories.Count > 0)
                {
                    //Add insert statement with value to sql statement
                    foreach (DrivingLicenseCategory drivingLicenseCategory in person.DrivingLicenseCategories)
                    {
                        SQL += " INSERT INTO PMIS_ADM.PersonDrivingLicenseCategories (PersonID, DrivingLicenseCategoryId) VALUES (:PersonID, " + drivingLicenseCategory.DrivingLicenseCategoryId + "); ";
                    }
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonID = new OracleParameter();
                paramPersonID.ParameterName = "PersonID";
                paramPersonID.OracleType = OracleType.Number;

                if (person.PersonId != 0)
                {
                    paramPersonID.Direction = ParameterDirection.Input;
                    paramPersonID.Value = person.PersonId;
                }
                else
                {
                    paramPersonID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonID);

                OracleParameter paramAddressID = new OracleParameter();
                paramAddressID.ParameterName = "AddressID";
                paramAddressID.OracleType = OracleType.Number;

                if (person.ContactAddress.AddressId != 0)
                {
                    paramAddressID.Direction = ParameterDirection.Input;
                    paramAddressID.Value = person.ContactAddress.AddressId;
                }
                else
                {
                    paramAddressID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramAddressID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "FirstName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.FirstName))
                    param.Value = person.FirstName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.LastName))
                    param.Value = person.LastName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "GenderID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.Gender != null)
                    param.Value = person.Gender.GenderId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                if (person.PersonId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (!String.IsNullOrEmpty(person.IdentNumber))
                        param.Value = person.IdentNumber;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                }

                param = new OracleParameter();
                param.ParameterName = "PermCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermCityId != null)
                    param.Value = person.PermCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermAddress))
                    param.Value = person.PermAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresCityId != null)
                    param.Value = person.PresCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresAddress))
                    param.Value = person.PresAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.ContactAddress.CityId != null)
                    param.Value = person.ContactAddress.CityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.ContactAddress.DistrictId != null)
                    param.Value = person.ContactAddress.DistrictId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.ContactAddress.PostCode))
                    param.Value = person.ContactAddress.PostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactAddressText";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.ContactAddress.AddressText))
                    param.Value = person.ContactAddress.AddressText;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.IDCardNumber))
                    param.Value = person.IDCardNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardIssuedBy";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.IDCardIssuedBy))
                    param.Value = person.IDCardIssuedBy;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardIssueDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (person.IDCardIssueDate.HasValue)
                    param.Value = person.IDCardIssueDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HomePhone";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.HomePhone != null)
                    param.Value = person.HomePhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MobilePhone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.MobilePhone))
                    param.Value = person.MobilePhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Email";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.Email))
                    param.Value = person.Email;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HasMilitaryService";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.HasMilitaryService != null)
                    param.Value = person.HasMilitaryService;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryTraining";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.MilitaryTraining != null)
                    param.Value = person.MilitaryTraining;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermDistrictId.HasValue)
                    param.Value = person.PermDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CurrAddrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresDistrictId.HasValue)
                    param.Value = person.PresDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermSecondPostCode))
                    param.Value = person.PermSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresSecondPostCode))
                    param.Value = person.PresSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (person.PersonId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "BirthCityID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    if (person.BirthCityId.HasValue)
                        param.Value = person.BirthCityId.Value;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "BirthCountryID";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (person.BirthCountry != null)
                        param.Value = person.BirthCountry.CountryId;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "BirthCityIfAbroad";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (!string.IsNullOrEmpty(person.BirthCityIfAbroad))
                        param.Value = person.BirthCityIfAbroad;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);
                }

                if (person.PersonId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (person.PersonId == 0)
                    person.PersonId = DBCommon.GetInt(paramPersonID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        //Save a particular Person into the DB
        //Use this method when updating a Person from the Add new Cadet process
        public static bool SavePerson_WhenAddingNewCadet(Person person, string changeEventType, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"DECLARE
                           ExistingPersonsCnt number;

                        BEGIN
                        
                       ";

                if (person.PersonId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_LS(EGN, FAM, IME, KOD_KZV, KOD_KAT, 
                                  NLD, ACK, KOD_NMA_MR, KOD_NMA_MJ, ADRES, TEL, /* KOD_NAR, */
                                  /* KOD_SPO, */ S_UD, N_UD, D_UD, V_BA, KOD_VIS,
                                  KOD_TIT, KOD_ZVA, Z_Z, Z_ZKOGA, KOD_SPZZ,
                                  /* KOD_JIL, KOD_KRG, */ DOPUSK, KOD_VSO, KOD_VSS,
                                  GDAT, ATES, ATES_TXT, KOD_UVO, PSTG, PSTM,
                                  PSTD, OFZV, KOD_PSO, /*OBR_SHZO, OBR_SHPZO,*/
                                  /* OBR_VU, OBR_VA, OBR_VISHE, OBR_SR, */ V_PODELENIE,
                                  V_KOMANDA, /* ZVR_SNET,  ZOTSR, ZOGVS, */ ZKOD_DLO,
                                  ZKOD_DLS, ZPR_DL, ZPDLO, ZPDLS, ZPPR_DL,
                                  KOD_KATV, ZKURS_DL, ZKURS_MES, ZVVO, ZDATA_OTCH,
                                  OT_UD, GR_UD, OSTG, OSTM, OSTD, TXT,
                                  DOPUSK_DO, Z_ZIZD, KOD_VSV, KOD_VSA,
                                  CurrAddrCityID, CurrAddress, 
                                  PermAddrDistrictID, CurrAddrDistrictID,
                                  PermSecondPostCode, PresSecondPostCode)
                             VALUES (:IdentNumber, :LastName, :FirstName, '" + Config.GetWebSetting("KOD_KZV_Cadet") + @"', " + Config.GetWebSetting("KOD_KAT_Cadet") + @" ,
                                  NULL, NULL, :BirthCityID, :PermCityID, :PermAddress, :HomePhone, /* 'Б',*/
                                  /* 'Н', */ NULL, NULL, NULL, SYSDATE, NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  /* 'БЖ', 'А1+', */ NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, /* 'N', 'N', */
                                  /* 'N', 'N', 'Y', 'Y', */ '" + Config.GetWebSetting("V_PODELENIE_Cadet") + @"',
                                  '" + Config.GetWebSetting("V_KOMANDA_Cadet") + @"',/* 'N', 'N', 'N' ,*/ NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL,
                                  :PresCityID, :PresAddress,
                                  :PermAddrDistrictID, :CurrAddrDistrictID,
                                  :PermSecondPostCode, :PresSecondPostCode);

                             SELECT VS_OWNER.VS_LS_PersonID_SEQ.currval INTO :PersonID FROM dual;

                             INSERT INTO PMIS_ADM.Persons (PersonID, GenderID,
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate,
                                IDCardNumber, IDCardIssuedBy, IDCardIssueDate,
                                MobilePhone, Email, MilitaryService,
                                BirthCountryID, BirthCityIfAbroad)
                             VALUES (:PersonID, :GenderID,
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate,
                                :IDCardNumber, :IDCardIssuedBy, :IDCardIssueDate,
                                :MobilePhone, :Email, :HasMilitaryService,
                                :BirthCountryID, :BirthCityIfAbroad);

                             UPDATE PMIS_ADM.Persons SET Initials = (SELECT PMIS_ADM.CommonFunctions.GetInitials(IME || ' ' || FAM) 
                                                                     FROM VS_OWNER.VS_LS 
                                                                     WHERE VS_OWNER.VS_LS.PersonID = :PersonID)
                             WHERE PMIS_ADM.Persons.PersonID = :PersonID;

                             INSERT INTO PMIS_ADM.Addresses (CityID, DistrictID, PostCode, AddressText)
                             VALUES (:ContactCityID, :ContactDistrictID, :ContactPostCode, :ContactAddressText);

                             SELECT PMIS_ADM.Addresses_ID_SEQ.currval INTO :AddressID FROM dual;  
 
                             INSERT INTO PMIS_ADM.PersonAddresses (PersonID, AddressID, AddressType)
                             VALUES (:PersonID, :AddressID, 'ADR_CONTACT');
                            ";

                    changeEvent = GetChangeEvents(changeEventType, null, person, currentUser);
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_LS SET
                               IME = :FirstName, 
                               FAM = :LastName,
                               ADRES = :PermAddress,
                               TEL = :HomePhone,
                               KOD_NMA_MJ = :PermCityID,
                               CurrAddrCityID = :PresCityID,
                               CurrAddress = :PresAddress,
                               PermAddrDistrictID = :PermAddrDistrictID,
                               CurrAddrDistrictID = :CurrAddrDistrictID,
                               PermSecondPostCode = :PermSecondPostCode, 
                               PresSecondPostCode = :PresSecondPostCode
                               /* ,KOD_KZV = '" + Config.GetWebSetting("KOD_KZV_Cadet") + @"' */
                             WHERE PersonID = :PersonID;

                             SELECT COUNT(*) INTO ExistingPersonsCnt
                             FROM PMIS_ADM.Persons a
                             WHERE a.PersonID = :PersonID;

                             IF (NVL(ExistingPersonsCnt, 0) = 0) THEN
                                INSERT INTO PMIS_ADM.Persons (PersonID)
                                VALUES (:PersonID);
                             END IF;

                             UPDATE PMIS_ADM.Persons SET
                               GenderID = :GenderID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END,
                               IDCardNumber = :IDCardNumber, 
                               IDCardIssuedBy = :IDCardIssuedBy, 
                               IDCardIssueDate = :IDCardIssueDate,
                               MobilePhone = :MobilePhone,
                               Email = :Email,
                               MilitaryService = :HasMilitaryService
                             WHERE PersonID = :PersonID;

                             UPDATE PMIS_ADM.Persons SET Initials = (SELECT PMIS_ADM.CommonFunctions.GetInitials(IME || ' ' || FAM) 
                                                                     FROM VS_OWNER.VS_LS 
                                                                     WHERE VS_OWNER.VS_LS.PersonID = :PersonID)
                             WHERE PMIS_ADM.Persons.PersonID = :PersonID;

                              DELETE FROM PMIS_ADM.PersonDrivingLicenseCategories 
                             WHERE PersonID = :PersonID;
                            ";

                    if (person.ContactAddress.AddressId > 0)
                    {
                        SQL += @" UPDATE PMIS_ADM.Addresses SET
                                      CityID = :ContactCityID,
                                      DistrictID = :ContactDistrictID,
                                      PostCode = :ContactPostCode,
                                      AddressText = :ContactAddressText
                                  WHERE AddressID = :AddressID;
                                ";
                    }
                    else
                    {
                        SQL += @" INSERT INTO PMIS_ADM.Addresses (CityID, DistrictID, PostCode, AddressText)
                                  VALUES (:ContactCityID, :ContactDistrictID, :ContactPostCode, :ContactAddressText);

                                  SELECT PMIS_ADM.Addresses_ID_SEQ.currval INTO :AddressID FROM dual;  
     
                                  INSERT INTO PMIS_ADM.PersonAddresses (PersonID, AddressID, AddressType)
                                  VALUES (:PersonID, :AddressID, 'ADR_CONTACT');                                  
                                ";
                    }

                    Person oldPerson = GetPerson(person.PersonId, currentUser);

                    changeEvent = GetChangeEvents(changeEventType, oldPerson, person, currentUser);

                }

                if (person.DrivingLicenseCategories.Count > 0)
                {
                    //Add insert statement with value to sql statement
                    foreach (DrivingLicenseCategory drivingLicenseCategory in person.DrivingLicenseCategories)
                    {
                        SQL += " INSERT INTO PMIS_ADM.PersonDrivingLicenseCategories (PersonID, DrivingLicenseCategoryId) VALUES (:PersonID, " + drivingLicenseCategory.DrivingLicenseCategoryId + "); ";
                    }
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonID = new OracleParameter();
                paramPersonID.ParameterName = "PersonID";
                paramPersonID.OracleType = OracleType.Number;

                if (person.PersonId != 0)
                {
                    paramPersonID.Direction = ParameterDirection.Input;
                    paramPersonID.Value = person.PersonId;
                }
                else
                {
                    paramPersonID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonID);

                OracleParameter paramAddressID = new OracleParameter();
                paramAddressID.ParameterName = "AddressID";
                paramAddressID.OracleType = OracleType.Number;

                if (person.ContactAddress.AddressId != 0)
                {
                    paramAddressID.Direction = ParameterDirection.Input;
                    paramAddressID.Value = person.ContactAddress.AddressId;
                }
                else
                {
                    paramAddressID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramAddressID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "FirstName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.FirstName))
                    param.Value = person.FirstName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.LastName))
                    param.Value = person.LastName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "GenderID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.Gender != null)
                    param.Value = person.Gender.GenderId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                if (person.PersonId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (!String.IsNullOrEmpty(person.IdentNumber))
                        param.Value = person.IdentNumber;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                }

                param = new OracleParameter();
                param.ParameterName = "PermCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermCityId != null)
                    param.Value = person.PermCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermAddress))
                    param.Value = person.PermAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresCityId != null)
                    param.Value = person.PresCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresAddress))
                    param.Value = person.PresAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.ContactAddress.CityId != null)
                    param.Value = person.ContactAddress.CityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.ContactAddress.DistrictId != null)
                    param.Value = person.ContactAddress.DistrictId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.ContactAddress.PostCode))
                    param.Value = person.ContactAddress.PostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ContactAddressText";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.ContactAddress.AddressText))
                    param.Value = person.ContactAddress.AddressText;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.IDCardNumber))
                    param.Value = person.IDCardNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardIssuedBy";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.IDCardIssuedBy))
                    param.Value = person.IDCardIssuedBy;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardIssueDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (person.IDCardIssueDate.HasValue)
                    param.Value = person.IDCardIssueDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HomePhone";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.HomePhone != null)
                    param.Value = person.HomePhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MobilePhone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.MobilePhone))
                    param.Value = person.MobilePhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Email";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.Email))
                    param.Value = person.Email;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HasMilitaryService";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.HasMilitaryService != null)
                    param.Value = person.HasMilitaryService;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermDistrictId.HasValue)
                    param.Value = person.PermDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CurrAddrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresDistrictId.HasValue)
                    param.Value = person.PresDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermSecondPostCode))
                    param.Value = person.PermSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresSecondPostCode))
                    param.Value = person.PresSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
               
                if (person.PersonId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "BirthCityID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    if (person.BirthCityId.HasValue)
                        param.Value = person.BirthCityId.Value;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "BirthCountryID";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (person.BirthCountry != null)
                        param.Value = person.BirthCountry.CountryId;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "BirthCityIfAbroad";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (!string.IsNullOrEmpty(person.BirthCityIfAbroad))
                        param.Value = person.BirthCityIfAbroad;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);
                }

                if (person.PersonId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (person.PersonId == 0)
                    person.PersonId = DBCommon.GetInt(paramPersonID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        //Save a particular Person into the DB
        //Use this method when updating a Person from the Add new Reservist process
        public static bool SavePerson_WhenAddingEditingReservist(Person person, string changeEventType, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"DECLARE
                           ExistingPersonsCnt number;
                        BEGIN
                        
                       ";

                if (person.PersonId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_LS(EGN, FAM, IME, KOD_KZV, KOD_KAT, 
                                  NLD, ACK, KOD_NMA_MR, KOD_NMA_MJ, ADRES, TEL, /* KOD_NAR, */
                                  KOD_SPO, S_UD, N_UD, D_UD, V_BA, KOD_VIS,
                                  KOD_TIT, KOD_ZVA, Z_Z, Z_ZKOGA, KOD_SPZZ,
                                  /* KOD_JIL, KOD_KRG, */ DOPUSK, KOD_VSO, KOD_VSS,
                                  GDAT, ATES, ATES_TXT, KOD_UVO, PSTG, PSTM,
                                  PSTD, OFZV, KOD_PSO, /*OBR_SHZO, OBR_SHPZO,*/
                                  /* OBR_VU, OBR_VA, OBR_VISHE, OBR_SR, */ V_PODELENIE,
                                  V_KOMANDA, /* ZVR_SNET,  ZOTSR, ZOGVS, */ ZKOD_DLO,
                                  ZKOD_DLS, ZPR_DL, ZPDLO, ZPDLS, ZPPR_DL,
                                  KOD_KATV, ZKURS_DL, ZKURS_MES, ZVVO, ZDATA_OTCH,
                                  OT_UD, GR_UD, OSTG, OSTM, OSTD, TXT,
                                  DOPUSK_DO, Z_ZIZD, KOD_VSV, KOD_VSA,
                                  CurrAddrCityID, CurrAddress, 
                                  PermAddrDistrictID, CurrAddrDistrictID,
                                  PermSecondPostCode, PresSecondPostCode)
                             VALUES (:IdentNumber, :LastName, :FirstName, '" + Config.GetWebSetting("KOD_KZV_Reservist") + @"', :MilCategoryId,
                                  NULL, NULL, :BirthCityID, :PermCityID, :PermAddress, :HomePhone, /* 'Б',*/
                                  :MaritalStatusKey, NULL, NULL, NULL, SYSDATE, NULL,
                                  NULL, :MilRankId, NULL, NULL, NULL,
                                  /* 'БЖ', 'А1+', */ NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, /* 'N', 'N', */
                                  /* 'N', 'N', 'Y', 'Y', */ '" + Config.GetWebSetting("V_PODELENIE_Reservist") + @"',
                                  '" + Config.GetWebSetting("V_KOMANDA_Reservist") + @"',/* 'N', 'N', 'N' ,*/ NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL, NULL, NULL,
                                  NULL, NULL, NULL, NULL,
                                  :PresCityID, :PresAddress, 
                                  :PermAddrDistrictID, :CurrAddrDistrictID,
                                  :PermSecondPostCode, :PresSecondPostCode);

                             SELECT VS_OWNER.VS_LS_PersonID_SEQ.currval INTO :PersonID FROM dual;

                             INSERT INTO PMIS_ADM.Persons (PersonID, GenderID,
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate,
                                MobilePhone, BusinessPhone, Email,
                                Initials, IDCardNumber, IDCardIssuedBy, IDCardIssueDate,
                                BirthCountryID, BirthCityIfAbroad, MilitaryService,
                                MilitaryTraining, RecordOfServiceSeries, RecordOfServiceNumber,
                                RecordOfServiceDate, RecordOfServiceCopy,
                                ParentsContact, ChildCount, 
                                SizeClothingID, SizeHatID, SizeShoesID, PersonHeight,
                                IsAbroad, AbroadCountryID, AbroadSince, AbroadPeriod)
                             VALUES (:PersonID, :GenderID,
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate,
                                :MobilePhone, :BusinessPhone, :Email,
                                :Initials, :IDCardNumber, :IDCardIssuedBy, :IDCardIssueDate,
                                :BirthCountryID, :BirthCityIfAbroad, :HasMilitaryService,
                                :MilitaryTraining, :RecordOfServiceSeries, :RecordOfServiceNumber,
                                :RecordOfServiceDate, :RecordOfServiceCopy,
                                :ParentsContact, :ChildCount, 
                                :SizeClothingID, :SizeHatID, :SizeShoesID, :PersonHeight,
                                :IsAbroad, :AbroadCountryID, :AbroadSince, :AbroadPeriod);

                            ";

                    changeEvent = GetChangeEvents_Reservist(changeEventType, null, person, currentUser);
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_LS SET
                               IME = :FirstName, 
                               FAM = :LastName,
                               ADRES = :PermAddress,
                               TEL = :HomePhone,
                               KOD_NMA_MJ = :PermCityID,
                               KOD_KAT = :MilCategoryId,
                               KOD_ZVA = :MilRankId,
                               KOD_NMA_MR = :BirthCityID,
                               KOD_SPO = :MaritalStatusKey,
                               CurrAddrCityID = :PresCityID,
                               CurrAddress = :PresAddress,
                               PermAddrDistrictID = :PermAddrDistrictID,
                               CurrAddrDistrictID = :CurrAddrDistrictID,
                               PermSecondPostCode = :PermSecondPostCode, 
                               PresSecondPostCode = :PresSecondPostCode";

                    if (person.OtherInfo != null)
                    {
                        SQL += ", TXT = :OtherInfo";
                    }
                    
                    SQL += @"
                             WHERE PersonID = :PersonID;

                             SELECT COUNT(*) INTO ExistingPersonsCnt
                             FROM PMIS_ADM.Persons a
                             WHERE a.PersonID = :PersonID;

                             IF (NVL(ExistingPersonsCnt, 0) = 0) THEN
                                INSERT INTO PMIS_ADM.Persons (PersonID)
                                VALUES (:PersonID);
                             END IF;

                             UPDATE PMIS_ADM.Persons SET
                               GenderID = :GenderID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END,
                               MobilePhone = :MobilePhone,
                               BusinessPhone = :BusinessPhone,
                               Email = :Email,
                               Initials = :Initials,
                               IDCardNumber = :IDCardNumber, 
                               IDCardIssuedBy = :IDCardIssuedBy, 
                               IDCardIssueDate = :IDCardIssueDate,
                               BirthCountryID = :BirthCountryID,
                               BirthCityIfAbroad = :BirthCityIfAbroad,
                               MilitaryService = :HasMilitaryService,
                               MilitaryTraining = :MilitaryTraining,
                               RecordOfServiceSeries = :RecordOfServiceSeries,
                               RecordOfServiceNumber = :RecordOfServiceNumber,
                               RecordOfServiceDate = :RecordOfServiceDate,
                               RecordOfServiceCopy = :RecordOfServiceCopy,
                               ParentsContact = :ParentsContact,
                               ChildCount = :ChildCount,
                               SizeClothingID = :SizeClothingID,
                               SizeHatID = :SizeHatID,
                               SizeShoesID = :SizeShoesID,
                               PersonHeight = :PersonHeight,
                               IsAbroad = :IsAbroad,
                               AbroadCountryID = :AbroadCountryID,
                               AbroadSince = :AbroadSince,
                               AbroadPeriod = :AbroadPeriod
                             WHERE PersonID = :PersonID;

                              DELETE FROM PMIS_ADM.PersonDrivingLicenseCategories 
                             WHERE PersonID = :PersonID;
                            ";

                    Person oldPerson = GetPerson(person.PersonId, currentUser);

                    changeEvent = GetChangeEvents_Reservist(changeEventType, oldPerson, person, currentUser);
                }

                if (person.DrivingLicenseCategories.Count > 0)
                {
                    //Add insert statement with value to sql statement
                    foreach (DrivingLicenseCategory drivingLicenseCategory in person.DrivingLicenseCategories)
                    {
                        SQL += " INSERT INTO PMIS_ADM.PersonDrivingLicenseCategories (PersonID, DrivingLicenseCategoryId) VALUES (:PersonID, " + drivingLicenseCategory.DrivingLicenseCategoryId + "); ";
                    }
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonID = new OracleParameter();
                paramPersonID.ParameterName = "PersonID";
                paramPersonID.OracleType = OracleType.Number;

                if (person.PersonId != 0)
                {
                    paramPersonID.Direction = ParameterDirection.Input;
                    paramPersonID.Value = person.PersonId;
                }
                else
                {
                    paramPersonID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "FirstName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.FirstName))
                    param.Value = person.FirstName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.LastName))
                    param.Value = person.LastName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "GenderID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.Gender != null && person.Gender.GenderId > 0)
                    param.Value = person.Gender.GenderId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                if (person.PersonId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    if (!String.IsNullOrEmpty(person.IdentNumber))
                        param.Value = person.IdentNumber;
                    else
                        param.Value = DBNull.Value;
                    cmd.Parameters.Add(param);

                }

                param = new OracleParameter();
                param.ParameterName = "PermCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermCityId != null)
                    param.Value = person.PermCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermAddress))
                    param.Value = person.PermAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresCityId != null)
                    param.Value = person.PresCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresAddress))
                    param.Value = person.PresAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HomePhone";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.HomePhone != null)
                    param.Value = person.HomePhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MobilePhone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.MobilePhone))
                    param.Value = person.MobilePhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Email";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.Email))
                    param.Value = person.Email;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                //New parameters for Reservists
                param = new OracleParameter();
                param.ParameterName = "Initials";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.Initials))
                    param.Value = person.Initials;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilCategoryId";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (person.MilitaryRank != null && person.MilitaryRank.MilitaryCategory != null && 
                    person.MilitaryRank.MilitaryCategory.CategoryId > 0)
                    param.Value = person.MilitaryRank.MilitaryCategory.CategoryId.ToString();
                else
                    param.Value = Config.GetWebSetting("KOD_KAT_For_Reservist_Without_MilitaryRank");
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilRankId";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (person.MilitaryRank != null)
                    param.Value = person.MilitaryRank.MilitaryRankId.ToString();
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.IDCardNumber))
                    param.Value = person.IDCardNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardIssuedBy";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.IDCardIssuedBy))
                    param.Value = person.IDCardIssuedBy;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IDCardIssueDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (person.IDCardIssueDate.HasValue)
                    param.Value = person.IDCardIssueDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BirthCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.BirthCityId.HasValue)
                    param.Value = person.BirthCityId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BirthCountryID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (person.BirthCountry != null)
                    param.Value = person.BirthCountry.CountryId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BirthCityIfAbroad";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.BirthCityIfAbroad))
                    param.Value = person.BirthCityIfAbroad;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HasMilitaryService";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.HasMilitaryService != null)
                    param.Value = person.HasMilitaryService;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryTraining";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.MilitaryTraining.HasValue)
                    param.Value = person.MilitaryTraining.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceSeries";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.RecordOfServiceSeries))
                    param.Value = person.RecordOfServiceSeries;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.RecordOfServiceNumber))
                    param.Value = person.RecordOfServiceNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (person.RecordOfServiceDate.HasValue)
                    param.Value = person.RecordOfServiceDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceCopy";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.RecordOfServiceCopy)
                    param.Value = 1;
                else
                    param.Value = 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermDistrictId.HasValue)
                    param.Value = person.PermDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CurrAddrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresDistrictId.HasValue)
                    param.Value = person.PresDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermSecondPostCode))
                    param.Value = person.PermSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresSecondPostCode))
                    param.Value = person.PresSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BusinessPhone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.BusinessPhone))
                    param.Value = person.BusinessPhone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MaritalStatusKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = person.MaritalStatus.MaritalStatusKey;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ParentsContact";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.ParentsContact))
                    param.Value = person.ParentsContact;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ChildCount";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.ChildCount.HasValue)
                    param.Value = person.ChildCount.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SizeClothingID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.SizeClothingId.HasValue)
                    param.Value = person.SizeClothingId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SizeHatID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.SizeHatId.HasValue)
                    param.Value = person.SizeHatId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SizeShoesID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.SizeShoesId.HasValue)
                    param.Value = person.SizeShoesId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonHeight";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PersonHeight.HasValue)
                    param.Value = person.PersonHeight.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsAbroad";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.IsAbroad)
                    param.Value = 1;
                else
                    param.Value = 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AbroadCountryID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(person.AbroadCountryId))
                    param.Value = person.AbroadCountryId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AbroadSince";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (person.AbroadSince.HasValue)
                    param.Value = person.AbroadSince.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AbroadPeriod";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.AbroadPeriod.HasValue)
                    param.Value = person.AbroadPeriod.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (person.OtherInfo != null)
                {
                    param = new OracleParameter();
                    param.ParameterName = "OtherInfo";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.OtherInfo;
                    cmd.Parameters.Add(param);   
                }

                if (person.PersonId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (person.PersonId == 0)
                    person.PersonId = DBCommon.GetInt(paramPersonID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool SavePerson_WhenImportData(Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                        INSERT INTO VS_OWNER.VS_LS(
                              EGN, 
                              FAM, 
                              IME, 
                              KOD_KZV, 
                              KOD_KAT,                               
                              KOD_NMA_MJ, 
                              ADRES, 
                              V_BA,                               
                              V_PODELENIE,
                              V_KOMANDA, 
                              CurrAddrCityID, 
                              CurrAddress,                               
                              PermSecondPostCode, 
                              PresSecondPostCode,
                              KOD_NMA_MR,
                              CurrAddrDistrictID,
                              PermAddrDistrictID
                         )
                         VALUES (
                              :IdentNumber, 
                              :LastName, 
                              :FirstName, 
                              '" + Config.GetWebSetting("KOD_KZV_Reservist") + @"',
                              " + Config.GetWebSetting("KOD_KAT_For_Reservist_Without_MilitaryRank") + @",                              
                              :PermCityID, 
                              :PermAddress,                               
                              SYSDATE,                               
                              '" + Config.GetWebSetting("V_PODELENIE_Reservist") + @"',
                              '" + Config.GetWebSetting("V_KOMANDA_Reservist") + @"',                      
                              :PresCityID, 
                              :PresAddress,                              
                              :PermSecondPostCode, 
                              :PresSecondPostCode,
                              :BirthCityID,
                              :CurrDistrictID,
                              :PermDistrictID

                         );

                         SELECT VS_OWNER.VS_LS_PersonID_SEQ.currval INTO :PersonID FROM dual;

                         INSERT INTO PMIS_ADM.Persons (
                            PersonID,                           
                            Initials,
                            BirthCityIfAbroad,
                            BirthCountryID,                            
                            CreatedBy, 
                            CreatedDate, 
                            LastModifiedBy, 
                            LastModifiedDate
                            )
                         VALUES (
                            :PersonID,                           
                            :Initials,
                            :BirthCityIfAbroad,
                            :BirthCountryID,     
                            :CreatedBy, 
                            :CreatedDate, 
                            :LastModifiedBy, 
                            :LastModifiedDate
                            );
                        ";

                changeEvent = new ChangeEvent("RES_MilitaryReportPersonDetails_Import", "", null, person, currentUser);// GetChangeEvents("RES_MilitaryReportPersonDetails_Import", null, person, currentUser);
                
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_FirstName", "", person.FirstName, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_LastName", "", person.LastName, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Initials", "", person.Initials, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_PermSecondPostCode", "", person.PermSecondPostCode, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_City", "", person.PermCity.CityName, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_Address", "", person.PermAddress, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_PresSecondPostCode", "", person.PresSecondPostCode, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_City", "", person.PresCity.CityName, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_Address", "", person.PresAddress, currentUser));

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonID = new OracleParameter();
                paramPersonID.ParameterName = "PersonID";
                paramPersonID.OracleType = OracleType.Number;
                paramPersonID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramPersonID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "IdentNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(person.IdentNumber))
                    param.Value = person.IdentNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FirstName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.FirstName))
                    param.Value = person.FirstName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.LastName))
                    param.Value = person.LastName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Initials";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.Initials))
                    param.Value = person.Initials;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermSecondPostCode))
                    param.Value = person.PermSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermCityId != null)
                    param.Value = person.PermCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PermAddress))
                    param.Value = person.PermAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresSecondPostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresSecondPostCode))
                    param.Value = person.PresSecondPostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresCityId != null)
                    param.Value = person.PresCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PresAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.PresAddress))
                    param.Value = person.PresAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                               
                param = new OracleParameter();
                param.ParameterName = "BirthCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.BirthCityId.HasValue)
                    param.Value = person.BirthCityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CurrDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PresDistrictId.HasValue)
                    param.Value = person.PresDistrictId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PermDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.PermDistrictId.HasValue)
                    param.Value = person.PermDistrictId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                             
                param = new OracleParameter();
                param.ParameterName = "BirthCityIfAbroad";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(person.BirthCityIfAbroad))
                    param.Value = person.BirthCityIfAbroad;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BirthCountryID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.BirthCountry != null)
                    param.Value = person.BirthCountry.CountryId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
                person.PersonId = DBCommon.GetInt(paramPersonID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }
      
        //Save a particular Person into the DB
        //Use this method when updating a Person from the Add new Reservist process
        public static bool SavePerson_WhenEditingMilitaryReportTab(Person person, User currentUser, ChangeEvent changeEvent)
        {
            bool result = false;

            string SQL = "";                       

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"   UPDATE PMIS_ADM.Persons SET
                               AdministrationID = :AdministrationID,
                               ClInformationAccLevelBG = :ClInformationAccLevelBG,
                               ClInformationAccLevelBGExpDate = :ClInformationAccLevelBGExpDate,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END                              
                             WHERE PersonID = :PersonID
                            ";

                Person oldPerson = GetPerson(person.PersonId, currentUser);

                if (oldPerson.AdministrationID != person.AdministrationID)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_Administration", oldPerson.Administration != null ? oldPerson.Administration.AdministrationName : "", person.Administration != null ? person.Administration.AdministrationName : "", currentUser));

                if (oldPerson.ClInformationAccLevelBgID != person.ClInformationAccLevelBgID)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_ClInformationAccLevelBg", oldPerson.ClInformationAccLevelBg, person.ClInformationAccLevelBg, currentUser));

                if (oldPerson.ClInformationAccLevelBgExpDate != person.ClInformationAccLevelBgExpDate)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_ClInformationAccLevelBgExpDate", CommonFunctions.FormatDate(oldPerson.ClInformationAccLevelBgExpDate), CommonFunctions.FormatDate(person.ClInformationAccLevelBgExpDate), currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonID = new OracleParameter();
                paramPersonID.ParameterName = "PersonID";
                paramPersonID.OracleType = OracleType.Number;

                if (person.PersonId != 0)
                {
                    paramPersonID.Direction = ParameterDirection.Input;
                    paramPersonID.Value = person.PersonId;
                }
                else
                {
                    paramPersonID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "AdministrationID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.AdministrationID.HasValue)
                    param.Value = person.AdministrationID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ClInformationAccLevelBG";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.ClInformationAccLevelBgID.HasValue)
                    param.Value = person.ClInformationAccLevelBgID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ClInformationAccLevelBGExpDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (person.ClInformationAccLevelBgExpDate.HasValue)
                    param.Value = person.ClInformationAccLevelBgExpDate.Value;
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
            
            return result;
        }

        //Save a particular Person into the DB
        //Use this method when updating a Person's work data from the Edit Reservist process
        public static bool SavePersonWorkData(Person person, User currentUser, Change changeEntry)
        {
            bool result = false;
            ChangeEvent changeEvent = new ChangeEvent("ADM_PersonDetails_Edit", "", null, person, currentUser);

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"   UPDATE PMIS_ADM.Persons SET
                               WorkCompanyID = :WorkCompanyID,
                               WorkPositionNKPDID = :WorkPositionNKPDID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END                              
                             WHERE PersonID = :PersonID
                            ";

                Person oldPerson = GetPerson(person.PersonId, currentUser);

                if (oldPerson.WorkCompanyId != person.WorkCompanyId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_WorkCompany", oldPerson.WorkCompany != null ? oldPerson.WorkCompany.CompanyName : "", person.WorkCompany != null ? person.WorkCompany.CompanyName : "", currentUser));

                if (oldPerson.WorkPositionNKPDId != person.WorkPositionNKPDId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_WorkPositionTitle", oldPerson.WorkPositionNKPD != null ? oldPerson.WorkPositionNKPD.CodeAndNameDisplay : "", person.WorkPositionNKPD != null ? person.WorkPositionNKPD.CodeAndNameDisplay : "", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonID = new OracleParameter();
                paramPersonID.ParameterName = "PersonID";
                paramPersonID.OracleType = OracleType.Number;
                paramPersonID.Direction = ParameterDirection.Input;
                paramPersonID.Value = person.PersonId;

                cmd.Parameters.Add(paramPersonID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "WorkCompanyID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.WorkCompanyId.HasValue)
                    param.Value = person.WorkCompanyId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "WorkPositionNKPDID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (person.WorkPositionNKPDId.HasValue)
                    param.Value = person.WorkPositionNKPDId.Value;
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
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool IsValidIdentityNumber(string identityNumber, User currentUser)
        {
            bool isValid = false;

            long identityNumberInteger = 0;
            long.TryParse(identityNumber, out identityNumberInteger);

            if (identityNumberInteger > 0)
            {
                OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
                conn.Open();

                try
                {
                    string SQL = @"VS_OWNER.CHECK_EGN";

                    OracleCommand cmd = new OracleCommand(SQL, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("cEG", OracleType.VarChar).Value = identityNumber;

                    OracleParameter paramResult = new OracleParameter();
                    paramResult.ParameterName = "identityNumber";
                    paramResult.OracleType = OracleType.VarChar;
                    paramResult.Size = 4000;
                    paramResult.Direction = System.Data.ParameterDirection.ReturnValue;

                    cmd.Parameters.Add(paramResult);

                    cmd.ExecuteNonQuery();

                    isValid = (cmd.Parameters["identityNumber"].Value.ToString() == "1");
                }
                finally
                {
                    conn.Close();
                }
            }

            return isValid;
        }

        public static ChangeEvent GetChangeEvents(string changeEventType, Person oldPerson, Person person, User currentUser)
        {
            ChangeEvent changeEvent = new ChangeEvent(changeEventType, "", null, person, currentUser);
            if (oldPerson == null)
            {
                //Fill object with data
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_FirstName", "",
                             person.FirstName, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_LastName", "",
                             person.LastName, currentUser));

                string genderName = person.Gender == null ? "" : person.Gender.GenderName;
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Gender", "",
                             genderName, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_City", "",
                               person.PermCity.CityName, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_District", "",
                               (person.PermDistrict != null ? person.PermDistrict.DistrictName : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_PermSecondPostCode", "",
                               person.PermSecondPostCode, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_Address", "",
                             person.PermAddress, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_City", "",
                               person.PresCity.CityName, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_District", "",
                               (person.PresDistrict != null ? person.PresDistrict.DistrictName : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_PresSecondPostCode", "",
                               person.PresSecondPostCode, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_Address", "",
                    person.PresAddress, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ContactAdr_City", "",
                               (person.ContactAddress.City != null ? person.ContactAddress.City.CityName : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ContactAdr_District", "",
                               (person.ContactAddress.District != null ? person.ContactAddress.District.DistrictName : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ContactAdr_PresSecondPostCode", "",
                               person.ContactAddress.PostCode, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ContactAdr_Address", "",
                    person.ContactAddress.AddressText, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardNumber", "",
                person.IDCardNumber, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardIssuedBy", "",
                person.IDCardIssuedBy, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardIssueDate", "",
                CommonFunctions.FormatDate(person.IDCardIssueDate), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_HomePhone", "",
                 person.HomePhone.ToString(), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MobilePhone", "",
                person.MobilePhone.ToString(), currentUser));

                string drivingLicences = "";
                for (int i = 0; i <= person.DrivingLicenseCategories.Count - 1; i++)
                {
                    drivingLicences += person.DrivingLicenseCategories[i].DrivingLicenseCategoryName;
                    if (i < person.DrivingLicenseCategories.Count - 1)
                    {
                        drivingLicences += ", ";
                    }
                }
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_DrivingLicence", "",
                                      drivingLicences, currentUser));

                string hasMilitaryServise = "";

                if (person.HasMilitaryService.HasValue)
                {
                    hasMilitaryServise = person.HasMilitaryService == 1 ? "1" : "0";
                }

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_HasMilitaryService", "",
                                    hasMilitaryServise, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MilitaryTraining", "", person.MilitaryTrainingString, currentUser));
            }
            else
            {  //update

                //Fill object with data

                if (oldPerson.FirstName != person.FirstName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_FirstName",
                                 oldPerson.FirstName,
                                 person.FirstName, currentUser));

                if (oldPerson.LastName != person.LastName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_LastName",
                                 oldPerson.LastName,
                                 person.LastName, currentUser));


                string oldGenderName = oldPerson.Gender == null ? "" : oldPerson.Gender.GenderName;
                string newGenderName = person.Gender == null ? "" : person.Gender.GenderName;
                if (oldGenderName != newGenderName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Gender",
                                 oldGenderName,
                                 newGenderName, currentUser));

                if ((oldPerson.PermCity != null ? oldPerson.PermCity.CityName : "") != (person.PermCity != null ? person.PermCity.CityName : ""))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_City",
                                   oldPerson.PermCity == null ? "" : oldPerson.PermCity.CityName,
                                   (person.PermCity != null ? person.PermCity.CityName : ""), currentUser));

                if (oldPerson.PermDistrictId != person.PermDistrictId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_District",
                                   oldPerson.PermDistrictId == null ? "" : oldPerson.PermDistrict.DistrictName,
                                   person.PermDistrictId == null ? "" : person.PermDistrict.DistrictName, currentUser));

                if (oldPerson.PermSecondPostCode != person.PermSecondPostCode)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_PermSecondPostCode",
                                        oldPerson.PermSecondPostCode == null ? "" : oldPerson.PermSecondPostCode,
                                        person.PermSecondPostCode, currentUser));

                if (oldPerson.PermAddress != person.PermAddress)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_Address",
                                        oldPerson.PermAddress == null ? "" : oldPerson.PermAddress,
                                        person.PermAddress, currentUser));

                if ((oldPerson.PresCity != null ? oldPerson.PresCity.CityName : "") != (person.PresCity != null ? person.PresCity.CityName : ""))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_City",
                                   oldPerson.PresCity == null ? "" : oldPerson.PresCity.CityName,
                                   (person.PresCity != null ? person.PresCity.CityName : ""), currentUser));

                if (oldPerson.PresDistrictId != person.PresDistrictId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_District",
                                   oldPerson.PresDistrictId == null ? "" : oldPerson.PresDistrict.DistrictName,
                                   person.PresDistrictId == null ? "" : person.PresDistrict.DistrictName, currentUser));

                if (oldPerson.PresSecondPostCode != person.PresSecondPostCode)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_PresSecondPostCode",
                                        oldPerson.PresSecondPostCode == null ? "" : oldPerson.PresSecondPostCode,
                                        person.PresSecondPostCode, currentUser));

                if (oldPerson.PresAddress != person.PresAddress)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_Address",
                                    oldPerson.PresAddress == null ? "" : oldPerson.PresAddress,
                                    person.PresAddress, currentUser));

                if ((oldPerson.ContactAddress.City != null ? oldPerson.ContactAddress.City.CityName : "") != (person.ContactAddress.City != null ? person.ContactAddress.City.CityName : ""))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ContactAdr_City",
                                   oldPerson.ContactAddress.City == null ? "" : oldPerson.ContactAddress.City.CityName,
                                   (person.ContactAddress.City != null ? person.ContactAddress.City.CityName : ""), currentUser));

                if (oldPerson.ContactAddress.DistrictId != person.ContactAddress.DistrictId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ContactAdr_District",
                                   oldPerson.ContactAddress.DistrictId == null ? "" : oldPerson.ContactAddress.District.DistrictName,
                                   person.ContactAddress.DistrictId == null ? "" : person.ContactAddress.District.DistrictName, currentUser));

                if (oldPerson.ContactAddress.PostCode != person.ContactAddress.PostCode)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ContactAdr_PresSecondPostCode",
                                        oldPerson.ContactAddress.PostCode == null ? "" : oldPerson.ContactAddress.PostCode,
                                        person.ContactAddress.PostCode, currentUser));

                if (oldPerson.ContactAddress.AddressText != person.ContactAddress.AddressText)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ContactAdr_Address",
                                    oldPerson.ContactAddress.AddressText == null ? "" : oldPerson.ContactAddress.AddressText,
                                    person.ContactAddress.AddressText, currentUser));

                if (oldPerson.IDCardNumber != person.IDCardNumber)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardNumber",
                                    oldPerson.IDCardNumber,
                                    person.IDCardNumber, currentUser));

                if (oldPerson.IDCardIssuedBy != person.IDCardIssuedBy)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardIssuedBy",
                                    oldPerson.IDCardIssuedBy,
                                    person.IDCardIssuedBy, currentUser));

                string oldIDCardIssueDate = CommonFunctions.FormatDate(oldPerson.IDCardIssueDate);
                string newIDCardIssueDate = CommonFunctions.FormatDate(person.IDCardIssueDate);
                if (oldIDCardIssueDate != newIDCardIssueDate)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardIssueDate",
                                    oldIDCardIssueDate,
                                    newIDCardIssueDate, currentUser));


                if (oldPerson.HomePhone != person.HomePhone)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_HomePhone",
                                    oldPerson.HomePhone.ToString(),
                                    person.HomePhone.ToString(), currentUser));

                string oldMobilPhone = oldPerson.MobilePhone == null ? "" : oldPerson.MobilePhone.ToString();
                string newMobilPhone = person.MobilePhone == null ? "" : person.MobilePhone.ToString();
                if (oldMobilPhone != newMobilPhone)
                {
                    if (oldPerson.MobilePhone != person.MobilePhone)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MobilePhone",
                                        oldPerson.MobilePhone == null ? "" : oldPerson.MobilePhone.ToString(),
                                        person.MobilePhone.ToString(), currentUser));
                }

                if (oldPerson.Email != person.Email)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Email",
                                    oldPerson.Email.ToString(),
                                    person.Email.ToString(), currentUser));

                string oldDrivingLicences = "";
                for (int i = 0; i <= oldPerson.DrivingLicenseCategories.Count - 1; i++)
                {
                    oldDrivingLicences += oldPerson.DrivingLicenseCategories[i].DrivingLicenseCategoryName;
                    if (i < oldPerson.DrivingLicenseCategories.Count - 1)
                    {
                        oldDrivingLicences += ", ";
                    }
                }

                string drivingLicences = "";
                for (int i = 0; i <= person.DrivingLicenseCategories.Count - 1; i++)
                {
                    drivingLicences += person.DrivingLicenseCategories[i].DrivingLicenseCategoryName;
                    if (i < person.DrivingLicenseCategories.Count - 1)
                    {
                        drivingLicences += ", ";
                    }
                }

                if (oldDrivingLicences.Trim() != drivingLicences.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_DrivingLicence",
                             oldDrivingLicences, drivingLicences, currentUser));


                if (oldPerson.HasMilitaryService != person.HasMilitaryService)
                {
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_HasMilitaryService",
                                     oldPerson.HasMilitaryService == 1 ? "1" : "0",
                                     person.HasMilitaryService == 1 ? "1" : "0", currentUser));
                }

                if (oldPerson.MilitaryTrainingString != person.MilitaryTrainingString)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MilitaryTraining",
                                    oldPerson.MilitaryTrainingString,
                                    person.MilitaryTrainingString, currentUser));
            }

            return changeEvent;
        }

        public static ChangeEvent GetChangeEvents_Reservist(string changeEventType, Person oldPerson, Person person, User currentUser)
        {
            ChangeEvent changeEvent = new ChangeEvent(changeEventType, "", null, person, currentUser);
            if (oldPerson == null)
            {
                //Fill object with data
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_FirstName", "",
                             person.FirstName, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_LastName", "",
                             person.LastName, currentUser));

                string genderName = person.Gender == null ? "" : person.Gender.GenderName;
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Gender", "",
                             genderName, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_City", "",
                               person.PermCity.RegionMunicipalityAndCity, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_District", "",
                               (person.PermDistrict != null ? person.PermDistrict.DistrictName : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_PermSecondPostCode", "",
                               person.PermSecondPostCode, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_Address", "",
                             person.PermAddress, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_City", "",
                               person.PresCity.CityName, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_District", "",
                               (person.PresDistrict != null ? person.PresDistrict.DistrictName : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_PresSecondPostCode", "",
                               person.PresSecondPostCode, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_Address", "",
                    person.PresAddress, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_HomePhone", "",
                 person.HomePhone.ToString(), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MobilePhone", "",
                person.MobilePhone.ToString(), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_BusinessPhone", "",
                person.BusinessPhone.ToString(), currentUser));

                string drivingLicences = "";
                for (int i = 0; i <= person.DrivingLicenseCategories.Count - 1; i++)
                {
                    drivingLicences += person.DrivingLicenseCategories[i].DrivingLicenseCategoryName;
                    if (i < person.DrivingLicenseCategories.Count - 1)
                    {
                        drivingLicences += ", ";
                    }
                }
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_DrivingLicence", "",
                                      drivingLicences, currentUser));


                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MilitaryRank", "",
                (person.MilitaryRank != null ? person.MilitaryRank.ShortName : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Initials", "",
                person.Initials, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardNumber", "",
                person.IDCardNumber, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardIssuedBy", "",
                person.IDCardIssuedBy, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardIssueDate", "",
                CommonFunctions.FormatDate(person.IDCardIssueDate), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_BirthCountry", "",
                (person.BirthCountry != null ? person.BirthCountry.CountryName : ""), currentUser));

                string birthCity = "";

                if (person.BirthCountry != null && person.BirthCountry.IsBulgaria)
                {
                    if (person.BirthCity != null)
                        birthCity = person.BirthCity.RegionMunicipalityAndCity;
                }
                else
                {
                    birthCity = person.BirthCityIfAbroad;
                }

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_BirthCity", "",
                birthCity, currentUser));

                string hasMilitaryServise = "";

                if (person.HasMilitaryService.HasValue)
                {
                    hasMilitaryServise = person.HasMilitaryService == 1 ? "1" : "0";
                }

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_HasMilitaryService", "",
                hasMilitaryServise, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MilitaryTraining", "",
                person.MilitaryTrainingString, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceSeries", "",
                person.RecordOfServiceSeries, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceNumber", "",
                person.RecordOfServiceNumber, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceDate", "",
                CommonFunctions.FormatDate(person.RecordOfServiceDate), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceCopy", "",
                person.RecordOfServiceCopy ? "1" : "0", currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MaritalStatus", "",
                person.MaritalStatus.MaritalStatusName, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ParentsContact", "",
                person.ParentsContact, currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ChildCount", "",
                (person.ChildCount.HasValue ? person.ChildCount.HasValue.ToString() : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_SizeClothing", "",
                (person.SizeClothingId.HasValue ? person.SizeClothing.TableValue : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_SizeHat", "",
                (person.SizeHatId.HasValue ? person.SizeHat.TableValue : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_SizeShoes", "",
                (person.SizeShoesId.HasValue ? person.SizeShoes.TableValue : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PersonHeight", "",
                (person.PersonHeight.HasValue ? person.PersonHeight.HasValue.ToString() : ""), currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IsAbroad",
                    person.IsAbroad ? "1" : "0", "", currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_AbroadCountry",
                    (person.AbroadCountry != null ? person.AbroadCountry.CountryName : ""), "", currentUser));

                string abroadSinceDate = CommonFunctions.FormatDate(person.AbroadSince);
                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_AbroadSince", abroadSinceDate, "", currentUser));

                changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_AbroadPeriod",
                    (person.AbroadPeriod.HasValue ? person.AbroadPeriod.Value.ToString() : ""), "", currentUser));
            }
            else
            {  //update

                //Fill object with data

                if (oldPerson.FirstName != person.FirstName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_FirstName",
                                 oldPerson.FirstName,
                                 person.FirstName, currentUser));

                if (oldPerson.LastName != person.LastName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_LastName",
                                 oldPerson.LastName,
                                 person.LastName, currentUser));


                string oldGenderName = oldPerson.Gender == null ? "" : oldPerson.Gender.GenderName;
                string newGenderName = person.Gender == null ? "" : person.Gender.GenderName;
                if (oldGenderName != newGenderName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Gender",
                                 oldGenderName,
                                 newGenderName, currentUser));

                if ((oldPerson.PermCity != null ? oldPerson.PermCity.CityId : 0) != (person.PermCity != null ? person.PermCity.CityId : 0))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_City",
                                   oldPerson.PermCity == null ? "" : oldPerson.PermCity.RegionMunicipalityAndCity,
                                   (person.PermCity != null ? person.PermCity.RegionMunicipalityAndCity : ""), currentUser));
               
                if (oldPerson.PermDistrictId != person.PermDistrictId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_District",
                                   oldPerson.PermDistrictId == null ? "" : oldPerson.PermDistrict.DistrictName,
                                   person.PermDistrictId == null ? "" : person.PermDistrict.DistrictName, currentUser));

                if (oldPerson.PermSecondPostCode != person.PermSecondPostCode)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_PermSecondPostCode",
                                        oldPerson.PermSecondPostCode == null ? "" : oldPerson.PermSecondPostCode,
                                        person.PermSecondPostCode, currentUser));

                if (oldPerson.PermAddress != person.PermAddress)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PermAdr_Address",
                                        oldPerson.PermAddress == null ? "" : oldPerson.PermAddress,
                                        person.PermAddress, currentUser));

                if ((oldPerson.PresCity != null ? oldPerson.PresCity.CityId : 0) != (person.PresCity != null ? person.PresCity.CityId : 0))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_City",
                                   oldPerson.PresCity == null ? "" : oldPerson.PresCity.RegionMunicipalityAndCity,
                                   (person.PresCity != null ? person.PresCity.RegionMunicipalityAndCity : ""), currentUser));

                if (oldPerson.PresDistrictId != person.PresDistrictId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_District",
                                   oldPerson.PresDistrictId == null ? "" : oldPerson.PresDistrict.DistrictName,
                                   person.PresDistrictId == null ? "" : person.PresDistrict.DistrictName, currentUser));

                if (oldPerson.PresSecondPostCode != person.PresSecondPostCode)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_PresSecondPostCode",
                                        oldPerson.PresSecondPostCode == null ? "" : oldPerson.PresSecondPostCode,
                                        person.PresSecondPostCode, currentUser));

                if (oldPerson.PresAddress != person.PresAddress)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PresAdr_Address",
                                    oldPerson.PresAddress == null ? "" : oldPerson.PresAddress,
                                    person.PresAddress, currentUser));

                if (oldPerson.HomePhone != person.HomePhone)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_HomePhone",
                                    oldPerson.HomePhone.ToString(),
                                    person.HomePhone.ToString(), currentUser));

                string oldMobilPhone = oldPerson.MobilePhone == null ? "" : oldPerson.MobilePhone.ToString();
                string newMobilPhone = person.MobilePhone == null ? "" : person.MobilePhone.ToString();
                if (oldMobilPhone != newMobilPhone)
                {
                    if (oldPerson.MobilePhone != person.MobilePhone)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MobilePhone",
                                        oldPerson.MobilePhone == null ? "" : oldPerson.MobilePhone.ToString(),
                                        person.MobilePhone.ToString(), currentUser));
                }

                string oldBusinessPhone = oldPerson.BusinessPhone == null ? "" : oldPerson.BusinessPhone.ToString();
                string newBusinessPhone = person.BusinessPhone == null ? "" : person.BusinessPhone.ToString();
                if (oldBusinessPhone != newBusinessPhone)
                {
                    if (oldPerson.MobilePhone != person.MobilePhone)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_BusinessPhone",
                                        oldBusinessPhone,
                                        newBusinessPhone, currentUser));
                }

                if (oldPerson.Email != person.Email)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Email",
                                    oldPerson.Email.ToString(),
                                    person.Email.ToString(), currentUser));

                string oldDrivingLicences = "";
                for (int i = 0; i <= oldPerson.DrivingLicenseCategories.Count - 1; i++)
                {
                    oldDrivingLicences += oldPerson.DrivingLicenseCategories[i].DrivingLicenseCategoryName;
                    if (i < oldPerson.DrivingLicenseCategories.Count - 1)
                    {
                        oldDrivingLicences += ", ";
                    }
                }

                string drivingLicences = "";
                for (int i = 0; i <= person.DrivingLicenseCategories.Count - 1; i++)
                {
                    drivingLicences += person.DrivingLicenseCategories[i].DrivingLicenseCategoryName;
                    if (i < person.DrivingLicenseCategories.Count - 1)
                    {
                        drivingLicences += ", ";
                    }
                }

                if (oldDrivingLicences.Trim() != drivingLicences.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_DrivingLicence",
                             oldDrivingLicences, drivingLicences, currentUser));

                string oldMilitaryRankName = oldPerson.MilitaryRank == null ? "" : oldPerson.MilitaryRank.ShortName;
                string newMilitaryRankName = person.MilitaryRank == null ? "" : person.MilitaryRank.ShortName;
                if (oldMilitaryRankName != newMilitaryRankName)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MilitaryRank",
                                 oldMilitaryRankName,
                                 newMilitaryRankName, currentUser));

                if (oldPerson.Initials != person.Initials)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_Initials",
                                    oldPerson.Initials,
                                    person.Initials, currentUser));

                if (oldPerson.IDCardNumber != person.IDCardNumber)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardNumber",
                                    oldPerson.IDCardNumber,
                                    person.IDCardNumber, currentUser));

                if (oldPerson.IDCardIssuedBy != person.IDCardIssuedBy)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardIssuedBy",
                                    oldPerson.IDCardIssuedBy,
                                    person.IDCardIssuedBy, currentUser));

                string oldIDCardIssueDate = CommonFunctions.FormatDate(oldPerson.IDCardIssueDate);
                string newIDCardIssueDate = CommonFunctions.FormatDate(person.IDCardIssueDate);
                if (oldIDCardIssueDate != newIDCardIssueDate)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IDCardIssueDate",
                                    oldIDCardIssueDate,
                                    newIDCardIssueDate, currentUser));

                string oldBirthCountryId = (oldPerson.BirthCountry != null ? oldPerson.BirthCountry.CountryId : "");
                string newBirthCountryId = (person.BirthCountry != null ? person.BirthCountry.CountryId : "");
                if (oldBirthCountryId != newBirthCountryId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_BirthCountry",
                                    (oldPerson.BirthCountry != null ? oldPerson.BirthCountry.CountryName : ""),
                                    (person.BirthCountry != null ? person.BirthCountry.CountryName : ""), currentUser));


                string oldBirthCity = "";
                if (oldPerson.BirthCountry != null && oldPerson.BirthCountry.IsBulgaria)
                {
                    if (oldPerson.BirthCity != null)
                        oldBirthCity = oldPerson.BirthCity.RegionMunicipalityAndCity;
                }
                else
                {
                    oldBirthCity = oldPerson.BirthCityIfAbroad;
                }

                string newBirthCity = "";
                if (person.BirthCountry != null && person.BirthCountry.IsBulgaria)
                {
                    if (person.BirthCity != null)
                        newBirthCity = person.BirthCity.RegionMunicipalityAndCity;
                }
                else
                {
                    newBirthCity = person.BirthCityIfAbroad;
                }

                if (oldBirthCity != newBirthCity)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_BirthCity",
                                    oldBirthCity,
                                    newBirthCity, currentUser));

                if (oldPerson.HasMilitaryService != person.HasMilitaryService)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_HasMilitaryService",
                                     oldPerson.HasMilitaryService == 1 ? "1" : "0",
                                     person.HasMilitaryService == 1 ? "1" : "0", currentUser));

                if (oldPerson.MilitaryTrainingString != person.MilitaryTrainingString)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MilitaryTraining",
                                    oldPerson.MilitaryTrainingString,
                                    person.MilitaryTrainingString, currentUser));

                if (oldPerson.RecordOfServiceSeries != person.RecordOfServiceSeries)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceSeries",
                                    oldPerson.RecordOfServiceSeries,
                                    person.RecordOfServiceSeries, currentUser));

                if (oldPerson.RecordOfServiceNumber != person.RecordOfServiceNumber)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceNumber",
                                    oldPerson.RecordOfServiceNumber,
                                    person.RecordOfServiceNumber, currentUser));

                string oldRecordOfServiceDate = CommonFunctions.FormatDate(oldPerson.RecordOfServiceDate);
                string newRecordOfServiceDate = CommonFunctions.FormatDate(person.RecordOfServiceDate);
                if (oldRecordOfServiceDate != newRecordOfServiceDate)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceDate",
                                    oldRecordOfServiceDate,
                                    newRecordOfServiceDate, currentUser));

                if (oldPerson.RecordOfServiceCopy != person.RecordOfServiceCopy)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_RecordOfServiceCopy",
                                    oldPerson.RecordOfServiceCopy ? "1" : "0",
                                    person.RecordOfServiceCopy ? "1" : "0", currentUser));

                if (oldPerson.MaritalStatus.MaritalStatusKey != person.MaritalStatus.MaritalStatusKey)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_MaritalStatus",
                                    oldPerson.MaritalStatus.MaritalStatusName,
                                    person.MaritalStatus.MaritalStatusName, currentUser));

                if (oldPerson.ParentsContact != person.ParentsContact)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ParentsContact",
                                    oldPerson.ParentsContact,
                                    person.ParentsContact, currentUser));

                if (!CommonFunctions.IsEqualInt(oldPerson.ChildCount, person.ChildCount))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_ChildCount",
                        (oldPerson.ChildCount.HasValue ? oldPerson.ChildCount.Value.ToString() : ""),
                        (person.ChildCount.HasValue ? person.ChildCount.Value.ToString() : ""), currentUser));

                if (!CommonFunctions.IsEqualInt(oldPerson.SizeClothingId, person.SizeClothingId))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_SizeClothing",
                        (oldPerson.SizeClothingId.HasValue ? oldPerson.SizeClothing.TableValue : ""),
                        (person.SizeClothingId.HasValue ? person.SizeClothing.TableValue : ""), currentUser));

                if (!CommonFunctions.IsEqualInt(oldPerson.SizeHatId, person.SizeHatId))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_SizeHat",
                        (oldPerson.SizeHatId.HasValue ? oldPerson.SizeHat.TableValue : ""),
                        (person.SizeHatId.HasValue ? person.SizeHat.TableValue : ""), currentUser));

                if (!CommonFunctions.IsEqualInt(oldPerson.SizeShoesId, person.SizeShoesId))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_SizeShoes",
                        (oldPerson.SizeShoesId.HasValue ? oldPerson.SizeShoes.TableValue : ""),
                        (person.SizeShoesId.HasValue ? person.SizeShoes.TableValue : ""), currentUser));

                if (!CommonFunctions.IsEqualInt(oldPerson.PersonHeight, person.PersonHeight))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_PersonHeight",
                        (oldPerson.PersonHeight.HasValue ? oldPerson.PersonHeight.Value.ToString() : ""),
                        (person.PersonHeight.HasValue ? person.PersonHeight.Value.ToString() : ""), currentUser));

                if (person.OtherInfo != null)
                {
                    if (oldPerson.OtherInfo != person.OtherInfo)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_OtherInfo",
                                        oldPerson.OtherInfo, person.OtherInfo, currentUser));   
                }

                if (oldPerson.IsAbroad != person.IsAbroad)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_IsAbroad",
                        oldPerson.IsAbroad ? "1" : "0",
                        person.IsAbroad ? "1" : "0", currentUser));

                string oldAbroadCountryId = (oldPerson.AbroadCountry != null ? oldPerson.AbroadCountry.CountryId : "");
                string newAbroadCountryId = (person.AbroadCountry != null ? person.AbroadCountry.CountryId : "");
                if (oldAbroadCountryId != newAbroadCountryId)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_AbroadCountry",
                                    (oldPerson.AbroadCountry != null ? oldPerson.AbroadCountry.CountryName : ""),
                                    (person.AbroadCountry != null ? person.AbroadCountry.CountryName : ""), currentUser));

                string oldAbroadSinceDate = CommonFunctions.FormatDate(oldPerson.AbroadSince);
                string newAbroadSinceDate = CommonFunctions.FormatDate(person.AbroadSince);
                if (oldAbroadSinceDate != newAbroadSinceDate)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_AbroadSince",
                                    oldAbroadSinceDate,
                                    newAbroadSinceDate, currentUser));

                if (!CommonFunctions.IsEqualInt(oldPerson.AbroadPeriod, person.AbroadPeriod))
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PersonDetails_AbroadPeriod",
                        (oldPerson.AbroadPeriod.HasValue ? oldPerson.AbroadPeriod.Value.ToString() : ""),
                        (person.AbroadPeriod.HasValue ? person.AbroadPeriod.Value.ToString() : ""), currentUser));
            }

            return changeEvent;

        }

        public static void SetPersonModified(int personId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_ADM.Persons SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE PersonID = :PersonID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        private static Dictionary<string, string> GetReservist_StatusDetailsByPerson(Person person, User currentUser)
        {
            Dictionary<string, string> details = new Dictionary<string, string>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "";

                SQL = @"SELECT a.personid, 
                               e.militarydepartmentname MilitaryDepartment, 
                               d.militaryreportstatusname Status,
                               d.militaryreportstatuskey StatusCode,
                               err.RequestNumber,
                               err.RequestDate, 
                               ra.MilitaryCommandName,
                               ra.MilitaryRankName,
                               ra.Position,
                               ra.MilReportingSpecialityCode,
                               ra.MilReportingSpecialityName
                        FROM vs_owner.vs_ls a
                        INNER JOIN pmis_res.reservists b ON b.personid = a.personid
                        INNER JOIN pmis_res.reservistmilrepstatuses c ON c.reservistid = b.reservistid AND c.iscurrent = 1
                        LEFT OUTER JOIN pmis_res.militaryreportstatuses d ON d.militaryreportstatusid = c.militaryreportstatusid
                        LEFT OUTER JOIN pmis_adm.militarydepartments e ON e.militarydepartmentid = c.sourcemildepartmentid
                        LEFT OUTER JOIN PMIS_RES.ReservistAppointments ra ON d.militaryreportstatuskey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND
                                                                             ra.ReservistID = b.ReservistID AND 
                                                                             ra.IsCurrent = 1
                        LEFT OUTER JOIN PMIS_RES.EquipmentReservistsRequests err ON err.equipmentreservistsrequestid = ra.equipmentreservistsrequestid
                        WHERE a.personid = :PersonID";
          
                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Int32).Value = person.PersonId;

                OracleDataReader dr = cmd.ExecuteReader();
                
                if (dr.Read())
                {
                    details.Add("ВО", dr["MilitaryDepartment"].ToString());
                    details.Add("Състояние", dr["Status"].ToString());
                                       
                    if (dr["StatusCode"].ToString() == "COMPULSORY_RESERVE_MOB_APPOINTMENT")
                    {
                        details.Add("Заявка ", dr["RequestNumber"].ToString() + "/" + CommonFunctions.FormatDate(dr["RequestDate"].ToString()));
                        details.Add("Команда", dr["MilitaryCommandName"].ToString()); 
                        details.Add("На звание", dr["MilitaryRankName"].ToString());  
                        details.Add("Длъжност", dr["Position"].ToString()); 
                        details.Add("Назначен на ВОС", dr["MilReportingSpecialityCode"].ToString() + " " + dr["MilReportingSpecialityName"].ToString());
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return details;
        }


        private static Dictionary<string, string> GetRegularService_StatusDetailsByPerson(Person person, User currentUser)
        {
            Dictionary<string, string> details = new Dictionary<string, string>();
                       
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "";

                SQL = @"SELECT personid,
                               b.vpn || ' ' || b.imees VPN , 
                               d.msst_text_dl Position,
                               kk.kat_ime as Category
                        FROM vs_owner.vs_ls a
                        LEFT OUTER JOIN ukaz_owner.mir b ON b.kod_mir = a.v_podelenie
                        LEFT OUTER JOIN vs_owner.vs_msst d ON a.egn = d.msst_egn
                        LEFT OUTER JOIN vs_owner.klv_kat kk ON d.msst_katkod = kk.kat_kod
                        WHERE a.personid = :PersonID";    

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Int32).Value = person.PersonId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    details.Add(PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit"), dr["VPN"].ToString());
                    details.Add("Длъжност", dr["Position"].ToString());
                    details.Add("Категория", dr["Category"].ToString());
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return details;
        }

        public static PersonStatus GetPersonStatusByPerson(Person person, User currentUser)
        {
            PersonStatus personStatus = null;

            if (person != null)
            {
                personStatus = new PersonStatus();
                personStatus.PersonID = person.PersonId;
                
                //Status
                if (person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_Reserve"))
                    personStatus.Status = Config.GetWebSetting("KOD_KZV_Reserve_Label");
                else if (person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_Applicant"))
                    personStatus.Status = Config.GetWebSetting("KOD_KZV_Applicant_Label");
                else if (person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_Cadet"))
                    personStatus.Status = Config.GetWebSetting("KOD_KZV_Cadet_Label");
                else if (person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_RegularService"))
                    personStatus.Status = Config.GetWebSetting("KOD_KZV_RegularService_Label");
                else if (person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_Reservist"))
                    personStatus.Status = Config.GetWebSetting("KOD_KZV_Reservist_Label");
                else if (person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_Discharged"))
                    personStatus.Status = Config.GetWebSetting("KOD_KZV_Discharged_Label");
                else if (person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_Conscript"))
                    personStatus.Status = Config.GetWebSetting("KOD_KZV_Conscript_Label");
               
                //Details
                if (person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_Reservist") ||
                    person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_Reserve"))
                    personStatus.Details = GetReservist_StatusDetailsByPerson(person, currentUser);
                else if(person.PersonTypeCode == Config.GetWebSetting("KOD_KZV_RegularService"))
                    personStatus.Details = GetRegularService_StatusDetailsByPerson(person, currentUser);
                else
                    personStatus.Details = new Dictionary<string, string>();
            }

            return personStatus;
        }
              
        public static bool TransferToVitosha(int pPersonID, int pMilitaryUnitID, User pCurrentUser, Change pChangeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                            UPDATE VS_OWNER.VS_LS
                                SET V_Podelenie = :MilitaryUnitID,
                                    KOD_KZV = :KOD_KZV
                            WHERE PersonID = :PersonID;
                            
                            UPDATE  PMIS_ADM.Persons
                                SET LastModifiedDate = :LastModifiedDate
                            WHERE PersonID = :PersonID;
                        END;                        
                       ";
                                            
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pPersonID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pMilitaryUnitID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "KOD_KZV";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = Config.GetWebSetting("KOD_KZV_TransferToVitosha");
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastModifiedDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DateTime.Now;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                Person person = GetPerson(pPersonID, pCurrentUser);

                changeEvent = new ChangeEvent("RES_Reservist_MilRep_TransferToVitosha", "", person.MilitaryUnit, person, pCurrentUser);
                pChangeEntry.AddEvent(changeEvent);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static bool RemovePersonRecordOfService(int personId, User currentUser)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"UPDATE PMIS_ADM.Persons SET
                            RecordOfServiceSeries = :RecordOfServiceSeries, 
                            RecordOfServiceNumber = :RecordOfServiceNumber,
                            RecordOfServiceDate = :RecordOfServiceDate, 
                            RecordOfServiceCopy = :RecordOfServiceCopy
                        WHERE PersonID = :PersonID";

                Person oldPerson = GetPerson(personId, currentUser);

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceSeries";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RecordOfServiceCopy";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personId;
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
    }
}
