using System.Collections.Generic;
using CODE.Scripts.Observer.Interfaces;
using UnityEngine;

namespace CODE.Scripts.Observer
{
	/// <summary>
	/// Менеджер, централизующий вызовы обновления (Update) для подписчиков в сцене.
	/// </summary>
	/// <remarks>
	/// Этот класс служит оптимизационной обёрткой над вызовами <c>MonoBehaviour.Update</c> —
	/// вместо множества отдельных <c>Update</c> на разных компонентах, все подписчики
	/// реализуют <see cref="IUpdateObserver"/> и регистрируются в этом менеджере.
	///
	/// Поведение и гарантии:
	/// - Подписчики вызываются в цикле от последнего добавленного к первому (обратный порядок),
	///   что позволяет безопасно удалять элементы во время итерации при корректной реализации.
	/// - Новые подписчики, добавленные через <see cref="Add"/>, помещаются во временный
	///   список (<c>PendingObservers</c>) и добавляются в основной список после завершения
	///   прохода по текущему фрейму; это предотвращает модификацию коллекции во время итерации.
	/// - При удалении подписчика вызывается <see cref="Remove"/>, который также декрементирует
	///   внутренний индекс итерации. Это сделано для сохранения корректного индекса при
	///   удалении элемента, однако при неправильном внешнем использовании может привести к
	///   отрицательному значению индекса — учтите это при расширении функциональности.
	///
	/// Производительность и безопасность:
	/// - Списки хранятся статически, поэтому менеджер доступен глобально и не требует ссылки на
	///   конкретный экземпляр. Это удобно, но подразумевает, что жизненный цикл списков
	///   привязан к домену приложения.
	/// - Избегайте выделений в методах подписчиков (например, в <c>CoreUpdate</c>), чтобы
	///   минимизировать нагрузку на сборщик мусора при каждом кадре.
	///
	/// Пример использования:
	/// <code>
	/// // класс-подписчик
	/// public class MyBehaviour : IUpdateObserver {
	///     public void CoreUpdate() { /* логика обновления */ }
	///     void OnEnable()  => UpdateSceneManager.Add(this);
	///     void OnDisable() => UpdateSceneManager.Remove(this);
	/// }
	/// </code>
	/// </remarks>
	public class UpdateSceneManager : MonoBehaviour
	{
		private static readonly List<IUpdateObserver> Observers = new();
		private static readonly List<IUpdateObserver> PendingObservers = new();

		private static int _index;

		/// <summary>
		/// Регистрирует подписчика для получения вызовов CoreUpdate каждый кадр.
		/// </summary>
		/// <param name="observer">Объект, реализующий <see cref="IUpdateObserver"/>.</param>
		/// <remarks>
		/// Добавление происходит в буфер <c>PendingObservers</c>, поэтому подписчик
		/// начнёт получать вызовы начиная со следующего прохода цикла Update (текущий кадр
		/// останется без вызова для данного подписчика).
		/// </remarks>
		public static void Add(IUpdateObserver observer)
		{
			PendingObservers.Add(observer);
		}

		/// <summary>
		/// Удаляет подписчика из списка обновления.
		/// </summary>
		/// <param name="observer">Подписчик, который должен быть удалён.</param>
		/// <remarks>
		/// Если подписчик отсутствует в списке, метод молча ничего не делает. Внимание:
		/// текущая реализация уменьшает внутренний индекс <c>_index</c>, что предполагает,
		/// что удаление происходит во время итерации; это поддерживает корректную
		/// индексацию следующего элемента при удалении текущего.
		/// </remarks>
		public static void Remove(IUpdateObserver observer)
		{
			Observers.Remove(observer);
			_index--;
		}

		/// <summary>
		/// Встроенный Unity-метод <c>Update</c> — выполняет обход подписчиков и вызывает
		/// у каждого <see cref="IUpdateObserver.CoreUpdate"/>.
		/// </summary>
		/// <remarks>
		/// Итерация выполняется в обратном порядке (от последнего элемента к первому).
		/// После обхода все отложенные подписчики из <c>PendingObservers</c> добавляются
		/// в основной список, а буфер очищается.
		/// </remarks>
		private void Update()
		{
			for (_index = Observers.Count - 1; _index >= 0; _index--)
			{
				Observers[_index].CoreUpdate();
			}

			Observers.AddRange(PendingObservers);
			PendingObservers.Clear();
		}
	}
}