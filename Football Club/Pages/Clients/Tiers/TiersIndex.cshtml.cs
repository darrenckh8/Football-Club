using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Football_Club.Pages.Clients.Tiers
{
    public class TiersIndexModel : PageModel
    {
        public List<MembershipTier> ListTiers { get; set; } = new List<MembershipTier>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DARREN-TOWER;Initial Catalog=football;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM MembershipTiers";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MembershipTier tier = new MembershipTier
                                {
                                    ID = reader.GetInt32(0),
                                    TierName = reader.GetString(1),
                                    Description = reader.GetString(2)
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
    }

    public class MembershipTier
    {
        public int ID { get; set; }
        public string TierName { get; set; }
        public string Description { get; set; }
    }
}
