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

        public IActionResult OnPost(int points, string id)
        {
            id = "6f147985-f73f-4f54-887c-d356eca77203";
            string qrText = $"{{\"createdDate\":\"Mar 3, 2022 16:09:43\",\"id\":,\"isDeleted\":false,\"password\":\"12345678\",\"phoneNumber\":\"0978665441\",\"point\":\"{points}\"}}";
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
