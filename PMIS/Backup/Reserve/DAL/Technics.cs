using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Technics into the system
    public class Technics : BaseDbObject
    {
        private int technicsId;
        private TechnicsType technicsType;
        private int? technicsCategoryId;
        private TechnicsCategory technicsCategory;
        private int itemsCount;
        private int? normativeTechnicsId;
        private NormativeTechnics normativeTechnics;
        private string residencePostCode;        
        private int? residenceCityId;
        private City residenceCity;
        private int? residenceDistrictId;
        private District residenceDistrict;
        private string residenceAddress = "";
        private string otherInfo = "";
        private int? driverReservistId;
        private Reservist driverReservist;
        private bool ownershipLeasing;
        private int? ownershipCompanyId;
        private Company ownershipCompany;
        private string groupManagementSection;
        private string section;
        private string deliverer;
        private int? punktID;
        private TechnicsRequestCommandPunkt punkt;
        private TechnicsMilRepStatus currTechMilRepStatus;


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

        public TechnicsType TechnicsType
        {
            get
            {
                return technicsType;
            }
            set
            {
                technicsType = value;
            }
        }

        public int? TechnicsCategoryId
        {
            get
            {
                return technicsCategoryId;
            }
            set
            {
                technicsCategoryId = value;
            }
        }

        public TechnicsCategory TechnicsCategory
        {
            get
            {
                //Lazy initialization
                if (technicsCategory == null && TechnicsCategoryId.HasValue)
                    technicsCategory = TechnicsCategoryUtil.GetTechnicsCategory(TechnicsCategoryId.Value, CurrentUser);

                return technicsCategory;
            }
            set
            {
                technicsCategory = value;
            }
        }

        public int ItemsCount
        {
            get
            {
                return itemsCount;
            }
            set
            {
                itemsCount = value;
            }
        }

        public int? NormativeTechnicsId
        {
            get { return normativeTechnicsId; }
            set { normativeTechnicsId = value; }
        }

        public NormativeTechnics NormativeTechnics
        {
            get
            {
                if (normativeTechnics == null && NormativeTechnicsId.HasValue)
                {
                    normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsObj(base.CurrentUser, NormativeTechnicsId.Value);
                }
                return normativeTechnics;

            }
            set { normativeTechnics = value; }
        }

        public string ResidencePostCode
        {
            get { return residencePostCode; }
            set { residencePostCode = value; }
        }

        public int? ResidenceCityId
        {
            get { return residenceCityId; }
            set { residenceCityId = value; }
        }

        public City ResidenceCity
        {
            get
            {
                if (residenceCity == null && ResidenceCityId != null)
                {
                    residenceCity = CityUtil.GetCity((int)ResidenceCityId, base.CurrentUser);
                }
                return residenceCity;

            }
            set { residenceCity = value; }
        }

        public int? ResidenceDistrictId
        {
            get { return residenceDistrictId; }
            set { residenceDistrictId = value; }
        }

        public District ResidenceDistrict
        {
            get
            {
                if (residenceDistrict == null && ResidenceDistrictId != null)
                {
                    residenceDistrict = DistrictUtil.GetDistrict((int)ResidenceDistrictId, base.CurrentUser);
                }
                return residenceDistrict;

            }
            set { residenceDistrict = value; }
        }

        public string ResidenceAddress
        {
            get { return residenceAddress; }
            set { residenceAddress = value; }
        }

        public string OtherInfo
        {
            get { return otherInfo; }
            set { otherInfo = value; }
        }

        public int? DriverReservistId
        {
            get
            {
                return driverReservistId;
            }
            set
            {
                driverReservistId = value;
            }
        }

        public Reservist DriverReservist
        {
            get
            {
                //Lazy initialization
                if (driverReservist == null && DriverReservistId.HasValue)
                    driverReservist = ReservistUtil.GetReservist(DriverReservistId.Value, CurrentUser);

                return driverReservist;
            }
            set
            {
                driverReservist = value;
            }
        }

        public bool OwnershipLeasing
        {
            get
            {
                return ownershipLeasing;
            }
            set
            {
                ownershipLeasing = value;
            }
        }

        public int? OwnershipCompanyId
        {
            get
            {
                return ownershipCompanyId;
            }
            set
            {
                ownershipCompanyId = value;
            }
        }

        public Company OwnershipCompany
        {
            get
            {
                if (ownershipCompany == null && OwnershipCompanyId.HasValue)
                    ownershipCompany = CompanyUtil.GetCompany(OwnershipCompanyId.Value, CurrentUser);

                return ownershipCompany;
            }
            set
            {
                ownershipCompany = value;
            }
        }

        public string OwnerDisplayText
        {
            get
            {
                string owner = "";

                if (OwnershipCompanyId.HasValue)
                {
                    owner = OwnershipCompany.CompanyName + " (" + (OwnershipCompany.OwnershipType != null && OwnershipCompany.OwnershipType.OwnershipTypeKey == "PERSON" ? "ЕГН" : CommonFunctions.GetLabelText("UnifiedIdentityCode")) + ": " + OwnershipCompany.UnifiedIdentityCode + ")";
                }

                return owner;
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

        public TechnicsRequestCommandPunkt Punkt
        {
            get
            {
                if (punkt == null && punktID.HasValue)
                    punkt = TechnicsRequestCommandPunktUtil.GetTechnicsRequestCommandPunkt(punktID.Value, CurrentUser);

                return punkt;
            }
            set { punkt = value; }
        }

        public TechnicsMilRepStatus CurrTechMilRepStatus
        {
            get
            {
                //Lazy initialization
                if (currTechMilRepStatus == null)
                    currTechMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(TechnicsId, CurrentUser);

                return currTechMilRepStatus;
            }
            set
            {
                currTechMilRepStatus = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public bool CanAccessMilitaryDepartment(User currentUser)
        {
            bool canAccess = true;

            if (this != null)
            {
                if (this.CurrTechMilRepStatus != null &&
                   this.CurrTechMilRepStatus.SourceMilDepartment != null)
                {
                    canAccess = false;

                    List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsPerUser(currentUser, currentUser);

                    foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
                    {
                        if (militaryDepartment.MilitaryDepartmentId == this.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentId)
                        {
                            canAccess = true;
                            break;
                        }
                    }
                }
            }

            return canAccess;
        }

        public Technics(User user)
            : base(user)
        {

        }
    }

    public static class TechnicsUtil
    {
        //This method creates and returns a Technics object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static Technics ExtractTechnics(OracleDataReader dr, User currentUser)
        {
            Technics technics = new Technics(currentUser);

            technics.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            technics.TechnicsType = TechnicsTypeUtil.ExtractTechnicsTypeFromDataReader(dr, currentUser);
            technics.TechnicsCategoryId = DBCommon.IsInt(dr["TechnicsCategoryID"]) ? DBCommon.GetInt(dr["TechnicsCategoryID"]) : (int?)null;
            technics.ItemsCount = DBCommon.GetInt(dr["ItemsCount"]);
            technics.NormativeTechnicsId = DBCommon.IsInt(dr["NormativeTechnicsID"]) ? DBCommon.GetInt(dr["NormativeTechnicsID"]) : (int?)null;
            technics.ResidencePostCode = dr["ResidencePostCode"].ToString();
            technics.ResidenceCityId = (DBCommon.IsInt(dr["ResidenceCityID"]) ? DBCommon.GetInt(dr["ResidenceCityID"]) : (int?)null);
            technics.ResidenceDistrictId = (DBCommon.IsInt(dr["ResidenceDistrictID"]) && technics.ResidenceCityId.HasValue ? DBCommon.GetInt(dr["ResidenceDistrictID"]) : (int?)null);
            technics.ResidenceAddress = dr["ResidenceAddress"].ToString();
            technics.OtherInfo = dr["OtherInfo"].ToString();
            technics.DriverReservistId = (DBCommon.IsInt(dr["DriverReservistID"]) ? DBCommon.GetInt(dr["DriverReservistID"]) : (int?)null);
            technics.OwnershipLeasing = (DBCommon.IsInt(dr["OwnershipLeasing"]) && DBCommon.GetInt(dr["OwnershipLeasing"]) == 1);
            technics.OwnershipCompanyId = (DBCommon.IsInt(dr["OwnershipCompanyID"]) ? DBCommon.GetInt(dr["OwnershipCompanyID"]) : (int?)null);
            technics.GroupManagementSection = dr["GroupManagementSection"].ToString();
            technics.Section = dr["Section"].ToString();
            technics.Deliverer = dr["Deliverer"].ToString();
            technics.PunktID = DBCommon.IsInt(dr["PunktID"]) ? DBCommon.GetInt(dr["PunktID"]) : (int?)null;

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, technics);

            return technics;
        }

        //Get a particular object by its ID
        public static Technics GetTechnics(int technicsId, User currentUser)
        {
            Technics technics = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";                

                string SQL = @"SELECT a.TechnicsID, 
                                      a.TechnicsTypeID, b.TechnicsTypeKey, b.TechnicsTypeName, b.Active as TechnicsTypeActive,
                                      a.TechnicsCategoryID,
                                      a.ItemsCount, a.NormativeTechnicsID,
                                      a.ResidencePostCode, a.ResidenceCityID, a.ResidenceDistrictID, a.ResidenceAddress,
                                      a.OtherInfo, a.DriverReservistID,
                                      a.OwnershipLeasing, a.OwnershipCompanyID,
                                      a.GroupManagementSection, a.Section, a.Deliverer, a.PunktID,
                               
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_RES.Technics a
                               INNER JOIN PMIS_RES.TechnicsTypes b ON a.TechnicsTypeID = b.TechnicsTypeID
                               WHERE a.TechnicsID = :TechnicsID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technics = ExtractTechnics(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technics;
        }

        public static string GetTechnicsLogDescription(int technicsId, User currentUser)
        {
            string logDescription = "";

            Technics technics = GetTechnics(technicsId, currentUser);

            logDescription += "Вид техника: " + technics.TechnicsType.TypeName + "; ";

            switch (technics.TechnicsType.TypeKey)
            {
                case "VEHICLES":
                        Vehicle vehicle = VehicleUtil.GetVehicleByTechnicsId(technicsId, currentUser);
                        logDescription += "Регистрационен номер: " + vehicle.RegNumber;
                        break;
                case "TRAILERS":
                        Trailer trailer = TrailerUtil.GetTrailerByTechnicsId(technicsId, currentUser);
                        logDescription += "Регистрационен номер: " + trailer.RegNumber;
                        break;
                case "TRACTORS":
                        Tractor tractor = TractorUtil.GetTractorByTechnicsId(technicsId, currentUser);
                        logDescription += "Регистрационен номер: " + tractor.RegNumber;
                        break;
                case "ENG_EQUIP":
                        EngEquip engEquip = EngEquipUtil.GetEngEquipByTechnicsId(technicsId, currentUser);
                        logDescription += "Регистрационен номер: " + engEquip.RegNumber;
                        break;
                case "MOB_LIFT_EQUIP":
                        MobileLiftingEquip mobileLiftingEquip = MobileLiftingEquipUtil.GetMobileLiftingEquipByTechnicsId(technicsId, currentUser);
                        logDescription += "Регистрационен номер: " + mobileLiftingEquip.RegNumber;
                        break;
                case "RAILWAY_EQUIP":
                        RailwayEquip railwayEquip = RailwayEquipUtil.GetRailwayEquipByTechnicsId(technicsId, currentUser);
                        logDescription += "Инвентарен номер: " + railwayEquip.InventoryNumber;
                        break;
                case "AVIATION_EQUIP":
                        AviationEquip aviationEquip = AviationEquipUtil.GetAviationEquipByTechnicsId(technicsId, currentUser);
                        logDescription += "Инвентарен номер: " + aviationEquip.AirInvNumber;
                        break;
                case "VESSELS":
                        Vessel vessel = VesselUtil.GetVesselByTechnicsId(technicsId, currentUser);
                        logDescription += "Име: " + vessel.VesselName + "; Инв. номер: " + vessel.InventoryNumber;
                        break;
                case "FUEL_CONTAINERS":
                        FuelContainer fuelContainer = FuelContainerUtil.GetFuelContainerByTechnicsId(technicsId, currentUser);
                        logDescription += "Инвентарен номер: " + fuelContainer.InventoryNumber;
                        break;
            }

            return logDescription;
        }

        //Save a particular object into the DB
        public static void SaveTechnics(Technics technics, User currentUser, ChangeEvent changeEvent)
        {
            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                //When adding new records only the first tab on the page is visible
                //this is why the INSERT clause doens't contain all fields.
                if (technics.TechnicsId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.Technics (TechnicsTypeID, TechnicsCategoryID, 
                                ItemsCount, ResidencePostCode, ResidenceCityID, ResidenceDistrictID, ResidenceAddress,
                                NormativeTechnicsID,
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:TechnicsTypeID, :TechnicsCategoryID, 
                                :ItemsCount, :ResidencePostCode, :ResidenceCityID, :ResidenceDistrictID, :ResidenceAddress,
                                :NormativeTechnicsID,
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_RES.Technics_ID_SEQ.currval INTO :TechnicsID FROM dual;

                            ";

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TechnicsCategory", "", (technics.TechnicsCategory != null ? technics.TechnicsCategory.CategoryName : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_NormativeTechnics", "", (technics.NormativeTechnics != null ? technics.NormativeTechnics.CodeAndText : ""), currentUser));

                    if (technics.TechnicsType.TypeKey == "RAILWAY_EQUIP" ||
                        technics.TechnicsType.TypeKey == "FUEL_CONTAINERS")
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ItemsCount", "", technics.ItemsCount.ToString(), currentUser));
                    }

                    if (technics.TechnicsType.TypeKey != "AVIATION_EQUIP" &&
                        technics.TechnicsType.TypeKey != "VESSELS" &&
                        technics.TechnicsType.TypeKey != "FUEL_CONTAINERS")
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_ResidencePostCode", "", technics.ResidencePostCode, currentUser));
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_ResidenceCity", "", technics.ResidenceCityId.HasValue ? technics.ResidenceCity.RegionMunicipalityAndCity : "", currentUser));
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_ResidenceDistrict", "", technics.ResidenceDistrictId.HasValue ? technics.ResidenceDistrict.DistrictName : "", currentUser));
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_ResidenceAddress", "", technics.ResidenceAddress, currentUser));
                    }

                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.Technics SET
                               TechnicsCategoryID = :TechnicsCategoryID,
                               ItemsCount = :ItemsCount,
                               NormativeTechnicsID = :NormativeTechnicsID,
                               ResidencePostCode = :ResidencePostCode,
                               ResidenceCityID = :ResidenceCityID,
                               ResidenceDistrictID = :ResidenceDistrictID,
                               ResidenceAddress = :ResidenceAddress,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END

                             WHERE TechnicsID = :TechnicsID ;                       

                            ";

                    Technics oldTechnics = GetTechnics(technics.TechnicsId, currentUser);

                    if ((oldTechnics.TechnicsCategory != null && oldTechnics.TechnicsCategory.CategoryName != null ? oldTechnics.TechnicsCategory.CategoryName : "") !=
                        (technics.TechnicsCategory != null && technics.TechnicsCategory.CategoryName != null ? technics.TechnicsCategory.CategoryName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_TechnicsCategory",
                            oldTechnics.TechnicsCategory != null && oldTechnics.TechnicsCategory.CategoryName != null ? oldTechnics.TechnicsCategory.CategoryName : "",
                            technics.TechnicsCategory != null && technics.TechnicsCategory.CategoryName != null ? technics.TechnicsCategory.CategoryName : "",
                            currentUser));

                    if ((oldTechnics.NormativeTechnics != null && oldTechnics.NormativeTechnics.CodeAndText != null ? oldTechnics.NormativeTechnics.CodeAndText : "") !=
                        (technics.NormativeTechnics != null && technics.NormativeTechnics.CodeAndText != null ? technics.NormativeTechnics.CodeAndText : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_NormativeTechnics",
                            oldTechnics.NormativeTechnics != null && oldTechnics.NormativeTechnics.CodeAndText != null ? oldTechnics.NormativeTechnics.CodeAndText : "",
                            technics.NormativeTechnics != null && technics.NormativeTechnics.CodeAndText != null ? technics.NormativeTechnics.CodeAndText : "",
                            currentUser));

                    if (technics.TechnicsType.TypeKey == "RAILWAY_EQUIP" ||
                        technics.TechnicsType.TypeKey == "FUEL_CONTAINERS")
                    {
                        if (oldTechnics.ItemsCount != technics.ItemsCount)
                            changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_ItemsCount", oldTechnics.ItemsCount.ToString(), technics.ItemsCount.ToString(), currentUser));
                    }

                    if (technics.TechnicsType.TypeKey != "AVIATION_EQUIP" &&
                        technics.TechnicsType.TypeKey != "VESSELS" &&
                        technics.TechnicsType.TypeKey != "FUEL_CONTAINERS")
                    {
                        if (oldTechnics.ResidencePostCode.Trim() != technics.ResidencePostCode.Trim())
                            changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_ResidencePostCode", oldTechnics.ResidencePostCode, technics.ResidencePostCode, currentUser));

                        if (!CommonFunctions.IsEqualInt(oldTechnics.ResidenceCityId, technics.ResidenceCityId))
                            changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_ResidenceCity", oldTechnics.ResidenceCityId.HasValue ? oldTechnics.ResidenceCity.RegionMunicipalityAndCity : "", technics.ResidenceCityId.HasValue ? technics.ResidenceCity.RegionMunicipalityAndCity : "", currentUser));

                        if (!CommonFunctions.IsEqualInt(oldTechnics.ResidenceDistrictId, technics.ResidenceDistrictId))
                            changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_ResidenceDistrict", oldTechnics.ResidenceDistrictId.HasValue ? oldTechnics.ResidenceDistrict.DistrictName : "", technics.ResidenceDistrictId.HasValue ? technics.ResidenceDistrict.DistrictName : "", currentUser));

                        if (oldTechnics.ResidenceAddress.Trim() != technics.ResidenceAddress.Trim())
                            changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_ResidenceAddress", oldTechnics.ResidenceAddress, technics.ResidenceAddress, currentUser));
                    }
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTechnicsID = new OracleParameter();
                paramTechnicsID.ParameterName = "TechnicsID";
                paramTechnicsID.OracleType = OracleType.Number;

                if (technics.TechnicsId != 0)
                {
                    paramTechnicsID.Direction = ParameterDirection.Input;
                    paramTechnicsID.Value = technics.TechnicsId;
                }
                else
                {
                    paramTechnicsID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramTechnicsID);

                OracleParameter param = null;

                if (technics.TechnicsId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TechnicsTypeID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = technics.TechnicsType.TechnicsTypeId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "TechnicsCategoryID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technics.TechnicsCategoryId.HasValue)
                    param.Value = technics.TechnicsCategoryId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ItemsCount";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technics.ItemsCount;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NormativeTechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technics.NormativeTechnicsId.HasValue)
                    param.Value = technics.NormativeTechnicsId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ResidencePostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technics.ResidencePostCode))
                    param.Value = technics.ResidencePostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ResidenceCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technics.ResidenceCityId.HasValue)
                    param.Value = technics.ResidenceCityId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ResidenceDistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technics.ResidenceDistrictId.HasValue)
                    param.Value = technics.ResidenceDistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ResidenceAddress";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technics.ResidenceAddress))
                    param.Value = technics.ResidenceAddress;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (technics.TechnicsId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (technics.TechnicsId == 0)
                    technics.TechnicsId = DBCommon.GetInt(paramTechnicsID.Value);
            }
            finally
            {
                conn.Close();
            }
        }

        //Save a particular Technics into the DB
        public static bool SaveTechnics_WhenEditingMilitaryReportTab(Technics technics, User currentUser, ChangeEvent changeEvent)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"UPDATE PMIS_RES.Technics SET
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
                             WHERE TechnicsID = :TechnicstID
                        ";

                Technics oldTechnics = TechnicsUtil.GetTechnics(technics.TechnicsId, currentUser);

                if (oldTechnics.GroupManagementSection.Trim() != technics.GroupManagementSection.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_GroupManagementSection", oldTechnics.GroupManagementSection, technics.GroupManagementSection, currentUser));

                if (oldTechnics.Section.Trim() != technics.Section.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_Section", oldTechnics.Section, technics.Section, currentUser));

                if (oldTechnics.Deliverer.Trim() != technics.Deliverer.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_Deliverer", oldTechnics.Deliverer, technics.Deliverer, currentUser));

                if (oldTechnics.PunktID != technics.PunktID)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_MilRepStatus_Punkt", oldTechnics.Punkt != null ? oldTechnics.Punkt.Text() : "", technics.Punkt != null ? technics.Punkt.Text() : "", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicstID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technics.TechnicsId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "GroupManagementSection";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(technics.GroupManagementSection))
                    param.Value = technics.GroupManagementSection;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Section";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(technics.Section))
                    param.Value = technics.Section;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Deliverer";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(technics.Deliverer))
                    param.Value = technics.Deliverer;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PunktID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technics.PunktID.HasValue)
                    param.Value = technics.PunktID.Value;
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

        public static bool SaveTechnics_SetAppointmentIsDelivered(int technicsId, int appointmentIsDelivered, User currentUser, ChangeEvent changeEvent)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"UPDATE PMIS_RES.FulfilTechnicsRequest SET
                               AppointmentIsDelivered = :AppointmentIsDelivered 
                             WHERE TechnicsID = :TechnicsID
                        ";

                List<FillTechnicsRequest> allRequests = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechnicsId(technicsId, currentUser);
                FillTechnicsRequest oldRequest = null;

                if (allRequests != null && allRequests.Count > 0)
                    oldRequest = allRequests[0];

                if (oldRequest.AppointmentIsDelivered != (appointmentIsDelivered == 1))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_AppointmentIsDelivered", oldRequest.AppointmentIsDelivered ? "Да" : "Не", appointmentIsDelivered == 1 ? "Да" : "Не", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsId;
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

        //Save a particular object into the DB
        public static void SaveTechnics_OtherInfo(int technicsId, string otherInfo, User currentUser, Change changeEntry)
        {
            string logDescription = "";

            Technics oldTechnics = GetTechnics(technicsId, currentUser);

            logDescription = TechnicsUtil.GetTechnicsLogDescription(technicsId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Technics_EditOtherInfo", logDescription, null, null, currentUser);

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                           UPDATE PMIS_RES.Technics SET
                               OtherInfo = :OtherInfo,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END

                             WHERE TechnicsID = :TechnicsID;
                        END;";


                if (oldTechnics.OtherInfo.Trim() != otherInfo.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_OtherInfo", oldTechnics.OtherInfo, otherInfo, currentUser));


                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OtherInfo";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(otherInfo))
                    param.Value = otherInfo;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }

        //Save a particular object into the DB
        public static void SaveTechnics_Owner(Technics technics, User currentUser, Change changeEntry)
        {
            string logDescription = "";

            Technics oldTechnics = GetTechnics(technics.TechnicsId, currentUser);

            logDescription = TechnicsUtil.GetTechnicsLogDescription(technics.TechnicsId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Technics_EditOwner", logDescription, null, null, currentUser);

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           UPDATE PMIS_RES.Technics SET
                               DriverReservistID = :DriverReservistID,
                               OwnershipLeasing = :OwnershipLeasing,
                               OwnershipCompanyID = :OwnershipCompanyID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END
                           WHERE TechnicsID = :TechnicsID;
                        END;";


                if (!CommonFunctions.IsEqualInt(oldTechnics.DriverReservistId, technics.DriverReservistId))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_Owner_DriverIdentNumber", oldTechnics.DriverReservistId.HasValue ? oldTechnics.DriverReservist.Person.IdentNumber : "", technics.DriverReservistId.HasValue ? technics.DriverReservist.Person.IdentNumber : "", currentUser));

                if (oldTechnics.OwnershipLeasing != technics.OwnershipLeasing)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Technics_OwnershipLeasing", oldTechnics.OwnershipLeasing ? "1" : "0", technics.OwnershipLeasing ? "1" : "0", currentUser));

                if (oldTechnics.OwnerDisplayText != technics.OwnerDisplayText)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_Owner_Owner", oldTechnics.OwnerDisplayText, technics.OwnerDisplayText, currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technics.TechnicsId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DriverReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technics.DriverReservistId.HasValue)
                    param.Value = technics.DriverReservistId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OwnershipLeasing";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technics.OwnershipLeasing ? 1 : 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OwnershipCompanyID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technics.OwnershipCompanyId.HasValue)
                    param.Value = technics.OwnershipCompanyId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }

        //When change any child record then update the last modified of technics (the parent object)
        public static void SetTechnicsModified(int technicsId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_RES.Technics SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE TechnicsId = :TechnicsId";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsId", OracleType.Number).Value = technicsId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public static string SearchTechMilRepStatuses()
        {
            string statuses = Config.GetWebSetting("SearchFulfilTechMilRepStatuses");
            string[] arrStatues = statuses.Split(',');

            statuses = "";
            foreach (string status in arrStatues)
            {
                statuses += (statuses == "" ? "" : ",") +
                             "'" + status.Replace("'", "''") + "'";
            }

            return statuses;
        }
    }
}