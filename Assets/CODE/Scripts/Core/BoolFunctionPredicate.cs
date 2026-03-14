using System;
using CODE.Scripts.Core.Interfaces;

namespace CODE.Scripts.Core
{
	public class BoolFunctionPredicate : IPredicate
	{
		private readonly Func<bool> _func;

		public BoolFunctionPredicate(Func<bool> func)
		{
			_func = func;
		}

		public bool Evaluate()
		{
			return _func.Invoke();
		}
	}
}