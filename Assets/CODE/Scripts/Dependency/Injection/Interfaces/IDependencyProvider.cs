using CODE.Scripts.Dependency.Injection.Attributes;

namespace CODE.Scripts.Dependency.Injection.Interfaces
{
	/// <summary>
	/// Маркерный интерфейс для компонентов, которые предоставляют зависимости инжектору.
	/// </summary>
	/// <remarks>
	/// Любой <c>MonoBehaviour</c>, реализующий этот интерфейс, будет просканирован
	/// инжектором <see cref="Injector"/>, и его
	/// методы, помеченные атрибутом <see cref="ProvideAttribute"/>,
	/// будут вызваны для регистрации возвращаемых значений в реестре зависимостей.
	///
	/// Реализация интерфейса не требует добавления методов — интерфейс служит только
	/// для обнаружения провайдеров при инициализации.
	/// </remarks>
	public interface IDependencyProvider
	{

	}
}