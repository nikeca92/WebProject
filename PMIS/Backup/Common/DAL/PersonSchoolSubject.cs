using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person education from VS_OWNER.KLV_SPE
    public class PersonSchoolSubject : IDropDownItem
    {
        private string personSchoolSubjectCode;
        private string personSchoolSubjectName;

        public string PersonSchoolSubjectCode
        {
            get { return personSchoolSubjectCode; }
            set { personSchoolSubjectCode = value; }
        }

        public string PersonSchoolSubjectName
        {
            get { return personSchoolSubjectName; }
            set { personSchoolSubjectName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return personSchoolSubjectName;
        }

        public string Value()
        {
            return personSchoolSubjectCode;
        }
    }

    public static class PersonSchoolSubjectUtil
    {
        public static PersonSchoolSubject GetPersonSchoolSubject(string personSchoolSubjectCode, User currentUser)
        {
            PersonSchoolSubject personSchoolSubject = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.SPE_KOD as SchoolSubjectCode, a.SPE_IME as SchoolSubjectName
                               FROM VS_OWNER.KLV_SPE a
                               WHERE a.SPE_KOD = :SchoolSubjectCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("SchoolSubjectCode", OracleType.VarChar).Value = personSchoolSubjectCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personSchoolSubject = new PersonSchoolSubject();
                    personSchoolSubject.PersonSchoolSubjectCode = dr["SchoolSubjectCode"].ToString();
                    personSchoolSubject.PersonSchoolSubjectName = dr["SchoolSubjectName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personSchoolSubject;
        }

        public static List<PersonSchoolSubject> GetAllPersonSchoolSubjects(User currentUser)
        {
            List<PersonSchoolSubject> personSchoolSubjects = new List<PersonSchoolSubject>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.SPE_KOD as SchoolSubjectCode, a.SPE_IME as SchoolSubjectName
                               FROM VS_OWNER.KLV_SPE a
                               ORDER BY a.SPE_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PersonSchoolSubject personSchoolSubject = new PersonSchoolSubject();
                    personSchoolSubject.PersonSchoolSubjectCode = dr["SchoolSubjectCode"].ToString();
                    personSchoolSubject.PersonSchoolSubjectName = dr["SchoolSubjectName"].ToString();
                    personSchoolSubjects.Add(personSchoolSubject);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personSchoolSubjects;
        }

        public static string GetPersonSchoolSubjects_ItemSelector(int pageIndex, int pageCount, string prefix,
                                                                  User currentUser)
        {
            StringBuilder sb = new StringBuilder();
            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            sb.Append("<response>");

            try
            {
                SQL = @"SELECT a.SchoolSubjectCode, a.SchoolSubjectName, b.COUNT
                        FROM (  SELECT a.SPE_KOD as SchoolSubjectCode, a.SPE_IME as SchoolSubjectName,
                                       -1 as RowNumber,
                                       0 as OrderNum
                                FROM VS_OWNER.KLV_SPE a
                                WHERE UPPER(a.SPE_IME) = UPPER(:matchprefix)

                                UNION

                                SELECT a.SPE_KOD as SchoolSubjectCode, a.SPE_IME as SchoolSubjectName,
                                       RANK() OVER (ORDER BY a.SPE_IME, a.SPE_KOD) as RowNumber,
                                       1 as OrderNum
                                FROM VS_OWNER.KLV_SPE a
                                WHERE  UPPER(a.SPE_IME) LIKE UPPER(:prefix)
                             ) a
                        LEFT OUTER JOIN ( SELECT COUNT(*) as COUNT
                                          FROM VS_OWNER.KLV_SPE a
                                          WHERE UPPER(a.SPE_IME) LIKE UPPER(:prefix)
                                        ) b ON 1=1
                        WHERE a.RowNumber = -1 OR a.RowNumber BETWEEN (:pageIndex - 1) * :pageCount + 1 AND :pageIndex * :pageCount
                        GROUP BY a.SchoolSubjectCode, a.SchoolSubjectName, b.COUNT
                        ORDER BY MIN(a.OrderNum), a.SchoolSubjectName";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("pageIndex", OracleType.Int32).Value = pageIndex;
                cmd.Parameters.Add("pageCount", OracleType.Int32).Value = pageCount;
                cmd.Parameters.Add("prefix", OracleType.VarChar).Value = prefix + "%";
                cmd.Parameters.Add("matchprefix", OracleType.VarChar).Value = prefix;

                OracleDataReader dr = cmd.ExecuteReader();

                int count = 0;
                sb.Append("<result>");
                while (dr.Read())
                {
                    count = int.Parse(dr["COUNT"].ToString());
                    sb.Append("<item>");
                    sb.Append("<text>");
                    sb.Append(AJAXTools.EncodeForXML(dr["SchoolSubjectName"].ToString()));
                    sb.Append("</text>");
                    sb.Append("<value>");
                    sb.Append(AJAXTools.EncodeForXML(dr["SchoolSubjectCode"].ToString()));
                    sb.Append("</value>");
                    sb.Append("</item>");
                }
                
                dr.Close();

                sb.Append("</result>");

                sb.Append("<count>");
                sb.Append(AJAXTools.EncodeForXML(count.ToString()));
                sb.Append("</count>");
            }
            finally
            {
                conn.Close();
            }

            sb.Append("</response>");

            return sb.ToString();
        }
    }
}
