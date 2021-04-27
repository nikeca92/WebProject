using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

using PMIS.Common;


namespace PMIS.Applicants.Common
{
    public class ReportDocumentsSentFilter
    {
        public int? VacancyAnnounceId { get; set; }
        public int? MilitaryDepartmentId { get; set; }       
    }

    public class ReportDocumentsSentBlock : BaseDbObject
    {
        public string MilitaryDepartment { get; set; }
        public int CntBy_Male { get; set; }
        public int CntBy_Female { get; set; }
        public int CntBy_NoMilitaryTraining_Male { get; set; }
        public int CntBy_NoMilitaryTraining_Female { get; set; }
        public int CntBy_Age_Under25 { get; set; }
        public int CntBy_Age_Under30 { get; set; }
        public int CntBy_Age_Over30 { get; set; }
        public int RowType { get; set; }

        public ReportDocumentsSentBlock(User user) : base(user){}
    }

    public class ReportDocumentsSentUtil
    {
        //This method get List of Reports
        public static List<ReportDocumentsSentBlock> GetReportDocumentsSentSearch(ReportDocumentsSentFilter filter, User currentUser)
        {
            ReportDocumentsSentBlock block;
            List<ReportDocumentsSentBlock> listBlocks = new List<ReportDocumentsSentBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " 1 = 2 ";
                }
                
                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryDepartmentId = " + filter.MilitaryDepartmentId.Value + " ";
                }
                
                where = (where == "" ? "" : " AND ") + where;


