using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SortsConcorrentes
{
    public class SortsController
    {
        private int[] arrayBubble;
        private int[] arrayQuick;
        private Mutex uiMutex = new Mutex();
        private ManualResetEvent syncStart = new ManualResetEvent(false);

        private Action<int[], string, int, TimeSpan> updateUiCallback;
        private CancellationToken cancellationToken;

        private int bubbleOps = 0;
        private int quickOps = 0;

        public SortsController(int[] data, Action<int[], string, int, TimeSpan> callback, CancellationToken token)
        {
            arrayBubble = (int[])data.Clone();
            arrayQuick = (int[])data.Clone();
            updateUiCallback = callback;
            cancellationToken = token;
        }

        public void Start()
        {
            Task.Run(() => ExecuteBubbleSort());
            Task.Run(() => ExecuteQuickSort());

            Thread.Sleep(500);
            syncStart.Set();
        }

        private void ExecuteBubbleSort()
        {
            syncStart.WaitOne();
            var sw = Stopwatch.StartNew();
            int n = arrayBubble.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    if (arrayBubble[j] > arrayBubble[j + 1])
                    {
                        int temp = arrayBubble[j];
                        arrayBubble[j] = arrayBubble[j + 1];
                        arrayBubble[j + 1] = temp;

                        bubbleOps++;
                        PublishUiUpdate(arrayBubble, "Bubble", bubbleOps, sw.Elapsed);


                        Thread.Sleep(15);
                    }
                }
            }
            sw.Stop();
            PublishUiUpdate(arrayBubble, "Bubble", bubbleOps, sw.Elapsed);
        }

        private void ExecuteQuickSort()
        {
            syncStart.WaitOne();
            var sw = Stopwatch.StartNew();

            QuickSort(0, arrayQuick.Length - 1, sw);

            sw.Stop();
            PublishUiUpdate(arrayQuick, "Quick", quickOps, sw.Elapsed);
        }

        private void QuickSort(int low, int high, Stopwatch sw)
        {
            if (cancellationToken.IsCancellationRequested) return;

            if (low < high)
            {
                int pi = Partition(low, high, sw);
                QuickSort(low, pi - 1, sw);
                QuickSort(pi + 1, high, sw);
            }
        }

        private int Partition(int low, int high, Stopwatch sw)
        {
            int pivot = arrayQuick[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (cancellationToken.IsCancellationRequested) return 0;

                if (arrayQuick[j] < pivot)
                {
                    i++;
                    int temp = arrayQuick[i];
                    arrayQuick[i] = arrayQuick[j];
                    arrayQuick[j] = temp;

                    quickOps++;
                    PublishUiUpdate(arrayQuick, "Quick", quickOps, sw.Elapsed);
                    Thread.Sleep(15);
                }
            }
            int temp1 = arrayQuick[i + 1];
            arrayQuick[i + 1] = arrayQuick[high];
            arrayQuick[high] = temp1;

            quickOps++;
            PublishUiUpdate(arrayQuick, "Quick", quickOps, sw.Elapsed);
            Thread.Sleep(15);

            return i + 1;
        }

        private void PublishUiUpdate(int[] currentState, string algorithmName, int ops, TimeSpan time)
        {
            uiMutex.WaitOne();
            try
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    updateUiCallback?.Invoke((int[])currentState.Clone(), algorithmName, ops, time);
                }
            }
            finally
            {
                uiMutex.ReleaseMutex();
            }
        }
    }
}