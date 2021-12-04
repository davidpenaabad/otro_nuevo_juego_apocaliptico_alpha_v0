using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SuffleCollections{
    public static void Suffle<T>(this T[] array, int numberMixes)
    {
        for (int i = 0; i < numberMixes; i++)
        {
            int ramdonIndex = Random.Range(1, array.Length);

            T temp = array[ramdonIndex];
            array[ramdonIndex] = array[0];
            array[0] = temp;
        }
    }

    public static void Suffle<T>(this List<T> list, int numberMixes)
    {
        for(int i = 0; i < numberMixes; i++)
        {
            int ramdonIndex = Random.Range(1, list.Count);

            T temp = list[ramdonIndex];
            list[ramdonIndex] = list[0];
            list[0] = temp;
        }
    }
}
