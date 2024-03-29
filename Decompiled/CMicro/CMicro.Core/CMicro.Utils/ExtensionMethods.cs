using System.Collections.Generic;
using System.Linq;

namespace CMicro.Utils
{
	public static class ExtensionMethods
	{
		public static void StackReverse<T>(this Stack<T> stack)
		{
			Stack<T> stack2 = new Stack<T>();
			foreach (T item in stack.Reverse())
			{
				stack2.Push(item);
			}
			stack.Clear();
			stack = stack2;
		}
	}
}
