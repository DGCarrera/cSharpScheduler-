using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ClassLibraryCore.Model
{
    [XmlRootAttribute("Classes")]
    public class ClassInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Memo { get; set; }
        public int InstructorId { get; set; }
        public string ClassStatus { get; set; }
        public int AssesmentType { get; set; }
        public string Asssesment1Name { get; set; }
        public string Assessment1StartDate { get; set; }
        public string Assessment1EndDate { get; set; }
        public int AssesmentType2 { get; set; }
        public string Asssesment2Name { get; set; }
        public string Assessment2StartDate { get; set; }
        public string Assessment2EndDate { get; set; }


        private List<ClassInfo> WGUClassList = new List<ClassInfo>();
        public void AddToClassList(ClassInfo classobject)
        {
            WGUClassList.Add(classobject);
        }

        public ClassInfo GetClassInfo(string search)
        {
            foreach (ClassInfo classinfo in WGUClassList)
            {
                int a;
                int.TryParse(search, out a);
                return classinfo.Id == a || classinfo.Name == search
                    ? (ClassInfo)classinfo
                    : (ClassInfo)false;
            }
            return (ClassInfo)false;
        }

        public static explicit operator ClassInfo(bool v)
        {
            throw new Exception("Class does not exist.");
        }

        public void DeleteClass(int id)
        {
            foreach (ClassInfo classinfo in WGUClassList)
            {
                if (classinfo.Id == id)
                {
                    WGUClassList.Remove(classinfo);
                }
            }
        }

        public void EditClassInfo(int id, string name, string startdate, string enddate, string memo, int instructorid)
        {
            foreach (ClassInfo classinfo in WGUClassList)
            {
                if (classinfo.Id == id)
                {
                    classinfo.Name = name;
                    classinfo.StartDate = startdate;
                    classinfo.EndDate = enddate;
                    classinfo.Memo = memo;
                    classinfo.InstructorId = instructorid;
                }

            }
        }

        public enum Assesment
        {
            Objective,
            Performance
        }

        public string AssesmentKind(int temp) 
        { 
            if ( temp == 0)
            {
                string tempAssesment = "Objective";
                return tempAssesment;
            } else if (temp == 1)
            {
                string tempAssesment = "Performance";
                return tempAssesment;
            } else
            {
                string tempAssesment = "None";
                return tempAssesment;
            }
        }

        public object GetAssesmentValues()
        {
            var values = Enum.GetValues(typeof(Assesment));
            return values;
        }


    }
}
