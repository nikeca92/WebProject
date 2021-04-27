using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class AviationOtherBaseMachineModel : BaseDbObject, IDropDownItem
    {
        private int aviationOtherBaseMachineModelId;
        private string aviationOtherBaseMachineModelName;
        private int aviationOtherBaseMachineMakeId;
        private AviationOtherBaseMachineMake aviationOtherBaseMachineMake;
        private bool canDelete;

        public int AviationOtherBaseMachineModelId
        {
            get { return aviationOtherBaseMachineModelId; }
            set { aviationOtherBaseMachineModelId = value; }
        }

        public string AviationOtherBaseMachineModelName
        {
            get { return aviationOtherBaseMachineModelName; }
            set { aviationOtherBaseMachineModelName = value; }
        }

        public int AviationOtherBaseMachineMakeId
        {
            get { return aviationOtherBaseMachineMakeId; }
            set { aviationOtherBaseMachineMakeId = value; }
        }

        public AviationOtherBaseMachineMake AviationOtherBaseMachineMake
        {
            get
            {
                if (aviationOtherBaseMachineMake == null)
                    aviationOtherBaseMachineMake = AviationOtherBaseMachineMakeUtil.GetAviationOtherBaseMachineMake(aviationOtherBaseMachineMakeId, CurrentUser);

                return aviationOtherBaseMachineMake;
            }
            set { aviationOtherBaseMachineMake = value; }
        }

        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; }
        }

        //IDropDownItem
        public string Value()
        {
            return AviationOtherBaseMachineModelId.ToString();
        }

        public string Text()
        {
            return AviationOtherBaseMachineModelName.ToString();
        }

        public AviationOtherBaseMachineModel(User user)
            : base(user)
        {
        }
    }

    public class AviationOtherBaseMachineModelFilter
    {
        public int? AviationOtherBaseMachineMakeId { get; set; }

        public int OrderBy { get; set; }

        public int PageIdx { get; set; }

        public int RowsPerPage { get; set; }
    }

    public static class AviationOtherBaseMachineModelUtil
    {
        //This method creates and returns a AviationOtherBaseMachineModel object. It extracts the data from a DataReader.
        public static AviationOtherBaseMachineModel ExtractAviationOtherBaseMachineModelFromDataReader(OracleDataReader dr, User currentUser)
        {
            AviationOtherBaseMachineModel aviationOtherBaseMachineModel = new AviationOtherBaseMachineModel(currentUser);

            aviationOtherBaseMachineModel.AviationOtherBaseMachineModelId = DBCommon.GetInt(dr["AviationOthBaseMachineModelID"]);
            aviationOtherBaseMachineModel.AviationOtherBaseMachineModelName = dr["AviationOthBaseMachineModel"].ToString();
            aviationOtherBaseMachineModel.AviationOtherBaseMachineMakeId = DBCommon.GetInt(dr["AviationOtherBaseMachineMakeID"]);
            aviationOtherBaseMachineModel.CanDelete = DBCommon.GetInt(dr["CanDelete"]) == 1;

            aviationOtherBaseMachineModel.AviationOtherBaseMachineMake = AviationOtherBaseMachineMakeUtil.GetAviationOtherBaseMachineMake(aviationOtherBaseMachineModel.AviationOtherBaseMachineMakeId, currentUser);

            return aviationOtherBaseMachineModel;
        }

        //Get a particular object by its ID
        public static AviationOtherBaseMachineModel GetAviationOtherBaseMachineModel(int aviationOtherBaseMachineModelId, User currentUser)
        {
            AviationOtherBaseMachineModel aviationOtherBaseMachineModel = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.AviationOthBaseMachineModelID, a.AviationOthBaseMachineModel, a.AviationOtherBaseMachineMakeID,
                                    CASE WHEN v.AviationOthBaseMachineModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete
                               FROM PMIS_RES.AviationOtherBaseMachineModels a                       
                               LEFT JOIN (SELECT DISTINCT AviationOthBaseMachineModelID FROM PMIS_RES.AviationEquipment) v ON a.AviationOthBaseMachineModelID = v.AviationOthBaseMachineModelID
                               WHERE a.AviationOthBaseMachineModelID = :AviationOthBaseMachineModelID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AviationOthBaseMachineModelID", OracleType.Number).Value = aviationOtherBaseMachineModelId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    aviationOtherBaseMachineModel = ExtractAviationOtherBaseMachineModelFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationOtherBaseMachineModel;
        }

        //Get a list of all model by a make
        public static List<AviationOtherBaseMachineModel> GetAllAviationOtherBaseMachineModels(int aviationOtherBaseMachineMakeId, User currentUser)
        {
            List<AviationOtherBaseMachineModel> aviationOtherBaseMachineModels = new List<AviationOtherBaseMachineModel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.AviationOthBaseMachineModelID, a.AviationOthBaseMachineModel, a.AviationOtherBaseMachineMakeID,
                                    CASE WHEN v.AviationOthBaseMachineModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete
                               FROM PMIS_RES.AviationOtherBaseMachineModels a
                               LEFT JOIN (SELECT DISTINCT AviationOthBaseMachineModelID FROM PMIS_RES.AviationEquipment) v ON a.AviationOthBaseMachineModelID = v.AviationOthBaseMachineModelID
                               WHERE a.AviationOtherBaseMachineMakeID = :AviationOtherBaseMachineMakeID
                               ORDER BY a.AviationOthBaseMachineModel";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AviationOtherBaseMachineMakeID", OracleType.Number).Value = aviationOtherBaseMachineMakeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    aviationOtherBaseMachineModels.Add(ExtractAviationOtherBaseMachineModelFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationOtherBaseMachineModels;
        }

        //Get all engEquipBase models by specified filter
        public static List<AviationOtherBaseMachineModel> GetAllAviationOtherBaseMachineModelsByFilter(AviationOtherBaseMachineModelFilter filter, User currentUser)
        {
            List<AviationOtherBaseMachineModel> aviationOtherBaseMachineModels = new List<AviationOtherBaseMachineModel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.AviationOtherBaseMachineMakeId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.AviationOtherBaseMachineMakeID = " + filter.AviationOtherBaseMachineMakeId.Value + " ";
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
                        orderBySQL = "a.AviationOthBaseMachineModel";
                        break;
                    case 2:
                        orderBySQL = "b.AviationOtherBaseMachineMake";
                        break;
                    default:
                        orderBySQL = "a.AviationOtherBaseMachineMake";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.AviationOthBaseMachineModelID, a.AviationOthBaseMachineModel, b.AviationOtherBaseMachineMakeID, b.AviationOtherBaseMachineMake,
                                    CASE WHEN v.AviationOthBaseMachineModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.AviationOthBaseMachineModelID) as RowNumber 
                                FROM PMIS_RES.AviationOtherBaseMachineModels a
                                INNER JOIN PMIS_RES.AviationOtherBaseMachineMakes b ON a.AviationOtherBaseMachineMakeID = b.AviationOtherBaseMachineMakeID
                               LEFT JOIN (SELECT DISTINCT AviationOthBaseMachineModelID FROM PMIS_RES.AviationEquipment) v ON a.AviationOthBaseMachineModelID = v.AviationOthBaseMachineModelID
                              " + where + @"    
                              ORDER BY " + orderBySQL + @", a.AviationOthBaseMachineModelID
                           ) tmp
                           " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    AviationOtherBaseMachineModel aviationOtherBaseMachineModel = ExtractAviationOtherBaseMachineModelFromDataReader(dr, currentUser);
                    aviationOtherBaseMachineModels.Add(aviationOtherBaseMachineModel);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationOtherBaseMachineModels;
        }

        public static int GetAllAviationOtherBaseMachineModelsByFilterCount(AviationOtherBaseMachineModelFilter filter, User currentUser)
        {
            int aviationOtherBaseMachineModelsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.AviationOtherBaseMachineMakeId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.AviationOtherBaseMachineMakeID = " + filter.AviationOtherBaseMachineMakeId.Value + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.AviationOtherBaseMachineModels a
                                INNER JOIN PMIS_RES.AviationOtherBaseMachineMakes b ON a.AviationOtherBaseMachineMakeID = b.AviationOtherBaseMachineMakeID                       
                              " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        aviationOtherBaseMachineModelsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return aviationOtherBaseMachineModelsCnt;
        }

        //Save a particular EngEquipBase Model into the DB
        public static bool SaveAviationOtherBaseMachineModel(AviationOtherBaseMachineModel aviationOtherBaseMachineModel, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            AviationOtherBaseMachineMake aviationOtherBaseMachineMake = AviationOtherBaseMachineMakeUtil.GetAviationOtherBaseMachineMake(aviationOtherBaseMachineModel.AviationOtherBaseMachineMakeId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (aviationOtherBaseMachineModel.AviationOtherBaseMachineModelId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.AviationOtherBaseMachineModels (AviationOthBaseMachineModel, AviationOtherBaseMachineMakeID)
                             VALUES (:AviationOthBaseMachineModel, :AviationOtherBaseMachineMakeID);
                            
                             SELECT PMIS_RES.AviationOtherBMModels_ID_SEQ.currval INTO :AviationOthBaseMachineModelID FROM dual;
                            ";

                    changeEvent = new ChangeEvent("RES_Lists_AviationOtherBaseMachineModels_Add", "Модел от марка: " + aviationOtherBaseMachineMake.AviationOtherBaseMachineMakeName, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_AviationOtherBaseMachineModels_AviationOtherBaseMachineModelName", "",
                        aviationOtherBaseMachineModel.AviationOtherBaseMachineModelName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.AviationOtherBaseMachineModels SET
                               AviationOthBaseMachineModel = :AviationOthBaseMachineModel,
                               AviationOtherBaseMachineMakeID = :AviationOtherBaseMachineMakeID
                             WHERE AviationOthBaseMachineModelID = :AviationOthBaseMachineModelID;
                            ";

                    AviationOtherBaseMachineModel oldAviationOtherBaseMachineModel = GetAviationOtherBaseMachineModel(aviationOtherBaseMachineModel.AviationOtherBaseMachineModelId, currentUser);

                    changeEvent = new ChangeEvent("RES_Lists_AviationOtherBaseMachineModels_Edit", "Модел от марка: " + aviationOtherBaseMachineModel.AviationOtherBaseMachineMake.AviationOtherBaseMachineMakeName, null, null, currentUser);

                    if (oldAviationOtherBaseMachineModel.AviationOtherBaseMachineModelName != aviationOtherBaseMachineModel.AviationOtherBaseMachineModelName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_AviationOtherBaseMachineModels_AviationOtherBaseMachineModelName",
                                     oldAviationOtherBaseMachineModel.AviationOtherBaseMachineModelName, aviationOtherBaseMachineModel.AviationOtherBaseMachineModelName, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramAviationOtherBaseMachineModelID = new OracleParameter();
                paramAviationOtherBaseMachineModelID.ParameterName = "AviationOthBaseMachineModelID";
                paramAviationOtherBaseMachineModelID.OracleType = OracleType.Number;

                if (aviationOtherBaseMachineModel.AviationOtherBaseMachineModelId != 0)
                {
                    paramAviationOtherBaseMachineModelID.Direction = ParameterDirection.Input;
                    paramAviationOtherBaseMachineModelID.Value = aviationOtherBaseMachineModel.AviationOtherBaseMachineModelId;
                }
                else
                {
                    paramAviationOtherBaseMachineModelID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramAviationOtherBaseMachineModelID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "AviationOthBaseMachineModel";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(aviationOtherBaseMachineModel.AviationOtherBaseMachineModelName))
                    param.Value = aviationOtherBaseMachineModel.AviationOtherBaseMachineModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AviationOtherBaseMachineMakeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = aviationOtherBaseMachineModel.AviationOtherBaseMachineMakeId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (aviationOtherBaseMachineModel.AviationOtherBaseMachineModelId == 0)
                    aviationOtherBaseMachineModel.AviationOtherBaseMachineModelId = DBCommon.GetInt(paramAviationOtherBaseMachineModelID.Value);

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

        //Delete a particular EngEquipBase Model from the DB
        public static bool DeleteAviationOtherBaseMachineModel(int aviationOtherBaseMachineModelId, User currentUser, Change changeEntry)
        {
            bool result = false;

            AviationOtherBaseMachineModel oldAviationOtherBaseMachineModel = AviationOtherBaseMachineModelUtil.GetAviationOtherBaseMachineModel(aviationOtherBaseMachineModelId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Lists_AviationOtherBaseMachineModels_Delete", "Модел от марка: " + oldAviationOtherBaseMachineModel.AviationOtherBaseMachineMake.AviationOtherBaseMachineMakeName, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_AviationOtherBaseMachineModels_AviationOtherBaseMachineModelName", oldAviationOtherBaseMachineModel.AviationOtherBaseMachineModelName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" DELETE FROM PMIS_RES.AviationOtherBaseMachineModels WHERE AviationOthBaseMachineModelID = :AviationOthBaseMachineModelID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("AviationOthBaseMachineModelID", OracleType.Number).Value = aviationOtherBaseMachineModelId;

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