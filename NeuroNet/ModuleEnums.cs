

namespace MO31_Laptander_Wisp.NeuroNet
{
    enum MemoryMode //режим работы памяти
    {
        GET, //считывание памяти
        SET,//сохранение памяти
        INIT//инициализация памяти
    }

    enum NeuronType //тип нейрона
    { 
        Hidden, //скрытый
        Output //выходной
    }

    enum NetworkMode //режим работы сети
    {
        Train, //обучение 
        Test, // проверка
        Demo // распознавание
    }
}
