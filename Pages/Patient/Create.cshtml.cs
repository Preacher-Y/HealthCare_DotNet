using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace HealthCare.Pages.Patient
{
    public class CreateModel : PageModel
    {
        public string message = "";
        public Patient.IndexModel.Patientinfo patientinfo = new Patient.IndexModel.Patientinfo();
        public void OnGet()
        {
        }
        public void OnPost() {
            try
            {
                patientinfo.name = Request.Form["name"];
                patientinfo.email = Request.Form["email"];
                patientinfo.phone = Request.Form["phone"];
                patientinfo.address = Request.Form["address"];

                if (patientinfo.name.Length == 0 || patientinfo.email.Length == 0 || patientinfo.phone.Length == 0 || patientinfo.address.Length == 0)
                {
                    message = "Please fill all the fields";
                    return;
                }

                string conStr = "Data Source=.;Initial Catalog=HeathCareDB;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "INSERT INTO Patient (name, email, phone, address) VALUES (@name, @email, @phone, @address)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@name", patientinfo.name);
                        cmd.Parameters.AddWithValue("@email", patientinfo.email);
                        cmd.Parameters.AddWithValue("@phone", patientinfo.phone);
                        cmd.Parameters.AddWithValue("@address", patientinfo.address);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            message= "Patient created successfully";
                            Response.Redirect("/Patient");
                        } else
                        {
                            message= "Patient creation failed";
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " +ex.Message);
            }
            finally
            {
                patientinfo.name = "";
                patientinfo.email = "";
                patientinfo.phone = "";
                patientinfo.address = "";
                message = "";
            }
        }
    }
}
