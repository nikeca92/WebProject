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
using PMIS.HealthSafety.Common;
using System.Collections.Generic;

namespace PMIS.HealthSafety.MasterPages
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public List<string> menuItemsHighlighted = new List<string>();
        public string[] specificUIKeys = new string[] { "HS_PROTOCOLS", "HS_ADDPROT", "HS_RISKASSESSMENTS", "HS_ADDRISKASSESS", 
            "HS_UNSAFEWCONDNOTICE", "HS_ADDUNSAFEWCONDNOTICE", "HS_LISTMAINT", "HS_LISTMAINT_PROTTYPES", "HS_LISTMAINT_MEASURES", "HS_LIST_INDICATORTYPES", "HS_LISTMAINT_WORKINGPLACES",
            "HS_EDITPROFILE", "HS_WCONDCARDS", "HS_ADDWCONDCARD", "HS_TRAININGHISTORY",
            "HS_LISTMAINT_PROBABILITIES", "HS_LISTMAINT_EXPOSURE", "HS_LISTMAINT_EFFECTWEIGHT", "HS_LISTMAINT_RISKRANK", "HS_MILITARYUNITPOSITIONS", "HS_LIST_RISKFACTORTYPES", 
            "HS_RISKCARD"};

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
            if (!(Page is HSPage))
                return;

            HSPage page = (HSPage)Page;

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

            if (page.GetUIItemAccessLevel("HS_PROTOCOLS") != UIAccessLevel.Hidden)
            {

                key = "Protocols";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Протоколи";
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

                key = "Protocols_Search";
                menuItemURL = "~/ContentPages/ManageProtocols.aspx";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                //Here we form the HTML of the hover panel for the specific main menu item
                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Търсене на протоколи от извършени измервания</span>
                             </td>
                          </tr>";

                itemCss = "subMenuItems";

                if (page.GetUIItemAccessLevel("HS_ADDPROT") == UIAccessLevel.Enabled && page.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Enabled)
                {
                    key = "Protocols_Add";
                    menuItemURL = "~/ContentPages/AddEditProtocol.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов протокол от извършени измервания</span>
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
                hoverMenuItem.OffsetX = -50;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);
            }


            if (page.GetUIItemAccessLevel("HS_WCONDCARDS") != UIAccessLevel.Hidden)
            {

                key = "WorkplaceConditionsCards";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Карти";
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

                key = "WorkplaceConditionsCards_Search";
                menuItemURL = "~/ContentPages/ManageWorkplaceConditionsCards.aspx";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Търсене на карти за комплексно оценяване на специфичните условия на труд и рискове за живота и здравето</span>
                             </td>
                          </tr>";

                itemCss = "subMenuItems";

                if (page.GetUIItemAccessLevel("HS_ADDWCONDCARD") == UIAccessLevel.Enabled && page.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Enabled)
                {
                    key = "WorkplaceConditionsCards_Add";
                    menuItemURL = "~/ContentPages/AddEditWorkplaceConditionsCard.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Новa карта за комплексно оценяване на специфичните условия на труд и рискове за живота и здравето</span>
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
                hoverMenuItem.OffsetX = -50;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);
            }


            if (page.GetUIItemAccessLevel("HS_RISKASSESSMENTS") != UIAccessLevel.Hidden)
            {
                key = "RiskAssessments";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Оценки на риска";
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


                if (page.GetUIItemAccessLevel("HS_MILITARYUNITPOSITIONS") != UIAccessLevel.Hidden
                    && page.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Enabled)
                {
                    key = "MilitaryUnitPositions";
                    menuItemURL = "~/ContentPages/MilitaryUnitPositions.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Класификация на дейностите</span>
                             </td>
                          </tr>";
                }


                if (page.GetUIItemAccessLevel("HS_RISKCARD") != UIAccessLevel.Hidden
                    && page.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Enabled)
                {
                    key = "RiskCard";
                    menuItemURL = "~/ContentPages/RiskCard.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Карта за оценка на риска</span>
                             </td>
                          </tr>";
                }


                key = "RiskAssessments_Search";
                menuItemURL = "~/ContentPages/ManageRiskAssessments.aspx";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Търсене на оценки на риска</span>
                             </td>
                          </tr>";

                itemCss = "subMenuItems";

                if (page.GetUIItemAccessLevel("HS_ADDRISKASSESS") == UIAccessLevel.Enabled
                    && page.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Enabled)
                {
                    key = "RiskAssessments_Add";
                    menuItemURL = "~/ContentPages/AddEditRiskAssessment.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Новa оценка на риска</span>
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
                hoverMenuItem.OffsetX = 0;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);
            }


            if (page.GetUIItemAccessLevel("HS_COMMITTEE") != UIAccessLevel.Hidden)
            {
                key = "Committees";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Комитети и групи";
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

                key = "Committees_Search";
                menuItemURL = "~/ContentPages/ManageCommittees.aspx";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Търсене на комитети и групи по условията на труд</span>
                             </td>
                          </tr>";

                if (page.GetUIItemAccessLevel("HS_ADDCOMMITTEE") == UIAccessLevel.Enabled && page.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Enabled)
                {
                    itemCss = "subMenuItems";

                    key = "Committees_Add";
                    menuItemURL = "~/ContentPages/AddEditCommittee.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов комитет или група по условията на труд</span>
                             </td>
                          </tr>";
                }


                if (page.GetUIItemAccessLevel("HS_TRAININGHISTORY") != UIAccessLevel.Hidden)
                {
                    itemCss = "subMenuItems";

                    key = "Committees_TrainingHistory";
                    menuItemURL = "~/ContentPages/ManageTrainingHistory.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">История на обученията</span>
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
                hoverMenuItem.OffsetX = -20;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);
            }


            if (page.GetUIItemAccessLevel("HS_DECLARATIONACC") != UIAccessLevel.Hidden ||
                page.GetUIItemAccessLevel("HS_INVPROTOCOLS") != UIAccessLevel.Hidden ||
                page.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") != UIAccessLevel.Hidden)
            {
                key = "Accidents";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Трудови злополуки";
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

                if (page.GetUIItemAccessLevel("HS_DECLARATIONACC") != UIAccessLevel.Hidden)
                {

                    key = "Accidents_SearchDeclarationsOfAccident";
                    menuItemURL = "~/ContentPages/ManageDeclarationOfAccident.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Търсене на декларации за трудови злополуки</span>
                                     </td>
                                  </tr>";


                    if (page.GetUIItemAccessLevel("HS_ADDDECLARATIONACC") == UIAccessLevel.Enabled && page.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Enabled)
                    {
                        itemCss = "subMenuItems";
                        key = "Accidents_AddDeclarationsOfAccident";
                        menuItemURL = "~/ContentPages/AddEditDeclarationOfAccident.aspx";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        subItems += @"<tr>
                                         <td style='vertical-align: top;'>
                                            <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нова декларация за трудова злополука</span>
                                         </td>
                                      </tr>";
                    }


                }

                if (page.GetUIItemAccessLevel("HS_INVPROTOCOLS") != UIAccessLevel.Hidden)
                {

                    itemCss = "subMenuItems";
                    key = "Accidents_SearchIvestigationProtocols";
                    menuItemURL = "~/ContentPages/ManageInvestigationProtocol.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Търсене на протоколи за резултатите от разследване на злополука</span>
                                     </td>
                                  </tr>";


                    if (page.GetUIItemAccessLevel("HS_ADDINVPROTOCOL") == UIAccessLevel.Enabled && page.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Enabled)
                    {
                        itemCss = "subMenuItems";
                        key = "Accidents_AddIvestigationProtocols";
                        menuItemURL = "~/ContentPages/AddEditInvestigationProtocol.aspx";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        subItems += @"<tr>
                                         <td style='vertical-align: top;'>
                                            <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов протокол за резултатите от разследване на злополука</span>
                                         </td>
                                      </tr>";

                    }
                }

                if (page.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") != UIAccessLevel.Hidden)
                {
                    itemCss = "subMenuItems";
                    key = "Accidents_SearchUnsafeWorkingConditionsNotices";
                    menuItemURL = "~/ContentPages/ManageUnsafeWorkingConditionsNotices.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Търсене на сведения за заболявания и наранявания свързани с работата</span>
                                     </td>
                                  </tr>";

                    if (page.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE") == UIAccessLevel.Enabled
                        && page.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Enabled)
                    {
                        itemCss = "subMenuItems";
                        key = "Accidents_AddUnsafeWorkingConditionsNotices";
                        menuItemURL = "~/ContentPages/AddEditUnsafeWorkingConditionsNotice.aspx";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        subItems += @"<tr>
                                         <td style='vertical-align: top;'>
                                            <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Ново сведение за заболявания и наранявания свързани с работата</span>
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
                hoverMenuItem.OffsetX = -50;
                hoverMenuItem.OffsetY = CommonFunctions.GetMenuOffsetY();
                hoverMenuItem.PopupPosition = HoverMenuPopupPosition.Bottom;
                hoverMenuItem.PopDelay = 10;
                hoverMenuItem.HoverCssClass = "hoveredMenuItem";

                HoverExtenderCont.Controls.Add(hoverMenuItem);
            }


            if (page.GetUIItemAccessLevel("HS_LISTMAINT") != UIAccessLevel.Hidden &&
                (page.GetUIItemAccessLevel("HS_LISTMAINT_PROTTYPES") != UIAccessLevel.Hidden ||
                 page.GetUIItemAccessLevel("HS_LISTMAINT_MEASURES") != UIAccessLevel.Hidden ||
                 page.GetUIItemAccessLevel("HS_LIST_INDICATORTYPES") != UIAccessLevel.Hidden ||
                 page.GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES") != UIAccessLevel.Hidden ||
                 page.GetUIItemAccessLevel("HS_LIST_PROBABILITIES") != UIAccessLevel.Hidden ||
                 page.GetUIItemAccessLevel("HS_LIST_EXPOSURE") != UIAccessLevel.Hidden ||
                 page.GetUIItemAccessLevel("HS_LIST_EFFECTWEIGHT") != UIAccessLevel.Hidden ||
                 page.GetUIItemAccessLevel("HS_LIST_RISKRANK") != UIAccessLevel.Hidden))
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

                if (page.GetUIItemAccessLevel("HS_LISTMAINT_PROTTYPES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_ProtocolType";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=HS_ProtocolType";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Измервания</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("HS_LISTMAINT_MEASURES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_Measures";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=HS_Measures";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Измервани величини</span>
                                     </td>
                                  </tr>";
                }

                /*This would be edited via the new GTable lightbox maintenance directly from the screen
                key = "Lists_HS_DegreeOfDanger";
                menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=HS_DegreeOfDanger";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                //Here we form the HTML of the hover panel for the specific main menu item
                subItems += @"<tr>
                                 <td style='vertical-align: top;'>
                                    <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Степени на опасност</span>
                                 </td>
                              </tr>";
                */

                if (page.GetUIItemAccessLevel("HS_LIST_INDICATORTYPES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_IndicatorTypes";
                    menuItemURL = "~/ContentPages/IndicatorTypesMaintenance.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                 <td style='vertical-align: top;'>
                                    <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Елементи на специфичните условия на труд</span>
                                 </td>
                              </tr>";
                }


                if (page.GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_WorkingPlaces";
                    menuItemURL = "~/ContentPages/ManageWorkingPlaces.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                 <td style='vertical-align: top;'>
                                    <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Място на измерване</span>
                                 </td>
                              </tr>";
                }

                if (page.GetUIItemAccessLevel("HS_LISTMAINT_PROBABILITIES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_Probabilities";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=HS_Probabilities";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Вероятност</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("HS_LISTMAINT_EXPOSURE") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_Exposure";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=HS_Exposure";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Експозиция</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("HS_LISTMAINT_EFFECTWEIGHT") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_EffectWeight";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=HS_EffectWeight";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Тежест (последици)</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("HS_LISTMAINT_RISKRANK") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_RiskRank";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=HS_RiskRank";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Риск</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("HS_LIST_RISKFACTORTYPES") != UIAccessLevel.Hidden)
                {
                    key = "Lists_HS_RiskFactorTypes";
                    menuItemURL = "~/ContentPages/RiskFactorTypesMaintenance.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                 <td style='vertical-align: top;'>
                                    <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Потенциални опасности</span>
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


            if (page.GetUIItemAccessLevel("HS_EDITPROFILE") != UIAccessLevel.Hidden)
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