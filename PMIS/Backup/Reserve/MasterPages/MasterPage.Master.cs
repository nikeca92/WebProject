using System;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.MasterPages
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public List<string> menuItemsHighlighted = new List<string>();
        public string[] specificUIKeys = new string[] { "RES_EDITPROFILE", 
            "RES_EQUIPRESREQUESTS", "RES_EQUIPRESREQUESTS_ADD", 
            "RES_EQUIPTECHREQUESTS", "RES_EQUIPTECHREQUESTS_ADD", 
            "RES_HUMANRES", "RES_HUMANRES_ADDRESERVIST_PERSONALDATA",
            "RES_TECHNICS", 
            "RES_TECHNICS_VEHICLES", "RES_TECHNICS_TRAILERS", "RES_TECHNICS_TRCTORS",
            "RES_TECHNICS_ENG_EQUIP", "RES_TECHNICS_MOB_LIFT_EQUIP", "RES_TECHNICS_RAILWAY_EQUIP",
            "RES_TECHNICS_AVIATION_EQUIP", "RES_TECHNICS_VESSELS", "RES_TECHNICS_FUEL_CONTAINERS",
            "RES_TECHNICS_VEHICLES_ADD", "RES_TECHNICS_TRAILERS_ADD", "RES_TECHNICS_TRCTORS_ADD",
            "RES_TECHNICS_ENG_EQUIP_ADD", "RES_TECHNICS_MOB_LIFT_EQUIP_ADD", "RES_TECHNICS_RAILWAY_EQUIP_ADD",
            "RES_TECHNICS_AVIATION_EQUIP_ADD", "RES_TECHNICS_VESSELS_ADD", "RES_TECHNICS_FUEL_CONTAINERS_ADD",
            "RES_LISTMAINT", "RES_LISTMAINT_MILITARYREADINESSNAMES", 
            "RES_LISTMAINT_VEHICLEMAKENAMES", "RES_LISTMAINT_VEHICLEMODELS",
            "RES_LISTMAINT_TRACTORMAKENAMES", "RES_LISTMAINT_TRACTORMODELS",
            "RES_LISTMAINT_ENGEQUIPBASEMAKENAMES", "RES_LISTMAINT_ENGEQUIPBASEMODELS",
            "RES_LISTMAINT_AVIATIONOTHERBASEMACHINEMAKENAMES", "RES_LISTMAINT_AVIATIONOTHERBASEMACHINEMODELS",
            "RES_LISTMAINT_COMPANIES",
            "RES_REPORTS",
            "RES_REPORTS_LISTRESWITHAPPOINTMENTS", "RES_REPORTS_LISTTECHWITHAPPOINTMENTS", 
            "RES_REPORTS_REPORTLISTRESERVISTSFROMCOMMAND", "RES_REPORTS_REPORTLISTTECHNICSFROMCOMMAND",
            "RES_REPORTS_VEHICLES", "RES_REPORTS_TRAILERS",
            "RES_REPORTS_TRACTORS", "RES_REPORTS_ENG_EQUIP", "RES_REPORTS_MOB_LIFT_EQUIP",
            "RES_REPORTS_RAILWAY_EQUIP", "RES_REPORTS_AVIATION_EQUIP", "RES_REPORTS_VESSELS", 
            "RES_REPORTS_FUEL_CONTAINERS",
            "RES_REPORTS_REPORTSV1", "RES_REPORTS_REPORTA31", "RES_REPORTS_REPORTA33", "RES_REPORTS_REPORTA33v2",
            "RES_REPORTS_REPORTANALYZECOMMAND", "RES_REPORTS_REPORTANALYZETECHCOMMAND", "RES_REPORTS_REPORTNORMATIVETECHNICS",
            "RES_REPORTS_REPORTANALYZERESFULFILMENT", "RES_REPORTS_REPORTSTAFFPOSITIONLIST",
            "RES_PRINT", "RES_PRINT_RESERVISTS", "RES_PRINT_TECHNICS", "RES_PRINT_POSTPONE_RESERVISTS", "RES_PRINT_POSTPONE_TECHNICS",
            "RES_PRINT_RESERVISTS_MK", "RES_PRINT_RESERVISTS_PZ",
            "RES_PRINT_TECHNICS_MK", "RES_PRINT_TECHNICS_PZ",
            "RES_POSTPONE", "RES_POSTPONE_RES", "RES_POSTPONE_TECH", 
            "RES_POSTPONE_REPORT", "RES_POSTPONE_REPORT_RES", "RES_POSTPONE_REPORT_TECH", "RES_POSTPONE_REPORT_RES_BY_ADMINISTRATION", "RES_POSTPONE_REPORT_TECH_BY_ADMINISTRATION"
         };

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
            if (!(Page is RESPage))
                return;

            RESPage page = (RESPage)Page;

            page.LoadSpecificUIItems(specificUIKeys);            

            //Define the specific propertied of the menu item
            string subItems = "";
            string itemCss = "subMenuItems";
            string menuSeparator = "<tr><td style='height:3px;font-size:3px;'><hr/></td></tr>";

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



            if (page.GetUIItemAccessLevel("RES_HUMANRES") != UIAccessLevel.Hidden)
            {
                //The same logic is for each specific menu item
                key = "HumanResources";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Човешки ресурси";
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

                if (page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") == UIAccessLevel.Enabled)
                {
                    key = "AddNewReservist";
                    menuItemURL = "~/ContentPages/AddEditReservist.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Създаване на нов запис</span>
                             </td>
                          </tr>";

                    itemCss = "subMenuItems";
                }

                key = "ManageReservists";
                menuItemURL = "~/ContentPages/ManageReservists.aspx";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на водените на отчет</span>
                             </td>
                          </tr>";


                itemCss = "subMenuItems";
                
                key = "ImportPersonsData";
                menuItemURL = "~/ContentPages/ImportPersonsData.aspx";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Импорт на данни</span>
                             </td>
                          </tr>";

                itemCss = "subMenuItems";

                /*
                if (page.GetUIItemAccessLevel("RES_HUMANRES_POSTPONE") != UIAccessLevel.Hidden)
                {
                    key = "Postponement";
                    menuItemURL = "~/ContentPages/ManagePostpones.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Отсрочване</span>
                             </td>
                          </tr>";
                }

                itemCss = "subMenuItems";
                */

                if (page.GetUIItemAccessLevel("RES_HUMANRES_GROUPTAKINGDOWN") != UIAccessLevel.Hidden)
                {
                    key = "GroupTakingDown";
                    menuItemURL = "~/ContentPages/GroupTakingDown.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Групово снемане от отчет</span>
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


            if (page.GetUIItemAccessLevel("RES_TECHNICS") != UIAccessLevel.Hidden)
            {
                key = "Technics";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Техника";
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

                if (page is RESPage)
                {
                    List<TechnicsType> technicsTypes = TechnicsTypeUtil.GetAllTechnicsTypes(((RESPage)page).CurrentUser);

                    bool anyTechnics = false;

                    foreach (TechnicsType technicsType in technicsTypes)
                    {
                        if (page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsType.TypeKey) == UIAccessLevel.Hidden)
                            continue;

                        if (anyTechnics)
                        {
                            subItems += menuSeparator;
                        }

                        anyTechnics = true;

                        key = "Technics_" + technicsType.TypeKey;
                        menuItemURL = "~/ContentPages/ManageTechnics_" + technicsType.TypeKey + @".aspx";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                         <td style='vertical-align: top;'>
                                            <span class=""" + itemCss + @""" style='padding-top: 0px; padding-bottom: 0px; line-height: 11px;' onclick=""MenuItemClick('" + menuItemURL + @"');"">" + technicsType.TypeName + @"</span>
                                         </td>
                                      </tr>";



                        if (page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsType.TypeKey) == UIAccessLevel.Enabled &&
                            page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsType.TypeKey + "_ADD") == UIAccessLevel.Enabled)
                        {

                            key = "Technics_AddEdit_" + technicsType.TypeKey;
                            menuItemURL = "~/ContentPages/AddEditTechnics.aspx?TechnicsTypeKey=" + technicsType.TypeKey;
                            itemCss = "subMenuItems";

                            if (menuItemsHighlighted.Contains(key))
                                itemCss = "SubMenuItemHighlighted";

                            if (menuItemURL.Contains("~"))
                                menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                            //Here we form the HTML of the hover panel for the specific main menu item
                            subItems += @"<tr>
                                             <td style='vertical-align: top; padding-left: 15px;'>
                                                <span class=""" + itemCss + @""" style='padding-top: 0px; padding-bottom: 0px; line-height: 11px;' onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов запис</span>
                                             </td>
                                          </tr>";
                        }

                        key = "Technics_List_" + technicsType.TypeKey;
                        menuItemURL = "~/ContentPages/ManageTechnics_" + technicsType.TypeKey + @".aspx";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                         <td style='vertical-align: top; padding-left: 15px;'>
                                            <span class=""" + itemCss + @""" style='padding-top: 0px; padding-bottom: 0px; line-height: 11px;' onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък</span>
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
            }


            if (page.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") != UIAccessLevel.Hidden ||
                page.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") != UIAccessLevel.Hidden)
            {
                key = "Equipment";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Комплектоване";
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

                bool anyItemsShown = false;

                subItems = "";

                if (page.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD") == UIAccessLevel.Enabled)
                {
                    key = "AddEditEquipmentReservistsRequest";
                    menuItemURL = "~/ContentPages/AddEditEquipmentReservistsRequest.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Заявка за комплектоване с резервисти</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") != UIAccessLevel.Hidden)
                {
                    key = "ManageEquipmentReservistsRequests";
                    menuItemURL = "~/ContentPages/ManageEquipmentReservistsRequests.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Въведени заявки за комплектоване с резервисти</span>
                                     </td>
                                  </tr>";

                    itemCss = "subMenuItems";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL") != UIAccessLevel.Hidden)
                {

                    key = "ManageEquipmentReservistsRequestsFulfilment";
                    menuItemURL = "~/ContentPages/ManageEquipmentReservistsRequestsFulfilment.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Изпълнение на заявки за комплектоване с резервисти</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (anyItemsShown)
                {
                    subItems += menuSeparator;
                }

                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled &&
                    page.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD") == UIAccessLevel.Enabled)
                {
                    itemCss = "subMenuItems";

                    key = "AddEditEquipmentTechnicsRequest";
                    menuItemURL = "~/ContentPages/AddEditEquipmentTechnicsRequest.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Заявка за комплектоване с техника</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") != UIAccessLevel.Hidden)
                {
                    itemCss = "subMenuItems";

                    key = "ManageEquipmentTechnicsRequests";
                    menuItemURL = "~/ContentPages/ManageEquipmentTechnicsRequests.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Въведени заявки за комплектоване с техника</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL") != UIAccessLevel.Hidden)
                {
                    itemCss = "subMenuItems";

                    key = "ManageEquipmentTechnicsRequestsFulfilment";
                    menuItemURL = "~/ContentPages/ManageEquipmentTechnicsRequestsFulfilment.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Изпълнение на заявки за комплектоване с техника</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (anyItemsShown)
                {
                    subItems += menuSeparator;
                }

                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL") != UIAccessLevel.Hidden)
                {
                    itemCss = "subMenuItems";

                    key = "FulfilReservistsMilCommand";
                    menuItemURL = "~/ContentPages/FulfilReservistsMilCommand.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Комплектоване на команда с резервисти</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL") != UIAccessLevel.Hidden)
                {
                    itemCss = "subMenuItems";

                    key = "FulfilTechnicsMilCommand";
                    menuItemURL = "~/ContentPages/FulfilTechnicsMilCommand.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Комплектоване на команда с техника</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
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

            if (page.GetUIItemAccessLevel("RES_POSTPONE") != UIAccessLevel.Hidden &&
                (
                 (page.GetUIItemAccessLevel("RES_POSTPONE_RES") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_POSTPONE_TECH") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES_BY_ADMINISTRATION") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH_BY_ADMINISTRATION") != UIAccessLevel.Hidden)
                )
               )
            {
                key = "Postpone";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Отсрочване";
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
                bool anyItemsShown = false;

                // ================================================================================================
                // Subitems
                // ================================================================================================
                if (page.GetUIItemAccessLevel("RES_POSTPONE_RES") != UIAccessLevel.Hidden)
                {
                    key = "PostponeReservists";
                    menuItemURL = "~/ContentPages/PostponeRes.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Заявки за отсрочване на запасни</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_POSTPONE_TECH") != UIAccessLevel.Hidden)
                {
                    key = "PostponeTechnics";
                    menuItemURL = "~/ContentPages/PostponeTech.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Заявки за отсрочване на техника</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT") != UIAccessLevel.Hidden)
                {
                    key = "ReportPostpone";
                    menuItemURL = "~/ContentPages/ReportPostpone.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Протокол за изпълнение на отсрочването</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES") != UIAccessLevel.Hidden)
                {
                    key = "ReportPostponeRes";
                    menuItemURL = "~/ContentPages/ReportPostponeRes.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Протокол за изпълнение отсрочването на запасни</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH") != UIAccessLevel.Hidden)
                {
                    key = "ReportPostponeTech";
                    menuItemURL = "~/ContentPages/ReportPostponeTech.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Протокол за изпълнение отсрочването на техника</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES_BY_ADMINISTRATION") != UIAccessLevel.Hidden)
                {
                    key = "ReportPostponeResByAdministration";
                    menuItemURL = "~/ContentPages/ReportPostponeResByAdministration.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Отчет за изпълнение отсрочването на запасни</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH_BY_ADMINISTRATION") != UIAccessLevel.Hidden)
                {
                    key = "ReportPostponeTechByAdministration";
                    menuItemURL = "~/ContentPages/ReportPostponeTechByAdministration.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Отчет за изпълнение отсрочването на техника</span>
                                     </td>
                                  </tr>";

                    anyItemsShown = true;
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

            if (page.GetUIItemAccessLevel("RES_REPORTS") != UIAccessLevel.Hidden &&
                (page.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_POSTPONES") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_VEHICLES") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_TRAILERS") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_TRACTORS") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_ENG_EQUIP") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_MOB_LIFT_EQUIP") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_RAILWAY_EQUIP") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_AVIATION_EQUIP") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_VESSELS") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_FUEL_CONTAINERS") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTSV1") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTA31") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTA33") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZECOMMAND") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZETECHCOMMAND") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTNORMATIVETECHNICS") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTRESERVISTSFROMCOMMAND") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTTECHNICSFROMCOMMAND") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZERESFULFILMENT") != UIAccessLevel.Hidden) ||
                (page.GetUIItemAccessLevel("RES_REPORTS_REPORTSTAFFPOSITIONLIST") != UIAccessLevel.Hidden))
            {
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
                bool anyItemsShown = false;

                // ================================================================================================
                // Subitems
                // ================================================================================================
                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") != UIAccessLevel.Hidden)
                {
                    key = "ReportA33v2";
                    menuItemURL = "~/ContentPages/ReportA33v2.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Сведение-анализ за състоянието на ресурсите от резерва</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTA31") != UIAccessLevel.Hidden)
                {
                    key = "ReportA31";
                    menuItemURL = "~/ContentPages/ReportA31.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Сведение за планираните за доставяне запасни и техника - запас</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTSV1") != UIAccessLevel.Hidden)
                {
                    key = "ReportSV1";
                    menuItemURL = "~/ContentPages/ReportSV1.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Отчетна ведомост за състоянието на ресурсите</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTNORMATIVETECHNICS") != UIAccessLevel.Hidden)
                {
                    key = "ReportNormativeTechnics";
                    menuItemURL = "~/ContentPages/ReportNormativeTechnics.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Отчетна ведомост за състоянието на техниката</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTRESERVISTSFROMCOMMAND") != UIAccessLevel.Hidden)
                {
                    key = "ReportListReservistsFromCommand";
                    menuItemURL = "~/ContentPages/ReportListReservistsFromCommand.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на хората с МН от команда</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZECOMMAND") != UIAccessLevel.Hidden)
                {
                    key = "ReportAnalyzeCommand";
                    menuItemURL = "~/ContentPages/ReportAnalyzeCommand.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Сведение-анализ за комплектуването на команда</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZERESFULFILMENT") != UIAccessLevel.Hidden)
                {
                    key = "ReportAnalyzeResFulfilment";
                    menuItemURL = "~/ContentPages/ReportAnalyzeResFulfilment.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Анализ на комплектуването</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTTECHNICSFROMCOMMAND") != UIAccessLevel.Hidden)
                {
                    key = "ReportListTechnicsFromCommand";
                    menuItemURL = "~/ContentPages/ReportListTechnicsFromCommand.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на техниката с МН от команда</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZETECHCOMMAND") != UIAccessLevel.Hidden)
                {
                    key = "ReportAnalyzeTechCommand";
                    menuItemURL = "~/ContentPages/ReportAnalyzeTechCommand.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Сведение–анализ за комплектуване на команда с техника-запас</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                List<TechnicsType> technicsTypes = TechnicsTypeUtil.GetAllTechnicsTypes(((RESPage)page).CurrentUser);

                foreach (TechnicsType technicsType in technicsTypes)
                {
                    string techTypeKey = technicsType.TypeKey;

                    if (page.GetUIItemAccessLevel("RES_REPORTS_" + techTypeKey) != UIAccessLevel.Hidden)
                    {
                        key = "ReportTechnics_" + techTypeKey;
                        menuItemURL = "~/ContentPages/ReportTechnics_" + techTypeKey + ".aspx";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        //Here we form the HTML of the hover panel for the specific main menu item
                        subItems += @"<tr>
                                         <td style='vertical-align: top;'>
                                            <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на техниката на военен отчет - " + technicsType.TypeName + @"</span>
                                         </td>
                                      </tr>";
                        anyItemsShown = true;
                    }
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS") != UIAccessLevel.Hidden)
                {
                    key = "ReportListReservistsWithAppointments";
                    menuItemURL = "~/ContentPages/ReportListReservistsWithAppointments.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на хората с МН по определена заявка</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (page.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS") != UIAccessLevel.Hidden)
                {
                    key = "ReportListTechnicsWithAppointments";
                    menuItemURL = "~/ContentPages/ReportListTechnicsWithAppointments.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на техниката с МН по определена заявка</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTA33") != UIAccessLevel.Hidden)
                {
                    key = "ReportA33";
                    menuItemURL = "~/ContentPages/ReportA33.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Справка A33</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
                }

                if (anyItemsShown)
                    subItems += menuSeparator;
                anyItemsShown = false;

                if (page.GetUIItemAccessLevel("RES_REPORTS_REPORTSTAFFPOSITIONLIST") != UIAccessLevel.Hidden)
                {
                    key = "ReportStaffPositionsList";
                    menuItemURL = "~/ContentPages/ReportStaffPositionsList.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Щатно-длъжностен списък</span>
                                     </td>
                                  </tr>";
                    anyItemsShown = true;
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

            if (page.GetUIItemAccessLevel("RES_PRINT") != UIAccessLevel.Hidden &&
                (
                 (page.GetUIItemAccessLevel("RES_PRINT_RESERVISTS") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_PRINT_TECHNICS") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_PRINT_POSTPONE_RESERVISTS") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_PRINT_POSTPONE_TECHNICS") != UIAccessLevel.Hidden)
                ) &&
                (
                 (page.GetUIItemAccessLevel("RES_PRINT_RESERVISTS_MK") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_PRINT_RESERVISTS_PZ") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_PRINT_TECHNICS_MK") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("RES_PRINT_TECHNICS_PZ") != UIAccessLevel.Hidden)
                )
               )
            {
                key = "Print";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Печат";
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

                // ================================================================================================
                // Subitems
                // ================================================================================================
                if (page.GetUIItemAccessLevel("RES_PRINT_RESERVISTS") != UIAccessLevel.Hidden)
                {
                    key = "PrintReservists";
                    menuItemURL = "~/ContentPages/PrintReservists.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Документи за хора</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("RES_PRINT_TECHNICS") != UIAccessLevel.Hidden)
                {
                    key = "PrintTechnics";
                    menuItemURL = "~/ContentPages/PrintTechnics.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Документи за техника</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("RES_PRINT_POSTPONE_RESERVISTS") != UIAccessLevel.Hidden)
                {
                    key = "PrintPostponeReservists";
                    menuItemURL = "~/ContentPages/PrintPostponeReservists.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Отсрочки на хора</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("RES_PRINT_POSTPONE_TECHNICS") != UIAccessLevel.Hidden)
                {
                    key = "PrintPostponeTechnics";
                    menuItemURL = "~/ContentPages/PrintPostponeTechnics.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Отсрочки на техника</span>
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
           
            PMIS.Reserve.ContentPages.TechParamsPageUtil techParamsUtil = new PMIS.Reserve.ContentPages.TechParamsPageUtil((RESPage)page, ((RESPage)page).CurrentUser);
            bool canUserAccessAnyMaintenanceList = techParamsUtil.CanUserAccessAnyMaintenanceList();

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Hidden &&
                    (page.GetUIItemAccessLevel("RES_LISTMAINT_MILITARYREADINESSNAMES") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMAKENAMES") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMAKENAMES") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMAKENAMES") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") != UIAccessLevel.Hidden ||
                     page.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") != UIAccessLevel.Hidden ||
                     canUserAccessAnyMaintenanceList
                    )
                )
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


                if (page.GetUIItemAccessLevel("RES_LISTMAINT_MILITARYREADINESSNAMES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_RES_MilitaryReadinessName";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=RES_MilitaryReadinessNames";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Готовност</span>
                                     </td>
                                  </tr>";
                }


                if (page.GetUIItemAccessLevel("RES_LISTMAINT_TECHNICSCATEGORYNAMES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_RES_TechnicsCategoryName";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=RES_TechnicsCategoryNames";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Категории техника</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_RES_Companies";
                    menuItemURL = "~/ContentPages/ManageCompanies.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Фирми</span>
                                     </td>
                                  </tr>";
                }

                if (canUserAccessAnyMaintenanceList)
                {
                    key = "Lists_RES_TechParams";
                    menuItemURL = "~/ContentPages/TechParams.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Параметри на техниката</span>
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



            if (page.GetUIItemAccessLevel("RES_EDITPROFILE") != UIAccessLevel.Hidden)
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