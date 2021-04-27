using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //This class represents the Position Title objects
    public class PositionTitle : IDropDownItem
    {
        public int PositionTitleId {get; set;}
        public string PositionTitleName {get; set;}
        public bool Active {get; set;}

        public string Text()
        {
            return PositionTitleName;
        }

        public string Value()
        {
            return PositionTitleId.ToString();
        }
    }

    //Some utility methods that help working with PositionTitle objects
    public static class PositionTitleUtil
    {
        //Extract a particular PositionTitle object from a data reader
        public static PositionTitle ExtractPositionTitleFromDR(User currentUser, OracleDataReader dr)
        {
            PositionTitle positionTitle = new PositionTitle();

            positionTitle.PositionTitleId = DBCommon.GetInt(dr["PositionTitleID"]);
            positionTitle.PositionTitleName = dr["PositionTitle"].ToString();
            positionTitle.Active = DBCommon.GetInt(dr["PositionTitleActive"]) == 1;

            return positionTitle;
        }

        //Get a particualr PositionTitle object from the DB by its ID
        public static PositionTitle GetPositionTitle(int positionTitleId, User currentUser)
        {
            PositionTitle positionTitle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PositionTitleID,
                                      a.PositionTitle,
                                      a.IsActive as PositionTitleActive
                               FROM PMIS_ADM.PositionTitles a
                               WHERE a.PositionTitleID = :PositionTitleID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PositionTitleID", OracleType.Number).Value = positionTitleId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    positionTitle = ExtractPositionTitleFromDR(currentUser, dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return positionTitle;
        }

        public static List<PositionTitle> GetAllPositionTitles(User currentUser)
        {
            List<PositionTitle> positionTitles = new List<PositionTitle>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.PositionTitleID,
                                      a.PositionTitle,
                                      a.IsActive as PositionTitleActive
                               FROM PMIS_ADM.PositionTitles a
                               WHERE NVL(a.IsActive, 0) = 1
                               ORDER BY a.PositionTitle";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    positionTitles.Add(ExtractPositionTitleFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return positionTitles;
        }
    }
}
