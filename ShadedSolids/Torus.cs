using System;
using MathNet.Numerics.LinearAlgebra;

namespace ShadedSolids
{
    class Torus
    {
        double[] aLight = new double[] { 1, 1, -5 };

        float light_R = 3;

        float theta_spacing = 0.07F;
        float phi_spacing = 0.03F;

        float K2 = 5;

        int screen_width = 200;
        int screen_height = 200;

        float R1 = 0.5F;
        float R2 = 2.0F;

        //char[] shades = ".'`^\",:;i][{)xuzo8B@".ToCharArray();
        char[] shades = ".,-~:;=!*#$@".ToCharArray();

        public char[,] RenderFrame(double A, double B)
        {
            float K1 = (screen_width * K2 * 3) / (8 * (R1 + R2));

            Vector<double> light = Vector<double>.Build.DenseOfArray(aLight);

            char[,] printout = new char[screen_width, screen_height];
            Populate<char>(printout, ' ');

            double[,] zBuffer = new double[screen_width, screen_height];
            Populate<double>(zBuffer, 0);

            for (float theta = 0; theta < 2 * Math.PI; theta += theta_spacing)
            {
                for (float phi = 0; phi < 2 * Math.PI; phi += phi_spacing)
                {   
                    //xyz of torus in parametric form
                    double[,] axyz = new double[,] { { (R2 + R1 * Math.Cos(theta)) * Math.Cos(phi),            //x
                                                        R1 * Math.Sin(theta),                                  //y
                                                        -1 * (R2 + R1 * Math.Cos(theta)) * Math.Sin(phi) } };  //z
                    Matrix<double> xyz = Matrix<double>.Build.DenseOfArray(axyz);

                    //matrix for rotation about the x axis by constant A
                    double[,] axMatrix = new double[,] { { 1,            0,              0       },
                                                         { 0,       Math.Cos(A),    Math.Sin(A) },
                                                         { 0,    -1 * Math.Sin(A),  Math.Cos(A) } };
                    Matrix<double> xMatrix = Matrix<double>.Build.DenseOfArray(axMatrix);

                    //matrix for rotation about the y axis by constant B
                    double[,] ayMatrix = new double[,] { {   Math.Cos(B),    Math.Sin(B), 0 },
                                                         { -1 * Math.Sin(B), Math.Cos(B), 0 },
                                                         {        0,              0,      1 } };
                    Matrix<double> yMatrix = Matrix<double>.Build.DenseOfArray(ayMatrix);

                    //vector normal to the torus surface
                    double[] aNormal = new double[] { Math.Sin(phi) * Math.Cos(theta),
                                                      Math.Sin(phi) * Math.Sin(theta),
                                                      Math.Cos(phi)                    };
                    Vector<double> normal = Vector<double>.Build.DenseOfArray(aNormal);

                    //lum = normal * light
                    double luminence = normal.DotProduct(light);

                    //apply the matrix transforms
                    Matrix<double> rxyz = xyz.Multiply(xMatrix).Multiply(yMatrix);
                    double rz = rxyz[0,2];
                    //One Over Z
                    double OOZ = 1 / rz;
                    
                    int xp = (int) Math.Round((screen_width / 2) + (K1 * OOZ * rxyz[0,0]));
                    int yp = (int) Math.Round((screen_height / 2) - (K1 * OOZ * rxyz[0, 1])); 

                    if (luminence > 0)
                    {
                        if (OOZ > zBuffer[xp, yp])
                        {
                            zBuffer[xp, yp] = OOZ;

                            int luminence_index = (int)(luminence * FindIndex(light));

                            printout[xp, yp] = shades[luminence_index];
                        }
                    } 
                }
            }

            return printout;
        }

        private float FindIndex(Vector<double> light)
        {
            float magnitude = (float) Math.Sqrt(Math.Pow(light[0], 2) + Math.Pow(light[1], 2) + Math.Pow(light[2], 2));

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

        public void runAnimation()
        {
            double A = 0;
            double B = 0;

            while (true)
            {
                PrintOut(RenderFrame(A, B));
                Thread.Sleep(500);
                Console.Clear();
                A += 0.1;
                B += 0.1;
            }
        }
    }
}