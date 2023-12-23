using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Football_Club.Pages.Clubs;
using Football_Club.Pages.Clients.Tiers;

namespace Football_Club.Pages.Clients.User
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";
        private readonly string _connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        public List<IndexClubInfo> ClubList { get; set; } = new List<IndexClubInfo>();
        public List<MembershipTier> ListTiers { get; set; } = new List<MembershipTier>();

        public void OnGet()
        {
            ClubList = new List<IndexClubInfo>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Clubs";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                IndexClubInfo clubInfo = new IndexClubInfo
                                {
                                    ClubID = reader.GetInt32(0),
                                    ClubName = reader.GetString(1),
                                    Location = reader.GetString(2)
                                };

                                ClubList.Add(clubInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display an error message to the user
                Console.WriteLine("Exception:" + ex.ToString());
            }

            try
            {
                string connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT TierName FROM MembershipTiers";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MembershipTier tier = new MembershipTier
                                {
                                    TierName = reader.GetString(0),
                                    
                                };

                                ListTiers.Add(tier);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.ToString());
            }

        }

        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            clientInfo.club = Request.Form["club"];
            clientInfo.membership_tier = Request.Form["membership_tier"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0 ||
                clientInfo.club.Length == 0 || clientInfo.membership_tier.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO clients " + "(name, email, phone, address, club, membership_tier) VALUES" + "(@name, @email, @phone, @address, @club, @membership_tier);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@club", clientInfo.club);
                        command.Parameters.AddWithValue("@membership_tier", clientInfo.membership_tier);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }


            clientInfo.name = "";
            clientInfo.email = "";
            clientInfo.phone = "";
            clientInfo.address = "";
            clientInfo.club = "";
            clientInfo.membership_tier = "";

            successMessage = "New client added successfully";
            Response.Redirect("/Clients/User/Index");
        }
    }
}
