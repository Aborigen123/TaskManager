using DataLayer;
namespace InventorSoftTestTask
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            context.Database.EnsureCreated();

            var identitySeed = scope.ServiceProvider.GetRequiredService<IdentitySeed>();
            await identitySeed.SeedBaseRecords();

            try
            {
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("The job operation was cancelled: {message}", ex.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}