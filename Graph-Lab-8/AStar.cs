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
        static public (Cell[],int,int) Run(Map map, Cell beginCell, Cell endCell, Func<Cell,Cell,int> h)
        {
            HashSet<Cell> closed = new HashSet<Cell>();
            PriorityQueue<Cell, int> open = new PriorityQueue<Cell, int>();
            Dictionary<Cell,int> costSoFar = new Dictionary<Cell,int>();
            Dictionary<Cell,int> estimatedCost = new Dictionary<Cell,int>();
            Dictionary<Cell,Cell> parents = new Dictionary<Cell,Cell>();
            costSoFar[beginCell] = 0;
            estimatedCost[beginCell] = costSoFar[beginCell] + h(beginCell,endCell);
            open.Enqueue(beginCell, 0);
            bool find = true;
            int visitedCells = 0;
            while(open.Count > 0)
            {
                Cell current = open.Dequeue();
                visitedCells++;
                if (current == endCell) { find = true;  break; }

                closed.Add(current);
                foreach (var neighbor in map.Neighbors(current).Where(c => !closed.Contains(c)))
                {                  
                    var newCost = costSoFar[current] + Math.Abs(map[neighbor] - map[current]) + 1;
                    if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor]) 
                    {
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
            if (!find) throw new Exception("NO PATH!");
            Cell currentCell = endCell;
            List<Cell> path = new List<Cell>();
            int weight = 0;
            while(currentCell != beginCell)
            {
                path.Add(currentCell);
                Cell parent = parents[currentCell];
                weight += Math.Abs(map[currentCell] - map[parent]) + 1;
                currentCell = parent;
            }
            path.Add(currentCell);
            path.Reverse();
            return (path.ToArray(), weight, visitedCells);
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
