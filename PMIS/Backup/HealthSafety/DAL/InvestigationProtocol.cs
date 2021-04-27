using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Text;
using PMIS.Common;


namespace PMIS.HealthSafety.Common
{

    //This class represents all fileds from AddEditInvestigationProtocol.aspx page and ManageInvestigationProtocol.aspx pages

    public class InvestigationProtocol : BaseDbObject
    {
        //Declare private fileds
        private int investigaitonProtocolId;
        private string investigaitonProtocolNumber;
        private DateTime? invProtDate;
        private int? declarationId;
        private DateTime? dateFrom;
        private DateTime? dateTo;
        private string legalReason;
        private string orderNum;
        private string commissionChairman;
        private string commissionMember1;
        private string commissionMember2;
        private string commissionMember3;
        private string commissionMember4;
        private string commissionMember5;
        private string injured;
        private string accidentDateAndPlace;
        private string witnesses;
        private string jobGeneralDesc;
        private string specificTaskActivity;
        private string deviationOfNormalActivity;
        private string injuryDetails;
        private string analysisOfAccidentCauses;
        private string legalViolations;
        private string itruders;
        private string actionsToAvoid;
        private string enclosures;
        private DeclarationOfAccident declarationOfAccident = null;

        //Declare public properties
        public int InvestigaitonProtocolId
        {
            get { return investigaitonProtocolId; }
            set { investigaitonProtocolId = value; }
        }
        public string InvestigaitonProtocolNumber
        {
            get { return investigaitonProtocolNumber; }
            set { investigaitonProtocolNumber = value; }
        }
        public DateTime? InvProtDate
        {
            get { return invProtDate; }
            set { invProtDate = value; }
        }
        public int? DeclarationId
        {
            get { return declarationId; }
            set { declarationId = value; }
        }
        public DateTime? DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }
        public DateTime? DateTo
        {
            get { return dateTo; }
            set { dateTo = value; }
        }
        public string LegalReason
        {
            get { return legalReason; }
            set { legalReason = value; }
        }
        public string OrderNum
        {
            get { return orderNum; }
            set { orderNum = value; }
        }
        public string CommissionChairman
        {
            get { return commissionChairman; }
            set { commissionChairman = value; }
        }
        public string CommissionMember1
        {
            get { return commissionMember1; }
            set { commissionMember1 = value; }
        }
        public string CommissionMember2
        {
            get { return commissionMember2; }
            set { commissionMember2 = value; }
        }
        public string CommissionMember3
        {
            get { return commissionMember3; }
            set { commissionMember3 = value; }
        }
        public string CommissionMember4
        {
            get { return commissionMember4; }
            set { commissionMember4 = value; }
        }
        public string CommissionMember5
        {
            get { return commissionMember5; }
            set { commissionMember5 = value; }
        }
        public string Injured
        {
            get { return injured; }
            set { injured = value; }
        }
        public string AccidentDateAndPlace
        {
            get { return accidentDateAndPlace; }
            set { accidentDateAndPlace = value; }
        }
        public string Witnesses
        {
            get { return witnesses; }
            set { witnesses = value; }
        }
        public string JobGeneralDesc
        {
            get { return jobGeneralDesc; }
            set { jobGeneralDesc = value; }
        }
        public string SpecificTaskActivity
        {
            get { return specificTaskActivity; }
            set { specificTaskActivity = value; }
        }
        public string DeviationOfNormalActivity
        {
            get { return deviationOfNormalActivity; }
            set { deviationOfNormalActivity = value; }
        }
        public string InjuryDetails
        {
            get { return injuryDetails; }
            set { injuryDetails = value; }
        }
        public string AnalysisOfAccidentCauses
        {
            get { return analysisOfAccidentCauses; }
            set { analysisOfAccidentCauses = value; }
        }
        public string LegalViolations
        {
            get { return legalViolations; }
            set { legalViolations = value; }
        }
        public string Itruders
        {
            get { return itruders; }
            set { itruders = value; }
        }
        public string ActionsToAvoid
        {
            get { return actionsToAvoid; }
            set { actionsToAvoid = value; }
        }
        public string Enclosures
        {
            get { return enclosures; }
            set { enclosures = value; }
        }

        public DeclarationOfAccident DeclarationOfAccident
        {
            get
            {
                if (declarationOfAccident == null)
                {
                    declarationOfAccident = DeclarationOfAccidentUtil.GetDeclarationOfAccidentForInvProtocol(declarationId, CurrentUser);
                }

                return declarationOfAccident;
            }
            set { declarationOfAccident = value; }
        }


