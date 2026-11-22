using System;
using System.IO;

namespace MO31_Laptander_Wisp.NeuroNet
{
    class InputLayer
    {
        private double[,] trainset;
        private double[,] testset;

        
        public double[,] Trainset { get => trainset; }
        public double[,] Testset { get => testset; }

        public InputLayer(NetworkMode nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] tmpArrStr;
            string[] tmpStr;

            switch (nm)
            {
                case NetworkMode.Train:
                    tmpArrStr = File.ReadAllLines(path + "train.txt");
                    trainset = new double[tmpArrStr.Length, 16];
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            trainset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    break;
                case NetworkMode.Test:
                    tmpArrStr = File.ReadAllLines(path + "test.txt");
                    testset = new double[tmpArrStr.Length, 16];
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                        {
                            testset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    break;
                case NetworkMode.Demo:
                    break;
                default:
                    break;
            }
        }

        public void Shuffling_Array_Rows(double[,] arr)
        {
            int j;
            Random random = new Random();

            for (int i = arr.GetLength(0) - 1; i > 0; i--)
            {
                j = random.Next(i + 1);

                for (int k = 0; k < arr.GetLength(1); k++)
                {
                    double temp = arr[i, k];
                    arr[i, k] = arr[j, k];
                    arr[j, k] = temp;
                }
            }
        }
    }
}
