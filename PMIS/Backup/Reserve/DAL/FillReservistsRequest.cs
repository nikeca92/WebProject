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
    //This class represents a FillReservistsRequest record
    public class FillReservistsRequest : BaseDbObject
    {
        private int fillReservistsRequestID;
        private int requestCommandPositionID;
        private int militaryDepartmentID;        
        private MilitaryDepartment militaryDepartment;        
        private int reservistID;
        private Reservist reservist;        
        private int reservistReadinessID;
        private int milReportSpecialityID;
        private MilitaryReportSpeciality militaryReportSpeciality;
        private bool needCourse;
        private bool appointmentIsDelivered;

        public int FillReservistsRequestID
        {
            get { return fillReservistsRequestID; }
            set { fillReservistsRequestID = value; }
        }

        public int RequestCommandPositionID
        {
            get { return requestCommandPositionID; }
            set { requestCommandPositionID = value; }
        }

        public int MilitaryDepartmentID
        {
            get { return militaryDepartmentID; }
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

        public int ReservistID
        {
            get { return reservistID; }
            set { reservistID = value; }
        }

        public Reservist Reservist
        {
            get 
            {
                if (reservist == null)
                    reservist = ReservistUtil.GetReservist(ReservistID, CurrentUser);

                return reservist; 
            }
            set { reservist = value; }
        }

        public int ReservistReadinessID
        {
            get { return reservistReadinessID; }
            set { reservistReadinessID = value; }
        }

        public string ReservistReadiness
        {
            get
            {
                return ReadinessUtil.ReadinessName(ReservistReadinessID);
            }
        }

        public int MilReportSpecialityID
        {
            get { return milReportSpecialityID; }
            set { milReportSpecialityID = value; }
        }

        public MilitaryReportSpeciality MilitaryReportSpeciality
        {
            get
            {
                if (militaryReportSpeciality == null)
                    militaryReportSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(MilReportSpecialityID, CurrentUser);

                return militaryReportSpeciality;
            }
            set { militaryReportSpeciality = value; }
        }

        public bool NeedCourse
        {
            get { return needCourse; }
            set { needCourse = value; }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }

        public FillReservistsRequest(User user)
            : base(user)
        {

        }
    }

    //Some methods for working with FillReservistsRequest objects
    public static class FillReservistsRequestUtil
    {
        //This method creates and returns a FillReservistsRequest object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static FillReservistsRequest ExtractFillReservistsRequest(OracleDataReader dr, User currentUser)
        {
            FillReservistsRequest fillReservistsRequest = new FillReservistsRequest(currentUser);

            fillReservistsRequest.FillReservistsRequestID = DBCommon.GetInt(dr["FillReservistsRequestID"]);
            fillReservistsRequest.RequestCommandPositionID = DBCommon.GetInt(dr["RequestCommandPositionID"]);
            fillReservistsRequest.MilitaryDepartmentID = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
            fillReservistsRequest.ReservistID = DBCommon.GetInt(dr["ReservistID"]);
            fillReservistsRequest.ReservistReadinessID = DBCommon.GetInt(dr["ReservistReadinessID"]);
            fillReservistsRequest.MilReportSpecialityID = DBCommon.GetInt(dr["MilReportSpecialityID"]);
            fillReservistsRequest.NeedCourse = DBCommon.GetInt(dr["NeedCourse"]) == 1;
            fillReservistsRequest.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;

            return fillReservistsRequest;
        }

        //Get a specific FillReservistsRequest record
        public static FillReservistsRequest GetFillReservistsRequest(int fillReservistsRequestID, User currentUser)
        {
            FillReservistsRequest fillReservistsRequest = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.FillReservistsRequestID,
                                      a.RequestCommandPositionID,
                                      a.MilitaryDepartmentID,
                                      a.ReservistID,
                                      a.ReservistReadinessID,
                                      a.MilReportSpecialityID,
                                      a.NeedCourse,
                                      a.AppointmentIsDelivered
                               FROM PMIS_RES.FillReservistsRequest a                               
                               WHERE a.FillReservistsRequestID = :FillReservistsRequestID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FillReservistsRequestID", OracleType.Number).Value = fillReservistsRequestID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    fillReservistsRequest = ExtractFillReservistsRequest(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillReservistsRequest;
        }

        //Get all FillReservistsRequest records for RequestCommandPosition and MilitaryDepartment with prefilled Reservist and Reservist.Person properties
        public static List<FillReservistsRequest> GetAllFillReservistsRequestByRequestCommandPosition(int requestCommandPositionID, int militaryDepartmentID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<FillReservistsRequest> fillReservistsRequests = new List<FillReservistsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "c.EGN";
                        break;
                    case 2:
                        orderBySQL = "c.IME || ' ' || c.FAM";
                        break;
                    case 3:
                        orderBySQL = "h.MilReportingSpecialityCode";
                        break;
                    case 4:
                        orderBySQL = "a.ReservistReadinessID";
                        break;
                    case 5:
                        orderBySQL = "a.AppointmentIsDelivered";
                        break;  
                    default:
                        orderBySQL = "c.IME";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.FillReservistsRequestID,
                                      tmp.RequestCommandPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.ReservistID,
                                      tmp.ReservistReadinessID,
                                      tmp.MilReportSpecialityID,
                                      tmp.NeedCourse,
                                      tmp.AppointmentIsDelivered,
                                      
                                      tmp.PersonID,
                                      tmp.GroupManagementSection, tmp.Section, tmp.Deliverer, tmp.PunktID, 
                                      tmp.OtherInfo,
                                      tmp.CreatedBy, tmp.CreatedDate, tmp.LastModifiedBy, tmp.LastModifiedDate,
                                      
                                      tmp.FirstName, 
                                      tmp.LastName, 
                                      tmp.IdentNumber, 
                                      tmp.PersonTypeCode,
                                      tmp.MilitaryRankID,
                                      tmp.CategoryCode,
                                      tmp.PermCityID,
                                      tmp.PermDistrictID,
                                      tmp.PermAddress,
                                      tmp.HomePhone,
                                      tmp.MilitaryUnitID,
                                      tmp.GenderID, tmp.GenderName,                                      
                                      tmp.PresCityID, tmp.PresDistrictID, tmp.PresAddress, 
                                      tmp.MobilePhone, tmp.BusinessPhone, tmp.Email, tmp.MilitaryService, tmp.MilitaryService,
                                      tmp.Initials, tmp.IDCardNumber, tmp.IDCardIssuedBy, tmp.IDCardIssueDate,
                                      tmp.BirthCountryID, tmp.BirthCountryName,
                                      tmp.BirthCityID, tmp.BirthCityIfAbroad,
                                      tmp.MilitaryTraining, 
                                      tmp.RecordOfServiceSeries, tmp.RecordOfServiceNumber, tmp.RecordOfServiceDate, tmp.RecordOfServiceCopy,
                                      tmp.MaritalStatusKey, tmp.MaritalStatusName, tmp.ParentsContact, tmp.ChildCount,
                                      tmp.SizeClothingID, tmp.SizeHatID, tmp.SizeShoesID, tmp.PersonHeight,
                                      tmp.IsAbroad, tmp.AbroadCountryID, tmp.AbroadSince, tmp.AbroadPeriod,
                                      tmp.AdministrationID,
                                      tmp.ClInformationAccLevelBg, tmp.ClInformationAccLevelBgExpDate,
                                      tmp.WorkPositionNKPDID, tmp.WorkCompanyID, tmp.PermSecondPostCode, tmp.PresSecondPostCode, tmp.IsSuitableForMobAppointment,
                                      tmp.RowNumber as RowNumber,
                                      tmp.ContactCityID, tmp.ContactDistrictID, tmp.ContactPostCode, tmp.ContactAddressText
                                      FROM (  SELECT  a.FillReservistsRequestID,
                                                      a.RequestCommandPositionID,
                                                      a.MilitaryDepartmentID,
                                                      a.ReservistID,
                                                      a.ReservistReadinessID,
                                                      a.MilReportSpecialityID,
                                                      a.NeedCourse,
                                                      a.AppointmentIsDelivered,
                                                      
                                                      b.PersonID, 
                                                      b.GroupManagementSection, b.Section, b.Deliverer, b.PunktID,
                                                      c.TXT as OtherInfo, 
                                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                                      
                                                      c.IME as FirstName, 
                                                      c.FAM as LastName, 
                                                      c.EGN as IdentNumber, 
                                                      c.KOD_KZV as PersonTypeCode,
                                                      c.KOD_ZVA as MilitaryRankID,
                                                      c.KOD_KAT as CategoryCode,
                                                      c.KOD_NMA_MJ as PermCityID,
                                                      c.PermAddrDistrictID as PermDistrictID,
                                                      c.ADRES as PermAddress,
                                                      c.TEL as HomePhone,
                                                      c.V_PODELENIE as MilitaryUnitID,
                                                      d.GenderID, e.GenderName,                                      
                                                      c.CurrAddrCityID as PresCityID, c.CurrAddrDistrictID as PresDistrictID, c.CurrAddress as PresAddress, 
                                                      d.MobilePhone, d.BusinessPhone, d.Email, d.MilitaryService,
                                                      d.Initials, d.IDCardNumber, d.IDCardIssuedBy, d.IDCardIssueDate,
                                                      f.DJJ_KOD as BirthCountryID, f.DJJ_IME as BirthCountryName,
                                                      c.KOD_NMA_MR as BirthCityID, d.BirthCityIfAbroad,
                                                      d.MilitaryTraining, 
                                                      d.RecordOfServiceSeries, d.RecordOfServiceNumber, d.RecordOfServiceDate, d.RecordOfServiceCopy,
                                                      g.MaritalStatusKey, g.MaritalStatusName, d.ParentsContact, d.ChildCount,
                                                      d.SizeClothingID, d.SizeHatID, d.SizeShoesID, d.PersonHeight,
                                                      d.IsAbroad, d.AbroadCountryID, d.AbroadSince, d.AbroadPeriod,
                                                      d.AdministrationID,
                                                      d.ClInformationAccLevelBg, d.ClInformationAccLevelBgExpDate,
                                                      d.WorkPositionNKPDID, d.WorkCompanyID, c.PermSecondPostCode, c.PresSecondPostCode, 
                                                      d.IsSuitableForMobAppointment,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.FillReservistsRequestID) as RowNumber,
                                                      j.CityID as ContactCityID, j.DistrictID as ContactDistrictID, j.PostCode as ContactPostCode, j.AddressText as ContactAddressText                                                                                            
                                              FROM PMIS_RES.FillReservistsRequest a
                                              INNER JOIN PMIS_RES.Reservists b ON a.ReservistID = b.ReservistID
                                              INNER JOIN VS_OWNER.VS_LS c ON b.PersonID = c.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Persons d ON c.PersonID = d.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Gender e ON d.GenderID = e.GenderID
                                              LEFT OUTER JOIN VS_OWNER.KLV_DJJ f ON d.BirthCountryID = f.DJJ_KOD
                                              LEFT OUTER JOIN PMIS_ADM.MaritalStatuses g ON c.KOD_SPO = g.MaritalStatusKey
                                              LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities h ON a.MilReportSpecialityID = h.MilReportSpecialityID
                                              LEFT OUTER JOIN PMIS_ADM.PersonAddresses i ON c.PersonID = i.PersonID AND i.AddressType = 'ADR_CONTACT'
                                              LEFT OUTER JOIN PMIS_ADM.Addresses j ON i.AddressID = j.AddressID
                                              WHERE a.RequestCommandPositionID = :RequestCommandPositionID AND a.MilitaryDepartmentID = :MilitaryDepartmentID
                                              ORDER BY " + orderBySQL + @", a.FillReservistsRequestID
                                            ) tmp
                                            " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Value = requestCommandPositionID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    FillReservistsRequest fillReservistsRequest = ExtractFillReservistsRequest(dr, currentUser);
                    fillReservistsRequest.Reservist = ReservistUtil.ExtractReservist(dr, currentUser);
                    fillReservistsRequest.Reservist.Person = PersonUtil.ExtractPersonFromDR(currentUser, dr);

                    fillReservistsRequests.Add(fillReservistsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillReservistsRequests;
        }

        //Get count of all FillReservistsRequest records for RequestCommandPosition with prefilled Reservist and Reservist.Person properties
        public static int GetAllFillReservistsRequestByRequestCommandPositionCount(int requestCommandPositionID, int militaryDepartmentID, User currentUser)
        {
            int fillReservistsRequests = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {                             
                string SQL = @" SELECT COUNT(*) as Cnt
                                FROM PMIS_RES.FillReservistsRequest a
                                INNER JOIN PMIS_RES.Reservists b ON a.ReservistID = b.ReservistID
                                INNER JOIN VS_OWNER.VS_LS c ON b.PersonID = c.PersonID
                                LEFT OUTER JOIN PMIS_ADM.Persons d ON c.PersonID = d.PersonID
                                LEFT OUTER JOIN PMIS_ADM.Gender e ON d.GenderID = e.GenderID
                                LEFT OUTER JOIN VS_OWNER.KLV_DJJ f ON d.BirthCountryID = f.DJJ_KOD
                                LEFT OUTER JOIN PMIS_ADM.MaritalStatuses g ON c.KOD_SPO = g.MaritalStatusKey
                                WHERE a.RequestCommandPositionID = :RequestCommandPositionID AND a.MilitaryDepartmentID = :MilitaryDepartmentID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Value = requestCommandPositionID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        fillReservistsRequests = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillReservistsRequests;
        }


        //Save a position for a particular FillReservistsRequest
        public static bool SaveRequestCommandReservist(FillReservistsRequest fillReservistsRequest, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(fillReservistsRequest.MilitaryDepartmentID, currentUser);

            RequestCommandPosition position = RequestCommandPositionUtil.GetRequestCommandPosition(currentUser, fillReservistsRequest.RequestCommandPositionID);

            string logDescription = "";
            logDescription += "Заявка №: " + position.RequestsCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.RequestsCommand.EquipmentReservistsRequest.RequestDate) +
                              "; Команда: " + position.RequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Позиция: " + position.Position +
                              "; ВОС: " + (fillReservistsRequest.MilitaryReportSpeciality != null ? fillReservistsRequest.MilitaryReportSpeciality.MilReportingSpecialityCode : "") +
                              "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                              "; Вид резерв: " + fillReservistsRequest.ReservistReadiness +
                              "; Резервист: " + fillReservistsRequest.Reservist.Person.FullName +
                              "; ЕГН: " + fillReservistsRequest.Reservist.Person.IdentNumber +
                              "; Нуждае се от курс: " + (fillReservistsRequest.NeedCourse ? "Да" : "Не");

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            changeEvent = new ChangeEvent("RES_EquipResRequests_AddReservist", logDescription, position.RequestsCommand.EquipmentReservistsRequest.MilitaryUnit, fillReservistsRequest.Reservist.Person, currentUser);

            try
            {
                SQL = @"BEGIN
                        
                       ";

                SQL += @"INSERT INTO PMIS_RES.FillReservistsRequest (RequestCommandPositionID, MilitaryDepartmentID, ReservistID, ReservistReadinessID, MilReportSpecialityID, NeedCourse)
                            VALUES (:RequestCommandPositionID, :MilitaryDepartmentID, :ReservistID, :ReservistReadinessID, :MilReportSpecialityID, :NeedCourse);

                            SELECT PMIS_RES.FillResRequest_ID_SEQ.currval INTO :FillReservistsRequestID FROM dual;

                            ";

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramFillReservistsRequestID = new OracleParameter();
                paramFillReservistsRequestID.ParameterName = "FillReservistsRequestID";
                paramFillReservistsRequestID.OracleType = OracleType.Number;
                paramFillReservistsRequestID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramFillReservistsRequestID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RequestCommandPositionID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fillReservistsRequest.RequestCommandPositionID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fillReservistsRequest.MilitaryDepartmentID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fillReservistsRequest.ReservistID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReservistReadinessID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fillReservistsRequest.ReservistReadinessID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReportSpecialityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (fillReservistsRequest.MilitaryReportSpeciality != null)
                    param.Value = fillReservistsRequest.MilitaryReportSpeciality.MilReportSpecialityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NeedCourse";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = (fillReservistsRequest.NeedCourse ? 1 : 0);
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                fillReservistsRequest.FillReservistsRequestID = DBCommon.GetInt(paramFillReservistsRequestID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        //Delete a particualar FillReservistsRequest by requestCommandPositionID and reservistID
        public static void DeleteRequestCommandReservist(int fillReservistsRequestID, int militaryDepartmentID, User currentUser, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, currentUser);

            FillReservistsRequest request = FillReservistsRequestUtil.GetFillReservistsRequest(fillReservistsRequestID, currentUser);

            RequestCommandPosition position = RequestCommandPositionUtil.GetRequestCommandPosition(currentUser, request.RequestCommandPositionID);
                        
            string logDescription = "";
            logDescription += "Заявка №: " + position.RequestsCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.RequestsCommand.EquipmentReservistsRequest.RequestDate) +
                              "; Команда: " + position.RequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Позиция: " + position.Position +
                              "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                              "; ВОС: " + (request.MilitaryReportSpeciality != null ? request.MilitaryReportSpeciality.MilReportingSpecialityCode : "") +
                              "; Вид резерв: " + request.ReservistReadiness +
                              "; Резервист: " + request.Reservist.Person.FullName +
                              "; ЕГН: " + request.Reservist.Person.IdentNumber +
                              "; Нуждае се от курс: " + (request.NeedCourse ? "Да" : "Не");

            changeEvent = new ChangeEvent("RES_EquipResRequests_DeleteReservist", logDescription, position.RequestsCommand.EquipmentReservistsRequest.MilitaryUnit, request.Reservist.Person, currentUser);
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"DELETE FROM PMIS_RES.FillReservistsRequest
                         WHERE FillReservistsRequestID = :FillReservistsRequestID";                

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FillReservistsRequestID", OracleType.Number).Value = fillReservistsRequestID;

                cmd.ExecuteNonQuery();                
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null)
                changeEntry.AddEvent(changeEvent);
        }

        //Get all FillReservistsRequest records for Reservist with prefilled Reservist and Reservist.Person properties
        public static List<FillReservistsRequest> GetAllFillReservistsRequestByReservist(int reservistId, User currentUser)
        {
            List<FillReservistsRequest> fillReservistsRequests = new List<FillReservistsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT tmp.FillReservistsRequestID,
                                      tmp.RequestCommandPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.ReservistID,
                                      tmp.ReservistReadinessID,
                                      tmp.MilReportSpecialityID,
                                      tmp.NeedCourse,
                                      tmp.AppointmentIsDelivered,

                                      
                                      tmp.PersonID,
                                      tmp.GroupManagementSection, tmp.Section, tmp.Deliverer, tmp.PunktID, 
                                      tmp.OtherInfo,
                                      tmp.CreatedBy, tmp.CreatedDate, tmp.LastModifiedBy, tmp.LastModifiedDate,
                                      
                                      tmp.FirstName, 
                                      tmp.LastName, 
                                      tmp.IdentNumber, 
                                      tmp.PersonTypeCode,
                                      tmp.MilitaryRankID,
                                      tmp.CategoryCode,
                                      tmp.PermCityID,
                                      tmp.PermDistrictID,
                                      tmp.PermAddress,
                                      tmp.HomePhone,
                                      tmp.MilitaryUnitID,
                                      tmp.GenderID, tmp.GenderName,                                      
                                      tmp.PresCityID, tmp.PresDistrictID, tmp.PresAddress, 
                                      tmp.MobilePhone, tmp.BusinessPhone, tmp.Email, tmp.MilitaryService, tmp.MilitaryService,
                                      tmp.Initials, tmp.IDCardNumber, tmp.IDCardIssuedBy, tmp.IDCardIssueDate,
                                      tmp.BirthCountryID, tmp.BirthCountryName,
                                      tmp.BirthCityID, tmp.BirthCityIfAbroad,
                                      tmp.MilitaryTraining, 
                                      tmp.RecordOfServiceSeries, tmp.RecordOfServiceNumber, tmp.RecordOfServiceDate, tmp.RecordOfServiceCopy,
                                      tmp.MaritalStatusKey, tmp.MaritalStatusName, tmp.ParentsContact, tmp.ChildCount,
                                      tmp.SizeClothingID, tmp.SizeHatID, tmp.SizeShoesID, tmp.PersonHeight,
                                      tmp.IsAbroad, tmp.AbroadCountryID, tmp.AbroadSince, tmp.AbroadPeriod,
                                      tmp.AdministrationID,
                                      tmp.ClInformationAccLevelBg, tmp.ClInformationAccLevelBgExpDate,
                                      tmp.WorkPositionNKPDID, tmp.WorkCompanyID, tmp.PermSecondPostCode, tmp.PresSecondPostCode,
                                      tmp.RowNumber as RowNumber, tmp.IsSuitableForMobAppointment,
                                      tmp.ContactCityID, tmp.ContactDistrictID, tmp.ContactPostCode, tmp.ContactAddressText
                                      FROM (  SELECT  a.FillReservistsRequestID,
                                                      a.RequestCommandPositionID,
                                                      a.MilitaryDepartmentID,
                                                      a.ReservistID,
                                                      a.ReservistReadinessID,
                                                      a.MilReportSpecialityID,
                                                      a.NeedCourse,
                                                      a.AppointmentIsDelivered,
                                                      
                                                      b.PersonID, 
                                                      b.GroupManagementSection, b.Section, b.Deliverer, b.PunktID,
                                                      c.TXT as OtherInfo, 
                                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                                      
                                                      c.IME as FirstName, 
                                                      c.FAM as LastName, 
                                                      c.EGN as IdentNumber, 
                                                      c.KOD_KZV as PersonTypeCode,
                                                      c.KOD_KAT as CategoryCode,
                                                      c.KOD_ZVA as MilitaryRankID,
                                                      c.KOD_NMA_MJ as PermCityID,
                                                      c.PermAddrDistrictID as PermDistrictID,
                                                      c.ADRES as PermAddress,
                                                      c.TEL as HomePhone,
                                                      c.V_PODELENIE as MilitaryUnitID,
                                                      d.GenderID, e.GenderName,                                      
                                                      c.CurrAddrCityID as PresCityID, c.CurrAddrDistrictID as PresDistrictID, c.CurrAddress as PresAddress, 
                                                      d.MobilePhone, d.BusinessPhone, d.Email, d.MilitaryService,
                                                      d.Initials, d.IDCardNumber, d.IDCardIssuedBy, d.IDCardIssueDate,
                                                      f.DJJ_KOD as BirthCountryID, f.DJJ_IME as BirthCountryName,
                                                      c.KOD_NMA_MR as BirthCityID, d.BirthCityIfAbroad,
                                                      d.MilitaryTraining, 
                                                      d.RecordOfServiceSeries, d.RecordOfServiceNumber, d.RecordOfServiceDate, d.RecordOfServiceCopy,
                                                      g.MaritalStatusKey, g.MaritalStatusName, d.ParentsContact, d.ChildCount,
                                                      d.SizeClothingID, d.SizeHatID, d.SizeShoesID, d.PersonHeight,
                                                      d.IsAbroad, d.AbroadCountryID, d.AbroadSince, d.AbroadPeriod,
                                                      d.AdministrationID,
                                                      d.ClInformationAccLevelBg, d.ClInformationAccLevelBgExpDate,
                                                      d.WorkPositionNKPDID, d.WorkCompanyID, c.PermSecondPostCode, c.PresSecondPostCode,
                                                      RANK() OVER (ORDER BY a.FillReservistsRequestID) as RowNumber, d.IsSuitableForMobAppointment,
                                                      i.CityID as ContactCityID, i.DistrictID as ContactDistrictID, i.PostCode as ContactPostCode, i.AddressText as ContactAddressText                                                                                            
                                              FROM PMIS_RES.FillReservistsRequest a
                                              INNER JOIN PMIS_RES.Reservists b ON a.ReservistID = b.ReservistID
                                              INNER JOIN VS_OWNER.VS_LS c ON b.PersonID = c.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Persons d ON c.PersonID = d.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Gender e ON d.GenderID = e.GenderID
                                              LEFT OUTER JOIN VS_OWNER.KLV_DJJ f ON d.BirthCountryID = f.DJJ_KOD
                                              LEFT OUTER JOIN PMIS_ADM.MaritalStatuses g ON c.KOD_SPO = g.MaritalStatusKey
                                              LEFT OUTER JOIN PMIS_ADM.PersonAddresses h ON c.PersonID = h.PersonID AND h.AddressType = 'ADR_CONTACT'
                                              LEFT OUTER JOIN PMIS_ADM.Addresses i ON h.AddressID = i.AddressID
                                              WHERE a.ReservistID = :ReservistID
                                              ORDER BY a.FillReservistsRequestID
                                            ) tmp
                                          ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistID", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    FillReservistsRequest fillReservistsRequest = ExtractFillReservistsRequest(dr, currentUser);
                    fillReservistsRequest.Reservist = ReservistUtil.ExtractReservist(dr, currentUser);
                    fillReservistsRequest.Reservist.Person = PersonUtil.ExtractPersonFromDR(currentUser, dr);

                    fillReservistsRequests.Add(fillReservistsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillReservistsRequests;
        }

        //Get all FillReservistsRequest records for Command and Military Department with prefilled Reservist and Reservist.Person properties
        public static List<FillReservistsRequest> GetAllFillReservistsRequestByCommandAndMilDept(int requestCommandId, int militaryDepartmentId, User currentUser)
        {
            List<FillReservistsRequest> fillReservistsRequests = new List<FillReservistsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT tmp.FillReservistsRequestID,
                                      tmp.RequestCommandPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.ReservistID,
                                      tmp.ReservistReadinessID,
                                      tmp.MilReportSpecialityID,
                                      tmp.NeedCourse,
                                      tmp.AppointmentIsDelivered,
                                      
                                      tmp.PersonID,
                                      tmp.GroupManagementSection, tmp.Section, tmp.Deliverer, tmp.PunktID, 
                                      tmp.OtherInfo,
                                      tmp.CreatedBy, tmp.CreatedDate, tmp.LastModifiedBy, tmp.LastModifiedDate,
                                      
                                      tmp.FirstName, 
                                      tmp.LastName, 
                                      tmp.IdentNumber, 
                                      tmp.PersonTypeCode,
                                      tmp.MilitaryRankID,
                                      tmp.CategoryCode,
                                      tmp.PermCityID,
                                      tmp.PermDistrictID,
                                      tmp.PermAddress,
                                      tmp.HomePhone,
                                      tmp.MilitaryUnitID,
                                      tmp.GenderID, tmp.GenderName,                                      
                                      tmp.PresCityID, tmp.PresDistrictID, tmp.PresAddress, 
                                      tmp.MobilePhone, tmp.BusinessPhone, tmp.Email, tmp.MilitaryService, tmp.MilitaryService,
                                      tmp.Initials, tmp.IDCardNumber, tmp.IDCardIssuedBy, tmp.IDCardIssueDate,
                                      tmp.BirthCountryID, tmp.BirthCountryName,
                                      tmp.BirthCityID, tmp.BirthCityIfAbroad,
                                      tmp.MilitaryTraining, 
                                      tmp.RecordOfServiceSeries, tmp.RecordOfServiceNumber, tmp.RecordOfServiceDate, tmp.RecordOfServiceCopy,
                                      tmp.MaritalStatusKey, tmp.MaritalStatusName, tmp.ParentsContact, tmp.ChildCount,
                                      tmp.SizeClothingID, tmp.SizeHatID, tmp.SizeShoesID, tmp.PersonHeight,
                                      tmp.IsAbroad, tmp.AbroadCountryID, tmp.AbroadSince, tmp.AbroadPeriod,
                                      tmp.AdministrationID,
                                      tmp.ClInformationAccLevelBg, tmp.ClInformationAccLevelBgExpDate,
                                      tmp.WorkPositionNKPDID, tmp.WorkCompanyID, tmp.PermSecondPostCode, tmp.PresSecondPostCode,
                                      tmp.RowNumber as RowNumber, tmp..IsSuitableForMobAppointment,
                                      tmp.ContactCityID, tmp.ContactDistrictID, tmp.ContactPostCode, tmp.ContactAddressText
                                      FROM (  SELECT  a.FillReservistsRequestID,
                                                      a.RequestCommandPositionID,
                                                      a.MilitaryDepartmentID,
                                                      a.ReservistID,
                                                      a.ReservistReadinessID,
                                                      a.MilReportSpecialityID,
                                                      a.NeedCourse,
                                                      a.AppointmentIsDelivered,
                                                      
                                                      b.PersonID, 
                                                      b.GroupManagementSection, b.Section, b.Deliverer, b.PunktID,
                                                      c.TXT as OtherInfo, 
                                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                                      
                                                      c.IME as FirstName, 
                                                      c.FAM as LastName, 
                                                      c.EGN as IdentNumber, 
                                                      c.KOD_KZV as PersonTypeCode,
                                                      c.KOD_ZVA as MilitaryRankID,
                                                      c.KOD_KAT as CategoryCode,
                                                      c.KOD_NMA_MJ as PermCityID,
                                                      c.PermAddrDistrictID as PermDistrictID,
                                                      c.ADRES as PermAddress,
                                                      c.TEL as HomePhone,
                                                      c.V_PODELENIE as MilitaryUnitID,
                                                      d.GenderID, e.GenderName,                                      
                                                      c.CurrAddrCityID as PresCityID, c.CurrAddrDistrictID as PresDistrictID, c.CurrAddress as PresAddress, 
                                                      d.MobilePhone, d.BusinessPhone, d.Email, d.MilitaryService,
                                                      d.Initials, d.IDCardNumber, d.IDCardIssuedBy, d.IDCardIssueDate,
                                                      f.DJJ_KOD as BirthCountryID, f.DJJ_IME as BirthCountryName,
                                                      c.KOD_NMA_MR as BirthCityID, d.BirthCityIfAbroad,
                                                      d.MilitaryTraining, 
                                                      d.RecordOfServiceSeries, d.RecordOfServiceNumber, d.RecordOfServiceDate, d.RecordOfServiceCopy,
                                                      g.MaritalStatusKey, g.MaritalStatusName, d.ParentsContact, d.ChildCount,
                                                      d.SizeClothingID, d.SizeHatID, d.SizeShoesID, d.PersonHeight,
                                                      d.IsAbroad, d.AbroadCountryID, d.AbroadSince, d.AbroadPeriod,
                                                      d.AdministrationID,
                                                      d.ClInformationAccLevelBg, d.ClInformationAccLevelBgExpDate,
                                                      d.WorkPositionNKPDID, d.WorkCompanyID, c.PermSecondPostCode, c.PresSecondPostCode,
                                                      RANK() OVER (ORDER BY a.FillReservistsRequestID) as RowNumber, d.IsSuitableForMobAppointment,
                                                      i.CityID as ContactCityID, i.DistrictID as ContactDistrictID, i.PostCode as ContactPostCode, i.AddressText as ContactAddressText                                                                                            
                                              FROM PMIS_RES.FillReservistsRequest a
                                              INNER JOIN PMIS_RES.Reservists b ON a.ReservistID = b.ReservistID
                                              INNER JOIN VS_OWNER.VS_LS c ON b.PersonID = c.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Persons d ON c.PersonID = d.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Gender e ON d.GenderID = e.GenderID
                                              LEFT OUTER JOIN VS_OWNER.KLV_DJJ f ON d.BirthCountryID = f.DJJ_KOD
                                              LEFT OUTER JOIN PMIS_ADM.MaritalStatuses g ON c.KOD_SPO = g.MaritalStatusKey
                                              LEFT OUTER JOIN PMIS_ADM.PersonAddresses h ON c.PersonID = h.PersonID AND h.AddressType = 'ADR_CONTACT'
                                              LEFT OUTER JOIN PMIS_ADM.Addresses i ON h.AddressID = i.AddressID
                                              WHERE a.MilitaryDepartmentID = :MilitaryDepartmentID AND
                                                    a.RequestCommandPositionID IN (SELECT RequestCommandPositionID 
                                                                                   FROM PMIS_RES.RequestCommandPositions
                                                                                   WHERE RequestsCommandID = :RequestsCommandID)
                                              ORDER BY a.FillReservistsRequestID
                                            ) tmp
                                          ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;
                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    FillReservistsRequest fillReservistsRequest = ExtractFillReservistsRequest(dr, currentUser);
                    fillReservistsRequest.Reservist = ReservistUtil.ExtractReservist(dr, currentUser);
                    fillReservistsRequest.Reservist.Person = PersonUtil.ExtractPersonFromDR(currentUser, dr);

                    fillReservistsRequests.Add(fillReservistsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillReservistsRequests;
        }


        //Get all FillReservistsRequest records for Request Command Position with prefilled Reservist and Reservist.Person properties
        public static List<FillReservistsRequest> GetAllFillReservistsRequestByReqCommandPosition(int requestCommandPositionId, User currentUser)
        {
            List<FillReservistsRequest> fillReservistsRequests = new List<FillReservistsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT tmp.FillReservistsRequestID,
                                      tmp.RequestCommandPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.ReservistID,
                                      tmp.ReservistReadinessID,
                                      tmp.MilReportSpecialityID,
                                      tmp.NeedCourse,
                                      tmp.AppointmentIsDelivered,
                                      
                                      tmp.PersonID,
                                      tmp.GroupManagementSection, tmp.Section, tmp.Deliverer, tmp.PunktID, 
                                      tmp.OtherInfo,
                                      tmp.CreatedBy, tmp.CreatedDate, tmp.LastModifiedBy, tmp.LastModifiedDate,
                                      
                                      tmp.FirstName, 
                                      tmp.LastName, 
                                      tmp.IdentNumber, 
                                      tmp.PersonTypeCode,
                                      tmp.MilitaryRankID,
                                      tmp.CategoryCode,
                                      tmp.PermCityID,
                                      tmp.PermDistrictID,
                                      tmp.PermAddress,
                                      tmp.HomePhone,
                                      tmp.MilitaryUnitID,
                                      tmp.GenderID, tmp.GenderName,                                      
                                      tmp.PresCityID, tmp.PresDistrictID, tmp.PresAddress, 
                                      tmp.MobilePhone, tmp.BusinessPhone, tmp.Email, tmp.MilitaryService, tmp.MilitaryService,
                                      tmp.Initials, tmp.IDCardNumber, tmp.IDCardIssuedBy, tmp.IDCardIssueDate,
                                      tmp.BirthCountryID, tmp.BirthCountryName,
                                      tmp.BirthCityID, tmp.BirthCityIfAbroad,
                                      tmp.MilitaryTraining, 
                                      tmp.RecordOfServiceSeries, tmp.RecordOfServiceNumber, tmp.RecordOfServiceDate, tmp.RecordOfServiceCopy,
                                      tmp.MaritalStatusKey, tmp.MaritalStatusName, tmp.ParentsContact, tmp.ChildCount,
                                      tmp.SizeClothingID, tmp.SizeHatID, tmp.SizeShoesID, tmp.PersonHeight,
                                      tmp.IsAbroad, tmp.AbroadCountryID, tmp.AbroadSince, tmp.AbroadPeriod,
                                      tmp.AdministrationID,
                                      tmp.ClInformationAccLevelBg, tmp.ClInformationAccLevelBgExpDate,
                                      tmp.WorkPositionNKPDID, tmp.WorkCompanyID, tmp.PermSecondPostCode, tmp.PresSecondPostCode,
                                      tmp.RowNumber as RowNumber, tmp.IsSuitableForMobAppointment,
                                      tmp.ContactCityID, tmp.ContactDistrictID, tmp.ContactPostCode, tmp.ContactAddressText
                                      FROM (  SELECT  a.FillReservistsRequestID,
                                                      a.RequestCommandPositionID,
                                                      a.MilitaryDepartmentID,
                                                      a.ReservistID,
                                                      a.ReservistReadinessID,
                                                      a.MilReportSpecialityID,
                                                      a.NeedCourse,
                                                      a.AppointmentIsDelivered,
                                                      
                                                      b.PersonID, 
                                                      b.GroupManagementSection, b.Section, b.Deliverer, b.PunktID,
                                                      c.TXT as OtherInfo, 
                                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                                      
                                                      c.IME as FirstName, 
                                                      c.FAM as LastName, 
                                                      c.EGN as IdentNumber, 
                                                      c.KOD_KZV as PersonTypeCode,
                                                      c.KOD_ZVA as MilitaryRankID,
                                                      c.KOD_KAT as CategoryCode,
                                                      c.KOD_NMA_MJ as PermCityID,
                                                      c.PermAddrDistrictID as PermDistrictID,
                                                      c.ADRES as PermAddress,
                                                      c.TEL as HomePhone,
                                                      c.V_PODELENIE as MilitaryUnitID,
                                                      d.GenderID, e.GenderName,                                      
                                                      c.CurrAddrCityID as PresCityID, c.CurrAddrDistrictID as PresDistrictID, c.CurrAddress as PresAddress, 
                                                      d.MobilePhone, d.BusinessPhone, d.Email, d.MilitaryService,
                                                      d.Initials, d.IDCardNumber, d.IDCardIssuedBy, d.IDCardIssueDate,
                                                      f.DJJ_KOD as BirthCountryID, f.DJJ_IME as BirthCountryName,
                                                      c.KOD_NMA_MR as BirthCityID, d.BirthCityIfAbroad,
                                                      d.MilitaryTraining, 
                                                      d.RecordOfServiceSeries, d.RecordOfServiceNumber, d.RecordOfServiceDate, d.RecordOfServiceCopy,
                                                      g.MaritalStatusKey, g.MaritalStatusName, d.ParentsContact, d.ChildCount,
                                                      d.SizeClothingID, d.SizeHatID, d.SizeShoesID, d.PersonHeight,
                                                      d.IsAbroad, d.AbroadCountryID, d.AbroadSince, d.AbroadPeriod,
                                                      d.AdministrationID,
                                                      d.ClInformationAccLevelBg, d.ClInformationAccLevelBgExpDate,
                                                      d.WorkPositionNKPDID, d.WorkCompanyID, c.PermSecondPostCode, c.PresSecondPostCode,
                                                      RANK() OVER (ORDER BY a.FillReservistsRequestID) as RowNumber, d.IsSuitableForMobAppointment,
                                                      i.CityID as ContactCityID, i.DistrictID as ContactDistrictID, i.PostCode as ContactPostCode, i.AddressText as ContactAddressText                                                                                             
                                              FROM PMIS_RES.FillReservistsRequest a
                                              INNER JOIN PMIS_RES.Reservists b ON a.ReservistID = b.ReservistID
                                              INNER JOIN VS_OWNER.VS_LS c ON b.PersonID = c.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Persons d ON c.PersonID = d.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Gender e ON d.GenderID = e.GenderID
                                              LEFT OUTER JOIN VS_OWNER.KLV_DJJ f ON d.BirthCountryID = f.DJJ_KOD
                                              LEFT OUTER JOIN PMIS_ADM.MaritalStatuses g ON c.KOD_SPO = g.MaritalStatusKey
                                              LEFT OUTER JOIN PMIS_ADM.PersonAddresses h ON c.PersonID = h.PersonID AND h.AddressType = 'ADR_CONTACT'
                                              LEFT OUTER JOIN PMIS_ADM.Addresses i ON h.AddressID = i.AddressID
                                              WHERE a.RequestCommandPositionID = :RequestCommandPositionID
                                              ORDER BY a.FillReservistsRequestID
                                            ) tmp
                                          ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Value = requestCommandPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    FillReservistsRequest fillReservistsRequest = ExtractFillReservistsRequest(dr, currentUser);
                    fillReservistsRequest.Reservist = ReservistUtil.ExtractReservist(dr, currentUser);
                    fillReservistsRequest.Reservist.Person = PersonUtil.ExtractPersonFromDR(currentUser, dr);

                    fillReservistsRequests.Add(fillReservistsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillReservistsRequests;
        }

        //Get all FillReservistsRequest records for Request Command with prefilled Reservist and Reservist.Person properties
        public static List<FillReservistsRequest> GetAllFillReservistsRequestByRequestCommand(int requestCommandId, User currentUser)
        {
            List<FillReservistsRequest> fillReservistsRequests = new List<FillReservistsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT tmp.FillReservistsRequestID,
                                      tmp.RequestCommandPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.ReservistID,
                                      tmp.ReservistReadinessID,
                                      tmp.MilReportSpecialityID,
                                      tmp.NeedCourse,
                                      tmp.AppointmentIsDelivered,
                                      
                                      tmp.PersonID,
                                      tmp.GroupManagementSection, tmp.Section, tmp.Deliverer, tmp.PunktID, 
                                      tmp.OtherInfo,
                                      tmp.CreatedBy, tmp.CreatedDate, tmp.LastModifiedBy, tmp.LastModifiedDate,
                                      
                                      tmp.FirstName, 
                                      tmp.LastName, 
                                      tmp.IdentNumber, 
                                      tmp.PersonTypeCode,
                                      tmp.MilitaryRankID,
                                      tmp.CategoryCode,
                                      tmp.PermCityID,
                                      tmp.PermDistrictID,
                                      tmp.PermAddress,
                                      tmp.HomePhone,
                                      tmp.MilitaryUnitID,
                                      tmp.GenderID, tmp.GenderName,                                      
                                      tmp.PresCityID, tmp.PresDistrictID, tmp.PresAddress, 
                                      tmp.MobilePhone, tmp.BusinessPhone, tmp.Email, tmp.MilitaryService, tmp.MilitaryService,
                                      tmp.Initials, tmp.IDCardNumber, tmp.IDCardIssuedBy, tmp.IDCardIssueDate,
                                      tmp.BirthCountryID, tmp.BirthCountryName,
                                      tmp.BirthCityID, tmp.BirthCityIfAbroad,
                                      tmp.MilitaryTraining, 
                                      tmp.RecordOfServiceSeries, tmp.RecordOfServiceNumber, tmp.RecordOfServiceDate, tmp.RecordOfServiceCopy,
                                      tmp.MaritalStatusKey, tmp.MaritalStatusName, tmp.ParentsContact, tmp.ChildCount,
                                      tmp.SizeClothingID, tmp.SizeHatID, tmp.SizeShoesID, tmp.PersonHeight,
                                      tmp.IsAbroad, tmp.AbroadCountryID, tmp.AbroadSince, tmp.AbroadPeriod,
                                      tmp.AdministrationID,
                                      tmp.ClInformationAccLevelBg, tmp.ClInformationAccLevelBgExpDate,
                                      tmp.WorkPositionNKPDID, tmp.WorkCompanyID, tmp.PermSecondPostCode, tmp.PresSecondPostCode,
                                      tmp.RowNumber as RowNumber, tmp.IsSuitableForMobAppointment,
                                      tmp.ContactCityID, tmp.ContactDistrictID, tmp.ContactPostCode, tmp.ContactAddressText
                                      FROM (  SELECT  a.FillReservistsRequestID,
                                                      a.RequestCommandPositionID,
                                                      a.MilitaryDepartmentID,
                                                      a.ReservistID,
                                                      a.ReservistReadinessID,
                                                      a.MilReportSpecialityID,
                                                      a.NeedCourse,
                                                      a.AppointmentIsDelivered,
                                                      
                                                      b.PersonID, 
                                                      b.GroupManagementSection, b.Section, b.Deliverer, b.PunktID,
                                                      c.TXT as OtherInfo, 
                                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                                      
                                                      c.IME as FirstName, 
                                                      c.FAM as LastName, 
                                                      c.EGN as IdentNumber, 
                                                      c.KOD_KZV as PersonTypeCode,
                                                      c.KOD_KAT as CategoryCode,
                                                      c.KOD_ZVA as MilitaryRankID,
                                                      c.KOD_NMA_MJ as PermCityID,
                                                      c.PermAddrDistrictID as PermDistrictID,
                                                      c.ADRES as PermAddress,
                                                      c.TEL as HomePhone,
                                                      c.V_PODELENIE as MilitaryUnitID,
                                                      d.GenderID, e.GenderName,                                      
                                                      c.CurrAddrCityID as PresCityID, c.CurrAddrDistrictID as PresDistrictID, c.CurrAddress as PresAddress, 
                                                      d.MobilePhone, d.BusinessPhone, d.Email, d.MilitaryService,
                                                      d.Initials, d.IDCardNumber, d.IDCardIssuedBy, d.IDCardIssueDate,
                                                      f.DJJ_KOD as BirthCountryID, f.DJJ_IME as BirthCountryName,
                                                      c.KOD_NMA_MR as BirthCityID, d.BirthCityIfAbroad,
                                                      d.MilitaryTraining, 
                                                      d.RecordOfServiceSeries, d.RecordOfServiceNumber, d.RecordOfServiceDate, d.RecordOfServiceCopy,
                                                      g.MaritalStatusKey, g.MaritalStatusName, d.ParentsContact, d.ChildCount,
                                                      d.SizeClothingID, d.SizeHatID, d.SizeShoesID, d.PersonHeight,
                                                      d.IsAbroad, d.AbroadCountryID, d.AbroadSince, d.AbroadPeriod,
                                                      d.AdministrationID,
                                                      d.ClInformationAccLevelBg, d.ClInformationAccLevelBgExpDate,
                                                      d.WorkPositionNKPDID, d.WorkCompanyID, c.PermSecondPostCode, c.PresSecondPostCode,
                                                      RANK() OVER (ORDER BY a.FillReservistsRequestID) as RowNumber, d.IsSuitableForMobAppointment,
                                                      i.CityID as ContactCityID, i.DistrictID as ContactDistrictID, i.PostCode as ContactPostCode, i.AddressText as ContactAddressText                                                                                            
                                              FROM PMIS_RES.FillReservistsRequest a
                                              INNER JOIN PMIS_RES.Reservists b ON a.ReservistID = b.ReservistID
                                              INNER JOIN VS_OWNER.VS_LS c ON b.PersonID = c.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Persons d ON c.PersonID = d.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Gender e ON d.GenderID = e.GenderID
                                              LEFT OUTER JOIN VS_OWNER.KLV_DJJ f ON d.BirthCountryID = f.DJJ_KOD
                                              LEFT OUTER JOIN PMIS_ADM.MaritalStatuses g ON c.KOD_SPO = g.MaritalStatusKey
                                              LEFT OUTER JOIN PMIS_ADM.PersonAddresses h ON c.PersonID = h.PersonID AND h.AddressType = 'ADR_CONTACT'
                                              LEFT OUTER JOIN PMIS_ADM.Addresses i ON h.AddressID = i.AddressID
                                              WHERE a.RequestCommandPositionID IN (SELECT RequestCommandPositionID 
                                                                                   FROM PMIS_RES.RequestCommandPositions
                                                                                   WHERE RequestsCommandID = :RequestsCommandID)
                                              ORDER BY a.FillReservistsRequestID
                                            ) tmp
                                          ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    FillReservistsRequest fillReservistsRequest = ExtractFillReservistsRequest(dr, currentUser);
                    fillReservistsRequest.Reservist = ReservistUtil.ExtractReservist(dr, currentUser);
                    fillReservistsRequest.Reservist.Person = PersonUtil.ExtractPersonFromDR(currentUser, dr);

                    fillReservistsRequests.Add(fillReservistsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillReservistsRequests;
        }

        //Get all FillReservistsRequest records for Equipment Request with prefilled Reservist and Reservist.Person properties
        public static List<FillReservistsRequest> GetAllFillReservistsRequestByEquipmentRequest(int equipmentReservistsRequestId, User currentUser)
        {
            List<FillReservistsRequest> fillReservistsRequests = new List<FillReservistsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT tmp.FillReservistsRequestID,
                                      tmp.RequestCommandPositionID,
                                      tmp.MilitaryDepartmentID,
                                      tmp.ReservistID,
                                      tmp.ReservistReadinessID,
                                      tmp.MilReportSpecialityID,
                                      tmp.NeedCourse,
                                      tmp.AppointmentIsDelivered,
                                      
                                      tmp.PersonID,
                                      tmp.GroupManagementSection, tmp.Section, tmp.Deliverer, tmp.PunktID, 
                                      tmp.OtherInfo,
                                      tmp.CreatedBy, tmp.CreatedDate, tmp.LastModifiedBy, tmp.LastModifiedDate,
                                      
                                      tmp.FirstName, 
                                      tmp.LastName, 
                                      tmp.IdentNumber, 
                                      tmp.PersonTypeCode,
                                      tmp.MilitaryRankID,
                                      tmp.CategoryCode,
                                      tmp.PermCityID,
                                      tmp.PermDistrictID,
                                      tmp.PermAddress,
                                      tmp.HomePhone,
                                      tmp.MilitaryUnitID,
                                      tmp.GenderID, tmp.GenderName,                                      
                                      tmp.PresCityID, tmp.PresDistrictID, tmp.PresAddress, 
                                      tmp.MobilePhone, tmp.BusinessPhone, tmp.Email, tmp.MilitaryService, tmp.MilitaryService,
                                      tmp.Initials, tmp.IDCardNumber, tmp.IDCardIssuedBy, tmp.IDCardIssueDate,
                                      tmp.BirthCountryID, tmp.BirthCountryName,
                                      tmp.BirthCityID, tmp.BirthCityIfAbroad,
                                      tmp.MilitaryTraining, 
                                      tmp.RecordOfServiceSeries, tmp.RecordOfServiceNumber, tmp.RecordOfServiceDate, tmp.RecordOfServiceCopy,
                                      tmp.MaritalStatusKey, tmp.MaritalStatusName, tmp.ParentsContact, tmp.ChildCount,
                                      tmp.SizeClothingID, tmp.SizeHatID, tmp.SizeShoesID, tmp.PersonHeight,
                                      tmp.IsAbroad, tmp.AbroadCountryID, tmp.AbroadSince, tmp.AbroadPeriod,
                                      tmp.AdministrationID,
                                      tmp.ClInformationAccLevelBg, tmp.ClInformationAccLevelBgExpDate,
                                      tmp.PermSecondPostCode, tmp.PresSecondPostCode,
                                      tmp.WorkPositionNKPDID, tmp.WorkCompanyID,
                                      tmp.RowNumber as RowNumber, tmp.IsSuitableForMobAppointment,
                                      tmp.ContactCityID, tmp.ContactDistrictID, tmp.ContactPostCode, tmp.ContactAddressText
                                      FROM (  SELECT  a.FillReservistsRequestID,
                                                      a.RequestCommandPositionID,
                                                      a.MilitaryDepartmentID,
                                                      a.ReservistID,
                                                      a.ReservistReadinessID,
                                                      a.MilReportSpecialityID,
                                                      a.NeedCourse,
                                                      a.AppointmentIsDelivered,
                                                      
                                                      b.PersonID, 
                                                      b.GroupManagementSection, b.Section, b.Deliverer, b.PunktID,
                                                      c.TXT as OtherInfo, 
                                                      b.CreatedBy, b.CreatedDate, b.LastModifiedBy, b.LastModifiedDate,
                                                      
                                                      c.IME as FirstName, 
                                                      c.FAM as LastName, 
                                                      c.EGN as IdentNumber, 
                                                      c.KOD_KZV as PersonTypeCode,
                                                      c.KOD_ZVA as MilitaryRankID,
                                                      c.KOD_KAT as CategoryCode,
                                                      c.KOD_NMA_MJ as PermCityID,
                                                      c.PermAddrDistrictID as PermDistrictID,
                                                      c.ADRES as PermAddress,
                                                      c.TEL as HomePhone,
                                                      c.V_PODELENIE as MilitaryUnitID,
                                                      d.GenderID, e.GenderName,                                      
                                                      c.CurrAddrCityID as PresCityID, c.CurrAddrDistrictID as PresDistrictID, c.CurrAddress as PresAddress, 
                                                      d.MobilePhone, d.BusinessPhone, d.Email, d.MilitaryService,
                                                      d.Initials, d.IDCardNumber, d.IDCardIssuedBy, d.IDCardIssueDate,
                                                      f.DJJ_KOD as BirthCountryID, f.DJJ_IME as BirthCountryName,
                                                      c.KOD_NMA_MR as BirthCityID, d.BirthCityIfAbroad,
                                                      d.MilitaryTraining, 
                                                      d.RecordOfServiceSeries, d.RecordOfServiceNumber, d.RecordOfServiceDate, d.RecordOfServiceCopy,
                                                      g.MaritalStatusKey, g.MaritalStatusName, d.ParentsContact, d.ChildCount,
                                                      d.SizeClothingID, d.SizeHatID, d.SizeShoesID, d.PersonHeight,
                                                      d.IsAbroad, d.AbroadCountryID, d.AbroadSince, d.AbroadPeriod,
                                                      d.AdministrationID,
                                                      d.ClInformationAccLevelBg, d.ClInformationAccLevelBgExpDate,
                                                      d.WorkPositionNKPDID, d.WorkCompanyID, c.PermSecondPostCode, c.PresSecondPostCode,
                                                      RANK() OVER (ORDER BY a.FillReservistsRequestID) as RowNumber, d.IsSuitableForMobAppointment,
                                                      i.CityID as ContactCityID, i.DistrictID as ContactDistrictID, i.PostCode as ContactPostCode, i.AddressText as ContactAddressText                                                                                             
                                              FROM PMIS_RES.FillReservistsRequest a
                                              INNER JOIN PMIS_RES.Reservists b ON a.ReservistID = b.ReservistID
                                              INNER JOIN VS_OWNER.VS_LS c ON b.PersonID = c.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Persons d ON c.PersonID = d.PersonID
                                              LEFT OUTER JOIN PMIS_ADM.Gender e ON d.GenderID = e.GenderID
                                              LEFT OUTER JOIN VS_OWNER.KLV_DJJ f ON d.BirthCountryID = f.DJJ_KOD
                                              LEFT OUTER JOIN PMIS_ADM.MaritalStatuses g ON c.KOD_SPO = g.MaritalStatusKey
                                              LEFT OUTER JOIN PMIS_ADM.PersonAddresses h ON c.PersonID = h.PersonID AND h.AddressType = 'ADR_CONTACT'
                                              LEFT OUTER JOIN PMIS_ADM.Addresses i ON h.AddressID = i.AddressID
                                              WHERE a.RequestCommandPositionID IN (SELECT RequestCommandPositionID 
                                                                                   FROM PMIS_RES.RequestCommandPositions
                                                                                   WHERE RequestsCommandID IN (SELECT RequestsCommandID 
                                                                                                               FROM PMIS_RES.RequestsCommands
                                                                                                               WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID)
                                                                                  )
                                              ORDER BY a.FillReservistsRequestID
                                            ) tmp
                                          ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentReservistsRequestID", OracleType.Number).Value = equipmentReservistsRequestId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    FillReservistsRequest fillReservistsRequest = ExtractFillReservistsRequest(dr, currentUser);
                    fillReservistsRequest.Reservist = ReservistUtil.ExtractReservist(dr, currentUser);
                    fillReservistsRequest.Reservist.Person = PersonUtil.ExtractPersonFromDR(currentUser, dr);

                    fillReservistsRequests.Add(fillReservistsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillReservistsRequests;
        }
    }
}