using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace AuthServiceTests
{
    public class TestConfiguration : IConfiguration
    {
        Dictionary<string,IConfigurationSection> _dictionary = new Dictionary<string, IConfigurationSection>();
        public IConfigurationSection GetSection(string key)
        {
            return _dictionary[key];
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public string this[string key]
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}