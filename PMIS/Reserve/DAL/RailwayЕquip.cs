using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular RailwayEquip into the system
    public class RailwayEquip : BaseDbObject
    {
        private int railwayEquipId;
        private int technicsId;
        private Technics technics;
        private string inventoryNumber;
        private int? railwayEquipKindId;
        private GTableItem railwayEquipKind;
        private int? railwayEquipTypeId;
        private GTableItem railwayEquipType;

        public int RailwayEquipId
        {
            get 
            {
                return railwayEquipId;
            }
            set
            {
                railwayEquipId = value;
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

        public int? RailwayEquipKindId
        {
            get
            {
                return railwayEquipKindId;
            }
            set
            {
                railwayEquipKindId = value;
            }
        }

        public GTableItem RailwayEquipKind
        {
            get
            {
                //Lazy initialization
                if (railwayEquipKind == null && RailwayEquipKindId.HasValue)
                    railwayEquipKind = GTableItemUtil.GetTableItem("RailwayEquipKind", RailwayEquipKindId.Value, ModuleUtil.RES(), CurrentUser);

                return railwayEquipKind;
            }
            set
            {
                railwayEquipKind = value;
            }
        }

        public int? RailwayEquipTypeId
        {
            get
            {
                return railwayEquipTypeId;
            }
            set
            {
                railwayEquipTypeId = value;
            }
        }

        public GTableItem RailwayEquipType
        {
            get
            {
                //Lazy initialization
                if (railwayEquipType == null && RailwayEquipTypeId.HasValue)
                    railwayEquipType = GTableItemUtil.GetTableItem("RailwayEquipType", RailwayEquipTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return railwayEquipType;
            }
            set
            {
                railwayEquipType = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public RailwayEquip(User user)
            : base(user)
        {

        }
    }

    public class RailwayEquipInvNumber
    {
        private string inventoryNumber;
        private DateTime changeDate;

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
    public class RailwayEquipManageFilter
    {
        string inventoryNumber;
        string technicsCategoryId;
        string railwayEquipKindId;
        string railwayEquipTypeId;
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

        public string RailwayEquipKindId
        {
            get { return railwayEquipKindId; }
            set { railwayEquipKindId = value; }
        }

        public string RailwayEquipTypeId
        {
            get { return railwayEquipTypeId; }
            set { railwayEquipTypeId = value; }
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
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int PageIdx
        {
            get { return pageIdx; }
            set { pageIdx = value; }
        }
    }

    public class RailwayEquipManageBlock
    {
        private int technicsId;
        private int railwayEquipId;
        string inventoryNumber;
        string technicsCategory;
        string railwayEquipKind;
        string railwayEquipType;
        string itemsCount;
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

        public int RailwayEquipId
        {
            get { return railwayEquipId; }
            set { railwayEquipId = value; }
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

        public string RailwayEquipKind
        {
            get { return railwayEquipKind; }
            set { railwayEquipKind = value; }
        }

        public string RailwayEquipType
        {
            get { return railwayEquipType; }
            set { railwayEquipType = value; }
        }

        public string ItemsCount
        {
            get { return itemsCount; }
            set { itemsCount = value; }
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

    public class RailwayEquipFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;        
        private int technicReadinessID;
        private int railwayEquipID;
        private int itemsCount;
        private RailwayEquip railwayEquip;
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

        public int ItemsCount
        {
            get { return itemsCount; }
            set { itemsCount = value; }
        }

        public int RailwayEquipID
        {
            get { return railwayEquipID; }
            set { railwayEquipID = value; }
        }

        public RailwayEquip RailwayEquip
        {
            get { return railwayEquip; }
            set { railwayEquip = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class RailwayEquipSearchFilter
    {
        string inventoryNumber;        
        string technicsCategoryId;
        string railwayEquipKindId;
        string railwayEquipTypeId;
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

        public string RailwayEquipKindId
        {
            get { return railwayEquipKindId; }
            set { railwayEquipKindId = value; }
        }

        public string RailwayEquipTypeId
        {
            get { return railwayEquipTypeId; }
            set { railwayEquipTypeId = value; }
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
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int PageIdx
        {
            get { return pageIdx; }
            set { pageIdx = value; }
        }
    }

    public class RailwayEquipSearchBlock
    {
        private int technicsId;
        private int railwayEquipId;
        string inventoryNumber;              
        string technicsCategory;
        string railwayEquipKind;
        string railwayEquipType;
        string itemsCount;
        string ownership;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int RailwayEquipId
        {
            get { return railwayEquipId; }
            set { railwayEquipId = value; }
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

        public string RailwayEquipKind
        {
            get { return railwayEquipKind; }
            set { railwayEquipKind = value; }
        }

        public string RailwayEquipType
        {
            get { return railwayEquipType; }
            set { railwayEquipType = value; }
        }

        public string ItemsCount
        {
            get { return itemsCount; }
            set { itemsCount = value; }
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

    public static class RailwayEquipUtil
    {
        //This method creates and returns a RailwayEquip object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static RailwayEquip ExtractRailwayEquip(OracleDataReader dr, User currentUser)
        {
            RailwayEquip railwayEquip = new RailwayEquip(currentUser);

            railwayEquip.RailwayEquipId = DBCommon.GetInt(dr["RailwayEquipID"]);
            railwayEquip.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            railwayEquip.InventoryNumber = dr["InventoryNumber"].ToString();
            railwayEquip.RailwayEquipKindId = (DBCommon.IsInt(dr["RailwayEquipKindID"]) ? DBCommon.GetInt(dr["RailwayEquipKindID"]) : (int?)null);
            railwayEquip.RailwayEquipTypeId = (DBCommon.IsInt(dr["RailwayEquipTypeID"]) ? DBCommon.GetInt(dr["RailwayEquipTypeID"]) : (int?)null);

            return railwayEquip;
        }

        //Get a particular object by its ID
        public static RailwayEquip GetRailwayEquip(int railwayEquipId, User currentUser)
        {
            RailwayEquip railwayEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.RailwayEquipID, a.TechnicsID, a.InventoryNumber,
                                  a.RailwayEquipKindID, a.RailwayEquipTypeID
                               FROM PMIS_RES.RailwayEquips a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.RailwayEquipID = :RailwayEquipID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RailwayEquipID", OracleType.Number).Value = railwayEquipId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    railwayEquip = ExtractRailwayEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquip;
        }

        //Get a particular object by its ID
        public static RailwayEquip GetRailwayEquipByTechnicsId(int technicsId, User currentUser)
        {
            RailwayEquip railwayEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.RailwayEquipID, a.TechnicsID, a.InventoryNumber,
                                  a.RailwayEquipKindID, a.RailwayEquipTypeID
                               FROM PMIS_RES.RailwayEquips a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID  
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    railwayEquip = ExtractRailwayEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquip;
        }

        //Get a particular object by its inventory number
        public static RailwayEquip GetRailwayEquipByInvNumber(string inventoryNumber, User currentUser)
        {
            RailwayEquip railwayEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.RailwayEquipID, a.TechnicsID, a.InventoryNumber,
                                  a.RailwayEquipKindID, a.RailwayEquipTypeID
                               FROM PMIS_RES.RailwayEquips a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.InventoryNumber = :InventoryNumber 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("InventoryNumber", OracleType.VarChar).Value = inventoryNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    railwayEquip = ExtractRailwayEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquip;
        }

        //Get all RailwayEquip objects
        public static List<RailwayEquip> GetAllRailwayEquips(User currentUser)
        {
            List<RailwayEquip> railwayEquips = new List<RailwayEquip>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.RailwayEquipID, a.TechnicsID, a.InventoryNumber,
                                  a.RailwayEquipKindID, a.RailwayEquipTypeID
                               FROM PMIS_RES.RailwayEquips a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    railwayEquips.Add(ExtractRailwayEquip(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquips;
        }

        public static List<RailwayEquipFulfilmentBlock> GetAllRailwayEquipFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<RailwayEquipFulfilmentBlock> railwayEquips = new List<RailwayEquipFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND e.CreatedBy = " + currentUser.UserId.ToString();
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
                        orderBySQL = "b.InventoryNumber";
                        break;
                    case 2:
                        orderBySQL = "c.TableValue";
                        break;
                    case 3:
                        orderBySQL = "d.TableValue";
                        break;
                    case 4:
                        orderBySQL = "a.TechnicReadinessID";
                        break;
                    case 5:
                        orderBySQL = "e.ItemsCount";
                        break;
                    case 6:
                        orderBySQL = "n.NormativeCode";
                        break;
                    case 7:
                        orderBySQL = "a.AppointmentIsDelivered";
                        break;
                    default:
                        orderBySQL = "b.InventoryNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.FulfilTechnicsRequestID,
                                      tmp.TechnicsRequestCmdPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.TechnicReadinessID,
                                      tmp.ItemsCount,
                                      tmp.AppointmentIsDelivered,

                                      tmp.RailwayEquipID, 
                                      tmp.TechnicsID,
                                      tmp.InventoryNumber,
                                      tmp.RailwayEquipKindID,
                                      tmp.RailwayEquipTypeID,
                                      
                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             e.ItemsCount,
                                             a.AppointmentIsDelivered,

                                             b.RailwayEquipID, 
                                             b.TechnicsID,
                                             b.InventoryNumber,
                                             b.RailwayEquipKindID, 
                                             b.RailwayEquipTypeID, 
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.RailwayEquips b ON a.TechnicsID = b.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.GTable c ON b.RailwayEquipKindID = c.TableKey AND c.TableName = 'RailwayEquipKind'
                                      LEFT OUTER JOIN PMIS_RES.GTable d ON b.RailwayEquipTypeID = d.TableKey AND d.TableName = 'RailwayEquipType'
                                      INNER JOIN PMIS_RES.Technics e ON b.TechnicsID = e.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON e.NormativeTechnicsID = n.NormativeTechnicsID
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
                    RailwayEquipFulfilmentBlock block = new RailwayEquipFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.ItemsCount = DBCommon.GetInt(dr["ItemsCount"]);
                    block.RailwayEquipID = DBCommon.GetInt(dr["RailwayEquipID"]);
                    block.RailwayEquip = ExtractRailwayEquip(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    railwayEquips.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquips;
        }

        public static int GetAllRailwayEquipFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int railwayEquips = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.RailwayEquips b ON a.TechnicsID = b.TechnicsID
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
                    railwayEquips = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquips;
        }

        //Save a particular object into the DB
        public static bool SaveRailwayEquip(RailwayEquip railwayEquip, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Инвентарен номер: " + railwayEquip.InventoryNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (railwayEquip.RailwayEquipId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.RailwayEquips (TechnicsID, InventoryNumber, RailwayEquipKindID, RailwayEquipTypeID)
                            VALUES (:TechnicsID, :InventoryNumber, :RailwayEquipKindID, :RailwayEquipTypeID);

                            SELECT PMIS_RES.RailwayEquips_ID_SEQ.currval INTO :RailwayEquipID FROM dual;

                            INSERT INTO PMIS_RES.RailwayEquipInvNumbers (RailwayEquipID, InventoryNumber, ChangeDate)
                            VALUES (:RailwayEquipID, :InventoryNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_RAILWAY_EQUIP_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_InventoryNumber", "", railwayEquip.InventoryNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_RailwayEquipKind", "", railwayEquip.RailwayEquipKindId.HasValue ? railwayEquip.RailwayEquipKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_RailwayEquipType", "", railwayEquip.RailwayEquipTypeId.HasValue ? railwayEquip.RailwayEquipType.TableValue : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.RailwayEquips SET
                               InventoryNumber = :InventoryNumber,
                               RailwayEquipKindID = :RailwayEquipKindID,
                               RailwayEquipTypeID = :RailwayEquipTypeID
                             WHERE RailwayEquipID = :RailwayEquipID ;

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_RAILWAY_EQUIP_Edit", logDescription, null, null, currentUser);

                    RailwayEquip oldRailwayEquip = GetRailwayEquip(railwayEquip.RailwayEquipId, currentUser);

                    if (oldRailwayEquip.InventoryNumber.Trim() != railwayEquip.InventoryNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_InventoryNumber", oldRailwayEquip.InventoryNumber, railwayEquip.InventoryNumber, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldRailwayEquip.RailwayEquipKindId, railwayEquip.RailwayEquipKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_RailwayEquipKind", oldRailwayEquip.RailwayEquipKindId.HasValue ? oldRailwayEquip.RailwayEquipKind.TableValue : "", railwayEquip.RailwayEquipKindId.HasValue ? railwayEquip.RailwayEquipKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldRailwayEquip.RailwayEquipTypeId, railwayEquip.RailwayEquipTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_RailwayEquipType", oldRailwayEquip.RailwayEquipTypeId.HasValue ? oldRailwayEquip.RailwayEquipType.TableValue : "", railwayEquip.RailwayEquipTypeId.HasValue ? railwayEquip.RailwayEquipType.TableValue : "", currentUser));
                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(railwayEquip.Technics, currentUser, changeEvent);
                railwayEquip.TechnicsId = railwayEquip.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRailwayEquipID = new OracleParameter();
                paramRailwayEquipID.ParameterName = "RailwayEquipID";
                paramRailwayEquipID.OracleType = OracleType.Number;

                if (railwayEquip.RailwayEquipId != 0)
                {
                    paramRailwayEquipID.Direction = ParameterDirection.Input;
                    paramRailwayEquipID.Value = railwayEquip.RailwayEquipId;
                }
                else
                {
                    paramRailwayEquipID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRailwayEquipID);

                OracleParameter param = null;

                if (railwayEquip.RailwayEquipId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = railwayEquip.TechnicsId;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "ChangeDate";
                    param.OracleType = OracleType.DateTime;
                    param.Direction = ParameterDirection.Input;
                    param.Value = DateTime.Now;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "InventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(railwayEquip.InventoryNumber))
                    param.Value = railwayEquip.InventoryNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RailwayEquipKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (railwayEquip.RailwayEquipKindId.HasValue)
                    param.Value = railwayEquip.RailwayEquipKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RailwayEquipTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (railwayEquip.RailwayEquipTypeId.HasValue)
                    param.Value = railwayEquip.RailwayEquipTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (railwayEquip.RailwayEquipId == 0)
                    railwayEquip.RailwayEquipId = DBCommon.GetInt(paramRailwayEquipID.Value);

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

        public static List<RailwayEquipManageBlock> GetAllRailwayEquipManageBlocks(RailwayEquipManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<RailwayEquipManageBlock> railwayEquipManageBlocks = new List<RailwayEquipManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryID IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.RailwayEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RailwayEquipKindID IN (" + filter.RailwayEquipKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.RailwayEquipTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RailwayEquipTypeID IN (" + filter.RailwayEquipTypeId + ") ";
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
                        orderBySQL = "a.InventoryNumber";
                        break;
                    case 2:
                        orderBySQL = "c.TechnicsCategoryName";
                        break;
                    case 3:
                        orderBySQL = "d.TableValue";
                        break;
                    case 4:
                        orderBySQL = "e.TableValue";
                        break;
                    case 5:
                        orderBySQL = "b.ItemsCount";
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
                        orderBySQL = "a.InventoryNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.RailwayEquipID,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as RailwayEquipKind,
                                       e.TableValue as RailwayEquipType,
                                       b.ItemsCount,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.RailwayEquips a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.RailwayEquipKindId = d.TableKey AND d.TableName = 'RailwayEquipKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.RailwayEquipTypeId = e.TableKey AND e.TableName = 'RailwayEquipType'
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
                    RailwayEquipManageBlock railwayEquipManageBlock = new RailwayEquipManageBlock();

                    railwayEquipManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    railwayEquipManageBlock.RailwayEquipId = DBCommon.GetInt(dr["RailwayEquipID"]);
                    railwayEquipManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    railwayEquipManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    railwayEquipManageBlock.RailwayEquipKind = dr["RailwayEquipKind"].ToString();
                    railwayEquipManageBlock.RailwayEquipType = dr["RailwayEquipType"].ToString();
                    railwayEquipManageBlock.ItemsCount = dr["ItemsCount"].ToString();
                    railwayEquipManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    railwayEquipManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    railwayEquipManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        railwayEquipManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        railwayEquipManageBlock.Ownership = "";
                    }

                    railwayEquipManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    railwayEquipManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    railwayEquipManageBlocks.Add(railwayEquipManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquipManageBlocks;
        }

        public static int GetAllRailwayEquipManageBlocksCount(RailwayEquipManageFilter filter, User currentUser)
        {
            int railwayEquipManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
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

                if (!string.IsNullOrEmpty(filter.RailwayEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RailwayEquipKindID IN (" + filter.RailwayEquipKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.RailwayEquipTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RailwayEquipTypeID IN (" + filter.RailwayEquipTypeId + ") ";
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
                                FROM PMIS_RES.RailwayEquips a
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
                        railwayEquipManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquipManageBlocksCnt;
        }

        public static List<RailwayEquipSearchBlock> GetAllRailwayEquipSearchBlocks(RailwayEquipSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<RailwayEquipSearchBlock> railwayEquipSearchBlocks = new List<RailwayEquipSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryID IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.RailwayEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RailwayEquipKindID IN (" + filter.RailwayEquipKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.RailwayEquipTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RailwayEquipTypeID IN (" + filter.RailwayEquipTypeId + ") ";
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
                        orderBySQL = "a.InventoryNumber";
                        break;
                    case 2:
                        orderBySQL = "c.TechnicsCategoryName";
                        break;
                    case 3:
                        orderBySQL = "d.TableValue";
                        break;
                    case 4:
                        orderBySQL = "e.TableValue";
                        break;
                    case 5:
                        orderBySQL = "b.ItemsCount";
                        break;
                    case 6:
                        orderBySQL = "j.CompanyName";
                        break;
                    case 7:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.InventoryNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.RailwayEquipID,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as RailwayEquipKind,
                                       d.TableValue as RailwayEquipType,
                                       b.ItemsCount,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.RailwayEquips a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.RailwayEquipKindId = d.TableKey AND d.TableName = 'RailwayEquipKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.RailwayEquipTypeId = e.TableKey AND e.TableName = 'RailwayEquipType'
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
                    RailwayEquipSearchBlock railwayEquipSearchBlock = new RailwayEquipSearchBlock();

                    railwayEquipSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    railwayEquipSearchBlock.RailwayEquipId = DBCommon.GetInt(dr["RailwayEquipID"]);
                    railwayEquipSearchBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    railwayEquipSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    railwayEquipSearchBlock.RailwayEquipKind = dr["RailwayEquipKind"].ToString();
                    railwayEquipSearchBlock.RailwayEquipType = dr["RailwayEquipType"].ToString();
                    railwayEquipSearchBlock.ItemsCount = dr["ItemsCount"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        railwayEquipSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        railwayEquipSearchBlock.Ownership = "";
                    }

                    railwayEquipSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    railwayEquipSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    railwayEquipSearchBlocks.Add(railwayEquipSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquipSearchBlocks;
        }

        public static int GetAllRailwayEquipSearchBlocksCount(RailwayEquipSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int railwayEquipSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_RAILWAY_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.InventoryNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.InventoryNumber) LIKE '%' || Upper('" + filter.InventoryNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryID IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.RailwayEquipKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RailwayEquipKindID IN (" + filter.RailwayEquipKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.RailwayEquipTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RailwayEquipTypeID IN (" + filter.RailwayEquipTypeId + ") ";
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
                                FROM PMIS_RES.RailwayEquips a
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
                        railwayEquipSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquipSearchBlocksCnt;
        }

        //Change the inventory number
        public static bool ChangeInventoryNumber(int railwayEquipId, string newInventoryNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            RailwayEquip railwayEquip = GetRailwayEquip(railwayEquipId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Инвентарен номер: " + railwayEquip.InventoryNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.RailwayEquips SET
                              InventoryNumber = :NewInventoryNumber
                           WHERE RailwayEquipID = :RailwayEquipID;

                           INSERT INTO PMIS_RES.RailwayEquipInvNumbers (RailwayEquipID, InventoryNumber, ChangeDate)
                           VALUES (:RailwayEquipID, :NewInventoryNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeInventoryNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_InventoryNumber", railwayEquip.InventoryNumber, newInventoryNumber, currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "RailwayEquipID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = railwayEquipId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NewInventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = newInventoryNumber;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ChangeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DateTime.Now;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                TechnicsUtil.SetTechnicsModified(railwayEquip.TechnicsId, currentUser);

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

        //Get all inventory number (history) by RailwayEquip with pagination
        public static List<RailwayEquipInvNumber> GetAllRailwayEquipInvNumbers(int railwayEquipId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<RailwayEquipInvNumber> railwayEquipInvNumbers = new List<RailwayEquipInvNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.RailwayEquipInvNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.InventoryNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.RailwayEquipInvNumberID) as RowNumber
                                               FROM PMIS_RES.RailwayEquipInvNumbers a
                                               WHERE a.RailwayEquipID = :RailwayEquipID
                                               ORDER BY " + orderBySQL + @", a.RailwayEquipInvNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RailwayEquipID", OracleType.Number).Value = railwayEquipId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    RailwayEquipInvNumber inventoryNumber = new RailwayEquipInvNumber();
                    inventoryNumber.InventoryNumber = dr["InventoryNumber"].ToString();
                    inventoryNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    railwayEquipInvNumbers.Add(inventoryNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return railwayEquipInvNumbers;
        }

        //Get all reg number (history) count by RailwayEquip for pagination
        public static int GetAllRailwayEquipInvNumbersCount(int railwayEquipId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.RailwayEquipInvNumbers a
                               WHERE a.RailwayEquipID = :RailwayEquipID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RailwayEquipID", OracleType.Number).Value = railwayEquipId;

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

        //Split a particular object (related to the fulfilment logic and the ItemsCount field)
        public static bool SplitRailwayEquip(int technicsId, int itemsCount, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            RailwayEquip railwayEquip = GetRailwayEquipByTechnicsId(technicsId, currentUser);

            string logDescription = "";
            logDescription += "Инвентарен номер: " + railwayEquip.InventoryNumber;

            string newInventoryNumber = "";
            bool unique = false;

            for (int i = 2; i < 100; i++)
            {
                newInventoryNumber = railwayEquip.InventoryNumber + " (" +  i.ToString()+ ")";
                RailwayEquip tmpRailwayEquip = GetRailwayEquipByInvNumber(newInventoryNumber, currentUser);

                if (tmpRailwayEquip == null)
                {
                    unique = true;
                    break;
                }
            }

            if (!unique)
                throw new Exception("Cannot generate unique inventory number");

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"DECLARE
                           NewTechnicsID number;
                           NewRailwayEquipID number;
                        BEGIN
                           INSERT INTO PMIS_RES.Technics (
                               TechnicsTypeID,
                               TechnicsCategoryID,
                               ItemsCount,
                               ResidenceCityID,
                               ResidenceDistrictID,
                               ResidenceAddress,
                               OtherInfo,
                               DriverReservistID,
                               OwnershipLeasing,
                               OwnershipCompanyID,

                               GroupManagementSection,
                               Section,
                               Deliverer,
                               PunktID,
                               
                               SourceTechnicsID,
                               
                               CreatedBy,
                               CreatedDate,
                               LastModifiedBy,
                               LastModifiedDate)
                            SELECT
                               a.TechnicsTypeID,
                               a.TechnicsCategoryID,
                               a.ItemsCount - :ItemsCount,
                               a.ResidenceCityID,
                               a.ResidenceDistrictID,
                               a.ResidenceAddress,
                               a.OtherInfo,
                               a.DriverReservistID,
                               a.OwnershipLeasing,
                               a.OwnershipCompanyID,

                               a.GroupManagementSection,
                               a.Section,
                               a.Deliverer,
                               a.PunktID,
                               
                               a.SourceTechnicsID,
                               
                               :CreatedBy,
                               :CreatedDate,
                               :LastModifiedBy,
                               :LastModifiedDate
                            FROM PMIS_RES.Technics a
                            WHERE a.TechnicsID = :TechnicsID;

                            SELECT PMIS_RES.Technics_ID_SEQ.currval INTO NewTechnicsID FROM dual;
                            
                            INSERT INTO PMIS_RES.TechnicsMilRepStatus (
                               TechnicsID,
                               IsCurrent,
                                 
                               TechMilitaryReportStatusID,
                               EnrolDate,
                               DischargeDate,
                               SourceMilDepartmentID,
                                  
                               Contract_ContractNumber,
                               Contract_ContractFromDate,
                               Contract_MilitaryUnitID,
                               Contract_ContractToDate,
                                  
                               CreatedBy,
                               CreatedDate,
                               LastModifiedBy,
                               LastModifiedDate)
                            SELECT 
                               NewTechnicsID,
                               a.IsCurrent,
                                 
                               a.TechMilitaryReportStatusID,
                               a.EnrolDate,
                               a.DischargeDate,
                               a.SourceMilDepartmentID,
                                  
                               a.Contract_ContractNumber,
                               a.Contract_ContractFromDate,
                               a.Contract_MilitaryUnitID,
                               a.Contract_ContractToDate,
                                  
                               :CreatedBy,
                               :CreatedDate,
                               :LastModifiedBy,
                               :LastModifiedDate
                            FROM PMIS_RES.TechnicsMilRepStatus a
                            WHERE a.TechnicsID = :TechnicsID AND a.IsCurrent = 1; 

                            INSERT INTO PMIS_RES.RailwayEquips (
                                TechnicsID,
                                InventoryNumber,
                                RailwayEquipKindID,
                                RailwayEquipTypeID)
                            SELECT
                                NewTechnicsID,
                                :InventoryNumber,
                                a.RailwayEquipKindID,
                                a.RailwayEquipTypeID
                            FROM PMIS_RES.RailwayEquips a
                            WHERE a.TechnicsID = :TechnicsID;

                            SELECT PMIS_RES.RailwayEquips_ID_SEQ.currval INTO NewRailwayEquipID FROM dual;

                            INSERT INTO PMIS_RES.RailwayEquipInvNumbers (
                               RailwayEquipID,
                               InventoryNumber,
                               ChangeDate)
                            VALUES (NewRailwayEquipID,
                                   :InventoryNumber,
                                   :ChangeDate);

                            UPDATE PMIS_RES.Technics SET
                               ItemsCount = :ItemsCount
                            WHERE TechnicsID = :TechnicsID;
                        END;
                        ";

                changeEvent = new ChangeEvent("RES_Technics_RAILWAY_EQUIP_Split", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_Split_Affected_InventoryNumber", "", railwayEquip.InventoryNumber, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_RAILWAY_EQUIP_Split_Created_InventoryNumber", "", newInventoryNumber, currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ChangeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DateTime.Now;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ItemsCount";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = itemsCount;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "InventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = newInventoryNumber;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                TechnicsUtil.SetTechnicsModified(technicsId, currentUser);

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
    }
}