using System.Collections.Generic;
using System;

//código robado de Rearden

public class Pathfinding {
    public static IEnumerable<T> AStar<T>(T start, Func<T, bool> isGoal, Func<T, IEnumerable<DistToElement<T>>> explode, Func<T, float> heuristic)
    {
        var queue = new PriorityQueue<T>();
        var distances = new Dictionary<T, float>();
        var parents = new Dictionary<T, T>();
        var visited = new HashSet<T>();

        //inicial... el elemento inicial esta a distancia cero
        //encolamos un DistToElement con el elemento y la distancia
        distances[start] = 0;
        queue.Enqueue(new DistToElement<T>(start, 0));

        //aca empieza todo
        //si la "Cola" no esta vacia
        while (!queue.IsEmpty)
        {
            //desencolo el mas cercano
            var dequeued = queue.Dequeue();

            //agrego a los visitados el que acabo de desencolar
            visited.Add(dequeued.Element);

            //si el elemento que le pasamos por parametro es la meta
            // es un func<T,bool> que le asignamos desde afuera
            if (isGoal(dequeued.Element))
                return CreatePath(parents, dequeued.Element);// si es el punto final, creo el camino

            // y si no era la meta...

            //me armo una coleccion con todos los vecinos del que desencole
            // es un func<T, Ienum<DistToElement<T>>>
            var toEnqueue = explode(dequeued.Element);

            //recorro todos los vecinos del que desencolé
            foreach (var transition in toEnqueue)
            {
                // obtengo elemento
                var neighbor = transition.Element;
                // obtengo peso
                var neighborToDequeuedDistance = transition.Weight;
                //ahora obtengo la distancia con el mas cercano que desencolé
                var startToDequeuedDistance = distances[dequeued.Element];

                //si el diccionario de <T,float> contiene a este elemento obtengo su distancia, sino un valor muy alto
                var startToNeighborDistance = distances.ContainsKey(neighbor) ? distances[neighbor] : float.MaxValue;

                //sumo la distancia del que desencolé con la distancia del vecino del que desencole
                var newDist = startToDequeuedDistance + neighborToDequeuedDistance;

                //si el vecino no esta en el conjunto de visitados 
                //y su distancia es mayor que la distancia del desencolado y el vecino del desencolado
                if (!visited.Contains(neighbor) && startToNeighborDistance > newDist)
                {
                    DictUpdate(distances, neighbor, newDist);
                    DictUpdate(parents, neighbor, dequeued.Element);
                    queue.Enqueue(new DistToElement<T>(neighbor, newDist + heuristic(neighbor)));
                }
            }
        }

        return null;
    }
    static IEnumerable<T> CreatePath<T>(Dictionary<T, T> parents, T goal)
    {
        var path = new Stack<T>();
        var current = goal;

        path.Push(goal);

        while (parents.ContainsKey(current))
        {
            path.Push(parents[current]);
            current = parents[current];
        }

        return path;
    }
    static void DictUpdate<K, V>(Dictionary<K, V> dict, K key, V value)
    {
        if (dict.ContainsKey(key))
            dict[key] = value;
        else
            dict.Add(key, value);
    }
}
