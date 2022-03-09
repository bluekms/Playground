using System;
using AuthDb;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Test.Models
{
    public class AuthDbFixture : IDisposable
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

        public AuthContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AuthContext>()
                .UseSqlite(connection)
                .Options;
            
            var result = new AuthContext(options);
            result.Database.EnsureCreated();
            
            return result;
        }
    }
}