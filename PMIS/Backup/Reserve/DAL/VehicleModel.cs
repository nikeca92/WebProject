using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class VehicleModel : BaseDbObject, IDropDownItem
    {
        private int vehicleModelId;
        private string vehicleModelName;
        private int vehicleMakeId;
        private VehicleMake vehicleMake;
        private bool canDelete;

        public int VehicleModelId
        {
            get { return vehicleModelId; }
            set { vehicleModelId = value; }
        }

        public string VehicleModelName
        {
            get { return vehicleModelName; }
            set { vehicleModelName = value; }
        }

        public int VehicleMakeId
        {
            get { return vehicleMakeId; }
            set { vehicleMakeId = value; }
        }

        public VehicleMake VehicleMake
        {
            get
            {
                if (vehicleMake == null)
                    vehicleMake = VehicleMakeUtil.GetVehicleMake(vehicleMakeId, CurrentUser);

                return vehicleMake;
            }
            set { vehicleMake = value; }
        }

        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; }
        }

        //IDropDownItem
        public string Value()
        {
            return VehicleModelId.ToString();
        }

        public string Text()
        {
            return VehicleModelName.ToString();
        }

        public VehicleModel(User user)
            : base(user)
        {
        }
    }

    public class VehicleModelFilter
    {
        public int? VehicleMakeId { get; set; }

        public int OrderBy { get; set; }

        public int PageIdx { get; set; }

        public int RowsPerPage { get; set; }
    }

    public static class VehicleModelUtil
    {
        //This method creates and returns a VehicleModel object. It extracts the data from a DataReader.
        public static VehicleModel ExtractVehicleModelFromDataReader(OracleDataReader dr, User currentUser)
        {
            VehicleModel vehicleModel = new VehicleModel(currentUser);

            vehicleModel.VehicleModelId = DBCommon.GetInt(dr["VehicleModelID"]);
            vehicleModel.VehicleModelName = dr["VehicleModelName"].ToString();
            vehicleModel.VehicleMakeId = DBCommon.GetInt(dr["VehicleMakeID"]);
            vehicleModel.CanDelete = DBCommon.GetInt(dr["CanDelete"]) == 1;

            vehicleModel.VehicleMake = VehicleMakeUtil.GetVehicleMake(vehicleModel.VehicleMakeId, currentUser);

            return vehicleModel;
        }

        //Get a particular object by its ID
        public static VehicleModel GetVehicleModel(int vehicleModelId, User currentUser)
        {
            VehicleModel vehicleModel = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VehicleModelID, a.VehicleModelName, a.VehicleMakeID,
                                    CASE WHEN v.VehicleModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete
                               FROM PMIS_RES.VehicleModels a
                               LEFT JOIN (SELECT DISTINCT VehicleModelID FROM PMIS_RES.Vehicles) v ON a.VehicleModelID = v.VehicleModelID
                               WHERE a.VehicleModelID = :VehicleModelID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VehicleModelID", OracleType.Number).Value = vehicleModelId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vehicleModel = ExtractVehicleModelFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleModel;
        }

        //Get a list of all model by a make
        public static List<VehicleModel> GetAllVehicleModels(int vehicleMakeId, User currentUser)
        {
            List<VehicleModel> vehicleModels = new List<VehicleModel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VehicleModelID, a.VehicleModelName, a.VehicleMakeID,
                                    CASE WHEN v.VehicleModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete
                               FROM PMIS_RES.VehicleModels a
                               LEFT JOIN (SELECT DISTINCT VehicleModelID FROM PMIS_RES.Vehicles) v ON a.VehicleModelID = v.VehicleModelID
                               WHERE a.VehicleMakeID = :VehicleMakeID
                               ORDER BY a.VehicleModelName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VehicleMakeID", OracleType.Number).Value = vehicleMakeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vehicleModels.Add(ExtractVehicleModelFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleModels;
        }

        //Get all vehicle models by specified filter
        public static List<VehicleModel> GetAllVehicleModelsByFilter(VehicleModelFilter filter, User currentUser)
        {
            List<VehicleModel> vehicleModels = new List<VehicleModel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.VehicleMakeId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.VehicleMakeID = " + filter.VehicleMakeId.Value + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && filter.RowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + filter.RowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + filter.RowsPerPage.ToString() + @" ";

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
                        orderBySQL = "a.VehicleModelName";
                        break;
                    case 2:
                        orderBySQL = "b.VehicleMakeName";
                        break;
                    default:
                        orderBySQL = "a.VehicleMakeName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.VehicleModelID, a.VehicleModelName, b.VehicleMakeID, b.VehicleMakeName,
                                    CASE WHEN v.VehicleModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete, 
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.VehicleModelID) as RowNumber 
                                FROM PMIS_RES.VehicleModels a
                                INNER JOIN PMIS_RES.VehicleMakes b ON a.VehicleMakeID = b.VehicleMakeID
                                LEFT JOIN (SELECT DISTINCT VehicleModelID FROM PMIS_RES.Vehicles) v ON a.VehicleModelID = v.VehicleModelID
                              " + where + @"    
                              ORDER BY " + orderBySQL + @", a.VehicleModelID
                           ) tmp
                           " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    VehicleModel vehicleModel = ExtractVehicleModelFromDataReader(dr, currentUser);
                    vehicleModels.Add(vehicleModel);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleModels;
        }

        public static int GetAllVehicleModelsByFilterCount(VehicleModelFilter filter, User currentUser)
        {
            int vehicleModelsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.VehicleMakeId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.VehicleMakeID = " + filter.VehicleMakeId.Value + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.VehicleModels a
                                INNER JOIN PMIS_RES.VehicleMakes b ON a.VehicleMakeID = b.VehicleMakeID                       
                              " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        vehicleModelsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vehicleModelsCnt;
        }

        //Save a particular Vehicle Model into the DB
        public static bool SaveVehicleModel(VehicleModel vehicleModel, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            VehicleMake vehicleMake = VehicleMakeUtil.GetVehicleMake(vehicleModel.VehicleMakeId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (vehicleModel.VehicleModelId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.VehicleModels (VehicleModelName, VehicleMakeID)
                             VALUES (:VehicleModelName, :VehicleMakeID);
                            
                             SELECT PMIS_RES.VehicleModels_ID_SEQ.currval INTO :VehicleModelID FROM dual;
                            ";

                    changeEvent = new ChangeEvent("RES_Lists_VehicleModels_Add", "Модел от марка: " + vehicleMake.VehicleMakeName, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_VehicleModels_VehicleModelName", "",
                        vehicleModel.VehicleModelName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.VehicleModels SET
                               VehicleModelName = :VehicleModelName,
                               VehicleMakeID = :VehicleMakeID
                             WHERE VehicleModelID = :VehicleModelID;
                            ";

                    VehicleModel oldVehicleModel = GetVehicleModel(vehicleModel.VehicleModelId, currentUser);

                    changeEvent = new ChangeEvent("RES_Lists_VehicleModels_Edit", "Модел от марка: " + vehicleModel.VehicleMake.VehicleMakeName, null, null, currentUser);

                    if (oldVehicleModel.VehicleModelName != vehicleModel.VehicleModelName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_VehicleModels_VehicleModelName",
                                     oldVehicleModel.VehicleModelName, vehicleModel.VehicleModelName, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramVehicleModelID = new OracleParameter();
                paramVehicleModelID.ParameterName = "VehicleModelID";
                paramVehicleModelID.OracleType = OracleType.Number;

                if (vehicleModel.VehicleModelId != 0)
                {
                    paramVehicleModelID.Direction = ParameterDirection.Input;
                    paramVehicleModelID.Value = vehicleModel.VehicleModelId;
                }
                else
                {
                    paramVehicleModelID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramVehicleModelID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "VehicleModelName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(vehicleModel.VehicleModelName))
                    param.Value = vehicleModel.VehicleModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VehicleMakeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vehicleModel.VehicleMakeId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (vehicleModel.VehicleModelId == 0)
                    vehicleModel.VehicleModelId = DBCommon.GetInt(paramVehicleModelID.Value);

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

        //Delete a particular Vehicle Model from the DB
        public static bool DeleteVehicleModel(int vehicleModelId, User currentUser, Change changeEntry)
        {
            bool result = false;

            VehicleModel oldVehicleModel = VehicleModelUtil.GetVehicleModel(vehicleModelId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Lists_VehicleModels_Delete", "Модел от марка: " + oldVehicleModel.VehicleMake.VehicleMakeName, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_VehicleModels_VehicleModelName", oldVehicleModel.VehicleModelName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" DELETE FROM PMIS_RES.VehicleModels WHERE VehicleModelID = :VehicleModelID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VehicleModelID", OracleType.Number).Value = vehicleModelId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }
    }

}