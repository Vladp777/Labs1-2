using MyDeq;
using System.Collections;

var a = new MyDeq<int>(10);
var b = new Queue<int>();
var c = new Queue();
List<int> list = new List<int>();
a.EnqueueItemAtEnd(1);
a.EnqueueItemAtEnd(2);
a.EnqueueItemAtEnd(3);
a.EnqueueItemAtEnd(4);
a.EnqueueItemAtEnd(5);
a.EnqueueItemAtStart(6);
a.EnqueueItemAtEnd(7);
a.EnqueueItemAtEnd(8);
a.EnqueueItemAtEnd(9);
a.EnqueueItemAtStart(10);
a.EnqueueItemAtStart(11);
a.EnqueueItemAtStart(12);
a.EnqueueItemAtStart(13);
//a.DequeueItemFromEnd();
//a.DequeueItemFromStart();
//a.DequeueItemFromEnd();
//a.DequeueItemFromStart();

foreach (var item in a)
{
    Console.WriteLine(item);
}

Console.WriteLine();

//for (int i = 0; i < a.Count; i++)
//{
//    Console.WriteLine(a[i]);
//}



var h = a.Any();

Console.ReadLine();
//a.Enqueue(1);
//a.Enqueue(2);
//a.Enqueue(3);
//a.Enqueue(4);
//a.Enqueue(5);
//a.Enqueue(6);
//a.Enqueue(7);
//a.Dequeue();
//a.Enqueue(8);
//a.Peek();
