using Despegar.Core.Neo.Business.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Contract.API
{
    public interface IMAPINotifications
    {
        Task<PushResponse> RegisterOnDespegarCloud(PushRegistrationRequest putBody);
    }
}
