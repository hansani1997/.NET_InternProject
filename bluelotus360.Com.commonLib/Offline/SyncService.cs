using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Offline
{
    public class SyncService : ISyncService
    {
        CancellationTokenSource _tokenSource;
        public SyncService() { 
            //_tokenSource = new CancellationTokenSource();
            //StartSyncing();
        }
        public void StartSyncing()
        {
            //var thread = new Thread(new ParameterizedThreadStart((Object obj) =>
            //{
            //    CancellationToken token = (CancellationToken)obj;
            //    while (!token.IsCancellationRequested)
            //    {
            //        Debug.WriteLine("Syncing working fine...");
            //        Thread.Sleep(1000);
            //    }
            //}));
            //thread.Start(_tokenSource.Token);
        }

        ~SyncService()
        {
            //Cancellation Token
            //_tokenSource.Cancel();
        }
    }
}
