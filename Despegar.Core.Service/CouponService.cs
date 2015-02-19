using Despegar.Core.Neo.Business;
using Despegar.Core.Neo.Business.Coupons;
using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.Exceptions;
using Despegar.Core.Neo.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Service
{
    public class CouponService : ICouponsService
    {
         private CoreContext context;

         public CouponService(CoreContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Validates a given Coupon by the user. It returns different Error codes
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<CouponResponse> Validity(CouponParameter parameter)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.CouponsValidity);
            serviceUrl = String.Format(serviceUrl, parameter.ReferenceCode, parameter.Beneficiary, parameter.TotalAmount, parameter.CurrencyCode, parameter.Quotation, parameter.Product);
            IConnector connector = context.GetServiceConnector(ServiceKey.CouponsValidity);
            
            try
            {
                return await connector.GetAsync<CouponResponse>(serviceUrl);
            }
            catch (APIErrorException e)
            {
                VoucherErrors errorCode;
                bool success = Enum.TryParse<VoucherErrors>(e.ErrorData.code.ToString(), out errorCode);

                if (!success)
                    errorCode = VoucherErrors.COUPON_INVALID;

                return new CouponResponse() { Error = errorCode };
            }
            catch (Exception)
            {
                return new CouponResponse() { Error = VoucherErrors.COUPON_INVALID };
            }

        }
     
    }
}