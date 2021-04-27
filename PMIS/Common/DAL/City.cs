using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class Region : BaseDbObject, IDropDownItem
    {
        private int regionId;
        private string regionName;
        private List<Municipality> municipalities;

        public int RegionId
        {
            get { return regionId; }
            set { regionId = value; }
        }        

        public string RegionName
        {
            get { return regionName; }
            set { regionName = value; }
        }

        public List<Municipality> Municipalities
        {
            get
            {
                if (municipalities == null)
                {
                    municipalities = MunicipalityUtil.GetMunicipalities(regionId, CurrentUser);
                }

                return municipalities;
            }
        }

        public Region(User currentUser)
            : base(currentUser)
        {
            regionId = 0;
        }
        public Region(int _regionId, User currentUser)
            : base(currentUser)
        {
            regionId = _regionId;
        }

        //IDropDownItem Members
        public string Text()
        {
            return regionName;
        }

        public string Value()
        {
            return regionId.ToString();
        }
    }

    public class Municipality : BaseDbObject, IDropDownItem
    {
        private int municipalityId;
        private string municipalityName;
        private List<City> cities;

        public int MunicipalityId
        {
            get { return municipalityId; }
            set { municipalityId = value; }
        }        

        public string MunicipalityName
        {
            get { return municipalityName; }
            set { municipalityName = value; }
        }

        public List<City> Cities
        {
            get
            {
                if (cities == null)
                {
                    cities = CityUtil.GetCities(municipalityId, CurrentUser);
                }

                return cities;
            }
        }

        public Municipality(User currentUser)
            : base(currentUser)
        {
            municipalityId = 0;
        }

        public Municipality(int _municipalityId, User currentUser)
            : base(currentUser)
        {
            municipalityId = _municipalityId;
        }

        //IDropDownItem Members
        public string Text()
        {
            return municipalityName;
        }

        public string Value()
        {
            return municipalityId.ToString();
        }
    }

    public class City : BaseDbObject, IDropDownItem
    {
        private int cityId;
        private string cityName = "";
        private int postCode;
        private int municipalityId;
        private Municipality municipality;
        private int regionId;
        private Region region;
        private List<District> districts;

        public int CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }
        
        public string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }

        public int PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public int MunicipalityId
        {
            get { return municipalityId; }
            set { municipalityId = value; }
        }

        public Municipality Municipality
        {
            get
            {
                if (municipality == null)
                    municipality = MunicipalityUtil.GetMunicipality(municipalityId, CurrentUser);

                return municipality;
            }
            set { municipality = value; }
        }

        public int RegionId
        {
            get { return regionId; }
            set { regionId = value; }
        }

        public Region Region
        {
            get
            {
                if (region == null)
                    region = RegionUtil.GetRegion(regionId, CurrentUser);

                return region;
            }
            set { region = value; }
        }

        public List<District> Districts
        {
            get
            {
                if (districts == null)
                {
                    districts = DistrictUtil.GetDistricts(cityId, CurrentUser);
                }

                return districts;
            }
        }

        public string RegionMunicipalityAndCity
        {
            get
            {
                string str = "";

                if (this != null)
                    str += Region.RegionName + ", " + Municipality.MunicipalityName + ", " + this.CityName;

                if (this.PostCode > 0)
                    str += ", п.к. " + PostCode.ToString();

                return str;
            }
        }

        public City(User currentUser)
            : base(currentUser)
        {

        }

        //IDropDownItem Members
        public string Text()
        {
            return cityName;
        }

        public string Value()
        {
            return cityId.ToString();
        }
    }

    public class District : BaseDbObject, IDropDownItem
    {
        private int districtId;
        private string districtName = "";
        private string postCode;

        public int DistrictId
        {
            get
            {
                return districtId;
            }
            set
            {
                districtId = value;
            }
        }

        public string DistrictName
        {
            get
            {
                return districtName;
            }
            set
            {
                districtName = value;
            }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }
                
        public District(User currentUser)
            : base(currentUser)
        {

        }

        //IDropDownItem Members
        public string Text()
        {
            return districtName;
        }

        public string Value()
        {
            return districtId.ToString();
        }
    }

    public static class RegionUtil
    {
        public static Region GetRegion(int regionId, User currentUser)
        {
            Region region = null;
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT Kod_Obl, Ime_Obl FROM UKAZ_OWNER.KL_OBL WHERE Kod_Obl= :RegionId ORDER BY Ime_Obl";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param = new OracleParameter();
                param.ParameterName = "RegionId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = regionId;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    region = new Region(currentUser);
                    region.RegionId = DBCommon.GetInt(dr["Kod_Obl"]);
                    region.RegionName = dr["Ime_Obl"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return region;
        }

        public static List<Region> GetRegions(User currentUser)
        {
            Region region;
            List<Region> listRegion = new List<Region>();
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT Kod_Obl, Ime_Obl FROM UKAZ_OWNER.KL_OBL ORDER BY  ime_Obl";
                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    region = new Region(currentUser);
                    region.RegionId = DBCommon.GetInt(dr["Kod_Obl"]);
                    region.RegionName = dr["Ime_Obl"].ToString();
                    listRegion.Add(region);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listRegion;
        }
    }

    public static class MunicipalityUtil
    {
        public static Municipality GetMunicipality(int municipalityId, User currentUser)
        {
            Municipality municipality = null;
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT Kod_Obs, Ime_Obs FROM UKAZ_OWNER.KL_OBS WHERE Kod_Obs= :MunicipalityId ORDER BY Ime_Obs";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param = new OracleParameter();
                param.ParameterName = "MunicipalityId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = municipalityId;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    municipality = new Municipality(currentUser);
                    municipality.MunicipalityId = DBCommon.GetInt(dr["Kod_Obs"]);
                    municipality.MunicipalityName = dr["Ime_Obs"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return municipality;
        }

        public static List<Municipality> GetMunicipalities(int regionId, User currentUser)
        {
            Municipality municipality;
            List<Municipality> listMunicipality = new List<Municipality>();

            if (regionId == 0)
            {
                return listMunicipality;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.Kod_Obs, a.Ime_Obs FROM UKAZ_OWNER.KL_OBS a
                                inner join UKAZ_OWNER.KL_OBL b
                                on a.kod_obl= b.kod_obl
                                where a.kod_obl= :regionId
                                order by a.ime_obs";


                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleParameter param = new OracleParameter();
                param = new OracleParameter();
                param.ParameterName = "regionId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = regionId;
                cmd.Parameters.Add(param);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    municipality = new Municipality(currentUser);
                    municipality.MunicipalityId = DBCommon.GetInt(dr["Kod_Obs"]);
                    municipality.MunicipalityName = dr["Ime_Obs"].ToString();
                    listMunicipality.Add(municipality);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMunicipality;
        }
    }

    public static class CityUtil
    {
        public static City GetCity(int cityId, User currentUser)
        {
            City city = new City(currentUser);
            if (cityId == 0)
            {
                return city;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.Kod_Nma as CityID, a.Ime_Nma as CityName, a.PK as PostCode, 
                                    a.KOD_OBS as MunicipalityID, a.KOD_OBL as RegionID 
                                FROM UKAZ_OWNER.KL_NMA a
                                WHERE a.Kod_Nma = :CityID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CityID", OracleType.Number).Value = cityId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    city = new City(currentUser);
                    city.CityId = cityId;
                    city.CityName = dr["CityName"].ToString();
                    city.PostCode = (DBCommon.IsInt(dr["PostCode"]) ? DBCommon.GetInt(dr["PostCode"]) : 0);
                    city.MunicipalityId = (DBCommon.IsInt(dr["MunicipalityID"]) ? DBCommon.GetInt(dr["MunicipalityID"]) : 0);
                    city.RegionId = (DBCommon.IsInt(dr["RegionID"]) ? DBCommon.GetInt(dr["RegionID"]) : 0);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return city;
        }

        public static City GetCityByName(string cityName, User currentUser)
        {
            City city = null;

            if (string.IsNullOrEmpty(cityName))
            {
                return city;
            }

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.Kod_Nma as CityID, a.Ime_Nma as CityName, a.PK as PostCode, 
                                    a.KOD_OBS as MunicipalityID, a.KOD_OBL as RegionID 
                                FROM UKAZ_OWNER.KL_NMA a
                                WHERE a.Ime_Nma = :CityName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CityName", OracleType.VarChar).Value = cityName;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    city = new City(currentUser);
                    city.CityId = DBCommon.GetInt(dr["CityID"]);
                    city.CityName = dr["CityName"].ToString();
                    city.PostCode = (DBCommon.IsInt(dr["PostCode"]) ? DBCommon.GetInt(dr["PostCode"]) : 0);
                    city.MunicipalityId = (DBCommon.IsInt(dr["MunicipalityID"]) ? DBCommon.GetInt(dr["MunicipalityID"]) : 0);
                    city.RegionId = (DBCommon.IsInt(dr["RegionID"]) ? DBCommon.GetInt(dr["RegionID"]) : 0);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return city;
        }

        //Get RegionId, MunisipalityId and CityId for postCode 
        public static City GetCityByPostCode(int postCode, User currentUser)
        {
            City city = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.Kod_Nma as CityID, a.Ime_Nma as CityName, a.PK as PostCode, 
                                    a.KOD_OBS as MunicipalityID, a.KOD_OBL as RegionID 
                                FROM UKAZ_OWNER.KL_NMA a
                                WHERE a.PK = :PostCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PostCode";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = postCode;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    city = new City(currentUser);
                    city.CityId = (DBCommon.IsInt(dr["CityID"]) ? DBCommon.GetInt(dr["CityID"]) : 0);
                    city.CityName = dr["CityName"].ToString();
                    city.PostCode = postCode;
                    city.MunicipalityId = (DBCommon.IsInt(dr["MunicipalityID"]) ? DBCommon.GetInt(dr["MunicipalityID"]) : 0);
                    city.RegionId = (DBCommon.IsInt(dr["RegionID"]) ? DBCommon.GetInt(dr["RegionID"]) : 0);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return city;
        }

        public static List<City> GetCities(int municipalityId, User currentUser)
        {
            City city;
            List<City> listCity = new List<City>();

            if (municipalityId == 0)
            {
                return listCity;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.Kod_Nma as CityID, a.Ime_Nma as CityName, a.PK as PostCode, 
                                    a.KOD_OBS as MunicipalityID, a.KOD_OBL as RegionID 
                                FROM UKAZ_OWNER.KL_NMA a
                                INNER JOIN UKAZ_OWNER.KL_OBS b
                                on a.kod_obs = b.kod_obs
                                where b.kod_obs= :municipalityId
                                order by a.ime_nma";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "municipalityId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = municipalityId;
                cmd.Parameters.Add(param);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    city = new City(currentUser);
                    city.CityId = DBCommon.GetInt(dr["CityID"]);
                    city.CityName = dr["CityName"].ToString();
                    city.PostCode = (DBCommon.IsInt(dr["MunicipalityID"]) ? DBCommon.GetInt(dr["MunicipalityID"]) : 0);
                    city.MunicipalityId = DBCommon.GetInt(dr["MunicipalityID"]);
                    city.RegionId = DBCommon.GetInt(dr["RegionID"]);
                    listCity.Add(city);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listCity;
        }

        public static List<string> GetRegionMunicipalityPostCodeByCityId(int? cityId, User currentUser)
        {

            List<String> listRegionMunicipalityPostCode = new List<String>();

            if (!cityId.HasValue)
            {
                return listRegionMunicipalityPostCode;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT b.ime_obl, c.ime_obs, a.ime_nma, a.PK FROM UKAZ_OWNER.KL_NMA a 
inner join UKAZ_OWNER.KL_OBL  b on a.KOD_OBL=(SELECT KOD_OBL FROM UKAZ_OWNER.KL_NMA c where c.kod_nma =:cityId)
inner join UKAZ_OWNER.KL_OBS  c on a.KOD_OBS=(SELECT KOD_OBS FROM UKAZ_OWNER.KL_NMA c where c.kod_nma =:cityId)
where a.kod_nma =:cityId and b.kod_obl= a.kod_obl and c.kod_obs= a.kod_obs";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "cityId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = cityId;
                cmd.Parameters.Add(param);
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    listRegionMunicipalityPostCode.Add(dr["ime_obl"].ToString());
                    listRegionMunicipalityPostCode.Add(dr["ime_obs"].ToString());
                    listRegionMunicipalityPostCode.Add(dr["ime_nma"].ToString());
                    listRegionMunicipalityPostCode.Add(dr["PK"].ToString());
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listRegionMunicipalityPostCode ;
        }
    }

    public static class DistrictUtil
    {
        //Get a particular District by its ID
        public static District GetDistrict(int districtId, User currentUser)
        {
            District district = new District(currentUser);

            if (districtId == 0)
            {
                return district;
            }

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.DistrictID, a.DistrictName, a.PostCode
                               FROM UKAZ_OWNER.Districts a
                               WHERE a.DistrictID = :DistrictID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("DistrictID", OracleType.Number).Value = districtId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    district = new District(currentUser);
                    district.DistrictId = districtId;
                    district.DistrictName = dr["DistrictName"].ToString();
                    district.PostCode = dr["PostCode"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return district;
        }

        //Get a particular District by its PostCode
        public static District GetDistrictByPostCode(string postCode, User currentUser)
        {
            District district = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.DistrictID, a.DistrictName, a.PostCode
                               FROM UKAZ_OWNER.Districts a
                               WHERE a.PostCode = :PostCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PostCode";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                param.Value = postCode;
                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    district = new District(currentUser);
                    district.DistrictId = DBCommon.GetInt(dr["DistrictID"]);
                    district.DistrictName = dr["DistrictName"].ToString();
                    district.PostCode = dr["PostCode"].ToString();                   
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return district;
        }

        public static List<District> GetDistricts(int cityId, User currentUser)
        {
            District district;
            List<District> listDistrict = new List<District>();

            if (cityId == 0)
            {
                return listDistrict;
            }

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.DistrictID, a.DistrictName, a.PostCode
                               FROM UKAZ_OWNER.Districts a
                               INNER JOIN UKAZ_OWNER.Districts_Cities b ON a.DistrictID = b.DistrictID
                               WHERE b.CityID = :CityID
                               ORDER BY a.DistrictName";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "CityID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = cityId;
                cmd.Parameters.Add(param);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    district = new District(currentUser);
                    district.DistrictId = DBCommon.GetInt(dr["DistrictID"]);
                    district.DistrictName = dr["DistrictName"].ToString();
                    district.PostCode = dr["PostCode"].ToString();

                    listDistrict.Add(district);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listDistrict;
        }
    }
}
