using System;
using System.Threading;
using System.Windows;

namespace SortsConcorrentes
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource? _cts;

        public MainWindow()
        {
            InitializeComponent();
            ResetUI();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnReset.IsEnabled = true;

            int[] data = GenerateRandomArray(50);

            _cts = new CancellationTokenSource();

            UpdateVisuals(data, "Bubble");
            UpdateVisuals(data, "Quick");

            SortsController controller = new SortsController(data, UpdateUi, _cts.Token);
            controller.Start();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _cts?.Cancel();

            ResetUI();

            btnStart.IsEnabled = true;
            btnReset.IsEnabled = false;
        }

        private void UpdateUi(int[] state, string algorithmName, int ops, TimeSpan time)
        {
            int[] displayState = (int[])state.Clone();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (algorithmName == "Bubble")
                {
                    icBubble.ItemsSource = displayState;
                    txtBubbleNumbers.Text = string.Join(", ", displayState);
                    txtBubbleOps.Text = $"Operações: {ops}";
                    txtBubbleTime.Text = $"Tempo: {time:mm\\:ss\\.fff}";
                }
                else if (algorithmName == "Quick")
                {
                    icQuick.ItemsSource = displayState;
                    txtQuickNumbers.Text = string.Join(", ", displayState);
                    txtQuickOps.Text = $"Operações: {ops}";
                    txtQuickTime.Text = $"Tempo: {time:mm\\:ss\\.fff}";
                }
            }));
        }

        private void UpdateVisuals(int[] state, string algorithmName)
        {
            int[] displayState = (int[])state.Clone();

            if (algorithmName == "Bubble")
            {
                icBubble.ItemsSource = displayState;
                txtBubbleNumbers.Text = string.Join(", ", displayState);
            }
            else
            {
                icQuick.ItemsSource = displayState;
                txtQuickNumbers.Text = string.Join(", ", displayState);
            }
        }

        private void ResetUI()
        {
            txtBubbleOps.Text = "Operações: 0";
            txtBubbleTime.Text = "Tempo: 00:00:00.000";
            txtBubbleNumbers.Text = "Aguardando inicialização...";
            icBubble.ItemsSource = null;

            txtQuickOps.Text = "Operações: 0";
            txtQuickTime.Text = "Tempo: 00:00:00.000";
            txtQuickNumbers.Text = "Aguardando inicialização...";
            icQuick.ItemsSource = null;
        }

        private int[] GenerateRandomArray(int size)
        {
            Random rand = new Random();
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = rand.Next(10, 300);
            }
            return arr;
        }
    }
}