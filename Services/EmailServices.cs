using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MimeKit;
using Repositories.DTO;
using System.Text;
using System.Threading.Tasks;
using static Services.EmailServices;

namespace Services
{

    public interface IEmailServices
    {
        Task SendEmailAsync(EmailDTO emailDTO);
        bool VerifyOtp(string email, string otp);

    }
    public class EmailServices : IEmailServices
    {
        private readonly EmailSettings emailSettings;
        private readonly IMemoryCache memoryCache;

        public EmailServices(IOptions<EmailSettings> options, IMemoryCache memoryCache)
        {
            this.emailSettings = options.Value;
            this.memoryCache = memoryCache;
        }

        private string GenerateOTP(int length)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public async Task SendEmailAsync(EmailDTO emailDTO)
        {
            try
            {
                var otp = GenerateOTP(6);

                // Lưu OTP vào MemoryCache với thời gian hết hạn là 15 phút
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(15));
                memoryCache.Set(emailDTO.ToEmail, otp, cacheEntryOptions);

                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(emailSettings.Email);
                email.To.Add(MailboxAddress.Parse(emailDTO.ToEmail));
                email.Subject = emailDTO.Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = $"<h1>Your OTP Code is: {otp}</h1>" // Thay đổi nội dung email
                };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(emailSettings.Email, emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                throw;
            }
        }

        public bool VerifyOtp(string email, string otp)
        {
            if (memoryCache.TryGetValue(email, out string cachedOtp))
            {
                return cachedOtp == otp;
            }
            return false;
        }
    }
}
