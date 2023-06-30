using Application.Repository.IRepository;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Repository
{
    public class SmsRespository : ISmsRepository
    {
        public async Task<bool> SendSMSPinWithBasicAuth(string PhoneNumber)
        {
            string _key = "1cfd2f94-926b-492d-95a9-838adef1bf1c";
            string _secret = "TFYlxOEoFUmE4OVwEWBbjQ==";
            string _sinchUrl = "https://verification.api.sinch.com/verification/v1/verifications";

            using (var _client = new HttpClient())
            {
                var requestBody = GetSMSVerificationRequestBody(PhoneNumber);
                var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_key}:{_secret}"));

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, _sinchUrl);
                requestMessage.Headers.TryAddWithoutValidation("authorization", "basic " + base64String);
                requestMessage.Content = requestBody;

                var status = await _client.SendAsync(requestMessage);

                if (status.IsSuccessStatusCode)
                {
                    // OTP sent successfully, now verify the OTP
                    var otpVerificationSuccess = await VerifyOTP("your_otp_value_here");
                    return otpVerificationSuccess;
                }
                else
                {
                    return false;
                }
            }
        }

            
        public static StringContent GetSMSVerificationRequestBody(string PhoneNumber)
        {
            var myData = new
            {
                identity = new
                {
                    type = "number",
                    endpoint = PhoneNumber
                },
                method = "sms",
            };

            return new StringContent(
                System.Text.Json.JsonSerializer.Serialize(myData),
                Encoding.UTF8,
                System.Net.Mime.MediaTypeNames.Application.Json
            );
        }

        public async Task<bool> VerifyOTP(string OTP)
        {
            string _key = "1cfd2f94-926b-492d-95a9-838adef1bf1c";
            string _secret = "TFYlxOEoFUmE4OVwEWBbjQ==";
            string _sinchUrl = $"https://verification.api.sinch.com/verification/v1/verifications/{OTP}";

            using (var _client = new HttpClient())
            {
                var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_key}:{_secret}"));

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, _sinchUrl);
                requestMessage.Headers.TryAddWithoutValidation("authorization", "basic " + base64String);

                var response = await _client.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    // OTP verification successful
                    return true;
                }
                else
                {
                    // OTP verification failed
                    return false;
                }
            }
        }
    }
}


