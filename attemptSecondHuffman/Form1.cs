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
     

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            ParallelForm pForm = new ParallelForm();
            pForm.Closed += (s, args) => this.Close(); 
            pForm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            SequintialForm seqForm = new SequintialForm();
            seqForm.Closed += (s, args) => this.Close(); 
            seqForm.ShowDialog();
        }
       
    }
}
