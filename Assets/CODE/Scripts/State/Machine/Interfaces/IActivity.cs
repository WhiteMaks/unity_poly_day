using System.Threading;
using System.Threading.Tasks;
using CODE.Scripts.State.Machine.Enums;

namespace CODE.Scripts.State.Machine.Interfaces
{
	/// <summary>
	/// Интерфейс для активностей, которые могут быть активированы и деактивированы в процессе жизненного цикла состояния.
	/// </summary>
	public interface IActivity
	{
		/// <summary>
		/// Текущее состояние активности (см. <see cref="CODE.Scripts.State.Machine.Enums.ActiveMode"/>).
		/// </summary>
		ActiveMode Mode { get; }

		/// <summary>
		/// Асинхронно активирует активность. Метод должен корректно обрабатывать токен отмены.
		/// </summary>
		/// <param name="token">Токен отмены.</param>
		/// <returns>Задача, завершающаяся после активации.</returns>
		Task ActivateAsync(CancellationToken token);

		/// <summary>
		/// Асинхронно деактивирует активность. Метод должен корректно обрабатывать токен отмены.
		/// </summary>
		/// <param name="token">Токен отмены.</param>
		/// <returns>Задача, завершающаяся после деактивации.</returns>
		Task DeactivateAsync(CancellationToken token);
	}
}