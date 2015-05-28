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
        AWorkFlow.Scenario scenario = AWorkFlow.Scenario.Compress;

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
               String res =  workingFlow.run();
               MessageBox.Show(res);
               this.Text = res;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text.ToString()) ||
             String.IsNullOrWhiteSpace(textBox1.Text.ToString()))
            {
                MessageBox.Show("Выберите файл");
            }
            priority = new CAlgoritmMinHeap();
         

            workingFlow = new CParallelWorkflow(priority, textBox1.Text.ToString(), scenario);
            try
            {
                String res = workingFlow.run();
                this.Text = res;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void button4_Click(object sender, EventArgs e)
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

    }
}
