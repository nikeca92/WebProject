using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents all information about the filter, the order and the paging information on the screen
    public class ReportVehicleManageFilter
    {
        string regNumber;
        string inventoryNumber;
        string technicsCategoryId;
        string vehicleKindId;

        //string vehicleMakeId;
        //string vehicleModelId;

        string vehicleMakeName;
        string vehicleModelName;

        string vehicleBodyTypeId;
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

        public string VehicleKindId
        {
            get { return vehicleKindId; }
            set { vehicleKindId = value; }
        }

        //public string VehicleMakeId
        //{
        //    get { return vehicleMakeId; }
        //    set { vehicleMakeId = value; }
        //}

        //public string VehicleModelId
        //{
        //    get { return vehicleModelId; }
        //    set { vehicleModelId = value; }
        //}

        public string VehicleMakeName
        {
            get { return vehicleMakeName; }
            set { vehicleMakeName = value; }
        }

        public string VehicleModelName
        {
            get { return vehicleModelName; }
            set { vehicleModelName = value; }
        }

        public string VehicleBodyTypeId
        {
            get { return vehicleBodyTypeId; }
            set { vehicleBodyTypeId = value; }
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

    public class ReportVehicleManageBlock
    {
        private int technicsId;
        private int vehicleId;
        private string regNumber;
        string inventoryNumber;
        string technicsCategory;
        string vehicleKind;
        string vehicleMake;
        string vehicleModel;
        string militaryReportStatus;
        string militaryDepartment;
        string ownership;
        string bodyType;
        string address;
        string normativeTechnicsCode;
        string normativeTechnicsName;

        public int TechnicsId
        {
            get { return technicsId; }
            set { technicsId = value; }
        }

        public int VehicleId
        {
            get { return vehicleId; }
            set { vehicleId = value; }
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

        public string VehicleKind
        {
            get { return vehicleKind; }
            set { vehicleKind = value; }
        }

        public string VehicleMake
        {
            get { return vehicleMake; }
            set { vehicleMake = value; }
        }

        public string VehicleModel
        {
            get { return vehicleModel; }
            set { vehicleModel = value; }
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

        public string BodyType
        {
            get { return bodyType; }
            set { bodyType = value; }
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

    public static class ReportVehicleUtil
    {
        public static List<ReportVehicleManageBlock> GetAllReportVehicleManageBlocks(ReportVehicleManageFilter filter, int rowsPerPage, User currentUser)
        {
            List<ReportVehicleManageBlock> reportVehicleManageBlocks = new List<ReportVehicleManageBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_REPORTS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.VehicleKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleKindId IN (" + filter.VehicleKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.VehicleMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleMakeId IN (" + filter.VehicleMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.VehicleModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleModelId IN (" + filter.VehicleModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.VehicleMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleMakeName) LIKE '%" + filter.VehicleMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleModelName) LIKE '%" + filter.VehicleModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleBodyTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleBodyTypeId IN (" + filter.VehicleBodyTypeId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryReportStatus))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.TechMilitaryReportStatusID IN ( " + filter.MilitaryReportStatus + ") ";
                }
                else
                {
                    // Ако за статус е избрано Всички, да взема всички без Изключени
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
                        orderBySQL = "a.VehicleMakeName";
                        break;
                    case 6:
                        orderBySQL = "a.VehicleModelName";
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
                        orderBySQL = "k.TableValue";
                        break;
                    case 12:
                        orderBySQL = "n.NormativeCode";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TechnicsID,
                                       a.VehicleID, 
                                       a.RegNumber,
                                       a.InventoryNumber,
                                       c.TechnicsCategoryName,
                                       d.TableValue as VehicleKind,
                                       a.VehicleMakeName,
                                       a.VehicleModelName,
                                       g.TechMilitaryReportStatusName,
                                       h.MilitaryDepartmentName,
                                       j.CompanyName, j.UnifiedIdentityCode, 
                                       k.TableValue as BodyType,
                                       PMIS_ADM.CommonFunctions.GetFullAddress(j.CityID, j.DistrictID, j.Address) as Address,
                                       n.NormativeCode, 
                                       n.NormativeName,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsID) as RowNumber 
                                FROM PMIS_RES.Vehicles a
                                INNER JOIN PMIS_RES.Technics b ON a.TechnicsID = b.TechnicsID
                                LEFT OUTER JOIN PMIS_RES.TechnicsCategories c ON b.TechnicsCategoryId = c.TechnicsCategoryId
                                LEFT OUTER JOIN PMIS_RES.GTable d ON a.VehicleKindId = d.TableKey AND d.TableName = 'VehicleKind'";

                                //LEFT OUTER JOIN PMIS_RES.VehicleMakes e ON a.VehicleMakeId = e.VehicleMakeId
                                //LEFT OUTER JOIN PMIS_RES.VehicleModels f ON a.VehicleModelId = f.VehicleModelId
                SQL += @"
                                LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus i ON a.TechnicsID = i.TechnicsID AND i.IsCurrent = 1
                                LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses g ON i.TechMilitaryReportStatusID = g.TechMilitaryReportStatusID
                                LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments h ON i.SourceMilDepartmentID = h.MilitaryDepartmentID
                                LEFT OUTER JOIN PMIS_ADM.Companies j ON b.OwnershipCompanyID = j.CompanyID
                                LEFT OUTER JOIN PMIS_RES.GTable k ON a.VehicleBodyTypeID = k.TableKey AND k.TableName = 'VehicleBodyType'
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
                    ReportVehicleManageBlock reportVehicleManageBlock = new ReportVehicleManageBlock();

                    reportVehicleManageBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
                    reportVehicleManageBlock.VehicleId = DBCommon.GetInt(dr["VehicleID"]);
                    reportVehicleManageBlock.RegNumber = dr["RegNumber"].ToString();
                    reportVehicleManageBlock.InventoryNumber = dr["InventoryNumber"].ToString();
                    reportVehicleManageBlock.TechnicsCategory = dr["TechnicsCategoryName"].ToString();
                    reportVehicleManageBlock.VehicleKind = dr["VehicleKind"].ToString();
                    reportVehicleManageBlock.BodyType = dr["BodyType"].ToString();
                    reportVehicleManageBlock.VehicleMake = dr["VehicleMakeName"].ToString();
                    reportVehicleManageBlock.VehicleModel = dr["VehicleModelName"].ToString();
                    reportVehicleManageBlock.MilitaryReportStatus = dr["TechMilitaryReportStatusName"].ToString();
                    reportVehicleManageBlock.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    reportVehicleManageBlock.Address = dr["Address"].ToString();

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        reportVehicleManageBlock.Ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        reportVehicleManageBlock.Ownership = "";
                    }

                    reportVehicleManageBlock.NormativeTechnicsCode = dr["NormativeCode"].ToString();
                    reportVehicleManageBlock.NormativeTechnicsName = dr["NormativeName"].ToString();

                    reportVehicleManageBlocks.Add(reportVehicleManageBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportVehicleManageBlocks;
        }

        public static int GetAllReportVehicleManageBlocksCount(ReportVehicleManageFilter filter, User currentUser)
        {
            int reportVehicleManageBlocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_REPORTS_VEHICLES", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!string.IsNullOrEmpty(filter.VehicleKindId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleKindId IN (" + filter.VehicleKindId + ") ";
                }

                //if (!string.IsNullOrEmpty(filter.VehicleMakeId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleMakeId IN (" + filter.VehicleMakeId + ") ";
                //}

                //if (!string.IsNullOrEmpty(filter.VehicleModelId))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             " a.VehicleModelId IN (" + filter.VehicleModelId + ") ";
                //}

                if (!string.IsNullOrEmpty(filter.VehicleMakeName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleMakeName) LIKE '%" + filter.VehicleMakeName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleModelName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " upper(a.VehicleModelName) LIKE '%" + filter.VehicleModelName.ToUpper().Replace("'", "''") + "%' ";
                }

                if (!string.IsNullOrEmpty(filter.VehicleBodyTypeId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VehicleBodyTypeId IN (" + filter.VehicleBodyTypeId + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryReportStatus))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" i.TechMilitaryReportStatusID IN ( " + filter.MilitaryReportStatus + ") ";
                }
                else
                {
                    // Ако за статус е избрано Всички, да взема всички без Изключени
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
                                FROM PMIS_RES.Vehicles a
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
                        reportVehicleManageBlocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportVehicleManageBlocksCnt;
        }
    }
}