using System.Threading;
using System.Threading.Tasks;

namespace CODE.Scripts.State.Machine.Interfaces
{
	/// <summary>
	/// Интерфейс для выполнения последовательности шагов с возможностью поэтапной проверки статуса.
	/// Реализации могут быть последовательными или параллельными.
	/// </summary>
	public interface ISequence
	{
		/// <summary>
		/// Признак того, что последовательность завершена.
		/// </summary>
		bool IsDone { get; }

		/// <summary>
		/// Инициализирует выполнение последовательности (запускает внутренние задачи).
		/// </summary>
		void Start();

		/// <summary>
		/// Выполняется регулярно для обновления состояния последовательности; возвращает true, если она завершена.
		/// </summary>
		bool Update();
	}

	/// <summary>
	/// Делегат, представляющий один шаг фазы перехода. Получает CancellationToken и возвращает Task.
	/// </summary>
	/// <param name="token">Токен отмены для выполнения шага.</param>
	/// <returns>Задача, завершающаяся после выполнения шага.</returns>
	public delegate Task PhaseStep(CancellationToken token);
}