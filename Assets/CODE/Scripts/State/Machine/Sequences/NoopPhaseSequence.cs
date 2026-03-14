using CODE.Scripts.State.Machine.Interfaces;

namespace CODE.Scripts.State.Machine.Sequences
{
	public class NoopPhaseSequence : ISequence
	{
		public bool IsDone { get; private set; }

		public void Start()
		{
			IsDone = true;
		}

		public bool Update()
		{
			return IsDone;
		}
	}
}