using System;
using System.Threading;
using System.Threading.Tasks;

namespace CODE.Scripts.State.Machine.Activities
{
	/// <summary>
	/// Простая активность, которая вводит задержку при активации перед вызовом базовой логики.
	/// </summary>
	/// <remarks>
	/// Используется для моделирования отложенных инициализаций или ожидания перед тем как активность
	/// пометится как <see cref="CODE.Scripts.State.Machine.Enums.ActiveMode.Active"/>.
	/// </remarks>
	public class DelayActivationActivity : Activity
	{
		private readonly float _seconds;

		/// <summary>
		/// Создаёт активность с указанной задержкой в секундах.
		/// </summary>
		/// <param name="seconds">Время задержки в секундах перед активацией.</param>
		public DelayActivationActivity(float seconds)
		{
			_seconds = seconds;
		}

		/// <summary>
		/// Выполняет задержку, затем вызывает базовую реализацию <see cref="Activity.ActivateAsync"/>.
		/// </summary>
		/// <param name="token">Токен отмены для асинхронной операции.</param>
		/// <returns>Задача, представляющая асинхронную операцию активации.</returns>
		public override async Task ActivateAsync(CancellationToken token)
		{
			await Task.Delay(TimeSpan.FromSeconds(_seconds), token);
			await base.ActivateAsync(token);
		}
	}
}