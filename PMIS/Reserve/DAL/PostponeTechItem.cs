using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class PostponeTechItem : BaseDbObject
    {
        public int PostponeTechItemID {get; set;}
        public int PostponeTechCompanyID {get; set;}
        public int TechnicsSubTypeID {get; set;}
        
        public int? PostponeAbsolutely {get; set;}
        public int? PostponeConditioned {get; set;}
        
        private TechnicsSubType technicsSubType;
        public TechnicsSubType TechnicsSubType
        {
            get 
            {
                if (technicsSubType == null)
                    technicsSubType = TechnicsSubTypeUtil.GetTechnicsSubType(TechnicsSubTypeID, CurrentUser);

                return technicsSubType; 
            }
            set { technicsSubType = value; }
        }

        private PostponeTechCompany postponeTechCompany;
        public PostponeTechCompany PostponeTechCompany
        {
            get 
            {
                if (postponeTechCompany == null)
                    postponeTechCompany = PostponeTechCompanyUtil.GetPostponeTechCompany(PostponeTechCompanyID, CurrentUser);

                return postponeTechCompany; 
            }
            set { postponeTechCompany = value; }
        }


        public PostponeTechItem(User user)
            :base(user)
        {

        }
    }

    public static class PostponeTechItemUtil
    {
        public static PostponeTechItem ExtractPostponeTechItemFromDataReader(OracleDataReader dr, User currentUser)
        {
            PostponeTechItem postponeTechItem = new PostponeTechItem(currentUser);

            postponeTechItem.PostponeTechItemID = DBCommon.GetInt(dr["PostponeTechItemID"]);
            postponeTechItem.PostponeTechCompanyID = DBCommon.GetInt(dr["PostponeTechCompanyID"]);
            postponeTechItem.TechnicsSubTypeID = DBCommon.GetInt(dr["TechnicsSubTypeID"]);

            postponeTechItem.PostponeConditioned = DBCommon.IsInt(dr["PostponeConditioned"]) ? (int?)DBCommon.GetInt(dr["PostponeConditioned"]) : null;
            postponeTechItem.PostponeAbsolutely = DBCommon.IsInt(dr["PostponeAbsolutely"]) ? (int?)DBCommon.GetInt(dr["PostponeAbsolutely"]) : null;

            return postponeTechItem;
        }

        public static PostponeTechItem GetPostponeTechItem(int postponeTechItemID, User currentUser)
        {
            PostponeTechItem postponeTechItem = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.PostponeTechItemID,
                                       a.PostponeTechCompanyID,
                                       a.TechnicsSubTypeID,
                                       a.PostponeConditioned,
                                       a.PostponeAbsolutely
                                FROM PMIS_RES.PostponeTechItems a
                                WHERE a.PostponeTechItemID = :PostponeTechItemID
                                ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeTechItemID", OracleType.Number).Value = postponeTechItemID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeTechItem = ExtractPostponeTechItemFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeTechItem;
        }

        public static List<PostponeTechItem> GetAllPostponeTechItemsByPostponeTechCompanyID(int postponeTechCompanyID, User currentUser)
        {
            List<PostponeTechItem> postponeTechItems = new List<PostponeTechItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                where = " a.PostponeTechCompanyID =  " + postponeTechCompanyID.ToString();

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT a.PostponeTechItemID,
                                       a.PostponeTechCompanyID,
                                       a.TechnicsSubTypeID,
                                       a.PostponeConditioned,
                                       a.PostponeAbsolutely
                                FROM PMIS_RES.PostponeTechItems a
                                INNER JOIN PMIS_RES.TechnicsSubTypes b ON a.TechnicsSubTypeID = b.TechnicsSubTypeID
                                INNER JOIN PMIS_RES.TechnicsTypes c ON b.TechnicsTypeID = c.TechnicsTypeID
                              " + where + @"
                                ORDER BY c.Seq, b.Seq
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    postponeTechItems.Add(ExtractPostponeTechItemFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeTechItems;
        }

        public static List<PostponeTechItem> GetDefaultPostponeTechItems(User currentUser)
        {
            List<PostponeTechItem> postponeTechItems = new List<PostponeTechItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT 0 as PostponeTechItemID,
                                       0 as PostponeTechCompanyID,
                                       a.TechnicsSubTypeID,
                                       NULL as PostponeConditioned,
                                       NULL as PostponeAbsolutely
                                FROM PMIS_RES.TechnicsSubTypes a
                                INNER JOIN PMIS_RES.TechnicsTypes b ON a.TechnicsTypeID = b.TechnicsTypeID
                                WHERE NVL(a.IsActive, 0) = 1 AND NVL(b.Active, 0) = 1
                                ORDER BY b.Seq, a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    postponeTechItems.Add(ExtractPostponeTechItemFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeTechItems;
        }

        public static bool SavePostponeTechItem(int postponeTechID, int postponeTechCompanyID, PostponeTechItem postponeTechItem, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            PostponeTech postponeTech = PostponeTechUtil.GetPostponeTech(postponeTechID, currentUser);
            PostponeTechCompany postponeTechCompany = PostponeTechCompanyUtil.GetPostponeTechCompany(postponeTechCompanyID, currentUser);
            
            string logDescription = "";
            logDescription += "Година: " + postponeTech.PostponeYear;
            logDescription += "; ВО: " + postponeTech.MilitaryDepartment.MilitaryDepartmentName;
            logDescription += "; Фирма: " + (postponeTechCompany.Company != null ? postponeTechCompany.Company.CompanyName : "");
            logDescription += "; Техника: " + postponeTechItem.TechnicsSubType.TechnicsType.TypeName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (postponeTechItem.PostponeTechItemID == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.PostponeTechItems (PostponeTechCompanyID,
                                                                     TechnicsSubTypeID,
                                                                     PostponeConditioned,
                                                                     PostponeAbsolutely)
                                                             VALUES (:PostponeTechCompanyID,
                                                                     :TechnicsSubTypeID,
                                                                     :PostponeConditioned,
                                                                     :PostponeAbsolutely);  

                            SELECT PMIS_RES.PostponeTechItems_ID_SEQ.currval INTO :PostponeTechItemID FROM dual;                         
                            ";

                    changeEvent = new ChangeEvent("RES_PostponeTech_AddPostponeTechItem", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechItem_TechnicsSubType", "", postponeTechItem.TechnicsSubType != null ? postponeTechItem.TechnicsSubType.TechnicsSubTypeName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechItem_PostponeConditioned", "", (postponeTechItem.PostponeConditioned.HasValue ? postponeTechItem.PostponeConditioned.Value.ToString() : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechItem_PostponeAbsolutely", "", (postponeTechItem.PostponeAbsolutely.HasValue ? postponeTechItem.PostponeAbsolutely.Value.ToString() : ""), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.PostponeTechItems SET
                                PostponeTechCompanyID = :PostponeTechCompanyID,
                                TechnicsSubTypeID = :TechnicsSubTypeID,
                                PostponeConditioned = :PostponeConditioned,
                                PostponeAbsolutely = :PostponeAbsolutely
                             WHERE PostponeTechItemID = :PostponeTechItemID;

                            ";

                    logDescription += "; Тип техника: " + (postponeTechItem.TechnicsSubType != null ? postponeTechItem.TechnicsSubType.TechnicsSubTypeName : "");

                    changeEvent = new ChangeEvent("RES_PostponeTech_EditPostponeTechItem", logDescription, null, null, currentUser);

                    PostponeTechItem oldPostponeTechItem = PostponeTechItemUtil.GetPostponeTechItem(postponeTechItem.PostponeTechItemID, currentUser);

                    if (oldPostponeTechItem.TechnicsSubTypeID != postponeTechItem.TechnicsSubTypeID)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechItem_TechnicsSubType", (oldPostponeTechItem.TechnicsSubType != null ? oldPostponeTechItem.TechnicsSubType.TechnicsSubTypeName : ""), (postponeTechItem.TechnicsSubType != null ? postponeTechItem.TechnicsSubType.TechnicsSubTypeName : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeTechItem.PostponeConditioned, postponeTechItem.PostponeConditioned))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechItem_PostponeConditioned", (oldPostponeTechItem.PostponeConditioned.HasValue ? oldPostponeTechItem.PostponeConditioned.Value.ToString() : ""), (postponeTechItem.PostponeConditioned.HasValue ? postponeTechItem.PostponeConditioned.Value.ToString() : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeTechItem.PostponeAbsolutely, postponeTechItem.PostponeAbsolutely))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechItem_PostponeAbsolutely", (oldPostponeTechItem.PostponeAbsolutely.HasValue ? oldPostponeTechItem.PostponeAbsolutely.Value.ToString() : ""), (postponeTechItem.PostponeAbsolutely.HasValue ? postponeTechItem.PostponeAbsolutely.Value.ToString() : ""), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPostponeTechItemID = new OracleParameter();
                paramPostponeTechItemID.ParameterName = "PostponeTechItemID";
                paramPostponeTechItemID.OracleType = OracleType.Number;

                if (postponeTechItem.PostponeTechItemID != 0)
                {
                    paramPostponeTechItemID.Direction = ParameterDirection.Input;
                    paramPostponeTechItemID.Value = postponeTechItem.PostponeTechItemID;
                }
                else
                {
                    paramPostponeTechItemID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPostponeTechItemID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PostponeTechCompanyID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = postponeTechCompanyID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsSubTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = postponeTechItem.TechnicsSubTypeID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PostponeConditioned";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeTechItem.PostponeConditioned.HasValue)
                    param.Value = postponeTechItem.PostponeConditioned.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PostponeAbsolutely";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeTechItem.PostponeAbsolutely.HasValue)
                    param.Value = postponeTechItem.PostponeAbsolutely.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (postponeTechItem.PostponeTechItemID == 0)
                    postponeTechItem.PostponeTechItemID = DBCommon.GetInt(paramPostponeTechItemID.Value);

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                {
                    PostponeTechUtil.SetPostponeTechModified(postponeTechID, currentUser);
                }

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }
    }
}