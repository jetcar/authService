using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using CommonTools.Utils;
using Microsoft.Extensions.Configuration;
using MobileId.Config;
using RestSharp;

namespace MobileId.MobileId
{
    public class MobileIdService : IMobileIdService
    {
        private IConfiguration _configuration;
        private MobileIdConfig _config;

        public MobileIdService(IConfiguration configuration)
        {
            _configuration = configuration;
            _config = new MobileIdConfig();
            _configuration.GetSection(MobileIdConfig.Position).Bind(_config);
        }

        public MobileIdSessionResponse StartLogin(string personalId, string number, Languages language, out string verificationCode)
        {
            var code = MyRandom.NextString(32);
            string codeHashed = "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(code));

                codeHashed = Convert.ToBase64String(bytes);
                verificationCode = GetCodeFromHash(codeHashed);
            }

            var client = new RestClient();
            var request = new RestRequest(_config.Url + "/authentication", Method.POST);
            var mobileIdRequest = new MobileIdRequest()
            {
                hash = codeHashed,
                nationalIdentityNumber = personalId,
                phoneNumber = number,
                language = language.ToString(),
                hashType = _config.HashType,
                relyingPartyName = _config.relyingPartyName,
                relyingPartyUUID = _config.relyingPartyUUID,
                displayText = _config.DisplayText,
            };
            request.AddJsonBody(mobileIdRequest, "application/json");

            var response = client.Execute<MobileIdSessionResponse>(request);

            if (!response.IsSuccessful)
                throw new MobileIdException(response.StatusCode, response.Data.error, null);
            return response.Data;
        }

        public static string GetCodeFromHash(string base64Hash)
        {
            var hash = Convert.FromBase64String(base64Hash);
            var code = ((0xFC & hash[0]) << 5) | (hash[hash.Length - 1] & 0x7F);
            return code.ToString().PadLeft(4, '0'); ;
        }

        public MobileIdCheckSessionRequest CheckSession(string sessionId)
        {
            var client = new RestClient();
            var request = new RestRequest(_config.Url + $"/authentication/session/{sessionId}?timeoutMs={_config.Timeout}", Method.GET);

            var response = client.Execute<MobileIdCheckSessionRequest>(request);
            if (response.Data.state == States.RUNNING)
                return null;
            if (!response.IsSuccessful || (response.Data.result != null && response.Data.result != Results.OK))
            {
                throw new MobileIdException(response.StatusCode, response.ErrorMessage, response.Data.result);
            }

            return response.Data;
        }
    }

    public class MobileIdException : Exception
    {
        private readonly HttpStatusCode _responseStatusCode;
        private readonly Results? _dataResult;

        public MobileIdException(HttpStatusCode responseStatusCode, string dataError, Results? dataResult) : base(dataError)
        {
            _responseStatusCode = responseStatusCode;
            _dataResult = dataResult;
        }
    }

    public interface IMobileIdService
    {
        MobileIdSessionResponse StartLogin(string personalId, string number, Languages language,
            out string verificationCode);

        MobileIdCheckSessionRequest CheckSession(string sessionId);
    }
}