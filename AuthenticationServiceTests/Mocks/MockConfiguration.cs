using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace AuthenticationServiceTests.Mocks
{

    internal class MockConfiguration : Microsoft.Extensions.Configuration.IConfiguration
    {
        private IConfigurationRoot configurationRoot;

        public MockConfiguration()
        {
            SetConfigurationRoot();
        }
        private void SetConfigurationRoot()
        {
            var appSettings = @"{ ""AppSettings"": {
                                   ""Token"": ""keykeykeykeykeykeykeykey"",
                                   ""ValidIssuer"": ""Issuer"",
                                   ""ValidAudience"": ""Audience""
                                 }}";

            var builder = new ConfigurationBuilder();

            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

            configurationRoot = builder.Build();
        }

        public IConfigurationSection GetSection(string key)
        {
            return new ConfigurationSection(configurationRoot, key);
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public string? this[string key]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
