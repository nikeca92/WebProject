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
    public class PostponeResCompany : BaseDbObject
    {
        public int PostponeResCompanyID { get; set; }
        public int CompanyID { get; set; }
        public int? EmployeesCnt { get; set; }

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

        private List<PostponeResItem> items;
        public List<PostponeResItem> Items
        {
            get
            {
                if (items == null)
                    items = PostponeResItemUtil.GetAllPostponeResItemsByPostponeResCompanyID(PostponeResCompanyID, CurrentUser);

                return items;
            }
        }

        public int OfficersConditioned { get { return Items.Sum(x => x.OfficersConditioned.HasValue ? x.OfficersConditioned.Value : 0); } }
        public int OfficersAbsolutely { get { return Items.Sum(x => x.OfficersAbsolutely.HasValue ? x.OfficersAbsolutely.Value : 0); } }
        public int OfCandConditioned { get { return Items.Sum(x => x.OfCandConditioned.HasValue ? x.OfCandConditioned.Value : 0); } }
        public int OfCandAbsolutely { get { return Items.Sum(x => x.OfCandAbsolutely.HasValue ? x.OfCandAbsolutely.Value : 0); } }
        public int SergeantsConditioned { get { return Items.Sum(x => x.SergeantsConditioned.HasValue ? x.SergeantsConditioned.Value : 0); } }
        public int SergeantsAbsolutely { get { return Items.Sum(x => x.SergeantsAbsolutely.HasValue ? x.SergeantsAbsolutely.Value : 0); } }
        public int SoldiersConditioned { get { return Items.Sum(x => x.SoldiersConditioned.HasValue ? x.SoldiersConditioned.Value : 0); } }
        public int SoldiersAbsolutely { get { return Items.Sum(x => x.SoldiersAbsolutely.HasValue ? x.SoldiersAbsolutely.Value : 0); } }

        public int TotalConditioned { get { return OfficersConditioned + OfCandConditioned + SergeantsConditioned + SoldiersConditioned; } }
        public int TotalAbsolutely { get { return OfficersAbsolutely + OfCandAbsolutely + SergeantsAbsolutely + SoldiersAbsolutely; } }

        public PostponeResCompany(User user)
            :base(user)
        {

        }
    }

    public static class PostponeResCompanyUtil
    {
        public static PostponeResCompany ExtractPostponeResCompanyFromDataReader(OracleDataReader dr, User currentUser)
        {
            PostponeResCompany postponeResCompany = new PostponeResCompany(currentUser);

            postponeResCompany.PostponeResCompanyID = DBCommon.GetInt(dr["PostponeResCompanyID"]);
            postponeResCompany.CompanyID = DBCommon.GetInt(dr["CompanyID"]);
            postponeResCompany.EmployeesCnt = DBCommon.IsInt(dr["EmployeesCnt"]) ? (int?)DBCommon.GetInt(dr["EmployeesCnt"]) : null;

            return postponeResCompany;
        }

        public static PostponeResCompany GetPostponeResCompany(int postponeYear, int militaryDepartmentId, int companyId, User currentUser)
        {
            PostponeResCompany postponeResCompany = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.PostponeResCompanyID,
                                       a.CompanyID,
                                       a.EmployeesCnt
                                FROM PMIS_RES.PostponeResCompanies a
                                INNER JOIN PMIS_RES.PostponeRes b ON a.PostponeResID = b.PostponeResID
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
                    postponeResCompany = ExtractPostponeResCompanyFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeResCompany;
        }

        public static PostponeResCompany GetPostponeResCompany(int postponeResCompanyID, User currentUser)
        {
            PostponeResCompany postponeResCompany = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.PostponeResCompanyID,
                                       a.CompanyID,
                                       a.EmployeesCnt
                                FROM PMIS_RES.PostponeResCompanies a
                                WHERE a.PostponeResCompanyID = :PostponeResCompanyID
                                ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeResCompanyID", OracleType.Number).Value = postponeResCompanyID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeResCompany = ExtractPostponeResCompanyFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeResCompany;
        }

        public static List<PostponeResCompany> GetPostponeResCompanies(int postponeYear, int militaryDepartmentId, int? orderBy, User currentUser)
        {
            List<PostponeResCompany> postponeResCompanies = new List<PostponeResCompany>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.PostponeResCompanyID,
                                       a.CompanyID,
                                       a.EmployeesCnt                                       
                                FROM PMIS_RES.PostponeResCompanies a
                                INNER JOIN PMIS_RES.PostponeRes b ON a.PostponeResID = b.PostponeResID
                                WHERE b.PostponeYear = :PostponeYear AND
                                      b.MilitaryDepartmentID = :MilitaryDepartmentID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeYear", OracleType.Number).Value = postponeYear;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    postponeResCompanies.Add(ExtractPostponeResCompanyFromDataReader(dr, currentUser));
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
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.Company.CompanyName).ToList();
                            break;
                        }
                    case 2:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.EmployeesCnt).ToList();
                            break;
                        }
                    case 3:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.TotalAbsolutely).ToList();
                            break;
                        }
                    case 4:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.TotalConditioned).ToList();
                            break;
                        }
                    case 5:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.OfficersAbsolutely).ToList();
                            break;
                        }
                    case 6:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.OfficersConditioned).ToList();
                            break;
                        }
                    case 7:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.OfCandAbsolutely).ToList();
                            break;
                        }
                    case 8:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.OfCandConditioned).ToList();
                            break;
                        }
                    case 9:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.SergeantsAbsolutely).ToList();
                            break;
                        }
                    case 10:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.SergeantsConditioned).ToList();
                            break;
                        }
                    case 11:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.SoldiersAbsolutely).ToList();
                            break;
                        }
                    case 12:
                        {
                            postponeResCompanies = postponeResCompanies.OrderBy(x => x.SoldiersConditioned).ToList();
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
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.Company.CompanyName).ToList();
                            break;
                        }
                    case 2:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.EmployeesCnt).ToList();
                            break;
                        }
                    case 3:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.TotalAbsolutely).ToList();
                            break;
                        }
                    case 4:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.TotalConditioned).ToList();
                            break;
                        }
                    case 5:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.OfficersAbsolutely).ToList();
                            break;
                        }
                    case 6:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.OfficersConditioned).ToList();
                            break;
                        }
                    case 7:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.OfCandAbsolutely).ToList();
                            break;
                        }
                    case 8:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.OfCandConditioned).ToList();
                            break;
                        }
                    case 9:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.SergeantsAbsolutely).ToList();
                            break;
                        }
                    case 10:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.SergeantsConditioned).ToList();
                            break;
                        }
                    case 11:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.SoldiersAbsolutely).ToList();
                            break;
                        }
                    case 12:
                        {
                            postponeResCompanies = postponeResCompanies.OrderByDescending(x => x.SoldiersConditioned).ToList();
                            break;
                        }
                }
            }

            return postponeResCompanies;
        }

        //Save a position for a particular postpone
        public static bool SavePostponeResCompany(int postponeResID, PostponeResCompany postponeResCompany, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            PostponeRes postponeRes = PostponeResUtil.GetPostponeRes(postponeResID, currentUser);

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
                if (postponeResCompany.PostponeResCompanyID == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.PostponeResCompanies (PostponeResID,
                                                                        CompanyID,
                                                                        EmployeesCnt)
                                                                VALUES (:PostponeResID,
                                                                        :CompanyID,
                                                                        :EmployeesCnt);  

                            SELECT PMIS_RES.PostponeResCompanies_ID_SEQ.currval INTO :PostponeResCompanyID FROM dual;                         
                            ";

                    changeEvent = new ChangeEvent("RES_PostponeRes_AddPostponeResCompany", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResCompany_Company", "", postponeResCompany.Company != null ? postponeResCompany.Company.CompanyName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResCompany_EmployeesCnt", "", (postponeResCompany.EmployeesCnt.HasValue ? postponeResCompany.EmployeesCnt.Value.ToString() : ""), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.PostponeResCompanies SET
                                PostponeResID = :PostponeResID,
                                CompanyID = :CompanyID,
                                EmployeesCnt = :EmployeesCnt
                             WHERE PostponeResCompanyID = :PostponeResCompanyID;

                            ";

                    changeEvent = new ChangeEvent("RES_PostponeRes_EditPostponeResCompany", logDescription, null, null, currentUser);

                    PostponeResCompany oldPostponeResCompany = PostponeResCompanyUtil.GetPostponeResCompany(postponeResCompany.PostponeResCompanyID, currentUser);

                    if (oldPostponeResCompany.CompanyID != postponeResCompany.CompanyID)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResCompany_Company", oldPostponeResCompany.Company != null ? oldPostponeResCompany.Company.CompanyName : "", postponeResCompany.Company != null ? postponeResCompany.Company.CompanyName : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldPostponeResCompany.EmployeesCnt, postponeResCompany.EmployeesCnt))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResCompany_EmployeesCnt", (oldPostponeResCompany.EmployeesCnt.HasValue ? oldPostponeResCompany.EmployeesCnt.Value.ToString() : ""), (postponeResCompany.EmployeesCnt.HasValue ? postponeResCompany.EmployeesCnt.Value.ToString() : ""), currentUser));
                }                         

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPostponeResCompanyID = new OracleParameter();
                paramPostponeResCompanyID.ParameterName = "PostponeResCompanyID";
                paramPostponeResCompanyID.OracleType = OracleType.Number;

                if (postponeResCompany.PostponeResCompanyID != 0)
                {
                    paramPostponeResCompanyID.Direction = ParameterDirection.Input;
                    paramPostponeResCompanyID.Value = postponeResCompany.PostponeResCompanyID;
                }
                else
                {
                    paramPostponeResCompanyID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPostponeResCompanyID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PostponeResID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = postponeResID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CompanyID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = postponeResCompany.CompanyID;
                cmd.Parameters.Add(param);
                
                param = new OracleParameter();
                param.ParameterName = "EmployeesCnt";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (postponeResCompany.EmployeesCnt.HasValue)
                    param.Value = postponeResCompany.EmployeesCnt.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (postponeResCompany.PostponeResCompanyID == 0)
                    postponeResCompany.PostponeResCompanyID = DBCommon.GetInt(paramPostponeResCompanyID.Value);

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    PostponeResUtil.SetPostponeResModified(postponeResID, currentUser);

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
        public static void DeletePostponeResCompany(int postponeResID, int postponeResCompanyID, User currentUser, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            PostponeRes postponeRes = PostponeResUtil.GetPostponeRes(postponeResID, currentUser);
            PostponeResCompany postponeResCompany = PostponeResCompanyUtil.GetPostponeResCompany(postponeResCompanyID, currentUser);

            string logDescription = "";
            logDescription += "Година: " + postponeRes.PostponeYear;
            logDescription += "; ВО: " + postponeRes.MilitaryDepartment.MilitaryDepartmentName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL += @"BEGIN
                            DELETE FROM PMIS_RES.PostponeResItems
                            WHERE PostponeResCompanyID = :PostponeResCompanyID;

                            DELETE FROM PMIS_RES.PostponeResCompanies
                            WHERE PostponeResCompanyID = :PostponeResCompanyID;
                         END;";

                changeEvent = new ChangeEvent("RES_PostponeRes_DeletePostponeResCompany", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResCompany_Company", postponeResCompany.Company != null ? postponeResCompany.Company.CompanyName : "", "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_PostponeResCompany_EmployeesCnt", (postponeResCompany.EmployeesCnt.HasValue ? postponeResCompany.EmployeesCnt.Value.ToString() : ""), "", currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeResCompanyID", OracleType.Number).Value = postponeResCompanyID;

                cmd.ExecuteNonQuery();

                PostponeResUtil.SetPostponeResModified(postponeResID, currentUser);
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