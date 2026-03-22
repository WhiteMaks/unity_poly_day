using CODE.Scripts.Dependency.Injection;
using CODE.Scripts.Dependency.Injection.Attributes;
using CODE.Scripts.Observer;
using CODE.Scripts.Observer.Interfaces;
using CODE.Scripts.Services;

namespace CODE.Scripts.Bootstraps
{
	public class GameBootstrap : Provider, IStartObserver
	{
		[Inject] private LoggerService _logger;

		[Provide]
		public LoggerService ProvideLoggerService()
		{
			return new LoggerService();
		}

		public void CoreStart()
		{
			_logger.Start();
			_logger.Trace("Logger ready to use.");
		}

		private void Awake()
		{
			StartSceneManager.Add(this);
		}

		private void OnDestroy()
		{
			_logger.Trace("Logger stopping.");
			_logger.Stop();
			StartSceneManager.Remove(this);
		}
	}
}