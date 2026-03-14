using CODE.Scripts.Dependency.Injection.Attributes;
using CODE.Scripts.Dependency.Injection.Interfaces;
using CODE.Scripts.Services;
using UnityEngine;

namespace CODE.Scripts.Dependency.Injection
{
	/// <summary>
	/// Простейший пример провайдера зависимостей, реализующий <see cref="IDependencyProvider"/>.
	/// </summary>
	/// <remarks>
	/// Класс демонстрирует, как помечать методы атрибутом <see cref="ProvideAttribute"/>
	/// для регистрации возвращаемых ими экземпляров в инжекторе <see cref="Injector"/>.
	/// В реальном проекте провайдеры могут предоставлять синглтоны, фабрики или конфигурируемые
	/// объекты (например, сервисы доступа к сети, кэш и т.д.).
	/// </remarks>
	public class Provider : MonoBehaviour, IDependencyProvider
	{
		/// <summary>
		/// Пример метода-поставщика, помеченного атрибутом <see cref="ProvideAttribute"/>.
		/// Возвращаемое значение будет зарегистрировано в реестре инжектора по типу <see cref="Service1"/>.
		/// </summary>
		/// <returns>Новый экземпляр <see cref="Service1"/>.</returns>
		[Provide]
		public Service1 ProvideService1()
		{
			return new Service1();
		}
	}
}