using System;
using CODE.Scripts.Core.Enums;

namespace CODE.Scripts.Core.Structs
{
	public struct LogMessage
	{
		public DateTime Time;
		public LogLevel Level;
		public string ClassName;
		public string Message;
		public int ThreadId;
	}
}