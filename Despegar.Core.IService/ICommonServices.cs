using Despegar.Core.Business.Common.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.IService
{
    public interface ICommonServices
    {
        Task<List<State>> GetStates(string country);
    }
}
