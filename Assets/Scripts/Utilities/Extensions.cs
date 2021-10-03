using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static T[] PickRandomElements<T>(this IEnumerable<T> elements, int count)
    {
        var elementsList = elements.ToList();
        
        if (elementsList.Count == 0)
            return default;
        
        if (elementsList.Count < count)
            count = elementsList.Count;

        T[] outArray = new T[count];
        for (int i = 0; i < count; i++)
        {
            var randomIndex = Random.Range(0, elementsList.Count);
            outArray[i] = elementsList[randomIndex];
            
            elementsList.RemoveAt(randomIndex);
        }

        return outArray;
    }
    
    public static T PickRandomElement<T>(this IEnumerable<T> elements)
    {
        var elementsList = elements.ToList();
        
        if (elementsList.Count == 0)
            return default;
        
        var randomIndex = Random.Range(0, elementsList.Count);
        return elementsList[randomIndex];
    }
}
