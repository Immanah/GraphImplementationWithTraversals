using System;
using System.Collections.Generic;
using System.Linq;
namespace GraphImplementation
{
 // Represents a single node (vertex) in the graph
 public class GraphNode
 {
 public int Id { get; } // Unique identifier for each node
 public GraphNode(int id)
 {
 this.Id = id;
 }
 }
 // Represents a weighted edge between two nodes in an undirected graph
 public class GraphEdge
 {
 public GraphNode NodeA { get; }
 public GraphNode NodeB { get; }
 public int Weight { get; }
 public GraphEdge(GraphNode nodeA, GraphNode nodeB, int weight)
 {
 this.NodeA = nodeA;
 this.NodeB = nodeB;

 this.Weight = weight;
 }
 // Returns the adjacent node for the given node
 public GraphNode GetAdjacentNode(GraphNode currentNode)
 {
 return currentNode == NodeA ? NodeB : NodeA;
 }
 // Checks if two edges connect the same nodes (to prevent duplicates)
 public bool IsEqual(GraphEdge otherEdge)
 {
 return (NodeA == otherEdge.NodeA && NodeB == otherEdge.NodeB) ||
 (NodeA == otherEdge.NodeB && NodeB == otherEdge.NodeA);
 }
 }
 // Represents the undirected, weighted graph
 public class Graph
 {
 private Dictionary<int, GraphNode> nodes; // Stores all nodes by ID for fast lookup
 private List<GraphEdge> edges; // Stores all edges in the graph
 private Dictionary<GraphNode, List<GraphEdge>> adjacencyList; // Adjacency list
for edges
 public Graph()
 {
 nodes = new Dictionary<int, GraphNode>();
 edges = new List<GraphEdge>();

 adjacencyList = new Dictionary<GraphNode, List<GraphEdge>>();
 }
 // Adds a new node to the graph
 public void AddNode(int id)
 {
 if (nodes.ContainsKey(id))
 {
 Console.WriteLine($"Error: Node with ID {id} already exists.");
 return;
 }
 var newNode = new GraphNode(id);
 nodes[id] = newNode;
 adjacencyList[newNode] = new List<GraphEdge>();
 }
 // Helper method to check if an edge exists between two nodes
 private bool EdgeExists(GraphNode nodeA, GraphNode nodeB)
 {
 return adjacencyList[nodeA].Any(edge => edge.IsEqual(new GraphEdge(nodeA,
nodeB, 0)));
 }
 // Adds an undirected, weighted edge between two nodes
 public void AddEdge(int id1, int id2, int weight)
 {
 if (id1 == id2)
 {

 Console.WriteLine($"Error: Self-loop on node {id1} is not allowed.");
 return;
 }
 if (!nodes.ContainsKey(id1) || !nodes.ContainsKey(id2))
 {
 Console.WriteLine($"Error: One or both nodes with IDs {id1} or {id2} not
found.");
 return;
 }
 if (weight <= 0)
 {
 Console.WriteLine("Error: Edge weight must be positive.");
 return;
 }
 var nodeA = nodes[id1];
 var nodeB = nodes[id2];
 if (EdgeExists(nodeA, nodeB))
 {
 Console.WriteLine($"Error: Edge between {id1} and {id2} already exists.");
 return;
 }
 var newEdge = new GraphEdge(nodeA, nodeB, weight);
 edges.Add(newEdge);
 adjacencyList[nodeA].Add(newEdge);

 adjacencyList[nodeB].Add(newEdge);
 }
 // Removes an edge between two nodes
 public void RemoveEdge(int id1, int id2)
 {
 if (!nodes.ContainsKey(id1) || !nodes.ContainsKey(id2))
 {
 Console.WriteLine($"Error: One or both nodes with IDs {id1} or {id2} not
found.");
 return;
 }
 var nodeA = nodes[id1];
 var nodeB = nodes[id2];
 GraphEdge edgeToRemove = null;
 foreach (var edge in adjacencyList[nodeA])
 {
 // Check if the current edge matches the one to remove
 if (edge.IsEqual(new GraphEdge(nodeA, nodeB, 0)))
 {
 edgeToRemove = edge;
 break;
 }
 }
 if (edgeToRemove != null)
 {

 adjacencyList[nodeA].Remove(edgeToRemove);
 adjacencyList[nodeB].Remove(edgeToRemove);
 edges.Remove(edgeToRemove);
 Console.WriteLine($"Notice: Edge between {id1} and {id2} removed
successfully.");
 }
 else
 {
 Console.WriteLine($"Error: Edge between {id1} and {id2} not found.");
 }
 }
 // Returns a list of all nodes in the graph
 public List<GraphNode> GetAllNodes()
 {
 return new List<GraphNode>(nodes.Values);
 }
 // Returns a list of all edges in the graph
 public List<GraphEdge> GetAllEdges()
 {
 return edges;
 }
 }
 // Handles display of graph information
 public class GraphDisplay
 {
 // Displays the graph structure: each node and its connections with weights

 public static void DisplayGraph(Graph graph)
 {
 foreach (var node in graph.GetAllNodes())
 {
 var connections = new List<string>();
 foreach (var edge in graph.GetAllEdges().Where(e => e.NodeA == node ||
e.NodeB == node))
 {
 var adjacentNode = edge.GetAdjacentNode(node);
 connections.Add($"(Connected to {adjacentNode.Id}, Weight:
{edge.Weight})");
 }
 Console.WriteLine($"Node {node.Id}: {string.Join(" ", connections)}");
 }
 }
 }
 // Main program to demonstrate the graph implementation
 class Program
 {
 static void Main(string[] args)
 {
 var graph = new Graph();
 // Adding nodes
 graph.AddNode(1);
 graph.AddNode(2);
 graph.AddNode(3);

 graph.AddNode(4);
 // Adding edges
 graph.AddEdge(1, 2, 10);
 graph.AddEdge(1, 3, 5);
 graph.AddEdge(2, 4, 2);
 graph.AddEdge(3, 4, 8);
 // Attempting duplicate edge (should result in an error)
 graph.AddEdge(1, 2, 10);
 // Attempting to add self-loop (should result in an error)
 graph.AddEdge(1, 1, 5);
 // Removing an edge
 graph.RemoveEdge(1, 2);
 // Displaying the graph structure using the GraphDisplay class
 GraphDisplay.DisplayGraph(graph);
 }
 }
}
