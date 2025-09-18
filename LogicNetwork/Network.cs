namespace LogicNetwork
{
    public class Network
    {
        public List<uint> Elements { get; set; }
        public List<(uint, uint)> Connections { get; set; } = [];

        public Network(uint elementsAmount)
        {
            if (elementsAmount == 0)
            {
                throw new ArgumentException("Elements amount must be greater than zero.");
            }

            Elements = [.. Enumerable.Range(1, (int)elementsAmount).Select(e => (uint)e)];
        }

        public void Connect((uint, uint) connection)
        {
            ValidateConnection(connection);
            ValidateExistingConnection(connection);
            Connections.Add(connection);
        }

        public void ConnectMany((uint, uint)[] connections)
        {
            foreach (var connection in connections)
            {
                ValidateConnection(connection);
                ValidateExistingConnection(connection);
            }

            Connections.AddRange(connections);
        }

        public bool Query((uint, uint) connection)
        {
            ValidateConnection(connection);

            return ConnectionExists(connection);
        }

        private bool ConnectionExists((uint, uint) connection)
        {
            HashSet<uint> visited = new();
            Queue<uint> queue = new();

            queue.Enqueue(connection.Item1);
            visited.Add(connection.Item1);

            while (queue.Count > 0)
            {
                uint current = queue.Dequeue();
                if (current == connection.Item2)
                {
                    return true;
                }
                var neighbors = Connections
                    .Where(c => c.Item1 == current || c.Item2 == current)
                    .Select(c => c.Item1 == current ? c.Item2 : c.Item1);

                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
            return false;
        }

        private void ValidateConnection((uint, uint) connection)
        {
            if (connection.Item1 == connection.Item2)
            {
                throw new ArgumentException("Cannot connect an element to itself.");
            }

            if (connection.Item1 > Elements.Count || connection.Item2 > Elements.Count)
            {
                throw new ArgumentException("Connection elements must be within the range of existing elements.");
            }

            if (connection.Item1 == 0 || connection.Item2 == 0)
            {
                throw new ArgumentException("Element indices must be greater than zero.");
            }
        }

        private void ValidateExistingConnection((uint, uint) connection)
        {
            if (Connections.Contains(connection) || Connections.Contains((connection.Item2, connection.Item1)))
            {
                throw new ArgumentException("Connection already exists.");
            }
        }
    }
}
