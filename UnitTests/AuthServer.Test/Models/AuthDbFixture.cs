using System;
using AuthDb;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Test.Models
{
    public sealed class AuthDbFixture : IDisposable
    {
        private readonly SqliteConnection connection;

        public AuthDbFixture()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public AuthDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AuthDbContext>()
                .UseSqlite(connection)
                .Options;

            var result = new AuthDbContext(options);
            result.Database.EnsureCreated();

            return result;
        }
    }
}