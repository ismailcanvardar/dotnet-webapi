using System;
using System.Threading.Tasks;

namespace KariyerAppApi.Helpers
{
    public interface IEmailManagement
    {
        Task<int> SendVerificationCode(string to);
        Task SendEmailForRegistiration(string newUserName, string newUserEmail);
        Task SendEmailToEmployerAboutApplication(string advertName, string employerEmail);
    }
}
