using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThetaStar
{
    public static List<T> Run<T>
       (
           T start,
           Func<T, bool> satisfies,
           Func<T, List<Tuple<T, float>>> expand,
           Func<T, float> heuristic,
           Func<T, T, bool> inSight,
           Func<T, T, float> cost
       )
    {
        //Nodos sin visitar
        var open = new PriorityQueue<T>();

        

        //Arranca con el primer nodo
        open.Enqueue(start, 0);

        //Nodos ya visitados
        var closed = new HashSet<T>();

        //Diccionario de padres, Key: Hijo, Value: Padre
        var parents = new Dictionary<T, T>();

        //Diccionarios de costos, Key: Nodo, Value: costo tentativo para llegar
        var costs = new Dictionary<T, float>();
        costs[start] = 0;

        while (!open.IsEmpty)//Todavia haya nodos para chequear
        {
            var currentPair = open.DequeuePair();//Obtenemos el nodo con el camino mas corto
            var current = currentPair.Item1;

            if (closed.Contains(current)) continue;//Lo necesitamos por que la priority queue
                                                   //no contempla que pasa cuando hay un valor con dos prioridades
                                                   //distintas. Al extraer el primero, no elimina el segundo.
                                                   //Entonces current puede ya haber sido procesado con otra prioridad.

            var currentCost = costs[current];

            if (satisfies(current))//Si el nodo cumple la condicion
            {
                return ConstructPath(current, parents);//Devolvemos el camino a ese nodo
            }

            //Ponemos al current en el closed asi no lo volvemos a chequear
            closed.Add(current);

            //Para cada hijo del current
            foreach (var childPair in expand(current))
            {
                var child = childPair.Item1;
                var childCost = childPair.Item2;

                //Si el nodo ya lo habimos procesado lo salteamos
                if (closed.Contains(child)) continue;

                if (parents.ContainsKey(current) && inSight(child, parents[current]))
                {
                    //Vamos por el abuelo
                    var grandParent = parents[current];
                    var tentativeCost = costs[grandParent] + cost(grandParent, child);
                    if (costs.ContainsKey(child) && tentativeCost > costs[child]) continue;

                    parents[child] = grandParent;
                    costs[child] = tentativeCost;
                    open.Enqueue(child, tentativeCost + heuristic(child));//Lo agregamos a la cola
                }
                else
                {
                    //Normal
                    var tentativeCost = currentCost + childCost;
                    if (costs.ContainsKey(child) && tentativeCost > costs[child]) continue;

                    parents[child] = current;
                    costs[child] = tentativeCost;
                    open.Enqueue(child, tentativeCost + heuristic(child));//Lo agregamos a la cola
                }
            }

        }

        //Si ningun nodo cumplio la condicion
        return null;
    }

    private static List<T> ConstructPath<T>(T end, Dictionary<T, T> parents)
    {
        //Conocemos el final del camino y de donde venimos por los parents
        //Vamos a armar el camino del final al inicio y despues lo revertimos

        var path = new List<T>();
        path.Add(end);//Emppezamos con el ultimo

        //Mientras el ultimo nodo tenga un padre
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            var lastNode = path[path.Count - 1];//El ultimo nodo

            //Agregamos el padre del ultimo nodo al final
            path.Add(parents[lastNode]);
        }

        path.Reverse();//Lo damos vuenta
        return path;
    }
}
