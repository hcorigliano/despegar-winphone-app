using Despegar.Core.Business.Hotels.HotelsAutocomplete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.IService
{
    public interface IHotelService
    {
        Task<HotelsAutocomplete> GetHotelsAutocomplete(string hotelString);
    }
}
