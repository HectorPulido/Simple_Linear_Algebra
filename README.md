# Simple linear algebra for C#

This is simple linear algebra for C# easy to understand

##Functions
![alt text](https://github.com/HectorPulido/Simple_Linear_Algebra/blob/master/Img/Functions.png?raw=true "Functions")

```csharp
double[,] a1 = { { 10, 20, 30 }, { 1, 3, 4 } };

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
```


Please consider Support on Patreon
https://www.patreon.com/HectorPulido

