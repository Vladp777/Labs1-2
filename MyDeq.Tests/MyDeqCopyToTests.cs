using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDeq.Tests
{
    public class MyDeqCopyToTests
    {
        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void CopyTo_WhenArrayIsEmpty_ShouldThrow<T>(MyDeq<T> values)
        {
            T[] array = null;

            var act = () =>
            {
                values.CopyTo(array, 0);
            };

            Assert.Throws<ArgumentNullException>(act);
        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void CopyTo_WhenIndexOutOfRange_ShouldThrow<T>(MyDeq<T> values)
        {
            T[] array = new T[values.Count + 10];

            var act = () =>
            {
                values.CopyTo(array, -1);
            };

            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void CopyTo_WhenWrongIndex_ShouldThrow<T>(MyDeq<T> values)
        {
            T[] array = new T[values.Count + 10];

            var act = () =>
            {
                values.CopyTo(array, 20);
            };

            Assert.Throws<ArgumentException>(act);
        }


        [Fact]
        public void CopyTo_WhenDeqIsEmpty_ShouldNotChangeArray()
        {
            int[] arr = { 1, 2, 3 };
            int[] arrExpected = { 1, 2, 3 };


            var myDeq = new MyDeq<int>();

            myDeq.CopyTo(arr, 0);

            Assert.Equal(arrExpected, arr);
        }

        [Theory]
        [ClassData(typeof (MyDeqTestData))]
        public void CopyTo_WhenValidParameters_ShouldCopy<T>(MyDeq<T> values)
        {
            var arr = new T[values.Count + 10];

            values.CopyTo(arr, 5);

            for (int i = 0; i < values.Count; i++)
            {
                Assert.Contains(values.PeekItemFromEnd(), arr);
            }
        }
    }
}
