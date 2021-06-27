using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindFiles
{
	public class Model
	{
		// можно сохранять данные в дереве, но для данной задачи это не нужно
		// Tree<string, Node> tree; 
		public TimeSpan MeasuredTime { get; set; }
		public int NumberOfMatchingElements { get; set; }
		public int NumberOfAllElements { get; set; }

		public Model()
		{
			// tree = new Tree<string, Node>();
			
		}

		public void AddNewElement(string x)
        {
			// tree.AddNode(null, x, new Node(x)); 

			if (TreeElementAdded != null)
				TreeElementAdded.Invoke(x);
		}
		public void DeleteAllElements()
		{
            // tree.ClearTree();
            NumberOfMatchingElements = 0;
            NumberOfAllElements = 0;
			MeasuredTime = new TimeSpan(0, 0, 0, 0);
			if (TreeCleared != null)
				TreeCleared.Invoke();
		}

		public event Action<string> TreeElementAdded;
		public event Action TreeCleared;
	}
	

	/*public class Node
	{
		public string Name { get; set; }

		public Node()
		{
		}
		public Node(string name)
		{
			Name = name;
		}
	}*/
}
