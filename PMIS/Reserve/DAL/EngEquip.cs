using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular EngEquip into the system
    public class EngEquip : BaseDbObject
    {
        private int engEquipId;
        private int technicsId;
        private Technics technics;
        private string regNumber;
        private string inventoryNumber;
        private int? engEquipKindId;
        private GTableItem engEquipKind;
        private int? engEquipTypeId;
        private GTableItem engEquipType;

        //private int? engEquipBaseMakeId;
        //private EngEquipBaseMake engEquipBaseMake;
        //private int? engEquipBaseModelId;
        //private EngEquipBaseModel engEquipBaseModel;

        private string engEquipBaseMakeName;
        private string engEquipBaseModelName;
        
        private int? engEquipBaseKindId;
        private GTableItem engEquipBaseKind;
        private int? engEquipBaseTypeId;
        private GTableItem engEquipBaseType;
        private int? engEquipBaseEngineTypeId;
        private GTableItem engEquipBaseEngineType;
        private DateTime? baseFirstRegDate;
        private decimal? baseMileage;
        private decimal? workingBodyPerformancePerHour;
        private int? engEquipWorkingBodyKindId;
        private GTableItem engEquipWorkingBodyKind;
        private DateTime? workingBodyFirstRegDate;
        private int? engEquipWorkBodyEngineTypeId;
        private GTableItem engEquipWorkBodyEngineType;
        
        
        public int EngEquipId
        {
            get
            {
                return engEquipId;
            }
            set
            {
                engEquipId = value;
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


        public int? EngEquipKindId
        {
            get
            {
                return engEquipKindId;
            }
            set
            {
                engEquipKindId = value;
            }
        }

        public GTableItem EngEquipKind
        {
            get
            {
                //Lazy initialization
                if (engEquipKind == null && EngEquipKindId.HasValue)
                    engEquipKind = GTableItemUtil.GetTableItem("EngEquipKind", EngEquipKindId.Value, ModuleUtil.RES(), CurrentUser);

                return engEquipKind;
            }
            set
            {
                engEquipKind = value;
            }
        }

        public int? EngEquipTypeId
        {
            get
            {
                return engEquipTypeId;
            }
            set
            {
                engEquipTypeId = value;
            }
        }

        public GTableItem EngEquipType
        {
            get
            {
                //Lazy initialization
                if (engEquipType == null && EngEquipTypeId.HasValue)
                    engEquipType = GTableItemUtil.GetTableItem("EngEquipType", EngEquipTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return engEquipType;
            }
            set
            {
                engEquipType = value;
            }
        }

        //public int? EngEquipBaseMakeId
        //{
        //    get
        //    {
        //        return engEquipBaseMakeId;
        //    }
        //    set
        //    {
        //        engEquipBaseMakeId = value;
        //    }
        //}

        //public EngEquipBaseMake EngEquipBaseMake
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (engEquipBaseMake == null && EngEquipBaseMakeId.HasValue)
        //            engEquipBaseMake = EngEquipBaseMakeUtil.GetEngEquipBaseMake(EngEquipBaseMakeId.Value, CurrentUser);

        //        return engEquipBaseMake;
        //    }
        //    set
        //    {
        //        engEquipBaseMake = value;
        //    }
        //}

        //public int? EngEquipBaseModelId
        //{
        //    get
        //    {
        //        return engEquipBaseModelId;
        //    }
        //    set
        //    {
        //        engEquipBaseModelId = value;
        //    }
        //}

        //public EngEquipBaseModel EngEquipBaseModel
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (engEquipBaseModel == null && EngEquipBaseModelId.HasValue)
        //            engEquipBaseModel = EngEquipBaseModelUtil.GetEngEquipBaseModel(EngEquipBaseModelId.Value, CurrentUser);

        //        return engEquipBaseModel;
        //    }
        //    set
        //    {
        //        engEquipBaseModel = value;
        //    }
        //}

        public string EngEquipBaseMakeName
        {
            get
            {
                return engEquipBaseMakeName;
            }
            set
            {
                engEquipBaseMakeName = value;
            }
        }

        public string EngEquipBaseModelName
        {
            get
            {
                return engEquipBaseModelName;
            }
            set
            {
                engEquipBaseModelName = value;
            }
        }

        public int? EngEquipBaseKindId
        {
            get
            {
                return engEquipBaseKindId;
            }
            set
            {
                engEquipBaseKindId = value;
            }
        }

        public GTableItem EngEquipBaseKind
        {
            get
            {
                //Lazy initialization
                if (engEquipBaseKind == null && EngEquipBaseKindId.HasValue)
                    engEquipBaseKind = GTableItemUtil.GetTableItem("EngEquipBaseKind", EngEquipBaseKindId.Value, ModuleUtil.RES(), CurrentUser);

                return engEquipBaseKind;
            }
            set
            {
                engEquipBaseKind = value;
            }
        }

        public int? EngEquipBaseTypeId
        {
            get
            {
                return engEquipBaseTypeId;
            }
            set
            {
                engEquipBaseTypeId = value;
            }
        }

        public GTableItem EngEquipBaseType
        {
            get
            {
                //Lazy initialization
                if (engEquipBaseType == null && EngEquipBaseTypeId.HasValue)
                    engEquipBaseType = GTableItemUtil.GetTableItem("EngEquipBaseType", EngEquipBaseTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return engEquipBaseType;
            }
            set
            {
                engEquipBaseType = value;
            }
        }

        public int? EngEquipBaseEngineTypeId
        {
            get
            {
                return engEquipBaseEngineTypeId;
            }
            set
            {
                engEquipBaseEngineTypeId = value;
            }
        }

        public GTableItem EngEquipBaseEngineType
        {
            get
            {
                //Lazy initialization
                if (engEquipBaseEngineType == null && EngEquipBaseEngineTypeId.HasValue)
                    engEquipBaseEngineType = GTableItemUtil.GetTableItem("EngEquipBaseEngineType", EngEquipBaseEngineTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return engEquipBaseEngineType;
            }
            set
            {
                engEquipBaseEngineType = value;
            }
        }

        public DateTime? BaseFirstRegDate
        {
            get
            {
                return baseFirstRegDate;
            }
            set
            {
                baseFirstRegDate = value;
            }
        }

        public decimal? BaseMileage
        {
            get
            {
                return baseMileage;
            }
            set
            {
                baseMileage = value;
            }
        }

        public decimal? WorkingBodyPerformancePerHour
        {
            get
            {
                return workingBodyPerformancePerHour;
            }
            set
            {
                workingBodyPerformancePerHour = value;
            }
        }

        public int? EngEquipWorkingBodyKindId
        {
            get
            {
                return engEquipWorkingBodyKindId;
            }
            set
            {
                engEquipWorkingBodyKindId = value;
            }
        }

        public GTableItem EngEquipWorkingBodyKind
        {
            get
            {
                //Lazy initialization
                if (engEquipWorkingBodyKind == null && EngEquipWorkingBodyKindId.HasValue)
                    engEquipWorkingBodyKind = GTableItemUtil.GetTableItem("EngEquipWorkingBodyKind", EngEquipWorkingBodyKindId.Value, ModuleUtil.RES(), CurrentUser);

                return engEquipWorkingBodyKind;
            }
            set
            {
                engEquipWorkingBodyKind = value;
            }
        }

        public DateTime? WorkingBodyFirstRegDate
        {
            get
            {
                return workingBodyFirstRegDate;
            }
            set
            {
                workingBodyFirstRegDate = value;
            }
        }

        public int? EngEquipWorkBodyEngineTypeId
        {
            get
            {
                return engEquipWorkBodyEngineTypeId;
            }
            set
            {
                engEquipWorkBodyEngineTypeId = value;
            }
        }

        public GTableItem EngEquipWorkBodyEngineType
        {
            get
            {
                //Lazy initialization
                if (engEquipWorkBodyEngineType == null && EngEquipWorkBodyEngineTypeId.HasValue)
                    engEquipWorkBodyEngineType = GTableItemUtil.GetTableItem("EngEquipWorkBodyEngineType", EngEquipWorkBodyEngineTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return engEquipWorkBodyEngineType;
            }
            set
            {
                engEquipWorkBodyEngineType = value;
            }
        }


        

        public bool CanDelete
        {
            get { return true; }

        }

        public EngEquip(User user)
            : base(user)
        {

        }
    }

    public class EngEquipRegNumber
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
    public class EngEquipManageFilter
    {
        string regNumber;
        string inventoryNumber;
        string technicsCategoryId;
        string engEquipKindId;

        //string engEquipBaseMakeId;
        //string engEquipBaseModelId;

        string engEquipBaseMakeName;
        string engEquipBaseModelName;

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

        public string EngEquipKindId
        {
            get { return engEquipKindId; }
            set { engEquipKindId = value; }
        }

        //public string EngEquipBaseMakeId
        //{
        //    get { return engEquipBaseMakeId; }
        //    set { engEquipBaseMakeId = value; }
        //}

        //public string EngEquipBaseModelId
        //{
        //    get { return engEquipBaseModelId; }
        //    set { engEquipBaseModelId = value; }
        //}

        public string EngEquipBaseMakeName
        {
            get { return engEquipBaseMakeName; }
            set { engEquipBaseMakeName = value; }
        }

        public string EngEquipBaseModelName
        {
            get { return engEquipBaseModelName; }
            set { engEquipBaseModelName = value; }
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

    public class EngEquipManageBlock
    {
        private int technicsId;
        private int engEquipId;
        private string regNumber;
        string inventoryNumber;
        string technicsCategory;
        string engEquipKind;

        //string engEquipBaseMake;
        //string engEquipBaseModel;

        string engEquipBaseMakeName;
        string engEquipBaseModelName;

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

        public int EngEquipId
        {
            get { return engEquipId; }
            set { engEquipId = value; }
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

        public string EngEquipKind
        {
            get { return engEquipKind; }
            set { engEquipKind = value; }
        }

        //public string EngEquipBaseMake
        //{
        //    get { return engEquipBaseMake; }
        //    set { engEquipBaseMake = value; }
        //}

        //public string EngEquipBaseModel
        //{
        //    get { return engEquipBaseModel; }
        //    set { engEquipBaseModel = value; }
        //}

        public string EngEquipBaseMakeName
        {
            get { return engEquipBaseMakeName; }
            set { engEquipBaseMakeName = value; }
        }

        public string EngEquipBaseModelName
        {
            get { return engEquipBaseModelName; }
            set { engEquipBaseModelName = value; }
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

    public class EngEquipFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;        
        private int technicReadinessID;        
        private int engEquipID;        
        private EngEquip engEquip;
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

        public int EngEquipID
        {
            get { return engEquipID; }
            set { engEquipID = value; }
        }

        public EngEquip EngEquip
        {
            get { return engEquip; }
            set { engEquip = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class EngEquipSearchFilter
    {
        string regNumber;
        string inventoryNumber;        
        string technicsCategoryId;        
        string engEquipKindId;

        //string engEquipBaseMakeId;
        //string engEquipBaseModelId;

        string engEquipBaseMakeName;
        string engEquipBaseModelName;

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

        public string EngEquipKindId
        {
            get { return engEquipKindId; }
            set { engEquipKindId = value; }
        }

        //public string EngEquipBaseMakeId
        //{
        //    get { return engEquipBaseMakeId; }
        //    set { engEquipBaseMakeId = value; }
        //}

        //public string EngEquipBaseModelId
        //{
        //    get { return engEquipBaseModelId; }
        //    set { engEquipBaseModelId = value; }
        //}

        public string EngEquipBaseMakeName
        {
            get { return engEquipBaseMakeName; }
            set { engEquipBaseMakeName = value; }
        }

        public string EngEquipBaseModelName
        {
            get { return engEquipBaseModelName; }
            set { engEquipBaseModelName = value; }
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

    public class EngEquipSearchBlock
    {
        private int technicsId;
        private int engEquipId;        
        private string regNumber;
        string inventoryNumber;              
        string technicsCategory;        
        string engEquipKind;        
        string engEquipBaseMake;
        string engEquipBaseModel;
        string ownership;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int EngEquipId
        {
            get { return engEquipId; }
            set { engEquipId = value; }
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

        public string EngEquipKind
        {
            get { return engEquipKind; }
            set { engEquipKind = value; }
        }

        public string EngEquipBaseMake
        {
            get { return engEquipBaseMake; }
            set { engEquipBaseMake = value; }
        }

        public string EngEquipBaseModel
        {
            get { return engEquipBaseModel; }
            set { engEquipBaseModel = value; }
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

    public static class EngEquipUtil
    {
        //This method creates and returns a EngEquip object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static EngEquip ExtractEngEquip(OracleDataReader dr, User currentUser)
        {
            EngEquip engEquip = new EngEquip(currentUser);

            engEquip.EngEquipId = DBCommon.GetInt(dr["EngEquipID"]);
            engEquip.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            engEquip.RegNumber = dr["RegNumber"].ToString();
            engEquip.InventoryNumber = dr["InventoryNumber"].ToString();
            engEquip.EngEquipKindId = (DBCommon.IsInt(dr["EngEquipKindID"]) ? DBCommon.GetInt(dr["EngEquipKindID"]) : (int?)null);
            engEquip.EngEquipTypeId = (DBCommon.IsInt(dr["EngEquipTypeID"]) ? DBCommon.GetInt(dr["EngEquipTypeID"]) : (int?)null);

            //engEquip.EngEquipBaseMakeId = (DBCommon.IsInt(dr["EngEquipBaseMakeID"]) ? DBCommon.GetInt(dr["EngEquipBaseMakeID"]) : (int?)null);
            //engEquip.EngEquipBaseModelId = (DBCommon.IsInt(dr["EngEquipBaseModelID"]) ? DBCommon.GetInt(dr["EngEquipBaseModelID"]) : (int?)null);

            engEquip.EngEquipBaseMakeName = dr["EngEquipBaseMakeName"].ToString();
            engEquip.EngEquipBaseModelName = dr["EngEquipBaseModelName"].ToString();

            engEquip.EngEquipBaseKindId = (DBCommon.IsInt(dr["EngEquipBaseKindID"]) ? DBCommon.GetInt(dr["EngEquipBaseKindID"]) : (int?)null);
            engEquip.EngEquipBaseTypeId = (DBCommon.IsInt(dr["EngEquipBaseTypeID"]) ? DBCommon.GetInt(dr["EngEquipBaseTypeID"]) : (int?)null);
            engEquip.EngEquipBaseEngineTypeId = (DBCommon.IsInt(dr["EngEquipBaseEngineTypeID"]) ? DBCommon.GetInt(dr["EngEquipBaseEngineTypeID"]) : (int?)null);
            engEquip.BaseFirstRegDate = (dr["BaseFirstRegDate"] is DateTime ? (DateTime)dr["BaseFirstRegDate"] : (DateTime?)null);
            engEquip.BaseMileage = (DBCommon.IsDecimal(dr["BaseMileage"]) ? DBCommon.GetDecimal(dr["BaseMileage"]) : (decimal?)null);
            engEquip.WorkingBodyPerformancePerHour = (DBCommon.IsDecimal(dr["WorkingBodyPerformancePerHour"]) ? DBCommon.GetDecimal(dr["WorkingBodyPerformancePerHour"]) : (decimal?)null);
            engEquip.EngEquipWorkingBodyKindId = (DBCommon.IsInt(dr["EngEquipWorkingBodyKindID"]) ? DBCommon.GetInt(dr["EngEquipWorkingBodyKindID"]) : (int?)null);
            engEquip.WorkingBodyFirstRegDate = (dr["WorkingBodyFirstRegDate"] is DateTime ? (DateTime)dr["WorkingBodyFirstRegDate"] : (DateTime?)null);
            engEquip.EngEquipWorkBodyEngineTypeId = (DBCommon.IsInt(dr["EngEquipWorkBodyEngineTypeID"]) ? DBCommon.GetInt(dr["EngEquipWorkBodyEngineTypeID"]) : (int?)null);

            return engEquip;
        }

        //Get a particular object by its ID
        public static EngEquip GetEngEquip(int engEquipId, User currentUser)
        {
            EngEquip engEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.EngEquipID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.EngEquipKindID,
                                  a.EngEquipTypeID,
                                  a.EngEquipBaseMakeName,
                                  a.EngEquipBaseModelName,
                                  a.EngEquipBaseKindID,
                                  a.EngEquipBaseTypeID,
                                  a.EngEquipBaseEngineTypeID,
                                  a.BaseFirstRegDate,
                                  a.BaseMileage,
                                  a.WorkingBodyPerformancePerHour,
                                  a.EngEquipWorkingBodyKindID,
                                  a.WorkingBodyFirstRegDate,
                                  a.EngEquipWorkBodyEngineTypeID
                               FROM PMIS_RES.EngEquipment a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.EngEquipID = :EngEquipID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EngEquipID", OracleType.Number).Value = engEquipId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    engEquip = ExtractEngEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquip;
        }

        //Get a particular object by its ID
        public static EngEquip GetEngEquipByTechnicsId(int technicsId, User currentUser)
        {
            EngEquip engEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.EngEquipID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.EngEquipKindID,
                                  a.EngEquipTypeID,
                                  a.EngEquipBaseMakeName,
                                  a.EngEquipBaseModelName,
                                  a.EngEquipBaseKindID,
                                  a.EngEquipBaseTypeID,
                                  a.EngEquipBaseEngineTypeID,
                                  a.BaseFirstRegDate,
                                  a.BaseMileage,
                                  a.WorkingBodyPerformancePerHour,
                                  a.EngEquipWorkingBodyKindID,
                                  a.WorkingBodyFirstRegDate,
                                  a.EngEquipWorkBodyEngineTypeID
                               FROM PMIS_RES.EngEquipment a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    engEquip = ExtractEngEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquip;
        }

        //Get a particular object by its reg number
        public static EngEquip GetEngEquipByRegNumber(string regNumber, User currentUser)
        {
            EngEquip engEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.EngEquipID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.EngEquipKindID,
                                  a.EngEquipTypeID,
                                  a.EngEquipBaseMakeName,
                                  a.EngEquipBaseModelName,
                                  a.EngEquipBaseKindID,
                                  a.EngEquipBaseTypeID,
                                  a.EngEquipBaseEngineTypeID,
                                  a.BaseFirstRegDate,
                                  a.BaseMileage,
                                  a.WorkingBodyPerformancePerHour,
                                  a.EngEquipWorkingBodyKindID,
                                  a.WorkingBodyFirstRegDate,
                                  a.EngEquipWorkBodyEngineTypeID
                               FROM PMIS_RES.EngEquipment a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.RegNumber = :RegNumber
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RegNumber", OracleType.VarChar).Value = regNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    engEquip = ExtractEngEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquip;
        }

        //Get all EngEquip objects
        public static List<EngEquip> GetAllEngEquips(User currentUser)
        {
            List<EngEquip> engEquips = new List<EngEquip>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.EngEquipID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.EngEquipKindID,
                                  a.EngEquipTypeID,
                                  a.EngEquipBaseMakeName,
                                  a.EngEquipBaseModelName,
                                  a.EngEquipBaseKindID,
                                  a.EngEquipBaseTypeID,
                                  a.EngEquipBaseEngineTypeID,
                                  a.BaseFirstRegDate,
                                  a.BaseMileage,
                                  a.WorkingBodyPerformancePerHour,
                                  a.EngEquipWorkingBodyKindID,
                                  a.WorkingBodyFirstRegDate,
                                  a.EngEquipWorkBodyEngineTypeID
                               FROM PMIS_RES.EngEquipment a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    engEquips.Add(ExtractEngEquip(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquips;
        }

        public static List<EngEquipFulfilmentBlock> GetAllEngEquipFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<EngEquipFulfilmentBlock> engEquips = new List<EngEquipFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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
                        orderBySQL = "b.EngEquipBaseMakeName || ' ' || b.EngEquipBaseModelName";
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

                                      tmp.EngEquipID, 
                                      tmp.TechnicsID, 
                                      tmp.RegNumber, 
                                      tmp.InventoryNumber,
                                      tmp.EngEquipKindID,
                                      tmp.EngEquipTypeID,
                                      tmp.EngEquipBaseMakeName,
                                      tmp.EngEquipBaseModelName,
                                      tmp.EngEquipBaseKindID,
                                      tmp.EngEquipBaseTypeID,
                                      tmp.EngEquipBaseEngineTypeID,
                                      tmp.BaseFirstRegDate,
                                      tmp.BaseMileage,
                                      tmp.WorkingBodyPerformancePerHour,
                                      tmp.EngEquipWorkingBodyKindID,
                                      tmp.WorkingBodyFirstRegDate,
                                      tmp.EngEquipWorkBodyEngineTypeID,
                                      
                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             a.AppointmentIsDelivered,

                                             b.EngEquipID, 
                                             b.TechnicsID, 
                                             b.RegNumber, 
                                             b.InventoryNumber,
                                             b.EngEquipKindID,
                                             b.EngEquipTypeID,
                                             b.EngEquipBaseMakeName,
                                             b.EngEquipBaseModelName,
                                             b.EngEquipBaseKindID,
                                             b.EngEquipBaseTypeID,
                                             b.EngEquipBaseEngineTypeID,
                                             b.BaseFirstRegDate,
                                             b.BaseMileage,
                                             b.WorkingBodyPerformancePerHour,
                                             b.EngEquipWorkingBodyKindID,
                                             b.WorkingBodyFirstRegDate,
                                             b.EngEquipWorkBodyEngineTypeID,
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.EngEquipment b ON a.TechnicsID = b.TechnicsID";

                                      //LEFT OUTER JOIN PMIS_RES.EngEquipBaseMakes c ON b.EngEquipBaseMakeID = c.EngEquipBaseMakeID
                                      //LEFT OUTER JOIN PMIS_RES.EngEquipBaseModels d ON b.EngEquipBaseModelID = d.EngEquipBaseModelID

                SQL += @"
                                      LEFT OUTER JOIN PMIS_RES.GTable e ON b.EngEquipKindID = e.TableKey AND e.TableName = 'EngEquipKind'
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
                    EngEquipFulfilmentBlock block = new EngEquipFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.EngEquipID = DBCommon.GetInt(dr["EngEquipID"]);
                    block.EngEquip = ExtractEngEquip(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    engEquips.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquips;
        }

        public static int GetAllEngEquipFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int engEquips = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.EngEquipment b ON a.TechnicsID = b.TechnicsID
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
                    engEquips = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquips;
        }

        //Save a particular object into the DB
        public static bool SaveEngEquip(EngEquip engEquip, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + engEquip.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (engEquip.EngEquipId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.EngEquipment (TechnicsID, RegNumber, 
                                InventoryNumber, EngEquipKindID, EngEquipTypeID,
                                EngEquipBaseMakeName, EngEquipBaseModelName,
                                EngEquipBaseKindID, EngEquipBaseTypeID,
                                EngEquipBaseEngineTypeID, BaseFirstRegDate,
                                BaseMileage,
                                WorkingBodyPerformancePerHour, EngEquipWorkingBodyKindID,
                                WorkingBodyFirstRegDate, EngEquipWorkBodyEngineTypeID)
                            VALUES (:TechnicsID, :RegNumber, 
                                :InventoryNumber, :EngEquipKindID, :EngEquipTypeID,
                                :EngEquipBaseMakeName, :EngEquipBaseModelName,
                                :EngEquipBaseKindID, :EngEquipBaseTypeID,
                                :EngEquipBaseEngineTypeID, :BaseFirstRegDate,
                                :BaseMileage,
                                :WorkingBodyPerformancePerHour, :EngEquipWorkingBodyKindID,
                                :WorkingBodyFirstRegDate, :EngEquipWorkBodyEngineTypeID);

                            SELECT PMIS_RES.EngEquipment_ID_SEQ.currval INTO :EngEquipID FROM dual;

                            INSERT INTO PMIS_RES.EngEquipmentRegNumbers (EngEquipID, 
                               RegNumber, ChangeDate)
                            VALUES (:EngEquipID, 
                               :RegNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_ENG_EQUIP_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_RegNumber", "", engEquip.RegNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_InventoryNumber", "", engEquip.InventoryNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipKind", "", engEquip.EngEquipKindId.HasValue ? engEquip.EngEquipKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipType", "", engEquip.EngEquipTypeId.HasValue ? engEquip.EngEquipType.TableValue : "", currentUser));

                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseMake", "", engEquip.EngEquipBaseMakeId.HasValue ? engEquip.EngEquipBaseMake.EngEquipBaseMakeName : "", currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseModel", "", engEquip.EngEquipBaseModelId.HasValue ? engEquip.EngEquipBaseModel.EngEquipBaseModelName : "", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseMake", "", engEquip.EngEquipBaseMakeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseModel", "", engEquip.EngEquipBaseModelName, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseKind", "", engEquip.EngEquipBaseKindId.HasValue ? engEquip.EngEquipBaseKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseType", "", engEquip.EngEquipBaseTypeId.HasValue ? engEquip.EngEquipBaseType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseEngineType", "", engEquip.EngEquipBaseEngineTypeId.HasValue ? engEquip.EngEquipBaseEngineType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_BaseFirstRegDate", "", CommonFunctions.FormatDate(engEquip.BaseFirstRegDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_BaseMileage", "", engEquip.BaseMileage.HasValue ? engEquip.BaseMileage.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_WorkingBodyPerformancePerHour", "", engEquip.WorkingBodyPerformancePerHour.HasValue ? engEquip.WorkingBodyPerformancePerHour.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipWorkingBodyKind", "", engEquip.EngEquipWorkingBodyKindId.HasValue ? engEquip.EngEquipWorkingBodyKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_WorkingBodyFirstRegDate", "", CommonFunctions.FormatDate(engEquip.WorkingBodyFirstRegDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipWorkBodyEngineType", "", engEquip.EngEquipWorkBodyEngineTypeId.HasValue ? engEquip.EngEquipWorkBodyEngineType.TableValue : "", currentUser));                    
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.EngEquipment SET
                               RegNumber = :RegNumber,
                               InventoryNumber = :InventoryNumber,
                               EngEquipKindID = :EngEquipKindID,
                               EngEquipTypeID = :EngEquipTypeID,
                               EngEquipBaseMakeName = :EngEquipBaseMakeName,
                               EngEquipBaseModelName = :EngEquipBaseModelName,
                               EngEquipBaseKindID = :EngEquipBaseKindID,
                               EngEquipBaseTypeID = :EngEquipBaseTypeID,
                               EngEquipBaseEngineTypeID = :EngEquipBaseEngineTypeID,
                               BaseFirstRegDate = :BaseFirstRegDate,
                               BaseMileage = :BaseMileage,
                               WorkingBodyPerformancePerHour = :WorkingBodyPerformancePerHour,
                               EngEquipWorkingBodyKindID = :EngEquipWorkingBodyKindID,
                               WorkingBodyFirstRegDate = :WorkingBodyFirstRegDate,
                               EngEquipWorkBodyEngineTypeID = :EngEquipWorkBodyEngineTypeID
                             WHERE EngEquipID = :EngEquipID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_ENG_EQUIP_Edit", logDescription, null, null, currentUser);

                    EngEquip oldEngEquip = GetEngEquip(engEquip.EngEquipId, currentUser);
                    
                    if (oldEngEquip.RegNumber.Trim() != engEquip.RegNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_RegNumber", oldEngEquip.RegNumber, engEquip.RegNumber, currentUser));

                    if (oldEngEquip.InventoryNumber.Trim() != engEquip.InventoryNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_InventoryNumber", oldEngEquip.InventoryNumber, engEquip.InventoryNumber, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipKindId, engEquip.EngEquipKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipKind", oldEngEquip.EngEquipKindId.HasValue ? oldEngEquip.EngEquipKind.TableValue : "", engEquip.EngEquipKindId.HasValue ? engEquip.EngEquipKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipTypeId, engEquip.EngEquipTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipType", oldEngEquip.EngEquipTypeId.HasValue ? oldEngEquip.EngEquipType.TableValue : "", engEquip.EngEquipTypeId.HasValue ? engEquip.EngEquipType.TableValue : "", currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipBaseMakeId, engEquip.EngEquipBaseMakeId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseMake", oldEngEquip.EngEquipBaseMakeId.HasValue ? oldEngEquip.EngEquipBaseMake.EngEquipBaseMakeName : "", engEquip.EngEquipBaseMakeId.HasValue ? engEquip.EngEquipBaseMake.EngEquipBaseMakeName : "", currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipBaseModelId, engEquip.EngEquipBaseModelId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseModel", oldEngEquip.EngEquipBaseModelId.HasValue ? oldEngEquip.EngEquipBaseModel.EngEquipBaseModelName : "", engEquip.EngEquipBaseModelId.HasValue ? engEquip.EngEquipBaseModel.EngEquipBaseModelName : "", currentUser));

                    if (oldEngEquip.EngEquipBaseMakeName != engEquip.EngEquipBaseMakeName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseMake", oldEngEquip.EngEquipBaseMakeName, engEquip.EngEquipBaseMakeName, currentUser));

                    if (oldEngEquip.EngEquipBaseModelName != engEquip.EngEquipBaseModelName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseModel", oldEngEquip.EngEquipBaseModelName, engEquip.EngEquipBaseModelName, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipBaseKindId, engEquip.EngEquipBaseKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseKind", oldEngEquip.EngEquipBaseKindId.HasValue ? oldEngEquip.EngEquipBaseKind.TableValue : "", engEquip.EngEquipBaseKindId.HasValue ? engEquip.EngEquipBaseKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipBaseTypeId, engEquip.EngEquipBaseTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseType", oldEngEquip.EngEquipBaseTypeId.HasValue ? oldEngEquip.EngEquipBaseType.TableValue : "", engEquip.EngEquipBaseTypeId.HasValue ? engEquip.EngEquipBaseType.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipBaseEngineTypeId, engEquip.EngEquipBaseEngineTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipBaseEngineType", oldEngEquip.EngEquipBaseEngineTypeId.HasValue ? oldEngEquip.EngEquipBaseEngineType.TableValue : "", engEquip.EngEquipBaseEngineTypeId.HasValue ? engEquip.EngEquipBaseEngineType.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqual(oldEngEquip.BaseFirstRegDate, engEquip.BaseFirstRegDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_BaseFirstRegDate", CommonFunctions.FormatDate(oldEngEquip.BaseFirstRegDate), CommonFunctions.FormatDate(engEquip.BaseFirstRegDate), currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldEngEquip.BaseMileage, engEquip.BaseMileage))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_BaseMileage", oldEngEquip.BaseMileage.HasValue ? oldEngEquip.BaseMileage.ToString() : "", engEquip.BaseMileage.HasValue ? engEquip.BaseMileage.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldEngEquip.WorkingBodyPerformancePerHour, engEquip.WorkingBodyPerformancePerHour))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_WorkingBodyPerformancePerHour", oldEngEquip.WorkingBodyPerformancePerHour.HasValue ? oldEngEquip.WorkingBodyPerformancePerHour.ToString() : "", engEquip.WorkingBodyPerformancePerHour.HasValue ? engEquip.WorkingBodyPerformancePerHour.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipWorkingBodyKindId, engEquip.EngEquipWorkingBodyKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipWorkingBodyKind", oldEngEquip.EngEquipWorkingBodyKindId.HasValue ? oldEngEquip.EngEquipWorkingBodyKind.TableValue : "", engEquip.EngEquipWorkingBodyKindId.HasValue ? engEquip.EngEquipWorkingBodyKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqual(oldEngEquip.WorkingBodyFirstRegDate, engEquip.WorkingBodyFirstRegDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_WorkingBodyFirstRegDate", CommonFunctions.FormatDate(oldEngEquip.WorkingBodyFirstRegDate), CommonFunctions.FormatDate(engEquip.WorkingBodyFirstRegDate), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldEngEquip.EngEquipWorkBodyEngineTypeId, engEquip.EngEquipWorkBodyEngineTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_EngEquipWorkBodyEngineType", oldEngEquip.EngEquipWorkBodyEngineTypeId.HasValue ? oldEngEquip.EngEquipWorkBodyEngineType.TableValue : "", engEquip.EngEquipWorkBodyEngineTypeId.HasValue ? engEquip.EngEquipWorkBodyEngineType.TableValue : "", currentUser));
                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(engEquip.Technics, currentUser, changeEvent);
                engEquip.TechnicsId = engEquip.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramEngEquipID = new OracleParameter();
                paramEngEquipID.ParameterName = "EngEquipID";
                paramEngEquipID.OracleType = OracleType.Number;

                if (engEquip.EngEquipId != 0)
                {
                    paramEngEquipID.Direction = ParameterDirection.Input;
                    paramEngEquipID.Value = engEquip.EngEquipId;
                }
                else
                {
                    paramEngEquipID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramEngEquipID);

                OracleParameter param = null;

                if (engEquip.EngEquipId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = engEquip.TechnicsId;
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
                if (!string.IsNullOrEmpty(engEquip.RegNumber))
                    param.Value = engEquip.RegNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "InventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(engEquip.InventoryNumber))
                    param.Value = engEquip.InventoryNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.EngEquipKindId.HasValue)
                    param.Value = engEquip.EngEquipKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.EngEquipTypeId.HasValue)
                    param.Value = engEquip.EngEquipTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "EngEquipBaseMakeID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (engEquip.EngEquipBaseMakeId.HasValue)
                //    param.Value = engEquip.EngEquipBaseMakeId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "EngEquipBaseModelID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (engEquip.EngEquipBaseModelId.HasValue)
                //    param.Value = engEquip.EngEquipBaseModelId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipBaseMakeName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(engEquip.EngEquipBaseMakeName))
                    param.Value = engEquip.EngEquipBaseMakeName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipBaseModelName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(engEquip.EngEquipBaseModelName))
                    param.Value = engEquip.EngEquipBaseModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipBaseKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.EngEquipBaseKindId.HasValue)
                    param.Value = engEquip.EngEquipBaseKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipBaseTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.EngEquipBaseTypeId.HasValue)
                    param.Value = engEquip.EngEquipBaseTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipBaseEngineTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.EngEquipBaseEngineTypeId.HasValue)
                    param.Value = engEquip.EngEquipBaseEngineTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BaseFirstRegDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (engEquip.BaseFirstRegDate.HasValue)
                    param.Value = engEquip.BaseFirstRegDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BaseMileage";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.BaseMileage.HasValue)
                    param.Value = engEquip.BaseMileage.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "WorkingBodyPerformancePerHour";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.WorkingBodyPerformancePerHour.HasValue)
                    param.Value = engEquip.WorkingBodyPerformancePerHour.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipWorkingBodyKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.EngEquipWorkingBodyKindId.HasValue)
                    param.Value = engEquip.EngEquipWorkingBodyKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "WorkingBodyFirstRegDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (engEquip.WorkingBodyFirstRegDate.HasValue)
                    param.Value = engEquip.WorkingBodyFirstRegDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipWorkBodyEngineTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (engEquip.EngEquipWorkBodyEngineTypeId.HasValue)
                    param.Value = engEquip.EngEquipWorkBodyEngineTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (engEquip.EngEquipId == 0)
                    engEquip.EngEquipId = DBCommon.GetInt(paramEngEquipID.Value);

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

        public static List<EngEquipManageBlock> GetAllEngEquipManageBlocks(EngEquipManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<EngEquipManageBlock> engEquipManageBlocks = new List<EngEquipManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.EngEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EngEquipKindId IN (" + filter.EngEquipKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.EngEquipBaseMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.EngEquipBaseMakeId IN (" + filter.EngEquipBaseMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.EngEquipBaseModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.EngEquipBaseModelId IN (" + filter.EngEquipBaseModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.EngEquipBaseMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.EngEquipBaseMakeName) LIKE '%" + filter.EngEquipBaseMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.EngEquipBaseModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.EngEquipBaseModelName) LIKE '%" + filter.EngEquipBaseModelName.ToUpper().Replace("'", "''") + "%' ";
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
                        orderBySQL = "a.EngEquipBaseMakeName";
                        break;
                    case 6:
                        orderBySQL = "a.EngEquipBaseModelName";
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
                                       a.EngEquipID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as EngEquipKind,
                                       a.EngEquipBaseMakeName,
                                       a.EngEquipBaseModelName,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.EngEquipment a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.EngEquipKindId = d.TableKey AND d.TableName = 'EngEquipKind'";

                                //LEFT OUTER JOIN PMIS_RES.EngEquipBaseMakes e ON a.EngEquipBaseMakeId = e.EngEquipBaseMakeId
                                //LEFT OUTER JOIN PMIS_RES.EngEquipBaseModels f ON a.EngEquipBaseModelId = f.EngEquipBaseModelId

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
                    EngEquipManageBlock engEquipManageBlock = new EngEquipManageBlock();

                    engEquipManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    engEquipManageBlock.EngEquipId = DBCommon.GetInt(dr["EngEquipID"]);
                    engEquipManageBlock.RegNumber = dr["RegNumber"].ToString();
                    engEquipManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    engEquipManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    engEquipManageBlock.EngEquipKind = dr["EngEquipKind"].ToString();
                    engEquipManageBlock.EngEquipBaseMakeName = dr["EngEquipBaseMakeName"].ToString();
                    engEquipManageBlock.EngEquipBaseModelName = dr["EngEquipBaseModelName"].ToString();
                    engEquipManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    engEquipManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    engEquipManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        engEquipManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        engEquipManageBlock.Ownership = "";
                    }

                    engEquipManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    engEquipManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    engEquipManageBlocks.Add(engEquipManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipManageBlocks;
        }

        public static int GetAllEngEquipManageBlocksCount(EngEquipManageFilter filter, User currentUser)
        {
            int engEquipManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RegNumber LIKE '%" + filter.RegNumber.Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.InventoryNumber LIKE '%" + filter.InventoryNumber.Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.EngEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EngEquipKindId IN (" + filter.EngEquipKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.EngEquipBaseMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.EngEquipBaseMakeId IN (" + filter.EngEquipBaseMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.EngEquipBaseModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.EngEquipBaseModelId IN (" + filter.EngEquipBaseModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.EngEquipBaseMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.EngEquipBaseMakeName) LIKE '%" + filter.EngEquipBaseMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.EngEquipBaseModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.EngEquipBaseModelName) LIKE '%" + filter.EngEquipBaseModelName.ToUpper().Replace("'", "''") + "%' ";
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
                                FROM PMIS_RES.EngEquipment a
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
                        engEquipManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipManageBlocksCnt;
        }

        public static List<EngEquipSearchBlock> GetAllEngEquipSearchBlocks(EngEquipSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<EngEquipSearchBlock> engEquipSearchBlocks = new List<EngEquipSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.EngEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EngEquipKindId IN (" + filter.EngEquipKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.EngEquipBaseMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.EngEquipBaseMakeId IN (" + filter.EngEquipBaseMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.EngEquipBaseModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.EngEquipBaseModelId IN (" + filter.EngEquipBaseModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.EngEquipBaseMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.EngEquipBaseMakeName) LIKE '%" + filter.EngEquipBaseMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.EngEquipBaseModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.EngEquipBaseModelName) LIKE '%" + filter.EngEquipBaseModelName.ToUpper().Replace("'", "''") + "%' ";
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
                        orderBySQL = "a.EngEquipBaseMakeName";
                        break;
                    case 6:
                        orderBySQL = "a.EngEquipBaseModelName";
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
                                       a.EngEquipID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as EngEquipKind,
                                       a.EngEquipBaseMakeName,
                                       a.EngEquipBaseModelName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.EngEquipment a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.EngEquipKindId = d.TableKey AND d.TableName = 'EngEquipKind'";

                                //LEFT OUTER JOIN PMIS_RES.EngEquipBaseMakes e ON a.EngEquipBaseMakeId = e.EngEquipBaseMakeId
                                //LEFT OUTER JOIN PMIS_RES.EngEquipBaseModels f ON a.EngEquipBaseModelId = f.EngEquipBaseModelId

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
                    EngEquipSearchBlock engEquipSearchBlock = new EngEquipSearchBlock();

                    engEquipSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    engEquipSearchBlock.EngEquipId = DBCommon.GetInt(dr["EngEquipID"]);
                    engEquipSearchBlock.RegNumber = dr["RegNumber"].ToString();
                    engEquipSearchBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    engEquipSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    engEquipSearchBlock.EngEquipKind = dr["EngEquipKind"].ToString();
                    engEquipSearchBlock.EngEquipBaseMake = dr["EngEquipBaseMakeName"].ToString();
                    engEquipSearchBlock.EngEquipBaseModel = dr["EngEquipBaseModelName"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        engEquipSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        engEquipSearchBlock.Ownership = "";
                    }

                    engEquipSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    engEquipSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    engEquipSearchBlocks.Add(engEquipSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipSearchBlocks;
        }

        public static int GetAllEngEquipSearchBlocksCount(EngEquipSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int engEquipSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_ENG_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.EngEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EngEquipKindId IN (" + filter.EngEquipKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.EngEquipBaseMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.EngEquipBaseMakeId IN (" + filter.EngEquipBaseMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.EngEquipBaseModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.EngEquipBaseModelId IN (" + filter.EngEquipBaseModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.EngEquipBaseMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.EngEquipBaseMakeName) LIKE '%" + filter.EngEquipBaseMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.EngEquipBaseModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.EngEquipBaseModelName) LIKE '%" + filter.EngEquipBaseModelName.ToUpper().Replace("'", "''") + "%' ";
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
                                FROM PMIS_RES.EngEquipment a
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
                        engEquipSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipSearchBlocksCnt;
        }

        //Change the reg number
        public static bool ChangeRegNumber(int engEquipId, string newRegNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            EngEquip engEquip = GetEngEquip(engEquipId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + engEquip.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.EngEquipment SET
                              RegNumber = :NewRegNumber
                           WHERE EngEquipID = :EngEquipID;

                           INSERT INTO PMIS_RES.EngEquipmentRegNumbers (EngEquipID, 
                              RegNumber, ChangeDate)
                           VALUES (:EngEquipID, 
                              :NewRegNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeRegNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ENG_EQUIP_RegNumber", engEquip.RegNumber, newRegNumber, currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "EngEquipID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = engEquipId;
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

                TechnicsUtil.SetTechnicsModified(engEquip.TechnicsId, currentUser);

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

        //Get all reg number (history) by EngEquip with pagination
        public static List<EngEquipRegNumber> GetAllEngEquipRegNumbers(int engEquipId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<EngEquipRegNumber> engEquipRegNumbers = new List<EngEquipRegNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.EngEquipmentRegNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.RegNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.EngEquipmentRegNumberID) as RowNumber
                                               FROM PMIS_RES.EngEquipmentRegNumbers a
                                               WHERE a.EngEquipID = :EngEquipID
                                               ORDER BY " + orderBySQL + @", a.EngEquipmentRegNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EngEquipID", OracleType.Number).Value = engEquipId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EngEquipRegNumber regNumber = new EngEquipRegNumber();
                    regNumber.RegNumber = dr["RegNumber"].ToString();
                    regNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    engEquipRegNumbers.Add(regNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipRegNumbers;
        }

        //Get all reg number (history) count by EngEquip for pagination
        public static int GetAllEngEquipRegNumbersCount(int engEquipId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.EngEquipmentRegNumbers a
                               WHERE a.EngEquipID = :EngEquipID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EngEquipID", OracleType.Number).Value = engEquipId;

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