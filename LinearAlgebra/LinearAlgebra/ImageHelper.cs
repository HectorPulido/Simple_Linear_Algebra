using System;
using System.Drawing;

namespace LinearAlgebra
{
    class ImageHelper
    {
        public static Matrix[] LoadImage(string directory, double SizeX, double SizeY)
        {
            Bitmap _img = (Bitmap)Image.FromFile(directory);
            var brush = new SolidBrush(Color.Black);
            float scale = Math.Min((float)SizeX / _img.Width, (float)SizeY / _img.Height);
            var img = new Bitmap((int)SizeX, (int)SizeY);
            var graph = Graphics.FromImage(img);
            int scaleWidth = (int)(_img.Width * scale);
            int scaleHeight = (int)(_img.Height * scale);
            graph.FillRectangle(brush, new RectangleF(0, 0, (float)SizeX, (float)SizeY));
            graph.DrawImage(_img, new Rectangle((int)((SizeX - scaleWidth) / 2.0), (int)((SizeY - scaleHeight) / 2.0), (int)SizeX, (int)SizeY));

            int h = img.Height;
            int w = img.Width;
            double[,] r = new double[h, w];
            double[,] g = new double[h, w];
            double[,] b = new double[h, w];
            double[,] a = new double[h, w];
            double[,] bw = new double[h, w];
            Matrix.MatrixLoop((i, j) => {
                var color = img.GetPixel(i, j);
                r[i, j] = color.R;
                g[i, j] = color.G;
                b[i, j] = color.B;
                a[i, j] = color.A;
                bw[i, j] = color.GetBrightness() * 255;
            }, h, w);
            return new Matrix[] { bw, r, g, b };
        }
        public static Matrix[] LoadImage(string directory)
        {
            Bitmap img = (Bitmap)Image.FromFile(directory);
            int h = img.Height;
            int w = img.Width;
            double[,] r = new double[h, w];
            double[,] g = new double[h, w];
            double[,] b = new double[h, w];
            double[,] a = new double[h, w];
            double[,] bw = new double[h, w];
            Matrix.MatrixLoop((i, j) => {
                var color = img.GetPixel(i, j);
                r[i, j] = color.R;
                g[i, j] = color.G;
                b[i, j] = color.B;
                a[i, j] = color.A;
                bw[i, j] = color.GetBrightness() * 255;
            }, h, w);
            return new Matrix[] { bw, r, g, b };
        }
        public static void SaveImage(Matrix bw, string directory)
        {
            Bitmap img = new Bitmap(bw.x, bw.y);
            Matrix.MatrixLoop((i, j) => {
                Color c = Color.FromArgb((int)bw.matrix[i, j], (int)bw.matrix[i, j], (int)bw.matrix[i, j]);
                img.SetPixel(i, j, c);
            }, bw.x, bw.y);
            img.Save(directory, System.Drawing.Imaging.ImageFormat.Bmp);
        }
        public static void SaveImage(Matrix r, Matrix g, Matrix b, string directory)
        {
            Bitmap img = new Bitmap(r.x, r.y);
            Matrix.MatrixLoop((i, j) => {
                Color c = Color.FromArgb((int)r.matrix[i, j], (int)g.matrix[i, j], (int)b.matrix[i, j]);
                img.SetPixel(i, j, c);
            }, r.x, r.y);
            img.Save(directory, System.Drawing.Imaging.ImageFormat.Bmp);
        }
        public static void SaveImage(Matrix r, Matrix g, Matrix b, Matrix a, string directory)
        {
            Bitmap img = new Bitmap(r.x, r.y);
            Matrix.MatrixLoop((i, j) => {
                Color c = Color.FromArgb((int)a.matrix[i, j], (int)r.matrix[i, j],
                    (int)g.matrix[i, j], (int)b.matrix[i, j]);
                img.SetPixel(i, j, c);
            }, r.x, r.y);
            img.Save(directory, System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}
