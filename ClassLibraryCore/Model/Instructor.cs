using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ClassLibraryCore.Model
{
    [XmlRootAttribute("Instructors")]
    public class Instructor
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        private List<Instructor> InstructorList = new List<Instructor>();
        public void AddInstructor (Instructor instructorObject)
        {
            InstructorList.Add(instructorObject);
        }

        public Instructor GetInstructor (string search)
        {
           foreach (Instructor instructor in InstructorList)
            {
                int a;
                int.TryParse(search, out a);
               return instructor.Id == a || instructor.Name == search || instructor.Email == search || instructor.PhoneNumber == search
                   ? (Instructor)instructor
                   : (Instructor)false;
            }
            return (Instructor)false;
        }

        public static explicit operator Instructor(bool v)
        {
            throw new Exception("This class does not exist.");
        }

        public void DeleteInstructor (int id)
        {
            foreach (Instructor instructor in InstructorList)
            {
                if (instructor.Id == id )
                {
                    InstructorList.Remove(instructor);
                } 
            }
        }

        public void EditInstructor (int id, string name, string phonenumber, string email)
        {
            foreach (Instructor instructor in InstructorList)
            {
                if (id == instructor.Id)
                {
                    instructor.Name = name;
                    instructor.PhoneNumber = phonenumber;
                    instructor.Email = email;
                } 
            }
        }

        public List<Instructor> Instructors = new List<Instructor>()
       {
          
       };

        public List<Instructor> GetInstructors()
        {
            return Instructors;
        } 

       
    }
}
