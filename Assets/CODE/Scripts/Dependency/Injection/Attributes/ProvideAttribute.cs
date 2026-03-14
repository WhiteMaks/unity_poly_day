using System;

namespace CODE.Scripts.Dependency.Injection.Attributes
{
	/// <summary>
	/// Атрибут, помечающий метод провайдера как источник зависимости.
	/// </summary>
	/// <remarks>
	/// Методы, помеченные этим атрибутом, вызываются инжектором при инициализации.
	/// Возвращаемый тип метода используется как ключ в реестре зависимостей
	/// (тип -> экземпляр). Метод должен возвращать готовый объект (или фабрику),
	/// а не <c>null</c> — инжектор выбросит исключение при получении <c>null</c>.
	///
	/// Пример:
	/// <code>
	/// [Provide]
	/// public MyService CreateService() { return new MyService(); }
	/// </code>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ProvideAttribute : Attribute
	{

	}
}