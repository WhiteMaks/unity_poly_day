using System.Threading;
using System.Threading.Tasks;
using CODE.Scripts.State.Machine.Enums;

namespace CODE.Scripts.State.Machine.Interfaces
{
	public interface IActivity
	{
		ActiveMode Mode { get; }

		Task ActivateAsync(CancellationToken token);
		Task DeactivateAsync(CancellationToken token);
	}
}