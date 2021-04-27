using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular PostponeTech into the system
    public class PostponeTech : BaseDbObject
    {
        private int postponeTechID;        
        private int? postponeYear;        
        private int? militaryDepartmentID;        
        private MilitaryDepartment militaryDepartment;        
        private List<PostponeTechCompany> postponeTechCompanies;

        public int PostponeTechID
        {
            get { return postponeTechID; }
            set { postponeTechID = value; }
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

        public List<PostponeTechCompany> PostponeTechCompanies
        {
            get 
            {
                if (postponeTechCompanies == null)
                    postponeTechCompanies = PostponeTechCompanyUtil.GetPostponeTechCompanies(
                        PostponeYear.HasValue ? PostponeYear.Value : 0, 
                        MilitaryDepartmentID.HasValue ? MilitaryDepartmentID.Value : 0, 
                        null, 
                        CurrentUser);

                return postponeTechCompanies; 
            }
            set { postponeTechCompanies = value; }
        }

        public bool CanDelete
        {
            get 
            { 
                return PostponeTechCompanies.Count == 0; 
            }

        }

        public PostponeTech(User user)
            : base(user)
        {

        }
    }

    public static class PostponeTechUtil
    {
        public static PostponeTech ExtractPostponeTech(OracleDataReader dr, User currentUser)
        {
            PostponeTech postponeTech = new PostponeTech(currentUser);

            postponeTech.PostponeTechID = DBCommon.GetInt(dr["PostponeTechID"]);
            postponeTech.PostponeYear = (DBCommon.IsInt(dr["PostponeYear"]) ? DBCommon.GetInt(dr["PostponeYear"]) : (int?)null);
            postponeTech.MilitaryDepartmentID = (DBCommon.IsInt(dr["MilitaryDepartmentID"]) ? DBCommon.GetInt(dr["MilitaryDepartmentID"]) : (int?)null);

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, postponeTech);

            return postponeTech;
        }

        //Get a particular object by its ID
        public static PostponeTech GetPostponeTech(int postponeTechID, User currentUser)
        {
            PostponeTech postponeTech = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_POSTPONE_TECH", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.PostponeTechID,
                                      a.PostponeYear, a.MilitaryDepartmentID,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_RES.PostponeTech a                               
                               WHERE a.PostponeTechID = :PostponeTechID 
                               " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeTechID", OracleType.Number).Value = postponeTechID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeTech = ExtractPostponeTech(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeTech;
        }

        public static PostponeTech GetPostponeTech(int postponeYear, int militaryDepartmentId, User currentUser)
        {
            PostponeTech postponeTech = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_POSTPONE_TECH", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere += " AND b.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.PostponeTechID,
                                      a.PostponeYear, a.MilitaryDepartmentID,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_RES.PostponeTech a                               
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
                    postponeTech = ExtractPostponeTech(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeTech;
        }

        public static bool IsThereAnyCompanyData(int militaryDepartmentID, int postponeYear, User currentUser)
        {
            bool result = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.PostponeTech a
                               INNER JOIN PMIS_RES.PostponeTechCompanies b ON a.PostponeTechID = b.PostponeTechID
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
        public static bool SavePostponeTech(PostponeTech postponeTech, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            string logDescription = "";
            logDescription += "Година: " + postponeTech.PostponeYear;
            logDescription += "; ВО: " + postponeTech.MilitaryDepartment.MilitaryDepartmentName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (postponeTech.PostponeTechID == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.PostponeTech (PostponeYear, MilitaryDepartmentID,
                                                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:PostponeYear, :MilitaryDepartmentID,
                                    :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_RES.PostponeTech_ID_SEQ.currval INTO :PostponeTechID FROM dual;
                            ";

                    changeEvent = new ChangeEvent("RES_PostponeTech_AddPostponeTech", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTech_PostponeYear", "", postponeTech.PostponeYear.HasValue ? postponeTech.PostponeYear.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTech_MilitaryDepartment", "", postponeTech.MilitaryDepartment != null && postponeTech.MilitaryDepartment.MilitaryDepartmentName != null ? postponeTech.MilitaryDepartment.MilitaryDepartmentName : "", currentUser));                
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.PostponeTech SET                              
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
                             WHERE PostponeTechID = :PostponeTechID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_PostponeTech_EditPostponeTech", logDescription, null, null, currentUser);

                    PostponeTech oldPostponeTech = GetPostponeTech(postponeTech.PostponeTechID, currentUser);

                    if (!CommonFunctions.IsEqualInt(oldPostponeTech.PostponeYear, postponeTech.PostponeYear))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTech_PostponeYear", oldPostponeTech.PostponeYear.HasValue ? oldPostponeTech.PostponeYear.Value.ToString() : "", postponeTech.PostponeYear.HasValue ? postponeTech.PostponeYear.Value.ToString() : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeTech.MilitaryDepartmentID, postponeTech.MilitaryDepartmentID))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Postpone_MilitaryDepartment", oldPostponeTech.MilitaryDepartment != null ? oldPostponeTech.MilitaryDepartment.MilitaryDepartmentName : "", postponeTech.MilitaryDepartment != null ? postponeTech.MilitaryDepartment.MilitaryDepartmentName : "", currentUser));
                }

                SQL += @"END;";               

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPostponeTechID = new OracleParameter();
                paramPostponeTechID.ParameterName = "PostponeTechID";
                paramPostponeTechID.OracleType = OracleType.Number;

                if (postponeTech.PostponeTechID != 0)
                {
                    paramPostponeTechID.Direction = ParameterDirection.Input;
                    paramPostponeTechID.Value = postponeTech.PostponeTechID;
                }
                else
                {
                    paramPostponeTechID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPostponeTechID);

                OracleParameter param = null;               

               
                param = new OracleParameter();
                param.ParameterName = "PostponeYear";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeTech.PostponeYear.HasValue)
                    param.Value = postponeTech.PostponeYear.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeTech.MilitaryDepartmentID.HasValue)
                    param.Value = postponeTech.MilitaryDepartmentID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (postponeTech.PostponeTechID == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (postponeTech.PostponeTechID == 0)
                    postponeTech.PostponeTechID = DBCommon.GetInt(paramPostponeTechID.Value);

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

        public static void CopyPostponeTech(PostponeTech postponeTech_Old, PostponeTech postponeTech_New, User currentUser, Change changeEntry)
        {            
            string logDescription = "";
            logDescription += "Първоначална година: " + postponeTech_Old.PostponeYear.Value.ToString();
            logDescription += "; Първоначално ВО: " + postponeTech_Old.MilitaryDepartment.MilitaryDepartmentName;
            logDescription += "; За година: " + postponeTech_New.PostponeYear.ToString();
            logDescription += "; ВО: " + postponeTech_New.MilitaryDepartment.MilitaryDepartmentName;

            ChangeEvent changeEvent = changeEvent = new ChangeEvent("RES_PostponeTech_CopyPostpone", logDescription, null, null, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL += @"DECLARE
                            CURSOR Old_PostponeTechCompanies IS
                            SELECT PostponeTechCompanyID
                            FROM PMIS_RES.PostponeTechCompanies
                            WHERE PostponeTechID = :OldPostponeTechID;
                         BEGIN
                            DELETE FROM PMIS_RES.PostponeTechItems 
                            WHERE PostponeTechCompanyID IN (SELECT PostponeTechCompanyID FROM PMIS_RES.PostponeTechCompanies
                                                            WHERE PostponeTechID = :NewPostponeTechID);

                            DELETE FROM PMIS_RES.PostponeTechCompanies
                            WHERE PostponeTechID = :NewPostponeTechID;

                            FOR old_p IN Old_PostponeTechCompanies LOOP
                               INSERT INTO PMIS_RES.PostponeTechCompanies (PostponeTechID, CompanyID)
                               SELECT :NewPostponeTechID as PostponeTechID,
                                      a.CompanyID
                               FROM PMIS_RES.PostponeTechCompanies a
                               WHERE a.PostponeTechCompanyID = old_p.PostponeTechCompanyID;

                               INSERT INTO PMIS_RES.PostponeTechItems (PostponeTechCompanyID,
                                       TechnicsSubTypeID,
                                       PostponeConditioned,
                                       PostponeAbsolutely)
                               SELECT PMIS_RES.PostponeTechCompanies_ID_SEQ.currval,
                                      a.TechnicsSubTypeID,
                                      a.PostponeConditioned,
                                      a.PostponeAbsolutely
                               FROM PMIS_RES.PostponeTechItems a
                               WHERE a.PostponeTechCompanyID = old_p.PostponeTechCompanyID;
                            END LOOP;
                         END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramNewPostponeTechID = new OracleParameter();
                paramNewPostponeTechID.ParameterName = "NewPostponeTechID";
                paramNewPostponeTechID.OracleType = OracleType.Number;
                paramNewPostponeTechID.Direction = ParameterDirection.Input;
                paramNewPostponeTechID.Value = postponeTech_New.PostponeTechID;
                cmd.Parameters.Add(paramNewPostponeTechID);

                OracleParameter paramOldPostponeTechID = new OracleParameter();
                paramOldPostponeTechID.ParameterName = "OldPostponeTechID";
                paramOldPostponeTechID.OracleType = OracleType.Number;
                paramOldPostponeTechID.Direction = ParameterDirection.Input;
                paramOldPostponeTechID.Value = postponeTech_Old.PostponeTechID;
                cmd.Parameters.Add(paramOldPostponeTechID);

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
        public static void SetPostponeTechModified(int postponeTechId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_RES.PostponeTech SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE PostponeTechID = :PostponeTechID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeTechID", OracleType.Number).Value = postponeTechId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        //Get all PostponeTech years
        public static List<int> GetAllPostponeTechYears(User currentUser)
        {
            List<int> postponeYears = new List<int>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT PostponeYear
                               FROM PMIS_RES.PostponeTech ORDER BY PostponeYear";

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

        public static List<int> GetAllPostponeTechFulfilYears(User currentUser)
        {
            List<int> fulfilYears = new List<int>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT TechnicsPostpone_Year as PostponeYear
                               FROM PMIS_RES.TechnicsMilRepStatus
                               WHERE IsCurrent = 1 AND TechnicsPostpone_Year IS NOT NULL
                               ORDER BY TechnicsPostpone_Year";

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