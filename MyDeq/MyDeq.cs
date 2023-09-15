using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MyDeq
{
    public class MyDeq<T> : IEnumerable<T>//, ICollection
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

        
        public MyDeq(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity.ToString());
            _array = new T[capacity];
        }
        //public MyDeq(IEnumerable<T> collection)
        //{
        //    if (collection == null)
        //        throw new ArgumentNullException(nameof(collection));

        //    _array = EnumerableHelpers.ToArray(collection, out _size);
        //    if (_size != _array.Length) _tail = _size;
        //}

        public int Count
        {
            get { return _size; }
        }

        //bool ICollection.IsSynchronized
        //{
        //    get { return false; }
        //}

        //object ICollection.SyncRoot => this;

        // Removes all Objects from the queue.

        public void EnqueueItemAtStart(T item)
        {
            if (_size == _array.Length)
            {
                Grow(_size + 1);
            }
            if(_size != 0)
                MoveNextLeft(ref _head);

            _array[_head] = item;

            _size++;
        }

        // Removes the object at the head of the queue and returns it. If the queue
        // is empty, this method throws an
        // InvalidOperationException.
        public T DequeueItemFromStart()
        {
            int head = _head;
            T[] array = _array;

            if (_size == 0)
            {
                ThrowForEmptyQueue();
            }

            T removed = array[head];
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                array[head] = default!;
            }
            MoveNextRight(ref _head);
            _size--;
            //_version++;
            return removed;
        }

        public void EnqueueItemAtEnd(T item)
        {
            if (_size == _array.Length)
            {
                Grow(_size + 1);
            }

            if (_size != 0)
                MoveNextRight(ref _tail);

            _array[_tail] = item;

            _size++;
            //_version++;
        }
        public T DequeueItemFromEnd()
        {
            int tail = _tail - 1;
            T[] array = _array;

            if (_size == 0)
            {
                ThrowForEmptyQueue();
            }

            T removed = array[tail];
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                array[tail] = default!;
            }
            MoveNextLeft(ref _tail);
            _size--;
            //_version++;
            return removed;
        }

        public T PeekItemFromStart()
        {
            if (_size == 0)
            {
                ThrowForEmptyQueue();
            }

            return _array[_head];
        }
        public T PeekItemFromEnd()
        {
            if (_size == 0)
            {
                ThrowForEmptyQueue();
            }

            return _array[_tail];
        }

        private void SetCapacity(int capacity)
        {
            T[] newarray = new T[capacity];
            if (_size > 0)
            {
                if (_head < _tail)
                {
                    Array.Copy(_array, _head, newarray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, newarray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, newarray, _array.Length - _head, _tail);
                }
            }

            _array = newarray;
            _head = 0;
            //???
            _tail = _size - 1;
            //_version++;
        }

        private void MoveNextRight(ref int index)
        {
            int tmp = index + 1;
            if (tmp == _array.Length)
            {
                tmp = 0;
            }
            index = tmp;
        }

        private void MoveNextLeft(ref int index)
        {
            int tmp = index - 1;
            if (tmp == -1)
            {
                tmp = _array.Length;
            }
            index = tmp;
        }

        private void ThrowForEmptyQueue()
        {
            throw new InvalidOperationException();
        }

        private void Grow(int capacity)
        {
            Debug.Assert(_array.Length < capacity);

            const int GrowFactor = 2;
            const int MinimumGrow = 4;

            int newcapacity = GrowFactor * _array.Length;

            // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
            if ((uint)newcapacity > Array.MaxLength) newcapacity = Array.MaxLength;

            // Ensure minimum growth is respected.
            newcapacity = Math.Max(newcapacity, _array.Length + MinimumGrow);

            // If the computed capacity is still less than specified, set to the original argument.
            // Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
            if (newcapacity < capacity) newcapacity = capacity;

            SetCapacity(newcapacity);
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