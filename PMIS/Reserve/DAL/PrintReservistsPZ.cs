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
    public class PrintReservistsPZBlock
    {
        public string Command { get; set; }
        public string CommandNumber { get; set; }
        public string CommandNumberPrintSymbol { get; set; }
        public string CommandNumberPrintSymbol2 { get; set; }
        public string CommandName { get; set; }
        public string CommandSuffix { get; set; }
        public string ReservistReadinessName { get; set; }
        public string AppointMilRepSpeciality { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentNumber { get; set; }
        public string IdentNumberEncrypt { get; set; }
        public string MilitaryRank { get; set; }
        public string PermAddress { get; set; }
        public string PermAddressDistrict { get; set; }
        public string DeliveryPlace { get; set; }
        public string MilitaryDepartment { get; set; }
    }

    public static class PrintReservistsPZUtil
    {
        public static PrintReservistsPZBlock GetPrintReservistsPZBlock(int reservistId, User currentUser)
        {
            PrintReservistsPZBlock block = new PrintReservistsPZBlock();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"
                               SELECT c.MilitaryCommandName as Command,
                                      u.NK as CommandNumber,
                                      u.IMEES as CommandName,
                                      f.militarycommandsuffix AS CommandSuffix,
                                      c.ReservistReadinessID,
                                      c.MilReportingSpecialityCode as AppointMilRepSpeciality,
                                      n.ZVA_IMEES as MilitaryRankName,
                                      b.IME as FirstName,
                                      b.FAM as LastName,
                                      b.EGN as IdentNumber,
                                      SUBSTR(b.EGN, 0, 6) || '****' as IdentNumberEncrypt,
                                      RTRIM(CASE WHEN q.Ime_Obl IS NOT NULL THEN 'обл. ' || q.Ime_Obl || ', ' ELSE '' END || CASE WHEN p.Ime_Obs IS NOT NULL THEN 'общ. ' || p.Ime_Obs || ', ' ELSE '' END || CASE WHEN o.Ime_Nma IS NOT NULL THEN o1.Ime_S || ' ' || o.Ime_Nma || ', ' ELSE '' END || CASE WHEN b.ADRES IS NOT NULL THEN b.ADRES || ', ' ELSE '' END, ', ') as PermAddress,
                                      o.PK as PermPostCode,
                                      b.PermSecondPostCode,
                                      n1.DistrictName as PermAddressDistrict,
                                      RTRIM(CASE WHEN i.Ime_Obl IS NOT NULL THEN 'обл. ' || i.Ime_Obl || ', ' ELSE '' END || CASE WHEN h.Ime_Obs IS NOT NULL THEN 'общ. ' || h.Ime_Obs || ', ' ELSE '' END || CASE WHEN g.Ime_Nma IS NOT NULL THEN g1.Ime_S || ' ' || g.Ime_Nma || ', ' ELSE '' END || CASE WHEN f.DeliveryPlace IS NOT NULL THEN f.DeliveryPlace || ', ' ELSE '' END, ', ') as DeliveryPlace,
                                      k.MilitaryDepartmentName,
                                      mr.PrintSymbol as CommandNumberPrintSymbol,
                                      CASE WHEN d.ReservistReadinessID = 2 
                                           THEN 1 
                                           ELSE 0 
                                      END as HasCommandNumPrintSymbol2
                               FROM PMIS_RES.Reservists a
                               INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                               INNER JOIN PMIS_ADM.Persons b1 ON a.PersonID = b1.PersonID
                               INNER JOIN PMIS_RES.ReservistAppointments c ON c.ReservistId = a.ReservistId AND c.IsCurrent = 1
                               INNER JOIN PMIS_RES.FillReservistsRequest d ON d.ReservistId = a.ReservistId
                               INNER JOIN PMIS_RES.RequestCommandPositions e ON e.RequestCommandPositionID = d.RequestCommandPositionID
                               INNER JOIN PMIS_RES.RequestsCommands f ON f.RequestsCommandID = e.RequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.MilReadiness mr ON mr.MilReadinessID =  f.MilReadinessID 
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA n ON n.ZVA_KOD = b.KOD_ZVA
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA o ON o.Kod_Nma = b.KOD_NMA_MJ
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM o1 ON o1.KOD_VNM = o.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS p ON p.kod_obs = o.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL q ON q.Kod_Obl = o.kod_obl
                               LEFT OUTER JOIN UKAZ_OWNER.Districts n1 ON n1.DistrictID = b.PermAddrDistrictID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA g ON g.Kod_Nma = f.DeliveryCityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM g1 ON g1.KOD_VNM = g.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS h ON h.kod_obs = g.kod_obs
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL i ON i.Kod_Obl = g.kod_obl
                               INNER JOIN PMIS_RES.ReservistMilRepStatuses j ON j.ReservistID = a.ReservistID AND j.IsCurrent = 1
                               INNER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = j.SourceMilDepartmentID
                               LEFT OUTER JOIN UKAZ_OWNER.VVR u ON u.KOD_VVR = f.MILITARYCOMMANDID
                               WHERE a.ReservistID = " + reservistId.ToString();

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

                    block.ReservistReadinessName = ReadinessUtil.ReadinessName(DBCommon.GetInt(dr["ReservistReadinessID"]));

                    if (!String.IsNullOrEmpty(block.Command) && !String.IsNullOrEmpty(block.ReservistReadinessName))
                    {
                        block.Command += ", ";
                    }

                    block.AppointMilRepSpeciality = dr["AppointMilRepSpeciality"].ToString();
                    block.MilitaryRank = dr["MilitaryRankName"].ToString().ToLower();
                    block.FirstName = dr["FirstName"].ToString();
                    block.LastName = dr["LastName"].ToString();
                    block.IdentNumber = dr["IdentNumber"].ToString();
                    block.IdentNumberEncrypt = dr["IdentNumberEncrypt"].ToString();
                    block.PermAddress = dr["PermAddress"].ToString();

                    string permPostCode = dr["PermPostCode"].ToString();
                    string permSecondPostCode = dr["PermSecondPostCode"].ToString();

                    if (!String.IsNullOrEmpty(permSecondPostCode))
                    {
                        block.PermAddress += (String.IsNullOrEmpty(block.PermAddress) ? "" : ", ПК ") + permSecondPostCode;
                    }
                    else if (!String.IsNullOrEmpty(permPostCode) && permPostCode != "0")
                    {
                        block.PermAddress += (String.IsNullOrEmpty(block.PermAddress) ? "" : ", ПК ") + permPostCode;
                    }

                    block.PermAddressDistrict = dr["PermAddressDistrict"].ToString();
                    if (!String.IsNullOrEmpty(block.PermAddressDistrict))
                    {
                        block.PermAddressDistrict = "Район: " + block.PermAddressDistrict;
                    }

                    block.DeliveryPlace = dr["DeliveryPlace"].ToString();
                    block.MilitaryDepartment = dr["MilitaryDepartmentName"].ToString();
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