using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class VesselCrew : BaseDbObject
    {
        private int vesselCrewID;
        private string identNumber;
        private string fullName;
        private string address;
        private string militaryRankID;
        private MilitaryRank militaryRank;
        private int? vesselCrewCategoryID;
        private VesselCrewCategory vesselCrewCategory;
        private bool? hasAppointment;

        public int VesselCrewID
        {
            get { return vesselCrewID; }
            set { vesselCrewID = value; }
        }

        public string IdentNumber
        {
            get { return identNumber; }
            set { identNumber = value; }
        }
        
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string MilitaryRankID
        {
            get { return militaryRankID; }
            set { militaryRankID = value; }
        }

        public MilitaryRank MilitaryRank
        {
            get 
            {
                if (militaryRank == null && !String.IsNullOrEmpty(militaryRankID))
                    militaryRank = MilitaryRankUtil.GetMilitaryRank(militaryRankID, CurrentUser);

                return militaryRank; 
            }
            set { militaryRank = value; }
        }

        public int? VesselCrewCategoryID
        {
            get { return vesselCrewCategoryID; }
            set { vesselCrewCategoryID = value; }
        }

        public VesselCrewCategory VesselCrewCategory
        {
            get 
            {
                if (vesselCrewCategory == null && vesselCrewCategoryID.HasValue)
                    vesselCrewCategory = VesselCrewCategoryUtil.GetVesselCrewCategory(CurrentUser, vesselCrewCategoryID.Value);
 
                return vesselCrewCategory; 
            }
            set { vesselCrewCategory = value; }
        }

        public bool? HasAppointment
        {
            get { return hasAppointment; }
            set { hasAppointment = value; }
        }

        public VesselCrew(User user)
            :base(user)
        {

        }
    }

    public static class VesselCrewUtil
    {
        //This method creates and returns a VesselCrew object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific ID, for example.
        public static VesselCrew ExtractVesselCrewFromDataReader(OracleDataReader dr, User currentUser)
        {
            VesselCrew vesselCrew = new VesselCrew(currentUser);

            vesselCrew.VesselCrewID = DBCommon.GetInt(dr["VesselCrewID"]);
            vesselCrew.IdentNumber = dr["IdentNumber"].ToString();
            vesselCrew.FullName = dr["FullName"].ToString();
            vesselCrew.Address = dr["Address"].ToString();
            vesselCrew.MilitaryRankID = dr["MilitaryRankID"].ToString();
            vesselCrew.VesselCrewCategoryID = DBCommon.IsInt(dr["VesselCrewCategoryID"]) ? (int?)DBCommon.GetInt(dr["VesselCrewCategoryID"]) : null;
            vesselCrew.HasAppointment = DBCommon.IsInt(dr["HasAppointment"]) ? (bool?)(DBCommon.GetInt(dr["HasAppointment"]) == 1) : null;           

            return vesselCrew;
        }

        public static VesselCrew GetVesselCrew(int vesselCrewID, User currentUser)
        {
            VesselCrew vesselCrew = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {               
                string SQL = @" SELECT a.VesselCrewID,
                                       a.IdentNumber,
                                       a.FullName,
                                       a.Address,
                                       a.MilitaryRankID,
                                       a.VesselCrewCategoryID,
                                       a.HasAppointment                                
                                FROM PMIS_RES.VesselCrew a
                                WHERE a.VesselCrewID = :VesselCrewID
                                ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VesselCrewID", OracleType.Number).Value = vesselCrewID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vesselCrew = ExtractVesselCrewFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselCrew;
        }

        public static List<VesselCrew> GetAllVesselCrewByVesselID(int vesselID, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<VesselCrew> vesselCrews = new List<VesselCrew>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                where = " a.VesselID =  " + vesselID.ToString();

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

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
                        orderBySQL = "b.VesselCrewCategoryName";
                        break;
                    case 2:
                        orderBySQL = "a.IdentNumber";
                        break;
                    case 3:
                        orderBySQL = "c.ZVA_IMEES";
                        break;      
                    case 4:
                        orderBySQL = "a.FullName";
                        break;
                    case 5:
                        orderBySQL = "a.HasAppointment";
                        break;
                    case 6:
                        orderBySQL = "a.Address";
                        break;      
                    default:
                        orderBySQL = "b.VesselCrewCategoryName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @" SELECT * FROM 
                                (
                                    SELECT a.VesselCrewID,
                                           a.IdentNumber,
                                           a.FullName,
                                           a.Address,
                                           a.MilitaryRankID,
                                           a.VesselCrewCategoryID,
                                           a.HasAppointment,
                                           RANK() OVER (ORDER BY " + orderBySQL + @", a.VesselCrewID) as RowNumber
                                    FROM PMIS_RES.VesselCrew a
                                    LEFT OUTER JOIN PMIS_ADM.VesselCrewCategories b ON a.VesselCrewCategoryID = b.VesselCrewCategoryID
                                    LEFT OUTER JOIN VS_OWNER.KLV_ZVA c ON a.MilitaryRankID = c.ZVA_KOD
                                    " + where + @"                                                                                                  
                                    ORDER BY " + orderBySQL + @", a.VesselCrewID
                               ) tmp 
                                " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vesselCrews.Add(ExtractVesselCrewFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselCrews;
        }

        public static int GetAllVesselCrewByVesselIDCount(int vesselID, User currentUser)
        {
            int vesselCrews = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                where = " a.VesselID =  " + vesselID.ToString();

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT COUNT(*) as Cnt 
                                FROM PMIS_RES.VesselCrew a                                    
                                " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vesselCrews = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vesselCrews;
        }

        //Save a position for a particular request command
        public static bool SaveVesselCrew(int vesselId, VesselCrew vesselCrew, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            Vessel vessel = VesselUtil.GetVessel(vesselId, currentUser);

            string logDescription = "";
            logDescription += "Име: " + vessel.VesselName;
            logDescription += "; Инвентарен номер: " + vessel.InventoryNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (vesselCrew.VesselCrewID == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.VesselCrew (VesselID, IdentNumber, FullName, Address, MilitaryRankID, VesselCrewCategoryID, HasAppointment)
                            VALUES (:VesselID, :IdentNumber, :FullName, :Address, :MilitaryRankID, :VesselCrewCategoryID, :HasAppointment);  

                            SELECT PMIS_RES.VesselCrew_ID_SEQ.currval INTO :VesselCrewID FROM dual;                         
                            ";

                    changeEvent = new ChangeEvent("RES_Technics_VESSELS_AddVesselCrew", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_IdentNumber", "", vesselCrew.IdentNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_FullName", "", vesselCrew.FullName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_Address", "", vesselCrew.Address, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_MilitaryRank", "", vesselCrew.MilitaryRank != null && vesselCrew.MilitaryRank.ShortName != null ? vesselCrew.MilitaryRank.ShortName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_VesselCrewCategory", "", vesselCrew.VesselCrewCategory != null ? vesselCrew.VesselCrewCategory.VesselCrewCategoryName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_HasAppointment", "", vesselCrew.HasAppointment != null ? (vesselCrew.HasAppointment.Value ? "Да" : "Не") : "", currentUser));                                        
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.VesselCrew SET
                               VesselID = :VesselID,
                               IdentNumber = :IdentNumber,
                               FullName = :FullName,
                               Address = :Address,
                               MilitaryRankID = :MilitaryRankID,
                               VesselCrewCategoryID = :VesselCrewCategoryID,
                               HasAppointment = :HasAppointment
                             WHERE VesselCrewID = :VesselCrewID;

                            ";

                    changeEvent = new ChangeEvent("RES_Technics_VESSELS_EditVesselCrew", logDescription, null, null, currentUser);

                    VesselCrew oldVesselCrew = VesselCrewUtil.GetVesselCrew(vesselCrew.VesselCrewID, currentUser);

                    if (oldVesselCrew.IdentNumber.Trim() != vesselCrew.IdentNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_IdentNumber", oldVesselCrew.IdentNumber, vesselCrew.IdentNumber, currentUser));

                    if (oldVesselCrew.FullName.Trim() != vesselCrew.FullName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_FullName", oldVesselCrew.FullName, vesselCrew.FullName, currentUser));

                    if (oldVesselCrew.Address.Trim() != vesselCrew.Address.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_Address", oldVesselCrew.Address, vesselCrew.Address, currentUser));

                    if ((oldVesselCrew.MilitaryRank != null && oldVesselCrew.MilitaryRank.ShortName != null ? oldVesselCrew.MilitaryRank.ShortName : "") !=
                        (vesselCrew.MilitaryRank != null && vesselCrew.MilitaryRank.ShortName != null ? vesselCrew.MilitaryRank.ShortName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_MilitaryRank",
                            oldVesselCrew.MilitaryRank != null && oldVesselCrew.MilitaryRank.ShortName != null ? oldVesselCrew.MilitaryRank.ShortName : "",
                            vesselCrew.MilitaryRank.ShortName != null && vesselCrew.MilitaryRank.ShortName != null ? vesselCrew.MilitaryRank.ShortName : "",
                            currentUser));

                    if ((oldVesselCrew.VesselCrewCategory != null ? oldVesselCrew.VesselCrewCategory.VesselCrewCategoryName : "") != (vesselCrew.VesselCrewCategory != null ? vesselCrew.VesselCrewCategory.VesselCrewCategoryName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_VesselCrewCategory", oldVesselCrew.VesselCrewCategory != null ? oldVesselCrew.VesselCrewCategory.VesselCrewCategoryName : "", vesselCrew.VesselCrewCategory.VesselCrewCategoryName != null ? vesselCrew.VesselCrewCategory.VesselCrewCategoryName : "", currentUser));

                    if (oldVesselCrew.HasAppointment != vesselCrew.HasAppointment)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_HasAppointment", oldVesselCrew.HasAppointment != null ? (oldVesselCrew.HasAppointment.Value ? "Да" : "Не") : "", vesselCrew.HasAppointment != null ? (vesselCrew.HasAppointment.Value ? "Да" : "Не") : "", currentUser));
                }                         

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramVesselCrewID = new OracleParameter();
                paramVesselCrewID.ParameterName = "VesselCrewID";
                paramVesselCrewID.OracleType = OracleType.Number;

                if (vesselCrew.VesselCrewID != 0)
                {
                    paramVesselCrewID.Direction = ParameterDirection.Input;
                    paramVesselCrewID.Value = vesselCrew.VesselCrewID;
                }
                else
                {
                    paramVesselCrewID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramVesselCrewID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "VesselID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vesselId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IdentNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(vesselCrew.IdentNumber))
                    param.Value = vesselCrew.IdentNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FullName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(vesselCrew.FullName))
                    param.Value = vesselCrew.FullName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Address";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(vesselCrew.Address))
                    param.Value = vesselCrew.Address;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryRankID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(vesselCrew.MilitaryRankID))
                    param.Value = vesselCrew.MilitaryRankID;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VesselCrewCategoryID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vesselCrew.VesselCrewCategoryID.HasValue)
                    param.Value = vesselCrew.VesselCrewCategoryID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);                             
          
                param = new OracleParameter();
                param.ParameterName = "HasAppointment";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vesselCrew.HasAppointment.Value ? 1 : 0;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (vesselCrew.VesselCrewID == 0)
                    vesselCrew.VesselCrewID = DBCommon.GetInt(paramVesselCrewID.Value);

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    TechnicsUtil.SetTechnicsModified(vessel.TechnicsId, currentUser);

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

        //Delete a particualr position from a particular command
        public static void DeleteVesselCrew(int vesselId, int vesselCrewId, User currentUser, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            Vessel vessel = VesselUtil.GetVessel(vesselId, currentUser);
            VesselCrew vesselCrew = VesselCrewUtil.GetVesselCrew(vesselCrewId, currentUser);

            string logDescription = "";
            logDescription += "Име: " + vessel.VesselName;
            logDescription += "; Инвентарен номер: " + vessel.InventoryNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN                            
                            DELETE FROM PMIS_RES.VesselCrew 
                            WHERE VesselCrewID = :VesselCrewID;
                         END;";

                changeEvent = new ChangeEvent("RES_Technics_VESSELS_DeleteVesselCrew", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_IdentNumber", vesselCrew.IdentNumber, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_FullName", vesselCrew.FullName, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_Address", vesselCrew.Address, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_MilitaryRank", vesselCrew.MilitaryRank != null && vesselCrew.MilitaryRank.ShortName != null ? vesselCrew.MilitaryRank.ShortName : "", "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_VesselCrewCategory", vesselCrew.VesselCrewCategory != null ? vesselCrew.VesselCrewCategory.VesselCrewCategoryName : "", "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Technics_VESSELS_VesselCrew_HasAppointment", vesselCrew.HasAppointment != null ? (vesselCrew.HasAppointment.Value ? "Да" : "Не") : "", "", currentUser));                                        

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VesselCrewID", OracleType.Number).Value = vesselCrewId;

                cmd.ExecuteNonQuery();

                TechnicsUtil.SetTechnicsModified(vessel.TechnicsId, currentUser);
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }
    }
}