using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Linq;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //This utility class contains some common functions that could be reused across all modules
    public static class CommonFunctions
    {
        //Returns the date format defiend in the web.config file
        public static string DateFormat
        {
            get { return Config.GetWebSetting("DateFormat"); }
        }

        public static string DateTimeFormat
        {
            get { return Config.GetWebSetting("DateTimeFormat"); }
        }

        public static string DateTimeFormatShort
        {
            get { return Config.GetWebSetting("DateTimeFormatShort"); }
        }


        //Format a date by using the format from the config file
        public static string FormatDate(string date)
        {
            return FormatDate(date, DateFormat);
        }

        //Format a date by using the format from the config file
        public static string FormatDate(DateTime? date)
        {
            return date.HasValue ? FormatDate(date.Value.ToString(), DateFormat) : "";
        }


        //Format a date by using the format from the config file
        public static string FormatDateTime(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString(DateTimeFormat) : "";
        }

        //Format a date by using the format from the config file
        public static string FormatDateTimeShort(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString(DateTimeFormatShort) : "";
        }


        //Format a date by using a specific format
        public static string FormatDate(string date, string format)
        {
            DateTime lDate;
            DateTime.TryParse(date, out lDate);
            return lDate.ToString(format);
        }

        //Parse a date from a string
        public static DateTime? ParseDate(string date)
        {
            if (date == null || string.IsNullOrEmpty(date.Trim()))
                return null;

            DateTime lDate;
            lDate = DateTime.ParseExact(date.Replace("/", "."), DateFormat, null);
            return lDate;
        }

        //Parse a date from a string
        public static bool TryParseDate(string date)
        {
            DateTime fakeDate;
            return DateTime.TryParseExact(date.Replace("/", "."), DateFormat, null, DateTimeStyles.None, out fakeDate);
        }

        //equals nullable dates
        public static bool IsEqual(DateTime? d1, DateTime? d2)
        {
            return (!d1.HasValue && !d2.HasValue) || (d1.HasValue && d2.HasValue && DateTime.Compare(d1.Value, d2.Value) == 0);
        }

        //equals nullable integers
        public static bool IsEqualInt(int? i1, int? i2)
        {
            return (!i1.HasValue && !i2.HasValue) || (i1.HasValue && i2.HasValue && i1.Value == i2.Value);
        }

        //equals nullable decimals
        public static bool IsEqualDecimal(decimal? d1, decimal? d2)
        {
            return (!d1.HasValue && !d2.HasValue) || (d1.HasValue && d2.HasValue && d1.Value == d2.Value);
        }

        //This function is used to return the selection of a list box as a string that contains a list of values delimited by commas
        public static string GetSelectedValues(ListBox lst)
        {
            string selection = "";

            foreach (ListItem li in lst.Items)
            {
                if (li.Selected)
                {
                    if (li.Value == ListItems.GetOptionAll().Value)
                    {
                        selection = "";
                        break;
                    }
                    else
                    {
                        selection += (selection == "" ? "" : ",") + li.Value;
                    }
                }
            }

            return selection;
        }

        public static bool IsValidInt(string s)
        {
            bool isInt = false;

            try
            {
                int.Parse(s);
                isInt = true;
            }
            catch
            {
                isInt = false;
            }

            return isInt;
        }

        //Use this function to prevent SQL Injection attacks when passing a list of IDs
        public static string AvoidSQLInjForListOfIDs(string listOfIds)
        {
            string fixedListOfIds = "";

            string[] ids = listOfIds.Split(',');

            foreach (string id in ids)
            {              
                if (!IsValidInt(id))
                {
                    fixedListOfIds = "";
                    break;
                }
                else
                {
                    fixedListOfIds += (fixedListOfIds == "" ? "" : ",") + int.Parse(id).ToString();
                }
            }

            //If the list is empty for any reason then put -1 to prevent SQL errors becasue of an empty list (e.g. WHERE ID IN ())
            if (String.IsNullOrEmpty(listOfIds))
                fixedListOfIds = "-1";

            return fixedListOfIds;
        }

        //Parse a decimal from a string
        public static decimal ParseDecimal(string s)
        {
            s = s.Replace(Config.GetWebSetting("DecimalPoint"), CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            return decimal.Parse(s);
        }

        //Return is string a valid decimal
        public static bool IsValidDecimal(string s)
        {
            decimal fake;
            s = s.Replace(Config.GetWebSetting("DecimalPoint"), CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            return decimal.TryParse(s, out fake);
        }

        //Format a decimal using specific delimiter
        public static string FormatDecimal(decimal d)
        {
            string s = d.ToString();
            s = s.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, Config.GetWebSetting("DecimalPoint"));
            return s;
        }

        //Use this CSS to set a particular textbox to have a date picker control next to it
        public static string DatePickerCSS()
        {
            return "InputDateField dateformat-d-dt-n-dt-Y statusformat-d-dt-n-dt-Y fill-grid no-animation";
        }

        public static string ErrorMessageNoRightsFieldTemplate
        {
            get
            {
                return "Полето #FIELD# е задължително, но вие нямате дефинирани права за достъп до него. Моля, свържете се със системния администратор!";
            }
        }

        public static string ErrorMessageNoRightsFieldsTemplate
        {
            get
            {
                return "Полетата #FIELDS# са задължителни, но вие нямате дефинирани права за достъп до тях. Моля, свържете се със системния администратор!";
            }
        }

        public static string GetErrorMessageNoRights(string[] fieldNames)
        {
            bool isOne = (fieldNames.Length > 1) ? false : true;
            string fieldsStr = "";

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (i == 0)
                {
                    fieldsStr = "\"" + fieldNames[i] + "\"";
                }
                else
                {
                    fieldsStr += (i < fieldNames.Length - 1 ? ", " : " и ") + "\"" + fieldNames[i] + "\"";
                }
            }

            if (isOne)
            {
                return ErrorMessageNoRightsFieldTemplate.Replace("#FIELD#", fieldsStr);
            }
            else
            {
                return ErrorMessageNoRightsFieldsTemplate.Replace("#FIELDS#", fieldsStr);
            }
        }

        public static string ErrorMessageMandatoryTemplate
        {
            get
            {
                return "Полето \"#FIELD#\" е задължително";
            }
        }

        public static string GetErrorMessageMandatory(string fieldName)
        {
            return ErrorMessageMandatoryTemplate.Replace("#FIELD#", fieldName);
        }

        public static string ErrorMessageDateTemplate
        {
            get
            {
                return "Форматът на датата в полето \"#FIELD#\" е неправилен";
            }
        }

        public static string GetErrorMessageDate(string fieldName)
        {
            return ErrorMessageDateTemplate.Replace("#FIELD#", fieldName);
        }

        public static string ErrorMessageNumberTemplate
        {
            get
            {
                return "Стойността на полето \"#FIELD#\" не е валидно число";
            }
        }

        public static string GetErrorMessageNumber(string fieldName)
        {
            return ErrorMessageNumberTemplate.Replace("#FIELD#", fieldName);
        }

        public static string ErrorMessageMandatoryColumnTemplate
        {
            get
            {
                return "Колоната \"#FIELD#\" е задължителна";
            }
        }

        public static string GetErrorMessageMandatoryColumn(string fieldName)
        {
            return ErrorMessageMandatoryColumnTemplate.Replace("#FIELD#", fieldName);
        }

        public static string ErrorMessageDateColumnTemplate
        {
            get
            {
                return "Форматът на датите в колоната \"#FIELD#\" е неправилен";
            }
        }

        public static string GetErrorMessageDateColumn(string fieldName)
        {
            return ErrorMessageDateColumnTemplate.Replace("#FIELD#", fieldName);
        }

        public static string ErrorMessageNumberColumnTemplate
        {
            get
            {
                return "Стойностите в колоната \"#FIELD#\" не са валидни числа";
            }
        }

        public static string GetErrorMessageNumberColumn(string fieldName)
        {
            return ErrorMessageNumberColumnTemplate.Replace("#FIELD#", fieldName);
        }

        public static void SetTextAreaEvents(WebControl control, int maxLength)
        {
            control.Attributes.Add("onkeypress", "doKeypress(event, this);");
            control.Attributes.Add("onbeforepaste", "doBeforePaste(event, this);");
            control.Attributes.Add("onpaste", "doPaste(event, this);");
            control.Attributes.Add("maxLength", maxLength.ToString());
        }

        //HTML encoding string
        public static string HtmlEncoding(string stringToEncode)
        {
            return HttpUtility.HtmlEncode(stringToEncode);
        }


        public static Control CustomFindControl(Control control, string controlID)
        {
            Control ctrl = control.FindControl(controlID);

            if (ctrl != null && ctrl.ID == controlID)
            {
                return ctrl;
            }
            else if (control.Controls.Count > 0)
            {
                foreach (Control child in control.Controls)
                {
                    ctrl = CustomFindControl(child, controlID);

                    if (ctrl != null)
                        return ctrl;
                }
            }

            return null;
        }

        // Replace all existed encode symbols for new line with appropriate html tag
        public static string ReplaceNewLinesInString(string text)
        {
            text = text.Replace("\r\n", "<br />");
            text = text.Replace("\n", "<br />");

            return text;
        }

        public static string GenerateButtons(bool isTop, int topSize, bool withExcel)
        {
            return GenerateButtons(isTop, topSize, withExcel, false);
        }

        // Generate HTML for print, export to excel, export to word and close buttons. It's used on the print screens.
        public static string GenerateButtons(bool isTop, int topSize, bool withExcel, bool withWord)
        {
            StringBuilder sb = new StringBuilder();
            if (isTop)
            {
                sb.Append("<table style=\"float: left; clear: left; margin-top: -" + topSize + "px;\" class=\"noPrint\">");
            }
            else
            {
                sb.Append("<table style=\"float: left; clear: left; margin-top: 4px;\" class=\"noPrint\">");
            }

            sb.Append("<tr><td nowrap='nowrap'>");
            sb.Append("<img style=\"cursor: pointer;\" src=\"../Images/print.png\" title=\"Печат\" alt=\"Печат\" onclick=\"window.print();\" />");
            sb.Append("&nbsp;");

            if (withExcel)
            {
                sb.Append("<img style=\"cursor: pointer;\" src=\"../Images/excel.png\" title=\"Запазване в Excel\" alt=\"Запазване в Excel\" onclick=\"ExportToExcel()\" />");
                sb.Append("&nbsp;");
            }

            //Not implemented yet
            if (withWord)
            {
                sb.Append("<img style=\"cursor: pointer;\" src=\"../Images/WordIcon.gif\" title=\"Запазване в Word\" alt=\"Запазване в Word\" onclick=\"ExportToWord()\" />");
                sb.Append("&nbsp;");
            }

            sb.Append("<img style=\"cursor: pointer;\" src=\"../Images/close.png\" title=\"Затвори\" alt=\"Затвори\" onclick=\"window.close();\" />");
            sb.Append("</td></tr>");
            sb.Append("</table>");

            return sb.ToString();
        }

        public static string GetLabelText(string key)
        {
            return (string)HttpContext.GetGlobalResourceObject("Labels", key);
        }

        // Add tooltip to each item in drop down. The tooltip is the same as Text value of the item.
        public static void SetDropDownTooltip(DropDownList dd)
        {
            foreach (ListItem item in dd.Items)
            {
                item.Attributes.Add("title", item.Text); 
            }
        }

        public static bool IsKeyInList(string key, string list)
        {
            bool inList = false;

            string[] listArr = list.Split(',');

            foreach (string k in listArr)
            {
                if (key.ToUpper().Trim() == k.ToUpper().Trim())
                {
                    inList = true;
                    break;
                }
            }

            return inList;
        }

        public static string Replicate(string str, int count)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                //The logic for &nbsp; should be probably changed to work only when a specific parameter is passed to be true
                sb.Append((str == "&nbsp;" ? (i % 2 == 0 ? "&nbsp;" : " ") : str));
            }

            return sb.ToString();
        }

        public static string PadRight(string str, string padChar, int len)
        {
            string res = (str == null ? "" : str);

            if (res.Length < len)
                res = res + Replicate(padChar, len - str.Length);

            return res;
        }

        public static string CharAt(string str, int idx, string noResult)
        {
            string res = noResult;

            if (!string.IsNullOrEmpty(str) && idx + 1 <= str.Length)
            {
                res = str.Substring(idx, 1);
            }

            return res;
        }

        public static int[] GetIdsFromString(string listOfIds)
        {
            List<int> result = new List<int>();

            string[] ids = listOfIds.Split(',');

            foreach (string id in ids)
            {
                if (IsValidInt(id))
                {
                    result.Add(int.Parse(id));  
                }                
            }           

            return result.ToArray();
        }

        public static string IntegerToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + IntegerToRoman(number - 1000);
            if (number >= 900) return "CM" + IntegerToRoman(number - 900);
            if (number >= 500) return "D" + IntegerToRoman(number - 500);
            if (number >= 400) return "CD" + IntegerToRoman(number - 400);
            if (number >= 100) return "C" + IntegerToRoman(number - 100);
            if (number >= 90) return "XC" + IntegerToRoman(number - 90);
            if (number >= 50) return "L" + IntegerToRoman(number - 50);
            if (number >= 40) return "XL" + IntegerToRoman(number - 40);
            if (number >= 10) return "X" + IntegerToRoman(number - 10);
            if (number >= 9) return "IX" + IntegerToRoman(number - 9);
            if (number >= 5) return "V" + IntegerToRoman(number - 5);
            if (number >= 4) return "IV" + IntegerToRoman(number - 4);
            if (number >= 1) return "I" + IntegerToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        public static bool IsInternetExplorer()
        {
            return HttpContext.Current.Request.Browser.Browser.ToLower() == "internetexplorer";
        }

        public static int GetMenuOffsetY()
        {
            return CommonFunctions.IsInternetExplorer() ? 5 : 0;
        }

        // Use this function to avoid this error - ORA-01795: maximum number of expressions in a list is 1000
        public static string GetOracleSQLINClause(string field, string listValues)
        {
            const int MAX_NUMBER_OF_EXPRESSIONS = 1000;

            string clause = "";

            var values = listValues.Split(',');
            int count = values.Length;

            int iterations = 0;

            if (count % MAX_NUMBER_OF_EXPRESSIONS == 0)
            {
                iterations = count / MAX_NUMBER_OF_EXPRESSIONS;
            }
            else
            {
                iterations = count / MAX_NUMBER_OF_EXPRESSIONS + 1;
            }

            if (count > MAX_NUMBER_OF_EXPRESSIONS)
            {
                clause += " (";
            }

            var currValues = values.Skip(0).Take(MAX_NUMBER_OF_EXPRESSIONS).ToArray();
            string joinedValues = string.Join(",", currValues);

            clause += field + " IN (" + AvoidSQLInjForListOfIDs(joinedValues) + ") ";

            for (int i = 1; i < iterations; i++)
            {
                currValues = values.Skip(i * MAX_NUMBER_OF_EXPRESSIONS).Take(MAX_NUMBER_OF_EXPRESSIONS).ToArray();
                joinedValues = string.Join(",", currValues);

                clause += " OR " + field + " IN (" + AvoidSQLInjForListOfIDs(joinedValues) + ") ";
            }

            if (count > MAX_NUMBER_OF_EXPRESSIONS)
            {
                clause += ") ";
            }

            return clause;
        }

        public static int GetAgeFromEGNbyDate(string identNumber, DateTime date, User currentUser)
        {
            int age = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGNbyDate(:IdentNumber, :ToDate) as Age
                               FROM dual";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentNumber", OracleType.VarChar).Value = identNumber;
                cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = date;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    age = DBCommon.GetInt(dr["Age"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return age;
        }

        public static int GetAgeMonthsPartFromEGNbyDate(string identNumber, DateTime date, User currentUser)
        {
            int ageMonthsPart = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT PMIS_ADM.COMMONFUNCTIONS.GetAgeMonthsPartFromEGNbyDate(:IdentNumber, :ToDate) as AgeMonthsPart
                               FROM dual";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentNumber", OracleType.VarChar).Value = identNumber;
                cmd.Parameters.Add("ToDate", OracleType.DateTime).Value = date;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ageMonthsPart = DBCommon.GetInt(dr["AgeMonthsPart"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return ageMonthsPart;
        }
    }
}
