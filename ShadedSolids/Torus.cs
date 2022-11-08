using System;
using MathNet.Numerics.LinearAlgebra;

namespace ShadedSolids
{
    class Torus
    {
        float light_X = 1;
        float light_Y = 1;
        float light_Z = -5;
        float light_R = 3;

        float theta_spacing = 0.07F;
        float phi_spacing = 0.03F;

        float K2 = 5;

        int screen_width = 85;
        int screen_height = 85;

        float R1 = 3.0F;
        float R2 = 5.0F;

        //char[] shades = ".'`^\",:;i][{)xuzo8B@".ToCharArray();
        char[] shades = ".,-~:;=!*#$@".ToCharArray();

        public char[,] RenderFrame(double A, double B)
        {
            //float K1 = (screen_width * K2 * 1) / (8 * (radius));

            char[,] printout = new char[screen_width, screen_height];
            Populate<char>(printout, ' ');

            float[,] zBuffer = new float[screen_width, screen_height];
            Populate<float>(zBuffer, 0);

            for (float theta = 0; theta < 2 * Math.PI; theta += theta_spacing)
            {
                for (float phi = 0; phi < 2 * Math.PI; phi += phi_spacing)
                {
                    double[] axyz = new double[] { R2 * Math.Cos(phi) + R1 * Math.Cos(theta) * Math.Cos(phi),
                                                   R1 * Math.Sin(theta),
                                                   -1 * R2 * Math.Sin(phi) + -1 * R1 * Math.Cos(theta) * Math.Sin(phi)};
                    Vector<double> xyz = Vector<double>.Build.DenseOfArray(axyz);

                    double[,] arMatrix = new double[,]{ {   Math.Cos(phi),   0, Math.Sin(phi)  },
                                                        {         0,         1,       0        },
                                                        {-1 * Math.Sin(phi), 0, Math.Cos(phi)  } };
                    Matrix<double> rMatrix = Matrix<double>.Build.DenseOfArray(arMatrix);

                    double[,] axMatrix = new double[,]{ { 1,            0,              0       },
                                                        { 0,       Math.Cos(A),    Math.Sin(A) },
                                                        { 0,    -1 * Math.Sin(A),  Math.Cos(A) } };
                    Matrix<double> xMatrix = Matrix<double>.Build.DenseOfArray(axMatrix);

                    double[,] ayMatrix = new double[,]{ {   Math.Cos(B),    Math.Sin(B), 0 },
                                                        { -1 * Math.Sin(B), Math.Cos(B), 0 },
                                                        {        0,              0,      1 } };
                    Matrix<double> yMatrix = Matrix<double>.Build.DenseOfArray(ayMatrix);


                }
            }

            return printout;
        }

        private float FindIndex()
        {
            float magnitude = (float)Math.Sqrt(Math.Pow(light_X, 2) + Math.Pow(light_Y, 2) + Math.Pow(light_Z, 2));

            return shades.Length / magnitude;
        }

        private void Populate<T>(T[,] arr, T value)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    arr[i, x] = value;
                }
            }
        }

        private void PrintOut(char[,] printout)
        {
            for (int j = 0; j < screen_height; j++)
            {
                for (int i = 0; i < screen_width; i++)
                {
                    Console.Write(printout[i, j] + " ");
                }

                Console.Write('\n');
            }
        }

        public void runAnimationCircle()
        {
            while (true)
            {
                for (float theta = 0; theta < 2 * Math.PI; theta += 0.15F)
                {
                    light_X = (float)(light_R * Math.Cos(theta));
                    light_Y = (float)(light_R * Math.Sin(theta));

                    PrintOut(RenderFrame());
                    Thread.Sleep(60);
                    Console.Clear();
                }
            }
        }
        public void runAnimationEightCurve()
        {
            while (true)
            {
                for (float theta = 0; theta < 2 * Math.PI; theta += 0.15F)
                {
                    light_X = (float)(light_R * Math.Sin(theta));
                    light_Y = (float)(light_R * Math.Sin(theta) * Math.Cos(theta));

                    PrintOut(RenderFrame());
                    Thread.Sleep(60);
                    Console.Clear();
                }
            }
        }
    }
}