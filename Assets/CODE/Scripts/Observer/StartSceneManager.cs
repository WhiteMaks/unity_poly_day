using System.Collections.Generic;
using CODE.Scripts.Observer.Interfaces;
using UnityEngine;

namespace CODE.Scripts.Observer
{
	/// <summary>
	/// Менеджер, централизующий вызов Start для подписчиках сцены.
	/// </summary>
	/// <remarks>
	/// Этот класс объединяет единственный вызов Unity <c>Start</c> и пересылает его всем
	/// зарегистрированным объектам, реализующим <see cref="IStartObserver"/>.
	///
	/// Поведение:
	/// - Все подписчики вызываются в порядке перечисления списка <c>Observers</c>.
	/// - Менеджер хранит список статически, поэтому регистрация доступна глобально.
	/// - <c>Start</c> вызывается один раз Unity-движком при активации компонента
	///   (обычно при загрузке сцены). Если некоторые подписчики будут зарегистрированы
	///   после выполнения <c>Start</c> менеджера, они не получат вызов <c>CoreStart</c>.
	///   Если требуется гарантированный вызов для поздних зарегистрированных объектов,
	///   рассмотрите вариант вызова их вручную или изменение логики менеджера.
	///
	/// Рекомендации по использованию:
	/// - Регистрация обычно делается в <c>OnEnable</c> и удаление в <c>OnDisable</c>.
	/// - Поскольку вызов <c>Start</c> происходит один раз, не используйте этот менеджер
	///   для частых инициализаций; для них лучше подойдёт отдельная логика.
	///
	/// Пример:
	/// <code>
	/// public class MyComponent : IStartObserver {
	///     public void CoreStart() { /* инициализация */ }
	///     void OnEnable() => StartSceneManager.Add(this);
	///     void OnDisable() => StartSceneManager.Remove(this);
	/// }
	/// </code>
	/// </remarks>
	public class StartSceneManager : MonoBehaviour
	{
		private static readonly List<IStartObserver> Observers = new();

		/// <summary>
		/// Регистрирует подписчика для получения вызова <c>CoreStart</c>.
		/// </summary>
		/// <param name="observer">Объект, реализующий <see cref="IStartObserver"/>.</param>
		public static void Add(IStartObserver observer)
		{
			Observers.Add(observer);
		}

		/// <summary>
		/// Удаляет подписчика из списка Start-инициализации.
		/// </summary>
		/// <param name="observer">Подписчик для удаления.</param>
		public static void Remove(IStartObserver observer)
		{
			Observers.Remove(observer);
		}

		/// <summary>
		/// Unity-метод <c>Start</c>, вызываемый один раз при активации компонента;
		/// перебирает всех зарегистрированных подписчиков и вызывает у них <c>CoreStart</c>.
		/// </summary>
		private void Start()
		{
			foreach (var observer in Observers)
			{
				observer.CoreStart();
			}
		}
	}
}