                string SQL = @"
                              SELECT tmp.militarydepartment,
			                      NVL(tmp.Cnt_M, 0) as Cnt_M,
			                      NVL(tmp.Cnt_F, 0) as Cnt_F, 
			                      NVL(tmp.Cnt_NoMilTr_M, 0) as Cnt_NoMilTr_M, 
		                           NVL(tmp.Cnt_NoMilTr_F, 0) as Cnt_NoMilTr_F,
		                           NVL(tmp.Cnt_Age_Under25, 0) as Cnt_Age_Under25, 
		                           NVL(tmp.Cnt_Age_Under30, 0) as Cnt_Age_Under30,
		                           NVL(tmp.Cnt_Age_Over30, 0) as Cnt_Age_Over30, 
                                     tmp.RowType 
                            FROM (
	                                SELECT  a.militarydepartmentname as militarydepartment,
			                              SUM(CASE WHEN LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as Cnt_M, 
			                              SUM(CASE WHEN LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as Cnt_F, 
			                              SUM(CASE WHEN a.NoMilitaryTraining = 1 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж'
                                                      THEN 1 
                                                      ELSE 0 
                                                 END) as Cnt_NoMilTr_M, 
		                                   SUM(CASE WHEN a.NoMilitaryTraining = 1 AND LOWER(a.GenderName) = 'жена' 
                                                      THEN 1 
                                                      ELSE 0 
                                                 END) as Cnt_NoMilTr_F,
		                                   SUM(CASE WHEN a.Age > 0 AND a.Age <= 25 THEN 1 ELSE 0 END) as Cnt_Age_Under25, 
		                                   SUM(CASE WHEN a.Age > 25 AND a.Age <= 30 THEN 1 ELSE 0 END) as Cnt_Age_Under30,
		                                   SUM(CASE WHEN a.Age > 30 THEN 1 ELSE 0 END) as Cnt_Age_Over30, 
                                             0 as RowType
	                                FROM
	                                (
	                                   SELECT DISTINCT md.militarydepartmentname,
	                                                   b.personid,
					                               PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age,
					                               g.GenderName,
					                               CASE WHEN NVL(f.MilitaryService, 2) = 2
								                     THEN 1 
								                     ELSE 0
							                     END as NoMilitaryTraining
	                                   FROM PMIS_APPL.Applicants a
	                                   INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
	                                   INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
	                                   INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
	                                   INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
	                                   LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
	                                   INNER JOIN PMIS_ADM.MilitaryDepartments md ON md.MilitaryDepartmentId = a.MilitaryDepartmentId
	                                   WHERE 1 = 1 AND c.applicantstatusid IS NOT NULL " + where + @"                                                   
	                                ) a
	                                GROUP BY militarydepartmentname

                                     UNION ALL

                                      SELECT 'Общо' as militarydepartment,
			                              SUM(CASE WHEN LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as Cnt_M, 
			                              SUM(CASE WHEN LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as Cnt_F, 
			                              SUM(CASE WHEN a.NoMilitaryTraining = 1 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж'
                                                      THEN 1 
                                                      ELSE 0 
                                                 END) as Cnt_NoMilTr_M, 
		                                   SUM(CASE WHEN a.NoMilitaryTraining = 1 AND LOWER(a.GenderName) = 'жена' 
                                                      THEN 1 
                                                      ELSE 0 
                                                 END) as Cnt_NoMilTr_F,
		                                   SUM(CASE WHEN a.Age > 0 AND a.Age <= 25 THEN 1 ELSE 0 END) as Cnt_Age_Under25, 
		                                   SUM(CASE WHEN a.Age > 25 AND a.Age <= 30 THEN 1 ELSE 0 END) as Cnt_Age_Under30,
		                                   SUM(CASE WHEN a.Age > 30 THEN 1 ELSE 0 END) as Cnt_Age_Over30, 
                                             1 as RowType
	                                FROM
	                                (
	                                   SELECT DISTINCT md.militarydepartmentname,
	                                                   b.personid,
					                               PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age,
					                               g.GenderName,
					                               CASE WHEN NVL(f.MilitaryService, 2) = 2
								                     THEN 1 
								                     ELSE 0
							                     END as NoMilitaryTraining
	                                   FROM PMIS_APPL.Applicants a
	                                   INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
	                                   INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
	                                   INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
	                                   INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
	                                   LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
	                                   INNER JOIN PMIS_ADM.MilitaryDepartments md ON md.MilitaryDepartmentId = a.MilitaryDepartmentId
	                                   WHERE 1 = 1 AND c.applicantstatusid IS NOT NULL " + where + @"                                                   
	                                ) a
                                 ) tmp  
                                 ORDER BY tmp.RowType ASC 
                       ";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    block = ExtractReportDocumentsSentBlockFromDataReader(dr, currentUser);
                    listBlocks.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listBlocks;
        }

        public static ReportDocumentsSentBlock ExtractReportDocumentsSentBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportDocumentsSentBlock reportDocumentsSentBlock = new ReportDocumentsSentBlock(currentUser);

            reportDocumentsSentBlock.MilitaryDepartment = dr["MilitaryDepartment"].ToString();
            reportDocumentsSentBlock.CntBy_Male = DBCommon.GetInt(dr["Cnt_M"]);
            reportDocumentsSentBlock.CntBy_Female = DBCommon.GetInt(dr["Cnt_F"]);
            reportDocumentsSentBlock.CntBy_NoMilitaryTraining_Male = DBCommon.GetInt(dr["Cnt_NoMilTr_M"]);
            reportDocumentsSentBlock.CntBy_NoMilitaryTraining_Female = DBCommon.GetInt(dr["Cnt_NoMilTr_F"]);
            reportDocumentsSentBlock.CntBy_Age_Under25 = DBCommon.GetInt(dr["Cnt_Age_Under25"]);
            reportDocumentsSentBlock.CntBy_Age_Under30 = DBCommon.GetInt(dr["Cnt_Age_Under30"]);
            reportDocumentsSentBlock.CntBy_Age_Over30 = DBCommon.GetInt(dr["Cnt_Age_OVer30"]);
            reportDocumentsSentBlock.RowType = DBCommon.GetInt(dr["RowType"]);

            return reportDocumentsSentBlock;
        }
    }
}

