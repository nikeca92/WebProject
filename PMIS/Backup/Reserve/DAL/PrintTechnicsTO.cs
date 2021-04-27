using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Common;
using System.Data.OracleClient;

namespace PMIS.Reserve.DAL
{
    public class PrintTechnicsTOBlock
    {
        public string TechnicsType { get; set; }
        public string RegNumber { get; set; }
        public string MakeModel { get; set; }
        public string MilitaryDepartmentName { get; set; }
        public string MilitaryDepartmentNameUpper { get; set; }
        public string DriverIdentNumber { get; set; }
        public string DriverIdentNumberEncrypt { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public string DriverPosition { get; set; }
        public string DriverPositionNKPD { get; set; }
        public string DriverCompanyName { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerFullNameUpper { get; set; }
        public string OwnerUIC { get; set; }
        public string Kind { get; set; }
        public string CarryingCapacity { get; set; }
        public string PostponeYear { get; set; }
        public string PostponeYearNext { get; set; }
    }

    public class PrintTechnicsTOUtil
    {
        public static PrintTechnicsTOBlock GetPrintTechnicsTOBlock(int technicsId, User currentUser)
        {
            PrintTechnicsTOBlock block = new PrintTechnicsTOBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT t.TechnicsTypeName,
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
                                       k.MilitaryDepartmentName,
                                       s.EGN as DriverIdentNumber,
                                       SUBSTR(s.EGN, 0, 6) || '****' as DriverIdentNumberEncrypt,
                                       s.IME as DriverFirstName,
                                       s.FAM as DriverLastName,
                                       NVL(dp1.PositionTitle, ' ') as DriverPosition,
                                       da.WorkPositionNKPDID as DriverPositionNKPD,
                                       dc.CompanyName as DriverCompanyName,                                       
                                       x.CompanyName as OwnerCompanyName,
                                       x.UnifiedIdentityCode as OwnerUIC,

                                       CASE t.TechnicsTypeKey                                          
                                            WHEN 'TRAILERS' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'TrailerKind' AND TableKey = a2.TrailerKindID)
                                            WHEN 'TRACTORS' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'TractorKind' AND TableKey = a3.TractorKindID)
                                            WHEN 'ENG_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'EngEquipKind' AND TableKey = a4.EngEquipKindID)
                                            WHEN 'MOB_LIFT_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'MobileLiftingEquipKind' AND TableKey = a5.MobileLiftingEquipKindID)
                                            WHEN 'RAILWAY_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'RailwayEquipKind' AND TableKey = a6.RailwayEquipKindID)
                                            WHEN 'AVIATION_EQUIP' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'AviationAirKind' AND TableKey = a7.AviationAirKindID)                                            
                                            WHEN 'VESSELS' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'VesselKind' AND TableKey = a8.VesselKindID)
                                            WHEN 'FUEL_CONTAINERS' THEN (SELECT TableValue FROM PMIS_RES.GTable WHERE TableName = 'FuelContainerKind' AND TableKey = a9.FuelContainerKindID)
                                            ELSE ''
                                        END as Kind,                                         
                                        CASE t.TechnicsTypeKey
                                             WHEN 'VEHICLES' THEN a1.CarryingCapacity
                                             WHEN 'TRAILERS' THEN a2.CarryingCapacity
                                             WHEN 'AVIATION_EQUIP' THEN a7.AirCarryingCapacity
                                             ELSE NULL
                                        END as CarryingCapacity,
                                        j.TechnicsPostpone_Year as PostponeYear
                   
                               FROM PMIS_RES.Technics a
                               LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus j ON j.TechnicsID = a.TechnicsID AND j.IsCurrent = 1
                               LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses js ON js.TechMilitaryReportStatusID =  j.TechMilitaryReportStatusID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = j.SourceMilDepartmentID
                               INNER JOIN PMIS_RES.TechnicsTypes t ON a.TechnicsTypeID = t.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.Vehicles a1 ON a.TechnicsID = a1.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Trailers a2 ON a.TechnicsID = a2.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Tractors a3 ON a.TechnicsID = a3.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.EngEquipment a4 ON a.TechnicsID = a4.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.MobileLiftingEquip a5 ON a.TechnicsID = a5.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.RailWayEquips a6 ON a.TechnicsID = a6.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.AviationEquipment a7 ON a.TechnicsID = a7.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Vessels a8 ON a.TechnicsID = a8.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.FuelContainers a9 ON a.TechnicsID = a9.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Reservists r ON r.ReservistID = a.DriverReservistID
                               LEFT OUTER JOIN VS_OWNER.VS_LS s ON s.PersonID = r.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Persons da ON da.PersonID = r.PersonID                               
                               LEFT OUTER JOIN PMIS_ADM.Companies dc ON dc.CompanyID = da.WorkCompanyID
                               LEFT OUTER JOIN PMIS_ADM.PersonPositionTitles dp on dp.PersonID = da.PersonID AND dp.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.PositionTitles dp1 on dp1.PositionTitleID = dp.PositionTitleID                               
                               LEFT OUTER JOIN PMIS_ADM.Companies x ON x.CompanyID = a.OwnershipCompanyID
                               WHERE a.TechnicsID =" + technicsId;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    block.TechnicsType = dr["TechnicsTypeName"].ToString();
                    block.RegNumber = dr["RegNumber"].ToString();

                    string makeName = dr["MakeName"].ToString();
                    string modelName = dr["ModelName"].ToString();

                    if (!String.IsNullOrEmpty(makeName) && !String.IsNullOrEmpty(modelName))
                        makeName += ", ";

                    block.MakeModel = makeName + modelName;
                    block.DriverIdentNumber = dr["DriverIdentNumber"].ToString();

                    if (dr["DriverIdentNumberEncrypt"].ToString() != "****")
                        block.DriverIdentNumberEncrypt = dr["DriverIdentNumberEncrypt"].ToString();

                    block.DriverFirstName = dr["DriverFirstName"].ToString();
                    block.DriverLastName = dr["DriverLastName"].ToString();
                    block.DriverCompanyName = dr["DriverCompanyName"].ToString();

                    NKPD workPositionNKPD = null;
                    if (DBCommon.IsInt(dr["DriverPositionNKPD"]))
                    {
                        workPositionNKPD = NKPDUtil.GetNKPD(DBCommon.GetInt(dr["DriverPositionNKPD"]), currentUser);
                        block.DriverPositionNKPD = workPositionNKPD.Code;
                        block.DriverPosition = workPositionNKPD.Name;
                    }
                    

                    string ownerCompanyname = dr["OwnerCompanyName"].ToString();
                    string ownerUIC = dr["OwnerUIC"].ToString();

                    if (!String.IsNullOrEmpty(ownerCompanyname))
                    {
                        block.OwnerFullName = ownerCompanyname;
                        block.OwnerFullNameUpper = ownerCompanyname.ToUpper();
                    }

                    if (!String.IsNullOrEmpty(ownerUIC))
                    {
                        block.OwnerUIC = ownerUIC;
                    }

                    block.Kind = dr["Kind"].ToString();
                    block.CarryingCapacity = dr["CarryingCapacity"].ToString();
                    block.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();
                    block.MilitaryDepartmentNameUpper = dr["MilitaryDepartmentName"].ToString().ToUpper();
                    if (!string.IsNullOrEmpty(dr["PostponeYear"].ToString()))
                    {
                        block.PostponeYear = dr["PostponeYear"].ToString();
                        string postponeYearNext = (Convert.ToInt32(block.PostponeYear) + 1).ToString();
                        block.PostponeYearNext = postponeYearNext;
                    }
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
