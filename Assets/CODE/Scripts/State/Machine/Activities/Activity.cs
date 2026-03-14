using System.Threading;
using System.Threading.Tasks;
using CODE.Scripts.State.Machine.Enums;
using CODE.Scripts.State.Machine.Interfaces;
using UnityEngine;

namespace CODE.Scripts.State.Machine.Activities
{
	public abstract class Activity : IActivity
	{
		public ActiveMode Mode { get; protected set; } = ActiveMode.Inactive;

		public virtual async Task ActivateAsync(CancellationToken token)
		{
			if (Mode != ActiveMode.Inactive)
			{
				return;
			}

			Mode = ActiveMode.Activating;
			await Task.CompletedTask;
			Mode = ActiveMode.Active;

			Debug.Log($"Activated {GetType().Name} (mode={Mode})");
		}

		public virtual async Task DeactivateAsync(CancellationToken token)
		{
			if (Mode != ActiveMode.Active)
			{
				return;
			}

			Mode = ActiveMode.Deactivating;
			await Task.CompletedTask;
			Mode = ActiveMode.Inactive;

			Debug.Log($"Deactivated {GetType().Name} (mode={Mode})");
		}
	}
}