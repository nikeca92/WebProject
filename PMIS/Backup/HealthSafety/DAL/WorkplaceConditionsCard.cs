using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class WorkplaceConditionsCard : BaseDbObject
    {
        private int workplaceConditionsCardId;        
        private int militaryUnitId;        
        private MilitaryUnit militaryUnit;        
        private string cardNumber;        
        private string jobType;        
        private int? workersCount;
        private decimal? complexAssessment;        
        private decimal? complexAssessmentPointValue;        
        private decimal? additionalReward;
        private List<WorkplaceConditionsCardItem> items;        

        public int WorkplaceConditionsCardId
        {
            get { return workplaceConditionsCardId; }
            set { workplaceConditionsCardId = value; }
        }

        public int MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get 
            {
                if (militaryUnit == null)
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(militaryUnitId, CurrentUser);

                return militaryUnit; 
            }
            set { militaryUnit = value; }
        }

        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        public string JobType
        {
            get { return jobType; }
            set { jobType = value; }
        }

        public int? WorkersCount
        {
            get { return workersCount; }
            set { workersCount = value; }
        }       

        public decimal? ComplexAssessment
        {
            get { return complexAssessment; }
            set { complexAssessment = value; }
        }

        public decimal? ComplexAssessmentPointValue
        {
            get { return complexAssessmentPointValue; }
            set { complexAssessmentPointValue = value; }
        }

        public decimal? AdditionalReward
        {
            get { return additionalReward; }
            set { additionalReward = value; }
        }

        public List<WorkplaceConditionsCardItem> Items
        {
            get 
            {
                if (items == null)
                    items = WorkplaceConditionsCardItemUtil.GetAllWorkplaceConditionsCardItemsByCard(workplaceConditionsCardId, CurrentUser);
              
                return items; 
            }
            set { items = value; }
        }

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public WorkplaceConditionsCard(User user)
            :base(user)
        {
        }  
    } 

    public static class WorkplaceConditionsCardUtil
    {
        public static WorkplaceConditionsCard GetWorkplaceConditionsCard(int workplaceConditionsCardId, User currentUser)
        {
            WorkplaceConditionsCard workplaceConditionsCard = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_WCONDCARDS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.MilitaryUnitID as MilitaryUnitID, a.CardNumber as CardNumber, a.JobType as JobType,
                                      a.WorkersCount as WorkersCount, a.ComplexAssessment as ComplexAssessment,
                                      a.ComplexAssessmentPointValue as ComplexAssessmentPointValue, a.AdditionalReward as AdditionalReward,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_HS.WorkplaceConditionsCards a                       
                               WHERE a.WorkplaceConditionsCardID = :WorkplaceConditionsCardID " + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("WorkplaceConditionsCardID", OracleType.Number).Value = workplaceConditionsCardId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    workplaceConditionsCard = new WorkplaceConditionsCard(currentUser);
                    workplaceConditionsCard.WorkplaceConditionsCardId = workplaceConditionsCardId;
                    workplaceConditionsCard.MilitaryUnitId = DBCommon.IsInt(dr["MilitaryUnitID"]) ? DBCommon.GetInt(dr["MilitaryUnitID"]) : 0;
                    workplaceConditionsCard.CardNumber = dr["CardNumber"].ToString();
                    workplaceConditionsCard.JobType = dr["JobType"].ToString();
                    workplaceConditionsCard.WorkersCount = DBCommon.IsInt(dr["WorkersCount"]) ? (int?)DBCommon.GetInt(dr["WorkersCount"]) : null;                    
                    workplaceConditionsCard.ComplexAssessment = (dr["ComplexAssessment"] is decimal ? (decimal?)dr["ComplexAssessment"] : null);
                    workplaceConditionsCard.ComplexAssessmentPointValue = (dr["ComplexAssessmentPointValue"] is decimal ? (decimal?)dr["ComplexAssessmentPointValue"] : null);
                    workplaceConditionsCard.AdditionalReward = (dr["AdditionalReward"] is decimal ? (decimal?)dr["AdditionalReward"] : null);

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, workplaceConditionsCard);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return workplaceConditionsCard;
        }

        public static List<WorkplaceConditionsCard> GetAllWorkplaceConditionsCards(string militaryUnitIds, string cardNumber, string jobType, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<WorkplaceConditionsCard> cards = new List<WorkplaceConditionsCard>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the uer to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_WCONDCARDS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(militaryUnitIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryUnitIds) + ") ";
                }

                if (!String.IsNullOrEmpty(cardNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CardNumber LIKE '%" + cardNumber.Replace("'", "''") + @"%' ";
                }

                if (!String.IsNullOrEmpty(jobType))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.JobType LIKE '%" + jobType.Replace("'", "''") + @"%' ";
                }              

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

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
                        orderBySQL = "a.CardNumber";
                        break;
                    case 2:
                        orderBySQL = "c.IMEES";
                        break;
                    case 3:
                        orderBySQL = "a.JobType";
                        break;                
                    default:
                        orderBySQL = "a.CardNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.WorkplaceConditionsCardID as WorkplaceConditionsCardID, tmp.RowNumber as RowNumber  FROM (
                                  SELECT a.WorkplaceConditionsCardID as WorkplaceConditionsCardID,                                         
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.WorkplaceConditionsCardID) as RowNumber 
                                  FROM PMIS_HS.WorkplaceConditionsCards a                     
                                  LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR           
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", WorkplaceConditionsCardID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["WorkplaceConditionsCardID"]))
                        cards.Add(WorkplaceConditionsCardUtil.GetWorkplaceConditionsCard(DBCommon.GetInt(dr["WorkplaceConditionsCardID"]), currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cards;
        }

        public static int GetAllWorkplaceConditionsCardsCount(string militaryUnitIds, string cardNumber, string jobType, User currentUser)
        {
            int cardsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the uer to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_WCONDCARDS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(militaryUnitIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryUnitIds) + ") ";
                }

                if (!String.IsNullOrEmpty(cardNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CardNumber LIKE '%" + cardNumber.Replace("'", "''") + @"%' ";
                }

                if (!String.IsNullOrEmpty(jobType))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.JobType LIKE '%" + jobType.Replace("'", "''") + @"%' ";
                }              

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_HS.WorkplaceConditionsCards a
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        cardsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cardsCnt;
        }       

        public static bool SaveWorkplaceConditionsCard(WorkplaceConditionsCard card, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Номер на карта: " + card.CardNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (card.WorkplaceConditionsCardId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.WorkplaceConditionsCards (MilitaryUnitID, CardNumber, JobType, WorkersCount, 
                                                                           ComplexAssessment, ComplexAssessmentPointValue, AdditionalReward,
                                                                           CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:MilitaryUnitID, :CardNumber, :JobType, :WorkersCount, 
                                    :ComplexAssessment, :ComplexAssessmentPointValue, :AdditionalReward,
                                    :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_HS.WConditionsCards_ID_SEQ.currval INTO :WorkplaceConditionsCardID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_WCondCard_AddCard", logDescription, card.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_MilitaryUnit", "", card.MilitaryUnit.DisplayTextForSelection, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_CardNumber", "", card.CardNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_JobType", "", card.JobType, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_WorkersCount", "", (card.WorkersCount.HasValue ? card.WorkersCount.Value.ToString() : ""), currentUser));                    
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_ComplexAssessment", "", (card.ComplexAssessment.HasValue ? CommonFunctions.FormatDecimal(card.ComplexAssessment.Value) : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_ComplexAssessmentPointValue", "", (card.ComplexAssessmentPointValue.HasValue ? CommonFunctions.FormatDecimal(card.ComplexAssessmentPointValue.Value) : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_AdditionalReward", "", (card.AdditionalReward.HasValue ? CommonFunctions.FormatDecimal(card.AdditionalReward.Value) : ""), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.WorkplaceConditionsCards SET
                               MilitaryUnitID = :MilitaryUnitID,
                               CardNumber = :CardNumber,
                               JobType = :JobType,
                               WorkersCount = :WorkersCount,                               
                               ComplexAssessment = :ComplexAssessment,
                               ComplexAssessmentPointValue = :ComplexAssessmentPointValue,
                               AdditionalReward = :AdditionalReward,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END
                            WHERE WorkplaceConditionsCardID = :WorkplaceConditionsCardID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_WCondCard_EditCard", logDescription, card.MilitaryUnit, null, currentUser);

                    WorkplaceConditionsCard oldCard = WorkplaceConditionsCardUtil.GetWorkplaceConditionsCard(card.WorkplaceConditionsCardId, currentUser);

                    if (oldCard.MilitaryUnit.DisplayTextForSelection.Trim() != card.MilitaryUnit.DisplayTextForSelection.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_MilitaryUnit", oldCard.MilitaryUnit.DisplayTextForSelection, card.MilitaryUnit.DisplayTextForSelection, currentUser));

                    if (oldCard.CardNumber.Trim() != card.CardNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_CardNumber", oldCard.CardNumber, card.CardNumber, currentUser));

                    if (oldCard.JobType.Trim() != card.JobType.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_JobType", oldCard.JobType, card.JobType, currentUser));

                    if (oldCard.WorkersCount != card.WorkersCount)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_WorkersCount", (oldCard.WorkersCount.HasValue ? oldCard.WorkersCount.Value.ToString() : ""), (card.WorkersCount.HasValue ? card.WorkersCount.Value.ToString() : "") , currentUser));
                    
                    if (oldCard.ComplexAssessment != card.ComplexAssessment)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_ComplexAssessment", (oldCard.ComplexAssessment.HasValue ? CommonFunctions.FormatDecimal(oldCard.ComplexAssessment.Value) : ""), (card.ComplexAssessment.HasValue ? CommonFunctions.FormatDecimal(card.ComplexAssessment.Value) : ""), currentUser));

                    if (oldCard.ComplexAssessmentPointValue != card.ComplexAssessmentPointValue)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_ComplexAssessmentPointValue", (oldCard.ComplexAssessmentPointValue.HasValue ? CommonFunctions.FormatDecimal(oldCard.ComplexAssessmentPointValue.Value) : ""), (card.ComplexAssessmentPointValue.HasValue ? CommonFunctions.FormatDecimal(card.ComplexAssessmentPointValue.Value) : ""), currentUser));

                    if (oldCard.AdditionalReward != card.AdditionalReward)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_AdditionalReward", (oldCard.AdditionalReward.HasValue ? CommonFunctions.FormatDecimal(oldCard.AdditionalReward.Value) : ""), (card.AdditionalReward.HasValue ? CommonFunctions.FormatDecimal(card.AdditionalReward.Value) : ""), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramWorkplaceConditionsCardID = new OracleParameter();
                paramWorkplaceConditionsCardID.ParameterName = "WorkplaceConditionsCardID";
                paramWorkplaceConditionsCardID.OracleType = OracleType.Number;

                if (card.WorkplaceConditionsCardId != 0)
                {
                    paramWorkplaceConditionsCardID.Direction = ParameterDirection.Input;
                    paramWorkplaceConditionsCardID.Value = card.WorkplaceConditionsCardId;
                }
                else
                {
                    paramWorkplaceConditionsCardID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramWorkplaceConditionsCardID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (card.MilitaryUnitId != -1)
                    param.Value = card.MilitaryUnitId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CardNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(card.CardNumber))
                    param.Value = card.CardNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "JobType";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(card.JobType))
                    param.Value = card.JobType;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "WorkersCount";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (card.WorkersCount.HasValue)
                    param.Value = card.WorkersCount.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);              

                param = new OracleParameter();
                param.ParameterName = "ComplexAssessment";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (card.ComplexAssessment.HasValue)
                    param.Value = card.ComplexAssessment.Value;
                else
                    param.Value = DBNull.Value;             
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ComplexAssessmentPointValue";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (card.ComplexAssessmentPointValue.HasValue)
                    param.Value = card.ComplexAssessmentPointValue.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AdditionalReward";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (card.AdditionalReward.HasValue)
                    param.Value = card.AdditionalReward.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (card.WorkplaceConditionsCardId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (card.WorkplaceConditionsCardId == 0)
                {
                    card.WorkplaceConditionsCardId = DBCommon.GetInt(paramWorkplaceConditionsCardID.Value);
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

        public static bool DeleteWorkplaceConditionsCard(int cardId, User currentUser, Change changeEntry)
        {
            bool result = false;                        

            WorkplaceConditionsCard oldCard = WorkplaceConditionsCardUtil.GetWorkplaceConditionsCard(cardId, currentUser);

            string logDescription = "";
            logDescription += "Номер на карта: " + oldCard.CardNumber;

            ChangeEvent changeEvent = new ChangeEvent("HS_WCondCard_DeleteCard", logDescription, oldCard.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_MilitaryUnit", oldCard.MilitaryUnit.DisplayTextForSelection, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_CardNumber", oldCard.CardNumber, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_JobType", oldCard.JobType, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_WorkersCount", (oldCard.WorkersCount.HasValue ? oldCard.WorkersCount.Value.ToString() : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_ComplexAssessment", (oldCard.ComplexAssessment.HasValue ? CommonFunctions.FormatDecimal(oldCard.ComplexAssessment.Value) : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_ComplexAssessmentPointValue", (oldCard.ComplexAssessmentPointValue.HasValue ? CommonFunctions.FormatDecimal(oldCard.ComplexAssessmentPointValue.Value) : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCard_AdditionalReward", (oldCard.AdditionalReward.HasValue ? CommonFunctions.FormatDecimal(oldCard.AdditionalReward.Value) : ""), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                DELETE FROM PMIS_HS.WorkplaceConditionsCardItems WHERE WorkplaceConditionsCardID = :WorkplaceConditionsCardID;
                                
                                DELETE FROM PMIS_HS.WorkplaceConditionsCards WHERE WorkplaceConditionsCardID = :WorkplaceConditionsCardID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("WorkplaceConditionsCardID", OracleType.Number).Value = cardId;

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

        public static void SetWorkplaceConditionsCardLastModified(int cardId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_HS.WorkplaceConditionsCards SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE WorkplaceConditionsCardID = :CardID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CardID", OracleType.Number).Value = cardId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }

}