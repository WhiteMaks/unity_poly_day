using Code.Scripts.Managers.Character;

namespace Code.Scripts.Managers.Player
{
	public class PlayerManager : CharacterManager
	{
		private PlayerLocomotionManager _locomotionManager;

		protected override void Awake()
		{
			base.Awake();

			_locomotionManager = GetComponent<PlayerLocomotionManager>();
		}

		protected override void Update()
		{
			base.Update();

			_locomotionManager.HandleMovement();
		}
	}
}
