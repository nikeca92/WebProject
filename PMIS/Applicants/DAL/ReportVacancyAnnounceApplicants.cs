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
    public class ReportVacancyAnnounceApplicantsFilter
    {
        private int? vacancyAnnounceId;
        public int? VacancyAnnounceId
        {
            get { return vacancyAnnounceId; }
            set { vacancyAnnounceId = value; }
        }
        
        private int? responsibleMilitaryUnitId;
        public int? ResponsibleMilitaryUnitId
        {
            get { return responsibleMilitaryUnitId; }
            set { responsibleMilitaryUnitId = value; }
        }
    }

    public class ReportVacancyAnnounceApplicantsBlock : BaseDbObject
    {
        public int CntBy_Age_Male_Under25 { get; set; }
        public int CntBy_Age_Male_Under30 { get; set; }
        public int CntBy_Age_Male_Over35 { get; set; }
        public int CntBy_Age_Female_Under25 { get; set; }
        public int CntBy_Age_Female_Under30 { get; set; }
        public int CntBy_Age_Female_Over35 { get; set; }

        public int CntBy_MilitaryService_Employed { get; set; }
        
        public int CntBy_Education_Male_UniversityDegree { get; set; }
        public int CntBy_Education_Male_HighSchoolDegree { get; set; }
        public int CntBy_Education_Female_UniversityDegree { get; set; }
        public int CntBy_Education_Female_HighSchoolDegree { get; set; }    

        public ReportVacancyAnnounceApplicantsBlock(User user) : base(user)
        {

        }
    }

    public class ReportVacancyAnnounceApplicantsUtil
    {
        //This method get List of Reports
        public static List<ReportVacancyAnnounceApplicantsBlock> GetReportVacancyAnnounceApplicantsSearch(ReportVacancyAnnounceApplicantsFilter filter, User currentUser)
        {
            ReportVacancyAnnounceApplicantsBlock block;
            List<ReportVacancyAnnounceApplicantsBlock> listBlocks = new List<ReportVacancyAnnounceApplicantsBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.VacancyAnnounceID = " + filter.VacancyAnnounceId.Value + " ";
                }

                if (filter.ResponsibleMilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.ResponsibleMilitaryUnitID = " + filter.ResponsibleMilitaryUnitId.Value + " ";
                }

                where = (where == "" ? "" : " AND ") + where;

                              
                string SQL = @"SELECT * 
                               FROM (
                                        SELECT  --a.personid,

                                                NVL(SUM(CASE WHEN a.Age > 0 AND a.Age <= 25 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END), 0) as Cnt_Age_M_Under25,
                                                NVL(SUM(CASE WHEN a.Age > 25 AND a.Age <= 30 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END), 0) as Cnt_Age_M_Under30,
                                                NVL(SUM(CASE WHEN a.Age > 30 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END), 0) as Cnt_Age_M_Over35,
                                                NVL(SUM(CASE WHEN a.Age > 0 AND a.Age <= 25 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END), 0) as Cnt_Age_F_Under25,
                                                NVL(SUM(CASE WHEN a.Age > 25 AND a.Age <= 30 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END), 0) as Cnt_Age_F_Under30,
                                                NVL(SUM(CASE WHEN a.Age > 30 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END), 0) as Cnt_Age_F_Over35,
                                                NVL(SUM(CASE WHEN a.MilitaryService = 1 THEN 1 ELSE 0 END), 0) as Cnt_MilServ_Employed,
                                                NVL(SUM(CASE WHEN a.UniversityEdu = 1 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END), 0) as Cnt_Edu_M_University,
                                                NVL(SUM(CASE WHEN a.UniversityEdu = 0 AND a.HighSchoolEdu = 1 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END), 0) as Cnt_Edu_M_HighSchool,
                                                NVL(SUM(CASE WHEN a.UniversityEdu = 1 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END), 0) as Cnt_Edu_F_University, 
                                                NVL(SUM(CASE WHEN a.UniversityEdu = 0 AND a.HighSchoolEdu = 1 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END), 0) as Cnt_Edu_F_HighSchool 
                                        FROM
                                        (
                                           SELECT DISTINCT b.personid,
                                                           PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age,
                                                           g.GenderName,
                                                           NVL(f.MilitaryService, 2) as MilitaryService,
                                                           NVL((SELECT CASE WHEN subp.OBRG_EGNLS > 0 THEN 1 ELSE 0 END
                                                                FROM VS_OWNER.VS_OBRG subp
                                                                WHERE subp.OBRG_EGNLS = b.EGN AND 
                                                                      subp.OBRG_KOD IN ('1', '2')
                                                                GROUP BY subp.OBRG_EGNLS
                                                                ), 0) UniversityEdu,
                                                                
                                                           NVL((SELECT CASE WHEN subp.OBRG_EGNLS > 0 THEN 1 ELSE 0 END
                                                                FROM VS_OWNER.VS_OBRG subp
                                                                 WHERE subp.OBRG_EGNLS = b.EGN AND 
                                                                       subp.OBRG_KOD IN ('3', '4', '5', '6', '7', '8', '9')
                                                                 GROUP BY subp.OBRG_EGNLS
                                                                 ), 0) HighSchoolEdu
                                           FROM PMIS_APPL.Applicants a
                                           INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                           INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
                                           INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
                                           INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
                                           LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                           WHERE 1 = 1 AND 
                                                 c.applicantstatusid IS NOT NULL " + where + @"                                                   
                                        ) a
                                        --GROUP BY a.personid                                                
                                     ) tmp                                 
                       ";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    block = ExtractReportVacancyAnnounceApplicantsBlockFromDataReader(dr, currentUser);
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

        public static ReportVacancyAnnounceApplicantsBlock ExtractReportVacancyAnnounceApplicantsBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportVacancyAnnounceApplicantsBlock reportVacancyAnnounceApplicantsBlock = new ReportVacancyAnnounceApplicantsBlock(currentUser);

            reportVacancyAnnounceApplicantsBlock.CntBy_Age_Male_Under25 = DBCommon.GetInt(dr["Cnt_Age_M_Under25"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Age_Male_Under30 = DBCommon.GetInt(dr["Cnt_Age_M_Under30"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Age_Male_Over35 = DBCommon.GetInt(dr["Cnt_Age_M_Over35"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Age_Female_Under25 = DBCommon.GetInt(dr["Cnt_Age_F_Under25"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Age_Female_Under30 = DBCommon.GetInt(dr["Cnt_Age_F_Under30"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Age_Female_Over35 = DBCommon.GetInt(dr["Cnt_Age_F_Over35"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_MilitaryService_Employed = DBCommon.GetInt(dr["Cnt_MilServ_Employed"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Education_Male_UniversityDegree = DBCommon.GetInt(dr["Cnt_Edu_M_University"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Education_Male_HighSchoolDegree = DBCommon.GetInt(dr["Cnt_Edu_M_HighSchool"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Education_Female_UniversityDegree = DBCommon.GetInt(dr["Cnt_Edu_F_University"]);
            reportVacancyAnnounceApplicantsBlock.CntBy_Education_Female_HighSchoolDegree = DBCommon.GetInt(dr["Cnt_Edu_F_HighSchool"]);
                       
            return reportVacancyAnnounceApplicantsBlock;
        }
    }
}
