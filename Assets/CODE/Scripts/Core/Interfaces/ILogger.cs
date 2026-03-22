using CODE.Scripts.Core.Structs;

namespace CODE.Scripts.Core.Interfaces
{
	public interface ILogger
	{
		void Start();

		void Log(LogMessage message);

		void Stop();
	}
}