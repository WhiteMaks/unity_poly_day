using System.Collections.Generic;
using System.Reflection;

namespace CODE.Scripts.State.Machine
{
	/// <summary>
	/// Утильный класс для создания и «подключения» (wiring) дерева состояний к экземпляру <see cref="StateMachine"/>.
	/// </summary>
	/// <remarks>
	/// При вызове <see cref="Build"/> создаётся объект <see cref="StateMachine"/>, а затем рекурсивно
	/// проходят все поля типа <see cref="BaseState"/> в переданном корневом состоянии и устанавливают ссылку
	/// на созданную машину состояний. Это позволяет не передавать вручную экземпляр StateMachine в каждом состоянии.
	/// </remarks>
	public class StateMachineBuilder
	{
		private readonly BaseState _rootState;

		/// <summary>
		/// Создаёт билдера для указанного корневого состояния.
		/// </summary>
		/// <param name="rootState">Корневое состояние всего дерева состояний.</param>
		public StateMachineBuilder(BaseState rootState)
		{
			_rootState = rootState;
		}

		/// <summary>
		/// Создаёт новый экземпляр <see cref="StateMachine"/>, привязывает его к корневому состоянию
		/// и рекурсивно проводит "проводку" ссылок на StateMachine для всех дочерних состояний.
		/// </summary>
		/// <returns>Настроенный экземпляр <see cref="StateMachine"/>.</returns>
		public StateMachine Build()
		{
			var result = new StateMachine(_rootState);

			Wire(_rootState, result, new HashSet<BaseState>());

			return result;
		}

		/// <summary>
		/// Рекурсивно перебирает поля типа <see cref="BaseState"/> в указанном состоянии и устанавливает
		/// ссылку на машину состояний, а также обходит дочерние состояния. Обрабатывает защищенные и приватные поля.
		/// </summary>
		/// <param name="state">Текущее состояние, которое нужно "проводить".</param>
		/// <param name="stateMachine">Экземпляр машины состояний для установки в поля состояний.</param>
		/// <param name="visited">Набор уже посещённых состояний для предотвращения циклов.</param>
		private static void Wire(BaseState state, StateMachine stateMachine, HashSet<BaseState> visited)
		{
			if (state == null)
			{
				return;
			}

			if (!visited.Add(state))
			{
				return;
			}

			const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

			var stateMachineManagerField = state.GetType().GetField("StateMachine", flags);
			if (stateMachineManagerField != null)
			{
				stateMachineManagerField.SetValue(state, stateMachine);
			}

			foreach (var field in state.GetType().GetFields(flags))
			{
				if (!typeof(BaseState).IsAssignableFrom(field.FieldType))
				{
					continue;
				}

				if (field.Name == "Parent")
				{
					continue;
				}

				var childState = (BaseState) field.GetValue(state);
				if (childState == null)
				{
					continue;
				}

				if (!ReferenceEquals(childState.GetParent(), state))
				{
					continue;
				}

				Wire(childState, stateMachine, visited);
			}
		}
	}
}