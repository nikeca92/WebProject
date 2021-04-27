using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.DAL
{
    public class PrintReservistsUOBlock
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentNumber { get; set; }
        public string IdentNumberEncrypt { get; set; }
        public string VosNumber { get; set; }
        public string VosText { get; set; }
        public string Position { get; set; }
        public string WorkCompanyName { get; set; }
        public string WorkCompanyNameUpperCase { get; set; }
        public string WorkPosition { get; set; }
        public string WorkPositionNKPD { get; set; }
        public string MilitaryDepartmentName { get; set; }
        public string MilitaryDepartmentUpperCase { get; set; }
        public string PostponeYear { get; set; }
        public string PostponeYearNext { get; set; }
    }

    public static class PrintReservistsUOUtil
    {
        public static PrintReservistsUOBlock GetPrintReservistsUOBlock(int reservistId, User currentUser)
        {
            PrintReservistsUOBlock block = new PrintReservistsUOBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"
                               SELECT b.IME as FirstName,
                                      b.FAM as LastName,
                                      b.EGN as IdentNumber,
                                      SUBSTR(b.EGN, 0, 6) || '****' as IdentNumberEncrypt,
                                      m.MilReportingSpecialityCode as VOSNumber,
                                      m.MilReportingSpecialityName as VOSText,
                                      NVL(s1.PositionTitle, ' ') as Position,
                                      r.CompanyName as Company, 
                                      b1.WorkPositionNKPDID,                                       
                                      md.MilitaryDepartmentName as MilitaryDepartmentName,
                                      rmrs.Postpone_Year as PostponeYear                                                                    
                                        
                               FROM PMIS_RES.Reservists a
                               INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                               INNER JOIN PMIS_ADM.Persons b1 ON b.PersonID = b1.PersonID
                               LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec l ON l.PersonID = b.PersonID AND l.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities m ON m.MilReportSpecialityID = l.MilReportSpecialityID
                               LEFT OUTER JOIN PMIS_ADM.Companies r ON r.CompanyID = b1.WorkCompanyID
                               LEFT OUTER JOIN PMIS_ADM.PersonPositionTitles s on s.PersonID = b1.PersonID AND s.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.PositionTitles s1 on s1.PositionTitleID = s.PositionTitleID 
                               LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses rmrs ON rmrs.ReservistID = a.ReservistID AND rmrs.IsCurrent = 1
                               LEFT OUTER JOIN PMIS_ADM.MILITARYDEPARTMENTS md ON md.MilitaryDepartmentID = rmrs.SourceMilDepartmentID

                               WHERE a.ReservistID = " + reservistId.ToString();

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.IdentNumber = dr["IdentNumber"].ToString();
                    block.IdentNumberEncrypt = dr["IdentNumberEncrypt"].ToString();
                    block.FirstName = dr["FirstName"].ToString();
                    block.LastName = dr["LastName"].ToString();
                    block.VosNumber = dr["VosNumber"].ToString();
                    block.VosText = dr["VosText"].ToString();
                    block.Position = dr["Position"].ToString();
                    block.WorkCompanyName = dr["Company"].ToString();
                    block.WorkCompanyNameUpperCase = dr["Company"].ToString().ToUpper();

                    NKPD workPositionNKPD = null;
                    if (DBCommon.IsInt(dr["WorkPositionNKPDID"]))
                    {
                        workPositionNKPD = NKPDUtil.GetNKPD(DBCommon.GetInt(dr["WorkPositionNKPDID"]), currentUser);
                        block.WorkPositionNKPD = workPositionNKPD.Code;
                        block.WorkPosition = workPositionNKPD.Name;
                    }

                    block.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();
                    block.MilitaryDepartmentUpperCase = dr["MilitaryDepartmentName"].ToString().ToUpper();
                    if (!string.IsNullOrEmpty(dr["PostponeYear"].ToString()))
                    {
                        block.PostponeYear = dr["PostponeYear"].ToString();
                        string postponeYearNext = (Convert.ToInt32(block.PostponeYear) + 1).ToString();
                        block.PostponeYearNext = postponeYearNext;
                    }
                }

                dr.Close();

                return block;
            }
            finally
            {
                conn.Close();
            }

        }
    }
}
