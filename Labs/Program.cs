using MyDeq;

var a = new MyDeq<int>(10);

a.AddEvent += (args) => Console.WriteLine("Element was added to deq");
a.RemoveEvent += (args) => Console.WriteLine("Element was removed from deq");
a.ClearEvent += () => Console.WriteLine("The deq was cleared");

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

var deq = new MyDeq<string>();
deq.EnqueueItemAtStart(null);

Console.WriteLine();

foreach (var item in a)
{
    Console.WriteLine(item);
}

Console.WriteLine();

var tail = a.PeekItemFromEnd();
Console.WriteLine("Tail: " + tail);

var head = a.PeekItemFromStart();
Console.WriteLine("Head: " + head);

Console.WriteLine();

var arr = new int[15];

a.CopyTo(arr, 3);

Console.WriteLine("Queue:");
foreach (var item in a)
{
    Console.Write(item + ", ");
}
Console.WriteLine();

Console.WriteLine("Copy Queue:");
foreach (var item in arr)
{ 
    Console.Write(item + ", "); 
}

Console.WriteLine();

var h = a.Any();
Console.WriteLine(h);

Console.WriteLine();

var hh = new MyDeq<int>();
for (int i = 0; i < 15; i++)
{
    hh.EnqueueItemAtEnd(i);
}

for(int i = 0;  i < 15; i++)
{
    _ = hh.DequeueItemFromEnd();
}



var enumerator= hh.GetEnumerator();
while (enumerator.MoveNext()) { 
    Console.WriteLine(enumerator.Current);}




Console.ReadLine();
