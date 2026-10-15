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
        public double[] Data //
        {
            set
            {
                for(int i = 0; i < numofneurons; i++)
                {
                    neurons[i].Activator(value);
                }
            }
        }


        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {

            //
            //
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

            for(int i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for(int j =  0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                neurons[i] = new Neuron(tmp_weights, nt);
            }

        }




        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            switch (mm)
            {
                case MemoryMode.GET:
                    break;
                case MemoryMode.SET:
                    break;
                case MemoryMode.INIT:
                    break;
                default:
                    break;
            }
            return new double[1, 1];
        }



    }
}

