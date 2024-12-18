using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CubeGuns.Sight;
using UnityEngine;

namespace CubeGuns.Pathfinding
{
    public static class Pathfinding
    {
        readonly static int LOOPSPERFRAME = 5;

        // Usamos mucho Await.
        // La idea original era delegar el metodo de AStar a otro thread. Sin embargo, eso no es posible, ya que dependemos mucho de
        // conseguir transforms, lo cual solo puede ocurrir en el main thread. Asi que ahora mismo await solo sirve como forma de evitar
        // que el juego se trabe mientras calculamos un camino complicado.
        // La alternativa seria usar corutinas, pero esto nos deja usar este metodo por fuera de un GameObject.

        /// <summary>
        /// Function that implements the A* pathfinding algorithm generically.
        /// </summary>
        /// <typeparam name="Node"></typeparam>
        /// <param name="start"> The starting node. </param>
        /// <param name="end"> The goal node. </param>
        /// <param name="getHeuristic"> The function we will use to get the heuristic cost between the two input nodes. </param>
        /// <param name="getNeighbours"> The function we will use to get a node's neighbours. Must return a tuple containing the neighbour-node itself and the distance to it. </param>
        /// <param name="isGoalReached"> The function we will use to check if our goal state has been reached. </param>
        /// <param name="output"> The function that will be called once a path is properly planned out. </param>
        /// <returns></returns>
        public static async Task<List<Node>> AStar<Node>(Node start, Node end, Func<Node, Node, float> getHeuristic, Func<Node, List<Tuple<Node, float>>> getNeighbours, Func<Node, bool> isGoalReached) where Node : class
        {
            // Open is the nodes that haven't been processed yet. Closed is the nodes we've already processed.
            HashSet<Node> open = new(), closed = new();
            open.Add(start);

            // G cost counts how far away we are from the starting node. H Cost is the heuristic cost.
            Dictionary<Node, float> GCost = new(), HCost = new();
            GCost[start] = 0;
            HCost[start] = getHeuristic(start, end);

            // Given a node key, gives you the node previous to it in the path so far.
            Dictionary<Node, Node> previousNodeOf = new();
            previousNodeOf[start] = null;

            int loops = 0;

            while (open.Count > 0)
            {
                // If the amount of loops is bigger than what we allow, we must let a frame pass.
                // This lets us balance frame times AND lets us try to make the most out of our pathfinding.
                loops++;
                if (loops > LOOPSPERFRAME)
                {
                    loops = 0;
                    await Task.Yield();
                }

                var current = open.OrderBy(x => HCost[x]).First();

                if (isGoalReached(current))
                {
                    var path = new List<Node>();

                    while (current != start)
                    {
                        path.Add(current);
                        current = previousNodeOf[current];
                    }

                    path.Add(start);
                    path.Reverse();

                    return path;
                }

                open.Remove(current);
                closed.Add(current);

                var neighbours = getNeighbours(current);
                if (neighbours == null || !neighbours.Any()) continue;

                var currentCost = GCost[current];

                foreach (var n in neighbours)
                {
                    Node neighbour = n.Item1;
                    float cost = n.Item2;

                    if (closed.Contains(neighbour)) continue;

                    var neighbourCost = currentCost + cost;
                    open.Add(neighbour);

                    if (neighbourCost > HCost.DefaultGet(neighbour, () => neighbourCost)) continue;

                    previousNodeOf[neighbour] = current;
                    GCost[neighbour] = neighbourCost;
                    HCost[neighbour] = neighbourCost + getHeuristic(neighbour, end);
                }
            }

            throw new Exception("Path could not be found!");
        }


        /// <summary>
        /// A* Pathfinding
        /// </summary>
        /// <param name="start"> The starting node. </param>
        /// <param name="end"> The end node. </param>
        /// <param name="output"> The result, sent as an action. </param>
        /// <returns></returns>
        public static async Task<List<Node>> AStar<Node>(Node start, Node end) where Node : class, IPathfindingNode
        {
            return await AStar
                (
                    start, end,
                    (n1, n2) => Vector3.Distance(n1.GetPosition(), n2.GetPosition()),
                    (n1) => n1.GetNeighbours().Select(n2 => new Tuple<Node, float>(n2 as Node, Vector3.Distance(n1.GetPosition(), n2.GetPosition()))).ToList(),
                    (n) => n == end
                );

        }

        public static async Task<List<Node>> ThetaStar<Node>(Node start, Node end, LayerMask wallLayer) where Node : class, IPathfindingNode
        {
            var path = await AStar(start, end);
            var listPath = path.ToList();

            if (path.Count() == 0) return path;

            int current = 0;
            while (current + 2 < listPath.Count)
            {
                if (SightHelpers.InLineOfSight(listPath[current].GetPosition(), listPath[current + 2].GetPosition(), wallLayer))
                    listPath.RemoveAt(current + 1);
                else
                    current++;
            }

            return listPath;
        }
    }

    public interface IPathfindingNode
    {
        public Vector3 GetPosition();
        public List<IPathfindingNode> GetNeighbours();
    }
}
