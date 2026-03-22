using System.Collections.Concurrent;
using System.Threading;
using CODE.Scripts.Core.Configs;
using CODE.Scripts.Core.Enums;
using CODE.Scripts.Core.Interfaces;
using CODE.Scripts.Core.Structs;

namespace CODE.Scripts.Core
{
	public class AsyncLogger : ILogger
	{
		private readonly ConcurrentQueue<LogMessage> _queue = new();
		private readonly Thread _thread;
		private readonly LoggerConfig _config;

		private bool _running;

		public AsyncLogger(LoggerConfig config)
		{
			_config = config;

			_thread = new Thread(ProcessQueue)
			{
				IsBackground = true
			};
		}

		public void Start()
		{
			_running = true;
			_thread.Start();
		}

		public void Log(LogMessage message)
		{
			_queue.Enqueue(message);
		}

		public void Stop()
		{
			_running = false;
			_thread.Join();
		}

		private void ProcessQueue()
		{
			while (_running)
			{
				while (_queue.TryDequeue(out var message))
				{
					Write(message);
				}
			}
		}

		private void Write(LogMessage message)
		{
			var formatted = $"[{message.Time:HH:mm:ss.fff}] [{message.Level}] [T:{message.ThreadId}] [{message.ClassName}] {message.Message}";

			switch (message.Level)
			{
				case LogLevel.Warn:
					UnityEngine.Debug.LogWarning(formatted);
					break;

				case LogLevel.Error:
					UnityEngine.Debug.LogError(formatted);
					break;

				default:
					UnityEngine.Debug.Log(formatted);
					break;
			}
		}
	}
}