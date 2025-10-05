using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting
{
    internal static class ImageCompress
    {
        public static Bitmap Compress(Bitmap bitmap, int width, int height)
        {
            Bitmap originalImage = bitmap;
            int newWidth = 40;
            int newHeight = 40;
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(resizedImage);
            g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            return resizedImage;
        }

        public static Bitmap Compress(Bitmap bitmap)
        {
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);//品質0到100分的中間值50分
            myEncoderParameters.Param[0] = myEncoderParameter;
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, jpgEncoder, myEncoderParameters);
            return new Bitmap(memoryStream);
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
