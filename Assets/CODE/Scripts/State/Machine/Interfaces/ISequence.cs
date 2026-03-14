using System.Threading;
using System.Threading.Tasks;

namespace CODE.Scripts.State.Machine.Interfaces
{
	public interface ISequence
	{
		bool IsDone { get; }

		void Start();
		bool Update();
	}

	public delegate Task PhaseStep(CancellationToken token);
}