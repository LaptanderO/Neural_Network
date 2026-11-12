namespace MO31_Laptander_Wisp.NeuroNet
{
    class OutputLayer : Layer
    {
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type)
        {
        }

        public override double[] BackwardPass(double[] errors)
        {
            double[] gr_sum = new double[numofprevneurons];

            for (int j = 0; j < numofprevneurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)
                {
                    sum += neurons[k].Weights[j + 1] * errors[k];
                }

                gr_sum[j] = sum;
            }

            for (int i = 0; i < numofneurons; i++)
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw;
                    if(n==0)
                        deltaw = momentum * lastdeltaweights[i, 0] + learningrate * errors[i]; 
                    else
                        deltaw = momentum * lastdeltaweights[i, n] + learningrate * neurons[i].Inputs[n-1] * errors[i];

                    lastdeltaweights[i, n] = deltaw;
                    neurons[i].Weights[n] += deltaw;
                }
            }

            return gr_sum;
        }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double e_sum = 0;
            for (int  i = 0;  i < neurons.Length;  i++)
            {
                e_sum += neurons[i].Output;
            }

            for (int i = 0; i < neurons.Length; i++)
            {
                net.Fact[i] = neurons[i].Output / e_sum;
            }
        }
    }
}
