using PayOS;
using PayOS.Models.V2.PaymentRequests;

namespace BookingTravelApi.Services
{
    public class PaymentService
    {
        private readonly PayOSClient _PayOs;

        public PaymentService(PayOSClient payOs)
        {
            this._PayOs = payOs;
        }

        public async Task<CreatePaymentLinkResponse> createPayment(int id, List<PaymentLinkItem> items, DateTime expiredAt)
        {
            var sum = items.Sum(i => i.Price * i.Quantity);

            var paymentRequest = new CreatePaymentLinkRequest()
            {
                OrderCode = id,
                Amount = sum,
                Description = $"booking {id}",
                Items = items,
                ExpiredAt = new DateTimeOffset(expiredAt).ToUnixTimeSeconds()
            };

            var paymentLink = await _PayOs.PaymentRequests.CreateAsync(paymentRequest);
            return paymentLink;
        }
    }
}