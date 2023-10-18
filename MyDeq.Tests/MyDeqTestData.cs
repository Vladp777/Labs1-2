using System.Collections;

namespace MyDeq.Tests;

public class MyDeqTestData: IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {

        var myDeq1 = new MyDeq<int>();
        for (int i = 0; i < 15; i++)
        {
            myDeq1.EnqueueItemAtStart(i+1);
        }

        var myDeq2 = new MyDeq<int>();
        for (int i = 0; i < 8; i++)
        {
            myDeq2.EnqueueItemAtEnd(i + 1);
        }
        _ =myDeq2.DequeueItemFromStart();
        _ =myDeq2.DequeueItemFromStart();
        _ =myDeq2.DequeueItemFromStart();

        for (int i = 0; i < 5; i++)
        {
            myDeq2.EnqueueItemAtEnd(i + 100);
        }
        
        var myDeq3 = new MyDeq<string>();
        for (int i = 0; i < 15; i++)
        {
            myDeq3.EnqueueItemAtStart($"{i+1}");
        }
        
        
        yield return new object[] { myDeq1 };
        yield return new object[] { myDeq2 };
        yield return new object[] { myDeq3 };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}