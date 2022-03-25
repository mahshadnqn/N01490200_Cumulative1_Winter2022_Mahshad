using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace N01490200_Cumulative1_Winter2022_Mahshad.Controllers
{
    public class TeacherDataController : Controller
    {

        private SchoolDbContext School = new SchoolDbContext();

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<string> ListTeachers(string SearchKey = null)
        {

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key) or lower(salary) like lower(@key) or lower(hiredate) like lower(@key)";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<String> Teachers = new List<Teachers> { };

            while (ResultSet.Read())
            {
             
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime Hire = Convert.ToDateTime(ResultSet["hiredate"].ToString());
                Decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher();

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.TeacherHireDate = Hire;
                NewTeacher.TeacherSalary = Salary;

                Teachers.Add(NewTeacher);
            }

            Conn.Close();
            return NewTeacher;
        }

        //Find teacher with specified id

        [HttpGet]
        public string FindTeacher(int id)
        {
            string query = "SELECT * FROM teachers WHERE teacherid =" + id;
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand Cmd = Conn.CreateCommand();
            Cmd.CommandText = query;
            MySqlDataReader ResultSet = Cmd.ExecuteReader();

            string TeacherName = "";
            while (ResultSet.Read())
            {
                TeacherName = ResultSet["teacherfname"] + " " + ResultSet["teacherlname"];
            }

            Conn.Close();
            return TeacherName;
        }

    }
}