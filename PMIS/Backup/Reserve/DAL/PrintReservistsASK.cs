using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.DAL
{
    public class PrintReservistsASKBlock
    {
        public string Rank { get; set; }
        public string RankShort { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentNumber { get; set; }
        public string IdentNumberEncrypt { get; set; }
        public string MilitaryForceSort { get; set; }
        public string VosNumber { get; set; }
        public string VosText { get; set; }
        public string Position { get; set; }
        public string PermRegion { get; set; }
        public string PermCity { get; set; }
        public string PermAddress { get; set; }
        public string CurrRegion { get; set; }
        public string CurrCity { get; set; }
        public string CurrAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string Email { get; set; }
        public string CommandNumber { get; set; }
        public string CommandNumberPrintSymbol { get; set; }
        public string CommandNumberPrintSymbol2 { get; set; }
        public string CommandSuffix { get; set; }
        public string CommandVPN { get; set; }
        public string ContractNumber { get; set; }
        public string ContractVPN { get; set; }
        public string ContractDuration { get; set; }
        public string WorkCompanyName { get; set; }
        public string CompanyCityAddressPostCode { get; set; }
        public string CompanyPhone { get; set; }
        public string Section { get; set; }
        public string OtherInfo { get; set; }
        public string RecordOfServiceSeries { get; set; }
        public string RecordOfServiceNumber { get; set; }
        public string CivilEducationName { get; set; }
        public string CivilSchoolSubjectName { get; set; }
        public string CivilGraduateYear { get; set; }
        public string MilEduMilitarySchoolName { get; set; }
        public string MilEduMilitarySchoolSubjectName { get; set; }
        public string MilEduGraduateYear { get; set; }
        public string MilAcadMilitaryAcademyName { get; set; }
        public string MilAcadMilitaryAcademySubjectName { get; set; }
        public string MilAcadGraduateYear { get; set; }
        public string MilAcadDurationYear { get; set; }
        public string ForeignLanguages { get; set; }
        public string MilitaryDepartmentName { get; set; }
        public string MilitaryDepartmentDate { get; set; }
        public string TemporaryRemovedDate { get; set; }
        public string TemporaryRemovedReason { get; set; }
        public string RemovedDate { get; set; }
        public string RemovedReason { get; set; }

        public string ScientificTitle { get; set; }
        public string ScientificTitleYear { get; set; }
        public string BirthCity { get; set; }
        public string BirthRegion { get; set; }
        public string BirthDate { get; set; }
        public string MilitaryCommandName { get; set; }
        public string AppointmentTime { get; set; }
        public string ChildCount { get; set; }
        public string MaritalStatusName { get; set; }
        public string PersonHeight { get; set; }
        public string SizeClothing { get; set; }
        public string SizeHat { get; set; }
        public string SizeShoes { get; set; }
        public string ClInformationAccLevelBg { get; set; }
        public string ClInformationAccLevelBgDate { get; set; }
        public List<MilitaryServiceBlock> MilitaryService { get; set; }
        public List<MilitaryRankBlock> MilitaryRank { get; set; }
        public List<VoluntaryReserveAnnexBlock> VoluntaryReserveAnnex { get; set; }

        public PrintReservistsASKBlock()
        {
            MilitaryService = new List<MilitaryServiceBlock>();
            MilitaryRank = new List<MilitaryRankBlock>();
            VoluntaryReserveAnnex = new List<VoluntaryReserveAnnexBlock>();
        }
    }

    public class MilitaryServiceBlock
    {
        public string MainTextData { get; set; }
        public string VaccAnnNum { get; set; }
        public string VaccAnnDateVacAnn { get; set; }
        public string VaccAnnDateWhen { get; set; }
        public string MilitaryCommanderRank { get; set; }
    }

    public class MilitaryRankBlock
    {
        public string MilitaryRankName { get; set; }
        public string VacAnn { get; set; }
        public string MilitaryCommanderRankName { get; set; }
        public string DateArchive { get; set; }
        public string DateWhen { get; set; }
    }

    public class VoluntaryReserveAnnexBlock
    {
        public string AnnexNumber { get; set; }
        public string AnnexDate { get; set; }
        public string AnnexDurationMonths { get; set; }
        public string AnnexExpireDate { get; set; }
    }

    public static class PrintReservistsASKUtil
    {
        public static PrintReservistsASKBlock GetPrintReservistsASKBlock(int reservistId, User currentUser)
        {
            PrintReservistsASKBlock block = new PrintReservistsASKBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"
                               SELECT n.ZVA_IME as Rank,
                                      n.ZVA_IMEES as RankShort,
                                      b.IME as FirstName,
                                      b.FAM as LastName,
                                      b.EGN as IdentNumber,
                                      SUBSTR(b.EGN, 0, 6) || '****' as IdentNumberEncrypt,
                                      h.BirthDate,
                                      kr.ROD_IME as MilitaryForceSort,
                                      m.MilReportingSpecialityCode as VOSNumber,
                                      m.MilReportingSpecialityName as VOSText,
                                      NVL(s1.PositionTitle, ' ') as Position,
                                      NVL(q.Ime_Obl,' ') as PermRegion,
                                      NVL(p.Ime_Obs, ' ') as PermMunicipality,
                                      NVL(o.Ime_Nma, ' ') as PermCity,
                                      NVL(permDis.DistrictName, ' ') as PermDistrict,
                                      NVL(b.ADRES, ' ') as PermAddress,
                                      NVL(b.PermSecondPostCode, o.pk) as PermPostcode,
                                      NVL(z.Ime_Obl,' ') as CurrRegion,
                                      NVL(w.Ime_Obs, ' ') as CurrMunicipality,
                                      NVL(v.Ime_Nma, ' ') as CurrCity,
                                      NVL(currDis.DistrictName, ' ') as CurrDistrict,
                                      NVL(b.CurrAddress, ' ') as CurrAddress,
                                      NVL(b.PresSecondPostCode, v.pk) as CurrPostcode,
                                      NVL(b.tel, 0) as TelephoneNumber,
                                      NVL(b1.MobilePhone, 0) as MobilePhoneNumber,
                                      NVL(b1.BusinessPhone, 0) as WorkPhoneNumber,
                                      NVL(b1.Email, '') as Email,
                                      u.NK as CommandNumber,
                                      mr.PrintSymbol as CommandNumberPrintSymbol,
                                      CASE WHEN d.ReservistReadinessID = 2 
                                           THEN 1 
                                           ELSE 0 
                                      END as HasCommandNumPrintSymbol2,
                                      f.militarycommandsuffix AS CommandSuffix,
                                      mi.VPN || ' ' || mi.IMEES as CommandVPN,
                                      dr.Voluntary_ContractNumber as ContractNumber,
                                      drvpn.VPN || ' ' || drvpn.IMEES as ContractVPN,
                                      dr.Voluntary_DurationMonths as ContractDuration,
                                      r.CompanyName as Company,
                                      RTRIM(CASE WHEN robl.Ime_Obl IS NOT NULL THEN robl.Ime_Obl || ', ' ELSE '' END || CASE WHEN robs.Ime_Obs IS NOT NULL THEN robs.Ime_Obs || ', ' ELSE '' END || CASE WHEN rnma.Ime_Nma IS NOT NULL THEN rnma.Ime_Nma || ', ' ELSE '' END || CASE WHEN r.Address IS NOT NULL THEN r.Address || ', ' ELSE '' END, ', ') as CompanyAddress,
                                      r.PostCode as CompanyPostCode,
                                      r.Phone as CompanyPhone,
                                      a.Section,
                                      b.TXT as OtherInfo,
                                      b1.RecordOfServiceSeries,
                                      b1.RecordOfServiceNumber,
                                        
                                      md.MilitaryDepartmentName as MilitaryDepartmentName,
                                      rmrs.EnrolDate as MilitaryDepartmentDate,

                                      CASE WHEN rmrss.MilitaryReportStatusKey = 'TEMPORARY_REMOVED'
                                           THEN rmrs.temporaryremoved_date 
                                           ELSE NULL
                                      END as TemporaryRemovedDate,
                                      
                                      CASE WHEN rmrss.MilitaryReportStatusKey = 'TEMPORARY_REMOVED'
                                           THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'MilRepStat_ТemporaryRemovedReasons' AND TableKey = rmrs.temporaryremoved_reasonid)
                                           ELSE ''
                                      END as TemporaryRemovedReason,
                                      
                                      CASE WHEN rmrss.MilitaryReportStatusKey = 'REMOVED'
                                           THEN rmrs.removed_date 
                                           WHEN rmrss.MilitaryReportStatusKey = 'DISCHARGED'
                                           THEN rmrs.EnrolDate
                                           ELSE NULL
                                      END as RemovedDate,
                                      
                                      CASE WHEN rmrss.MilitaryReportStatusKey = 'REMOVED'
                                           THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'MilRepStat_RemovedReasons' AND TableKey = rmrs.removed_reasonid)
                                           WHEN rmrss.MilitaryReportStatusKey = 'DISCHARGED'
                                           THEN 'Отчислен'
                                           ELSE ''
                                      END as RemovedReason,
                                      
                                      c.Djj_Ime as BirthCountryName,
                                      b1.BirthCityIfAbroad as BirthCityAbroad,
                                      g.Ime_Nma as BirthCity,
                                      h.Ime_Obl as BirthRegion,
                                      b1.ChildCount,
                                      ms.MaritalStatusName,
                                      b1.PersonHeight,
                                      t.TableValue as SizeClothing,
                                      t1.TableValue as SizeHat,
                                      t2.TableValue as SizeShoes,
                                      j.MilitaryCommandName,
                                      j.AppointmentTime, 
                                      y.Rv_Meaning as ClInformationAccLevelBg,
                                      b1.ClInformationAccLevelBgExpDate as ClInformationAccLevelBgDate                    
                                   
                               FROM PMIS_RES.Reservists a
                               INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                               INNER JOIN PMIS_ADM.Persons b1 ON a.PersonID = b1.PersonID
                               LEFT OUTER JOIN PMIS_RES.FillReservistsRequest d ON d.ReservistId = a.ReservistId
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositions e ON e.RequestCommandPositionID = d.RequestCommandPositionID
                               LEFT OUTER JOIN PMIS_RES.RequestsCommands f ON f.RequestsCommandID = e.RequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.MilReadiness mr ON mr.MilReadinessID =  f.MilReadinessID 
                               LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec l ON l.PersonID = a.PersonID AND l.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities m ON m.MilReportSpecialityID = l.MilReportSpecialityID
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA n ON n.ZVA_KOD = b.KOD_ZVA
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA o ON o.Kod_Nma = b.KOD_NMA_MJ
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS p ON p.kod_obs = o.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL q ON q.Kod_Obl = o.kod_obl
                               LEFT OUTER JOIN UKAZ_OWNER.Districts permDis on permDis.DistrictID = b.PermAddrDistrictID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA v on v.kod_nma = b.CurrAddrCityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS w on w.kod_obs = v.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL z on z.kod_obl = v.kod_obl
                               LEFT OUTER JOIN UKAZ_OWNER.Districts currDis on currDis.DistrictID = b.CurrAddrDistrictID
                               LEFT OUTER JOIN PMIS_ADM.Companies r ON r.CompanyID = b1.WorkCompanyID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA rnma ON rnma.Kod_Nma = r.CityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS robs ON rnma.kod_obs = robs.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL robl ON rnma.Kod_Obl = robl.kod_obl
                               LEFT OUTER JOIN UKAZ_OWNER.VVR u ON u.KOD_VVR = f.MILITARYCOMMANDID
                               LEFT OUTER JOIN PMIS_ADM.PersonPositionTitles s on s.PersonID = b1.PersonID AND s.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.PositionTitles s1 on s1.PositionTitleID = s.PositionTitleID
                               LEFT OUTER JOIN VS_OWNER.KLV_ROD kr on kr.RODID = m.MilitaryForceSortID
                               LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses drs ON drs.MilitaryReportStatusKey = 'VOLUNTARY_RESERVE'
                               LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses dr ON dr.ReservistID = a.ReservistID AND dr.IsCurrent = 1 AND dr.MilitaryReportStatusID = drs.MilitaryReportStatusID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR drvpn ON drvpn.KOD_MIR = dr.Voluntary_FulfilPlaceID
                               
                               LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses rmrs ON rmrs.ReservistID = a.ReservistID AND rmrs.IsCurrent = 1 
                               LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses rmrss ON rmrss.MilitaryReportStatusID = rmrs.MilitaryReportStatusID
                               LEFT OUTER JOIN PMIS_ADM.MILITARYDEPARTMENTS md ON md.MilitaryDepartmentID = rmrs.SourceMilDepartmentID

                               LEFT OUTER JOIN VS_OWNER.KLV_DJJ c ON b1.BirthCountryId = c.Djj_Kod
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA g ON b.Kod_Nma_Mr = g.Kod_Nma
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL h ON g.Kod_Obl = h.Kod_Obl 
                               
                               INNER JOIN (SELECT EGN as EGN, PMIS_ADM.COMMONFUNCTIONS.GetBirthDateFromEGN(EGN) as BirthDate FROM VS_OWNER.VS_LS) h ON h.EGN = b.EGN
                               LEFT OUTER JOIN PMIS_RES.ReservistAppointments j ON j.ReservistId = a.ReservistId AND j.IsCurrent = 1
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA x ON x.Kod_Nma = f.DeliveryCityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM x1 ON x1.KOD_VNM = x.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS x2 ON x2.kod_obs = x.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL x3 ON x3.Kod_Obl = x.kod_obl
                               LEFT OUTER JOIN PMIS_ADM.MaritalStatuses ms ON b.Kod_Spo = ms.MaritalStatusKey
                               LEFT OUTER JOIN PMIS_RES.GTable t ON t.TableName = 'SizeClothing' AND t.TableKey = b1.SizeClothingID
                               LEFT OUTER JOIN PMIS_RES.GTable t1 ON t1.TableName = 'SizeHat' AND t1.TableKey = b1.SizeHatID
                               LEFT OUTER JOIN PMIS_RES.GTable t2 ON t2.TableName = 'SizeShoes' AND t2.TableKey = b1.SizeShoesID
                               LEFT OUTER JOIN VS_OWNER.CG_REF_CODES y ON b1.ClInformationAccLevelBg = y.Rv_Low_Value and y.Rv_Domain = 'NIVO_KL_INF'
                                            
                               LEFT OUTER JOIN PMIS_RES.EquipmentReservistsRequests err ON f.equipmentreservistsrequestid = err.equipmentreservistsrequestid
                               LEFT OUTER JOIN PMIS_RES.EquipWithResRequestsStatuses ewrs ON err.EquipWithResRequestsStatusID = ewrs.EquipWithResRequestsStatusID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR mi ON err.MilitaryUnitID = mi.KOD_MIR

                               WHERE a.ReservistID = " + reservistId.ToString();

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.Rank = dr["Rank"].ToString();
                    block.RankShort = dr["RankShort"].ToString();
                    block.FirstName = dr["FirstName"].ToString();
                    block.LastName = dr["LastName"].ToString();
                    block.IdentNumber = dr["IdentNumber"].ToString();
                    block.IdentNumberEncrypt = dr["IdentNumberEncrypt"].ToString();
                    block.MilitaryForceSort = dr["MilitaryForceSort"].ToString();
                    block.VosNumber = dr["VosNumber"].ToString();
                    block.VosText = dr["VosText"].ToString();
                    block.Position = dr["Position"].ToString();
                    block.PermRegion = dr["PermRegion"].ToString();
                    block.PermCity = dr["PermCity"].ToString();
                    block.PermAddress = dr["PermAddress"].ToString();
                    block.CurrRegion = dr["CurrRegion"].ToString();
                    block.CurrCity = dr["CurrCity"].ToString();
                    block.CurrAddress = dr["CurrAddress"].ToString();
                    block.TelephoneNumber = dr["TelephoneNumber"].ToString();
                    if (block.TelephoneNumber == "0")
                        block.TelephoneNumber = "";
                    block.MobilePhoneNumber = dr["MobilephoneNumber"].ToString();
                    if (block.MobilePhoneNumber == "0")
                        block.MobilePhoneNumber = "";
                    block.WorkPhoneNumber = dr["WorkPhoneNumber"].ToString();
                    if (block.WorkPhoneNumber == "0")
                        block.WorkPhoneNumber = "";
                    block.Email = dr["Email"].ToString();
                    block.CommandNumber = dr["CommandNumber"].ToString();
                    block.CommandNumberPrintSymbol = dr["CommandNumberPrintSymbol"].ToString();
                    if (DBCommon.GetInt(dr["HasCommandNumPrintSymbol2"]) == 1)
                    {
                        block.CommandNumberPrintSymbol2 = Config.GetWebSetting("PrintReservistsCommandNumPrintSymbol2");
                    }
                    block.CommandSuffix = dr["CommandSuffix"] is string ? dr["CommandSuffix"].ToString() : "";
                    block.CommandVPN = dr["CommandVPN"].ToString();
                    block.ContractNumber = dr["ContractNumber"].ToString();
                    block.ContractVPN = dr["ContractVPN"].ToString();

                    if (!string.IsNullOrEmpty(dr["ContractDuration"].ToString()))
                    {
                        string monthString = "";
                        if (Convert.ToInt32(dr["ContractDuration"]) > 1)
                            monthString = " месеца";
                        else
                            monthString = " месец";

                        block.ContractDuration = dr["ContractDuration"].ToString() + monthString;
                    }

                    block.WorkCompanyName = dr["Company"].ToString();
                    block.CompanyCityAddressPostCode = dr["CompanyAddress"].ToString();
                    string companyPostCode = dr["CompanyPostCode"].ToString();
                    if (!String.IsNullOrEmpty(companyPostCode))
                    {
                        block.CompanyCityAddressPostCode += (String.IsNullOrEmpty(block.CompanyCityAddressPostCode) ? "" : ", ПК ") + companyPostCode;
                    }
                    block.CompanyPhone = dr["CompanyPhone"].ToString();
                    block.Section = dr["Section"].ToString();
                    block.OtherInfo = dr["OtherInfo"].ToString();
                    block.OtherInfo = CommonFunctions.ReplaceNewLinesInString(block.OtherInfo);
                    block.RecordOfServiceSeries = dr["RecordOfServiceSeries"].ToString();
                    block.RecordOfServiceNumber = dr["RecordOfServiceNumber"].ToString();

                    block.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();

                    if (!string.IsNullOrEmpty(dr["MilitaryDepartmentDate"].ToString()))
                        block.MilitaryDepartmentDate = CommonFunctions.FormatDate(dr["MilitaryDepartmentDate"].ToString());

                    if (!string.IsNullOrEmpty(dr["TemporaryRemovedDate"].ToString()))
                        block.TemporaryRemovedDate = CommonFunctions.FormatDate(dr["TemporaryRemovedDate"].ToString());

                    block.TemporaryRemovedReason = dr["TemporaryRemovedReason"].ToString();

                    if (!string.IsNullOrEmpty(dr["RemovedDate"].ToString()))
                        block.RemovedDate = CommonFunctions.FormatDate(dr["RemovedDate"].ToString());

                    block.RemovedReason = dr["RemovedReason"].ToString();
                    block.BirthDate = CommonFunctions.FormatDate(dr["BirthDate"].ToString());

                    string birthCountry = dr["BirthCountryName"].ToString();

                    if (birthCountry.ToUpper() == "БЪЛГАРИЯ")
                    {
                        block.BirthCity = dr["BirthCity"].ToString();
                        block.BirthRegion = dr["BirthRegion"].ToString();
                    }
                    else
                    {
                        block.BirthCity = dr["BirthCityAbroad"].ToString();
                        block.BirthRegion = "";
                    }
                    block.MilitaryCommandName = dr["MilitaryCommandName"].ToString();
                    block.AppointmentTime = dr["AppointmentTime"].ToString();
                    block.ChildCount = dr["ChildCount"].ToString();
                    block.MaritalStatusName = dr["MaritalStatusName"].ToString();                    
                    block.SizeClothing = dr["SizeClothing"].ToString();
                    block.SizeHat = dr["SizeHat"].ToString();
                    block.SizeShoes = dr["SizeShoes"].ToString();

                    if(!string.IsNullOrEmpty(dr["PersonHeight"].ToString()))
                        block.PersonHeight = dr["PersonHeight"].ToString() + " см";

                    block.ClInformationAccLevelBg = dr["ClInformationAccLevelBg"].ToString();
                    if (!string.IsNullOrEmpty(dr["ClInformationAccLevelBgDate"].ToString()))
                        block.ClInformationAccLevelBgDate = " до " + CommonFunctions.FormatDate(dr["ClInformationAccLevelBgDate"].ToString()) + " г.";
                    
                }

                dr.Close();

                SQL = @"SELECT d.OBR_IME as EducationName,
                               e.SPE_IME as SchoolSubjectName,
                               c.OBRG_KOGA as GraduatedYear
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
                    block.CivilGraduateYear = dr["GraduatedYear"].ToString();

                    if (!String.IsNullOrEmpty(block.CivilGraduateYear))
                        block.CivilGraduateYear += " г.";
                }

                dr.Close();


                SQL = @"SELECT d.VVU_IME as MilitarySchoolName,
                               e.VSP_IME as MilitarySchoolSubjectName,
                               c.OBRV_KOGA as GraduatedYear
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
                    block.MilEduGraduateYear = dr["GraduatedYear"].ToString();

                    if (!String.IsNullOrEmpty(block.MilEduGraduateYear))
                        block.MilEduGraduateYear += " г.";
                }

                dr.Close();


                SQL = @"SELECT d.VVA_IME as MilitaryAcademyName,
                               e.SVA_IME as MilitaryAcademySubjectName,
                               c.OBRVA_KOGA AS GraduatedYear,
                               c.OBRVA_PROD AS MilAcadDurationYear
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
                    block.MilAcadGraduateYear = dr["GraduatedYear"].ToString();
                    block.MilAcadDurationYear = dr["MilAcadDurationYear"].ToString();

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

                    block.ForeignLanguages += (String.IsNullOrEmpty(block.ForeignLanguages) ? "" : "; ") +
                        languageName + ", " + languageLevelOfKnowledgeName;
                }

                dr.Close();

                SQL = @"SELECT d.NZV_IME as ScientificTitle,
                        c.NZV_KOGA as ScientificTitleYear
                        FROM PMIS_RES.Reservists a
                        INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID                        
                        LEFT JOIN VS_OWNER.VS_NZV c ON c.NZV_EGNLS = b.EGN
                        INNER JOIN VS_OWNER.KLV_NZV d ON c.NZV_NZVKOD = d.NZV_KOD
                        WHERE a.ReservistID = " + reservistId.ToString() + @"
                        ORDER BY c.NZV_KOGA DESC, d.NZV_KOD";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.ScientificTitle = dr["ScientificTitle"].ToString();
                    block.ScientificTitleYear = dr["ScientificTitleYear"].ToString();

                    if (!String.IsNullOrEmpty(block.ScientificTitleYear))
                        block.ScientificTitleYear += " г.";
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
                    string militaryService = positionName + ", " + militaryUnitVpn + " " + militaryUnitName;

                    MilitaryServiceBlock milServiceBlock = new MilitaryServiceBlock();
                    milServiceBlock.MainTextData = militaryService;
                    milServiceBlock.VaccAnnNum = dr["VaccAnnNum"].ToString();
                    milServiceBlock.MilitaryCommanderRank = dr["MilitaryCommanderRank"].ToString();

                    if (!string.IsNullOrEmpty(dr["VaccAnnDateWhen"].ToString()))
                        milServiceBlock.VaccAnnDateWhen = CommonFunctions.FormatDate(dr["VaccAnnDateWhen"].ToString());
                    if (!string.IsNullOrEmpty(dr["VaccAnnDateVacAnn"].ToString()))
                        milServiceBlock.VaccAnnDateVacAnn = CommonFunctions.FormatDate(dr["VaccAnnDateVacAnn"].ToString());
                          
                    block.MilitaryService.Add(milServiceBlock);
                }

                dr.Close();

                SQL = @"SELECT e.ZVA_IME as MilitaryRankName,
                               c.ARZVA_ZPVD as VacAnn,
                               d.SPZ_IME as MilitaryCommanderRankName,
                               c.ARZVA_ZIZD as DateArchive,
                               c.ARZVA_ZKOGA as DateWhen
                        FROM PMIS_RES.Reservists a
                        INNER JOIN VS_OWNER.VS_LS b ON b.PersonID = a.PersonID 
                        LEFT OUTER JOIN VS_OWNER.VS_AR_ZVA c ON c.ARZVA_EGNLS = b.EGN
                        LEFT OUTER JOIN VS_OWNER.KLV_SPZ d ON d.SPZ_KOD = c.ARZVA_SPZKOD
                        LEFT OUTER JOIN VS_OWNER.KLV_ZVA e ON c.ARZVA_ZVAKOD = e.ZVA_KOD
                        WHERE a.ReservistID = " + reservistId.ToString() + @"
                        ORDER BY c.ARZVA_ZIZD DESC, c.ARZVA_ZPVD";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {                    
                    MilitaryRankBlock milRankBlock = new MilitaryRankBlock();
                    milRankBlock.MilitaryRankName = dr["MilitaryRankName"].ToString();
                    
                    milRankBlock.MilitaryCommanderRankName = dr["MilitaryCommanderRankName"].ToString();

                    if (!string.IsNullOrEmpty(dr["VacAnn"].ToString()))
                        milRankBlock.VacAnn = "№ " + dr["VacAnn"].ToString();
                    if (!string.IsNullOrEmpty(dr["DateArchive"].ToString()))
                        milRankBlock.DateArchive = CommonFunctions.FormatDate(dr["DateArchive"].ToString());
                    if (!string.IsNullOrEmpty(dr["DateWhen"].ToString()))
                        milRankBlock.DateWhen = CommonFunctions.FormatDate(dr["DateWhen"].ToString()); 

                    block.MilitaryRank.Add(milRankBlock);
                }

                dr.Close();


                SQL = @"SELECT d.AnnexNumber, 
                               d.AnnexDate, 
                               d.AnnexDurationMonths, 
                               d.AnnexExpireDate 
                        FROM PMIS_RES.Reservists a
                        LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses b ON b.MilitaryReportStatusKey = 'VOLUNTARY_RESERVE'
                        LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses c ON c.ReservistID = a.ReservistID AND c.IsCurrent = 1 AND c.MilitaryReportStatusID = b.MilitaryReportStatusID
                        LEFT OUTER JOIN PMIS_RES.VoluntaryReserveAnnexes d ON c.ReservistMilRepStatusId = d.ReservistMilRepStatusId
                        WHERE a.ReservistId = " + reservistId.ToString() + @"
                        ORDER BY d.VoluntaryReserveAnnexId";

                cmd = new OracleCommand(SQL, conn);

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    VoluntaryReserveAnnexBlock volResAnnexBlock = new VoluntaryReserveAnnexBlock();
                    if (!string.IsNullOrEmpty(dr["AnnexNumber"].ToString()))
                        volResAnnexBlock.AnnexNumber = "№ " + dr["AnnexNumber"].ToString() + ",";
                    if (!string.IsNullOrEmpty(dr["AnnexDate"].ToString()))
                        volResAnnexBlock.AnnexDate = CommonFunctions.FormatDate(dr["AnnexDate"].ToString()) + " г.";
                    if (!string.IsNullOrEmpty(dr["AnnexDurationMonths"].ToString()))
                    {
                        string monthString = "";
                        if (Convert.ToInt32(dr["AnnexDurationMonths"]) > 1)
                            monthString = " месеца";
                        else
                            monthString = " месец";

                        volResAnnexBlock.AnnexDurationMonths = dr["AnnexDurationMonths"].ToString() + monthString;
                    }
                    if (!string.IsNullOrEmpty(dr["AnnexExpireDate"].ToString()))
                        volResAnnexBlock.AnnexExpireDate = "до " + CommonFunctions.FormatDate(dr["AnnexExpireDate"].ToString()) + " г.";

                    block.VoluntaryReserveAnnex.Add(volResAnnexBlock);
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
