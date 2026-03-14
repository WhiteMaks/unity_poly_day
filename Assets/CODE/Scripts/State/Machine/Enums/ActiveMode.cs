namespace CODE.Scripts.State.Machine.Enums
{
	/// <summary>
	/// Состояние активности для объектов <see cref="CODE.Scripts.State.Machine.Interfaces.IActivity"/>.
	/// </summary>
	public enum ActiveMode
	{
		/// <summary>
		/// Активность неактивна и готова к активации.
		/// </summary>
		Inactive,

		/// <summary>
		/// Процесс активации запущен (ожидание завершения).
		/// </summary>
		Activating,

		/// <summary>
		/// Активность активна и функционирует.
		/// </summary>
		Active,

		/// <summary>
		/// Процесс деактивации запущен (ожидание завершения).
		/// </summary>
		Deactivating
	}
}