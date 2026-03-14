using System.Collections.Generic;
using CODE.Scripts.Observer.Interfaces;
using UnityEngine;

namespace CODE.Scripts.Observer
{
	/// <summary>
	/// Менеджер, централизующий вызовы <c>FixedUpdate</c> для подписчиков в сцене.
	/// </summary>
	/// <remarks>
	/// Этот класс предназначен для объединения вызовов физически-детерминированной
	/// логики, которая обычно выполняется в Unity-методе <c>FixedUpdate</c>.
	/// Подписчики должны реализовать <see cref="IFixedUpdateObserver"/> и регистрироваться
	/// через <see cref="Add(IFixedUpdateObserver)"/>.
	///
	/// Поведение и особенности:
	/// - Итерация происходит в обратном порядке для безопасного удаления элементов во
	///   время прохода по списку.
	/// - Добавление новых подписчиков помещается в <c>PendingObservers</c> и вступает
	///   в силу после завершения текущего прохода <c>FixedUpdate</c>.
	/// - Статические списки обеспечивают глобальную доступность менеджера, но привязывают
	///   жизненный цикл к домену приложения.
	///
	/// Рекомендации:
	/// - Используйте этот менеджер для физической логики (движение, столкновения,
	///   интеграция), которую нужно выполнять с постоянным шагом.
	/// - Старайтесь избегать аллокаций и тяжёлых операций в методах <c>CoreFixedUpdate</c>.
	/// </remarks>
	public class FixedUpdateSceneManager : MonoBehaviour
	{
		private static readonly List<IFixedUpdateObserver> Observers = new();
		private static readonly List<IFixedUpdateObserver> PendingObservers = new();

		private static int _index;

		/// <summary>
		/// Регистрирует подписчика для получения вызовов <c>CoreFixedUpdate</c> каждый фиксированный шаг.
		/// </summary>
		/// <param name="observer">Объект, реализующий <see cref="IFixedUpdateObserver"/>.</param>
		public static void Add(IFixedUpdateObserver observer)
		{
			PendingObservers.Add(observer);
		}

		/// <summary>
		/// Удаляет подписчика из списка FixedUpdate.
		/// </summary>
		/// <param name="observer">Подписчик для удаления.</param>
		/// <remarks>Если удаление происходит во время итерации, внутренний индекс уменьшается.</remarks>
		public static void Remove(IFixedUpdateObserver observer)
		{
			Observers.Remove(observer);
			_index--;
		}

		/// <summary>
		/// Unity-метод <c>FixedUpdate</c>, который вызывает <c>CoreFixedUpdate</c>
		/// у всех зарегистрированных подписчиков.
		/// </summary>
		private void FixedUpdate()
		{
			for (_index = Observers.Count - 1; _index >= 0; _index--)
			{
				Observers[_index].CoreFixedUpdate();
			}

			Observers.AddRange(PendingObservers);
			PendingObservers.Clear();
		}
	}
}