        public InvestigationProtocol(int _investigaitonProtocolId, User currentUser)
            : base(currentUser)
        {
            investigaitonProtocolId = _investigaitonProtocolId;
        }
        public InvestigationProtocol(User currentUser)
            : base(currentUser)
        {
            investigaitonProtocolId = 0;
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class InvestigationProtocolFilter
    {
        string investigaitonProtocolNumber;

        public string InvestigaitonProtocolNumber
        {
            get { return investigaitonProtocolNumber; }
            set { investigaitonProtocolNumber = value; }
        }
        DateTime? invProtDateFrom;

        public DateTime? InvProtDateFrom
        {
            get { return invProtDateFrom; }
            set { invProtDateFrom = value; }
        }
        DateTime? invProtDateTo;

        public DateTime? InvProtDateTo
        {
            get { return invProtDateTo; }
            set { invProtDateTo = value; }
        }
        string workerFullName;

        public string WorkerFullName
        {
            get { return workerFullName; }
            set { workerFullName = value; }
        }
        DateTime? accDateTimeFrom;

        public DateTime? AccDateTimeFrom
        {
            get { return accDateTimeFrom; }
            set { accDateTimeFrom = value; }
        }
        DateTime? accDateTimeTo;

        public DateTime? AccDateTimeTo
        {
            get { return accDateTimeTo; }
            set { accDateTimeTo = value; }
        }
        private int orderBy;

        public int OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }
        private int pageIndex;

        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        private int pageCount;

        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }

    }

    //This class stored methods for looking and changes Investigation Protocol
    public static class InvestigationProtocolUtil
    {
        //Get Investigation Protocol
        public static InvestigationProtocol GetInvestigationProtocol(int investigaitonProtocolId, User currentUser)
        {
            //Create InvestigationProtocol object
            InvestigationProtocol investigationProtocol = null;

            string SQL = "";

            if (investigaitonProtocolId == 0)
            {
                return investigationProtocol;  //return null object
            }

            //Connec to the database
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_INVPROTOCOLS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                //Write Oracle Statement
                SQL = @"SELECT a.InvestigaitonProtocolid,
                               a.InvestigaitonProtocolNumber,
                               a.InvProtDate,
                               a.DeclarationId,
                               a.DateFrom,
                               a.DateTo,
                               a.LegalReason,
                               a.OrderNum,
                               a.CommissionChairman,
                               a.CommissionMember1,
                               a.CommissionMember2,
                               a.CommissionMember3,
                               a.CommissionMember4,
                               a.CommissionMember5,
                               a.Injured,
                               a.AccidentDateAndPlace,
                               a.Witnesses,
                               a.JobGeneralDesc,
                               a.SpecificTaskActivity,
                               a.DeviationOfNormalActivity,
                               a.InjuryDetails,
                               a.AnalysisOfAccidentcauses,
                               a.LegalViolations,
                               a.Itruders,
                               a.ActionsToAvoid,
                               a.Enclosures,
                               a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                        FROM PMIS_HS.InvestigationProtocols a
                        WHERE a.InvestigaitonProtocolId = :investigaitonProtocolId " + where;

                //Create comand object
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "investigaitonProtocolId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = investigaitonProtocolId;

                cmd.Parameters.Add(param);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())   // we have data
                {
                    //Fill obect with data from data reader
                    investigationProtocol = new InvestigationProtocol(investigaitonProtocolId, currentUser);

                    investigationProtocol.InvestigaitonProtocolNumber = dr["InvestigaitonProtocolNumber"].ToString();
                    if (dr["InvProtDate"] is DateTime) investigationProtocol.InvProtDate = (DateTime)dr["InvProtDate"];

                    if (DBCommon.IsInt(dr["DeclarationId"])) investigationProtocol.DeclarationId = DBCommon.GetInt(dr["DeclarationId"]);

                    if (dr["DateFrom"] is DateTime) investigationProtocol.DateFrom = (DateTime)dr["DateFrom"];
                    if (dr["DateTo"] is DateTime) investigationProtocol.DateTo = (DateTime)dr["DateTo"];
                    investigationProtocol.LegalReason = dr["LegalReason"].ToString();
                    investigationProtocol.OrderNum = dr["OrderNum"].ToString();
                    investigationProtocol.CommissionChairman = dr["CommissionChairman"].ToString();
                    investigationProtocol.CommissionMember1 = dr["CommissionMember1"].ToString();
                    investigationProtocol.CommissionMember2 = dr["CommissionMember2"].ToString();
                    investigationProtocol.CommissionMember3 = dr["CommissionMember3"].ToString();
                    investigationProtocol.CommissionMember4 = dr["CommissionMember4"].ToString();
                    investigationProtocol.CommissionMember5 = dr["CommissionMember5"].ToString();
                    investigationProtocol.Injured = dr["Injured"].ToString();
                    investigationProtocol.AccidentDateAndPlace = dr["AccidentDateAndPlace"].ToString();
                    investigationProtocol.Witnesses = dr["Witnesses"].ToString();
                    investigationProtocol.JobGeneralDesc = dr["JobGeneralDesc"].ToString();
                    investigationProtocol.SpecificTaskActivity = dr["SpecificTaskActivity"].ToString();
                    investigationProtocol.DeviationOfNormalActivity = dr["DeviationOfNormalActivity"].ToString();
                    investigationProtocol.InjuryDetails = dr["InjuryDetails"].ToString();
                    investigationProtocol.AnalysisOfAccidentCauses = dr["AnalysisOfAccidentCauses"].ToString();
                    investigationProtocol.LegalViolations = dr["LegalViolations"].ToString();
                    investigationProtocol.Itruders = dr["Itruders"].ToString();
                    investigationProtocol.ActionsToAvoid = dr["ActionsToAvoid"].ToString();
                    investigationProtocol.Enclosures = dr["Enclosures"].ToString();

                    //Extrat created and last modified fields from a data reader. Coomon for all objects
                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, investigationProtocol);

                    //DeclarationOfAccident declarationOfAccident = new DeclarationOfAccident();
                    //declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName = dr["workerFullName"].ToString();
                    //if (dr["AccDateTime"] is DateTime) declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime = (DateTime)dr["AccDateTime"];
                    //investigationProtocol.DeclarationOfAccident = declarationOfAccident;

                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return investigationProtocol;
        }

        //Perform INSERT/UPDATE and return true/false
        public static bool SaveInvestigationProtocol(InvestigationProtocol investigationProtocol, User currentUser, Change changeEntry)
        {
            ChangeEvent changeEvent;
            string SQL = "";
            bool result = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (investigationProtocol.InvestigaitonProtocolId == 0) // Insert
                {

                    SQL += @"INSERT INTO PMIS_HS.INVESTIGATIONPROTOCOLS (
                                investigaitonProtocolNumber, 
                                invProtDate, 
                                declarationId, 
                                dateFrom, 
                                dateTo, 
                                legalReason, 
                                orderNum, 
                                commissionChairman, 
                                commissionMember1, 
                                commissionMember2, 
                                commissionMember3, 
                                commissionMember4, 
                                commissionMember5, 
                                injured, 
                                accidentDateAndPlace, 
                                Witnesses, 
                                jobGeneralDesc, 
                                specificTaskActivity, 
                                deviationOfNormalActivity, 
                                injuryDetails, 
                                analysisOfAccidentCauses, 
                                legalViolations, 
                                itruders, 
                                actionsToAvoid, 
                                enclosures,
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (
                                :investigaitonProtocolNumber, 
                                :invProtDate, 
                                :declarationId, 
                                :dateFrom, 
                                :dateTo, 
                                :legalReason, 
                                :orderNum, 
                                :commissionChairman, 
                                :commissionMember1, 
                                :commissionMember2, 
                                :commissionMember3, 
                                :commissionMember4, 
                                :commissionMember5, 
                                :injured, 
                                :accidentDateAndPlace, 
                                :Witnesses, 
                                :jobGeneralDesc, 
                                :specificTaskActivity, 
                                :deviationOfNormalActivity, 
                                :injuryDetails, 
                                :analysisOfAccidentCauses, 
                                :legalViolations, 
                                :itruders, 
                                :actionsToAvoid, 
                                :enclosures,
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);
                         
             SELECT PMIS_HS.INVPROTOCOLS_ID_SEQ.currval INTO :investigaitonProtocolId FROM dual;  ";

                    //Create obect using log for INSERT records
                    changeEvent = new ChangeEvent("HS_InvProt_AddProtocol", "", null, null, currentUser);

                    //Fill object with data
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtNumber", "",
                                     investigationProtocol.InvestigaitonProtocolNumber, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDate", "",
                        CommonFunctions.FormatDate(investigationProtocol.InvProtDate.ToString()), currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDateFrom", "",
                               CommonFunctions.FormatDate(investigationProtocol.DateFrom.ToString()), currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDateТо", "",
                               CommonFunctions.FormatDate(investigationProtocol.DateTo.ToString()), currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_LegalReason", "",
                                                      investigationProtocol.LegalReason, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ОrderNum", "",
                                                      investigationProtocol.OrderNum, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionChairman", "", investigationProtocol.CommissionChairman, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember1", "", investigationProtocol.CommissionMember1, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember2", "", investigationProtocol.CommissionMember2, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember3", "", investigationProtocol.CommissionMember3, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember4", "", investigationProtocol.CommissionMember4, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember5", "", investigationProtocol.CommissionMember5, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Injured", "",
                                                      investigationProtocol.Injured, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AccidentDateAndPlace", "", investigationProtocol.AccidentDateAndPlace, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Witnesses", "",
                                                      investigationProtocol.Witnesses, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_JobGeneralDesc", "",
                                                      investigationProtocol.JobGeneralDesc, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_SpecificTaskActivity", "", investigationProtocol.SpecificTaskActivity, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_DeviationOfNormalActivity", "", investigationProtocol.DeviationOfNormalActivity, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_InjuryDetails", "",
                                                      investigationProtocol.InjuryDetails, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AnalysisOfAccidentCauses", "", investigationProtocol.AnalysisOfAccidentCauses, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_LegalViolations", "", investigationProtocol.LegalViolations, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Itruders", "",
                                                      investigationProtocol.Itruders, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ActionsToAvoid", "",
                                                      investigationProtocol.ActionsToAvoid, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Еnclosures", "",
                                                      investigationProtocol.Enclosures, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AccDeclarationNumber", "",
                        investigationProtocol.DeclarationId.ToString(), currentUser));

                }
                else //Update
                {

                    SQL += @" UPDATE PMIS_HS.INVESTIGATIONPROTOCOLS  SET

                                 investigaitonProtocolNumber = :investigaitonProtocolNumber, 
                                 invProtDate = :invProtDate, 
                                 declarationId = :declarationId, 
                                 dateFrom = :dateFrom, 
                                 dateTo = :dateTo, 
                                 legalReason = :legalReason, 
                                 orderNum = :orderNum, 
                                 commissionChairman = :commissionChairman, 
                                 commissionMember1 = :commissionMember1, 
                                 commissionMember2 = :commissionMember2, 
                                 commissionMember3 = :commissionMember3, 
                                 commissionMember4 = :commissionMember4, 
                                 commissionMember5 = :commissionMember5, 
                                 injured = :injured, 
                                 accidentDateAndPlace = :accidentDateAndPlace, 
                                 Witnesses = :Witnesses, 
                                 jobGeneralDesc = :jobGeneralDesc, 
                                 specificTaskActivity = :specificTaskActivity, 
                                 deviationOfNormalActivity = :deviationOfNormalActivity, 
                                 injuryDetails = :injuryDetails, 
                                 analysisOfAccidentCauses = :analysisOfAccidentCauses, 
                                 legalViolations = :legalViolations, 
                                 itruders = :itruders, 
                                 actionsToAvoid = :actionsToAvoid, 
                                 enclosures = :enclosures,
                                 LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedBy
                                                       ELSE LastModifiedBy
                                                  END, 
                                 LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                         THEN :LastModifiedDate
                                                         ELSE LastModifiedDate
                                                    END

                             WHERE investigaitonProtocolId = :investigaitonProtocolId ;";


                    //Create InvestigationProtocol object using GetInvestigationProtocol method
                    InvestigationProtocol oldInvestigationProtocol = GetInvestigationProtocol(investigationProtocol.InvestigaitonProtocolId, currentUser);

                    //Create obect using log for UPDATE records
                    changeEvent = new ChangeEvent("HS_InvProt_EditProtocol", "", null, null, currentUser);

                    //Fill object with data
                    if (oldInvestigationProtocol.InvestigaitonProtocolNumber.Trim() !=
                           investigationProtocol.InvestigaitonProtocolNumber.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtNumber",
                                      oldInvestigationProtocol.InvestigaitonProtocolNumber,
                                         investigationProtocol.InvestigaitonProtocolNumber, currentUser));
                    }

                    if (oldInvestigationProtocol.InvProtDate !=
                           investigationProtocol.InvProtDate)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDate",
                            CommonFunctions.FormatDate(oldInvestigationProtocol.InvProtDate.ToString()), CommonFunctions.FormatDate(investigationProtocol.InvProtDate.ToString()), currentUser));
                    }
                    if (oldInvestigationProtocol.DateFrom !=
                           investigationProtocol.DateFrom)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDateFrom",
                                CommonFunctions.FormatDate(oldInvestigationProtocol.DateFrom.ToString()),
                                   CommonFunctions.FormatDate(investigationProtocol.DateFrom.ToString()), currentUser));
                    }

                    if (oldInvestigationProtocol.DateTo !=
                           investigationProtocol.DateTo)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDateТо",
                                CommonFunctions.FormatDate(oldInvestigationProtocol.DateTo.ToString()),
                                   CommonFunctions.FormatDate(investigationProtocol.DateTo.ToString()), currentUser));
                    }

                    if (oldInvestigationProtocol.LegalReason !=
                           investigationProtocol.LegalReason)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_LegalReason",
                                                       oldInvestigationProtocol.LegalReason,
                                                          investigationProtocol.LegalReason, currentUser));
                    }

                    if (oldInvestigationProtocol.OrderNum !=
                           investigationProtocol.OrderNum)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ОrderNum",
                                                       oldInvestigationProtocol.OrderNum,
                                                          investigationProtocol.OrderNum, currentUser));
                    }

                    if (oldInvestigationProtocol.CommissionChairman !=
                           investigationProtocol.CommissionChairman)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionChairman", oldInvestigationProtocol.CommissionChairman,
                                                          investigationProtocol.CommissionChairman, currentUser));
                    }

                    if (oldInvestigationProtocol.CommissionMember1 !=
                           investigationProtocol.CommissionMember1)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember1", oldInvestigationProtocol.CommissionMember1,
                                                          investigationProtocol.CommissionMember1, currentUser));
                    }

                    if (oldInvestigationProtocol.CommissionMember2 !=
                           investigationProtocol.CommissionMember2)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember2", oldInvestigationProtocol.CommissionMember2,
                                                          investigationProtocol.CommissionMember2, currentUser));
                    }

                    if (oldInvestigationProtocol.CommissionMember3 !=
                           investigationProtocol.CommissionMember3)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember3", oldInvestigationProtocol.CommissionMember3,
                                                          investigationProtocol.CommissionMember3, currentUser));
                    }

                    if (oldInvestigationProtocol.CommissionMember4 !=
                           investigationProtocol.CommissionMember4)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember4", oldInvestigationProtocol.CommissionMember4,
                                                          investigationProtocol.CommissionMember4, currentUser));
                    }

                    if (oldInvestigationProtocol.CommissionMember5 !=
                           investigationProtocol.CommissionMember5)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember5", oldInvestigationProtocol.CommissionMember5,
                                                          investigationProtocol.CommissionMember5, currentUser));
                    }

                    if (oldInvestigationProtocol.Injured !=
                           investigationProtocol.Injured)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Injured",
                                                       oldInvestigationProtocol.Injured,
                                                          investigationProtocol.Injured, currentUser));
                    }

                    if (oldInvestigationProtocol.AccidentDateAndPlace !=
                           investigationProtocol.AccidentDateAndPlace)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AccidentDateAndPlace", oldInvestigationProtocol.AccidentDateAndPlace, investigationProtocol.AccidentDateAndPlace, currentUser));
                    }

                    if (oldInvestigationProtocol.Witnesses !=
                           investigationProtocol.Witnesses)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Witnesses",
                                                       oldInvestigationProtocol.Witnesses,
                                                          investigationProtocol.Witnesses, currentUser));
                    }

                    if (oldInvestigationProtocol.JobGeneralDesc !=
                           investigationProtocol.JobGeneralDesc)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_JobGeneralDesc", oldInvestigationProtocol.JobGeneralDesc,
                                                          investigationProtocol.JobGeneralDesc, currentUser));
                    }

                    if (oldInvestigationProtocol.SpecificTaskActivity !=
                           investigationProtocol.SpecificTaskActivity)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_SpecificTaskActivity", oldInvestigationProtocol.SpecificTaskActivity, investigationProtocol.SpecificTaskActivity, currentUser));
                    }

                    if (oldInvestigationProtocol.DeviationOfNormalActivity !=
                           investigationProtocol.DeviationOfNormalActivity)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_DeviationOfNormalActivity", oldInvestigationProtocol.DeviationOfNormalActivity, investigationProtocol.DeviationOfNormalActivity, currentUser));
                    }

                    if (oldInvestigationProtocol.InjuryDetails !=
                           investigationProtocol.InjuryDetails)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_InjuryDetails", oldInvestigationProtocol.InjuryDetails,
                                                          investigationProtocol.InjuryDetails, currentUser));
                    }

                    if (oldInvestigationProtocol.AnalysisOfAccidentCauses !=
                           investigationProtocol.AnalysisOfAccidentCauses)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AnalysisOfAccidentCauses", oldInvestigationProtocol.AnalysisOfAccidentCauses, investigationProtocol.AnalysisOfAccidentCauses, currentUser));
                    }

                    if (oldInvestigationProtocol.LegalViolations !=
                           investigationProtocol.LegalViolations)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_LegalViolations", oldInvestigationProtocol.LegalViolations,
                                                          investigationProtocol.LegalViolations, currentUser));
                    }

                    if (oldInvestigationProtocol.Itruders !=
                           investigationProtocol.Itruders)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Itruders",
                                                       oldInvestigationProtocol.Itruders,
                                                          investigationProtocol.Itruders, currentUser));
                    }

                    if (oldInvestigationProtocol.ActionsToAvoid !=
                           investigationProtocol.ActionsToAvoid)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ActionsToAvoid", oldInvestigationProtocol.ActionsToAvoid,
                                                          investigationProtocol.ActionsToAvoid, currentUser));
                    }


                    if (oldInvestigationProtocol.Enclosures !=
                           investigationProtocol.Enclosures)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Еnclosures",
                                                       oldInvestigationProtocol.Enclosures,
                                                          investigationProtocol.Enclosures, currentUser));
                    }

                    if (oldInvestigationProtocol.DeclarationId != investigationProtocol.DeclarationId)
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AccDeclarationNumber",
                            oldInvestigationProtocol.DeclarationId.ToString(),
                               investigationProtocol.DeclarationId.ToString(), currentUser));
                    }

                }

                SQL += @" END;";

                SQL = DBCommon.FixNewLines(SQL);

                //Create command object
                OracleCommand cmd = new OracleCommand(SQL, conn);

                //Add Special Parameter using to hold New investigaitonProtocolId for Insert 
                //or using in Where clause for Update

                OracleParameter investigaitonProtocolId = new OracleParameter();
                investigaitonProtocolId.ParameterName = "investigaitonProtocolId";
                investigaitonProtocolId.OracleType = OracleType.Number;

                if (investigationProtocol.InvestigaitonProtocolId != 0) // we have Update
                {
                    investigaitonProtocolId.Direction = ParameterDirection.InputOutput;
                    investigaitonProtocolId.Value = investigationProtocol.InvestigaitonProtocolId;
                }
                else
                {
                    investigaitonProtocolId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(investigaitonProtocolId);


                //Add Parameters common for Insert and Update

                OracleParameter param = new OracleParameter();
                param.ParameterName = "investigaitonProtocolNumber";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.InvestigaitonProtocolNumber))
                {
                    param.Value = investigationProtocol.InvestigaitonProtocolNumber;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "invProtDate";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.DateTime;
                if (investigationProtocol.InvProtDate.HasValue)
                {
                    param.Value = investigationProtocol.InvProtDate;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "declarationId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                if (investigationProtocol.DeclarationId.HasValue)
                {
                    param.Value = investigationProtocol.DeclarationId;
                }
                else
                {
                    param.Value = DBNull.Value;
                }

                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "dateFrom";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.DateTime;
                if (investigationProtocol.DateFrom.HasValue)
                    param.Value = investigationProtocol.DateFrom;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "dateTo";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.DateTime;
                if (investigationProtocol.DateTo.HasValue)
                    param.Value = investigationProtocol.DateTo;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "legalReason";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.LegalReason))
                    param.Value = investigationProtocol.LegalReason;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "orderNum";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.OrderNum))
                    param.Value = investigationProtocol.OrderNum;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "commissionChairman";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.CommissionChairman))
                    param.Value = investigationProtocol.CommissionChairman;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "commissionMember1";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.CommissionMember1))
                    param.Value = investigationProtocol.CommissionMember1;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "commissionMember2";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.CommissionMember2))
                    param.Value = investigationProtocol.CommissionMember2;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "commissionMember3";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.CommissionMember3))
                    param.Value = investigationProtocol.CommissionMember3;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "commissionMember4";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.CommissionMember4))
                    param.Value = investigationProtocol.CommissionMember4;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "commissionMember5";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.CommissionMember5))
                    param.Value = investigationProtocol.CommissionMember5;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "injured";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.Injured))
                    param.Value = investigationProtocol.Injured;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "accidentDateAndPlace";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.AccidentDateAndPlace))
                    param.Value = investigationProtocol.AccidentDateAndPlace;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "Witnesses";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.Witnesses))
                    param.Value = investigationProtocol.Witnesses;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "jobGeneralDesc";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.JobGeneralDesc))
                    param.Value = investigationProtocol.JobGeneralDesc;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "specificTaskActivity";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.SpecificTaskActivity))
                    param.Value = investigationProtocol.SpecificTaskActivity;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "deviationOfNormalActivity";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.DeviationOfNormalActivity))
                    param.Value = investigationProtocol.DeviationOfNormalActivity;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "injuryDetails";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.InjuryDetails))
                    param.Value = investigationProtocol.InjuryDetails;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "analysisOfAccidentCauses";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.AnalysisOfAccidentCauses))
                    param.Value = investigationProtocol.AnalysisOfAccidentCauses;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "legalViolations";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.LegalViolations))
                    param.Value = investigationProtocol.LegalViolations;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "itruders";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.Itruders))
                    param.Value = investigationProtocol.Itruders;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "actionsToAvoid";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.ActionsToAvoid))
                    param.Value = investigationProtocol.ActionsToAvoid;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "enclosures";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!string.IsNullOrEmpty(investigationProtocol.Enclosures))
                    param.Value = investigationProtocol.Enclosures;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (investigationProtocol.InvestigaitonProtocolId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                //Run Query
                cmd.ExecuteNonQuery();
                //Set outputparameter
                investigationProtocol.InvestigaitonProtocolId = DBCommon.GetInt(investigaitonProtocolId.Value);

                result = true;

                if (changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                }
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Perform DELETEand return true/false
        public static bool DeleteInvestigationProtocol(int investigaitonProtocolId, User currentUser, Change changeEntry)
        {
            string SQL = "";
            bool isDeleted = false;

            //Create Old InvestigationProtocol obect using GetInvestigationProtocol method
            InvestigationProtocol oldInvestigationProtocol = GetInvestigationProtocol(investigaitonProtocolId, currentUser);
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();
            try
            {
                SQL = @"DELETE FROM PMIS_HS.INVESTIGATIONPROTOCOLS WHERE investigaitonProtocolId = :investigaitonProtocolId";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "investigaitonProtocolId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = investigaitonProtocolId;
                cmd.Parameters.Add(param);

                isDeleted = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (isDeleted) // Succes Delete Operation
            {
                //Create obect using log for DELETE operation
                ChangeEvent changeEvent = new ChangeEvent("HS_InvProt_DeleteProtocol", "", null, null, currentUser);

                if (!String.IsNullOrEmpty(oldInvestigationProtocol.InvestigaitonProtocolNumber))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtNumber",
                                 oldInvestigationProtocol.InvestigaitonProtocolNumber, "", currentUser));
                }

                if (oldInvestigationProtocol.InvProtDate.HasValue)
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDate",
                       CommonFunctions.FormatDate(oldInvestigationProtocol.InvProtDate.ToString()), "", currentUser));
                }

                if (oldInvestigationProtocol.DateFrom.HasValue)
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDateFrom",
            CommonFunctions.FormatDate(oldInvestigationProtocol.DateFrom.ToString()), "", currentUser));

                }

                if (oldInvestigationProtocol.DateTo.HasValue)
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ProtDateТо",
             CommonFunctions.FormatDate(oldInvestigationProtocol.DateTo.ToString()), "", currentUser));
                }

                if (!String.IsNullOrEmpty(oldInvestigationProtocol.LegalReason))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_LegalReason",
                                                  oldInvestigationProtocol.LegalReason, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.OrderNum))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ОrderNum",
                                                  oldInvestigationProtocol.OrderNum, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.CommissionChairman))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionChairman",
                                                   oldInvestigationProtocol.CommissionChairman, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.CommissionMember1))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember1",
                                                   oldInvestigationProtocol.CommissionMember1, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.CommissionMember2))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember2",
                                                   oldInvestigationProtocol.CommissionMember2, "", currentUser));
                }

                if (!String.IsNullOrEmpty(oldInvestigationProtocol.CommissionMember3))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember3",
                                                   oldInvestigationProtocol.CommissionMember3, "", currentUser));
                }

                if (!String.IsNullOrEmpty(oldInvestigationProtocol.CommissionMember4))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember4",
                                                   oldInvestigationProtocol.CommissionMember4, "", currentUser));
                }

                if (!String.IsNullOrEmpty(oldInvestigationProtocol.CommissionMember5))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_CommissionMember5",
                                                   oldInvestigationProtocol.CommissionMember5, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.Injured))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Injured",
                                                   oldInvestigationProtocol.Injured, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.AccidentDateAndPlace))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AccidentDateAndPlace",
                        oldInvestigationProtocol.AccidentDateAndPlace, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.Witnesses))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Witnesses",
                                                   oldInvestigationProtocol.Witnesses, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.JobGeneralDesc))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_JobGeneralDesc",
                                                 oldInvestigationProtocol.JobGeneralDesc, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.SpecificTaskActivity))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_SpecificTaskActivity",
                        oldInvestigationProtocol.SpecificTaskActivity, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.DeviationOfNormalActivity))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_DeviationOfNormalActivity",
                        oldInvestigationProtocol.DeviationOfNormalActivity, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.InjuryDetails))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_InjuryDetails",
                                                   oldInvestigationProtocol.InjuryDetails, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.AnalysisOfAccidentCauses))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AnalysisOfAccidentCauses",
                        oldInvestigationProtocol.AnalysisOfAccidentCauses, "", currentUser));

                }

                if (!String.IsNullOrEmpty(oldInvestigationProtocol.LegalViolations))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_LegalViolations",
                                                  oldInvestigationProtocol.LegalViolations, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.Itruders))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Itruders",
                                                 oldInvestigationProtocol.Itruders, "", currentUser));
                }

                if (!String.IsNullOrEmpty(oldInvestigationProtocol.ActionsToAvoid))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_ActionsToAvoid",
                                                 oldInvestigationProtocol.ActionsToAvoid, "", currentUser));
                }


                if (!String.IsNullOrEmpty(oldInvestigationProtocol.Enclosures))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_Еnclosures",
                                                  oldInvestigationProtocol.Enclosures, "", currentUser));
                }


                if (oldInvestigationProtocol.DeclarationId.HasValue)
                {
                    changeEvent.AddDetail(new ChangeEventDetail("HS_InvProt_AccDeclarationNumber",
                      oldInvestigationProtocol.DeclarationId.ToString(), "", currentUser));
                }


                changeEntry.AddEvent(changeEvent);
            }
            return isDeleted;
        }

        //Get a list of all changes log records according to the provided filter
        public static List<InvestigationProtocol> GetAllInvestigationProtocols(InvestigationProtocolFilter filter, User currentUser)
        {
            string SQL = "";
            string sqlFilterOrderBy = "";
            string sqlFilterWhere = "";

            //Create list of InvestigationProtocol obect
            List<InvestigationProtocol> ListInvestigationProtocol = new List<InvestigationProtocol>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            try
            {
                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_INVPROTOCOLS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    sqlFilterWhere += " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                //Create order by filter
                switch (filter.OrderBy)
                {
                    case 1:
                        sqlFilterOrderBy += "a.investigaitonProtocolNumber ASC" + DBCommon.FixNullsOrder("ASC");
                        break;
                    case 2:
                        sqlFilterOrderBy += "a.invProtDate ASC" + DBCommon.FixNullsOrder("ASC");
                        break;
                    case 3:
                        sqlFilterOrderBy += "b.workerFullName ASC" + DBCommon.FixNullsOrder("ASC");
                        break;
                    case 4:
                        sqlFilterOrderBy += "b.AccDateTime ASC" + DBCommon.FixNullsOrder("ASC");
                        break;
                    case 5:
                        sqlFilterOrderBy += "a.investigaitonProtocolNumber DESC" + DBCommon.FixNullsOrder("DESC");
                        break;
                    case 6:
                        sqlFilterOrderBy += "a.invProtDate DESC" + DBCommon.FixNullsOrder("DESC");
                        break;
                    case 7:
                        sqlFilterOrderBy += "b.workerFullName DESC" + DBCommon.FixNullsOrder("DESC");
                        break;
                    case 8:
                        sqlFilterOrderBy += "b.AccDateTime DESC" + DBCommon.FixNullsOrder("DESC");
                        break;
                    default:
                        sqlFilterOrderBy += "a.investigaitonProtocolNumber ASC" + DBCommon.FixNullsOrder("ASC");
                        break;
                }

                //Create command object
                OracleCommand cmd = new OracleCommand();
                //Create parameter object
                OracleParameter param = new OracleParameter();

                //Configure where parameters
                //1. PrtocolNumber
                if (!String.IsNullOrEmpty(filter.InvestigaitonProtocolNumber))
                {
                    sqlFilterWhere += " AND UPPER(a.investigaitonProtocolNumber) LIKE :investigaitonProtocolNumber";
                    param = new OracleParameter();
                    param.ParameterName = "investigaitonProtocolNumber";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    param.Value = "%" + filter.InvestigaitonProtocolNumber.ToUpper() + "%";
                    cmd.Parameters.Add(param);
                }
                //2. ProtocolDateFrom <-> ProtocolDateTo
                if ((filter.InvProtDateFrom.HasValue) || (filter.InvProtDateTo.HasValue))
                {
                    if ((filter.InvProtDateFrom.HasValue) && (filter.InvProtDateTo.HasValue))
                    {
                        sqlFilterWhere += " AND a.invProtDate >= :invProtDateFrom AND  a.invProtDate < :invProtDateTo ";
                    }
                    else
                    {
                        if (filter.InvProtDateFrom.HasValue)
                        {
                            sqlFilterWhere += " AND a.invProtDate >= :invProtDateFrom ";
                        }

                        if (filter.InvProtDateTo.HasValue)
                        {
                            sqlFilterWhere += " AND a.invProtDate < :invProtDateTo";
                        }
                    }
                }

                if (filter.InvProtDateFrom.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "invProtDateFrom";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.InvProtDateFrom;
                    cmd.Parameters.Add(param);
                }

                if (filter.InvProtDateTo.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "invProtDateTo";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.InvProtDateTo.Value.AddDays(1);
                    cmd.Parameters.Add(param);
                }
                //3. Workername
                if (!String.IsNullOrEmpty(filter.WorkerFullName))
                {
                    sqlFilterWhere += " AND UPPER(b.workerFullName) LIKE :workerFullName";
                    param = new OracleParameter();
                    param.ParameterName = "workerFullName";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    param.Value = "%" + filter.WorkerFullName.ToUpper() + "%";
                    cmd.Parameters.Add(param);
                }

                //4. AccDateTimeFrom <-> AccDateTimeTo
                if ((filter.AccDateTimeFrom.HasValue) || (filter.AccDateTimeTo.HasValue))
                {
                    if ((filter.AccDateTimeFrom.HasValue) && (filter.AccDateTimeTo.HasValue))
                    {
                        sqlFilterWhere += " AND b.AccDateTime >= :accDateTimeFrom AND  b.AccDateTime < :accDateTimeTo ";
                    }
                    else
                    {
                        if (filter.AccDateTimeFrom.HasValue)
                        {
                            sqlFilterWhere += " AND b.AccDateTime >= :accDateTimeFrom ";
                        }

                        if (filter.AccDateTimeTo.HasValue)
                        {
                            sqlFilterWhere += " AND b.AccDateTime < :accDateTimeTo";
                        }
                    }
                }

                if (filter.AccDateTimeFrom.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "accDateTimeFrom";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.AccDateTimeFrom.Value;
                    cmd.Parameters.Add(param);
                }

                if (filter.AccDateTimeTo.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "accDateTimeTo";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.AccDateTimeTo.Value.AddDays(1);
                    cmd.Parameters.Add(param);
                }
                //Configure Sql statement
                SQL = @" SELECT b.COUNT, a.investigaitonProtocolId, a.investigaitonProtocolNumber, a.invProtDate,                                          a.workerFullName, a.AccDateTime,
                                         a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                         FROM (SELECT a.investigaitonProtocolId, a.investigaitonProtocolNumber, a.invProtDate, 
                                     b.workerFullName, b.AccDateTime, 
                                     a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                     RANK() OVER (ORDER BY " + sqlFilterOrderBy + @" , a.investigaitonProtocolId) as RowNumber 
                         FROM PMIS_HS.INVESTIGATIONPROTOCOLS a 
                                LEFT OUTER JOIN PMIS_HS.DECLARATIONSOFACCIDENT b
                                ON  a.declarationId = b.declarationId
                                WHERE 1=1 ";
                SQL += sqlFilterWhere;
                SQL += @") a
                        LEFT OUTER JOIN (SELECT COUNT(*) as COUNT
                                            FROM PMIS_HS.INVESTIGATIONPROTOCOLS  a 
                                            LEFT OUTER JOIN PMIS_HS.DECLARATIONSOFACCIDENT  b
                                            ON  a.declarationId = b.declarationId
                                        ) b ON 1=1 ";

                //Configure page filter if need
                if (filter.PageIndex > 0 && filter.PageCount > 0)
                {
                    string sqlPageWhere = "";
                    sqlPageWhere = " WHERE a.RowNumber BETWEEN (" + filter.PageIndex.ToString() + @" - 1) * " + filter.PageCount.ToString() + @" + 1 AND " + filter.PageIndex.ToString() + @" * " + filter.PageCount.ToString() + @" ";
                    SQL += sqlPageWhere;
                }

                SQL = DBCommon.FixNewLines(SQL);

                //Set connection and command text 
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = SQL;

                OracleDataReader dr = cmd.ExecuteReader();

                //Iterate the data reader object and construct business objects that are being added to the result list
                while (dr.Read())
                {
                    InvestigationProtocol investigationProtocol = new InvestigationProtocol(Convert.ToInt32(dr["investigaitonProtocolId"]), currentUser);

                    //Select Column to display

                    investigationProtocol.InvestigaitonProtocolNumber = dr["investigaitonProtocolNumber"].ToString();
                    if (dr["invProtDate"] is DateTime) investigationProtocol.InvProtDate = Convert.ToDateTime(dr["invProtDate"]);

                    DeclarationOfAccident DeclarationOfAccident = new DeclarationOfAccident(currentUser);
                    DeclarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName = dr["workerFullName"].ToString();
                    if (dr["AccDateTime"] is DateTime) DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime = Convert.ToDateTime(dr["AccDateTime"]);

                    investigationProtocol.DeclarationOfAccident = DeclarationOfAccident;

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, investigationProtocol);

                    ListInvestigationProtocol.Add(investigationProtocol);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return ListInvestigationProtocol;
        }

        //Get a count of all changes log records according to the provided filter
        public static int CountProtocols(InvestigationProtocolFilter filter, User currentUser)
        {
            int count = 0;
            string SQL = "";
            string SQLWHERE = "";

            //Create connection object
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            try
            {
                SQL = @"SELECT COUNT(*)  
                                        FROM PMIS_HS.INVESTIGATIONPROTOCOLS a
                                        LEFT OUTER JOIN PMIS_HS.DECLARATIONSOFACCIDENT b ON
                                        a.declarationId = b.declarationId 
                                        WHERE 1=1 ";

                //Create command object
                OracleCommand cmd = new OracleCommand();
                //Create parameter object
                OracleParameter param = new OracleParameter();

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_INVPROTOCOLS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    SQLWHERE += " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(filter.InvestigaitonProtocolNumber))
                {
                    SQLWHERE += " AND UPPER(a.investigaitonProtocolNumber) LIKE :investigaitonProtocolNumber";

                    param = new OracleParameter();
                    param.ParameterName = "investigaitonProtocolNumber";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    param.Value = "%" + filter.InvestigaitonProtocolNumber.ToUpper() + "%";
                    cmd.Parameters.Add(param);
                }

                if ((filter.InvProtDateFrom.HasValue) || (filter.InvProtDateTo.HasValue))
                {
                    if ((filter.InvProtDateFrom.HasValue) && (filter.InvProtDateTo.HasValue))
                    {
                        SQLWHERE += " AND a.invProtDate >= :invProtDateFrom AND  a.invProtDate < :invProtDateTo ";
                    }
                    else
                    {
                        if (filter.InvProtDateFrom.HasValue)
                        {
                            SQLWHERE += " AND a.invProtDate >= :invProtDateFrom ";
                        }

                        if (filter.InvProtDateTo.HasValue)
                        {
                            SQLWHERE += " AND a.invProtDate < :invProtDateTo";
                        }
                    }
                }


                if (filter.InvProtDateFrom.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "invProtDateFrom";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.InvProtDateFrom;
                    cmd.Parameters.Add(param);
                }

                if (filter.InvProtDateTo.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "invProtDateTo";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.InvProtDateTo.Value.AddDays(1);
                    cmd.Parameters.Add(param);
                }

                if (!String.IsNullOrEmpty(filter.WorkerFullName))
                {
                    SQLWHERE += " AND UPPER(b.workerFullName) LIKE :workerFullName";
                    param = new OracleParameter();
                    param.ParameterName = "workerFullName";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.VarChar;
                    param.Value = "%" + filter.WorkerFullName.ToUpper() + "%";
                    cmd.Parameters.Add(param);
                }



                if ((filter.AccDateTimeFrom.HasValue) || (filter.AccDateTimeTo.HasValue))
                {
                    if ((filter.AccDateTimeFrom.HasValue) && (filter.AccDateTimeTo.HasValue))
                    {
                        SQLWHERE += " AND b.AccDateTime >= :accDateTimeFrom AND  b.AccDateTime < :accDateTimeTo ";
                    }
                    else
                    {
                        if (filter.AccDateTimeFrom.HasValue)
                        {
                            SQLWHERE += " AND b.AccDateTime >= :accDateTimeFrom ";
                        }

                        if (filter.AccDateTimeTo.HasValue)
                        {
                            SQLWHERE += " AND b.AccDateTime < :accDateTimeTo";
                        }
                    }
                }


                if (filter.AccDateTimeFrom.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "accDateTimeFrom";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.AccDateTimeFrom.Value;
                    cmd.Parameters.Add(param);
                }

                if (filter.AccDateTimeTo.HasValue)
                {
                    param = new OracleParameter();
                    param.ParameterName = "accDateTimeTo";
                    param.Direction = ParameterDirection.Input;
                    param.OracleType = OracleType.DateTime;
                    param.Value = filter.AccDateTimeTo.Value.AddDays(1);
                    cmd.Parameters.Add(param);
                }

                SQL += SQLWHERE;

                SQL = DBCommon.FixNewLines(SQL);

                //Set connection and comand text to command object
                cmd.Connection = conn;
                cmd.CommandText = SQL;

                //Execute command and getnumber of row
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        //Get a count of Investigation Protocols for current declaration
        public static int CountProtocolsForDeclarationAcc(int declarationId, User currentUser)
        {
            int count = 0;
            //Create connection object
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            //Create command object
            OracleCommand cmd = new OracleCommand();
            //Create paramaeter object
            OracleParameter param = new OracleParameter();

            conn.Open();

            try
            {
                string SQL = @"select count(*) 
                             FROM PMIS_HS.INVESTIGATIONPROTOCOLS a
                             where a.declarationid = :declarationId ";

                //Fill parameter from filter
                param = new OracleParameter();
                param.ParameterName = "declarationId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = declarationId;
                cmd.Parameters.Add(param);

                SQL = DBCommon.FixNewLines(SQL);

                //Set connection and comand text to command object
                cmd.Connection = conn;
                cmd.CommandText = SQL;

                //Execute command and get number of row
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

    }
}
