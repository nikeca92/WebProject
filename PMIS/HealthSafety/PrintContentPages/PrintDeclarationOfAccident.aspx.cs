using System;
using System.Text;
using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Collections.Generic;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintDeclarationOfAccident : HSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnGenerateWord.Style.Add("display", "none");

            int accidentDeclarationID = 0;

            if (int.TryParse(Request.Params["DeclarationOfAccidentID"], out accidentDeclarationID))
            {
                // Check visibility right for the print screen
                bool screenHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC") == UIAccessLevel.Hidden ||
                                    this.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Hidden;

                if (screenHidden)
                {
                    Response.Redirect("~");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        string html = GenerateDeclaration(false);
                        pnlDeclaration.InnerHtml = html;
                    }
                }
            }
        }

        private string GenerateDeclaration(bool exportToWord)
        {
            int accidentDeclarationId = int.Parse(Request.Params["DeclarationOfAccidentID"]);
            DeclarationOfAccident declarationOfAccident = DeclarationOfAccidentUtil.GetDeclarationOfAccident(accidentDeclarationId, CurrentUser, "btnTabEmpl");

            string emptyCheckbox = "<span style='font-size: 11pt; font-style:normal;'>&#x2610;</span>";
            string filledCheckbox = "<span style='font-size: 11pt; font-style:normal;'>&#x2612;</span>";

            string numberBoxWidth = "";
            string numberBoxWidthCSS = "width:15.2pt;";
            string numberBoxHeightCSS = "height:11.45pt";

            string declarNumberDB = "";
            string declarDateFromDB = "";
            string referNumberDB = "";
            string referDateFromDB = "";
            string fileNumberDB = "";

            DeclarationOfAccidentHeader declarationOfAccidentHeader = declarationOfAccident.DeclarationOfAccidentHeader;
            if (declarationOfAccidentHeader != null)
            {
                declarNumberDB = declarationOfAccidentHeader.DeclarationNumber.ToString();
                declarDateFromDB = (declarationOfAccidentHeader.DeclarationDate.HasValue ? CommonFunctions.FormatDate(declarationOfAccidentHeader.DeclarationDate.ToString()) : CommonFunctions.Replicate("&nbsp;", 10)) + " г.";
                referNumberDB = declarationOfAccidentHeader.ReferenceNumber.ToString();
                referDateFromDB = (declarationOfAccidentHeader.ReferenceDate.HasValue ? CommonFunctions.FormatDate(declarationOfAccidentHeader.ReferenceDate.ToString()) : CommonFunctions.Replicate("&nbsp;", 10)) + " г.";
                fileNumberDB = declarationOfAccidentHeader.FileNumber.ToString();
            }

            string emplNameDB = "";
            string emplEikDB = "";
            string emplRegionDB = "";
            string emplMunicipalityDB = "";
            string emplCityDB = "";
            string emplPostCodeDB = "";
            string emplStreetDB = "";
            string emplStreetNumberDB = "";
            string emplDistrictDB = "";
            string emplBlockDB = "";
            string emplEntranceDB = "";
            string emplFloorDB = "";
            string emplAptDB = "";
            string emplPhoneDB = "";
            string emplFaxDB = "";
            string emplEmailDB = "";
            string emplNumberOfEmployeesDB = "";
            string emplFemaleEmployeesDB = "";

            Employer employer = declarationOfAccident.Employer;
            if (employer != null)
            {
                emplNameDB = employer.EmployerName.ToString();
                emplEikDB = employer.EmplEik.ToString();

                if (employer.City != null && employer.City.CityId > 0)
                {
                    List<int> listResultEmployer = DeclarationOfAccidentUtil.GetListForEmployerCityId(employer.City.CityId, CurrentUser);
                    emplRegionDB = RegionUtil.GetRegion(listResultEmployer[0], CurrentUser).RegionName.ToString();
                    emplMunicipalityDB = MunicipalityUtil.GetMunicipality(listResultEmployer[1], CurrentUser).MunicipalityName.ToString();
                    emplCityDB = employer.City.CityName.ToString();
                    emplPostCodeDB = listResultEmployer[2].ToString();
                }

                emplStreetDB = employer.EmplStreet.ToString();
                emplStreetNumberDB = employer.EmplStreetNum.ToString();
                emplDistrictDB = employer.EmplDistrict.ToString();
                emplBlockDB = employer.EmplBlock.ToString();
                emplEntranceDB = employer.EmplEntrance.ToString();
                emplFloorDB = employer.EmplFloor.ToString();
                emplAptDB = employer.EmplApt.ToString();

                emplPhoneDB = employer.EmplPhone.ToString();
                emplFaxDB = employer.EmplFax.ToString();
                emplEmailDB = employer.EmplEmail.ToString();
                emplNumberOfEmployeesDB = employer.EmplNumberOfEmployees.ToString();
                emplFemaleEmployeesDB = employer.EmplFemaleEmployees.ToString();
            }

            string workerFullNameDB = "";
            string workerEgnDB = "";
            string workerRegionDB = "";
            string workerMunicipalityDB = "";
            string workerCityDB = "";
            string workerPostCodeDB = "";
            string workerStreetDB = "";
            string workerStreetNumberDB = "";
            string workerDistrictDB = "";
            string workerBlockDB = "";
            string workerEntranceDB = "";
            string workerFloorDB = "";
            string workerAptDB = "";
            string workerPhoneDB = "";
            string workerFaxDB = "";
            string workerEmailDB = "";
            string workerBirthDateDB = "";
            string wGenderMaleDB = "";
            string wGenderFemaleDB = "";
            string workerCitizenshipDB = "";
            string wHireType1DB = "";
            string wHireType2DB = "";
            string wWorkTime1DB = "";
            string wWorkTime2DB = "";
            string workerHireDateDB = "";
            string workerJobTitleDB = "";
            string workerJobCodeDB = "";
            string workerJobCategory1DB = "";
            string workerJobCategory2DB = "";
            string workerJobCategory3DB = "";
            string workerYearsOnServiceDB = "";
            string workerCurrentJobYearsOnServiceDB = "";
            string workerBranchDB = "";


            DeclarationOfAccidentWorker declarationOfAccidentWorker = DeclarationOfAccidentUtil.GetDeclarationOfAccident(accidentDeclarationId, CurrentUser, "btnTabWorker").DeclarationOfAccidentWorker;
            if (declarationOfAccidentWorker != null)
            {
                workerFullNameDB = declarationOfAccidentWorker.WorkerFullName.ToString();
                workerEgnDB = declarationOfAccidentWorker.WorkerEgn.ToString();

                List<int> listResultWorker = DeclarationOfAccidentUtil.GetListForEmployerCityId(declarationOfAccidentWorker.City.CityId, CurrentUser);
                if (listResultWorker.Count > 0)
                {
                    workerRegionDB = RegionUtil.GetRegion(listResultWorker[0], CurrentUser).RegionName.ToString();
                    workerMunicipalityDB = MunicipalityUtil.GetMunicipality(listResultWorker[1], CurrentUser).MunicipalityName.ToString();
                    workerCityDB = declarationOfAccidentWorker.City.CityName;
                    workerPostCodeDB = listResultWorker[2].ToString();
                }

                workerStreetDB = declarationOfAccidentWorker.WStreet.ToString();
                workerStreetNumberDB = declarationOfAccidentWorker.WStreetNum.ToString();
                workerDistrictDB = declarationOfAccidentWorker.WDistrict.ToString();
                workerBlockDB = declarationOfAccidentWorker.WBlock.ToString();
                workerEntranceDB = declarationOfAccidentWorker.WEntrance.ToString();
                workerFloorDB = declarationOfAccidentWorker.WFloor.ToString();
                workerAptDB = declarationOfAccidentWorker.WApt.ToString();
                workerPhoneDB = declarationOfAccidentWorker.WPhone.ToString();
                workerFaxDB = declarationOfAccidentWorker.WFax.ToString();
                workerEmailDB = declarationOfAccidentWorker.WEmail.ToString();
                workerBirthDateDB = (declarationOfAccidentWorker.WBirthDate.HasValue ? CommonFunctions.FormatDate(declarationOfAccidentWorker.WBirthDate.ToString()) : CommonFunctions.Replicate("&nbsp;", 10)) + " г.,";

                switch (declarationOfAccidentWorker.WGender)
                {
                    case 1:
                        wGenderMaleDB = "1";
                        break;
                    case 2:
                        wGenderFemaleDB = "1";
                        break;
                }

                workerCitizenshipDB = declarationOfAccidentWorker.WCitizenship.ToString();

                switch (declarationOfAccidentWorker.WHireType)
                {
                    case 1:
                        wHireType1DB = "1";
                        break;
                    case 2:
                        wHireType2DB = "1";
                        break;
                }

                switch (declarationOfAccidentWorker.WWorkTime)
                {
                    case 1:
                        wWorkTime1DB = "1";
                        break;
                    case 2:
                        wWorkTime2DB = "1";
                        break;
                }

                workerHireDateDB = (declarationOfAccidentWorker.WHireDate.HasValue ? CommonFunctions.FormatDate(declarationOfAccidentWorker.WHireDate.ToString()) : CommonFunctions.Replicate("&nbsp;", 10)) + " г.";
                workerJobTitleDB = declarationOfAccidentWorker.WJobTitle.ToString();
                workerJobCodeDB = declarationOfAccidentWorker.WJobCode.ToString();

                switch (declarationOfAccidentWorker.WJobCategory)
                {
                    case 1:
                        workerJobCategory1DB = "1";
                        break;
                    case 2:
                        workerJobCategory2DB = "1";
                        break;
                    case 3:
                        workerJobCategory3DB = "1";
                        break;
                }

                workerYearsOnServiceDB = declarationOfAccidentWorker.WYearsOnService.ToString();
                workerCurrentJobYearsOnServiceDB = declarationOfAccidentWorker.WCurrentJobYearsOnService.ToString();
                workerBranchDB = declarationOfAccidentWorker.WBranch.ToString();
            }

            string accidentTimeHourDB = "";
            string accidentTimeMinutesDB = "";
            string accidentDateDB = "";
            string accidentWorkFromHour1ValueDB = "";
            string accidentWorkFromMin1ValueDB = "";
            string accidentWorkToHour1ValueDB = "";
            string accidentWorkToMin1ValueDB = "";
            string accidentWorkFromHour2ValueDB = "";
            string accidentWorkFromMin2ValueDB = "";
            string accidentWorkToHour2ValueDB = "";
            string accidentWorkToMin2ValueDB = "";
            string accidentPlaceValueDB = "";
            string accidentCountryValueDB = "";
            string accidentRegionValueDB = "";
            string accidentMunicipalityValueDB = "";
            string accidentCityValueDB = "";
            string accidentPostCodeValueDB = "";
            string accidentStreetValueDB = "";
            string accidentStreetNumberValueDB = "";
            string accidentDistrictValueDB = "";
            string accidentBlockValueDB = "";
            string accidentEntranceValueDB = "";
            string accidentFloorValueDB = "";
            string accidentAptValueDB = "";
            string accidentPhoneValueDB = "";
            string accidentFaxValueDB = "";
            string accidentEmailValueDB = "";
            string accHappenedAt1DB = "";
            string accHappenedAt2DB = "";
            string accHappenedAt3DB = "";
            string accHappenedAtOtherDB = "";
            string accidentJobTypeValueDB = "";
            string accidentTaskTypeValueDB = "";
            string deviationFromTaskValueDB = "";
            string accidentInjurDescValueDB = "";
            string accidentInjHasRights1DB = "";
            string accidentInjHasRights2DB = "";
            string accidentInjHasRights3DB = "";
            string accidentLegalRef1DB = "";
            string accidentLegalRef2DB = "";
            string accidentPlannedActionsValueDB = "";


            DeclarationOfAccidentAcc declarationOfAccidentAcc = DeclarationOfAccidentUtil.GetDeclarationOfAccident(accidentDeclarationId, CurrentUser, "btnTabAcc").DeclarationOfAccidentAcc;
            if (declarationOfAccidentAcc != null)
            {
                if (declarationOfAccidentAcc.AccDateTime.HasValue)
                {
                    accidentTimeHourDB = declarationOfAccidentAcc.AccDateTime.Value.Hour.ToString();
                    accidentTimeMinutesDB = declarationOfAccidentAcc.AccDateTime.Value.Minute.ToString();
                    accidentDateDB = (declarationOfAccidentAcc.AccDateTime.HasValue ? CommonFunctions.FormatDate(declarationOfAccidentAcc.AccDateTime.ToString()) : CommonFunctions.Replicate("&nbsp;", 10)) + " г.";
                }

                accidentWorkFromHour1ValueDB = declarationOfAccidentAcc.AccWorkFromHour1.ToString();
                accidentWorkFromMin1ValueDB = declarationOfAccidentAcc.AccWorkFromMin1.ToString();
                accidentWorkToHour1ValueDB = declarationOfAccidentAcc.AccWorkToHour1.ToString();
                accidentWorkToMin1ValueDB = declarationOfAccidentAcc.AccWorkToMin1.ToString();
                accidentWorkFromHour2ValueDB = declarationOfAccidentAcc.AccWorkFromHour2.ToString();
                accidentWorkFromMin2ValueDB = declarationOfAccidentAcc.AccWorkFromMin2.ToString();
                accidentWorkToHour2ValueDB = declarationOfAccidentAcc.AccWorkToHour2.ToString();
                accidentWorkToMin2ValueDB = declarationOfAccidentAcc.AccWorkToMin2.ToString();
                accidentPlaceValueDB = declarationOfAccidentAcc.AccPlace.ToString();
                accidentCountryValueDB = declarationOfAccidentAcc.AccCountry.ToString();

                if (declarationOfAccidentAcc.City != null && declarationOfAccidentAcc.City.CityId > 0)
                {
                    List<int> listResultAcc = DeclarationOfAccidentUtil.GetListForEmployerCityId(declarationOfAccidentAcc.City.CityId, CurrentUser);
                    accidentRegionValueDB = RegionUtil.GetRegion(listResultAcc[0], CurrentUser).RegionName.ToString();
                    accidentMunicipalityValueDB = MunicipalityUtil.GetMunicipality(listResultAcc[1], CurrentUser).MunicipalityName.ToString();
                    accidentCityValueDB = declarationOfAccidentAcc.City.CityName;
                    accidentPostCodeValueDB = listResultAcc[2].ToString();
                }

                accidentStreetValueDB = declarationOfAccidentAcc.AccStreet.ToString();
                accidentStreetNumberValueDB = declarationOfAccidentAcc.AccStreetNum.ToString();

                accidentDistrictValueDB = declarationOfAccidentAcc.AccDistrict.ToString();
                accidentBlockValueDB = declarationOfAccidentAcc.AccBlock.ToString();
                accidentEntranceValueDB = declarationOfAccidentAcc.AccEntrance.ToString();
                accidentFloorValueDB = declarationOfAccidentAcc.AccFloor.ToString();
                accidentAptValueDB = declarationOfAccidentAcc.AccApt.ToString();
                accidentPhoneValueDB = declarationOfAccidentAcc.AccPhone.ToString();
                accidentFaxValueDB = declarationOfAccidentAcc.AccFax.ToString();
                accidentEmailValueDB = declarationOfAccidentAcc.AccEmail.ToString();

                switch (declarationOfAccidentAcc.AccHappenedAt)
                {
                    case 1:
                        accHappenedAt1DB = "1";
                        break;
                    case 2:
                        accHappenedAt2DB = "1";
                        break;
                    case 3:
                        accHappenedAt3DB = "1";
                        accHappenedAtOtherDB = declarationOfAccidentAcc.AccHappenedOther.ToString();
                        break;
                }

                accidentJobTypeValueDB = declarationOfAccidentAcc.AccJobType.ToString();
                accidentTaskTypeValueDB = CommonFunctions.ReplaceNewLinesInString(declarationOfAccidentAcc.AccTaskType.ToString());
                deviationFromTaskValueDB = CommonFunctions.ReplaceNewLinesInString(declarationOfAccidentAcc.AccDeviationFromTask.ToString());
                accidentInjurDescValueDB = CommonFunctions.ReplaceNewLinesInString(declarationOfAccidentAcc.AccInjurDesc.ToString());

                switch (declarationOfAccidentAcc.AccInjHasRights)
                {
                    case 1:
                        accidentInjHasRights1DB = "1";
                        break;
                    case 2:
                        accidentInjHasRights2DB = "1";
                        break;
                    case 3:
                        accidentInjHasRights3DB = "1";
                        break;
                }

                switch (declarationOfAccidentAcc.AccLegalRef)
                {
                    case 1:
                        accidentLegalRef1DB = "1";
                        break;
                    case 2:
                        accidentLegalRef2DB = "";
                        break;
                }

                accidentPlannedActionsValueDB = declarationOfAccidentAcc.AccPlannedActions.ToString();
            }

            string harmTypeDB = "";
            string harmBodyPartsDB = "";
            string harmResult1DB = "";
            string harmResult2DB = "";
            string harmResult3DB = "";

            DeclarationOfAccidentHarm declarationOfAccidentHarm = DeclarationOfAccidentUtil.GetDeclarationOfAccident(accidentDeclarationId, CurrentUser, "btnTabHarm").DeclarationOfAccidentHarm;
            if (declarationOfAccidentHarm != null)
            {
                harmTypeDB = CommonFunctions.ReplaceNewLinesInString(declarationOfAccidentHarm.HarmType.ToString());
                harmBodyPartsDB = CommonFunctions.ReplaceNewLinesInString(declarationOfAccidentHarm.HarmBodyParts.ToString());

                switch (declarationOfAccidentHarm.HarmResult)
                {
                    case 1:
                        harmResult1DB = "1";
                        break;
                    case 2:
                        harmResult2DB = "1";
                        break;
                    case 3:
                        harmResult3DB = "1";
                        break;
                }
            }

            string witnessDataDB = GenerateWitnessData(accidentDeclarationId);


            string applicantType1DB = "";
            string applicantType2DB = "";
            string applicantType3DB = "";
            string aplicantType2PositionDB = "";
            string aplicantType2NameDB = "";

            DeclarationOfAccidentFooter declarationOfAccidentFooter = declarationOfAccident.DeclarationOfAccidentFooter;
            if (declarationOfAccidentFooter != null)
            {
                switch (declarationOfAccidentFooter.ApplicantType)
                {
                    case 1:
                        applicantType2DB = "1";
                        aplicantType2PositionDB = declarationOfAccidentFooter.AplicantPosition;
                        aplicantType2NameDB = declarationOfAccidentFooter.AplicantName;
                        break;
                    case 2:
                        applicantType1DB = "1";
                        break;
                    case 3:
                        applicantType3DB = "1";
                        break;
                }
            }

            string heirFullNameDB = "";
            string heirEgnDB = "";
            string heirRegionDB = "";
            string heirMunicipalityDB = "";
            string heirCityDB = "";
            string heirPostCodeDB = "";
            string heirStreetDB = "";
            string heirStreetNumberDB = "";
            string heirDistrictDB = "";
            string heirBlockDB = "";
            string heirEntranceDB = "";
            string heirFloorDB = "";
            string heirAptDB = "";
            string heirPhoneDB = "";

            DeclarationOfAccidentHeir declarationOfAccidentHeir = DeclarationOfAccidentUtil.GetDeclarationOfAccident(accidentDeclarationId, CurrentUser, "btnTabHeir").DeclarationOfAccidentHeir;
            if (declarationOfAccidentHeir != null)
            {
                heirFullNameDB = declarationOfAccidentHeir.HeirFullName.ToString();
                heirEgnDB = declarationOfAccidentHeir.HeirEgn.ToString();

                if (declarationOfAccidentHeir.City != null && declarationOfAccidentHeir.City.CityId > 0)
                {
                    List<int> listResultHeir = DeclarationOfAccidentUtil.GetListForEmployerCityId(declarationOfAccidentHeir.City.CityId, CurrentUser);
                    heirRegionDB = RegionUtil.GetRegion(listResultHeir[0], CurrentUser).RegionName.ToString();
                    heirMunicipalityDB = MunicipalityUtil.GetMunicipality(listResultHeir[1], CurrentUser).MunicipalityName.ToString();
                    heirCityDB = declarationOfAccidentHeir.City.CityName;
                    heirPostCodeDB = listResultHeir[2].ToString();
                }

                heirStreetDB = declarationOfAccidentHeir.HeirStreet.ToString();
                heirStreetNumberDB = declarationOfAccidentHeir.HeirStreetNum.ToString();
                heirDistrictDB = declarationOfAccidentHeir.HeirDistrict.ToString();
                heirBlockDB = declarationOfAccidentHeir.HeirBlock.ToString();
                heirEntranceDB = declarationOfAccidentHeir.HeirEntrance.ToString();
                heirFloorDB = declarationOfAccidentHeir.HeirFloor.ToString();
                heirAptDB = declarationOfAccidentHeir.HeirApt.ToString();
                heirPhoneDB = declarationOfAccidentHeir.HeirPhone.ToString();
            }


            //Setup UIItems
            // Visibility of Header fields
            bool isDeclarNumberHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_DECLARATIONNUMBER") == UIAccessLevel.Hidden;
            if (isDeclarNumberHidden)
            {
                declarNumberDB = "";
            }

            bool isDeclarDateHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_DECLARATIONDATE") == UIAccessLevel.Hidden;
            if (isDeclarDateHidden)
            {
                declarDateFromDB = "";
            }

            bool isReferNumberHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_REFERENCENUMBER") == UIAccessLevel.Hidden;
            if (isReferNumberHidden)
            {
                referNumberDB = "";
            }
            bool isReferDateHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_REFERENCEDATE") == UIAccessLevel.Hidden;
            if (isReferDateHidden)
            {
                referDateFromDB = "";
            }

            bool isFileNumberHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_FILENUMBER") == UIAccessLevel.Hidden;
            if (isFileNumberHidden)
            {
                fileNumberDB = "";
            }

            // Visibility of Aplicant fields
            bool isAplicantTypeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_APLICANTTYPE") == UIAccessLevel.Hidden;
            if (isAplicantTypeHidden)
            {
                applicantType1DB = "";
                applicantType2DB = "";
                applicantType3DB = "";
                aplicantType2NameDB = "";
                aplicantType2PositionDB = "";
            }

            // Visibility of Employer tab
            bool isEmployerTabHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP") == UIAccessLevel.Hidden;
            
            bool isEmplIdHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLOYERID") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplIdHidden)
            {
                emplNameDB = "";
            }

            bool isEmplEikHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLEIK") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplEikHidden)
            {
                emplEikDB = "";
            }

            bool isCityIdHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLCITYID") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isCityIdHidden)
            {
                emplRegionDB = "";
                emplMunicipalityDB = "";
                emplCityDB = "";
                emplPostCodeDB = "";
            }

            bool isEmplStreetHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLSTREET") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplStreetHidden)
            {
                emplStreetDB = "";
            }

            bool isEmplStreetNumHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLSTREETNUM") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplStreetNumHidden)
            {
                emplStreetNumberDB = "";
            }

            bool isEmplDistrictHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLDISTRICT") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplDistrictHidden)
            {
                emplDistrictDB = "";
            }

            bool isEmplBlockHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLBLOCK") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplBlockHidden)
            {
                emplBlockDB = "";
            }

            bool isEmplEntranceHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLENTRANCE") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplEntranceHidden)
            {
                emplEntranceDB = "";
            }

            bool isEmplFloorHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLFLOOR") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplFloorHidden)
            {
                emplFloorDB = "";
            }

            bool isEmplAptHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLAPT") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplAptHidden)
            {
                emplAptDB = "";
            }

            bool isEmplPhoneHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLPHONE") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplPhoneHidden)
            {
                emplPhoneDB = "";
            }

            bool isEmplFaxHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLFAX") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplFaxHidden)
            {
                emplFaxDB = "";
            }

            bool isEmplEmailHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLEMAIL") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplEmailHidden)
            {
                emplEmailDB = "";
            }

            bool isEmplNumberOfEmplHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLNUMBEROFEMPLOYEES") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplNumberOfEmplHidden)
            {
                emplNumberOfEmployeesDB = "";
            }

            bool isEmplFemaleEmplHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_EMLP_EMPLFEMALEEMPLOYEES") == UIAccessLevel.Hidden;
            if (isEmployerTabHidden || isEmplFemaleEmplHidden)
            {
                emplFemaleEmployeesDB = "";
            }
            

            // Visibility of Worker tab
            bool isWorkerTabHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER") == UIAccessLevel.Hidden;
            
            bool isWorkerFullNameHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WORKERFULLNAME") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerFullNameHidden)
            {
                workerFullNameDB = "";
            }

            bool isWorkerEgnHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WORKEREGN") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerEgnHidden)
            {
                workerEgnDB = "";
            }

            bool isWorkerCityHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WCITYID") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerCityHidden)
            {
                workerRegionDB = "";
                workerMunicipalityDB = "";
                workerCityDB = "";
                workerPostCodeDB = "";
            }

            bool isWorkerStreetHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WSTREET") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerStreetHidden)
            {
                workerStreetDB = "";
            }

            bool isWorkerStreetNumHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WSTREETNUM") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerStreetNumHidden)
            {
                workerStreetNumberDB = "";
            }

            bool isWorkerDistrictHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WDISTRICT") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerDistrictHidden)
            {
                workerDistrictDB = "";
            }

            bool isWorkerBlockHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WBLOCK") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerBlockHidden)
            {
                workerBlockDB = "";
            }

            bool isWorkerEntranceHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WENTRANCE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerEntranceHidden)
            {
                workerEntranceDB = "";
            }

            bool isWorkerFloorHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WFLOOR") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerFloorHidden)
            {
                workerFloorDB = "";
            }

            bool isWorkerAptHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WAPT") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerAptHidden)
            {
                workerAptDB = "";
            }

            bool isWorkerPhoneHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WPHONE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerPhoneHidden)
            {
                workerPhoneDB = "";
            }

            bool isWorkerFaxHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WFAX") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerFaxHidden)
            {
                workerFaxDB = "";
            }

            bool isWorkerEmailHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WEMAIL") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerEmailHidden)
            {
                workerEmailDB = "";
            }

            bool isWorkerBirthDateHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WBIRTHDATE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerBirthDateHidden)
            {
                workerBirthDateDB = "";
            }

            bool isWorkerGenderHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WGENDER") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerGenderHidden)
            {
                wGenderMaleDB = "";
                wGenderFemaleDB = "";
            }

            bool isWorkerCitizenshipHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WCITIZENSHIP") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerCitizenshipHidden)
            {
                workerCitizenshipDB = "";
            }

            bool isWorkerHireTypeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WHIRETYPE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerHireTypeHidden)
            {
                wHireType1DB = "";
                wHireType2DB = "";
            }

            bool isWorkerWorkTimeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WWORKTIME") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerWorkTimeHidden)
            {
                wWorkTime1DB = "";
                wWorkTime2DB = "";
            }

            bool isWorkerHireTimeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WHIREDATE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerHireTimeHidden)
            {
                workerHireDateDB = "";
            }

            bool isWorkerJobTitleHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WJOBTITLE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerJobTitleHidden)
            {
                workerJobTitleDB = "";
            }

            bool isWorkerJobCodeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WJOBCODE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerJobCodeHidden)
            {
                workerJobCodeDB = "";
            }

            bool isWorkerJobCategoryHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WJOBCATEGORY") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerJobCategoryHidden)
            {
                workerJobCategory1DB = "";
                workerJobCategory2DB = "";
                workerJobCategory3DB = "";
            }

            bool isWorkerYearsOnServiceHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WYEARSONSERVICE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerYearsOnServiceHidden)
            {
                workerYearsOnServiceDB = "";
            }

            bool isWorkerCurrJobYearsOnServiceHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WCURRJOBYEARSONSERVICE") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerCurrJobYearsOnServiceHidden)
            {
                workerCurrentJobYearsOnServiceDB = "";
            }

            bool isWorkerBranchHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WORKER_WBRANCH") == UIAccessLevel.Hidden;
            if (isWorkerTabHidden || isWorkerBranchHidden)
            {
                workerBranchDB = "";
            }
    

            // Visibility of Accident tab
            bool isAccidentHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC") == UIAccessLevel.Hidden;
            
            bool isAccidentTimeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCDATETIME") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentTimeHidden)
            {
                accidentTimeHourDB = "";
                accidentTimeMinutesDB = "";
                accidentDateDB = "";
            }

            bool isAccidentWorkFromHour1Hidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCWORKFROMHOUR1") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentWorkFromHour1Hidden)
            {
                accidentWorkFromHour1ValueDB = "";
            }

            bool isAccidentWorkFromMin1Hidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCWORKFROMMIN1") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentWorkFromMin1Hidden)
            {
                accidentWorkFromMin1ValueDB = "";
            }

            bool isAccidentWorkToHour1Hidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCWORKTOHOUR1") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentWorkToHour1Hidden)
            {
                accidentWorkToHour1ValueDB = "";
            }

            bool isAccidentWorkToMin1Hidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCWORKTOMIN1") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentWorkToMin1Hidden)
            {
                accidentWorkToMin1ValueDB = "";
            }

            bool isAccidentWorkFromHour2Hidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCWORKFROMHOUR2") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentWorkFromHour2Hidden)
            {
                accidentWorkFromHour2ValueDB = "";
            }

            bool isAccidentWorkFromMin2Hidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCWORKFROMMIN2") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentWorkFromMin2Hidden)
            {
                accidentWorkFromMin2ValueDB = "";
            }

            bool isAccidentWorkToHour2Hidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCWORKTOHOUR2") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentWorkToHour2Hidden)
            {
                accidentWorkToHour2ValueDB = "";
            }

            bool isAccidentWorkToMin2Hidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCWORKTOMIN2") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentWorkToMin2Hidden)
            {
                accidentWorkToMin2ValueDB = "";
            }

            bool isAccidentPlaceHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCPLACE") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentPlaceHidden)
            {
                accidentPlaceValueDB = "";
            }

            bool isAccidentCountryHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCCOUNTRY") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentCountryHidden)
            {
                accidentCountryValueDB = "";
            }

            bool isAccidentCityHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCCITYID") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentCityHidden)
            {
                accidentRegionValueDB = "";
                accidentMunicipalityValueDB = "";
                accidentCityValueDB = "";
                accidentPostCodeValueDB = "";
            }

            bool isAccidentStreetHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCSTREET") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentStreetHidden)
            {
                accidentStreetValueDB = "";
            }

            bool isAccidentStreetNumberHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCSTREETNUM") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentStreetNumberHidden)
            {
                accidentStreetNumberValueDB = "";
            }

            bool isAccidentStreetNumHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCSTREETNUM") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentStreetNumHidden)
            {
                accidentStreetNumberValueDB = "";
            }

            bool isAccidentDistrictHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCDISTRICT") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentDistrictHidden)
            {
                accidentDistrictValueDB = "";
            }

            bool isAccidentBlockHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCBLOCK") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentBlockHidden)
            {
                accidentBlockValueDB = "";
            }

            bool isAccidentEntranceHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCENTRANCE") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentEntranceHidden)
            {
                accidentEntranceValueDB = "";
            }

            bool isAccidentFloorHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCFLOOR") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentFloorHidden)
            {
                accidentFloorValueDB = "";
            }

            bool isAccidentAptHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCAPT") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentAptHidden)
            {
                accidentAptValueDB = "";
            }

            bool isAccidentPhoneHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCPHONE") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentPhoneHidden)
            {
                accidentPhoneValueDB = "";
            }

            bool isAccidentFaxHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCFAX") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentFaxHidden)
            {
                accidentFaxValueDB = "";
            }

            bool isAccidentEmailHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCEMAIL") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentEmailHidden)
            {
                accidentEmailValueDB = "";
            }

            bool isAccidentHappendAtHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCHAPPENEDAT") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentHappendAtHidden)
            {
                accHappenedAt1DB = "";
                accHappenedAt2DB = "";
                accHappenedAt3DB = "";
                accHappenedAtOtherDB = "";
            }

            bool isAccidentJobTypeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCJOBTYPE") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentJobTypeHidden)
            {
                accidentJobTypeValueDB = "";
            }

            bool isAccidentTaskTypeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCTASKTYPE") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentTaskTypeHidden)
            {
                accidentTaskTypeValueDB = "";
            }

            bool isDeviationFromTaskHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCDEVIATIONFROMTASK") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isDeviationFromTaskHidden)
            {
                deviationFromTaskValueDB = "";
            }

            bool isAccidentInjurDescHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCINJURDESC") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentInjurDescHidden)
            {
                accidentInjurDescValueDB = "";
            }

            bool isAccidentInjHasRightsHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCINJHASRIGHTS") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentInjHasRightsHidden)
            {
                accidentInjHasRights1DB = "";
                accidentInjHasRights2DB = "";
                accidentInjHasRights3DB = "";
            }

            bool isAccidentLegalRefHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCLEGALREF") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentLegalRefHidden)
            {
                accidentLegalRef1DB = "";
                accidentLegalRef2DB = "";
            }

            bool isAccidentPlannedActionsHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_ACC_ACCPLANNEDACTIONS") == UIAccessLevel.Hidden;
            if (isAccidentHidden || isAccidentPlannedActionsHidden)
            {
                accidentPlannedActionsValueDB = "";
            }

            // Visibility of Harm tab
            bool isHarmHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HARM") == UIAccessLevel.Hidden;
            
            bool isHarmTypeHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HARM_HARMTYPE") == UIAccessLevel.Hidden;
            if (isHarmHidden || isHarmTypeHidden)
            {
                harmTypeDB = "";
            }

            bool isHarmBodyPartsHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HARM_HARMBODYPARTS") == UIAccessLevel.Hidden;
            if (isHarmHidden || isHarmBodyPartsHidden)
            {
                harmBodyPartsDB = "";
            }

            bool isHarmResultHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HARM_HARMRESULT") == UIAccessLevel.Hidden;
            if (isHarmHidden || isHarmResultHidden)
            {
                harmResult1DB = "";
                harmResult2DB = "";
                harmResult3DB = "";
            }


            // Visibility of Heir tab
            bool isHeirHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR") == UIAccessLevel.Hidden;
            
            bool isHeirFullNameHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRFULLNAME") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirFullNameHidden)
            {
                heirFullNameDB = "";
            }

            bool isHeirEgnHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIREGN") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirEgnHidden)
            {
                heirEgnDB = "";
            }

            bool isHeirCityHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRCITYID") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirCityHidden)
            {
                heirRegionDB = "";
                heirMunicipalityDB = "";
                heirCityDB = "";
                heirPostCodeDB = "";
            }

            bool isHeirStreetHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRSTREET") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirStreetHidden)
            {
                heirStreetDB = "";
            }

            bool isHeirStreetNumHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRSTREETNUM") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirStreetNumHidden)
            {
                heirStreetNumberDB = "";
            }

            bool isHeirDistrictHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRDISTRICT") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirDistrictHidden)
            {
                heirDistrictDB = "";
            }

            bool isHeirBlockHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRBLOCK") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirBlockHidden)
            {
                heirBlockDB = "";
            }

            bool isHeirEntranceHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRENTRANCE") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirEntranceHidden)
            {
                heirEntranceDB = "";
            }

            bool isHeirFloorHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRFLOOR") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirFloorHidden)
            {
                heirFloorDB = "";
            }

            bool isHeirAptHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRAPT") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirAptHidden)
            {
                heirAptDB = "";
            }

            bool isHeirPhoneHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_HEIR_HEIRPHONE") == UIAccessLevel.Hidden;
            if (isHeirHidden || isHeirPhoneHidden)
            {
                heirPhoneDB = "";
            }




            string padChar = "&nbsp;";

            string declarNumberValue = CommonFunctions.PadRight(declarNumberDB, padChar, 10);
            string declarDateFromValue = CommonFunctions.PadRight(declarDateFromDB, padChar, 0);
            string referNumberValue = CommonFunctions.PadRight(referNumberDB, padChar, 20);
            string referDateFromValue = CommonFunctions.PadRight(referDateFromDB, padChar, 0);
            string fileNumberValue = CommonFunctions.PadRight(fileNumberDB, padChar, 0);

            string emplNameValue = CommonFunctions.PadRight(emplNameDB, padChar, 0);
            string emplEikValue = CommonFunctions.PadRight(emplEikDB, padChar, 0);
            string emplRegionValue = CommonFunctions.PadRight(emplRegionDB, padChar, 30);
            string emplMunicipalityValue = CommonFunctions.PadRight(emplMunicipalityDB, padChar, 20);
            string emplCityValue = CommonFunctions.PadRight(emplCityDB, padChar, 20);
            string emplPostCodeValue = CommonFunctions.PadRight(emplPostCodeDB, padChar, 20);
            string emplStreetValue = CommonFunctions.PadRight(emplStreetDB, padChar, 40);
            string emplStreetNumberValue = CommonFunctions.PadRight(emplStreetNumberDB, padChar, 0);
            string emplDistrictValue = CommonFunctions.PadRight(emplDistrictDB, padChar, 22);
            string emplBlockValue = CommonFunctions.PadRight(emplBlockDB, padChar, 4);
            string emplEntranceValue = CommonFunctions.PadRight(emplEntranceDB, padChar, 4);
            string emplFloorValue = CommonFunctions.PadRight(emplFloorDB, padChar, 4);
            string emplAptValue = CommonFunctions.PadRight(emplAptDB, padChar, 4);
            string emplPhoneValue = CommonFunctions.PadRight(emplPhoneDB, padChar, 21);
            string emplFaxValue = CommonFunctions.PadRight(emplFaxDB, padChar, 15);
            string emplEmailValue = CommonFunctions.PadRight(emplEmailDB, padChar, 0);
            string emplNumberOfEmployeesValue = CommonFunctions.PadRight(emplNumberOfEmployeesDB, padChar, 5);
            string emplFemaleEmployeesValue = CommonFunctions.PadRight(emplFemaleEmployeesDB, padChar, 5);

            string workerFullNameValue = CommonFunctions.PadRight(workerFullNameDB, padChar, 35);
            string workerEgnValue = CommonFunctions.PadRight(workerEgnDB, padChar, 0);
            string workerRegionValue = CommonFunctions.PadRight(workerRegionDB, padChar, 24);
            string workerMunicipalityValue = CommonFunctions.PadRight(workerMunicipalityDB, padChar, 0);
            string workerCityValue = CommonFunctions.PadRight(workerCityDB, padChar, 20);
            string workerPostCodeValue = CommonFunctions.PadRight(workerPostCodeDB, padChar, 0);
            string workerStreetValue = CommonFunctions.PadRight(workerStreetDB, padChar, 0);
            string workerStreetNumberValue = CommonFunctions.PadRight(workerStreetNumberDB, padChar, 0);
            string workerDistrictValue = CommonFunctions.PadRight(workerDistrictDB, padChar, 23);
            string workerBlockValue = CommonFunctions.PadRight(workerBlockDB, padChar, 4);
            string workerEntranceValue = CommonFunctions.PadRight(workerEntranceDB, padChar, 4);
            string workerFloorValue = CommonFunctions.PadRight(workerFloorDB, padChar, 4);
            string workerAptValue = CommonFunctions.PadRight(workerAptDB, padChar, 4);
            string workerPhoneValue = CommonFunctions.PadRight(workerPhoneDB, padChar, 21);
            string workerFaxValue = CommonFunctions.PadRight(workerFaxDB, padChar, 15);
            string workerEmailValue = CommonFunctions.PadRight(workerEmailDB, padChar, 0);
            string workerBirthDateValue = CommonFunctions.PadRight(workerBirthDateDB, padChar, 0);
            string wGenderMaleValue = (wGenderMaleDB == "1" ? filledCheckbox : emptyCheckbox);
            string wGenderFemaleValue = (wGenderFemaleDB == "1" ? filledCheckbox : emptyCheckbox);
            string workerCitizenshipValue = CommonFunctions.PadRight(workerCitizenshipDB, padChar, 0);
            string wHireType1Value = (wHireType1DB == "1" ? filledCheckbox : emptyCheckbox);
            string wHireType2Value = (wHireType2DB == "1" ? filledCheckbox : emptyCheckbox);
            string wWorkTime1Value = (wWorkTime1DB == "1" ? filledCheckbox : emptyCheckbox);
            string wWorkTime2Value = (wWorkTime2DB == "1" ? filledCheckbox : emptyCheckbox);
            string workerHireDateValue = CommonFunctions.PadRight(workerHireDateDB, padChar, 0);
            string workerJobTitleValue = CommonFunctions.PadRight(workerJobTitleDB, padChar, 30);
            string workerJobCodeValue = CommonFunctions.PadRight(workerJobCodeDB, padChar, 0);
            string workerJobCategory1Value = (workerJobCategory1DB == "1" ? filledCheckbox : emptyCheckbox);
            string workerJobCategory2Value = (workerJobCategory2DB == "1" ? filledCheckbox : emptyCheckbox);
            string workerJobCategory3Value = (workerJobCategory3DB == "1" ? filledCheckbox : emptyCheckbox);
            string workerYearsOnServiceValue = CommonFunctions.PadRight(workerYearsOnServiceDB, padChar, 0);
            string workerCurrentJobYearsOnServiceValue = CommonFunctions.PadRight(workerCurrentJobYearsOnServiceDB, padChar, 0);
            string workerBranchValue = CommonFunctions.PadRight(workerBranchDB, padChar, 0);


            string accidentTimeHourValue = CommonFunctions.PadRight(accidentTimeHourDB, padChar, 2);
            string accidentTimeMinutesValue = CommonFunctions.PadRight(accidentTimeMinutesDB, padChar, 2);
            string accidentDateValue = CommonFunctions.PadRight(accidentDateDB, padChar, 0);
            string accidentWorkFromHour1ValueValue = CommonFunctions.PadRight(accidentWorkFromHour1ValueDB, padChar, 2);
            string accidentWorkFromMin1ValueValue = CommonFunctions.PadRight(accidentWorkFromMin1ValueDB, padChar, 2);
            string accidentWorkToHour1ValueValue = CommonFunctions.PadRight(accidentWorkToHour1ValueDB, padChar, 2);
            string accidentWorkToMin1ValueValue = CommonFunctions.PadRight(accidentWorkToMin1ValueDB, padChar, 2);
            string accidentWorkFromHour2ValueValue = CommonFunctions.PadRight(accidentWorkFromHour2ValueDB, padChar, 2);
            string accidentWorkFromMin2ValueValue = CommonFunctions.PadRight(accidentWorkFromMin2ValueDB, padChar, 2);
            string accidentWorkToHour2ValueValue = CommonFunctions.PadRight(accidentWorkToHour2ValueDB, padChar, 2);
            string accidentWorkToMin2ValueValue = CommonFunctions.PadRight(accidentWorkToMin2ValueDB, padChar, 2);
            string accidentPlaceValueValue = CommonFunctions.PadRight(accidentPlaceValueDB, padChar, 0);
            string accidentCountryValueValue = CommonFunctions.PadRight(accidentCountryValueDB, padChar, 22);
            string accidentRegionValueValue = CommonFunctions.PadRight(accidentRegionValueDB, padChar, 25);
            string accidentMunicipalityValueValue = CommonFunctions.PadRight(accidentMunicipalityValueDB, padChar, 0);
            string accidentCityValueValue = CommonFunctions.PadRight(accidentCityValueDB, padChar, 25);
            string accidentPostCodeValueValue = CommonFunctions.PadRight(accidentPostCodeValueDB, padChar, 0);
            string accidentStreetValueValue = CommonFunctions.PadRight(accidentStreetValueDB, padChar, 35);
            string accidentStreetNumberValueValue = CommonFunctions.PadRight(accidentStreetNumberValueDB, padChar, 0);
            string accidentDistrictValueValue = CommonFunctions.PadRight(accidentDistrictValueDB, padChar, 27);
            string accidentBlockValueValue = CommonFunctions.PadRight(accidentBlockValueDB, padChar, 4);
            string accidentEntranceValueValue = CommonFunctions.PadRight(accidentEntranceValueDB, padChar, 4);
            string accidentFloorValueValue = CommonFunctions.PadRight(accidentFloorValueDB, padChar, 4);
            string accidentAptValueValue = CommonFunctions.PadRight(accidentAptValueDB, padChar, 4);
            string accidentPhoneValueValue = CommonFunctions.PadRight(accidentPhoneValueDB, padChar, 26);
            string accidentFaxValueValue = CommonFunctions.PadRight(accidentFaxValueDB, padChar, 16);
            string accidentEmailValueValue = CommonFunctions.PadRight(accidentEmailValueDB, padChar, 0);
            string accHappenedAt1Value = (accHappenedAt1DB == "1" ? filledCheckbox : emptyCheckbox);
            string accHappenedAt2Value = (accHappenedAt2DB == "1" ? filledCheckbox : emptyCheckbox);
            string accHappenedAt3Value = (accHappenedAt3DB == "1" ? filledCheckbox : emptyCheckbox);
            string accHappenedAtOtherValue = CommonFunctions.PadRight(accHappenedAtOtherDB, padChar, 0);
            string accidentJobTypeValueValue = CommonFunctions.PadRight(accidentJobTypeValueDB, padChar, 0);

            string newLine = "<br />";

            int newLines = HowManyLines(accidentTaskTypeValueDB, newLine, 0, 90);
            newLines = (newLines > 3 ? 3 : newLines);
            string accidentTaskTypeValueValue = accidentTaskTypeValueDB + CommonFunctions.Replicate(newLine + "&nbsp;", 3 - newLines);

            newLines = HowManyLines(deviationFromTaskValueDB, newLine, 0, 90);
            newLines = (newLines > 3 ? 3 : newLines);
            string deviationFromTaskValueValue = deviationFromTaskValueDB + CommonFunctions.Replicate(newLine + "&nbsp;", 3 - newLines);

            newLines = HowManyLines(accidentInjurDescValueDB, newLine, 0, 90);
            newLines = (newLines > 3 ? 3 : newLines);
            string accidentInjurDescValueValue = accidentInjurDescValueDB + CommonFunctions.Replicate(newLine + "&nbsp;", 3 - newLines);

            string accidentInjHasRights1Value = (accidentInjHasRights1DB == "1" ? filledCheckbox : emptyCheckbox);
            string accidentInjHasRights2Value = (accidentInjHasRights2DB == "1" ? filledCheckbox : emptyCheckbox);
            string accidentInjHasRights3Value = (accidentInjHasRights3DB == "1" ? filledCheckbox : emptyCheckbox);
            string accidentLegalRef1Value = (accidentLegalRef1DB == "1" ? filledCheckbox : emptyCheckbox);
            string accidentLegalRef2Value = (accidentLegalRef2DB == "1" ? filledCheckbox : emptyCheckbox);
            string accidentPlannedActionsValueValue = CommonFunctions.PadRight(accidentPlannedActionsValueDB, padChar, 150);

            string harmTypeValue = CommonFunctions.PadRight(harmTypeDB, padChar, 0);
            string harmBodyPartsValue = CommonFunctions.PadRight(harmBodyPartsDB, padChar, 0);
            string harmResult1Value = (harmResult1DB == "1" ? filledCheckbox : emptyCheckbox);
            string harmResult2Value = (harmResult2DB == "1" ? filledCheckbox : emptyCheckbox);
            string harmResult3Value = (harmResult3DB == "1" ? filledCheckbox : emptyCheckbox);

            string witnessDataValue = CommonFunctions.PadRight(witnessDataDB, padChar, 0);

            string aplicantType2PositionValue = CommonFunctions.PadRight(aplicantType2PositionDB, padChar, 0);
            string aplicantType2NameValue = CommonFunctions.PadRight(aplicantType2NameDB, padChar, 0);
            string applicantType1Value = (applicantType1DB == "1" ? filledCheckbox : emptyCheckbox);
            string applicantType2Value = (applicantType2DB == "1" ? filledCheckbox : emptyCheckbox);
            string applicantType3Value = (applicantType3DB == "1" ? filledCheckbox : emptyCheckbox);


            string heirFullNameValue = CommonFunctions.PadRight(heirFullNameDB, padChar, 45);
            string heirEgnValue = CommonFunctions.PadRight(heirEgnDB, padChar, 0);
            string heirRegionValue = CommonFunctions.PadRight(heirRegionDB, padChar, 35);
            string heirMunicipalityValue = CommonFunctions.PadRight(heirMunicipalityDB, padChar, 0);
            string heirCityValue = CommonFunctions.PadRight(heirCityDB, padChar, 25);
            string heirPostCodeValue = CommonFunctions.PadRight(heirPostCodeDB, padChar, 0);
            string heirStreetValue = CommonFunctions.PadRight(heirStreetDB, padChar, 25);
            string heirStreetNumberValue = CommonFunctions.PadRight(heirStreetNumberDB, padChar, 4);
            string heirDistrictValue = CommonFunctions.PadRight(heirDistrictDB, padChar, 28);
            string heirBlockValue = CommonFunctions.PadRight(heirBlockDB, padChar, 4);
            string heirEntranceValue = CommonFunctions.PadRight(heirEntranceDB, padChar, 4);
            string heirFloorValue = CommonFunctions.PadRight(heirFloorDB, padChar, 4);
            string heirAptValue = CommonFunctions.PadRight(heirAptDB, padChar, 4);
            string heirPhoneValue = CommonFunctions.PadRight(heirPhoneDB, padChar, 0);


            string @html = "";

            if (exportToWord)
            {
                @html += @"<html xmlns:v=""urn:schemas-microsoft-com:vml""
xmlns:o=""urn:schemas-microsoft-com:office:office""
xmlns:w=""urn:schemas-microsoft-com:office:word"" 
charset=""UTF-8"" >
<head>
   <style>
     @page Section1
	 {   
         size:595.3pt 841.9pt;
	     margin:21.3pt 42.45pt 14.2pt 1.25in;
     }

     div.Section1
	 {page:Section1;}

      body
      {
         font-family: ""Times New Roman"";
         font-size: 10pt;
      }

      p
      {
         margin-top: 0px;
         margin-bottom: 0px;
      }     

      h1
	  {
         margin:0in;
	     margin-bottom:.0001pt;
	     text-align:center;
	     page-break-after:avoid;
	     font-size:9.0pt;
	     font-family:""Times New Roman"";
      }

      h2
	  {
         margin:0in;
	     margin-bottom:.0001pt;
	     text-align:center;
	     page-break-after:avoid;
	     font-size:14.0pt;
	     font-family:""Times New Roman"";
      }

      h3
	  {
         margin:0in;
	     margin-bottom:.0001pt;
	     text-align:justify;
	     page-break-after:avoid;
	     font-size:9.0pt;
	     font-family:""Times New Roman"";
      }

      h4
	  {
         margin:0in;
	     margin-bottom:.0001pt;
	     page-break-after:avoid;
	     font-size:12.0pt;
	     font-family:""Arial Narrow"";
	     font-weight:normal;
	     font-style:italic;
      }

      h5
	  {
         margin:0in;
	     margin-bottom:.0001pt;
	     text-align:justify;
	     page-break-after:avoid;
	     font-size:10.0pt;
	     font-family:""Times New Roman"";
      }

      h6
	  {
         margin:0in;
	     margin-bottom:.0001pt;
	     text-align:center;
	     text-indent:-49.65pt;
	     page-break-after:avoid;
	     font-size:9.0pt;
	     font-family:""Times New Roman"";
      }

      p.MsoBodyText3, li.MsoBodyText3, div.MsoBodyText3
	  {
         margin:0in;
	     margin-bottom:.0001pt;
	     text-align:justify;
	     font-size:8.0pt;
	     font-family:""Times New Roman"";
	     font-style:italic;
      }


      .FormValue
      {
         font-family: ""Courier New"";
         font-size: 9pt;
      }
   </style>

   <xml>
      <w:WordDocument>
         <w:View>Print</w:View>
         <w:Zoom>100</w:Zoom>
      </w:WordDocument>
   </xml>
</head>
<body>
<div class=""Section1"">
";
            }

            html += @"


