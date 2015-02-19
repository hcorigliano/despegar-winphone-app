using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Despegar.Core.Neo.Business.Configuration;
using Despegar.Core.Neo.Log;


namespace Despegar.Core.Neo.IService
{
    public interface IUPAService
    {
        Task<UpaField> GetUPA(IBugTracker bugtracker);
    }
}