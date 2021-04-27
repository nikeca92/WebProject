using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;

namespace PMIS.Common
{
    //Use this class a base class for all DAL objects
    //We need this to store the CurrentUser as a property of this basic class because each particular DB call is done 
    //by using the ConnectionString property of each particular User. This is done to allow each specific system user to has its own DB user
    public class BaseDbObject
    {
        private User currentUser;
        private int? createdByUserId;
        private User createdBy;
        private DateTime? createdDate;
        private int? lastModifiedByUserId;
        private User lastModifiedBy;
        private DateTime? lastModifiedDate;

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        //This class is also suitable to define the CreatedBy, CreatedDate, LastModifiedBy and LastModifiedDate properties
        //which stores details about who and when has created and modified the particular DB object
        public int? CreatedByUserId
        {
            get
            {
                return createdByUserId;
            }
            set
            {
                createdByUserId = value;
            }
        }

        public User CreatedBy
        {
            get
            {
                if (createdBy == null && CreatedByUserId.HasValue)
                    createdBy = UserUtil.GetUser(CurrentUser, CreatedByUserId.Value);

                return createdBy;
            }
        }

        public DateTime? CreatedDate
        {
            get
            {
                return createdDate;
            }
            set
            {
                createdDate = value;
            }
        }

        public int? LastModifiedByUserId
        {
            get
            {
                return lastModifiedByUserId;
            }
            set
            {
                lastModifiedByUserId = value;
            }
        }

        public User LastModifiedBy
        {
            get
            {
                if (lastModifiedBy == null && LastModifiedByUserId.HasValue)
                    lastModifiedBy = UserUtil.GetUser(CurrentUser, LastModifiedByUserId.Value);

                return lastModifiedBy;
            }
        }

        public DateTime? LastModifiedDate
        {
            get
            {
                return lastModifiedDate;
            }
            set
            {
                lastModifiedDate = value;
            }
        }

        public BaseDbObject(User user)
        {
            this.currentUser = user;
        }
    }

    //Some basic methods for working with base DB objects
    public static class BaseDbObjectUtil
    {
        //Set the CreatedBy command parameters
        public static void SetCreatedParams(OracleCommand cmd, User currentUser)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = "CreatedBy";
            param.OracleType = OracleType.Number;
            param.Direction = ParameterDirection.Input;
            param.Value = currentUser.UserId;
            cmd.Parameters.Add(param);

            param = new OracleParameter();
            param.ParameterName = "CreatedDate";
            param.OracleType = OracleType.DateTime;
            param.Direction = ParameterDirection.Input;
            param.Value = DateTime.Now;
            cmd.Parameters.Add(param);
        }

        //Set the LastModified command parameters
        public static void SetLastModifiedParams(OracleCommand cmd, User currentUser)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = "LastModifiedBy";
            param.OracleType = OracleType.Number;
            param.Direction = ParameterDirection.Input;
            param.Value = currentUser.UserId;
            cmd.Parameters.Add(param);

            param = new OracleParameter();
            param.ParameterName = "LastModifiedDate";
            param.OracleType = OracleType.DateTime;
            param.Direction = ParameterDirection.Input;
            param.Value = DateTime.Now;
            cmd.Parameters.Add(param);
        }

        //Set the AnyActualChanges parameter
        public static void SetAnyActualChanges(OracleCommand cmd, ChangeEvent changeEvent)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = "AnyActualChanges";
            param.OracleType = OracleType.Number;
            param.Direction = ParameterDirection.Input;
            param.Value = changeEvent.ChangeEventDetails.Count > 0 ? 1 : 0;
            cmd.Parameters.Add(param);
        }

        //Fill the created and last modified fields from a data reader
        public static void ExtractCreatedAndLastModified(OracleDataReader dr, BaseDbObject dbObject)
        {
            if (DBCommon.IsInt(dr["CreatedBy"]))
                dbObject.CreatedByUserId = DBCommon.GetInt(dr["CreatedBy"]);

            if (dr["CreatedDate"] is DateTime)
                dbObject.CreatedDate = (DateTime)dr["CreatedDate"];

            if (DBCommon.IsInt(dr["LastModifiedBy"]))
                dbObject.LastModifiedByUserId = DBCommon.GetInt(dr["LastModifiedBy"]);

            if (dr["LastModifiedDate"] is DateTime)
                dbObject.LastModifiedDate = (DateTime)dr["LastModifiedDate"];
        }
    }
}