using System;
using System.IO;
using System.Windows.Forms;

namespace MO31_Laptander_Wisp.NeuroNet
{
    abstract class Layer
    {
        protected string name_Layer;

        string pathDirWeights;

        string pathFileWeights;

        protected int numofneurons;
        protected int numofprevneurons;
        protected const double learningrate = 0.05d;
        protected const double momentum = 0.050d;
        protected double[,] lastdeltaweights;
        protected Neuron[] neurons;

        //свойства
        public double[] Data
        {
            set
            {
                for (int i = 0; i < numofneurons; i++)
                {
                    neurons[i].Activator(value);
                }
            }
        }


        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            numofneurons = non;
            numofprevneurons = nopn;
            neurons = new Neuron[non];
            name_Layer = nm_Layer;

            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";

            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            lastdeltaweights = new double[non, nopn + 1];
            double[,] Weights;

            if (File.Exists(pathFileWeights))
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            for (int i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                neurons[i] = new Neuron(tmp_weights, nt);
            }

        }

        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' };
            string tmpStr;
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_element;
                    for (int i = 0; i < numofneurons; i++)
                    {
                        memory_element = tmpStrWeights[i].Split(delim);
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                case MemoryMode.SET:
                    tmpStrWeights = new string[numofneurons];
                    if (!File.Exists(path))
                    {
                        MessageBox.Show("!!!Внимание!!!\nФайл " + name_Layer + "_memory.csv синаптических весов" + "\nНЕ НАЙДЕН!!!\nПосле нажатия кнопки ОК автоматически создатся отсутсвующий файл," +
                            " и все значения весов будут сохранены в нём.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    for (int i = 0; i < numofneurons; i++)
                    {
                        tmpStr = neurons[i].Weights[0].ToString();
                        for (int j = 1; j < numofprevneurons + 1; j++)
                        {
                            tmpStr += delim[0] + neurons[i].Weights[j].ToString();
                        }
                        tmpStrWeights[i] = tmpStr;
                    }
                    File.WriteAllLines(path, tmpStrWeights);

                    break;

                case MemoryMode.INIT:

                    MessageBox.Show("!!! В Н И М А Н И Е !!!\nФайл " + name_Layer + "_memory.csv" +
                        " синаптических весов\nНЕ НАЙДЕН!!!\nПосле нажатия кнопки ОК автоматически проинициализируются веса и создастся отсутствующий файл," +
                        " нейросеть вернётся к исходному состоянию <<новорожденности>>, и потребуется обучить её ЗАНОВО.",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    Random random = new Random();

                    double tmpRatio;
                    double tmpShift;

                    // Генерация случайных весов
                    tmpStrWeights = new string[numofneurons];

                    double[,] weightsInit =
                        new double[numofneurons, numofprevneurons + 1];

                    double sum = 0.0;
                    int count = numofneurons * (numofprevneurons + 1);

                    for (int i = 0; i < numofneurons; i++)
                    {
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weightsInit[i, j] = random.NextDouble();
                            sum += weightsInit[i, j];
                        }
                    }

                    // Вычисление среднего значения
                    double mean = sum / count;

                    // Центрирование весов относительно нуля
                    for (int i = 0; i < numofneurons; i++)
                    {
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weightsInit[i, j] -= mean;
                        }
                    }

                    // Вычисление стандартного отклонения
                    double variance = 0.0;

                    for (int i = 0; i < numofneurons; i++)
                    {
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            variance += weightsInit[i, j] * weightsInit[i, j];
                        }
                    }

                    double stdDev = Math.Sqrt(variance / count);

                    // Целевое стандартное отклонение
                    double targetStdDev = 1.0 / Math.Sqrt(numofprevneurons);

                    tmpRatio = stdDev > 0.0
                        ? targetStdDev / stdDev
                        : 1.0;

                    // Масштабирование весов
                    for (int i = 0; i < numofneurons; i++)
                    {
                        tmpStr = "";

                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weightsInit[i, j] *= tmpRatio;

                            if (j > 0)
                                tmpStr += ";";

                            tmpStr += weightsInit[i, j].ToString(
                                "R",
                                System.Globalization.CultureInfo.InvariantCulture);
                        }

                        tmpStrWeights[i] = tmpStr;
                    }

                    // Сохранение весов в CSV
                    Directory.CreateDirectory(pathDirWeights);
                    File.WriteAllLines(path, tmpStrWeights);

                    return weightsInit;
                }

            return weights;
        }
    }
}
