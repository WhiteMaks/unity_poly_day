using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CODE.Scripts.State.Machine.Interfaces;

namespace CODE.Scripts.State.Machine.Sequences
{
	public class SequentialPhaseSequence : ISequence
	{
		private readonly List<PhaseStep> _steps;
		private readonly CancellationToken _token;

		private Task _currentTask;

		private int _index = -1;

		public bool IsDone { get; private set; }

		public SequentialPhaseSequence(List<PhaseStep> steps, CancellationToken token)
		{
			_steps = steps;
			_token = token;
		}

		public void Start()
		{
			Next();
		}

		public bool Update()
		{
			if (IsDone)
			{
				return true;
			}

			if (_currentTask == null || _currentTask.IsCompleted)
			{
				Next();
			}

			return IsDone;
		}

		private void Next()
		{
			_index++;

			if (_index >= _steps.Count)
			{
				IsDone = true;
				return;
			}

			_currentTask = _steps[_index](_token);
		}

	}
}