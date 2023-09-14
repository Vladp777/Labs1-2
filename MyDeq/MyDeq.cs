using System;
using System.Collections;
using System.Diagnostics;

namespace MyDeq
{
    public class MyDeq<T> : ICollection
    {
        private T[] _array;
        private int _head;       
        private int _tail;       
        private int _size;
        //private int _version;

        public MyDeq()
        {
            _array = Array.Empty<T>();
        }

        // Creates a queue with room for capacity objects. The default grow factor
        // is used.
        public MyDeq(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity.ToString());
            _array = new T[capacity];
        }
        public int Count => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Iterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Iterator : IEnumerator<T>
        {
            private MyDeq<T> _myDeq;
            private T? _current;
            private int _index;

            public Iterator(MyDeq<T> myDeq)
            {
                _myDeq = myDeq;
                _index = -1;
                _current = default;
            }

            //public T Current => _current;

            //object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                _index = -2;
                _current = default;
            }

            public bool MoveNext()
            {
                if (_index == -2)
                    return false;

                _index++;

                if (_index == _myDeq._size)
                {
                    // We've run past the last element
                    _index = -2;
                    _current = default;
                    return false;
                }

                // Cache some fields in locals to decrease code size
                T[] array = _myDeq._array;
                int capacity = array.Length;

                // _index represents the 0-based index into the queue, however the queue
                // doesn't have to start from 0 and it may not even be stored contiguously in memory.

                int arrayIndex = _myDeq._head + _index; // this is the actual index into the queue's backing array
                if (arrayIndex >= capacity)
                {
                    // NOTE: Originally we were using the modulo operator here, however
                    // on Intel processors it has a very high instruction latency which
                    // was slowing down the loop quite a bit.
                    // Replacing it with simple comparison/subtraction operations sped up
                    // the average foreach loop by 2x.

                    arrayIndex -= capacity; // wrap around if needed
                }

                _current = array[arrayIndex];
                return true;
            }
            public T Current
            {
                get
                {
                    if (_current == null)
                        throw new InvalidOperationException("Enumeration has not been started ot it is already finished");
                    return _current;
                }
            }


            object? IEnumerator.Current
            {
                get { return Current; }
            }

            void IEnumerator.Reset()
            {
                
                _index = -1;
                _current = default;
            }
        }
    }

    
}