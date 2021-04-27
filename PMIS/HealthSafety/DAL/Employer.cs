using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Text;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Employer : BaseDbObject
    {
        private int employerId;
        public int EmployerId
        {
            get { return employerId; }
            set { employerId = value; }
        }

        private string employerName = "";
        public string EmployerName
        {
            get { return employerName; }
            set { employerName = value; }
        }

        private string emplEik = "";
        public string EmplEik
        {
            get { return emplEik; }
            set { emplEik = value; }
        }

        private City city;
        public City City
        {
            get
            {
                return city;
            }
            set { city = value; }
        }

        private string emplStreet = "";
        public string EmplStreet
        {
            get { return emplStreet; }
            set { emplStreet = value; }
        }

        private string emplStreetNum = "";
        public string EmplStreetNum
        {
            get { return emplStreetNum; }
            set { emplStreetNum = value; }
        }

        private string emplDistrict = "";
        public string EmplDistrict
        {
            get { return emplDistrict; }
            set { emplDistrict = value; }
        }

        private string emplBlock = "";
        public string EmplBlock
        {
            get { return emplBlock; }
            set { emplBlock = value; }
        }

        private string emplEntrance = "";
        public string EmplEntrance
        {
            get { return emplEntrance; }
            set { emplEntrance = value; }
        }

        private string emplFloor = "";
        public string EmplFloor
        {
            get { return emplFloor; }
            set { emplFloor = value; }
        }

        private string emplApt = "";
        public string EmplApt
        {
            get { return emplApt; }
            set { emplApt = value; }
        }

        private string emplPhone = "";
        public string EmplPhone
        {
            get { return emplPhone; }
            set { emplPhone = value; }
        }

        private string emplFax = "";
        public string EmplFax
        {
            get { return emplFax; }
            set { emplFax = value; }
        }

        private string emplEmail = "";
        public string EmplEmail
        {
            get { return emplEmail; }
            set { emplEmail = value; }
        }

        private int? emplNumberOfEmployees;
        public int? EmplNumberOfEmployees
        {
            get { return emplNumberOfEmployees; }
            set { emplNumberOfEmployees = value; }
        }

        private int? emplFemaleEmployees;
        public int? EmplFemaleEmployees
        {
            get { return emplFemaleEmployees; }
            set { emplFemaleEmployees = value; }
        }

        public Employer(User currentUser)
            : base(currentUser)
        {
            employerId = 0;
        }
        public Employer(int _employerId, User currentUser)
            : base(currentUser)
        {
            employerId = _employerId;
        }

        

    }

    public static class EmployerUtil
    {
        public static List<Employer> GetEmployers(User currentUser)
        {
            Employer employer;
            List<Employer> listEmployer = new List<Employer>();
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT EmployerId,
                              EmployerName
                              FROM PMIS_HS.EMPLOYERS 
                              ORDER BY EmployerId
                              ASC";
                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    employer = new Employer(currentUser);

                    employer.EmployerId = DBCommon.GetInt(dr["EmployerId"]);
                    employer.EmployerName = dr["EmployerName"].ToString();
                    listEmployer.Add(employer);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listEmployer;
        }

        public static Employer GetEmployerForDeclarationId(int declarationId, User currentUser)
        {
            Employer employer = new Employer(currentUser);

            if (declarationId == 0)
            {
                return employer;
            }

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                SQL = @"SELECT          EmployerId,
                                        EmployerName,
                                        EmplEik,
                                        EmplCityId,
                                        EmplStreet,
                                        EmplStreetNum,
                                        EmplDistrict,
                                        EmplBlock,
                                        EmplEntrance,
                                        EmplFloor,
                                        EmplApt,
                                        EmplPhone,
                                        EmplFax,
                                        EmplEmail,
                                        EmplNumberOfEmployees,
                                        EmplFemaleEmployees

                                FROM PMIS_HS.EMPLOYERS 
                                WHERE EmployerId = (SELECT employerId 
                                FROM PMIS_HS.DECLARATIONSOFACCIDENT
                                where DECLARATIONID=:declarationId)
                              ";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "declarationId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = declarationId;

                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    employer = FillEmployeer(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return employer;
        }

        public static Employer GetEmployerForEmployerId(int employerId, User currentUser)
        {
            Employer employer = new Employer(currentUser);

            if (employerId == 0)
            {
                return employer;
            }

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                SQL = @"SELECT          EmployerId,
                                        EmployerName,
                                        EmplEik,
                                        EmplCityId,
                                        EmplStreet,
                                        EmplStreetNum,
                                        EmplDistrict,
                                        EmplBlock,
                                        EmplEntrance,
                                        EmplFloor,
                                        EmplApt,
                                        EmplPhone,
                                        EmplFax,
                                        EmplEmail,
                                        EmplNumberOfEmployees,
                                        EmplFemaleEmployees

                                FROM PMIS_HS.EMPLOYERS 
                                WHERE EmployerId = :employerId
                              ";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "employerId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = employerId;

                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    employer = FillEmployeer(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return employer;
        }

        private static Employer FillEmployeer(OracleDataReader dr, User currentUser)
        {
            Employer employer = new Employer(currentUser);

            employer.EmployerId = DBCommon.GetInt(dr["EmployerId"]);

            if (!string.IsNullOrEmpty(dr["EmployerName"].ToString()))
            {
                employer.EmployerName = dr["EmployerName"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["EmplEik"].ToString()))
            {
                employer.EmplEik = dr["EmplEik"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["EmplStreet"].ToString()))
            {
                employer.EmplStreet = dr["EmplStreet"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplStreetNum"].ToString()))
            {
                employer.EmplStreetNum = dr["EmplStreetNum"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplDistrict"].ToString()))
            {
                employer.EmplDistrict = dr["EmplDistrict"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplBlock"].ToString()))
            {
                employer.EmplBlock = dr["EmplBlock"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplEntrance"].ToString()))
            {
                employer.EmplEntrance = dr["EmplEntrance"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplFloor"].ToString()))
            {
                employer.EmplFloor = dr["EmplFloor"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplApt"].ToString()))
            {
                employer.EmplApt = dr["EmplApt"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplPhone"].ToString()))
            {
                employer.EmplPhone = dr["EmplPhone"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplFax"].ToString()))
            {
                employer.EmplFax = dr["EmplFax"].ToString();
            }
            if (!string.IsNullOrEmpty(dr["EmplEmail"].ToString()))
            {
                employer.EmplEmail = dr["EmplEmail"].ToString();
            }

            City city = new City(currentUser);
            if (DBCommon.IsInt(dr["emplCityId"])) city.CityId = DBCommon.GetInt(dr["emplCityId"]);
            //fill parent object
            employer.City = city;

            if (DBCommon.IsInt(dr["emplNumberOfEmployees"])) employer.EmplNumberOfEmployees = DBCommon.GetInt(dr["emplNumberOfEmployees"]);
            if (DBCommon.IsInt(dr["emplFemaleEmployees"])) employer.EmplFemaleEmployees = DBCommon.GetInt(dr["emplFemaleEmployees"]);

            return employer;
        }
    }
}