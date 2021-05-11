using System.Threading;

namespace Xattacker.Utility.Threading
{
    public abstract class ThreadPrototype
    {
        #region data member

        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        protected volatile bool isStarted = false;
        protected volatile bool isTerminated = true;

        #endregion


        #region destructor

        ~ThreadPrototype()
        {
            this.StatusListener = null;
        }

        #endregion


        #region data member related function

        public bool IsStarted
        {
            get
            {
                return this.isStarted;
            }
        }

        public bool IsTerminated
        {
            get
            {
                return this.isTerminated;
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.isStarted && !this.isTerminated;
            }
        }

        public IThreadStatusListener StatusListener { get; protected set; }

        #endregion


        #region protected function

        protected void Sleep(int milliSeconds)
        {
            Thread.Sleep(milliSeconds);
        }

        #endregion


        #region abstract function

        public abstract bool Start();

        public abstract void Close();

        protected abstract void Run();

        #endregion
    }
}
