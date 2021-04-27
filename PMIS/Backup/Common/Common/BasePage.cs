using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PMIS.Common
{
    //All pages from all modules inherit the BasePage class to re-use some basic properties and/or functions
    //Also, in each particular module (project) there would be a specific for the that module base page.
    public class BasePage : Page
    {
        //This is the key of the particular module
        //Its actual value is overriden in each particular module
        public virtual string ModuleKey
        {
            get
            {
                return "";
            }
        }

        public virtual string PageUIKey
        {
            get
            {
                return "";
            }
        }

        //Get the LoginLogId of the currently logged in user by using the FormsAuth functionality
        public string FormsAuthLoginLogId
        {
            get
            {
                return @User.Identity.Name;
            }
        }        

        private User currentUser;

        //Represents the currently logged in user
        public User CurrentUser
        {
            get
            {
                //If we accesss this object for the first time then pull the data from the DB, otherwise return the already created object
                if (currentUser == null)
                {
                    int loginLogId = 0;

                    try
                    {
                        loginLogId = int.Parse(FormsAuthLoginLogId);
                    }
                    catch
                    {
                        loginLogId = 0;
                    }

                    //Get the details from the DB
                    currentUser = UserUtil.GetUserByLoginLogID(loginLogId, ModuleKey);
                    currentUser.LoginLogId = loginLogId;
                }

                return currentUser;
            }
        }

        //This is a list of all UI items that are on the currently opened page
        //We pull this list only once to prevent executing a separate DB call to check the "access status" for each specific UI item
        private List<UIItem> pageUIItems;
        public List<UIItem> PageUIItems
        {
            get
            {
                if (pageUIItems == null && currentUser != null)
                    pageUIItems = UIItemUtil.GetUIItems(PageUIKey, currentUser, true, currentUser.Role.RoleId, null);

                return pageUIItems;
            }
        }

        //Add some specific UIItems to the UIItems list on the page
        //Use this from MasterPage to load some items for the Menu
        public void LoadSpecificUIItems(string[] uiItemKeys)
        {
            List<UIItem> lstUIItems = UIItemUtil.GetUIItems(uiItemKeys, CurrentUser, false, CurrentUser.Role.RoleId, null);
            PageUIItems.AddRange(lstUIItems);
        }

        //This returns the specific access level for a particular UI item
        //We identify the UI items by their uiKey property
        public UIAccessLevel GetUIItemAccessLevel(string uiKey)
        {
            UIAccessLevel accLevel = UIAccessLevel.Enabled;

            UIItem uiItem = UIItemUtil.FindUIItem(uiKey, PageUIItems);

            //The UIItem is not found within the loaded UIItem on the page
            //In this case load the UI Item from the DB
            if (uiItem == null)
            {
                List<UIItem> lstUIItems = UIItemUtil.GetUIItems(uiKey, CurrentUser, false, CurrentUser.Role.RoleId, null);

                if (lstUIItems.Count > 0)
                    uiItem = lstUIItems[0];
            }

            if (uiItem != null)
                accLevel = uiItem.AccessLevel;

            return accLevel;
        }

        public List<WebControl> pageDisabledControls = new List<WebControl>();
        public List<WebControl> pageHiddenControls = new List<WebControl>();

        public BasePage()
        {
            PreLoad += new EventHandler(BasePage_Load);
            LoadComplete += new EventHandler(BasePage_LoadComplete);
        }

        //Call some basic functionalty for all pages from all modules here
        void BasePage_Load(object sender, EventArgs e)
        {
            //If the user is logged in, however, the password has been expired then redirect the user to enter a new one
            if (CurrentUser != null && CurrentUser.UserId > 0 && !CurrentUser.PasswordDoesNotExpire)
            {
                PasswordPolicy passwordPolicy = PasswordPolicyUtil.GetPasswordPolicy(CurrentUser);

                if (passwordPolicy.ExpiresAfterDays.HasValue && passwordPolicy.ExpiresAfterDays.Value > 0)
                {
                    bool expired = false;

                    if (CurrentUser.PasswordUpdateDate.HasValue)
                    {
                        int passwordAge = (int)(DateTime.Now - CurrentUser.PasswordUpdateDate.Value).TotalDays;

                        if (passwordAge > passwordPolicy.ExpiresAfterDays)
                            expired = true;
                    }
                    else
                    {
                        expired = true;
                    }

                    if (expired && !Request.Url.ToString().ToUpper().Contains("PASSWORDEXPIRED.ASPX"))
                    {
                        Response.Redirect("~/ContentPages/PasswordExpired.aspx");
                    }
                }
            }

            if (Master != null)
            {
                //Set the Full Name label on the Master page with the full name of the currently logged user
                SetMsterFullName();

                //Hide the Edit Profile icon
                Control imgEditUserProfile = Master.FindControl("imgEditUserProfile");

                if (imgEditUserProfile is HtmlGenericControl)
                {
                    ((HtmlGenericControl)imgEditUserProfile).Visible = CurrentUser.UserId > 0 && !Request.Url.ToString().ToUpper().Contains("PASSWORDEXPIRED.ASPX");

                    if (CurrentUser.UserId > 0)
                    {
                        if (GetUIItemAccessLevel(ModuleKey + "_EDITPROFILE") == UIAccessLevel.Hidden)
                        {
                            ((HtmlGenericControl)imgEditUserProfile).Visible = false;
                        }
                    }
                }   
            }
        }

        //Call some basic functionalty after page load for all pages from all modules here
        void BasePage_LoadComplete(object sender, EventArgs e)
        {
            SetupPageControls();
        }

        //This returns the ASPX page name of the currently loaded screen
        public string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
            string sRet = oInfo.Name;
            return sRet;
        }

        //This returns the absolute server path (e.g. http://www.something.example:8080/)
        protected string GetServerPath()
        {
            string Port = Request.ServerVariables["SERVER_PORT"];

            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = Request.ServerVariables["SERVER_PORT_SECURE"];

            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            return Protocol + Request.ServerVariables["SERVER_NAME"] + Port;
        }


        //Set the label on the master page that displays the user's full name
        protected void SetMsterFullName()
        {
            if (CurrentUser.UserId > 0 && Master != null)
            {
                Control lblMasterFullName = Master.FindControl("lblMasterFullName");

                if (lblMasterFullName is HtmlGenericControl)
                {
                    ((HtmlGenericControl)lblMasterFullName).InnerHtml = CurrentUser.FullName;
                }
            }
        }

        //Hide any Back buttons when a particular screen has been opened from the menu bar (there is no information which exactly is the Back/Prev screen)
        protected void HideNavigationControls(params object[] listContrrols)
        {
            string hideNavigationControls = Config.GetWebSetting("HideNavigationControls").ToUpper();
            bool fromMenu = false;

            try
            {
                fromMenu = Request.Params["fm"].ToString() == "1" ? true : false;
            }
            catch
            {
                fromMenu = false;
            }

            if (hideNavigationControls == "TRUE" && fromMenu)
            {
                for (int i = 0; i < listContrrols.Length; i++)
                {
                    if (listContrrols[i] != null && listContrrols[i] is Control)
                    {
                        ((Control)listContrrols[i]).Visible = false;
                    }

                    if (listContrrols[i] != null && listContrrols[i] is HtmlControl)
                    {
                        ((HtmlControl)listContrrols[i]).Visible = false;
                    }
                }
            }
        }

        // disable server control
        protected void DisableControl(WebControl c)
        {
            c.Enabled = false;
        }

        // enable server control
        protected void EnableControl(WebControl c)
        {
            c.Enabled = true;
        }
        
        // hide page server controls
        protected void HideControl(WebControl c)
        {
            c.CssClass = "";
            c.Style.Add("display", "none");
        }


        // Setup disabled and hidden server page controls
        protected void SetupPageControls()
        {
            foreach (WebControl c in pageDisabledControls)
                if (c is LinkButton)
                    DisableButton(c);
                else
                    DisableControl(c);

            foreach (WebControl c in pageHiddenControls)
                HideControl(c);
        }

        // disable server button(change css class)
        protected void DisableButton(WebControl c)
        {
            c.CssClass = "DisabledButton";
            c.Enabled = false;
        }

        // enable server button(restore css class)
        protected void EnableButton(WebControl c)
        {
            c.CssClass = "Button";
            c.Enabled = true;
        }

        // setup all disabled client controls in hidden field on master page
        public void SetDisabledClientControls(string[] controls)
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnDisabledClientControls");

            hf.Value += (hf.Value == "" ? "" : ",") + string.Join(",", controls);
        }

        // setup all hidden client controls in hidden field on master page
        public void SetHiddenClientControls(string[] controls)
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHiddenClientControls");

            hf.Value += (hf.Value == "" ? "" : ",") + string.Join(",", controls);
        }

        // Set attribute to Button in order to prevent "Save changes"-message
        protected void BtnForceNoChangesCheck(Button btn)
        {
            if (!string.IsNullOrEmpty(btn.OnClientClick))
                btn.OnClientClick = "ForceNoChanges();" + btn.OnClientClick;
            else
                btn.OnClientClick = "ForceNoChanges();";
        }

        // Set attribute to LinkButton in order to prevent "Save changes"-message
        protected void LnkForceNoChangesCheck(LinkButton lnk)
        {
            if (!string.IsNullOrEmpty(lnk.OnClientClick))
                lnk.OnClientClick = "ForceNoChanges();" + lnk.OnClientClick;
            else
                lnk.OnClientClick = "ForceNoChanges();";
        }

        // Set attribute to CheckBox in order to prevent "Save changes"-message
        protected void ChkForceNoChangesCheck(CheckBox chk)
        {
            if (chk.Attributes["onclick"] != null)
                chk.Attributes["onclick"] = "ForceNoChanges();" + chk.Attributes["onclick"];
            else
                chk.Attributes.Add("onclick", "ForceNoChanges();");
        }

        // Set hidden field, that indicates saved changes in order to force reloading "original values" of the page(save changes on unload functionality)
        protected void SetSavedChanges()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnSavedChanges");

            hf.Value = "True";
        }

        //Use this function when the user has opened a screen without the required access level
        protected void RedirectAccessDenied()
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Use this function when the user has opened an ajax request without the required access level
        protected void RedirectAjaxAccessDenied()
        {
            string stat = AJAXTools.NOACCESS;
            string response = "";
            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Set the LocationHash field on the MasterPage. Use it for URL redirection on AddEdit screens
        public void SetLocationHash(string locationHash)
        {
            HiddenField hf = (HiddenField)CommonFunctions.CustomFindControl(Page, "hdnLocationHash");

            if (hf != null)
                hf.Value = locationHash;
        }
    }
}
