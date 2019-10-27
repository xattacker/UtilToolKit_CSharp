using System;
using System.Threading;

namespace Xattacker.Utility.Threading
{
    public abstract class CustomTimer : ThreadPrototype
    {
        #region data member

        // a signals the provided event object when it's run is completed
        private WaitHandle wait;

        // a handle that can stop the thread
        private RegisteredWaitHandle registeredWait;

        #endregion


        #region constructor and destructor

        public CustomTimer(ulong milliSecondsInterval, bool isRunOnceOnly, bool isRunImmediately)
        {
            this.wait = new AutoResetEvent(isRunImmediately);
            this.MilliSecondsTimeOutInterval = milliSecondsInterval;
            this.IsRunOnceOnly = isRunOnceOnly;
        }

        public CustomTimer(ulong milliSecondsInterval)
            : this(milliSecondsInterval, false, false)
        {
        }

        ~CustomTimer()
        {
            if (this.wait != null)
            {
                this.wait.Close();
                this.wait = null;
            }

            this.registeredWait = null;
        }

        #endregion


        #region data member related function

        // The time-out in milliseconds. If the millisecondsTimeOutInterval parameter is 0 (zero), 
        // the function tests the object's state and returns immediately
        public ulong MilliSecondsTimeOutInterval { get; private set; }
     
        // true to indicate that the thread will no longer wait on the waitObject parameter 
        // after the delegate has been called;
        // false to indicate that the timer is reset every time the wait operation completes 
        // until the wait is unregistered.
        public bool IsRunOnceOnly { get; private set; }

        public ulong RunTimes { get; private set; }

        #endregion


        #region implement from ProcessType

        public sealed override bool Start()
        {
            bool result = false;

            if (this.isTerminated)
            {
                /// put the Timer into thread pool's schedule
                WaitOrTimerCallback callback = new WaitOrTimerCallback(this.ExecRun);
                this.registeredWait = ThreadPool.RegisterWaitForSingleObject(wait, callback, null, (long)this.MilliSecondsTimeOutInterval, this.IsRunOnceOnly);

                this.isStarted = false;
                this.isTerminated = false;

                result = true;
            }

            return result;
        }

        public sealed override void Close()
        {
            if (this.registeredWait != null)
            {
                this.registeredWait.Unregister(this.wait);
                this.registeredWait = null;
            }

            this.isTerminated = true;
        }

        #endregion


        #region callback function

        private void ExecRun(object obj, bool timeout)
        {
            this.isStarted = true;
            this.isTerminated = false;

            try
            {
                // template method
                // the implementor should not use infinite while loop in the function if executeOnlyOnce = false;
                this.Run();

                this.RunTimes++;
            }
            catch (Exception ex)
            {
                if (this.StatusListener != null)
                {
                    this.StatusListener.OnThreadError(this, ex);
                }
            }

            if (this.IsRunOnceOnly || this.isTerminated)
            {
                this.isStarted = false;
                this.isTerminated = true;

                if (this.StatusListener != null)
                {
                    this.StatusListener.OnThreadEnd(this);
                }
            }
        }

        #endregion
    }
}
