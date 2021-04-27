using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class Country : BaseDbObject, IDropDownItem
    {
        private string countryId;
        private string countryName;

        public string CountryId
        {
            get { return countryId; }
            set { countryId = value; }
        }        

        public string CountryName
        {
            get { return countryName; }
            set { countryName = value; }
        }

        public bool IsBulgaria
        {
            get
            {
                return CountryName.ToUpper() == "БЪЛГАРИЯ";
            }
        }


        public Country(User currentUser)
            : base(currentUser)
        {
        }
        
        //IDropDownItem Members
        public string Text()
        {
            return CountryName;
        }

        public string Value()
        {
            return CountryId;
        }
    }

    public static class CountryUtil
    {
        public static Country GetCountry(string countryId, User currentUser)
        {
            Country country = null;
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT DJJ_KOD as CountryID, 
                                      DJJ_IME as CountryName
                               FROM VS_OWNER.KLV_DJJ 
                               WHERE DJJ_KOD = :CountryID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param = new OracleParameter();
                param.ParameterName = "CountryID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                param.Value = countryId;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    country = new Country(currentUser);
                    country.CountryId = dr["CountryID"].ToString();
                    country.CountryName = dr["CountryName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return country;
        }

        public static Country GetCountryBulgaria(User currentUser)
        {
            Country country = null;
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT DJJ_KOD as CountryID, 
                                      DJJ_IME as CountryName
                               FROM VS_OWNER.KLV_DJJ 
                               WHERE UPPER(DJJ_IME) = 'БЪЛГАРИЯ'";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    country = new Country(currentUser);
                    country.CountryId = dr["CountryID"].ToString();
                    country.CountryName = dr["CountryName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return country;
        }

        public static List<Country> GetCountries(User currentUser)
        {
            Country country;
            List<Country> listCountries = new List<Country>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT DJJ_KOD as CountryID, 
                                      DJJ_IME as CountryName
                               FROM VS_OWNER.KLV_DJJ 
                               ORDER BY DJJ_IME";
                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    country = new Country(currentUser);
                    country.CountryId = dr["CountryID"].ToString();
                    country.CountryName = dr["CountryName"].ToString();

                    listCountries.Add(country);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listCountries;
        }
    }
}
