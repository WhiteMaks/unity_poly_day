using CODE.Scripts.Dependency.Injection.Attributes;
using CODE.Scripts.Dependency.Injection.Interfaces;
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
	}
}