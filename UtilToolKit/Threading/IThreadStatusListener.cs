using System;

namespace Xattacker.Utility.Threading
{
    public interface IThreadStatusListener
    {
       void OnThreadError(ThreadPrototype thread, Exception ex);

       void OnThreadEnd(ThreadPrototype thread);
    }
}
