using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleDevice.NodeRedAPI
{
    public class Flow
    {

        public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string info { get; set; }
        public Node[] nodes { get; set; }

        public void addNode(Node node)
        {
            List<Node> nodesList = nodes.ToList();
            nodesList.Add(node);
            nodes = nodesList.ToArray();
        }

        public void updateNode(Node node)
        {
            Console.WriteLine(node.name);
            if (node != null && node.id != null && containsNode(node.id))
            {
                List<Node> nodesList = nodes.ToList();
                for (int i = 0; i < nodesList.Count; i++)
                {
                    Console.WriteLine(nodesList.ElementAt(i).id + nodesList.ElementAt(i).name);
                }
                for (int i = 0; i < nodesList.Count; i++)
                {
                    if (nodesList.ElementAt(i).id == node.id)
                    {
                        Console.WriteLine(nodesList.ElementAt(i).id + nodesList.ElementAt(i).name);
                        nodesList.RemoveAt(i);
                        nodesList.Insert(i, node);
                        Console.WriteLine(nodesList.ElementAt(i).id + nodesList.ElementAt(i).name);
                        break;
                    }
                }
                for (int i = 0; i < nodesList.Count; i++)
                {
                    Console.WriteLine(nodesList.ElementAt(i).id + nodesList.ElementAt(i).name);
                }
                nodes = nodesList.ToArray();
            }
        }

        public void removeNode(string nodeId)
        {
            List<Node> nodesList = nodes.ToList();
            for (int i = 0; i < nodesList.Count; i++)
            {
                if (nodesList.ElementAt(i).id == nodeId)
                {
                    nodesList.RemoveAt(i);
                    break;
                }
            }
            nodes = nodesList.ToArray();
        }

        public bool addLink(string sourceId, string destId)
        {
            List<Node> nodesList = nodes.ToList();

            if (containsNode(sourceId) && containsNode(destId))
            {
                Node source = getNode(sourceId);
                if (source.wires.Length == 0)
                {
                    source.wires = new string[1][];
                }
                List<string> wires = source.wires[0].ToList();
                wires.Add(destId);
                source.wires[0] = wires.ToArray();
                return true;
            }
            return false;
        }

        public bool removeLink(string sourceId, string destId)
        {
            List<Node> nodesList = nodes.ToList();

            if (containsNode(sourceId) && containsNode(destId))
            {
                Node source = getNode(sourceId);
                if (source.wires.Length == 0)
                {
                    source.wires = new string[1][];
                }
                List<string> wires = source.wires[0].ToList();
                wires.Remove(destId);
                source.wires[0] = wires.ToArray();
                return true;
            }
            return false;
        }

        public bool containsNode(string id)
        {
            List<Node> nodesList = nodes.ToList();
            foreach (Node node in nodesList)
            {
                if (node.id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public Node getNode(string id)
        {
            List<Node> nodesList = nodes.ToList();
            foreach (Node node in nodesList)
            {
                if (node.id == id)
                {
                    return node;
                }
            }
            return null;
        }

    }
}
