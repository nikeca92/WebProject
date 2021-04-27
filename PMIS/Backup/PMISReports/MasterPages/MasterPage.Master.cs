using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using PMIS.Common;
using PMIS.PMISReports.Common;
using System.Collections.Generic;

namespace PMIS.PMISReports.MasterPages
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public List<string> menuItemsHighlighted = new List<string>();
        public string[] specificUIKeys = new string[] { "REP_EDITPROFILE" };

        public string ShowPopupForSaveChanges = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionTimer.InitSession();
            if (!Page.IsPostBack && !Request.Url.ToString().ToUpper().Contains("LOGIN.ASPX"))
            {
                if (Session["sessionTime"] != null && !string.IsNullOrEmpty(Session["sessionTime"].ToString()))
                {
                    hdnSessionTimeout.Value = "";
                }
                else
                {
                    hdnSessionTimeout.Value = (Session.Timeout - 1).ToString();
                }

                hdnInitialTime.Value = Session["InitialSessionTime"].ToString();
                Session["sessionTime"] = DateTime.Now.Month + ":" + DateTime.Now.Day + ":" + DateTime.Now.Year + ":" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ":" + DateTime.Now.Millisecond;
            }

            if (Request.Url.ToString().ToUpper().Contains("LOGIN.ASPX") || Request.Url.ToString().ToUpper().Contains("LOGOUT.ASPX"))
            {
                Session["InitialSessionTime"] = null;
            }

            if (ConfigurationManager.AppSettings["ShowPopupForSaveChanges"] != null)
                ShowPopupForSaveChanges = ConfigurationManager.AppSettings["ShowPopupForSaveChanges"];

            if (!Request.Url.ToString().ToUpper().Contains("LOGIN.ASPX") && !Request.Url.ToString().ToUpper().Contains("LOGOUT.ASPX") &&
                !Request.Url.ToString().ToUpper().Contains("PASSWORDEXPIRED.ASPX"))
            {
                LoadMenu();
            }

            int asyncPostBackTimeout = 0;

            try
            {
                asyncPostBackTimeout = int.Parse(Config.GetWebSetting("AsyncPostBackTimeout"));
            }
            catch
            {
                asyncPostBackTimeout = 0;
            }

            if (asyncPostBackTimeout > 0)
                ScriptManager1.AsyncPostBackTimeout = asyncPostBackTimeout;
        }

        //Load the Menu
        //We use the HoverMenuExtender object from the AjaxControlToolkit library. It is used to connect a partciular menu item (actually a table cell) to its hover panel (actually a div)
        //For each menu item we specify a key (used to have unique IDs), a label and a URL (the page that would be opened from the specific menu item)
        private void LoadMenu()
        {
            if (!(Page is REPPage))
                return;

            REPPage page = page = (REPPage)Page;

            page.LoadSpecificUIItems(specificUIKeys);

            //Define the specific propertied of the menu item
            string subItems = "";
            string itemCss = "subMenuItems";

            string key = "Home";
            string menuItemURL = "~/ContentPages/Home.aspx";
            string tdID = "td" + key;

            HtmlTableCell td = new HtmlTableCell();
            td.ID = tdID;
            td.InnerHtml = "<img src='../Images/m_Home.png' class='MenuBarIcon' title='Начало' />";
            td.Attributes.Add("class", "mainMenuItems");
            td.Attributes.Add("style", "padding: 0px; padding-left: 8px; padding-right: 8px;");

            //Highlight the menu item for the currently loaded screen
            if (menuItemsHighlighted.Contains(td.ID.Substring(2)))
                td.Attributes.Add("class", "MainMenuItemHighlighted");

            if (menuItemURL.Contains("~"))
                menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

            //The redirect is implemented on the client side by the MenuItemClick JS function
            td.Attributes.Add("onclick", "MenuItemClick('" + menuItemURL + "');");

            rowMenuItems.Cells.Add(td);

            string pnlID = "pnl" + key;

            //This is the hover panel that should apppear. However, this one is empty (i.e. the Home link) and 
            //this is why the panel is empty and nothing would appear
            Panel pnl = new Panel();
            pnl.ID = pnlID;
            pnl.CssClass = "popUpMenu";

            HtmlGenericControl div = new HtmlGenericControl();

            HoverPanelsCont.Controls.Add(pnl);

            //Setup the HoverMenuExtender to link the menu item with the hover panel
            HoverMenuExtender hoverMenuItem = new HoverMenuExtender();
            hoverMenuItem.ID = key;
            hoverMenuItem.TargetControlID = tdID;
            hoverMenuItem.PopupControlID = pnlID;
            hoverMenuItem.OffsetX = 5;
            hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
            hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
            hoverMenuItem.PopDelay = 10;
            hoverMenuItem.HoverCssClass = "hoveredMenuItem";

            HoverExtenderCont.Controls.Add(hoverMenuItem);

            if (page.GetUIItemAccessLevel("REP_ADDEDITREPORT") == UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("REP_LISTREPORTS") != UIAccessLevel.Hidden)
            {
                //The same logic is for each specific menu item
                key = "Reports";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Справки";
                td.Attributes.Add("class", "mainMenuItems");

                if (menuItemsHighlighted.Contains(td.ID.Substring(2)))
                    td.Attributes.Add("class", "MainMenuItemHighlighted");

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                td.Attributes.Add("onclick", "MenuItemClick('" + menuItemURL + "');");

                rowMenuItems.Cells.Add(td);

                pnlID = "pnl" + key;

                pnl = new Panel();
                pnl.ID = pnlID;
                pnl.CssClass = "popUpMenu";

                subItems = "";

                if (page.GetUIItemAccessLevel("REP_ADDEDITREPORT") == UIAccessLevel.Enabled)
                {

                    key = "Reports_Add";
                    menuItemURL = "~/ContentPages/ReportDesigner.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нова справка</span>
                             </td>
                          </tr>";

                    itemCss = "subMenuItems";
                }

                if (page.GetUIItemAccessLevel("REP_LISTREPORTS") != UIAccessLevel.Hidden)
                {
                    key = "Reports_List";
                    menuItemURL = "~/ContentPages/ReportList.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък</span>
                             </td>
                          </tr>";
                }

                div = new HtmlGenericControl();
                div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                pnl.Controls.Add(div);

                HoverPanelsCont.Controls.Add(pnl);

                hoverMenuItem = new HoverMenuExtender();
                hoverMenuItem.ID = "ex" + key;
                hoverMenuItem.TargetControlID = tdID;
                hoverMenuItem.PopupControlID = pnlID;
                hoverMenuItem.OffsetX = 8;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);
            }


            if (page.GetUIItemAccessLevel("REP_SETTINGS") == UIAccessLevel.Enabled)
            {

                //The same logic is for each specific menu item
                key = "Settings";
                menuItemURL = "~/ContentPages/Settings.aspx";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Настройки";
                td.Attributes.Add("class", "mainMenuItems");

                if (menuItemsHighlighted.Contains(td.ID.Substring(2)))
                    td.Attributes.Add("class", "MainMenuItemHighlighted");

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                td.Attributes.Add("onclick", "MenuItemClick('" + menuItemURL + "');");

                rowMenuItems.Cells.Add(td);

                pnlID = "pnl" + key;

                pnl = new Panel();
                pnl.ID = pnlID;
                pnl.CssClass = "popUpMenu";

                HoverPanelsCont.Controls.Add(pnl);

                hoverMenuItem = new HoverMenuExtender();
                hoverMenuItem.ID = "ex" + key;
                hoverMenuItem.TargetControlID = tdID;
                hoverMenuItem.PopupControlID = pnlID;
                hoverMenuItem.OffsetX = -50;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);

            }                  



            if (page.GetUIItemAccessLevel("REP_EDITPROFILE") != UIAccessLevel.Hidden)
            {
                key = "UserProfile";
                menuItemURL = "~/ContentPages/EditUserProfile.aspx";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Профил";
                td.Attributes.Add("class", "mainMenuItems");

                if (menuItemsHighlighted.Contains(td.ID.Substring(2)))
                    td.Attributes.Add("class", "MainMenuItemHighlighted");

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                td.Attributes.Add("onclick", "MenuItemClick('" + menuItemURL + "');");

                rowMenuItems.Cells.Add(td);

                pnlID = "pnl" + key;

                pnl = new Panel();
                pnl.ID = pnlID;
                pnl.CssClass = "popUpMenu";

                HoverPanelsCont.Controls.Add(pnl);

                hoverMenuItem = new HoverMenuExtender();
                hoverMenuItem.ID = "ex" + key;
                hoverMenuItem.TargetControlID = tdID;
                hoverMenuItem.PopupControlID = pnlID;
                hoverMenuItem.OffsetX = 7;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);
            }


            key = "Logout";
            menuItemURL = "~/ContentPages/Logout.aspx";
            tdID = "td" + key;

            td = new HtmlTableCell();
            td.ID = tdID;
            td.InnerHtml = "Изход";
            td.Attributes.Add("class", "mainMenuItems");

            if (menuItemsHighlighted.Contains(td.ID.Substring(2)))
                td.Attributes.Add("class", "MainMenuItemHighlighted");

            if (menuItemURL.Contains("~"))
                menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

            td.Attributes.Add("onclick", "MenuItemClick('" + menuItemURL + "');");

            rowMenuItems.Cells.Add(td);

            pnlID = "pnl" + key;

            pnl = new Panel();
            pnl.ID = pnlID;
            pnl.CssClass = "popUpMenu";

            HoverPanelsCont.Controls.Add(pnl);

            hoverMenuItem = new HoverMenuExtender();
            hoverMenuItem.ID = "ex" + key;
            hoverMenuItem.TargetControlID = tdID;
            hoverMenuItem.PopupControlID = pnlID;
            hoverMenuItem.OffsetX = 7;
            hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
            hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
            hoverMenuItem.PopDelay = 10;
            hoverMenuItem.HoverCssClass = "hoveredMenuItem";

            HoverExtenderCont.Controls.Add(hoverMenuItem);
        }

        protected string GetDecimalPoint()
        {
            return Config.GetWebSetting("DecimalPoint");
        }       

        public string LocationHashClientID
        {
            get
            {
                Control hdnLocationHash = CommonFunctions.CustomFindControl(Page, "hdnLocationHash");

                if (hdnLocationHash != null)
                    return hdnLocationHash.ClientID;
                else
                    return "";
            }
        }
    }
}