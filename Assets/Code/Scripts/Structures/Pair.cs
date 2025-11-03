namespace Code.Scripts.Structures
{
	public readonly struct Pair<TLeft, TRight>
	{
		private readonly TLeft _left;
		private readonly TRight _right;

		public Pair(TLeft left, TRight right)
		{
			_left = left;
			_right = right;
		}

		public TLeft GetLeft()
		{
			return _left;
		}

		public TRight GetRight()
		{
			return _right;
		}
	}
}
