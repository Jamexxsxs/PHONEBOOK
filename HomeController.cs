using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace PhoneBook.Controllers
{
    public class HomeController : Controller
    {
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Denise Valerie\source\repos\PhoneBook\PhoneBook\App_Data\Phonebook.mdf;Integrated Security=True";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult addContact()
        {
            return View();
        }
        public ActionResult postAddContact()
        {
            var data = new List<object>();
            string name = Request["name"];
            string areacode =Request["areacode"];
            string phonenumber = Request["phonenumber"];
            string mobilenumber = Request["mobilenumber"];
            int housenumber = int.Parse(Request["housenumber"]);
            string street = Request["street"];
            string city = Request["city"];
            string province = Request["province"];
            int zipcode = int.Parse(Request["zipcode"]);
            string emailaddress = Request["emailaddress"];


            using (var db = new SqlConnection(connStr))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO CONTACT (CON_NAME, CON_AREACODE, CON_PHONENUMBER, CON_MOBILENUMBER, CON_HOUSENUMBER, CON_STREET, CON_CITY, CON_PROVINCE, CON_ZIP, CON_EMAILADDRESS)" +
                        " VALUES (@con_name, @con_areacode, @con_phonenumber, @con_mobilenumber, @con_housenumber, @con_street, @con_city, @con_province, @con_zip, @con_emailaddress)";
                    cmd.Parameters.AddWithValue("@con_name", name);
                    cmd.Parameters.AddWithValue("@con_areacode", areacode);
                    cmd.Parameters.AddWithValue("@con_phonenumber", phonenumber);
                    cmd.Parameters.AddWithValue("@con_mobilenumber", mobilenumber);
                    cmd.Parameters.AddWithValue("@con_housenumber", housenumber);
                    cmd.Parameters.AddWithValue("@con_street", street);
                    cmd.Parameters.AddWithValue("@con_city", city);
                    cmd.Parameters.AddWithValue("@con_province", province);
                    cmd.Parameters.AddWithValue("@con_zip", zipcode);
                    cmd.Parameters.AddWithValue("@con_emailaddress", emailaddress);

                    try
                    {
                        var ctr = cmd.ExecuteNonQuery();

                        if (ctr >= 1)
                        {
                            data.Add(new
                            {
                                mess = 1
                            });
                        }
                    }
                    catch(SqlException ex)
                    {
                        if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                        {
                            data.Add(new
                            {
                                mess = 0
                            });
                        }
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult viewAllContact()
        {
            var data = new List<Dictionary<string, object>>();

            using (var db = new SqlConnection(connStr))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT CON_NAME, CON_AREACODE, CON_PHONENUMBER, CON_MOBILENUMBER, CON_HOUSENUMBER, CON_STREET, CON_CITY, CON_PROVINCE, CON_ZIP, CON_EMAILADDRESS FROM CONTACT";
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contact = new Dictionary<string, object>
                            {
                                { "CON_NAME", reader["CON_NAME"] },
                                { "CON_AREACODE", reader["CON_AREACODE"] },
                                { "CON_PHONENUMBER", reader["CON_PHONENUMBER"] },
                                { "CON_MOBILENUMBER", reader["CON_MOBILENUMBER"] },
                                { "CON_HOUSENUMBER", reader["CON_HOUSENUMBER"] },
                                { "CON_STREET", reader["CON_STREET"] },
                                { "CON_CITY", reader["CON_CITY"] },
                                { "CON_PROVINCE", reader["CON_PROVINCE"] },
                                { "CON_ZIP", reader["CON_ZIP"] },
                                { "CON_EMAILADDRESS", reader["CON_EMAILADDRESS"] },
                            };
                            data.Add(contact);
                        }
                    }
                }
            }
            ViewBag.Contacts = data;
            return View();
        }

        public ActionResult postSearch()
        {
            var data = new List<Dictionary<string, object>>();
            string search = Request["search"];

            using (var db = new SqlConnection(connStr))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT CON_NAME, CON_AREACODE, CON_PHONENUMBER, CON_MOBILENUMBER, CON_HOUSENUMBER, CON_STREET, CON_CITY, CON_PROVINCE, CON_ZIP, CON_EMAILADDRESS FROM CONTACT " +
                        " WHERE CON_PHONENUMBER LIKE @search";
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contact = new Dictionary<string, object>
                            {
                                { "CON_NAME", reader["CON_NAME"] },
                                { "CON_AREACODE", reader["CON_AREACODE"] },
                                { "CON_PHONENUMBER", reader["CON_PHONENUMBER"] },
                                { "CON_MOBILENUMBER", reader["CON_MOBILENUMBER"] },
                                { "CON_HOUSENUMBER", reader["CON_HOUSENUMBER"] },
                                { "CON_STREET", reader["CON_STREET"] },
                                { "CON_CITY", reader["CON_CITY"] },
                                { "CON_PROVINCE", reader["CON_PROVINCE"] },
                                { "CON_ZIP", reader["CON_ZIP"] },
                                { "CON_EMAILADDRESS", reader["CON_EMAILADDRESS"] }
                            };
                            data.Add(contact);
                        }
                    }

                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}