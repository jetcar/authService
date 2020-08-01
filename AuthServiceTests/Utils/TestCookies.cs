using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AuthServiceTests.Controllers
{
    public class TestCookies : IResponseCookies
    {
        public Dictionary<string,string> _dict = new Dictionary<string, string>();
        public List<string> _toDelete = new List<string>();
        public string Get(string key)
        {
            return _dict[key];
        }

        public void Append(string key, string value)
        {
           _dict[key] = value;
        }

        public void Append(string key, string value, CookieOptions options)
        {
            _dict[key] = value;
        }

        public void Delete(string key)
        {
           _toDelete.Add(key);
        }

        public void Delete(string key, CookieOptions options)
        {
           _toDelete.Add(key);
        }
    }
}