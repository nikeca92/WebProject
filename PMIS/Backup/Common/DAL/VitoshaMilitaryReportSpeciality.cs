using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class VitoshaMilitaryReportSpeciality : BaseDbObject
    {
        private string mVitoshaMilReportingSpecialityCode;
        public string VitoshaMilReportingSpecialityCode
        {
            get { return mVitoshaMilReportingSpecialityCode; }
            set { mVitoshaMilReportingSpecialityCode = value; }
        }

        private string mVitoshaMilReportSpecialityTypeID;  
        public string VitoshaMilReportSpecialityTypeID
        {
            get { return mVitoshaMilReportSpecialityTypeID; }
            set { mVitoshaMilReportSpecialityTypeID = value; }
        }
     
        private VitoshaMilitaryReportSpecialityType mVitoshaMilReportSpecialityType;
        public VitoshaMilitaryReportSpecialityType VitoshaMilReportSpecialityType
        {
            get
            {
                if (mVitoshaMilReportSpecialityType == null)
                    mVitoshaMilReportSpecialityType = VitoshaMilitaryReportSpecialityTypeUtil.GetVitoshaMilitaryReportSpecialityType(mVitoshaMilReportSpecialityTypeID, CurrentUser);

                return mVitoshaMilReportSpecialityType;
            }
            set { mVitoshaMilReportSpecialityType = value; }
        }

        private string mVitoshaMilReportingSpecialityName;
        public string VitoshaMilReportingSpecialityName
        {
            get{ return mVitoshaMilReportingSpecialityName; }
            set{ mVitoshaMilReportingSpecialityName = value; }
        }

        private string mCodeAndName = null;
        public string CodeAndName
        {
            get
            {
                if (mCodeAndName == null)
                    mCodeAndName = VitoshaMilReportingSpecialityCode + " " + VitoshaMilReportingSpecialityName;

                return mCodeAndName;
            }

            set { mCodeAndName = value; }
        }
             
        public VitoshaMilitaryReportSpeciality(User user)
            : base(user)
        {
        }
    }

    public static class VitoshaMilitaryReportSpecialityUtil
    {
        public static VitoshaMilitaryReportSpeciality GetVitoshaMilitaryReportSpeciality(string pVitoshaMilitaryReportSpecialityCode, User pCurrentUser)
        {
            VitoshaMilitaryReportSpeciality vitoshaMilitaryReportSpeciality = null;

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT VSO_KOD, 
                                      VSO_IME, 
                                      VSO_TYPE 
                               FROM VS_OWNER.KLV_VSO 
                               WHERE VSO_KOD = :VSO_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VSO_KOD", OracleType.NVarChar).Value = pVitoshaMilitaryReportSpecialityCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vitoshaMilitaryReportSpeciality = ExtractVitoshaMilitaryReportSpecialityFromDR(pCurrentUser, dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vitoshaMilitaryReportSpeciality;
        }

        public static List<VitoshaMilitaryReportSpeciality> GetVitoshaMilitaryReportSpecialitysByType(string pVitoshaMilitaryReportSpecialitysByType, User pCurrentUser)
        {
            List<VitoshaMilitaryReportSpeciality> list = new List<VitoshaMilitaryReportSpeciality>();

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT VSO_KOD, 
                                      VSO_IME, 
                                      VSO_TYPE 
                               FROM VS_OWNER.KLV_VSO 
                               WHERE VSO_TYPE = :VSO_TYPE
                               ORDER BY VSO_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VSO_TYPE", OracleType.NVarChar).Value = pVitoshaMilitaryReportSpecialitysByType;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                   list.Add(ExtractVitoshaMilitaryReportSpecialityFromDR(pCurrentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return list;
        }

        private static VitoshaMilitaryReportSpeciality ExtractVitoshaMilitaryReportSpecialityFromDR(User pCurrentUser, OracleDataReader pDR)
        {
            VitoshaMilitaryReportSpeciality vitoshaMilitaryReportSpeciality = new VitoshaMilitaryReportSpeciality(pCurrentUser);

            vitoshaMilitaryReportSpeciality.VitoshaMilReportingSpecialityCode = pDR["VSO_KOD"].ToString();
            vitoshaMilitaryReportSpeciality.VitoshaMilReportingSpecialityName = pDR["VSO_IME"].ToString();
            vitoshaMilitaryReportSpeciality.VitoshaMilReportSpecialityTypeID = pDR["VSO_TYPE"].ToString();
                    
            return vitoshaMilitaryReportSpeciality;
        }
    }

}
