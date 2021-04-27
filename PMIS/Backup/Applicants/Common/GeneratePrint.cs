using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;

namespace PMIS.Applicants.Common
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

        public static PartTemplate GetPartTemplateForDynamicRows(string template, string repeaterStartString)
        {
            PartTemplate partTemplate = new PartTemplate();
            string repeaterEndString = "[REPEATER_END]";
            int startIndexOfSubstring = template.IndexOf(repeaterStartString) - 1;

            if (startIndexOfSubstring < 0) {
                return partTemplate;
            }

            string substringForReplace = template.Substring(startIndexOfSubstring);
            int endIndexOfString = substringForReplace.IndexOf(repeaterEndString) + repeaterEndString.Length;
            substringForReplace = substringForReplace.Substring(0, endIndexOfString);

            int endIndexOfFirstRepeater = substringForReplace.IndexOf(']');
            string rowTemplate = substringForReplace.Substring(endIndexOfFirstRepeater + 1);
            rowTemplate = rowTemplate.Replace(repeaterEndString, "");
                        
            partTemplate.substringForReplace = substringForReplace;
            partTemplate.templateRow = rowTemplate;
            partTemplate.minRowsNumber = 0;

            return partTemplate;
        }

        public static string ImageToBase64(string imagePath, int? imageHeight, int? imageWidth)
        {
            string base64String = null;

            using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    if (imageHeight.HasValue && imageWidth.HasValue)
                    {
                        int width = imageWidth.Value;
                        int height = imageHeight.Value;

                        var newImage = new Bitmap(width, height);
                        Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);
                        Bitmap bmp = new Bitmap(newImage);

                        ImageConverter converter = new ImageConverter();
                        var data = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                        base64String = Convert.ToBase64String(data);
                    }
                    else
                    {
                        image.Save(mStream, image.RawFormat);
                        byte[] imageBytes = mStream.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                    } 

                    return "data:image/jpg;base64," + base64String;
                }
            }
        }
    }   
    
}
