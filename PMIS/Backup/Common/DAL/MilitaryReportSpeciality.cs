using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //This class represents the Military Report Speciality objects
    public class MilitaryReportSpeciality : BaseDbObject
    {
        private int milReportSpecialityId;
        private int milReportSpecialityTypeId;
        private MilitaryReportSpecialityType milReportSpecialityType;
        private string milReportingSpecialityName;
        private string milReportingSpecialityCode;
        private string codeAndName = null;
        private bool active;
        private int? militaryForceSortId;
        private MilitaryForceSort militaryForceSort;

        public int MilReportSpecialityId
        {
            get
            {
                return milReportSpecialityId;
            }
            set
            {
                milReportSpecialityId = value;
            }
        }

        public int MilReportSpecialityTypeId
        {
            get { return milReportSpecialityTypeId; }
            set { milReportSpecialityTypeId = value; }
        }

        public MilitaryReportSpecialityType MilReportSpecialityType
        {
            get 
            {
                if (milReportSpecialityType == null)
                    milReportSpecialityType = MilitaryReportSpecialityTypeUtil.GetMilitaryReportSpecialityType(milReportSpecialityTypeId, CurrentUser);

                return milReportSpecialityType; 
            }
            set { milReportSpecialityType = value; }
        }

        public string MilReportingSpecialityName
        {
            get
            {
                return milReportingSpecialityName;
            }
            set
            {
                milReportingSpecialityName = value;
            }
        }

        public string MilReportingSpecialityCode
        {
            get
            {
                return milReportingSpecialityCode;
            }
            set
            {
                milReportingSpecialityCode = value;
            }
        }

        public string CodeAndName
        {
            get
            {
                if (codeAndName == null)
                    codeAndName = MilReportingSpecialityCode + " " + milReportingSpecialityName;

                return codeAndName;
            }

            set
            {
                codeAndName = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                return MilitaryReportSpecialityUtil.CanDelete(MilReportSpecialityId, CurrentUser);
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public int? MilitaryForceSortId
        {
            get { return militaryForceSortId; }
            set { militaryForceSortId = value; }
        }

        public MilitaryForceSort MilitaryForceSort
        {
            get
            {
                if (militaryForceSort == null && MilitaryForceSortId.HasValue)
                    militaryForceSort = MilitaryForceSortUtil.GetMilitaryForceSort(MilitaryForceSortId.Value, CurrentUser);

                return militaryForceSort;
            }
            set { militaryForceSort = value; }
        }

        public MilitaryReportSpeciality(User user)
            : base(user)
        {
        }
    }

    public class MilRepSpecialitiesFilter
    {
        public string Types
        {
            get; set;
        }

        public string Code
        {
            get; set;
        }

        public string Name
        {
            get;
            set;
        }

        public int? Active
        {
            get;
            set;
        }

        public int OrderBy
        {
            get; set;
        }

        public int PageIdx
        {
            get; set;
        }
    }

    //Some utility methods that help working with MilitaryReportSpeciality objects
    public static class MilitaryReportSpecialityUtil
    {
        //Extract a particular MilitaryReportSpeciality object from a data reader
        public static MilitaryReportSpeciality ExtractMilitaryReportSpecialityFromDR(User currentUser, OracleDataReader dr)
        {
            MilitaryReportSpeciality militaryReportSpeciality = new MilitaryReportSpeciality(currentUser);

            militaryReportSpeciality.MilReportSpecialityId = DBCommon.GetInt(dr["MilReportSpecialityID"]);
            militaryReportSpeciality.MilReportSpecialityTypeId = DBCommon.IsInt(dr["Type"]) ? DBCommon.GetInt(dr["Type"]) : 0;
            militaryReportSpeciality.MilReportingSpecialityName = dr["MilReportingSpecialityName"].ToString();
            militaryReportSpeciality.MilReportingSpecialityCode = dr["MilReportingSpecialityCode"].ToString();
            militaryReportSpeciality.Active = DBCommon.GetInt(dr["MilReportingSpecialityActive"]) == 1;
            militaryReportSpeciality.MilitaryForceSortId = DBCommon.IsInt(dr["MilitaryForceSortID"]) ? DBCommon.GetInt(dr["MilitaryForceSortID"]) : (int?)null;

            return militaryReportSpeciality;
        }

        //Get a particualr MilitaryReportSpeciality object from the DB by its ID
        public static MilitaryReportSpeciality GetMilitaryReportSpeciality(int milReportSpecialityId, User currentUser)
        {
            MilitaryReportSpeciality militaryReportSpeciality = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilReportSpecialityID,
                                      a.Type,
                                      a.MilReportingSpecialityName,
                                      a.MilReportingSpecialityCode,
                                      a.Active as MilReportingSpecialityActive,
                                      a.MilitaryForceSortID
                               FROM PMIS_ADM.MilitaryReportSpecialities a
                               WHERE a.MilReportSpecialityID = :MilReportSpecialityId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilReportSpecialityId", OracleType.Number).Value = milReportSpecialityId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryReportSpeciality = ExtractMilitaryReportSpecialityFromDR(currentUser, dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportSpeciality;
        }

        //Get a particualr MilitaryReportSpeciality object from the DB by its Code
        public static MilitaryReportSpeciality GetMilitaryReportSpecialityByCode(string milReportingSpecialityCode, User currentUser)
        {
            MilitaryReportSpeciality militaryReportSpeciality = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilReportSpecialityID,
                                      a.Type,
                                      a.MilReportingSpecialityName,
                                      a.MilReportingSpecialityCode,
                                      a.Active as MilReportingSpecialityActive,
                                      a.MilitaryForceSortID
                               FROM PMIS_ADM.MilitaryReportSpecialities a
                               WHERE a.MilReportingSpecialityCode = :MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilReportingSpecialityCode", OracleType.VarChar).Value = milReportingSpecialityCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryReportSpeciality = ExtractMilitaryReportSpecialityFromDR(currentUser, dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportSpeciality;
        }

        //Get a list of all MilitaryReportSpeciality that are of a particular type
        public static List<MilitaryReportSpeciality> GetMilitaryReportSpecialitiesByType(User currentUser, int type)
        {
            List<MilitaryReportSpeciality> militaryReportSpecialities = new List<MilitaryReportSpeciality>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilReportSpecialityID,
                                      a.Type,
                                      a.MilReportingSpecialityName,
                                      a.MilReportingSpecialityCode,
                                      a.Active as MilReportingSpecialityActive,
                                      a.MilitaryForceSortID
                               FROM PMIS_ADM.MilitaryReportSpecialities a
                               WHERE (a.Type = :Type AND a.Active = 1) OR
                                     (:Type = -2 AND a.Active = 0)
                               ORDER BY a.MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("Type", OracleType.Number).Value = type;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryReportSpecialities.Add(ExtractMilitaryReportSpecialityFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportSpecialities;
        }

        //Get a list of all MilitaryReportSpeciality that are of a particular Military Force Sort
        public static List<MilitaryReportSpeciality> GetMilitaryReportSpecialitiesByMilitaryForceSort(User currentUser, int militaryForceSortId, bool onlyActiveMRS, bool onlyActiveMFS)
        {
            List<MilitaryReportSpeciality> militaryReportSpecialities = new List<MilitaryReportSpeciality>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilReportSpecialityID,
                                      a.Type,
                                      a.MilReportingSpecialityName,
                                      a.MilReportingSpecialityCode,
                                      a.Active as MilReportingSpecialityActive,
                                      a.MilitaryForceSortID
                               FROM PMIS_ADM.MilitaryReportSpecialities a
                               INNER JOIN VS_OWNER.KLV_ROD b ON a.MilitaryForceSortID = b.RODID ";

                string where = "";

                if (onlyActiveMRS)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.Active = 1 ";
                }

                if (onlyActiveMFS)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.Active = 1 ";
                }

                if (militaryForceSortId > 0)
                {
                     where += (where == "" ? "" : " AND ") +
                              " a.MilitaryForceSortID = :MilitaryForceSortID";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                SQL += where;

                SQL += @"
                        ORDER BY a.MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                if (militaryForceSortId > 0)
                {
                    cmd.Parameters.Add("MilitaryForceSortID", OracleType.Number).Value = militaryForceSortId;
                }

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryReportSpecialities.Add(ExtractMilitaryReportSpecialityFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportSpecialities;
        }

        //Get a list of all MilitaryReportSpecialities for the selected filter
        public static List<MilitaryReportSpeciality> GetMilitaryReportSpecialities(User currentUser, MilRepSpecialitiesFilter filter, int rowsPerPage)
        {
            List<MilitaryReportSpeciality> militaryReportSpecialities = new List<MilitaryReportSpeciality>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.Types))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.Type IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Types) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.Code))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilReportingSpecialityCode LIKE '%" + filter.Code.Replace("'", "''") + "%' ";
                }

                if (!String.IsNullOrEmpty(filter.Name))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilReportingSpecialityName LIKE '%" + filter.Name.Replace("'", "''") + "%' ";
                }

                if (filter.Active.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                        " a.Active = " + (filter.Active.Value == 1 ? "1" : "0");
                }

                //Paging (load the rows only for the target page)
                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                where = (where == "" ? "" : " WHERE ") + where;

                //Construct the ORDER BY clause according to the order column
                string orderBySQL = "";
                string orderByDir = "ASC";

                int orderBy = filter.OrderBy;

                //The DESCending order is specified by using column number + 100 (e.g. 101, 102, etc.)
                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                //Get the specific order by expression
                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "NVL(b.TypeName, '_')";
                        break;
                    case 2:
                        orderBySQL = "a.MilReportingSpecialityCode";
                        break;
                    case 3:
                        orderBySQL = "a.MilReportingSpecialityName";
                        break;
                    case 4:
                        orderBySQL = "a.Active";
                        break;
                    default:
                        orderBySQL = "NVL(b.TypeName, '_')";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir) + ", a.MilReportingSpecialityCode ASC, a.MilReportSpecialityID ASC" + DBCommon.FixNullsOrder("ASC");

                string SQL = @"SELECT * 
                               FROM ( SELECT a.MilReportSpecialityID,
                                             a.Type,
                                             a.MilReportingSpecialityName,
                                             a.MilReportingSpecialityCode,
                                             a.Active as MilReportingSpecialityActive,
                                             a.MilitaryForceSortID,
                                             RANK() OVER (ORDER BY " + orderBySQL + @") as RowNumber
                                      FROM PMIS_ADM.MilitaryReportSpecialities a
                                      LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialityTypes b ON a.Type = b.Type
                                      " + where + @"
                                      ORDER BY " + orderBySQL + @"
                                     ) tmp
                               " + pageWhere;


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryReportSpecialities.Add(ExtractMilitaryReportSpecialityFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportSpecialities;
        }

        //Return a list of all MilitaryReportSpecialities by their IDs
        public static List<MilitaryReportSpeciality> GetAllMilitaryReportSpecialitiesByIDs(User currentUser, string militaryReportSpecialityIds)
        {
            List<MilitaryReportSpeciality> militaryReportSpecialities = new List<MilitaryReportSpeciality>();

            if (String.IsNullOrEmpty(militaryReportSpecialityIds))
                militaryReportSpecialityIds = "-1";

            string clause = CommonFunctions.GetOracleSQLINClause("a.MilReportSpecialityID", militaryReportSpecialityIds);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string SQL = @"SELECT a.MilReportSpecialityID,
                                      a.Type,
                                      a.MilReportingSpecialityName,
                                      a.MilReportingSpecialityCode,
                                      a.Active as MilReportingSpecialityActive,
                                      a.MilitaryForceSortID
                               FROM PMIS_ADM.MilitaryReportSpecialities a
                               WHERE " + clause + @"
                               ORDER BY a.MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryReportSpecialities.Add(ExtractMilitaryReportSpecialityFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryReportSpecialities;
        }

        public static int GetMilitaryReportSpecialitiesCnt(MilRepSpecialitiesFilter filter, User currentUser)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.Types))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.Type IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Types) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.Code))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilReportingSpecialityCode LIKE '%" + filter.Code.Replace("'", "''") + "%' ";
                }

                if (!String.IsNullOrEmpty(filter.Name))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilReportingSpecialityName LIKE '%" + filter.Name.Replace("'", "''") + "%' ";
                }

                if (filter.Active.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                        " a.Active = " + (filter.Active.Value == 1 ? "1" : "0");
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT COUNT(*) as Cnt                                            
                                FROM PMIS_ADM.MilitaryReportSpecialities a                
                                " + where;


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cnt;
        }

        //Save a particular military report speciality.
        public static bool SaveMilReportSpeciality(User currentUser, MilitaryReportSpeciality militaryReportSpeciality, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //If a new record then INSERT it
                if (militaryReportSpeciality.MilReportSpecialityId == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.MilitaryReportSpecialities (MilReportingSpecialityName,  
                                MilReportingSpecialityCode, Type, MilitaryForceSortID, Active)
                             VALUES (:MilReportingSpecialityName,  
                                :MilReportingSpecialityCode, :Type, :MilitaryForceSortID, :Active);

                             SELECT PMIS_ADM.MilRepSpecialities_ID_SEQ.currval INTO :MilReportSpecialityID FROM dual;
                            ";

                    //Add this change to the Audit Trail
                    ChangeEvent changeEvent = new ChangeEvent("ADM_Lists_MilReportSpeciality_Add", "", null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Type", "", (militaryReportSpeciality.MilReportSpecialityType != null ? militaryReportSpeciality.MilReportSpecialityType.TypeName : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Code", "", militaryReportSpeciality.MilReportingSpecialityCode, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Name", "", militaryReportSpeciality.MilReportingSpecialityName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_MilForceSort", "", (militaryReportSpeciality.MilitaryForceSort != null ? militaryReportSpeciality.MilitaryForceSort.MilitaryForceSortName : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Active", "", (militaryReportSpeciality.Active ? "1" : "0"), currentUser));

                    changeEntry.AddEvent(changeEvent);
                }
                else //UPDATE an existing record
                {
                    SQL += @"UPDATE PMIS_ADM.MilitaryReportSpecialities SET
                               MilReportingSpecialityName = :MilReportingSpecialityName, 
                               MilReportingSpecialityCode = :MilReportingSpecialityCode,
                               Type = :Type,
                               MilitaryForceSortID = :MilitaryForceSortID,
                               Active = :Active
                             WHERE MilReportSpecialityID = :MilReportSpecialityID;
                            ";

                    //If there are any actual changes then track them to the Audit Trail log
                    ChangeEvent changeEvent = new ChangeEvent("ADM_Lists_MilReportSpeciality_Edit", "", null, null, currentUser);

                    MilitaryReportSpeciality oldMilitaryReportSpeciality = GetMilitaryReportSpeciality(militaryReportSpeciality.MilReportSpecialityId, currentUser);

                    if (!CommonFunctions.IsEqualInt(oldMilitaryReportSpeciality.MilReportSpecialityTypeId, militaryReportSpeciality.MilReportSpecialityTypeId))
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Type", (oldMilitaryReportSpeciality.MilReportSpecialityType != null ? oldMilitaryReportSpeciality.MilReportSpecialityType.TypeName : ""), (militaryReportSpeciality.MilReportSpecialityType != null ? militaryReportSpeciality.MilReportSpecialityType.TypeName : ""), currentUser));

                    if (oldMilitaryReportSpeciality.MilReportingSpecialityCode.Trim() != militaryReportSpeciality.MilReportingSpecialityCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Code", oldMilitaryReportSpeciality.MilReportingSpecialityCode, militaryReportSpeciality.MilReportingSpecialityCode, currentUser));

                    if (oldMilitaryReportSpeciality.MilReportingSpecialityName.Trim() != militaryReportSpeciality.MilReportingSpecialityName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Name", oldMilitaryReportSpeciality.MilReportingSpecialityName, militaryReportSpeciality.MilReportingSpecialityName, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldMilitaryReportSpeciality.MilitaryForceSortId, militaryReportSpeciality.MilitaryForceSortId))
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_MilForceSort", (oldMilitaryReportSpeciality.MilitaryForceSort != null ? oldMilitaryReportSpeciality.MilitaryForceSort.MilitaryForceSortName : ""), (militaryReportSpeciality.MilitaryForceSort != null ? militaryReportSpeciality.MilitaryForceSort.MilitaryForceSortName : ""), currentUser));

                    if (oldMilitaryReportSpeciality.Active != militaryReportSpeciality.Active)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Active", (oldMilitaryReportSpeciality.Active ? "1" : "0"), militaryReportSpeciality.Active ? "1" : "0", currentUser));

                    if (changeEvent.ChangeEventDetails.Count > 0)
                        changeEntry.AddEvent(changeEvent);
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                //Add the MilReportSpecialityID parameter. If it is a new record the pass it as an Output parameter to be ablet to return the new ID.
                OracleParameter paramMilReportSpecialityID = new OracleParameter();
                paramMilReportSpecialityID.ParameterName = "MilReportSpecialityID";
                paramMilReportSpecialityID.OracleType = OracleType.Number;

                if (militaryReportSpeciality.MilReportSpecialityId != 0)
                {
                    paramMilReportSpecialityID.Direction = ParameterDirection.Input;
                    paramMilReportSpecialityID.Value = militaryReportSpeciality.MilReportSpecialityId;
                }
                else
                {
                    paramMilReportSpecialityID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramMilReportSpecialityID);

                //Add the other parameters to the query
                OracleParameter param = new OracleParameter();
                param.ParameterName = "MilReportingSpecialityName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = militaryReportSpeciality.MilReportingSpecialityName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReportingSpecialityCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = militaryReportSpeciality.MilReportingSpecialityCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Type";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = militaryReportSpeciality.MilReportSpecialityTypeId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryForceSortID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (militaryReportSpeciality.MilitaryForceSortId.HasValue)
                    param.Value = militaryReportSpeciality.MilitaryForceSortId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Active";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = militaryReportSpeciality.Active ? 1 : 0;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (militaryReportSpeciality.MilReportSpecialityId == 0)
                    militaryReportSpeciality.MilReportSpecialityId = DBCommon.GetInt(paramMilReportSpecialityID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Delete a military report speciality
        public static bool DeleteMilReportSpeciality(User currentUser, int milReportSpecialityId, Change changeEntry)
        {
            bool result = false;

            MilitaryReportSpeciality oldMilReportSpeciality = GetMilitaryReportSpeciality(milReportSpecialityId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"BEGIN
                                  DELETE FROM PMIS_ADM.MilitaryReportSpecialities
                                  WHERE MilReportSpecialityID = :MilReportSpecialityID;
                               END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilReportSpecialityID", OracleType.Number).Value = milReportSpecialityId;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            //Save the operation to the changes log
            ChangeEvent changeEvent = new ChangeEvent("ADM_Lists_MilReportSpeciality_Delete", "", null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Type", (oldMilReportSpeciality.MilReportSpecialityType != null ? oldMilReportSpeciality.MilReportSpecialityType.TypeName : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Code", oldMilReportSpeciality.MilReportingSpecialityCode, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Name", oldMilReportSpeciality.MilReportingSpecialityName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_MilForceSort", (oldMilReportSpeciality.MilitaryForceSort != null ? oldMilReportSpeciality.MilitaryForceSort.MilitaryForceSortName : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Lists_MilReportSpeciality_Active", (oldMilReportSpeciality.Active ? "1" : "0"), "", currentUser));

            changeEntry.AddEvent(changeEvent);

            return result;
        }

        //Check if can delete a particular record
        public static bool CanDelete(int milReportSpecialityId, User currentUser)
        {
            bool canDelete = true;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_ADM.PersonMilRepSpec a
                               WHERE a.MilReportSpecialityID = :MilReportSpecialityId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilReportSpecialityId", OracleType.Number).Value = milReportSpecialityId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.GetInt(dr["Cnt"]) > 0)
                    {
                        canDelete = false;
                        break;
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return canDelete;
        }
    }

}
