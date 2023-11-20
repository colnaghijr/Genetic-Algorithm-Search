using GeneticAlgorithm;
using System;
using System.Threading;
using System.Windows.Forms;

namespace GeneticaAlgorithm {
    public partial class frmMain : Form {
        private GeneticaAlgorithmSearch geneticAlgorithmSearch;
        private Thread worker;

        public frmMain() {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e) {
        }

        private void btnStart_Click(object sender, EventArgs e) {
            txtOutput.Text = "Starting...";

            geneticAlgorithmSearch = new GeneticaAlgorithmSearch();
            geneticAlgorithmSearch.BestOfGenerationFound += GABestSoFar;
            geneticAlgorithmSearch.Finished += GAFinished;
            geneticAlgorithmSearch.targetWord = txtLookFor.Text;
            geneticAlgorithmSearch.maxGenerations = 180;
            geneticAlgorithmSearch.maxPopulationPerGeneration = 30000;

            worker = new Thread(geneticAlgorithmSearch.StartSearch);
            worker.Start();
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                geneticAlgorithmSearch.BestOfGenerationFound -= GABestSoFar;
                geneticAlgorithmSearch.Finished -= GAFinished;
                worker.Abort();
                this.Close();
            }
        }

        private void GABestSoFar(GeneticAlgorithmSearchResult result) {
            if (InvokeRequired) {
                Invoke(new SearchResultHandler(GABestSoFar), new object[] { result });
            }
            else {
                txtOutput.Text = string.Format("GEN:{0} BestChromosome:{1}\r\n", result.GenerationsRun, result.Best.ToString()) + txtOutput.Text;
            }
        }

        private void GAFinished(GeneticAlgorithmSearchResult result) {
            if (InvokeRequired) {
                Invoke(new SearchResultHandler(GAFinished), new object[] { result });
            }
            else {
                txtOutput.Text = string.Format("FINISHED [Generations:{0} BestChromosome:{1}]\r\n", result.GenerationsRun, result.Best.ToString()) + txtOutput.Text;
            }
        }
    }
}
