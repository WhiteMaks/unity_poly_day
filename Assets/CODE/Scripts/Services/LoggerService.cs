using System;
using System.Diagnostics;
using System.Threading;
using CODE.Scripts.Core;
using CODE.Scripts.Core.Configs;
using CODE.Scripts.Core.Enums;
using CODE.Scripts.Core.Interfaces;
using CODE.Scripts.Core.Structs;

namespace CODE.Scripts.Services
{
	public class LoggerService
	{
		private readonly LoggerConfig _config;
		private readonly ILogger _logger;

		public LoggerService()
		{
			_config = new LoggerConfig
			{
				Level = LogLevel.Trace
			};

			_logger = new AsyncLogger(_config);
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public void Start()
		{
			_logger.Start();
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public void Trace(string message)
		{
			Log(LogLevel.Trace, message);
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public void Debug(string message)
		{
			Log(LogLevel.Debug, message);
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public void Info(string message)
		{
			Log(LogLevel.Info, message);
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public void Warn(string message)
		{
			Log(LogLevel.Warn, message);
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public void Error(string message)
		{
			Log(LogLevel.Error, message);
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		public void Stop()
		{
			_logger.Stop();
		}

		private void Log(LogLevel level, string message)
		{
			if (level < _config.Level)
			{
				return;
			}

			var logMessage = new LogMessage
			{
				Time = DateTime.Now,
				Level = level,
				ClassName = GetCaller(),
				ThreadId = Thread.CurrentThread.ManagedThreadId,
				Message = message
			};

			_logger.Log(logMessage);
		}

		private static string GetCaller()
		{
			var stack = new StackTrace();

			for (var i = 2; i < stack.FrameCount; i++)
			{
				var method = stack.GetFrame(i).GetMethod();
				var type = method.DeclaringType;

				if (type != typeof(LoggerService) && type != null)
				{
					return type.Name;
				}
			}

			return "Unknown";
		}
	}
}