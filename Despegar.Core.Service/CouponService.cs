using Despegar.Core.Business;
using Despegar.Core.Business.Coupons;
using Despegar.Core.Connector;
using Despegar.Core.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Service
{
    public class CouponService : ICouponsService
    {
         private CoreContext context;

         public CouponService(CoreContext context)
        {
            this.context = context;
        }

        public async Task<CouponResponse> Validity(CouponParameter parameter)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.CouponsValidity);
            serviceUrl = String.Format(serviceUrl, parameter.ReferenceCode, parameter.Beneficiary, parameter.TotalAmount, parameter.CurrencyCode, parameter.Quotation, parameter.Product);
            IConnector connector = context.GetServiceConnector(ServiceKey.CouponsValidity);
            
            try
            {
                return await connector.GetAsync<CouponResponse>(serviceUrl);
            }
            catch (Exception)
            {
                return null;
            }

        }
     
    }
}