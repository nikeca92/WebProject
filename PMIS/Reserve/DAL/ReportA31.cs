using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;

using PMIS.Common;
using System.Collections;

namespace PMIS.Reserve.Common
{
    //This class represents all information about the filter, the order and the paging information on the screen
    public class ReportA31Filter
    {
        public string MilitaryDepartmentIds { get; set; }

        public string Region { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
     
        public int PageIdx { get; set; }
        public int PageSize { get; set; }
    }

    public class ReportA31Result
    {
        public ReportA31Filter Filter { get; set; }

        public int MaxPage
        {
            get
            {
                return Filter.PageSize == 0 ? 1 : Rows.Count / Filter.PageSize + (Rows.Count != 0 && Rows.Count % Filter.PageSize == 0 ? 0 : 1);
            }
        }

        public string[] HeaderCells { get; set; }
        public ArrayList Rows { get; set; }
    }

    public static class ReportA31Util
    {
        public static ReportA31Result GetReportA31(ReportA31Filter filter, User currentUser)
        {
            ReportA31Result reportResult = new ReportA31Result();
            reportResult.Filter = filter;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string emptyStringForNullJoin = "~=~";

                string whereClause_Common = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause_Common += (whereClause_Common == "" ? "" : " AND ") +
                                    " a.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause_Common += (whereClause_Common == "" ? "" : " AND ") +
                         @" (a.MilitaryDepartmentId IS NULL OR a.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                string whereClause_Persons = whereClause_Common;
                if (!string.IsNullOrEmpty(filter.Region))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.Municipality))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.KOD_NMA_MJ IN ( " + filter.City + ") ";
                }

                if (!string.IsNullOrEmpty(filter.District))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.PermAddrDistrictID IN ( " + filter.District + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Address))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         " UPPER(g.ADRES) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.PostCode))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.PermSecondPostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                }

                string whereClause_Technics = whereClause_Common;
                if (!string.IsNullOrEmpty(filter.Region))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" h.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.Municipality))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" h.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" h.CityID IN ( " + filter.City + ") ";
                }

                if (!string.IsNullOrEmpty(filter.District))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" h.DistrictID IN ( " + filter.District + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Address))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         " UPPER(h.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.PostCode))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" h.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                }

                whereClause_Common = (whereClause_Common == "" ? "" : " WHERE ") + whereClause_Common;
                whereClause_Persons = (whereClause_Persons == "" ? "" : " AND ") + whereClause_Persons;
                whereClause_Technics = (whereClause_Technics == "" ? "" : " AND ") + whereClause_Technics;
               
                string SQL = @"
SELECT MilReadiness, FullCommand, Command, MilitaryDepartmentId, MilitaryDepartmentName, DeliveryCityID, DeliveryPlace,
       ColumnOrder, ColumnLabel, ColumnValue
