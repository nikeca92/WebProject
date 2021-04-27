using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class WCondIndicatorType : BaseDbObject
    {
        private int indicatorTypeId;
        private string indicatorTypeName;
        private int seq;        
        private List<WCondIndicator> indicators;

        public int IndicatorTypeId
        {
            get { return indicatorTypeId; }
            set { indicatorTypeId = value; }
        }

        public string IndicatorTypeName
        {
            get { return indicatorTypeName; }
            set { indicatorTypeName = value; }
        }

        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }

        public List<WCondIndicator> Indicators
        {
            get
            {
                if (indicators == null)
                    indicators = WCondIndicatorUtil.GetAllIndicatorsByType(indicatorTypeId, CurrentUser);

                return indicators;
            }
            set
            {
                indicators = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (Indicators.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public WCondIndicatorType(int indicatorTypeId, string indicatorTypeName, int seq, User user)
            :base(user)
        {
            this.indicatorTypeId = indicatorTypeId;
            this.indicatorTypeName = indicatorTypeName;
            this.seq = seq;
        }

        public WCondIndicatorType(User user)
            :base(user)
        {
        }  
    }

    public static class WCondIndicatorTypeUtil
    {
        public static WCondIndicatorType GetIndicatorType(int indicatorTypeId, User currentUser)
        {
            WCondIndicatorType indicatorType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.IndicatorTypeID as IndicatorTypeID, a.IndicatorTypeName as IndicatorTypeName, a.Seq as Seq
                               FROM PMIS_HS.WConditionsIndicatorTypes a                       
                               WHERE a.IndicatorTypeID = :IndicatorTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IndicatorTypeID", OracleType.Number).Value = indicatorTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    indicatorType = new WCondIndicatorType(currentUser);
                    indicatorType.IndicatorTypeId = indicatorTypeId;
                    indicatorType.IndicatorTypeName = dr["IndicatorTypeName"].ToString();
                    indicatorType.Seq = DBCommon.GetInt(dr["Seq"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return indicatorType;
        }

        public static List<WCondIndicatorType> GetAllIndicatorTypes(User currentUser)
        {
            List<WCondIndicatorType> indicatorTypes = new List<WCondIndicatorType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.IndicatorTypeID as IndicatorTypeID, a.IndicatorTypeName as IndicatorTypeName, a.Seq as Seq
                               FROM PMIS_HS.WConditionsIndicatorTypes a
                               ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["IndicatorTypeID"]))
                        indicatorTypes.Add(new WCondIndicatorType(DBCommon.GetInt(dr["IndicatorTypeID"]), dr["IndicatorTypeName"].ToString(), DBCommon.GetInt(dr["Seq"]), currentUser));                  
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return indicatorTypes;
        }

        public static bool SaveIndicatorType(WCondIndicatorType indicatorType, User currentUser, Change changeEntry)
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
                if (indicatorType.IndicatorTypeId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.WConditionsIndicatorTypes (IndicatorTypeName, Seq)
                            VALUES (:IndicatorTypeName, :Seq);

                            SELECT PMIS_HS.WCondIndicatorTypes_ID_SEQ.currval INTO :IndicatorTypeID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_IndicatorTypes_AddIndicatorType", "", null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_IndicatorTypes_IndicatorTypeName", "", indicatorType.IndicatorTypeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_IndicatorTypes_Seq", "", indicatorType.Seq.ToString(), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.WConditionsIndicatorTypes SET
                               IndicatorTypeName = :IndicatorTypeName,
                               Seq = :Seq
                            WHERE IndicatorTypeID = :IndicatorTypeID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_IndicatorTypes_EditIndicatorType", "", null, null, currentUser);

                    WCondIndicatorType oldIndicatorType = WCondIndicatorTypeUtil.GetIndicatorType(indicatorType.IndicatorTypeId, currentUser);

                    if (oldIndicatorType.IndicatorTypeName.Trim() != indicatorType.IndicatorTypeName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_IndicatorTypes_IndicatorTypeName", oldIndicatorType.IndicatorTypeName, indicatorType.IndicatorTypeName, currentUser));

                    if (oldIndicatorType.Seq != indicatorType.Seq)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_IndicatorTypes_Seq", oldIndicatorType.Seq.ToString(), indicatorType.Seq.ToString(), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramIndicatorTypeID = new OracleParameter();
                paramIndicatorTypeID.ParameterName = "IndicatorTypeID";
                paramIndicatorTypeID.OracleType = OracleType.Number;

                if (indicatorType.IndicatorTypeId != 0)
                {
                    paramIndicatorTypeID.Direction = ParameterDirection.Input;
                    paramIndicatorTypeID.Value = indicatorType.IndicatorTypeId;
                }
                else
                {
                    paramIndicatorTypeID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramIndicatorTypeID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "IndicatorTypeName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(indicatorType.IndicatorTypeName))
                    param.Value = indicatorType.IndicatorTypeName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Seq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = indicatorType.Seq;                
                cmd.Parameters.Add(param);              

                cmd.ExecuteNonQuery();

                if (indicatorType.IndicatorTypeId == 0)
                {
                    indicatorType.IndicatorTypeId = DBCommon.GetInt(paramIndicatorTypeID.Value);
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
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool DeleteIndicatorType(int indicatorTypeId, User currentUser, Change changeEntry)
        {
            bool result = false;

            WCondIndicatorType oldIndicatorType = WCondIndicatorTypeUtil.GetIndicatorType(indicatorTypeId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Lists_IndicatorTypes_DeleteIndicatorType", "", null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_IndicatorTypes_IndicatorTypeName", oldIndicatorType.IndicatorTypeName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_IndicatorTypes_Seq", oldIndicatorType.Seq.ToString(), "", currentUser));
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                DELETE FROM PMIS_HS.WConditionsIndicators WHERE IndicatorTypeID = :IndicatorTypeID;
                                
                                DELETE FROM PMIS_HS.WConditionsIndicatorTypes WHERE IndicatorTypeID = :IndicatorTypeID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IndicatorTypeID", OracleType.Number).Value = indicatorTypeId;

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
    }

}