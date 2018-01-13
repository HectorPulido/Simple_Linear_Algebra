using System;
using System.Collections.Generic;

namespace LinearAlgebra.ExampleProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            double[,] a1 = { { 10, 20, 30 },
                             {  1,  3,  4 } };

            Matrix a = new Matrix(a1);
            Console.WriteLine("Matrix a");
            Console.WriteLine(a.ToString());

            Matrix b = Matrix.Identy(2);
            Console.WriteLine("Matrix identy b");
            Console.WriteLine(b.ToString());

            Matrix c = a.T;
            Console.WriteLine("Matrix a transposed c");
            Console.WriteLine(c.ToString());

            Matrix d = a + 1;
            Console.WriteLine("Matrix a + 1, d");
            Console.WriteLine(d.ToString());

            Matrix e = a - 1;
            Console.WriteLine("Matrix a - 1, e");
            Console.WriteLine(e.ToString());

            Matrix f = a * 10;
            Console.WriteLine("Matrix a * 10, f");
            Console.WriteLine(f.ToString());

            Matrix g = Matrix.Random(2, 3, new Random(123));
            Console.WriteLine("Matrix g, random matrix");
            Console.WriteLine(g.ToString());

            Matrix h = a + g;
            Console.WriteLine("Matrix h, a + g");
            Console.WriteLine(h.ToString());

            Matrix i = a * g.T;
            Console.WriteLine("Matrix h, a * g' ");
            Console.WriteLine(i.ToString());

            Matrix j = a.GetColumn(0);
            Console.WriteLine("Matrix j, first column of a");
            Console.WriteLine(j.ToString());

            Matrix k = a.GetRow(0);
            Console.WriteLine("Matrix k, first row of a");
            Console.WriteLine(k.ToString());

            Matrix l = a.AddColumn(Matrix.Ones(2, 1));
            Console.WriteLine("Matrix l, a with a added column of ones");
            Console.WriteLine(l.ToString());

            Matrix m = a.AddRow(Matrix.Zeros(1, 3));
            Console.WriteLine("Matrix m, a with a added row of zeros");
            Console.WriteLine(m.ToString());

            if (Helper.SaveMatrix(a, "a1.dat"))
                Console.WriteLine("a, saved");

            Matrix _a1;
            if (Helper.LoadMatrix(out _a1, "a1.dat"))
                Console.WriteLine("a1, loaded \n " + _a1.ToString());

            Console.WriteLine("Images can be loaded, and the size can be changed");
            Matrix[] img = Helper.LoadImage("Image.bmp", 15, 15);
            Console.WriteLine(img[0]); //BW
            Console.WriteLine(img[1]); //R
            Console.WriteLine(img[2]); //G
            Console.WriteLine(img[3]); //B

            Console.WriteLine("Images can be Saved");
            Helper.SaveImage(img[0], "BWImg.bmp"); // Saved on BW
            Helper.SaveImage(img[1], img[2], img[3], "ColorImg.bmp"); //Saved on Color
            
            if (Helper.SaveCsv(Helper.MatrixToCsv(h), "a.csv"))
                Console.WriteLine("Matrix saved on csv file"); 
                        
            string[][] data;
            if (Helper.ReadCsv(out data, "CsvFile.csv"))
            {
                Console.WriteLine("Data readed from csv file");
                Helper.MapCsv(ref data, new Dictionary<string, string> { { "Ford" , "0" },
                                                                         { "Chevy", "1" },
                                                                         { "Jeep" , "2" }});
                Console.WriteLine(Helper.CsvToMatrix(data));
            }

            Console.ReadKey();
        }
    }
}