<p style=""text-align: right;"">
   <span lang=BG style=""font-size: 8pt;"">Обр. О-11/2006/НОИ</span>
</p>";

            string tmpStyle = @"style=""width: 100%; border: solid 1px #000000; border-collapse:collapse;"" ";

            if (exportToWord)
            {
                tmpStyle = @" border=1 cellspacing=0 cellpadding=0 
  style=""width:517.45pt; margin-left:-44.25pt; border-collapse:collapse; border:none;"" width=""690"" ";
            }

            html += @"
<table " + tmpStyle + @" >
   <tr style=""height:49.55pt"">
      <td width=""321"" style=""width:241.05pt; border:solid 1px; padding:0in 5.4pt 0in 5.4pt;"">
         <p style='text-align:justify;'><span lang=BG style=""font-size:8.0pt"">&nbsp;</span></p>
         <p style='text-align:justify;'>
            <span lang=BG style=""font-size: 9.0pt"">Декларация<b>&nbsp;</b>№ <span class='FormValue'>" + declarNumberValue + @"</span> от <span class='FormValue'>" + declarDateFromValue + @"</span> </span>
         </p>
         <p style='text-align:justify;'>
            <i><span lang=BG style=""font-size:8.0pt"">" + CommonFunctions.Replicate("&nbsp;", exportToWord ? 70 : 60) + @"(дд мм гггг)</span></i>
         </p>
         <p style='text-align:justify;'>
            <i><span lang=BG style=""font-size:8.0pt"">(Попълва се номерът, под който декларацията е вписана в регистъра на осигурителя)</span></i>
         </p>
      </td>
      <td width=""369"" style=""width:276.4pt; border:solid 1px; padding:0in 5.4pt 0in 5.4pt;"">
         <p style='text-align:justify'>
            <span lang=BG style='font-size:3.0pt'>&nbsp;</span>
         </p>
         <p style='text-align:justify'>
            <b><span lang=BG style='font-size:9.0pt'>Информация, попълвана от служител в ТП на НОИ</span></b><span style='font-size:9.0pt'>:</span>
         </p>
         <p style='text-align:justify'>
            <span lang=BG style='font-size: 5.0pt;'>&nbsp;</span>
         </p>
         <p style='text-align:justify'>
            <span lang=BG style='font-size: 9.0pt;'>Входящ № <span class='FormValue'>" + referNumberValue + @"</span> от <span class='FormValue'>" + referDateFromValue + @"</span> </span>
         </p>
         <p style='text-align:justify'>
            <i><span lang=BG style='font-size:8.0pt'>" + CommonFunctions.Replicate("&nbsp;", exportToWord ? 90 : 80) + @"(дд мм гггг)</span></i>
         </p>
         <p style='text-align:justify'>
            <span lang=BG style='font-size: 9.0pt'>Досие №</span>
            <b><span lang=BG style='font-size:9.0pt'> </span></b>
            <span lang=BG style='font-size:9.0pt'><span class='FormValue'>" + fileNumberValue + @"</span></span>
         </p>
         <p style='text-align:justify'>
            <span lang=BG style='font-size: 3.0pt'>&nbsp;</span>
         </p>
      </td>
   </tr>
   <tr style='height:8.4pt'>
      <td width=690 colspan=2 valign=top style='width:517.45pt;
         border-top:none; padding:0in 5.4pt 0in 5.4pt; height:8.4pt;'>
         <p style='text-align:justify'>
           <b><span lang=BG style='font-size:1.0pt'>&nbsp;</span></b>
         </p>
         <p align=center style='text-align:center'>
           <b><i><span lang=BG style='font-size:1.0pt'>&nbsp;</span></i></b>
         </p>
         <p align=center style='text-align:center'>
           <b><i><span lang=BG style='font-size:10.0pt'>Попълването на всички полета в декларацията е задължително!</span></i></b>
         </p>
         <p align=center style='text-align:center'>
            <b><i><span lang=BG style='font-size:10.0pt'>Където има информация, дадена с квадратчета, вярното се отбелязва с “Х”.</span>
         </p>
      </td>
   </tr>
