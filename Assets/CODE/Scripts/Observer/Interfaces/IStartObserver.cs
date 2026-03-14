namespace CODE.Scripts.Observer.Interfaces
{
	/// <summary>
	/// Интерфейс для объектов, которые должны получать один раз вызов инициализации
	/// через менеджер <c>StartSceneManager</c>.
	/// </summary>
	/// <remarks>
	/// Метод <see cref="CoreStart"/> используется для одноразовой инициализации после
	/// загрузки сцены или активации компонента. В отличие от <c>Update</c>/<c>FixedUpdate</c>/<c>LateUpdate</c>,
	/// этот метод вызывается один раз и не предназначен для выполнения логики каждый кадр.
	///
	/// Рекомендации:
	/// - Регистрируйте подписчиков в <c>OnEnable</c> и удаляйте в <c>OnDisable</c>.
	/// - Поскольку <c>Start</c> менеджера вызывается Unity один раз, объекты, зарегистрированные
	///   после выполнения <c>Start</c>, не получат вызов <c>CoreStart</c> автоматически.
	/// </remarks>
	public interface IStartObserver
	{
		/// <summary>
		/// Метод одноразовой инициализации, вызываемый менеджером <c>StartSceneManager</c>.
		/// </summary>
		void CoreStart();
	}
}