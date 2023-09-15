using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MyDeq
{
    public class MyDeq<T> : IEnumerable<T>, ICollection
    {
        private T[] _array;
        private int _head;       
        private int _tail;       
        private int _size;

        public MyDeq()
        {
            _array = Array.Empty<T>();
        }

        
        public MyDeq(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            _array = new T[capacity];
        }

        public event Action<CustomEventArgs<T>> AddEvent;
        public event Action<CustomEventArgs<T>> RemoveEvent;
        public event Action ClearEvent;

        protected void OnAddEvent(CustomEventArgs<T> e) => AddEvent?.Invoke(e);
        protected void OnRemoveEvent(CustomEventArgs<T> e) => RemoveEvent?.Invoke(e);
        protected void OnClearEvent() => ClearEvent?.Invoke();

        //public T this[int index]
        //{
        //    get 
        //    { 
        //        if (_head < _tail)
        //            return _array[_head + index];
        //        else
        //        {
        //            if (_head + index < _array.Length)
        //            {
        //                return _array[_head + index];
        //            }
        //            else
        //                return _array[index - (_array.Length - _head)];
        //        }
        //    }
        //    set 
        //    {
        //        if (_head < _tail)
        //            _array[_head + index] = value;
        //        else
        //        {
        //            if (_head + index < _array.Length)
        //            {
        //                _array[_head + index] = value;
        //            }
        //            else
        //                _array[index - (_array.Length - _head)]= value;
        //        }
        //    }
        //}
        public int Count
        {
            get { return _size; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot => this;

        public void EnqueueItemAtStart(T item)
        {
            if (_size == _array.Length)
            {
                Grow(_size + 1);
            }
            if(_size != 0)
                MoveNextLeft(ref _head);

            _array[_head] = item;

            OnAddEvent(new CustomEventArgs<T>(item));

            _size++;
        }

        public T DequeueItemFromStart()
        {
            int head = _head;
            T[] array = _array;

            if (_size == 0)
            {
                ThrowForEmptyQueue();
            }

            T removed = array[head];
            //if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                array[head] = default!;
            }
            MoveNextRight(ref _head);
            _size--;

            OnRemoveEvent(new CustomEventArgs<T>(removed));

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

            OnAddEvent(new CustomEventArgs<T>(item));

            _size++;
            //_version++;
        }
        public T DequeueItemFromEnd()
        {
            int tail = _tail;
            T[] array = _array;

            if (_size == 0)
            {
                ThrowForEmptyQueue();
            }

            T removed = array[tail];
            //if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                array[tail] = default!;
            }
            MoveNextLeft(ref _tail);
            _size--;
            //_version++;

            OnRemoveEvent(new CustomEventArgs<T>(removed));

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
                    Array.Copy(_array, 0, newarray, _array.Length - _head, _tail + 1);
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
                tmp = _array.Length - 1;
            }
            index = tmp;
        }

        private void ThrowForEmptyQueue()
        {
            throw new InvalidOperationException("Deq is empty");
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

        //public IEnumerator<T> GetEnumerator()
        //{
        //    return new Iterator(this);
        //}


        public IEnumerator<T> GetEnumerator()
        {
            if (_head < _tail)
            {
                for (int i = 0; i < _size; i++)
                {
                    yield return _array[i];
                }
            }
            else
            {
                for (int i = _head; i < _array.Length; i++)
                {
                    yield return _array[i];
                }

                for (int i = 0; i < _tail + 1; i++)
                {
                    yield return _array[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            if (_size != 0)
            {
                if (_head < _tail)
                {
                    Array.Clear(_array, _head, _size);
                }
                else
                {
                    Array.Clear(_array, _head, _array.Length - _head);
                    Array.Clear(_array, 0, _tail + 1);
                }
                
                _size = 0;
            }

            _head = 0;
            _tail = 0;

            OnClearEvent();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex.ToString());
            }

            if (array.Length - arrayIndex < _size)
            {
                throw new ArgumentException("Invalid length of the array");
            }

            int numToCopy = _size;
            if (numToCopy == 0) return;
            try
            {
                int firstPart = Math.Min(_array.Length - _head, numToCopy);
                Array.Copy(_array, _head, array, arrayIndex, firstPart);
                numToCopy -= firstPart;
                if (numToCopy > 0)
                {
                    Array.Copy(_array, 0, array, arrayIndex + _array.Length - _head, numToCopy);
                }
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid type of array", nameof(array));
            }
            
        }

        public bool Contains(T item)
        {
            if (_size == 0)
            {
                return false;
            }

            if (_head < _tail)
            {
                return Array.IndexOf(_array, item, _head, _size) >= 0;
            }

            return
                Array.IndexOf(_array, item, _head, _array.Length - _head) >= 0 ||
                Array.IndexOf(_array, item, 0, _tail + 1) >= 0;
        }
        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo(array as T[], index);
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
                    _index = -2;
                    _current = default;
                    return false;
                }

                T[] array = _myDeq._array;
                int capacity = array.Length;

                int arrayIndex = _myDeq._head + _index;
                if (arrayIndex >= capacity)
                {
                    arrayIndex -= capacity;
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