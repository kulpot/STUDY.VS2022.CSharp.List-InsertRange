using System;
using System.Collections;
using System.Collections.Generic;

//ref link:https://www.youtube.com/watch?v=qlo4s5gJ2zE&list=PLRwVmtr-pp07QlmssL4igw1rnrttJXerL&index=22
//ctrl+shift+space --- check target details 
// list -- are dynamic, can grow and shrink
// list -- manage array underneath
// all link function rely on IEnumerator
// IEnumerable -- the container sequence just like LINQ while IEnumerator --- can walk through the sequence of both linq and IEnumrable
// Indexer -- knowledge in operator overloading
// Insert Range --- insert many items at time

class MeList<T> : IEnumerable<T>
{
    //T[] items = new T[5];
    T[] items;
    //int count;
    public MeList(int capacity = 5)
    {
        items = new T[capacity];
    }
    public void Add(T item)
    {
        EnsureCapacity();   // MS Implementation method
        if (Count == items.Length)
            Array.Resize(ref items, items.Length * 2);  // resize the underlying containers --- add slots by x2 of previous slot
        items[Count++] = item;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return items[i];      // requires yield return knowledge
        //return new MeEnumerator(this);
    }

    IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }
    public T this[int index]    //indexer -- looks like property
    //public T this[int index, string blah, char c]
    {
        get
        {
            //if (index >= count || index < 0)              highlight+ctrl+. + extract method 
            //    throw new IndexOutOfRangeException();
            CheckBoundaries(index);
            return items[index];
        }
        set
        {
            //if (index >= count || index < 0)
            //    throw new IndexOutOfRangeException();
            CheckBoundaries(index);
            items[index] = value;
        }
    }

    void CheckBoundaries(int index)
    {
        if (index >= Count || index < 0)
            throw new IndexOutOfRangeException();
    }
    public int Capacity
    {
        get { return items.Length; }
    }
    //public int Count { get { return count; } }
    public int Count { get; private set; }
    public void Clear()     // for data waste not cleaned
    {
        /*      Optimization
        Count = 0;
        if (typeof(T).BaseType.Equals(typeof(ValueType)))   // no worry garbage collection
            return;*/

        // nullifying all items(value/reference) types
        for (int i = 0; i < Count; i++)
            items[i] = default(T);
        Count = 0;
    }
    public void TrimExcess()    // for MeList<int>
    {
        T[] newArray = new T[Count];
        Array.Copy(items, newArray, Count);
        items = newArray;
    }
    public void Insert(int index, T item)
    {
        // resize the underlying containers --- add slots by x2 of previous slot
        //if (Count == items.Length)
        //    Array.Resize(ref items, items.Length * 2);
        EnsureCapacity();   // for dupplication of code

        // Shuffle everyone down the existing array
        //Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
        Array.Copy(items, index, items, index + 1, Count - index);
        items[index] = item;    // output : 32, 83, 25, 99, 42, 31
        Count++;

    }
    public void InsertRange(int index, IEnumerable<T> newItems)
    {
        EnsureCapacity(Count + newItems.Count());
        // Shuffle everyone down the existing array
        //Array.Copy(sourceArray, sourceIndex, destinationArray, destinationIndex, length);
        Array.Copy(items, index, items, index + newItems.Count(), Count - index);
        foreach (T newItem in newItems)
            items[index++] = newItem;
        Count += newItems.Count();
    }
    void EnsureCapacity()
    {
        EnsureCapacity(Count + 1);
    }
    //void EnsureCapacity()
    void EnsureCapacity(int neededCapacity)
    //void EnsureCapacity(int neededCapacity = Count + 1) // error: Runtime value(int neededCapacity = Count + 1) must be default args implementation and constant compile time value(int neededCapacity = 8)
    {
        //if (Count == items.Length)
        //    Array.Resize(ref items, items.Length * 2);
        if (neededCapacity > Capacity)
        {
            int targetSize = items.Length * 2;
            if(targetSize < neededCapacity)
                targetSize = neededCapacity;
            Array.Resize(ref items, targetSize);
        }
    }
}

class MainClass
{
    static void Main()
    {

        // Insert, InsertRange, GetRange
        // Remove, RemoveAll, RemoveAt, RemoveRange
        // Contains, Exist, Find, FindAll, FindIndex, FindIndex, FindLast, FindLastIndex, LastIndexOf, index
        // TrueForAll, ForEach, GetEnumerator
        // BinarySearch, Sort, Reverse
        // ConvertAll
        // CopyTo, ToArray
        // AsReadOnly

        //List<int> myPartyAges = new List<int>(5) { 32, 83, 99, 42, 31 }; // Built-In implementation
        //MeList<int> myPartyAges = new MeList<int>(9) { 32, 83, 99, 42, 31 };
        MeList<int> myPartyAges = new MeList<int>(5) { 32, 83, 99, 42, 31 }; // error: initial capacity (5) not enough for insert .... requires resize (EnsureCapacity Method)
        //myPartyAges.Insert(2, 25);
        //myPartyAges.InsertRange(2, new[] { 55, 65, 75 });   // Built-In implementation
        myPartyAges.InsertRange(2, new[] { 55, 65, 75, 75, 75, 75, 75, 75, 75, 75 });   
        foreach (int age in myPartyAges)
            Console.WriteLine(age);

        // Capacity, Count, TrimExcess, Clear
        //List<int> myPartyAges = new List<int>() { 25, 34, 32 };
        //List<int> myPartyAges = new List<int>(10) { 25, 34, 32 };
        //Console.WriteLine(myPartyAges.Count);
        //Console.WriteLine(myPartyAges.Capacity);
        //myPartyAges.Add(99);
        //myPartyAges.Add(101);
        //Console.WriteLine(myPartyAges.Count);
        //Console.WriteLine(myPartyAges.Capacity);
        //List<int> myPartyAges = new List<int>() { };
        //List<int> myPartyAges = new List<int>(6000) { };
        //MeList<int> myPartyAges = new MeList<int>(6000) { };
        //MeList<int> myPartyAges = new MeList<int>() { };
        //int currentCapacity = myPartyAges.Capacity;
        //Console.WriteLine(currentCapacity);
        //for (int i = 0; i < 500; i++)
        //{
        //    myPartyAges.Add(i);
        //if(currentCapacity != myPartyAges.Capacity)
        //{
        //    Console.WriteLine("Resized to " + myPartyAges.Capacity);
        //    currentCapacity = myPartyAges.Capacity;
        //}
        //}
        //Console.WriteLine(myPartyAges.Capacity);
        //myPartyAges.TrimExcess();   // remove excess byte(int)
        //Console.WriteLine(myPartyAges.Capacity);
        //myPartyAges.Clear();    // remove all items on the array
        //Console.WriteLine(myPartyAges.Capacity);
    }
}