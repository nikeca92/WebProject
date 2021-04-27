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
    public class ReportDocumentsAppliedFilter 
    {
        public int? VacancyAnnounceId { get; set; }
        public int? MilitaryDepartmentId { get; set; }        
    }
                 
    public class ReportDocumentsAppliedBlock : BaseDbObject
    {
        public string MilitaryDepartment { get; set; }
        public int CntBy_DocumentsApplied_Male { get; set; }
        public int CntBy_DocumentsApplied_Female { get; set; }
        public int CntBy_MedCertFit_Male { get; set; }
        public int CntBy_MedCertFit_Female { get; set; }
        public int CntBy_MedCertFit_NoMilitaryTraining_Male { get; set; }
        public int CntBy_MedCertFit_NoMilitaryTraining_Female { get; set; }
        public int CntBy_MedCertFit_Age_Under25 { get; set; }
        public int CntBy_MedCertFit_Age_Under30 { get; set; }
        public int CntBy_MedCertFit_Age_Over30 { get; set; }
        public int RowType { get; set; }

        public ReportDocumentsAppliedBlock(User user) : base(user)
        {

        }
    }

    public class ReportDocumentsAppliedUtil
    {
        //This method get List of Reports
        public static List<ReportDocumentsAppliedBlock> GetReportDocumentsAppliedSearch(ReportDocumentsAppliedFilter filter, User currentUser)
        {
            ReportDocumentsAppliedBlock block;
            List<ReportDocumentsAppliedBlock> listBlocks = new List<ReportDocumentsAppliedBlock>();

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


                string SQL = @" SELECT tmp.militarydepartment,
                                       NVL(tmp.Cnt_DocAppl_M, 0) as Cnt_DocAppl_M,
                                       NVL(tmp.Cnt_DocAppl_F, 0) as Cnt_DocAppl_F,
                                       NVL(tmp.Cnt_MCFit_M, 0) as Cnt_MCFit_M,
                                       NVL(tmp.Cnt_MCFit_F, 0) as Cnt_MCFit_F,
                                       NVL(tmp.Cnt_MCFit_NoMilTr_M, 0) as Cnt_MCFit_NoMilTr_M,
                                       NVL(tmp.Cnt_MCFit_NoMilTr_F, 0) as Cnt_MCFit_NoMilTr_F,
                                       NVL(tmp.Cnt_MCFit_Age_Under25, 0) as Cnt_MCFit_Age_Under25,
                                       NVL(tmp.Cnt_MCFit_Age_Under30, 0) as Cnt_MCFit_Age_Under30,
                                       NVL(tmp.Cnt_MCFit_Age_Over30, 0) as Cnt_MCFit_Age_Over30,
                                       tmp.RowType 
                                FROM (
	                                    SELECT  a.militarydepartmentname as militarydepartment,
			                                  SUM(CASE WHEN LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as Cnt_DocAppl_M,
			                                  SUM(CASE WHEN LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as Cnt_DocAppl_F,
			                                  SUM(CASE WHEN a.MedCertFit = 1 AND
                                                               LOWER(NVL(a.GenderName, 'мъж')) = 'мъж'
                                                          THEN 1
                                                          ELSE 0
                                                     END) as Cnt_MCFit_M, 
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND
                                                               LOWER(a.GenderName) = 'жена'
                                                          THEN 1
                                                          ELSE 0
                                                     END) as Cnt_MCFit_F, 
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND
                                                               a.NoMilitaryTraining = 1 AND
                                                               LOWER(NVL(a.GenderName, 'мъж')) = 'мъж'
                                                          THEN 1
                                                          ELSE 0
                                                     END) as Cnt_MCFit_NoMilTr_M, 
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND
                                                               a.NoMilitaryTraining = 1 AND
                                                               LOWER(a.GenderName) = 'жена'
                                                          THEN 1
                                                          ELSE 0
                                                     END) as Cnt_MCFit_NoMilTr_F,
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND a.Age > 0 AND a.Age <= 25 
                                                          THEN 1 
                                                          ELSE 0 
                                                     END) as Cnt_MCFit_Age_Under25, 
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND a.Age > 25 AND a.Age <= 30 
                                                          THEN 1 
                                                          ELSE 0 
                                                     END) as Cnt_MCFit_Age_Under30,
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND a.Age > 30 
                                                          THEN 1 
                                                          ELSE 0 
                                                     END) as Cnt_MCFit_Age_Over30,
                                                 0 as RowType
	                                    FROM
	                                    (
	                                       SELECT DISTINCT md.militarydepartmentname,
	                                                       b.personid,
				                                        PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age,
				                                        g.GenderName,
				                                        CASE WHEN NVL( mmc.militarymedicalconclusionkey, 'NOTFIT') = 'FIT' AND
                                                                      NVL(mpc.militarymedicalconclusionkey, 'NOTFIT') = 'FIT' 
                                                                 THEN 1 
                                                                 ELSE 0 
                                                            END as MedCertFit, 
				                                        CASE WHEN NVL(f.MilitaryService, 2) = 2
                                                                 THEN 1 
                                                                 ELSE 0
                                                            END as NoMilitaryTraining 
	                                       FROM PMIS_APPL.Applicants a
                                            INNER JOIN PMIS_ADM.MilitaryDepartments md ON md.MilitaryDepartmentId = a.MilitaryDepartmentId
	                                       INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
	                                       INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
	                                       INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
	                                       INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
	                                       LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                           LEFT OUTER JOIN (SELECT a.PersonID, a.ConclusionID, 
                                                                   RANK() OVER (PARTITION BY PersonID ORDER BY MedCertDate DESC, MedCertID desc) med_rank
                                                            FROM PMIS_ADM.MedCert a) mc on a.PersonID = mc.PersonID and mc.med_rank = 1
                                           LEFT OUTER JOIN (SELECT a.PersonID, a.ConclusionID, 
                                                                   RANK() OVER (PARTITION BY PersonID ORDER BY PsychCertDate DESC, PsychCertID desc) psych_rank
                                                            FROM PMIS_ADM.PsychCert a) pc on a.PersonID = pc.PersonID and pc.psych_rank = 1
                                           LEFT OUTER JOIN PMIS_ADM.MilitaryMedicalConclusions mmc ON mmc.militarymedicalconclusionid = mc.ConclusionID
                                           LEFT OUTER JOIN PMIS_ADM.MilitaryMedicalConclusions mpc ON mpc.militarymedicalconclusionid = pc.ConclusionID
	                                       WHERE 1 = 1 " + where + @"                                                   
	                                    ) a
	                                    GROUP BY militarydepartmentname

                                         UNION ALL

                                         SELECT  'Общо' as militarydepartment,
			                                  SUM(CASE WHEN LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as Cnt_DocAppl_M,
			                                  SUM(CASE WHEN LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as Cnt_DocAppl_F,
			                                  SUM(CASE WHEN a.MedCertFit = 1 AND
                                                               LOWER(NVL(a.GenderName, 'мъж')) = 'мъж'
                                                          THEN 1
                                                          ELSE 0
                                                     END) as Cnt_MCFit_M, 
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND
                                                               LOWER(a.GenderName) = 'жена'
                                                          THEN 1
                                                          ELSE 0
                                                     END) as Cnt_MCFit_F, 
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND
                                                               a.NoMilitaryTraining = 1 AND
                                                               LOWER(NVL(a.GenderName, 'мъж')) = 'мъж'
                                                          THEN 1
                                                          ELSE 0
                                                     END) as Cnt_MCFit_NoMilTr_M, 
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND
                                                               a.NoMilitaryTraining = 1 AND
                                                               LOWER(a.GenderName) = 'жена'
                                                          THEN 1
                                                          ELSE 0
                                                     END) as Cnt_MCFit_NoMilTr_F,
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND a.Age > 0 AND a.Age <= 25 
                                                          THEN 1 
                                                          ELSE 0 
                                                     END) as Cnt_MCFit_Age_Under25, 
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND a.Age > 25 AND a.Age <= 30 
                                                          THEN 1 
                                                          ELSE 0 
                                                     END) as Cnt_MCFit_Age_Under30,
		                                       SUM(CASE WHEN a.MedCertFit = 1 AND a.Age > 30 
                                                          THEN 1 
                                                          ELSE 0 
                                                     END) as Cnt_MCFit_Age_Over30,
                                                 1 as RowType
	                                    FROM
	                                    (
	                                       SELECT DISTINCT b.personid,
				                                        PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age,
				                                        g.GenderName,
				                                        CASE WHEN NVL( mmc.militarymedicalconclusionkey, 'NOTFIT') = 'FIT' AND
                                                                      NVL(mpc.militarymedicalconclusionkey, 'NOTFIT') = 'FIT' 
                                                                 THEN 1 
                                                                 ELSE 0 
                                                            END as MedCertFit, 
				                                        CASE WHEN NVL(f.MilitaryService, 2) = 2
                                                                 THEN 1 
                                                                 ELSE 0
                                                            END as NoMilitaryTraining 
	                                       FROM PMIS_APPL.Applicants a
                                            INNER JOIN PMIS_ADM.MilitaryDepartments md ON md.MilitaryDepartmentId = a.MilitaryDepartmentId
	                                       INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
	                                       INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
	                                       INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
	                                       INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
	                                       LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                           LEFT OUTER JOIN (SELECT a.PersonID, a.ConclusionID, 
                                                                   RANK() OVER (PARTITION BY PersonID ORDER BY MedCertDate DESC, MedCertID desc) med_rank
                                                            FROM PMIS_ADM.MedCert a) mc on a.PersonID = mc.PersonID and mc.med_rank = 1
                                           LEFT OUTER JOIN (SELECT a.PersonID, a.ConclusionID, 
                                                                   RANK() OVER (PARTITION BY PersonID ORDER BY PsychCertDate DESC, PsychCertID desc) psych_rank
                                                            FROM PMIS_ADM.PsychCert a) pc on a.PersonID = pc.PersonID and pc.psych_rank = 1
                                           LEFT OUTER JOIN PMIS_ADM.MilitaryMedicalConclusions mmc ON mmc.militarymedicalconclusionid = mc.ConclusionID
                                           LEFT OUTER JOIN PMIS_ADM.MilitaryMedicalConclusions mpc ON mpc.militarymedicalconclusionid = pc.ConclusionID
	                                       WHERE 1 = 1 " + where + @"                                                   
	                                    ) a	                                    
                                     ) tmp   
                                     ORDER BY tmp.RowType ASC
                       ";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    block = ExtractReportDocumentsAppliedBlockFromDataReader(dr, currentUser);
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

        public static ReportDocumentsAppliedBlock ExtractReportDocumentsAppliedBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportDocumentsAppliedBlock reportDocumentsAppliedBlock = new ReportDocumentsAppliedBlock(currentUser);

            reportDocumentsAppliedBlock.MilitaryDepartment = dr["MilitaryDepartment"].ToString();
            reportDocumentsAppliedBlock.CntBy_DocumentsApplied_Male = DBCommon.GetInt(dr["Cnt_DocAppl_M"]);
            reportDocumentsAppliedBlock.CntBy_DocumentsApplied_Female = DBCommon.GetInt(dr["Cnt_DocAppl_F"]);
            reportDocumentsAppliedBlock.CntBy_MedCertFit_Male = DBCommon.GetInt(dr["Cnt_MCFit_M"]);
            reportDocumentsAppliedBlock.CntBy_MedCertFit_Female = DBCommon.GetInt(dr["Cnt_MCFit_F"]);
            reportDocumentsAppliedBlock.CntBy_MedCertFit_NoMilitaryTraining_Male = DBCommon.GetInt(dr["Cnt_MCFit_NoMilTr_M"]);
            reportDocumentsAppliedBlock.CntBy_MedCertFit_NoMilitaryTraining_Female = DBCommon.GetInt(dr["Cnt_MCFit_NoMilTr_F"]);
            reportDocumentsAppliedBlock.CntBy_MedCertFit_Age_Under25 = DBCommon.GetInt(dr["Cnt_MCFit_Age_Under25"]);
            reportDocumentsAppliedBlock.CntBy_MedCertFit_Age_Under30 = DBCommon.GetInt(dr["Cnt_MCFit_Age_Under30"]);
            reportDocumentsAppliedBlock.CntBy_MedCertFit_Age_Over30 = DBCommon.GetInt(dr["Cnt_MCFit_Age_Over30"]);
            reportDocumentsAppliedBlock.RowType = DBCommon.GetInt(dr["RowType"]);

            return reportDocumentsAppliedBlock;
        }
    }
}
