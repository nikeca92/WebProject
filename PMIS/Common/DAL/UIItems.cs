using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;

namespace PMIS.Common
{
    //This represents a specific UI item object. We collect all possible UI items in a tree structure to be able to specify
    //which UI item shoudl be either Visible, Invisible or Disabled for a particular user role
    //Each UIItem object has its id, key and name and also a list of its child UI items.
    //The AccessLevel property returns the "access status" (e.g. Disabled, Invisible, etc.)
    public class UIItem : IComparable<UIItem>
    {
        private int uiItemId;
        private string uiKey;
        private string uiName;
        private UIItem parentUIItem;
        private List<UIItem> childUIItems;
        private UIAccessLevel accessLevel;
        private bool canSetAccessOnlyOwnData;
        private bool accessOnlyOwnData;

        public int UIItemId
        {
            get
            {
                return uiItemId;
            }

            set
            {
                uiItemId = value;
            }
        }

        public string UIKey
        {
            get
            {
                return uiKey;
            }

            set
            {
                uiKey = value;
            }
        }

        public string UIName
        {
            get
            {
                return uiName;
            }

            set
            {
                uiName = value;
            }
        }

        public UIItem ParentUIItem
        {
            get
            {
                return parentUIItem;
            }

            set
            {
                parentUIItem = value;
            }
        }

        public List<UIItem> ChildUIItems
        {
            get
            {
                return childUIItems;
            }

            set
            {
                childUIItems = value;
            }
        }

        public UIAccessLevel AccessLevel
        {
            get
            {
                return accessLevel;
            }

            set
            {
                accessLevel = value;
            }
        }

        //Specifies if the particular UI item has the ability to display the checkbox that turns On/Off the setting related
        //to showing only records created by the user itself
        public bool CanSetAccessOnlyOwnData
        {
            get
            {
                return canSetAccessOnlyOwnData;
            }
            set
            {
                canSetAccessOnlyOwnData = value;
            }
        }

        //Specifies if the user should see only records (of that particular type) that have been created by himself
        public bool AccessOnlyOwnData
        {
            get
            {
                return accessOnlyOwnData;
            }
            set
            {
                accessOnlyOwnData = value;
            }
        }

        //Use this method to compare two objects of the UIItem type
        //It is used to be abel to sort a list of such an objects
        public int CompareTo(UIItem other) 
        {
            int result = 0;

            bool currHasChilds = false;
            bool otherHasChilds = false;

            if (ChildUIItems != null)
                currHasChilds = ChildUIItems.Count > 0;

            if (other.ChildUIItems != null)
                otherHasChilds = other.ChildUIItems.Count > 0;

            if (currHasChilds != otherHasChilds)
            {
                result = currHasChilds ? -1 : 1;
            }
            else
            {
                result = UIName.CompareTo(other.UIName);
            }

            return result;
        }

        public UIItem() { }
    }

    //These are the possible assecc level statuses for a specific role
    public enum UIAccessLevel
    {
        Enabled = 0,
        Disabled = 1,
        Hidden = 2
    }

    //The UIItemUtil static utility class has various method that help when working with UIItem objects
    public static class UIItemUtil
    {
        //Return the name of a particular UIAccessLevel as string
        public static string UIAccessLеvelToString(UIAccessLevel accessLevel)
        {
            string str = "";

            switch (accessLevel)
            {
                case UIAccessLevel.Enabled:
                    {
                        str = "Разрешен";
                        break;
                    }
                case UIAccessLevel.Disabled:
                    {
                        str = "Само за четене";
                        break;
                    }
                case UIAccessLevel.Hidden:
                    {
                        str = "Скрит";
                        break;
                    }
            }

            return str;
        }

