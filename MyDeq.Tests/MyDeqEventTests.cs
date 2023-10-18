using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDeq.Tests
{
    public class MyDeqEventTests
    {
        [Fact]
        public void AddEvent_WhenAddItem_ShouldBeEqual()
        {
            var myDeq = new MyDeq<int>();
            var countAddedItems = 0;
            myDeq.AddEvent += (e) => countAddedItems++;

            myDeq.EnqueueItemAtEnd(1);
            myDeq.EnqueueItemAtStart(2);

            Assert.Equal(2, countAddedItems);
        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void RemoveEvent_WhenRemoveItem_ShouldBeEqual<T>(MyDeq<T> values)
        {
            var countRemovedItems = 0;
            values.RemoveEvent += (e) => countRemovedItems++;

            values.DequeueItemFromEnd();
            values.DequeueItemFromStart();

            Assert.Equal(2, countRemovedItems);
        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void ClearEvent_WhenClearDeq_ShouldBeEqual<T>(MyDeq<T> values)
        {
            var countClearedTimes = 0;
            values.ClearEvent += () => countClearedTimes++;

            values.Clear();

            Assert.Equal(1, countClearedTimes);
        }
    }
}
