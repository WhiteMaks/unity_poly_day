using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CODE.Scripts.State.Machine.Interfaces;

namespace CODE.Scripts.State.Machine.Sequences
{
	/// <summary>
	/// Параллельное исполнение набора шагов — все шаги запускаются одновременно и ожидаются.
	/// </summary>
	public class ParallelPhaseSequence : ISequence
	{
		private readonly List<PhaseStep> _steps;
		private readonly CancellationToken _token;

		private List<Task> _tasks;

		/// <summary>
		/// Признак завершённости последовательности.
		/// </summary>
		public bool IsDone { get; private set; }

		/// <summary>
		/// Создаёт параллельную последовательность шагов.
		/// </summary>
		/// <param name="steps">Список шагов для выполнения.</param>
		/// <param name="token">Токен отмены для шагов.</param>
		public ParallelPhaseSequence(List<PhaseStep> steps, CancellationToken token)
		{
			_steps = steps;
			_token = token;
		}

		/// <summary>
		/// Запускает все шаги одновременно и сохраняет задачи для последующей проверки статуса.</summary>
		public void Start()
		{
			if (_steps == null || _steps.Count == 0)
			{
				IsDone = true;
				return;
			}

			_tasks = new List<Task>(_steps.Count);
			foreach (var step in _steps)
			{
				_tasks.Add(step(_token));
			}
		}

		/// <summary>
		/// Проверяет завершённость всех задач. Возвращает true, когда все задачи завершены.
		/// </summary>
		/// <returns>True, если все шаги завершены.</returns>
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