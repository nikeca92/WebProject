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
    public static class GeneratePrintTechnicsUtil
    {
        public static string PrintMK(List<int> technicsIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsMK_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsMK_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsMK_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < technicsIDs.Count; i++)
            {
                int technicsId = technicsIDs[i];
                PrintTechnicsMKBlock block = PrintTechnicsMKUtil.GetPrintTechnicsMKBlock(technicsId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("COMMAND", block.Command);
                data.Add("COMMAND_NUM", block.CommandNumber);
                data.Add("COMMAND_NUM_PRINTSYMBOL", block.CommandNumberPrintSymbol);
                data.Add("COMMAND_NUM_PRINTSYMBOL_2", block.CommandNumberPrintSymbol2);
                data.Add("COMMAND_NAME", block.CommandName);
                data.Add("COMMAND_SUF", block.CommandSuffix);
                data.Add("MILITARY_DEPARTMENT", block.MilitaryDepartment);
                data.Add("APPOINTMENT_TIME", block.AppointmentTime);
                data.Add("DELIVERY_PLACE", block.DeliveryPlace);
                data.Add("TECHNICS_TYPE", block.TechnicsType);
                data.Add("NORMATIVE_TECHNICS", block.NormativeTechnics);
                data.Add("REG_NUMBER", block.RegNumber);
                data.Add("MAKE_MODEL", block.MakeModel);
                data.Add("FIRST_REGISTRATION_DATE", block.FirstRegistrationDate);
                data.Add("CARRYING_CAPACITY", block.CarryingCapacity);
                data.Add("ENGINE_TYPE", block.EngineType);
                data.Add("LOAD", block.Load);
                data.Add("OTHER_INFO", block.OtherInfo);
                data.Add("RESIDENCE_CITY_ADDRESS_POSTCODE", block.ResidenceAddress);
                data.Add("DRIVER_FIRST_NAME", block.DriverFirstName);
                data.Add("DRIVER_LAST_NAME", block.DriverLastName);
                data.Add("DRIVER_IDENT_NUMBER", block.DriverIdentNumber);
                data.Add("DRIVER_IDENT_NUMBER_ENCRYPT", block.DriverIdentNumberEncrypt);
                data.Add("DRIVER_MILITARY_RANK", block.DriverMilitaryRank);
                data.Add("DRIVER_MIL_REP_SPECIALITY", block.DriverMilRepSpeciality);
                data.Add("DRIVER_CITY_ADDRESS_POSTCODE", block.DriverAddress);
                data.Add("OWNER", block.Owner);
                data.Add("OWNER_CITY_ADDRESS_POSTCODE", block.OwnerAddress);
                data.Add("OWNER_PHONE", block.OwnerPhone);
                data.Add("DATE", CommonFunctions.FormatDate(DateTime.Now));

                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == technicsIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintPZ(List<int> technicsIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsPZ_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsPZ_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsPZ_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < technicsIDs.Count; i++)
            {
                int technicsId = technicsIDs[i];
                PrintTechnicsPZBlock block = PrintTechnicsPZUtil.GetPrintTechnicsPZBlock(technicsId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("COMMAND", block.Command);
                data.Add("COMMAND_NUM", block.CommandNumber);
                data.Add("COMMAND_NUM_PRINTSYMBOL", block.CommandNumberPrintSymbol);
                data.Add("COMMAND_NUM_PRINTSYMBOL_2", block.CommandNumberPrintSymbol2);
                data.Add("COMMAND_NAME", block.CommandName);
                data.Add("COMMAND_SUF", block.CommandSuffix);
                data.Add("MILITARY_DEPARTMENT", block.MilitaryDepartment);
                data.Add("OWNER", block.Owner);
                data.Add("OWNER_CITY_ADDRESS_POSTCODE", block.OwnerAddress);
                data.Add("OWNER_ADDRESS_DISTRICT", block.OwnerAddressDistrict);
                data.Add("OWNER_PHONE", block.OwnerPhone);
                data.Add("DELIVERY_PLACE", block.DeliveryPlace);
                data.Add("TECHNICS_TYPE", block.TechnicsType);
                data.Add("NORMATIVE_TECHNICS", block.NormativeTechnics);
                data.Add("REG_NUMBER", block.RegNumber);
                data.Add("MAKE_MODEL", block.MakeModel);
                data.Add("DRIVER_FIRST_NAME", block.DriverFirstName);
                data.Add("DRIVER_LAST_NAME", block.DriverLastName);
                data.Add("DRIVER_IDENT_NUMBER", block.DriverIdentNumber);
                data.Add("DRIVER_IDENT_NUMBER_ENCRYPT", block.DriverIdentNumberEncrypt);
                data.Add("DRIVER_CITY_ADDRESS_POSTCODE", block.DriverAddress);

                
                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == technicsIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintOK(List<int> technicsIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsOK_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsOK_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsOK_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < technicsIDs.Count; i++)
            {
                int technicsId = technicsIDs[i];
                PrintTechnicsOKBlock block = PrintTechnicsOKUtil.GetPrintTechnicsOKBlock(technicsId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("COMMAND_NUM", block.CommandNumber);
                data.Add("COMMAND_NUM_PRINTSYMBOL", block.CommandNumberPrintSymbol);
                data.Add("COMMAND_NUM_PRINTSYMBOL_2", block.CommandNumberPrintSymbol2);
                data.Add("COMMAND_SUF", block.CommandSuffix);  
                data.Add("TECHNICS_TYPE", block.TechnicsType);
                data.Add("REG_NUMBER", block.RegNumber);
                data.Add("MAKE_MODEL", block.MakeModel);
                data.Add("FIRST_REGISTRATION_DATE", block.FirstRegistrationDate);
                data.Add("DRIVER_FIRST_NAME", block.DriverFirstName);
                data.Add("DRIVER_LAST_NAME", block.DriverLastName);
                data.Add("DRIVER_IDENT_NUMBER", block.DriverIdentNumber);
                data.Add("DRIVER_IDENT_NUMBER_ENCRYPT", block.DriverIdentNumberEncrypt);
                data.Add("OWNER", block.OwnerFullName);
                data.Add("OWNER_CITY_ADDRESS_POSTCODE", block.OwnerAddress);
                data.Add("OWNER_PHONE", block.OwnerPhone);
                data.Add("OWNER_UIC", block.OwnerUIC);
                data.Add("TYPE", block.Type);
                data.Add("ENGINE_TYPE", block.EngineType);
                data.Add("POWER", block.Power);
                data.Add("SEATS", block.Seats);
                data.Add("ROADABILITY", block.Roadability);
                data.Add("CARRYING_CAPACITY", block.CarryingCapacity);
                data.Add("LOAD", block.Load);
                data.Add("MIL_REP_STATUS", block.MilRepStatus);
                data.Add("OTHER_EQUIP", block.OtherEquip);
                data.Add("STOP_DATE", block.StopDate);
                data.Add("CONTRACT_NUMBER", block.ContractNumber);
                data.Add("MILITARY_UNIT", block.MilitaryUnit);
                data.Add("CONTRACT_VPN", block.ContractVPN);
                data.Add("CONTRACT_DURATION", block.ContractDuration);
                data.Add("APPOINTMENT_TIME", block.AppointmentTime);
                data.Add("DRIVER_VOS_NUMBER", block.DriverVOSNumber);
                data.Add("DRIVER_VOS_TEXT", block.DriverVOSText);

                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == technicsIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintTO(List<int> technicsIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsTO_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsTO_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/TechnicsTO_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < technicsIDs.Count; i++)
            {
                int technicsId = technicsIDs[i];
                PrintTechnicsTOBlock block = PrintTechnicsTOUtil.GetPrintTechnicsTOBlock(technicsId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("TECHNICS_TYPE", block.TechnicsType);
                data.Add("REG_NUMBER", block.RegNumber);
                data.Add("MAKE_MODEL", block.MakeModel);
                data.Add("OWNER", block.OwnerFullName);
                data.Add("OWNER_UPPER", block.OwnerFullNameUpper);
                data.Add("OWNER_UIC", block.OwnerUIC);
                data.Add("KIND", block.Kind);
                data.Add("CARRYING_CAPACITY", block.CarryingCapacity);
                data.Add("MILITARY_DEPARTMENT_NAME", block.MilitaryDepartmentName);
                data.Add("MILITARY_DEPARTMENT_NAME_UPPER", block.MilitaryDepartmentNameUpper);
                data.Add("DRIVER_IDENT_NUMBER", block.DriverIdentNumber);
                data.Add("DRIVER_IDENT_NUMBER_ENCRYPT", block.DriverIdentNumberEncrypt);
                data.Add("DRIVER_FIRST_NAME", block.DriverFirstName);
                data.Add("DRIVER_LAST_NAME", block.DriverLastName);
                data.Add("DRIVER_WORK_POSITION", block.DriverPosition);
                data.Add("DRIVER_WORK_POSITION_NKPD", block.DriverPositionNKPD);
                data.Add("DRIVER_WORK_COMPANY", block.DriverCompanyName);

                data.Add("POSTPONE_YEAR", block.PostponeYear);
                data.Add("POSTPONE_YEAR_NEXT", block.PostponeYearNext);

                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == technicsIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }
    }
}