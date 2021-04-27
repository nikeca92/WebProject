using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //This class represents the DrivingLicenseCategory object
    public class DrivingLicenseCategory : BaseDbObject
    {
        private int drivingLicenseCategoryId;
        private string drivingLicenseCategoryName;

        public int DrivingLicenseCategoryId
        {
            get
            {
                return drivingLicenseCategoryId;
            }
            set
            {
                drivingLicenseCategoryId = value;
            }
        }

        public string DrivingLicenseCategoryName
        {
            get
            {
                return drivingLicenseCategoryName;
            }
            set
            {
                drivingLicenseCategoryName = value;
            }
        }

        public DrivingLicenseCategory(User user)
            : base(user)
        {
        }
    }

    //This class provides some methods for working with DrivingLicenseCategory objects
    public static class DrivingLicenseCategoryUtil
    {
        //This method extracts a new object with type of DrivingLicenseCategory from a particular data reader
        //It is defined as a separate method to be reused easier
        public static DrivingLicenseCategory ExtractDrivingLicenseCategoryFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? drivingLicenseCategoryId = null;

            if (DBCommon.IsInt(dr["DrivingLicenseCategoryID"]))
                drivingLicenseCategoryId = DBCommon.GetInt(dr["DrivingLicenseCategoryID"]);

            string drivingLicenseCategoryName = dr["DrivingLicenseCategoryName"].ToString();

            DrivingLicenseCategory drivingLicenseCategory = new DrivingLicenseCategory(currentUser);

            if (drivingLicenseCategoryId.HasValue)
            {
                drivingLicenseCategory.DrivingLicenseCategoryId = drivingLicenseCategoryId.Value;
                drivingLicenseCategory.DrivingLicenseCategoryName = drivingLicenseCategoryName;
            }

            return drivingLicenseCategory;
        }

        //Return a list of all DrivingLicenseCategory records
        public static List<DrivingLicenseCategory> GetAllDrivingLicenseCategories(User currentUser)
        {
            List<DrivingLicenseCategory> listDrivingLicenseCategories = new List<DrivingLicenseCategory>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.DrivingLicenseCategoryID, a.DrivingLicenseCategoryName
                               FROM PMIS_ADM.DrivingLicenseCategories a
                               ORDER BY a.DrivingLicenseCategoryName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["DrivingLicenseCategoryID"]))
                    {
                        DrivingLicenseCategory drivingLicenseCategory = ExtractDrivingLicenseCategoryFromDataReader(dr, currentUser);
                        listDrivingLicenseCategories.Add(drivingLicenseCategory);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listDrivingLicenseCategories;
        }

        //Return a list of DrivingLicenseCategory records for PersonId
        public static List<DrivingLicenseCategory> GetPersonDrivingLicenseCategories(int personId, User currentUser)
        {
            List<DrivingLicenseCategory> listDrivingLicenseCategories = new List<DrivingLicenseCategory>();

            if (personId == 0) return listDrivingLicenseCategories;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.DrivingLicenseCategoryID, a.DrivingLicenseCategoryName
                              FROM PMIS_ADM.DrivingLicenseCategories a
                              LEFT OUTER JOIN PMIS_ADM.PersonDrivingLicenseCategories b
                              on b.drivinglicensecategoryid = a.drivinglicensecategoryid
                              where b.personid=:personId
                              ORDER BY a.DrivingLicenseCategoryName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "personId";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personId;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["DrivingLicenseCategoryID"]))
                    {
                        DrivingLicenseCategory drivingLicenseCategory = ExtractDrivingLicenseCategoryFromDataReader(dr, currentUser);
                        listDrivingLicenseCategories.Add(drivingLicenseCategory);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listDrivingLicenseCategories;
        }

        //Return a list of DrivingLicenseCategory records for PersonId
        public static List<DrivingLicenseCategory> GetDrivingLicenseCategoryByCategoryId(string  listDrivingLicenseCategoriesId, User currentUser)
        {
          List<  DrivingLicenseCategory> listDrivingLicenseCategory = new List<DrivingLicenseCategory>();

            if (listDrivingLicenseCategoriesId==string.Empty  ) return listDrivingLicenseCategory;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.DrivingLicenseCategoryID, a.DrivingLicenseCategoryName
                              FROM PMIS_ADM.DrivingLicenseCategories a
                              WHERE a.DrivingLicenseCategoryID IN (" + listDrivingLicenseCategoriesId + @")
                              ORDER BY a.DrivingLicenseCategoryName";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["DrivingLicenseCategoryID"]))
                    {
                        DrivingLicenseCategory drivingLicenseCategory = new DrivingLicenseCategory(currentUser);
                         drivingLicenseCategory = ExtractDrivingLicenseCategoryFromDataReader(dr, currentUser);
                         listDrivingLicenseCategory.Add(drivingLicenseCategory);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listDrivingLicenseCategory ;
        }
    }
}
