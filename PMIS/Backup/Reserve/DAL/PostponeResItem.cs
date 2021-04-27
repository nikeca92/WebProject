using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class PostponeResItem : BaseDbObject
    {
        public int PostponeResItemID {get; set;}
        public int PostponeResCompanyID {get; set;}
        public int NKPDID {get; set;}
        
        public int? OfficersConditioned {get; set;}
        public int? OfficersAbsolutely {get; set;}
        public int? OfCandConditioned {get; set;}
        public int? OfCandAbsolutely {get; set;}
        public int? SergeantsConditioned {get; set;}
        public int? SergeantsAbsolutely {get; set;}
        public int? SoldiersConditioned {get; set;}
        public int? SoldiersAbsolutely {get; set;}

        private NKPD nkpd;
        public NKPD NKPD
        {
            get 
            {
                if (nkpd == null)
                    nkpd = NKPDUtil.GetNKPD(NKPDID, CurrentUser);

                return nkpd; 
            }
            set { nkpd = value; }
        }

         private PostponeResCompany postponeResCompany;
        public PostponeResCompany PostponeResCompany
        {
            get 
            {
                if (postponeResCompany == null)
                    postponeResCompany = PostponeResCompanyUtil.GetPostponeResCompany(PostponeResCompanyID, CurrentUser);

                return postponeResCompany; 
            }
            set { postponeResCompany = value; }
        }


        public PostponeResItem(User user)
            :base(user)
        {

        }
    }

    public static class PostponeResItemUtil
    {
        public static PostponeResItem ExtractPostponeResItemFromDataReader(OracleDataReader dr, User currentUser)
        {
            PostponeResItem postponeResItem = new PostponeResItem(currentUser);

            postponeResItem.PostponeResItemID = DBCommon.GetInt(dr["PostponeResItemID"]);
            postponeResItem.PostponeResCompanyID = DBCommon.GetInt(dr["PostponeResCompanyID"]);
            postponeResItem.NKPDID = DBCommon.GetInt(dr["NKPDID"]);

            postponeResItem.OfficersConditioned = DBCommon.IsInt(dr["OfficersConditioned"]) ? (int?)DBCommon.GetInt(dr["OfficersConditioned"]) : null;
            postponeResItem.OfficersAbsolutely = DBCommon.IsInt(dr["OfficersAbsolutely"]) ? (int?)DBCommon.GetInt(dr["OfficersAbsolutely"]) : null;
            postponeResItem.OfCandConditioned = DBCommon.IsInt(dr["OfCandConditioned"]) ? (int?)DBCommon.GetInt(dr["OfCandConditioned"]) : null;
            postponeResItem.OfCandAbsolutely = DBCommon.IsInt(dr["OfCandAbsolutely"]) ? (int?)DBCommon.GetInt(dr["OfCandAbsolutely"]) : null;
            postponeResItem.SergeantsConditioned = DBCommon.IsInt(dr["SergeantsConditioned"]) ? (int?)DBCommon.GetInt(dr["SergeantsConditioned"]) : null;
            postponeResItem.SergeantsAbsolutely = DBCommon.IsInt(dr["SergeantsAbsolutely"]) ? (int?)DBCommon.GetInt(dr["SergeantsAbsolutely"]) : null;
            postponeResItem.SoldiersConditioned = DBCommon.IsInt(dr["SoldiersConditioned"]) ? (int?)DBCommon.GetInt(dr["SoldiersConditioned"]) : null;
            postponeResItem.SoldiersAbsolutely = DBCommon.IsInt(dr["SoldiersAbsolutely"]) ? (int?)DBCommon.GetInt(dr["SoldiersAbsolutely"]) : null;

            return postponeResItem;
        }

        public static PostponeResItem GetPostponeResItem(int postponeResItemID, User currentUser)
        {
            PostponeResItem postponeResItem = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.PostponeResItemID,
                                       a.PostponeResCompanyID,
                                       a.NKPDID,
                                       a.OfficersConditioned,
                                       a.OfficersAbsolutely,
                                       a.OfCandConditioned,
                                       a.OfCandAbsolutely,
                                       a.SergeantsConditioned,
                                       a.SergeantsAbsolutely,
                                       a.SoldiersConditioned,
                                       a.SoldiersAbsolutely
                                FROM PMIS_RES.PostponeResItems a
                                WHERE a.PostponeResItemID = :PostponeResItemID
                                ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeResItemID", OracleType.Number).Value = postponeResItemID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeResItem = ExtractPostponeResItemFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeResItem;
        }

        public static List<PostponeResItem> GetAllPostponeResItemsByPostponeResCompanyID(int postponeResCompanyID, User currentUser)
        {
            List<PostponeResItem> postponeResItems = new List<PostponeResItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                where = " a.PostponeResCompanyID =  " + postponeResCompanyID.ToString();

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT a.PostponeResItemID,
                                       a.PostponeResCompanyID,
                                       a.NKPDID,
                                       a.OfficersConditioned,
                                       a.OfficersAbsolutely,
                                       a.OfCandConditioned,
                                       a.OfCandAbsolutely,
                                       a.SergeantsConditioned,
                                       a.SergeantsAbsolutely,
                                       a.SoldiersConditioned,
                                       a.SoldiersAbsolutely
                                FROM PMIS_RES.PostponeResItems a
                              " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    postponeResItems.Add(ExtractPostponeResItemFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeResItems;
        }

        public static List<PostponeResItem> GetDefaultPostponeResItems(User currentUser)
        {
            List<PostponeResItem> postponeResItems = new List<PostponeResItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT 0 as PostponeResItemID,
                                       0 as PostponeResCompanyID,
                                       a.NKPDID,
                                       NULL as OfficersConditioned,
                                       NULL as OfficersAbsolutely,
                                       NULL as OfCandConditioned,
                                       NULL as OfCandAbsolutely,
                                       NULL as SergeantsConditioned,
                                       NULL as SergeantsAbsolutely,
                                       NULL as SoldiersConditioned,
                                       NULL as SoldiersAbsolutely
                                FROM PMIS_ADM.NKPD a
                                WHERE a.NKPDParentID IS NULL AND NVL(a.IsActive, 0) = 1
                                ORDER BY a.NKPDCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    postponeResItems.Add(ExtractPostponeResItemFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeResItems;
        }

        public static bool SavePostponeResItem(int postponeResID, int postponeResCompanyID, PostponeResItem postponeResItem, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            PostponeRes postponeRes = PostponeResUtil.GetPostponeRes(postponeResID, currentUser);
            PostponeResCompany postponeResCompany = PostponeResCompanyUtil.GetPostponeResCompany(postponeResCompanyID, currentUser);
            
            string logDescription = "";
            logDescription += "Година: " + postponeRes.PostponeYear;
            logDescription += "; ВО: " + postponeRes.MilitaryDepartment.MilitaryDepartmentName;
            logDescription += "; Фирма: " + (postponeResCompany.Company != null ? postponeResCompany.Company.CompanyName : "");

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (postponeResItem.PostponeResItemID == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.PostponeResItems (PostponeResCompanyID,
                                                                    NKPDID,
                                                                    OfficersConditioned,
                                                                    OfficersAbsolutely,
                                                                    OfCandConditioned,
                                                                    OfCandAbsolutely,
                                                                    SergeantsConditioned,
                                                                    SergeantsAbsolutely,
                                                                    SoldiersConditioned,
                                                                    SoldiersAbsolutely)
                                                                VALUES (:PostponeResCompanyID,
                                                                        :NKPDID,
                                                                        :OfficersConditioned,
                                                                        :OfficersAbsolutely,
                                                                        :OfCandConditioned,
                                                                        :OfCandAbsolutely,
                                                                        :SergeantsConditioned,
                                                                        :SergeantsAbsolutely,
                                                                        :SoldiersConditioned,
                                                                        :SoldiersAbsolutely);  

                            SELECT PMIS_RES.PostponeResItems_ID_SEQ.currval INTO :PostponeResItemID FROM dual;                         
                            ";

                    changeEvent = new ChangeEvent("RES_PostponeRes_AddPostponeResItem", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_NKPD", "", postponeResItem.NKPD != null ? postponeResItem.NKPD.Nickname : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_OfficersConditioned", "", (postponeResItem.OfficersConditioned.HasValue ? postponeResItem.OfficersConditioned.Value.ToString() : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_OfficersAbsolutely", "", (postponeResItem.OfficersAbsolutely.HasValue ? postponeResItem.OfficersAbsolutely.Value.ToString() : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_OfCandConditioned", "", (postponeResItem.OfCandConditioned.HasValue ? postponeResItem.OfCandConditioned.Value.ToString() : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_OfCandAbsolutely", "", (postponeResItem.OfCandAbsolutely.HasValue ? postponeResItem.OfCandAbsolutely.Value.ToString() : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_SergeantsConditioned", "", (postponeResItem.SergeantsConditioned.HasValue ? postponeResItem.SergeantsConditioned.Value.ToString() : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_SergeantsAbsolutely", "", (postponeResItem.SergeantsAbsolutely.HasValue ? postponeResItem.SergeantsAbsolutely.Value.ToString() : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_SoldiersConditioned", "", (postponeResItem.SoldiersConditioned.HasValue ? postponeResItem.SoldiersConditioned.Value.ToString() : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_SoldiersAbsolutely", "", (postponeResItem.SoldiersAbsolutely.HasValue ? postponeResItem.SoldiersAbsolutely.Value.ToString() : ""), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.PostponeResItems SET
                                PostponeResCompanyID = :PostponeResCompanyID,
                                NKPDID = :NKPDID,
                                OfficersConditioned = :OfficersConditioned,
                                OfficersAbsolutely = :OfficersAbsolutely,
                                OfCandConditioned = :OfCandConditioned,
                                OfCandAbsolutely = :OfCandAbsolutely,
                                SergeantsConditioned = :SergeantsConditioned,
                                SergeantsAbsolutely = :SergeantsAbsolutely,
                                SoldiersConditioned = :SoldiersConditioned,
                                SoldiersAbsolutely = :SoldiersAbsolutely
                             WHERE PostponeResItemID = :PostponeResItemID;

                            ";

                    logDescription += "; Клас по НКПД: " + (postponeResItem.NKPD != null ? postponeResItem.NKPD.Nickname : "");

                    changeEvent = new ChangeEvent("RES_PostponeRes_EditPostponeResItem", logDescription, null, null, currentUser);

                    PostponeResItem oldPostponeResItem = PostponeResItemUtil.GetPostponeResItem(postponeResItem.PostponeResItemID, currentUser);

                    if (oldPostponeResItem.NKPDID != postponeResItem.NKPDID)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_NKPD", (oldPostponeResItem.NKPD != null ? oldPostponeResItem.NKPD.Nickname : ""), (postponeResItem.NKPD != null ? postponeResItem.NKPD.Nickname : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResItem.OfficersConditioned, postponeResItem.OfficersConditioned))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_OfficersConditioned", (oldPostponeResItem.OfficersConditioned.HasValue ? oldPostponeResItem.OfficersConditioned.Value.ToString() : ""), (postponeResItem.OfficersConditioned.HasValue ? postponeResItem.OfficersConditioned.Value.ToString() : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResItem.OfficersAbsolutely, postponeResItem.OfficersAbsolutely))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_OfficersAbsolutely", (oldPostponeResItem.OfficersAbsolutely.HasValue ? oldPostponeResItem.OfficersAbsolutely.Value.ToString() : ""), (postponeResItem.OfficersAbsolutely.HasValue ? postponeResItem.OfficersAbsolutely.Value.ToString() : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResItem.OfCandConditioned, postponeResItem.OfCandConditioned))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_OfCandConditioned", (oldPostponeResItem.OfCandConditioned.HasValue ? oldPostponeResItem.OfCandConditioned.Value.ToString() : ""), (postponeResItem.OfCandConditioned.HasValue ? postponeResItem.OfCandConditioned.Value.ToString() : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResItem.OfCandAbsolutely, postponeResItem.OfCandAbsolutely))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_OfCandAbsolutely", (oldPostponeResItem.OfCandAbsolutely.HasValue ? oldPostponeResItem.OfCandAbsolutely.Value.ToString() : ""), (postponeResItem.OfCandAbsolutely.HasValue ? postponeResItem.OfCandAbsolutely.Value.ToString() : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResItem.SergeantsConditioned, postponeResItem.SergeantsConditioned))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_SergeantsConditioned", (oldPostponeResItem.SergeantsConditioned.HasValue ? oldPostponeResItem.SergeantsConditioned.Value.ToString() : ""), (postponeResItem.SergeantsConditioned.HasValue ? postponeResItem.SergeantsConditioned.Value.ToString() : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResItem.SergeantsAbsolutely, postponeResItem.SergeantsAbsolutely))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_SergeantsAbsolutely", (oldPostponeResItem.SergeantsAbsolutely.HasValue ? oldPostponeResItem.SergeantsAbsolutely.Value.ToString() : ""), (postponeResItem.SergeantsAbsolutely.HasValue ? postponeResItem.SergeantsAbsolutely.Value.ToString() : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResItem.SoldiersConditioned, postponeResItem.SoldiersConditioned))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_SoldiersConditioned", (oldPostponeResItem.SoldiersConditioned.HasValue ? oldPostponeResItem.SoldiersConditioned.Value.ToString() : ""), (postponeResItem.SoldiersConditioned.HasValue ? postponeResItem.SoldiersConditioned.Value.ToString() : ""), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResItem.SoldiersAbsolutely, postponeResItem.SoldiersAbsolutely))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResItem_SoldiersAbsolutely", (oldPostponeResItem.SoldiersAbsolutely.HasValue ? oldPostponeResItem.SoldiersAbsolutely.Value.ToString() : ""), (postponeResItem.SoldiersAbsolutely.HasValue ? postponeResItem.SoldiersAbsolutely.Value.ToString() : ""), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPostponeResItemID = new OracleParameter();
                paramPostponeResItemID.ParameterName = "PostponeResItemID";
                paramPostponeResItemID.OracleType = OracleType.Number;

                if (postponeResItem.PostponeResItemID != 0)
                {
                    paramPostponeResItemID.Direction = ParameterDirection.Input;
                    paramPostponeResItemID.Value = postponeResItem.PostponeResItemID;
                }
                else
                {
                    paramPostponeResItemID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPostponeResItemID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PostponeResCompanyID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = postponeResCompanyID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NKPDID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = postponeResItem.NKPDID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OfficersConditioned";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResItem.OfficersConditioned.HasValue)
                    param.Value = postponeResItem.OfficersConditioned.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OfficersAbsolutely";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResItem.OfficersAbsolutely.HasValue)
                    param.Value = postponeResItem.OfficersAbsolutely.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OfCandConditioned";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResItem.OfCandConditioned.HasValue)
                    param.Value = postponeResItem.OfCandConditioned.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OfCandAbsolutely";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResItem.OfCandAbsolutely.HasValue)
                    param.Value = postponeResItem.OfCandAbsolutely.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SergeantsConditioned";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResItem.SergeantsConditioned.HasValue)
                    param.Value = postponeResItem.SergeantsConditioned.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SergeantsAbsolutely";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResItem.SergeantsAbsolutely.HasValue)
                    param.Value = postponeResItem.SergeantsAbsolutely.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SoldiersConditioned";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResItem.SoldiersConditioned.HasValue)
                    param.Value = postponeResItem.SoldiersConditioned.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SoldiersAbsolutely";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResItem.SoldiersAbsolutely.HasValue)
                    param.Value = postponeResItem.SoldiersAbsolutely.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (postponeResItem.PostponeResItemID == 0)
                    postponeResItem.PostponeResItemID = DBCommon.GetInt(paramPostponeResItemID.Value);

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                {
                    PostponeResUtil.SetPostponeResModified(postponeResID, currentUser);
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