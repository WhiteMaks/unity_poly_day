using CODE.Scripts.Core;
using CODE.Scripts.State.Machine;

namespace CODE.Scripts.Character.States
{
	public abstract class CharacterBaseState : BaseState
	{
		protected readonly CharacterContext Context;
		protected readonly EventBus EventBus;

		protected CharacterBaseState(StateMachine stateMachine, BaseState parent, CharacterContext context, EventBus eventBus) : base(stateMachine, parent)
		{
			Context = context;
			EventBus = eventBus;
		}
	}
}