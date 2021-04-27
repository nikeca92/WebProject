using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;
using System.Collections;

using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class PrintTechnicsMKBlock
    {
        public string Command { get; set; }
        public string CommandNumber { get; set; }
        public string CommandNumberPrintSymbol { get; set; }
        public string CommandNumberPrintSymbol2 { get; set; }
        public string CommandName { get; set; }
        public string CommandSuffix { get; set; }
        public string AppointmentTime { get; set; }
        public string DeliveryPlace { get; set; }
        public string MilitaryDepartment { get; set; }
        public string TechnicsType { get; set; }
        public string NormativeTechnics { get; set; }
        public string RegNumber { get; set; }
        public string MakeModel { get; set; }
        public string FirstRegistrationDate { get; set; }
        public string CarryingCapacity { get; set; }
        public string EngineType { get; set; }
        public string Load { get; set; }
        public string OtherInfo { get; set; }
        public string ResidenceAddress { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string DriverIdentNumber { get; set; }
        public string DriverIdentNumberEncrypt { get; set; }
        public string DriverMilitaryRank { get; set; }
        public string DriverMilRepSpeciality { get; set; }
        public string DriverAddress { get; set; }
        public string Owner { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
    }

    public static class PrintTechnicsMKUtil
    {
        public static PrintTechnicsMKBlock GetPrintTechnicsMKBlock(int technicsId, User currentUser)
        {
            PrintTechnicsMKBlock block = new PrintTechnicsMKBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"
                               SELECT c.MilitaryCommandName as Command,
                                      u.NK as CommandNumber,
                                      u.IMEES as CommandName,
                                      f.militarycommandsuffix AS CommandSuffix,
                                      c.AppointmentTime,
                                      RTRIM(CASE WHEN i.Ime_Obl IS NOT NULL THEN 'обл. ' || i.Ime_Obl || ', ' ELSE '' END || CASE WHEN h.Ime_Obs IS NOT NULL THEN 'общ. ' || h.Ime_Obs || ', ' ELSE '' END || CASE WHEN g.Ime_Nma IS NOT NULL THEN g1.Ime_S || ' ' || g.Ime_Nma || ', ' ELSE '' END || CASE WHEN f.DeliveryPlace IS NOT NULL THEN f.DeliveryPlace || ', ' ELSE '' END, ', ') as DeliveryPlace,
                                      k.MilitaryDepartmentName,
                                      t.TechnicsTypeName,
                                      t2.NormativeCode, t2.NormativeName,
                                      CASE t.TechnicsTypeKey
                                           WHEN 'VEHICLES' THEN a1.RegNumber
                                           WHEN 'TRAILERS' THEN a2.RegNumber
                                           WHEN 'TRACTORS' THEN a3.RegNumber
                                           WHEN 'ENG_EQUIP' THEN a4.RegNumber
                                           WHEN 'MOB_LIFT_EQUIP' THEN a5.RegNumber
                                           WHEN 'RAILWAY_EQUIP' THEN a6.InventoryNumber
                                           WHEN 'AVIATION_EQUIP' THEN a7.AirInvNumber
                                           WHEN 'VESSELS' THEN a8.InventoryNumber
                                           WHEN 'FUEL_CONTAINERS' THEN a9.InventoryNumber
                                           ELSE ''
                                      END as RegNumber,
                                      CASE t.TechnicsTypeKey
                                           WHEN 'VEHICLES' THEN a1.VehicleMakeName
                                           WHEN 'TRAILERS' THEN ''
                                           WHEN 'TRACTORS' THEN a3.TractorMakeName
                                           WHEN 'ENG_EQUIP' THEN a4.EngEquipBaseMakeName
                                           WHEN 'MOB_LIFT_EQUIP' THEN ''
                                           WHEN 'RAILWAY_EQUIP' THEN ''
                                           WHEN 'AVIATION_EQUIP' THEN a7.OtherBaseMachineMakeName
                                           WHEN 'VESSELS' THEN ''
                                           WHEN 'FUEL_CONTAINERS' THEN ''
                                           ELSE ''
                                      END as MakeName,
                                      CASE t.TechnicsTypeKey
                                           WHEN 'VEHICLES' THEN a1.VehicleModelName
                                           WHEN 'TRAILERS' THEN ''
                                           WHEN 'TRACTORS' THEN a3.TractorModelName
                                           WHEN 'ENG_EQUIP' THEN a4.EngEquipBaseModelName
                                           WHEN 'MOB_LIFT_EQUIP' THEN ''
                                           WHEN 'RAILWAY_EQUIP' THEN ''
                                           WHEN 'AVIATION_EQUIP' THEN a7.OtherBaseMachineModelName
                                           WHEN 'VESSELS' THEN ''
                                           WHEN 'FUEL_CONTAINERS' THEN ''
                                           ELSE ''
                                      END as ModelName,
                                      CASE t.TechnicsTypeKey
                                           WHEN 'VEHICLES' THEN a1.FirstRegistrationDate
                                           WHEN 'TRAILERS' THEN a2.FirstRegistrationDate
                                           WHEN 'TRACTORS' THEN a3.FirstRegistrationDate
                                           WHEN 'ENG_EQUIP' THEN a4.BaseFirstRegDate
                                           WHEN 'MOB_LIFT_EQUIP' THEN NULL
                                           WHEN 'RAILWAY_EQUIP' THEN NULL
                                           WHEN 'AVIATION_EQUIP' THEN NULL
                                           WHEN 'VESSELS' THEN NULL
                                           WHEN 'FUEL_CONTAINERS' THEN NULL
                                           ELSE NULL
                                      END as FirstRegistrationDate,
                                      CASE t.TechnicsTypeKey
                                           WHEN 'VEHICLES' THEN a1.CarryingCapacity
                                           WHEN 'TRAILERS' THEN a2.CarryingCapacity
                                           WHEN 'TRACTORS' THEN NULL
                                           WHEN 'ENG_EQUIP' THEN NULL
                                           WHEN 'MOB_LIFT_EQUIP' THEN NULL
                                           WHEN 'RAILWAY_EQUIP' THEN NULL
                                           WHEN 'AVIATION_EQUIP' THEN NULL
                                           WHEN 'VESSELS' THEN NULL
                                           WHEN 'FUEL_CONTAINERS' THEN NULL
                                           ELSE NULL
                                      END as CarryingCapacity,
                                      CASE t.TechnicsTypeKey
                                           WHEN 'VEHICLES' THEN a1_e.TableValue
                                           WHEN 'TRAILERS' THEN NULL
                                           WHEN 'TRACTORS' THEN NULL
                                           WHEN 'ENG_EQUIP' THEN a4_e.TableValue
                                           WHEN 'MOB_LIFT_EQUIP' THEN NULL
                                           WHEN 'RAILWAY_EQUIP' THEN NULL
                                           WHEN 'AVIATION_EQUIP' THEN NULL
                                           WHEN 'VESSELS' THEN NULL
                                           WHEN 'FUEL_CONTAINERS' THEN NULL
                                           ELSE NULL
                                      END as EngineType,
                                      CASE t.TechnicsTypeKey
                                           WHEN 'VEHICLES' THEN NULL
                                           WHEN 'TRAILERS' THEN NULL
                                           WHEN 'TRACTORS' THEN a3.Power
                                           WHEN 'ENG_EQUIP' THEN a4.WorkingBodyPerformancePerHour
                                           WHEN 'MOB_LIFT_EQUIP' THEN a5.LoadingCapacity
                                           WHEN 'RAILWAY_EQUIP' THEN NULL
                                           WHEN 'AVIATION_EQUIP' THEN NULL
                                           WHEN 'VESSELS' THEN NULL
                                           WHEN 'FUEL_CONTAINERS' THEN NULL
                                           ELSE NULL
                                      END as Load,
                                      a.OtherInfo,
                                      RTRIM(CASE WHEN q.Ime_Obl IS NOT NULL THEN 'обл. ' || q.Ime_Obl || ', ' ELSE '' END || CASE WHEN p.Ime_Obs IS NOT NULL THEN 'общ. ' || p.Ime_Obs || ', ' ELSE '' END || CASE WHEN o.Ime_Nma IS NOT NULL THEN o1.Ime_S || ' ' || o.Ime_Nma || ', ' ELSE '' END || CASE WHEN a.ResidenceAddress IS NOT NULL THEN a.ResidenceAddress || ', ' ELSE '' END, ', ') as ResidenceAddress,
                                      o.PK as ResidencePostCode,
                                      a.ResidencePostCode as ResidenceSecondPostCode,
                                      s.EGN as DriverIdentNumber,
                                      SUBSTR(s.EGN, 0, 6) || '****' as DriverIdentNumberEncrypt,
                                      s.IME as DriverFirstName,
                                      s.FAM as DriverLastName,
                                      RTRIM(CASE WHEN da3.Ime_Obl IS NOT NULL THEN 'обл. ' || da3.Ime_Obl || ', ' ELSE '' END || CASE WHEN da2.Ime_Obs IS NOT NULL THEN 'общ. ' || da2.Ime_Obs || ', ' ELSE '' END || CASE WHEN da1.Ime_Nma IS NOT NULL THEN da12.Ime_S || ' ' || da1.Ime_Nma || ', ' ELSE '' END || CASE WHEN s.Adres IS NOT NULL THEN s.Adres || ', ' ELSE '' END, ', ') as DriverAddress,
                                      da1.PK as DriverPostCode,
                                      s.PermSecondPostCode as DriverSecondPostCode,
                                      w.ZVA_IME as DriverMilitaryRankName,
                                      v.MilReportingSpecialityCode as DriverMilRepSpeciality,
                                      x.CompanyName as OwnerCompanyName,
                                      x.UnifiedIdentityCode as OwnerUIC,
                                      RTRIM(CASE WHEN oa3.Ime_Obl IS NOT NULL THEN 'обл. ' || oa3.Ime_Obl || ', ' ELSE '' END || CASE WHEN oa2.Ime_Obs IS NOT NULL THEN 'общ. ' || oa2.Ime_Obs || ', ' ELSE '' END || CASE WHEN oa1.Ime_Nma IS NOT NULL THEN oa12.Ime_S || ' ' || oa1.Ime_Nma || ', ' ELSE '' END || CASE WHEN x.Address IS NOT NULL THEN x.Address || ', ' ELSE '' END, ', ') as OwnershipAddress,
                                      oa1.PK as OwnershipPostCode,
                                      x.PostCode as OwnershipSecondPostCode,
                                      x.Phone as OwnershipPhone,
                                      mr.PrintSymbol as CommandNumberPrintSymbol,
                                      CASE WHEN d.TechnicReadinessID = 2 
                                           THEN 1 
                                           ELSE 0 
                                      END as HasCommandNumPrintSymbol2
                               FROM PMIS_RES.Technics a
                               INNER JOIN PMIS_RES.TechnicsAppointments c ON c.TechnicsID = a.TechnicsID AND c.IsCurrent = 1
                               INNER JOIN PMIS_RES.FulfilTechnicsRequest d ON d.TechnicsID = a.TechnicsID
                               INNER JOIN PMIS_RES.TechnicsRequestCmdPositions e ON e.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID
                               INNER JOIN PMIS_RES.TechnicsRequestCommands f ON f.TechRequestsCommandID = e.TechRequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.MilReadiness mr ON mr.MilReadinessID =  f.MilReadinessID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA g ON g.Kod_Nma = f.DeliveryCityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM g1 ON g1.KOD_VNM = g.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS h ON h.kod_obs = g.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL i ON i.Kod_Obl = g.kod_obl
                               INNER JOIN PMIS_RES.TechnicsMilRepStatus j ON j.TechnicsID = a.TechnicsID AND j.IsCurrent = 1
                               INNER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = j.SourceMilDepartmentID
                               INNER JOIN PMIS_RES.TechnicsTypes t ON a.TechnicsTypeID = t.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics t2 ON a.NormativeTechnicsID = t2.NormativeTechnicsID
                               LEFT OUTER JOIN PMIS_RES.Vehicles a1 ON a.TechnicsID = a1.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Trailers a2 ON a.TechnicsID = a2.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Tractors a3 ON a.TechnicsID = a3.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.EngEquipment a4 ON a.TechnicsID = a4.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.MobileLiftingEquip a5 ON a.TechnicsID = a5.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.RailWayEquips a6 ON a.TechnicsID = a6.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.AviationEquipment a7 ON a.TechnicsID = a7.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Vessels a8 ON a.TechnicsID = a8.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.FuelContainers a9 ON a.TechnicsID = a9.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.GTable a1_e ON a1_e.TableName = 'VehicleEngineType' AND a1_e.TableKey = a1.VehicleEngineTypeID
                               LEFT OUTER JOIN PMIS_RES.GTable a4_e ON a4_e.TableName = 'EngEquipBaseEngineType' AND a4_e.TableKey = a4.EngEquipBaseEngineTypeID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA o ON o.Kod_Nma = a.ResidenceCityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM o1 ON o1.KOD_VNM = o.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS p ON p.kod_obs = o.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL q ON q.Kod_Obl = o.kod_obl
                               LEFT OUTER JOIN PMIS_RES.Reservists r ON r.ReservistID = a.DriverReservistID
                               LEFT OUTER JOIN VS_OWNER.VS_LS s ON s.PersonID = r.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Persons da ON da.PersonID = r.PersonID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA da1 ON da1.Kod_Nma = s.KOD_NMA_MJ
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM da12 ON da12.KOD_VNM = da1.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS da2 ON da2.kod_obs = da1.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL da3 ON da3.Kod_Obl = da1.kod_obl
                               LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec u ON u.PersonID = r.PersonID AND u.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities v ON v.MilReportSpecialityID = u.MilReportSpecialityID
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA w ON w.ZVA_KOD = s.KOD_ZVA
                               LEFT OUTER JOIN PMIS_ADM.Companies x ON x.CompanyID = a.OwnershipCompanyID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA oa1 ON oa1.Kod_Nma = x.CityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM oa12 ON oa12.KOD_VNM = oa1.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS oa2 ON oa2.kod_obs = oa1.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL oa3 ON oa3.Kod_Obl = oa1.kod_obl
                               LEFT OUTER JOIN UKAZ_OWNER.VVR u ON u.KOD_VVR = f.MILITARYCOMMANDID
                               WHERE a.TechnicsID = " + technicsId.ToString();

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.Command = dr["Command"].ToString();
                    block.CommandNumber = dr["CommandNumber"].ToString();
                    block.CommandNumberPrintSymbol = dr["CommandNumberPrintSymbol"].ToString();

                    if (DBCommon.GetInt(dr["HasCommandNumPrintSymbol2"]) == 1)
                    {
                        block.CommandNumberPrintSymbol2 = Config.GetWebSetting("PrintReservistsCommandNumPrintSymbol2");
                    }

                    block.CommandName = dr["CommandName"].ToString();
                    block.CommandSuffix = dr["CommandSuffix"] is string ? dr["CommandSuffix"].ToString() : "";
                    block.AppointmentTime = (DBCommon.IsDecimal(dr["AppointmentTime"]) ? DBCommon.GetDecimal(dr["AppointmentTime"]).ToString() : "");
                    block.DeliveryPlace = dr["DeliveryPlace"].ToString();
                    block.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
                    block.TechnicsType = dr["TechnicsTypeName"].ToString();
                    
                    string normativeCode = dr["NormativeCode"].ToString();
                    string normativeName = dr["NormativeName"].ToString();

                    if (!String.IsNullOrEmpty(normativeCode) || !String.IsNullOrEmpty(normativeName))
                        block.NormativeTechnics = normativeCode + " " + normativeName;

                    block.RegNumber = dr["RegNumber"].ToString();

                    string makeName = dr["MakeName"].ToString();
                    string modelName = dr["ModelName"].ToString();

                    if (!String.IsNullOrEmpty(makeName) && !String.IsNullOrEmpty(modelName))
                        makeName += ", ";

                    block.MakeModel = makeName + modelName;

                    if (dr["FirstRegistrationDate"] is DateTime)
                        block.FirstRegistrationDate = CommonFunctions.FormatDate((DateTime)dr["FirstRegistrationDate"]);

                    if (DBCommon.IsDecimal(dr["CarryingCapacity"]))
                        block.CarryingCapacity = ((decimal)dr["CarryingCapacity"]).ToString();

                    block.EngineType = dr["EngineType"].ToString();

                    if (DBCommon.IsDecimal(dr["Load"]))
                        block.Load = ((decimal)dr["Load"]).ToString();

                    block.OtherInfo = CommonFunctions.ReplaceNewLinesInString(dr["OtherInfo"].ToString());

                    block.ResidenceAddress = dr["ResidenceAddress"].ToString();

                    string residencePostCode = dr["ResidencePostCode"].ToString();
                    string residenceSecondPostCode = dr["ResidenceSecondPostCode"].ToString();

                    if (!String.IsNullOrEmpty(residenceSecondPostCode))
                    {
                        block.ResidenceAddress += (String.IsNullOrEmpty(block.ResidenceAddress) ? "" : ", ПК ") + residenceSecondPostCode;
                    }
                    else if (!String.IsNullOrEmpty(residencePostCode) && residencePostCode != "0")
                    {
                        block.ResidenceAddress += (String.IsNullOrEmpty(block.ResidenceAddress) ? "" : ", ПК ") + residencePostCode;
                    }

                    block.DriverIdentNumber = dr["DriverIdentNumber"].ToString();

                    if (dr["DriverIdentNumberEncrypt"].ToString() != "****")
                        block.DriverIdentNumberEncrypt = dr["DriverIdentNumberEncrypt"].ToString();

                    block.DriverFirstName = dr["DriverFirstName"].ToString();
                    block.DriverLastName = dr["DriverLastName"].ToString();
                    block.DriverMilitaryRank = dr["DriverMilitaryRankName"].ToString();
                    block.DriverMilRepSpeciality = dr["DriverMilRepSpeciality"].ToString();

                    block.DriverAddress = dr["DriverAddress"].ToString();

                    string driverPostCode = dr["DriverPostCode"].ToString();
                    string driverSecondPostCode = dr["DriverSecondPostCode"].ToString();

                    if (!String.IsNullOrEmpty(driverSecondPostCode))
                    {
                        block.DriverAddress += (String.IsNullOrEmpty(block.DriverAddress) ? "" : ", ПК ") + driverSecondPostCode;
                    }
                    else if (!String.IsNullOrEmpty(driverPostCode) && driverPostCode != "0")
                    {
                        block.DriverAddress += (String.IsNullOrEmpty(block.DriverAddress) ? "" : ", ПК ") + driverPostCode;
                    }

                    string ownerCompanyname = dr["OwnerCompanyName"].ToString();
                    string ownerUIC = dr["OwnerUIC"].ToString();

                    if (!String.IsNullOrEmpty(ownerCompanyname) || !String.IsNullOrEmpty(ownerUIC))
                    {
                        block.Owner = ownerCompanyname;

                        if (!String.IsNullOrEmpty(ownerUIC))
                            block.Owner += (String.IsNullOrEmpty(block.Owner) ? "" : ", ") + ownerUIC;
                    }

                    block.OwnerAddress = dr["OwnershipAddress"].ToString();

                    string ownershipPostCode = dr["OwnershipPostCode"].ToString();
                    string ownershipSecondPostCode = dr["OwnershipSecondPostCode"].ToString();

                    if (!String.IsNullOrEmpty(ownershipSecondPostCode))
                    {
                        block.OwnerAddress += (String.IsNullOrEmpty(block.OwnerAddress) ? "" : ", ПК ") + ownershipSecondPostCode;
                    }
                    else if (!String.IsNullOrEmpty(ownershipPostCode) && ownershipPostCode != "0")
                    {
                        block.OwnerAddress += (String.IsNullOrEmpty(block.OwnerAddress) ? "" : ", ПК ") + ownershipPostCode;
                    }

                    block.OwnerPhone = dr["OwnershipPhone"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return block;
        }
    }
}