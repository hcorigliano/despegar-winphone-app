using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Business.Configuration;
using Despegar.Core.Log;


namespace Despegar.Core.IService
{
    public interface IUPAService
    {
        Task<UpaField> GetUPA(IBugTracker bugtracker);
    }
}