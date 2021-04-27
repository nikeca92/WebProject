using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular MobileLiftingEquip into the system
    public class MobileLiftingEquip : BaseDbObject
    {
        private int mobileLiftingEquipId;
        private int technicsId;
        private Technics technics;
        private string regNumber;
        private string inventoryNumber;              
        private int? mobileLiftingEquipKindId;
        private GTableItem mobileLiftingEquipKind;
        private int? mobileLiftingEquipTypeId;
        private GTableItem mobileLiftingEquipType;
        private decimal? loadingCapacity;      
        
        public int MobileLiftingEquipId
        {
            get
            {
                return mobileLiftingEquipId;
            }
            set
            {
                mobileLiftingEquipId = value;
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

        public int? MobileLiftingEquipKindId
        {
            get
            {
                return mobileLiftingEquipKindId;
            }
            set
            {
                mobileLiftingEquipKindId = value;
            }
        }

        public GTableItem MobileLiftingEquipKind
        {
            get
            {
                //Lazy initialization
                if (mobileLiftingEquipKind == null && MobileLiftingEquipKindId.HasValue)
                    mobileLiftingEquipKind = GTableItemUtil.GetTableItem("MobileLiftingEquipKind", MobileLiftingEquipKindId.Value, ModuleUtil.RES(), CurrentUser);

                return mobileLiftingEquipKind;
            }
            set
            {
                mobileLiftingEquipKind = value;
            }
        }

        public int? MobileLiftingEquipTypeId
        {
            get
            {
                return mobileLiftingEquipTypeId;
            }
            set
            {
                mobileLiftingEquipTypeId = value;
            }
        }

        public GTableItem MobileLiftingEquipType
        {
            get
            {
                //Lazy initialization
                if (mobileLiftingEquipType == null && MobileLiftingEquipTypeId.HasValue)
                    mobileLiftingEquipType = GTableItemUtil.GetTableItem("MobileLiftingEquipType", MobileLiftingEquipTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return mobileLiftingEquipType;
            }
            set
            {
                mobileLiftingEquipType = value;
            }
        }

        public decimal? LoadingCapacity
        {
            get
            {
                return loadingCapacity;
            }
            set
            {
                loadingCapacity = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public MobileLiftingEquip(User user)
            : base(user)
        {

        }
    }

    public class MobileLiftingEquipRegNumber
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
    public class MobileLiftingEquipManageFilter
    {
        string regNumber;
        string inventoryNumber;
        string technicsCategoryId;
        string mobileLiftingEquipKindId;
        string mobileLiftingEquipTypeId;
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

        public string MobileLiftingEquipKindId
        {
            get { return mobileLiftingEquipKindId; }
            set { mobileLiftingEquipKindId = value; }
        }

        public string MobileLiftingEquipTypeId
        {
            get { return mobileLiftingEquipTypeId; }
            set { mobileLiftingEquipTypeId = value; }
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

    public class MobileLiftingEquipManageBlock
    {
        private int technicsId;
        private int mobileLiftingEquipId;
        private string regNumber;
        string inventoryNumber;
        string technicsCategory;
        string mobileLiftingEquipKind;
        string mobileLiftingEquipType;
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

        public int MobileLiftingEquipId
        {
            get { return mobileLiftingEquipId; }
            set { mobileLiftingEquipId = value; }
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

        public string MobileLiftingEquipKind
        {
            get { return mobileLiftingEquipKind; }
            set { mobileLiftingEquipKind = value; }
        }

        public string MobileLiftingEquipType
        {
            get { return mobileLiftingEquipType; }
            set { mobileLiftingEquipType = value; }
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

    public class MobileLiftingEquipFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;        
        private int technicReadinessID;        
        private int mobileLiftingEquipID;        
        private MobileLiftingEquip mobileLiftingEquip;
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

        public int MobileLiftingEquipID
        {
            get { return mobileLiftingEquipID; }
            set { mobileLiftingEquipID = value; }
        }

        public MobileLiftingEquip MobileLiftingEquip
        {
            get { return mobileLiftingEquip; }
            set { mobileLiftingEquip = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class MobileLiftingEquipSearchFilter
    {
        string regNumber;
        string inventoryNumber;        
        string technicsCategoryId;        
        string mobileLiftingEquipKindId;
        string mobileLiftingEquipTypeId;
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

        public string MobileLiftingEquipKindId
        {
            get { return mobileLiftingEquipKindId; }
            set { mobileLiftingEquipKindId = value; }
        }

        public string MobileLiftingEquipTypeId
        {
            get { return mobileLiftingEquipTypeId; }
            set { mobileLiftingEquipTypeId = value; }
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

    public class MobileLiftingEquipSearchBlock
    {
        private int technicsId;
        private int mobileLiftingEquipId;        
        private string regNumber;
        string inventoryNumber;              
        string technicsCategory;        
        string mobileLiftingEquipKind;
        string mobileLiftingEquipType;
        string ownership;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int MobileLiftingEquipId
        {
            get { return mobileLiftingEquipId; }
            set { mobileLiftingEquipId = value; }
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

        public string MobileLiftingEquipKind
        {
            get { return mobileLiftingEquipKind; }
            set { mobileLiftingEquipKind = value; }
        }

        public string MobileLiftingEquipType
        {
            get { return mobileLiftingEquipType; }
            set { mobileLiftingEquipType = value; }
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

    public static class MobileLiftingEquipUtil
    {
        //This method creates and returns a MobileLiftingEquip object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static MobileLiftingEquip ExtractMobileLiftingEquip(OracleDataReader dr, User currentUser)
        {
            MobileLiftingEquip mobileLiftingEquip = new MobileLiftingEquip(currentUser);

            mobileLiftingEquip.MobileLiftingEquipId = DBCommon.GetInt(dr["MobileLiftingEquipID"]);
            mobileLiftingEquip.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            mobileLiftingEquip.RegNumber = dr["RegNumber"].ToString();
            mobileLiftingEquip.InventoryNumber = dr["InventoryNumber"].ToString();
            mobileLiftingEquip.MobileLiftingEquipKindId = (DBCommon.IsInt(dr["MobileLiftingEquipKindID"]) ? DBCommon.GetInt(dr["MobileLiftingEquipKindID"]) : (int?)null);
            mobileLiftingEquip.MobileLiftingEquipTypeId = (DBCommon.IsInt(dr["MobileLiftingEquipTypeID"]) ? DBCommon.GetInt(dr["MobileLiftingEquipTypeID"]) : (int?)null);
            mobileLiftingEquip.LoadingCapacity = (DBCommon.IsDecimal(dr["LoadingCapacity"]) ? DBCommon.GetDecimal(dr["LoadingCapacity"]) : (decimal?)null);            

            return mobileLiftingEquip;
        }

        //Get a particular object by its ID
        public static MobileLiftingEquip GetMobileLiftingEquip(int mobileLiftingEquipId, User currentUser)
        {
            MobileLiftingEquip mobileLiftingEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.MobileLiftingEquipID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.MobileLiftingEquipKindID, a.MobileLiftingEquipTypeID,
                                  a.LoadingCapacity                              
                               FROM PMIS_RES.MobileLiftingEquip a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.MobileLiftingEquipID = :MobileLiftingEquipID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MobileLiftingEquipID", OracleType.Number).Value = mobileLiftingEquipId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    mobileLiftingEquip = ExtractMobileLiftingEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquip;
        }

        //Get a particular object by its ID
        public static MobileLiftingEquip GetMobileLiftingEquipByTechnicsId(int technicsId, User currentUser)
        {
            MobileLiftingEquip mobileLiftingEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.MobileLiftingEquipID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.MobileLiftingEquipKindID, a.MobileLiftingEquipTypeID,
                                  a.LoadingCapacity                                 
                               FROM PMIS_RES.MobileLiftingEquip a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID  
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    mobileLiftingEquip = ExtractMobileLiftingEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquip;
        }

        //Get a particular object by its reg number
        public static MobileLiftingEquip GetMobileLiftingEquipByRegNumber(string regNumber, User currentUser)
        {
            MobileLiftingEquip mobileLiftingEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.MobileLiftingEquipID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.MobileLiftingEquipKindID, a.MobileLiftingEquipTypeID,
                                  a.LoadingCapacity
                               FROM PMIS_RES.MobileLiftingEquip a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.RegNumber = :RegNumber 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RegNumber", OracleType.VarChar).Value = regNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    mobileLiftingEquip = ExtractMobileLiftingEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquip;
        }

        //Get all MobileLiftingEquip objects
        public static List<MobileLiftingEquip> GetAllMobileLiftingEquips(User currentUser)
        {
            List<MobileLiftingEquip> mobileLiftingEquips = new List<MobileLiftingEquip>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.MobileLiftingEquipID, a.TechnicsID, a.RegNumber, a.InventoryNumber,
                                  a.MobileLiftingEquipKindID, a.MobileLiftingEquipTypeID,
                                  a.LoadingCapacity
                               FROM PMIS_RES.MobileLiftingEquip a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    mobileLiftingEquips.Add(ExtractMobileLiftingEquip(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquips;
        }

        public static List<MobileLiftingEquipFulfilmentBlock> GetAllMobileLiftingEquipFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<MobileLiftingEquipFulfilmentBlock> mobileLiftingEquips = new List<MobileLiftingEquipFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND j.CreatedBy = " + currentUser.UserId.ToString();
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
                        orderBySQL = "e.TableValue";
                        break;
                    case 3:
                        orderBySQL = "f.TableValue";
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
                        orderBySQL = "b.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.FulfilTechnicsRequestID,
                                      tmp.TechnicsRequestCmdPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.TechnicReadinessID,
                                      tmp.AppointmentIsDelivered,

                                      tmp.MobileLiftingEquipID, 
                                      tmp.TechnicsID, 
                                      tmp.RegNumber, 
                                      tmp.InventoryNumber,                                    
                                      tmp.MobileLiftingEquipKindID, 
                                      tmp.MobileLiftingEquipTypeID,
                                      tmp.LoadingCapacity,
                                      
                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             a.AppointmentIsDelivered,

                                             b.MobileLiftingEquipID, 
                                             b.TechnicsID, 
                                             b.RegNumber, 
                                             b.InventoryNumber,                                         
                                             b.MobileLiftingEquipKindID, 
                                             b.MobileLiftingEquipTypeID,
                                             b.LoadingCapacity,
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.MobileLiftingEquip b ON a.TechnicsID = b.TechnicsID                                      
                                      LEFT OUTER JOIN PMIS_RES.GTable e ON b.MobileLiftingEquipKindID = e.TableKey AND e.TableName = 'MobileLiftingEquipKind'
                                      LEFT OUTER JOIN PMIS_RES.GTable f ON b.MobileLiftingEquipTypeID = f.TableKey AND f.TableName = 'MobileLiftingEquipType'
                                      INNER JOIN PMIS_RES.Technics j ON b.TechnicsID = j.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON j.NormativeTechnicsID = n.NormativeTechnicsID
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
                    MobileLiftingEquipFulfilmentBlock block = new MobileLiftingEquipFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.MobileLiftingEquipID = DBCommon.GetInt(dr["MobileLiftingEquipID"]);
                    block.MobileLiftingEquip = ExtractMobileLiftingEquip(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    mobileLiftingEquips.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquips;
        }

        public static int GetAllMobileLiftingEquipFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int mobileLiftingEquips = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.MobileLiftingEquip b ON a.TechnicsID = b.TechnicsID
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
                    mobileLiftingEquips = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquips;
        }

        //Save a particular object into the DB
        public static bool SaveMobileLiftingEquip(MobileLiftingEquip mobileLiftingEquip, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + mobileLiftingEquip.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (mobileLiftingEquip.MobileLiftingEquipId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.MobileLiftingEquip (TechnicsID, RegNumber, 
                                InventoryNumber, MobileLiftingEquipKindID, MobileLiftingEquipTypeID,
                                LoadingCapacity)
                            VALUES (:TechnicsID, :RegNumber, 
                                :InventoryNumber, :MobileLiftingEquipKindID, :MobileLiftingEquipTypeID,
                                :LoadingCapacity);

                            SELECT PMIS_RES.MobileLiftingEquip_ID_SEQ.currval INTO :MobileLiftingEquipID FROM dual;

                            INSERT INTO PMIS_RES.MobileLiftingEquipRegNumbers (MobileLiftingEquipID, 
                               RegNumber, ChangeDate)
                            VALUES (:MobileLiftingEquipID, 
                               :RegNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_MOB_LIFT_EQUIP_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_RegNumber", "", mobileLiftingEquip.RegNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_InventoryNumber", "", mobileLiftingEquip.InventoryNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_MobLiftEquipKind", "", mobileLiftingEquip.MobileLiftingEquipKindId.HasValue ? mobileLiftingEquip.MobileLiftingEquipKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_MobLiftEquipType", "", mobileLiftingEquip.MobileLiftingEquipTypeId.HasValue ? mobileLiftingEquip.MobileLiftingEquipType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_LoadingCapacity", "", mobileLiftingEquip.LoadingCapacity.HasValue ? mobileLiftingEquip.LoadingCapacity.Value.ToString() : "", currentUser));                    
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.MobileLiftingEquip SET
                               RegNumber = :RegNumber,
                               InventoryNumber = :InventoryNumber,                              
                               MobileLiftingEquipKindID = :MobileLiftingEquipKindID,
                               MobileLiftingEquipTypeID = :MobileLiftingEquipTypeID,
                               LoadingCapacity = :LoadingCapacity               
                             WHERE MobileLiftingEquipID = :MobileLiftingEquipID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_MOB_LIFT_EQUIP_Edit", logDescription, null, null, currentUser);

                    MobileLiftingEquip oldMobileLiftingEquip = GetMobileLiftingEquip(mobileLiftingEquip.MobileLiftingEquipId, currentUser);

                    if (oldMobileLiftingEquip.RegNumber.Trim() != mobileLiftingEquip.RegNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_RegNumber", oldMobileLiftingEquip.RegNumber, mobileLiftingEquip.RegNumber, currentUser));

                    if (oldMobileLiftingEquip.InventoryNumber.Trim() != mobileLiftingEquip.InventoryNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_InventoryNumber", oldMobileLiftingEquip.InventoryNumber, mobileLiftingEquip.InventoryNumber, currentUser));
                    
                    if (!CommonFunctions.IsEqualInt(oldMobileLiftingEquip.MobileLiftingEquipKindId, mobileLiftingEquip.MobileLiftingEquipKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_MobLiftEquipKind", oldMobileLiftingEquip.MobileLiftingEquipKindId.HasValue ? oldMobileLiftingEquip.MobileLiftingEquipKind.TableValue : "", mobileLiftingEquip.MobileLiftingEquipKindId.HasValue ? mobileLiftingEquip.MobileLiftingEquipKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldMobileLiftingEquip.MobileLiftingEquipTypeId, mobileLiftingEquip.MobileLiftingEquipTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_MobLiftEquipType", oldMobileLiftingEquip.MobileLiftingEquipTypeId.HasValue ? oldMobileLiftingEquip.MobileLiftingEquipType.TableValue : "", mobileLiftingEquip.MobileLiftingEquipTypeId.HasValue ? mobileLiftingEquip.MobileLiftingEquipType.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldMobileLiftingEquip.LoadingCapacity, mobileLiftingEquip.LoadingCapacity))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_LoadingCapacity", oldMobileLiftingEquip.LoadingCapacity.HasValue ? oldMobileLiftingEquip.LoadingCapacity.ToString() : "", mobileLiftingEquip.LoadingCapacity.HasValue ? mobileLiftingEquip.LoadingCapacity.ToString() : "", currentUser));                    
                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(mobileLiftingEquip.Technics, currentUser, changeEvent);
                mobileLiftingEquip.TechnicsId = mobileLiftingEquip.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramMobileLiftingEquipID = new OracleParameter();
                paramMobileLiftingEquipID.ParameterName = "MobileLiftingEquipID";
                paramMobileLiftingEquipID.OracleType = OracleType.Number;

                if (mobileLiftingEquip.MobileLiftingEquipId != 0)
                {
                    paramMobileLiftingEquipID.Direction = ParameterDirection.Input;
                    paramMobileLiftingEquipID.Value = mobileLiftingEquip.MobileLiftingEquipId;
                }
                else
                {
                    paramMobileLiftingEquipID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramMobileLiftingEquipID);

                OracleParameter param = null;

                if (mobileLiftingEquip.MobileLiftingEquipId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = mobileLiftingEquip.TechnicsId;
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
                if (!string.IsNullOrEmpty(mobileLiftingEquip.RegNumber))
                    param.Value = mobileLiftingEquip.RegNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "InventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(mobileLiftingEquip.InventoryNumber))
                    param.Value = mobileLiftingEquip.InventoryNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);             

                param = new OracleParameter();
                param.ParameterName = "MobileLiftingEquipKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (mobileLiftingEquip.MobileLiftingEquipKindId.HasValue)
                    param.Value = mobileLiftingEquip.MobileLiftingEquipKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MobileLiftingEquipTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (mobileLiftingEquip.MobileLiftingEquipTypeId.HasValue)
                    param.Value = mobileLiftingEquip.MobileLiftingEquipTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LoadingCapacity";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (mobileLiftingEquip.LoadingCapacity.HasValue)
                    param.Value = mobileLiftingEquip.LoadingCapacity.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);              

                cmd.ExecuteNonQuery();

                if (mobileLiftingEquip.MobileLiftingEquipId == 0)
                    mobileLiftingEquip.MobileLiftingEquipId = DBCommon.GetInt(paramMobileLiftingEquipID.Value);

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

        public static List<MobileLiftingEquipManageBlock> GetAllMobileLiftingEquipManageBlocks(MobileLiftingEquipManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<MobileLiftingEquipManageBlock> mobileLiftingEquipManageBlocks = new List<MobileLiftingEquipManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.MobileLiftingEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MobileLiftingEquipKindId IN (" + filter.MobileLiftingEquipKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MobileLiftingEquipTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MobileLiftingEquipTypeId IN (" + filter.MobileLiftingEquipTypeId + ") ";
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
                        orderBySQL = "e.TableValue";
                        break;
                    case 6:
                        orderBySQL = "h.MilitaryDepartmentName";
                        break;
                    case 7:
                        orderBySQL = "g.TechMilitaryReportStatusName";
                        break;
                    case 8:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 9:
                        orderBySQL = "PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address)";
                        break;
                    case 10:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.MobileLiftingEquipID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as MobileLiftingEquipKind,  
                                       e.TableValue as MobileLiftingEquipType,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.MobileLiftingEquip a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.MobileLiftingEquipKindId = d.TableKey AND d.TableName = 'MobileLiftingEquipKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.MobileLiftingEquipTypeId = e.TableKey AND e.TableName = 'MobileLiftingEquipType'
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
                    MobileLiftingEquipManageBlock mobileLiftingEquipManageBlock = new MobileLiftingEquipManageBlock();

                    mobileLiftingEquipManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    mobileLiftingEquipManageBlock.MobileLiftingEquipId = DBCommon.GetInt(dr["MobileLiftingEquipID"]);
                    mobileLiftingEquipManageBlock.RegNumber = dr["RegNumber"].ToString();
                    mobileLiftingEquipManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    mobileLiftingEquipManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    mobileLiftingEquipManageBlock.MobileLiftingEquipKind = dr["MobileLiftingEquipKind"].ToString();
                    mobileLiftingEquipManageBlock.MobileLiftingEquipType = dr["MobileLiftingEquipType"].ToString();
                    mobileLiftingEquipManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    mobileLiftingEquipManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    mobileLiftingEquipManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        mobileLiftingEquipManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        mobileLiftingEquipManageBlock.Ownership = "";
                    }

                    mobileLiftingEquipManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    mobileLiftingEquipManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();
                   
                    mobileLiftingEquipManageBlocks.Add(mobileLiftingEquipManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquipManageBlocks;
        }

        public static int GetAllMobileLiftingEquipManageBlocksCount(MobileLiftingEquipManageFilter filter, User currentUser)
        {
            int mobileLiftingEquipManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.MobileLiftingEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MobileLiftingEquipKindId IN (" + filter.MobileLiftingEquipKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MobileLiftingEquipTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MobileLiftingEquipTypeId IN (" + filter.MobileLiftingEquipTypeId + ") ";
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
                                FROM PMIS_RES.MobileLiftingEquip a
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
                        mobileLiftingEquipManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquipManageBlocksCnt;
        }

        public static List<MobileLiftingEquipSearchBlock> GetAllMobileLiftingEquipSearchBlocks(MobileLiftingEquipSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<MobileLiftingEquipSearchBlock> mobileLiftingEquipSearchBlocks = new List<MobileLiftingEquipSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.MobileLiftingEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MobileLiftingEquipKindId IN (" + filter.MobileLiftingEquipKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MobileLiftingEquipTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MobileLiftingEquipTypeId IN (" + filter.MobileLiftingEquipTypeId + ") ";
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
                        orderBySQL = "e.TableValue";
                        break;
                    case 6:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 7:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.MobileLiftingEquipID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as MobileLiftingEquipKind,
                                       e.TableValue as MobileLiftingEquipType,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.MobileLiftingEquip a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.MobileLiftingEquipKindId = d.TableKey AND d.TableName = 'MobileLiftingEquipKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.MobileLiftingEquipTypeId = e.TableKey AND e.TableName = 'MobileLiftingEquipType'
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
                    MobileLiftingEquipSearchBlock mobileLiftingEquipSearchBlock = new MobileLiftingEquipSearchBlock();

                    mobileLiftingEquipSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    mobileLiftingEquipSearchBlock.MobileLiftingEquipId = DBCommon.GetInt(dr["MobileLiftingEquipID"]);
                    mobileLiftingEquipSearchBlock.RegNumber = dr["RegNumber"].ToString();
                    mobileLiftingEquipSearchBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    mobileLiftingEquipSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    mobileLiftingEquipSearchBlock.MobileLiftingEquipKind = dr["MobileLiftingEquipKind"].ToString();
                    mobileLiftingEquipSearchBlock.MobileLiftingEquipType = dr["MobileLiftingEquipType"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        mobileLiftingEquipSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        mobileLiftingEquipSearchBlock.Ownership = "";
                    }

                    mobileLiftingEquipSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    mobileLiftingEquipSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    mobileLiftingEquipSearchBlocks.Add(mobileLiftingEquipSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquipSearchBlocks;
        }

        public static int GetAllMobileLiftingEquipSearchBlocksCount(MobileLiftingEquipSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int mobileLiftingEquipSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_MOB_LIFT_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.MobileLiftingEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MobileLiftingEquipKindId IN (" + filter.MobileLiftingEquipKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MobileLiftingEquipTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MobileLiftingEquipTypeId IN (" + filter.MobileLiftingEquipTypeId + ") ";
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
                                FROM PMIS_RES.MobileLiftingEquip a
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
                        mobileLiftingEquipSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquipSearchBlocksCnt;
        }

        //Change the reg number
        public static bool ChangeRegNumber(int mobileLiftingEquipId, string newRegNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            MobileLiftingEquip mobileLiftingEquip = GetMobileLiftingEquip(mobileLiftingEquipId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Регистрационен номер: " + mobileLiftingEquip.RegNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.MobileLiftingEquip SET
                              RegNumber = :NewRegNumber
                           WHERE MobileLiftingEquipID = :MobileLiftingEquipID;

                           INSERT INTO PMIS_RES.MobileLiftingEquipRegNumbers (MobileLiftingEquipID, 
                              RegNumber, ChangeDate)
                           VALUES (:MobileLiftingEquipID, 
                              :NewRegNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeRegNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MOB_LIFT_EQUIP_RegNumber", mobileLiftingEquip.RegNumber, newRegNumber, currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "MobileLiftingEquipID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = mobileLiftingEquipId;
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

                TechnicsUtil.SetTechnicsModified(mobileLiftingEquip.TechnicsId, currentUser);

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

        //Get all reg number (history) by MobileLiftingEquip with pagination
        public static List<MobileLiftingEquipRegNumber> GetAllMobileLiftingEquipRegNumbers(int mobileLiftingEquipId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<MobileLiftingEquipRegNumber> mobileLiftingEquipRegNumbers = new List<MobileLiftingEquipRegNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.MobileLiftingEquipRegNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.RegNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.MobileLiftingEquipRegNumberID) as RowNumber
                                               FROM PMIS_RES.MobileLiftingEquipRegNumbers a
                                               WHERE a.MobileLiftingEquipID = :MobileLiftingEquipID
                                               ORDER BY " + orderBySQL + @", a.MobileLiftingEquipRegNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MobileLiftingEquipID", OracleType.Number).Value = mobileLiftingEquipId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MobileLiftingEquipRegNumber regNumber = new MobileLiftingEquipRegNumber();
                    regNumber.RegNumber = dr["RegNumber"].ToString();
                    regNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    mobileLiftingEquipRegNumbers.Add(regNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return mobileLiftingEquipRegNumbers;
        }

        //Get all reg number (history) count by MobileLiftingEquip for pagination
        public static int GetAllMobileLiftingEquipRegNumbersCount(int mobileLiftingEquipId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.MobileLiftingEquipRegNumbers a
                               WHERE a.MobileLiftingEquipID = :MobileLiftingEquipID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MobileLiftingEquipID", OracleType.Number).Value = mobileLiftingEquipId;

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