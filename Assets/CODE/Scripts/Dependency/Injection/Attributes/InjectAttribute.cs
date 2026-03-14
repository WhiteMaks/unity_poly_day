using System;

namespace CODE.Scripts.Dependency.Injection.Attributes
{
	/// <summary>
	/// Атрибут, помечающий поле или метод для внедрения зависимости инжектором.
	/// </summary>
	/// <remarks>
	/// Если атрибут применён к полю, инжектор попытается найти в реестре экземпляр
	/// соответствующего типа и установить значение поля через отражение.
	/// Атрибут может также предназначаться для будущего расширения логики внедрения в методы.
	///
	/// Пример использования:
	/// <code>
	/// [Inject]
	/// private MyService _service;
	/// </code>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
	public sealed class InjectAttribute : Attribute
	{

	}
}