using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace BookingTravelApi.Services
{
    public class MailService
    {
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587; // Cổng tiêu chuẩn cho TLS/STARTTLS

        // Thông tin đăng nhập và hiển thị
        private const string SenderName = "Dịch vụ gửi Email của bookingtour"; // Tên hiển thị
        private const string SenderEmailId = "nguyenphamphuongnam05@gmail.com"; // Địa chỉ email gửi
        private const string SmtpUsername = "nguyenphamphuongnam05@gmail.com"; // Tên đăng nhập SMTP 
        private const string SmtpPassword = "aknw pevn hyza vapc"; // Mật khẩu ứng dụng 

        public async Task<bool> SendMailAsync(string toEmail, string body, int timeValidity)
        {
            try
            {

                var emailMessage = new MimeMessage();

                // Thiết lập người gửi
                var from = new MailboxAddress(SenderName, SenderEmailId);
                emailMessage.From.Add(from);

                // Thiết lập người nhận
                var to = new MailboxAddress("", toEmail);
                emailMessage.To.Add(to);

                // Tiêu đề
                emailMessage.Subject = "Mã OTP - Booking Tour";

                // Nội dung 
                emailMessage.Body = new TextPart("html")
                {
                    Text = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial; background: #f4f4f4; padding: 20px; }}
        .container {{ max-width: 600px; margin: 0 auto; background: white; padding: 30px; border-radius: 10px; }}
        .otp {{ background: #229784; color: white; padding: 20px; text-align: center; font-size: 36px; font-weight: bold; border-radius: 8px; margin: 20px 0; letter-spacing: 8px; }}
        .footer {{ color: #666; font-size: 12px; text-align: center; margin-top: 30px; }}
    </style>
</head>
<body>
    <div class='container'>
        <h2 style='color: #229784;'>Mã Xác Thực OTP</h2>
        <p>Mã OTP của bạn là:</p>
        <div class='otp'>{body}</div>
        <p>Mã có hiệu lực trong <strong>{timeValidity} phút</strong>.</p>
        <p>Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email.</p>
        <div class='footer'>
            <p>© 2024 Booking Tour. All rights reserved.</p>
        </div>
    </div>
</body>
</html>"
                };

                //Gửi email qua SmtpClient
                using (var mailClient = new SmtpClient())
                {
                    // Kết nối đến máy chủ SMTP
                    await mailClient.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);

                    // Xác thực 
                    await mailClient.AuthenticateAsync(SmtpUsername, SmtpPassword);

                    // Gửi thư
                    await mailClient.SendAsync(emailMessage);

                    // Ngắt kết nối
                    await mailClient.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                return false;
            }
        }
    }
}