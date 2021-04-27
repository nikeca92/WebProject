using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class ProtocolItem : BaseDbObject
    {
        private int protocolItemID;
        private WorkingPlace workingPlace;
        private int workingPeople;
        private int measureID;
        private Measure measure;        
        private decimal measured;
        private decimal threshold;
        private string other;       

        public int ProtocolItemID
        {
            get { return protocolItemID; }
            set { protocolItemID = value; }
        }

        public WorkingPlace WorkingPlace
        {
            get { return workingPlace; }
            set { workingPlace = value; }
        }

        public int WorkingPeople
        {
            get { return workingPeople; }
            set { workingPeople = value; }
        }

        public int MeasureID
        {
            get { return measureID; }
            set { measureID = value; }
        }

        public Measure Measure
        {
            get 
            {
                if (measure == null)
                {
                    measure = MeasureUtil.GetMeasure(MeasureID, CurrentUser);
                }
                return measure; 
            }
            set 
            {
                measure = value; 
            }
        }

        public decimal Measured
        {
            get { return measured; }
            set { measured = value; }
        }

        public decimal Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        public string Other
        {
            get { return other; }
            set { other = value; }
        }

        public ProtocolItem(User user)
            : base(user)
        {
        }
    }

    public static class ProtocolItemUtil
    {
        public static ProtocolItem GetProtocolItem(int protocolItemId, User currentUser)
        {
            ProtocolItem protocolItem = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ProtocolItemID, b.WorkingPlaceID, b.WorkingPlace,
                                      a.WorkingPeople, a.MeasureID, 
	                                  a.Measured, a.Threshold, a.Other 
                               FROM PMIS_HS.ProtocolItems a
                               INNER JOIN PMIS_HS.WorkingPlaces b ON a.WorkingPlaceID = b.WorkingPlaceID
                               WHERE a.ProtocolItemID = :ProtocolItemID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ProtocolItemID", OracleType.Number).Value = protocolItemId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    protocolItem = new ProtocolItem(currentUser);
                    protocolItem.ProtocolItemID = protocolItemId;
                    protocolItem.WorkingPlace = new WorkingPlace(currentUser);
                    protocolItem.WorkingPlace.WorkingPlaceId = DBCommon.GetInt(dr["WorkingPlaceID"]);
                    protocolItem.WorkingPlace.WorkingPlaceName = dr["WorkingPlace"].ToString();
                    protocolItem.WorkingPeople = DBCommon.GetInt(dr["WorkingPeople"]);
                    protocolItem.MeasureID = DBCommon.GetInt(dr["MeasureID"]);
                    protocolItem.Measured = (dr["Measured"] is decimal ? (decimal)dr["Measured"] : 0);
                    protocolItem.Threshold = (dr["Threshold"] is decimal ? (decimal)dr["Threshold"] : 0);
                    protocolItem.Other = dr["Other"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return protocolItem;
        }

        public static List<ProtocolItem> GetProtocolItemsByProtocol(int protocolId, User currentUser)
        {
            List<ProtocolItem> protocolItems = new List<ProtocolItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ProtocolItemID
                               FROM PMIS_HS.ProtocolItems a
                               WHERE a.ProtocolID = :ProtocolID
                               ORDER BY a.ProtocolItemID ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ProtocolID", OracleType.Number).Value = protocolId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["ProtocolItemID"]))
                        protocolItems.Add(ProtocolItemUtil.GetProtocolItem(DBCommon.GetInt(dr["ProtocolItemID"]), currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return protocolItems;
        }

        public static bool SaveProtocolItem(int protocolId, ProtocolItem protocolItem, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            Protocol protocol = ProtocolUtil.GetProtocol(protocolId, currentUser);

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                string logDescription = "";
                logDescription += "Номер на протокол: " + protocol.ProtocolNumber;

                if (protocolItem.ProtocolItemID == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.ProtocolItems (ProtocolID, WorkingPlaceID, WorkingPeople, MeasureID, Measured, Threshold, Other)
                            VALUES (:ProtocolID, :WorkingPlaceID, :WorkingPeople, :MeasureID, :Measured, :Threshold, :Other);

                            SELECT PMIS_HS.ProtocolItems_ID_SEQ.currval INTO :ProtocolItemID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Prot_AddItem", logDescription, protocol.MilitaryUnit, null, currentUser);                    

                    changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_WorkingPlace", "", protocolItem.WorkingPlace.WorkingPlaceName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_WorkingPeople", "", protocolItem.WorkingPeople.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Measure", "", protocolItem.Measure != null ? protocolItem.Measure.MeasureName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Measured", "", CommonFunctions.FormatDecimal(protocolItem.Measured), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Threshold", "", CommonFunctions.FormatDecimal(protocolItem.Threshold), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Other", "", protocolItem.Other, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.ProtocolItems SET
                               ProtocolID = :ProtocolID, 
                               WorkingPlaceID = :WorkingPlaceID, 
                               WorkingPeople = :WorkingPeople, 
                               MeasureID = :MeasureID, 
                               Measured = :Measured, 
                               Threshold = :Threshold, 
                               Other = :Other
                            WHERE ProtocolItemID = :ProtocolItemID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_Prot_EditItem", logDescription, protocol.MilitaryUnit, null, currentUser);

                    ProtocolItem oldProtocolItem = ProtocolItemUtil.GetProtocolItem(protocolItem.ProtocolItemID, currentUser);

                    if (oldProtocolItem.WorkingPlace.WorkingPlaceName.Trim() != protocolItem.WorkingPlace.WorkingPlaceName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_WorkingPlace", oldProtocolItem.WorkingPlace.WorkingPlaceName, protocolItem.WorkingPlace.WorkingPlaceName, currentUser));

                    if (oldProtocolItem.WorkingPeople != protocolItem.WorkingPeople)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_WorkingPeople", oldProtocolItem.WorkingPeople.ToString(), protocolItem.WorkingPeople.ToString(), currentUser));

                    if ((oldProtocolItem.Measure != null ? oldProtocolItem.Measure.MeasureName : "") != (protocolItem.Measure != null ? protocolItem.Measure.MeasureName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Measure", oldProtocolItem.Measure != null ? oldProtocolItem.Measure.MeasureName : "", protocolItem.Measure != null ? protocolItem.Measure.MeasureName : "", currentUser));

                    if (oldProtocolItem.Measured != protocolItem.Measured)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Measured", CommonFunctions.FormatDecimal(oldProtocolItem.Measured), CommonFunctions.FormatDecimal(protocolItem.Measured), currentUser));

                    if (oldProtocolItem.Threshold != protocolItem.Threshold)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Threshold", CommonFunctions.FormatDecimal(oldProtocolItem.Threshold), CommonFunctions.FormatDecimal(protocolItem.Threshold), currentUser));

                    if (oldProtocolItem.Other.Trim() != protocolItem.Other.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Other", oldProtocolItem.Other, protocolItem.Other, currentUser));

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramProtocolItemID = new OracleParameter();
                paramProtocolItemID.ParameterName = "ProtocolItemID";
                paramProtocolItemID.OracleType = OracleType.Number;

                if (protocolItem.ProtocolItemID != 0)
                {
                    paramProtocolItemID.Direction = ParameterDirection.Input;
                    paramProtocolItemID.Value = protocolItem.ProtocolItemID;
                }
                else
                {
                    paramProtocolItemID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramProtocolItemID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ProtocolID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;                
                param.Value = protocolId;                
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "WorkingPlaceID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = protocolItem.WorkingPlace.WorkingPlaceId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "WorkingPeople";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;                
                param.Value = protocolItem.WorkingPeople;                
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MeasureID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (protocolItem.MeasureID != -1)
                    param.Value = protocolItem.MeasureID;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Measured";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = protocolItem.Measured;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Threshold";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = protocolItem.Threshold;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Other";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(protocolItem.Other))
                    param.Value = protocolItem.Other;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);               

                cmd.ExecuteNonQuery();

                if (protocolItem.ProtocolItemID == 0)
                {
                    protocolItem.ProtocolItemID = DBCommon.GetInt(paramProtocolItemID.Value);
                }

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                    ProtocolUtil.SetProtocolLastModified(protocolId, currentUser);
                }
            }

            return result;
        }

        public static bool DeleteProtocolItem(int protocolId, int protocolItemId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Protocol protocol = ProtocolUtil.GetProtocol(protocolId, currentUser);

            string logDescription = "";
            logDescription += "Номер на протокол: " + protocol.ProtocolNumber;

            ProtocolItem oldProtocolItem = ProtocolItemUtil.GetProtocolItem(protocolItemId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Prot_DeleteItem", logDescription, protocol.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_WorkingPlace", oldProtocolItem.WorkingPlace.WorkingPlaceName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_WorkingPeople", oldProtocolItem.WorkingPeople.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Measure", oldProtocolItem.Measure != null ? oldProtocolItem.Measure.MeasureName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Measured", CommonFunctions.FormatDecimal(oldProtocolItem.Measured), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Threshold", CommonFunctions.FormatDecimal(oldProtocolItem.Threshold), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_ProtItem_Other", oldProtocolItem.Other, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_HS.ProtocolItems WHERE ProtocolItemID = :ProtocolItemID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ProtocolItemID", OracleType.Number).Value = protocolItemId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);

                ProtocolUtil.SetProtocolLastModified(protocolId, currentUser);
            }

            return result;
        }
    }

}