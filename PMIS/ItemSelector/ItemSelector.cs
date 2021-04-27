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

/*
 * http://msdn.microsoft.com/en-us/library/yhzc935f.aspx
 * http://weblogs.asp.net/dwahlin/archive/2007/04/29/creating-custom-asp-net-server-controls-with-embedded-javascript.aspx
 * UniqueID vs. ClientID http://stackoverflow.com/questions/1612016/c-asp-net-why-is-there-a-difference-between-clientid-and-uniqueid
 */

namespace ItemSelector
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ItemSelector runat=server></{0}:ItemSelector>")]
    public class ItemSelector : WebControl, IPostBackDataHandler
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
                    selectedValue = "";

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

        private string divFullListTitle = null;
        public string DivFullListTitle
        {
            get { return divFullListTitle != null ? divFullListTitle : ""; }
            set { divFullListTitle = value; }
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

        private string onBeforeFullList = null;
        public string OnBeforeFullList
        {
            get { return onBeforeFullList != null ? onBeforeFullList : ""; }
            set { onBeforeFullList = value; }
        }

        private bool unsavedCheckSkipMe = false;
        public bool UnsavedCheckSkipMe
        {
            get { return unsavedCheckSkipMe; }
            set { unsavedCheckSkipMe = value; }
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
            String s13 = (String)Context.Request.Form["hdn" + postDataKey + "OnEndOfSelection"];
            String s14 = (String)Context.Request.Form["hdn" + postDataKey + "OnBeforeFullList"];

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
                onEndOfSelection = s13;
                onBeforeFullList = s14;

                return true;
            }

            return false;
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            string unsavedCheckSkipMeString = "";
            if (UnsavedCheckSkipMe)
                unsavedCheckSkipMeString = "UnsavedCheckSkipMe='true'";

            string disabled = "";
            if (!this.Enabled)
                disabled = " disabled='disabled' ";

            string style = "";
            if (this.Style["display"] == "none")
                style = " style='display: none;' ";

            string html = @"<input type=""hidden"" id=""" + ClientID + @""" name=""" + UniqueID + @""" value="""" />" +
                          @"<div class=""" + DivMainCss + @""" ><table><tr><td><input type=""text"" onfocus=""ItemSelectorUtil.Focus(this);"" onblur=""ItemSelectorUtil.Blur(this);"" onkeydown=""return ItemSelectorUtil.KeyPressDown(this, event);"" onkeyup=""return ItemSelectorUtil.KeyPressUp(this, event);"" id=""txt" + ClientID + @""" name=""txt" + UniqueID + @""" value=""" + HttpContext.Current.Server.HtmlEncode(SelectedText) + @""" autocomplete=""off""  " + unsavedCheckSkipMeString + disabled + style + " /></td>" +
                          @"<td><div id=""img" + ClientID + @""" class=""ViewListButton"" onclick=""ItemSelectorUtil.DisplayFullList('txt" + ClientID + @"', 1);"" style= ""display: " + (this.Enabled && this.Style["display"] != "none" ? "inline-block" : "none") + @";""></div></td></tr></table></div>" +
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
                          @"<input type=""hidden"" id=""hdn" + ClientID + @"OnBeforeList"" name=""hdn" + UniqueID + @"OnBeforeList"" value=""" + OnBeforeList + @""" />" +
                          @"<input type=""hidden"" id=""hdn" + ClientID + @"OnEndOfSelection"" name=""hdn" + UniqueID + @"OnEndOfSelection"" value=""" + OnEndOfSelection + @""" />" +
                          @"<input type=""hidden"" id=""hdn" + ClientID + @"OnBeforeFullList"" name=""hdn" + UniqueID + @"OnBeforeFullList"" value=""" + OnBeforeFullList + @""" />";

            output.Write(html);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string resourceName = "ItemSelector.ItemSelector.js";

            ClientScriptManager cs = this.Page.ClientScript;
            cs.RegisterClientScriptResource(typeof(ItemSelector), resourceName);
        }
    }
}
