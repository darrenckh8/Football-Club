using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Football_Club.Pages.Clients.Tiers
{
    public class TiersCreateModel : PageModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string errorMessage = "";
        public string successMessage = "";

        private readonly string _connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public void OnPost()
        {
            Name = Request.Form["name"];
            Description = Request.Form["description"];

            if (Name.Length == 0 || Description.Length == 0)
            {
                errorMessage = "Name and Description are required fields.";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO MembershipTiers (TierName, Description) VALUES (@name, @description)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", Name);
                        command.Parameters.AddWithValue("@description", Description);

                        command.ExecuteNonQuery();
                    }
                }

                successMessage = "New membership tier added successfully";
                Response.Redirect("/Clients/Tiers/TiersIndex");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
