using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WMS.CrossCuttingConcerns.Configuration;
using WorkManagementSystem.Infrastructure.Persistence;

namespace WorkManagementSystem.Infrastructure.Factory
{
    public class ContextFactory : IContextFactory
    {
        private readonly IOptions<ConnectionSettings> _connectionOptions;

        public ContextFactory(IOptions<ConnectionSettings> connectionOptions)
        {
            _connectionOptions = connectionOptions;
        }

        public IDatabaseContext DbContext => new MainDbContext(GetDataContext().Options);

        private DbContextOptionsBuilder<MainDbContext> GetDataContext()
        {
           // ValidateDefaultConnection();

            var sqlConnectionBuilder = new SqlConnectionStringBuilder("Server=103.229.42.125, 1433;Database=NotificationDbNew;User Id=sa;Password=Digins@2022;TrustServerCertificate=true;");

            var contextOptionsBuilder = new DbContextOptionsBuilder<MainDbContext>();

            contextOptionsBuilder.UseSqlServer(sqlConnectionBuilder.ConnectionString);

            return contextOptionsBuilder;
        }

        //private void ValidateDefaultConnection()
        //{
        //    if (string.IsNullOrEmpty(_connectionOptions.Value.DefaultConnection))
        //    {
        //        throw new ArgumentNullException(nameof(_connectionOptions.Value.DefaultConnection));
        //    }
        //}
    }
}
