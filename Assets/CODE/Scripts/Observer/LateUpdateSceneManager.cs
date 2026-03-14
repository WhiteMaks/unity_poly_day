using System.Collections.Generic;
using CODE.Scripts.Observer.Interfaces;
using UnityEngine;

namespace CODE.Scripts.Observer
{
	/// <summary>
	/// Менеджер, централизующий вызовы <c>LateUpdate</c> для подписчиков в сцене.
	/// </summary>
	/// <remarks>
	/// Аналогично <see cref="UpdateSceneManager"/>, этот класс собирает подписчиков,
	/// реализующих <see cref="ILateUpdateObserver"/>, и вызывает у них метод
	/// <see cref="ILateUpdateObserver.CoreLateUpdate"/> в Unity-методе <c>LateUpdate</c>.
	///
	/// Ключевые особенности:
	/// - Итерация выполняется в обратном порядке, что позволяет безопаснее удалять
	///   элементы во время прохода.
	/// - Новые подписчики добавляются в буфер <c>PendingObservers</c> и будут
	///   учтены только после завершения текущего вызова <c>LateUpdate</c>.
	/// - Удаление декрементирует внутренний индекс для поддержания корректной
	///   итерации при удалении во время прохождения списка.
	///
	/// Применение:
	/// Используйте этот менеджер, когда нужно выполнять логику, зависящую от всех
	/// Update-операций, но после них (например, корректировка позиции камеры после
	/// перемещения объектов в Update).
	/// </remarks>
	public class LateUpdateSceneManager : MonoBehaviour
	{
		private static readonly List<ILateUpdateObserver> Observers = new();
		private static readonly List<ILateUpdateObserver> PendingObservers = new();

		private static int _index;

		/// <summary>
		/// Регистрирует подписчика для получения вызовов <c>CoreLateUpdate</c>.
		/// </summary>
		/// <param name="observer">Объект, реализующий <see cref="ILateUpdateObserver"/>.</param>
		public static void Add(ILateUpdateObserver observer)
		{
			PendingObservers.Add(observer);
		}

		/// <summary>
		/// Удаляет подписчика из списка LateUpdate.
		/// </summary>
		/// <param name="observer">Подписчик для удаления.</param>
		/// <remarks>Если удаление происходит во время итерации, внутренний индекс уменьшается.</remarks>
		public static void Remove(ILateUpdateObserver observer)
		{
			Observers.Remove(observer);
			_index--;
		}

		/// <summary>
		/// Unity-метод <c>LateUpdate</c>, который вызывает <c>CoreLateUpdate</c>
		/// у всех зарегистрированных подписчиков.
		/// </summary>
		private void LateUpdate()
		{
			for (_index = Observers.Count - 1; _index >= 0; _index--)
			{
				Observers[_index].CoreLateUpdate();
			}

			Observers.AddRange(PendingObservers);
			PendingObservers.Clear();
		}
	}
}