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
    public partial class ParallelForm : Form
    {
        AWorkFlow workingFlow;
        AHeapPriority priority;
        AWorkFlow.Scenario scenario = AWorkFlow.Scenario.Compress;
        public ParallelForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
       
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                String selPath = folderBrowserDialog1.SelectedPath;
                scenario = AWorkFlow.Scenario.DeCompress;
                textBox1.Text = selPath;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void button3_Click(object sender, EventArgs e)
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

        public void setTime(String str)
        {
            textBox2.Text = str;
        }

        public void setTime2(String str)
        {
            textBox3.Text = str;
        }

        public void setTime3(String str)
        {
            textBox4.Text = str;
        }

      
        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text.ToString()) ||
              String.IsNullOrWhiteSpace(textBox1.Text.ToString()))
            {
                MessageBox.Show("Выберите файл");
                return;
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text.ToString()) ||
           String.IsNullOrWhiteSpace(textBox1.Text.ToString()))
            {
                MessageBox.Show("Выберите файл");
                return;
            }
            priority = new CAlgoritmMinHeap();


            workingFlow = new CParallelWorkflow(priority, textBox1.Text.ToString(), scenario, ref progressBar1, this);
            try
            {
                String res = workingFlow.run();
                this.Invoke(new Action(() =>
                {
                    this.Text = res;
                }));
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

    }
}
