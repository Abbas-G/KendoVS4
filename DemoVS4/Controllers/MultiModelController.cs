using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoVS4.Controllers
{
    public class MultiModelController : Controller
    {
        //
        // GET: /MultiModel/
        //LINK many ways here http://www.c-sharpcorner.com/UploadFile/ff2f08/multiple-models-in-single-view-in-mvc/
        //one of the way is using tuple
        public ActionResult tuple()
        {
            var tupleModel = new Tuple<List<Teacher>, List<Student>,Product>(GetTeachers(), GetStudents(),null);
            return View(tupleModel);
        }

        /*
         if you look at Tuple implementation you will see that there is no any parameter-less constructor for it,
         * and MVC model binding works with parameter-less constructor. so you can't use Tuple in MVC model binding.
         */
        [HttpPost]
        public ActionResult form(Tuple<List<Teacher>, List<Student>,Product> a) {
            string name = a.Item3.ProductName;
            return RedirectToAction("tuple");
        }
        
        private List<Teacher> GetTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();
            teachers.Add(new Teacher { TeacherId = 1, Code = "TT", Name = "Tejas Trivedi" });
            teachers.Add(new Teacher { TeacherId = 2, Code = "JT", Name = "Jignesh Trivedi" });
            teachers.Add(new Teacher { TeacherId = 3, Code = "RT", Name = "Rakesh Trivedi" });
            return teachers;
        }

        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            students.Add(new Student { StudentId = 1, Code = "L0001", Name = "Amit Gupta", EnrollmentNo = "201404150001" });
            students.Add(new Student { StudentId = 2, Code = "L0002", Name = "Chetan Gujjar", EnrollmentNo = "201404150002" });
            students.Add(new Student { StudentId = 3, Code = "L0003", Name = "Bhavin Patel", EnrollmentNo = "201404150003" });
            return students;
        }

    }
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string EnrollmentNo { get; set; }
    }

    public class Product
    {
        public string ProductName { get; set; }
    }
}