        //This method returns a list of child UI items for a specific UI item
        //To get all childs and their childs to the end of the tree this methods calls itslef (recursion)
        private static List<UIItem> GetChildsOfUIItem(int? uiItemID, User currentUser, int roleId)
        {
            List<UIItem> UIItems = new List<UIItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"PMIS_ADM.CommonFunctions.GetUIItemAndChilds";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.CommandType = CommandType.StoredProcedure;

                if(uiItemID.HasValue)
                    cmd.Parameters.Add("P_UIItemID", OracleType.Number).Value = uiItemID.Value;
                else
                    cmd.Parameters.Add("P_UIItemID", OracleType.Number).Value = DBNull.Value;

                cmd.Parameters.Add("P_RoleID", OracleType.Number).Value = roleId;

                cmd.Parameters.Add("P_UIItems", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int UIItemId = int.Parse(dr["UIItemID"].ToString());
                    string uiKey = dr["UIKey"].ToString();
                    string uiName = dr["UIName"].ToString();
                    int? parentUIItemId = null;

                    if(dr["ParentUIItemID"].ToString() != "")
                        parentUIItemId = int.Parse(dr["ParentUIItemID"].ToString());

                    bool canSetAccessOnlyOwnData = false;
                    bool accessOnlyOwnData = false;

                    UIItem uiItem = new UIItem();
                    uiItem.UIItemId = UIItemId;
                    uiItem.UIKey = uiKey;
                    uiItem.UIName = uiName;

                    if (parentUIItemId != null)
                    {
                        uiItem.ParentUIItem = new UIItem();
                        uiItem.ParentUIItem.UIItemId = parentUIItemId.Value;
                    }

                    int? accessLevel = null;
                    UIAccessLevel accLevel = UIAccessLevel.Enabled;

                    if (dr["AccessLevel"].ToString() != "")
                        accessLevel = int.Parse(dr["AccessLevel"].ToString());

                    if (accessLevel.HasValue && accessLevel.Value == 1)
                        accLevel = UIAccessLevel.Disabled;
                    else if (accessLevel.HasValue && accessLevel.Value == 2)
                        accLevel = UIAccessLevel.Hidden;

                    uiItem.AccessLevel = accLevel;

                    if (DBCommon.IsInt(dr["CanSetAccessOnlyOwnData"]) &&
                       DBCommon.GetInt(dr["CanSetAccessOnlyOwnData"]) == 1)
                        canSetAccessOnlyOwnData = true;

                    if (DBCommon.IsInt(dr["AccessOnlyOwnData"]) &&
                       DBCommon.GetInt(dr["AccessOnlyOwnData"]) == 1)
                        accessOnlyOwnData = true;

                    uiItem.CanSetAccessOnlyOwnData = canSetAccessOnlyOwnData;
                    uiItem.AccessOnlyOwnData = accessOnlyOwnData;

                    uiItem.ChildUIItems = new List<UIItem>();

                    UIItems.Add(uiItem);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            FixTreeStructure(UIItems);

            return UIItems;
        }

        private static void FixTreeStructure(List<UIItem> uiItems)
        {
            List<UIItem> forRemove = new List<UIItem>();

            foreach(UIItem uiItem in uiItems)
            {
                if(uiItem.ParentUIItem != null)
                {
                    UIItem parent = FindParent(uiItems, uiItem.ParentUIItem.UIItemId);

                    if (parent != null)
                    {
                        parent.ChildUIItems.Add(uiItem);
                        forRemove.Add(uiItem);
                    }
                }
            }

            foreach (UIItem uiItem in forRemove)
            {
                uiItems.Remove(uiItem);
            }
        }

        private static UIItem FindParent(List<UIItem> uiItems, int parentUIItemID)
        {
            foreach (UIItem uiItem in uiItems)
            {
                if (uiItem.UIItemId == parentUIItemID)
                    return uiItem;

                if (uiItem.ChildUIItems != null)
                {
                    UIItem childsResult = FindParent(uiItem.ChildUIItems, parentUIItemID);

                    if (childsResult != null)
                        return childsResult;
                }
            }

            return null;
        }

        //Sort the UI elements in the tree structure
        public static void SortUIItems(List<UIItem> uiItems)
        {
            uiItems.Sort();

            foreach (UIItem uiItem in uiItems)
            {
                if (uiItem.ChildUIItems != null)
                    SortUIItems(uiItem.ChildUIItems);
            }
        }

        //This method returns a specific UIItem object and its access level for the current user, as well as all child UI elements
        //We use this to get all UI items on a particular screen in the system
        public static List<UIItem> GetUIItems(string uiKey, User currentUser, bool includeChilds, int roleId, int? uiItemId)
        {
            return GetUIItems(new string[]{uiKey}, currentUser, includeChilds, roleId, uiItemId);
        }

        public static List<UIItem> GetUIItems(string[] uiKeys, User currentUser, bool includeChilds, int roleId, int? uiItemId)
        {
            List<UIItem> UIItems = new List<UIItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string keys = "";

            foreach (string uiKey in uiKeys)
            {
                keys += (keys == "" ? "" : ", ") + "'" + uiKey.Replace("'", "''") + "'";
            }

            string SQL = "";

            try
            {
                SQL = @"SELECT a.UIItemID, a.UIKey, a.UIName, a.ParentUIItemID, b.AccessLevel,
                               a.CanSetAccessOnlyOwnData, b.AccessOnlyOwnData
                        FROM PMIS_ADM.UIItems a
                        LEFT OUTER JOIN PMIS_ADM.UIItemsPerRole b ON a.UIItemID = b.UIItemID AND b.RoleID = :RoleID
                        WHERE (a.UIKey IN (" + keys + @") AND :UIItemID IS NULL) OR
                              a.UIITemID = :UIItemID
                       ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RoleID", OracleType.Number).Value = roleId;

                if(uiItemId.HasValue)
                    cmd.Parameters.Add("UIItemID", OracleType.Number).Value = uiItemId.Value;
                else
                    cmd.Parameters.Add("UIItemID", OracleType.Number).Value = DBNull.Value;


                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int UIItemId = int.Parse(dr["UIItemID"].ToString());
                    string uiName = dr["UIName"].ToString();
                    string uiKey = dr["UIKey"].ToString();
                    int? parentUIItemId = null;

                    if (dr["ParentUIItemID"].ToString() != "")
                        parentUIItemId = int.Parse(dr["ParentUIItemID"].ToString());

                    bool canSetAccessOnlyOwnData = false;
                    bool accessOnlyOwnData = false;

                    UIItem uiItem = new UIItem();
                    uiItem.UIItemId = UIItemId;
                    uiItem.UIKey = uiKey;
                    uiItem.UIName = uiName;

                    if (parentUIItemId != null)
                    {
                        uiItem.ParentUIItem = new UIItem();
                        uiItem.ParentUIItem.UIItemId = parentUIItemId.Value;
                    }

                    int? accessLevel = null;
                    UIAccessLevel accLevel = UIAccessLevel.Enabled;

                    if (dr["AccessLevel"].ToString() != "")
                        accessLevel = int.Parse(dr["AccessLevel"].ToString());

                    if (accessLevel.HasValue && accessLevel.Value == 1)
                        accLevel = UIAccessLevel.Disabled;
                    else if (accessLevel.HasValue && accessLevel.Value == 2)
                        accLevel = UIAccessLevel.Hidden;

                    uiItem.AccessLevel = accLevel;

                    if (DBCommon.IsInt(dr["CanSetAccessOnlyOwnData"]) &&
                       DBCommon.GetInt(dr["CanSetAccessOnlyOwnData"]) == 1)
                        canSetAccessOnlyOwnData = true;

                    if (DBCommon.IsInt(dr["AccessOnlyOwnData"]) &&
                       DBCommon.GetInt(dr["AccessOnlyOwnData"]) == 1)
                        accessOnlyOwnData = true;

                    uiItem.CanSetAccessOnlyOwnData = canSetAccessOnlyOwnData;
                    uiItem.AccessOnlyOwnData = accessOnlyOwnData;

                    if (includeChilds)
                        uiItem.ChildUIItems = GetChildsOfUIItem(uiItem.UIItemId, currentUser, roleId);

                    UIItems.Add(uiItem);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return UIItems;
        }

        //This method is used to find a specific UI item (by its key) within a list of UI items (actually a tree)
        //When we get a sub-tree that represents a particular page and its elements then we use this method to find a specific UI items and its access level
        public static UIItem FindUIItem(string uiKey, List<UIItem> uiItems)
        {
            if (uiItems != null)
            {
                foreach (UIItem uiItem in uiItems)
                {
                    if (uiItem.UIKey == uiKey)
                        return uiItem;

                    if (uiItem.ChildUIItems != null)
                    {
                        UIItem childsResult = FindUIItem(uiKey, uiItem.ChildUIItems);

                        if (childsResult != null)
                            return childsResult;
                    }
                }   
            }

            return null;
        }

        //This method is used to change the Access Level of a particula UIItem for a particular Role
        public static bool SetUIItemAccessLevel(User currentUser, int roleId, int uiItemId, UIAccessLevel accessLevel, 
                                                bool accessOnlyOwnData, Change changeEntry)
        {
            bool status = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                //Track any changes to the Audit Trail log
                UIItem uiItemOld = UIItemUtil.GetUIItems("", currentUser, false, roleId, uiItemId)[0];

                if (uiItemOld.AccessLevel != accessLevel ||
                    uiItemOld.AccessOnlyOwnData != accessOnlyOwnData)
                {
                    string uiItemTreePath = "";
                    string objectDesc = "";

                    UIItem tmp = UIItemUtil.GetUIItems("", currentUser, false, roleId, uiItemId)[0];
                    uiItemTreePath = tmp.UIName + (uiItemTreePath == "" ? "" : " - ") + uiItemTreePath;

                    while (tmp.ParentUIItem != null)
                    {
                        tmp = UIItemUtil.GetUIItems("", currentUser, false, roleId, tmp.ParentUIItem.UIItemId)[0];
                        uiItemTreePath = tmp.UIName + (uiItemTreePath == "" ? "" : " - ") + uiItemTreePath;
                    }

                    UserRole role = UserRoleUtil.GetUserRole(currentUser, roleId);

                    objectDesc += "Роля: " + role.RoleName + "; Елемент: " + uiItemTreePath;

                    ChangeEvent changeEvent = new ChangeEvent("ADM_UIItemsPerRole_EditAccLevel", objectDesc, null, null, currentUser);

                    if (uiItemOld.AccessLevel != accessLevel)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_UIItemsPerRole_AccessLevel", UIAccessLеvelToString(uiItemOld.AccessLevel), UIAccessLеvelToString(accessLevel), currentUser));

                    if (uiItemOld.AccessOnlyOwnData != accessOnlyOwnData)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_UIItemsPerRole_AccessOnlyOwnData", uiItemOld.AccessOnlyOwnData ? "1" : "0", accessOnlyOwnData ? "1" : "0", currentUser));

                    changeEntry.AddEvent(changeEvent);
                }

                SQL = @"DECLARE
                           IsExisting number;
                        BEGIN
                           SELECT COUNT(*) INTO IsExisting 
                           FROM PMIS_ADM.UIItemsPerRole 
                           WHERE UIItemID = :UIItemID AND RoleID = :RoleID;

                           IsExisting := NVL(IsExisting, 0);

                           IF IsExisting > 0 THEN
                              UPDATE PMIS_ADM.UIItemsPerRole SET
                                 AccessLevel = :AccessLevel,
                                 AccessOnlyOwnData = :AccessOnlyOwnData
                              WHERE UIItemID = :UIItemID AND RoleID = :RoleID;
                           ELSE
                              INSERT INTO PMIS_ADM.UIItemsPerRole (UIItemID, RoleID, AccessLevel, AccessOnlyOwnData)
                              VALUES (:UIItemID, :RoleID, :AccessLevel, :AccessOnlyOwnData);
                           END IF;
                        END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UIItemID", OracleType.Number).Value = uiItemId;
                cmd.Parameters.Add("RoleID", OracleType.Number).Value = roleId;

                int accLevel = 0;

                if (accessLevel == UIAccessLevel.Enabled)
                    accLevel = 0;
                else if (accessLevel == UIAccessLevel.Disabled)
                    accLevel = 1;
                else if (accessLevel == UIAccessLevel.Hidden)
                    accLevel = 2;

                cmd.Parameters.Add("AccessLevel", OracleType.Number).Value = accLevel;
                cmd.Parameters.Add("AccessOnlyOwnData", OracleType.Number).Value = accessOnlyOwnData ? 1 : 0;

                cmd.ExecuteNonQuery();

                status = true;
            }
            finally
            {
                conn.Close();
            }

            return status;
        }
    }
}