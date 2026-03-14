using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CODE.Scripts.State.Machine.Interfaces;

namespace CODE.Scripts.State.Machine.Sequences
{
	/// <summary>
	/// Последовательное исполнение набора шагов (<see cref="PhaseStep"/>).
	/// Каждый шаг запускается после завершения предыдущего.
	/// </summary>
	public class SequentialPhaseSequence : ISequence
	{
		private readonly List<PhaseStep> _steps;
		private readonly CancellationToken _token;

		private Task _currentTask;

		private int _index = -1;

		/// <summary>
		/// Признак завершённости последовательности.
		/// </summary>
		public bool IsDone { get; private set; }

		/// <summary>
		/// Создаёт последовательную последовательность шагов.
		/// </summary>
		/// <param name="steps">Список шагов для выполнения.</param>
		/// <param name="token">Токен отмены, который будет передаваться шагам.</param>
		public SequentialPhaseSequence(List<PhaseStep> steps, CancellationToken token)
		{
			_steps = steps;
			_token = token;
		}

		/// <summary>
		/// Запускает выполнение — инициируется первый шаг.
		/// </summary>
		public void Start()
		{
			Next();
		}

		/// <summary>
		/// Вызывается регулярно (например, из Update) для проверки завершённости текущего шага и продвижения.
		/// Возвращает true, когда вся последовательность завершена.
		/// </summary>
		/// <returns>True, если все шаги выполнены.</returns>
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