FROM (
  SELECT MilReadiness, Command as FullCommand, CONCAT(Command, CommandSuffix) as Command, MilitaryDepartmentId, MilitaryDepartmentName, 
         a.DeliveryCityID, UPPER(a.DeliveryPlace) as DeliveryPlace,
         1 as ColumnOrder, 'КОМАНДА' as ColumnLabel, CONCAT(Command, CommandSuffix) as ColumnValue
  FROM PMIS_RES.ViewA31 a
  " + whereClause_Common + @"
  GROUP BY MilReadiness, Command, CONCAT(Command, CommandSuffix), MilitaryDepartmentId, MilitaryDepartmentName, a.DeliveryCityID, UPPER(a.DeliveryPlace)
  
  UNION ALL

  SELECT MilReadiness, Command as FullCommand, CONCAT(Command, CommandSuffix) as Command, MilitaryDepartmentId, MilitaryDepartmentName,
         a.DeliveryCityID, UPPER(a.DeliveryPlace) as DeliveryPlace,
         2 as ColumnOrder, '" + CommonFunctions.GetLabelText("MilitaryDepartment") + @"' as ColumnLabel, MilitaryDepartmentName as ColumnValue 
  FROM PMIS_RES.ViewA31 a
  " + whereClause_Common + @"
  GROUP BY MilReadiness, MilitaryDepartmentId, MilitaryDepartmentName, Command, CONCAT(Command, CommandSuffix), a.DeliveryCityID, UPPER(a.DeliveryPlace)
  
  UNION ALL
  
  SELECT a.MilReadiness as MilReadiness, a.Command as FullCommand, CONCAT(a.Command, a.CommandSuffix) as Command, MilitaryDepartmentId, MilitaryDepartmentName,
         a.DeliveryCityID, UPPER(a.DeliveryPlace) as DeliveryPlace,
         3 as ColumnOrder, 'действително наименование на формированието' as ColumnLabel, MAX(vvr.IMEES) as ColumnValue
  FROM PMIS_RES.ViewA31 a
  INNER JOIN UKAZ_OWNER.VVR vvr ON a.KOD_VVR = vvr.KOD_VVR
  " + whereClause_Common + @"
  GROUP BY a.MilReadiness, a.Command, CONCAT(a.Command, a.CommandSuffix), a.MilitaryDepartmentId, a.MilitaryDepartmentName, a.DeliveryCityID, UPPER(a.DeliveryPlace)
  
  UNION ALL
  
  SELECT a.MilReadiness as MilReadiness, a.Command as FullCommand, CONCAT(a.Command, a.CommandSuffix) as Command, a.MilitaryDepartmentId, a.MilitaryDepartmentName,
         a.DeliveryCityID, UPPER(a.DeliveryPlace) as DeliveryPlace,
         4 as ColumnOrder, 'населено място, където се доставя командата' as ColumnLabel, 
         RTRIM(CASE WHEN e.Ime_Obl IS NOT NULL THEN e.Ime_Obl || ', ' ELSE '' END || CASE WHEN d.Ime_Obs IS NOT NULL THEN d.Ime_Obs || ', ' ELSE '' END || CASE WHEN c.Ime_Nma IS NOT NULL THEN c.Ime_Nma || ', ' ELSE '' END || CASE WHEN MAX(a.DeliveryPlace) IS NOT NULL THEN MAX(a.DeliveryPlace) || ', ' ELSE '' END, ', ') as ColumnValue
  FROM PMIS_RES.ViewA31 a
  LEFT OUTER JOIN UKAZ_OWNER.KL_NMA c ON c.Kod_Nma = a.DeliveryCityID
  LEFT OUTER JOIN UKAZ_OWNER.KL_OBS d ON d.kod_obs = c.kod_obs
  LEFT OUTER JOIN UKAZ_OWNER.KL_OBL e ON e.Kod_Obl = c.kod_obl
  " + whereClause_Common + @"
  GROUP BY a.MilReadiness, a.Command, CONCAT(a.Command, a.CommandSuffix), a.MilitaryDepartmentId, a.MilitaryDepartmentName, a.DeliveryCityID, UPPER(a.DeliveryPlace),
           e.Ime_Obl, d.Ime_Obs, c.Ime_Nma
  
  UNION ALL
  
  SELECT a.MilReadiness as MilReadiness, a.Command as FullCommand, CONCAT(a.Command, a.CommandSuffix) as Command, a.MilitaryDepartmentId, a.MilitaryDepartmentName,
         a.DeliveryCityID, UPPER(a.DeliveryPlace) as DeliveryPlace,
         4 + k.ColumnOrder as ColumnOrder, k.ColumnLabel as ColumnLabel, TO_CHAR(SUM(CASE WHEN i.MilitaryRankCategory IN k.MilitaryRankCategory AND i.MilitaryRankSubCategory IN k.MilitaryRankSubCategory"  + whereClause_Persons + @" THEN 1 ELSE 0 END)) as ColumnValue
  FROM PMIS_RES.ViewA31 a
  INNER JOIN UKAZ_OWNER.VVR vvr ON a.KOD_VVR = vvr.KOD_VVR
  LEFT OUTER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = vvr.KOD_VVR AND 
                                                 NVL(a.CommandSuffix, :EmptyStringForNullJoin) = NVL(b.MilitaryCommandSuffix, :EmptyStringForNullJoin) AND 
                                                 a.MilReadinessID = b.MilReadinessID AND 
                                                 NVL(a.DeliveryCityID, 0) = NVL(b.DeliveryCityID, 0) AND
                                                 NVL(a.DeliveryPlace, :EmptyStringForNullJoin) = NVL(b.DeliveryPlace, :EmptyStringForNullJoin)
  LEFT OUTER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
  LEFT OUTER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.MilitaryDepartmentId = a.MilitaryDepartmentId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
  LEFT OUTER JOIN PMIS_RES.RESERVISTS e ON e.ReservistId = d.ReservistId
  LEFT OUTER JOIN VS_OWNER.VS_LS g ON g.PersonId = e.PersonId
  LEFT JOIN PMIS_ADM.Persons g2 ON g2.PersonID = g.PersonID
  LEFT OUTER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA
  LEFT OUTER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
  LEFT OUTER JOIN
				 (SELECT 'Оф.' as ColumnLabel, 1 as ColumnOrder, (2) as MilitaryRankCategory, (1) as MilitaryRankSubCategory FROM dual
			 	  UNION ALL
				  SELECT 'Оф. к-ти' as ColumnLabel, 2 as ColumnOrder, (1) as MilitaryRankCategory, (3) as MilitaryRankSubCategory FROM dual
                  UNION ALL
				  SELECT 'Серж.' as ColumnLabel, 3 as ColumnOrder, (1) as MilitaryRankCategory, (1) as MilitaryRankSubCategory FROM dual
                  UNION ALL
				  SELECT 'В-ци' as ColumnLabel, 4 as ColumnOrder, (1) as MilitaryRankCategory, (2) as MilitaryRankSubCategory FROM dual
				  ) k ON 1=1
  " + whereClause_Common + @"
  GROUP BY a.MilReadiness, a.Command, CONCAT(a.Command, a.CommandSuffix), k.ColumnOrder, k.ColumnLabel, k.MilitaryRankCategory, a.MilitaryDepartmentId, a.MilitaryDepartmentName,
           a.DeliveryCityID, UPPER(a.DeliveryPlace)
  
  UNION ALL
  
  SELECT a.MilReadiness as MilReadiness, a.Command as FullCommand, CONCAT(a.Command, a.CommandSuffix) as Command, a.MilitaryDepartmentId, a.MilitaryDepartmentName,
         a.DeliveryCityID, UPPER(a.DeliveryPlace) as DeliveryPlace,
         (DENSE_RANK() OVER (ORDER BY g.TableSeq)) + 9 as ColumnOrder, g.TableValue as ColumnLabel, TO_CHAR(SUM(CASE WHEN f.VehicleID IS NOT NULL " + whereClause_Technics + @" THEN 1 ELSE 0 END)) as ColumnValue
  FROM PMIS_RES.ViewA31 a
  INNER JOIN UKAZ_OWNER.VVR vvr ON a.KOD_VVR = vvr.KOD_VVR
  LEFT OUTER JOIN PMIS_RES.TECHNICSREQUESTCOMMANDS b ON b.MilitaryCommandId = vvr.KOD_VVR AND 
                                                        NVL(a.CommandSuffix, :EmptyStringForNullJoin) = NVL(b.MilitaryCommandSuffix, :EmptyStringForNullJoin) AND 
                                                        a.MilReadinessID = b.MilReadinessID AND
                                                        NVL(a.DeliveryCityID, 0) = NVL(b.DeliveryCityID, 0) AND
                                                        NVL(a.DeliveryPlace, :EmptyStringForNullJoin) = NVL(b.DeliveryPlace, :EmptyStringForNullJoin)
  LEFT OUTER JOIN PMIS_RES.TECHNICSREQUESTCMDPOSITIONS c ON c.TechRequestsCommandId = b.TechRequestsCommandId
  LEFT OUTER JOIN PMIS_RES.FULFILTECHNICSREQUEST d ON d.TechnicsRequestCmdPositionId = c.TechnicsRequestCmdPositionId AND d.MilitaryDepartmentId = a.MilitaryDepartmentId AND d.TechnicReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
  LEFT OUTER JOIN PMIS_RES.TECHNICS e ON e.TechnicsId = d.TechnicsId AND e.TechnicsTypeId IN (SELECT TechnicsTypeId FROM PMIS_RES.TECHNICSTYPES WHERE TechnicsTypeKey = 'VEHICLES')
  LEFT OUTER JOIN PMIS_RES.GTABLE g ON g.TableName = 'VehicleKind'
  LEFT OUTER JOIN PMIS_RES.VEHICLES f ON f.TechnicsId = e.TechnicsId AND f.VehicleKindID = g.TableKey
  LEFT OUTER JOIN PMIS_ADM.Companies h ON e.OwnershipCompanyID = h.CompanyID
  " + whereClause_Common + @"
  GROUP BY a.MilReadiness, a.Command, CONCAT(a.Command, a.CommandSuffix), g.TableSeq, g.TableValue, a.MilitaryDepartmentId, a.MilitaryDepartmentName,
           a.DeliveryCityID, UPPER(a.DeliveryPlace)
  
  UNION ALL
  
  SELECT a.MilReadiness as MilReadiness, a.Command as FullCommand, CONCAT(a.Command, a.CommandSuffix) as Command, a.MilitaryDepartmentId, a.MilitaryDepartmentName,
         a.DeliveryCityID, UPPER(a.DeliveryPlace) as DeliveryPlace,
         9 + (SELECT COUNT(*) FROM PMIS_RES.GTABLE WHERE TableName = 'VehicleKind') + (DENSE_RANK() OVER (ORDER BY f.Seq)) as ColumnOrder, f.TechnicsTypeName as ColumnLabel, TO_CHAR(SUM(CASE WHEN f.TechnicsTypeId = e.TechnicsTypeId AND e.TechnicsId IS NOT NULL " + whereClause_Technics + @" THEN e.ItemsCount ELSE 0 END)) as ColumnValue
  FROM PMIS_RES.ViewA31 a
  INNER JOIN UKAZ_OWNER.VVR vvr ON a.KOD_VVR = vvr.KOD_VVR
  LEFT OUTER JOIN PMIS_RES.TECHNICSREQUESTCOMMANDS b ON b.MilitaryCommandId = vvr.KOD_VVR AND 
                                                        NVL(a.CommandSuffix, :EmptyStringForNullJoin) = NVL(b.MilitaryCommandSuffix, :EmptyStringForNullJoin) AND 
                                                        a.MilReadinessID = b.MilReadinessID AND
                                                        NVL(a.DeliveryCityID, 0) = NVL(b.DeliveryCityID, 0) AND
                                                        NVL(a.DeliveryPlace, :EmptyStringForNullJoin) = NVL(b.DeliveryPlace, :EmptyStringForNullJoin)
  LEFT OUTER JOIN PMIS_RES.TECHNICSREQUESTCMDPOSITIONS c ON c.TechRequestsCommandId = b.TechRequestsCommandId
  LEFT OUTER JOIN PMIS_RES.FULFILTECHNICSREQUEST d ON d.TechnicsRequestCmdPositionId = c.TechnicsRequestCmdPositionId AND d.MilitaryDepartmentId = a.MilitaryDepartmentId AND d.TechnicReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
  LEFT OUTER JOIN PMIS_RES.TECHNICS e ON e.TechnicsId = d.TechnicsId
  LEFT OUTER JOIN PMIS_RES.TECHNICSTYPES f ON NVL(f.Active, 0) = 1
  LEFT OUTER JOIN PMIS_ADM.Companies h ON e.OwnershipCompanyID = h.CompanyID
  " + (!String.IsNullOrEmpty(whereClause_Common) ? whereClause_Common + " AND " : "WHERE ") + @"f.TechnicsTypeKey <> 'VEHICLES'  
  GROUP BY a.MilReadiness, a.MilitaryDepartmentId, a.MilitaryDepartmentName, a.Command, CONCAT(a.Command, a.CommandSuffix), a.MilitaryDepartmentId, a.MilitaryDepartmentName, 
           f.Seq, f.TechnicsTypeName, a.DeliveryCityID, UPPER(a.DeliveryPlace)
  
  UNION ALL
  
  SELECT a.MilReadiness as MilReadiness, a.Command as FullCommand, CONCAT(a.Command, a.CommandSuffix) as Command, a.MilitaryDepartmentId, a.MilitaryDepartmentName,
         a.DeliveryCityID, UPPER(a.DeliveryPlace) as DeliveryPlace,
         100 as ColumnOrder, 'с водачи' as ColumnName, TO_CHAR(NVL(SUM(d.DriversCount), 0)) as ColumnValue
  FROM PMIS_RES.ViewA31 a
  INNER JOIN UKAZ_OWNER.VVR vvr ON a.KOD_VVR = vvr.KOD_VVR
  LEFT OUTER JOIN PMIS_RES.TECHNICSREQUESTCOMMANDS b ON b.MilitaryCommandId = vvr.KOD_VVR AND 
                                                        NVL(a.CommandSuffix, :EmptyStringForNullJoin) = NVL(b.MilitaryCommandSuffix, :EmptyStringForNullJoin) AND 
                                                        a.MilReadinessID = b.MilReadinessID AND
                                                        NVL(a.DeliveryCityID, 0) = NVL(b.DeliveryCityID, 0) AND
                                                        NVL(a.DeliveryPlace, :EmptyStringForNullJoin) = NVL(b.DeliveryPlace, :EmptyStringForNullJoin)
  LEFT OUTER JOIN PMIS_RES.TECHNICSREQUESTCMDPOSITIONS c ON c.TechRequestsCommandId = b.TechRequestsCommandId
  LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept d ON d.TechnicsRequestCmdPositionId = c.TechnicsRequestCmdPositionId AND d.MilitaryDepartmentId = a.MilitaryDepartmentId
  " + whereClause_Common + @"
  GROUP BY a.MilReadiness, a.Command, CONCAT(a.Command, a.CommandSuffix), a.MilitaryDepartmentId, a.MilitaryDepartmentName, a.DeliveryCityID, UPPER(a.DeliveryPlace)
 ) a
