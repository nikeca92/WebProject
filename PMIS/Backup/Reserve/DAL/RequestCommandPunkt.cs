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
    //This class represents a punkt of RequestCommand for particular MilitaryDepartment
    public class RequestCommandPunkt : BaseDbObject, IDropDownItem
    {
        private int requestCommandPunktID;
        private int requestCommandID;
        private int militaryDepartmentID;
        private MilitaryDepartment militaryDepartment;
        private int? cityID;
        private City city;
        private string place;

        public int RequestCommandPunktID
        {
            get { return requestCommandPunktID; }
            set { requestCommandPunktID = value; }
        }

        public int RequestCommandID
        {
            get { return requestCommandID; }
            set { requestCommandID = value; }
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

        public RequestCommandPunkt(User user)
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
            return requestCommandPunktID.ToString();
        }

        #endregion
    }

    //Some methods for working with RequestCommandPunkt objects
    public static class RequestCommandPunktUtil
    {
        //This method creates and returns a RequestCommandPunkt object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static RequestCommandPunkt ExtractRequestCommandPunktFromDataReader(OracleDataReader dr, User currentUser)
        {
            RequestCommandPunkt requestCommandPunkt = new RequestCommandPunkt(currentUser);

            requestCommandPunkt.RequestCommandPunktID = DBCommon.GetInt(dr["RequestCommandPunktID"]);
            requestCommandPunkt.RequestCommandID = DBCommon.GetInt(dr["RequestCommandID"]);
            requestCommandPunkt.MilitaryDepartmentID = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
            requestCommandPunkt.CityID = DBCommon.IsInt(dr["CityID"]) ? (int?)DBCommon.GetInt(dr["CityID"]) : null;
            requestCommandPunkt.Place = dr["Place"].ToString();

            return requestCommandPunkt;
        }

        //Get a specific RequestCommandPunkt record
        public static RequestCommandPunkt GetRequestCommandPunkt(int requestCommandPunktID, User currentUser)
        {
            RequestCommandPunkt requestCommandPunkt = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestCommandPunktID,
                                      a.RequestCommandID,
                                      a.MilitaryDepartmentID,
                                      a.CityID,
                                      a.Place
                               FROM PMIS_RES.RequestCommandPunkt a                               
                               WHERE a.RequestCommandPunktID = :RequestCommandPunktID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPunktID", OracleType.Number).Value = requestCommandPunktID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    requestCommandPunkt = ExtractRequestCommandPunktFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPunkt;
        }

        //Get a specific RequestCommandPunkt record by RequestCommandID and MilitaryDepartmentID
        public static RequestCommandPunkt GetRequestCommandPunkt(int requestCommandID, int militaryDepartmentID, User currentUser)
        {
            RequestCommandPunkt requestCommandPunkt = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestCommandPunktID,
                                      a.RequestCommandID,
                                      a.MilitaryDepartmentID,
                                      a.CityID,
                                      a.Place
                               FROM PMIS_RES.RequestCommandPunkt a                               
                               WHERE a.RequestCommandID = :RequestCommandID AND a.MilitaryDepartmentID = :MilitaryDepartmentID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandID", OracleType.Number).Value = requestCommandID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    requestCommandPunkt = ExtractRequestCommandPunktFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPunkt;
        }

        //Get all RequestCommandPunkt-s record by MilitaryDepartmentID
        public static List<RequestCommandPunkt> GetAllRequestCommandPunktByMilDeptID(int militaryDepartmentID, User currentUser)
        {
            List<RequestCommandPunkt> requestCommandPunkts = new List<RequestCommandPunkt>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestCommandPunktID,
                                      a.RequestCommandID,
                                      a.MilitaryDepartmentID,
                                      a.CityID,
                                      a.Place
                               FROM PMIS_RES.RequestCommandPunkt a                               
                               WHERE a.MilitaryDepartmentID = :MilitaryDepartmentID AND
                                     NVL(a.CityID, 0) > 0 OR a.Place IS NOT NULL";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    requestCommandPunkts.Add(ExtractRequestCommandPunktFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPunkts;
        }

        public static bool SaveRequestCommandPunkt(RequestCommandPunkt punkt, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            RequestCommandPunkt oldPunkt = RequestCommandPunktUtil.GetRequestCommandPunkt(punkt.RequestCommandID, punkt.MilitaryDepartmentID, currentUser);

            RequestCommand requestsCommand = RequestCommandUtil.GetRequestsCommand(currentUser, punkt.RequestCommandID);

            ChangeEvent changeEvent = null;

            string logDescription = "";
            logDescription += "Заявка №: " + requestsCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(requestsCommand.EquipmentReservistsRequest.RequestDate) +
                              "; Команда: " + requestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; ВО: " + punkt.MilitaryDepartment.MilitaryDepartmentName;

            changeEvent = new ChangeEvent("RES_EquipResRequests_EditPunkt", logDescription, requestsCommand.EquipmentReservistsRequest.MilitaryUnit, null, currentUser);

            string oldRegionMunicipalityAndCity = oldPunkt != null && oldPunkt.City != null ? oldPunkt.City.RegionMunicipalityAndCity : "";
            string newRegionMunicipalityAndCity = punkt != null && punkt.City != null ? punkt.City.RegionMunicipalityAndCity : "";


            if (oldRegionMunicipalityAndCity != newRegionMunicipalityAndCity)
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Punkt_City", oldRegionMunicipalityAndCity, newRegionMunicipalityAndCity, currentUser));

            if (oldPunkt == null)
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Punkt_Place", "", punkt.Place, currentUser));
            else
                if (oldPunkt.Place.Trim() != punkt.Place.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Punkt_Place", oldPunkt.Place, punkt.Place, currentUser));


            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @" MERGE INTO PMIS_RES.RequestCommandPunkt USING dual ON (RequestCommandID=" + punkt.RequestCommandID.ToString() + @" AND MilitaryDepartmentID=" + punkt.MilitaryDepartmentID.ToString() + @" )
                          WHEN MATCHED THEN UPDATE SET CityID = " + (punkt.CityID.HasValue ? punkt.CityID.Value.ToString() : "NULL") + @", Place = '" + punkt.Place + @"'
                          WHEN NOT MATCHED THEN INSERT (RequestCommandID, MilitaryDepartmentID, CityID, Place) VALUES ( " + punkt.RequestCommandID.ToString() + @", " + punkt.MilitaryDepartmentID.ToString() + @", " + (punkt.CityID.HasValue ? punkt.CityID.Value.ToString() : "NULL") + @", '" + punkt.Place + @"' )
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