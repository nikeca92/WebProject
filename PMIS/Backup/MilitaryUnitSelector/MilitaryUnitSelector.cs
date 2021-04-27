using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using PMIS.Common;
using System.IO;

/*
 * http://msdn.microsoft.com/en-us/library/yhzc935f.aspx
 * http://weblogs.asp.net/dwahlin/archive/2007/04/29/creating-custom-asp-net-server-controls-with-embedded-javascript.aspx
 * UniqueID vs. ClientID http://stackoverflow.com/questions/1612016/c-asp-net-why-is-there-a-difference-between-clientid-and-uniqueid
 */

namespace MilitaryUnitSelector
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MilitaryUnitSelector runat=server></{0}:MilitaryUnitSelector>")]
    public class MilitaryUnitSelector : WebControl, IPostBackDataHandler
    {
        private string selectedText = null;
        public string SelectedText
        {
            get
            {
                if (selectedText == null)
                    selectedText = "";

                return selectedText;
            }

            set
            {
                selectedText = value;
            }
        }

        private string selectedValue = null;
        public string SelectedValue
        {
            get
            {
                if (selectedValue == null)
                    selectedValue = "-1";

                return selectedValue;
            }

            set
            {
                selectedValue = value;
            }
        }

        private string dataSourceWebPage = null;
        public string DataSourceWebPage
        {
            get
            {
                if (dataSourceWebPage == null)
                    dataSourceWebPage = "";

                return dataSourceWebPage;
            }

            set
            {
                dataSourceWebPage = value;
            }
        }

        private string dataSourceKey = null;
        public string DataSourceKey
        {
            get
            {
                if (dataSourceKey == null)
                    dataSourceKey = "";

                return dataSourceKey;
            }

            set
            {
                dataSourceKey = value;
            }
        }

        private int resultMaxCount = 100; //Default
        public int ResultMaxCount
        {
            get
            {
                return resultMaxCount;
            }

            set
            {
                resultMaxCount = value;
            }
        }

        private bool onlyListValues = true; //Default
        public bool OnlyListValues
        {
            get
            {
                return onlyListValues;
            }

            set
            {
                onlyListValues = value;
            }
        }

        private int pageCount = 20; //Default
        public int PageCount
        {
            get
            {
                return pageCount;
            }

            set
            {
                pageCount = value;
            }
        }

        private string divListCss = null;
        public string DivListCss
        {
            get { return divListCss != null ? divListCss : ""; }
            set { divListCss = value; }
        }

        private string divFullListCss = null;
        public string DivFullListCss
        {
            get { return divFullListCss != null ? divFullListCss : ""; }
            set { divFullListCss = value; }
        }

        private string divMainCss = null;
        public string DivMainCss
        {
            get { return divMainCss != null ? divMainCss : ""; }
            set { divMainCss = value; }
        }

        private string divFullListTitle = "ВПН/Структура"; //Default
        public string DivFullListTitle
        {
            get { return divFullListTitle != null ? divFullListTitle : ""; }
            set { divFullListTitle = value; }
        }

        private int dropDownLimit = 15; //Default
        public int DropDownLimit
        {
            get { return dropDownLimit; }
            set { dropDownLimit = value; }
        }

        private string onBeforeList = null;
        public string OnBeforeList
        {
            get { return onBeforeList != null ? onBeforeList : ""; }
            set { onBeforeList = value; }
        }

        private string onEndOfSelection = null;
        public string OnEndOfSelection
        {
            get { return onEndOfSelection != null ? onEndOfSelection : ""; }
            set { onEndOfSelection = value; }
        }

        private bool unsavedCheckSkipMe = false;
        public bool UnsavedCheckSkipMe
        {
            get { return unsavedCheckSkipMe; }
            set { unsavedCheckSkipMe = value; }
        }

        private bool allowPickUpNodesWithoutVPN = false; //Default
        public bool AllowPickUpNodesWithoutVPN
        {
            get
            {
                return allowPickUpNodesWithoutVPN;
            }

            set
            {
                allowPickUpNodesWithoutVPN = value;
            }
        }

        private bool includeOnlyActual = true; //Default
        public bool IncludeOnlyActual
        {
            get
            {
                return includeOnlyActual;
            }

            set
            {
                includeOnlyActual = value;
            }
        }

        public virtual void RaisePostDataChangedEvent()
        {

        }

        public virtual bool LoadPostData(string postDataKey, NameValueCollection values)
        {
            String s1 = (String)Context.Request.Form["text" + postDataKey];
            String s2 = (String)Context.Request.Form["hdn" + postDataKey];
            String s3 = (String)Context.Request.Form["hdn" + postDataKey + "DataSourceWebPage"];
            String s4 = (String)Context.Request.Form["hdn" + postDataKey + "DataSourceKey"];
            String s5 = (String)Context.Request.Form["hdn" + postDataKey + "ResultMaxCount"];
            String s6 = (String)Context.Request.Form["hdn" + postDataKey + "DivListCss"];
            String s7 = (String)Context.Request.Form["hdn" + postDataKey + "DivFullListCss"];
            String s8 = (String)Context.Request.Form["hdn" + postDataKey + "DivMainCss"];
            String s9 = (String)Context.Request.Form["hdn" + postDataKey + "PageCount"];
            String s10 = (String)Context.Request.Form["hdn" + postDataKey + "DivFullListTitle"];
            String s11 = (String)Context.Request.Form["hdn" + postDataKey + "OnlyListValues"];
            String s12 = (String)Context.Request.Form["hdn" + postDataKey + "OnBeforeList"];
            String s13 = (String)Context.Request.Form["hdn" + postDataKey + "DropDownLimit"];
            String s14 = (String)Context.Request.Form["hdn" + postDataKey + "AllowPickUpNodesWithoutVPN"];
            String s15 = (String)Context.Request.Form["hdn" + postDataKey + "OnEndOfSelection"];
            String s16 = (String)Context.Request.Form["hdn" + postDataKey + "IncludeOnlyActual"];

            if (s1 != null)
            {
                selectedText = s1;
                selectedValue = s2;
                dataSourceWebPage = s3;
                dataSourceKey = s4;
                resultMaxCount = int.Parse(s5);
                divListCss = s6;
                divFullListCss = s7;
                divMainCss = s8;
                pageCount = int.Parse(s9);
                divFullListTitle = s10;
                onlyListValues = bool.Parse(s11);
                onBeforeList = s12;
                dropDownLimit = int.Parse(s13);
                allowPickUpNodesWithoutVPN = bool.Parse(s14);
                onEndOfSelection = s15;
                includeOnlyActual = bool.Parse(s16);

                return true;
            }

            return false;
        }

        private string GenerateMilitaryUnitDropDownHtml(List<MilitaryUnit> militaryUnits)
        {
            StringBuilder sb = new StringBuilder();

            string unsavedCheckSkipMeString = "";
            if (UnsavedCheckSkipMe)
                unsavedCheckSkipMeString = "UnsavedCheckSkipMe='true'";

            string disabled = "";
            if (!this.Enabled)
                disabled = " disabled='disabled' ";

            string style = "";
            if (this.Style["display"] == "none")
                style = " style='display: none;' ";

            sb.Append(@"<select id=""" + "dd" + ClientID + @""" onchange=""MilitaryUnitSelectorUtil.DropDownChanged('" + ClientID + @"');"" " + unsavedCheckSkipMeString + disabled + style + " >");

            bool hasSelection = false;
            foreach (MilitaryUnit unit in militaryUnits)
            {
                string selected = "";
                string text = unit.DisplayTextForSelection;
                if (SelectedValue == unit.MilitaryUnitId.ToString())
                {
                    hasSelection = true;
                    selected = @" selected=""selected"" ";
                }
                sb.Append(@"<option value=""" + unit.MilitaryUnitId + @"""" + selected + @">" + text + @"</option>");
            }

            if (!hasSelection && militaryUnits.Count > 0)
            {
                MilitaryUnit firstUnit = militaryUnits.First();
                SelectedValue = firstUnit.MilitaryUnitId.ToString();
                SelectedText = firstUnit.DisplayTextForSelection;
            }
            else if (militaryUnits.Count == 0)
            {
                SelectedValue = "-1";
                SelectedText = "";
            }
            
            sb.Append("</select>");

            return sb.ToString();
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            string html = "";

            string unsavedCheckSkipMeString = "";
            if (UnsavedCheckSkipMe)
                unsavedCheckSkipMeString = "UnsavedCheckSkipMe='true'";

            User currentUser = null;
            if (Page is BasePage)
                currentUser = (Page as BasePage).CurrentUser;

            if (currentUser is TestBasePage)
                currentUser = (Page as TestBasePage).CurrentUser;

            string disabled = "";
            if (!this.Enabled)
                disabled = " disabled='disabled' ";

            string style = "";
            if (this.Style["display"] == "none")
                style = " style='display: none;' ";

            int militaryUnitsCount = MilitaryUnitUtil.GetAllMilitaryUnitsCount(currentUser);

            if (dropDownLimit > militaryUnitsCount)
            {
                List<MilitaryUnit> units = MilitaryUnitUtil.GetAllMilitaryUnits(currentUser);

                //Add a blank element in the drop-down list
                MilitaryUnit blankUnit = new MilitaryUnit(currentUser);
                blankUnit.MilitaryUnitId = -1;
                blankUnit.VPN = "";
                blankUnit.ShortName = "";
                units.Insert(0, blankUnit);

                html += GenerateMilitaryUnitDropDownHtml(units);
            }
            else
            {
                html += @"<div class=""" + DivMainCss + @""" ><table cellpadding='0' cellspacing='0'><tr><td><input type=""text"" onfocus=""MilitaryUnitSelectorUtil.Focus(this);"" onblur=""MilitaryUnitSelectorUtil.Blur(this);"" onkeydown=""return MilitaryUnitSelectorUtil.KeyPressDown(this, event);"" onkeyup=""return MilitaryUnitSelectorUtil.KeyPressUp(this, event);"" id=""txt" + ClientID + @""" name=""txt" + UniqueID + @""" value=""" + HttpContext.Current.Server.HtmlEncode(SelectedText) + @""" autocomplete=""off""  " + unsavedCheckSkipMeString + disabled + style + " /></td>" +
                    @"<td><div id=""img" + ClientID + @""" class=""ViewListButton"" onclick=""MilitaryUnitSelectorUtil.DisplayFullList('txt" + ClientID + @"', 1);"" style= ""margin-left: 4px; display: " + (this.Enabled && this.Style["display"] != "none" ? "inline-block" : "none") + @";""></div></td></tr></table></div>";
            }

            html += @"<input type=""hidden"" id=""" + ClientID + @""" name=""" + UniqueID + @""" value="""" />" +
                    @"<input type=""hidden"" id=""text" + ClientID + @""" name=""text" + UniqueID + @""" value=""" + HttpContext.Current.Server.HtmlEncode(SelectedText) + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @""" name=""hdn" + UniqueID + @""" value=""" + SelectedValue + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"DataSourceWebPage"" name=""hdn" + UniqueID + @"DataSourceWebPage"" value=""" + DataSourceWebPage + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"DataSourceKey"" name=""hdn" + UniqueID + @"DataSourceKey"" value=""" + DataSourceKey + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"ResultMaxCount"" name=""hdn" + UniqueID + @"ResultMaxCount"" value=""" + ResultMaxCount + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"OnlyListValues"" name=""hdn" + UniqueID + @"OnlyListValues"" value=""" + OnlyListValues + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"DivListCss"" name=""hdn" + UniqueID + @"DivListCss"" value=""" + DivListCss + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"DivFullListCss"" name=""hdn" + UniqueID + @"DivFullListCss"" value=""" + DivFullListCss + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"DivMainCss"" name=""hdn" + UniqueID + @"DivMainCss"" value=""" + DivMainCss + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"PageCount"" name=""hdn" + UniqueID + @"PageCount"" value=""" + PageCount + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"DivFullListTitle"" name=""hdn" + UniqueID + @"DivFullListTitle"" value=""" + DivFullListTitle + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"DropDownLimit"" name=""hdn" + UniqueID + @"DropDownLimit"" value=""" + DropDownLimit + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"OnBeforeList"" name=""hdn" + UniqueID + @"OnBeforeList"" value=""" + OnBeforeList + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"AllowPickUpNodesWithoutVPN"" name=""hdn" + UniqueID + @"AllowPickUpNodesWithoutVPN"" value=""" + AllowPickUpNodesWithoutVPN + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"OnEndOfSelection"" name=""hdn" + UniqueID + @"OnEndOfSelection"" value=""" + OnEndOfSelection + @""" />" +
                    @"<input type=""hidden"" id=""hdn" + ClientID + @"IncludeOnlyActual"" name=""hdn" + UniqueID + @"IncludeOnlyActual"" value=""" + IncludeOnlyActual + @""" />";

            output.Write(html);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string resourceName = "MilitaryUnitSelector.MilitaryUnitSelector.js";

            ClientScriptManager cs = this.Page.ClientScript;
            cs.RegisterClientScriptResource(typeof(MilitaryUnitSelector), resourceName);
        }      
    }
}