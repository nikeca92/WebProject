using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Vessel into the system
    public class Vessel : BaseDbObject
    {
        private int vesselId;
        private int technicsId;
        private Technics technics;        
        private string inventoryNumber;        
        private int? vesselKindId;
        private GTableItem vesselKind;
        private int? vesselTypeId;
        private GTableItem vesselType;
        private string vesselName;        
        private decimal? loadDisplacement;        
        private decimal? lightDisplacement;        
        private decimal? length;        
        private decimal? width;        
        private decimal? maxHeight;        
        private decimal? maxWadeLoad;        
        private decimal? maxWadeLight;        
        private int? officers;        
        private int? sailors;        
        private decimal? enginePower;        
        private decimal? speedNodes;        
        private DateTime? stopDate;        
        private string stopReasons;        
        
        public int VesselId
        {
            get
            {
                return vesselId;
            }
            set
            {
                vesselId = value;
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

        public int? VesselKindId
        {
            get
            {
                return vesselKindId;
            }
            set
            {
                vesselKindId = value;
            }
        }

        public GTableItem VesselKind
        {
            get
            {
                //Lazy initialization
                if (vesselKind == null && VesselKindId.HasValue)
                    vesselKind = GTableItemUtil.GetTableItem("VesselKind", VesselKindId.Value, ModuleUtil.RES(), CurrentUser);

                return vesselKind;
            }
            set
            {
                vesselKind = value;
            }
        }

        public int? VesselTypeId
        {
            get
            {
                return vesselTypeId;
            }
            set
            {
                vesselTypeId = value;
            }
        }

        public GTableItem VesselType
        {
            get
            {
                //Lazy initialization
                if (vesselType == null && VesselTypeId.HasValue)
                    vesselType = GTableItemUtil.GetTableItem("VesselType", VesselTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return vesselType;
            }
            set
            {
                vesselType = value;
            }
        }

        public string VesselName
        {
            get { return vesselName; }
            set { vesselName = value; }
        }

        public decimal? LoadDisplacement
        {
            get { return loadDisplacement; }
            set { loadDisplacement = value; }
        }

        public decimal? LightDisplacement
        {
            get { return lightDisplacement; }
            set { lightDisplacement = value; }
        }

        public decimal? Length
        {
            get { return length; }
            set { length = value; }
        }

        public decimal? Width
        {
            get { return width; }
            set { width = value; }
        }

        public decimal? MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }

        public decimal? MaxWadeLoad
        {
            get { return maxWadeLoad; }
            set { maxWadeLoad = value; }
        }

        public decimal? MaxWadeLight
        {
            get { return maxWadeLight; }
            set { maxWadeLight = value; }
        }
        public int? Officers
        {
            get { return officers; }
            set { officers = value; }
        }

        public int? Sailors
        {
            get { return sailors; }
            set { sailors = value; }
        }

        public decimal? EnginePower
        {
            get { return enginePower; }
            set { enginePower = value; }
        }
        public decimal? SpeedNodes
        {
            get { return speedNodes; }
            set { speedNodes = value; }
        }

        public DateTime? StopDate
        {
            get { return stopDate; }
            set { stopDate = value; }
        }

        public string StopReasons
        {
            get { return stopReasons; }
            set { stopReasons = value; }
        }    

        public bool CanDelete
        {
            get { return true; }

        }

        public Vessel(User user)
            : base(user)
        {

        }
    }

    public class VesselInventoryNumber
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
    public class VesselManageFilter
    {
        string vesselName;
        string inventoryNumber;
        string technicsCategoryId;
        string vesselKindId;
        string vesselTypeId;
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

        public string VesselName
        {
            get
            {
                return vesselName;
            }
            set
            {
                vesselName = value;
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

        public string VesselKindId
        {
            get { return vesselKindId; }
            set { vesselKindId = value; }
        }

        public string VesselTypeId
        {
            get { return vesselTypeId; }
            set { vesselTypeId = value; }
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

    public class VesselManageBlock
    {
        private int technicsId;
        private int vesselId;
        private string vesselName;
        string inventoryNumber;
        string technicsCategory;
        string vesselKind;
        string vesselType;
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

        public int VesselId
        {
            get { return vesselId; }
            set { vesselId = value; }
        }

        public string VesselName
        {
            get { return vesselName; }
            set { vesselName = value; }
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

        public string VesselKind
        {
            get { return vesselKind; }
            set { vesselKind = value; }
        }

        public string VesselType
        {
            get { return vesselType; }
            set { vesselType = value; }
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

    public class VesselFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;        
        private int technicReadinessID;        
        private int vesselID;        
        private Vessel vessel;
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

        public int VesselID
        {
            get { return vesselID; }
            set { vesselID = value; }
        }

        public Vessel Vessel
        {
            get { return vessel; }
            set { vessel = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class VesselSearchFilter
    {
        string vesselName;
        string inventoryNumber;
        string technicsCategoryId;
        string vesselKindId;
        string vesselTypeId;
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

        public string VesselName
        {
            get
            {
                return vesselName;
            }
            set
            {
                vesselName = value;
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

        public string VesselKindId
        {
            get { return vesselKindId; }
            set { vesselKindId = value; }
        }

        public string VesselTypeId
        {
            get { return vesselTypeId; }
            set { vesselTypeId = value; }
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

    public class VesselSearchBlock
    {
        private int technicsId;
        private int vesselId;
        private string vesselName;
        string inventoryNumber;
        string technicsCategory;
        string vesselKind;
        string vesselType;
        string ownership;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int VesselId
        {
            get { return vesselId; }
            set { vesselId = value; }
        }

        public string VesselName
        {
            get { return vesselName; }
            set { vesselName = value; }
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

        public string VesselKind
        {
            get { return vesselKind; }
            set { vesselKind = value; }
        }

        public string VesselType
        {
            get { return vesselType; }
            set { vesselType = value; }
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

    public static class VesselUtil
    {
        //This method creates and returns a Vessel object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static Vessel ExtractVessel(OracleDataReader dr, User currentUser)
        {
            Vessel vessel = new Vessel(currentUser);

            vessel.VesselId = DBCommon.GetInt(dr["VesselID"]);
            vessel.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);            
            vessel.InventoryNumber = dr["InventoryNumber"].ToString();            
            vessel.VesselKindId = (DBCommon.IsInt(dr["VesselKindID"]) ? DBCommon.GetInt(dr["VesselKindID"]) : (int?)null);
            vessel.VesselTypeId = (DBCommon.IsInt(dr["VesselTypeID"]) ? DBCommon.GetInt(dr["VesselTypeID"]) : (int?)null);
            vessel.VesselName = dr["VesselName"].ToString();
            vessel.LoadDisplacement = (DBCommon.IsDecimal(dr["LoadDisplacement"]) ? DBCommon.GetDecimal(dr["LoadDisplacement"]) : (decimal?)null);
            vessel.LightDisplacement = (DBCommon.IsDecimal(dr["LightDisplacement"]) ? DBCommon.GetDecimal(dr["LightDisplacement"]) : (decimal?)null);
            vessel.Length = (DBCommon.IsDecimal(dr["Length"]) ? DBCommon.GetDecimal(dr["Length"]) : (decimal?)null);
            vessel.Width = (DBCommon.IsDecimal(dr["Width"]) ? DBCommon.GetDecimal(dr["Width"]) : (decimal?)null);
            vessel.MaxHeight = (DBCommon.IsDecimal(dr["MaxHeight"]) ? DBCommon.GetDecimal(dr["MaxHeight"]) : (decimal?)null);
            vessel.MaxWadeLoad = (DBCommon.IsDecimal(dr["MaxWadeLoad"]) ? DBCommon.GetDecimal(dr["MaxWadeLoad"]) : (decimal?)null);
            vessel.MaxWadeLight = (DBCommon.IsDecimal(dr["MaxWadeLight"]) ? DBCommon.GetDecimal(dr["MaxWadeLight"]) : (decimal?)null);
            vessel.Officers = (DBCommon.IsInt(dr["Officers"]) ? DBCommon.GetInt(dr["Officers"]) : (int?)null);
            vessel.Sailors = (DBCommon.IsInt(dr["Sailors"]) ? DBCommon.GetInt(dr["Sailors"]) : (int?)null);
            vessel.EnginePower = (DBCommon.IsDecimal(dr["EnginePower"]) ? DBCommon.GetDecimal(dr["EnginePower"]) : (decimal?)null);
            vessel.SpeedNodes = (DBCommon.IsDecimal(dr["SpeedNodes"]) ? DBCommon.GetDecimal(dr["SpeedNodes"]) : (decimal?)null);
            vessel.StopDate = (dr["StopDate"] is DateTime ? (DateTime)dr["StopDate"] : (DateTime?)null);
            vessel.StopReasons = dr["StopReasons"].ToString();            

            return vessel;
        }

        //Get a particular object by its ID
        public static Vessel GetVessel(int vesselId, User currentUser)
        {
            Vessel vessel = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VesselID, a.TechnicsID, a.InventoryNumber,
                                      a.VesselKindID, a.VesselTypeID, a.VesselName,
                                      a.LoadDisplacement, a.LightDisplacement, a.Length, a.Width,
                                      a.MaxHeight, a.MaxWadeLoad, a.MaxWadeLight, a.Officers,
                                      a.Sailors, a.EnginePower, a.SpeedNodes, a.StopDate, a.StopReasons
                               FROM PMIS_RES.Vessels a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.VesselID = :VesselID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VesselID", OracleType.Number).Value = vesselId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vessel = ExtractVessel(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vessel;
        }

        //Get a particular object by its ID
        public static Vessel GetVesselByTechnicsId(int technicsId, User currentUser)
        {
            Vessel vessel = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VesselID, a.TechnicsID, a.InventoryNumber,
                                      a.VesselKindID, a.VesselTypeID, a.VesselName,
                                      a.LoadDisplacement, a.LightDisplacement, a.Length, a.Width,
                                      a.MaxHeight, a.MaxWadeLoad, a.MaxWadeLight, a.Officers,
                                      a.Sailors, a.EnginePower, a.SpeedNodes, a.StopDate, a.StopReasons
                               FROM PMIS_RES.Vessels a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID  
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vessel = ExtractVessel(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vessel;
        }

        //Get a particular object by its reg number
        public static Vessel GetVesselByInventoryNumber(string inventoryNumber, User currentUser)
        {
            Vessel vessel = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VesselID, a.TechnicsID, a.InventoryNumber,
                                      a.VesselKindID, a.VesselTypeID, a.VesselName,
                                      a.LoadDisplacement, a.LightDisplacement, a.Length, a.Width,
                                      a.MaxHeight, a.MaxWadeLoad, a.MaxWadeLight, a.Officers,
                                      a.Sailors, a.EnginePower, a.SpeedNodes, a.StopDate, a.StopReasons
                               FROM PMIS_RES.Vessels a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.InventoryNumber = :InventoryNumber 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("InventoryNumber", OracleType.VarChar).Value = inventoryNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vessel = ExtractVessel(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vessel;
        }

        //Get all Vessel objects
        public static List<Vessel> GetAllVessels(User currentUser)
        {
            List<Vessel> vessels = new List<Vessel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VesselID, a.TechnicsID, a.InventoryNumber,
                                      a.VesselKindID, a.VesselTypeID, a.VesselName,
                                      a.LoadDisplacement, a.LightDisplacement, a.Length, a.Width,
                                      a.MaxHeight, a.MaxWadeLoad, a.MaxWadeLight, a.Officers,
                                      a.Sailors, a.EnginePower, a.SpeedNodes, a.StopDate, a.StopReasons
                               FROM PMIS_RES.Vessels a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vessels.Add(ExtractVessel(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vessels;
        }

        public static List<VesselFulfilmentBlock> GetAllVesselFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<VesselFulfilmentBlock> vessels = new List<VesselFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
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
                        orderBySQL = "b.VesselName";
                        break;
                    case 2:
                        orderBySQL = "b.InventoryNumber";
                        break;
                    case 3:
                        orderBySQL = "c.TableValue";
                        break;
                    case 4:
                        orderBySQL = "d.TableValue";
                        break;
                    case 5:
                        orderBySQL = "a.TechnicReadinessID";
                        break;
                    case 6:
                        orderBySQL = "n.NormativeCode";
                        break;
                    case 7:
                        orderBySQL = "a.AppointmentIsDelivered";
                        break;
                    default:
                        orderBySQL = "b.VesselName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.FulfilTechnicsRequestID,
                                      tmp.TechnicsRequestCmdPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.TechnicReadinessID,
                                      tmp.AppointmentIsDelivered,

                                      tmp.VesselID, 
                                      tmp.TechnicsID, 
                                      tmp.InventoryNumber,
                                      tmp.VesselKindID, 
                                      tmp.VesselTypeID, 
                                      tmp.VesselName,
                                      tmp.LoadDisplacement, 
                                      tmp.LightDisplacement, 
                                      tmp.Length, 
                                      tmp.Width,
                                      tmp.MaxHeight, 
                                      tmp.MaxWadeLoad, 
                                      tmp.MaxWadeLight, 
                                      tmp.Officers,
                                      tmp.Sailors, 
                                      tmp.EnginePower, 
                                      tmp.SpeedNodes, 
                                      tmp.StopDate, 
                                      tmp.StopReasons,

                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             a.AppointmentIsDelivered,

                                             b.VesselID, 
                                             b.TechnicsID, 
                                             b.InventoryNumber,
                                             b.VesselKindID, 
                                             b.VesselTypeID, 
                                             b.VesselName,
                                             b.LoadDisplacement, 
                                             b.LightDisplacement, 
                                             b.Length, 
                                             b.Width,
                                             b.MaxHeight, 
                                             b.MaxWadeLoad, 
                                             b.MaxWadeLight, 
                                             b.Officers,
                                             b.Sailors, 
                                             b.EnginePower, 
                                             b.SpeedNodes, 
                                             b.StopDate, 
                                             b.StopReasons,
                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.Vessels b ON a.TechnicsID = b.TechnicsID
                                      LEFT OUTER JOIN PMIS_RES.GTable c ON b.VesselKindID = c.TableKey AND c.TableName = 'VesselKind'
                                      LEFT OUTER JOIN PMIS_RES.GTable d ON b.VesselKindID = d.TableKey AND d.TableName = 'VesselType'
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
                    VesselFulfilmentBlock block = new VesselFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.VesselID = DBCommon.GetInt(dr["VesselID"]);
                    block.Vessel = ExtractVessel(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    vessels.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vessels;
        }

        public static int GetAllVesselFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int vessels = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.Vessels b ON a.TechnicsID = b.TechnicsID
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
                    vessels = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vessels;
        }

        //Save a particular object into the DB
        public static bool SaveVessel(Vessel vessel, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Инвентарен номер: " + vessel.InventoryNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (vessel.VesselId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.Vessels (VesselID, TechnicsID, InventoryNumber,
                                                           VesselKindID, VesselTypeID, VesselName,
                                                           LoadDisplacement, LightDisplacement, Length, Width,
                                                           MaxHeight, MaxWadeLoad, MaxWadeLight, Officers,
                                                           Sailors, EnginePower, SpeedNodes, StopDate, StopReasons)
                            VALUES (:VesselID, :TechnicsID, :InventoryNumber,
                                    :VesselKindID, :VesselTypeID, :VesselName,
                                    :LoadDisplacement, :LightDisplacement, :Length, :Width,
                                    :MaxHeight, :MaxWadeLoad, :MaxWadeLight, :Officers,
                                    :Sailors, :EnginePower, :SpeedNodes, :StopDate, :StopReasons);

                            SELECT PMIS_RES.Vessels_ID_SEQ.currval INTO :VesselID FROM dual;

                            INSERT INTO PMIS_RES.VesselInvNumbers (VesselID, InventoryNumber, ChangeDate)
                            VALUES (:VesselID, :InventoryNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_VESSELS_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselName", "", vessel.VesselName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_InventoryNumber", "", vessel.InventoryNumber, currentUser));                    
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselKind", "", vessel.VesselKindId.HasValue ? vessel.VesselKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselType", "", vessel.VesselTypeId.HasValue ? vessel.VesselType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_LoadDisplacement", "", vessel.LoadDisplacement.HasValue ? CommonFunctions.FormatDecimal(vessel.LoadDisplacement.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_LightDisplacement", "", vessel.LightDisplacement.HasValue ? CommonFunctions.FormatDecimal(vessel.LightDisplacement.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_Length", "", vessel.Length.HasValue ? CommonFunctions.FormatDecimal(vessel.Length.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_Width", "", vessel.Width.HasValue ? CommonFunctions.FormatDecimal(vessel.Width.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_MaxHeight", "", vessel.MaxHeight.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxHeight.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_MaxWadeLoad", "", vessel.MaxWadeLoad.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxWadeLoad.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_MaxWadeLight", "", vessel.MaxWadeLight.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxWadeLight.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_Officers", "", vessel.Officers.HasValue ? vessel.Officers.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_Sailors", "", vessel.Sailors.HasValue ? vessel.Sailors.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_EnginePower", "", vessel.EnginePower.HasValue ? CommonFunctions.FormatDecimal(vessel.EnginePower.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_SpeedNodes", "", vessel.SpeedNodes.HasValue ? CommonFunctions.FormatDecimal(vessel.SpeedNodes.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_StopDate", "", CommonFunctions.FormatDate(vessel.StopDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_StopReasons", "", vessel.StopReasons, currentUser));                  
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.Vessels SET                              
                               InventoryNumber = :InventoryNumber,
                               VesselKindID = :VesselKindID, 
                               VesselTypeID = :VesselTypeID, 
                               VesselName = :VesselName,
                               LoadDisplacement = :LoadDisplacement, 
                               LightDisplacement = :LightDisplacement, 
                               Length = :Length, 
                               Width = :Width,
                               MaxHeight = :MaxHeight, 
                               MaxWadeLoad = :MaxWadeLoad, 
                               MaxWadeLight = :MaxWadeLight,    
                               Officers = :Officers,
                               Sailors = :Sailors, 
                               EnginePower = :EnginePower, 
                               SpeedNodes = :SpeedNodes, 
                               StopDate = :StopDate, 
                               StopReasons = :StopReasons                     
                             WHERE VesselID = :VesselID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_VESSELS_Edit", logDescription, null, null, currentUser);

                    Vessel oldVessel = GetVessel(vessel.VesselId, currentUser);

                    if (oldVessel.VesselName.Trim() != vessel.VesselName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselName", oldVessel.VesselName, vessel.VesselName, currentUser));

                    if (oldVessel.InventoryNumber.Trim() != vessel.InventoryNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_InventoryNumber", oldVessel.InventoryNumber, vessel.InventoryNumber, currentUser));
                 
                    if (!CommonFunctions.IsEqualInt(oldVessel.VesselKindId, vessel.VesselKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselKind", oldVessel.VesselKindId.HasValue ? oldVessel.VesselKind.TableValue : "", vessel.VesselKindId.HasValue ? vessel.VesselKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldVessel.VesselTypeId, vessel.VesselTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselType", oldVessel.VesselTypeId.HasValue ? oldVessel.VesselType.TableValue : "", vessel.VesselTypeId.HasValue ? vessel.VesselType.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.LoadDisplacement, vessel.LoadDisplacement))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_LoadDisplacement", oldVessel.LoadDisplacement.HasValue ? CommonFunctions.FormatDecimal(oldVessel.LoadDisplacement.Value) : "", vessel.LoadDisplacement.HasValue ? CommonFunctions.FormatDecimal(vessel.LoadDisplacement.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.LightDisplacement, vessel.LightDisplacement))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_LightDisplacement", oldVessel.LightDisplacement.HasValue ? CommonFunctions.FormatDecimal(oldVessel.LightDisplacement.Value) : "", vessel.LightDisplacement.HasValue ? CommonFunctions.FormatDecimal(vessel.LightDisplacement.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.Length, vessel.Length))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_Length", oldVessel.Length.HasValue ? CommonFunctions.FormatDecimal(oldVessel.Length.Value) : "", vessel.Length.HasValue ? CommonFunctions.FormatDecimal(vessel.Length.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.Width, vessel.Width))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_Width", oldVessel.Width.HasValue ? CommonFunctions.FormatDecimal(oldVessel.Width.Value) : "", vessel.Width.HasValue ? CommonFunctions.FormatDecimal(vessel.Width.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.MaxHeight, vessel.MaxHeight))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_MaxHeight", oldVessel.MaxHeight.HasValue ? CommonFunctions.FormatDecimal(oldVessel.MaxHeight.Value) : "", vessel.MaxHeight.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxHeight.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.MaxWadeLoad, vessel.MaxWadeLoad))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_MaxWadeLoad", oldVessel.MaxWadeLoad.HasValue ? CommonFunctions.FormatDecimal(oldVessel.MaxWadeLoad.Value) : "", vessel.MaxWadeLoad.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxWadeLoad.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.MaxWadeLight, vessel.MaxWadeLight))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_MaxWadeLight", oldVessel.MaxWadeLight.HasValue ? CommonFunctions.FormatDecimal(oldVessel.MaxWadeLight.Value) : "", vessel.MaxWadeLight.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxWadeLight.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldVessel.Officers, vessel.Officers))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_Officers", oldVessel.Officers.HasValue ? oldVessel.Officers.Value.ToString() : "", vessel.Officers.HasValue ? vessel.Officers.Value.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldVessel.Sailors, vessel.Sailors))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_Sailors", oldVessel.Sailors.HasValue ? oldVessel.Sailors.Value.ToString() : "", vessel.Sailors.HasValue ? vessel.Sailors.Value.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.EnginePower, vessel.EnginePower))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_EnginePower", oldVessel.EnginePower.HasValue ? CommonFunctions.FormatDecimal(oldVessel.EnginePower.Value) : "", vessel.EnginePower.HasValue ? CommonFunctions.FormatDecimal(vessel.EnginePower.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldVessel.SpeedNodes, vessel.SpeedNodes))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_SpeedNodes", oldVessel.SpeedNodes.HasValue ? CommonFunctions.FormatDecimal(oldVessel.SpeedNodes.Value) : "", vessel.SpeedNodes.HasValue ? CommonFunctions.FormatDecimal(vessel.SpeedNodes.Value) : "", currentUser));

                    if (!CommonFunctions.IsEqual(oldVessel.StopDate, vessel.StopDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_StopDate", CommonFunctions.FormatDate(oldVessel.StopDate), CommonFunctions.FormatDate(vessel.StopDate), currentUser));

                    if (oldVessel.StopReasons.Trim() != vessel.StopReasons.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_StopReasons", oldVessel.StopReasons, vessel.StopReasons, currentUser));

                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(vessel.Technics, currentUser, changeEvent);
                vessel.TechnicsId = vessel.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramVesselID = new OracleParameter();
                paramVesselID.ParameterName = "VesselID";
                paramVesselID.OracleType = OracleType.Number;

                if (vessel.VesselId != 0)
                {
                    paramVesselID.Direction = ParameterDirection.Input;
                    paramVesselID.Value = vessel.VesselId;
                }
                else
                {
                    paramVesselID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramVesselID);

                OracleParameter param = null;

                if (vessel.VesselId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = vessel.TechnicsId;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "ChangeDate";
                    param.OracleType = OracleType.DateTime;
                    param.Direction = ParameterDirection.Input;
                    param.Value = DateTime.Now;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "VesselName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vessel.VesselName))
                    param.Value = vessel.VesselName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "InventoryNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vessel.InventoryNumber))
                    param.Value = vessel.InventoryNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);            

                param = new OracleParameter();
                param.ParameterName = "VesselKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.VesselKindId.HasValue)
                    param.Value = vessel.VesselKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VesselTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.VesselTypeId.HasValue)
                    param.Value = vessel.VesselTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LoadDisplacement";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.LoadDisplacement.HasValue)
                    param.Value = vessel.LoadDisplacement.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LightDisplacement";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.LightDisplacement.HasValue)
                    param.Value = vessel.LightDisplacement.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Length";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.Length.HasValue)
                    param.Value = vessel.Length.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Width";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.Width.HasValue)
                    param.Value = vessel.Width.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MaxHeight";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.MaxHeight.HasValue)
                    param.Value = vessel.MaxHeight.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MaxWadeLoad";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.MaxWadeLoad.HasValue)
                    param.Value = vessel.MaxWadeLoad.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MaxWadeLight";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.MaxWadeLight.HasValue)
                    param.Value = vessel.MaxWadeLight.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Officers";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.Officers.HasValue)
                    param.Value = vessel.Officers.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Sailors";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.Sailors.HasValue)
                    param.Value = vessel.Sailors.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EnginePower";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.EnginePower.HasValue)
                    param.Value = vessel.EnginePower.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SpeedNodes";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vessel.SpeedNodes.HasValue)
                    param.Value = vessel.SpeedNodes.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "StopDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (vessel.StopDate.HasValue)
                    param.Value = vessel.StopDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "StopReasons";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vessel.StopReasons))
                    param.Value = vessel.StopReasons;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (vessel.VesselId == 0)
                    vessel.VesselId = DBCommon.GetInt(paramVesselID.Value);

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

        public static List<VesselManageBlock> GetAllVesselManageBlocks(VesselManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<VesselManageBlock> vesselManageBlocks = new List<VesselManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.VesselName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselName LIKE '%" + filter.VesselName.Replace("'", "''") + "%' ";
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

                if (!string.IsNullOrEmpty(filter.VesselKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselKindId IN (" + filter.VesselKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.VesselTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselTypeId IN (" + filter.VesselTypeId + ") ";
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
                        orderBySQL = "a.VesselName";
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
                        orderBySQL = "a.VesselName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.VesselID, 
                                       a.VesselName,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as VesselKind,
                                       e.TableValue as VesselType,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Vessels a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.VesselKindId = d.TableKey AND d.TableName = 'VesselKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.VesselKindId = e.TableKey AND e.TableName = 'VesselType'                                
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
                    VesselManageBlock vesselManageBlock = new VesselManageBlock();

                    vesselManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    vesselManageBlock.VesselId = DBCommon.GetInt(dr["VesselID"]);
                    vesselManageBlock.VesselName = dr["VesselName"].ToString();
                    vesselManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    vesselManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    vesselManageBlock.VesselKind = dr["VesselKind"].ToString();
                    vesselManageBlock.VesselType = dr["VesselType"].ToString();
                    vesselManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    vesselManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    vesselManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        vesselManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        vesselManageBlock.Ownership = "";
                    }

                    vesselManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    vesselManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    vesselManageBlocks.Add(vesselManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselManageBlocks;
        }

        public static int GetAllVesselManageBlocksCount(VesselManageFilter filter, User currentUser)
        {
            int vesselManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.VesselName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselName LIKE '%" + filter.VesselName.Replace("'", "''") + "%' ";
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

                if (!string.IsNullOrEmpty(filter.VesselKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselKindId IN (" + filter.VesselKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.VesselTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselTypeId IN (" + filter.VesselTypeId + ") ";
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
                                FROM PMIS_RES.Vessels a
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
                        vesselManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselManageBlocksCnt;
        }

        public static List<VesselSearchBlock> GetAllVesselSearchBlocks(VesselSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<VesselSearchBlock> vesselSearchBlocks = new List<VesselSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.VesselName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselName LIKE '%" + filter.VesselName.Replace("'", "''") + "%' ";
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

                if (!string.IsNullOrEmpty(filter.VesselKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselKindId IN (" + filter.VesselKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.VesselTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselTypeId IN (" + filter.VesselTypeId + ") ";
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
                        orderBySQL = "a.VesselName";
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
                        orderBySQL = "a.VesselName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.VesselID, 
                                       a.VesselName,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as VesselKind,
                                       e.TableValue as VesselType,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Vessels a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.VesselKindId = d.TableKey AND d.TableName = 'VesselKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.VesselKindId = e.TableKey AND e.TableName = 'VesselType'
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
                    VesselSearchBlock vesselSearchBlock = new VesselSearchBlock();

                    vesselSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    vesselSearchBlock.VesselId = DBCommon.GetInt(dr["VesselID"]);
                    vesselSearchBlock.VesselName = dr["VesselName"].ToString();
                    vesselSearchBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    vesselSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    vesselSearchBlock.VesselKind = dr["VesselKind"].ToString();
                    vesselSearchBlock.VesselType = dr["VesselType"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        vesselSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        vesselSearchBlock.Ownership = "";
                    }

                    vesselSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    vesselSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    vesselSearchBlocks.Add(vesselSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselSearchBlocks;
        }

        public static int GetAllVesselSearchBlocksCount(VesselSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int vesselSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_VESSELS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.VesselName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselName LIKE '%" + filter.VesselName.Replace("'", "''") + "%' ";
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

                if (!string.IsNullOrEmpty(filter.VesselKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselKindId IN (" + filter.VesselKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.VesselTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VesselTypeId IN (" + filter.VesselTypeId + ") ";
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
                                FROM PMIS_RES.Vessels a
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
                        vesselSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselSearchBlocksCnt;
        }

        //Change the reg number
        public static bool ChangeInventoryNumber(int vesselId, string newInventoryNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            Vessel vessel = GetVessel(vesselId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Инвентарен номер: " + vessel.InventoryNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.Vessels SET
                              InventoryNumber = :NewInventoryNumber
                           WHERE VesselID = :VesselID;

                           INSERT INTO PMIS_RES.VesselInvNumbers (VesselID, 
                              InventoryNumber, ChangeDate)
                           VALUES (:VesselID, 
                              :NewInventoryNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeInventoryNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_InventoryNumber", vessel.InventoryNumber, newInventoryNumber, currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "VesselID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vesselId;
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

                TechnicsUtil.SetTechnicsModified(vessel.TechnicsId, currentUser);

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

        //Get all reg number (history) by Vessel with pagination
        public static List<VesselInventoryNumber> GetAllVesselInventoryNumbers(int vesselId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<VesselInventoryNumber> vesselInventoryNumbers = new List<VesselInventoryNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.VesselInvNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.InventoryNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.VesselInvNumberID) as RowNumber
                                               FROM PMIS_RES.VesselInvNumbers a
                                               WHERE a.VesselID = :VesselID
                                               ORDER BY " + orderBySQL + @", a.VesselInvNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VesselID", OracleType.Number).Value = vesselId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    VesselInventoryNumber inventoryNumber = new VesselInventoryNumber();
                    inventoryNumber.InventoryNumber = dr["InventoryNumber"].ToString();
                    inventoryNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    vesselInventoryNumbers.Add(inventoryNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselInventoryNumbers;
        }

        //Get all reg number (history) count by Vessel for pagination
        public static int GetAllVesselInventoryNumbersCount(int vesselId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.VesselInvNumbers a
                               WHERE a.VesselID = :VesselID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VesselID", OracleType.Number).Value = vesselId;

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