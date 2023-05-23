using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace GAHW
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtOutput.Text = "Starting...";

            GA ga = new GA();
            ga.BestOfGenerationFound += GABestSoFar;
            ga.Finished += GAFinished;
            ga.targetWord = txtLookFor.Text;
            ga.maxGenerations = 180;
            ga.maxPopulationPerGeneration = 30000;

            Thread worker = new Thread(ga.StartGeneration);
            worker.Start();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void GABestSoFar(GASearchResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new SearchResultHandler(GABestSoFar), new object[] {result});
            }
            else
            {
                txtOutput.Text = string.Format("g:{0} f:{2} v:{1}\r\n", result.GenerationsRun, result.Best.ToString(), result.Best.Fitness) + txtOutput.Text;
            }
        }

        private void GAFinished(GASearchResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new SearchResultHandler(GAFinished), new object[] { result }); 
            }
            else
            {
                txtOutput.Text = string.Format("FINISHED? [ng:{0} f:{2} v:{1}]\r\n", result.GenerationsRun, result.Best.ToString(), result.Best.Fitness) + txtOutput.Text;
            }
        }
    }
}
