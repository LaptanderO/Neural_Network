namespace MO31_Laptander_Wisp.NeuroNet
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type)
        {
        }

        public override double[] BackwardPass(double[] gr_sums)
        {
            double[] gr_sum = new double[numofprevneurons];
            return gr_sum; //заглушка
        }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double[] hidden_out = new double[numofneurons];
            for (int i = 0; i < numofneurons; i++)
            {
                hidden_out[i] = neurons[i].Output;
            }
            nextLayer.Data = hidden_out; //передача выходного сигнала на вход следующему слою
        }
    }
}
