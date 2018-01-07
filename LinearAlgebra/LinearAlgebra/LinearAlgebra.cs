using System;

namespace LinearAlgebra
{
    struct Matrix
    {
        double[,] _matrix;
        public double[,] matrix { get { return _matrix; } set { _matrix = value; } }
        public int x { get { return _matrix.GetLength(0); } }
        public int y { get { return _matrix.GetLength(1); } }
        public Matrix T { get { return Transpose(this); } }

        //constructor
        public Matrix(int sizex, int sizey)
        {
            _matrix = new double[sizex, sizey];
        }
        public Matrix(double[,] matrix)
        {
            _matrix = matrix;
        }
        //values
        public void SetValue(int x, int y, double value)
        {
            _matrix[x, y] = value;
        }
        public double GetValue(int x, int y)
        {
            return matrix[x, y];
        }
        public Matrix Slice(int x1, int y1, int x2, int y2)
        {
            double[,] slice = new double[x1 - x2, y1 - y2];

            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    slice[i - x1, j - y1] = matrix[i, j];
                }
            }
            return new Matrix(slice);
        }
        public Matrix Slice(int x, int y)
        {
            return Slice(0,0,x,y);
        }
        public Matrix GetRow(int x)
        {
            double[,] row = new double[1, y];
            for (int j = 0; j < y; j++)
            {
                row[0, j] = matrix[x, j];
            }
            return new Matrix(row);
        }
        public Matrix GetColumn(int y)
        {
            double[,] column = new double[x, 1];
            for (int i = 0; i < x; i++)
            {
                column[i, 0] = matrix[i, y];
            }
            return new Matrix(column);
        }
        public Matrix AddColumn(Matrix m2)
        {
            if (m2.y != 1)
                return new Matrix(0,0);
            if (m2.x != x)
                return new Matrix(0, 0);

            double[,] newMatrix = new double[x, y + 1];
            double[,] m = matrix;

            for (int i = 0; i < x; i++)
            {
                newMatrix[i, 0] = m2.matrix[i, 0];
            }
            MatrixLoop((i, j) => 
            {
                newMatrix[i, j + 1] = m[i, j];
            }, x, y);
            return new Matrix(newMatrix);
        }
        public Matrix AddRow(Matrix m2)
        {
            if (m2.x != 1)
                return new Matrix(0, 0);
            if (m2.y != y)
                return new Matrix(0, 0);

            double[,] newMatrix = new double[x + 1, y];
            double[,] m = matrix;

            for (int j = 0; j < y; j++)
            {
                newMatrix[0, j] = m2.matrix[0, j];
            }
            MatrixLoop((i, j) =>
            {
                newMatrix[i + 1, j] = m[i, j];
            }, x, y);
            return new Matrix(newMatrix);
        }
        //Overriding
        public override string ToString()
        {
            string c = "";
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    c += matrix[i, j] + " ";
                }
                c += "\n";
            }
            return c;
        }
        //PREMADES
        public static Matrix Zeros(int x, int y)
        {
            double[,] zeros = new double[x, y];
            MatrixLoop((i, j) => {
                zeros[i, j] = 0;
            }, x, y);
            return new Matrix(zeros);
        }
        public static Matrix Ones(int x, int y)
        {
            double[,] ones = new double[x, y];
            MatrixLoop((i, j) => {
                ones[i, j] = 1;
            }, x, y);
            return new Matrix(ones);
        }
        public static Matrix Identy(int x)
        {
            double[,] identy = new double[x, x];
            MatrixLoop((i, j) => {
                if (i == j)
                    identy[i, j] = 1;
                else
                    identy[i, j] = 0;
            }, x, x);
            return new Matrix(identy);
        }
        public static Matrix Random(int x, int y, Random r)
        {
            double[,] random = new double[x, y];
            MatrixLoop((i, j) => {
                random[i, j] = r.NextDouble();
            }, x, y);
            return new Matrix(random);
        }
        //Operations
        public static Matrix Transpose(Matrix m)
        {
            double[,] mT = new double[m.y, m.x];
            MatrixLoop((i, j) => {
                mT[j, i] = m.matrix[i,j];
            }, m.x, m.y);
            return new Matrix(mT);
        }
        //ADDITIONS
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return MatSum(m1, m2);
        }
        public static Matrix operator +(Matrix m2, double m1)
        {
            return MatDoubleSum(m1, m2);
        }
        public static Matrix MatDoubleSum(double m1, Matrix m2)
        {
            double[,] a = m2.matrix;
            double[,] b = new double[m2.x, m2.y];

            MatrixLoop((i, j) => {

                b[i, j] = a[i, j] + m1;

            }, b.GetLength(0), b.GetLength(1));

            return new Matrix(b);
        }
        public static Matrix MatSum(Matrix m1, Matrix m2, bool neg = false)
        {
            if (m1.x != m2.x || m1.y != m2.y)
                return new Matrix(0,0);
            double[,] a = m1.matrix;
            double[,] b = m2.matrix;
            double[,] c = new double[m1.x,m2.y];
            MatrixLoop((i, j) => {
                if(!neg)
                    c[i, j] = a[i, j] + b[i, j];
                else
                    c[i, j] = a[i, j] - b[i, j];
            }, c.GetLength(0), c.GetLength(1));
            return new Matrix(c);
        }
        //SUBSTRACTIONS
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return MatSum(m1, m2, true);
        }
        public static Matrix operator -(Matrix m2, double m1)
        {
            return MatDoubleSum(-m1, m2);
        }
        //MULTIPLICATIONS
        public static Matrix operator *(Matrix m2, double m1)
        {
            return MatDoubleMult(m2, m1);
        }
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return MatMult(m1, m2);
        }
        public static Matrix MatDoubleMult(Matrix m2, double m1)
        {
            double[,] a = m2.matrix;
            double[,] b = new double[m2.x, m2.y];

            MatrixLoop((i, j) => {

                b[i, j] = a[i, j] * m1;

            }, b.GetLength(0), b.GetLength(1));

            return new Matrix(b);
        }
        public static Matrix MatMult(Matrix m1, Matrix m2)
        {
            if (m1.y != m2.x)
                return new Matrix(0, 0);

            int n = m1.x;
            int m = m1.y;
            int p = m2.y;

            double[,] a = m1.matrix;
            double[,] b = m2.matrix;
            double[,] c = new double[n, p];
            MatrixLoop((i,j) => {
                double sum = 0;
                for (int k = 0; k < m; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                c[i, j] = sum;

            }, n, p);
            return new Matrix(c);
        }
        //DIVISION
        public static Matrix operator / (Matrix m2, double m1)
        {
            return MatDoubleMult(m2, 1 / m1);
        }
        //Handlers
        static void MatrixLoop(Action<int, int> e, int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    e(i, j);
                }
            }
        }
    }
}
