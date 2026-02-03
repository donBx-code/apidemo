using Microsoft.EntityFrameworkCore;


namespace apidemo.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var clients = new List<Client>();
            var metricNames = new[] { "AUM", "NAV", "Return", "Yield", "Risk" };
            var random = new Random(42); // Fixed seed for reproducible data
            var startDate = new DateTime(2025, 1, 1);

            for (int i = 1; i <= 1100; i++)
            {
                clients.Add(new Client
                {
                    Id = i,
                    client_id = 100 + (i % 50),  // 50 unique clients
                    fund_id = 1000 + (i % 20),   // 20 unique funds
                    as_of_date = startDate.AddDays(random.Next(0, 365)),
                    metric_name = metricNames[i % metricNames.Length],
                    metric_value = Math.Round((decimal)(random.NextDouble() * 1000000), 2)
                });
            }

            modelBuilder.Entity<Client>().HasData(clients);
        }
    }
}
