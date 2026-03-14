using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CODE.Scripts.Dependency.Injection.Attributes;
using CODE.Scripts.Dependency.Injection.Interfaces;
using CODE.Scripts.Observer;
using CODE.Scripts.Observer.Interfaces;
using UnityEngine;

namespace CODE.Scripts.Dependency.Injection
{
	/// <summary>
	/// Централизованный простой инжектор зависимостей для сцены.
	/// </summary>
	/// <remarks>
	/// Этот <c>MonoBehaviour</c> отвечает за обнаружение всех <c>MonoBehaviour</c> в
	/// сцене, регистрацию поставщиков зависимостей (<see cref="IDependencyProvider"/>)
	/// и внедрение зависимостей в поля, помеченные атрибутом <see cref="InjectAttribute"/>.
	///
	/// Основные особенности и контракт:
	/// - Инжектор является синглтоном по сцене: если в сцене есть несколько экземпляров,
	///   лишние будут уничтожены (поведение реализовано в <see cref="Awake"/>).
	/// - Все провайдеры, методы которых помечены атрибутом <see cref="ProvideAttribute"/>,

	///   вызываются для получения инстансов, которые затем помещаются в внутренний
	///   реестр по ключу типа возвращаемого значения.
	/// - Внедрение происходит путём отражения полей (включая приватные) и установки
	///   значения из реестра. Если для требуемого типа нет зарегистрированного значения,
	///   выбрасывается исключение, чтобы уведомить об ошибке конфигурации.
	/// - Инжектор автоматически помечает себя как неразрушаемый при смене сцен
	///   (см. <see cref="UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object)"/>), чтобы сохранить реестр между сценами при необходимости.
	///
	/// Ограничения и рекомендации:
	/// - Текущая реализация хранит объекты в словаре с ключом <c>Type</c>, поэтому
	///   зарегистрировать несколько провайдеров на один тип нельзя (будет исключение при добавлении).
	/// - Методы провайдеров вызываются один раз при инициализации инжектора; если требуется
	///   ленивое создание или фабрика — реализацию нужно расширить.
	/// - Избегайте длительных вычислений и аллокаций в методах с атрибутом <see cref="ProvideAttribute"/>
	///   в момент инициализации сцены.
	/// </remarks>
	public class Injector : MonoBehaviour, IStartObserver
	{
		// Экземпляр синглтона инжектора
		private static Injector _instance;

		// Флаги доступа, используемые для поиска полей и методов (включая приватные)
		private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

		// Реестр: тип -> экземпляр
		private readonly Dictionary<Type, object> _registry = new();

		/// <summary>
		/// Вызывается менеджером StartSceneManager один раз при старте сцены.
		/// </summary>
		/// <remarks>
		/// В текущей реализации метод используется для установки поведения
		/// <see cref="UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object)"/>, чтобы инжектор
		/// переживал смену сцен.
		/// </remarks>
		public void CoreStart()
		{
			DontDestroyOnLoad(gameObject);
		}

		/// <summary>
		/// Unity Awake: обеспечивает поведение синглтона и выполняет инициализацию
		/// при первом экземпляре.
		/// </summary>
		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				StartSceneManager.Remove(_instance);
			}
			else
			{
				_instance = this;
				Initialize();
				StartSceneManager.Add(_instance);
			}
		}

		/// <summary>
		/// Выполняет инициализацию инжектора: находит провайдеров и внедряемые объекты,
		/// регистрирует провайдеры и инжектит поля.
		/// </summary>
		private void Initialize()
		{
			var monoBehaviours = FindAllMonoBehaviours();

			var providers = monoBehaviours.OfType<IDependencyProvider>();
			foreach (var provider in providers)
			{
				RegisterProvider(provider);
			}

			var injectables = monoBehaviours.Where(IsInjectable);
			foreach (var injectable in injectables)
			{
				Inject(injectable);
			}
		}

		/// <summary>
		/// Выполняет внедрение зависимостей в поля указанного <paramref name="behaviour"/>.
		/// </summary>
		/// <param name="behaviour">Экземпляр <see cref="MonoBehaviour"/>, поля которого будут проинжечены.</param>
		/// <exception cref="Exception">Выбрасывается, если для типа поля отсутствует зарегистрированный экземпляр.</exception>
		private void Inject(MonoBehaviour behaviour)
		{
			var type = behaviour.GetType();
			var injectableFields = type.GetFields(BindingFlags);
			foreach (var injectableField in injectableFields)
			{
				if (!Attribute.IsDefined(injectableField, typeof(InjectAttribute)))
				{
					continue;
				}

				var fieldType = injectableField.FieldType;
				var instance = Resolve(fieldType);
				if (instance == null)
				{
					throw new Exception($"Failed to inject {fieldType.Name} into {type.Name}");
				}

				injectableField.SetValue(behaviour, instance);
				Debug.Log($"Injected {fieldType.Name} into {type.Name}");
			}
		}

		/// <summary>
		/// Возвращает зарегистрированный экземпляр для заданного типа или <c>null</c>, если он не найден.
		/// </summary>
		/// <param name="type">Тип зависимости, которую необходимо разрешить.</param>
		/// <returns>Зарегистрированный экземпляр или <c>null</c>.</returns>
		private object Resolve(Type type)
		{
			_registry.TryGetValue(type, out var result);
			return result;
		}

		/// <summary>
		/// Проверяет, содержит ли <paramref name="behaviour"/> элементы, помеченные атрибутом <see cref="InjectAttribute"/>.
		/// </summary>
		/// <param name="behaviour">Проверяемый экземпляр <see cref="MonoBehaviour"/>.</param>
		/// <returns><c>true</c>, если есть члены, помеченные <see cref="InjectAttribute"/>; иначе <c>false</c>.</returns>
		private static bool IsInjectable(MonoBehaviour behaviour)
		{
			var members = behaviour.GetType().GetMembers(BindingFlags);
			return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
		}

		/// <summary>
		/// Регистрирует все объекты, возвращаемые методами провайдера, помеченными <see cref="ProvideAttribute"/>.
		/// </summary>
		/// <param name="provider">Экземпляр <see cref="IDependencyProvider"/>, содержащий методы-поставщики.</param>
		/// <exception cref="Exception">Выбрасывается, если метод-провайдер вернул <c>null</c> для ожидаемого типа.</exception>
		private void RegisterProvider(IDependencyProvider provider)
		{
			var methods = provider.GetType().GetMethods(BindingFlags);
			foreach (var method in methods)
			{
				if (!Attribute.IsDefined(method, typeof(ProvideAttribute)))
				{
					continue;
				}

				var returnType = method.ReturnType;
				var providerInstance = method.Invoke(provider, null);
				if (providerInstance == null)
				{
					throw new Exception($"Provider {provider.GetType().Name} return null for {returnType.Name}");
				}

				_registry.Add(returnType, providerInstance);
				Debug.Log($"Registered {returnType.Name} from {provider.GetType().Name}");
			}
		}

		/// <summary>
		/// Находит все объекты типа <see cref="MonoBehaviour"/> в сцене.
		/// </summary>
		/// <returns>Массив всех найденных <see cref="MonoBehaviour"/>.</returns>
		private static MonoBehaviour[] FindAllMonoBehaviours()
		{
			return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
		}
	}
}