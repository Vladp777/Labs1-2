using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace MyDeq.Tests
{
    public class MyDeqCollectionTests
    {
        [Fact]
        public void Clear_WhenDeqIsEmpty_ShouldNotThrow()
        {
            var myDeq = new MyDeq<int>();

            myDeq.Clear();

            Assert.Empty(myDeq);
        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void Clear_WhenDeqIsNotEmpty_ShouldBeEmpty<T>(MyDeq<T> values)
        {
            values.Clear();

            Assert.Empty(values);
        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void Contains_WhenValidItem_ShuouldBeTrue<T>(MyDeq<T> values)
        {
            var myDeq = new MyDeq<T>(15);
            myDeq.EnqueueItemAtEnd(values.PeekItemFromEnd());

            var act = myDeq.Contains(values.PeekItemFromEnd());

            Assert.True(act);

        }
        
        [Fact]

        public void Contains_WhenValidItem2_ShuouldBeTrue()
        {
            var myDeq = new MyDeq<int>();
            for (int i = 0; i < 8; i++)
            {
                myDeq.EnqueueItemAtEnd(i + 1);
            }
            _ =myDeq.DequeueItemFromStart();
            _ =myDeq.DequeueItemFromStart();
            _ =myDeq.DequeueItemFromStart();

            for (int i = 0; i < 5; i++)
            {
                myDeq.EnqueueItemAtEnd(i + 100);
            }

            var act = myDeq.Contains(8);

            Assert.True(act);

        }

        [Fact]
        public void Contains_WhenItemIsNull_ShuouldThrow()
        {
            var myDeq = new MyDeq<string>();
            string item = null;


            var act = () =>
            {
                myDeq.Contains(item);
            };

            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Contains_WhenNotContainsItem_ShuouldBeFalse()
        {
            var myDeq = new MyDeq<int>(4);
            myDeq.EnqueueItemAtEnd(22);

            var act = myDeq.Contains(0);

            Assert.False(act);

        }

        [Fact]
        public void Contains_WhenDeqIsEmpty_ShuouldBeFalse()
        {
            var myDeq = new MyDeq<int>();

            var act = myDeq.Contains(0);

            Assert.False(act);

        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void GetEnumerator_ShouldBeEqual<T>(MyDeq<T> values)
        {
            var arr = new T[values.Count];
            values.CopyTo(arr, 0);
            var enumerator = values.GetEnumerator();

            int index = 0;
            while (enumerator.MoveNext())
            {
                Assert.Equal(arr[index], enumerator.Current);
                index++;
            }
            enumerator.Reset();
        }

        [Fact]
        public void Constructor_WhenInvalidCapacity_ShouldThrow()
        {
            var act = () => {

                var myDeq = new MyDeq<int>(-1);
            };

            Assert.Throws<ArgumentOutOfRangeException>(act);
        }
    }
}
