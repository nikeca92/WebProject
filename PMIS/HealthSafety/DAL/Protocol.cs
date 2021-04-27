using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Protocol : BaseDbObject
    {
        private int protocolId;
        private string protocolNumber;
        private DateTime? protocolDate;
        private int protocolTypeId;
        private ProtocolType protocolType;        
        private int militaryUnitId;
        private MilitaryUnit militaryUnit;        
        private string obekt;
        private string requesting;
        private DateTime? measurementDate;
        private string normativeDocument;
        private string measurementMethod;
        private string address;
        private string usedEquipments;
        private string peoplePresent;
        private List<ProtocolItem> protocolItems;        

        public int ProtocolId
        {
            get { return protocolId; }
            set { protocolId = value; }
        }

        public string ProtocolNumber
        {
            get { return protocolNumber; }
            set { protocolNumber = value; }
        }

        public DateTime? ProtocolDate
        {
            get { return protocolDate; }
            set { protocolDate = value; }
        }

        public int ProtocolTypeId
        {
            get { return protocolTypeId; }
            set { protocolTypeId = value; }
        }

        public ProtocolType ProtocolType
        {
            get
            {
                if (protocolType == null)
                {
                    protocolType = ProtocolTypeUtil.GetProtocolType(protocolTypeId, CurrentUser);
                }
                return protocolType; 
            }
            set 
            { 
                protocolType = value; 
            }
        }

        public int MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get 
            {
                if (militaryUnit == null)
                {
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(militaryUnitId, CurrentUser);
                }
                return militaryUnit; 
            }
            set 
            { 
                militaryUnit = value; 
            }
        }

        public string Obekt
        {
            get { return obekt; }
            set { obekt = value; }
        }

        public string Requesting
        {
            get { return requesting; }
            set { requesting = value; }
        }

        public DateTime? MeasurementDate
        {
            get { return measurementDate; }
            set { measurementDate = value; }
        }

        public string NormativeDocument
        {
            get { return normativeDocument; }
            set { normativeDocument = value; }
        }

        public string MeasurementMethod
        {
            get { return measurementMethod; }
            set { measurementMethod = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string UsedEquipments
        {
            get { return usedEquipments; }
            set { usedEquipments = value; }
        }

        public string PeoplePresent
        {
            get { return peoplePresent; }
            set { peoplePresent = value; }
        }

        public List<ProtocolItem> ProtocolItems
        {
            get 
            {
                if (protocolItems == null)
                {
                    protocolItems = ProtocolItemUtil.GetProtocolItemsByProtocol(ProtocolId, CurrentUser);
                }
                return protocolItems; 
            }
            set 
            {
                protocolItems = value; 
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public Protocol(User user)
            :base(user)
        {

        }
    }

    public static class ProtocolUtil
    {
        public static Protocol GetProtocol(int protocolId, User currentUser)
        {
            Protocol protocol = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_PROTOCOLS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.ProtocolID, a.ProtocolNumber, a.ProtocolDate, a.ProtocolTypeID, a.MilitaryUnitID,
                                      a.Object, a.Requesting, a.MeasurementDate, a.NormativeDocument, a.MeasurementMethod,
                                      a.Address, a.UsedEquipments, a.PeoplePresent,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_HS.Protocols a
                               WHERE a.ProtocolID = :ProtocolID " + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ProtocolID", OracleType.Number).Value = protocolId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    protocol = new Protocol(currentUser);
                    protocol.ProtocolId = protocolId;
                    protocol.ProtocolNumber = dr["ProtocolNumber"].ToString();
                    if (dr["ProtocolDate"] is DateTime)
                        protocol.ProtocolDate = (DateTime)dr["ProtocolDate"];
                    else
                        protocol.ProtocolDate = null;
                    protocol.ProtocolTypeId = DBCommon.GetInt(dr["ProtocolTypeID"]);
                    protocol.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    protocol.Obekt = dr["Object"].ToString();
                    protocol.Requesting = dr["Requesting"].ToString();
                    if (dr["MeasurementDate"] is DateTime)
                        protocol.MeasurementDate = (DateTime)dr["MeasurementDate"];
                    else
                        protocol.MeasurementDate = null;
                    protocol.NormativeDocument = dr["NormativeDocument"].ToString();
                    protocol.MeasurementMethod = dr["MeasurementMethod"].ToString();
                    protocol.Address = dr["Address"].ToString();
                    protocol.UsedEquipments = dr["UsedEquipments"].ToString();
                    protocol.PeoplePresent = dr["PeoplePresent"].ToString();

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, protocol);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return protocol;
        }

        public static List<Protocol> GetAllProtocols(string protocolNum, string protocolTypeIds, DateTime? dateFrom, DateTime? dateTo, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<Protocol> protocols = new List<Protocol>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_PROTOCOLS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString(); 
                }

                if (!String.IsNullOrEmpty(protocolNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ProtocolNumber LIKE '%" + protocolNum.Replace("'", "''") + @"%' ";
                }

                if (!String.IsNullOrEmpty(protocolTypeIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ProtocolTypeId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(protocolTypeIds) + ") ";
                }

                if (dateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ProtocolDate >= " + DBCommon.DateToDBCode(dateFrom.Value) + " ";
                }

                if (dateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ProtocolDate < " + DBCommon.DateToDBCode(dateTo.Value.AddDays(1)) + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }

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
                        orderBySQL = "a.ProtocolNumber";
                        break;
                    case 2:
                        orderBySQL = "a.ProtocolDate";
                        break;
                    case 3:
                        orderBySQL = "b.ProtocolTypeName";
                        break;
                    case 4:
                        orderBySQL = "c.IMEES";
                        break;
                    default:
                        orderBySQL = "a.ProtocolNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.ProtocolID as ProtocolID, tmp.RowNumber as RowNumber  FROM (
                                  SELECT a.ProtocolID as ProtocolID,                                         
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.ProtocolID) as RowNumber 
                                  FROM PMIS_HS.Protocols a                 
                                  LEFT OUTER JOIN PMIS_HS.ProtocolTypes b ON a.ProtocolTypeID = b.ProtocolTypeID      
                                  LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR           
                                  " + where + @"    
                                  ORDER BY " + orderBySQL +@", ProtocolID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["ProtocolID"]))
                        protocols.Add(ProtocolUtil.GetProtocol(DBCommon.GetInt(dr["ProtocolID"]), currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return protocols;
        }

        public static int GetAllProtocolsCount(string protocolNum, string protocolTypeIds, DateTime? dateFrom, DateTime? dateTo, User currentUser)
        {
            int protocolsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_PROTOCOLS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(protocolNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ProtocolNumber LIKE '%" + protocolNum.Replace("'", "''") + @"%' ";
                }

                if (!String.IsNullOrEmpty(protocolTypeIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ProtocolTypeId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(protocolTypeIds) + ") ";
                }

                if (dateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ProtocolDate >= " + DBCommon.DateToDBCode(dateFrom.Value) + " ";
                }

                if (dateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.ProtocolDate < " + DBCommon.DateToDBCode(dateTo.Value.AddDays(1)) + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_HS.Protocols a
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        protocolsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return protocolsCnt;
        }

        public static bool SaveProtocol(Protocol protocol, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (protocol.ProtocolId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.Protocols (ProtocolNumber, ProtocolDate, ProtocolTypeID, MilitaryUnitID, Object, Requesting, 
                                                            MeasurementDate, NormativeDocument, MeasurementMethod, Address, UsedEquipments, PeoplePresent,
                                                            CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:ProtocolNumber, :ProtocolDate, :ProtocolTypeID, :MilitaryUnitID, :Obekt, :Requesting, 
                                    :MeasurementDate, :NormativeDocument, :MeasurementMethod, :Address, :UsedEquipments, :PeoplePresent,
                                    :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_HS.Protocols_ID_SEQ.currval INTO :ProtocolID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Prot_AddProtocol", "", protocol.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtNumber", "", protocol.ProtocolNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtDate", "", CommonFunctions.FormatDate(protocol.ProtocolDate.Value), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtType", "", protocol.ProtocolType != null ? protocol.ProtocolType.ProtocolTypeName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MilitaryUnit", "", protocol.MilitaryUnit != null ? protocol.MilitaryUnit.DisplayTextForSelection : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Object", "", protocol.Obekt, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Requesting", "", protocol.Requesting, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MeasurementDate", "", CommonFunctions.FormatDate(protocol.MeasurementDate.Value), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_NormativeDocument", "", protocol.NormativeDocument, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MeasurementMethod", "", protocol.MeasurementMethod, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Address", "", protocol.Address, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_UsedEquipments", "", protocol.UsedEquipments, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_PeoplePresent", "", protocol.PeoplePresent, currentUser));

                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.Protocols SET
                               ProtocolNumber = :ProtocolNumber, 
                               ProtocolDate = :ProtocolDate, 
                               ProtocolTypeID = :ProtocolTypeID, 
                               MilitaryUnitID = :MilitaryUnitID, 
                               Object = :Obekt, 
                               Requesting = :Requesting, 
                               MeasurementDate = :MeasurementDate, 
                               NormativeDocument = :NormativeDocument, 
                               MeasurementMethod = :MeasurementMethod, 
                               Address = :Address, 
                               UsedEquipments = :UsedEquipments, 
                               PeoplePresent = :PeoplePresent,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END
                            WHERE ProtocolID = :ProtocolID ;                       

                            ";

                    changeEvent = new ChangeEvent("HS_Prot_EditProtocol", "", protocol.MilitaryUnit, null, currentUser);

                    Protocol oldProtocol = ProtocolUtil.GetProtocol(protocol.ProtocolId, currentUser);

                    if (oldProtocol.ProtocolNumber.Trim() != protocol.ProtocolNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtNumber", oldProtocol.ProtocolNumber, protocol.ProtocolNumber, currentUser));

                    if (!CommonFunctions.IsEqual(oldProtocol.ProtocolDate, protocol.ProtocolDate))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtDate", CommonFunctions.FormatDate(oldProtocol.ProtocolDate.Value), CommonFunctions.FormatDate(protocol.ProtocolDate.Value), currentUser));

                    if ((oldProtocol.ProtocolType != null ? oldProtocol.ProtocolType.ProtocolTypeName : "") != (protocol.ProtocolType != null ? protocol.ProtocolType.ProtocolTypeName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtType", oldProtocol.ProtocolType != null ? oldProtocol.ProtocolType.ProtocolTypeName : "", protocol.ProtocolType != null ? protocol.ProtocolType.ProtocolTypeName : "", currentUser));

                    if ((oldProtocol.MilitaryUnit != null ? oldProtocol.MilitaryUnit.DisplayTextForSelection : "") != (protocol.MilitaryUnit != null ? protocol.MilitaryUnit.DisplayTextForSelection : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MilitaryUnit", oldProtocol.MilitaryUnit != null ? oldProtocol.MilitaryUnit.DisplayTextForSelection : "", protocol.MilitaryUnit != null ? protocol.MilitaryUnit.DisplayTextForSelection : "", currentUser));

                    if (oldProtocol.Obekt.Trim() != protocol.Obekt.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Object", oldProtocol.Obekt, protocol.Obekt, currentUser));

                    if (oldProtocol.Requesting.Trim() != protocol.Requesting.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Requesting", oldProtocol.Requesting, protocol.Requesting, currentUser));

                    if (!CommonFunctions.IsEqual(oldProtocol.MeasurementDate, protocol.MeasurementDate))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MeasurementDate", CommonFunctions.FormatDate(oldProtocol.MeasurementDate.Value), CommonFunctions.FormatDate(protocol.MeasurementDate.Value), currentUser));

                    if (oldProtocol.NormativeDocument.Trim() != protocol.NormativeDocument.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_NormativeDocument", oldProtocol.NormativeDocument, protocol.NormativeDocument, currentUser));

                    if (oldProtocol.MeasurementMethod.Trim() != protocol.MeasurementMethod.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MeasurementMethod", oldProtocol.MeasurementMethod, protocol.MeasurementMethod, currentUser));

                    if (oldProtocol.Address.Trim() != protocol.Address.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Address", oldProtocol.Address, protocol.Address, currentUser));

                    if (oldProtocol.UsedEquipments.Trim() != protocol.UsedEquipments.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_UsedEquipments", oldProtocol.UsedEquipments, protocol.UsedEquipments, currentUser));

                    if (oldProtocol.PeoplePresent.Trim() != protocol.PeoplePresent.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_PeoplePresent", oldProtocol.PeoplePresent, protocol.PeoplePresent, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramProtocolID = new OracleParameter();
                paramProtocolID.ParameterName = "ProtocolID";
                paramProtocolID.OracleType = OracleType.Number;

                if (protocol.ProtocolId != 0)
                {
                    paramProtocolID.Direction = ParameterDirection.Input;
                    paramProtocolID.Value = protocol.ProtocolId;
                }
                else
                {
                    paramProtocolID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramProtocolID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ProtocolNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocol.ProtocolNumber))
                    param.Value = protocol.ProtocolNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ProtocolDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (protocol.ProtocolDate.HasValue)
                    param.Value = protocol.ProtocolDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ProtocolTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (protocol.ProtocolTypeId != -1)
                    param.Value = protocol.ProtocolTypeId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (protocol.MilitaryUnitId != -1)
                    param.Value = protocol.MilitaryUnitId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Obekt";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocol.Obekt))
                    param.Value = protocol.Obekt;
                else
                    param.Value = DBNull.Value;               
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Requesting";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocol.Requesting))
                    param.Value = protocol.Requesting;
                else
                    param.Value = DBNull.Value; 
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MeasurementDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (protocol.MeasurementDate.HasValue)
                    param.Value = protocol.MeasurementDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NormativeDocument";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocol.NormativeDocument))
                    param.Value = protocol.NormativeDocument;
                else
                    param.Value = DBNull.Value; 
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MeasurementMethod";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocol.MeasurementMethod))
                    param.Value = protocol.MeasurementMethod;
                else
                    param.Value = DBNull.Value; 
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Address";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocol.Address))
                    param.Value = protocol.Address;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "UsedEquipments";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocol.UsedEquipments))
                    param.Value = protocol.UsedEquipments;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PeoplePresent";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocol.PeoplePresent))
                    param.Value = protocol.PeoplePresent;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (protocol.ProtocolId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (protocol.ProtocolId == 0)
                    protocol.ProtocolId = DBCommon.GetInt(paramProtocolID.Value);
                
                result = true;                
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool DeleteProtocol(int protocolId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Protocol oldProtocol = ProtocolUtil.GetProtocol(protocolId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Prot_DeleteProtocol", "", oldProtocol.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtNumber", oldProtocol.ProtocolNumber, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtDate", CommonFunctions.FormatDate(oldProtocol.ProtocolDate.Value), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_ProtType", oldProtocol.ProtocolType != null ? oldProtocol.ProtocolType.ProtocolTypeName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MilitaryUnit", oldProtocol.MilitaryUnit != null ? oldProtocol.MilitaryUnit.DisplayTextForSelection : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Object", oldProtocol.Obekt, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Requesting", oldProtocol.Requesting, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MeasurementDate", CommonFunctions.FormatDate(oldProtocol.MeasurementDate.Value), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_NormativeDocument", oldProtocol.NormativeDocument, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_MeasurementMethod", oldProtocol.MeasurementMethod, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_Address", oldProtocol.Address, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_UsedEquipments", oldProtocol.UsedEquipments, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Prot_PeoplePresent", oldProtocol.PeoplePresent, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                DELETE FROM PMIS_HS.ProtocolItems WHERE ProtocolID = :ProtocolID;
                                
                                DELETE FROM PMIS_HS.Protocols WHERE ProtocolID = :ProtocolID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ProtocolID", OracleType.Number).Value = protocolId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }        

        public static void SetProtocolLastModified(int protocolId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_HS.Protocols SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE ProtocolID = :ProtocolID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ProtocolID", OracleType.Number).Value = protocolId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}