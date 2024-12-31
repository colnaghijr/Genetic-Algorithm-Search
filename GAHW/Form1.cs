using GeneticAlgorithm;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticaAlgorithm {
    public partial class frmMain : Form
    {
        private GeneticaAlgorithmSearch geneticAlgorithmSearch;
        private CancellationTokenSource cancellationTokenSource;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Cancel")
            {
                cancellationTokenSource.Cancel();
                return;
            }

            txtOutput.Text = "Starting...";
            btnStart.Text = "Cancel";

            geneticAlgorithmSearch = new GeneticaAlgorithmSearch();
            geneticAlgorithmSearch.BestOfGenerationFound += GABestSoFar;
            geneticAlgorithmSearch.Finished += GAFinished;
            geneticAlgorithmSearch.targetWord = txtLookFor.Text;
            geneticAlgorithmSearch.maxGenerations = int.Parse(txtMaxGenerations.Text);
            geneticAlgorithmSearch.maxPopulationPerGeneration = int.Parse(txtPopulationPerGeneration.Text);

            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Run(() => geneticAlgorithmSearch.StartSearch(cancellationTokenSource.Token));
            }
            catch (OperationCanceledException)
            {
                txtOutput.Text = "Operation canceled.\r\n" + txtOutput.Text;
            }
            finally
            {
                btnStart.Text = "Start";
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                geneticAlgorithmSearch.BestOfGenerationFound -= GABestSoFar;
                geneticAlgorithmSearch.Finished -= GAFinished;
                cancellationTokenSource.Cancel();
                this.Close();
            }
        }

        private void GABestSoFar(GeneticAlgorithmSearchResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new SearchResultHandler(GABestSoFar), new object[] { result });
            }
            else
            {
                txtOutput.Text = string.Format("GEN:{0} BestChromosome:{1}\r\n", result.GenerationsRun, result.Best.ToString()) + txtOutput.Text;
            }
        }

        private void GAFinished(GeneticAlgorithmSearchResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new SearchResultHandler(GAFinished), new object[] { result });
            }
            else
            {
                txtOutput.Text = string.Format("FINISHED [Generations:{0} BestChromosome:{1}]\r\n", result.GenerationsRun, result.Best.ToString()) + txtOutput.Text;
            }
        }

        private void txtMaxGenerations_KeyPress(object sender, KeyPressEventArgs e) {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back) {
                e.Handled = true; // Prevent non-numeric characters from being entered

            }
        }

        private void txtPopulationPerGeneration_KeyPress(object sender, KeyPressEventArgs e) {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Prevent non-numeric characters from being entered

            }
        }
    }
}
