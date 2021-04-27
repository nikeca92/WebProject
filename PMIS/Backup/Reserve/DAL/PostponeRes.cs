using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular PostponeRes into the system
    public class PostponeRes : BaseDbObject
    {
        private int postponeResID;        
        private int? postponeYear;        
        private int? militaryDepartmentID;        
        private MilitaryDepartment militaryDepartment;        
        private List<PostponeResCompany> postponeResCompanies;

        public int PostponeResID
        {
            get { return postponeResID; }
            set { postponeResID = value; }
        }
        public int? PostponeYear
        {
            get { return postponeYear; }
            set { postponeYear = value; }
        }

        public int? MilitaryDepartmentID
        {
            get { return militaryDepartmentID; }
            set { militaryDepartmentID = value; }
        }

        public MilitaryDepartment MilitaryDepartment
        {
            get 
            {
                if (militaryDepartment == null && militaryDepartmentID.HasValue)
                    militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID.Value, CurrentUser);

                return militaryDepartment; 
            }
            set { militaryDepartment = value; }
        }

        public List<PostponeResCompany> PostponeResCompanies
        {
            get 
            {
                if (postponeResCompanies == null)
                    postponeResCompanies = PostponeResCompanyUtil.GetPostponeResCompanies(
                        PostponeYear.HasValue ? PostponeYear.Value : 0, 
                        MilitaryDepartmentID.HasValue ? MilitaryDepartmentID.Value : 0, 
                        null, 
                        CurrentUser);

                return postponeResCompanies; 
            }
            set { postponeResCompanies = value; }
        }

        public bool CanDelete
        {
            get 
            { 
                return PostponeResCompanies.Count == 0; 
            }

        }

        public PostponeRes(User user)
            : base(user)
        {

        }
    }   

    public static class PostponeResUtil
    {
        public static PostponeRes ExtractPostponeRes(OracleDataReader dr, User currentUser)
        {
            PostponeRes postponeRes = new PostponeRes(currentUser);

            postponeRes.PostponeResID = DBCommon.GetInt(dr["PostponeResID"]);
            postponeRes.PostponeYear = (DBCommon.IsInt(dr["PostponeYear"]) ? DBCommon.GetInt(dr["PostponeYear"]) : (int?)null);
            postponeRes.MilitaryDepartmentID = (DBCommon.IsInt(dr["MilitaryDepartmentID"]) ? DBCommon.GetInt(dr["MilitaryDepartmentID"]) : (int?)null);

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, postponeRes);

            return postponeRes;
        }

        //Get a particular object by its ID
        public static PostponeRes GetPostponeRes(int postponeResID, User currentUser)
        {
            PostponeRes postponeRes = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_POSTPONE_RES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.PostponeResID,
                                      a.PostponeYear, a.MilitaryDepartmentID,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_RES.PostponeRes a                               
                               WHERE a.PostponeResID = :PostponeResID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeResID", OracleType.Number).Value = postponeResID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeRes = ExtractPostponeRes(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeRes;
        }

        public static PostponeRes GetPostponeRes(int postponeYear, int militaryDepartmentId, User currentUser)
        {
            PostponeRes postponeRes = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_POSTPONE_RES", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.PostponeResID,
                                      a.PostponeYear, a.MilitaryDepartmentID,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_RES.PostponeRes a                               
                               WHERE a.PostponeYear = :PostponeYear AND
                                     a.MilitaryDepartmentID = :MilitaryDepartmentID
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeYear", OracleType.Number).Value = postponeYear;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeRes = ExtractPostponeRes(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeRes;
        }

        public static bool IsThereAnyCompanyData(int militaryDepartmentID, int postponeYear, User currentUser)
        {
            bool result = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.PostponeRes a
                               INNER JOIN PMIS_RES.PostponeResCompanies b ON a.PostponeResID = b.PostponeResID
                               WHERE a.MilitaryDepartmentID = :MilitaryDepartmentID AND a.PostponeYear = :PostponeYear";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;
                cmd.Parameters.Add("PostponeYear", OracleType.Number).Value = postponeYear;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {                    
                    if (DBCommon.IsInt(dr["Cnt"]))
                        result = DBCommon.GetInt(dr["Cnt"]) == 0;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Save a particular object into the DB
        public static bool SavePostponeRes(PostponeRes postponeRes, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            string logDescription = "";
            logDescription += "Година: " + postponeRes.PostponeYear;
            logDescription += "; ВО: " + postponeRes.MilitaryDepartment.MilitaryDepartmentName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (postponeRes.PostponeResID == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.PostponeRes (PostponeYear, MilitaryDepartmentID,
                                                               CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:PostponeYear, :MilitaryDepartmentID,
                                    :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_RES.PostponeRes_ID_SEQ.currval INTO :PostponeResID FROM dual;
                            ";

                    changeEvent = new ChangeEvent("RES_PostponeRes_AddPostponeRes", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeRes_PostponeYear", "", postponeRes.PostponeYear.HasValue ? postponeRes.PostponeYear.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeRes_MilitaryDepartment", "", postponeRes.MilitaryDepartment != null && postponeRes.MilitaryDepartment.MilitaryDepartmentName != null ? postponeRes.MilitaryDepartment.MilitaryDepartmentName : "", currentUser));                
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.PostponeRes SET                              
                               PostponeYear = :PostponeYear, 
                               MilitaryDepartmentID = :MilitaryDepartmentID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END
                             WHERE PostponeResID = :PostponeResID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_PostponeRes_EditPostponeRes", logDescription, null, null, currentUser);

                    PostponeRes oldPostponeRes = GetPostponeRes(postponeRes.PostponeResID, currentUser);

                    if (!CommonFunctions.IsEqualInt(oldPostponeRes.PostponeYear, postponeRes.PostponeYear))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeRes_PostponeYear", oldPostponeRes.PostponeYear.HasValue ? oldPostponeRes.PostponeYear.Value.ToString() : "", postponeRes.PostponeYear.HasValue ? postponeRes.PostponeYear.Value.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeRes.MilitaryDepartmentID, postponeRes.MilitaryDepartmentID))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Postpone_MilitaryDepartment", oldPostponeRes.MilitaryDepartment != null ? oldPostponeRes.MilitaryDepartment.MilitaryDepartmentName : "", postponeRes.MilitaryDepartment != null ? postponeRes.MilitaryDepartment.MilitaryDepartmentName : "", currentUser));
                }

                SQL += @"END;";               

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPostponeResID = new OracleParameter();
                paramPostponeResID.ParameterName = "PostponeResID";
                paramPostponeResID.OracleType = OracleType.Number;

                if (postponeRes.PostponeResID != 0)
                {
                    paramPostponeResID.Direction = ParameterDirection.Input;
                    paramPostponeResID.Value = postponeRes.PostponeResID;
                }
                else
                {
                    paramPostponeResID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPostponeResID);

                OracleParameter param = null;               

               
                param = new OracleParameter();
                param.ParameterName = "PostponeYear";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeRes.PostponeYear.HasValue)
                    param.Value = postponeRes.PostponeYear.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeRes.MilitaryDepartmentID.HasValue)
                    param.Value = postponeRes.MilitaryDepartmentID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (postponeRes.PostponeResID == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (postponeRes.PostponeResID == 0)
                    postponeRes.PostponeResID = DBCommon.GetInt(paramPostponeResID.Value);

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

        public static void CopyPostponeRes(PostponeRes postponeRes_Old, PostponeRes postponeRes_New, User currentUser, Change changeEntry)
        {            
            string logDescription = "";
            logDescription += "Първоначална година: " + postponeRes_Old.PostponeYear.Value.ToString();
            logDescription += "; Първоначално ВО: " + postponeRes_Old.MilitaryDepartment.MilitaryDepartmentName;
            logDescription += "; За година: " + postponeRes_New.PostponeYear.ToString();
            logDescription += "; ВО: " + postponeRes_New.MilitaryDepartment.MilitaryDepartmentName;

            ChangeEvent changeEvent = changeEvent = new ChangeEvent("RES_PostponeRes_CopyPostpone", logDescription, null, null, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL += @"DECLARE
                            CURSOR Old_PostponeResCompanies IS
                            SELECT PostponeResCompanyID
                            FROM PMIS_RES.PostponeResCompanies
                            WHERE PostponeResID = :OldPostponeResID;
                         BEGIN
                            DELETE FROM PMIS_RES.PostponeResItems 
                            WHERE PostponeResCompanyID IN (SELECT PostponeResCompanyID FROM PMIS_RES.PostponeResCompanies
                                                           WHERE PostponeResID = :NewPostponeResID);

                            DELETE FROM PMIS_RES.PostponeResCompanies
                            WHERE PostponeResID = :NewPostponeResID;

                            FOR old_p IN Old_PostponeResCompanies LOOP
                               INSERT INTO PMIS_RES.PostponeResCompanies (PostponeResID, CompanyID, EmployeesCnt)
                               SELECT :NewPostponeResID as PostponeResID,
                                      a.CompanyID,
                                      a.EmployeesCnt
                               FROM PMIS_RES.PostponeResCompanies a
                               WHERE a.PostponeResCompanyID = old_p.PostponeResCompanyID;

                               INSERT INTO PMIS_RES.PostponeResItems (PostponeResCompanyID,
                                       NKPDID,
                                       OfficersConditioned,
                                       OfficersAbsolutely,
                                       OfCandConditioned,
                                       OfCandAbsolutely,
                                       SergeantsConditioned,
                                       SergeantsAbsolutely,
                                       SoldiersConditioned,
                                       SoldiersAbsolutely)
                               SELECT PMIS_RES.PostponeResCompanies_ID_SEQ.currval,
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
                               WHERE a.PostponeResCompanyID = old_p.PostponeResCompanyID;
                            END LOOP;
                         END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramNewPostponeResID = new OracleParameter();
                paramNewPostponeResID.ParameterName = "NewPostponeResID";
                paramNewPostponeResID.OracleType = OracleType.Number;
                paramNewPostponeResID.Direction = ParameterDirection.Input;
                paramNewPostponeResID.Value = postponeRes_New.PostponeResID;
                cmd.Parameters.Add(paramNewPostponeResID);

                OracleParameter paramOldPostponeResID = new OracleParameter();
                paramOldPostponeResID.ParameterName = "OldPostponeResID";
                paramOldPostponeResID.OracleType = OracleType.Number;
                paramOldPostponeResID.Direction = ParameterDirection.Input;
                paramOldPostponeResID.Value = postponeRes_Old.PostponeResID;
                cmd.Parameters.Add(paramOldPostponeResID);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null)
                changeEntry.AddEvent(changeEvent);
        }

        //When change any child record then update the last modified of postpone (the parent object)
        public static void SetPostponeResModified(int postponeResId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_RES.PostponeRes SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE PostponeResID = :PostponeResID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeResID", OracleType.Number).Value = postponeResId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        //Get all PostponeRes years
        public static List<int> GetAllPostponeResYears(User currentUser)
        {
            List<int> postponeYears = new List<int>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT PostponeYear
                               FROM PMIS_RES.PostponeRes ORDER BY PostponeYear";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    postponeYears.Add(DBCommon.GetInt(dr["PostponeYear"]));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeYears;
        }

        public static List<int> GetAllPostponeResFulfilYears(User currentUser)
        {
            List<int> fulfilYears = new List<int>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT Postpone_Year as PostponeYear
                               FROM PMIS_RES.ReservistMilRepStatuses
                               WHERE IsCurrent = 1 AND Postpone_Year IS NOT NULL
                               ORDER BY Postpone_Year";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    fulfilYears.Add(DBCommon.GetInt(dr["PostponeYear"]));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fulfilYears;
        }
    }
}