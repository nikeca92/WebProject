using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Tractor into the system
    public class Tractor : BaseDbObject
    {
        private int tractorId;
        private int technicsId;
        private Technics technics;
        private string regNumber;
        private string inventoryNumber;

        //private int? tractorMakeId;
        //private TractorMake tractorMake;
        //private int? tractorModelId;
        //private TractorModel tractorModel;

        private string tractorMakeName;
        private string tractorModelName;
        
        private int? tractorKindId;
        private GTableItem tractorKind;
        private int? tractorTypeId;
        private GTableItem tractorType;
        private decimal? power;
        private DateTime? firstRegistrationDate;
        private DateTime? lastAnnualTechnicalReviewDate;
        private decimal? mileage;
        
        public int TractorId
        {
            get
            {
                return tractorId;
            }
            set
            {
                tractorId = value;
            }
        }

        public int TechnicsId
        {
            get
            {
                return technicsId;
            }
            set
            {
                technicsId = value;
            }
        }

        public Technics Technics
        {
            get
            {
                if(technics == null)
                   technics = TechnicsUtil.GetTechnics(technicsId, CurrentUser);

                return technics;
            }
            set
            {
                technics = value;
            }
        }

        public string RegNumber
        {
            get
            {
                return regNumber;
            }
            set
            {
                regNumber = value;
            }
        }

        public string InventoryNumber
        {
            get
            {
                return inventoryNumber;
            }
            set
            {
                inventoryNumber = value;
            }
        }

        //public int? TractorMakeId
        //{
        //    get
        //    {
        //        return tractorMakeId;
        //    }
        //    set
        //    {
        //        tractorMakeId = value;
        //    }
        //}

        //public TractorMake TractorMake
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (tractorMake == null && TractorMakeId.HasValue)
        //            tractorMake = TractorMakeUtil.GetTractorMake(TractorMakeId.Value, CurrentUser);

        //        return tractorMake;
        //    }
        //    set
        //    {
        //        tractorMake = value;
        //    }
        //}

        //public int? TractorModelId
        //{
        //    get
        //    {
        //        return tractorModelId;
        //    }
        //    set
        //    {
        //        tractorModelId = value;
        //    }
        //}

        //public TractorModel TractorModel
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (tractorModel == null && TractorModelId.HasValue)
        //            tractorModel = TractorModelUtil.GetTractorModel(TractorModelId.Value, CurrentUser);

        //        return tractorModel;
        //    }
        //    set
        //    {
        //        tractorModel = value;
        //    }
        //}

        public string TractorMakeName
        {
            get
            {
                return tractorMakeName;
            }
            set
            {
                tractorMakeName = value;
            }
        }

        public string TractorModelName
        {
            get
            {
                return tractorModelName;
            }
            set
            {
                tractorModelName = value;
            }
        }

        public int? TractorKindId
        {
            get
            {
                return tractorKindId;
            }
            set
            {
                tractorKindId = value;
            }
        }

        public GTableItem TractorKind
        {
            get
            {
                //Lazy initialization
                if (tractorKind == null && TractorKindId.HasValue)
                    tractorKind = GTableItemUtil.GetTableItem("TractorKind", TractorKindId.Value, ModuleUtil.RES(), CurrentUser);

                return tractorKind;
            }
            set
            {
                tractorKind = value;
            }
        }

        public int? TractorTypeId
        {
            get
            {
                return tractorTypeId;
            }
            set
            {
                tractorTypeId = value;
            }
        }

        public GTableItem TractorType
        {
            get
            {
                //Lazy initialization
                if (tractorType == null && TractorTypeId.HasValue)
                    tractorType = GTableItemUtil.GetTableItem("TractorType", TractorTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return tractorType;
            }
            set
            {
                tractorType = value;
            }
        }

        public decimal? Power
        {
            get
            {
                return power;
            }
            set
            {
                power = value;
            }
        }

        public DateTime? FirstRegistrationDate
        {
            get
            {
                return firstRegistrationDate;
            }
            set
            {
                firstRegistrationDate = value;
            }
        }

        public DateTime? LastAnnualTechnicalReviewDate
        {
            get
            {
                return lastAnnualTechnicalReviewDate;
            }
            set
            {
                lastAnnualTechnicalReviewDate = value;
            }
        }

        public decimal? Mileage
        {
            get
            {
                return mileage;
            }
            set
            {
                mileage = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public Tractor(User user)
            : base(user)
        {

        }
    }

    public class TractorRegNumber
    {
        private string regNumber;
        private DateTime changeDate;

        public string RegNumber
        {
            get
            {
                return regNumber;
            }
            set
            {
                regNumber = value;
            }
        }

        public DateTime ChangeDate
        {
            get
            {
                return changeDate;
            }
            set
            {
                changeDate = value;
            }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class TractorManageFilter
    {
        string regNumber;
        string inventoryNumber;
        string technicsCategoryId;
        string tractorKindId;

        //string tractorMakeId;
        //string tractorModelId;

        string tractorMakeName;
        string tractorModelName;

        string militaryReportStatus;
        string militaryDepartment;
        string ownershipNumber;
        string ownershipName;
        bool isOwnershipAddress;
        string region;
        string municipality;
        string city;
        string district;
        string postCode;
        string address;
        string normativeTechnics;
        string appointmentIsDelivered;
        string readiness;

        int orderBy;
        int pageIdx;

        public string RegNumber
        {
            get
            {
                return regNumber;
            }
            set
            {
                regNumber = value;
            }
        }

        public string InventoryNumber
        {
            get { return inventoryNumber; }
            set { inventoryNumber = value; }
        }

        public string TechnicsCategoryId
        {
            get { return technicsCategoryId; }
            set { technicsCategoryId = value; }
        }

        public string TractorKindId
        {
            get { return tractorKindId; }
            set { tractorKindId = value; }
        }

        //public string TractorMakeId
        //{
        //    get { return tractorMakeId; }
        //    set { tractorMakeId = value; }
        //}

        //public string TractorModelId
        //{
        //    get { return tractorModelId; }
        //    set { tractorModelId = value; }
        //}

        public string TractorMakeName
        {
            get { return tractorMakeName; }
            set { tractorMakeName = value; }
        }

        public string TractorModelName
        {
            get { return tractorModelName; }
            set { tractorModelName = value; }
        }

        public string MilitaryReportStatus
        {
            get { return militaryReportStatus; }
            set { militaryReportStatus = value; }
        }

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }

        public string OwnershipNumber
        {
            get { return ownershipNumber; }
            set { ownershipNumber = value; }
        }

        public string OwnershipName
        {
            get { return ownershipName; }
            set { ownershipName = value; }
        }

        public bool IsOwnershipAddress
        {
            get { return isOwnershipAddress; }
            set { isOwnershipAddress = value; }
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

        public string NormativeTechnics
        {
            get { return normativeTechnics; }
            set { normativeTechnics = value; }
        }

        public string AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }

        public string Readiness
        {
            get { return readiness; }
            set { readiness = value; }
        }

        public int OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                orderBy = value;
            }
        }

        public int PageIdx
        {
            get
            {
                return pageIdx;
            }
            set
            {
                pageIdx = value;
            }
        }
    }

    public class TractorManageBlock
    {
        private int technicsId;
        private int tractorId;
        private string regNumber;
        string inventoryNumber;
        string technicsCategory;
        string tractorKind;

        //string tractorMake;
        //string tractorModel;

        string tractorMakeName;
        string tractorModelName;
        
        string militaryReportStatus;
        string militaryDepartment;
        string ownership;
        string address;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int TractorId
        {
            get { return tractorId; }
            set { tractorId = value; }
        }

        public string RegNumber
        {
            get { return regNumber; }
            set { regNumber = value; }
        }

        public string InventoryNumber
        {
            get { return inventoryNumber; }
            set { inventoryNumber = value; }
        }

        public string TechnicsCategory
        {
            get { return technicsCategory; }
            set { technicsCategory = value; }
        }

        public string TractorKind
        {
            get { return tractorKind; }
            set { tractorKind = value; }
        }

        //public string TractorMake
        //{
        //    get { return tractorMake; }
        //    set { tractorMake = value; }
        //}

        //public string TractorModel
        //{
        //    get { return tractorModel; }
        //    set { tractorModel = value; }
        //}

        public string TractorMakeName
        {
            get { return tractorMakeName; }
            set { tractorMakeName = value; }
        }

        public string TractorModelName
        {
            get { return tractorModelName; }
            set { tractorModelName = value; }
        }

        public string MilitaryReportStatus
        {
            get { return militaryReportStatus; }
            set { militaryReportStatus = value; }
        }

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }

        public string Ownership
        {
            get { return ownership; }
            set { ownership = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string NormativeTechnicsCode
        {
            get { return normativeTechnicsCode; }
            set { normativeTechnicsCode = value; }
        }

        public string NormativeTechnicsName
        {
            get { return normativeTechnicsName; }
            set { normativeTechnicsName = value; }
        }
    }

    public class TractorFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;        
        private int technicReadinessID;        
        private int tractorID;        
        private Tractor tractor;
        private bool appointmentIsDelivered;

        public int FulfilTechnicsRequestID
        {
            get { return fulfilTechnicsRequestID; }
            set { fulfilTechnicsRequestID = value; }
        }

        public int TechnicReadinessID
        {
            get { return technicReadinessID; }
            set { technicReadinessID = value; }
        }

        public string TechnicReadiness
        {
            get
            {
                return ReadinessUtil.ReadinessName(TechnicReadinessID);
            }
        }

        public int TractorID
        {
            get { return tractorID; }
            set { tractorID = value; }
        }

        public Tractor Tractor
        {
            get { return tractor; }
            set { tractor = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class TractorSearchFilter
    {
        string regNumber;
        string inventoryNumber;        
        string technicsCategoryId;        
        string tractorKindId;

        //string tractorMakeId;        
        //string tractorModelId;

        string tractorMakeName;
        string tractorModelName;
        
        string ownershipNumber;
        string ownershipName;
        bool isOwnershipAddress;
        string region;
        string municipality;
        string city;
        string district;
        string postCode;
        string address;
        string normativeTechnics;

        int orderBy;
        int pageIdx;

        public string RegNumber
        {
            get
            {
                return regNumber;
            }
            set
            {
                regNumber = value;
            }
        }

        public string InventoryNumber
        {
            get { return inventoryNumber; }
            set { inventoryNumber = value; }
        }

        public string TechnicsCategoryId
        {
            get { return technicsCategoryId; }
            set { technicsCategoryId = value; }
        }

        public string TractorKindId
        {
            get { return tractorKindId; }
            set { tractorKindId = value; }
        }

        //public string TractorMakeId
        //{
        //    get { return tractorMakeId; }
        //    set { tractorMakeId = value; }
        //}

        //public string TractorModelId
        //{
        //    get { return tractorModelId; }
        //    set { tractorModelId = value; }
        //}

        public string TractorMakeName
        {
            get { return tractorMakeName; }
            set { tractorMakeName = value; }
        }

        public string TractorModelName
        {
            get { return tractorModelName; }
            set { tractorModelName = value; }
        }

        public string OwnershipNumber
        {
            get { return ownershipNumber; }
            set { ownershipNumber = value; }
        }

        public string OwnershipName
        {
            get { return ownershipName; }
            set { ownershipName = value; }
        }

        public bool IsOwnershipAddress
        {
            get { return isOwnershipAddress; }
            set { isOwnershipAddress = value; }
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

        public string NormativeTechnics
        {
            get { return normativeTechnics; }
            set { normativeTechnics = value; }
        }

        public int OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                orderBy = value;
            }
        }

        public int PageIdx
        {
            get
            {
                return pageIdx;
            }
            set
            {
                pageIdx = value;
            }
        }
    }

    public class TractorSearchBlock
    {
        private int technicsId;
        private int tractorId;        
        private string regNumber;
        string inventoryNumber;              
        string technicsCategory;        
        string tractorKind;
        string tractorMake;        
        string tractorModel;
        string normativeTechnicsCode;
        string normativeTechnicsName;
        
        string ownership;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int TractorId
        {
            get { return tractorId; }
            set { tractorId = value; }
        }

        public string RegNumber
        {
            get { return regNumber; }
            set { regNumber = value; }
        }

        public string InventoryNumber
        {
            get { return inventoryNumber; }
            set { inventoryNumber = value; }
        }

        public string TechnicsCategory
        {
            get { return technicsCategory; }
            set { technicsCategory = value; }
        }

        public string TractorKind
        {
            get { return tractorKind; }
            set { tractorKind = value; }
        }

        public string TractorMake
        {
            get { return tractorMake; }
            set { tractorMake = value; }
        }

        public string TractorModel
        {
            get { return tractorModel; }
            set { tractorModel = value; }
        }

        public string Ownership
        {
            get { return ownership; }
            set { ownership = value; }
        }

        public string NormativeTechnicsCode
        {
            get { return normativeTechnicsCode; }
            set { normativeTechnicsCode = value; }
        }

        public string NormativeTechnicsName
        {
            get { return normativeTechnicsName; }
            set { normativeTechnicsName = value; }
        }
    }

    public static class TractorUtil
    {
        //This method creates and returns a Tractor object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static Tractor ExtractTractor(OracleDataReader dr, User currentUser)
        {
            Tractor tractor = new Tractor(currentUser);

            tractor.TractorId = DBCommon.GetInt(dr["TractorID"]);
            tractor.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            tractor.RegNumber = dr["RegNumber"].ToString();
            tractor.InventoryNumber = dr["InventoryNumber"].ToString();

            //tractor.TractorMakeId = (DBCommon.IsInt(dr["TractorMakeID"]) ? DBCommon.GetInt(dr["TractorMakeID"]) : (int?)null);
            //tractor.TractorModelId = (DBCommon.IsInt(dr["TractorModelID"]) ? DBCommon.GetInt(dr["TractorModelID"]) : (int?)null);

            tractor.TractorMakeName = dr["TractorMakeName"].ToString();
            tractor.TractorModelName = dr["TractorModelName"].ToString();
            
            tractor.TractorKindId = (DBCommon.IsInt(dr["TractorKindID"]) ? DBCommon.GetInt(dr["TractorKindID"]) : (int?)null);
            tractor.TractorTypeId = (DBCommon.IsInt(dr["TractorTypeID"]) ? DBCommon.GetInt(dr["TractorTypeID"]) : (int?)null);
            tractor.Power = (DBCommon.IsDecimal(dr["Power"]) ? DBCommon.GetDecimal(dr["Power"]) : (decimal?)null);
            tractor.FirstRegistrationDate = (dr["FirstRegistrationDate"] is DateTime ? (DateTime)dr["FirstRegistrationDate"] : (DateTime?)null);
            tractor.LastAnnualTechnicalReviewDate = (dr["LastAnnualTechnicalReviewDate"] is DateTime ? (DateTime)dr["LastAnnualTechnicalReviewDate"] : (DateTime?)null);
            tractor.Mileage = (DBCommon.IsDecimal(dr["Mileage"]) ? DBCommon.GetDecimal(dr["Mileage"]) : (decimal?)null);

            return tractor;
        }

        //Get a particular object by its ID
        public static Tractor GetTractor(int tractorId, User currentUser)
        {
            Tractor tractor = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.TractorID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.TractorMakeName, a.TractorModelName, a.TractorKindID, a.TractorTypeID,
                                  a.Power,
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate,
                                  a.Mileage
                               FROM PMIS_RES.Tractors a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TractorID = :TractorID 
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TractorID", OracleType.Number).Value = tractorId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tractor = ExtractTractor(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractor;
        }

        //Get a particular object by its ID
        public static Tractor GetTractorByTechnicsId(int technicsId, User currentUser)
        {
            Tractor tractor = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.TractorID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.TractorMakeName, a.TractorModelName, a.TractorKindID, a.TractorTypeID,
                                  a.Power,
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate,
                                  a.Mileage
                               FROM PMIS_RES.Tractors a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID 
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tractor = ExtractTractor(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractor;
        }

        //Get a particular object by its reg number
        public static Tractor GetTractorByRegNumber(string regNumber, User currentUser)
        {
            Tractor tractor = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.TractorID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.TractorMakeName, a.TractorModelName, a.TractorKindID, a.TractorTypeID,
                                  a.Power,
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate,
                                  a.Mileage
                               FROM PMIS_RES.Tractors a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.RegNumber = :RegNumber
                            " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RegNumber", OracleType.VarChar).Value = regNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tractor = ExtractTractor(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractor;
        }

        //Get all Tractor objects
        public static List<Tractor> GetAllTractors(User currentUser)
        {
            List<Tractor> tractors = new List<Tractor>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.TractorID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.TractorMakeName, a.TractorModelName, a.TractorKindID, a.TractorTypeID,
                                  a.Power,
                                  a.FirstRegistrationDate, a.LastAnnualTechnicalReviewDate,
                                  a.Mileage
                               FROM PMIS_RES.Tractors a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tractors.Add(ExtractTractor(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractors;
        }

        public static List<TractorFulfilmentBlock> GetAllTractorFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<TractorFulfilmentBlock> tractors = new List<TractorFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND f.CreatedBy = " + currentUser.UserId.ToString();
                }

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
                        orderBySQL = "b.RegNumber";
                        break;
                    case 2:
                        orderBySQL = "b.TractorMakeName || ' ' || b.TractorModelName";
                        break;
                    case 3:
                        orderBySQL = "e.TableValue";
                        break;
                    case 4:
                        orderBySQL = "a.TechnicReadinessID";
                        break;
                    case 5:
                        orderBySQL = "n.NormativeCode";
                        break;
                    case 6:
                        orderBySQL = "a.AppointmentIsDelivered";
                        break;
                    default:
                        orderBySQL = "c.IME";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.FulfilTechnicsRequestID,
                                      tmp.TechnicsRequestCmdPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.TechnicReadinessID,
                                      tmp.AppointmentIsDelivered,

                                      tmp.TractorID, 
                                      tmp.TechnicsID, 
                                      tmp.RegNumber, 
                                      tmp.InventoryNumber,
                                      tmp.TractorMakeName, 
                                      tmp.TractorModelName, 
                                      tmp.TractorKindID, 
                                      tmp.TractorTypeID,
                                      tmp.Power,
                                      tmp.FirstRegistrationDate,
                                      tmp.LastAnnualTechnicalReviewDate,
                                      tmp.Mileage,
                                      
                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             a.AppointmentIsDelivered,

                                             b.TractorID, 
                                             b.TechnicsID, 
                                             b.RegNumber, 
                                             b.InventoryNumber,
                                             b.TractorMakeName, 
                                             b.TractorModelName, 
                                             b.TractorKindID, 
                                             b.TractorTypeID,
                                             b.Power,
                                             b.FirstRegistrationDate,
                                             b.LastAnnualTechnicalReviewDate,
                                             b.Mileage,
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.Tractors b ON a.TechnicsID = b.TechnicsID";

                                      //LEFT OUTER JOIN PMIS_RES.TractorMakes c ON b.TractorMakeID = c.TractorMakeID
                                      //LEFT OUTER JOIN PMIS_RES.TractorModels d ON b.TractorModelID = d.TractorModelID

                SQL += @"
                                      LEFT OUTER JOIN PMIS_RES.GTable e ON b.TractorKindID = e.TableKey AND e.TableName = 'TractorKind'
                                      INNER JOIN PMIS_RES.Technics f ON b.TechnicsID = f.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON f.NormativeTechnicsID = n.NormativeTechnicsID
                                      WHERE a.TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID AND a.MilitaryDepartmentID = :MilitaryDepartmentID
                                      " + addWhere + @"
                                      ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID
                                    ) tmp " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsRequestCmdPositionID", OracleType.Number).Value = technicsRequestCommandPositionID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TractorFulfilmentBlock block = new TractorFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.TractorID = DBCommon.GetInt(dr["TractorID"]);
                    block.Tractor = ExtractTractor(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    tractors.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractors;
        }

        public static int GetAllTractorFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int tractors = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.Tractors b ON a.TechnicsID = b.TechnicsID
                                      INNER JOIN PMIS_RES.Technics c ON b.TechnicsID = c.TechnicsID
                                      WHERE a.TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID AND a.MilitaryDepartmentID = :MilitaryDepartmentID
                                      " + addWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsRequestCmdPositionID", OracleType.Number).Value = technicsRequestCommandPositionID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tractors = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractors;
        }

        //Save a particular object into the DB
        public static bool SaveTractor(Tractor tractor, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + tractor.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (tractor.TractorId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.Tractors (TechnicsID, RegNumber, 
                                InventoryNumber, TractorMakeName, TractorModelName, 
                                TractorKindID, TractorTypeID,
                                Power,  
                                FirstRegistrationDate, LastAnnualTechnicalReviewDate,
                                Mileage)
                            VALUES (:TechnicsID, :RegNumber, 
                                :InventoryNumber, :TractorMakeName, :TractorModelName, 
                                :TractorKindID, :TractorTypeID,
                                :Power, 
                                :FirstRegistrationDate, :LastAnnualTechnicalReviewDate,
                                :Mileage);

                            SELECT PMIS_RES.Tractors_ID_SEQ.currval INTO :TractorID FROM dual;

                            INSERT INTO PMIS_RES.TractorRegNumbers (TractorID, 
                               RegNumber, ChangeDate)
                            VALUES (:TractorID, 
                               :RegNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_TRACTORS_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_RegNumber", "", tractor.RegNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_InventoryNumber", "", tractor.InventoryNumber, currentUser));

                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorMake", "", tractor.TractorMakeId.HasValue ? tractor.TractorMake.TractorMakeName : "", currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorModel", "", tractor.TractorModelId.HasValue ? tractor.TractorModel.TractorModelName : "", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorMake", "", tractor.TractorMakeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorModel", "", tractor.TractorModelName, currentUser));
                    
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorKind", "", tractor.TractorKindId.HasValue ? tractor.TractorKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorType", "", tractor.TractorTypeId.HasValue ? tractor.TractorType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_Power", "", tractor.Power.HasValue ? tractor.Power.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_FirstRegistrationDate", "", CommonFunctions.FormatDate(tractor.FirstRegistrationDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_LastAnnualTechnicalReviewDate", "", CommonFunctions.FormatDate(tractor.LastAnnualTechnicalReviewDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_Mileage", "", tractor.Mileage.HasValue ? tractor.Mileage.Value.ToString() : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.Tractors SET
                               RegNumber = :RegNumber,
                               InventoryNumber = :InventoryNumber,
                               TractorMakeName = :TractorMakeName,
                               TractorModelName = :TractorModelName,
                               TractorKindID = :TractorKindID,
                               TractorTypeID = :TractorTypeID,
                               Power = :Power,
                               FirstRegistrationDate = :FirstRegistrationDate,
                               LastAnnualTechnicalReviewDate = :LastAnnualTechnicalReviewDate,                               
                               Mileage = :Mileage                               
                             WHERE TractorID = :TractorID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_TRACTORS_Edit", logDescription, null, null, currentUser);

                    Tractor oldTractor = GetTractor(tractor.TractorId, currentUser);

                    if (oldTractor.RegNumber.Trim() != tractor.RegNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_RegNumber", oldTractor.RegNumber, tractor.RegNumber, currentUser));

                    if (oldTractor.InventoryNumber.Trim() != tractor.InventoryNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_InventoryNumber", oldTractor.InventoryNumber, tractor.InventoryNumber, currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldTractor.TractorMakeId, tractor.TractorMakeId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorMake", oldTractor.TractorMakeId.HasValue ? oldTractor.TractorMake.TractorMakeName : "", tractor.TractorMakeId.HasValue ? tractor.TractorMake.TractorMakeName : "", currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldTractor.TractorModelId, tractor.TractorModelId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorModel", oldTractor.TractorModelId.HasValue ? oldTractor.TractorModel.TractorModelName : "", tractor.TractorModelId.HasValue ? tractor.TractorModel.TractorModelName : "", currentUser));

                    if (oldTractor.TractorMakeName.Trim() != tractor.TractorMakeName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorMake", oldTractor.TractorMakeName, tractor.TractorMakeName, currentUser));

                    if (oldTractor.TractorModelName.Trim() != tractor.TractorModelName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorModel", oldTractor.TractorModelName, tractor.TractorModelName, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldTractor.TractorKindId, tractor.TractorKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorKind", oldTractor.TractorKindId.HasValue ? oldTractor.TractorKind.TableValue : "", tractor.TractorKindId.HasValue ? tractor.TractorKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldTractor.TractorTypeId, tractor.TractorTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_TractorType", oldTractor.TractorTypeId.HasValue ? oldTractor.TractorType.TableValue : "", tractor.TractorTypeId.HasValue ? tractor.TractorType.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldTractor.Power, tractor.Power))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_Power", oldTractor.Power.HasValue ? oldTractor.Power.ToString() : "", tractor.Power.HasValue ? tractor.Power.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqual(oldTractor.FirstRegistrationDate, tractor.FirstRegistrationDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_FirstRegistrationDate", CommonFunctions.FormatDate(oldTractor.FirstRegistrationDate), CommonFunctions.FormatDate(tractor.FirstRegistrationDate), currentUser));

                    if (!CommonFunctions.IsEqual(oldTractor.LastAnnualTechnicalReviewDate, tractor.LastAnnualTechnicalReviewDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_LastAnnualTechnicalReviewDate", CommonFunctions.FormatDate(oldTractor.LastAnnualTechnicalReviewDate), CommonFunctions.FormatDate(tractor.LastAnnualTechnicalReviewDate), currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldTractor.Mileage, tractor.Mileage))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_Mileage", oldTractor.Mileage.HasValue ? oldTractor.Mileage.ToString() : "", tractor.Mileage.HasValue ? tractor.Mileage.ToString() : "", currentUser));
                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(tractor.Technics, currentUser, changeEvent);
                tractor.TechnicsId = tractor.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTractorID = new OracleParameter();
                paramTractorID.ParameterName = "TractorID";
                paramTractorID.OracleType = OracleType.Number;

                if (tractor.TractorId != 0)
                {
                    paramTractorID.Direction = ParameterDirection.Input;
                    paramTractorID.Value = tractor.TractorId;
                }
                else
                {
                    paramTractorID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramTractorID);

                OracleParameter param = null;

                if (tractor.TractorId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = tractor.TechnicsId;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "ChangeDate";
                    param.OracleType = OracleType.DateTime;
                    param.Direction = ParameterDirection.Input;
                    param.Value = DateTime.Now;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "RegNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(tractor.RegNumber))
                    param.Value = tractor.RegNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "InventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(tractor.InventoryNumber))
                    param.Value = tractor.InventoryNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "TractorMakeID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (tractor.TractorMakeId.HasValue)
                //    param.Value = tractor.TractorMakeId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "TractorModelID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (tractor.TractorModelId.HasValue)
                //    param.Value = tractor.TractorModelId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TractorMakeName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(tractor.TractorMakeName))
                    param.Value = tractor.TractorMakeName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TractorModelName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(tractor.TractorModelName))
                    param.Value = tractor.TractorModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TractorKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (tractor.TractorKindId.HasValue)
                    param.Value = tractor.TractorKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TractorTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (tractor.TractorTypeId.HasValue)
                    param.Value = tractor.TractorTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Power";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (tractor.Power.HasValue)
                    param.Value = tractor.Power.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FirstRegistrationDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (tractor.FirstRegistrationDate.HasValue)
                    param.Value = tractor.FirstRegistrationDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastAnnualTechnicalReviewDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (tractor.LastAnnualTechnicalReviewDate.HasValue)
                    param.Value = tractor.LastAnnualTechnicalReviewDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Mileage";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (tractor.Mileage.HasValue)
                    param.Value = tractor.Mileage.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (tractor.TractorId == 0)
                    tractor.TractorId = DBCommon.GetInt(paramTractorID.Value);

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

        public static List<TractorManageBlock> GetAllTractorManageBlocks(TractorManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<TractorManageBlock> tractorManageBlocks = new List<TractorManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.RegNumber) LIKE '%' || Upper('" + filter.RegNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TractorKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TractorKindId IN (" + filter.TractorKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.TractorMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.TractorMakeId IN (" + filter.TractorMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.TractorModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.TractorModelId IN (" + filter.TractorModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.TractorMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.TractorMakeName) LIKE '%" + filter.TractorMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.TractorModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.TractorModelName) LIKE '%" + filter.TractorModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryReportStatus))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.TechMilitaryReportStatusID IN ( " + filter.MilitaryReportStatus + ") ";
                }
                else
                {
                    // Ако е избран за статус Всички, да взема всички без Изключени
                    TechMilitaryReportStatus removed = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("REMOVED", currentUser);

                    where += (where == "" ? "" : " AND ") + @" NVL(i.TechMilitaryReportStatusID,0) <> " + removed.TechMilitaryReportStatusId + " ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.UnifiedIdentityCode) LIKE UPPER('%" + filter.OwnershipNumber.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.CompanyName) LIKE UPPER('%" + filter.OwnershipName.Replace("'", "''") + "%') ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(j.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                if (!string.IsNullOrEmpty(filter.NormativeTechnics))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.NormativeTechnicsID IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.NormativeTechnics) + ") ";
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.AppointmentIsDelivered))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (ft.TechnicsID IS NULL OR (ft.TechnicsID IS NOT NULL AND NVL(ft.AppointmentIsDelivered, 0) = " + (filter.AppointmentIsDelivered == ListItems.GetOptionYes().Value ? "1" : "0") + ")) ";
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.Readiness))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (ft.TechnicsID IS NULL OR (ft.TechnicsID IS NOT NULL AND ft.TechnicReadinessID = " + int.Parse(filter.Readiness).ToString() + ")) ";
                }


                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IS NULL OR i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

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
                        orderBySQL = "a.RegNumber";
                        break;
                    case 2:
                        orderBySQL = "a.InventoryNumber";
                        break;
                    case 3:
                        orderBySQL = "c.TechnicsCategoryName";
                        break;
                    case 4:
                        orderBySQL = "d.TableValue";
                        break;
                    case 5:
                        orderBySQL = "a.TractorMakeName";
                        break;
                    case 6:
                        orderBySQL = "a.TractorModelName";
                        break;
                    case 7:
                        orderBySQL = "h.MilitaryDepartmentName";
                        break;
                    case 8:
                        orderBySQL = "g.TechMilitaryReportStatusName";
                        break;
                    case 9:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 10:
                        orderBySQL = "PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address)";
                        break;
                    case 11:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.TractorID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as TractorKind,
                                       a.TractorMakeName,
                                       a.TractorModelName,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Tractors a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.TractorKindId = d.TableKey AND d.TableName = 'TractorKind'";

                                //LEFT OUTER JOIN PMIS_RES.TractorMakes e ON a.TractorMakeId = e.TractorMakeId
                                //LEFT OUTER JOIN PMIS_RES.TractorModels f ON a.TractorModelId = f.TractorModelId                                  

                SQL += @"
                                LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus i ON a.TechnicsID = i.TechnicsID AND i.IsCurrent = 1
                                LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses g ON i.TechMilitaryReportStatusID = g.TechMilitaryReportStatusID
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments h ON i.SourceMilDepartmentID = h.MilitaryDepartmentID
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON b.NormativeTechnicsID = n.NormativeTechnicsID
                                LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON a.TechnicsID = ft.TechnicsID
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", a.TechnicsID
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TractorManageBlock tractorManageBlock = new TractorManageBlock();

                    tractorManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    tractorManageBlock.TractorId = DBCommon.GetInt(dr["TractorID"]);
                    tractorManageBlock.RegNumber = dr["RegNumber"].ToString();
                    tractorManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    tractorManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    tractorManageBlock.TractorKind = dr["TractorKind"].ToString();
                    tractorManageBlock.TractorMakeName = dr["TractorMakeName"].ToString();
                    tractorManageBlock.TractorModelName = dr["TractorModelName"].ToString();
                    tractorManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    tractorManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    tractorManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        tractorManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        tractorManageBlock.Ownership = "";
                    }

                    tractorManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    tractorManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    tractorManageBlocks.Add(tractorManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorManageBlocks;
        }

        public static int GetAllTractorManageBlocksCount(TractorManageFilter filter, User currentUser)
        {
            int tractorManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.RegNumber) LIKE '%' || Upper('" + filter.RegNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TractorKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TractorKindId IN (" + filter.TractorKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.TractorMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.TractorMakeId IN (" + filter.TractorMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.TractorModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.TractorModelId IN (" + filter.TractorModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.TractorMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.TractorMakeName) LIKE '%" + filter.TractorMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.TractorModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.TractorModelName) LIKE '%" + filter.TractorModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryReportStatus))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.TechMilitaryReportStatusID IN ( " + filter.MilitaryReportStatus + ") ";
                }
                else
                {
                    // Ако е избран за статус Всички, да взема всички без Изключени
                    TechMilitaryReportStatus removed = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("REMOVED", currentUser);

                    where += (where == "" ? "" : " AND ") + @" NVL(i.TechMilitaryReportStatusID,0) <> " + removed.TechMilitaryReportStatusId + " ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartment))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.SourceMilDepartmentID IN ( " + filter.MilitaryDepartment + ") ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.UnifiedIdentityCode) LIKE UPPER('%" + filter.OwnershipNumber.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.CompanyName) LIKE UPPER('%" + filter.OwnershipName.Replace("'", "''") + "%') ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(j.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                if (!string.IsNullOrEmpty(filter.NormativeTechnics))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.NormativeTechnicsID IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.NormativeTechnics) + ") ";
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.AppointmentIsDelivered))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (ft.TechnicsID IS NULL OR (ft.TechnicsID IS NOT NULL AND NVL(ft.AppointmentIsDelivered, 0) = " + (filter.AppointmentIsDelivered == ListItems.GetOptionYes().Value ? "1" : "0") + ")) ";
                }

                //Include all other records (e.g. FREE, etc.), if it has an appointment then apply the filter for that records
                if (!string.IsNullOrEmpty(filter.Readiness))
                {
                    where += (where == "" ? "" : " AND ") +
                        @" (ft.TechnicsID IS NULL OR (ft.TechnicsID IS NOT NULL AND ft.TechnicReadinessID = " + int.Parse(filter.Readiness).ToString() + ")) ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" (i.SourceMilDepartmentID IS NULL OR i.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.Tractors a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus i ON a.TechnicsID = i.TechnicsID AND i.IsCurrent = 1
                                LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses g ON i.TechMilitaryReportStatusID = g.TechMilitaryReportStatusID
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments h ON i.SourceMilDepartmentID = h.MilitaryDepartmentID
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON a.TechnicsID = ft.TechnicsID
                                  " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        tractorManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorManageBlocksCnt;
        }

        public static List<TractorSearchBlock> GetAllTractorSearchBlocks(TractorSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<TractorSearchBlock> tractorSearchBlocks = new List<TractorSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.RegNumber) LIKE '%' || Upper('" + filter.RegNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TractorKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TractorKindId IN (" + filter.TractorKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.TractorMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.TractorMakeId IN (" + filter.TractorMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.TractorModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.TractorModelId IN (" + filter.TractorModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.TractorMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.TractorMakeName) LIKE '%" + filter.TractorMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.TractorModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.TractorModelName) LIKE '%" + filter.TractorModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.UnifiedIdentityCode) LIKE UPPER('%" + filter.OwnershipNumber.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.CompanyName) LIKE UPPER('%" + filter.OwnershipName.Replace("'", "''") + "%') ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(j.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                if (!string.IsNullOrEmpty(filter.NormativeTechnics))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.NormativeTechnicsID IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.NormativeTechnics) + ") ";
                }

                where += (where == "" ? "" : " AND ") +
                             @" a.TechnicsID NOT IN ( SELECT TechnicsID 
                                                       FROM PMIS_RES.FulfilTechnicsRequest 
                                                       WHERE TechnicsRequestCmdPositionID = " + requestCommandPositionID + @" AND
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
                        orderBySQL = "a.RegNumber";
                        break;
                    case 2:
                        orderBySQL = "a.InventoryNumber";
                        break;
                    case 3:
                        orderBySQL = "c.TechnicsCategoryName";
                        break;
                    case 4:
                        orderBySQL = "d.TableValue";
                        break;
                    case 5:
                        orderBySQL = "a.TractorMakeName";
                        break;
                    case 6:
                        orderBySQL = "a.TractorModelName";
                        break;
                    case 7:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 8:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.TractorID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as TractorKind,
                                       a.TractorMakeName,
                                       a.TractorModelName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Tractors a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.TractorKindId = d.TableKey AND d.TableName = 'TractorKind'";

                                //LEFT OUTER JOIN PMIS_RES.TractorMakes e ON a.TractorMakeId = e.TractorMakeId
                                //LEFT OUTER JOIN PMIS_RES.TractorModels f ON a.TractorModelId = f.TractorModelId

                SQL += @"
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON b.NormativeTechnicsID = n.NormativeTechnicsID
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", a.TechnicsID
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TractorSearchBlock tractorSearchBlock = new TractorSearchBlock();

                    tractorSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    tractorSearchBlock.TractorId = DBCommon.GetInt(dr["TractorID"]);
                    tractorSearchBlock.RegNumber = dr["RegNumber"].ToString();
                    tractorSearchBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    tractorSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    tractorSearchBlock.TractorKind = dr["TractorKind"].ToString();
                    tractorSearchBlock.TractorMake = dr["TractorMakeName"].ToString();
                    tractorSearchBlock.TractorModel = dr["TractorModelName"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        tractorSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        tractorSearchBlock.Ownership = "";
                    }

                    tractorSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    tractorSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    tractorSearchBlocks.Add(tractorSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorSearchBlocks;
        }

        public static int GetAllTractorSearchBlocksCount(TractorSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int tractorSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_TRACTORS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.RegNumber) LIKE '%' || Upper('" + filter.RegNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.TractorKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.TractorKindId IN (" + filter.TractorKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.TractorMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.TractorMakeId IN (" + filter.TractorMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.TractorModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.TractorModelId IN (" + filter.TractorModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.TractorMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.TractorMakeName) LIKE '%" + filter.TractorMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.TractorModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.TractorModelName) LIKE '%" + filter.TractorModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.UnifiedIdentityCode) LIKE UPPER('%" + filter.OwnershipNumber.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.OwnershipName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(j.CompanyName) LIKE UPPER('%" + filter.OwnershipName.Replace("'", "''") + "%') ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(j.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" j.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 " UPPER(b.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                if (!string.IsNullOrEmpty(filter.NormativeTechnics))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" b.NormativeTechnicsID IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.NormativeTechnics) + ") ";
                }

                where += (where == "" ? "" : " AND ") +
                             @" a.TechnicsID NOT IN ( SELECT TechnicsID 
                                                       FROM PMIS_RES.FulfilTechnicsRequest 
                                                       WHERE TechnicsRequestCmdPositionID = " + requestCommandPositionID + @" AND
                                                             MilitaryDepartmentID = " + militaryDepartmentID + " ) ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.Tractors a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                  " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        tractorSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorSearchBlocksCnt;
        }

        //Change the reg number
        public static bool ChangeRegNumber(int tractorId, string newRegNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            Tractor tractor = GetTractor(tractorId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + tractor.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.Tractors SET
                              RegNumber = :NewRegNumber
                           WHERE TractorID = :TractorID;

                           INSERT INTO PMIS_RES.TractorRegNumbers (TractorID, 
                              RegNumber, ChangeDate)
                           VALUES (:TractorID, 
                              :NewRegNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeRegNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TRACTORS_RegNumber", tractor.RegNumber, newRegNumber, currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "TractorID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = tractorId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NewRegNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = newRegNumber;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ChangeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DateTime.Now;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                TechnicsUtil.SetTechnicsModified(tractor.TechnicsId, currentUser);

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

        //Get all reg number (history) by Tractor with pagination
        public static List<TractorRegNumber> GetAllTractorRegNumbers(int tractorId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<TractorRegNumber> tractorRegNumbers = new List<TractorRegNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.TractorRegNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.RegNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.TractorRegNumberID) as RowNumber
                                               FROM PMIS_RES.TractorRegNumbers a
                                               WHERE a.TractorID = :TractorID
                                               ORDER BY " + orderBySQL + @", a.TractorRegNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TractorID", OracleType.Number).Value = tractorId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TractorRegNumber regNumber = new TractorRegNumber();
                    regNumber.RegNumber = dr["RegNumber"].ToString();
                    regNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    tractorRegNumbers.Add(regNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorRegNumbers;
        }

        //Get all reg number (history) count by Tractor for pagination
        public static int GetAllTractorRegNumbersCount(int tractorId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.TractorRegNumbers a
                               WHERE a.TractorID = :TractorID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TractorID", OracleType.Number).Value = tractorId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        count = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return count;
        }
    }
}