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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace PMIS.PMISAdmin.ContentPages
{
    public partial class PasswordPolicyPage : AdmPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "ADM_PASSWORDPOLICY";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Highlight the current page in the menu bar
            HighlightMenuItems("Users_PasswordPolicy");

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                LoadData();
            }

            SetupPageUI();
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            PasswordPolicy passwordPolicy = PasswordPolicyUtil.GetPasswordPolicy(CurrentUser);

            chkAllowBlankSpace.Checked = passwordPolicy.AllowBlankSpace;
            chkCaseSensitivity.Checked = passwordPolicy.CaseSensitivity;
            chkLetterChars.Checked = passwordPolicy.LetterChars.HasValue;
            txtLetterChars.Text = passwordPolicy.LetterChars.HasValue ? passwordPolicy.LetterChars.Value.ToString() : "";
            chkLowerCaseChars.Checked = passwordPolicy.LowerCaseChars.HasValue;
            txtLowerCaseChars.Text = passwordPolicy.LowerCaseChars.HasValue ? passwordPolicy.LowerCaseChars.Value.ToString() : "";
            chkUpperCaseChars.Checked = passwordPolicy.UpperCaseChars.HasValue;
            txtUpperCaseChars.Text = passwordPolicy.UpperCaseChars.HasValue ? passwordPolicy.UpperCaseChars.Value.ToString() : "";
            chkNumericChars.Checked = passwordPolicy.NumericChars.HasValue;
            txtNumericChars.Text = passwordPolicy.NumericChars.HasValue ? passwordPolicy.NumericChars.Value.ToString() : "";
            chkSpecialChars.Checked = passwordPolicy.SpecialChars.HasValue;
            txtSpecialChars.Text = passwordPolicy.SpecialChars.HasValue ? passwordPolicy.SpecialChars.Value.ToString() : "";
            chkExpiresAfterDays.Checked = passwordPolicy.ExpiresAfterDays.HasValue;
            txtExpiresAfterDays.Text = passwordPolicy.ExpiresAfterDays.HasValue ? passwordPolicy.ExpiresAfterDays.Value.ToString() : "";
            chkCannotReusePrevPasswords.Checked = passwordPolicy.CannotReusePrevPasswords.HasValue;
            txtCannotReusePrevPasswords.Text = passwordPolicy.CannotReusePrevPasswords.HasValue ? passwordPolicy.CannotReusePrevPasswords.Value.ToString() : "";
            chkBlockUserAfterFailedLogins.Checked = passwordPolicy.BlockUserAfterFailedLogins.HasValue;
            txtBlockUserAfterFailedLogins.Text = passwordPolicy.BlockUserAfterFailedLogins.HasValue ? passwordPolicy.BlockUserAfterFailedLogins.Value.ToString() : "";
            chkMinLenght.Checked = passwordPolicy.MinLenght.HasValue;
            txtMinLenght.Text = passwordPolicy.MinLenght.HasValue ? passwordPolicy.MinLenght.Value.ToString() : "";

            chkCaseSensitivity_Click(this, new EventArgs());
            chkLetterChars_Click(this, new EventArgs());
            chkLowerCaseChars_Click(this, new EventArgs());
            chkUpperCaseChars_Click(this, new EventArgs());
            chkNumericChars_Click(this, new EventArgs());
            chkSpecialChars_Click(this, new EventArgs());
            chkExpiresAfterDays_Click(this, new EventArgs());
            chkCannotReusePrevPasswords_Click(this, new EventArgs());
            chkBlockUserAfterFailedLogins_Click(this, new EventArgs());
            chkMinLenght_Click(this, new EventArgs());
        }

        //Collect the information from the page form
        //Store the data in a object in the memory and use that object when peforming DB operations
        private PasswordPolicy CollectData()
        {
            PasswordPolicy passwordPolicy = new PasswordPolicy();

            passwordPolicy.AllowBlankSpace = chkAllowBlankSpace.Checked;
            passwordPolicy.CaseSensitivity = chkCaseSensitivity.Checked;
            passwordPolicy.LetterChars = (String.IsNullOrEmpty(txtLetterChars.Text) ? (int?)null : int.Parse(txtLetterChars.Text));
            passwordPolicy.LowerCaseChars = (String.IsNullOrEmpty(txtLowerCaseChars.Text) ? (int?)null : int.Parse(txtLowerCaseChars.Text));
            passwordPolicy.UpperCaseChars = (String.IsNullOrEmpty(txtUpperCaseChars.Text) ? (int?)null : int.Parse(txtUpperCaseChars.Text));
            passwordPolicy.NumericChars = (String.IsNullOrEmpty(txtNumericChars.Text) ? (int?)null : int.Parse(txtNumericChars.Text));
            passwordPolicy.SpecialChars = (String.IsNullOrEmpty(txtSpecialChars.Text) ? (int?)null : int.Parse(txtSpecialChars.Text));
            passwordPolicy.ExpiresAfterDays = (String.IsNullOrEmpty(txtExpiresAfterDays.Text) ? (int?)null : int.Parse(txtExpiresAfterDays.Text));
            passwordPolicy.CannotReusePrevPasswords = (String.IsNullOrEmpty(txtCannotReusePrevPasswords.Text) ? (int?)null : int.Parse(txtCannotReusePrevPasswords.Text));
            passwordPolicy.BlockUserAfterFailedLogins = (String.IsNullOrEmpty(txtBlockUserAfterFailedLogins.Text) ? (int?)null : int.Parse(txtBlockUserAfterFailedLogins.Text));
            passwordPolicy.MinLenght = (String.IsNullOrEmpty(txtMinLenght.Text) ? (int?)null : int.Parse(txtMinLenght.Text));


            return passwordPolicy;
        }

        //Validate the form data before doing any server actions
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (chkLetterChars.Checked)
            {
                if (txtLetterChars.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtLetterChars) || pageHiddenControls.Contains(txtLetterChars))
                        errRightsFields.Add("Мин. брой букви");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Мин. брой букви") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtLetterChars.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Мин. брой букви") + "<br/>";
                    }
                }
            }

            if (chkLowerCaseChars.Checked)
            {
                if (txtLowerCaseChars.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtLowerCaseChars) || pageHiddenControls.Contains(txtLowerCaseChars))
                        errRightsFields.Add("Мин. брой малки букви");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Мин. брой малки букви") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtLowerCaseChars.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Мин. брой малки букви") + "<br/>";
                    }
                }
            }

            if (chkUpperCaseChars.Checked)
            {
                if (txtUpperCaseChars.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtUpperCaseChars) || pageHiddenControls.Contains(txtUpperCaseChars))
                        errRightsFields.Add("Мин. брой главни букви");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Мин. брой главни букви") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtUpperCaseChars.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Мин. брой главни букви") + "<br/>";
                    }
                }
            }

            if (chkNumericChars.Checked)
            {
                if (txtNumericChars.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtNumericChars) || pageHiddenControls.Contains(txtNumericChars))
                        errRightsFields.Add("Мин. брой цифри");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Мин. брой цифри") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtNumericChars.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Мин. брой цифри") + "<br/>";
                    }
                }
            }

            if (chkSpecialChars.Checked)
            {
                if (txtSpecialChars.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtSpecialChars) || pageHiddenControls.Contains(txtSpecialChars))
                        errRightsFields.Add("Мин. брой специални символи");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Мин. брой специални символи") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtSpecialChars.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Мин. брой специални символи") + "<br/>";
                    }
                }
            }

            if (chkMinLenght.Checked)
            {
                if (txtMinLenght.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtMinLenght) || pageHiddenControls.Contains(txtMinLenght))
                        errRightsFields.Add("Минимална дължина на паролата");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Минимална дължина на паролата") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtMinLenght.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Минимална дължина на паролата") + "<br/>";
                    }
                }
            }

            if (chkExpiresAfterDays.Checked)
            {
                if (txtExpiresAfterDays.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtExpiresAfterDays) || pageHiddenControls.Contains(txtExpiresAfterDays))
                        errRightsFields.Add("Брой дни след, които изтича паролата");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Брой дни след, които изтича паролата") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtExpiresAfterDays.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Брой дни след, които изтича паролата") + "<br/>";
                    }
                }
            }

            if (chkCannotReusePrevPasswords.Checked)
            {
                if (txtCannotReusePrevPasswords.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtCannotReusePrevPasswords) || pageHiddenControls.Contains(txtCannotReusePrevPasswords))
                        errRightsFields.Add("Брой стари пароли, които да се проверяват за повтаряемост");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Брой стари пароли, които да се проверяват за повтаряемост") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtCannotReusePrevPasswords.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Брой стари пароли, които да се проверяват за повтаряемост") + "<br/>";
                    }
                }
            }

            if (chkBlockUserAfterFailedLogins.Checked)
            {
                if (txtBlockUserAfterFailedLogins.Text.Trim() == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtBlockUserAfterFailedLogins) || pageHiddenControls.Contains(txtBlockUserAfterFailedLogins))
                        errRightsFields.Add("Брой опити за достъп преди да се блокира потребителят");
                    else
                        errMsg += CommonFunctions.GetErrorMessageMandatory("Брой опити за достъп преди да се блокира потребителят") + "<br/>";
                }
                else
                {
                    int tmpInt = 0;

                    try
                    {
                        tmpInt = int.Parse(txtBlockUserAfterFailedLogins.Text);
                    }
                    catch
                    {
                        tmpInt = 0;
                    }

                    if (tmpInt <= 0)
                    {
                        isDataValid = false;
                        errMsg += CommonFunctions.GetErrorMessageNumber("Брой опити за достъп преди да се блокира потребителят") + "<br/>";
                    }
                }
            }

            if (!isDataValid)
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Save the data
        private void SaveData()
        {
            //First collect the data from the page form
            PasswordPolicy passwordPolicy = CollectData();

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "ADM_PasswordPolicy");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (PasswordPolicyUtil.SavePasswordPolicy(CurrentUser, passwordPolicy, change))
            {
                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Записът е успешен";

                hdnSavedChanges.Value = "True";
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Записът не е успешен";
            }

            //Finally write any changes to the log
            change.WriteLog();
        }

        //Save the form data (first check if it is valid)
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                SaveData();
            }
        }

        protected void chkCaseSensitivity_Click(object sender, EventArgs e)
        {
            if (chkCaseSensitivity.Checked)
            {
                rowCaseInsensitive.Visible = false;

                rowCaseSensitive1.Visible = true;
                rowCaseSensitive2.Visible = true;

                chkLetterChars.Checked = false;
                chkLetterChars_Click(sender, new EventArgs());
            }
            else
            {
                rowCaseInsensitive.Visible = true;

                rowCaseSensitive1.Visible = false;
                rowCaseSensitive2.Visible = false;

                chkLowerCaseChars.Checked = false;
                chkLowerCaseChars_Click(sender, new EventArgs());
                chkUpperCaseChars.Checked = false;
                chkUpperCaseChars_Click(sender, new EventArgs());
            }
        }

        protected void chkLetterChars_Click(object sender, EventArgs e)
        {
            if (chkLetterChars.Checked)
            {
                lblLetterChars.Enabled = true;
                txtLetterChars.Enabled = true;
                lblLetterChars2.Enabled = true;
                txtLetterChars.CssClass = "RequiredInputField";
            }
            else
            {
                lblLetterChars.Enabled = false;
                txtLetterChars.Enabled = false;
                lblLetterChars2.Enabled = false;
                txtLetterChars.Text = "";
                txtLetterChars.CssClass = "InputField";
            }
        }

        protected void chkLowerCaseChars_Click(object sender, EventArgs e)
        {
            if (chkLowerCaseChars.Checked)
            {
                lblLowerCaseChars.Enabled = true;
                txtLowerCaseChars.Enabled = true;
                lblLowerCaseChars2.Enabled = true;
                txtLowerCaseChars.CssClass = "RequiredInputField";
            }
            else
            {
                lblLowerCaseChars.Enabled = false;
                txtLowerCaseChars.Enabled = false;
                lblLowerCaseChars2.Enabled = false;
                txtLowerCaseChars.Text = "";
                txtLowerCaseChars.CssClass = "InputField";
            }
        }

        protected void chkUpperCaseChars_Click(object sender, EventArgs e)
        {
            if (chkUpperCaseChars.Checked)
            {
                lblUpperCaseChars.Enabled = true;
                txtUpperCaseChars.Enabled = true;
                lblUpperCaseChars2.Enabled = true;
                txtUpperCaseChars.CssClass = "RequiredInputField";
            }
            else
            {
                lblUpperCaseChars.Enabled = false;
                txtUpperCaseChars.Enabled = false;
                lblUpperCaseChars2.Enabled = false;
                txtUpperCaseChars.Text = "";
                txtUpperCaseChars.CssClass = "InputField";
            }
        }

        protected void chkNumericChars_Click(object sender, EventArgs e)
        {
            if (chkNumericChars.Checked)
            {
                lblNumericChars.Enabled = true;
                txtNumericChars.Enabled = true;
                lblNumericChars2.Enabled = true;
                txtNumericChars.CssClass = "RequiredInputField";
            }
            else
            {
                lblNumericChars.Enabled = false;
                txtNumericChars.Enabled = false;
                lblNumericChars2.Enabled = false;
                txtNumericChars.Text = "";
                txtNumericChars.CssClass = "InputField";
            }
        }

        protected void chkSpecialChars_Click(object sender, EventArgs e)
        {
            if (chkSpecialChars.Checked)
            {
                lblSpecialChars.Enabled = true;
                txtSpecialChars.Enabled = true;
                lblSpecialChars2.Enabled = true;
                txtSpecialChars.CssClass = "RequiredInputField";
            }
            else
            {
                lblSpecialChars.Enabled = false;
                txtSpecialChars.Enabled = false;
                lblSpecialChars2.Enabled = false;
                txtSpecialChars.Text = "";
                txtSpecialChars.CssClass = "InputField";
            }
        }

        protected void chkMinLenght_Click(object sender, EventArgs e)
        {
            if (chkMinLenght.Checked)
            {
                lblMinLenght.Enabled = true;
                txtMinLenght.Enabled = true;
                lblMinLenght2.Enabled = true;
                txtMinLenght.CssClass = "RequiredInputField";
            }
            else
            {
                lblMinLenght.Enabled = false;
                txtMinLenght.Enabled = false;
                lblMinLenght2.Enabled = false;
                txtMinLenght.Text = "";
                txtMinLenght.CssClass = "InputField";
            }
        }

        protected void chkExpiresAfterDays_Click(object sender, EventArgs e)
        {
            if (chkExpiresAfterDays.Checked)
            {
                lblExpiresAfterDays.Enabled = true;
                txtExpiresAfterDays.Enabled = true;
                lblExpiresAfterDays2.Enabled = true;
                txtExpiresAfterDays.CssClass = "RequiredInputField";
            }
            else
            {
                lblExpiresAfterDays.Enabled = false;
                txtExpiresAfterDays.Enabled = false;
                lblExpiresAfterDays2.Enabled = false;
                txtExpiresAfterDays.Text = "";
                txtExpiresAfterDays.CssClass = "InputField";
            }
        }

        protected void chkCannotReusePrevPasswords_Click(object sender, EventArgs e)
        {
            if (chkCannotReusePrevPasswords.Checked)
            {
                lblCannotReusePrevPasswords.Enabled = true;
                txtCannotReusePrevPasswords.Enabled = true;
                lblCannotReusePrevPasswords2.Enabled = true;
                txtCannotReusePrevPasswords.CssClass = "RequiredInputField";
            }
            else
            {
                lblCannotReusePrevPasswords.Enabled = false;
                txtCannotReusePrevPasswords.Enabled = false;
                lblCannotReusePrevPasswords2.Enabled = false;
                txtCannotReusePrevPasswords.Text = "";
                txtCannotReusePrevPasswords.CssClass = "InputField";
            }
        }

        protected void chkBlockUserAfterFailedLogins_Click(object sender, EventArgs e)
        {
            if (chkBlockUserAfterFailedLogins.Checked)
            {
                lblBlockUserAfterFailedLogins.Enabled = true;
                txtBlockUserAfterFailedLogins.Enabled = true;
                lblBlockUserAfterFailedLogins2.Enabled = true;
                txtBlockUserAfterFailedLogins.CssClass = "RequiredInputField";
            }
            else
            {
                lblBlockUserAfterFailedLogins.Enabled = false;
                txtBlockUserAfterFailedLogins.Enabled = false;
                lblBlockUserAfterFailedLogins2.Enabled = false;
                txtBlockUserAfterFailedLogins.Text = "";
                txtBlockUserAfterFailedLogins.CssClass = "InputField";
            }
        }

        // Setup user interface elements according to rights of the user
        private void SetupPageUI()
        {
            bool screenHidden = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Hidden ||
                                GetUIItemAccessLevel("ADM_PASSWORDPOLICY") == UIAccessLevel.Hidden;

            bool screenDisabled = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Disabled ||
                                  GetUIItemAccessLevel("ADM_PASSWORDPOLICY") == UIAccessLevel.Disabled;

            if (screenHidden)
                RedirectAccessDenied();

            if (screenDisabled)
            {
                pageDisabledControls.Add(btnSave);
            }

            UIAccessLevel l;

            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_ALLOWBLANKSPACE");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkAllowBlankSpace);
                pageDisabledControls.Add(lblAllowBlankSpace);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkAllowBlankSpace);
                pageHiddenControls.Add(lblAllowBlankSpace);
            }


            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_CASESENSITIVITY");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkCaseSensitivity);
                pageDisabledControls.Add(lblCaseSensitivity);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkCaseSensitivity);
                pageHiddenControls.Add(lblCaseSensitivity);
            }

            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_LETTERCHARS");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkLetterChars);
                pageDisabledControls.Add(lblLetterChars);
                pageDisabledControls.Add(txtLetterChars);
                pageDisabledControls.Add(lblLetterChars2);

                pageDisabledControls.Add(chkLowerCaseChars);
                pageDisabledControls.Add(lblLowerCaseChars);
                pageDisabledControls.Add(txtLowerCaseChars);
                pageDisabledControls.Add(lblLowerCaseChars2);

                pageDisabledControls.Add(chkUpperCaseChars);
                pageDisabledControls.Add(lblUpperCaseChars);
                pageDisabledControls.Add(txtUpperCaseChars);
                pageDisabledControls.Add(lblUpperCaseChars2);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkLetterChars);
                pageHiddenControls.Add(lblLetterChars);
                pageHiddenControls.Add(txtLetterChars);
                pageHiddenControls.Add(lblLetterChars2);

                pageHiddenControls.Add(chkLowerCaseChars);
                pageHiddenControls.Add(lblLowerCaseChars);
                pageHiddenControls.Add(txtLowerCaseChars);
                pageHiddenControls.Add(lblLowerCaseChars2);

                pageHiddenControls.Add(chkUpperCaseChars);
                pageHiddenControls.Add(lblUpperCaseChars);
                pageHiddenControls.Add(txtUpperCaseChars);
                pageHiddenControls.Add(lblUpperCaseChars2);
            }

            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_NUMERICCHARS");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkNumericChars);
                pageDisabledControls.Add(lblNumericChars);
                pageDisabledControls.Add(txtNumericChars);
                pageDisabledControls.Add(lblNumericChars2);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkNumericChars);
                pageHiddenControls.Add(lblNumericChars);
                pageHiddenControls.Add(txtNumericChars);
                pageHiddenControls.Add(lblNumericChars2);
            }

            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_SPECIALCHARS");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkSpecialChars);
                pageDisabledControls.Add(lblSpecialChars);
                pageDisabledControls.Add(txtSpecialChars);
                pageDisabledControls.Add(lblSpecialChars2);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkSpecialChars);
                pageHiddenControls.Add(lblSpecialChars);
                pageHiddenControls.Add(txtSpecialChars);
                pageHiddenControls.Add(lblSpecialChars2);
            }

            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_MINLENGHT");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkMinLenght);
                pageDisabledControls.Add(lblMinLenght);
                pageDisabledControls.Add(txtMinLenght);
                pageDisabledControls.Add(lblMinLenght2);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkMinLenght);
                pageHiddenControls.Add(lblMinLenght);
                pageHiddenControls.Add(txtMinLenght);
                pageHiddenControls.Add(lblMinLenght2);
            }

            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_EXPIRESAFTERDAYS");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkExpiresAfterDays);
                pageDisabledControls.Add(lblExpiresAfterDays);
                pageDisabledControls.Add(txtExpiresAfterDays);
                pageDisabledControls.Add(lblExpiresAfterDays2);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkExpiresAfterDays);
                pageHiddenControls.Add(lblExpiresAfterDays);
                pageHiddenControls.Add(txtExpiresAfterDays);
                pageHiddenControls.Add(lblExpiresAfterDays2);
            }

            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_CANNOTREUSEPREVPASSWORDS");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkCannotReusePrevPasswords);
                pageDisabledControls.Add(lblCannotReusePrevPasswords);
                pageDisabledControls.Add(txtCannotReusePrevPasswords);
                pageDisabledControls.Add(lblCannotReusePrevPasswords2);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkCannotReusePrevPasswords);
                pageHiddenControls.Add(lblCannotReusePrevPasswords);
                pageHiddenControls.Add(txtCannotReusePrevPasswords);
                pageHiddenControls.Add(lblCannotReusePrevPasswords2);
            }

            l = GetUIItemAccessLevel("ADM_PASSWORDPOLICY_BLOCKUSERAFTERFAILEDLOGINS");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(chkBlockUserAfterFailedLogins);
                pageDisabledControls.Add(lblBlockUserAfterFailedLogins);
                pageDisabledControls.Add(txtBlockUserAfterFailedLogins);
                pageDisabledControls.Add(lblBlockUserAfterFailedLogins2);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(chkBlockUserAfterFailedLogins);
                pageHiddenControls.Add(lblBlockUserAfterFailedLogins);
                pageHiddenControls.Add(txtBlockUserAfterFailedLogins);
                pageHiddenControls.Add(lblBlockUserAfterFailedLogins2);
            }
        }
    }
}
