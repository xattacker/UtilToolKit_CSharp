using System;
using System.Threading;

namespace Xattacker.Utility.Threading
{
    public abstract class CustomThread : ThreadPrototype
    {
        #region data member

        private Thread thread;

        #endregion


        #region implement from ProcessType

        public sealed override bool Start()
        {
            // the default thread priority is Normal
            return this.Start(ThreadPriority.Normal);
        }

        public bool Start(ThreadPriority priority)
        {
            bool result = false;

            if (this.isTerminated)
            {
                this.thread = new Thread(this.ExecRun);
                this.thread.Priority = priority;

                this.isStarted = false;
                this.isTerminated = false;

                // Start the thread.
                this.thread.Start();

                // Loop until worker thread activates.
                while (!this.thread.IsAlive) ;

                result = true;
            }

            return result;
        }

        public virtual void Cancel()
        {
            this.isStarted = false;
            this.isTerminated = true;

            if (this.thread != null)
            {
                this.thread.Abort();
                this.thread = null;
            }
        }

        public override void Close()
        {
            this.isTerminated = true;

            if (this.thread != null)
            {
                this.thread.Join();
                this.thread = null;
            }
        }

        #endregion


        #region callback function

        private void ExecRun()
        {
            this.isStarted = true;
            this.isTerminated = false;

            try
            {
                // template method
                this.Run();
            }
            catch (Exception ex)
            {
                if (this.StatusListener != null)
                {
                    this.StatusListener.OnThreadError(this, ex);
                }
            }

            this.isStarted = false;
            this.isTerminated = true;
            this.thread = null;

            if (this.StatusListener != null)
            {
                this.StatusListener.OnThreadEnd(this);
            }
        }

        #endregion


        #region reserved

        /*
         * 對應用程式而言，Thread.Suspend 和 Thread.Resume 方法通常沒有什麼用處，並且不應與同步處理機制混淆。
         * 因為 Thread.Suspend 和 Thread.Resume 並不需要受控制的執行緒合作，所以具有高度干擾性，
         * 並且會引發如死結 (Deadlock) 之類的嚴重應用程式問題 (例如，如果您暫止的執行緒持有另一個執行緒所需的資源)。

        public void Suspend()
        {
            if (isRunning)
            {
                thread.Suspend();
            }
        }

        public void Resume()
        {
            if (isRunning)
            {
                thread.Resume();
            }
        }
        */
        #endregion
    }
}
