using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace PMIS.Common
{
    //This is common class used for specifying list item options (All, Select One, etc.)
    public static class ListItems
    {
        public static ListItem GetOptionAll()
        {
            ListItem li = new ListItem("<Всички>", "-1");
            return li;
        }

        public static ListItem GetOptionChooseOne()
        {
            ListItem li = new ListItem("", "-1");
            return li;
        }

        public static ListItem GetOptionYes()
        {
            ListItem li = new ListItem("Да", "yes");
            return li;
        }

        public static ListItem GetOptionNo()
        {
            ListItem li = new ListItem("Не", "no");
            return li;
        }

        public static string GetDropDownHtml(List<IDropDownItem> items, IDropDownItem additionalItem, string id, bool hasChoose, IDropDownItem selectedItem, string onChangeHandler, string attributes)
        {
            return GetDropDownHtml(items, additionalItem, id, hasChoose, selectedItem, onChangeHandler, attributes, false);
        }

        public static string GetDropDownHtml(List<IDropDownItem> items, IDropDownItem additionalItem, string id, bool hasChoose, IDropDownItem selectedItem, string onChangeHandler, string attributes, bool setOptionTextAsTooltip)
        {
            if (additionalItem != null)
            {
                bool found = false;
                foreach (IDropDownItem item in items)
                {
                    if (item.Value() == additionalItem.Value())
                        found = true;
                }
                if (!found)
                    items.Add(additionalItem);
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<select id='" + id + "'");
            
            if (!String.IsNullOrEmpty(onChangeHandler))
            {
                sb.Append(" onchange='" + onChangeHandler + "'");    
            }

            if (!String.IsNullOrEmpty(attributes))
            {
                sb.Append(" " + attributes + " ");   
            }
            
            sb.Append(">");

            if (hasChoose)
            {
                ListItem chooseOne = GetOptionChooseOne();
                sb.Append(@"<option value=""" + chooseOne.Value + @""">" + chooseOne.Text + @"</option>");
            }

            if (items != null)
            {
                foreach (IDropDownItem item in items)
                {
                    if (selectedItem != null && selectedItem.Value() == item.Value())
                    {
                        sb.Append(@"<option value=""" + item.Value() + @""" selected='selected' " + (setOptionTextAsTooltip ? @"title=""" + CommonFunctions.HtmlEncoding(item.Text()) + @"""" : "") + " >" + item.Text() + @"</option>");
                    }
                    else
                    {
                        sb.Append(@"<option value=""" + item.Value() + @""" " + (setOptionTextAsTooltip ? @"title=""" + CommonFunctions.HtmlEncoding(item.Text()) + @"""" : "") + " >" + item.Text() + @"</option>");
                    }
                }   
            }

            sb.Append("</select>");

            return sb.ToString();
        }

        public static string GetDropDownHtml(List<IDropDownItem> items, string id, bool hasChoose)
        {
            return GetDropDownHtml(items, null, id, hasChoose, null, null, null);
        }

        public static string GetDropDownHtmlWithTooltip(List<IDropDownItemWithTooltip> items, IDropDownItemWithTooltip additionalItem, string id, bool hasChoose, IDropDownItemWithTooltip selectedItem, string onChangeHandler, string attributes)
        {
            if (additionalItem != null)
            {
                bool found = false;
                foreach (IDropDownItemWithTooltip item in items)
                {
                    if (item.Value() == additionalItem.Value())
                        found = true;
                }
                if (!found)
                    items.Add(additionalItem);
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<select id='" + id + "'");

            if (!String.IsNullOrEmpty(onChangeHandler))
            {
                sb.Append(" onchange='" + onChangeHandler + "'");
            }

            if (!String.IsNullOrEmpty(attributes))
            {
                sb.Append(" " + attributes + " ");
            }

            sb.Append(">");

            if (hasChoose)
            {
                ListItem chooseOne = GetOptionChooseOne();
                sb.Append(@"<option value=""" + chooseOne.Value + @""">" + chooseOne.Text + @"</option>");
            }

            if (items != null)
            {
                foreach (IDropDownItemWithTooltip item in items)
                {
                    if (selectedItem != null && selectedItem.Value() == item.Value())
                    {
                        sb.Append(@"<option value=""" + item.Value() + @""" selected='selected' title=""" + CommonFunctions.HtmlEncoding(item.Tooltip()) + @""" >" + item.Text() + @"</option>");
                    }
                    else
                    {
                        sb.Append(@"<option value=""" + item.Value() + @""" title=""" + CommonFunctions.HtmlEncoding(item.Tooltip()) + @""" >" + item.Text() + @"</option>");
                    }
                }
            }

            sb.Append("</select>");

            return sb.ToString();
        }

        public static void SetTextAsTooltip(params DropDownList[] dropDowns)
        {
            for (int i = 0; i < dropDowns.Length; i++)
            {
                for (int j = 0; j < dropDowns[i].Items.Count; j++)
                {
                    dropDowns[i].Items[j].Attributes.Add("title", dropDowns[i].Items[j].Text);
                }
            }
        }
    }

    public interface IDropDownItem
    {
        string Text();
        string Value();
    }

    public class DropDownItem : IDropDownItem
    {
        private string text;
        private string val;

        public string Txt
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public string Val
        {
            get
            {
                return val;
            }

            set
            {
                val = value;
            }
        }

        public string Text()
        {
            return Txt;
        }

        public string Value()
        {
            return Val;
        }
    }

    public interface IDropDownItemWithTooltip
    {
        string Text();
        string Value();
        string Tooltip();
    }
}

