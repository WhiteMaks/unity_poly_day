using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CODE.Scripts.State.Machine.Interfaces;

namespace CODE.Scripts.State.Machine.Sequences
{
	public class ParallelPhaseSequence : ISequence
	{
		private readonly List<PhaseStep> _steps;
		private readonly CancellationToken _token;

		private List<Task> _tasks;

		public bool IsDone { get; private set; }

		public ParallelPhaseSequence(List<PhaseStep> steps, CancellationToken token)
		{
			_steps = steps;
			_token = token;
		}

		public void Start()
		{
			if (_steps == null || _steps.Count == 0)
			{
				IsDone = true;
				return;
			}

			_tasks = new  List<Task>(_steps.Count);
			foreach (var step in _steps)
			{
				_tasks.Add(step(_token));
			}
		}

		public bool Update()
		{
			if (IsDone)
			{
				return true;
			}

			IsDone = _tasks == null || _tasks.TrueForAll(task => task.IsCompleted);

			return IsDone;
		}
	}
}