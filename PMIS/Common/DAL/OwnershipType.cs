using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single ownership type from PMIS_ADM.OwnershipTypes table
    public class OwnershipType : IDropDownItem
    {
        private int ownershipTypeId;
        private string ownershipTypeName;
        private string ownershipTypeKey;

        public int OwnershipTypeId
        {
            get { return ownershipTypeId; }
            set { ownershipTypeId = value; }
        }

        public string OwnershipTypeName
        {
            get { return ownershipTypeName; }
            set { ownershipTypeName = value; }
        }

        public string OwnershipTypeKey
        {
            get { return ownershipTypeKey; }
            set { ownershipTypeKey = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return ownershipTypeName;
        }

        public string Value()
        {
            return ownershipTypeId.ToString();
        }
    }

    public static class OwnershipTypeUtil
    {
        public static OwnershipType GetOwnershipType(int ownershipTypeId, User currentUser)
        {
            OwnershipType ownershipType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.OwnershipTypeName, a.OwnershipTypeKey
                               FROM PMIS_ADM.OwnershipTypes a
                               WHERE a.OwnershipTypeID = :OwnershipTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("OwnershipTypeID", OracleType.Number).Value = ownershipTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ownershipType = new OwnershipType();
                    ownershipType.OwnershipTypeId = ownershipTypeId;
                    ownershipType.OwnershipTypeName = dr["OwnershipTypeName"].ToString();
                    ownershipType.OwnershipTypeKey = dr["OwnershipTypeKey"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return ownershipType;
        }

        public static List<OwnershipType> GetAllOwnershipTypes(User currentUser)
        {
            List<OwnershipType> ownershipTypes = new List<OwnershipType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.OwnershipTypeID, a.OwnershipTypeName, a.OwnershipTypeKey
                               FROM PMIS_ADM.OwnershipTypes a
                               ORDER BY a.OwnershipTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    OwnershipType ownershipType = new OwnershipType();
                    ownershipType.OwnershipTypeId = DBCommon.GetInt(dr["OwnershipTypeID"]);
                    ownershipType.OwnershipTypeName = dr["OwnershipTypeName"].ToString();
                    ownershipType.OwnershipTypeKey = dr["OwnershipTypeKey"].ToString();
                    ownershipTypes.Add(ownershipType);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return ownershipTypes;
        }
    }
}
