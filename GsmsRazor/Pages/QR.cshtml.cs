using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using QRCoder;
namespace GsmsRazor.Pages
{
    public class QRModel : PageModel
    {
        public IActionResult OnGet()
        {
            ViewData["encodedBytes"] = null;
            return Page();
        }

        public IActionResult OnPost(string qrText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
            QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            string encodedBytes = Convert.ToBase64String(BitmapToBytes(qrCodeImage));
            ViewData["encodedBytes"] = encodedBytes;
            return Page();
        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
