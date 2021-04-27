using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;
using System.Text;

namespace PMIS.HealthSafety.Common
{
    public class WorkingPlace : BaseDbObject
    {
        private int workingPlaceId;
        private int militaryUnitId;
        private MilitaryUnit militaryUnit;        
        private string workingPlace;      

        public int WorkingPlaceId
        {
            get { return workingPlaceId; }
            set { workingPlaceId = value; }
        }

        public int MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get 
            {
                if (militaryUnit == null)
                {
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(militaryUnitId, CurrentUser);
                }
                return militaryUnit; 
            }
            set 
            { 
                militaryUnit = value; 
            }
        }

        public string WorkingPlaceName
        {
            get { return workingPlace; }
            set { workingPlace = value; }
        }

        bool? canDelete = null;
        public bool CanDelete
        {
            set { canDelete = value; }
            get
            {
                if (canDelete.HasValue)
                    return canDelete.Value;
                else
                    throw new Exception("Value not set");
            }

        }

        public WorkingPlace(User user)
            :base(user)
        {

        }
    }

    public class WorkingPlacesFilter
    {
        public string MilitaryUnitId
        {
            get;
            set;
        }

        public string WorkingPlace
        {
            get;
            set;
        }

        public int OrderBy
        {
            get;
            set;
        }

        public int PageIdx
        {
            get;
            set;
        }
    }

    public static class WorkingPlaceUtil
    {
        public static WorkingPlace GetWorkingPlace(int workingPlaceId, User currentUser)
        {
            WorkingPlace workingPlace = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.WorkingPlaceID, a.MilitaryUnitID, WorkingPlace
                               FROM PMIS_HS.WorkingPlaces a
                               WHERE a.WorkingPlaceID = :WorkingPlaceID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("WorkingPlaceID", OracleType.Number).Value = workingPlaceId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    workingPlace = new WorkingPlace(currentUser);
                    workingPlace.WorkingPlaceId = workingPlaceId;
                    workingPlace.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    workingPlace.WorkingPlaceName = dr["WorkingPlace"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return workingPlace;
        }

        public static string GetWorkingPlacesPerMilUnit_ItemSelector(int pageIndex, int pageCount, string prefix,
                                                                     int militaryUnitID, User currentUser)
        {
            StringBuilder sb = new StringBuilder();
            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            sb.Append("<response>");

            try
            {
                SQL = @"SELECT a.WorkingPlaceID, a.WorkingPlace, b.COUNT
                        FROM (  SELECT a.WorkingPlaceID, a.WorkingPlace,
                                       -1 as RowNumber,
                                       0 as OrderNum
                                FROM PMIS_HS.WorkingPlaces a
                                WHERE a.MilitaryUnitID = :MilitaryUnitID AND 
                                UPPER(a.WorkingPlace) = UPPER(:matchprefix)

                                UNION

                                SELECT a.WorkingPlaceID, a.WorkingPlace,
                                       RANK() OVER (ORDER BY a.WorkingPlace, a.WorkingPlaceID) as RowNumber,
                                       1 as OrderNum
                                FROM PMIS_HS.WorkingPlaces a
                                WHERE  a.MilitaryUnitID = :MilitaryUnitID AND 
                                       UPPER(a.WorkingPlace) LIKE UPPER(:prefix)
                             ) a
                        LEFT OUTER JOIN ( SELECT COUNT(*) as COUNT
                                          FROM PMIS_HS.WorkingPlaces a
                                          WHERE  a.MilitaryUnitID = :MilitaryUnitID AND 
                                          UPPER(a.WorkingPlace) LIKE UPPER(:prefix)
                                        ) b ON 1=1
                        WHERE a.RowNumber = -1 OR a.RowNumber BETWEEN (:pageIndex - 1) * :pageCount + 1 AND :pageIndex * :pageCount
                        GROUP BY a.WorkingPlaceID, a.WorkingPlace, b.COUNT
                        ORDER BY MIN(a.OrderNum), a.WorkingPlace";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("pageIndex", OracleType.Int32).Value = pageIndex;
                cmd.Parameters.Add("pageCount", OracleType.Int32).Value = pageCount;
                cmd.Parameters.Add("prefix", OracleType.VarChar).Value = prefix + "%";
                cmd.Parameters.Add("matchprefix", OracleType.VarChar).Value = prefix;
                cmd.Parameters.Add("MilitaryUnitID", OracleType.Int32).Value = militaryUnitID;

                OracleDataReader dr = cmd.ExecuteReader();

                int count = 0;
                sb.Append("<result>");
                while (dr.Read())
                {
                    count = int.Parse(dr["COUNT"].ToString());
                    sb.Append("<item>");
                    sb.Append("<text>");
                    sb.Append(AJAXTools.EncodeForXML(dr["WorkingPlace"].ToString()));
                    sb.Append("</text>");
                    sb.Append("<value>");
                    sb.Append(AJAXTools.EncodeForXML(dr["WorkingPlaceID"].ToString()));
                    sb.Append("</value>");
                    sb.Append("</item>");
                }

                dr.Close();

                sb.Append("</result>");

                sb.Append("<count>");
                sb.Append(AJAXTools.EncodeForXML(count.ToString()));
                sb.Append("</count>");
            }
            finally
            {
                conn.Close();
            }

            sb.Append("</response>");

            return sb.ToString();
        }

        public static bool AddWorkingPlace(WorkingPlace workingPlace, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                            INSERT INTO PMIS_HS.WorkingPlaces (MilitaryUnitID, WorkingPlace)
                            VALUES (:MilitaryUnitID, :WorkingPlace);

                            SELECT PMIS_HS.WorkingPlaces_ID_SEQ.currval INTO :WorkingPlaceID FROM dual;
                        END;";


                changeEvent = new ChangeEvent("HS_Lists_WorkingPlaces_Add", "", workingPlace.MilitaryUnit, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_WorkingPlaces_Add_WorkingPlace", "", workingPlace.WorkingPlaceName, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_WorkingPlaces_Add_MilitaryUnit", "", workingPlace.MilitaryUnit.DisplayTextForSelection, currentUser));
                    
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramWorkingPlacelID = new OracleParameter();
                paramWorkingPlacelID.ParameterName = "WorkingPlaceID";
                paramWorkingPlacelID.OracleType = OracleType.Number;
                paramWorkingPlacelID.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(paramWorkingPlacelID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = workingPlace.MilitaryUnitId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "WorkingPlace";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = workingPlace.WorkingPlaceName;
                cmd.Parameters.Add(param);                

                cmd.ExecuteNonQuery();

                workingPlace.WorkingPlaceId = DBCommon.GetInt(paramWorkingPlacelID.Value);

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

        //Get a list of all WorkingPlaces for the selected filter
        public static List<WorkingPlace> GetWorkingPlaces(User currentUser, WorkingPlacesFilter filter, int rowsPerPage)
        {
            List<WorkingPlace> workingPlaces = new List<WorkingPlace>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.MilitaryUnitId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryUnitId) + ") ";
                }

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                              @" (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")) ";

                if (!String.IsNullOrEmpty(filter.WorkingPlace))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.WorkingPlace) LIKE UPPER('%" + filter.WorkingPlace.Replace("'", "''") + "%') ";
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
                        orderBySQL = "b.VPN";
                        break;
                    case 2:
                        orderBySQL = "a.WorkingPlace";
                        break;
                    default:
                        orderBySQL = "b.VPN";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir) + ", a.WorkingPlace ASC, a.WorkingPlaceID ASC" + DBCommon.FixNullsOrder("ASC");

                string SQL = @"SELECT * 
                               FROM ( SELECT a.WorkingPlaceID,
                                             a.WorkingPlace,
                                             a.MilitaryUnitID,
                                             NVL((SELECT COUNT(*) FROM PMIS_HS.ProtocolItems WHERE WorkingPlaceID = a.WorkingPlaceID), 0) as RelatedRecords,
                                             RANK() OVER (ORDER BY " + orderBySQL + @") as RowNumber
                                      FROM PMIS_HS.WorkingPlaces a
                                      INNER JOIN UKAZ_OWNER.MIR b ON a.MilitaryUnitID = b.KOD_MIR
                                      " + where + @"
                                      ORDER BY " + orderBySQL + @"
                                     ) tmp
                               " + pageWhere;


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    WorkingPlace workingPlace = new WorkingPlace(currentUser);
                    workingPlace.WorkingPlaceId = DBCommon.GetInt(dr["WorkingPlaceID"]);
                    workingPlace.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    workingPlace.WorkingPlaceName = dr["WorkingPlace"].ToString();

                    int relatedRecords = DBCommon.GetInt(dr["RelatedRecords"]);
                    workingPlace.CanDelete = relatedRecords == 0;

                    workingPlaces.Add(workingPlace);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return workingPlaces;
        }

        public static int GetWorkingPlacesCnt(WorkingPlacesFilter filter, User currentUser)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.MilitaryUnitId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryUnitId) + ") ";
                }

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                              @" (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")) ";

                if (!String.IsNullOrEmpty(filter.WorkingPlace))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.WorkingPlace) LIKE UPPER('%" + filter.WorkingPlace.Replace("'", "''") + "%') ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT COUNT(*) as Cnt                                            
                                FROM PMIS_HS.WorkingPlaces a                
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

        //Delete a working place
        public static bool DeleteWorkingPlace(User currentUser, int workingPlaceId, Change changeEntry)
        {
            bool result = false;

            WorkingPlace oldWorkingPlace = GetWorkingPlace(workingPlaceId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"BEGIN
                                  DELETE FROM PMIS_HS.WorkingPlaces
                                  WHERE WorkingPlaceID = :WorkingPlaceID;
                               END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("WorkingPlaceID", OracleType.Number).Value = workingPlaceId;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            //Save the operation to the changes log
            ChangeEvent changeEvent = new ChangeEvent("HS_Lists_WorkingPlaces_Delete", "", oldWorkingPlace.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_WorkingPlaces_Add_WorkingPlace", oldWorkingPlace.WorkingPlaceName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_WorkingPlaces_Add_MilitaryUnit", oldWorkingPlace.MilitaryUnit.DisplayTextForSelection, "", currentUser));

            changeEntry.AddEvent(changeEvent);

            return result;
        }

        //Save a particular working place.
        public static bool SaveWorkingPlace(User currentUser, WorkingPlace workingPlace, Change changeEntry)
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
                if (workingPlace.WorkingPlaceId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.WorkingPlaces (WorkingPlace, MilitaryUnitID)
                             VALUES (:WorkingPlace, :MilitaryUnitID);

                             SELECT PMIS_HS.WorkingPlaces_ID_SEQ.currval INTO :WorkingPlaceID FROM dual;
                            ";

                    //Add this change to the Audit Trail
                    ChangeEvent changeEvent = new ChangeEvent("HS_Lists_WorkingPlaces_Add", "", workingPlace.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_WorkingPlaces_Add_WorkingPlace", "", workingPlace.WorkingPlaceName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_WorkingPlaces_Add_MilitaryUnit", "", workingPlace.MilitaryUnit.DisplayTextForSelection, currentUser));

                    changeEntry.AddEvent(changeEvent);
                }
                else //UPDATE an existing record
                {
                    SQL += @"UPDATE PMIS_HS.WorkingPlaces SET
                               WorkingPlace = :WorkingPlace, 
                               MilitaryUnitID = :MilitaryUnitID
                             WHERE WorkingPlaceID = :WorkingPlaceID;
                            ";

                    //If there are any actual changes then track them to the Audit Trail log
                    ChangeEvent changeEvent = new ChangeEvent("HS_Lists_WorkingPlaces_Edit", "", workingPlace.MilitaryUnit, null, currentUser);

                    WorkingPlace oldWorkingPlace = GetWorkingPlace(workingPlace.WorkingPlaceId, currentUser);

                    if (!CommonFunctions.IsEqualInt(oldWorkingPlace.MilitaryUnitId, workingPlace.MilitaryUnitId))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_WorkingPlaces_Add_MilitaryUnit", oldWorkingPlace.MilitaryUnit.DisplayTextForSelection, workingPlace.MilitaryUnit.DisplayTextForSelection, currentUser));

                    if (oldWorkingPlace.WorkingPlaceName.Trim() != workingPlace.WorkingPlaceName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_WorkingPlaces_Add_WorkingPlace", oldWorkingPlace.WorkingPlaceName, workingPlace.WorkingPlaceName, currentUser));

                    if (changeEvent.ChangeEventDetails.Count > 0)
                        changeEntry.AddEvent(changeEvent);
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                //Add the MilReportSpecialityID parameter. If it is a new record the pass it as an Output parameter to be ablet to return the new ID.
                OracleParameter paramWorkingPlaceID = new OracleParameter();
                paramWorkingPlaceID.ParameterName = "WorkingPlaceID";
                paramWorkingPlaceID.OracleType = OracleType.Number;

                if (workingPlace.WorkingPlaceId != 0)
                {
                    paramWorkingPlaceID.Direction = ParameterDirection.Input;
                    paramWorkingPlaceID.Value = workingPlace.WorkingPlaceId;
                }
                else
                {
                    paramWorkingPlaceID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramWorkingPlaceID);

                //Add the other parameters to the query
                OracleParameter param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = workingPlace.MilitaryUnitId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "WorkingPlace";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = workingPlace.WorkingPlaceName;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (workingPlace.WorkingPlaceId == 0)
                    workingPlace.WorkingPlaceId = DBCommon.GetInt(paramWorkingPlaceID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static WorkingPlace GetWorkingPlaceByName(int militaryUnitId, string workingPlaceName, User currentUser)
        {
            WorkingPlace workingPlace = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.WorkingPlaceID, a.MilitaryUnitID, WorkingPlace
                               FROM PMIS_HS.WorkingPlaces a
                               WHERE a.MilitaryUnitID = :MilitaryUnitID AND
                                     UPPER(a.WorkingPlace) = UPPER(:WorkingPlace) ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryUnitID", OracleType.Number).Value = militaryUnitId;
                cmd.Parameters.Add("WorkingPlace", OracleType.VarChar).Value = workingPlaceName;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    workingPlace = new WorkingPlace(currentUser);
                    workingPlace.WorkingPlaceId = DBCommon.GetInt(dr["WorkingPlaceID"]);
                    workingPlace.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    workingPlace.WorkingPlaceName = dr["WorkingPlace"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return workingPlace;
        }
    }
}