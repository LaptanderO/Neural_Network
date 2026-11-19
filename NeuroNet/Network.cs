namespace MO31_Laptander_Wisp.NeuroNet
{
    class Network
    {
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(70, 15, NeuronType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(35, 70, NeuronType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 35, NeuronType.Output, nameof(output_layer));

        private double[] fact = new double[10];
        private double[] e_error_avr;

        public double[] Fact { get => fact; }
        
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }

        public Network() { }

        public void ForwardPass(double[] netInput)
        {
            hidden_layer1.Data = netInput;
            hidden_layer1.Recognize(null, hidden_layer2);
            hidden_layer2.Recognize(null, output_layer);
            output_layer.Recognize(this, null);
        }

        public void Train(Network net)
        {
            input_layer = new InputLayer(NetworkMode.Train);

            int epoches = 200;
            double tmpSumError;
            double[] errors;
            double[] temp_gsums1;
            double[] temp_gsums2;

            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++)
            {
                e_error_avr[k] = 0;
                input_layer.Shuffling_Array_Rows(input_layer.Trainset);
                for (int i = 0; i < input_layer.Trainset.GetLength(0); i++)
                {
                    double[] tmpTrain = new double[15];
                    for (int j = 0; j < tmpTrain.Length; j++)
                    {
                        tmpTrain[j] = input_layer.Trainset[i, j + 1];
                    }

                    ForwardPass(tmpTrain);

                    tmpSumError = 0;
                    errors = new double[fact.Length];
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == input_layer.Trainset[i, 0])
                            errors[x] = 1.0 - fact[x];
                        else
                            errors[x] = -fact[x];

                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length;

                    temp_gsums2 = output_layer.BackwardPass(errors);
                    temp_gsums1 = hidden_layer2.BackwardPass(temp_gsums2);
                    hidden_layer1.BackwardPass(temp_gsums1);
                }
                e_error_avr[k] /= input_layer.Trainset.GetLength(0);
            }

            input_layer = null;

            hidden_layer1.WeightInitialize(MemoryMode.SET, nameof(hidden_layer1) + "_memory.csv");
            hidden_layer2.WeightInitialize(MemoryMode.SET, nameof(hidden_layer2) + "_memory.csv");
            output_layer.WeightInitialize(MemoryMode.SET, nameof(output_layer) + "_memory.csv");
        }
    }
}
