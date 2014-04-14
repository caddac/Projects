using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    class LinkedListTest
    {
        static void Main()
        {
            GenericLinkList<int> list = new GenericLinkList<int>(new int[] {1,2,3,4,4,5,5,6,});


            foreach (int i in list)
            {
                System.Console.WriteLine(i + System.Environment.NewLine);
            }
        }
    }
}
