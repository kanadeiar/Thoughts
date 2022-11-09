using QRCoder;

namespace Thoughts.UI.MVC.Controllers
{
    [ApiController, Route("api/qr")]
    public class QrApiController : ControllerBase
    {
        private readonly ILogger<QrApiController> _logger;
        public QrApiController(ILogger<QrApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string code)
        {
            if (string.IsNullOrEmpty(code)) 
                return NotFound();

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(code,
                QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var result = qrCode.GetGraphic(10);

            return File(result, "application/png", "qrcode.png");
        }
    }
}
