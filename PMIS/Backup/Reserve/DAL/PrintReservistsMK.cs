using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;
using System.Collections;

using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class PrintReservistsMKBlock
    {
        public string Command { get; set; }
        public string CommandNumber { get; set; }
        public string CommandNumberPrintSymbol { get; set; }
        public string CommandNumberPrintSymbol2 { get; set; }
        public string CommandName { get; set; }
        public string CommandSuffix { get; set; }        
        public string AppointmentTime { get; set; }
        public string DeliveryPlace { get; set; }
        public string MilitaryDepartment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentNumber { get; set; }
        public string IdentNumberEncrypt { get; set; }
        public string MilRepSpeciality { get; set; }
        public string MilitaryRank { get; set; }
        public string AppointMilRepSpeciality { get; set; }
        public string AppointMilitaryRank { get; set; }
        public string AppointPosition { get; set; }
        public string PermAddress { get; set; }
        public string CivilEducationName { get; set; }
        public string CivilSchoolSubjectName { get; set; }
        public string CivilGraduateYear { get; set; }
        public string MilEduMilitarySchoolName { get; set; }
        public string MilEduMilitarySchoolSubjectName { get; set; }
        public string MilEduGraduateYear { get; set; }
        public string MilAcadMilitaryAcademyName { get; set; }
        public string MilAcadMilitaryAcademySubjectName { get; set; }
        public string MilAcadGraduateYear { get; set; }
        public string ForeignLanguages { get; set; }
        public string WorkCompanyName { get; set; }
        public string WorkPositionNKPDDisplay { get; set; }
        public string MaritalStatus { get; set; }
        public string ChildCount { get; set; }
        public string SizeClothing { get; set; }
        public string SizeHat { get; set; }
        public string SizeShoes { get; set; }
        public List<MilitaryServiceBlock> MilitaryService { get; set; }

        public PrintReservistsMKBlock()
        {
            MilitaryService = new List<MilitaryServiceBlock>();
        }
    }

    public class MilitaryServiceBlock
    {
        //This is the old text data that was available in the first version
        public string MainTextData { get; set; }

        //2015-01-13: From now on we will support these separate fields too
        public string VaccAnnNum { get; set; }
        public string VaccAnnDateVacAnn { get; set; }
        public string VaccAnnDateWhen { get; set; }
        public string MilitaryCommanderRank { get; set; }
    }

    public static class PrintReservistsMKUtil
    {
        public static PrintReservistsMKBlock GetPrintReservistsMKBlock(int reservistId, User currentUser)
        {
            PrintReservistsMKBlock block = new PrintReservistsMKBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"
                               SELECT c.MilitaryCommandName as Command,
                                      u.NK as CommandNumber,
                                      u.IMEES as CommandName,
                                      f.militarycommandsuffix AS CommandSuffix,
                                      c.AppointmentTime,
                                      b.IME as FirstName,
                                      b.FAM as LastName,
                                      RTRIM(CASE WHEN i.Ime_Obl IS NOT NULL THEN 'обл. ' || i.Ime_Obl || ', ' ELSE '' END || CASE WHEN h.Ime_Obs IS NOT NULL THEN 'общ. ' || h.Ime_Obs || ', ' ELSE '' END || CASE WHEN g.Ime_Nma IS NOT NULL THEN g1.Ime_S || ' ' || g.Ime_Nma || ', ' ELSE '' END || CASE WHEN f.DeliveryPlace IS NOT NULL THEN f.DeliveryPlace || ', ' ELSE '' END, ', ') as DeliveryPlace,
                                      k.MilitaryDepartmentName,
                                      b.EGN as IdentNumber,
                                      SUBSTR(b.EGN, 0, 6) || '****' as IdentNumberEncrypt,
                                      m.MilReportingSpecialityCode,
                                      n.ZVA_IME as MilitaryRankName,
                                      c.MilReportingSpecialityCode as AppointMilRepSpeciality,
                                      c.MilitaryRankName as AppointMilitaryRank,
                                      c.Position as AppointPosition,
                                      RTRIM(CASE WHEN q.Ime_Obl IS NOT NULL THEN 'обл. ' || q.Ime_Obl || ', ' ELSE '' END || CASE WHEN p.Ime_Obs IS NOT NULL THEN 'общ. ' || p.Ime_Obs || ', ' ELSE '' END || CASE WHEN o.Ime_Nma IS NOT NULL THEN o1.Ime_S || ' ' || o.Ime_Nma || ', ' ELSE '' END || CASE WHEN b.ADRES IS NOT NULL THEN b.ADRES || ', ' ELSE '' END, ', ') as PermAddress,
                                      o.PK as PermPostCode,
                                      b.PermSecondPostCode,
                                      r.CompanyName as WorkCompanyName,
                                      b1.WorkPositionNKPDID,
                                      s.MaritalStatusName,
                                      b1.ChildCount,
                                      t.TableValue as SizeClothing,
                                      t1.TableValue as SizeHat,
                                      t2.TableValue as SizeShoes,
                                      mr.PrintSymbol as CommandNumberPrintSymbol,
                                      CASE WHEN d.ReservistReadinessID = 2 
                                           THEN 1 
                                           ELSE 0 
                                      END as HasCommandNumPrintSymbol2
                               FROM PMIS_RES.Reservists a
                               INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                               INNER JOIN PMIS_ADM.Persons b1 ON a.PersonID = b1.PersonID
                               INNER JOIN PMIS_RES.ReservistAppointments c ON c.ReservistId = a.ReservistId AND c.IsCurrent = 1
                               INNER JOIN PMIS_RES.FillReservistsRequest d ON d.ReservistId = a.ReservistId
                               INNER JOIN PMIS_RES.RequestCommandPositions e ON e.RequestCommandPositionID = d.RequestCommandPositionID
                               INNER JOIN PMIS_RES.RequestsCommands f ON f.RequestsCommandID = e.RequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.MilReadiness mr ON mr.MilReadinessID =  f.MilReadinessID 
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA g ON g.Kod_Nma = f.DeliveryCityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM g1 ON g1.KOD_VNM = g.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS h ON h.kod_obs = g.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL i ON i.Kod_Obl = g.kod_obl
                               INNER JOIN PMIS_RES.ReservistMilRepStatuses j ON j.ReservistID = a.ReservistID AND j.IsCurrent = 1
                               INNER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = j.SourceMilDepartmentID
                               LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec l ON l.PersonID = a.PersonID AND l.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities m ON m.MilReportSpecialityID = l.MilReportSpecialityID
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA n ON n.ZVA_KOD = b.KOD_ZVA
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA o ON o.Kod_Nma = b.KOD_NMA_MJ
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM o1 ON o1.KOD_VNM = o.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS p ON p.kod_obs = o.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL q ON q.Kod_Obl = o.kod_obl
                               LEFT OUTER JOIN PMIS_ADM.Companies r ON r.CompanyID = b1.WorkCompanyID
                               LEFT OUTER JOIN PMIS_ADM.MaritalStatuses s ON s.MaritalStatusKey = b.KOD_SPO
                               LEFT OUTER JOIN PMIS_RES.GTable t ON t.TableName = 'SizeClothing' AND t.TableKey = b1.SizeClothingID
                               LEFT OUTER JOIN PMIS_RES.GTable t1 ON t1.TableName = 'SizeHat' AND t1.TableKey = b1.SizeHatID
                               LEFT OUTER JOIN PMIS_RES.GTable t2 ON t2.TableName = 'SizeShoes' AND t2.TableKey = b1.SizeShoesID
                               LEFT OUTER JOIN UKAZ_OWNER.VVR u ON u.KOD_VVR = f.MILITARYCOMMANDID
                               WHERE a.ReservistID = " + reservistId.ToString();

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.Command = dr["Command"].ToString();
                    block.CommandNumber = dr["CommandNumber"].ToString();
                    block.CommandNumberPrintSymbol = dr["CommandNumberPrintSymbol"].ToString();

                    if (DBCommon.GetInt(dr["HasCommandNumPrintSymbol2"]) == 1)
                    {
                        block.CommandNumberPrintSymbol2 = Config.GetWebSetting("PrintReservistsCommandNumPrintSymbol2");
                    }
                                            
                    block.CommandName = dr["CommandName"].ToString();
                    block.CommandSuffix = dr["CommandSuffix"] is string ? dr["CommandSuffix"].ToString() : "";
                    block.FirstName = dr["FirstName"].ToString();
                    block.LastName = dr["LastName"].ToString();
                    block.AppointmentTime = (DBCommon.IsDecimal(dr["AppointmentTime"]) ? DBCommon.GetDecimal(dr["AppointmentTime"]).ToString() : "");
                    block.DeliveryPlace = dr["DeliveryPlace"].ToString();
                    block.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    block.IdentNumber = dr["IdentNumber"].ToString();
                    block.IdentNumberEncrypt = dr["IdentNumberEncrypt"].ToString();
                    block.MilRepSpeciality = dr["MilReportingSpecialityCode"].ToString();
                    block.MilitaryRank = dr["MilitaryRankName"].ToString();
                    block.AppointMilRepSpeciality = dr["AppointMilRepSpeciality"].ToString();
                    block.AppointMilitaryRank = dr["AppointMilitaryRank"].ToString();
                    block.AppointPosition = dr["AppointPosition"].ToString();
                    block.PermAddress = dr["PermAddress"].ToString();

                    string permPostCode = dr["PermPostCode"].ToString();
                    string permSecondPostCode = dr["PermSecondPostCode"].ToString();

                    if (!String.IsNullOrEmpty(permSecondPostCode))
                    {
                        block.PermAddress += (String.IsNullOrEmpty(block.PermAddress) ? "" : ", ПК ") + permSecondPostCode;
                    }
                    else if (!String.IsNullOrEmpty(permPostCode) && permPostCode != "0")
                    {
                        block.PermAddress += (String.IsNullOrEmpty(block.PermAddress) ? "" : ", ПК ") + permPostCode;
                    }

                    block.WorkCompanyName = dr["WorkCompanyName"].ToString();

                    NKPD workPositionNKPD = null;
                    if (DBCommon.IsInt(dr["WorkPositionNKPDID"]))
                    {
                        workPositionNKPD = NKPDUtil.GetNKPD(DBCommon.GetInt(dr["WorkPositionNKPDID"]), currentUser);
                        block.WorkPositionNKPDDisplay = workPositionNKPD.CodeAndNameDisplay;
                    }

                    if (!String.IsNullOrEmpty(block.WorkCompanyName) && !String.IsNullOrEmpty(block.WorkPositionNKPDDisplay))
                    {
                        block.WorkCompanyName += ",";
                    }

                    block.MaritalStatus = dr["MaritalStatusName"].ToString();
                    block.ChildCount = (DBCommon.IsInt(dr["ChildCount"]) ? DBCommon.GetInt(dr["ChildCount"]).ToString() : "");
                    block.SizeClothing = dr["SizeClothing"].ToString();
                    block.SizeHat = dr["SizeHat"].ToString();
                    block.SizeShoes = dr["SizeShoes"].ToString();
                }

                dr.Close();


                SQL = @"SELECT d.OBR_IME as EducationName,
                               e.SPE_IME as SchoolSubjectName,
                               c.OBRG_KOGA as GraduateYear
                        FROM PMIS_RES.Reservists a
                        INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                        INNER JOIN VS_OWNER.VS_OBRG c ON c.OBRG_EGNLS = b.EGN
                        INNER JOIN VS_OWNER.KLV_OBR d ON d.OBR_KOD = c.OBRG_KOD
                        INNER JOIN VS_OWNER.KLV_SPE e ON e.SPE_KOD = c.OBRG_SPEKOD
                        WHERE a.ReservistID = " + reservistId.ToString() + @"
                        ORDER BY c.OBRG_KOGA DESC, c.OBRG_KOD DESC";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.CivilEducationName = dr["EducationName"].ToString();
                    block.CivilSchoolSubjectName = dr["SchoolSubjectName"].ToString();
                    block.CivilGraduateYear = dr["GraduateYear"].ToString();

                    if(!String.IsNullOrEmpty(block.CivilSchoolSubjectName) && !String.IsNullOrEmpty(block.CivilGraduateYear))
                    {
                        block.CivilSchoolSubjectName += ",";
                    }

                    if (!String.IsNullOrEmpty(block.CivilGraduateYear))
                        block.CivilGraduateYear += " г.";
                }

                dr.Close();



                SQL = @"SELECT d.VVU_IME as MilitarySchoolName,
                               e.VSP_IME as MilitarySchoolSubjectName,
                               c.OBRV_KOGA as GraduateYear
                        FROM PMIS_RES.Reservists a
                        INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                        INNER JOIN VS_OWNER.VS_OBRV c ON c.OBRV_EGNLS = b.EGN
                        INNER JOIN VS_OWNER.KLV_VVU d ON d.VVU_KOD = c.OBRV_VVUKOD
                        INNER JOIN VS_OWNER.KLV_VSP e ON e.VSP_KOD = c.OBRV_VSPKOD
                        WHERE a.ReservistID = " + reservistId.ToString() + @"
                        ORDER BY c.OBRV_KOGA DESC, c.OBRV_VVOKOD DESC, c.OBRV_VSPKOD DESC, c.OBRV_RVUKOD DESC";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.MilEduMilitarySchoolName = dr["MilitarySchoolName"].ToString();
                    block.MilEduMilitarySchoolSubjectName = dr["MilitarySchoolSubjectName"].ToString();
                    block.MilEduGraduateYear = dr["GraduateYear"].ToString();

                    if (!String.IsNullOrEmpty(block.MilEduMilitarySchoolName) && !String.IsNullOrEmpty(block.MilEduMilitarySchoolSubjectName) &&
                        !String.IsNullOrEmpty(block.MilEduGraduateYear))
                    {
                        block.MilEduMilitarySchoolName += ",";
                        block.MilEduMilitarySchoolSubjectName += ",";
                    }

                    if (!String.IsNullOrEmpty(block.MilEduGraduateYear))
                        block.MilEduGraduateYear += " г.";
                }

                dr.Close();


                SQL = @"SELECT d.VVA_IME as MilitaryAcademyName,
                               e.SVA_IME as MilitaryAcademySubjectName,
                               c.OBRVA_KOGA AS GraduateYear
                        FROM PMIS_RES.Reservists a
                        INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                        INNER JOIN VS_OWNER.VS_OBRVA c ON c.OBRVA_EGNLS = b.EGN
                        INNER JOIN VS_OWNER.KLV_VVA d ON d.VVA_KOD = c.OBRVA_VVAKOD
                        LEFT OUTER JOIN VS_OWNER.KLV_SVA e ON e.SVA_KOD = c.OBRVA_SVAKOD
                        WHERE a.ReservistID = " + reservistId.ToString() + @"
                        ORDER BY c.OBRVA_KOGA DESC, c.OBRVA_SVAKOD DESC, c.OBRVA_GSTKOD DESC";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.MilAcadMilitaryAcademyName = dr["MilitaryAcademyName"].ToString();
                    block.MilAcadMilitaryAcademySubjectName = dr["MilitaryAcademySubjectName"].ToString();
                    block.MilAcadGraduateYear = dr["GraduateYear"].ToString();

                    if (!String.IsNullOrEmpty(block.MilAcadMilitaryAcademyName) && 
                        (!String.IsNullOrEmpty(block.MilAcadMilitaryAcademySubjectName) || !String.IsNullOrEmpty(block.MilAcadGraduateYear))
                       )
                    {
                        block.MilAcadMilitaryAcademyName += ",";
                    }

                    if (!String.IsNullOrEmpty(block.MilAcadMilitaryAcademySubjectName) && !String.IsNullOrEmpty(block.MilAcadGraduateYear))
                    {
                        block.MilAcadMilitaryAcademySubjectName += ",";
                    }

                    if (!String.IsNullOrEmpty(block.MilAcadGraduateYear))
                        block.MilAcadGraduateYear += " г.";
                }

                dr.Close();


                SQL = @"SELECT d.EZK_IME as LanguageName,
                               e.LanguageLevelOfKnowledgeName
                        FROM PMIS_RES.Reservists a
                        INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                        INNER JOIN VS_OWNER.VS_EZIK c ON c.EZIK_EGNLS = b.EGN
                        INNER JOIN VS_OWNER.KLV_EZK d ON d.EZK_KOD = c.EZIK_EZKKOD
                        INNER JOIN PMIS_ADM.LanguageLevelOfKnowledge e ON e.LanguageLevelOfKnowledgeKey = c.EZIK_SVEKOD
                        WHERE a.ReservistID = " + reservistId.ToString() + @"
                        ORDER BY c.EZIKID ";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string languageName = dr["LanguageName"].ToString();
                    string languageLevelOfKnowledgeName = dr["LanguageLevelOfKnowledgeName"].ToString();

                    block.ForeignLanguages += (String.IsNullOrEmpty(block.ForeignLanguages) ?  "" : "; ") +
                        languageName + ", " + languageLevelOfKnowledgeName;
                }

                dr.Close();



                SQL = @"SELECT c.ARDLO_VPN as MilitaryUnitVpn,
                               c.ARDLO_IMEPOD as MilitaryUnitName,
                               c.ARDLO_TEXTDL as PositionName,
                               c.ARDLO_KOGA as VaccAnnDateWhen,
                               c.ARDLO_ZPVD AS VaccAnnNum,
                               c.ARDLO_IZD AS VaccAnnDateVacAnn,
                               d.SPZ_IME AS MilitaryCommanderRank
                        FROM PMIS_RES.Reservists a
                        INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                        INNER JOIN VS_OWNER.VS_AR_DLO c ON c.ARDLO_EGNLS = b.EGN
                        LEFT OUTER JOIN VS_OWNER.KLV_SPZ d ON c.ARDLO_SPZKOD = d.SPZ_KOD
                        WHERE a.ReservistID = " + reservistId.ToString() + @"
                        ORDER BY c.ARDLO_IZD DESC, c.ARDLO_ZPVD";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string militaryUnitVpn = dr["MilitaryUnitVpn"].ToString();
                    string militaryUnitName = dr["MilitaryUnitName"].ToString();
                    string positionName = dr["PositionName"].ToString();
                    DateTime VaccAnnDateWhen = (DateTime)(dr["VaccAnnDateWhen"]);
                    string vaccAnnNum = dr["VaccAnnNum"].ToString();
                    DateTime vaccAnnDateVacAnn = (DateTime)(dr["VaccAnnDateVacAnn"]);
                    string militaryCommanderRank = dr["MilitaryCommanderRank"].ToString();

                    string militaryService = militaryUnitVpn + " " + militaryUnitName + ", " + positionName + ", в сила от " + CommonFunctions.FormatDate(VaccAnnDateWhen);

                    MilitaryServiceBlock milServiceBlock = new MilitaryServiceBlock();
                    milServiceBlock.MainTextData = militaryService;
                    milServiceBlock.VaccAnnNum = vaccAnnNum;
                    milServiceBlock.VaccAnnDateWhen = CommonFunctions.FormatDate(VaccAnnDateWhen);
                    milServiceBlock.VaccAnnDateVacAnn = CommonFunctions.FormatDate(vaccAnnDateVacAnn);
                    milServiceBlock.MilitaryCommanderRank = militaryCommanderRank;

                    block.MilitaryService.Add(milServiceBlock);
                }

                dr.Close();



                SQL = @"SELECT b.MilitaryUnit,
                               b.Position,
                               b.DateFrom,
                               b.DateTo
                        FROM PMIS_RES.Reservists a
                        INNER JOIN PMIS_RES.PersonConscription b ON a.PersonID = b.PersonID
                        WHERE a.ReservistID = " + reservistId.ToString() + @"
                        ORDER BY b.DateFrom DESC, b.DateTo";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string militaryUnit = dr["MilitaryUnit"].ToString();
                    string position = dr["Position"].ToString();
                    DateTime dateFrom = (DateTime)(dr["DateFrom"]);
                    DateTime dateTo = (DateTime)(dr["DateTo"]);

                    string militaryService = militaryUnit + ", " + position + ", от " + CommonFunctions.FormatDate(dateFrom) + " до " + CommonFunctions.FormatDate(dateTo);

                    MilitaryServiceBlock milServiceBlock = new MilitaryServiceBlock();
                    milServiceBlock.MainTextData = militaryService;
                    milServiceBlock.VaccAnnNum = "";
                    milServiceBlock.VaccAnnDateWhen = CommonFunctions.FormatDate(dateFrom);
                    milServiceBlock.VaccAnnDateVacAnn = "";
                    milServiceBlock.MilitaryCommanderRank = "";

                    block.MilitaryService.Add(milServiceBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return block;
        }
    }
}