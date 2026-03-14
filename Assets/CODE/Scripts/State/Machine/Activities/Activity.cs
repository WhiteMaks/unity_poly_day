using System.Threading;
using System.Threading.Tasks;
using CODE.Scripts.State.Machine.Enums;
using CODE.Scripts.State.Machine.Interfaces;
using UnityEngine;

namespace CODE.Scripts.State.Machine.Activities
{
	/// <summary>
	/// Базовая абстракция для активности, которую можно активировать и деактивировать.
	/// </summary>
	/// <remarks>
	/// Activity хранит состояние в виде <see cref="Mode"/>, предоставляя асинхронные методы
	/// <see cref="ActivateAsync"/> и <see cref="DeactivateAsync"/>, которые можно переопределять
	/// в наследниках для выполнения длительных операций при входе/выходе.
	/// </remarks>
	public abstract class Activity : IActivity
	{
		/// <summary>
		/// Текущее состояние активности (Inactive, Activating, Active, Deactivating).
		/// </summary>
		public ActiveMode Mode { get; protected set; } = ActiveMode.Inactive;

		/// <summary>
		/// Асинхронно активирует активность. Базовая реализация помечает режим Activating -> Active.
		/// Наследники должны соблюдать контракт и корректно проверять и выставлять <see cref="Mode"/>.
		/// </summary>
		/// <param name="token">Токен отмены, позволяющий прервать операцию активации.</param>
		/// <returns>Задача, завершающаяся после установки режима Active.</returns>
		public virtual async Task ActivateAsync(CancellationToken token)
		{
			if (Mode != ActiveMode.Inactive)
			{
				return;
			}

			Mode = ActiveMode.Activating;
			await Task.CompletedTask;
			Mode = ActiveMode.Active;

			Debug.Log($"Activated {GetType().Name} (mode={Mode})");
		}

		/// <summary>
		/// Асинхронно деактивирует активность. Базовая реализация помечает режим Deactivating -> Inactive.
		/// Наследники должны корректно обрабатывать отмену через токен.
		/// </summary>
		/// <param name="token">Токен отмены для операции деактивации.</param>
		/// <returns>Задача, завершающаяся после установки режима Inactive.</returns>
		public virtual async Task DeactivateAsync(CancellationToken token)
		{
			if (Mode != ActiveMode.Active)
			{
				return;
			}

			Mode = ActiveMode.Deactivating;
			await Task.CompletedTask;
			Mode = ActiveMode.Inactive;

			Debug.Log($"Deactivated {GetType().Name} (mode={Mode})");
		}
	}
}