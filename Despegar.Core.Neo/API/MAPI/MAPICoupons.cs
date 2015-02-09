using Despegar.Core.Neo.API;
using Despegar.Core.Neo.Business.Coupons;
using Despegar.Core.Neo.Connector;
using Despegar.Core.Neo.Contract;
using Despegar.Core.Neo.Contract.API;
using Despegar.Core.Neo.Contract.Connector;
using Despegar.Core.Neo.Exceptions;
using System;
using System.Threading.Tasks;

namespace Despegar.Core.Neo.Service
{
    public class MAPICoupons : IMAPICoupons
    {
        private ICoreContext context;
        private IMapiConnector connector;

        public MAPICoupons(ICoreContext context, IMapiConnector connector)
        {
            this.context = context;
            this.connector = connector;
        }

        /// <summary>
        /// Validates a given Coupon by the user. It returns different Error codes
        /// </summary>        
        public async Task<CouponResponse> Validity(CouponParameter parameter)
        {
            string serviceUrl = ServiceURL.GetServiceURL(ServiceKey.CouponsValidity, parameter.ReferenceCode, parameter.Beneficiary, parameter.TotalAmount, parameter.CurrencyCode, parameter.Quotation, parameter.Product);
                       
            try
            {
                return await connector.GetAsync<CouponResponse>(serviceUrl, ServiceKey.CouponsValidity);
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