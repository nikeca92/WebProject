using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;

using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class PostponeTechCompany : BaseDbObject
    {
        public int PostponeTechCompanyID { get; set; }
        public int CompanyID { get; set; }

        private Company company;
        public Company Company
        {
            get 
            {
                if (company == null)
                    company = CompanyUtil.GetCompany(CompanyID, CurrentUser);

                return company; 
            }
            set { company = value; }
        }

        private List<PostponeTechItem> items;
        public List<PostponeTechItem> Items
        {
            get
            {
                if (items == null)
                    items = PostponeTechItemUtil.GetAllPostponeTechItemsByPostponeTechCompanyID(PostponeTechCompanyID, CurrentUser);

                return items;
            }
        }

        public int PostponeConditioned { get { return Items.Sum(x => x.PostponeConditioned.HasValue ? x.PostponeConditioned.Value : 0); } }
        public int PostponeAbsolutely { get { return Items.Sum(x => x.PostponeAbsolutely.HasValue ? x.PostponeAbsolutely.Value : 0); } }

        public PostponeTechCompany(User user)
            :base(user)
        {

        }
    }

    public static class PostponeTechCompanyUtil
    {
        public static PostponeTechCompany ExtractPostponeTechCompanyFromDataReader(OracleDataReader dr, User currentUser)
        {
            PostponeTechCompany postponeTechCompany = new PostponeTechCompany(currentUser);

            postponeTechCompany.PostponeTechCompanyID = DBCommon.GetInt(dr["PostponeTechCompanyID"]);
            postponeTechCompany.CompanyID = DBCommon.GetInt(dr["CompanyID"]);

            return postponeTechCompany;
        }

        public static PostponeTechCompany GetPostponeTechCompany(int postponeYear, int militaryDepartmentId, int companyId, User currentUser)
        {
            PostponeTechCompany postponeTechCompany = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.PostponeTechCompanyID,
                                       a.CompanyID
                                FROM PMIS_RES.PostponeTechCompanies a
                                INNER JOIN PMIS_RES.PostponeTech b ON a.PostponeTechID = b.PostponeTechID
                                WHERE a.CompanyID = :CompanyID AND
                                      b.PostponeYear = :PostponeYear AND
                                      b.MilitaryDepartmentID = :MilitaryDepartmentID
                                ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CompanyID", OracleType.Number).Value = companyId;
                cmd.Parameters.Add("PostponeYear", OracleType.Number).Value = postponeYear;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeTechCompany = ExtractPostponeTechCompanyFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeTechCompany;
        }

        public static PostponeTechCompany GetPostponeTechCompany(int postponeTechCompanyID, User currentUser)
        {
            PostponeTechCompany postponeTechCompany = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.PostponeTechCompanyID,
                                       a.CompanyID
                                FROM PMIS_RES.PostponeTechCompanies a
                                WHERE a.PostponeTechCompanyID = :PostponeTechCompanyID
                                ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeTechCompanyID", OracleType.Number).Value = postponeTechCompanyID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeTechCompany = ExtractPostponeTechCompanyFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeTechCompany;
        }

        public static List<PostponeTechCompany> GetPostponeTechCompanies(int postponeYear, int militaryDepartmentId, int? orderBy, User currentUser)
        {
            List<PostponeTechCompany> postponeTechCompanies = new List<PostponeTechCompany>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.PostponeTechCompanyID,
                                       a.CompanyID
                                FROM PMIS_RES.PostponeTechCompanies a
                                INNER JOIN PMIS_RES.PostponeTech b ON a.PostponeTechID = b.PostponeTechID
                                WHERE b.PostponeYear = :PostponeYear AND
                                      b.MilitaryDepartmentID = :MilitaryDepartmentID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeYear", OracleType.Number).Value = postponeYear;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    postponeTechCompanies.Add(ExtractPostponeTechCompanyFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            bool isAscending = true;

            if (orderBy > 100)
            {
                orderBy -= 100;
                isAscending = false;
            }

            if(isAscending)
            {
                switch (orderBy)
                {
                    case 1:
                        {
                            postponeTechCompanies = postponeTechCompanies.OrderBy(x => x.Company.CompanyName).ToList();
                            break;
                        }
                    case 2:
                        {
                            postponeTechCompanies = postponeTechCompanies.OrderBy(x => x.PostponeAbsolutely).ToList();
                            break;
                        }
                    case 3:
                        {
                            postponeTechCompanies = postponeTechCompanies.OrderBy(x => x.PostponeConditioned).ToList();
                            break;
                        }
                }
            }
            else
            {
                switch (orderBy)
                {
                    case 1:
                        {
                            postponeTechCompanies = postponeTechCompanies.OrderByDescending(x => x.Company.CompanyName).ToList();
                            break;
                        }
                    case 2:
                        {
                            postponeTechCompanies = postponeTechCompanies.OrderByDescending(x => x.PostponeAbsolutely).ToList();
                            break;
                        }
                    case 3:
                        {
                            postponeTechCompanies = postponeTechCompanies.OrderByDescending(x => x.PostponeConditioned).ToList();
                            break;
                        }
                }
            }

            return postponeTechCompanies;
        }

        //Save a position for a particular postpone
        public static bool SavePostponeTechCompany(int postponeTechID, PostponeTechCompany postponeTechCompany, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            PostponeTech postponeTech = PostponeTechUtil.GetPostponeTech(postponeTechID, currentUser);

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
                if (postponeTechCompany.PostponeTechCompanyID == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.PostponeTechCompanies (PostponeTechID,
                                                                         CompanyID)
                                                                VALUES (:PostponeTechID,
                                                                        :CompanyID);

                            SELECT PMIS_RES.PostponeTechCompanies_ID_SEQ.currval INTO :PostponeTechCompanyID FROM dual;                         
                            ";

                    changeEvent = new ChangeEvent("RES_PostponeTech_AddPostponeTechCompany", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechCompany_Company", "", postponeTechCompany.Company != null ? postponeTechCompany.Company.CompanyName : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.PostponeTechCompanies SET
                                PostponeTechID = :PostponeTechID,
                                CompanyID = :CompanyID
                             WHERE PostponeTechCompanyID = :PostponeTechCompanyID;
                            ";

                    changeEvent = new ChangeEvent("RES_PostponeTech_EditPostponeTechCompany", logDescription, null, null, currentUser);

                    PostponeTechCompany oldPostponeTechCompany = PostponeTechCompanyUtil.GetPostponeTechCompany(postponeTechCompany.PostponeTechCompanyID, currentUser);

                    if (oldPostponeTechCompany.CompanyID != postponeTechCompany.CompanyID)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechCompany_Company", oldPostponeTechCompany.Company != null ? oldPostponeTechCompany.Company.CompanyName : "", postponeTechCompany.Company != null ? postponeTechCompany.Company.CompanyName : "", currentUser));
                }                         

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPostponeTechCompanyID = new OracleParameter();
                paramPostponeTechCompanyID.ParameterName = "PostponeTechCompanyID";
                paramPostponeTechCompanyID.OracleType = OracleType.Number;

                if (postponeTechCompany.PostponeTechCompanyID != 0)
                {
                    paramPostponeTechCompanyID.Direction = ParameterDirection.Input;
                    paramPostponeTechCompanyID.Value = postponeTechCompany.PostponeTechCompanyID;
                }
                else
                {
                    paramPostponeTechCompanyID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPostponeTechCompanyID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PostponeTechID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = postponeTechID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CompanyID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = postponeTechCompany.CompanyID;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (postponeTechCompany.PostponeTechCompanyID == 0)
                    postponeTechCompany.PostponeTechCompanyID = DBCommon.GetInt(paramPostponeTechCompanyID.Value);

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    PostponeTechUtil.SetPostponeTechModified(postponeTechID, currentUser);

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

        //Delete a particualr position from a particular postpone
        public static void DeletePostponeTechCompany(int postponeTechID, int postponeTechCompanyID, User currentUser, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            PostponeTech postponeTech = PostponeTechUtil.GetPostponeTech(postponeTechID, currentUser);
            PostponeTechCompany postponeTechCompany = PostponeTechCompanyUtil.GetPostponeTechCompany(postponeTechCompanyID, currentUser);

            string logDescription = "";
            logDescription += "Година: " + postponeTech.PostponeYear;
            logDescription += "; ВО: " + postponeTech.MilitaryDepartment.MilitaryDepartmentName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL += @"BEGIN
                            DELETE FROM PMIS_RES.PostponeTechItems
                            WHERE PostponeTechCompanyID = :PostponeTechCompanyID;

                            DELETE FROM PMIS_RES.PostponeTechCompanies
                            WHERE PostponeTechCompanyID = :PostponeTechCompanyID;
                         END;";

                changeEvent = new ChangeEvent("RES_PostponeTech_DeletePostponeTechCompany", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeTechCompany_Company", postponeTechCompany.Company != null ? postponeTechCompany.Company.CompanyName : "", "", currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeTechCompanyID", OracleType.Number).Value = postponeTechCompanyID;

                cmd.ExecuteNonQuery();

                PostponeTechUtil.SetPostponeTechModified(postponeTechID, currentUser);
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }
    }
}