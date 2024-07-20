using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Common;

namespace OpenBLive.Runtime.Utilities
{
    public static class QrCodeUtility
    {
        private static Texture2D GenerateQrImageWithColor(string content, int width, int height, Color color,
            out BitMatrix bitMatrix)
        {
            MultiFormatWriter writer = new();
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>
            {
                //设置字符串转换格式，确保字符串信息保持正确
                {EncodeHintType.CHARACTER_SET, "UTF-8"},
                //设置二维码边缘留白宽度（值越大留白宽度大，二维码就减小）
                {EncodeHintType.MARGIN, 1},
                {EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.M}
            };
            //实例化字符串绘制二维码工具
            bitMatrix = writer.encode(content, BarcodeFormat.QR_CODE, width, height, hints);

            //转成texture2d
            int w = bitMatrix.Width;
            int h = bitMatrix.Height;

            Texture2D texture = new Texture2D(w, h);
            for (int x = 0; x < h; x++)
            {
                for (int y = 0; y < w; y++)
                {
                    texture.SetPixel(y, x, bitMatrix[x, y] ? color : Color.white);
                }
            }

            texture.Apply();

            return texture;
        }

        private static void CombineIcon(Texture2D originTex, Texture2D icon, BitMatrix bitMatrix)
        {
            int w = bitMatrix.Width;
            int h = bitMatrix.Height;

            // 添加小图
            int halfWidth = originTex.width / 2;
            int halfHeight = originTex.height / 2;
            int halfWidthOfIcon = icon.width / 2;
            int halfHeightOfIcon = icon.height / 2;
            for (int x = 0; x < h; x++)
            {
                for (int y = 0; y < w; y++)
                {
                    var centerOffsetX = x - halfWidth;
                    var centerOffsetY = y - halfHeight;
                    if (Mathf.Abs(centerOffsetX) <= halfWidthOfIcon && Mathf.Abs(centerOffsetY) <= halfHeightOfIcon)
                    {
                        var iconColor = icon.GetPixel(centerOffsetX + halfWidthOfIcon,
                            centerOffsetY + halfHeightOfIcon);
                        var originTexColor = originTex.GetPixel(x, y);

                        Color finalColor = Color.Lerp(originTexColor, iconColor, iconColor.a);
                        originTex.SetPixel(x, y, finalColor);
                        // originTex.SetPixel(x, y,
                        //     icon.GetPixel(centerOffsetX + halfWidthOfIcon, centerOffsetY + halfHeightOfIcon));
                    }
                }
            }

            originTex.Apply();
        }

        public static Texture2D GenerateQrImage(string content, int width = 512, int height = 512,
            Color color = default)
        {
            Texture2D texture = GenerateQrImageWithColor(content, width, height, color, out var bitMatrix);
            return texture;
        }


        public static Texture2D GenerateQrImage(string content, Texture2D icon,
            int width = 512, int height = 512, Color color = default)
        {
            Texture2D texture = GenerateQrImageWithColor(content, width, height, color, out var bitMatrix);
            if(icon!= null) CombineIcon(texture, icon, bitMatrix);
            return texture;
        }
    }
}