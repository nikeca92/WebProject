using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular AviationEquip into the system
    public class AviationEquip : BaseDbObject
    {
        private int aviationEquipId;
        private int technicsId;
        private Technics technics;
        private string airInvNumber;
        private int? aviationAirKindId;
        private GTableItem aviationAirKind;
        private int? aviationAirTypeId;
        private GTableItem aviationAirType;

        //private int? aviationAirModelId;
        //private GTableItem aviationAirModel;

        private string aviationAirModelName;

        private int? airSeats;
        private decimal? airCarryingCapacity;
        private decimal? airMaxDistance;
        private DateTime? airLastTechnicalReviewDate;
        private string otherInvNumber;
        private int? aviationOtherKindId;
        private GTableItem aviationOtherKind;
        private int? aviationOtherTypeId;
        private GTableItem aviationOtherType;

        //private int? aviationOtherBaseMachineMakeId;
        //private AviationOtherBaseMachineMake aviationOtherBaseMachineMake;
        //private int? aviationOtherBaseMachineModelId;
        //private AviationOtherBaseMachineModel aviationOtherBaseMachineModel;

        private string aviationOtherBaseMachineMakeName;
        private string aviationOtherBaseMachineModelName;

        private int? aviationOtherBaseMachineKindId;
        private GTableItem aviationOtherBaseMachineKind;
        private int? aviationOtherBaseMachineTypeId;
        private GTableItem aviationOtherBaseMachineType;
        private string baseMachineMileageHoursSinceLastRepair;
        private int? aviationOtherEquipmentKindId;
        private GTableItem aviationOtherEquipmentKind;
        private string equipMileageHourSinceLstRepair;

        
        
        
        public int AviationEquipId
        {
            get
            {
                return aviationEquipId;
            }
            set
            {
                aviationEquipId = value;
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


        public string AirInvNumber
        {
            get
            {
                return airInvNumber;
            }
            set
            {
                airInvNumber = value;
            }
        }

        public int? AviationAirKindId
        {
            get
            {
                return aviationAirKindId;
            }
            set
            {
                aviationAirKindId = value;
            }
        }

        public GTableItem AviationAirKind
        {
            get
            {
                //Lazy initialization
                if (aviationAirKind == null && AviationAirKindId.HasValue)
                    aviationAirKind = GTableItemUtil.GetTableItem("AviationAirKind", AviationAirKindId.Value, ModuleUtil.RES(), CurrentUser);

                return aviationAirKind;
            }
            set
            {
                aviationAirKind = value;
            }
        }

        public int? AviationAirTypeId
        {
            get
            {
                return aviationAirTypeId;
            }
            set
            {
                aviationAirTypeId = value;
            }
        }

        public GTableItem AviationAirType
        {
            get
            {
                //Lazy initialization
                if (aviationAirType == null && AviationAirTypeId.HasValue)
                    aviationAirType = GTableItemUtil.GetTableItem("AviationAirType", AviationAirTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return aviationAirType;
            }
            set
            {
                aviationAirType = value;
            }
        }

        //public int? AviationAirModelId
        //{
        //    get
        //    {
        //        return aviationAirModelId;
        //    }
        //    set
        //    {
        //        aviationAirModelId = value;
        //    }
        //}

        //public GTableItem AviationAirModel
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (aviationAirModel == null && AviationAirModelId.HasValue)
        //            aviationAirModel = GTableItemUtil.GetTableItem("AviationAirModel", AviationAirModelId.Value, ModuleUtil.RES(), CurrentUser);

        //        return aviationAirModel;
        //    }
        //    set
        //    {
        //        aviationAirModel = value;
        //    }
        //}

        public string AviationAirModelName
        {
            get
            {
                return aviationAirModelName;
            }
            set
            {
                aviationAirModelName = value;
            }
        }

        public int? AirSeats
        {
            get
            {
                return airSeats;
            }
            set
            {
                airSeats = value;
            }
        }

        public decimal? AirCarryingCapacity
        {
            get
            {
                return airCarryingCapacity;
            }
            set
            {
                airCarryingCapacity = value;
            }
        }

        public decimal? AirMaxDistance
        {
            get
            {
                return airMaxDistance;
            }
            set
            {
                airMaxDistance = value;
            }
        }

        public DateTime? AirLastTechnicalReviewDate
        {
            get
            {
                return airLastTechnicalReviewDate;
            }
            set
            {
                airLastTechnicalReviewDate = value;
            }
        }

        public string OtherInvNumber
        {
            get
            {
                return otherInvNumber;
            }
            set
            {
                otherInvNumber = value;
            }
        }

        public int? AviationOtherKindId
        {
            get
            {
                return aviationOtherKindId;
            }
            set
            {
                aviationOtherKindId = value;
            }
        }

        public GTableItem AviationOtherKind
        {
            get
            {
                //Lazy initialization
                if (aviationOtherKind == null && AviationOtherKindId.HasValue)
                    aviationOtherKind = GTableItemUtil.GetTableItem("AviationOtherKind", AviationOtherKindId.Value, ModuleUtil.RES(), CurrentUser);

                return aviationOtherKind;
            }
            set
            {
                aviationOtherKind = value;
            }
        }

        public int? AviationOtherTypeId
        {
            get
            {
                return aviationOtherTypeId;
            }
            set
            {
                aviationOtherTypeId = value;
            }
        }

        public GTableItem AviationOtherType
        {
            get
            {
                //Lazy initialization
                if (aviationOtherType == null && AviationOtherTypeId.HasValue)
                    aviationOtherType = GTableItemUtil.GetTableItem("AviationOtherType", AviationOtherTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return aviationOtherType;
            }
            set
            {
                aviationOtherType = value;
            }
        }

        //public int? AviationOtherBaseMachineMakeId
        //{
        //    get
        //    {
        //        return aviationOtherBaseMachineMakeId;
        //    }
        //    set
        //    {
        //        aviationOtherBaseMachineMakeId = value;
        //    }
        //}

        //public AviationOtherBaseMachineMake AviationOtherBaseMachineMake
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (aviationOtherBaseMachineMake == null && AviationOtherBaseMachineMakeId.HasValue)
        //            aviationOtherBaseMachineMake = AviationOtherBaseMachineMakeUtil.GetAviationOtherBaseMachineMake(AviationOtherBaseMachineMakeId.Value, CurrentUser);

        //        return aviationOtherBaseMachineMake;
        //    }
        //    set
        //    {
        //        aviationOtherBaseMachineMake = value;
        //    }
        //}

        //public int? AviationOtherBaseMachineModelId
        //{
        //    get
        //    {
        //        return aviationOtherBaseMachineModelId;
        //    }
        //    set
        //    {
        //        aviationOtherBaseMachineModelId = value;
        //    }
        //}

        //public AviationOtherBaseMachineModel AviationOtherBaseMachineModel
        //{
        //    get
        //    {
        //        //Lazy initialization
        //        if (aviationOtherBaseMachineModel == null && AviationOtherBaseMachineModelId.HasValue)
        //            aviationOtherBaseMachineModel = AviationOtherBaseMachineModelUtil.GetAviationOtherBaseMachineModel(AviationOtherBaseMachineModelId.Value, CurrentUser);

        //        return aviationOtherBaseMachineModel;
        //    }
        //    set
        //    {
        //        aviationOtherBaseMachineModel = value;
        //    }
        //}

        public string AviationOtherBaseMachineMakeName
        {
            get
            {
                return aviationOtherBaseMachineMakeName;
            }
            set
            {
                aviationOtherBaseMachineMakeName = value;
            }
        }

        public string AviationOtherBaseMachineModelName
        {
            get
            {
                return aviationOtherBaseMachineModelName;
            }
            set
            {
                aviationOtherBaseMachineModelName = value;
            }
        }

        public int? AviationOtherBaseMachineKindId
        {
            get
            {
                return aviationOtherBaseMachineKindId;
            }
            set
            {
                aviationOtherBaseMachineKindId = value;
            }
        }

        public GTableItem AviationOtherBaseMachineKind
        {
            get
            {
                //Lazy initialization
                if (aviationOtherBaseMachineKind == null && AviationOtherBaseMachineKindId.HasValue)
                    aviationOtherBaseMachineKind = GTableItemUtil.GetTableItem("AviationOtherBaseMachineKind", AviationOtherBaseMachineKindId.Value, ModuleUtil.RES(), CurrentUser);


                return aviationOtherBaseMachineKind;
            }
            set
            {
                aviationOtherBaseMachineKind = value;
            }
        }

        public int? AviationOtherBaseMachineTypeId
        {
            get
            {
                return aviationOtherBaseMachineTypeId;
            }
            set
            {
                aviationOtherBaseMachineTypeId = value;
            }
        }

        public GTableItem AviationOtherBaseMachineType
        {
            get
            {
                //Lazy initialization
                if (aviationOtherBaseMachineType == null && AviationOtherBaseMachineTypeId.HasValue)
                    aviationOtherBaseMachineType = GTableItemUtil.GetTableItem("AviationOtherBaseMachineType", AviationOtherBaseMachineTypeId.Value, ModuleUtil.RES(), CurrentUser);

                return aviationOtherBaseMachineType;
            }
            set
            {
                aviationOtherBaseMachineType = value;
            }
        }

        public string BaseMachineMileageHoursSinceLastRepair
        {
            get
            {
                return baseMachineMileageHoursSinceLastRepair;
            }
            set
            {
                baseMachineMileageHoursSinceLastRepair = value;
            }
        }

        public int? AviationOtherEquipmentKindId
        {
            get
            {
                return aviationOtherEquipmentKindId;
            }
            set
            {
                aviationOtherEquipmentKindId = value;
            }
        }

        public GTableItem AviationOtherEquipmentKind
        {
            get
            {
                //Lazy initialization
                if (aviationOtherEquipmentKind == null && AviationOtherEquipmentKindId.HasValue)
                    aviationOtherEquipmentKind = GTableItemUtil.GetTableItem("AviationOtherEquipmentKind", AviationOtherEquipmentKindId.Value, ModuleUtil.RES(), CurrentUser);

                return aviationOtherEquipmentKind;
            }
            set
            {
                aviationOtherEquipmentKind = value;
            }
        }

        public string EquipMileageHourSinceLstRepair
        {
            get
            {
                return equipMileageHourSinceLstRepair;
            }
            set
            {
                equipMileageHourSinceLstRepair = value;
            }
        }


        

        public bool CanDelete
        {
            get { return true; }

        }

        public AviationEquip(User user)
            : base(user)
        {

        }
    }

    public class AviationEquipAirInvNumber
    {
        private string airInvNumber;
        private DateTime changeDate;

        public string AirInvNumber
        {
            get
            {
                return airInvNumber;
            }
            set
            {
                airInvNumber = value;
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
    public class AviationEquipManageFilter
    {
        string airInvNumber;
        string technicsCategoryId;
        string aviationAirKindId;
        string aviationAirTypeId;

        //string aviationAirModelId;

        string aviationAirModelName;

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

        public string AirInvNumber
        {
            get
            {
                return airInvNumber;
            }
            set
            {
                airInvNumber = value;
            }
        }

        public string TechnicsCategoryId
        {
            get { return technicsCategoryId; }
            set { technicsCategoryId = value; }
        }

        public string AviationAirKindId
        {
            get { return aviationAirKindId; }
            set { aviationAirKindId = value; }
        }

        public string AviationAirTypeId
        {
            get { return aviationAirTypeId; }
            set { aviationAirTypeId = value; }
        }

        //public string AviationAirModelId
        //{
        //    get { return aviationAirModelId; }
        //    set { aviationAirModelId = value; }
        //}

        public string AviationAirModelName
        {
            get { return aviationAirModelName; }
            set { aviationAirModelName = value; }
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

    public class AviationEquipManageBlock
    {
        private int technicsId;
        private int aviationEquipId;
        private string airInvNumber;
        string technicsCategory;
        string aviationAirKind;
        string aviationAirType;

        //string aviationAirModel;

        string aviationAirModelName;

        string ownershipNumber;
        string ownershipName;
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

        public int AviationEquipId
        {
            get { return aviationEquipId; }
            set { aviationEquipId = value; }
        }

        public string AirInvNumber
        {
            get { return airInvNumber; }
            set { airInvNumber = value; }
        }

        public string TechnicsCategory
        {
            get { return technicsCategory; }
            set { technicsCategory = value; }
        }

        public string AviationAirKind
        {
            get { return aviationAirKind; }
            set { aviationAirKind = value; }
        }

        public string AviationAirType
        {
            get { return aviationAirType; }
            set { aviationAirType = value; }
        }

        //public string AviationAirModel
        //{
        //    get { return aviationAirModel; }
        //    set { aviationAirModel = value; }
        //}

        public string AviationAirModelName
        {
            get { return aviationAirModelName; }
            set { aviationAirModelName = value; }
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

    public class AviationEquipFulfilmentBlock
    {
        private int fulfilTechnicsRequestID;        
        private int technicReadinessID;        
        private int aviationEquipID;        
        private AviationEquip aviationEquip;
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

        public int AviationEquipID
        {
            get { return aviationEquipID; }
            set { aviationEquipID = value; }
        }

        public AviationEquip AviationEquip
        {
            get { return aviationEquip; }
            set { aviationEquip = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class AviationEquipSearchFilter
    {
        string airInvNumber;
        string technicsCategoryId;
        string aviationAirKindId;
        string aviationAirTypeId;

        //string aviationAirModelId;

        string aviationAirModelName;

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

        public string AirInvNumber
        {
            get
            {
                return airInvNumber;
            }
            set
            {
                airInvNumber = value;
            }
        }

        public string TechnicsCategoryId
        {
            get { return technicsCategoryId; }
            set { technicsCategoryId = value; }
        }

        public string AviationAirKindId
        {
            get { return aviationAirKindId; }
            set { aviationAirKindId = value; }
        }

        public string AviationAirTypeId
        {
            get { return aviationAirTypeId; }
            set { aviationAirTypeId = value; }
        }

        //public string AviationAirModelId
        //{
        //    get { return aviationAirModelId; }
        //    set { aviationAirModelId = value; }
        //}

        public string AviationAirModelName
        {
            get { return aviationAirModelName; }
            set { aviationAirModelName = value; }
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

    public class AviationEquipSearchBlock
    {
        private int technicsId;
        private int aviationEquipId;
        string airInvNumber;
        string technicsCategory;
        string aviationAirKind;
        string aviationAirType;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        //string aviationAirModel;

        string aviationAirModelName;
        
        string ownership;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int AviationEquipId
        {
            get { return aviationEquipId; }
            set { aviationEquipId = value; }
        }

        public string AirInvNumber
        {
            get { return airInvNumber; }
            set { airInvNumber = value; }
        }

        public string TechnicsCategory
        {
            get { return technicsCategory; }
            set { technicsCategory = value; }
        }

        public string AviationAirKind
        {
            get { return aviationAirKind; }
            set { aviationAirKind = value; }
        }

        public string AviationAirType
        {
            get { return aviationAirType; }
            set { aviationAirType = value; }
        }

        //public string AviationAirModel
        //{
        //    get { return aviationAirModel; }
        //    set { aviationAirModel = value; }
        //}

        public string AviationAirModelName
        {
            get { return aviationAirModelName; }
            set { aviationAirModelName = value; }
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

    public static class AviationEquipUtil
    {
        //This method creates and returns a AviationEquip object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static AviationEquip ExtractAviationEquip(OracleDataReader dr, User currentUser)
        {
            AviationEquip aviationEquip = new AviationEquip(currentUser);

            aviationEquip.AviationEquipId = DBCommon.GetInt(dr["AviationEquipmentID"]);
            aviationEquip.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            aviationEquip.AirInvNumber = dr["AirInvNumber"].ToString();
            aviationEquip.AviationAirKindId = (DBCommon.IsInt(dr["AviationAirKindID"]) ? DBCommon.GetInt(dr["AviationAirKindID"]) : (int?)null);
            aviationEquip.AviationAirTypeId = (DBCommon.IsInt(dr["AviationAirTypeID"]) ? DBCommon.GetInt(dr["AviationAirTypeID"]) : (int?)null);
            
            //aviationEquip.AviationAirModelId = (DBCommon.IsInt(dr["AviationAirModelID"]) ? DBCommon.GetInt(dr["AviationAirModelID"]) : (int?)null);

            aviationEquip.AviationAirModelName = dr["AviationAirModelName"].ToString();
            
            aviationEquip.AirSeats = (DBCommon.IsInt(dr["AirSeats"]) ? DBCommon.GetInt(dr["AirSeats"]) : (int?)null);
            aviationEquip.AirCarryingCapacity = (DBCommon.IsDecimal(dr["AirCarryingCapacity"]) ? DBCommon.GetDecimal(dr["AirCarryingCapacity"]) : (decimal?)null);
            aviationEquip.AirMaxDistance = (DBCommon.IsDecimal(dr["AirMaxDistance"]) ? DBCommon.GetDecimal(dr["AirMaxDistance"]) : (decimal?)null);
            aviationEquip.AirLastTechnicalReviewDate = (dr["AirLastTechnicalReviewDate"] is DateTime ? (DateTime)dr["AirLastTechnicalReviewDate"] : (DateTime?)null);
            aviationEquip.OtherInvNumber = dr["OtherInvNumber"].ToString();
            aviationEquip.AviationOtherKindId = (DBCommon.IsInt(dr["AviationOtherKindID"]) ? DBCommon.GetInt(dr["AviationOtherKindID"]) : (int?)null);
            aviationEquip.AviationOtherTypeId = (DBCommon.IsInt(dr["AviationOtherTypeID"]) ? DBCommon.GetInt(dr["AviationOtherTypeID"]) : (int?)null);

            //aviationEquip.AviationOtherBaseMachineMakeId = (DBCommon.IsInt(dr["AviationOtherBaseMachineMakeID"]) ? DBCommon.GetInt(dr["AviationOtherBaseMachineMakeID"]) : (int?)null);
            //aviationEquip.AviationOtherBaseMachineModelId = (DBCommon.IsInt(dr["AviationOthBaseMachineModelID"]) ? DBCommon.GetInt(dr["AviationOthBaseMachineModelID"]) : (int?)null);

            aviationEquip.AviationOtherBaseMachineMakeName = dr["OtherBaseMachineMakeName"].ToString();
            aviationEquip.AviationOtherBaseMachineModelName = dr["OtherBaseMachineModelName"].ToString();

            aviationEquip.AviationOtherBaseMachineKindId = (DBCommon.IsInt(dr["AviationOtherBaseMachineKindID"]) ? DBCommon.GetInt(dr["AviationOtherBaseMachineKindID"]) : (int?)null);
            aviationEquip.AviationOtherBaseMachineTypeId = (DBCommon.IsInt(dr["AviationOtherBaseMachineTypeID"]) ? DBCommon.GetInt(dr["AviationOtherBaseMachineTypeID"]) : (int?)null);
            aviationEquip.BaseMachineMileageHoursSinceLastRepair = dr["BMMileageHoursSinceLastRepair"].ToString();
            aviationEquip.AviationOtherEquipmentKindId = (DBCommon.IsInt(dr["AviationOtherEquipmentKindID"]) ? DBCommon.GetInt(dr["AviationOtherEquipmentKindID"]) : (int?)null);
            aviationEquip.EquipMileageHourSinceLstRepair = dr["EquipMileageHourSinceLstRepair"].ToString();

            return aviationEquip;
        }

        //Get a particular object by its ID
        public static AviationEquip GetAviationEquip(int aviationEquipId, User currentUser)
        {
            AviationEquip aviationEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.AviationEquipmentID, a.TechnicsID,
                                  a.AirInvNumber,
                                  a.AviationAirKindID,
                                  a.AviationAirTypeID,
                                  a.AviationAirModelName,
                                  a.AirSeats,
                                  a.AirCarryingCapacity,
                                  a.AirMaxDistance,
                                  a.AirLastTechnicalReviewDate,
                                  a.OtherInvNumber,
                                  a.AviationOtherKindID,
                                  a.AviationOtherTypeID,
                                  a.OtherBaseMachineMakeName,
                                  a.OtherBaseMachineModelName,
                                  a.AviationOtherBaseMachineKindID,
                                  a.AviationOtherBaseMachineTypeID,
                                  a.BMMileageHoursSinceLastRepair,
                                  a.AviationOtherEquipmentKindID,
                                  a.EquipMileageHourSinceLstRepair
                               FROM PMIS_RES.AviationEquipment a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.AviationEquipmentID = :AviationEquipmentID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AviationEquipmentID", OracleType.Number).Value = aviationEquipId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    aviationEquip = ExtractAviationEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquip;
        }

        //Get a particular object by its ID
        public static AviationEquip GetAviationEquipByTechnicsId(int technicsId, User currentUser)
        {
            AviationEquip aviationEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.AviationEquipmentID, a.TechnicsID,
                                  a.AirInvNumber,
                                  a.AviationAirKindID,
                                  a.AviationAirTypeID,
                                  a.AviationAirModelName,
                                  a.AirSeats,
                                  a.AirCarryingCapacity,
                                  a.AirMaxDistance,
                                  a.AirLastTechnicalReviewDate,
                                  a.OtherInvNumber,
                                  a.AviationOtherKindID,
                                  a.AviationOtherTypeID,
                                  a.OtherBaseMachineMakeName,
                                  a.OtherBaseMachineModelName,
                                  a.AviationOtherBaseMachineKindID,
                                  a.AviationOtherBaseMachineTypeID,
                                  a.BMMileageHoursSinceLastRepair,
                                  a.AviationOtherEquipmentKindID,
                                  a.EquipMileageHourSinceLstRepair
                               FROM PMIS_RES.AviationEquipment a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.TechnicsID = :TechnicsID  
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    aviationEquip = ExtractAviationEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquip;
        }

        //Get a particular object by its reg number
        public static AviationEquip GetAviationEquipByAirInvNumber(string airInvNumber, User currentUser)
        {
            AviationEquip aviationEquip = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.AviationEquipmentID, a.TechnicsID, 
                                  a.AirInvNumber,
                                  a.AviationAirKindID,
                                  a.AviationAirTypeID,
                                  a.AviationAirModelName,
                                  a.AirSeats,
                                  a.AirCarryingCapacity,
                                  a.AirMaxDistance,
                                  a.AirLastTechnicalReviewDate,
                                  a.OtherInvNumber,
                                  a.AviationOtherKindID,
                                  a.AviationOtherTypeID,
                                  a.OtherBaseMachineMakeName,
                                  a.OtherBaseMachineModelName,
                                  a.AviationOtherBaseMachineKindID,
                                  a.AviationOtherBaseMachineTypeID,
                                  a.BMMileageHoursSinceLastRepair,
                                  a.AviationOtherEquipmentKindID,
                                  a.EquipMileageHourSinceLstRepair
                               FROM PMIS_RES.AviationEquipment a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               WHERE a.AirInvNumber = :AirInvNumber 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AirInvNumber", OracleType.VarChar).Value = airInvNumber;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    aviationEquip = ExtractAviationEquip(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquip;
        }

        //Get all AviationEquip objects
        public static List<AviationEquip> GetAllAviationEquips(User currentUser)
        {
            List<AviationEquip> aviationEquips = new List<AviationEquip>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " WHERE b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.AviationEquipmentID, a.TechnicsID, 
                                  a.AirInvNumber,
                                  a.AviationAirKindID,
                                  a.AviationAirTypeID,
                                  a.AviationAirModelName,
                                  a.AirSeats,
                                  a.AirCarryingCapacity,
                                  a.AirMaxDistance,
                                  a.AirLastTechnicalReviewDate,
                                  a.OtherInvNumber,
                                  a.AviationOtherKindID,
                                  a.AviationOtherTypeID,
                                  a.OtherBaseMachineMakeName,
                                  a.OtherBaseMachineModelName,
                                  a.AviationOtherBaseMachineKindID,
                                  a.AviationOtherBaseMachineTypeID,
                                  a.BMMileageHoursSinceLastRepair,
                                  a.AviationOtherEquipmentKindID,
                                  a.EquipMileageHourSinceLstRepair
                               FROM PMIS_RES.AviationEquipment a
                               INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                               " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    aviationEquips.Add(ExtractAviationEquip(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquips;
        }

        public static List<AviationEquipFulfilmentBlock> GetAllAviationEquipFulfilmentBlocks(int technicsRequestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<AviationEquipFulfilmentBlock> aviationEquips = new List<AviationEquipFulfilmentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
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
                        orderBySQL = "b.AirInvNumber";
                        break;
                    case 2:
                        orderBySQL = "e.TableValue";
                        break;
                    case 3:
                        orderBySQL = "e.TableValue";
                        break;
                    case 4:
                        orderBySQL = "b.AviationAirModelName";
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
                        orderBySQL = "b.AirInvNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.FulfilTechnicsRequestID,
                                      tmp.TechnicsRequestCmdPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.TechnicReadinessID,
                                      tmp.AppointmentIsDelivered,

                                      tmp.AviationEquipmentID, 
                                      tmp.TechnicsID, 
                                      tmp.AirInvNumber,
                                      tmp.AviationAirKindID,
                                      tmp.AviationAirTypeID,
                                      tmp.AviationAirModelName,
                                      tmp.AirSeats,
                                      tmp.AirCarryingCapacity,
                                      tmp.AirMaxDistance,
                                      tmp.AirLastTechnicalReviewDate,
                                      tmp.OtherInvNumber,
                                      tmp.AviationOtherKindID,
                                      tmp.AviationOtherTypeID,
                                      tmp.OtherBaseMachineMakeName,
                                      tmp.OtherBaseMachineModelName,
                                      tmp.AviationOtherBaseMachineKindID,
                                      tmp.AviationOtherBaseMachineTypeID,
                                      tmp.BMMileageHoursSinceLastRepair,
                                      tmp.AviationOtherEquipmentKindID,
                                      tmp.EquipMileageHourSinceLstRepair,
                                      
                                      tmp.RowNumber as RowNumber
                               FROM ( SELECT a.FulfilTechnicsRequestID,
                                             a.TechnicsRequestCmdPositionID,
                                             a.MilitaryDepartmentID,
                                             a.TechnicReadinessID,
                                             a.AppointmentIsDelivered,

                                             b.AviationEquipmentID, 
                                             b.TechnicsID, 
                                             b.AirInvNumber,
                                             b.AviationAirKindID,
                                             b.AviationAirTypeID,
                                             b.AviationAirModelName,
                                             b.AirSeats,
                                             b.AirCarryingCapacity,
                                             b.AirMaxDistance,
                                             b.AirLastTechnicalReviewDate,
                                             b.OtherInvNumber,
                                             b.AviationOtherKindID,
                                             b.AviationOtherTypeID,
                                             b.OtherBaseMachineMakeName,
                                             b.OtherBaseMachineModelName,
                                             b.AviationOtherBaseMachineKindID,
                                             b.AviationOtherBaseMachineTypeID,
                                             b.BMMileageHoursSinceLastRepair,
                                             b.AviationOtherEquipmentKindID,
                                             b.EquipMileageHourSinceLstRepair,

                                             RANK() OVER (ORDER BY " + orderBySQL + @", a.FulfilTechnicsRequestID) as RowNumber 
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.AviationEquipment b ON a.TechnicsID = b.TechnicsID";

                                      //LEFT OUTER JOIN PMIS_RES.AviationOtherBaseMachineMakes c ON b.AviationOtherBaseMachineMakeID = c.AviationOtherBaseMachineMakeID
                                      //LEFT OUTER JOIN PMIS_RES.AviationOtherBaseMachineModels d ON b.AviationOthBaseMachineModelID = d.AviationOthBaseMachineModelID

                SQL += @"
                                      LEFT OUTER JOIN PMIS_RES.GTable e ON b.AviationAirKindID = e.TableKey AND e.TableName = 'AviationAirKind'
                                      LEFT OUTER JOIN PMIS_RES.GTable g ON b.AviationAirTypeID = g.TableKey AND e.TableName = 'AviationAirType'
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
                    AviationEquipFulfilmentBlock block = new AviationEquipFulfilmentBlock();
                    block.FulfilTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
                    block.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
                    block.AviationEquipID = DBCommon.GetInt(dr["AviationEquipmentID"]);
                    block.AviationEquip = ExtractAviationEquip(dr, currentUser);
                    block.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    aviationEquips.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquips;
        }

        public static int GetAllAviationEquipFulfilmentBlocksCount(int technicsRequestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int aviationEquips = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string addWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    addWhere += " AND c.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_RES.FulfilTechnicsRequest a
                                      INNER JOIN PMIS_RES.AviationEquipment b ON a.TechnicsID = b.TechnicsID
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
                    aviationEquips = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquips;
        }

        //Save a particular object into the DB
        public static bool SaveAviationEquip(AviationEquip aviationEquip, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Инвентарен номер (въздухоплавателно средство): " + aviationEquip.AirInvNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (aviationEquip.AviationEquipId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.AviationEquipment (TechnicsID, AirInvNumber, 
                                AviationAirKindID,
                                AviationAirTypeID,
                                AviationAirModelName,
                                AirSeats,
                                AirCarryingCapacity,
                                AirMaxDistance,
                                AirLastTechnicalReviewDate,
                                OtherInvNumber,
                                AviationOtherKindID,
                                AviationOtherTypeID,
                                OtherBaseMachineMakeName,
                                OtherBaseMachineModelName,
                                AviationOtherBaseMachineKindID,
                                AviationOtherBaseMachineTypeID,
                                BMMileageHoursSinceLastRepair,
                                AviationOtherEquipmentKindID,
                                EquipMileageHourSinceLstRepair)
                            VALUES (:TechnicsID, :AirInvNumber, 
                                :AviationAirKindID,
                                :AviationAirTypeID,
                                :AviationAirModelName,
                                :AirSeats,
                                :AirCarryingCapacity,
                                :AirMaxDistance,
                                :AirLastTechnicalReviewDate,
                                :OtherInvNumber,
                                :AviationOtherKindID,
                                :AviationOtherTypeID,
                                :OtherBaseMachineMakeName,
                                :OtherBaseMachineModelName,
                                :AviationOtherBaseMachineKindID,
                                :AviationOtherBaseMachineTypeID,
                                :BMMileageHoursSinceLastRepair,
                                :AviationOtherEquipmentKindID,
                                :EquipMileageHourSinceLstRepair);

                            SELECT PMIS_RES.AviationEquipment_ID_SEQ.currval INTO :AviationEquipmentID FROM dual;

                            INSERT INTO PMIS_RES.AviationEquipInvNumbers (AviationEquipmentID, 
                               AirInvNumber, ChangeDate)
                            VALUES (:AviationEquipmentID, 
                               :AirInvNumber, :ChangeDate);

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_AVIATION_EQUIP_Add", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirInvNumber", "", aviationEquip.AirInvNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationAirKind", "", aviationEquip.AviationAirKindId.HasValue ? aviationEquip.AviationAirKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationAirType", "", aviationEquip.AviationAirTypeId.HasValue ? aviationEquip.AviationAirType.TableValue : "", currentUser));
                    
                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationAirModel", "", aviationEquip.AviationAirModelId.HasValue ? aviationEquip.AviationAirModel.TableValue : "", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationAirModel", "", aviationEquip.AviationAirModelName, currentUser));
                    
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirSeats", "", aviationEquip.AirSeats.HasValue ? aviationEquip.AirSeats.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirCarryingCapacity", "", aviationEquip.AirCarryingCapacity.HasValue ? aviationEquip.AirCarryingCapacity.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirMaxDistance", "", aviationEquip.AirMaxDistance.HasValue ? aviationEquip.AirMaxDistance.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirLastTechnicalReviewDate", "", CommonFunctions.FormatDate(aviationEquip.AirLastTechnicalReviewDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_OtherInvNumber", "", aviationEquip.OtherInvNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherKind", "", aviationEquip.AviationOtherKindId.HasValue ? aviationEquip.AviationOtherKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherType", "", aviationEquip.AviationOtherTypeId.HasValue ? aviationEquip.AviationOtherType.TableValue : "", currentUser));

                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineMake", "", aviationEquip.AviationOtherBaseMachineMakeName.HasValue ? aviationEquip.AviationOtherBaseMachineMake.AviationOtherBaseMachineMakeName : "", currentUser));
                    //changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineModel", "", aviationEquip.AviationOtherBaseMachineModelName.HasValue ? aviationEquip.AviationOtherBaseMachineModel.AviationOtherBaseMachineModelName : "", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineMake", "", aviationEquip.AviationOtherBaseMachineMakeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineModel", "", aviationEquip.AviationOtherBaseMachineModelName, currentUser));
                    
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineKind", "", aviationEquip.AviationOtherBaseMachineKindId.HasValue ? aviationEquip.AviationOtherBaseMachineKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineType", "", aviationEquip.AviationOtherBaseMachineTypeId.HasValue ? aviationEquip.AviationOtherBaseMachineType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_BaseMachineMileageHoursSinceLastRepair", "", aviationEquip.BaseMachineMileageHoursSinceLastRepair, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherEquipmentKind", "", aviationEquip.AviationOtherEquipmentKindId.HasValue ? aviationEquip.AviationOtherEquipmentKind.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_EquipMileageHourSinceLstRepair", "", aviationEquip.EquipMileageHourSinceLstRepair, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.AviationEquipment SET
                               AirInvNumber = :AirInvNumber,
                               AviationAirKindID = :AviationAirKindID,
                               AviationAirTypeID = :AviationAirTypeID,
                               AviationAirModelName = :AviationAirModelName,
                               AirSeats = :AirSeats,
                               AirCarryingCapacity = :AirCarryingCapacity,
                               AirMaxDistance = :AirMaxDistance,
                               AirLastTechnicalReviewDate = :AirLastTechnicalReviewDate,
                               OtherInvNumber = :OtherInvNumber,
                               AviationOtherKindID = :AviationOtherKindID,
                               AviationOtherTypeID = :AviationOtherTypeID,
                               OtherBaseMachineMakeName = :OtherBaseMachineMakeName,
                               OtherBaseMachineModelName = :OtherBaseMachineModelName,
                               AviationOtherBaseMachineKindID = :AviationOtherBaseMachineKindID,
                               AviationOtherBaseMachineTypeID = :AviationOtherBaseMachineTypeID,
                               BMMileageHoursSinceLastRepair = :BMMileageHoursSinceLastRepair,
                               AviationOtherEquipmentKindID = :AviationOtherEquipmentKindID,
                               EquipMileageHourSinceLstRepair = :EquipMileageHourSinceLstRepair
                             WHERE AviationEquipmentID = :AviationEquipmentID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_AVIATION_EQUIP_Edit", logDescription, null, null, currentUser);

                    AviationEquip oldAviationEquip = GetAviationEquip(aviationEquip.AviationEquipId, currentUser);

                    if (oldAviationEquip.AirInvNumber.Trim() != aviationEquip.AirInvNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirInvNumber", oldAviationEquip.AirInvNumber, aviationEquip.AirInvNumber, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationAirKindId, aviationEquip.AviationAirKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationAirKind", oldAviationEquip.AviationAirKindId.HasValue ? oldAviationEquip.AviationAirKind.TableValue : "", aviationEquip.AviationAirKindId.HasValue ? aviationEquip.AviationAirKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationAirTypeId, aviationEquip.AviationAirTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationAirType", oldAviationEquip.AviationAirTypeId.HasValue ? oldAviationEquip.AviationAirType.TableValue : "", aviationEquip.AviationAirTypeId.HasValue ? aviationEquip.AviationAirType.TableValue : "", currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationAirModelId, aviationEquip.AviationAirModelId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationAirModel", oldAviationEquip.AviationAirModelId.HasValue ? oldAviationEquip.AviationAirModel.TableValue : "", aviationEquip.AviationAirModelId.HasValue ? aviationEquip.AviationAirModel.TableValue : "", currentUser));

                    if (oldAviationEquip.AviationAirModelName != aviationEquip.AviationAirModelName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationAirModel", oldAviationEquip.AviationAirModelName, aviationEquip.AviationAirModelName, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldAviationEquip.AirSeats, aviationEquip.AirSeats))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirSeats", oldAviationEquip.AirSeats.HasValue ? oldAviationEquip.AirSeats.ToString() : "", aviationEquip.AirSeats.HasValue ? aviationEquip.AirSeats.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldAviationEquip.AirCarryingCapacity, aviationEquip.AirCarryingCapacity))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirCarryingCapacity", oldAviationEquip.AirCarryingCapacity.HasValue ? oldAviationEquip.AirCarryingCapacity.ToString() : "", aviationEquip.AirCarryingCapacity.HasValue ? aviationEquip.AirCarryingCapacity.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualDecimal(oldAviationEquip.AirMaxDistance, aviationEquip.AirMaxDistance))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirMaxDistance", oldAviationEquip.AirMaxDistance.HasValue ? oldAviationEquip.AirMaxDistance.ToString() : "", aviationEquip.AirMaxDistance.HasValue ? aviationEquip.AirMaxDistance.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqual(oldAviationEquip.AirLastTechnicalReviewDate, aviationEquip.AirLastTechnicalReviewDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirLastTechnicalReviewDate", CommonFunctions.FormatDate(oldAviationEquip.AirLastTechnicalReviewDate), CommonFunctions.FormatDate(aviationEquip.AirLastTechnicalReviewDate), currentUser));

                    if (oldAviationEquip.OtherInvNumber.Trim() != aviationEquip.OtherInvNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_OtherInvNumber", oldAviationEquip.OtherInvNumber, aviationEquip.OtherInvNumber, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationOtherKindId, aviationEquip.AviationOtherKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherKind", oldAviationEquip.AviationOtherKindId.HasValue ? oldAviationEquip.AviationOtherKind.TableValue : "", aviationEquip.AviationOtherKindId.HasValue ? aviationEquip.AviationOtherKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationOtherTypeId, aviationEquip.AviationOtherTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherType", oldAviationEquip.AviationOtherTypeId.HasValue ? oldAviationEquip.AviationOtherType.TableValue : "", aviationEquip.AviationOtherTypeId.HasValue ? aviationEquip.AviationOtherType.TableValue : "", currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationOtherBaseMachineMakeId, aviationEquip.AviationOtherBaseMachineMakeId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineMake", oldAviationEquip.AviationOtherBaseMachineMakeId.HasValue ? oldAviationEquip.AviationOtherBaseMachineMake.AviationOtherBaseMachineMakeName : "", aviationEquip.AviationOtherBaseMachineMakeId.HasValue ? aviationEquip.AviationOtherBaseMachineMake.AviationOtherBaseMachineMakeName : "", currentUser));

                    //if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationOtherBaseMachineModelId, aviationEquip.AviationOtherBaseMachineModelId))
                    //    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineModel", oldAviationEquip.AviationOtherBaseMachineModelId.HasValue ? oldAviationEquip.AviationOtherBaseMachineModel.AviationOtherBaseMachineModelName : "", aviationEquip.AviationOtherBaseMachineModelId.HasValue ? aviationEquip.AviationOtherBaseMachineModel.AviationOtherBaseMachineModelName : "", currentUser));

                    if (oldAviationEquip.AviationOtherBaseMachineMakeName != aviationEquip.AviationOtherBaseMachineMakeName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineMake", oldAviationEquip.AviationOtherBaseMachineMakeName, aviationEquip.AviationOtherBaseMachineMakeName, currentUser));

                    if (oldAviationEquip.AviationOtherBaseMachineModelName != aviationEquip.AviationOtherBaseMachineModelName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineModel", oldAviationEquip.AviationOtherBaseMachineModelName, aviationEquip.AviationOtherBaseMachineModelName, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationOtherBaseMachineKindId, aviationEquip.AviationOtherBaseMachineKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineKind", oldAviationEquip.AviationOtherBaseMachineKindId.HasValue ? oldAviationEquip.AviationOtherBaseMachineKind.TableValue : "", aviationEquip.AviationOtherBaseMachineKindId.HasValue ? aviationEquip.AviationOtherBaseMachineKind.TableValue : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationOtherBaseMachineTypeId, aviationEquip.AviationOtherBaseMachineTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherBaseMachineType", oldAviationEquip.AviationOtherBaseMachineTypeId.HasValue ? oldAviationEquip.AviationOtherBaseMachineType.TableValue : "", aviationEquip.AviationOtherBaseMachineTypeId.HasValue ? aviationEquip.AviationOtherBaseMachineType.TableValue : "", currentUser));

                    if (oldAviationEquip.BaseMachineMileageHoursSinceLastRepair.Trim() != aviationEquip.BaseMachineMileageHoursSinceLastRepair.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_BaseMachineMileageHoursSinceLastRepair", oldAviationEquip.BaseMachineMileageHoursSinceLastRepair, aviationEquip.BaseMachineMileageHoursSinceLastRepair, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldAviationEquip.AviationOtherEquipmentKindId, aviationEquip.AviationOtherEquipmentKindId))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AviationOtherEquipmentKind", oldAviationEquip.AviationOtherEquipmentKindId.HasValue ? oldAviationEquip.AviationOtherEquipmentKind.TableValue : "", aviationEquip.AviationOtherEquipmentKindId.HasValue ? aviationEquip.AviationOtherEquipmentKind.TableValue : "", currentUser));

                    if (oldAviationEquip.EquipMileageHourSinceLstRepair.Trim() != aviationEquip.EquipMileageHourSinceLstRepair.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_EquipMileageHourSinceLstRepair", oldAviationEquip.EquipMileageHourSinceLstRepair, aviationEquip.EquipMileageHourSinceLstRepair, currentUser));
                }

                SQL += @"END;";

                TechnicsUtil.SaveTechnics(aviationEquip.Technics, currentUser, changeEvent);
                aviationEquip.TechnicsId = aviationEquip.Technics.TechnicsId;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramAviationEquipID = new OracleParameter();
                paramAviationEquipID.ParameterName = "AviationEquipmentID";
                paramAviationEquipID.OracleType = OracleType.Number;

                if (aviationEquip.AviationEquipId != 0)
                {
                    paramAviationEquipID.Direction = ParameterDirection.Input;
                    paramAviationEquipID.Value = aviationEquip.AviationEquipId;
                }
                else
                {
                    paramAviationEquipID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramAviationEquipID);

                OracleParameter param = null;

                if (aviationEquip.AviationEquipId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = aviationEquip.TechnicsId;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "ChangeDate";
                    param.OracleType = OracleType.DateTime;
                    param.Direction = ParameterDirection.Input;
                    param.Value = DateTime.Now;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "AirInvNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(aviationEquip.AirInvNumber))
                    param.Value = aviationEquip.AirInvNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationAirKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AviationAirKindId.HasValue)
                    param.Value = aviationEquip.AviationAirKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationAirTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AviationAirTypeId.HasValue)
                    param.Value = aviationEquip.AviationAirTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "AviationAirModelID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (aviationEquip.AviationAirModelId.HasValue)
                //    param.Value = aviationEquip.AviationAirModelId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationAirModelName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(aviationEquip.AviationAirModelName))
                    param.Value = aviationEquip.AviationAirModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AirSeats";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AirSeats.HasValue)
                    param.Value = aviationEquip.AirSeats.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AirCarryingCapacity";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AirCarryingCapacity.HasValue)
                    param.Value = aviationEquip.AirCarryingCapacity.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AirMaxDistance";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AirMaxDistance.HasValue)
                    param.Value = aviationEquip.AirMaxDistance.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AirLastTechnicalReviewDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AirLastTechnicalReviewDate.HasValue)
                    param.Value = aviationEquip.AirLastTechnicalReviewDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OtherInvNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(aviationEquip.OtherInvNumber))
                    param.Value = aviationEquip.OtherInvNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationOtherKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AviationOtherKindId.HasValue)
                    param.Value = aviationEquip.AviationOtherKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationOtherTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AviationOtherTypeId.HasValue)
                    param.Value = aviationEquip.AviationOtherTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "AviationOtherBaseMachineMakeID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (aviationEquip.AviationOtherBaseMachineMakeId.HasValue)
                //    param.Value = aviationEquip.AviationOtherBaseMachineMakeId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                //param = new OracleParameter();
                //param.ParameterName = "AviationOthBaseMachineModelID";
                //param.OracleType = OracleType.Number;
                //param.Direction = ParameterDirection.Input;
                //if (aviationEquip.AviationOtherBaseMachineModelId.HasValue)
                //    param.Value = aviationEquip.AviationOtherBaseMachineModelId.Value;
                //else
                //    param.Value = DBNull.Value;
                //cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OtherBaseMachineMakeName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(aviationEquip.AviationOtherBaseMachineMakeName))
                    param.Value = aviationEquip.AviationOtherBaseMachineMakeName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OtherBaseMachineModelName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(aviationEquip.AviationOtherBaseMachineModelName))
                    param.Value = aviationEquip.AviationOtherBaseMachineModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationOtherBaseMachineKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AviationOtherBaseMachineKindId.HasValue)
                    param.Value = aviationEquip.AviationOtherBaseMachineKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationOtherBaseMachineTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AviationOtherBaseMachineTypeId.HasValue)
                    param.Value = aviationEquip.AviationOtherBaseMachineTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BMMileageHoursSinceLastRepair";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(aviationEquip.BaseMachineMileageHoursSinceLastRepair))
                    param.Value = aviationEquip.BaseMachineMileageHoursSinceLastRepair;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationOtherEquipmentKindID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (aviationEquip.AviationOtherEquipmentKindId.HasValue)
                    param.Value = aviationEquip.AviationOtherEquipmentKindId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EquipMileageHourSinceLstRepair";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(aviationEquip.EquipMileageHourSinceLstRepair))
                    param.Value = aviationEquip.EquipMileageHourSinceLstRepair;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (aviationEquip.AviationEquipId == 0)
                    aviationEquip.AviationEquipId = DBCommon.GetInt(paramAviationEquipID.Value);

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

        public static List<AviationEquipManageBlock> GetAllAviationEquipManageBlocks(AviationEquipManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<AviationEquipManageBlock> aviationEquipManageBlocks = new List<AviationEquipManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.AirInvNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.AirInvNumber) LIKE '%' || Upper('" + filter.AirInvNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.AviationAirKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AviationAirKindID IN (" + filter.AviationAirKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.AviationAirTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AviationAirTypeID IN (" + filter.AviationAirTypeId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.AviationAirModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.AviationAirModelID IN (" + filter.AviationAirModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.AviationAirModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.AviationAirModelName) LIKE '%" + filter.AviationAirModelName.ToUpper().Replace("'", "''") + "%' ";
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
                        orderBySQL = "a.AirInvNumber";
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
                        orderBySQL = "a.AviationAirModelName";
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
                        orderBySQL = "a.AirInvNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.AviationEquipmentID, 
                                       a.AirInvNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as AviationEquipKind,
                                       e.TableValue as AviationAirType,
                                       a.AviationAirModelName,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.AviationEquipment a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.AviationAirKindID = d.TableKey AND d.TableName = 'AviationAirKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.AviationAirTypeID = e.TableKey AND e.TableName = 'AviationAirType'";

                                //LEFT OUTER JOIN PMIS_RES.GTable f ON a.AviationAirModelID = f.TableKey AND f.TableName = 'AviationAirModel'

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
                    AviationEquipManageBlock aviationEquipManageBlock = new AviationEquipManageBlock();

                    aviationEquipManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    aviationEquipManageBlock.AviationEquipId = DBCommon.GetInt(dr["AviationEquipmentID"]);
                    aviationEquipManageBlock.AirInvNumber = dr["AirInvNumber"].ToString();
                    aviationEquipManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    aviationEquipManageBlock.AviationAirKind = dr["AviationEquipKind"].ToString();
                    aviationEquipManageBlock.AviationAirType = dr["AviationAirType"].ToString();
                    
                    //aviationEquipManageBlock.AviationAirModel = dr["AviationAirModel"].ToString();

                    aviationEquipManageBlock.AviationAirModelName = dr["AviationAirModelName"].ToString();
                    
                    aviationEquipManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    aviationEquipManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    aviationEquipManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        aviationEquipManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        aviationEquipManageBlock.Ownership = "";
                    }

                    aviationEquipManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    aviationEquipManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    aviationEquipManageBlocks.Add(aviationEquipManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquipManageBlocks;
        }

        public static int GetAllAviationEquipManageBlocksCount(AviationEquipManageFilter filter, User currentUser)
        {
            int aviationEquipManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.AirInvNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.AirInvNumber) LIKE '%' || Upper('" + filter.AirInvNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.AviationAirKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AviationAirKindID IN (" + filter.AviationAirKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.AviationAirTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AviationAirTypeID IN (" + filter.AviationAirTypeId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.AviationAirModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.AviationAirModelID IN (" + filter.AviationAirModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.AviationAirModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.AviationAirModelName) LIKE '%" + filter.AviationAirModelName.ToUpper().Replace("'", "''") + "%' ";
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
                                FROM PMIS_RES.AviationEquipment a
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
                        aviationEquipManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquipManageBlocksCnt;
        }

        public static List<AviationEquipSearchBlock> GetAllAviationEquipSearchBlocks(AviationEquipSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, int rowsPerPage, User currentUser)
        {
            List<AviationEquipSearchBlock> aviationEquipSearchBlocks = new List<AviationEquipSearchBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.AirInvNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.AirInvNumber) LIKE '%' || Upper('" + filter.AirInvNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.AviationAirKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AviationAirKindID IN (" + filter.AviationAirKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.AviationAirTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AviationAirTypeID IN (" + filter.AviationAirTypeId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.AviationAirModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.AviationAirModelID IN (" + filter.AviationAirModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.AviationAirModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.AviationAirModelName) LIKE '%" + filter.AviationAirModelName.ToUpper().Replace("'", "''") + "%' ";
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
                        orderBySQL = "a.AirInvNumber";
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
                        orderBySQL = "j.CompanyName";
                        break;
                    case 6:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.AirInvNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.AviationEquipmentID, 
                                       a.AirInvNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as AviationAirKind,
                                       e.TableValue as AviationAirType,
                                       a.AviationAirModelName,
                                       j.CompanyName, j.UnifiedIdentityCode,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.AviationEquipment a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                INNER JOIN PMIS_RES.TechnicsMilRepStatus ts ON b.TechnicsID = ts.TechnicsID AND ts.IsCurrent = 1 AND ts.SourceMilDepartmentID = " + militaryDepartmentID.ToString() + @"
                                INNER JOIN PMIS_RES.TechMilitaryReportStatuses s ON ts.TechMilitaryReportStatusID = s.TechMilitaryReportStatusID AND 
                                                                                    s.TechMilitaryReportStatusKey IN (" + TechnicsUtil.SearchTechMilRepStatuses() + @")
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.AviationAirKindID = d.TableKey AND d.TableName = 'AviationAirKind'
                                LEFT OUTER JOIN PMIS_RES.GTable e ON a.AviationAirTypeID = e.TableKey AND e.TableName = 'AviationAirType'";

                                //LEFT OUTER JOIN PMIS_RES.GTable f ON a.AviationAirModelID = f.TableKey AND f.TableName = 'AviationAirModel'

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
                    AviationEquipSearchBlock aviationEquipSearchBlock = new AviationEquipSearchBlock();

                    aviationEquipSearchBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    aviationEquipSearchBlock.AviationEquipId = DBCommon.GetInt(dr["AviationEquipmentID"]);
                    aviationEquipSearchBlock.AirInvNumber = dr["AirInvNumber"].ToString();
                    aviationEquipSearchBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    aviationEquipSearchBlock.AviationAirKind = dr["AviationAirKind"].ToString();
                    aviationEquipSearchBlock.AviationAirType = dr["AviationAirType"].ToString();
                    
                    //aviationEquipSearchBlock.AviationAirModel = dr["AviationAirModel"].ToString();

                    aviationEquipSearchBlock.AviationAirModelName = dr["AviationAirModelName"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        aviationEquipSearchBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        aviationEquipSearchBlock.Ownership = "";
                    }

                    aviationEquipSearchBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    aviationEquipSearchBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    aviationEquipSearchBlocks.Add(aviationEquipSearchBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquipSearchBlocks;
        }

        public static int GetAllAviationEquipSearchBlocksCount(AviationEquipSearchFilter filter, int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int aviationEquipSearchBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_TECHNICS_AVIATION_EQUIP", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }


                if (!string.IsNullOrEmpty(filter.AirInvNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " Upper(a.AirInvNumber) LIKE '%' || Upper('" + filter.AirInvNumber.Replace("'", "''") + "') || '%' ";
                }

                if (!string.IsNullOrEmpty(filter.TechnicsCategoryId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TechnicsCategoryId IN (" + filter.TechnicsCategoryId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.AviationAirKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AviationAirKindID IN (" + filter.AviationAirKindId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.AviationAirTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AviationAirTypeID IN (" + filter.AviationAirTypeId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.AviationAirModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.AviationAirModelID IN (" + filter.AviationAirModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.AviationAirModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.AviationAirModelName) LIKE '%" + filter.AviationAirModelName.ToUpper().Replace("'", "''") + "%' ";
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
                                FROM PMIS_RES.AviationEquipment a
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
                        aviationEquipSearchBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquipSearchBlocksCnt;
        }

        //Change the reg number
        public static bool ChangeAirInvNumber(int aviationEquipId, string newAirInvNumber, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            AviationEquip aviationEquip = GetAviationEquip(aviationEquipId, currentUser);

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Инвентарен номер (въздухоплавателно средство): " + aviationEquip.AirInvNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.AviationEquipment SET
                              AirInvNumber = :NewAirInvNumber
                           WHERE AviationEquipmentID = :AviationEquipmentID;

                           INSERT INTO PMIS_RES.AviationEquipInvNumbers (AviationEquipmentID, 
                              AirInvNumber, ChangeDate)
                           VALUES (:AviationEquipmentID, 
                              :NewAirInvNumber, :ChangeDate);
                        END;
                       ";

                changeEvent = new ChangeEvent("RES_Technics_ChangeInventoryNumber", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_AVIATION_EQUIP_AirInvNumber", aviationEquip.AirInvNumber, newAirInvNumber, currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "AviationEquipmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = aviationEquipId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NewAirInvNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = newAirInvNumber;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ChangeDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = DateTime.Now;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                TechnicsUtil.SetTechnicsModified(aviationEquip.TechnicsId, currentUser);

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

        //Get all air inv number (history) by AviationEquip with pagination
        public static List<AviationEquipAirInvNumber> GetAllAviationEquipAirInvNumbers(int aviationEquipId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<AviationEquipAirInvNumber> aviationEquipAirInvNumbers = new List<AviationEquipAirInvNumber>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.AviationEquipInvNumberID";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.AirInvNumber,
                                                      a.ChangeDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.AviationEquipInvNumberID) as RowNumber
                                               FROM PMIS_RES.AviationEquipInvNumbers a
                                               WHERE a.AviationEquipmentID = :AviationEquipmentID
                                               ORDER BY " + orderBySQL + @", a.AviationEquipInvNumberID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AviationEquipmentID", OracleType.Number).Value = aviationEquipId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    AviationEquipAirInvNumber aviationEquipAirInvNumber = new AviationEquipAirInvNumber();
                    aviationEquipAirInvNumber.AirInvNumber = dr["AirInvNumber"].ToString();
                    aviationEquipAirInvNumber.ChangeDate = (DateTime)dr["ChangeDate"];

                    aviationEquipAirInvNumbers.Add(aviationEquipAirInvNumber);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationEquipAirInvNumbers;
        }

        //Get all air inv number (history) count by AviationEquip for pagination
        public static int GetAllAviationEquipAirInvNumbersCount(int aviationEquipId, User currentUser)
        {
            int count = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.AviationEquipInvNumbers a
                               WHERE a.AviationEquipmentID = :AviationEquipmentID
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AviationEquipmentID", OracleType.Number).Value = aviationEquipId;

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