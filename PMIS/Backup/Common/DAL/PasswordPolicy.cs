using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

using PMIS.Common;

namespace PMIS.Common
{
    public class PasswordPolicy
    {
        public bool AllowBlankSpace { get; set; }
        public bool CaseSensitivity { get; set; }
        public int? LetterChars { get; set; }
        public int? LowerCaseChars { get; set; }
        public int? UpperCaseChars { get; set; }
        public int? NumericChars { get; set; }
        public int? SpecialChars { get; set; }
        public int? ExpiresAfterDays { get; set; }
        public int? CannotReusePrevPasswords { get; set; }
        public int? BlockUserAfterFailedLogins { get; set; }
        public int? MinLenght { get; set; }
    }

    public class PasswordPolicyUtil
    {
        public static PasswordPolicy GetPasswordPolicy(User currentUser)
        {
            PasswordPolicy passwordPolicy = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT AllowBlankSpace, CaseSensitivity, LetterChars,
									   LowerCaseChars, UpperCaseChars, NumericChars,
									   SpecialChars, ExpiresAfterDays, CannotReusePrevPasswords,
									   BlockUserAfterFailedLogins, MinLenght 
                                FROM PMIS_ADM.PasswordPolicy";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    passwordPolicy = new PasswordPolicy();

                    passwordPolicy.AllowBlankSpace = DBCommon.IsInt(dr["AllowBlankSpace"]) && DBCommon.GetInt(dr["AllowBlankSpace"]) == 1;
                    passwordPolicy.CaseSensitivity = DBCommon.IsInt(dr["CaseSensitivity"]) && DBCommon.GetInt(dr["CaseSensitivity"]) == 1;
                    passwordPolicy.LetterChars = (DBCommon.IsInt(dr["LetterChars"]) ? (int?)DBCommon.GetInt(dr["LetterChars"]) : null);
                    passwordPolicy.LowerCaseChars = (DBCommon.IsInt(dr["LowerCaseChars"]) ? (int?)DBCommon.GetInt(dr["LowerCaseChars"]) : null);
                    passwordPolicy.UpperCaseChars = (DBCommon.IsInt(dr["UpperCaseChars"]) ? (int?)DBCommon.GetInt(dr["UpperCaseChars"]) : null);
                    passwordPolicy.NumericChars = (DBCommon.IsInt(dr["NumericChars"]) ? (int?)DBCommon.GetInt(dr["NumericChars"]) : null);
                    passwordPolicy.SpecialChars = (DBCommon.IsInt(dr["SpecialChars"]) ? (int?)DBCommon.GetInt(dr["SpecialChars"]) : null);
                    passwordPolicy.ExpiresAfterDays = (DBCommon.IsInt(dr["ExpiresAfterDays"]) ? (int?)DBCommon.GetInt(dr["ExpiresAfterDays"]) : null);
                    passwordPolicy.CannotReusePrevPasswords = (DBCommon.IsInt(dr["CannotReusePrevPasswords"]) ? (int?)DBCommon.GetInt(dr["CannotReusePrevPasswords"]) : null);
                    passwordPolicy.BlockUserAfterFailedLogins = (DBCommon.IsInt(dr["BlockUserAfterFailedLogins"]) ? (int?)DBCommon.GetInt(dr["BlockUserAfterFailedLogins"]) : null);
                    passwordPolicy.MinLenght = (DBCommon.IsInt(dr["MinLenght"]) ? (int?)DBCommon.GetInt(dr["MinLenght"]) : null);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return passwordPolicy;
        }

