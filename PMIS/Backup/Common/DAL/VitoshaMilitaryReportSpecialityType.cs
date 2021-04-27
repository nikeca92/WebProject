using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMIS.Common
{
    public class VitoshaMilitaryReportSpecialityType : IDropDownItem
    {
        private string mType;
        private string mTypeName;

        public string Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public string TypeName
        {
            get { return mTypeName; }
            set { mTypeName = value; }
        }

        public string Text()
        {
            return TypeName;
        }

        public string Value()
        {
            return Type.ToString();
        }
    }

    public static class VitoshaMilitaryReportSpecialityTypeUtil
    {
        public static List<VitoshaMilitaryReportSpecialityType> VitoshaMilitaryReportSpecialityTypes = new List<VitoshaMilitaryReportSpecialityType>(){
                new VitoshaMilitaryReportSpecialityType(){Type = "5", TypeName = "войници"},
                new VitoshaMilitaryReportSpecialityType(){Type = "6", TypeName = "сержанти"},
                new VitoshaMilitaryReportSpecialityType(){Type = "7", TypeName = "офицери"},
                new VitoshaMilitaryReportSpecialityType(){Type = "8", TypeName = "офицери с ВА"},
                new VitoshaMilitaryReportSpecialityType(){Type = "9", TypeName = "генерали"}
            };         

        public static List<VitoshaMilitaryReportSpecialityType> GetAllVitoshaMilitaryReportSpecialityTypes(User pCurrentUser)
        {
            return VitoshaMilitaryReportSpecialityTypes;         
        }

        public static VitoshaMilitaryReportSpecialityType GetVitoshaMilitaryReportSpecialityType(string pVitoshaMilReportSpecialityTypeID, User pCurrentUser)
        {
            VitoshaMilitaryReportSpecialityType vitoshaMilitaryReportSpecialityType = null;
            for (int i = 0; i != VitoshaMilitaryReportSpecialityTypes.Count; ++i)
            {
                if (VitoshaMilitaryReportSpecialityTypes[i].Type == pVitoshaMilReportSpecialityTypeID)
                    vitoshaMilitaryReportSpecialityType = VitoshaMilitaryReportSpecialityTypes[i];
            }

            return vitoshaMilitaryReportSpecialityType;
        }
    }
}
