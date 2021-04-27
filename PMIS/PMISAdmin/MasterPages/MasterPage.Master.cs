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
using PMIS.PMISAdmin.Common;
using System.Collections.Generic;

namespace PMIS.PMISAdmin.MasterPages
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public List<string> menuItemsHighlighted = new List<string>();
        public string[] specificUIKeys = new string[] { "ADM_AUDITTRAIL", "ADM_SECURITY", "ADM_SECURITY_ROLES", "ADM_SECURITY_ROLES_ADDROLE",
          "ADM_SECURITY_UIITEMSPERROLE", "ADM_SECURITY_USERS", "ADM_SECURITY_USERS_ADDUSER", "ADM_EDITPROFILE", "ADM_LISTMAINT_MILITFORCETYPES", 
          "ADM_LISTMAINT_MILITARYDEPARTMENTS", "ADM_LISTMAINT_ADMINISTRATIONS", "ADM_LISTMAINT_DRIVINGLICENSECATEGORIES", "ADM_LISTMAINT_MILREPORTSPECIALITY" };

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
            if (!(Page is AdmPage))
                return;

            AdmPage page = (AdmPage)Page;

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

            //The same logic is for each specific menu item

            if (page.GetUIItemAccessLevel("ADM_AUDITTRAIL") != UIAccessLevel.Hidden)
            {
                key = "Audit";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Одит";
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

                key = "Audit_AuditTrail";
                menuItemURL = "~/ContentPages/AuditTrail.aspx";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                //Here we form the HTML of the hover panel for the specific main menu item
                subItems += @"<tr>
                                 <td style='vertical-align: top;'>
                                    <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Одитни записи</span>
                                 </td>
                              </tr>";

                if (page.GetUIItemAccessLevel("ADM_LOGINLOG") != UIAccessLevel.Hidden)
                {

                    key = "Audit_LoginLog";
                    menuItemURL = "~/ContentPages/LoginLog.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                 <td style='vertical-align: top;'>
                                    <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Потребителски сесии</span>
                                 </td>
                              </tr>";
                }

                if (page.GetUIItemAccessLevel("ADM_FAILEDLOGINS") != UIAccessLevel.Hidden)
                {

                    key = "Audit_FailedLogins";
                    menuItemURL = "~/ContentPages/FailedLogins.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                 <td style='vertical-align: top;'>
                                    <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Неуспешни опити за достъп</span>
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
                hoverMenuItem.OffsetX = 5;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);
            }


            if (page.GetUIItemAccessLevel("ADM_SECURITY") != UIAccessLevel.Hidden)
            {
                key = "Users";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Потребители и сигурност";
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

                if (page.GetUIItemAccessLevel("ADM_SECURITY_ROLES") != UIAccessLevel.Hidden)
                {
                    key = "Users_ManageRoles";
                    menuItemURL = "~/ContentPages/ManageRoles.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Управление на потребителските роли</span>
                                     </td>
                                  </tr>";


                    if (page.GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("ADM_SECURITY_ROLES") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("ADM_SECURITY_ROLES_ADDROLE") == UIAccessLevel.Enabled)
                    {
                        key = "Users_AddRole";
                        menuItemURL = "~/ContentPages/AddEditRole.aspx";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                         <td style='vertical-align: top;'>
                                            <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нова потребителска роля</span>
                                         </td>
                                      </tr>";
                    }
                }

                if (page.GetUIItemAccessLevel("ADM_SECURITY_UIITEMSPERROLE") != UIAccessLevel.Hidden)
                {
                    key = "Users_UIItemsPerRole";
                    menuItemURL = "~/ContentPages/UIItemsPerRole.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Дефиниране на права и достъп според потребителската роля</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("ADM_SECURITY_USERS") != UIAccessLevel.Hidden)
                {
                    key = "Users_ManageUsers";
                    menuItemURL = "~/ContentPages/ManageUsers.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Управление на потребителите</span>
                                     </td>
                                  </tr>";


                    if (page.GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("ADM_SECURITY_USERS") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER") == UIAccessLevel.Enabled)
                    {
                        key = "Users_AddUser";
                        menuItemURL = "~/ContentPages/AddEditUser.aspx";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                         <td style='vertical-align: top;'>
                                            <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов потребител</span>
                                         </td>
                                      </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Enabled &&
                        page.GetUIItemAccessLevel("ADM_PASSWORDPOLICY") == UIAccessLevel.Enabled)
                    {
                        key = "Users_PasswordPolicy";
                        menuItemURL = "~/ContentPages/PasswordPolicy.aspx";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                         <td style='vertical-align: top;'>
                                            <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Пароли</span>
                                         </td>
                                      </tr>";
                    }
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
                hoverMenuItem.OffsetX = 5;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);

                if (page.GetUIItemAccessLevel("ADM_LISTMAINT") != UIAccessLevel.Hidden &&
                    (page.GetUIItemAccessLevel("ADM_LISTMAINT_MILITFORCETYPES") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("ADM_LISTMAINT_MILITARYDEPARTMENTS") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("ADM_LISTMAINT_ADMINISTRATIONS") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("ADM_LISTMAINT_DRIVINGLICENSECATEGORIES") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY") != UIAccessLevel.Hidden))
                {
                    key = "Lists";
                    menuItemURL = "";
                    tdID = "td" + key;

                    td = new HtmlTableCell();
                    td.ID = tdID;
                    td.InnerHtml = "Класификатори";
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


                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_MILITFORCETYPES") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_MilitaryForceType";
                        menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=ADM_MilitaryForceType";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Видове войски</span>
                                     </td>
                                  </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_MILITARYDEPARTMENTS") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_MilitaryDepartment";
                        menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=ADM_MilitaryDepartment";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Военни окръжия</span>
                                     </td>
                                  </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_ADMINISTRATIONS") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_Administrations";
                        menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=ADM_Administrations";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Ведомства</span>
                                     </td>
                                  </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_DRIVINGLICENSECATEGORIES") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_DrivingLicenseCategories";
                        menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=ADM_DrivingLicenseCategories";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Шоф. книжка - категории</span>
                                     </td>
                                  </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_MilRepSpecialities";
                        menuItemURL = "~/ContentPages/ManageMilRepSpecialities.aspx";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">ВОС</span>
                                     </td>
                                  </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_POSITIONTITLES") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_PositionTitles";
                        menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=ADM_PositionTitles";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Длъжности</span>
                                     </td>
                                  </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_MEDRUBRICS") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_MedRubrics";
                        menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=ADM_MedRubrics";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Медицински рубрики</span>
                                     </td>
                                  </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_MILITARYSTRUCTURES") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_MilitaryStructures";
                        menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=ADM_MilitaryStructures";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Структури</span>
                                     </td>
                                  </tr>";
                    }

                    if (page.GetUIItemAccessLevel("ADM_LISTMAINT_MILITARYFORCESORTS") != UIAccessLevel.Hidden)
                    {
                        key = "Lists_ADM_MilitaryForceSorts";
                        menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=ADM_MilitaryForceSorts";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Родове войски</span>
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
                    hoverMenuItem.OffsetX = 5;
                    hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                    hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                    hoverMenuItem.PopDelay = 10;
                    hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                    HoverExtenderCont.Controls.Add(hoverMenuItem);
                }
            }


            if (page.GetUIItemAccessLevel("ADM_EDITPROFILE") != UIAccessLevel.Hidden)
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

        public string ErrorMessageNoRightsFieldTemplate
        {
            get
            {
                return CommonFunctions.ErrorMessageNoRightsFieldTemplate;
            }
        }

        public string ErrorMessageNoRightsFieldsTemplate
        {
            get
            {
                return CommonFunctions.ErrorMessageNoRightsFieldsTemplate;
            }
        }

        public string ErrorMessageMandatoryTemplate
        {
            get
            {
                return CommonFunctions.ErrorMessageMandatoryTemplate;
            }
        }

        public string ErrorMessageDateTemplate
        {
            get
            {
                return CommonFunctions.ErrorMessageDateTemplate;
            }
        }

        public string ErrorMessageNumberTemplate
        {
            get
            {
                return CommonFunctions.ErrorMessageNumberTemplate;
            }
        }

        public string ErrorMessageMandatoryColumnTemplate
        {
            get
            {
                return CommonFunctions.ErrorMessageMandatoryColumnTemplate;
            }
        }

        public string ErrorMessageDateColumnTemplate
        {
            get
            {
                return CommonFunctions.ErrorMessageDateColumnTemplate;
            }
        }

        public string ErrorMessageNumberColumnTemplate
        {
            get
            {
                return CommonFunctions.ErrorMessageNumberColumnTemplate;
            }
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