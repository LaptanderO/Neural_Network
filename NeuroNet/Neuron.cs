using static System.Math;


namespace MO31_Laptander_Wisp.NeuroNet
{
    class Neuron
    {
        //поля
        private NeuronType type; //тип нейрона
        private double[] weights; // его веса
        private double[] inputs;//его входы
        private double output;//его выход
        private double derivative;//производная

        //параметры обобщённой функции активации tanh
        // f(x) = alpha * tanh(beta * x)
        private double alpha = 1.0d; //масштаб выхода (амплитуда)
        private double beta = 1.0d; //крутизна (steepness)

        //свойства
        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }

        public double Output { get => output; }

        public double Derivative { get => derivative; }

        //свойства для параметров (если понадобится менять извне)
        public double Alpha { get => alpha; set => alpha = value; }
        public double Beta { get => beta; set => beta = value; }

        //конструктор
        public Neuron(double[] memoryWeights, NeuronType typeNeuron)
        {
            type = typeNeuron;
            weights = memoryWeights;
        }

        //метод активации нейрона (нелинейные преобразования входного сигнала)
        public void Activator(double[] i)
        {
            inputs = i; // передача вектора входного сигнала в массив входных данных нейрона
            double sum = weights[0]; //аффинное преобразование через смещение (нулевой вес - порог нейрона)

            for (int j = 0; j < inputs.Length; j++) //цикл вычисления индуцированного поля нейрона
            {
                sum += inputs[j] * weights[j + 1]; // линейные преобразования входных сигналов
            }

            switch (type)
            {
                case NeuronType.Hidden: //для нейронов скрытого слоя
                    output = Tanh(sum);
                    derivative = Tanh_Derivator(sum);
                    break;

                case NeuronType.Output: //для нейронов выходного слоя
                    output = Exp(sum);
                    break;
            }
        }

        //обобщённая функция активации: alpha * tanh(beta * sum)
        private double Tanh(double sum)
        {
            double z = beta * sum;
            return alpha * (Exp(z) - Exp(-z)) / (Exp(z) + Exp(-z));
        }

        //производная: d/dx [alpha * tanh(beta * x)] = alpha * beta * (1 - tanh^2(beta * x))
        private double Tanh_Derivator(double sum)
        {
            double z = beta * sum;
            double t = (Exp(z) - Exp(-z)) / (Exp(z) + Exp(-z)); //tanh(beta * sum), без alpha
            return alpha * beta * (1.0 - t * t);
        }
    }
}