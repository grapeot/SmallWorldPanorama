using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace grapeot.SmallWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var img = new Image<Bgr, byte>(@"src.jpg");
            var w = img.Width;
            var h = img.Height;
            var r0 = 512;   // half of the image size
            var p = 0.5;    // control the "height"
            var phi0 = -Math.PI / 2 + 0.12;    // initial phrase
            var newImg = new Image<Bgr, byte>(r0 * 2, r0 * 2, new Bgr(Color.White));
            for (int row = 0; row < r0 * 2; row++)
            {
                for (int col = 0; col < r0 * 2; col++)
                {
                    var phi = (Math.Atan2(row - r0, col - r0) + phi0); // * 0.99 to get rid of the small gaps of the original panorama
                    if (phi > Math.PI) phi -= 2 * Math.PI;
                    if (phi < -Math.PI) phi += 2 * Math.PI;
                    var r = Math.Sqrt((row - r0) * (row - r0) + (col - r0) * (col - r0));
                    r = Math.Pow(r0, 1 - p) * Math.Pow(r, p);
                    var y = (int)(h - r / r0 * h);
                    var x = (int)((phi + Math.PI) / Math.PI / 2 * w);
                    if (y >= 0 && y < h && x >= 0 && x < w)
                        newImg[row, col] = img[y, x];
                }
            }
            //ImageViewer.Show(newImg);
            newImg.Save("result.jpg");
            newImg.Dispose();
            img.Dispose();
        }
    }
}
