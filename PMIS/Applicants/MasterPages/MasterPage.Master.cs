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
using PMIS.Applicants.Common;
using System.Collections.Generic;

namespace PMIS.Applicants.MasterPages
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        public List<string> menuItemsHighlighted = new List<string>();
        public string[] specificUIKeys = new string[] { "APPL_EDITPROFILE", "APPL_LISTMAINT" };

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
            if (!(Page is APPLPage))
                return;

            APPLPage page = (APPLPage)Page;

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

            if (page.GetUIItemAccessLevel("APPL_VACANN") != UIAccessLevel.Hidden)
            {
                //The same logic is for each specific menu item
                key = "VacancyAnnounces";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Конкурси";
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
            }

            if (page.GetUIItemAccessLevel("APPL_VACANN") != UIAccessLevel.Hidden)
            {

                key = "VacancyAnnounces_Search";
                menuItemURL = "~/ContentPages/ManageVacancyAnnounces.aspx";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                //Here we form the HTML of the hover panel for the specific main menu item
                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на обявените конкурси</span>
                             </td>
                          </tr>";

                itemCss = "subMenuItems";

                if (page.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN") == UIAccessLevel.Enabled && page.GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Enabled)
                {

                    key = "VacancyAnnounces_Add";
                    menuItemURL = "~/ContentPages/AddEditVacancyAnnounce.aspx";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов обявен конкурс</span>
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


            if (page.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
            {

                //The same logic is for each specific menu item
                key = "Applicants";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Кандидати";
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

                key = "Applicants_Search";
                menuItemURL = "~/ContentPages/ManageApplicants.aspx";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                //Here we form the HTML of the hover panel for the specific main menu item
                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на кандидатите по обявен конкурс</span>
                             </td>
                          </tr>";


                if (page.GetUIItemAccessLevel("APPL_APPL_ADDAPPL") == UIAccessLevel.Enabled && page.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled)
                {

                    key = "Applicants_Add";
                    menuItemURL = "~/ContentPages/AddApplicant_SelectPerson.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов кандидат</span>
                             </td>
                          </tr>";
                }

                if (page.GetUIItemAccessLevel("APPL_APPL_ALLOWANCE") != UIAccessLevel.Hidden && page.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
                {

                    key = "Applicants_Allowance";
                    menuItemURL = "~/ContentPages/ApplicantsAllowance.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Допускане на кандидати</span>
                             </td>
                          </tr>";
                }

                if (page.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS") != UIAccessLevel.Hidden && page.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
                {

                    key = "Applicants_Exams";
                    menuItemURL = "~/ContentPages/ApplicantsExams.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Изпити на кандидати</span>
                             </td>
                          </tr>";
                }

                if (page.GetUIItemAccessLevel("APPL_APPL_RANKING") != UIAccessLevel.Hidden && page.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
                {

                    key = "Applicants_Ranking";
                    menuItemURL = "~/ContentPages/ApplicantsRanking.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Класиране на кандидати</span>
                             </td>
                          </tr>";
                }

                if (page.GetUIItemAccessLevel("APPL_APPL_NOMINATING") != UIAccessLevel.Hidden && page.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
                {

                    key = "Applicants_Nomination";
                    menuItemURL = "~/ContentPages/ApplicantsNomination.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Назначаване на кандидати</span>
                             </td>
                          </tr>";
                }



                //Set UI logic for PotencialApplicants

                if (page.GetUIItemAccessLevel("APPL_POTENCIALAPPL") != UIAccessLevel.Hidden)
                {
                    key = "ManagePotencialApplicants";
                    menuItemURL = "~/ContentPages/ManagePotencialApplicants.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на потенциалните кандидати</span>
                             </td>
                          </tr>";

                    itemCss = "subMenuItems";


                    if (page.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Enabled && page.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Enabled)
                    {

                        key = "PotencialApplicants_Add";
                        menuItemURL = "~/ContentPages/AddPotencialApplicant_SelectPerson.aspx";
                        itemCss = "subMenuItems";

                        if (menuItemsHighlighted.Contains(key))
                            itemCss = "SubMenuItemHighlighted";

                        if (menuItemURL.Contains("~"))
                            menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                        subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов потенциален кандидат</span>
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




            //Set UI logic for Cadets
            if (page.GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Hidden)
            {
                key = "Cadets";
                menuItemURL = "";
                tdID = "td" + key;

                td = new HtmlTableCell();
                td.ID = tdID;
                td.InnerHtml = "Курсанти";
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


                key = "Cadets_Search";
                menuItemURL = "~/ContentPages/ManageCadets.aspx";
                itemCss = "subMenuItems";

                if (menuItemsHighlighted.Contains(key))
                    itemCss = "SubMenuItemHighlighted";

                if (menuItemURL.Contains("~"))
                    menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                //Here we form the HTML of the hover panel for the specific main menu item
                subItems += @"<tr>
                             <td style='vertical-align: top;'>
                                <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на курсантите</span>
                             </td>
                          </tr>";


                if (page.GetUIItemAccessLevel("APPL_CADETS_ADDCADET") == UIAccessLevel.Enabled
                    && page.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled)
                {
                    key = "Cadets_Add";
                    menuItemURL = "~/ContentPages/AddCadet_SelectPerson.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Нов курсант</span>
                                     </td>
                                  </tr>";
                }


                if (page.GetUIItemAccessLevel("APPL_CADETS_RANKING") != UIAccessLevel.Hidden
                    && page.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled)
                {
                    key = "Cadets_Ranking";
                    menuItemURL = "~/ContentPages/CadetsRanking.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Класиране на курсанти</span>
                                     </td>
                                  </tr>";
                }


                if (page.GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") != UIAccessLevel.Hidden)
                {
                    key = "Specializations_ManageMilitarySchoolSpecializations";
                    menuItemURL = "~/ContentPages/ManageMilitarySchoolSpecializations.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Специалности и специализации за военните училища</span>
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



            //Set UI logic for Reports
            if ((page.GetUIItemAccessLevel("APPL_REPORTS") != UIAccessLevel.Hidden)
                && (page.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED") != UIAccessLevel.Hidden
                   || page.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE") != UIAccessLevel.Hidden
                   || page.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING") != UIAccessLevel.Hidden
                   || page.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION") != UIAccessLevel.Hidden
                   || page.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET") != UIAccessLevel.Hidden
                   || page.GetUIItemAccessLevel("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY") != UIAccessLevel.Hidden))
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

                // 1 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED") != UIAccessLevel.Hidden)
                {
                    key = "VacAnnApplDetailedReport";
                    menuItemURL = "~/ContentPages/ReportVacAnnApplDetailed.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Детайлна справка за кандидатите по обявен конкурс</span>
                                     </td>
                                  </tr>";

                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 1 item

                // 2 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE") != UIAccessLevel.Hidden)
                {
                    key = "VacAnnApplListParticipate";
                    menuItemURL = "~/ContentPages/ReportVacAnnApplListParticipate.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на кандидатите участвали в конкурс</span>
                                     </td>
                                  </tr>";


                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 2 item



                // 3 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING") != UIAccessLevel.Hidden)
                {
                    key = "VacAnnApplListRanking";
                    menuItemURL = "~/ContentPages/ReportVacAnnApplListRanking.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на кандидатите класирани в конкурс</span>
                                     </td>
                                  </tr>";


                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 3 item


                // 4 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION") != UIAccessLevel.Hidden)
                {
                    key = "VacAnnApplNominatedReport";
                    menuItemURL = "~/ContentPages/ReportVacAnnApplListNominated.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Списък на кандидатите определени за назначаване</span>
                                     </td>
                                  </tr>";


                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 4 item

                // 5 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET") != UIAccessLevel.Hidden)
                {
                    key = "VacAnnApplCadetReport";
                    menuItemURL = "~/ContentPages/ReportsCadet.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Справка кандидат - курсанти</span>
                                     </td>
                                  </tr>";


                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 5 item

                // 6 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY") != UIAccessLevel.Hidden)
                {
                    key = "ReportRatedApplicantsSummary";
                    menuItemURL = "~/ContentPages/ReportRatedApplicantsSummary.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Сведение за класираните кандидати</span>
                                     </td>
                                  </tr>";

                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 6 item

                // 7 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_REPORT_VACANCY_ANNOUNCE_APPLICANTS") != UIAccessLevel.Hidden)
                {
                    key = "ReportVacancyAnnounceApplicants";
                    menuItemURL = "~/ContentPages/ReportVacancyAnnounceApplicants.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Кандидати за военна служба по обявен конкурс</span>
                                     </td>
                                  </tr>";

                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 7 item

                // 8 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_REPORT_DOCUMENTS_APPLIED") != UIAccessLevel.Hidden)
                {
                    key = "ReportDocumentsApplied";
                    menuItemURL = "~/ContentPages/ReportDocumentsApplied.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Сведение за подалите документи за военна служба</span>
                                     </td>
                                  </tr>";

                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 8 item

                // 9 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_REPORT_DOCUMENTS_SENT") != UIAccessLevel.Hidden)
                {
                    key = "ReportDocumentsSent";
                    menuItemURL = "~/ContentPages/ReportDocumentsSent.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Сведение за изпратените документи за военна служба</span>
                                     </td>
                                  </tr>";

                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 9 item

                // 10 item
                subItems = "";

                if (page.GetUIItemAccessLevel("APPL_REPORTS_REGISTER") != UIAccessLevel.Hidden)
                {
                    key = "ReportRegister";
                    menuItemURL = "~/ContentPages/ReportRegister.aspx";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Регистър</span>
                                     </td>
                                  </tr>";

                    div = new HtmlGenericControl();
                    div.InnerHtml = @"<table>
                                 " + subItems + @"
                              </table>";
                    pnl.Controls.Add(div);
                }
                //End 10 item

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




            if (page.GetUIItemAccessLevel("APPL_LISTMAINT") != UIAccessLevel.Hidden &&
                ((page.GetUIItemAccessLevel("APPL_DOCUMENTS") != UIAccessLevel.Hidden) ||
                 (page.GetUIItemAccessLevel("APPL_EXAMS") != UIAccessLevel.Hidden))
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


                if (page.GetUIItemAccessLevel("APPL_DOCUMENTS") != UIAccessLevel.Hidden)
                {
                    key = "Lists_APPL_Documents";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=APPL_Documents";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Документи</span>
                                     </td>
                                  </tr>";
                }

                if (page.GetUIItemAccessLevel("APPL_EXAMS") != UIAccessLevel.Hidden)
                {
                    key = "Lists_APPL_Exams";
                    menuItemURL = "~/ContentPages/Maintenance.aspx?MaintKey=APPL_Exams";
                    itemCss = "subMenuItems";

                    if (menuItemsHighlighted.Contains(key))
                        itemCss = "SubMenuItemHighlighted";

                    if (menuItemURL.Contains("~"))
                        menuItemURL = menuItemURL.Replace("~", ResolveUrl("~"));

                    //Here we form the HTML of the hover panel for the specific main menu item
                    subItems += @"<tr>
                                     <td style='vertical-align: top;'>
                                        <span class=""" + itemCss + @""" onclick=""MenuItemClick('" + menuItemURL + @"');"">Изпити</span>
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




            if (page.GetUIItemAccessLevel("APPL_EDITPROFILE") != UIAccessLevel.Hidden)
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