</table>

<p><span lang=BG style='font-size:6.0pt'>&nbsp;</span></p>

<h2 style='margin-left:42.55pt; text-indent: " + (exportToWord ? "-82.55pt" : "-42.55pt") + @"'>
   <span lang=BG >ДЕКЛАРАЦИЯ ЗА  ТРУДОВА  ЗЛОПОЛУКА</span>
</h2>

<p><span lang=BG style='font-size:1.0pt'>&nbsp;</span></p>

<p align=center style='margin-left:42.55pt;text-align:center; text-indent: " + (exportToWord ? "-82.55pt" : "-42.55pt") + @"'>
   <span lang=BG style='font-size:8.0pt'>Приложение към чл.3, ал.1 от Наредбата за установяване, разследване, регистриране и отчитане на трудовите злополуки</span>
</p>

<p style='text-align:justify'>
   <b><span lang=BG style='font-size:6.0pt'>&nbsp;</span></b>
</p>

<h1 style='margin-left:49.65pt;text-indent:" + (exportToWord ? "-89.65pt" : "-49.65pt") + @"'>
   <span lang=BG >I. ДАННИ ЗА ОСИГУРИТЕЛЯ</span>
</h1>
";

            tmpStyle = @"style=""width: 100%; border: solid 1px #000000; border-collapse:collapse;"" ";

            if (exportToWord)
            {
                tmpStyle = @" border=1 cellspacing=0 cellpadding=0 width=690
       style='width:517.45pt;margin-left:-44.25pt;border-collapse:collapse; border:none;' ";
            }

            html += @"
