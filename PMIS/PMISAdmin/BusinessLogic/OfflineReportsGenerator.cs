using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;
using System.Text;
using PMIS.Reserve.Common;

namespace PMIS.PMISAdmin.Common
{
    public class OfflineReportsGenerator
    {
        private User user;

        public string GenerateReservistsTab(MilitaryDepartment militaryDepartment)
        {
            StringBuilder sb = new StringBuilder();            

            ReservistManageFilter filter = new ReservistManageFilter();
            filter.MilitaryReportStatus = MilitaryReportStatusUtil.GetMilitaryReportStatusByKey("COMPULSORY_RESERVE_MOB_APPOINTMENT", user).MilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<ReservistManageBlock> reservists = ReservistUtil.GetAllReservistManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""Резервисти"">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""180""/>
                           <Column ss:Width=""180""/>
                           <Column ss:Width=""58""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""200""/>
                           <Column ss:Width=""150""/>
                           <Column ss:Width=""60""/>
                           <Column ss:Width=""155""/>
                           <Column ss:Width=""100""/>
                           <Column ss:Width=""150""/> 
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""8"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""8"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""8"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""8"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на водените на военен отчет резервисти във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>  
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Име и презиме</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Фамилия</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">ЕГН</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Звание</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Населено място</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">На отчет в</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Състояние по отчета</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Команда</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Подходяща длъжност</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (ReservistManageBlock reservist in reservists)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.FirstAndSurName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.FamilyName+ @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.IdentNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.MilitaryRankName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.RegionMuniciplaityAndCity + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.MilitaryDepartment + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.MilitaryCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.MilitaryReportStatus + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.MilitaryCommand + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + reservist.PositionTitle + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateVehiclesTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            VehicleManageFilter filter = new VehicleManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<VehicleManageBlock> vehicles = VehicleUtil.GetAllVehicleManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Регистрационен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Марка</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Модел</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (VehicleManageBlock vehicle in vehicles)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                                <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                                <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vehicle.NormativeTechnicsCode + " " + vehicle.NormativeTechnicsName + @"</Data></Cell>
                                <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vehicle.RegNumber + @"</Data></Cell>
                                <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vehicle.InventoryNumber + @"</Data></Cell>
                                <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vehicle.TechnicsCategory + @"</Data></Cell>
                                <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vehicle.VehicleKind + @"</Data></Cell>
                                <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vehicle.VehicleMakeName + @"</Data></Cell>
                                <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vehicle.VehicleModelName + @"</Data></Cell>
                             </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateTrailersTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            TrailerManageFilter filter = new TrailerManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<TrailerManageBlock> trailers = TrailerUtil.GetAllTrailerManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Регистрационен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Тип</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (TrailerManageBlock trailer in trailers)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + trailer.NormativeTechnicsCode + " " + trailer.NormativeTechnicsName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + trailer.RegNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + trailer.InventoryNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + trailer.TechnicsCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + trailer.TrailerKind + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + trailer.TrailerType + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateTractorsTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            TractorManageFilter filter = new TractorManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<TractorManageBlock> tractors = TractorUtil.GetAllTractorManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Регистрационен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Марка</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Модел</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (TractorManageBlock tractor in tractors)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + tractor.NormativeTechnicsCode + " " + tractor.NormativeTechnicsName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + tractor.RegNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + tractor.InventoryNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + tractor.TechnicsCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + tractor.TractorKind + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + tractor.TractorMakeName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + tractor.TractorModelName + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateEngEquipTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            EngEquipManageFilter filter = new EngEquipManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<EngEquipManageBlock> engEquips = EngEquipUtil.GetAllEngEquipManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>  
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""7"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Регистрационен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Марка</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Модел</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (EngEquipManageBlock engEquip in engEquips)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + engEquip.NormativeTechnicsCode + " " + engEquip.NormativeTechnicsName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + engEquip.RegNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + engEquip.InventoryNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + engEquip.TechnicsCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + engEquip.EngEquipKind + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + engEquip.EngEquipBaseMakeName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + engEquip.EngEquipBaseModelName + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateMobileLiftingEquipTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            MobileLiftingEquipManageFilter filter = new MobileLiftingEquipManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<MobileLiftingEquipManageBlock> mobileLiftingEquips = MobileLiftingEquipUtil.GetAllMobileLiftingEquipManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""140""/>
                           <Column ss:Width=""140""/>
                           <Column ss:Width=""140""/>
                           <Column ss:Width=""140""/>
                           <Column ss:Width=""140""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row> 
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Регистрационен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Тип</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (MobileLiftingEquipManageBlock mobileLiftingEquip in mobileLiftingEquips)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + mobileLiftingEquip.NormativeTechnicsCode + " " + mobileLiftingEquip.NormativeTechnicsName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + mobileLiftingEquip.RegNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + mobileLiftingEquip.InventoryNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + mobileLiftingEquip.TechnicsCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + mobileLiftingEquip.MobileLiftingEquipKind + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + mobileLiftingEquip.MobileLiftingEquipType + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateRailwayEquipTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            RailwayEquipManageFilter filter = new RailwayEquipManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<RailwayEquipManageBlock> railwayEquips = RailwayEquipUtil.GetAllRailwayEquipManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""140""/>
                           <Column ss:Width=""140""/>
                           <Column ss:Width=""140""/>
                           <Column ss:Width=""140""/>
                           <Column ss:Width=""140""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Тип</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Брой</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (RailwayEquipManageBlock railwayEquip in railwayEquips)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + railwayEquip.NormativeTechnicsCode + " " + railwayEquip.NormativeTechnicsName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + railwayEquip.InventoryNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + railwayEquip.TechnicsCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + railwayEquip.RailwayEquipKind + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + railwayEquip.RailwayEquipType + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""Number"">" + railwayEquip.ItemsCount + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateAviationEquipTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            AviationEquipManageFilter filter = new AviationEquipManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<AviationEquipManageBlock> aviationEquips = AviationEquipUtil.GetAllAviationEquipManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Тип</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Модел</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (AviationEquipManageBlock aviationEquip in aviationEquips)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + aviationEquip.NormativeTechnicsCode + " " + aviationEquip.NormativeTechnicsName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + aviationEquip.AirInvNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + aviationEquip.TechnicsCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + aviationEquip.AviationAirKind + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + aviationEquip.AviationAirType + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + aviationEquip.AviationAirModelName + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateVesselTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            VesselManageFilter filter = new VesselManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<VesselManageBlock> vessels = VesselUtil.GetAllVesselManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Име</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Тип</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (VesselManageBlock vessel in vessels)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vessel.NormativeTechnicsCode + " " + vessel.NormativeTechnicsName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vessel.VesselName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vessel.InventoryNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vessel.TechnicsCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vessel.VesselKind + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + vessel.VesselType + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateFuelContainerTab(MilitaryDepartment militaryDepartment, TechnicsType technicsType)
        {
            StringBuilder sb = new StringBuilder();

            FuelContainerManageFilter filter = new FuelContainerManageFilter();
            filter.MilitaryReportStatus = TechMilitaryReportStatusUtil.GetTechMilitaryReportStatusByKey("MOBILE_APPOINTMENT", user).TechMilitaryReportStatusId.ToString();
            filter.MilitaryDepartment = militaryDepartment.MilitaryDepartmentId.ToString();

            List<FuelContainerManageBlock> fuelContainers = FuelContainerUtil.GetAllFuelContainerManageBlocks(filter, 0, user);

            sb.Append(@"<Worksheet ss:Name=""" + PrepareWorksheetNameForExcel(technicsType.TypeName) + @""">
                          <Table>
                           <Column ss:Width=""30""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Column ss:Width=""120""/>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">Списък на техниката водена на военен отчет във " + militaryDepartment.MilitaryDepartmentName + @"</Data></Cell>
                           </Row>
                           <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                            <Cell ss:MergeAcross=""6"" ss:StyleID=""HT3""><Data ss:Type=""String"">" + technicsType.TypeName + @"</Data></Cell>
                           </Row>
                           <Row>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                            <Cell ss:StyleID=""DT""/>
                           </Row>
                           <Row ss:Height=""30.75"">
                            <Cell ss:StyleID=""LTH""><Data ss:Type=""String"">№</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Нормативна категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Инвентарен номер</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Категория</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Вид</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Тип</Data></Cell>
                            <Cell ss:StyleID=""TH""><Data ss:Type=""String"">Брой</Data></Cell>
                           </Row>");

            int counter = 0;

            foreach (FuelContainerManageBlock fuelContainer in fuelContainers)
            {
                counter += 1;

                sb.Append(@"<Row ss:Height=""13.5"">
                            <Cell ss:StyleID=""LTC""><Data ss:Type=""Number"">" + counter + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + fuelContainer.NormativeTechnicsCode + " " + fuelContainer.NormativeTechnicsName + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + fuelContainer.InventoryNumber + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + fuelContainer.TechnicsCategory + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + fuelContainer.FuelContainerKind + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""String"">" + fuelContainer.FuelContainerType + @"</Data></Cell>
                            <Cell ss:StyleID=""TC""><Data ss:Type=""Number"">" + fuelContainer.ItemsCount + @"</Data></Cell>
                           </Row>");
            }

            sb.Append(@"</Table>
                          <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                           <Print>
                            <ValidPrinterInfo/>
                            <HorizontalResolution>200</HorizontalResolution>
                            <VerticalResolution>200</VerticalResolution>
                           </Print>                           
                           <DoNotDisplayGridlines/>                         
                           <ProtectObjects>False</ProtectObjects>
                           <ProtectScenarios>False</ProtectScenarios>
                          </WorksheetOptions>
                         </Worksheet>");

            return sb.ToString();
        }

        public string GenerateReport(MilitaryDepartment militaryDepartment)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"<?xml version=""1.0""?>
                        <?mso-application progid=""Excel.Sheet""?>
                        <Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""
                         xmlns:o=""urn:schemas-microsoft-com:office:office""
                         xmlns:x=""urn:schemas-microsoft-com:office:excel""
                         xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""
                         xmlns:html=""http://www.w3.org/TR/REC-html40"">                       
                         <ExcelWorkbook xmlns=""urn:schemas-microsoft-com:office:excel"">
                          <WindowHeight>11055</WindowHeight>
                          <WindowWidth>21075</WindowWidth>
                          <WindowTopX>240</WindowTopX>
                          <WindowTopY>60</WindowTopY>
                          <ProtectStructure>False</ProtectStructure>
                          <ProtectWindows>False</ProtectWindows>
                         </ExcelWorkbook>
                         <Styles>
                          <Style ss:ID=""Default"" ss:Name=""Normal"">
                           <Alignment ss:Vertical=""Bottom""/>
                           <Borders/>
                           <Font/>
                           <Interior/>
                           <NumberFormat/>
                           <Protection/>
                          </Style>
                          <Style ss:ID=""DT"">
                           <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                           <Font ss:FontName=""Arial Unicode MS""/>
                          </Style>
                          <Style ss:ID=""HT1"">
                           <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                           <Font ss:FontName=""Arial Unicode MS"" ss:Size=""16"" ss:Bold=""1""/>
                          </Style>
                          <Style ss:ID=""HT2"">
                           <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                           <Font ss:FontName=""Arial Unicode MS"" ss:Size=""20"" ss:Bold=""1""/>
                          </Style>
                          <Style ss:ID=""HT3"">
                           <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                           <Font ss:FontName=""Arial Unicode MS"" ss:Size=""13"" ss:Bold=""1""/>
                          </Style>                         
                          <Style ss:ID=""LTH"">
                           <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""/>
                           <Borders>
                            <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#FFFFFF""/>
                           </Borders>
                           <Font ss:FontName=""Arial Unicode MS"" ss:Color=""#FFFFFF"" ss:Bold=""1""/>
                           <Interior ss:Color=""#000000"" ss:Pattern=""Solid""/>
                          </Style>
                          <Style ss:ID=""TH"">
                           <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""/>
                           <Borders>
                            <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#FFFFFF""/>
                            <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#FFFFFF""/>
                           </Borders>
                           <Font ss:FontName=""Arial Unicode MS"" ss:Color=""#FFFFFF"" ss:Bold=""1""/>
                           <Interior ss:Color=""#000000"" ss:Pattern=""Solid""/>
                          </Style>
                          <Style ss:ID=""LTC"">
                           <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                           <Borders>
                            <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#000000""/>
                            <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#000000""/>
                            <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#000000""/>
                            <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#000000""/>
                           </Borders>
                           <Font ss:FontName=""Arial Unicode MS""/>
                          </Style>                      
                          <Style ss:ID=""TC"">
                           <Alignment ss:Horizontal=""Left"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                           <Borders>
                            <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#000000""/>
                            <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#000000""/>
                            <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#000000""/>
                            <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""2""
                             ss:Color=""#000000""/>
                           </Borders>
                           <Font ss:FontName=""Arial Unicode MS""/>
                          </Style>
                         </Styles>");

            sb.Append(GenerateReservistsTab(militaryDepartment));

            List<TechnicsType> technicsTypes = TechnicsTypeUtil.GetAllTechnicsTypes(user);

            foreach (TechnicsType technicsType in technicsTypes)
            {
                if(technicsType.TypeKey == "VEHICLES")
                    sb.Append(GenerateVehiclesTab(militaryDepartment, technicsType));
                else if (technicsType.TypeKey == "TRAILERS")
                    sb.Append(GenerateTrailersTab(militaryDepartment, technicsType));
                else if (technicsType.TypeKey == "TRACTORS")
                    sb.Append(GenerateTractorsTab(militaryDepartment, technicsType));
                else if (technicsType.TypeKey == "ENG_EQUIP")
                    sb.Append(GenerateEngEquipTab(militaryDepartment, technicsType));
                else if (technicsType.TypeKey == "MOB_LIFT_EQUIP")
                    sb.Append(GenerateMobileLiftingEquipTab(militaryDepartment, technicsType));
                else if (technicsType.TypeKey == "RAILWAY_EQUIP")
                    sb.Append(GenerateRailwayEquipTab(militaryDepartment, technicsType));
                else if (technicsType.TypeKey == "AVIATION_EQUIP")
                    sb.Append(GenerateAviationEquipTab(militaryDepartment, technicsType));
                else if (technicsType.TypeKey == "VESSELS")
                    sb.Append(GenerateVesselTab(militaryDepartment, technicsType));
                else if (technicsType.TypeKey == "FUEL_CONTAINERS")
                    sb.Append(GenerateFuelContainerTab(militaryDepartment, technicsType));
            }    

            sb.Append("</Workbook>");

            return sb.ToString();
        }

        public Dictionary<string, string> GenerateOfflineReports()
        {
            Dictionary<string, string> allReports = new Dictionary<string, string>();

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsWithoutRestrictions(user);

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                allReports.Add(militaryDepartment.MilitaryDepartmentId.ToString() + " - " + militaryDepartment.MilitaryDepartmentName ,GenerateReport(militaryDepartment));
            }

            return allReports;
        }

        public OfflineReportsGenerator(User user)
        {
            this.user = user;
        }

        private string PrepareWorksheetNameForExcel(string worksheetName)
        {
            if (worksheetName.Length > 30)
                worksheetName = worksheetName.Substring(0, 30);

            worksheetName = worksheetName.Replace("?", "");
            worksheetName = worksheetName.Replace("\\", "");
            worksheetName = worksheetName.Replace("/", "");
            worksheetName = worksheetName.Replace("\"", "");
            worksheetName = worksheetName.Replace("'", "");
            worksheetName = worksheetName.Replace("<", "");
            worksheetName = worksheetName.Replace(">", "");
            worksheetName = worksheetName.Replace("*", "");
            worksheetName = worksheetName.Replace("|", "");
            worksheetName = worksheetName.Replace(":", "");
            worksheetName = worksheetName.Replace("[", "");
            worksheetName = worksheetName.Replace("]", "");

            return worksheetName;
        }
    }
}