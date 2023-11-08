using System.Collections.Generic;

public static class ListExtension
{
    public static void Shuffle<T>(this List<T> list, int shuffleTime)
    {
        while (shuffleTime > 0)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            int otherIndex = UnityEngine.Random.Range(0, list.Count);

            if (index != otherIndex)
            {
                T value = list[index];
                list[index] = list[otherIndex];
                list[otherIndex] = value;
            }

            --shuffleTime;
        }
    }
}
