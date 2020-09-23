using System;
using System.Threading.Tasks;
using KariyerAppApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace KariyerAppApi.Controllers
{
    [Route("api/emails")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailManagement _emailManagement;

        public EmailsController(IEmailManagement emailManagement)
        {
            _emailManagement = emailManagement;
        }

        [HttpGet("sendVerification/{email}")]
        public async Task<ActionResult> SendVerification(string email)
        {
            var verificationCode = await _emailManagement.SendVerificationCode(email);
            return Ok(new { verificationCode });
        }
    }
}
