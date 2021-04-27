using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class EngEquipBaseModel : BaseDbObject, IDropDownItem
    {
        private int engEquipBaseModelId;
        private string engEquipBaseModelName;
        private int engEquipBaseMakeId;
        private EngEquipBaseMake engEquipBaseMake;
        private bool canDelete;

        public int EngEquipBaseModelId
        {
            get { return engEquipBaseModelId; }
            set { engEquipBaseModelId = value; }
        }

        public string EngEquipBaseModelName
        {
            get { return engEquipBaseModelName; }
            set { engEquipBaseModelName = value; }
        }

        public int EngEquipBaseMakeId
        {
            get { return engEquipBaseMakeId; }
            set { engEquipBaseMakeId = value; }
        }

        public EngEquipBaseMake EngEquipBaseMake
        {
            get
            {
                if (engEquipBaseMake == null)
                    engEquipBaseMake = EngEquipBaseMakeUtil.GetEngEquipBaseMake(engEquipBaseMakeId, CurrentUser);

                return engEquipBaseMake;
            }
            set { engEquipBaseMake = value; }
        }

        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; }
        }

        //IDropDownItem
        public string Value()
        {
            return EngEquipBaseModelId.ToString();
        }

        public string Text()
        {
            return EngEquipBaseModelName.ToString();
        }

        public EngEquipBaseModel(User user)
            : base(user)
        {
        }
    }

    public class EngEquipBaseModelFilter
    {
        public int? EngEquipBaseMakeId { get; set; }

        public int OrderBy { get; set; }

        public int PageIdx { get; set; }

        public int RowsPerPage { get; set; }
    }

    public static class EngEquipBaseModelUtil
    {
        //This method creates and returns a EngEquipBaseModel object. It extracts the data from a DataReader.
        public static EngEquipBaseModel ExtractEngEquipBaseModelFromDataReader(OracleDataReader dr, User currentUser)
        {
            EngEquipBaseModel engEquipBaseModel = new EngEquipBaseModel(currentUser);

            engEquipBaseModel.EngEquipBaseModelId = DBCommon.GetInt(dr["EngEquipBaseModelID"]);
            engEquipBaseModel.EngEquipBaseModelName = dr["EngEquipBaseModelName"].ToString();
            engEquipBaseModel.EngEquipBaseMakeId = DBCommon.GetInt(dr["EngEquipBaseMakeID"]);
            engEquipBaseModel.CanDelete = DBCommon.GetInt(dr["CanDelete"]) == 1;

            engEquipBaseModel.EngEquipBaseMake = EngEquipBaseMakeUtil.GetEngEquipBaseMake(engEquipBaseModel.EngEquipBaseMakeId, currentUser);

            return engEquipBaseModel;
        }

        //Get a particular object by its ID
        public static EngEquipBaseModel GetEngEquipBaseModel(int engEquipBaseModelId, User currentUser)
        {
            EngEquipBaseModel engEquipBaseModel = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EngEquipBaseModelID, a.EngEquipBaseModelName, a.EngEquipBaseMakeID,
                                    CASE WHEN v.EngEquipBaseModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete
                               FROM PMIS_RES.EngEquipBaseModels a
                               LEFT JOIN (SELECT DISTINCT EngEquipBaseModelID FROM PMIS_RES.EngEquipment) v ON a.EngEquipBaseModelID = v.EngEquipBaseModelID
                               WHERE a.EngEquipBaseModelID = :EngEquipBaseModelID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EngEquipBaseModelID", OracleType.Number).Value = engEquipBaseModelId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    engEquipBaseModel = ExtractEngEquipBaseModelFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipBaseModel;
        }

        //Get a list of all model by a make
        public static List<EngEquipBaseModel> GetAllEngEquipBaseModels(int engEquipBaseMakeId, User currentUser)
        {
            List<EngEquipBaseModel> engEquipBaseModels = new List<EngEquipBaseModel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EngEquipBaseModelID, a.EngEquipBaseModelName, a.EngEquipBaseMakeID,
                                    CASE WHEN v.EngEquipBaseModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete
                               FROM PMIS_RES.EngEquipBaseModels a
                               LEFT JOIN (SELECT DISTINCT EngEquipBaseModelID FROM PMIS_RES.EngEquipment) v ON a.EngEquipBaseModelID = v.EngEquipBaseModelID
                               WHERE a.EngEquipBaseMakeID = :EngEquipBaseMakeID
                               ORDER BY a.EngEquipBaseModelName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EngEquipBaseMakeID", OracleType.Number).Value = engEquipBaseMakeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    engEquipBaseModels.Add(ExtractEngEquipBaseModelFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipBaseModels;
        }

        //Get all engEquipBase models by specified filter
        public static List<EngEquipBaseModel> GetAllEngEquipBaseModelsByFilter(EngEquipBaseModelFilter filter, User currentUser)
        {
            List<EngEquipBaseModel> engEquipBaseModels = new List<EngEquipBaseModel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.EngEquipBaseMakeId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.EngEquipBaseMakeID = " + filter.EngEquipBaseMakeId.Value + " ";
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
                        orderBySQL = "a.EngEquipBaseModelName";
                        break;
                    case 2:
                        orderBySQL = "b.EngEquipBaseMakeName";
                        break;
                    default:
                        orderBySQL = "a.EngEquipBaseMakeName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.EngEquipBaseModelID, a.EngEquipBaseModelName, b.EngEquipBaseMakeID, b.EngEquipBaseMakeName,
                                    CASE WHEN v.EngEquipBaseModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.EngEquipBaseModelID) as RowNumber 
                                FROM PMIS_RES.EngEquipBaseModels a
                                INNER JOIN PMIS_RES.EngEquipBaseMakes b ON a.EngEquipBaseMakeID = b.EngEquipBaseMakeID
                                LEFT JOIN (SELECT DISTINCT EngEquipBaseModelID FROM PMIS_RES.EngEquipment) v ON a.EngEquipBaseModelID = v.EngEquipBaseModelID
                              " + where + @"    
                              ORDER BY " + orderBySQL + @", a.EngEquipBaseModelID
                           ) tmp
                           " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EngEquipBaseModel engEquipBaseModel = ExtractEngEquipBaseModelFromDataReader(dr, currentUser);
                    engEquipBaseModels.Add(engEquipBaseModel);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipBaseModels;
        }

        public static int GetAllEngEquipBaseModelsByFilterCount(EngEquipBaseModelFilter filter, User currentUser)
        {
            int engEquipBaseModelsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.EngEquipBaseMakeId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.EngEquipBaseMakeID = " + filter.EngEquipBaseMakeId.Value + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.EngEquipBaseModels a
                                INNER JOIN PMIS_RES.EngEquipBaseMakes b ON a.EngEquipBaseMakeID = b.EngEquipBaseMakeID                       
                              " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        engEquipBaseModelsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return engEquipBaseModelsCnt;
        }

        //Save a particular EngEquipBase Model into the DB
        public static bool SaveEngEquipBaseModel(EngEquipBaseModel engEquipBaseModel, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            EngEquipBaseMake engEquipBaseMake = EngEquipBaseMakeUtil.GetEngEquipBaseMake(engEquipBaseModel.EngEquipBaseMakeId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (engEquipBaseModel.EngEquipBaseModelId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.EngEquipBaseModels (EngEquipBaseModelName, EngEquipBaseMakeID)
                             VALUES (:EngEquipBaseModelName, :EngEquipBaseMakeID);
                            
                             SELECT PMIS_RES.EngEquipBaseModels_ID_SEQ.currval INTO :EngEquipBaseModelID FROM dual;
                            ";

                    changeEvent = new ChangeEvent("RES_Lists_EngEquipBaseModels_Add", "Модел от марка: " + engEquipBaseMake.EngEquipBaseMakeName, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_EngEquipBaseModels_EngEquipBaseModelName", "",
                        engEquipBaseModel.EngEquipBaseModelName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.EngEquipBaseModels SET
                               EngEquipBaseModelName = :EngEquipBaseModelName,
                               EngEquipBaseMakeID = :EngEquipBaseMakeID
                             WHERE EngEquipBaseModelID = :EngEquipBaseModelID;
                            ";

                    EngEquipBaseModel oldEngEquipBaseModel = GetEngEquipBaseModel(engEquipBaseModel.EngEquipBaseModelId, currentUser);

                    changeEvent = new ChangeEvent("RES_Lists_EngEquipBaseModels_Edit", "Модел от марка: " + engEquipBaseModel.EngEquipBaseMake.EngEquipBaseMakeName, null, null, currentUser);

                    if (oldEngEquipBaseModel.EngEquipBaseModelName != engEquipBaseModel.EngEquipBaseModelName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_EngEquipBaseModels_EngEquipBaseModelName",
                                     oldEngEquipBaseModel.EngEquipBaseModelName, engEquipBaseModel.EngEquipBaseModelName, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramEngEquipBaseModelID = new OracleParameter();
                paramEngEquipBaseModelID.ParameterName = "EngEquipBaseModelID";
                paramEngEquipBaseModelID.OracleType = OracleType.Number;

                if (engEquipBaseModel.EngEquipBaseModelId != 0)
                {
                    paramEngEquipBaseModelID.Direction = ParameterDirection.Input;
                    paramEngEquipBaseModelID.Value = engEquipBaseModel.EngEquipBaseModelId;
                }
                else
                {
                    paramEngEquipBaseModelID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramEngEquipBaseModelID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "EngEquipBaseModelName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(engEquipBaseModel.EngEquipBaseModelName))
                    param.Value = engEquipBaseModel.EngEquipBaseModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EngEquipBaseMakeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = engEquipBaseModel.EngEquipBaseMakeId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (engEquipBaseModel.EngEquipBaseModelId == 0)
                    engEquipBaseModel.EngEquipBaseModelId = DBCommon.GetInt(paramEngEquipBaseModelID.Value);

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
        public static bool DeleteEngEquipBaseModel(int engEquipBaseModelId, User currentUser, Change changeEntry)
        {
            bool result = false;

            EngEquipBaseModel oldEngEquipBaseModel = EngEquipBaseModelUtil.GetEngEquipBaseModel(engEquipBaseModelId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Lists_EngEquipBaseModels_Delete", "Модел от марка: " + oldEngEquipBaseModel.EngEquipBaseMake.EngEquipBaseMakeName, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_EngEquipBaseModels_EngEquipBaseModelName", oldEngEquipBaseModel.EngEquipBaseModelName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" DELETE FROM PMIS_RES.EngEquipBaseModels WHERE EngEquipBaseModelID = :EngEquipBaseModelID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EngEquipBaseModelID", OracleType.Number).Value = engEquipBaseModelId;

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