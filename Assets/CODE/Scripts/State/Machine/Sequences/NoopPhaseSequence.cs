using CODE.Scripts.State.Machine.Interfaces;

namespace CODE.Scripts.State.Machine.Sequences
{
	/// <summary>
	/// Пустая последовательность, которая сразу помечается как завершённая.
	/// Полезна как заглушка при отсутствии шагов.
	/// </summary>
	public class NoopPhaseSequence : ISequence
	{
		/// <summary>
		/// Признак завершённости — всегда true после Start.
		/// </summary>
		public bool IsDone { get; private set; }

		/// <summary>
		/// Устанавливает IsDone = true.
		/// </summary>
		public void Start()
		{
			IsDone = true;
		}

		/// <summary>
		/// Возвращает значение IsDone (true после Start).
		/// </summary>
		/// <returns>True, если последовательность завершена.</returns>
		public bool Update()
		{
			return IsDone;
		}
	}
}