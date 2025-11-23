using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace HealthCare.Pages.Patient
{
    public class IndexModel : PageModel
    {
        public static List<Patientinfo> ListOfPatients = new List<Patientinfo>();
        public void OnGet()
        {
            ListOfPatients.Clear();

            try
            {
                string conStr = "Data Source=.;Initial Catalog=HeathCareDB;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "SELECT * FROM Patient";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Patientinfo patient = new Patientinfo();
                                patient.id = Convert.ToInt32(reader["id"]);
                                patient.name = reader["name"].ToString();
                                patient.email = reader["email"].ToString();
                                patient.phone = reader["phone"].ToString();
                                patient.address = reader["address"].ToString();
                                patient.createdAt = Convert.ToDateTime(reader["createdAt"]).ToString();

                                ListOfPatients.Add(patient);
                            }
                        }
                    }
                    con.Close();
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class Patientinfo
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public string createdAt { get; set; }
        }
    }
}
