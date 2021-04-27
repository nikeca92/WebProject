using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using PMIS.Common;
using PMIS.Applicants.DAL;

namespace PMIS.Applicants.Common
{
    public class GeneratePrintApplicantUtil
    {
        public enum ApplicationValue
        {
            VS,
            DR4B,
            DR6,
            NVP,
            SVP
        }

        public static string PrintDocuments(List<int> personIDs, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantDocuments_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantDocuments_Body.html");
            using (StreamReader sr = new StreamReader(tempalteBodyPath))
            {
                templateBody = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantDocuments_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            for (int i = 0; i < personIDs.Count; i++)
            {
                int personId = personIDs[i];
                PrintApplicantDocumentsBlock block = PrintApplicantDocumentsUtil.GetPrintApplicantDocumentsBlock(personId, currentUser);

                Dictionary<string, string> data = new Dictionary<string, string>();

                data.Add("FIRST_NAME", block.FirstName);
                data.Add("LAST_NAME", block.LastName);
                data.Add("IDENT_NUMBER", block.IdentNumber);
                data.Add("CITY_ADDRESS_POSTCODE", block.PermAddress);
                data.Add("ID_CARD_NUMBER", block.IDCardNumber);
                data.Add("ID_CARD_ISSUE_BY", block.IDCardIssuedBy);
                data.Add("ID_CARD_ISSUE_DATE", block.IDCardIssueDate);

                string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                if (i == personIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                {
                    bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                }

                body.Append(bodyContent);
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintLetter(List<int> applicantIDs, string selectedLetter, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantLetter_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantLetter_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();

            if (selectedLetter != "-1")
            {
                string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantLetter_Body_" + selectedLetter + ".html");
                using (StreamReader sr = new StreamReader(tempalteBodyPath))
                {
                    templateBody = sr.ReadToEnd();
                }

                for (int i = 0; i < applicantIDs.Count; i++)
                {
                    int applicantId = applicantIDs[i];
                    PrintApplicantLetterBlock block = PrintApplicantLetterUtil.GetPrintApplicantLetterBlock(applicantId, currentUser);

                    Dictionary<string, string> data = new Dictionary<string, string>();

                    data.Add("FIRST_NAME", block.FirstName);
                    data.Add("LAST_NAME", block.LastName);
                    data.Add("IDENT_NUMBER", block.IdentNumber);
                    data.Add("ID_CARD_NUMBER", block.IDCardNumber);
                    data.Add("ID_CARD_ISSUE_BY", block.IDCardIssuedBy);
                    data.Add("ID_CARD_ISSUE_DATE", block.IDCardIssueDate);
                    data.Add("MILITARY_DEPARTMENT_NAME_UPPER", block.MilitaryDepartmentTextUpper);

                    string imgPath = HttpContext.Current.Server.MapPath(@"~/Images/logo_vos_plovdiv.png");
                    int imgHeight = 83;
                    int imgWidth = 89;
                    string imageSRC = GeneratePrintUtil.ImageToBase64(imgPath, imgHeight, imgWidth);

                    data.Add("IMAGE_SRC", imageSRC);

                    templateBody = GeneratePrintApplicantUtil.GetPositionsTemplate(templateBody, block.Positions);

                    string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                    if (i == applicantIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                    {
                        bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                    }

                    body.Append(bodyContent);
                }
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }

        public static string PrintApplication(List<int> applicantIDs, string appValue, int vacancyAnnounceId, int responsibleMilitaryUnitId, User currentUser)
        {
            string templateStart = "";
            string templateBody = "";
            string templateEnd = "";

            string tempalteStartPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantApplication_Start.html");
            using (StreamReader sr = new StreamReader(tempalteStartPath))
            {
                templateStart = sr.ReadToEnd();
            }

            string tempalteEndPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantApplication_End.html");
            using (StreamReader sr = new StreamReader(tempalteEndPath))
            {
                templateEnd = sr.ReadToEnd();
            }

            string bageBreak = "<br clear=all style='mso-special-character:line-break;page-break-before:always'>";

            StringBuilder body = new StringBuilder();
            bool enumContainsAppValue = Enum.GetNames(typeof(ApplicationValue)).Contains(appValue);

            if (enumContainsAppValue)
            {
                string tempalteBodyPath = HttpContext.Current.Server.MapPath("~/PrintTemplates/ApplicantApplication_Body_" + appValue + ".html");
                using (StreamReader sr = new StreamReader(tempalteBodyPath))
                {
                    templateBody = sr.ReadToEnd();
                }

                for (int i = 0; i < applicantIDs.Count; i++)
                {
                    int applicantId = applicantIDs[i];
                    PrintApplicantApplicationBlock block = PrintApplicantApplicationUtil.GetPrintApplicantApplicationBlock(applicantId, vacancyAnnounceId, responsibleMilitaryUnitId, currentUser);

                    Dictionary<string, string> data = new Dictionary<string, string>();

                    data.Add("FIRST_NAME", block.FirstName);
                    data.Add("LAST_NAME", block.LastName);
                    data.Add("IDENT_NUMBER", block.IdentNumber);
                    data.Add("PERM_CITY_ADDRESS", block.PermAddress);
                    data.Add("PERM_POSTCODE", block.PermPostCode);
                    data.Add("CURR_CITY_ADDRESS", block.CurrAddress);
                    data.Add("CURR_POSTCODE", block.CurrPostCode);
                    data.Add("CONTACT_CITY_ADDRESS", block.ContactAddress);
                    data.Add("CONTACT_POSTCODE", block.ContactPostCode);
                    data.Add("MOBILE_PHONE_NUMBER", block.MobilePhoneNumber);
                    data.Add("TELEPHONE_NUMBER", block.TelephoneNumber);
                    data.Add("EMAIL", block.Email);
                    data.Add("RES_MILITARY_UNIT_VPN", block.ResMilitaryUnitVPN);
                    data.Add("RES_MILITARY_UNIT_NAME_UPPER", block.ResMilitaryUnitNameUpper);
                    data.Add("MILITARY_DEPARTMENT_NAME_UPPER", block.MilitaryDepartmentTextUpper);
                    data.Add("VACANCY_ANNOUNCE_NUMBER", block.VacancyAnnounceNumber);
                    data.Add("VACANCY_ANNOUNCE_DATE", block.VacancyAnnounceDate);
                    data.Add("REG_NUMBER", block.RegNumber);
                    data.Add("REG_DATE", block.RegDate);

                    templateBody = GeneratePrintApplicantUtil.GetPositionsTemplate(templateBody, block.Positions);

                    string bodyContent = GeneratePrintUtil.PopulateTemplate(templateBody, data);

                    if (i == applicantIDs.Count - 1 && bodyContent.EndsWith(bageBreak))
                    {
                        bodyContent = bodyContent.TrimEnd(bageBreak.ToCharArray());
                    }

                    body.Append(bodyContent);
                }
            }

            string result = templateStart + body.ToString() + templateEnd;

            return result;
        }


        public static string GetPositionsTemplate(string templateBody, List<PositionBlock> positions)
        {
            string startRepeater = "REPEATER_START";            

            PartTemplate milPositionPartTemplate = GeneratePrintUtil.GetPartTemplateForDynamicRows(templateBody, startRepeater);

            if (milPositionPartTemplate.substringForReplace == null && milPositionPartTemplate.templateRow == null)
                return templateBody;

            string positionTable = "";

            for (int m = 0; m < positions.Count; m++)
            {
                Dictionary<string, string> positionData = new Dictionary<string, string>();

                if (m < positions.Count)
                {
                    positionData.Add("ROW_NUMBER", (m + 1).ToString());
                    positionData.Add("POSITION_NAME", positions[m].PositionName);
                    positionData.Add("MILITARY_UNIT_NAME", positions[m].MilitaryUnitName);
                    positionData.Add("MILITARY_UNIT_VPN", positions[m].MilitaryUnitVPN);
                }
                else
                {
                    positionData.Add("ROW_NUMBER", (m + 1).ToString());
                    positionData.Add("POSITION_NAME", "");
                    positionData.Add("MILITARY_UNIT_NAME", "");
                    positionData.Add("MILITARY_UNIT_VPN", "");
                }

                string rowContent = "";
                rowContent = GeneratePrintUtil.PopulateTemplate(milPositionPartTemplate.templateRow, positionData);
                positionTable += rowContent;
            }

            templateBody = templateBody.Replace(milPositionPartTemplate.substringForReplace, positionTable);

            return templateBody;
        }
    }
}
