using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class TractorModel : BaseDbObject, IDropDownItem
    {
        private int tractorModelId;
        private string tractorModelName;
        private int tractorMakeId;
        private TractorMake tractorMake;
        private bool canDelete;

        public int TractorModelId
        {
            get { return tractorModelId; }
            set { tractorModelId = value; }
        }

        public string TractorModelName
        {
            get { return tractorModelName; }
            set { tractorModelName = value; }
        }

        public int TractorMakeId
        {
            get { return tractorMakeId; }
            set { tractorMakeId = value; }
        }

        public TractorMake TractorMake
        {
            get
            {
                if (tractorMake == null)
                    tractorMake = TractorMakeUtil.GetTractorMake(tractorMakeId, CurrentUser);

                return tractorMake;
            }
            set { tractorMake = value; }
        }

        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; }
        }

        //IDropDownItem
        public string Value()
        {
            return TractorModelId.ToString();
        }

        public string Text()
        {
            return TractorModelName.ToString();
        }

        public TractorModel(User user)
            : base(user)
        {
        }
    }

    public class TractorModelFilter
    {
        public int? TractorMakeId { get; set; }

        public int OrderBy { get; set; }

        public int PageIdx { get; set; }

        public int RowsPerPage { get; set; }
    }

    public static class TractorModelUtil
    {
        //This method creates and returns a TractorModel object. It extracts the data from a DataReader.
        public static TractorModel ExtractTractorModelFromDataReader(OracleDataReader dr, User currentUser)
        {
            TractorModel tractorModel = new TractorModel(currentUser);

            tractorModel.TractorModelId = DBCommon.GetInt(dr["TractorModelID"]);
            tractorModel.TractorModelName = dr["TractorModelName"].ToString();
            tractorModel.TractorMakeId = DBCommon.GetInt(dr["TractorMakeID"]);
            tractorModel.CanDelete = DBCommon.GetInt(dr["CanDelete"]) == 1;

            tractorModel.TractorMake = TractorMakeUtil.GetTractorMake(tractorModel.TractorMakeId, currentUser);

            return tractorModel;
        }

        //Get a particular object by its ID
        public static TractorModel GetTractorModel(int tractorModelId, User currentUser)
        {
            TractorModel tractorModel = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TractorModelID, a.TractorModelName, a.TractorMakeID,
                                    CASE WHEN v.TractorModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete
                               FROM PMIS_RES.TractorModels a  
                               LEFT JOIN (SELECT DISTINCT TractorModelID FROM PMIS_RES.Tractors) v ON a.TractorModelID = v.TractorModelID
                               WHERE a.TractorModelID = :TractorModelID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TractorModelID", OracleType.Number).Value = tractorModelId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tractorModel = ExtractTractorModelFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorModel;
        }

        //Get a list of all model by a make
        public static List<TractorModel> GetAllTractorModels(int tractorMakeId, User currentUser)
        {
            List<TractorModel> tractorModels = new List<TractorModel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TractorModelID, a.TractorModelName, a.TractorMakeID,
                                    CASE WHEN v.TractorModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete
                               FROM PMIS_RES.TractorModels a
                               LEFT JOIN (SELECT DISTINCT TractorModelID FROM PMIS_RES.Tractors) v ON a.TractorModelID = v.TractorModelID
                               WHERE a.TractorMakeID = :TractorMakeID
                               ORDER BY a.TractorModelName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TractorMakeID", OracleType.Number).Value = tractorMakeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    tractorModels.Add(ExtractTractorModelFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorModels;
        }

        //Get all tractor models by specified filter
        public static List<TractorModel> GetAllTractorModelsByFilter(TractorModelFilter filter, User currentUser)
        {
            List<TractorModel> tractorModels = new List<TractorModel>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.TractorMakeId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TractorMakeID = " + filter.TractorMakeId.Value + " ";
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
                        orderBySQL = "a.TractorModelName";
                        break;
                    case 2:
                        orderBySQL = "b.TractorMakeName";
                        break;
                    default:
                        orderBySQL = "a.TractorMakeName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.TractorModelID, a.TractorModelName, b.TractorMakeID, b.TractorMakeName,
                                    CASE WHEN v.TractorModelID IS NULL 
                                                     THEN 1
                                                     ELSE 0
                                                END as CanDelete,
                                  RANK() OVER (ORDER BY " + orderBySQL + @", a.TractorModelID) as RowNumber 
                                FROM PMIS_RES.TractorModels a
                                INNER JOIN PMIS_RES.TractorMakes b ON a.TractorMakeID = b.TractorMakeID
                                LEFT JOIN (SELECT DISTINCT TractorModelID FROM PMIS_RES.Tractors) v ON a.TractorModelID = v.TractorModelID
                              " + where + @"    
                              ORDER BY " + orderBySQL + @", a.TractorModelID
                           ) tmp
                           " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    TractorModel tractorModel = ExtractTractorModelFromDataReader(dr, currentUser);
                    tractorModels.Add(tractorModel);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorModels;
        }

        public static int GetAllTractorModelsByFilterCount(TractorModelFilter filter, User currentUser)
        {
            int tractorModelsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.TractorMakeId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.TractorMakeID = " + filter.TractorMakeId.Value + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.TractorModels a
                                INNER JOIN PMIS_RES.TractorMakes b ON a.TractorMakeID = b.TractorMakeID                       
                              " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        tractorModelsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tractorModelsCnt;
        }

        //Save a particular Tractor Model into the DB
        public static bool SaveTractorModel(TractorModel tractorModel, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            TractorMake tractorMake = TractorMakeUtil.GetTractorMake(tractorModel.TractorMakeId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (tractorModel.TractorModelId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.TractorModels (TractorModelName, TractorMakeID)
                             VALUES (:TractorModelName, :TractorMakeID);
                            
                             SELECT PMIS_RES.TractorModels_ID_SEQ.currval INTO :TractorModelID FROM dual;
                            ";

                    changeEvent = new ChangeEvent("RES_Lists_TractorModels_Add", "Модел от марка: " + tractorMake.TractorMakeName, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_TractorModels_TractorModelName", "",
                        tractorModel.TractorModelName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.TractorModels SET
                               TractorModelName = :TractorModelName,
                               TractorMakeID = :TractorMakeID
                             WHERE TractorModelID = :TractorModelID;
                            ";

                    TractorModel oldTractorModel = GetTractorModel(tractorModel.TractorModelId, currentUser);

                    changeEvent = new ChangeEvent("RES_Lists_TractorModels_Edit", "Модел от марка: " + tractorModel.TractorMake.TractorMakeName, null, null, currentUser);

                    if (oldTractorModel.TractorModelName != tractorModel.TractorModelName)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_TractorModels_TractorModelName",
                                     oldTractorModel.TractorModelName, tractorModel.TractorModelName, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTractorModelID = new OracleParameter();
                paramTractorModelID.ParameterName = "TractorModelID";
                paramTractorModelID.OracleType = OracleType.Number;

                if (tractorModel.TractorModelId != 0)
                {
                    paramTractorModelID.Direction = ParameterDirection.Input;
                    paramTractorModelID.Value = tractorModel.TractorModelId;
                }
                else
                {
                    paramTractorModelID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramTractorModelID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TractorModelName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(tractorModel.TractorModelName))
                    param.Value = tractorModel.TractorModelName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TractorMakeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = tractorModel.TractorMakeId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (tractorModel.TractorModelId == 0)
                    tractorModel.TractorModelId = DBCommon.GetInt(paramTractorModelID.Value);

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

        //Delete a particular Tractor Model from the DB
        public static bool DeleteTractorModel(int tractorModelId, User currentUser, Change changeEntry)
        {
            bool result = false;

            TractorModel oldTractorModel = TractorModelUtil.GetTractorModel(tractorModelId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Lists_TractorModels_Delete", "Модел от марка: " + oldTractorModel.TractorMake.TractorMakeName, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Lists_TractorModels_TractorModelName", oldTractorModel.TractorModelName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" DELETE FROM PMIS_RES.TractorModels WHERE TractorModelID = :TractorModelID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TractorModelID", OracleType.Number).Value = tractorModelId;

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