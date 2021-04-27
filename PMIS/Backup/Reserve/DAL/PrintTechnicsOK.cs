using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Common;
using System.Data.OracleClient;

namespace PMIS.Reserve.DAL
{
    public class PrintTechnicsOKBlock
    {
        public string CommandNumber { get; set; }
        public string CommandNumberPrintSymbol { get; set; }
        public string CommandNumberPrintSymbol2 { get; set; }
        public string CommandSuffix { get; set; }
        public string TechnicsType { get; set; }
        public string RegNumber { get; set; }
        public string MakeModel { get; set; }
        public string FirstRegistrationDate { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string DriverIdentNumber { get; set; }
        public string DriverIdentNumberEncrypt { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerUIC { get; set; }
        public string Type { get; set; }
        public string EngineType { get; set; }
        public string Power { get; set; }
        public string Seats { get; set; }
        public string Roadability { get; set; }
        public string CarryingCapacity { get; set; }
        public string Load { get; set; }
        public string MilRepStatus { get; set; }
        public string OtherEquip { get; set; }
        public string StopDate { get; set; }
        public string ContractNumber { get; set; }
        public string MilitaryUnit { get; set; }
        public string ContractVPN { get; set; }
        public string ContractDuration { get; set; }
        public string AppointmentTime { get; set; }
        public string DriverVOSNumber { get; set; }
        public string DriverVOSText { get; set; }
    }

    public class PrintTechnicsOKUtil
    {
        public static PrintTechnicsOKBlock GetPrintTechnicsOKBlock(int technicsId, User currentUser)
        {
            PrintTechnicsOKBlock block = new PrintTechnicsOKBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"
                               SELECT  u.NK as CommandNumber,
                                       f.militarycommandsuffix AS CommandSuffix,
                                       mr.PrintSymbol as CommandNumberPrintSymbol,
                                       CASE WHEN d.TechnicReadinessID = 2 
                                            THEN 1 
                                            ELSE 0 
                                       END as HasCommandNumPrintSymbol2,   
                                       t.TechnicsTypeName,
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
                                       s.EGN as DriverIdentNumber,
                                       SUBSTR(s.EGN, 0, 6) || '****' as DriverIdentNumberEncrypt,
                                       s.IME as DriverFirstName,
                                       s.FAM as DriverLastName,
                                       x.CompanyName as OwnerCompanyName,
                                       x.UnifiedIdentityCode as OwnerUIC,
                                       RTRIM(CASE WHEN oa3.Ime_Obl IS NOT NULL THEN 'обл. ' || oa3.Ime_Obl || ', ' ELSE '' END || 
                                		  CASE WHEN oa2.Ime_Obs IS NOT NULL THEN 'общ. ' ||oa2.Ime_Obs || ', ' ELSE '' END || 
                                		  CASE WHEN oa1.Ime_Nma IS NOT NULL THEN oa5.Ime_S || ' ' || oa1.Ime_Nma || ', ' ELSE '' END || 
                                		  CASE WHEN x.Address IS NOT NULL THEN x.Address || ', ' ELSE '' END, ', ') as OwnershipAddress,
                                       oa1.PK as OwnershipPostCode,
                                       x.PostCode as OwnershipSecondPostCode,
                                       x.Phone as OwnershipPhone,
                                       



                                       CASE t.TechnicsTypeKey                                          
                                            WHEN 'TRAILERS' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'TrailerType' AND TableKey = a2.TrailerTypeID)
                                            WHEN 'TRACTORS' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'TractorType' AND TableKey = a3.TractorTypeID)
                                            WHEN 'ENG_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'EngEquipType' AND TableKey = a4.EngEquipTypeID)
                                            WHEN 'MOB_LIFT_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'MobileLiftingEquipType' AND TableKey = a5.MobileLiftingEquipTypeID)
                                            WHEN 'RAILWAY_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'RailwayEquipType' AND TableKey = a6.RailwayEquipTypeID)
                                            WHEN 'AVIATION_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'AviationAirType' AND TableKey = a7.AviationAirTypeID)                                            
                                            WHEN 'VESSELS' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'VesselType' AND TableKey = a8.VesselTypeID)
                                            WHEN 'FUEL_CONTAINERS' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'FuelContainerType' AND TableKey = a9.FuelContainerTypeID)
                                            ELSE ''
                                        END as Type,
                                        CASE t.TechnicsTypeKey
                                             WHEN 'VEHICLES' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'VehicleEngineType' AND TableKey = a1.VehicleEngineTypeID)                                           
                                             WHEN 'ENG_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'EngEquipBaseEngineType' AND TableKey = a4.EngEquipBaseEngineTypeID)
                                             ELSE ''
                                        END as EngineType,
                                        CASE t.TechnicsTypeKey
                                             WHEN 'TRACTORS' THEN TO_CHAR(a3.power)
                                             WHEN 'VESSELS' THEN TO_CHAR(a8.EnginePower)
                                             ELSE ''
                                        END as Power,
                                        CASE t.TechnicsTypeKey
                                             WHEN 'VEHICLES' THEN TO_CHAR(a1.Seats)
                                             WHEN 'AVIATION_EQUIP' THEN TO_CHAR(a7.AirSeats)
                                             ELSE ''
                                        END as Seats,
                                        CASE t.TechnicsTypeKey
                                             WHEN 'VEHICLES' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'VehicleRoadability' AND TableKey = a1.VehicleRoadabilityID)                                                                                        
                                             ELSE ''
                                        END as Roadability,
                                        CASE t.TechnicsTypeKey
                                             WHEN 'VEHICLES' THEN a1.CarryingCapacity
                                             WHEN 'TRAILERS' THEN a2.CarryingCapacity
                                             WHEN 'AVIATION_EQUIP' THEN a7.AirCarryingCapacity
                                             ELSE NULL
                                        END as CarryingCapacity,
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
                                        js.TechMilitaryReportStatusName as MilRepStatus,
                                        
                                        CASE t.TechnicsTypeKey
                                            WHEN 'AVIATION_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'AviationOtherEquipmentKind' AND TableKey = a7.AviationOtherEquipmentKindID)
                                            ELSE NULL
                                        END as OtherEquipType,
                                        CASE t.TechnicsTypeKey
                                            WHEN 'AVIATION_EQUIP' THEN a7.EquipMileageHourSinceLstRepair
                                            ELSE NULL
                                        END as OtherEquipTypeDetails,
                                        CASE t.TechnicsTypeKey 
                                            WHEN 'VESSELS' THEN a8.StopDate
                                            ELSE NULL
                                        END as StopDate,
                                        j.Voluntary_ContractNumber as ContractNumber,
                                        t_mir.VPN || ' ' || t_mir.IMEES as MilitaryUnit,
                                        t_mir2.VPN || ' ' || t_mir2.IMEES as ContractVPN,
                                        j.Voluntary_DurationMonths as ContractDuration,
                                        f.AppointmentTime as AppointmentTime,
                                        mrs.MilReportingSpecialityCode as DriverVOSNumber,
                                        mrs.MilReportingSpecialityName as DriverVOSText                             
                               FROM PMIS_RES.Technics a
                               LEFT OUTER JOIN PMIS_RES.TechnicsAppointments c ON c.TechnicsID = a.TechnicsID AND c.IsCurrent = 1
                               LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest d ON d.TechnicsID = a.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions e ON e.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID
                               LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands f ON f.TechRequestsCommandID = e.TechRequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.EquipmentTechnicsRequests etr ON etr.EquipmentTechnicsRequestID = f.EquipmentTechnicsRequestID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR t_mir ON etr.MilitaryUnitID = t_mir.KOD_MIR                                                            
                               LEFT OUTER JOIN PMIS_RES.MilReadiness mr ON mr.MilReadinessID =  f.MilReadinessID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA g ON g.Kod_Nma = f.DeliveryCityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS h ON h.kod_obs = g.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL i ON i.Kod_Obl = g.kod_obl
                               LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus j ON j.TechnicsID = a.TechnicsID AND j.IsCurrent = 1
                               LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses js ON js.TechMilitaryReportStatusID =  j.TechMilitaryReportStatusID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = j.SourceMilDepartmentID
                               INNER JOIN PMIS_RES.TechnicsTypes t ON a.TechnicsTypeID = t.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics t2 ON a.NormativeTechnicsID = t2.NormativeTechnicsID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR t_mir2 ON t_mir2.KOD_MIR = j.Voluntary_FulfilPlaceID
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
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS p ON p.kod_obs = o.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL q ON q.Kod_Obl = o.kod_obl
                               LEFT OUTER JOIN PMIS_RES.Reservists r ON r.ReservistID = a.DriverReservistID
                               LEFT OUTER JOIN VS_OWNER.VS_LS s ON s.PersonID = r.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Persons da ON da.PersonID = r.PersonID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA da1 ON da1.Kod_Nma = s.KOD_NMA_MJ
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS da2 ON da2.kod_obs = da1.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL da3 ON da3.Kod_Obl = da1.kod_obl
                               LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec u ON u.PersonID = r.PersonID AND u.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities v ON v.MilReportSpecialityID = u.MilReportSpecialityID
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA w ON w.ZVA_KOD = s.KOD_ZVA
                               LEFT OUTER JOIN PMIS_ADM.Companies x ON x.CompanyID = a.OwnershipCompanyID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA oa1 ON oa1.Kod_Nma = x.CityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM oa5 ON oa1.kod_vnm = oa5.kod_vnm
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS oa2 ON oa2.kod_obs = oa1.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL oa3 ON oa3.Kod_Obl = oa1.kod_obl
                               LEFT OUTER JOIN UKAZ_OWNER.VVR u ON u.KOD_VVR = f.MILITARYCOMMANDID
                               LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec pmrs ON pmrs.PersonID = s.PersonID AND pmrs.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities mrs ON mrs.MilReportSpecialityID = pmrs.MilReportSpecialityID
                               WHERE a.TechnicsID = " + technicsId;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.CommandNumber = dr["CommandNumber"].ToString();

                    block.CommandNumberPrintSymbol = dr["CommandNumberPrintSymbol"].ToString();

                    if (DBCommon.GetInt(dr["HasCommandNumPrintSymbol2"]) == 1)
                        block.CommandNumberPrintSymbol2 = Config.GetWebSetting("PrintReservistsCommandNumPrintSymbol2");

                    block.CommandSuffix = dr["CommandSuffix"] is string ? dr["CommandSuffix"].ToString() : "";

                    block.TechnicsType = dr["TechnicsTypeName"].ToString();

                    block.RegNumber = dr["RegNumber"].ToString();

                    string makeName = dr["MakeName"].ToString();
                    string modelName = dr["ModelName"].ToString();

                    if (!String.IsNullOrEmpty(makeName) && !String.IsNullOrEmpty(modelName))
                        makeName += ", ";

                    block.MakeModel = makeName + modelName;

                    if (dr["FirstRegistrationDate"] is DateTime)
                        block.FirstRegistrationDate = CommonFunctions.FormatDate((DateTime)dr["FirstRegistrationDate"]);

                    block.DriverIdentNumber = dr["DriverIdentNumber"].ToString();

                    if (dr["DriverIdentNumberEncrypt"].ToString() != "****")
                        block.DriverIdentNumberEncrypt = dr["DriverIdentNumberEncrypt"].ToString();

                    block.DriverFirstName = dr["DriverFirstName"].ToString();
                    block.DriverLastName = dr["DriverLastName"].ToString();

                    string ownerCompanyname = dr["OwnerCompanyName"].ToString();
                    string ownerUIC = dr["OwnerUIC"].ToString();

                    if (!String.IsNullOrEmpty(ownerCompanyname))
                    {
                        block.OwnerFullName = ownerCompanyname;
                    }

                    if (!String.IsNullOrEmpty(ownerUIC))
                    {
                        block.OwnerUIC = ownerUIC;
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
                    block.Type = dr["Type"].ToString();
                    block.EngineType = dr["EngineType"].ToString();
                    block.Power = dr["Power"].ToString();
                    block.Seats = dr["Seats"].ToString();
                    block.Roadability = dr["Roadability"].ToString();
                    block.CarryingCapacity = dr["CarryingCapacity"].ToString();
                    block.Load = dr["Load"].ToString();
                    block.MilRepStatus = dr["MilRepStatus"].ToString();
                    block.MilitaryUnit = dr["MilitaryUnit"].ToString();

                    if (!string.IsNullOrEmpty(dr["OtherEquipType"].ToString()))
                        block.OtherEquip = "<br/>Вид: " + dr["OtherEquipType"].ToString();

                    if (!string.IsNullOrEmpty(dr["OtherEquipTypeDetails"].ToString()))
                        block.OtherEquip += "<br/>Километри/часове от последния ремонт: " + dr["OtherEquipTypeDetails"].ToString();

                    if (!string.IsNullOrEmpty(dr["StopDate"].ToString()))
                        block.StopDate = CommonFunctions.FormatDate(dr["StopDate"].ToString());
                    block.ContractNumber = dr["ContractNumber"].ToString();
                    block.ContractVPN = dr["ContractVPN"].ToString();
                    block.ContractDuration = dr["ContractDuration"].ToString();
                    block.AppointmentTime = !string.IsNullOrEmpty(dr["AppointmentTime"].ToString()) ? dr["AppointmentTime"].ToString() + " часа" : "";

                    block.DriverVOSNumber = dr["DriverVOSNumber"].ToString();
                    block.DriverVOSText = dr["DriverVOSText"].ToString();
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
