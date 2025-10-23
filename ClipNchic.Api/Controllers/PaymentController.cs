using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ClipNchic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _token = "YSUOMYTA6HK4KL3MDRWO0V9IU7NQH92FGJBUXVDJC4S3R2VK6FKB1LXRGZCMDU8B";

        public PaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckPayment(string account)
        {
            try
            {
                var url = $"https://my.sepay.vn/userapi/transactions/list?account_number={account}&limit=20";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Lỗi khi kiểm tra thanh toán", details = ex.Message });
            }
        }
    }
}
