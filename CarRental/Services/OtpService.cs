using CarRental.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace CarRental.Services
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;

        public OtpService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string GenerateOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _cache.Set(email, otp, TimeSpan.FromMinutes(5));

            Console.WriteLine($"[DEBUG] OTP generated for {email}: {otp}");
            return otp;
        }

        public bool VerifyOtp(string email, string otp)
        {
            if (_cache.TryGetValue(email, out string storedOtp))
            {
                return storedOtp == otp;
            }
            return false;
        }
    }
}
