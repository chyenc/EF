using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyChy.Frame.Common.Extensions;

namespace MyChy.Frame.Common.Helper
{
    public sealed class ImageCode
    {
        private const string Imgcode = "Common_ImageCode";

        #region 使用Sessions存储

        public static void SessionCode(string sessionName)
        {
            SessionCode(sessionName, 4);
        }

        public static void SessionCode(string sessionName = Imgcode, int num = 4)
        {
            string imgcode = StringHelper.GenerateRandomCode(num);
            CreatImage(imgcode);
            HttpContext.Current.Session[sessionName] = imgcode;
        }
        public static bool SessionCodeCheck(string value)
        {
            return SessionCodeCheck(Imgcode, value);
        }

        public static bool SessionCodeCheck(string sessionName, string value)
        {
            var val = HttpContext.Current.Session[sessionName].ToT<string>();
            return value.ToUpper() == val;
        }





        #endregion


        #region 使用Sessions MVC存储
        /// <summary>
        /// 使用Sessions存储
        /// </summary>
        public static void SessionCodeMvc(HttpContextBase httpContext)
        {
            SessionCodeMvc(httpContext, Imgcode, 4);
        }

        public static void SessionCodeMvc(HttpContextBase httpContext, string sessionName, int num = 4)
        {
            string imgcode = StringHelper.GenerateRandomCode(num);
            //string x = HttpContext.Current.Session[SessionName].ToT<string>();
            CreatImage(imgcode);
            if (httpContext.Session != null) httpContext.Session[sessionName] = imgcode;
        }

        public static bool SessionCodeMvcCheck(HttpContextBase httpContext, string value)
        {
            return SessionCodeMvcCheck(httpContext, Imgcode, value);
        }

        public static bool SessionCodeMvcCheck(HttpContextBase httpContext, string sessionName, string value)
        {
            if (httpContext.Session != null)
            {
                var val = httpContext.Session[sessionName].ToT<string>();
                if (value.ToUpper() == val)
                {
                    return true;
                }
            }
            return false;
        }





        #endregion

        #region 使用Cookie存储

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string CookieCode(int num = 4)
        {
            string res = StringHelper.GenerateRandomCode(num);
            CreatImage(res);
            return res;
        }


        #endregion

        private static void CreatImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            const int wHeight = 25;
            var gheight = checkCode.Length * 20 + 5;

            var img = new System.Drawing.Bitmap(gheight, wHeight);
            var g = Graphics.FromImage(img);

            try
            {
                var rnd = new Random(DateTime.Now.Second);
                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = rnd.Next(img.Width);
                    int x2 = rnd.Next(img.Width);
                    int y1 = rnd.Next(img.Height);
                    int y2 = rnd.Next(img.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }


                var fontFamilyArray = new string[] { "Tahoma", "Arial", "Times New Roman" };

      
                var fontfamily = fontFamilyArray[rnd.Next(0, fontFamilyArray.Length)];
                const FontStyle fontstyle = FontStyle.Regular;


                int seed = rnd.Next(0, 100);

                float x = 2;
                for (int i = 0; i < checkCode.Length; i++)
                {
                    var fontsize = rnd.Next(12, 18);
                    var font = new Font(fontfamily, fontsize, fontstyle);
                    float y = (wHeight - font.Height) - 1;
                    g.DrawString(checkCode[i].ToString(), font, new System.Drawing.SolidBrush(RandomColor(seed + i + 1)), x, y);
                    x += fontsize + 3;
                }
                //g.Flush();

                Bitmap tImg = TwistImage(img, RandomColor(seed));

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, img.Width - 1, img.Height - 1);

                var ms = new System.IO.MemoryStream();
                tImg.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Gif";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                img.Dispose();
            }


        }

        private static void CreatImage2(string checkCode)
        {
            var image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5 + 20)), 22);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                var random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                var font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                var brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                    Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                for (var i = 0; i < 100; i++)
                {
                    image.SetPixel(random.Next(image.Width), random.Next(image.Height), Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                var ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Gif";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        private static Color RandomColor(int seed)
        {
            var colors = new Color[] { Color.Black, Color.Red, Color.Blue, Color.DarkRed, Color.DarkGreen };
            var rnd = new Random(seed);
            Color srcColor = colors[rnd.Next(0, colors.Length)];
            var alpha = (int)srcColor.A;
            var blue = (int)srcColor.B;
            var red = (int)srcColor.R;
            var green = (int)srcColor.G;
            blue = blue + rnd.Next(10, 50);
            if (blue > 255)
                blue = blue - 255;
            return Color.FromArgb(rnd.Next(200, 250), red, green, blue);
        }

        private static Bitmap TwistImage(Bitmap srcBmp, Color linecolor)
        {
            var rnd = new Random(DateTime.Now.Second);
            const int maxDx = 8;
            var rn = rnd.Next(50, 80);
            var hh = rnd.Next(3, srcBmp.Height);
            var destBmp = new Bitmap(srcBmp.Width + maxDx, srcBmp.Height);
            for (int i = 0; i < srcBmp.Width; i++)
            {
                for (int j = 0; j < srcBmp.Height; j++)
                {
                    Color color = srcBmp.GetPixel(i, j);
                    if (color != Color.FromArgb(0))
                    {
                        float k = 0;
                        if (j <= hh)
                            k = (float)(hh - j) / (float)hh;
                        else
                            k = (float)(j - hh) / (float)(srcBmp.Height - hh);

                        float dx = k * (float)maxDx * (float)rn / 100;
                        float x = i + dx;
                        destBmp.SetPixel((int)x, j, color);
                    }
                }
            }
            for (var i = 0; i < destBmp.Width; i++)
            {
                for (var j = 0; j < destBmp.Height; j++)
                {
                    var color = destBmp.GetPixel(i, j);
                    if (color == Color.FromArgb(0))
                    {
                        destBmp.SetPixel(i, j, Color.FromArgb(204, 204, 204));
                    }
                }
            }
            return destBmp;
        }
    }
}
