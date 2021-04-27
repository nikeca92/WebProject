using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class MilitaryUnit : BaseDbObject
    {
        private int militaryUnitId;
        private int? parentId;        
        private int cityId;
        private City city;
        private string shortName;
        private string longName;
        private string vpn;

        public int MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }

        public int? ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        public int CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }

        public City City
        {
            get 
            {
                if (city == null)
                    city = CityUtil.GetCity(cityId, CurrentUser);
                return city; 
            }
            set { city = value; }
        }

        public string ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }

        public string LongName
        {
            get { return longName; }
            set { longName = value; }
        }

        public string VPN
        {
            get { return vpn; }
            set { vpn = value; }
        }

        public string DisplayTextForSelection
        {
            get
            {
                var text = "";

                if (!String.IsNullOrEmpty(VPN))
                    text += vpn;

                if (!String.IsNullOrEmpty(ShortName))
                    text += (String.IsNullOrEmpty(text) ? "" : " ") + ShortName;

                return text;
            }
        }

        public MilitaryUnit(User user)
            : base(user)
        {
        }     
    }

    public class MilitaryUnitNode : MilitaryUnit
    {
        private int depth;

        public int Depth
        {
            get
            {
                return depth;
            }
            set
            {
                depth = value;
            }
        }

        public MilitaryUnitNode(MilitaryUnit militaryUnit, int depth, User user)
            : base(user)
        {
            this.MilitaryUnitId = militaryUnit.MilitaryUnitId;
            this.ParentId = militaryUnit.ParentId;
            this.CityId = militaryUnit.CityId;
            this.ShortName = militaryUnit.ShortName;
            this.LongName = militaryUnit.LongName;
            this.VPN = militaryUnit.VPN;

            this.Depth = depth;
        }
    }

    public static class MilitaryUnitUtil
    {
        public static MilitaryUnit ExtractMilitaryUnitFromDR(User currentUser, OracleDataReader dr)
        {
            MilitaryUnit militaryUnit = new MilitaryUnit(currentUser);

            militaryUnit.MilitaryUnitId = (DBCommon.IsInt(dr["MilitaryUnitID"]) ? DBCommon.GetInt(dr["MilitaryUnitID"]) : 0);
            militaryUnit.ParentId = (DBCommon.IsInt(dr["ParentID"]) ? (int?)DBCommon.GetInt(dr["ParentID"]) : null);
            militaryUnit.CityId = (DBCommon.IsInt(dr["CityID"]) ? DBCommon.GetInt(dr["CityID"]) : 0);
            militaryUnit.ShortName = dr["ShortName"].ToString();
            militaryUnit.LongName = dr["LongName"].ToString();
            militaryUnit.VPN = dr["VPN"].ToString();
            
            return militaryUnit;
        }

        public static MilitaryUnit GetMilitaryUnit(int militaryUnitId, User currentUser)
        {
            MilitaryUnit militaryUnit = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KOD_MIR as MilitaryUnitID, 
                                      b.STR_KOD_MIR as ParentID,
                                      a.KOD_NMA as CityID, 
                                      a.IMEES as ShortName, 
                                      a.IMEED as LongName,
                                      a.VPN as VPN
                               FROM UKAZ_OWNER.MIR a
                               INNER JOIN UKAZ_OWNER.STRM b ON a.KOD_MIR = b.KOD_MIR
                               WHERE a.KOD_MIR = :MilitaryUnitId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryUnitId", OracleType.Number).Value = militaryUnitId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryUnit = ExtractMilitaryUnitFromDR(currentUser, dr);              
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryUnit;
        }

        public static string GetMilitaryUnitIDsPerUser(User user)
        {
            string sql = "SELECT * FROM TABLE(PMIS_ADM.CommonFunctions.GetMilitaryUnitIDsPerUser(" + user.UserId.ToString() + "))";
            return sql;
        }

        public static List<MilitaryUnit> GetAllMilitaryUnits(User currentUser)
        {
            return GetAllMilitaryUnits(currentUser, "", true);
        }

        public static List<MilitaryUnit> GetAllMilitaryUnits(User currentUser, string searchText, bool includeItemsWithoutVPN)
        {
            return GetAllMilitaryUnits(currentUser, "", true, true);
        }

        public static List<MilitaryUnit> GetAllMilitaryUnits(User currentUser, string searchText, bool includeItemsWithoutVPN, bool includeOnlyActual)
        {
            List<MilitaryUnit> militaryUnits = new List<MilitaryUnit>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KOD_MIR as MilitaryUnitID, 
                                      b.STR_KOD_MIR as ParentID, 
                                      a.KOD_NMA as CityID, 
                                      a.IMEES as ShortName, 
                                      a.IMEED as LongName,
                                      a.VPN as VPN
                               FROM UKAZ_OWNER.MIR a
                               INNER JOIN UKAZ_OWNER.STRM b ON a.KOD_MIR = b.KOD_MIR
                               WHERE (UPPER(a.IMEES) LIKE UPPER(:SearchText) OR UPPER(a.VPN) LIKE UPPER(:SearchText) OR UPPER(a.VPN || ' ' || a.IMEES) LIKE UPPER(:SearchText)) 
                              ";

                if (includeOnlyActual)
                    SQL += @" AND PMIS_ADM.CommonFunctions.IsMilitaryUnitActual(a.KOD_MIR) = 1 
                            ";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND b.KOD_MIR IN (" + currentUser.MilitaryUnitIDs + @")
                            ";

                if (!includeItemsWithoutVPN)
                    SQL += @" AND a.VPN IS NOT NULL
                            ";

                SQL += " ORDER BY a.VPN";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("SearchText", OracleType.VarChar).Value = "%" + searchText + "%";

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        militaryUnits.Add(ExtractMilitaryUnitFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }               

            return militaryUnits;
        }

        public static List<MilitaryUnit> GetMilitaryUnitsAssignedToUserWithoutChilds(User currentUser, User user)
        {
            List<MilitaryUnit> militaryUnits = new List<MilitaryUnit>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KOD_MIR as MilitaryUnitID, 
                                      b.STR_KOD_MIR as ParentID, 
                                      a.KOD_NMA as CityID, 
                                      a.IMEES as ShortName, 
                                      a.IMEED as LongName,
                                      a.VPN as VPN
                               FROM UKAZ_OWNER.MIR a
                               INNER JOIN UKAZ_OWNER.STRM b ON a.KOD_MIR = b.KOD_MIR
                               INNER JOIN PMIS_ADM.MilitaryUnitsPerUser c ON a.KOD_MIR = c.MilitaryUnitID
                               WHERE c.UserID = :UserID
                              ";

                SQL += " ORDER BY a.VPN";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UserID", OracleType.Number).Value = user.UserId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        militaryUnits.Add(ExtractMilitaryUnitFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryUnits;
        }

        public static List<MilitaryUnit> GetMilitaryUnitsByIDsWithoutChilds(User currentUser, string militaryUnitIds)
        {
            List<MilitaryUnit> militaryUnits = new List<MilitaryUnit>();

            if (String.IsNullOrEmpty(militaryUnitIds))
                militaryUnitIds = "-1";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KOD_MIR as MilitaryUnitID, 
                                      b.STR_KOD_MIR as ParentID, 
                                      a.KOD_NMA as CityID, 
                                      a.IMEES as ShortName, 
                                      a.IMEED as LongName,
                                      a.VPN as VPN
                               FROM UKAZ_OWNER.MIR a
                               INNER JOIN UKAZ_OWNER.STRM b ON a.KOD_MIR = b.KOD_MIR
                               WHERE b.KOD_MIR IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryUnitIds) + @")
                              ";

                SQL += " ORDER BY a.VPN";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        militaryUnits.Add(ExtractMilitaryUnitFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryUnits;
        }

        public static int GetAllMilitaryUnitsCount(User currentUser)
        {
            int Cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM UKAZ_OWNER.MIR a
                               INNER JOIN UKAZ_OWNER.STRM b ON a.KOD_MIR = b.KOD_MIR
                               WHERE PMIS_ADM.CommonFunctions.IsMilitaryUnitActual(a.KOD_MIR) = 1
                             ";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND b.KOD_MIR IN (" + currentUser.MilitaryUnitIDs + @")
                            ";

                SQL += " ORDER BY a.VPN";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        Cnt = DBCommon.GetInt(dr["Cnt"]);                        
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return Cnt;
        }

        public static List<MilitaryUnit> GetTopMilitaryUnits(List<MilitaryUnit> militaryUnits)
        {
            List<MilitaryUnit> topMilitaryUnits = new List<MilitaryUnit>();
            
            foreach (MilitaryUnit unit in militaryUnits)
            {
                if (!unit.ParentId.HasValue)
                {
                    topMilitaryUnits.Add(unit);
                }
                else
                {
                    bool isTop = true;
                    int parentId = unit.ParentId.Value;

                    foreach(MilitaryUnit u in militaryUnits)
                        if (u.MilitaryUnitId == parentId)
                        {
                            isTop = false;
                            break;
                        }

                    if (isTop)
                    {
                        topMilitaryUnits.Add(unit);
                    }
                }
            }

            return topMilitaryUnits;
        }

        public static void GenerateTreeList(List<MilitaryUnit> militaryUnits, ref List<MilitaryUnitNode> treeList, MilitaryUnit node, int depth)
        {
            if (node == null)
            {
                List<MilitaryUnit> topUnits = GetTopMilitaryUnits(militaryUnits);

                treeList = new List<MilitaryUnitNode>();

                foreach (MilitaryUnit unit in topUnits)
                {
                    treeList.Add(new MilitaryUnitNode(unit, 0, null));
                    GenerateTreeList(militaryUnits, ref treeList, unit, 1);
                }
            }
            else
            {
                foreach (MilitaryUnit unit in militaryUnits)
                {
                    if (unit.ParentId == node.MilitaryUnitId)
                    {
                        treeList.Add(new MilitaryUnitNode(unit, depth, null));
                        GenerateTreeList(militaryUnits, ref treeList, unit, depth + 1);
                    }
                }
            }
        }

        public static string GenerateXML(List<MilitaryUnitNode> militaryUnits, int pageIndex, int pageCount)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<response>");

            sb.Append("<result>");

            List<MilitaryUnitNode> pageMilitaryUnits = militaryUnits.Skip((pageIndex - 1) * pageCount).Take(pageCount).ToList();

            foreach (MilitaryUnitNode mu in pageMilitaryUnits)
            {
                sb.Append("<item>");
                sb.Append("<ID>");
                sb.Append(AJAXTools.EncodeForXML(mu.MilitaryUnitId.ToString()));
                sb.Append("</ID>");
                sb.Append("<VPN>");
                sb.Append(AJAXTools.EncodeForXML(mu.VPN));
                sb.Append("</VPN>");
                sb.Append("<Name>");
                sb.Append(AJAXTools.EncodeForXML(mu.ShortName));
                sb.Append("</Name>");
                sb.Append("<Depth>");
                sb.Append(mu.Depth.ToString());
                sb.Append("</Depth>");
                sb.Append("</item>");
            }
            sb.Append("</result>");

            sb.Append("<count>");
            sb.Append(militaryUnits.Count.ToString());
            sb.Append("</count>");


            sb.Append("</response>");

            return sb.ToString();
        }

        //Update the list of Military Units per user
        public static bool UpdateMilitaryUnitsPerUser(User user, string newMilitaryUnitIds, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            //Changes log
            string logDescription = "Потребител: " + user.Username + "; Име: " + user.FullName;

            List<MilitaryUnit> oldMilitaryUnits = GetMilitaryUnitsAssignedToUserWithoutChilds(currentUser, user);
            List<MilitaryUnit> newMilitaryUnits = GetMilitaryUnitsByIDsWithoutChilds(currentUser, newMilitaryUnitIds);

            var newMilUnitIds = (from u in newMilitaryUnits select u.MilitaryUnitId);
            var oldMilUnitIds = (from u in oldMilitaryUnits select u.MilitaryUnitId);

            var deletedMilitaryUnits = (from u in oldMilitaryUnits
                                        where !newMilUnitIds.Contains(u.MilitaryUnitId)
                                        select u);

            var newlyAddedMilitaryUnits = (from u in newMilitaryUnits
                                           where !oldMilUnitIds.Contains(u.MilitaryUnitId)
                                           select u);


            //Log all items that have been deleted if any
            foreach (MilitaryUnit deletedMilitaryUnit in deletedMilitaryUnits)
            {
                ChangeEvent changeEvent = changeEvent = new ChangeEvent("ADM_MilStructureAccess_DelMilUnit", logDescription, null, null, currentUser);
                changeEvent.AddDetail(new ChangeEventDetail("ADM_MilStructureAccess_MilUnit", deletedMilitaryUnit.DisplayTextForSelection, "", currentUser));
                changeEntry.AddEvent(changeEvent);
            }

            //Log all items that have been newly added if any
            foreach (MilitaryUnit newlyAddedMilitaryUnit in newlyAddedMilitaryUnits)
            {
                ChangeEvent changeEvent = changeEvent = new ChangeEvent("ADM_MilStructureAccess_AddMilUnit", logDescription, null, null, currentUser);
                changeEvent.AddDetail(new ChangeEventDetail("ADM_MilStructureAccess_MilUnit", "", newlyAddedMilitaryUnit.DisplayTextForSelection, currentUser));
                changeEntry.AddEvent(changeEvent);
            }

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           DELETE FROM PMIS_ADM.MilitaryUnitsPerUser
                           WHERE UserID = :UserID;

                       ";


                if (!String.IsNullOrEmpty(newMilitaryUnitIds))
                {
                    SQL += @"INSERT INTO PMIS_ADM.MilitaryUnitsPerUser (UserID, MilitaryUnitID)
                             SELECT :UserID, a.KOD_MIR as MilitaryUnitID
                             FROM UKAZ_OWNER.MIR a
                             WHERE a.KOD_MIR IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(newMilitaryUnitIds) + @");
                            ";
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UserID", OracleType.Number).Value = user.UserId;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Get  GetMilitaryUnitId
        public static int GetMilitaryUnitsId(string vpn, string shortName, User currentUser)
        {
            int Cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KOD_MIR as MilitaryUnitID                                  
                               FROM UKAZ_OWNER.MIR a                   
                               where a.vpn = :vpn and a.imees =:shortName
                              ";
               
                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("vpn", OracleType.VarChar).Value = vpn;
                cmd.Parameters.Add("shortName", OracleType.VarChar).Value = shortName;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        Cnt = DBCommon.GetInt(dr["MilitaryUnitID"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return Cnt;
        }

        public static bool CanAccess(int militaryUnitId, User currentUser)
        {
            bool canAccess = false;

            List<MilitaryUnit> availableMilitaryUnits = GetAllMilitaryUnits(currentUser);

            foreach (MilitaryUnit militaryUnit in availableMilitaryUnits)
            {
                if (militaryUnit.MilitaryUnitId == militaryUnitId)
                {
                    canAccess = true;
                    break;
                }
            }

            return canAccess;
        }
    }

    public static class MilitaryUnitNodeUtil
    {
        public static List<MilitaryUnitNode> TransformMilitaryUnitsList(List<MilitaryUnit> militaryUnits, User currentUser)
        {
            List<MilitaryUnitNode> result = new List<MilitaryUnitNode>();

            foreach(MilitaryUnit unit in militaryUnits)
                result.Add(new MilitaryUnitNode(unit, 0, currentUser));

            return result;
        }
    }
}
