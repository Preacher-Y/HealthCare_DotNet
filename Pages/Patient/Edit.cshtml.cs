using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using static HealthCare.Pages.Patient.IndexModel;

namespace HealthCare.Pages.Patient
{
    public class EditModel : PageModel
    {
        public Patientinfo patientInfo = new Patientinfo();
        public int patientId { get; set;}
        public string message = "";
        public string style = "green";
        public void OnGet()
        {
            patientId = int.Parse(Request.Query["id"]);
            patientInfo = ListOfPatients.Find(p => p.id == patientId);
            if (patientInfo == null)
            {
                style = "red";
                message = "Patient not found";
            }
        }

        public void OnPost()
        {
            try
            {
                patientId = int.Parse(Request.Form["id"]);
                Console.WriteLine("Patient Id: "+ patientId);
                patientInfo.name = Request.Form["name"];
                patientInfo.email = Request.Form["email"];
                patientInfo.phone = Request.Form["phone"];
                patientInfo.address = Request.Form["address"];

                if (patientId == 0)
                {
                    message = "Patient not found";
                    return;
                }

                if (patientInfo.name.Length == 0 || patientInfo.email.Length == 0 || patientInfo.phone.Length == 0 || patientInfo.address.Length == 0)
                {
                    message = "Please fill all the fields";
                    return;
                }

                string conStr = "Data Source=.;Initial Catalog=HeathCareDB;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = "UPDATE Patient SET name = @name, email = @email, phone = @phone, address = @address WHERE id = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", patientId);
                        cmd.Parameters.AddWithValue("@name", patientInfo.name);
                        cmd.Parameters.AddWithValue("@email", patientInfo.email);
                        cmd.Parameters.AddWithValue("@phone", patientInfo.phone);
                        cmd.Parameters.AddWithValue("@address", patientInfo.address);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            message = "Patient updated successfully";
                            Response.Redirect("/Patient");
                        }
                        else
                        {
                            message = "Patient update failed";
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                patientInfo.name = "";
                patientInfo.email = "";
                patientInfo.phone = "";
                patientInfo.address = "";
                message = "";
            }
        }
    }
}
