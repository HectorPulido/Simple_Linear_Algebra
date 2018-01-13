﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace LinearAlgebra
{
    class Helper
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
        public static void SaveImage(Matrix r, Matrix g, Matrix b,Matrix a, string directory)
        {
            Bitmap img = new Bitmap(r.x, r.y);
            Matrix.MatrixLoop((i, j) => {
                Color c = Color.FromArgb((int)a.matrix[i,j], (int)r.matrix[i, j], 
                    (int)g.matrix[i, j], (int)b.matrix[i, j]);
                img.SetPixel(i, j, c);
            }, r.x, r.y);
            img.Save(directory, System.Drawing.Imaging.ImageFormat.Bmp);
        }
        public static bool SaveMatrix(Matrix m, string directory)
        {
            FileStream fs = new FileStream(directory, FileMode.Create);
            
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, m.matrix);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serializate: " + e.Message);
                return false;
            }
            finally
            {
                fs.Close();
            }
            return true;
        }
        public static bool LoadMatrix(out Matrix m, string directory)
        {
            FileStream fs = new FileStream(directory, FileMode.Open);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                m = (double[,])bf.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserializate: " + e.Message);
                m = null;
                return false;
            }
            finally
            {
                fs.Close();
            }
            return true;
        }
        public static bool SaveMatrix(Matrix[] m, string directory)
        {
            FileStream fs = new FileStream(directory, FileMode.Create);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                double[][,] _m = new double[m.Length][,];
                for (int i = 0; i < m.Length; i++)
                {
                    _m[i] = m[i].matrix;
                }

                bf.Serialize(fs, _m);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serializate: " + e.Message);
                return false;
            }
            finally
            {
                fs.Close();
            }
            return true;
        }
        public static bool LoadMatrix(out Matrix[] m, string directory)
        {
            FileStream fs = new FileStream(directory, FileMode.Open);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                double[][,] _m = (double[][,])bf.Deserialize(fs);
                m = new Matrix[_m.Length];

                for (int i = 0; i < _m.Length; i++)
                {
                    m[i] = _m[i];
                }                
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserializate: " + e.Message);
                m = null;
                return false;
            }
            finally
            {
                fs.Close();
            }
            return true;
        }
        public static bool ReadCsv(out string[][]data, string directory, char separator = ';')
        {
            string file = "";
            try
            {
                file = File.ReadAllText(directory).Replace("\r", "").Replace(".", ",");
            }
            catch
            {
                data = null;
                return false;
            }
            
            var columns = file.Split(Environment.NewLine.ToCharArray());
            data = new string[columns.Length][];

            for (int i = 0; i < columns.Length; i++)
            {
                data[i] = columns[i].Split(separator);
            }
            return true;
        }
        public static bool SaveCsv(string[][] data, string directory, char separator = ';')
        {
            string[] s = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    s[i] += data[i][j] + separator;
                }
            }
            try
            {
                File.WriteAllLines(directory, s);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static void MapCsv(ref string[][]data, Dictionary<string, string> mapping)
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    foreach (var p in mapping.Keys)
                    {
                        data[i][j] = data[i][j].Replace(p, mapping[p]);
                    }
                }
            }
        }
        public static string[][] MatrixToCsv(Matrix m)
        {
            double[,] _m = m;
            string[][] output = new string[m.x][];
            for (int i = 0; i < m.x; i++)
            {
                output[i] = new string[m.y];
                for (int j = 0; j < m.y; j++)
                {
                    output[i][j] = _m[i, j].ToString();
                }                
            }
            return output;
        }
        public static Matrix CsvToMatrix(string[][] data)
        {
            double[,] m = new double[data.Length, data[0].Length];

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (!double.TryParse(data[i][j], out m[i, j]))
                        m[i, j] = 0;
                }
            }
            return m;
        }
    }
}
