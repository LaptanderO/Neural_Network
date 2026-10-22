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
