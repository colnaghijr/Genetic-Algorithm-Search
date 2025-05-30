using GeneticAlgorithm;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticAlgorithm {
    public partial class MainForm : Form // Renamed frmMain to MainForm
    {
        private GeneticAlgorithmSearch geneticAlgorithmSearch;
        private CancellationTokenSource cancellationTokenSource;

        public MainForm() // Renamed frmMain to MainForm constructor
        {
            InitializeComponent();
            // Ensure event handlers in Designer.cs are updated if they were frmMain_Load etc.
            // This part is manual if not handled by IDE:
            // this.Load -= new System.EventHandler(this.frmMain_Load);
            // this.Load += new System.EventHandler(this.MainForm_Load);
            // this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            // this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
        }

        private void MainForm_Load(object sender, EventArgs e) // Renamed frmMain_Load to MainForm_Load
        {
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Cancel")
            {
                if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Cancel();
                }
                return;
            }

            // Improved Input Parsing
            if (string.IsNullOrWhiteSpace(txtLookFor.Text))
            {
                MessageBox.Show("Target word cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtMaxGenerations.Text, out int maxGenerationsInput)) {
                MessageBox.Show("Invalid input for Max Generations. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtPopulationPerGeneration.Text, out int populationPerGenerationInput)) {
                MessageBox.Show("Invalid input for Population per Generation. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            txtOutput.Text = "Starting...\r\n"; // Add newline for subsequent messages
            btnStart.Text = "Cancel";
            
            SetInputControlsEnabled(false); // Disable input fields during search

            await StartGeneticAlgorithmAsync(txtLookFor.Text, maxGenerationsInput, populationPerGenerationInput);
        }

        private async Task StartGeneticAlgorithmAsync(string targetWord, int maxGen, int popPerGen)
        {
            geneticAlgorithmSearch = new GeneticAlgorithmSearch
            {
                TargetWord = targetWord, // Using new property names
                MaxGenerations = maxGen,
                MaxPopulationPerGeneration = popPerGen,
                ElitismRate = 0.10f, // Assuming default, or add UI fields for these
                MutationRate = 0.25f  // Assuming default, or add UI fields for these
            };
            geneticAlgorithmSearch.BestOfGenerationFound += GABestSoFar;
            geneticAlgorithmSearch.Finished += GAFinished;

            // Recreate CTS for each run to ensure it's fresh
            cancellationTokenSource?.Dispose(); 
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Run(() => geneticAlgorithmSearch.StartSearch(cancellationTokenSource.Token), cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                txtOutput.AppendText("Operation canceled.\r\n"); // Append to avoid overwriting
            }
            finally
            {
                if (InvokeRequired)
                {
                    Invoke((Action)(() => {
                        btnStart.Text = "Start";
                        SetInputControlsEnabled(true);
                    }));
                }
                else
                {
                    btnStart.Text = "Start";
                    SetInputControlsEnabled(true);
                }
                // Dispose CTS here as the operation it was controlling is complete or cancelled.
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }
        
        private void SetInputControlsEnabled(bool enabled)
        {
            txtLookFor.Enabled = enabled;
            txtMaxGenerations.Enabled = enabled;
            txtPopulationPerGeneration.Enabled = enabled;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) // Renamed frmMain_KeyDown to MainForm_KeyDown
        {
            if (e.KeyCode == Keys.Escape && cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
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
                txtOutput.Text = string.Format("FINISHED Generations:{0} BestChromosome:{1}\r\n", result.GenerationsRun, result.Best.ToString()) + txtOutput.Text;
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
