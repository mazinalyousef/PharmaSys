using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;

namespace API.Interfaces
{
    public interface IEmailSender
    {
         bool SendEmail(EMailMessage message);
    }
}