using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.VisualStyles;//прстранство имен для работы с файловой системой

namespace MO31_Laptander_Wisp
{
    public partial class FormMain : Form
    {
        private double[] inputPixels;//Массив входных данных
        public FormMain()
        {
            InitializeComponent();
            inputPixels = new double[15];
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if(btn.BackColor == Color.White)
            {
                btn.BackColor = Color.Black;
                inputPixels[btn.TabIndex] = 1d;
            }
            else
            {
                btn.BackColor = Color.White;
                inputPixels[btn.TabIndex] = 0d;
            }
        }
        //сохранить в файл ОБУЧАЮЩИЙ пример
        private void ButtonSaveTrainSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n"; //переход на новую строку текста

            File.AppendAllText(path, tmpStr);
        }
        //сохранить в файл ТЕСТОВЫЙ пример
        private void ButtonSaveTestSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n"; //переход на новую строку текста

            File.AppendAllText(path, tmpStr);
        }
    }
}
