using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;

namespace AuthService.Security
{
    public class JwtHelper
    {
        public static string Encode<T>(T jsonObject)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var key = Environment.GetEnvironmentVariable("secret");
            return encoder.Encode(jsonObject, key);
        }

        public static T Decode<T>(string jsonObject)
        {
            var builder = new JwtBuilder();
            builder.WithAlgorithm(new HMACSHA256Algorithm());
            builder.WithSecret(Environment.GetEnvironmentVariable("secret"));
            builder.WithVerifySignature(true);

            T decodedDto = builder.Decode<T>(jsonObject);
            return decodedDto;
        }

    }
}
