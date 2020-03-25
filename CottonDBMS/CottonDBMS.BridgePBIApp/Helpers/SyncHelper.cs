using CottonDBMS.Bridges.Shared.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CottonDBMS.BridgePBIApp.Helpers
{
    public static class PBISyncHelper
    {
        private static CancellationTokenSource tokenSource = null;
        private static CancellationToken ct;
        public static bool IsRunning { get; private set; }

        static PBISyncHelper()
        {
            IsRunning = false;
        }

        public static void Cancel()
        {
            tokenSource.Cancel();
        }

        public static void RunSync()
        {
            if (IsRunning) return;
            IsRunning = true;
            Task.Run(async () =>
            {
                if (tokenSource == null)
                {
                    tokenSource = new CancellationTokenSource();
                    ct = tokenSource.Token;
                }
                
                var task = new PBILoggerSyncTask(ct);
                await task.DoSync();
                IsRunning = false;
            });
        }

        public static void WaitForCompletion()
        {
            int waitCount = 0;
            while (IsRunning && waitCount < 60)
            {
                System.Threading.Thread.Sleep(1000);
                waitCount++;
            }
        }
    }
}
