using System;
using AuthDb;
using AuthServer.Handlers.Account;
using AuthServer.Test.Models;
using CommonLibrary.Models;
using Xunit;

namespace AuthServer.Test.Handlers.Account
{
    public class LoginRuleCheckerTester : IDisposable
    {
        private readonly AuthDbFixture authDbFixture;
        private readonly AuthContext context;
        
        public LoginRuleCheckerTester()
        {
            authDbFixture = new();
            context = authDbFixture.CreateContext();

            InitData();
        }

        private void InitData()
        {
            context.Accounts.Add(new()
            {
                Token = string.Empty,
                AccountId = "bluekms",
                Password = "1234",
                CreatedAt = DateTime.Now,
                Role = UserRoles.Administrator,
            });
            context.SaveChanges();
        }

        [Theory]
        [InlineData("bluekms", "1234")]
        public async void Test(string accountId, string password)
        {
            var ruleChecker = new LoginRuleChecker(context);
            await ruleChecker.CheckAsync(new(accountId, password));
        }

        public void Dispose()
        {
            context.Dispose();
            authDbFixture.Dispose();
        }
    }
}