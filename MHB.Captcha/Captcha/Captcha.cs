using System;
using System.Drawing;
using System.IO;

namespace MHB.Captcha
{
    public class Captcha
    {
        public static string GetCaptchaBitmap(string tempPath, out Int32 result)
        {
            try
            {
                result = 0;
                string captchaText = string.Empty;
                Bitmap captchaImg = new Bitmap(1, 1);

                int seed = Math.Abs(DateTime.Now.Year / DateTime.Now.Millisecond + DateTime.Now.Hour - DateTime.Now.Second - DateTime.Now.Hour);

                Random ran = new Random(seed);

                Int32 firstNum = ran.Next(94);
                Int32 secondNum = ran.Next(13);

                if (firstNum == 0)
                {
                    firstNum++;
                }
                if (secondNum == 0)
                {
                    secondNum++;
                }

                switch (ran.Next(3))
                {
                    case 0:
                        result = firstNum + secondNum;
                        captchaText = firstNum.ToString() + " + " + secondNum.ToString() + " = ?";
                        break;

                    case 1:
                        result = firstNum * secondNum;
                        captchaText = firstNum.ToString() + " x " + secondNum.ToString() + " = ?";
                        break;

                    case 2:
                        captchaText = firstNum.ToString() + " - " + secondNum.ToString() + " = ?";
                        result = firstNum - secondNum;
                        break;
                }

                Graphics g = Graphics.FromImage(captchaImg);

                Font f = new Font("Comic Sans MS", 26, FontStyle.Italic);

                Int16 width = (Int16)g.MeasureString(captchaText, f).Width;
                Int16 height = (Int16)g.MeasureString(captchaText, f).Height;

                captchaImg = new Bitmap(width, height);

                g = Graphics.FromImage(captchaImg);

                // put a Bisque background
                g.Clear(Color.Bisque);

                Pen p = new Pen(Color.BurlyWood, 2);
                Pen p1 = new Pen(Color.Snow, 2);
                Pen p2 = new Pen(Color.Silver, 2);
                Pen p3 = new Pen(Color.Khaki, 1);

                Point[] points = new Point[61];
                Point[] points1 = new Point[10];

                Rectangle[] rectangles = new Rectangle[10];

                Random ran1 = new Random(1);

                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new Point(ran1.Next(width), ran1.Next(height));

                    if (i < 10)
                    {
                        points1[i] = new Point(ran1.Next(width), ran1.Next(height));
                        rectangles[i] = new Rectangle(ran1.Next(width), ran1.Next(height), ran1.Next(width), ran1.Next(height));
                    }
                }

                g.DrawPolygon(p1, points);

                g.DrawBeziers(p, points);

                g.DrawLines(p2, points1);

                g.DrawString(captchaText, f, new SolidBrush(Color.OldLace), new PointF(5, 5));
                g.DrawString(captchaText, f, new SolidBrush(Color.Pink), new PointF(4, 4));
                g.DrawString(captchaText, f, new SolidBrush(Color.PaleGoldenrod), new PointF(3, 3));
                g.DrawString(captchaText, f, new SolidBrush(Color.Tomato), new PointF(2, 2));
                g.DrawString(captchaText, f, new SolidBrush(Color.YellowGreen), new PointF(1, 1));
                g.DrawString(captchaText, f, new SolidBrush(Color.Cornsilk), new PointF(0, 0));

                g.DrawRectangles(p3, rectangles);
                g.DrawCurve(p3, points1);

                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(tempPath + "\\temp");
                if (!di.Exists)
                {
                    di.Create();
                }

                try
                {
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        if (fi.CreationTime < DateTime.Now.AddHours(-1.00))
                        {
                            fi.Delete();
                        }
                    }
                }
                catch { }

                string path = AppDomain.CurrentDomain.RelativeSearchPath + "\\..";
                string fileName = "\\temp\\Captcha__" + Guid.NewGuid() + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".gif";

                captchaImg.Save(tempPath + fileName, System.Drawing.Imaging.ImageFormat.Gif);

                g.Flush();
                f.Dispose();
                captchaImg.Dispose();
                p.Dispose();
                p1.Dispose();
                p2.Dispose();
                p3.Dispose();

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("GetCaptchaBitmap() threw an exception: " + ex.Message, ex);
            }
        }
    }
}