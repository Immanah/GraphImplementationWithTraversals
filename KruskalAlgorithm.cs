using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
class Program
{
 static void Main(string[] args)
 {
 // Load graph from the "graph.txt" file
 Graph graph = LoadGraphFromFile("graph.txt");
 // Run Prim's algorithm 10 times and capture runtimes

 List<long> runtimes = new List<long>();
 for (int i = 0; i < 10; i++)
 {
 Stopwatch stopwatch = new Stopwatch();
 stopwatch.Start();
 List<Edge> mst = graph.Prim();
 stopwatch.Stop();
 runtimes.Add(stopwatch.ElapsedMilliseconds);
 // Output the minimum spanning tree for each run
 Console.WriteLine($"\nRun {i + 1} - Prim's Algorithm Runtime:
{stopwatch.ElapsedMilliseconds} ms");
 Console.WriteLine("Minimum Spanning Tree:");
 foreach (var edge in mst)
 {
 Console.WriteLine($"{edge.StartNode} -- {edge.EndNode} (Weight:
{edge.Weight})");
 }
 }
 // Output all runtimes for analysis
 Console.WriteLine("\nRuntimes for Prim's algorithm across 10 runs:");
 foreach (var runtime in runtimes)
 {
 Console.WriteLine($"{runtime} ms");
 }

 // (Optional) Save runtimes to a file for external plotting
 File.WriteAllLines("prim_runtimes.txt", runtimes.Select(rt => rt.ToString()));
 }
 // Method to load graph from the given file "graph.txt"
 static Graph LoadGraphFromFile(string filename)
 {
 List<Edge> edges = new List<Edge>();
 int nodeCount;
 using (StreamReader reader = new StreamReader(filename))
 {
 nodeCount = int.Parse(reader.ReadLine() ?? "0"); // Read the number of nodes
 int edgeCount = int.Parse(reader.ReadLine() ?? "0"); // Read the number of
edges
 while (!reader.EndOfStream)
 {
 string line = reader.ReadLine();
 var parts = line.Split(' ');
 if (parts.Length == 3)
 {
 int startNode = int.Parse(parts[0]);
 int endNode = int.Parse(parts[1]);
 int weight = int.Parse(parts[2]);
 edges.Add(new Edge(startNode, endNode, weight));
 }

 }
 }
 return new Graph(nodeCount, edges);
 }
}
class Graph
{
 private readonly int NodeCount;
 private readonly List<Edge> Edges;
 public Graph(int nodeCount, List<Edge> edges)
 {
 NodeCount = nodeCount;
 Edges = edges;
 }
 // Prim's algorithm to find the minimum spanning tree
 public List<Edge> Prim()
 {
 Dictionary<int, List<Edge>> adjacencyList = new Dictionary<int, List<Edge>>();

 // Build adjacency list from edges
 foreach (var edge in Edges)
 {
 if (!adjacencyList.ContainsKey(edge.StartNode))
 adjacencyList[edge.StartNode] = new List<Edge>();

 if (!adjacencyList.ContainsKey(edge.EndNode))
 adjacencyList[edge.EndNode] = new List<Edge>();
 adjacencyList[edge.StartNode].Add(edge);
 adjacencyList[edge.EndNode].Add(edge); // Since it's an undirected graph
 }
 bool[] visited = new bool[NodeCount + 1];
 List<Edge> mst = new List<Edge>();
 PriorityQueue<Edge> minHeap = new PriorityQueue<Edge>();
 // Start from node 1
 visited[1] = true;
 foreach (var edge in adjacencyList[1])
 {
 minHeap.Enqueue(edge);
 }
 while (minHeap.Count > 0 && mst.Count < NodeCount - 1)
 {
 Edge smallestEdge = minHeap.Dequeue();
 int nextNode = visited[smallestEdge.EndNode] ? smallestEdge.StartNode :
smallestEdge.EndNode;
 if (!visited[nextNode])
 {
 mst.Add(smallestEdge);
 visited[nextNode] = true;

 foreach (var edge in adjacencyList[nextNode])
 {
 if (!visited[edge.EndNode] || !visited[edge.StartNode])
 {
 minHeap.Enqueue(edge);
 }
 }
 }
 }
 return mst;
 }
}
class Edge : IComparable<Edge>
{
 public int StartNode { get; }
 public int EndNode { get; }
 public int Weight { get; }
 public Edge(int startNode, int endNode, int weight)
 {
 StartNode = startNode;
 EndNode = endNode;
 Weight = weight;
 }
 public int CompareTo(Edge other)

 {
 return Weight.CompareTo(other.Weight);
 }
}
// Priority queue class to handle edges in Prim's algorithm
public class PriorityQueue<T> where T : IComparable<T>
{
 private readonly List<T> elements = new List<T>();
 public int Count => elements.Count;
 public void Enqueue(T item)
 {
 elements.Add(item);
 elements.Sort(); // Maintain priority based on weight
 }
 public T Dequeue()
 {
 if (Count == 0)
 throw new InvalidOperationException("Queue is empty");
 T item = elements[0];
 elements.RemoveAt(0);
 return item;
 }
}
