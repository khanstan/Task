using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XMLSourceMicroservice.Services
{
    public class HMACAuthenticationService : IHMACAuthenticationService
    {
        private static Dictionary<string, string> allowedApps = new Dictionary<string, string>();

        public HMACAuthenticationService()
        {
            if (allowedApps.Count == 0)
            {
                // Format: (APPId, APIKey)
                allowedApps.Add("asdKVUsa2PdcY", "Q2IsMWNfYmpmKGtEPUQ9Lic=");
            }
        }
        public string[] SplitHeader(string header)
        {
            string[] split = header.Split(':');

            return split;
        }

        public bool IsValidRequest(string header)
        {
            string authScheme = SplitHeader(header)[0].Split(' ')[0];
            string APPid = SplitHeader(header)[0].Split(' ')[1];
            string requestSignatureBase64String = SplitHeader(header)[1];
            string nonce = SplitHeader(header)[2];
            string requestTimeStamp = SplitHeader(header)[3];

            if (!allowedApps.ContainsKey(APPid))
            {
                return false;
            }

            if (authScheme != "Basic")
            {
                return false;
            }

            string sharedKey = allowedApps[APPid];

            //byte[] hash = await ComputeHash(req.Content);
            //if (hash != null)
            //{
            //    requestContentBase64String = Convert.ToBase64String(hash);
            //}

            string data = String.Format("{0}{1}{2}", APPid, requestTimeStamp, nonce);
            byte[] secretKeyBytes = Convert.FromBase64String(sharedKey);
            byte[] signature = Encoding.UTF8.GetBytes(data);
            using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                return requestSignatureBase64String.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal);
            }
        }

        public async Task<byte[]> ComputeHash(HttpContent httpContent)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = null;
                byte[] content = await httpContent.ReadAsByteArrayAsync();
                if (content.Length != 0)
                {
                    hash = md5.ComputeHash(content);
                }
                return hash;
            }
        }
    }
}