        public static PasswordPolicy GetPasswordPolicy()
        {
            PasswordPolicy passwordPolicy = null;

            OracleConnection conn = new OracleConnection(Config.GetWebSetting("WebConnectionString"));
            conn.Open();

            try
            {
                string SQL = @" SELECT AllowBlankSpace, CaseSensitivity, LetterChars,
									   LowerCaseChars, UpperCaseChars, NumericChars,
									   SpecialChars, ExpiresAfterDays, CannotReusePrevPasswords,
									   BlockUserAfterFailedLogins, MinLenght 
                                FROM PMIS_ADM.PasswordPolicy";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    passwordPolicy = new PasswordPolicy();

                    passwordPolicy.AllowBlankSpace = DBCommon.IsInt(dr["AllowBlankSpace"]) && DBCommon.GetInt(dr["AllowBlankSpace"]) == 1;
                    passwordPolicy.CaseSensitivity = DBCommon.IsInt(dr["CaseSensitivity"]) && DBCommon.GetInt(dr["CaseSensitivity"]) == 1;
                    passwordPolicy.LetterChars = (DBCommon.IsInt(dr["LetterChars"]) ? (int?)DBCommon.GetInt(dr["LetterChars"]) : null);
                    passwordPolicy.LowerCaseChars = (DBCommon.IsInt(dr["LowerCaseChars"]) ? (int?)DBCommon.GetInt(dr["LowerCaseChars"]) : null);
                    passwordPolicy.UpperCaseChars = (DBCommon.IsInt(dr["UpperCaseChars"]) ? (int?)DBCommon.GetInt(dr["UpperCaseChars"]) : null);
                    passwordPolicy.NumericChars = (DBCommon.IsInt(dr["NumericChars"]) ? (int?)DBCommon.GetInt(dr["NumericChars"]) : null);
                    passwordPolicy.SpecialChars = (DBCommon.IsInt(dr["SpecialChars"]) ? (int?)DBCommon.GetInt(dr["SpecialChars"]) : null);
                    passwordPolicy.ExpiresAfterDays = (DBCommon.IsInt(dr["ExpiresAfterDays"]) ? (int?)DBCommon.GetInt(dr["ExpiresAfterDays"]) : null);
                    passwordPolicy.CannotReusePrevPasswords = (DBCommon.IsInt(dr["CannotReusePrevPasswords"]) ? (int?)DBCommon.GetInt(dr["CannotReusePrevPasswords"]) : null);
                    passwordPolicy.BlockUserAfterFailedLogins = (DBCommon.IsInt(dr["BlockUserAfterFailedLogins"]) ? (int?)DBCommon.GetInt(dr["BlockUserAfterFailedLogins"]) : null);
                    passwordPolicy.MinLenght = (DBCommon.IsInt(dr["MinLenght"]) ? (int?)DBCommon.GetInt(dr["MinLenght"]) : null);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return passwordPolicy;
        }

        public static bool SavePasswordPolicy(User currentUser, PasswordPolicy passwordPolicy, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"BEGIN
                            UPDATE PMIS_ADM.PasswordPolicy SET
                               AllowBlankSpace = :AllowBlankSpace,
                               CaseSensitivity = :CaseSensitivity, 
                               LetterChars = :LetterChars,
							   LowerCaseChars = :LowerCaseChars,
							   UpperCaseChars = :UpperCaseChars,
							   NumericChars = :NumericChars,
							   SpecialChars = :SpecialChars,
							   ExpiresAfterDays = :ExpiresAfterDays,
							   CannotReusePrevPasswords = :CannotReusePrevPasswords,
							   BlockUserAfterFailedLogins = :BlockUserAfterFailedLogins,
							   MinLenght = :MinLenght;
                         END;";


                PasswordPolicy oldPasswordPolicy = GetPasswordPolicy(currentUser);

                string logDescription = "";

                changeEvent = new ChangeEvent("ADM_PasswordPolicy_Edit", logDescription, null, null, currentUser);

                if (oldPasswordPolicy.AllowBlankSpace != passwordPolicy.AllowBlankSpace)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_AllowBlankSpace", oldPasswordPolicy.AllowBlankSpace ? "1" : "0", passwordPolicy.AllowBlankSpace ? "1" : "0", currentUser));

                if (oldPasswordPolicy.CaseSensitivity != passwordPolicy.CaseSensitivity)
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_CaseSensitivity", oldPasswordPolicy.CaseSensitivity ? "1" : "0", passwordPolicy.CaseSensitivity ? "1" : "0", currentUser));

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.LetterChars, passwordPolicy.LetterChars))
                {
                    if (oldPasswordPolicy.LetterChars.HasValue != passwordPolicy.LetterChars.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqLetterChars", oldPasswordPolicy.LetterChars.HasValue ? "1" : "0", passwordPolicy.LetterChars.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_LetterChars", oldPasswordPolicy.LetterChars.HasValue ? oldPasswordPolicy.LetterChars.Value.ToString() : "", passwordPolicy.LetterChars.HasValue ? passwordPolicy.LetterChars.Value.ToString() : "", currentUser));
                }

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.LowerCaseChars, passwordPolicy.LowerCaseChars))
                {
                    if (oldPasswordPolicy.LowerCaseChars.HasValue != passwordPolicy.LowerCaseChars.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqLowerCaseChars", oldPasswordPolicy.LowerCaseChars.HasValue ? "1" : "0", passwordPolicy.LowerCaseChars.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_LowerCaseChars", oldPasswordPolicy.LowerCaseChars.HasValue ? oldPasswordPolicy.LowerCaseChars.Value.ToString() : "", passwordPolicy.LowerCaseChars.HasValue ? passwordPolicy.LowerCaseChars.Value.ToString() : "", currentUser));
                }

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.UpperCaseChars, passwordPolicy.UpperCaseChars))
                {
                    if (oldPasswordPolicy.UpperCaseChars.HasValue != passwordPolicy.UpperCaseChars.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqUpperCaseChars", oldPasswordPolicy.UpperCaseChars.HasValue ? "1" : "0", passwordPolicy.UpperCaseChars.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_UpperCaseChars", oldPasswordPolicy.UpperCaseChars.HasValue ? oldPasswordPolicy.UpperCaseChars.Value.ToString() : "", passwordPolicy.UpperCaseChars.HasValue ? passwordPolicy.UpperCaseChars.Value.ToString() : "", currentUser));
                }

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.NumericChars, passwordPolicy.NumericChars))
                {
                    if (oldPasswordPolicy.NumericChars.HasValue != passwordPolicy.NumericChars.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqNumericChars", oldPasswordPolicy.NumericChars.HasValue ? "1" : "0", passwordPolicy.NumericChars.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_NumericChars", oldPasswordPolicy.NumericChars.HasValue ? oldPasswordPolicy.NumericChars.Value.ToString() : "", passwordPolicy.NumericChars.HasValue ? passwordPolicy.NumericChars.Value.ToString() : "", currentUser));
                }

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.SpecialChars, passwordPolicy.SpecialChars))
                {
                    if (oldPasswordPolicy.SpecialChars.HasValue != passwordPolicy.SpecialChars.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqSpecialChars", oldPasswordPolicy.SpecialChars.HasValue ? "1" : "0", passwordPolicy.SpecialChars.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_SpecialChars", oldPasswordPolicy.SpecialChars.HasValue ? oldPasswordPolicy.SpecialChars.Value.ToString() : "", passwordPolicy.SpecialChars.HasValue ? passwordPolicy.SpecialChars.Value.ToString() : "", currentUser));
                }

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.MinLenght, passwordPolicy.MinLenght))
                {
                    if (oldPasswordPolicy.MinLenght.HasValue != passwordPolicy.MinLenght.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqMinLenght", oldPasswordPolicy.MinLenght.HasValue ? "1" : "0", passwordPolicy.MinLenght.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_MinLenght", oldPasswordPolicy.MinLenght.HasValue ? oldPasswordPolicy.MinLenght.Value.ToString() : "", passwordPolicy.MinLenght.HasValue ? passwordPolicy.MinLenght.Value.ToString() : "", currentUser));
                }

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.ExpiresAfterDays, passwordPolicy.ExpiresAfterDays))
                {
                    if (oldPasswordPolicy.ExpiresAfterDays.HasValue != passwordPolicy.ExpiresAfterDays.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqExpiresAfterDays", oldPasswordPolicy.ExpiresAfterDays.HasValue ? "1" : "0", passwordPolicy.ExpiresAfterDays.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ExpiresAfterDays", oldPasswordPolicy.ExpiresAfterDays.HasValue ? oldPasswordPolicy.ExpiresAfterDays.Value.ToString() : "", passwordPolicy.ExpiresAfterDays.HasValue ? passwordPolicy.ExpiresAfterDays.Value.ToString() : "", currentUser));
                }

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.CannotReusePrevPasswords, passwordPolicy.CannotReusePrevPasswords))
                {
                    if (oldPasswordPolicy.CannotReusePrevPasswords.HasValue != passwordPolicy.CannotReusePrevPasswords.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqCannotReusePrevPasswords", oldPasswordPolicy.CannotReusePrevPasswords.HasValue ? "1" : "0", passwordPolicy.CannotReusePrevPasswords.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_CannotReusePrevPasswords", oldPasswordPolicy.CannotReusePrevPasswords.HasValue ? oldPasswordPolicy.CannotReusePrevPasswords.Value.ToString() : "", passwordPolicy.CannotReusePrevPasswords.HasValue ? passwordPolicy.CannotReusePrevPasswords.Value.ToString() : "", currentUser));
                }

                if (!CommonFunctions.IsEqualInt(oldPasswordPolicy.BlockUserAfterFailedLogins, passwordPolicy.BlockUserAfterFailedLogins))
                {
                    if (oldPasswordPolicy.BlockUserAfterFailedLogins.HasValue != passwordPolicy.BlockUserAfterFailedLogins.HasValue)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_ReqBlockUserAfterFailedLogins", oldPasswordPolicy.BlockUserAfterFailedLogins.HasValue ? "1" : "0", passwordPolicy.BlockUserAfterFailedLogins.HasValue ? "1" : "0", currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_PasswordPolicy_BlockUserAfterFailedLogins", oldPasswordPolicy.BlockUserAfterFailedLogins.HasValue ? oldPasswordPolicy.BlockUserAfterFailedLogins.Value.ToString() : "", passwordPolicy.BlockUserAfterFailedLogins.HasValue ? passwordPolicy.BlockUserAfterFailedLogins.Value.ToString() : "", currentUser));
                }

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = null;

                //Update
                param = new OracleParameter();
                param.ParameterName = "AllowBlankSpace";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = passwordPolicy.AllowBlankSpace ? 1 : 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CaseSensitivity";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = passwordPolicy.CaseSensitivity ? 1 : 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LetterChars";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.LetterChars.HasValue)
                    param.Value = passwordPolicy.LetterChars.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LowerCaseChars";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.LowerCaseChars.HasValue)
                    param.Value = passwordPolicy.LowerCaseChars.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "UpperCaseChars";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.UpperCaseChars.HasValue)
                    param.Value = passwordPolicy.UpperCaseChars.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NumericChars";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.NumericChars.HasValue)
                    param.Value = passwordPolicy.NumericChars.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SpecialChars";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.SpecialChars.HasValue)
                    param.Value = passwordPolicy.SpecialChars.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ExpiresAfterDays";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.ExpiresAfterDays.HasValue)
                    param.Value = passwordPolicy.ExpiresAfterDays.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CannotReusePrevPasswords";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.CannotReusePrevPasswords.HasValue)
                    param.Value = passwordPolicy.CannotReusePrevPasswords.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "BlockUserAfterFailedLogins";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.BlockUserAfterFailedLogins.HasValue)
                    param.Value = passwordPolicy.BlockUserAfterFailedLogins.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MinLenght";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (passwordPolicy.MinLenght.HasValue)
                    param.Value = passwordPolicy.MinLenght.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                }
            }

            return result;
        }

        public static bool IsStrongPassword(string password, User currentUser)
        {
            bool strong = true;

            PasswordPolicy passwordPolicy = GetPasswordPolicy(currentUser);

            if (strong)
            {
                if (!passwordPolicy.AllowBlankSpace && password.Contains(" "))
                {
                    strong = false;
                }
            }

            if (strong)
            {
                if (!passwordPolicy.CaseSensitivity && passwordPolicy.LetterChars.HasValue)
                {
                    int reqCnt = passwordPolicy.LetterChars.Value;

                    if (reqCnt > 0 && GetCharsCount(password.ToLower(), CharType.LowerCaseLetter) < reqCnt)
                    {
                        strong = false;
                    }
                }
            }

            if (strong)
            {
                if (passwordPolicy.CaseSensitivity && passwordPolicy.LowerCaseChars.HasValue)
                {
                    int reqCnt = passwordPolicy.LowerCaseChars.Value;

                    if (reqCnt > 0 && GetCharsCount(password, CharType.LowerCaseLetter) < reqCnt)
                    {
                        strong = false;
                    }
                }
            }

            if (strong)
            {
                if (passwordPolicy.CaseSensitivity && passwordPolicy.UpperCaseChars.HasValue)
                {
                    int reqCnt = passwordPolicy.UpperCaseChars.Value;

                    if (reqCnt > 0 && GetCharsCount(password, CharType.UpperCaseLetter) < reqCnt)
                    {
                        strong = false;
                    }
                }
            }

            if (strong)
            {
                if (passwordPolicy.NumericChars.HasValue)
                {
                    int reqCnt = passwordPolicy.NumericChars.Value;

                    if (reqCnt > 0 && GetCharsCount(password, CharType.Numeric) < reqCnt)
                    {
                        strong = false;
                    }
                }
            }

            if (strong)
            {
                if (passwordPolicy.SpecialChars.HasValue)
                {
                    int reqCnt = passwordPolicy.SpecialChars.Value;

                    if (reqCnt > 0 && GetCharsCount(password, CharType.SpecialCharacter) < reqCnt)
                    {
                        strong = false;
                    }
                }
            }

            if (strong)
            {
                if (passwordPolicy.MinLenght.HasValue)
                {
                    int minLen = passwordPolicy.MinLenght.Value;

                    if (minLen > 0 && password.Length < minLen)
                    {
                        strong = false;
                    }
                }
            }

            return strong;
        }

        private enum CharType { LowerCaseLetter, UpperCaseLetter, Numeric, SpecialCharacter }

        private static int GetCharsCount(string s, CharType charType)
        {
            int found = 0;
            char[] specialChars = "!@#$%^&*()_+=-`~[]{}'\";:\\|<>,./?".ToCharArray();

            foreach (char c in s.ToCharArray())
            {
                if ((charType == CharType.LowerCaseLetter && (c >= 'a' && c <= 'z' || c >= 'а' && c <= 'я')) ||
                    (charType == CharType.UpperCaseLetter && (c >= 'A' && c <= 'Z' || c >= 'А' && c <= 'Я')) ||
                    (charType == CharType.Numeric && (c >= '0' && c <= '9')) ||
                    (charType == CharType.SpecialCharacter && (specialChars.Contains(c))))
                {
                    found++;
                }
            }

            return found;
        }

        public static string StrongPasswordRequirements(User currentUser)
        {
            string text = "";
            string delim = "\n";

            PasswordPolicy passwordPolicy = GetPasswordPolicy(currentUser);


            if (!passwordPolicy.AllowBlankSpace)
            {
                text += (text == "" ? "" : delim) +
                        " - да не съдържа интервал";
            }

            if (!passwordPolicy.CaseSensitivity)
            {
                if (passwordPolicy.LetterChars.HasValue && passwordPolicy.LetterChars.Value > 0)
                    text += (text == "" ? "" : delim) +
                        " - да съдържа поне " + passwordPolicy.LetterChars.Value.ToString() + " букв" + (passwordPolicy.LetterChars.Value == 1 ? "а" : "и");
            }
            else
            {
                if (passwordPolicy.LowerCaseChars.HasValue && passwordPolicy.LowerCaseChars.Value > 0)
                    text += (text == "" ? "" : delim) +
                        " - да съдържа поне " + passwordPolicy.LowerCaseChars.Value.ToString() + " малк" + (passwordPolicy.LowerCaseChars.Value == 1 ? "а" : "и") + " букв" + (passwordPolicy.LowerCaseChars.Value == 1 ? "а" : "и");

                if (passwordPolicy.UpperCaseChars.HasValue && passwordPolicy.UpperCaseChars.Value > 0)
                    text += (text == "" ? "" : delim) +
                        " - да съдържа поне " + passwordPolicy.UpperCaseChars.Value.ToString() + " главн" + (passwordPolicy.UpperCaseChars.Value == 1 ? "а" : "и") + " букв" + (passwordPolicy.UpperCaseChars.Value == 1 ? "а" : "и");
            }

            if (passwordPolicy.NumericChars.HasValue && passwordPolicy.NumericChars.Value > 0)
                text += (text == "" ? "" : delim) +
                    " - да съдържа поне " + passwordPolicy.NumericChars.Value.ToString() + " цифр" + (passwordPolicy.NumericChars.Value == 1 ? "а" : "и");

            if (passwordPolicy.SpecialChars.HasValue && passwordPolicy.SpecialChars.Value > 0)
                text += (text == "" ? "" : delim) +
                    " - да съдържа поне " + passwordPolicy.SpecialChars.Value.ToString() + " " + (passwordPolicy.SpecialChars.Value == 1 ? "специален" : "специални") + " символ" + (passwordPolicy.SpecialChars.Value == 1 ? "" : "а");

            if (passwordPolicy.MinLenght.HasValue && passwordPolicy.MinLenght.Value > 0)
                text += (text == "" ? "" : delim) +
                    " - да бъде дълга поне " + passwordPolicy.MinLenght.Value.ToString() + " символ" + (passwordPolicy.MinLenght.Value == 1 ? "" : "а");

            if (text != "")
            {
                text = "Паролата трябва:" + delim + text;
            }

            return text;
        }
    }
}


