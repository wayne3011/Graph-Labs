using Graph_Lab_8.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Graph_Lab_8
{
    internal class AStar
    {
        static public (Cell[],int) Run(Map map, Cell beginCell, Cell endCell, Func<Cell,Cell,int> h)
        {
            HashSet<Cell> closed = new HashSet<Cell>();
            PriorityQueue<Cell, int> open = new PriorityQueue<Cell, int>();
            Dictionary<Cell,int> costSoFar = new Dictionary<Cell,int>();
            Dictionary<Cell,int> estimatedCost = new Dictionary<Cell,int>();
            Dictionary<Cell,Cell> parents = new Dictionary<Cell,Cell>();
            costSoFar[beginCell] = 0;
            estimatedCost[beginCell] = costSoFar[beginCell] + h(beginCell,endCell);
            open.Enqueue(beginCell, 0);
            while(open.Count > 0)
            {
                Cell current = open.Dequeue();
                if (current == endCell) break;
                closed.Add(current);
                if(current.X == 1 && current.Y == 9)
                {
                    Console.WriteLine("EHFFF");
                }
                foreach (var neighbor in map.Neighbors(current))
                {
                    if (closed.Contains(neighbor)) continue;
                    var newCost = costSoFar[current] + map[neighbor.X, neighbor.Y];
                    if (!open.Contains(neighbor) || newCost < costSoFar[neighbor]) {
                        if (neighbor.X < 2 && neighbor.Y >7)
                        {
                            Console.WriteLine("EHFFF");
                        }
                        parents[neighbor] = current;
                        costSoFar[neighbor] = newCost;
                        estimatedCost[neighbor] = costSoFar[neighbor] + h(neighbor,endCell);
                        if (open.Contains(neighbor))
                        {
                            open.ChangePriority(neighbor, estimatedCost[neighbor]);
                        }
                        else 
                            open.Enqueue(neighbor, estimatedCost[neighbor]);
                    }
                }
            }
            //if (open.Count == 0) throw new Exception("NO PATH!");
            Cell currentCell = endCell;
            List<Cell> path = new List<Cell>();
            while(currentCell != beginCell)
            {
                path.Add(currentCell);
                currentCell = parents[currentCell];
            }
            path.Add(currentCell);
            path.Reverse();
            return (path.ToArray(), costSoFar[endCell]);
        }

    }
    static class Extesions
    {
        static public void ChangePriority(this PriorityQueue<Cell,int> queue, Cell value, int priority)
        {
            List<(Cell,int)> temp = new List<(Cell,int)> ();
            while(queue.Count > 0)
            {
                queue.TryDequeue(out Cell element, out int currentPriority);
                if (element == value) break;
                temp.Add((element, currentPriority));
            }
            queue.Enqueue(value, priority);
            foreach(var element in temp)
            {
                queue.Enqueue(element.Item1, element.Item2);
            }

        }
        static public bool Contains(this PriorityQueue<Cell, int> queue, Cell value)
        {
            return queue.UnorderedItems.Any(x => x.Element == value);
        }
    }
}
