using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a punkt of TechnicsRequestCommandPunkt for particular MilitaryDepartment
    public class TechnicsRequestCommandPunkt : BaseDbObject, IDropDownItem
    {
        private int technicsRequestCommandPunktID;
        private int technicsRequestCommandID;
        private int militaryDepartmentID;
        private MilitaryDepartment militaryDepartment;
        private int? cityID;
        private City city;
        private string place;

        public int TechnicsRequestCommandPunktID
        {
            get { return technicsRequestCommandPunktID; }
            set { technicsRequestCommandPunktID = value; }
        }

        public int TechnicsRequestCommandID
        {
            get { return technicsRequestCommandID; }
            set { technicsRequestCommandID = value; }
        }

        public int MilitaryDepartmentID
        {
            get
            { return militaryDepartmentID; }
            set { militaryDepartmentID = value; }
        }

        public MilitaryDepartment MilitaryDepartment
        {
            get
            {
                if (militaryDepartment == null)
                    militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(MilitaryDepartmentID, CurrentUser);

                return militaryDepartment;
            }
            set { militaryDepartment = value; }
        }

        public int? CityID
        {
            get
            { return cityID; }
            set { cityID = value; }
        }

        public City City
        {
            get
            {
                if (city == null && CityID.HasValue)
                    city = CityUtil.GetCity(CityID.Value, CurrentUser);

                return city;
            }
            set { city = value; }
        }

        public string Place
        {
            get { return place; }
            set { place = value; }
        }

        public TechnicsRequestCommandPunkt(User user)
            : base(user)
        {

        }

        #region IDropDownItem Members

        public string Text()
        {
            return (City != null ? City.CityName + ", " : "") + place;
        }

        public string Value()
        {
            return technicsRequestCommandPunktID.ToString();
        }

        #endregion
    }

    //Some methods for working with RequestCommandPunkt objects
    public static class TechnicsRequestCommandPunktUtil
    {
        //This method creates and returns a TechnicsRequestCommandPunkt object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static TechnicsRequestCommandPunkt ExtractTechnicsRequestCommandPunktFromDataReader(OracleDataReader dr, User currentUser)
        {
            TechnicsRequestCommandPunkt requestCommandPunkt = new TechnicsRequestCommandPunkt(currentUser);

            requestCommandPunkt.TechnicsRequestCommandPunktID = DBCommon.GetInt(dr["TechRequestCommandPunktID"]);
            requestCommandPunkt.TechnicsRequestCommandID = DBCommon.GetInt(dr["TechRequestsCommandID"]);
            requestCommandPunkt.MilitaryDepartmentID = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
            requestCommandPunkt.CityID = DBCommon.IsInt(dr["CityID"]) ? (int?)DBCommon.GetInt(dr["CityID"]) : null;
            requestCommandPunkt.Place = dr["Place"].ToString();

            return requestCommandPunkt;
        }

        //Get a specific RequestCommandPunkt record
        public static TechnicsRequestCommandPunkt GetTechnicsRequestCommandPunkt(int techRequestCommandPunktID, User currentUser)
        {
            TechnicsRequestCommandPunkt requestCommandPunkt = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechRequestCommandPunktID,
                                      a.TechRequestsCommandID,
                                      a.MilitaryDepartmentID,
                                      a.CityID,
                                      a.Place
                               FROM PMIS_RES.TechRequestCommandPunkt a                               
                               WHERE a.TechRequestCommandPunktID = :TechRequestCommandPunktID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestCommandPunktID", OracleType.Number).Value = techRequestCommandPunktID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    requestCommandPunkt = ExtractTechnicsRequestCommandPunktFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPunkt;
        }

        //Get a specific TechnicsRequestCommandPunkt record by TechRequestCommandID and MilitaryDepartmentID
        public static TechnicsRequestCommandPunkt GetTechnicsRequestCommandPunkt(int techRequestCommandID, int militaryDepartmentID, User currentUser)
        {
            TechnicsRequestCommandPunkt requestCommandPunkt = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechRequestCommandPunktID,
                                      a.TechRequestsCommandID,
                                      a.MilitaryDepartmentID,
                                      a.CityID,
                                      a.Place
                               FROM PMIS_RES.TechRequestCommandPunkt a                               
                               WHERE a.TechRequestsCommandID = :TechRequestsCommandID AND a.MilitaryDepartmentID = :MilitaryDepartmentID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = techRequestCommandID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    requestCommandPunkt = ExtractTechnicsRequestCommandPunktFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPunkt;
        }

        //Get all TechnicsRequestCommandPunkt-s record by MilitaryDepartmentID
        public static List<TechnicsRequestCommandPunkt> GetAllTechnicsRequestCommandPunktByMilDeptID(int militaryDepartmentID, User currentUser)
        {
            List<TechnicsRequestCommandPunkt> requestCommandPunkts = new List<TechnicsRequestCommandPunkt>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechRequestCommandPunktID,
                                      a.TechRequestsCommandID,
                                      a.MilitaryDepartmentID,
                                      a.CityID,
                                      a.Place
                               FROM PMIS_RES.TechRequestCommandPunkt a                               
                               WHERE a.MilitaryDepartmentID = :MilitaryDepartmentID AND
                                     NVL(a.CityID, 0) > 0 OR a.Place IS NOT NULL";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    requestCommandPunkts.Add(ExtractTechnicsRequestCommandPunktFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPunkts;
        }

        public static bool SaveTechnicsRequestCommandPunkt(TechnicsRequestCommandPunkt punkt, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            TechnicsRequestCommandPunkt oldPunkt = TechnicsRequestCommandPunktUtil.GetTechnicsRequestCommandPunkt(punkt.TechnicsRequestCommandID, punkt.MilitaryDepartmentID, currentUser);

            TechnicsRequestCommand requestsCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(currentUser, punkt.TechnicsRequestCommandID);

            ChangeEvent changeEvent = null;

            string logDescription = "";
            logDescription += "Заявка №: " + requestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(requestsCommand.EquipmentTechnicsRequest.RequestDate) +
                              "; Команда: " + requestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; ВО: " + punkt.MilitaryDepartment.MilitaryDepartmentName;

            changeEvent = new ChangeEvent("RES_EquipTechRequests_EditPunkt", logDescription, requestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, currentUser);

            string oldRegionMunicipalityAndCity = oldPunkt != null && oldPunkt.City != null ? oldPunkt.City.RegionMunicipalityAndCity : "";
            string newRegionMunicipalityAndCity = punkt != null && punkt.City != null ? punkt.City.RegionMunicipalityAndCity : "";


            if (oldRegionMunicipalityAndCity != newRegionMunicipalityAndCity)
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Punkt_City", oldRegionMunicipalityAndCity, newRegionMunicipalityAndCity, currentUser));

            if (oldPunkt == null)
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Punkt_Place", "", punkt.Place, currentUser));
            else
                if (oldPunkt.Place.Trim() != punkt.Place.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Punkt_Place", oldPunkt.Place, punkt.Place, currentUser));


            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @" MERGE INTO PMIS_RES.TechRequestCommandPunkt USING dual ON (TechRequestsCommandID=" + punkt.TechnicsRequestCommandID.ToString() + @" AND MilitaryDepartmentID=" + punkt.MilitaryDepartmentID.ToString() + @" )
                          WHEN MATCHED THEN UPDATE SET CityID = " + (punkt.CityID.HasValue ? punkt.CityID.Value.ToString() : "NULL") + @", Place = '" + punkt.Place + @"'
                          WHEN NOT MATCHED THEN INSERT (TechRequestsCommandID, MilitaryDepartmentID, CityID, Place) VALUES ( " + punkt.TechnicsRequestCommandID.ToString() + @", " + punkt.MilitaryDepartmentID.ToString() + @", " + (punkt.CityID.HasValue ? punkt.CityID.Value.ToString() : "NULL") + @", '" + punkt.Place + @"' )
                        ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }
    }
}