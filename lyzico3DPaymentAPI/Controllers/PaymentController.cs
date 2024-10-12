using Iyzico3DPayment.Shared.Models;
using Iyzico3DPaymentAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Iyzico3DPaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequestModel model)
        {
            try
            {
                model.Buyer.Ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                var result = await _paymentService.InitiatePayment(model);
                if (result.Status == "success")
                {
                    return Ok(new { HtmlContent = result.HtmlContent });
                }
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while initiating payment");
                return StatusCode(500, new { ErrorMessage = "An internal error occurred" });
            }
        }

        [HttpGet("callback")]
        [HttpPost("callback")]
        public async Task<IActionResult> Callback([FromForm] IFormCollection form)
        {
            try
            {
                var callbackData = form.ToDictionary(x => x.Key, x => x.Value.ToString());
                var result = await _paymentService.ProcessCallback(callbackData);

                if (result.Status == "success")
                {
                    // Ödeme başarılı, veritabanınızı güncelleyin ve kullanıcıyı bilgilendirin
                    return Ok(new { Message = "Ödeme başarılı", PaymentId = result.PaymentId });
                }
                else
                {
                    // Ödeme başarısız, kullanıcıyı bilgilendirin
                    return BadRequest(new { ErrorMessage = result.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing payment callback");
                return StatusCode(500, new { ErrorMessage = "An internal error occurred" });
            }
        }
    }
}