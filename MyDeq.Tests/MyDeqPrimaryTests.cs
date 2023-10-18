namespace MyDeq.Tests
{
    public class MyDeqPrimaryTests
    {
        [Fact]
        public void EnqueueItemAtStart_ShouldAddItem()
        {
            // Arrange
            var myDeq = new MyDeq<int>();

            // Act
            myDeq.EnqueueItemAtStart(1);
            myDeq.EnqueueItemAtStart(1);


            // Assert
            Assert.Equal(2, myDeq.Count);
        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void EnqueueItemAtStart_WhenSizeMoreDeqCapacity_ShouldResizeDeq<T>(MyDeq<T> values)
        {
            // Arrange
            var myDeq = new MyDeq<T>();

            // Act
            foreach (var value in values)
            {
                myDeq.EnqueueItemAtStart(value);
            }

            // Assert
            Assert.Equal(values.Count, myDeq.Count);
        }

        [Fact]
        public void EnqueueItemAtEnd_ShouldAddItem()
        {
            // Arrange
            var myDeq = new MyDeq<int>();

            // Act
            myDeq.EnqueueItemAtEnd(1);
            myDeq.EnqueueItemAtEnd(1);


            // Assert
            Assert.Equal(2, myDeq.Count);
        }
        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void EnqueueItemAtEnd_WhenSizeMoreDeqCapacity_ShouldResizeDeq<T>(MyDeq<T> values)
        {
            // Arrange
            var myDeq = new MyDeq<T>();

            // Act
            foreach (var value in values)
            {
                myDeq.EnqueueItemAtEnd(value);
            }

            // Assert
            Assert.Equal(values.Count, myDeq.Count);
        }

        [Fact]
        public void DequeueItemFromStart_WhenDeqIsEmpty_ShouldThrow()
        {
            var myDeq = new MyDeq<int>();

            var act = () =>
            {
                myDeq.DequeueItemFromStart();
            };

            Assert.Throws<InvalidOperationException>(act);
        }

        [Theory]
        [ClassData(typeof (MyDeqTestData))]
        public void DequeueItemFromStart_AllItem_ShouldBeEmpty<T>(MyDeq<T> values)
        {
            var myDeq = new MyDeq<T>();
            foreach (var item in values)
            {
                myDeq.EnqueueItemAtEnd(item);
            }


            for (int i = 0; i < values.Count; i++)
            {
                _ = myDeq.DequeueItemFromStart();
            }

            Assert.Empty(myDeq);
        }

        [Fact]
        public void DequeueItemFromEnd_WhenDeqIsEmpty_ShouldThrow()
        {
            var myDeq = new MyDeq<int>();

            var act = () =>
            {
                myDeq.DequeueItemFromEnd();
            };

            Assert.Throws<InvalidOperationException>(act);
        }

        [Theory]
        [ClassData(typeof(MyDeqTestData))]
        public void DequeueItemFromEnd_AllItem_ShouldBeEmpty<T>(MyDeq<T> values)
        {
            var myDeq = new MyDeq<T>();
            foreach (var item in values)
            {
                myDeq.EnqueueItemAtEnd(item);
            }


            for (int i = 0; i < values.Count; i++)
            {
                _ = myDeq.DequeueItemFromEnd();
            }

            Assert.Empty(myDeq);
        }


        [Fact]
        public void PeekItemFromEnd_WhenDeqIsEmpty_ShouldThrow()
        {
            var myDeq = new MyDeq<int>();

            var act = () =>
            {
                myDeq.PeekItemFromEnd();
            };

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void PeekItemFromEnd_ShouldReturnItem()
        {
            var myDeq = new MyDeq<int>();
            myDeq.EnqueueItemAtEnd(69);

            var act = myDeq.PeekItemFromEnd();

            Assert.Equal(69, act);
        }

        [Fact]
        public void PeekItemFromStart_WhenDeqIsEmpty_ShouldThrow()
        {
            var myDeq = new MyDeq<int>();

            var act = () =>
            {
                myDeq.PeekItemFromStart();
            };

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void PeekItemFromStart_ShouldReturnItem()
        {
            var myDeq = new MyDeq<int>();
            myDeq.EnqueueItemAtEnd(69);

            var act = myDeq.PeekItemFromStart();

            Assert.Equal(69, act);
        }
    }
}