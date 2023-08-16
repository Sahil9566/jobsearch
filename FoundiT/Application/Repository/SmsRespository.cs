using Application.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Twilio.Types;
using static System.Net.Mime.MediaTypeNames;
using Domain.Models;

namespace Application.Repository
{
    public class SmsRespository : ISmsRepository
    {
        public async Task<bool> SendSMSPinWithBasicAuth(string PhoneNumber)
        {
            string _key = "0e515370-570c-4016-b2d4-6bd9db84b035";
            string _secret = "NRBRTjM4qUeFPP9aZSevNg==";
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
                    //var otpVerificationSuccess = await VerifyOTP("your_otp_value_here");
                    return true;
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

        public async Task<bool> VerifyOTP(VerifyOTPVM param)
        {
            string _key = "0e515370-570c-4016-b2d4-6bd9db84b035";
            string _secret = "NRBRTjM4qUeFPP9aZSevNg==";
            //string _sinchUrl = $"https://verification.api.sinch.com/verification/v1/verifications/{OTP}";

            //using (var _client = new HttpClient())
            //{
            //    var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_key}:{_secret}"));

            //    var requestMessage = new HttpRequestMessage(HttpMethod.Get, _sinchUrl);
            //    requestMessage.Headers.TryAddWithoutValidation("authorization", "basic " + base64String);

            //    var response = await _client.SendAsync(requestMessage);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        // OTP verification successful
            //        return true;
            //    }
            //    else
            //    {
            //        // OTP verification failed
            //        return false;
            //    }
            //}

            var _verificationUrl = "https://verification.api.sinch.com/verification/v1/verifications/number/";

            HttpClient client = new HttpClient();
            string otp = param.OTP;

            string requestBodyJson = $"{{ \"method\": \"sms\", \"sms\": {{ \"code\": \"{otp}\" }} }}";
            HttpContent requestBody = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, _verificationUrl + param.PhoneNumber);
            var base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_key}:{_secret}"));
            request.Headers.Add("Authorization", "Basic " + base64String);
            request.Content = requestBody;

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
                
                return (true);
            else
                return false;
        }
    }
}



