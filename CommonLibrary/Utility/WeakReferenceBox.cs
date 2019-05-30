using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xattacker.Utility.Except;

namespace Xattacker.Utility
{
    public class WeakReferenceBox <T>
    {
        private WeakReference reference;

        public WeakReferenceBox(T obj)
        {
            this.reference = new WeakReference(obj);
        }

        ~WeakReferenceBox()
        {
            this.reference = null;
        }

        public bool IsAlive
        {
            get
            {
                return this.reference != null && this.reference.IsAlive;
            }
        }

        public T Target
        {
            get
            {
                return (T)this.reference.Target;
            }
        }
    }


    public class WeakReferenceList <T>
    {
        public WeakReferenceList()
        {
            this.Listeners = new List<WeakReferenceBox<T>>();
        }

        ~WeakReferenceList()
        {
            if (this.Listeners != null)
            {
                this.Listeners.Clear();
                this.Listeners = null;
            }
        }

        public List<WeakReferenceBox<T>> Listeners { get; private set; }

        public int Count
        {
            get
            {
                return this.Listeners != null ? this.Listeners.Count : 0;
            }
        }


        #region indexer function

        public virtual T this[int index]
        {
            get
            {
                WeakReferenceBox<T> obj = default(WeakReferenceBox<T>);

                if (index >= 0 && index < this.Listeners.Count)
                {
                    obj = this.Listeners[index];
                }
                else
                {
                    throw new CustomException(ErrorId.OUT_OF_INDEX, this.GetType());
                }

                return obj.Target;
            }
            set
            {
                if (value == null)
                {
                    throw new CustomException(ErrorId.NULL_POINTER, this.GetType());
                }

                if (index >= 0 && index < this.Listeners.Count)
                {
                    this.Listeners[index] = new WeakReferenceBox<T>(value);
                }
                else
                {
                    throw new CustomException(ErrorId.OUT_OF_INDEX, this.GetType());
                }
            }
        }

        #endregion


        public bool HasListener()
        {
            return this.Listeners != null && this.Listeners.Count > 0;
        }

        public void CheckListenerAlive()
        {
            if (this.Listeners != null && this.Listeners.Count > 0)
            {
                this.Listeners.RemoveAll(e => !e.IsAlive);
            }
        }

        public void Add(T listener)
        {
            if (listener != null && this.Listeners != null)
            {
                WeakReferenceBox<T> found = this.Listeners.Find(e => e.IsAlive && e.Target.Equals(listener));
                if (found == null)
                {
                    this.Listeners.Add(new WeakReferenceBox<T>(listener));
                }

                this.CheckListenerAlive();
            }
        }

        public void Remove(T listener)
        {
            if (listener != null && this.Listeners != null)
            {
                this.Listeners.RemoveAll(e => e.IsAlive && e.Target.Equals(listener));

                this.CheckListenerAlive();
            }
        }
    }
}
