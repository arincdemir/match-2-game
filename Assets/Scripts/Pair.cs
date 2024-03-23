using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<T1, T2>
{
    public T1 First;
    public T2 Second;

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Pair<T1, T2> other = (Pair<T1, T2>)obj;
        return EqualityComparer<T1>.Default.Equals(First, other.First) &&
               EqualityComparer<T2>.Default.Equals(Second, other.Second);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + EqualityComparer<T1>.Default.GetHashCode(First);
            hash = hash * 23 + EqualityComparer<T2>.Default.GetHashCode(Second);
            return hash;
        }
    }
}