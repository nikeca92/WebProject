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

using PMIS.Common;
using PMIS.PMISAdmin.Common;

namespace PMIS.PMISAdmin.ContentPages
{
    public partial class Home : AdmPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HighlightMenuItems("Home");

            if (GetUIItemAccessLevel("ADM_AUDITTRAIL") == UIAccessLevel.Hidden)
            {
                divAudtTrail.Visible = false;
                contAudit_Search.Visible = false;
                contAutit_LoginLog.Visible = false;
            }

            if (GetUIItemAccessLevel("ADM_LOGINLOG") == UIAccessLevel.Hidden)
            {
                contAutit_LoginLog.Visible = false;
            }

            if (GetUIItemAccessLevel("ADM_FAILEDLOGINS") == UIAccessLevel.Hidden)
            {
                contAutit_FailedLogins.Visible = false;
            }

            if (GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Hidden)
            {
                divSecurity.Visible = false;
                contUsers_ManageRoles.Visible = false;
                contUsers_AddRole.Visible = false;
                contUsers_UIItemsPerRole.Visible = false;
                contUsers_ManageUsers.Visible = false;
                contUsers_AddUser.Visible = false;
            }
            else
            {
                if (GetUIItemAccessLevel("ADM_SECURITY_USERS") == UIAccessLevel.Hidden)
                {
                    contUsers_ManageUsers.Visible = false;
                    contUsers_AddUser.Visible = false;
                }
                else
                {
                    if (GetUIItemAccessLevel("ADM_SECURITY") != UIAccessLevel.Enabled ||
                        GetUIItemAccessLevel("ADM_SECURITY_USERS") != UIAccessLevel.Enabled ||
                        GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER") != UIAccessLevel.Enabled)
                    {
                        contUsers_AddUser.Visible = false;
                    }
                }

                if (GetUIItemAccessLevel("ADM_SECURITY_ROLES") == UIAccessLevel.Hidden)
                {
                    contUsers_ManageRoles.Visible = false;
                    contUsers_AddRole.Visible = false;
                }
                else
                {
                    if (GetUIItemAccessLevel("ADM_SECURITY") != UIAccessLevel.Enabled ||
                        GetUIItemAccessLevel("ADM_SECURITY_ROLES") != UIAccessLevel.Enabled ||
                        GetUIItemAccessLevel("ADM_SECURITY_ROLES_ADDROLE") != UIAccessLevel.Enabled)
                    {
                        contUsers_AddRole.Visible = false;
                    }
                }

                if (GetUIItemAccessLevel("ADM_SECURITY_UIITEMSPERROLE") == UIAccessLevel.Hidden)
                {
                    contUsers_UIItemsPerRole.Visible = false;
                }
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("ADM_LISTMAINT_MILITFORCETYPES") == UIAccessLevel.Hidden)
            {
                divListsMilitaryForceTypes.Visible = false;
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("ADM_LISTMAINT_MILITARYDEPARTMENTS") == UIAccessLevel.Hidden)
            {
                divListsMilitaryDepartments.Visible = false;
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("ADM_LISTMAINT_ADMINISTRATIONS") == UIAccessLevel.Hidden)
            {
                divListsAdministrations.Visible = false;
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("ADM_LISTMAINT_DRIVINGLICENSECATEGORIES") == UIAccessLevel.Hidden)
            {
                divDrivLicenseCat.Visible = false;
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY") == UIAccessLevel.Hidden)
            {
                divMilRepSpecialities.Visible = false;
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("ADM_LISTMAINT_POSITIONTITLES") == UIAccessLevel.Hidden)
            {
                divListsPositionTitles.Visible = false;
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("ADM_LISTMAINT_MEDRUBRICS") == UIAccessLevel.Hidden)
            {
                divMedRibrics.Visible = false;
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("ADM_LISTMAINT_MILITARYSTRUCTURES") == UIAccessLevel.Hidden)
            {
                divListsMilitaryStructures.Visible = false;
            }

            if (this.GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden
                || this.GetUIItemAccessLevel("ADM_LISTMAINT_MILITARYFORCESORTS") == UIAccessLevel.Hidden)
            {
                divMilitaryForceSorts.Visible = false;
            }
        }
    }
}