LEFT OUTER JOIN UKAZ_OWNER.KL_NMA c ON c.Kod_Nma = a.DeliveryCityID
LEFT OUTER JOIN UKAZ_OWNER.KL_OBS d ON d.kod_obs = c.kod_obs
LEFT OUTER JOIN UKAZ_OWNER.KL_OBL e ON e.Kod_Obl = c.kod_obl
ORDER BY MilReadiness, FullCommand, Command, MilitaryDepartmentName, MilitaryDepartmentId, e.Ime_Obl, d.Ime_Obs, c.Ime_Nma, DeliveryPlace, ColumnOrder
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EmptyStringForNullJoin", OracleType.VarChar).Value = emptyStringForNullJoin;

                OracleDataReader dr = cmd.ExecuteReader();

                bool isFirstRow = true;
                ArrayList headerCellsList = new ArrayList();
                ArrayList rowsList = new ArrayList();

                ArrayList tmpRow = new ArrayList();
                ArrayList totalCommandRow = new ArrayList();
                ArrayList totalCommand = new ArrayList();
                ArrayList totalReadiness = new ArrayList();
                ArrayList totalReport = new ArrayList();
                string oldCommand = "";
                string oldFullCommand = "";
                string oldMilReadiness = "";
                string oldMilitaryDepartmentName = "";
                int oldDeliveryCityID = -1;
                string oldDeliveryPlace = null;

                while (dr.Read())
                {
                    string milReadiness = (string)dr["MilReadiness"];
                    string fullCommand = (string)dr["FullCommand"];
                    string command = (string)dr["Command"];
                    string miliaryDepartmentName = (string)dr["MilitaryDepartmentName"];
                    int deliveryCityID = DBCommon.IsInt(dr["DeliveryCityID"]) ? DBCommon.GetInt(dr["DeliveryCityID"]) : 0;
                    string deliveryPlace = dr["DeliveryPlace"].ToString();
                    int columnOrder = int.Parse(dr["ColumnOrder"].ToString());
                    string columnLabel = (string)dr["ColumnLabel"];
                    string columnValue = dr["ColumnValue"].ToString();

                    if ((!string.IsNullOrEmpty(oldCommand) && oldCommand != command) ||
                        (!string.IsNullOrEmpty(oldMilitaryDepartmentName) && oldMilitaryDepartmentName != miliaryDepartmentName) ||
                        (oldDeliveryCityID != -1 && oldDeliveryCityID != deliveryCityID) ||
                        (oldDeliveryPlace != null && oldDeliveryPlace != deliveryPlace) ||
                        (!string.IsNullOrEmpty(oldMilReadiness) && oldMilReadiness != milReadiness)
                       )
                    { 
                        //new row
                        isFirstRow = false;

                        rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                        totalCommand.Add((int[])totalCommandRow.ToArray(typeof(int)));
                        totalReadiness.Add((int[])totalCommandRow.ToArray(typeof(int)));
                        totalReport.Add((int[])totalCommandRow.ToArray(typeof(int)));
                        tmpRow.Clear();
                        totalCommandRow.Clear();
                    }

                    if (!string.IsNullOrEmpty(oldFullCommand) && oldFullCommand != fullCommand ||
                        !string.IsNullOrEmpty(oldMilReadiness) && oldMilReadiness != milReadiness)
                    {                        
                        if (totalCommand.Count > 1)
                        {
                            int[][] total = (int[][])totalCommand.ToArray(typeof(int[]));
                            tmpRow.Add(oldFullCommand);
                            tmpRow.Add("");
                            tmpRow.Add("");
                            tmpRow.Add("");

                            for (int i = 0; i < total[0].Count(); i++)
                            {
                                int tmpValue = 0;

                                for (int j = 0; j < total.Count(); j++)
                                {
                                    tmpValue += total[j][i];
                                }

                                tmpRow.Add(tmpValue.ToString());
                            }

                            rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                            tmpRow.Clear();
                        }

                        totalCommand.Clear();
                    }

                    if (!string.IsNullOrEmpty(oldMilReadiness) && oldMilReadiness != milReadiness)
                    {
                        if (totalReadiness.Count >= 1)
                        {
                            int[][] total = (int[][])totalReadiness.ToArray(typeof(int[]));
                            tmpRow.Add("Всичко за " + oldMilReadiness);
                            tmpRow.Add("");
                            tmpRow.Add("");
                            tmpRow.Add("");

                            for (int i = 0; i < total[0].Count(); i++)
                            {
                                int tmpValue = 0;

                                for (int j = 0; j < total.Count(); j++)
                                {
                                    tmpValue += total[j][i];
                                }

                                tmpRow.Add(tmpValue.ToString());
                            }

                            rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                            tmpRow.Clear();
                        }

                        totalCommand.Clear();
                        totalReadiness.Clear();
                    }

                    oldCommand = command;
                    oldFullCommand = fullCommand;
                    oldMilReadiness = milReadiness;
                    oldMilitaryDepartmentName = miliaryDepartmentName;
                    oldDeliveryCityID = deliveryCityID;
                    oldDeliveryPlace = deliveryPlace;

                    if (isFirstRow)
                    {
                        headerCellsList.Add(columnLabel);
                    }

                    tmpRow.Add(columnValue);
                    if (columnOrder > 4)
                    {
                        totalCommandRow.Add(int.Parse(columnValue));
                    }
                }

                dr.Close();

                if (tmpRow.Count > 0)
                    rowsList.Add((string[])tmpRow.ToArray(typeof(string)));

                if (totalCommandRow.Count > 0)
                {
                    totalCommand.Add((int[])totalCommandRow.ToArray(typeof(int)));
                    totalReadiness.Add((int[])totalCommandRow.ToArray(typeof(int)));
                    totalReport.Add((int[])totalCommandRow.ToArray(typeof(int)));
                }
                
                tmpRow.Clear();

                if (totalCommand.Count > 1)
                {
                    int[][] total = (int[][])totalCommand.ToArray(typeof(int[]));
                    tmpRow.Add(oldFullCommand);
                    tmpRow.Add("");
                    tmpRow.Add("");
                    tmpRow.Add("");

                    for (int i = 0; i < total[0].Count(); i++)
                    {
                        int tmpValue = 0;

                        for (int j = 0; j < total.Count(); j++)
                        {
                            tmpValue += total[j][i];
                        }

                        tmpRow.Add(tmpValue.ToString());
                    }

                    rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                    tmpRow.Clear();
                }

                if (totalReadiness.Count >= 1)
                {
                    int[][] total = (int[][])totalReadiness.ToArray(typeof(int[]));
                    tmpRow.Add("Всичко за " + oldMilReadiness);
                    tmpRow.Add("");
                    tmpRow.Add("");
                    tmpRow.Add("");

                    for (int i = 0; i < total[0].Count(); i++)
                    {
                        int tmpValue = 0;

                        for (int j = 0; j < total.Count(); j++)
                        {
                            tmpValue += total[j][i];
                        }

                        tmpRow.Add(tmpValue.ToString());
                    }

                    rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                    tmpRow.Clear();
                }

                if (totalReport.Count >= 1)
                {
                    int[][] total = (int[][])totalReport.ToArray(typeof(int[]));
                    tmpRow.Add("Всичко ресурси");
                    tmpRow.Add("");
                    tmpRow.Add("");
                    tmpRow.Add("");

                    for (int i = 0; i < total[0].Count(); i++)
                    {
                        int tmpValue = 0;

                        for (int j = 0; j < total.Count(); j++)
                        {
                            tmpValue += total[j][i];
                        }

                        tmpRow.Add(tmpValue.ToString());
                    }

                    rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                    tmpRow.Clear();
                }

                reportResult.HeaderCells = (string[])headerCellsList.ToArray(typeof(string));
                reportResult.Rows = rowsList;
            }
            finally
            {
                conn.Close();
            }
           
            return reportResult;
        }
    }
}