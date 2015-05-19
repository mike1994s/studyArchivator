using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace attemptSecondHuffman
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }
        AWorkFlow workingFlow;
        AHeapPriority priority;
        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                if (filePath == "")
                {
                    return;
                }
                textBox1.Text = filePath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text.ToString()) ||
              String.IsNullOrWhiteSpace(textBox1.Text.ToString()))
            {
                MessageBox.Show("Выберите файл");
            }
            priority = new CAlgoritmMinHeap();
            AWorkFlow.Scenario scenario;
            if (!textBox1.Text.ToString().Contains(".lema"))
            {
                scenario = AWorkFlow.Scenario.Compress;
            }
            else
            {
                scenario = AWorkFlow.Scenario.DeCompress;
            }
            workingFlow = new CSequintialWorkFlow(priority, textBox1.Text.ToString(), scenario);
            try
            {
                workingFlow.run();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
