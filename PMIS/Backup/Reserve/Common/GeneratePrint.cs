using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.IO;
using System.Text;

using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class PartTemplate
    {
        public string substringForReplace { get; set; }
        public string templateRow { get; set; }
        public int minRowsNumber { get; set; }
    }

    public static class GeneratePrintUtil
    {
        public static string PopulateTemplate(string template, Dictionary<string, string> data)
        {
            string result = template;

            foreach (KeyValuePair<string, string> d in data)
            {
                result = result.Replace("[" + d.Key + "]", d.Value);
            }

            return result;
        }

        public static PartTemplate GetPartTemplate(string template, string repeaterStartString)
        {
            string repeaterEndString = "[REPEATER_END]";
            int startIndexOfSubstring = template.IndexOf(repeaterStartString) - 1;
            string substringForReplace = template.Substring(startIndexOfSubstring);
            int endIndexOfString = substringForReplace.IndexOf(repeaterEndString) + repeaterEndString.Length;
            substringForReplace = substringForReplace.Substring(0, endIndexOfString);

            int endIndexOfFirstRepeater = substringForReplace.IndexOf(']');
            string rowTemplate = substringForReplace.Substring(endIndexOfFirstRepeater + 1);
            rowTemplate = rowTemplate.Replace(repeaterEndString, "");

            int equalIndex = substringForReplace.IndexOf("=");
            int rowsNumber = Convert.ToInt32(substringForReplace.Substring(equalIndex + 1, (endIndexOfFirstRepeater - (equalIndex + 1))));

            PartTemplate partTemplate = new PartTemplate();
            partTemplate.substringForReplace = substringForReplace;
            partTemplate.templateRow = rowTemplate;
            partTemplate.minRowsNumber = rowsNumber;

            return partTemplate;
        }
    }
}