<table " + tmpStyle + @" >
  <tr>";

            tmpStyle = @" style='width:30px; border:solid windowtext 1.0pt; padding-left; 5px; text-align: left; vertical-align: top;' ";

            if (exportToWord)
            {
                tmpStyle = @" width=34 valign=top style='width:25.5pt;border:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt' ";
            }

            html += @"
      <td " + tmpStyle + @">
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:6.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG>1</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG>2</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG>3</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG>4</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:5.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG>5</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG style='font-size:1.0pt'>&nbsp;</span></b></p>
         <p align=center style='text-align:center'><b><span lang=BG>6</span></b></p>
      </td>
      <td width=658 valign=top style='width:493.35pt;border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt'>
        <p style='text-align:justify'><span lang=BG style='font-size: 6.0pt'>&nbsp;</span></p>
        <p style='text-align:justify'>
            <b><span lang=BG style='font-size:9.0pt'>  </span><span lang=BG style='font-size: 10.0pt'>Пълно наименование:</span></b><span lang=BG style='font-size:9.0pt'> <span class='FormValue'>" + emplNameValue + @"</span> </span>
        </p>
        <table border=1 cellspacing=0 cellpadding=0
               style='border-collapse:collapse;border:none'>
           <tr style='height:13.45pt'>
              <td width=295 valign=bottom style='width:221.15pt;border:none;border-right: solid windowtext 1.0pt;padding:0in 5.4pt 0in 5.4pt;height:13.45pt'>
                 <h3><span lang=BG>ЕИК по Търговски регистър</span></h3>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 0, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 1, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 2, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 3, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 4, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 5, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 6, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 7, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 8, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 9, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 10, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 11, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(emplEikValue, 12, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
           </tr>
        </table>
        <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
        <h4>
           <b><span lang=BG style='font-size:10.0pt;font-family:""Times New Roman""; font-style:normal'>  Адрес:</span></b>
           <span lang=BG style='font-size:10.0pt; font-family:""Times New Roman"";font-style:normal'> обл. <span class='FormValue'>" + emplRegionValue + @"</span>  
общ. <span class='FormValue'>" + emplMunicipalityValue + @"</span> </span>
        </h4>
        <h4>
           <span lang=BG>  </span>
           <span lang=BG style='font-size:10.0pt;font-family: ""Times New Roman"";font-style:normal'>гр.(с.)
  <span class='FormValue'>" + emplCityValue + @"</span> ул.
  <span class='FormValue'>" + emplStreetValue + @"</span>  
  № <span class='FormValue'>" + emplStreetNumberValue + @"</span> </span>
        </h4>
        
        <table Table border=1 cellspacing=0 cellpadding=0
               style='border-collapse:collapse;border:none'>
            <tr style='height:14.25pt'>
               <td valign=bottom style='border:none;border-right: solid windowtext 1.0pt;padding:0in 5.4pt 0in 5.4pt;height:14.25pt'>
                  <p style='font-size:10.0pt;' lang=BG>жк. <span class='FormValue'>" + emplDistrictValue + @"</span>, бл. <span class='FormValue'>" + emplBlockValue + @"</span>, вх. <span class='FormValue'>" + emplEntranceValue + @"</span>, ет. <span class='FormValue'>" + emplFloorValue + @"</span>, ап. <span class='FormValue'>" + emplAptValue + @"</span>;</span>
                     <span lang=BG>&nbsp;&nbsp;пощенски код</span>
                  </p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(emplPostCodeValue, 0, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(emplPostCodeValue, 1, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(emplPostCodeValue, 2, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(emplPostCodeValue, 3, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
            </tr>
        </table>
 
        <p ><span lang=BG style='font-size:3.0pt'>&nbsp;</span></p>
        <p style='font-size:10.0pt;'><span lang=BG>  тел.
  <span class='FormValue'>" + emplPhoneValue + @"</span> факс
  <span class='FormValue'>" + emplFaxValue + @"</span> e-mail
  <span class='FormValue'>" + emplEmailValue + @"</span> </span>
        </p>
        <p ><i><span lang=BG style='font-size:3.0pt'>&nbsp;</span></i></p>
        <p  style='text-align:justify; font-size: 10.0pt;'>
           <b><span lang=BG>  Списъчен брой на работниците и служителите:</span></b>
           <span lang=BG>  <span class='FormValue'>" + emplNumberOfEmployeesValue + @"</span>, <b>от тях жени: </b> <span class='FormValue'>" + emplFemaleEmployeesValue + @"</span> </span>
           <i><span lang=BG style='font-size:8.0pt'>(Попълва се броя на подлежащите</span></i>
        </p>
        <p  style='text-align:justify'>
           <i><span lang=BG style='font-size:8.0pt'>  на осигуряване лица за трудова злополука и професионална болест в началото на месеца, през който е станала злополуката)</span></i>
        </p>
        <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
      </td>
   </tr>
</table>


<p  style='text-align:justify'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>

<h6 style='margin-left:49.65pt'><span lang=BG>II. ДАННИ ЗА ПОСТРАДАЛИЯ</span></h6>

";

            tmpStyle = @"style=""width: 100%; border: solid 1px #000000; border-collapse:collapse;"" ";

            if (exportToWord)
            {
                tmpStyle = @" border=1 cellspacing=0 cellpadding=0 width=690
 style='width:517.45pt;margin-left:-44.25pt;border-collapse:collapse; border:none' ";
            }

            html += @"
<table " + tmpStyle + @">
   <tr style='height:256.2pt'>";

            tmpStyle = @" style='width:30px; border:solid windowtext 1.0pt; padding-left; 5px; text-align: left; vertical-align: top;' ";

            if (exportToWord)
            {
                tmpStyle = @" width=34 valign=top style='width:25.5pt;border:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt' ";
            }

            html += @"
      <td " + tmpStyle + @">
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:6.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>7</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:5.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>8</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>9</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>10</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:2.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>11</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:2.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>12</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:1.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>13</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:23.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>14</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:2.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>15</span></b></p>
         <p ><b><span lang=BG style='font-size:11.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>16</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:5.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>17</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:4.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>18</span></b></p>
      </td>
      <td valign=top style='border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;height:256.2pt'>
         <p  style='text-align:justify'><span lang=BG style='font-size: 6.0pt'>&nbsp;</span></p>
         <table border=1 cellspacing=0 cellpadding=0 style='border-collapse:collapse;border:none'>
            <tr style='height:13.45pt'>
               <td style='border:none;border-right:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt;height:13.45pt'>
                  <h3><span lang=BG style='font-size:10.0pt'>Трите имена:</span> <span lang=BG style='font-weight:normal'><span class='FormValue'>" + workerFullNameValue + @"</span></span><span lang=BG>ЕГН</span></h3>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 0, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 1, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 2, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 3, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 4, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 5, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 6, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 7, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 8, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(workerEgnValue, 9, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
            </tr>
         </table>
  
         <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>

         <h4>
            <b><span lang=BG style='font-size:10.0pt;font-family:""Times New Roman""; font-style:normal'>  Постоянен адрес:</span></b>
            <span lang=BG style='font-size:10.0pt;font-family:""Times New Roman"";font-style:normal'>обл. <span class='FormValue'>" + workerRegionValue + @"</span> общ. <span class='FormValue'>" + workerMunicipalityValue + @"</span></span>
         </h4>
         <h4>
            <span lang=BG>  </span>
            <span lang=BG style='font-size:10.0pt;font-family: ""Times New Roman"";font-style:normal'>
               гр.(с.) <span class='FormValue'>" + workerCityValue + @"</span> 
               ул. <span class='FormValue'>" + workerStreetValue + @"</span> 
               № <span class='FormValue'>" + workerStreetNumberValue + @"</span> 
            </span>
         </h4>
  
         <table Table border=1 cellspacing=0 cellpadding=0 style='border-collapse:collapse;border:none'>
            <tr style='height:14.25pt'>
               <td valign=bottom style='border:none;border-right: solid windowtext 1.0pt;padding:0in 5.4pt 0in 5.4pt;height:14.25pt'>
                  <p class=MsoHeader lang=BG>
                     жк. <span class='FormValue'>" + workerDistrictValue + @"</span>, 
                     бл. <span class='FormValue'>" + workerBlockValue + @"</span>, 
                     вх. <span class='FormValue'>" + workerEntranceValue + @"</span>, 
                     ет. <span class='FormValue'>" + workerFloorValue + @"</span>, 
                     ап. <span class='FormValue'>" + workerAptValue + @"</span>;</span>
                     <span lang=BG>пощенски код</span>
                  </p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? "width:16.2pt" : "") + @";border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(workerPostCodeValue, 0, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? "width:16.2pt" : "") + @";border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(workerPostCodeValue, 1, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? "width:16.2pt" : "") + @";border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(workerPostCodeValue, 2, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? "width:16.2pt" : "") + @";border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(workerPostCodeValue, 3, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
            </tr>
         </table>
         
         <p ><span lang=BG style='font-size:3.0pt'>&nbsp;</span></p>
         
         <p >
            <span lang=BG>  
               тел. <span class='FormValue'>" + workerPhoneValue + @"</span>   
               факс <span class='FormValue'>" + workerFaxValue + @"</span>  
               e-mail <span class='FormValue'>" + workerEmailValue + @"</span>
            </span>
         </p>
         
         <p  style='text-align:justify'>
            <b><span lang=BG>  Дата на раждане:</span></b><span lang=BG> <span class='FormValue'>" + workerBirthDateValue + @"</span> </span>
            <span lang=BG> </span><i><span lang=BG style='font-size:8.0pt'>(дд мм гггг)</span></i><span lang=BG>,  </span>        
            <span lang=BG>         </span>
            <b><span lang=BG>Пол:</span></b>
            <span lang=BG>   </span>
            " + wGenderMaleValue + @"
            <span lang=BG style='font-size:9.0pt'> </span><span lang=BG>- мъж;</span>
            <span lang=BG style='font-size:9.0pt'>               </span>
            " + wGenderFemaleValue + @"
            <span lang=BG style='font-size:9.0pt'> </span><span lang=BG>- жена;</span>
         </p>
      
         <p class=MsoBodyText3>
            <span lang=BG style='font-size:3.0pt'>&nbsp;</span>
         </p>

        <p class=MsoBodyText3>
           <b><span lang=BG style='font-size:10.0pt;font-style: normal'>  Гражданство:</span></b>
           <span lang=BG style='font-size:10.0pt; font-style:normal'> <span class='FormValue'>" + workerCitizenshipValue + @"</span> </span>
        </p>
  
        <p class=MsoBodyText3>
           <span lang=BG style='font-size:10.0pt'>  </span>
           <b><span lang=BG style='font-size:10.0pt;font-style:normal'>Пострадалият е нает за:</span></b>
        </p>
  
        <p class=MsoBodyText3>
           <span lang=BG style='font-size:9.0pt;font-style:normal'> </span>
           <span lang=BG style='font-size:9.0pt'> </span>
           <b><span style='font-size:10.0pt; font-style:normal'>1</span></b>
           <b><span lang=BG style='font-size:10.0pt; font-style:normal'>)</span></b>
           <span lang=BG style='font-size:10.0pt; font-style:normal'> </span>
           " + wHireType1Value + @"<span lang=BG style='font-size:10.0pt; font-style:normal'>- </span>
           <span lang=BG style='font-size:9.0pt;font-style: normal'>неопределено време или</span>
           <span lang=BG style='font-size:10.0pt; font-style:normal'>   </span>
           " + wHireType2Value + @"<span lang=BG style='font-size:10.0pt;font-style:normal'>- </span>
           <span lang=BG style='font-size:9.0pt;font-style:normal'>определен срок</span>
           <span lang=BG style='font-size:10.0pt;font-style:normal'>;   </span>
           <b><span style='font-size:10.0pt;font-style:normal'>2</span></b>
           <b><span lang=BG style='font-size:10.0pt;font-style:normal'>)</span></b>
           <span lang=BG style='font-size:10.0pt;font-style:normal'> </span>
           " + wWorkTime1Value + @"<span lang=BG style='font-size:10.0pt;font-style:normal'>- </span>
           <span lang=BG style='font-size:9.0pt;font-style:normal'>пълно работно време или</span>
           <span lang=BG style='font-size:10.0pt;font-style:normal'>   </span>
           " + wWorkTime2Value + @"<span lang=BG style='font-size:10.0pt;font-style:normal'>- </span>
           <span lang=BG style='font-size:9.0pt;font-style:normal'>непълно работно време</span>
        </p>
  
        <p class=MsoBodyText3>
           <span lang=BG>  (Зачертава се по едно квадратче от позиции 1 и 2 в зависимост от условията, при които е сключено трудовото/служебното правоотношение)</span>
        </p>
       
        <p class=MsoBodyText3>
           <span lang=BG style='font-size:3.0pt'>&nbsp;</span>
        </p>

        <p  style='text-align:justify'>
           <b><span lang=BG>  Дата на постъпване в предприятието:</span></b>
           <span lang=BG> <span class='FormValue'>" + workerHireDateValue + @"</span> ;</span>  
           <i><span lang=BG style='font-size:8.0pt'>(дд мм гггг)</span></i>
        </p>
 
        <p  style='text-align:justify'>
           <i><span lang=BG style='font-size:1.0pt'>&nbsp;</span></i>
        </p>

        <table Table border=1 cellspacing=0 cellpadding=0 style='border-collapse:collapse;border:none'>
           <tr style='page-break-inside:avoid;height:14.2pt'>
              <td valign=bottom style='border:none;border-right: solid windowtext 1.0pt;padding:0in 5.4pt 0in 5.4pt;height:14.2pt'>
                 <p class=MsoBodyText3>
                    <b><span lang=BG style='font-size:10.0pt;font-style: normal'>Професия (длъжност):</span></b>
                    <span lang=BG style='font-size:10.0pt; font-style:normal'> <span class='FormValue'>" + workerJobTitleValue + @"</span> </span>
                    <b><span lang=BG style='font-size:10.0pt;font-style:normal'>Код по НКПД</span></b>
                 </p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=bottom style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p class=MsoBodyText3><span lang=BG style='font-size:10.0pt;font-style: normal'>" + CommonFunctions.CharAt(workerJobCodeValue, 0, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=bottom style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p class=MsoBodyText3><span lang=BG style='font-size:10.0pt;font-style: normal'>" + CommonFunctions.CharAt(workerJobCodeValue, 1, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=bottom style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p class=MsoBodyText3><span lang=BG style='font-size:10.0pt;font-style: normal'>" + CommonFunctions.CharAt(workerJobCodeValue, 2, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=bottom style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p class=MsoBodyText3><span lang=BG style='font-size:10.0pt;font-style: normal'>" + CommonFunctions.CharAt(workerJobCodeValue, 3, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=bottom style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p class=MsoBodyText3><span lang=BG style='font-size:10.0pt;font-style: normal'>" + CommonFunctions.CharAt(workerJobCodeValue, 4, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=bottom style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p class=MsoBodyText3><span lang=BG style='font-size:10.0pt;font-style: normal'>" + CommonFunctions.CharAt(workerJobCodeValue, 5, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=bottom style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p class=MsoBodyText3><span lang=BG style='font-size:10.0pt;font-style: normal'>" + CommonFunctions.CharAt(workerJobCodeValue, 6, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=bottom style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p class=MsoBodyText3><span lang=BG style='font-size:10.0pt;font-style: normal'>" + CommonFunctions.CharAt(workerJobCodeValue, 7, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
           </tr>
        </table>
        
        <p class=MsoBodyText3>
           <span lang=BG>  </span>
           <span lang=BG style='font-size: 7.5pt'>(Посочва се длъжността и нейният осемзначен цифров код съгласно Националната класификация на професиите и длъжностите – 2005</span>
           <span lang=BG>)</span>
        </p>

        <p class=MsoBodyText3>
           <span lang=BG style='font-size:10.0pt;font-style:normal'> </span>
           <b><span lang=BG style='font-size:10.0pt;font-style:normal'>Категория труд:</span></b>
           <span lang=BG style='font-size:10.0pt;font-style:normal'>   </span>
           " + workerJobCategory1Value + @"
           <span lang=BG style='font-size:10.0pt;font-style:normal'> - първа </span>
           " + workerJobCategory2Value + @"
           <span lang=BG style='font-size:10.0pt;font-style:normal'> - втора</span>
           " + workerJobCategory3Value + @"
           <span lang=BG style='font-size:10.0pt;font-style:normal'> - трета </span>
        </p>

        <p class=MsoBodyText3>
           <span lang=BG style='font-size:3.0pt;font-style:normal'>&nbsp;</span>
        </p>
        
        <p class=MsoBodyText3>
           <span lang=BG style='font-size:10.0pt;font-style:normal'> </span>
           <b><span lang=BG style='font-size:10.0pt;font-style:normal'>Трудов стаж</span></b>
           <span lang=BG style='font-size:10.0pt;font-style:normal'> </span>
           <span lang=BG style='font-size:10.0pt'>(години)</span>
           <span lang=BG style='font-size:10.0pt'> </span>
           <span style='font-size:10.0pt;font-style: normal'>общо: <span class='FormValue'>" + workerYearsOnServiceValue + @"</span> г.,</span>
           <span lang=BG style='font-size:10.0pt;font-style: normal'> </span>
           <span lang=BG style='font-size:10.0pt;font-style:normal'>по посочената професия: <span class='FormValue'>" + workerCurrentJobYearsOnServiceValue + @"</span> г.</span>
        </p>

        <p class=MsoBodyText3>
           <span lang=BG style='font-size:3.0pt;font-style:normal'>&nbsp;</span>
        </p>

        <p class=MsoBodyText3>
           <span lang=BG style='font-size:10.0pt;font-style:normal'>  </span>
           <b><span lang=BG style='font-size:10.0pt;font-style:normal'>Административна единица в която е назначен:</span></b>
           <span lang=BG style='font-size:10.0pt; font-style:normal'> <span class='FormValue'>" + workerBranchValue + @"</span> </span>
        </p>
        <p class=MsoBodyText3>
           <span lang=BG>  (Посочва се административната единица – цех, участък, отдел и т.н., в която е назначен пострадалият)</span>
        </p>
        <p  style='text-align:justify'>
           <span lang=BG style='font-size: 3.0pt'>&nbsp;</span>
        </p>
     </td>
  </tr>
</table>

<p  style='text-align:justify'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>

<h1 style='margin-left:49.65pt;text-indent:-49.65pt'><span lang=BG>III. ДАННИ ЗА ЗЛОПОЛУКАТА</span></h1>

";

            tmpStyle = @"style=""width: 100%; border: solid 1px #000000; border-collapse:collapse;"" ";

            if (exportToWord)
            {
                tmpStyle = @" border=1 cellspacing=0 cellpadding=0 width=690
 style='width:517.45pt;margin-left:-44.25pt;border-collapse:collapse; border:none;' ";
            }

            html += @"
<table " + tmpStyle + @" >
   <tr style='height:3.5pt'>";

            tmpStyle = @" style='width:30px; border:solid windowtext 1.0pt; border-bottom: none; padding-left; 5px; text-align: left; vertical-align: top;' ";

            if (exportToWord)
            {
                tmpStyle = @" width=34 valign=top style='width:25.5pt;border:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt' ";
            }

            html += @"
      <td " + tmpStyle + @">
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:6.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>19</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>20</span></b></p>
         <p ><b><span lang=BG style='font-size:5.0pt'>&nbsp;</span></b></p>
         <p ><b><span lang=BG style='font-size:" + (exportToWord ? "9.0pt" : "16.0pt") + @"'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>21</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:" + (exportToWord ? "7.0pt" : "11.0pt") + @"'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>22</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:9.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:9.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:9.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:" + (exportToWord ? "13.0pt" : "11.0pt") + @"'>&nbsp;</span></b></p>
         <p ><b><span lang=BG style='font-size:12.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>23</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><span lang=BG style='font-size:7.0pt'>&nbsp;</span></p>
      </td>
      <td valign=top style='border:solid windowtext 1.0pt; border-left:none;" + (exportToWord ? "" : "border-bottom: none;") + @"padding:0in 5.4pt 0in 5.4pt;height:3.5pt;'>
         <p  style='text-align:justify'><b><span lang=BG style='font-size:6.0pt'>&nbsp;</span></b></p>
         <p  style='text-align:justify'>
            <b><span lang=BG>  Злополуката е станала</span></b>
            <span lang=BG>   в <span class='FormValue'>" + accidentTimeHourValue + @"</span> часа   и <span class='FormValue'>" + accidentTimeMinutesValue + @"</span> мин.  на <span class='FormValue'>" + accidentDateValue + @"</span>  </span>
            <i><span lang=BG style='font-size:8.0pt'>(дд мм гггг)</span></i>
         </p>
         <p class=MsoBodyText3><span lang=BG style='font-size:4.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'>
            <b><span lang=BG>  Работно време:</span></b>
            <span lang=BG>  </span>
            <b><span lang=BG style='font-size: 9.0pt'>от</span></b>
            <span lang=BG style='font-size:9.0pt'> <span class='FormValue'>" + accidentWorkFromHour1ValueValue + @"</span> часа и <span class='FormValue'>" + accidentWorkFromMin1ValueValue + @"</span> мин. </span>
            <b><span lang=BG style='font-size:9.0pt'>до</span></b>
            <span lang=BG style='font-size:9.0pt'> <span class='FormValue'>" + accidentWorkToHour1ValueValue + @"</span> часа  и <span class='FormValue'>" + accidentWorkToMin1ValueValue + @"</span> мин. </span><b>
            <span lang=BG style='font-size:9.0pt'>и  от</span></b>
            <span lang=BG style='font-size:9.0pt'> <span class='FormValue'>" + accidentWorkFromHour2ValueValue + @"</span> часа  и <span class='FormValue'>" + accidentWorkFromMin2ValueValue + @"</span> мин. </span><b>
            <span lang=BG style='font-size:9.0pt'>до</span></b>
            <span lang=BG style='font-size: 9.0pt'> <span class='FormValue'>" + accidentWorkToHour2ValueValue + @"</span> часа  и <span class='FormValue'>" + accidentWorkToMin2ValueValue + @"</span> мин.</span>
         </p>
         <p class=MsoBodyText3><span lang=BG>  (Посочва се предвиденото за деня на злополуката разпределение на работното време на пострадалия спрямо почивката за хранене)</span></p>
         <p  style='text-align:justify'><i><span lang=BG style='font-size:3.0pt'>&nbsp;</span></i></p>
         <p  style='text-align:justify'>
            <b><span lang=BG>  Място на злополуката:</span></b>
            <span lang=BG> <span class='FormValue'>" + accidentPlaceValueValue + @"</span></span>
         </p>
         <p class=MsoBodyText3>
            <span lang=BG>  (Посочва се подробно мястото или помещението, където се е намирал пострадалият в момента на злополуката – производствен или 
            ремонтен цех, склад, строителен обект, рудник, селскостопански или горски обект, канцелария, училище, търговски обект, лечебно 
            заведение, път, тротоар, чакалня, превозно средство, жилище, спортен обект  и т.н)</span>
         </p>
         <p  style='text-align:justify'>
            <span lang=BG style='font-size: 3.0pt'>&nbsp;</span>
         </p>
         <p  style='text-align:justify'>  
            <b><span lang=BG>Адрес на мястото, където е станала злополуката:</span></b>
         </p>
         <p  style='text-align:justify'>
            <b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b>
         </p>
         <p  style='text-align:justify'>
            <b><span lang=BG>  </span></b>
            <span lang=BG>Държава:<b> </b> <span class='FormValue'>" + accidentCountryValueValue + @"</span> обл.  <span class='FormValue'>" + accidentRegionValueValue + @"</span>  общ. <span class='FormValue'>" + accidentMunicipalityValueValue + @"</span></span>
         </p>
         <p  style='text-align:justify'>
            <span lang=BG style='font-size: 3.0pt'>&nbsp;</span>
         </p>
        
         <h4>
            <span lang=BG>  </span>
            <span lang=BG style='font-size:10.0pt;font-family: ""Times New Roman"";font-style:normal'>гр.(с.) <span class='FormValue'>" + accidentCityValueValue + @"</span> ул. <span class='FormValue'>" + accidentStreetValueValue + @"</span>  № <span class='FormValue'>" + accidentStreetNumberValueValue + @"</span></span>
         </h4>

         <table border=1 cellspacing=0 cellpadding=0 style='border-collapse:collapse;border:none'>
            <tr style='height:14.25pt'>
               <td valign=bottom style='border:none;border-right: solid windowtext 1.0pt;padding:0in 5.4pt 0in 5.4pt;height:14.25pt'>
                  <p class=MsoHeader lang=BG>жк. <span class='FormValue'>" + accidentDistrictValueValue + @"</span> <span lang=BG> бл. <span class='FormValue'>" + accidentBlockValueValue + @"</span>, вх. <span class='FormValue'>" + accidentEntranceValueValue + @"</span>, ет. <span class='FormValue'>" + accidentFloorValueValue + @"</span>, ап. <span class='FormValue'>" + accidentAptValueValue + @"</span>;</span>     <span lang=BG>пощенски код</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(accidentPostCodeValueValue, 0, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(accidentPostCodeValueValue, 1, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(accidentPostCodeValueValue, 2, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p ><span lang=BG>" + CommonFunctions.CharAt(accidentPostCodeValueValue, 3, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
            </tr>
         </table>
         <p ><span lang=BG style='font-size:3.0pt'>&nbsp;</span></p>
         <p >
            <span lang=BG>  тел. <span class='FormValue'>" + accidentPhoneValueValue + @"</span>   факс <span class='FormValue'>" + accidentFaxValueValue + @"</span>  e-mail <span class='FormValue'>" + accidentEmailValueValue + @"</span></span>
         </p>
         <p ><span style='font-size:1.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'><b><span lang=BG>Злополуката е станала на:</span></b></p>
         <p  style='text-align:justify'>
            " + accHappenedAt1Value + @"
            <i><span lang=BG> </span></i>
            <span lang=BG>- обичайното стационарно работно място </span>
            <i><span lang=BG style='font-size:8.0pt'>(обичайното помещение, сграда, цех, съоръжение или друго териториално определено място в предприятието, където лицето полага труда си и изпълнява трудовите/служебните си задължения)</span></i>
         </p>
         <p  style='text-align:justify'>
            " + accHappenedAt2Value + @"
            <i><span lang=BG  </span></i><span lang=BG>- случайно работно място </span>
            <i><span lang=BG style='font-size: 8.0pt'>(случайно местонахождение на лицето по повод извършваната работа)</span></i>
            <span lang=BG>, нестационарно (мобилно) работно място </span>
            <i><span lang=BG style='font-size:8.0pt'>(за пътни полицаи, шофьори и др.)</span></i>
            <span lang=BG>, временно работно място </span><i>
            <span lang=BG style='font-size: 8.0pt'>(в предприятието или извън него)</span></i>
         </p>
         <p  style='text-align:justify'>
            " + accHappenedAt3Value + @"
            <i><span lang=BG> </span></i>
            <span lang=BG>- друго </span>
            <i><span lang=BG style='font-size:8.0pt'>(посочва се) </span></i>
            <span lang=BG><span class='FormValue'>" + accHappenedAtOtherValue + @"</span></span>
         </p>
      </td>
   </tr>
   <tr style='height:28.2pt'>";

            tmpStyle = @" style='width:30px; border:solid windowtext 1.0pt; border-bottom: none; border-top: none; padding-left; 5px; text-align: left; vertical-align: top;' ";

            if (exportToWord)
            {
                tmpStyle = @" width=34 valign=top style='width:25.5pt;border:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt' ";
            }

            html += @"
      <td " + tmpStyle + @">
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:6.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>24</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:9.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:9.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:7.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:" + (exportToWord ? "8.0pt" : "22.0pt") + @"'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>25</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         " + (exportToWord ? "" : 
                             "<p  align=center style='text-align:center'><span lang=BG style='font-size: 5.0pt;'>&nbsp;</span></p>") + @"
         <p ><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>26</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:" + (exportToWord ? "7.0pt" : "24.0pt") + @"'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>27</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:" + (exportToWord ? "12.0pt" : "31.0pt") + @"'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>28</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>29</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>30</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  style='text-align:justify'><span lang=BG style='font-size: 7.0pt'>&nbsp;</span></p>
      </td>
      <td valign=top style='border-top:" + (exportToWord ? "1pt solid" : "none") + @";border-left: none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt;height:28.2pt'>
         <p  style='text-align:justify'><b><span lang=BG style='font-size:6.0pt'>&nbsp;</span></b></p>
         <p  style='text-align:justify'>
             <b><span lang=BG>Вид работа: </span></b>
             <span lang=BG><span class='FormValue'>" + accidentJobTypeValueValue + @"</span></span>
         </p>
         <p class=MsoBodyText3>
            <span lang=BG>(Посочва се видът работа в по-широк
смисъл, която пострадалият е извършвал в периода преди злополуката. Това не е
професията на пострадалия. Тази работа (работен процес) обикновено съдържа
специфичното действие, посочено в ред 25. Видът работа може да обхваща
дейности по произвеждане, обработване, преработване, складиране; земни
работи, строителни и монтажни работи, събаряне; селскостопанска и
горскостопанска работа; предоставяне на услуги, интелектуален труд;
монтиране, демонтиране, ремонт, регулиране, наблюдение и контрол на
производствен процес или оборудване; движение, пътуване и др.)</span>
         </p>
         <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'>
            <b><span lang=BG>Специфично действие, извършвано от пострадалия и свързаният с това действие материален фактор:</span></b>
         </p>
         <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'><span lang=BG><span class='FormValue'>" + accidentTaskTypeValueValue + @"</span></span></p>
         <p class=MsoBodyText3>
            <span lang=BG style='font-size:7.5pt'>(Описват се
подробно специфичното действие, извършвано от пострадалия непосредствено
преди злополуката, и конкретният материален фактор (сгради, конструкции,
съоръжения; машини, инструменти; превозни средства; материали, предмети,
товари; вещества; хора; животни; природни бедствия и т.н.), свързан с това
действие. Специфичното действие може да е работа с машини; работа с ръчни
инструменти; управление на/пътуване с превозни средства или
подемно-транспортни средства; боравене с предмети; пренасяне на ръка;
движение, присъствие и др. Материалният фактор, посочен в редове 25, 26 и 27,
може да бъде както един и същ, така и различен)</span>
         </p>
         <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'><b><span lang=BG>Отклонение от нормалните действия (условия) и материален фактор, свързан с това отклонение:</span></b></p>
         <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'><span lang=BG><span class='FormValue'>" + deviationFromTaskValueValue + @"</span></span></p>
         <p class=MsoBodyText3>
            <span lang=BG>(Описва се подробно отклонението от
нормалните действия или условия, довело до злополуката. Отклонението може да
е в резултат от проблем с електричеството, експлозия, пожар; препълване,
преобръщане, протичане, изтичане, изпаряване, емисия, счупване, разрушаване,
плъзгане, падане, срутване на материалния фактор; загуба на контрол върху
машина, ръчни инструменти, предмети или животни; подхлъзване, спъване, падане
на човек; движение, физичеко натоварване, физическо насилие и  т.н.)</span></p>
<p  style='text-align:justify'><span lang=BG style='font-size:
3.0pt'>&nbsp;</span>
         </p>
         <p  style='text-align:justify'><b><span lang=BG>Начин на увреждането и материален фактор, причинил увреждането:</span></b></p>
         <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'><span lang=BG><span class='FormValue'>" + accidentInjurDescValueValue + @"</span></span></p>
         <p class=MsoBodyText3>
            <span lang=BG>(Описва се как е увреден пострадалият и
как е влязъл в контакт с материалния фактор, причинил увреждането. Начинът на
увреждане може да е в резултат на контакт с електрически ток, пламък, опасни
вещества; задушаване чрез удавяне, затрупване; удар или сблъсък от/с предмет;
контакт с режещ, пробождащ предмет; захващане, притискане, смазване,
смачкване от предмет или машина; физическо натоварване на мускули, стави или
органи; психическо натоварване; ухапване, ритане от животно или човек и 
т.н.)</span>
         </p>
         <p  style='text-align:justify'>
            <b><span lang=BG>Пострадалият имал ли е необходимата правоспособност:   </span></b>
            " + accidentInjHasRights1Value + @"
            <i><span lang=BG> </span></i><span lang=BG>- да </span>
            " + accidentInjHasRights2Value + @"
            <i><span lang=BG> </span></i><span lang=BG>- не </span>
            " + accidentInjHasRights3Value + @"
            <i><span lang=BG> </span></i><span lang=BG>- не се изисква</span>
         </p>
         <p  style='text-align:justify'>
            <b><span lang=BG>Злополуката е: </span></b>
            " + accidentLegalRef1Value + @"
            <i><span lang=BG> </span></i>
            <span lang=BG>- по чл. 55, ал. 1 КСО </span>
            " + accidentLegalRef2Value + @"
            <i><span lang=BG> </span></i>
            <span lang=BG>- по чл. 55, ал. 2 КСО </span>
         </p>
         <p  style='text-align:justify'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
         <p  style='text-align:justify'>
            <b><span lang=BG>Набелязани мерки: </span></b>
            <span lang=BG><span class='FormValue'>" + accidentPlannedActionsValueValue + @"</span></span>
         </p>
         <p class=MsoBodyText3>
            <span lang=BG>(Посочват се какви мерки, свързани с
човешкия и материалния фактор, е предприел осигурителят за предотвратяване на
подобни</span>
         </p>
         <p class=MsoBodyText3><span lang=BG> злополуки)</span></p>
         <p  style='text-align:justify'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>
      </td>
   </tr>
</table>

<p  style='text-align:justify'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>

<h1 style='margin-left:49.65pt;text-indent:" + (exportToWord ? "-89.65pt" : "-49.65pt") + @"'>
   <span lang=BG >IV. ДАННИ ЗА УВРЕЖДАНЕТО</span>
</h1>";

            tmpStyle = @"style=""width: 100%; border: solid 1px #000000; border-collapse:collapse;"" ";

            if (exportToWord)
            {
                tmpStyle = @" border=1 cellspacing=0 cellpadding=0 width=690
 style='width:517.45pt;margin-left:-44.25pt;border-collapse:collapse; border:none' ";
            }

            html += @"
<table " + tmpStyle + @">
   <tr>";

            tmpStyle = @" style='width:30px; border:solid windowtext 1.0pt; border-bottom: none; border-top: none; padding-left; 5px; text-align: left; vertical-align: top;' ";

            if (exportToWord)
            {
                tmpStyle = @" width=34 valign=top style='width:25.35pt;border:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt' ";
            }

            html += @"
      <td " + tmpStyle + @">
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:6.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>31</span></b></p>
         <p ><b><span lang=BG style='font-size:" + (exportToWord ? "19.0pt" : "30.0pt") + @"'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>32</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG style='font-size:7.0pt'>&nbsp;</span></b></p>
         <p  align=center style='text-align:center'><b><span lang=BG>33</span></b></p>
      </td>
      <td valign=top style='border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt'> 
         <p  style='text-align:justify'><span lang=BG style='font-size: 6.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'>
            <b><span lang=BG>Вид на уврежданията:</span></b>
            <span lang=BG><span class='FormValue'>" + harmTypeValue + @"</span></span>
         </p>
         <p class=MsoBodyText3>
            <span lang=BG>(Посочва се видът на уврежданията на
  пострадалия (рани, счупвания, изкълчвания, ампутации, мозъчно сътресение,
  вътрешни травми, изгаряния, измръзвания, отравяне, удавяне, задушаване  и
  т.н.) съгласно болничния лист и/или друг медицински документ)</span>
         </p>
         <p class=MsoBodyText3><span lang=BG style='font-size:3.0pt'>&nbsp;</span></p>
         <p  style='text-align:justify'>
            <b><span lang=BG>Увредени части на тялото:</span></b>
            <span lang=BG><span class='FormValue'>" + harmBodyPartsValue + @"</span></span>
         </p>
         <p class=MsoBodyText3>
            <span lang=BG>(Посочват се увредените части на тялото –
  глава (лице, очи, уши, зъби и т.н.), шия, гръб, туловище и органи, горни
  крайници (рамо, лакът, китка, пръсти и т.н.), долни крайници (бедро, коляно,
  глезен, ходило, пръсти и т.н.) и др. Когато има две еднакви части на тялото,
  се посочва коя от тях е увредена – лява, дясна или и двете)</span>
         </p>
         <p  style='text-align:justify'>
            <b><span lang=BG>Последици:   </span></b>
            " + harmResult1Value + @"
            <i><span lang=BG> </span></i>
            <span lang=BG>- смърт</span>
            " + harmResult2Value + @"
            <span lang=BG>- вероятна инвалидност               </span>
            " + harmResult3Value + @"
            <span lang=BG>- временна неработоспособност</span>
         </p>
      </td>
   </tr>
</table>

<p style='text-align:justify'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>";

            tmpStyle = @"style=""width: 100%; border: solid 1px #000000; border-collapse:collapse;"" ";

            if (exportToWord)
            {
                tmpStyle = @" border=1 cellspacing=0 cellpadding=0 width=690 style='width:517.45pt;margin-left:-44.25pt;border-collapse:collapse; border:none' ";
            }


            html += @"
<table " + tmpStyle + @" >
   <tr>
      <td valign=top style='border:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt'>
         <p  style='text-align:justify'>
            <span style='font-size:6.0pt'>&nbsp;</span>
         </p>
         <h5>
            <span lang=BG>Свидетели:  </span>
            <span lang=BG style='font-weight:normal'><span class='FormValue'>" + witnessDataValue + @"</span></span>
         </h5>
         <h5><i><span lang=BG style='font-size:8.0pt;font-weight:normal'>(Посочват се трите имена и адресите на свидетелите)</span></i></h5>
         <p ><span lang=BG style='font-size:3.0pt'>&nbsp;</span></p>
      </td>
   </tr>
</table>

<p  style='text-align:justify'><b><span lang=BG style='font-size:4.0pt'>&nbsp;</span></b></p>

<p  align=center style='margin-left:42.55pt;text-align:center; text-indent:" + (exportToWord ? "-89.65pt" : "-49.65pt") + @"'>
   <b><span lang=BG style='font-size:9.0pt'>ДЕКЛАРАЦИЯТА СЕ ПОДАВА ОТ:</span></b>
</p>

<p  align=center style='margin-left:42.55pt;text-align:center; text-indent:" + (exportToWord ? "-89.65pt" : "-49.65pt") + @"'>
   <i><span lang=BG style='font-size:8.0pt'>(Подписва се само от подаващия декларацията)</span></i>
</p>

<p  align=center style='margin-left:42.55pt;text-align:center; text-indent: " + (exportToWord ? "-82.55pt" : "-42.55pt") + @"'>
   <i><span lang=BG style='font-size:3.0pt'>&nbsp;</span></i>
</p>

<p  style='margin-left:42.55pt;text-indent:" + (exportToWord ? "-82.55pt" : "-42.55pt") + @"'>
   " + applicantType1Value + @"
   <span lang=BG style='font-size:16.0pt'> </span>
   <b><span lang=BG>Пострадал: </span></b><span lang=BG>............................           </span>
   " + applicantType2Value + @"
   <span lang=BG style='font-size:16.0pt'> </span><b><span lang=BG>Осигурител:</span></b>
   <span lang=BG> <span class='FormValue'>" + aplicantType2PositionValue + @"</span>  <span class='FormValue'>" + aplicantType2NameValue + @"</span></span>
</p>

<p  style='margin-left:42.55pt;text-indent:" + (exportToWord ? "-82.55pt" : "-42.55pt") + @"'>
<i>
   <span lang=BG style='font-size:8.0pt'>
   " + CommonFunctions.Replicate("&nbsp;", (exportToWord ? 35 : 30)) + @"(подпис)
   " + CommonFunctions.Replicate("&nbsp;", (exportToWord ? 70 : 55)) + @"(длъжност)
   " + CommonFunctions.Replicate("&nbsp;", 5) + @"(име и фамилия)
   </span>
</i>
</p>

<p  style='margin-left:42.55pt;text-indent:" + (exportToWord ? "-82.55pt" : "-42.55pt") + @"'>
   <span lang=BG style='font-size:4.0pt'>&nbsp;</span>
</p>

<p  style='margin-left:42.55pt;text-indent:" + (exportToWord ? "-82.55pt" : "-42.55pt") + @"'>
   " + applicantType3Value + @"
   <span lang=BG style='font-size:16.0pt'> </span>
   <b><span lang=BG>Наследник:</span></b>
   <span lang=BG>..............................
" + CommonFunctions.Replicate("&nbsp;", 60) + @" ........................................</span>
</p>

<p  style='margin-left:42.55pt;text-indent:" + (exportToWord ? "-82.55pt" : "-42.55pt") + @"'>
   <i><span lang=BG style='font-size:8.0pt'>
" + CommonFunctions.Replicate("&nbsp;", (exportToWord ? 35 : 30)) + @"(подпис)
" + CommonFunctions.Replicate("&nbsp;", (exportToWord ? 105 : 80)) + @"(подпис, печат)
   </span></i>
</p>

<p  style='margin-left:42.55pt;text-indent:" + (exportToWord ? "-82.55pt" : "0") + @"'>
   <i><span lang=BG style='font-size:1.0pt'>&nbsp;</span></i>
</p>

<h1 style='text-align :left; text-indent: " + (exportToWord ? "-42.55pt" : "0") + @";'>
   <span lang=BG >ДАННИ ЗА НАСЛЕДНИКА:</span>
</h1>

<p style='text-align :left; text-indent: " + (exportToWord ? "-42.55pt" : "0") + @";'>
   <i><span lang=BG style='font-size:8.0pt'>(Попълва се при смърт на пострадалия)</span></i>
</p>";

            tmpStyle = @"style=""width: 100%; border: solid 1px #000000; border-collapse:collapse;"" ";

            if (exportToWord)
            {
                tmpStyle = @" border=1 cellspacing=0 cellpadding=0 width=690 style='width:517.45pt;margin-left:-44.25pt;border-collapse:collapse; border:none' ";
            }

            html += @"
<table " + tmpStyle + @" >
   <tr style='height:66.75pt'>
      <td valign=top style='border:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt;height:66.75pt'>
         <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
         <table Table border=1 cellspacing=0 cellpadding=0 style='border-collapse:collapse;border:none'>
            <tr style='height:13.45pt'>
               <td style='border:none;border-right:solid windowtext 1.0pt; padding:0in 5.4pt 0in 5.4pt;height:13.45pt'>
                  <h3>
                     <span lang=BG style='font-size:10.0pt'>Трите имена:</span>
                     <span lang=BG style='font-weight:normal'><span class='FormValue'>" + heirFullNameValue + @"</span> </span>
                     <span lang=BG>ЕГН</span>
                  </h3>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 0, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 1, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG tyle='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 2, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 3, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 4, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 5, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 6, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 7, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 8, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td> 
               <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                  <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirEgnValue, 9, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
               </td>
            </tr>
         </table>
         <p  style='text-align:justify'><span lang=BG style='font-size: 3.0pt'>&nbsp;</span></p>
         <h4>
            <b>
               <span lang=BG style='font-size:10.0pt;font-family:""Times New Roman""; font-style:normal'>  Адрес:</span>
            </b>
            <span lang=BG style='font-size:10.0pt; font-family:""Times New Roman"";font-style:normal'> 
               обл. <span class='FormValue'>" + heirRegionValue + @"</span>   
               общ. <span class='FormValue'>" + heirMunicipalityValue + @"</span>
            </span>
         </h4>
         <table border=1 cellspacing=0 cellpadding=0 style='border-collapse:collapse;border:none'>
            <tr style='height:14.25pt'>
               <td valign=bottom style='border:none;border-right: solid windowtext 1.0pt;padding:0in 5.4pt 0in 5.4pt;height:14.25pt'>
                  <p class=MsoHeader>
                     гр.(с.) <span class='FormValue'>" + heirCityValue + @"</span>   
                     ул. <span class='FormValue'>" + heirStreetValue + @"</span>   
                     № <span class='FormValue'>" + heirStreetNumberValue + @"</span>   
                    <span lang=BG>пощенски код</span>
                  </p>
               </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirPostCodeValue, 0, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirPostCodeValue, 1, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirPostCodeValue, 2, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
              <td " + (exportToWord ? numberBoxWidth : "") + @" valign=top style='" + (exportToWord ? numberBoxWidthCSS : "") + @"border:solid windowtext 1.0pt; border-left:none;padding:0in 5.4pt 0in 5.4pt;" + (exportToWord ? numberBoxHeightCSS : "") + @"'>
                 <p  style='text-align:justify'><span lang=BG style='font-size:9.0pt'>" + CommonFunctions.CharAt(heirPostCodeValue, 3, (exportToWord ? "" : "&nbsp;")) + @"</span></p>
              </td>
          </tr>
       </table>
       <p ><span lang=BG style='font-size:3.0pt'>&nbsp;</span></p>
       <p ><span lang=BG>  
          жк. <span class='FormValue'>" + heirDistrictValue + @"</span>,
          бл. <span class='FormValue'>" + heirBlockValue + @"</span>, 
          вх. <span class='FormValue'>" + heirEntranceValue + @"</span>, 
          ет. <span class='FormValue'>" + heirFloorValue + @"</span>, 
          ап. <span class='FormValue'>" + heirAptValue + @"</span>;      
          тел. <span class='FormValue'>" + heirPhoneValue + @"</span></span>
       </p>
       <p ><span lang=BG style='font-size:3.0pt'>&nbsp;</span></p>
    </td>
 </tr>
</table>

<p ><i><span lang=BG style='font-size:1.0pt'>&nbsp;</span></i></p>

<p  align=center style='text-align:center'><b><span lang=BG style='font-size:3.0pt'>&nbsp;</span></b></p>

<p align=center style='text-align:center; text-indent: " + (exportToWord ? "-42.55pt" : "-22.55pt") + @";'>
   <b><span lang=BG style='font-size:8.0pt'>ВНИМАНИЕ! Декларацията се подава в териториалното поделение на НОИ по регистрация на осигурителя в четири екземпляра.</span></b>
</p>

";

            if (exportToWord)
            {
                html += @"
</div>
</body>
</html>";
            }

            return html;
        }

        protected void btnGenerateWord_Click(object sender, EventArgs e)
        {
            string result = GenerateDeclaration(true);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=Declaration.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private string GenerateWitnessData(int accidentDeclarationId)
        {
            StringBuilder sb = new StringBuilder();

            // Visibility of Witness tab
            bool isWitnessHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH") == UIAccessLevel.Hidden;
            if (!isWitnessHidden)
            {
                DeclarationOfAccidentWith declarationOfAccidentWith = DeclarationOfAccidentUtil.GetDeclarationOfAccident(accidentDeclarationId, CurrentUser, "btnTabWith").DeclarationOfAccidentWith;
                if (declarationOfAccidentWith != null)
                {
                    bool isWitnessFullNameHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITNESSFULLNAME") == UIAccessLevel.Hidden;
                    if (!isWitnessFullNameHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitnessFullName.ToString()))
                    {
                        sb.Append(" Трите имена: " + declarationOfAccidentWith.WitnessFullName.ToString());
                    }

                    bool isWitnessCityHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITCITYID") == UIAccessLevel.Hidden;
                    if (!isWitnessCityHidden)
                    {
                        if (declarationOfAccidentWith.City != null && declarationOfAccidentWith.City.CityId > 0)
                        {
                            List<int> listResultWith = DeclarationOfAccidentUtil.GetListForEmployerCityId(declarationOfAccidentWith.City.CityId, CurrentUser);
                            string regionName = RegionUtil.GetRegion(listResultWith[0], CurrentUser).RegionName.ToString();
                            if (!String.IsNullOrEmpty(regionName))
                            {
                                if (sb.ToString() != "")
                                {
                                    sb.Append(", Област: " + regionName);
                                }
                                else
                                {
                                    sb.Append(" Област: " + regionName);
                                }
                            }

                            string municipalityName = MunicipalityUtil.GetMunicipality(listResultWith[1], CurrentUser).MunicipalityName.ToString();
                            if (!String.IsNullOrEmpty(municipalityName))
                            {
                                if (sb.ToString() != "")
                                {
                                    sb.Append(", Община: " + municipalityName);
                                }
                                else
                                {
                                    sb.Append(" Община: " + municipalityName);
                                }
                            }

                            if (!String.IsNullOrEmpty(declarationOfAccidentWith.City.CityName))
                            {
                                if (sb.ToString() != "")
                                {
                                    sb.Append(", Град: " + declarationOfAccidentWith.City.CityName);
                                }
                                else
                                {
                                    sb.Append(" Град: " + declarationOfAccidentWith.City.CityName);
                                }
                            }

                            if (sb.ToString() != "")
                            {
                                sb.Append(", п.к." + listResultWith[2].ToString());
                            }
                            else
                            {
                                sb.Append(" п.к." + listResultWith[2].ToString());
                            }
                        }
                    }

                    bool isWitnessStreetHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITSTREET") == UIAccessLevel.Hidden;
                    if (!isWitnessStreetHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitStreet.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", Ул: " + declarationOfAccidentWith.WitStreet.ToString());
                        }
                        else
                        {
                            sb.Append(" Ул: " + declarationOfAccidentWith.WitStreet.ToString());
                        }
                    }

                    bool isWitnessStreetNumHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITSTREETNUM") == UIAccessLevel.Hidden;
                    if (!isWitnessStreetNumHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitStreetNum.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", №: " + declarationOfAccidentWith.WitStreetNum.ToString());
                        }
                        else
                        {
                            sb.Append(" №: " + declarationOfAccidentWith.WitStreetNum.ToString());
                        }
                    }

                    bool isWitnessDistrictHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITDISTRICT") == UIAccessLevel.Hidden;
                    if (!isWitnessDistrictHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitDistrict.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", Жк.:" + declarationOfAccidentWith.WitDistrict.ToString());
                        }
                        else
                        {
                            sb.Append(" Жк.:" + declarationOfAccidentWith.WitDistrict.ToString());
                        }
                    }

                    bool isWitnessBlockHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITBLOCK") == UIAccessLevel.Hidden;
                    if (!isWitnessBlockHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitBlock.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", Бл.:" + declarationOfAccidentWith.WitBlock.ToString());
                        }
                        else
                        {
                            sb.Append(" Бл.:" + declarationOfAccidentWith.WitBlock.ToString());
                        }
                    }

                    bool isWitnessEntranceHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITENTRANCE") == UIAccessLevel.Hidden;
                    if (!isWitnessEntranceHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitEntrance.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", Вх.: " + declarationOfAccidentWith.WitEntrance.ToString());
                        }
                        else
                        {
                            sb.Append(" Вх.: " + declarationOfAccidentWith.WitEntrance.ToString());
                        }
                    }

                    bool isWitnessFloorHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITFLOOR") == UIAccessLevel.Hidden;
                    if (!isWitnessFloorHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitFloor.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", Ет.:" + declarationOfAccidentWith.WitFloor.ToString());
                        }
                        else
                        {
                            sb.Append(" Ет.:" + declarationOfAccidentWith.WitFloor.ToString());
                        }
                    }

                    bool isWitnessAptHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITAPT") == UIAccessLevel.Hidden;
                    if (!isWitnessAptHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitApt.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", Ап.:" + declarationOfAccidentWith.WitApt.ToString());
                        }
                        else
                        {
                            sb.Append(" Ап.:" + declarationOfAccidentWith.WitApt.ToString());
                        }
                    }

                    bool isWitnessPhoneHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITPHONE") == UIAccessLevel.Hidden;
                    if (!isWitnessPhoneHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitPhone.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", Телефон: " + declarationOfAccidentWith.WitPhone.ToString());
                        }
                        else
                        {
                            sb.Append(" Телефон: " + declarationOfAccidentWith.WitPhone.ToString());
                        }
                    }

                    bool isWitnessFaxHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITFAX") == UIAccessLevel.Hidden;
                    if (!isWitnessFaxHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitFax.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", Факс: " + declarationOfAccidentWith.WitFax.ToString());
                        }
                        else
                        {
                            sb.Append(" Факс: " + declarationOfAccidentWith.WitFax.ToString());
                        }
                    }

                    bool isWitnessEmailHidden = this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC_WITH_WITEMAIL") == UIAccessLevel.Hidden;
                    if (!isWitnessEmailHidden && !String.IsNullOrEmpty(declarationOfAccidentWith.WitEmail.ToString()))
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(", E-mail: " + declarationOfAccidentWith.WitEmail.ToString());
                        }
                        else
                        {
                            sb.Append(" E-mail: " + declarationOfAccidentWith.WitEmail.ToString());
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private int HowManyLines(string str, string newLine, int firstLineWidth, int lineWidth)
        {
            if (str == null)
                str = "";

            int lines = 0;

            if (firstLineWidth == 0)
                firstLineWidth = lineWidth;

            int i = 0;

            while (str != "")
            {
                i++;
                int width = (i == 1 ? firstLineWidth : lineWidth);

                if (str.IndexOf(newLine) >= 0 && str.IndexOf(newLine) <= width)
                {
                    str = str.Substring(str.IndexOf(newLine) + newLine.Length);
                    lines++;
                    continue;
                }

                if (str.Length >= width)
                {
                    str = str.Substring(width);
                    lines++;
                    continue;
                }

                if (str != "")
                {
                    str = "";
                    lines++;
                    continue;
                }
            }

            if (lines == 0)
                lines = 1;

            return lines;
        }
    }
}
