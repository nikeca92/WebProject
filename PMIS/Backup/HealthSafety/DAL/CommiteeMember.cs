using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class CommitteeMember : BaseDbObject
    {
        private int committeeMemberId;
        private int? personId;
        private Person person;

        public int CommitteeMemberId
        {
            get { return committeeMemberId; }
            set { committeeMemberId = value; }
        }

        public int? PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        public Person Person
        {
            get
            {
                if (person == null && personId.HasValue)
                    person = PersonUtil.GetPerson(personId.Value, CurrentUser);

                return person;
            }
            set { person = value; }
        }

        public CommitteeMember(User user)
            : base(user)
        {
        }
    }

    public static class CommitteeMemberUtil
    {
        private static CommitteeMember ExtractCommitteeMemberFromDR(OracleDataReader dr, User currentUser)
        {
            CommitteeMember committeeMember = new CommitteeMember(currentUser);

            committeeMember.CommitteeMemberId = DBCommon.GetInt(dr["CommitteeMemberID"]);
            committeeMember.PersonId = (DBCommon.IsInt(dr["PersonID"]) ? (int?)DBCommon.GetInt(dr["PersonID"]) : null);

            return committeeMember;
        }

        public static CommitteeMember GetCommitteeMember(int committeeMemberId, User currentUser)
        {
            CommitteeMember committeeMember = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CommitteeMemberID as CommitteeMemberID,                                      
                                      a.PersonID as PersonID                                     
                               FROM PMIS_HS.CommitteeMembers a
                               WHERE a.CommitteeMemberID = :CommitteeMemberID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CommitteeMemberID", OracleType.Number).Value = committeeMemberId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    committeeMember = ExtractCommitteeMemberFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return committeeMember;
        }

        public static bool IsCommitteeMemberExist(int personId, int committeeId, User currentUser)
        {
            bool result = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT NULL                                     
                               FROM PMIS_HS.CommitteeMembers a
                               WHERE a.PersonID = :PersonID AND a.CommitteeID = :CommitteeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;
                cmd.Parameters.Add("CommitteeID", OracleType.Number).Value = committeeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    result = true;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static List<CommitteeMember> GetAllCommitteeMembersByCommittee(int committeeId, User currentUser)
        {
            List<CommitteeMember> committeeMembers = new List<CommitteeMember>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CommitteeMemberID as CommitteeMemberID,                                     
                                      a.PersonID as PersonID
                               FROM PMIS_HS.CommitteeMembers a
                               WHERE a.CommitteeID = :CommitteeID
                               ORDER BY a.CommitteeMemberID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CommitteeID", OracleType.Number).Value = committeeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["CommitteeMemberID"]))
                        committeeMembers.Add(ExtractCommitteeMemberFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return committeeMembers;
        }

        public static bool SaveCommitteeMember(int committeeId, CommitteeMember committeeMember, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            Committee committee = CommitteeUtil.GetCommittee(committeeId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                string logDescription = "";
                logDescription += CommonFunctions.GetLabelText("MilitaryUnit") + ": " + committee.MilitaryUnit.DisplayTextForSelection;

                if (committeeMember.CommitteeMemberId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.CommitteeMembers (CommitteeID, PersonID)
                            VALUES (:CommitteeID, :PersonID);

                            SELECT PMIS_HS.CommitteeMembers_ID_SEQ.currval INTO :CommitteeMemberID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Committee_AddCommitteeMember", logDescription, committee.MilitaryUnit, committeeMember.Person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_CommitteeMember_PersonName", "", committeeMember.Person.FullName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_CommitteeMember_PersonIdentNumber", "", committeeMember.Person.IdentNumber, currentUser));
                }
                else
                {
                    // ONLY FOR COMPATIBILITY - NOT REAL CASE

                    SQL += @"UPDATE PMIS_HS.ProtocolItems SET
                               CommitteeID = :CommitteeID, 
                               PersonID = :PersonID
                            WHERE CommitteeMemberID = :CommitteeMemberID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_Committee_EditCommitteeMember", logDescription, committee.MilitaryUnit, committeeMember.Person, currentUser);

                    CommitteeMember oldCommitteeMember = CommitteeMemberUtil.GetCommitteeMember(committeeMember.CommitteeMemberId, currentUser);

                    if (oldCommitteeMember.Person.FullName.Trim() != committeeMember.Person.FullName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_CommitteeMember_PersonName", oldCommitteeMember.Person.FullName, committeeMember.Person.FullName, currentUser));

                    if (oldCommitteeMember.Person.IdentNumber.Trim() != committeeMember.Person.IdentNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_CommitteeMember_PersonIdentNumber", oldCommitteeMember.Person.IdentNumber, committeeMember.Person.IdentNumber, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramCommitteeMemberID = new OracleParameter();
                paramCommitteeMemberID.ParameterName = "CommitteeMemberID";
                paramCommitteeMemberID.OracleType = OracleType.Number;

                if (committeeMember.CommitteeMemberId != 0)
                {
                    paramCommitteeMemberID.Direction = ParameterDirection.Input;
                    paramCommitteeMemberID.Value = committeeMember.CommitteeMemberId;
                }
                else
                {
                    paramCommitteeMemberID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramCommitteeMemberID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "CommitteeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = committeeId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = committeeMember.PersonId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (committeeMember.CommitteeMemberId == 0)
                {
                    committeeMember.CommitteeMemberId = DBCommon.GetInt(paramCommitteeMemberID.Value);
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
                {
                    changeEntry.AddEvent(changeEvent);
                    CommitteeUtil.SetCommitteeModified(committeeId, currentUser);
                }
            }

            return result;
        }

        public static bool DeleteCommitteeMember(int committeeMemberId, int committeeId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Committee committee = CommitteeUtil.GetCommittee(committeeId, currentUser);

            CommitteeMember oldCommitteeMember = CommitteeMemberUtil.GetCommitteeMember(committeeMemberId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Committee_DeleteCommitteeMember", CommonFunctions.GetLabelText("MilitaryUnit") + ": " + committee.MilitaryUnit.DisplayTextForSelection, committee.MilitaryUnit, oldCommitteeMember.Person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_CommitteeMember_PersonName", oldCommitteeMember.Person.FullName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_CommitteeMember_PersonIdentNumber", oldCommitteeMember.Person.IdentNumber, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_HS.CommitteeMembers WHERE CommitteeMemberID = :CommitteeMemberID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CommitteeMemberID", OracleType.Number).Value = committeeMemberId;

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