using MyDeq;
using System.Collections;

var a = new MyDeq<int>(10);

a.AddEvent += AddEventHandler;
a.RemoveEvent += RemoveEventHandler;
a.ClearEvent += ClearEventHandler;

a.EnqueueItemAtStart(20);
a.EnqueueItemAtStart(100);

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

a.DequeueItemFromEnd();
a.DequeueItemFromStart();
a.DequeueItemFromEnd();
a.DequeueItemFromStart();

var result = a.Where(x => x.GetHashCode() > 5);

foreach (var item in a)
{
    Console.WriteLine(item);
}

Console.WriteLine();

//for (int i = 0; i < a.Count; i++)
//{
//    Console.WriteLine(a[i]);
//}

var tail = a.PeekItemFromEnd();
Console.WriteLine("Tail: " + tail);

var head = a.PeekItemFromStart();
Console.WriteLine("Head: " + head);

Console.WriteLine();

var arr = new int[15];

a.CopyTo(arr, 3);

foreach (var item in arr)
{ 
    Console.WriteLine(item); 
}

void ClearEventHandler()
{
    Console.WriteLine("The deq was cleared");
}

void RemoveEventHandler(CustomEventArgs<int> obj)
{
    Console.WriteLine("Element was removed from deq");
}

void AddEventHandler(CustomEventArgs<int> obj)
{
    Console.WriteLine("Element was added to deq");
}

var h = a.Any();
Console.WriteLine(h);

Console.ReadLine();
