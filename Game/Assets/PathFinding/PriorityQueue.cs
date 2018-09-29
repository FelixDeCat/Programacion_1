using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{

    List<DistToElement<T>> queue = new List<DistToElement<T>>();

    //encola (agrega a la lista) normalmente
    public void Enqueue(DistToElement<T> element)
    {
        queue.Add(element);
    }

    //desencola el mas cercano
    public DistToElement<T> Dequeue()
    {

        //crea un nuevo elemento para guardarlo si es el mas cercano
        var min = default(DistToElement<T>);
        //max value para que me guarde el primero
        var minDist = float.MaxValue;

        //recorro toda la queue y busco el mas chico, y lo guardo
        foreach (var e in queue)
        {
            if (e.Weight < minDist)
            {
                min = e;
                minDist = e.Weight;
            }
        }

        //creo una nueva lista
        var newQueue = new List<DistToElement<T>>();

        //como es una lista y no una queue real tenemos que volver a meter todo
        //en otra nueva exeptuando el que queremos sacar
        foreach (var element in queue)
            if (element != min)
                newQueue.Add(element);

        //pisamos la pseudo-queue con la nueva
        queue = newQueue;

        //retornamos el elemento mas cercano
        return min;
    }

    //un getter para saber si la "cola" esta vacia
    public bool IsEmpty { get { return queue.Count == 0; } }

}