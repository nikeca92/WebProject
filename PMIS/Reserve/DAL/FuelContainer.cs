using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular FuelContainer into the system
    public class FuelContainer : BaseDbObject
    {
        private int fuelContainerId;
        private int technicsId;
        private Technics technics;
        private string inventoryNumber;
        private int? fuelContainerKindId;
        private GTableItem fuelContainerKind;
        private int? fuelContainerTypeId;
        private GTableItem fuelContainerType;

        public int FuelContainerId
        {
            get
            {
                return fuelContainerId;
            }
            set
            {
                fuelContainerId = value;
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
                if (technics == null)
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

        public int? FuelContainerKindId
        {
            get
            {
                return fuelContainerKindId;
            }
            set
            {
                fuelContainerKindId = value;
            }
        }

        public GTableItem FuelContainerKind
        {
            get
            {
                //Lazy initialization
                if (fuelContainerKind == null && FuelContainerKindId.HasValue)
                    fuelContainerKind = GTableItemUtil.GetTableItem("FuelContainerKind", FuelContainerKindId.Value, ModuleUtil.RES(), CurrentUser);

                return fuelContainerKind;
            }
            set
            {
                fuelContainerKind = value;
            }
        }

        public int? FuelContainerTypeId
        {
            get
            {
                return fuelContainerTypeId;
            }
            set
            {
                fuelContainerTypeId = value;
            }
        }

        public GTableItem FuelContainerType
        {
            get
            {
                //Lazy initialization
                if (fuelContainerType == null && FuelContainerTypeId.HasValue)
                    fuelContainerType = GTableItemUtil.GetTableItem("FuelContainerType", FuelContainerTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return fuelContainerType;
            }
            set
            {
                fuelContainerType = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public FuelContainer(User user)
            : base(user)
        {

        }
    }

    public class FuelContainerInvNumber
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
    public class FuelContainerManageFilter
    {
        string inventoryNumber;
        string technicsCategoryId;
        string fuelContainerKindId;
        string fuelContainerTypeId;
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

        public string FuelContainerKindId
        {
            get { return fuelContainerKindId; }
            set { fuelContainerKindId = value; }
        }

        public string FuelContainerTypeId
        {
            get { return fuelContainerTypeId; }
            set { fuelContainerTypeId = value; }
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

    public class FuelContainerManageBlock
    {
        private int technicsId;
        private int fuelContainerId;
        string inventoryNumber;
        string technicsCategory;
        string fuelContainerKind;
        string fuelContainerType;
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

        public int FuelContainerId
        {
            get { return fuelContainerId; }
            set { fuelContainerId = value; }
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

        public string FuelContainerKind
        {
            get { return fuelContainerKind; }
            set { fuelContainerKind = value; }
        }

        public string FuelContainerType
        {
            get { return fuelContainerType; }
            set { fuelContainerType = value; }
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

    public class FuelContainerFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;
        private int technicReadinessID;
        private int itemsCount;
        private int fuelContainerID;
        private FuelContainer fuelContainer;
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

        public int FuelContainerID
        {
            get { return fuelContainerID; }
            set { fuelContainerID = value; }
        }

        public FuelContainer FuelContainer
        {
            get { return fuelContainer; }
            set { fuelContainer = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class FuelContainerSearchFilter
    {
        string inventoryNumber;
        string technicsCategoryId;
        string fuelContainerKindId;
        string fuelContainerTypeId;
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

        public string FuelContainerKindId
        {
            get { return fuelContainerKindId; }
            set { fuelContainerKindId = value; }
        }

        public string FuelContainerTypeId
        {
            get { return fuelContainerTypeId; }
            set { fuelContainerTypeId = value; }
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

    public class FuelContainerSearchBlock
    {
        private int technicsId;
        private int fuelContainerId;
        string inventoryNumber;
        string technicsCategory;
        string fuelContainerKind;
        string fuelContainerType;
        string itemsCount;
        string ownership;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int FuelContainerId
        {
            get { return fuelContainerId; }
            set { fuelContainerId = value; }
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

        public string FuelContainerKind
        {
            get { return fuelContainerKind; }
            set { fuelContainerKind = value; }
        }

        public string FuelContainerType
        {
            get { return fuelContainerType; }
            set { fuelContainerType = value; }
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

    public static class FuelContainerUtil
    {
        //This method creates and returns a FuelContainer object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static FuelContainer ExtractFuelContainer(OracleDataReader dr, User currentUser)
        {
            FuelContainer fuelContainer = new FuelContainer(currentUser);

            fuelContainer.FuelContainerId = DBCommon.GetInt(dr["FuelContainerID"]);
            fuelContainer.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            fuelContainer.InventoryNumber = dr["InventoryNumber"].ToString();
            fuelContainer.FuelContainerKindId = (DBCommon.IsInt(dr["FuelContainerKindID"]) ? DBCommon.GetInt(dr["FuelContainerKindID"]) : (int?)null);
            fuelContainer.FuelContainerTypeId = (DBCommon.IsInt(dr["FuelContainerTypeID"]) ? DBCommon.GetInt(dr["FuelContainerTypeID"]) : (int?)null);

            return fuelContainer;
        }

        //Get a particular object by its ID
        public static FuelContainer GetFuelContainer(int fuelContainerId, User currentUser)
        {
            FuelContainer fuelContainer = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.FuelContainerID, a.TechnicsID, a.InventoryNumber,
                                  a.FuelContainerKindID, a.FuelContainerTypeID
                               FROM PMIS_RES.FuelContainers a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.FuelContainerID = :FuelContainerID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FuelContainerID", OracleType.Number).Value = fuelContainerId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    fuelContainer = ExtractFuelContainer(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainer;
        }

        //Get a particular object by its ID
        public static FuelContainer GetFuelContainerByTechnicsId(int technicsId, User currentUser)
        {
            FuelContainer fuelContainer = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.FuelContainerID, a.TechnicsID, a.InventoryNumber,
                                  a.FuelContainerKindID, a.FuelContainerTypeID
                               FROM PMIS_RES.FuelContainers a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID  
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    fuelContainer = ExtractFuelContainer(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainer;
        }

        //Get a particular object by its inventory number
        public static FuelContainer GetFuelContainerByInvNumber(string inventoryNumber, User currentUser)
        {
            FuelContainer fuelContainer = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.FuelContainerID, a.TechnicsID, a.InventoryNumber,
                                  a.FuelContainerKindID, a.FuelContainerTypeID
                               FROM PMIS_RES.FuelContainers a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.InventoryNumber = :InventoryNumber
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("InventoryNumber", OracleType.VarChar).Value = inventoryNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    fuelContainer = ExtractFuelContainer(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainer;
        }

        //Get all FuelContainer objects
        public static List<FuelContainer> GetAllFuelContainers(User currentUser)
        {
            List<FuelContainer> fuelContainers = new List<FuelContainer>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.FuelContainerID, a.TechnicsID, a.InventoryNumber,
                                  a.FuelContainerKindID, a.FuelContainerTypeID
                               FROM PMIS_RES.FuelContainers a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    fuelContainers.Add(ExtractFuelContainer(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainers;
        }

        public static List<FuelContainerFulfilmentBlock> GetAllFuelContainerFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<FuelContainerFulfilmentBlock> fuelContainers = new List<FuelContainerFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                                      tmp.FuelContainerID, 
                                      tmp.TechnicsID,
                                      tmp.InventoryNumber,
                                      tmp.FuelContainerKindID,
                                      tmp.FuelContainerTypeID,
                                      
                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             e.ItemsCount,
                                             a.AppointmentIsDelivered,

                                             b.FuelContainerID, 
                                             b.TechnicsID,
                                             b.InventoryNumber,
                                             b.FuelContainerKindID, 
                                             b.FuelContainerTypeID, 
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.FuelContainers b ON a.TechnicsID = b.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.GTable c ON b.FuelContainerKindID = c.TableKey AND c.TableName = 'FuelContainerKind'
                                      LEFT OUTER JOIN PMIS_RES.GTable d ON b.FuelContainerTypeID = d.TableKey AND d.TableName = 'FuelContainerType'
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
                    FuelContainerFulfilmentBlock block = new FuelContainerFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.ItemsCount = DBCommon.GetInt(dr["ItemsCount"]);
                    block.FuelContainerID = DBCommon.GetInt(dr["FuelContainerID"]);
                    block.FuelContainer = ExtractFuelContainer(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    fuelContainers.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainers;
        }

        public static int GetAllFuelContainerFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int fuelContainersCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.FuelContainers b ON a.TechnicsID = b.TechnicsID
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
                    fuelContainersCount = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainersCount;
        }

        //Save a particular object into the DB
        public static bool SaveFuelContainer(FuelContainer fuelContainer, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Инвентарен номер: " + fuelContainer.InventoryNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (fuelContainer.FuelContainerId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.FuelContainers (TechnicsID, InventoryNumber, FuelContainerKindID, FuelContainerTypeID)
                            VALUES (:TechnicsID, :InventoryNumber, :FuelContainerKindID, :FuelContainerTypeID);

                            SELECT PMIS_RES.FuelContainers_ID_SEQ.currval INTO :FuelContainerID FROM dual;

                            INSERT INTO PMIS_RES.FuelContainerInvNumbers (FuelContainerID, InventoryNumber, ChangeDate)
                            VALUES (:FuelContainerID, :InventoryNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_FUEL_CONTAINERS_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_InventoryNumber", "", fuelContainer.InventoryNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_FuelContainerKind", "", fuelContainer.FuelContainerKindId.HasValue ? fuelContainer.FuelContainerKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_FuelContainerType", "", fuelContainer.FuelContainerTypeId.HasValue ? fuelContainer.FuelContainerType.TableValue : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.FuelContainers SET
                               InventoryNumber = :InventoryNumber,
                               FuelContainerKindID = :FuelContainerKindID,
                               FuelContainerTypeID = :FuelContainerTypeID
                             WHERE FuelContainerID = :FuelContainerID ;

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_FUEL_CONTAINERS_Edit", logDescription, null, null, currentUser);

                    FuelContainer oldFuelContainer = GetFuelContainer(fuelContainer.FuelContainerId, currentUser);

                    if (oldFuelContainer.InventoryNumber.Trim() != fuelContainer.InventoryNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_InventoryNumber", oldFuelContainer.InventoryNumber, fuelContainer.InventoryNumber, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldFuelContainer.FuelContainerKindId, fuelContainer.FuelContainerKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_FuelContainerKind", oldFuelContainer.FuelContainerKindId.HasValue ? oldFuelContainer.FuelContainerKind.TableValue : "", fuelContainer.FuelContainerKindId.HasValue ? fuelContainer.FuelContainerKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldFuelContainer.FuelContainerTypeId, fuelContainer.FuelContainerTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_FuelContainerType", oldFuelContainer.FuelContainerTypeId.HasValue ? oldFuelContainer.FuelContainerType.TableValue : "", fuelContainer.FuelContainerTypeId.HasValue ? fuelContainer.FuelContainerType.TableValue : "", currentUser));
                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(fuelContainer.Technics, currentUser, changeEvent);
                fuelContainer.TechnicsId = fuelContainer.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramFuelContainerID = new OracleParameter();
                paramFuelContainerID.ParameterName = "FuelContainerID";
                paramFuelContainerID.OracleType = OracleType.Number;

                if (fuelContainer.FuelContainerId != 0)
                {
                    paramFuelContainerID.Direction = ParameterDirection.Input;
                    paramFuelContainerID.Value = fuelContainer.FuelContainerId;
                }
                else
                {
                    paramFuelContainerID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramFuelContainerID);

                OracleParameter param = null;

                if (fuelContainer.FuelContainerId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = fuelContainer.TechnicsId;
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
                if (!string.IsNullOrEmpty(fuelContainer.InventoryNumber))
                    param.Value = fuelContainer.InventoryNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FuelContainerKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (fuelContainer.FuelContainerKindId.HasValue)
                    param.Value = fuelContainer.FuelContainerKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FuelContainerTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (fuelContainer.FuelContainerTypeId.HasValue)
                    param.Value = fuelContainer.FuelContainerTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (fuelContainer.FuelContainerId == 0)
                    fuelContainer.FuelContainerId = DBCommon.GetInt(paramFuelContainerID.Value);

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

        public static List<FuelContainerManageBlock> GetAllFuelContainerManageBlocks(FuelContainerManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<FuelContainerManageBlock> fuelContainerManageBlocks = new List<FuelContainerManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.FuelContainerKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.FuelContainerKindID IN (" + filter.FuelContainerKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.FuelContainerTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.FuelContainerTypeID IN (" + filter.FuelContainerTypeId + ") ";
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
                                       a.FuelContainerID,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as FuelContainerKind,
                                       e.TableValue as FuelContainerType,
                                       b.ItemsCount,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.FuelContainers a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.FuelContainerKindId = d.TableKey AND d.TableName = 'FuelContainerKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.FuelContainerTypeId = e.TableKey AND e.TableName = 'FuelContainerType'
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
                    FuelContainerManageBlock fuelContainerManageBlock = new FuelContainerManageBlock();

                    fuelContainerManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    fuelContainerManageBlock.FuelContainerId = DBCommon.GetInt(dr["FuelContainerID"]);
                    fuelContainerManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    fuelContainerManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    fuelContainerManageBlock.FuelContainerKind = dr["FuelContainerKind"].ToString();
                    fuelContainerManageBlock.FuelContainerType = dr["FuelContainerType"].ToString();
                    fuelContainerManageBlock.ItemsCount = dr["ItemsCount"].ToString();
                    fuelContainerManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    fuelContainerManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    fuelContainerManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        fuelContainerManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        fuelContainerManageBlock.Ownership = "";
                    }

                    fuelContainerManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    fuelContainerManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    fuelContainerManageBlocks.Add(fuelContainerManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainerManageBlocks;
        }

        public static int GetAllFuelContainerManageBlocksCount(FuelContainerManageFilter filter, User currentUser)
        {
            int fuelContainerManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.FuelContainerKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.FuelContainerKindID IN (" + filter.FuelContainerKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.FuelContainerTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.FuelContainerTypeID IN (" + filter.FuelContainerTypeId + ") ";
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
                                FROM PMIS_RES.FuelContainers a
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
                        fuelContainerManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainerManageBlocksCnt;
        }

        public static List<FuelContainerSearchBlock> GetAllFuelContainerSearchBlocks(FuelContainerSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<FuelContainerSearchBlock> fuelContainerSearchBlocks = new List<FuelContainerSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.FuelContainerKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.FuelContainerKindID IN (" + filter.FuelContainerKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.FuelContainerTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.FuelContainerTypeID IN (" + filter.FuelContainerTypeId + ") ";
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
                                       a.FuelContainerID,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as FuelContainerKind,
                                       d.TableValue as FuelContainerType,
                                       b.ItemsCount,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.FuelContainers a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.FuelContainerKindId = d.TableKey AND d.TableName = 'FuelContainerKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.FuelContainerTypeId = e.TableKey AND e.TableName = 'FuelContainerType'
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
                    FuelContainerSearchBlock fuelContainerSearchBlock = new FuelContainerSearchBlock();

                    fuelContainerSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    fuelContainerSearchBlock.FuelContainerId = DBCommon.GetInt(dr["FuelContainerID"]);
                    fuelContainerSearchBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    fuelContainerSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    fuelContainerSearchBlock.FuelContainerKind = dr["FuelContainerKind"].ToString();
                    fuelContainerSearchBlock.FuelContainerType = dr["FuelContainerType"].ToString();
                    fuelContainerSearchBlock.ItemsCount = dr["ItemsCount"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        fuelContainerSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        fuelContainerSearchBlock.Ownership = "";
                    }

                    fuelContainerSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    fuelContainerSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    fuelContainerSearchBlocks.Add(fuelContainerSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainerSearchBlocks;
        }

        public static int GetAllFuelContainerSearchBlocksCount(FuelContainerSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int fuelContainerSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_FUEL_CONTAINERS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.FuelContainerKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.FuelContainerKindID IN (" + filter.FuelContainerKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.FuelContainerTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.FuelContainerTypeID IN (" + filter.FuelContainerTypeId + ") ";
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
                                FROM PMIS_RES.FuelContainers a
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
                        fuelContainerSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainerSearchBlocksCnt;
        }

        //Change the inventory number
        public static bool ChangeInventoryNumber(int fuelContainerId, string newInventoryNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            FuelContainer fuelContainer = GetFuelContainer(fuelContainerId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Инвентарен номер: " + fuelContainer.InventoryNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.FuelContainers SET
                              InventoryNumber = :NewInventoryNumber
                           WHERE FuelContainerID = :FuelContainerID;

                           INSERT INTO PMIS_RES.FuelContainerInvNumbers (FuelContainerID, InventoryNumber, ChangeDate)
                           VALUES (:FuelContainerID, :NewInventoryNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeInventoryNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_InventoryNumber", fuelContainer.InventoryNumber, newInventoryNumber, currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "FuelContainerID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fuelContainerId;
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

                TechnicsUtil.SetTechnicsModified(fuelContainer.TechnicsId, currentUser);

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

        //Get all inventory number (history) by FuelContainer with pagination
        public static List<FuelContainerInvNumber> GetAllFuelContainerInvNumbers(int fuelContainerId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<FuelContainerInvNumber> fuelContainerInvNumbers = new List<FuelContainerInvNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.FuelContainerInvNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.InventoryNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.FuelContainerInvNumberID) as RowNumber
                                               FROM PMIS_RES.FuelContainerInvNumbers a
                                               WHERE a.FuelContainerID = :FuelContainerID
                                               ORDER BY " + orderBySQL + @", a.FuelContainerInvNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FuelContainerID", OracleType.Number).Value = fuelContainerId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    FuelContainerInvNumber inventoryNumber = new FuelContainerInvNumber();
                    inventoryNumber.InventoryNumber = dr["InventoryNumber"].ToString();
                    inventoryNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    fuelContainerInvNumbers.Add(inventoryNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fuelContainerInvNumbers;
        }

        //Get all reg number (history) count by FuelContainer for pagination
        public static int GetAllFuelContainerInvNumbersCount(int fuelContainerId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.FuelContainerInvNumbers a
                               WHERE a.FuelContainerID = :FuelContainerID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FuelContainerID", OracleType.Number).Value = fuelContainerId;

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
        public static bool SplitFuelContainer(int technicsId, int itemsCount, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            FuelContainer fuelContainer = GetFuelContainerByTechnicsId(technicsId, currentUser);

            string logDescription = "";
            logDescription += "Инвентарен номер: " + fuelContainer.InventoryNumber;

            string newInventoryNumber = "";
            bool unique = false;

            for (int i = 2; i < 100; i++)
            {
                newInventoryNumber = fuelContainer.InventoryNumber + " (" + i.ToString() + ")";
                FuelContainer tmpFuelContainer = GetFuelContainerByInvNumber(newInventoryNumber, currentUser);

                if (tmpFuelContainer == null)
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
                           NewFuelContainerID number;
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

                            INSERT INTO PMIS_RES.FuelContainers (
                                TechnicsID,
                                InventoryNumber,
                                FuelContainerKindID,
                                FuelContainerTypeID)
                            SELECT
                                NewTechnicsID,
                                :InventoryNumber,
                                a.FuelContainerKindID,
                                a.FuelContainerTypeID
                            FROM PMIS_RES.FuelContainers a
                            WHERE a.TechnicsID = :TechnicsID;

                            SELECT PMIS_RES.FuelContainers_ID_SEQ.currval INTO NewFuelContainerID FROM dual;

                            INSERT INTO PMIS_RES.FuelContainerInvNumbers (
                               FuelContainerID,
                               InventoryNumber,
                               ChangeDate)
                            VALUES (NewFuelContainerID,
                                   :InventoryNumber,
                                   :ChangeDate);

                            UPDATE PMIS_RES.Technics SET
                               ItemsCount = :ItemsCount
                            WHERE TechnicsID = :TechnicsID;
                        END;
                        ";

                changeEvent = new ChangeEvent("RES_Technics_FUEL_CONTAINERS_Split", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_Split_Affected_InventoryNumber", "", fuelContainer.InventoryNumber, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_FUEL_CONTAINERS_Split_Created_InventoryNumber", "", newInventoryNumber, currentUser));

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