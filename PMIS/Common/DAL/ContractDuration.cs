using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;


namespace PMIS.Common
{
    public class ContractDuration : IDropDownItem
    {
        public string ContractDurationKey { get; set; }
        public string ContractDurationName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return ContractDurationName;
        }

        public string Value()
        {
            return ContractDurationKey;
        }
    }

    public class ContractDurationUtil
    {
        public static ContractDuration GetContractDuration(string contractDurationKey, User currentUser)
        {
            ContractDuration contractDuration = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TDG_KOD as ContractDurationKey,
                                      a.TDG_IME as ContractDurationName
                               FROM VS_OWNER.KLV_TDG a 
                               WHERE a.TDG_KOD = :ContractDurationKey ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ContractDurationKey", OracleType.VarChar).Value = contractDurationKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    contractDuration = new ContractDuration();
                    contractDuration.ContractDurationKey = contractDurationKey;
                    contractDuration.ContractDurationName = dr["ContractDurationName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return contractDuration;
        }

        public static List<ContractDuration> GetAllContractDuration(User currentUser)
        {
            List<ContractDuration> list = new List<ContractDuration>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string SQL = @"SELECT a.TDG_KOD as ContractDurationKey,
                                      a.TDG_IME as ContractDurationName
                               FROM VS_OWNER.KLV_TDG a                              
                               ORDER BY a.TDG_IME ASC";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ContractDuration contractDuration = new ContractDuration();
                    contractDuration.ContractDurationKey = dr["ContractDurationKey"].ToString();
                    contractDuration.ContractDurationName = dr["ContractDurationName"].ToString();
                    list.Add(contractDuration);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return list;
        }
    }
}
