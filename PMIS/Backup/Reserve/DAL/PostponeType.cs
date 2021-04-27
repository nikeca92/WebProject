using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class PostponeType : IDropDownItem
    {
        private int postponeTypeId;
        private string postponeTypeKey;
        private string postponeTypeName;

        public int PostponeTypeId
        {
            get
            {
                return postponeTypeId;
            }
            set
            {
                postponeTypeId = value;
            }
        }

        public string PostponeTypeKey
        {
            get
            {
                return postponeTypeKey;
            }
            set
            {
                postponeTypeKey = value;
            }
        }

        public string PostponeTypeName
        {
            get
            {
                return postponeTypeName;
            }
            set
            {
                postponeTypeName = value;
            }
        }



        #region IDropDownItem Members

        public string Text()
        {
            return PostponeTypeName;
        }

        public string Value()
        {
            return PostponeTypeId.ToString();
        }

        #endregion
    }

    public static class PostponeTypeUtil
    {
        //This method creates and returns a TechnicsType object. It extracts the data from a DataReader.
        public static PostponeType ExtractPostponeTypeFromDataReader(OracleDataReader dr, User currentUser)
        {
            PostponeType postponeType = new PostponeType();

            postponeType.PostponeTypeId = DBCommon.GetInt(dr["PostponeTypeID"]);
            postponeType.PostponeTypeKey = dr["PostponeTypeKey"].ToString();
            postponeType.PostponeTypeName = dr["PostponeTypeName"].ToString();

            return postponeType;
        }

        //Get a particular object by its ID
        public static PostponeType GetPostponeType(int postponeTypeId, User currentUser)
        {
            PostponeType postponeType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PostponeTypeID, a.PostponeTypeKey, a.PostponeTypeName
                               FROM PMIS_RES.PostponeTypes a                       
                               WHERE a.PostponeTypeID = :PostponeTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeTypeID", OracleType.Number).Value = postponeTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeType = ExtractPostponeTypeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeType;
        }

        //Get a particular object by its key
        public static PostponeType GetPostponeType(string postponeTypeKey, User currentUser)
        {
            PostponeType postponeType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PostponeTypeID, a.PostponeTypeKey, a.PostponeTypeName
                               FROM PMIS_RES.PostponeTypes a                       
                               WHERE a.PostponeTypeKey = :PostponeTypeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PostponeTypeKey", OracleType.VarChar).Value = postponeTypeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    postponeType = ExtractPostponeTypeFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeType;
        }

        //Get a list of all types
        public static List<PostponeType> GetAllPostponeTypes(User currentUser)
        {
            List<PostponeType> postponeTypes = new List<PostponeType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PostponeTypeID, a.PostponeTypeKey, a.PostponeTypeName
                               FROM PMIS_RES.PostponeTypes a 
                               ORDER BY a.PostponeTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    postponeTypes.Add(ExtractPostponeTypeFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return postponeTypes;
        }
    }

}