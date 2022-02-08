using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HMACClient
{
    public class HMACDelegatingHandler : DelegatingHandler
    {
        private string APPId = "KVUsa2PdcY";
        private string APIKey = "Q2IsMWNfYmpmKGtEPUQ9Lic=";

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            //Get the Request HTTP Method type
            string requestHttpMethod = request.Method.Method;

            //Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //Create the random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            //Creating the raw signature string by combinging
            //APPId, request TimeStamp, nonce
            string signatureRawData = String.Format("{0}{1}{2}", APPId, requestTimeStamp, nonce);

            //Converting the APIKey into byte array
            byte[] secretKeyByteArray = Convert.FromBase64String(APIKey);

            //Converting the signatureRawData into byte array
            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            //Generate the hmac signature and set it in the Authorization header
            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

                //Setting the values in the Authorization header using custom scheme (hmacauth)
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", string.Format("{0}:{1}:{2}:{3}", APPId, requestSignatureBase64String, nonce, requestTimeStamp));
            }
            response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}