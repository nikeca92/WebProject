using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class Address : BaseDbObject
    {
        private int addressId;
        private int? cityId;
        private City city;
        private int? districtId;
        private District district;
        private string postCode = "";
        private string addressText = "";

        public int AddressId
        {
            get { return addressId; }
            set { addressId = value; }
        }

        public int? CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }

        public City City
        {
            get
            {
                if (city == null && cityId != null)
                {
                    city = CityUtil.GetCity((int)cityId, base.CurrentUser);
                }

                return city;
            }
            set { city = value; }
        }

        public int? DistrictId
        {
            get { return districtId; }
            set { districtId = value; }
        }

        public District District
        {
            get
            {
                if (district == null && districtId != null)
                {
                    district = DistrictUtil.GetDistrict((int)districtId, base.CurrentUser);
                }
                return district;

            }
            set { district = value; }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public string AddressText
        {
            get { return addressText; }
            set { addressText = value; }
        }

        public Address(User currentUser)
            : base(currentUser)
        {
        }
    }

    public static class AddressUtil
    {
        public static Address ExtractAddressFromDR(string addressType, User currentUser, OracleDataReader dr)
        {
            Address address = new Address(currentUser);

            address.CityId = (DBCommon.IsInt(dr[addressType + "CityID"]) ? DBCommon.GetInt(dr[addressType + "CityID"]) : (int?)null);
            address.DistrictId = (DBCommon.IsInt(dr[addressType + "DistrictID"]) ? DBCommon.GetInt(dr[addressType + "DistrictID"]) : (int?)null);

            if (!string.IsNullOrEmpty(dr[addressType + "PostCode"].ToString()))
            {
                address.PostCode = dr[addressType + "PostCode"].ToString();
            }

            if (!string.IsNullOrEmpty(dr[addressType + "AddressText"].ToString()))
            {
                address.AddressText = dr[addressType + "AddressText"].ToString();
            }

            return address;
        }

        public static int GetAddressIdByPersonIdAndAddressType(int personId, string addressType, User currentUser)
        {
            int addressId = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.AddressID
                               FROM PMIS_ADM.PersonAddresses a
                               WHERE a.PersonID = :PersonID AND a.AddressType = :AddressType";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;
                cmd.Parameters.Add("AddressType", OracleType.VarChar).Value = addressType;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["AddressID"]))
                        addressId = DBCommon.GetInt(dr["AddressID"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return addressId;
        }
    }
}
