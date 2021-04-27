using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;
using System.Text;

namespace PMIS.Common
{
    public class Company : BaseDbObject
    {
        private int companyId;
        private int ownershipTypeId;
        private string unifiedIdentityCode;
        private string companyName;
        private int? cityId;
        private int? districtId;
        private string address;
        private string postCode;        
        private string phone;
        private int? administrationId;        
        private Administration administration;                

        private OwnershipType ownershipType;
        private City city;
        private District district;

        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        public int OwnershipTypeId
        {
            get { return ownershipTypeId; }
            set { ownershipTypeId = value; }
        }

        public string UnifiedIdentityCode
        {
            get { return unifiedIdentityCode; }
            set { unifiedIdentityCode = value; }
        }

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        public int? CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }

        public int? DistrictId
        {
            get { return districtId; }
            set { districtId = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public int? AdministrationId
        {
            get { return administrationId; }
            set { administrationId = value; }
        }

        public Administration Administration
        {
            get 
            {
                if (administration == null && administrationId.HasValue)
                    administration = AdministrationUtil.GetAdministration(administrationId.Value, CurrentUser);
                return administration; 
            }
            set { administration = value; }
        }

        public OwnershipType OwnershipType
        {
            get
            {
                if (ownershipType == null)
                    ownershipType = OwnershipTypeUtil.GetOwnershipType(ownershipTypeId, CurrentUser);
                return ownershipType;
            }
            set { ownershipType = value; }
        }

        public City City
        {
            get
            {
                if (city == null && cityId != null)
                {
                    city = CityUtil.GetCity((int)cityId, CurrentUser);
                }
                return city;
            }
            set
            {
                city = value;
            }
        }

        public District District
        {
            get
            {
                //Lazy initialization
                if (district == null && DistrictId.HasValue)
                {
                    district = DistrictUtil.GetDistrict(districtId.Value, CurrentUser);
                }
                return district;
            }
            set
            {
                district = value;
            }
        }

        public Company(User user)
            : base(user)
        {
        }
    }

    public class CompanyFilter
    {
        public string OwnershipTypes
        {
            get;
            set;
        }

        public string UnifiedIdentityCode
        {
            get;
            set;
        }

        public string CompanyName
        {
            get;
            set;
        }

        public string Administrations
        {
            get;
            set;
        }

        public int OrderBy
        {
            get;
            set;
        }

        public int PageIdx
        {
            get;
            set;
        }
    }

    public class CompanyBlock
    {
        public int CompanyID
        {
            get;
            set;
        }

        public string CompanyName
        {
            get;
            set;
        }

        public string UnifiedIdentityCode
        {
            get;
            set;
        }

        public string OwnershipType
        {
            get;
            set;
        }

        public string Administration
        {
            get;
            set;
        }

        public string CityAndAddress
        {
            get;
            set;
        }

        public bool CanDelete
        {
            get;
            set;
        }
    }

    public class CompanyUtil
    {
        public static Company ExtractCompanyFromDR(OracleDataReader dr, User currentUser)
        {
            Company company = new Company(currentUser);

            company.CompanyId = DBCommon.GetInt(dr["CompanyID"]);
            company.OwnershipTypeId = DBCommon.GetInt(dr["OwnershipTypeID"]);
            company.UnifiedIdentityCode = dr["UnifiedIdentityCode"].ToString();
            company.CompanyName = dr["CompanyName"].ToString();
            company.CityId = DBCommon.IsInt(dr["CityID"]) ? (int?)DBCommon.GetInt(dr["CityID"]) : null;
            company.DistrictId = DBCommon.IsInt(dr["DistrictID"]) ? (int?)DBCommon.GetInt(dr["DistrictID"]) : null;
            company.Address = dr["Address"].ToString();
            company.PostCode = dr["PostCode"].ToString();
            company.Phone = dr["Phone"].ToString();
            company.AdministrationId = DBCommon.IsInt(dr["AdministrationID"]) ? (int?)DBCommon.GetInt(dr["AdministrationID"]) : null;

            return company;
        }

        public static Company GetCompany(int companyId, User currentUser)
        {
            Company company = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CompanyID, a.OwnershipTypeID, a.UnifiedIdentityCode,
                                      a.CompanyName, a.CityID, a.DistrictID, a.Address, a.PostCode, a.Phone, a.AdministrationID
                               FROM PMIS_ADM.Companies a
                               WHERE a.CompanyID = :CompanyID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CompanyID", OracleType.Number).Value = companyId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    company = ExtractCompanyFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return company;
        }

        public static Company GetCompanyByUnifiedIdentityCode(string unifiedIdentityCode, User currentUser)
        {
            Company company = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CompanyID, a.OwnershipTypeID, a.UnifiedIdentityCode,
                                      a.CompanyName, a.CityID, a.DistrictID, a.Address, a.PostCode, a.Phone, a.AdministrationID
                               FROM PMIS_ADM.Companies a
                               WHERE a.UnifiedIdentityCode = :UnifiedIdentityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UnifiedIdentityCode", OracleType.NVarChar).Value = unifiedIdentityCode.Trim();

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    company = ExtractCompanyFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return company;
        }

        public static Company GetCompanyByCompanyName(string companyName, User currentUser)
        {
            Company company = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CompanyID, a.OwnershipTypeID, a.UnifiedIdentityCode,
                                      a.CompanyName, a.CityID, a.DistrictID, a.Address, a.PostCode, a.Phone, a.AdministrationID
                               FROM PMIS_ADM.Companies a
                               WHERE UPPER(a.CompanyName) = UPPER(:CompanyName)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CompanyName", OracleType.NVarChar).Value = companyName.Trim();

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    company = ExtractCompanyFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return company;
        }

        //Get a list of all Companies for the selected filter
        public static List<CompanyBlock> GetAllCompanies(User currentUser, CompanyFilter filter, int rowsPerPage)
        {
            List<CompanyBlock> companies = new List<CompanyBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.OwnershipTypes))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.OwnershipTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.OwnershipTypes) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.UnifiedIdentityCode))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.UnifiedIdentityCode) LIKE '%" + filter.UnifiedIdentityCode.Replace("'", "''").ToUpper() + "%' ";
                }

                if (!String.IsNullOrEmpty(filter.CompanyName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.CompanyName) LIKE '%" + filter.CompanyName.Replace("'", "''").ToUpper() + "%' ";
                }

                if (!String.IsNullOrEmpty(filter.Administrations))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Administrations) + ") ";
                }

                //Paging (load the rows only for the target page)
                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                where = (where == "" ? "" : " WHERE ") + where;

                //Construct the ORDER BY clause according to the order column
                string orderBySQL = "";
                string orderByDir = "ASC";

                int orderBy = filter.OrderBy;

                //The DESCending order is specified by using column number + 100 (e.g. 101, 102, etc.)
                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                //Get the specific order by expression
                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "a.CompanyName";
                        break;
                    case 2:
                        orderBySQL = "a.UnifiedIdentityCode";
                        break;
                    case 3:
                        orderBySQL = "b.OwnershipTypeName";
                        break;
                    case 4:
                        orderBySQL = "c.AdministrationName";
                        break;
                    default:
                        orderBySQL = "a.CompanyName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir) + ", a.CompanyID ";

                string SQL = @"SELECT * 
                               FROM ( SELECT a.CompanyID, 
                                             b.OwnershipTypeName, 
                                             a.UnifiedIdentityCode,
                                             a.CompanyName,                                            
                                             c.AdministrationName,
                                             NVL(d.PostponeRefs, 0) + 
                                             NVL(e.TechnicsRefs, 0) + 
                                             NVL(f.PersonsRefs, 0) + 
                                             NVL(g.PostponedTechnics, 0)+ 
                                             NVL(h.PosponedReserves, 0) as AllRefs,
                                             RANK() OVER (ORDER BY " + orderBySQL + @") as RowNumber
                                      FROM PMIS_ADM.Companies a  
                                      LEFT OUTER JOIN PMIS_ADM.OwnershipTypes b ON a.OwnershipTypeID = b.OwnershipTypeID
                                      LEFT OUTER JOIN PMIS_ADM.Administrations c ON a.AdministrationID = c.AdministrationID
                                      LEFT OUTER JOIN ( SELECT CompanyID, COUNT (*) as PostponeRefs 
	                                					FROM PMIS_RES.PostponeItems 
	                                					GROUP BY CompanyID) d ON a.CompanyID = d.CompanyID
	                                					
                                      LEFT OUTER JOIN ( SELECT OwnershipCompanyID, COUNT (*) as TechnicsRefs 
	                                					FROM PMIS_RES.Technics 
	                                					GROUP BY OwnershipCompanyID) e ON a.CompanyID = e.OwnershipCompanyID
	                                					
                                      LEFT OUTER JOIN ( SELECT WorkCompanyID, COUNT (*) as PersonsRefs 
	                                					FROM PMIS_ADM.Persons 
	                                					GROUP BY WorkCompanyID) f ON a.CompanyID = f.WorkCompanyID
                                      
                                      LEFT OUTER JOIN ( SELECT t.CompanyID, COUNT(*) as PostponedTechnics
                                                        FROM PMIS_RES.PostponeTechItems ti 
                                                        INNER JOIN PMIS_RES.PostponeTechCompanies t on t.PostponeTechcompanyId = ti.PostponeTechcompanyId
                                                        GROUP BY t.CompanyID) g ON a.CompanyID = g.CompanyID

                                      LEFT OUTER JOIN ( SELECT CompanyID, COUNT(*) as PosponedReserves
                                                        FROM PMIS_RES.PostponeResCompanies
                                                        GROUP BY CompanyID) h ON a.CompanyID = h.CompanyID
                                      " + where + @"
                                      ORDER BY " + orderBySQL + @"
                                     ) tmp
                               " + pageWhere;


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CompanyBlock block = new CompanyBlock();
                    block.CompanyID = DBCommon.GetInt(dr["CompanyID"]);
                    block.OwnershipType = dr["OwnershipTypeName"].ToString();
                    block.UnifiedIdentityCode = dr["UnifiedIdentityCode"].ToString();
                    block.CompanyName = dr["CompanyName"].ToString();
                    block.Administration = dr["AdministrationName"].ToString();
                    block.CanDelete = DBCommon.GetInt(dr["AllRefs"]) == 0;

                    companies.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return companies;
        }

        public static int GetAllCompaniesCount(User currentUser, CompanyFilter filter)
        {
            int Cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.OwnershipTypes))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.OwnershipTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.OwnershipTypes) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.UnifiedIdentityCode))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.UnifiedIdentityCode) LIKE '%" + filter.UnifiedIdentityCode.Replace("'", "''").ToUpper() + "%' ";
                }

                if (!String.IsNullOrEmpty(filter.CompanyName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.CompanyName) LIKE '%" + filter.CompanyName.Replace("'", "''").ToUpper() + "%' ";
                }

                if (!String.IsNullOrEmpty(filter.Administrations))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Administrations) + ") ";
                }
                
                where = (where == "" ? "" : " WHERE ") + where;                               

                string SQL = @" SELECT COUNT(*) as Cnt
                                FROM PMIS_ADM.Companies a                                        
                              " + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Cnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return Cnt;
        }

        public static bool SaveCompany(Company company, User currentUser, Change changeEntry)
        {
            //Ignore saving empty companies. The app should not allow saving such records, but we added this check here just in case
            if (String.IsNullOrEmpty(company.CompanyName))
                return false;

            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (company.CompanyId == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.Companies (OwnershipTypeID, UnifiedIdentityCode, 
                                CompanyName, CityID, DistrictID, Address, PostCode, Phone, AdministrationID)
                             VALUES (:OwnershipTypeID, :UnifiedIdentityCode, 
                                :CompanyName, :CityID, :DistrictID, :Address, :PostCode, :Phone, :AdministrationID);

                             SELECT PMIS_ADM.Companies_ID_SEQ.currval INTO :CompanyID FROM dual;
                            ";

                    changeEvent = new ChangeEvent("ADM_Companies_AddCompany", "", null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Type", "", company.OwnershipType.OwnershipTypeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_UnifiedIdentityCode", "", company.UnifiedIdentityCode.Trim(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Name", "", company.CompanyName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_City", "", company.City != null ? company.City.RegionMunicipalityAndCity : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_District", "", company.District != null ? company.District.DistrictName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Address", "", company.Address, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_PostCode", "", company.PostCode, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Phone", "", company.Phone, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Administration", "", company.Administration != null ? company.Administration.AdministrationName : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_ADM.Companies SET
                               OwnershipTypeID = :OwnershipTypeID, 
                               UnifiedIdentityCode = :UnifiedIdentityCode, 
                               CompanyName = :CompanyName, 
                               CityID = :CityID,
                               DistrictID = :DistrictID,
                               Address = :Address,
                               PostCode = :PostCode,
                               Phone = :Phone,
                               AdministrationID = :AdministrationID
                            WHERE CompanyID = :CompanyID;                       

                            ";

                    string changeDescription = "";
                    if (!String.IsNullOrEmpty(company.CompanyName))
                        changeDescription += company.CompanyName;

                    if (!String.IsNullOrEmpty(company.UnifiedIdentityCode))
                        changeDescription += (changeDescription == "" ? "" : " - ") + company.UnifiedIdentityCode;

                    changeEvent = new ChangeEvent("ADM_Companies_EditCompany", changeDescription, null, null, currentUser);

                    Company oldCompany = CompanyUtil.GetCompany(company.CompanyId, currentUser);

                    if (oldCompany.OwnershipTypeId != company.OwnershipTypeId)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Type", oldCompany.OwnershipType.OwnershipTypeName, company.OwnershipType.OwnershipTypeName, currentUser));

                    if (oldCompany.UnifiedIdentityCode.Trim() != company.UnifiedIdentityCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_UnifiedIdentityCode", oldCompany.UnifiedIdentityCode.Trim(), company.UnifiedIdentityCode.Trim(), currentUser));

                    if (oldCompany.CompanyName != company.CompanyName)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Name", oldCompany.CompanyName, company.CompanyName, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldCompany.CityId, company.CityId))
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_City", 
                            oldCompany.City != null ? oldCompany.City.RegionMunicipalityAndCity : "", 
                            company.City != null ? company.City.RegionMunicipalityAndCity : "", currentUser));

                    if (!CommonFunctions.IsEqualInt(oldCompany.DistrictId, company.DistrictId))
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_District",
                            oldCompany.District != null ? oldCompany.District.DistrictName : "",
                            company.District != null ? company.District.DistrictName : "", currentUser));

                    if (oldCompany.Address != company.Address)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Address",
                            oldCompany.Address != null ? oldCompany.Address : oldCompany.Address,
                            company.Address != null ? company.Address : company.Address, currentUser));

                    if (oldCompany.PostCode != company.PostCode)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_PostCode",
                            oldCompany.PostCode != null ? oldCompany.PostCode : oldCompany.PostCode,
                            company.PostCode != null ? company.PostCode : company.PostCode, currentUser));

                    if (oldCompany.Phone != company.Phone)
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Phone",
                            oldCompany.Phone != null ? oldCompany.Phone : oldCompany.Phone,
                            company.Phone != null ? company.Phone : company.Phone, currentUser));

                    if (!CommonFunctions.IsEqualInt(oldCompany.AdministrationId, company.AdministrationId))
                        changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Administration",
                            oldCompany.Administration != null ? oldCompany.Administration.AdministrationName : "",
                            company.Administration != null ? company.Administration.AdministrationName : "", currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramCompanyID = new OracleParameter();
                paramCompanyID.ParameterName = "CompanyID";
                paramCompanyID.OracleType = OracleType.Number;

                if (company.CompanyId != 0)
                {
                    paramCompanyID.Direction = ParameterDirection.Input;
                    paramCompanyID.Value = company.CompanyId;
                }
                else
                {
                    paramCompanyID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramCompanyID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "OwnershipTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = company.OwnershipTypeId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "UnifiedIdentityCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(company.UnifiedIdentityCode.Trim()))
                    param.Value = company.UnifiedIdentityCode.Trim();
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CompanyName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(company.CompanyName))
                    param.Value = company.CompanyName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (company.CityId.HasValue)
                    param.Value = company.CityId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DistrictID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (company.DistrictId.HasValue)
                    param.Value = company.DistrictId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Address";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(company.Address))
                    param.Value = company.Address;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PostCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(company.PostCode))
                    param.Value = company.PostCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Phone";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(company.Phone))
                    param.Value = company.Phone;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AdministrationID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (company.AdministrationId.HasValue)
                    param.Value = company.AdministrationId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (company.CompanyId == 0)
                    company.CompanyId = DBCommon.GetInt(paramCompanyID.Value);

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
                }
            }

            return result;
        }

        //Delete a military report speciality
        public static bool DeleteCompany(User currentUser, int companyId, Change changeEntry)
        {
            bool result = false;

            Company oldCompany = GetCompany(companyId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"BEGIN
                                  DELETE FROM PMIS_ADM.Companies
                                  WHERE CompanyID = :CompanyID;
                               END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CompanyID", OracleType.Number).Value = companyId;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            //Save the operation to the changes log
            ChangeEvent changeEvent = new ChangeEvent("ADM_Companies_DeleteCompany", "", null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Type", oldCompany.OwnershipType.OwnershipTypeName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_UnifiedIdentityCode", oldCompany.UnifiedIdentityCode.Trim(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Name", oldCompany.CompanyName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_City", oldCompany.City != null ? oldCompany.City.RegionMunicipalityAndCity : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_District", oldCompany.District != null ? oldCompany.District.DistrictName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Address", oldCompany.Address, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_PostCode", oldCompany.PostCode, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Phone", oldCompany.Phone, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("ADM_Companies_Administration", oldCompany.Administration != null ? oldCompany.Administration.AdministrationName : "", "", currentUser));            

            changeEntry.AddEvent(changeEvent);

            return result;
        }

        //http://hardwarebg.com/forum/showthread.php?t=63767
        //http://bulstat.registryagency.bg/About.html
        public static bool IsValidUnifiedIdentityNumber(string unifiedIdentityNumber, User currentUser)
        {
            bool isValid = true;

            string checkCode = unifiedIdentityNumber.ToUpper().Trim();

            if (checkCode.StartsWith("BG"))
            {
                checkCode = checkCode.Length > 2 ? checkCode.Substring(2).Trim() : "";
            }

            Match MyRegMatch = Regex.Match(checkCode, "^\\d+$");

            if (!MyRegMatch.Success)
            {
                isValid = false;
            }
            else
            {
                if (checkCode.Length != 9 &&
                    checkCode.Length != 13)
                {
                    isValid = false;
                }
                else
                {
                    int[] a = new int[13];

                    for (int i = 0; i < 13; i++)
                    {
                        a[i] = 0;
                    }

                    for (int i = 0; i < 9; i++)
                    {
                        a[i] = int.Parse(checkCode.Substring(i, 1));
                    }

                    int control9 = 0;

                    for (int i = 0; i < 8; i++)
                    {
                        control9 += (i + 1) * a[i];
                    }

                    int remainder9 = control9 % 11;

                    if (remainder9 != 10)
                    {
                        control9 = remainder9;
                    }
                    else
                    {
                        int secondControl9 = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            secondControl9 += (i + 3) * a[i];
                        }

                        int secondRemainder9 = secondControl9 % 11;

                        if (secondRemainder9 != 10)
                        {
                            control9 = secondRemainder9;
                        }
                        else
                        {
                            control9 = 0;
                        }
                    }

                    if (control9 != a[8])
                    {
                        isValid = false;
                    }
                    else
                    {
                        if (checkCode.Length == 13)
                        {
                            for (int i = 9; i < 13; i++)
                            {
                                a[i] = int.Parse(checkCode.Substring(i, 1));
                            }

                            int control13 = 2 * a[8] + 7 * a[9] + 3 * a[10] + 5 * a[11];

                            int remainder13 = control13 % 11;

                            if (remainder13 != 10)
                            {
                                control13 = remainder13;
                            }
                            else
                            {
                                int secondControl13 = 4 * a[8] + 9 * a[9] + 5 * a[10] + 7 * a[11];

                                int secondRemainder13 = secondControl13 % 11;

                                if (secondRemainder13 != 10)
                                {
                                    control13 = secondRemainder13;
                                }
                                else
                                {
                                    control13 = 0;
                                }
                            }

                            if (control13 != a[12])
                            {
                                isValid = false;
                            }
                        }
                    }
                }
            }

            return isValid;
        }

        public static string GetAllCompanies_ItemSelector(int pageIndex, int pageCount, string prefix, User currentUser)
        {
            StringBuilder sb = new StringBuilder();
            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            sb.Append("<response>");

            try
            {
                SQL = @"SELECT a.CompanyID, a.CompanyName, b.COUNT
                        FROM (
                                SELECT CompanyID, CompanyName, RANK() OVER (ORDER BY UPPER(CompanyName), companyid) as RowNumber
                                FROM PMIS_ADM.Companies 
                                where UPPER(CompanyName) like UPPER(:prefix) 
                              )  a
                        LEFT OUTER JOIN ( SELECT COUNT(CompanyID) as COUNT FROM PMIS_ADM.Companies where UPPER(CompanyName) like UPPER(:prefix)) b ON 1=1
                        WHERE a.RowNumber BETWEEN (:pageIndex - 1) * :pageCount + 1 AND :pageIndex * :pageCount
                        GROUP BY a.CompanyID, a.CompanyName, b.COUNT
                        ORDER BY UPPER(a.CompanyName)";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("pageIndex", OracleType.Int32).Value = pageIndex;
                cmd.Parameters.Add("pageCount", OracleType.Int32).Value = pageCount;
                cmd.Parameters.Add("prefix", OracleType.VarChar).Value = prefix + "%";

                OracleDataReader dr = cmd.ExecuteReader();

                int count = 0;
                sb.Append("<result>");
                while (dr.Read())
                {
                    count = int.Parse(dr["COUNT"].ToString());
                    sb.Append("<item>");
                    sb.Append("<text>");
                    sb.Append(AJAXTools.EncodeForXML(dr["CompanyName"].ToString()));
                    sb.Append("</text>");
                    sb.Append("<value>");
                    sb.Append(AJAXTools.EncodeForXML(dr["CompanyID"].ToString()));
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

        //Use this function when the company's UIC (EIK, bulstat) should be displayed when searching/browsing the list of companies
        public static string GetAllCompanies_WithPickupItem_ItemSelector(int pageIndex, int pageCount, string prefix, User currentUser)
        {
            StringBuilder sb = new StringBuilder();
            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            sb.Append("<response>");

            try
            {
                SQL = @"SELECT a.CompanyID, a.CompanyName, a.CompanyName_PickupItem, b.COUNT
                        FROM (
                                SELECT CompanyID, LTRIM(UnifiedIdentityCode || ' ' || CompanyName) as CompanyName, CompanyName as CompanyName_PickupItem, RANK() OVER (ORDER BY UPPER(CompanyName), companyid) as RowNumber
                                FROM PMIS_ADM.Companies 
                                where UPPER(CompanyName) like UPPER(:prefix) OR UPPER(UnifiedIdentityCode) like UPPER(:prefix) 
                              )  a
                        LEFT OUTER JOIN ( SELECT COUNT(CompanyID) as COUNT FROM PMIS_ADM.Companies where UPPER(CompanyName) like UPPER(:prefix) OR UPPER(UnifiedIdentityCode) like UPPER(:prefix) ) b ON 1=1
                        WHERE a.RowNumber BETWEEN (:pageIndex - 1) * :pageCount + 1 AND :pageIndex * :pageCount
                        GROUP BY a.CompanyID, a.CompanyName, a.CompanyName_PickupItem, b.COUNT
                        ORDER BY UPPER(a.CompanyName)";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("pageIndex", OracleType.Int32).Value = pageIndex;
                cmd.Parameters.Add("pageCount", OracleType.Int32).Value = pageCount;
                cmd.Parameters.Add("prefix", OracleType.VarChar).Value = prefix + "%";

                OracleDataReader dr = cmd.ExecuteReader();

                int count = 0;
                sb.Append("<result>");
                while (dr.Read())
                {
                    count = int.Parse(dr["COUNT"].ToString());
                    sb.Append("<item>");
                    sb.Append("<text>");
                    sb.Append(AJAXTools.EncodeForXML(dr["CompanyName"].ToString()));
                    sb.Append("</text>");
                    sb.Append("<value>");
                    sb.Append(AJAXTools.EncodeForXML(dr["CompanyID"].ToString()));
                    sb.Append("</value>");
                    sb.Append("<text_pickupitem>");
                    sb.Append(AJAXTools.EncodeForXML(dr["CompanyName_PickupItem"].ToString()));
                    sb.Append("</text_pickupitem>");
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

        public static string CompanySelector_SearchCompanies(string searchType, string searchText, int maxRowNumbers, User currentUser)
        {
            StringBuilder sb = new StringBuilder();
            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            sb.Append("<response>");

            try
            {
                SQL = @"SELECT * FROM (
                            SELECT a.CompanyID, a.CompanyName, a.UnifiedIdentityCode, b.OwnershipTypeName,
                                   d.IME_S || ' ' || c.IME_NMA as CityName,
                                   RANK() OVER (ORDER BY UPPER(a.CompanyName), a.CompanyID) as RowNumber 
                            FROM PMIS_ADM.Companies a
                            LEFT OUTER JOIN PMIS_ADM.OwnershipTypes b ON a.OwnershipTypeID = b.OwnershipTypeID
                            LEFT OUTER JOIN UKAZ_OWNER.KL_NMA c ON a.CityID = c.KOD_NMA
                            LEFT OUTER JOIN UKAZ_OWNER.KL_VNM d ON c.KOD_VNM = d.KOD_VNM
                            WHERE UPPER(a.CompanyName) LIKE UPPER(:searchText) OR
                                  UPPER(a.UnifiedIdentityCode) LIKE UPPER(:searchText)
                        ORDER BY UPPER(a.CompanyName)
                        ) tmp
                        " + (maxRowNumbers > 0 ? " WHERE RowNumber <= " + maxRowNumbers.ToString() : "");

                OracleCommand cmd = new OracleCommand(SQL, conn);
                
                string searchTextLIKE = "";
                switch (searchType)
                {
                    case "starts_with": 
                        searchTextLIKE = searchText.Trim() + "%";
                        break;
                    case "contains":
                        searchTextLIKE = "%" + searchText.Trim() + "%";
                        break;
                    case "ends_with":
                        searchTextLIKE = "%" + searchText.Trim();
                        break;
                }

                cmd.Parameters.Add("searchText", OracleType.VarChar).Value = searchTextLIKE;

                OracleDataReader dr = cmd.ExecuteReader();

                sb.Append("<result>");
                while (dr.Read())
                {
                    sb.Append("<company>");
                    sb.Append("<companyID>");
                    sb.Append(AJAXTools.EncodeForXML(dr["CompanyID"].ToString()));
                    sb.Append("</companyID>");
                    sb.Append("<companyName>");
                    sb.Append(AJAXTools.EncodeForXML(dr["CompanyName"].ToString()));
                    sb.Append("</companyName>");
                    sb.Append("<companyUnifiedIdentityCode>");
                    sb.Append(AJAXTools.EncodeForXML(dr["UnifiedIdentityCode"].ToString()));
                    sb.Append("</companyUnifiedIdentityCode>");
                    sb.Append("<owneshipType>");
                    sb.Append(AJAXTools.EncodeForXML(dr["OwnershipTypeName"].ToString()));
                    sb.Append("</owneshipType>");
                    sb.Append("<cityName>");
                    sb.Append(AJAXTools.EncodeForXML(dr["CityName"].ToString()));
                    sb.Append("</cityName>");
                    sb.Append("</company>");
                }

                dr.Close();

                SQL = @"SELECT COUNT(*) as Cnt
                        FROM PMIS_ADM.Companies a
                        WHERE UPPER(a.CompanyName) LIKE UPPER(:searchText) OR
                              UPPER(a.UnifiedIdentityCode) LIKE UPPER(:searchText)
                        ";

                cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("searchText", OracleType.VarChar).Value = searchTextLIKE;

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    sb.Append("<totalRowsCount>");
                    sb.Append(AJAXTools.EncodeForXML(dr["Cnt"].ToString()));
                    sb.Append("</totalRowsCount>");
                }

                dr.Close();

                sb.Append("</result>");
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
