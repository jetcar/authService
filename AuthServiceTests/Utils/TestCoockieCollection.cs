using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AuthServiceTests
{
    internal class TestCoockieCollection : IRequestCookieCollection
    {
        Dictionary<string,string> _dictionary = new Dictionary<string, string>();
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out string value)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, string value)
        {
            _dictionary[key] = value;
        }

        public int Count { get; }

        public string this[string key] => _dictionary[key];

        public ICollection<string> Keys { get; }
    }
}