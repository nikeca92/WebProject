using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PMIS.Common;


namespace PMIS.Reserve.Common
{
    public class ReservistPersonsDataImporter
    {
        private User currentUser;
        private StreamReader reader;
        private int militaryDepartmentID;
        private int allLines;
        private int errorLines;
        private int noChangesCnt;
        private List<Person> persons;
        private int professionAndSpecialityRecordsImported;
        private List<string> exceptions;
        private int linesCount;
   
        private char colSplitChar = '|';

        public int AllLines
        {
            get { return allLines; }
        }

        public int ErrorLines
        {
            get { return errorLines; }
        }

        public List<Person> Persons
        {
            get { return persons; }
        }

        public int ProfessionAndSpecialityRecordsImported
        {
            get { return professionAndSpecialityRecordsImported; }
        }

        public string[] Exceptions
        {
            get { return exceptions.ToArray(); }
        }

        public int LinesCount
        {
            get { return linesCount; }
        }

        public int NoChangesCnt 
        {
            get { return noChangesCnt; }
        }

        private void ParseLine()
        {
            Change change = new Change(currentUser, "RES_MilitaryReportPersons");

            string line = reader.ReadLine();

            //Define this variable here so that it is avialble in the catch{} block at the bottom
            string lineWithOnlyKnonwnFields = line;

            string[] fields = line.Split(colSplitChar);

            allLines += 1;

            try
            {
                int supportedColumnsCnt = 13;

                if (fields.Count() < supportedColumnsCnt)
                {
                    exceptions.Add(AddExceptionReason(line, "Недостатъчен брой полета"));
                    return;
                }

                string identNumber = fields[0].Trim();
                string firstName = fields[1].Trim();
                string middleName = fields[2].Trim();
                string lastName = fields[3].Trim();
                string inititals = fields[4].Trim();
                string permPostCode = fields[5].Trim();
                string permCityName = fields[6].Trim();
                string permAddress = fields[7].Trim();
                string presPostCode = fields[8].Trim();
                string presCityName = fields[9].Trim();
                string presAddress = fields[10].Trim();
                string professionName = fields[11].Trim();
                string specialityName = fields[12].Trim();

                //use this when registering exceptions to prevent adding more and more exception columns at the end if the user tries to re-upload a problematic record which cannont be imported again and again
                lineWithOnlyKnonwnFields = "";
                for (int i = 0; i < supportedColumnsCnt; i++)
                    lineWithOnlyKnonwnFields += (i == 0 ? "" : colSplitChar.ToString()) + fields[i];

                Profession profession = ProfessionUtil.GetProfession(professionName, currentUser);
                Speciality speciality = null;
                if (profession != null && !String.IsNullOrEmpty(specialityName))
                {
                    speciality = SpecialityUtil.GetSpeciality(profession.ProfessionId, specialityName, currentUser);
                }

                if (profession == null && !String.IsNullOrEmpty(professionName))
                {
                    exceptions.Add(AddExceptionReason(lineWithOnlyKnonwnFields, "Непозната професия"));
                    return;
                }
                else
                {
                    if (speciality == null && !String.IsNullOrEmpty(specialityName))
                    {
                        exceptions.Add(AddExceptionReason(lineWithOnlyKnonwnFields, "Непозната специалност"));
                        return;
                    }
                    else
                    {
                        Person person = PersonUtil.GetPersonByIdentNumber(identNumber, currentUser);
                        
                        if (person != null)
                        {
                            bool noChange = true;
                            bool isMilRepPerson = true;
                            Reservist tmpReservist = ReservistUtil.GetReservistByIdentNumber(person.IdentNumber, currentUser);
                            if (tmpReservist == null)
                            {
                                isMilRepPerson = false;                                
                            }
                            else 
                            {
                                ReservistMilRepStatus milRepStat = ReservistMilRepStatusUtil.GetReservistMilRepCurrentStatusByReservistId(tmpReservist.ReservistId, currentUser);
                                if (milRepStat.MilitaryReportStatus.MilitaryReportStatusKey != "MILITARY_REPORT_PERSONS")
                                {                                   
                                    isMilRepPerson = false;
                                }
                            
                            }

                            if (!isMilRepPerson)
                            {
                                noChange = false;
                                exceptions.Add(AddExceptionReason(lineWithOnlyKnonwnFields, "Присъства в АСУ в състояние различно от ВОЛ"));
                            }
                            else
                            {
                               if ((person.FirstName != firstName + " " + middleName) ||
                               (person.LastName != lastName) ||
                               (person.Initials != inititals) ||
                               ((string.IsNullOrEmpty(person.PermSecondPostCode) ? "" : person.PermSecondPostCode) != permPostCode) ||
                               ((person.PermCity != null ? person.PermCity.CityName : "") != permCityName) ||
                               ((string.IsNullOrEmpty(person.PermAddress) ? "" : person.PermAddress) != permAddress) ||
                               ((string.IsNullOrEmpty(person.PresSecondPostCode) ? "" : person.PresSecondPostCode) != presPostCode) ||
                               ((person.PresCity != null ? person.PresCity.CityName : "") != presCityName) ||
                               ((string.IsNullOrEmpty(person.PresAddress) ? "" : person.PresAddress) != presAddress))
                                {
                                    noChange = false;
                                    exceptions.Add(AddExceptionReason(lineWithOnlyKnonwnFields, "Присъства в АСУ с различни лични данни"));
                                }
                                else
                                {
                                    if (profession != null)
                                    {
                                        PersonSpeciality personSpeciality = PersonSpecialityUtil.GetPersonSpeciality(person.PersonId, profession.ProfessionId, (speciality != null ? speciality.SpecialityId : (int?)null), currentUser);
                                        if (personSpeciality == null)
                                        {
                                            personSpeciality = new PersonSpeciality(currentUser);

                                            personSpeciality.PersonSpecialityID = 0;
                                            personSpeciality.PersonID = person.PersonId;
                                            personSpeciality.ProfessionID = profession.ProfessionId;
                                            personSpeciality.SpecialityID = (speciality != null ? speciality.SpecialityId : (int?)null);

                                            PersonSpecialityUtil.SavePersonSpeciality(personSpeciality, person, currentUser, change);

                                            professionAndSpecialityRecordsImported++;
                                            noChange = false;
                                        }
                                    }
                                }
                            }
                                                     
                            if(noChange)
                                this.noChangesCnt++;

                            return;
                        }
                        else
                        {
                            City permCity = CityUtil.GetCityByName(permCityName, currentUser);
                            City presCity = CityUtil.GetCityByName(presCityName, currentUser);

                            person = new Person(currentUser);
                        
                            person.IdentNumber = identNumber;                         
                            person.FirstName = firstName + " " + middleName;
                            person.LastName = lastName;
                            person.Initials = inititals;
                            person.PermSecondPostCode = permPostCode;
                            person.PermCityId = permCity != null ? (int?)permCity.CityId : null;
                            person.PermAddress = permAddress;
                            person.PresSecondPostCode = presPostCode;
                            person.PresCityId = presCity != null ? (int?)presCity.CityId : null;
                            person.PresAddress = presAddress;

                            PersonUtil.SavePerson_WhenImportData(person, currentUser, change);

                            Reservist reservist = new Reservist(currentUser);
                            reservist.PersonId = person.PersonId;
                            
                            ReservistUtil.AddReservist(reservist, currentUser, change);

                            ReservistMilRepStatusUtil.SetMilRepStatusTo_MILITARY_REPORT_PERSONS(reservist.ReservistId, militaryDepartmentID, currentUser, change);

                            if (profession != null)
                            {
                                PersonSpeciality personSpeciality = new PersonSpeciality(currentUser);

                                personSpeciality.PersonSpecialityID = 0;
                                personSpeciality.PersonID = person.PersonId;
                                personSpeciality.ProfessionID = profession.ProfessionId;
                                personSpeciality.SpecialityID = (speciality != null ? speciality.SpecialityId : (int?)null);

                                PersonSpecialityUtil.SavePersonSpeciality(personSpeciality, person, currentUser, change);

                                professionAndSpecialityRecordsImported++;
                            }

                            Persons.Add(person);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                errorLines += 1;
                exceptions.Add(AddExceptionReason(lineWithOnlyKnonwnFields, "Системна грешка по време на обработката: [" + ex.Message.Replace("\r\n", " ").Replace("\n\r", " ").Replace("\n", " ").Replace("\r", " ").Replace(colSplitChar.ToString(), " ") + "]"));
            }

            if (change.HasEvents)
                change.WriteLog();
        }

        public void ParseImportFile()
        {
            Persons.Clear();
            exceptions.Clear();
            allLines = 0;
            errorLines = 0;
            professionAndSpecialityRecordsImported = 0;

            while (!reader.EndOfStream)
                ParseLine();
        }

        public ReservistPersonsDataImporter(User currentUser, Stream importFile, int militaryDepartmentID)
        {
            this.currentUser = currentUser;
            this.militaryDepartmentID = militaryDepartmentID;
            this.reader = new StreamReader(importFile, Encoding.Default);
            this.persons = new List<Person>();
            this.exceptions = new List<string>();

            var stream2 = new MemoryStream();
            CopyStream(importFile, stream2);
            var reader2 = new StreamReader(stream2, Encoding.Default);
            while (!reader2.EndOfStream)
            {
                reader2.ReadLine();
                this.linesCount++;
            }

        }

        //https://stackoverflow.com/questions/230128/how-do-i-copy-the-contents-of-one-stream-to-another
        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }

            input.Seek(0, SeekOrigin.Begin);
            output.Seek(0, SeekOrigin.Begin);
        }

        private string AddExceptionReason(string line, string reason)
        {
            return line.TrimEnd() + " " + colSplitChar + " " + reason;
        }
    }
}
