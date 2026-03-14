using System;
using System.Threading;
using System.Threading.Tasks;

namespace CODE.Scripts.State.Machine.Activities
{
	public class DelayActivationActivity : Activity
	{
		private readonly float _seconds;

		public DelayActivationActivity(float seconds)
		{
			_seconds = seconds;
		}

		public override async Task ActivateAsync(CancellationToken token)
		{
			await Task.Delay(TimeSpan.FromSeconds(_seconds), token);
			await base.ActivateAsync(token);
		}
	}
}