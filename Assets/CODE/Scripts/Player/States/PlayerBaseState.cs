using CODE.Scripts.Character;
using CODE.Scripts.State.Machine;

namespace CODE.Scripts.Player.States
{
	public abstract class PlayerBaseState : BaseState
	{
		protected readonly CharacterContext _context;

		protected PlayerBaseState(StateMachine stateMachine, BaseState parent, CharacterContext context) : base(stateMachine, parent)
		{
			_context = context;
		}
	}
}