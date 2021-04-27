using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.IO;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.DAL;

namespace PMIS.Reserve.Common
{
    public static class GeneratePrintReservistsUtil
    {
        public static string PrintMK(List<int> reservistIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistMK_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistMK_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistMK_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < reservistIDs.Count; i++)
            {
                int reservistId = reservistIDs[i];
                PrintReservistsMKBlock block = PrintReservistsMKUtil.GetPrintReservistsMKBlock(reservistId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("COMMAND", block.Command);
                data.Add("COMMAND_NUM", block.CommandNumber);
                data.Add("COMMAND_NAME", block.CommandName);
                data.Add("COMMAND_NUM_PRINTSYMBOL", block.CommandNumberPrintSymbol);
                data.Add("COMMAND_NUM_PRINTSYMBOL_2", block.CommandNumberPrintSymbol2);
                data.Add("COMMAND_SUF", block.CommandSuffix);
                data.Add("APPOINTMENT_TIME", block.AppointmentTime);
                data.Add("DELIVERY_PLACE", block.DeliveryPlace);
                data.Add("MILITARY_DEPARTMENT", block.MilitaryDepartment);
                data.Add("FIRST_NAME", block.FirstName);
                data.Add("LAST_NAME", block.LastName);
                data.Add("IDENT_NUMBER", block.IdentNumber);
                data.Add("IDENT_NUMBER_ENCRYPT", block.IdentNumberEncrypt);
                data.Add("MIL_REP_SPECIALITY", block.MilRepSpeciality);
                data.Add("MILITARY_RANK", block.MilitaryRank);
                data.Add("APPT_MIL_REP_SPECIALITY", block.AppointMilRepSpeciality);
                data.Add("APPT_MILITARY_RANK", block.AppointMilitaryRank);
                data.Add("APPT_POSITION", block.AppointPosition);
                data.Add("CITY_ADDRESS_POSTCODE", block.PermAddress);
                data.Add("CIVIL_EDU_NAME", block.CivilEducationName);
                data.Add("CIVIL_SUBJECT_NAME", block.CivilSchoolSubjectName);
                data.Add("CIVIL_GRADUATE_YEAR", block.CivilGraduateYear);
                data.Add("MILEDU_SCHOOL_NAME", block.MilEduMilitarySchoolName);
                data.Add("MILEDU_SUBJECT_NAME", block.MilEduMilitarySchoolSubjectName);
                data.Add("MILEDU_GRADUATE_YEAR", block.MilEduGraduateYear);
                data.Add("MILACAD_ACADEMY_NAME", block.MilAcadMilitaryAcademyName);
                data.Add("MILACAD_SUBJECT_NAME", block.MilAcadMilitaryAcademySubjectName);
                data.Add("MILACAD_GRADUATE_YEAR", block.MilAcadGraduateYear);
                data.Add("FOREIGN_LANGUAGES", block.ForeignLanguages);
                data.Add("WORK_COMPANY", block.WorkCompanyName);
                data.Add("WORK_POSITION", block.WorkPositionNKPDDisplay);
                data.Add("MARITAL_STATUS", block.MaritalStatus);
                data.Add("CHILD_COUNT", block.ChildCount);
                data.Add("SIZE_CLOTHING", block.SizeClothing);
                data.Add("SIZE_HAT", block.SizeHat);
                data.Add("SIZE_SHOES", block.SizeShoes);

                for (int j = 0; j < block.MilitaryService.Count; j++)
                {
                    data.Add("MILITARY_SERVICE_" + ((int)(j + 1)).ToString(), block.MilitaryService[j].MainTextData);

                    data.Add("MIL_SRV_VACANN_NUM_" + ((int)(j + 1)).ToString(), block.MilitaryService[j].VaccAnnNum);
                    data.Add("MIL_SRV_VACANN_DATE_" + ((int)(j + 1)).ToString(), block.MilitaryService[j].VaccAnnDateVacAnn);
                    data.Add("MIL_SRV_VACANN_WHEN_" + ((int)(j + 1)).ToString(), block.MilitaryService[j].VaccAnnDateWhen);
                    data.Add("MIL_SRV_MILCOMMANDER_" + ((int)(j + 1)).ToString(), block.MilitaryService[j].MilitaryCommanderRank);
                }

                for (int j = block.MilitaryService.Count + 1; j <= 100; j++)
                {
                    data.Add("MILITARY_SERVICE_" + j.ToString(), "");
                }

                data.Add("DATE", CommonFunctions.FormatDate(DateTime.Now));


                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == reservistIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintPZ(List<int> reservistIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistPZ_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistPZ_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistPZ_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < reservistIDs.Count; i++)
            {
                int reservistId = reservistIDs[i];
                PrintReservistsPZBlock block = PrintReservistsPZUtil.GetPrintReservistsPZBlock(reservistId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("COMMAND", block.Command);
                data.Add("COMMAND_NUM", block.CommandNumber);
                data.Add("COMMAND_NUM_PRINTSYMBOL", block.CommandNumberPrintSymbol);
                data.Add("COMMAND_NUM_PRINTSYMBOL_2", block.CommandNumberPrintSymbol2);
                data.Add("COMMAND_NAME", block.CommandName);
                data.Add("COMMAND_SUF", block.CommandSuffix);
                data.Add("READINESS", block.ReservistReadinessName);
                data.Add("APPT_MIL_REP_SPECIALITY", block.AppointMilRepSpeciality);
                data.Add("MILITARY_RANK", block.MilitaryRank);
                data.Add("FIRST_NAME", block.FirstName);
                data.Add("LAST_NAME", block.LastName);
                data.Add("IDENT_NUMBER", block.IdentNumber);
                data.Add("IDENT_NUMBER_ENCRYPT", block.IdentNumberEncrypt);
                data.Add("CITY_ADDRESS_POSTCODE", block.PermAddress);
                data.Add("ADDRESS_DISTRICT", block.PermAddressDistrict);
                data.Add("DELIVERY_PLACE", block.DeliveryPlace);
                data.Add("MILITARY_DEPARTMENT", block.MilitaryDepartment);

                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == reservistIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintAK(List<int> reservistIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistAK_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistAK_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistAK_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < reservistIDs.Count; i++)
            {
                int reservistId = reservistIDs[i];
                PrintReservistsAKBlock block = PrintReservistsAKUtil.GetPrintReservistsAKBlock(reservistId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("RANK", block.Rank);
                data.Add("RANK_SHORT", block.RankShort);
                data.Add("FIRST_NAME", block.FirstName);
                data.Add("LAST_NAME", block.LastName);
                data.Add("IDENT_NUMBER", block.IdentNumber);
                data.Add("IDENT_NUMBER_ENCRYPT", block.IdentNumberEncrypt);
                data.Add("MIL_FORCE_SORT", block.MilitaryForceSort);
                data.Add("VOS_NUMBER", block.VosNumber);
                data.Add("VOS_TEXT", block.VosText);
                data.Add("POSITION", block.Position);
                data.Add("PERM_REGION", block.PermRegion);
                data.Add("PERM_MUNICIPALITY", block.PermMunicipality);
                data.Add("PERM_CITY", block.PermCity);
                data.Add("PERM_DISTRICT", block.PermDistrict);
                data.Add("PERM_ADDRESS", block.PermAddress);
                data.Add("PERM_POSTCODE", block.PermPostcode);
                data.Add("CURR_REGION", block.CurrRegion);
                data.Add("CURR_MUNICIPALITY", block.CurrMunicipality);
                data.Add("CURR_CITY", block.CurrCity);
                data.Add("CURR_DISTRICT", block.CurrDistrict);
                data.Add("CURR_ADDRESS", block.CurrAddress);
                data.Add("CURR_POSTCODE", block.CurrPostcode);
                data.Add("MOBILE_PHONE_NUMBER", block.MobilePhoneNumber);
                data.Add("TELEPHONE_NUMBER", block.TelephoneNumber);
                data.Add("EMAIL", block.Email);
                data.Add("COMMAND_NUM", block.CommandNumber);
                data.Add("COMMAND_NUM_PRINTSYMBOL", block.CommandNumberPrintSymbol);
                data.Add("COMMAND_NUM_PRINTSYMBOL_2", block.CommandNumberPrintSymbol2);
                data.Add("COMMAND_SUF", block.CommandSuffix);
                data.Add("CONTRACT_NUMBER", block.ContractNumber);
                data.Add("CONTRACT_VPN", block.ContractVPN);
                data.Add("CONTRACT_DURATION", block.ContractDuration);
                data.Add("WORK_COMPANY", block.WorkCompanyName);
                data.Add("COMPANY_CITY_ADDRESS_POSTCODE", block.CompanyCityAddressPostCode);
                data.Add("COMPANY_PHONE", block.CompanyPhone);
                data.Add("SECTION", block.Section);
                data.Add("OTHER_INFO", block.OtherInfo);
                data.Add("RECORDS_OF_SERVICE_SERIES", block.RecordOfServiceSeries);
                data.Add("RECORDS_OF_SERVICE_NUMBER", block.RecordOfServiceNumber);




                data.Add("CIVIL_EDU_NAME", block.CivilEducationName);
                data.Add("CIVIL_SUBJECT_NAME", block.CivilSchoolSubjectName);
                data.Add("CIVIL_GRADUATE_YEAR", block.CivilGraduateYear);
                data.Add("MILEDU_SCHOOL_NAME", block.MilEduMilitarySchoolName);
                data.Add("MILEDU_SUBJECT_NAME", block.MilEduMilitarySchoolSubjectName);
                data.Add("MILEDU_GRADUATE_YEAR", block.MilEduGraduateYear);
                data.Add("MILACAD_ACADEMY_NAME", block.MilAcadMilitaryAcademyName);
                data.Add("MILACAD_SUBJECT_NAME", block.MilAcadMilitaryAcademySubjectName);
                data.Add("MILACAD_GRADUATE_YEAR", block.MilAcadGraduateYear);

                data.Add("FOREIGN_LANGUAGES", block.ForeignLanguages);

                data.Add("MILITARY_DEPARTMENT_NAME", block.MilitaryDepartmentName);
                data.Add("MILITARY_DEPARTMENT_DATE", block.MilitaryDepartmentDate);
                data.Add("TEMPORARY_REMOVED_DATE", block.TemporaryRemovedDate);
                data.Add("TEMPORARY_REMOVED_REASON", block.TemporaryRemovedReason);
                data.Add("REMOVED_DATE", block.RemovedDate);
                data.Add("REMOVED_REASON", block.RemovedReason);

                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == reservistIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintASK(List<int> reservistIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistASK_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistASK_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistASK_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < reservistIDs.Count; i++)
            {
                int reservistId = reservistIDs[i];
                PrintReservistsASKBlock block = PrintReservistsASKUtil.GetPrintReservistsASKBlock(reservistId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();

                data.Add("RANK", block.Rank);
                data.Add("RANK_SHORT", block.RankShort);
                data.Add("FIRST_NAME", block.FirstName);
                data.Add("LAST_NAME", block.LastName);
                data.Add("IDENT_NUMBER", block.IdentNumber);
                data.Add("IDENT_NUMBER_ENCRYPT", block.IdentNumberEncrypt);
                data.Add("BIRTH_CITY", block.BirthCity);
                data.Add("BIRTH_REGION", block.BirthRegion);
                data.Add("BIRTH_DATE", block.BirthDate);
                data.Add("MIL_FORCE_SORT", block.MilitaryForceSort);
                data.Add("VOS_NUMBER", block.VosNumber);
                data.Add("VOS_TEXT", block.VosText);
                data.Add("POSITION", block.Position);
                data.Add("PERM_REGION", block.PermRegion);
                data.Add("PERM_CITY", block.PermCity);
                data.Add("PERM_ADDRESS", block.PermAddress);
                data.Add("CURR_REGION", block.CurrRegion);
                data.Add("CURR_CITY", block.CurrCity);
                data.Add("CURR_ADDRESS", block.CurrAddress);
                data.Add("MOBILE_PHONE_NUMBER", block.MobilePhoneNumber);
                data.Add("TELEPHONE_NUMBER", block.TelephoneNumber);
                data.Add("WORK_NUMBER", block.WorkPhoneNumber);
                data.Add("EMAIL", block.Email);
                data.Add("COMMAND_NUM", block.CommandNumber);
                data.Add("COMMAND_NUM_PRINTSYMBOL", block.CommandNumberPrintSymbol);
                data.Add("COMMAND_NUM_PRINTSYMBOL_2", block.CommandNumberPrintSymbol2);
                data.Add("COMMAND_SUF", block.CommandSuffix);
                data.Add("COMMAND_VPN", block.CommandVPN);
                data.Add("CONTRACT_NUMBER", block.ContractNumber);
                data.Add("CONTRACT_VPN", block.ContractVPN);
                data.Add("CONTRACT_DURATION", block.ContractDuration);
                data.Add("WORK_COMPANY", block.WorkCompanyName);
                data.Add("COMPANY_CITY_ADDRESS_POSTCODE", block.CompanyCityAddressPostCode);
                data.Add("COMPANY_PHONE", block.CompanyPhone);
                data.Add("SECTION", block.Section);
                data.Add("OTHER_INFO", block.OtherInfo);
                data.Add("RECORDS_OF_SERVICE_SERIES", block.RecordOfServiceSeries);
                data.Add("RECORDS_OF_SERVICE_NUMBER", block.RecordOfServiceNumber);

                data.Add("CIVIL_EDU_NAME", block.CivilEducationName);
                data.Add("CIVIL_SUBJECT_NAME", block.CivilSchoolSubjectName);
                data.Add("CIVIL_GRADUATE_YEAR", block.CivilGraduateYear);
                data.Add("MILEDU_SCHOOL_NAME", block.MilEduMilitarySchoolName);
                data.Add("MILEDU_SUBJECT_NAME", block.MilEduMilitarySchoolSubjectName);
                data.Add("MILEDU_GRADUATE_YEAR", block.MilEduGraduateYear);
                data.Add("MILACAD_ACADEMY_NAME", block.MilAcadMilitaryAcademyName);
                data.Add("MILACAD_SUBJECT_NAME", block.MilAcadMilitaryAcademySubjectName);
                data.Add("MILACAD_GRADUATE_YEAR", block.MilAcadGraduateYear);
                data.Add("MILACAD_DURATION_YEAR", block.MilAcadDurationYear);

                data.Add("SCIENTIFIC_TITLE", block.ScientificTitle);
                data.Add("SCIENTIFIC_TITLE_YEAR", block.ScientificTitleYear);
                data.Add("FOREIGN_LANGUAGES", block.ForeignLanguages);

                data.Add("MILITARY_DEPARTMENT_NAME", block.MilitaryDepartmentName);
                data.Add("MILITARY_DEPARTMENT_DATE", block.MilitaryDepartmentDate);
                data.Add("TEMPORARY_REMOVED_DATE", block.TemporaryRemovedDate);
                data.Add("TEMPORARY_REMOVED_REASON", block.TemporaryRemovedReason);
                data.Add("REMOVED_DATE", block.RemovedDate);
                data.Add("REMOVED_REASON", block.RemovedReason);

                data.Add("MILITARY_COMMAND_NAME", block.MilitaryCommandName);
                data.Add("APPOINTMENT_TIME", block.AppointmentTime);
                data.Add("CILD_COUNT", block.ChildCount);
                data.Add("MARITAL_STATUS_NAME", block.MaritalStatusName);
                data.Add("PERSON_HEIGHT", block.PersonHeight);
                data.Add("SIZE_CLOTHING", block.SizeClothing);
                data.Add("SIZE_HAT", block.SizeHat);
                data.Add("SIZE_SHOES", block.SizeShoes);
                data.Add("ACCESS_LEVEL", block.ClInformationAccLevelBg + block.ClInformationAccLevelBgDate);
                

                for (int j = 0; j < block.IdentNumber.Length; j++)
                {
                    data.Add("IDENT_NUMBER_" + ((int)(j + 1)).ToString(), block.IdentNumber[j].ToString());
                }

                string startRepeaterMilService = "REPEATER_START_MILITARY_SERVICE";
                PartTemplate milServicePartTemplate = GeneratePrintUtil.GetPartTemplate(templateBody, startRepeaterMilService);
                int milServiceMinRows = milServicePartTemplate.minRowsNumber;

                if (block.MilitaryService.Count > milServiceMinRows)
                    milServiceMinRows = block.MilitaryService.Count;

                string militaryServiceTable = "";

                for (int k = 0; k < milServiceMinRows; k++)
                {
                    Dictionary<string, string> militaryData = new Dictionary<string, string>();

                    if (k < block.MilitaryService.Count)
                    {
                        militaryData.Add("MILITARY_SERVICE", block.MilitaryService[k].MainTextData);
                        militaryData.Add("MIL_SRV_VACANN_NUM", block.MilitaryService[k].VaccAnnNum);
                        militaryData.Add("MIL_SRV_VACANN_DATE", block.MilitaryService[k].VaccAnnDateVacAnn);
                        militaryData.Add("MIL_SRV_VACANN_WHEN", block.MilitaryService[k].VaccAnnDateWhen);
                        militaryData.Add("MIL_SRV_MILCOMMANDER", block.MilitaryService[k].MilitaryCommanderRank);
                    }
                    else
                    {
                        militaryData.Add("MILITARY_SERVICE", "");
                        militaryData.Add("MIL_SRV_VACANN_NUM", "");
                        militaryData.Add("MIL_SRV_VACANN_DATE", "");
                        militaryData.Add("MIL_SRV_VACANN_WHEN", "");
                        militaryData.Add("MIL_SRV_MILCOMMANDER", "");
                    }
                    string milServiceRowContent = "";
                    milServiceRowContent = GeneratePrintUtil.PopulateTemplate(milServicePartTemplate.templateRow, militaryData);
                    militaryServiceTable += milServiceRowContent;
                }

                templateBody = templateBody.Replace(milServicePartTemplate.substringForReplace, militaryServiceTable);


                string startRepeaterMilRank = "REPEATER_START_RANK";

                PartTemplate milRankPartTemplate = GeneratePrintUtil.GetPartTemplate(templateBody, startRepeaterMilRank);
                int milRankMinRows = milRankPartTemplate.minRowsNumber;

                if (block.MilitaryRank.Count > milRankMinRows)
                    milRankMinRows = block.MilitaryRank.Count;

                string militaryRankTable = "";

                for (int m = 0; m < milRankMinRows; m++)
                {
                    Dictionary<string, string> militaryData = new Dictionary<string, string>();

                    if (m < block.MilitaryRank.Count)
                    {
                        militaryData.Add("RANK_NAME", block.MilitaryRank[m].MilitaryRankName);
                        militaryData.Add("RANK_ORDER_NUM", block.MilitaryRank[m].VacAnn);
                        militaryData.Add("RANK_ORDER_SIGNEDBY", block.MilitaryRank[m].MilitaryCommanderRankName);
                        militaryData.Add("RANK_ORDER_DATE", block.MilitaryRank[m].DateArchive);
                        militaryData.Add("RANK_APPLY_DATE", block.MilitaryRank[m].DateWhen);
                    }
                    else
                    {
                        militaryData.Add("RANK_NAME", "");
                        militaryData.Add("RANK_ORDER_NUM", "");
                        militaryData.Add("RANK_ORDER_SIGNEDBY", "");
                        militaryData.Add("RANK_ORDER_DATE", "");
                        militaryData.Add("RANK_APPLY_DATE", "");
                    }
                    string rowContent = "";
                    rowContent = GeneratePrintUtil.PopulateTemplate(milRankPartTemplate.templateRow, militaryData);
                    militaryRankTable += rowContent;
                }

                templateBody = templateBody.Replace(milRankPartTemplate.substringForReplace, militaryRankTable);


                string startRepeaterVoluntAnnex = "REPEATER_START_VOLUNTARY_ANNEX";

                PartTemplate volAnnexPartTemplate = GeneratePrintUtil.GetPartTemplate(templateBody, startRepeaterVoluntAnnex);
                int volAnnexMinRows = volAnnexPartTemplate.minRowsNumber;

                if (block.VoluntaryReserveAnnex.Count > volAnnexMinRows)
                    volAnnexMinRows = block.VoluntaryReserveAnnex.Count;

                string voluntaryAnnexTable = "";

                for (int n = 0; n < volAnnexMinRows; n++)
                {
                    Dictionary<string, string> voluntaryAnnexData = new Dictionary<string, string>();

                    if (n < block.VoluntaryReserveAnnex.Count)
                    {
                        voluntaryAnnexData.Add("ANNEX_NUMBER", block.VoluntaryReserveAnnex[n].AnnexNumber);
                        voluntaryAnnexData.Add("ANNEX_DURATION_MONTHS", block.VoluntaryReserveAnnex[n].AnnexDurationMonths);
                        voluntaryAnnexData.Add("ANNEX_DATE", block.VoluntaryReserveAnnex[n].AnnexDate);
                        voluntaryAnnexData.Add("ANNEX_EXPIRE_DATE", block.VoluntaryReserveAnnex[n].AnnexExpireDate);
                    }
                    else
                    {
                        voluntaryAnnexData.Add("ANNEX_NUMBER", "");
                        voluntaryAnnexData.Add("ANNEX_DURATION_MONTHS", "");
                        voluntaryAnnexData.Add("ANNEX_DATE", "");
                        voluntaryAnnexData.Add("ANNEX_EXPIRE_DATE", "");
                    }
                    string rowContent = "";
                    rowContent = GeneratePrintUtil.PopulateTemplate(volAnnexPartTemplate.templateRow, voluntaryAnnexData);
                    voluntaryAnnexTable += rowContent;
                }

                templateBody = templateBody.Replace(volAnnexPartTemplate.substringForReplace, voluntaryAnnexTable);


                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);
                

                if (i == reservistIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintUO(List<int> reservistIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistUO_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistUO_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ReservistUO_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string columnBreak = "<br clear=all style='mso-column-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < reservistIDs.Count; i++)
            {
                int reservistId = reservistIDs[i];
                PrintReservistsUOBlock block = PrintReservistsUOUtil.GetPrintReservistsUOBlock(reservistId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("IDENT_NUMBER", block.IdentNumber);
                data.Add("IDENT_NUMBER_ENCRYPT", block.IdentNumberEncrypt);
                data.Add("FIRST_NAME", block.FirstName);
                data.Add("LAST_NAME", block.LastName);
                data.Add("VOS_NUMBER", block.VosNumber);
                data.Add("VOS_TEXT", block.VosText);                
                data.Add("WORK_COMPANY", block.WorkCompanyName);
                data.Add("WORK_POSITION", block.WorkPosition);
                data.Add("WORK_POSITION_NKPD", block.WorkPositionNKPD);
                data.Add("WORK_COMPANY_UPPER", block.WorkCompanyNameUpperCase);
                data.Add("MILITARY_DEPARTMENT_NAME", block.MilitaryDepartmentName);
                data.Add("MILITARY_DEPARTMENT_UPPER", block.MilitaryDepartmentUpperCase);
                data.Add("POSTPONE_YEAR", block.PostponeYear);
                data.Add("POSTPONE_YEAR_NEXT", block.PostponeYearNext);

                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == reservistIDs.Count - 1 && bodyContent.EndsWith(columnBreak))
                {
                    bodyContent = bodyContent.TrimEnd(columnBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }
    }
}