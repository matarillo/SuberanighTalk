#region using
using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.IO;

#endregion

namespace SharpHsql
{
	/// <summary>
	/// Provides static methods that supply helper utilities for logging data whith the ILog object. 
	/// This class cannot be inherited. 
	/// </summary>
	/// <author>Andrés G Vettori</author>
	sealed class LogHelper
	{
		#region Constants

		//log4net XML constants.
		
		/// <summary>
		/// TargetNamespace.
		/// </summary>
		public const string TargetNamespace = "http://log4net.sourceforge.net";

		/// <summary>
		/// DefaultPrefix.
		/// </summary>
		public const string DefaultPrefix = "log4net";

		/// <summary>
		/// log.
		/// </summary>
		public const string RootElement = "log";
		
		private const string newLine = "\n\r";

		#endregion Constants

		#region Enums
		/// <summary>
		/// Specifies the event type of an log entry.
		/// </summary>
		/// <remarks>
		/// The type of an log entry is used to indicate the severity of a log entry.
		/// Each log must be of a single type, which the application indicates when it reports the log.
		/// </remarks>
		public enum LogEntryType
		{
			/// <summary>
			/// An audit log. This indicates a successful audit.
			/// </summary>
			Audit,
			/// <summary>
			/// A debug log. This is for testing and debugging operations.
			/// </summary>
			Debug,
			/// <summary>
			/// An information log. This indicates a significant, successful operation.
			/// </summary>
			Information,
			/// <summary>
			/// A warning log. 
			/// This indicates a problem that is not immediately significant, 
			/// but that may signify conditions that could cause future problems.
			/// </summary>
			Warning,
			/// <summary>
			/// An error log. 
			/// This indicates a significant problem the user should know about; 
			/// usually a loss of functionality or data.
			/// </summary>
			Error,
			/// <summary>
			/// An fatal log. 
			/// This indicates a fatal problem the user should know about; 
			/// allways a loss of functionality or data.
			/// </summary>
			Fatal
		}
		#endregion

		#region Private utility methods & constructors

		//Since this class provides only static methods, make the default constructor private to prevent 
		//instances from being created with "new LogHelper()".
		private LogHelper() {}

		static LogHelper()
		{
		}

		private static string InternalFormattedMessage(string message, Exception exception, Assembly assembly)
		{
			return InternalFormattedMessage(message, exception, assembly, true);
		}

		private static string InternalFormattedMessage(string message, Exception exception, Assembly assembly, bool showStack)
		{
			const string TEXT_SEPARATOR = "*********************************************";			

			// Create StringBuilder to maintain publishing information.
			StringBuilder strInfo = new StringBuilder(String.Concat(newLine, newLine, message, newLine, newLine));

			try
			{
				if (exception != null)
				{
					#region Loop through each exception class in the chain of exception objects
					// Loop through each exception class in the chain of exception objects.
				
					if(message == null) message = exception.Message;

					Exception currentException = exception; // Temp variable to hold BaseApplicationException object during the loop.

					int intExceptionCount = 1;// Count variable to track the number of exceptions in the chain.
					
					do
					{
						// Write title information for the exception object.
						strInfo.AppendFormat(null, "{1}) Exception Information{0}{2}", newLine, intExceptionCount.ToString(), TEXT_SEPARATOR);
						strInfo.AppendFormat(null, "{0}Exception Type: {1}", newLine, currentException.GetType().FullName);
				
						#region Loop through the public properties of the exception object and record their value
						// Loop through the public properties of the exception object and record their value.
						PropertyInfo[] aryPublicProperties = currentException.GetType().GetProperties();
						foreach (PropertyInfo p in aryPublicProperties)
						{
							// Do not log information for the InnerException or StackTrace. This information is 
							// captured later in the process.
							if (!p.Name.Equals("InnerException") && !p.Name.Equals("StackTrace") && !p.Name.Equals("BaseInnerException"))
							{
								object prop = null;
								try
								{
									prop = p.GetValue(currentException,null);
								}
								catch(TargetInvocationException) {}

								if (prop == null)
								{
									strInfo.AppendFormat(null, "{0}{1}: NULL", newLine, p.Name);
								}
								else
								{
									strInfo.AppendFormat(null, "{0}{1}: {2}", newLine, p.Name, p.GetValue(currentException,null));
								}
							}
						}
						#endregion

						#region Record the Exception StackTrace

						#endregion

						strInfo.AppendFormat(null, "{0}{0}", newLine);

						// Reset the temp exception object and iterate the counter.
						currentException = currentException.InnerException;
						intExceptionCount++;
					} while (currentException != null);
					#endregion
				}

				strInfo.AppendFormat(null, "{1}Assembly version: {0}", 
					assembly.GetName().Version.ToString(), newLine);
			}
			catch (Exception ex)
			{
				strInfo.AppendFormat(null, "{0}{0}Exception in PublishException:{4}{0}{1}{0}Original message:{0}{2}{0}Original Exception:{0}{3}", newLine, ex.Message, message, exception.Message, TEXT_SEPARATOR);
			}
			return strInfo.ToString();
		}

		private static void PublishInternal(string message, Exception exception, LogEntryType exceptionTpe)
		{
		}

		#endregion

		#region Public members

		#region Publish
		/// <summary>
		/// Write Exception Info to the ILog interface.
		/// </summary>
		/// <remarks>
		/// For Debugging or Information uses, its faster to use ILog 
		/// interface directly, instead of this method. 
		/// </remarks>
		/// <param name="message">Additional exception info.</param>
		public static void Publish(string message)
		{
			PublishInternal(message, null, LogEntryType.Information);
		}

		/// <summary>
		/// Write Exception Info to the ILog interface.
		/// </summary>
		/// <param name="exception">Exception object.</param>
		public static void Publish(Exception exception)
		{
			PublishInternal(null, exception, LogEntryType.Error);
		}

		/// <summary>
		/// Write Exception Info to the ILog interface.
		/// </summary>
		/// <param name="exception">Exception object.</param>
		/// <param name="exceptionTpe">See <see cref="LogEntryType"/>.</param>
		public static void Publish(Exception exception, LogEntryType exceptionTpe)
		{
			PublishInternal(null, exception, exceptionTpe);
		}

		/// <summary>
		/// Write Exception Info to the ILog interface.
		/// </summary>
		/// <remarks>
		/// For Debugging or Information uses, its faster to use ILog 
		/// interface directly, instead of this method. 
		/// </remarks>
		/// <param name="message">Additional exception info.</param>
		/// <param name="exceptionTpe">See <see cref="LogEntryType"/>.</param>
		public static void Publish(string message, LogEntryType exceptionTpe)
		{
			PublishInternal(message, null, exceptionTpe);
		}

		/// <summary>
		/// Write Exception Info to the ILog interface.
		/// </summary>
		/// <param name="message">Additional exception info.</param>
		/// <param name="exception">Exception object.</param>
		public static void Publish(string message, Exception exception)
		{
			PublishInternal(message, exception, LogEntryType.Error);
		}

		/// <summary>
		/// Write Exception Info to the ILog interface.
		/// </summary>
		/// <param name="message">Additional exception info.</param>
		/// <param name="exception">Exception object.</param>
		/// <param name="exceptionTpe">See <see cref="LogEntryType"/>.</param>
		public static void Publish(string message, Exception exception, LogEntryType exceptionTpe)
		{ 
			PublishInternal(message, exception, exceptionTpe);
		}
		
		#endregion

		#region Logger

		#endregion

		#region FormattedMessage
		/// <summary>
		/// Gets the Exception Info to be writen to the Log.
		/// </summary>
		/// <param name="exception">Exception object.</param>
		public static string FormattedMessage(Exception exception)
		{
			return InternalFormattedMessage(null, exception, Assembly.GetCallingAssembly());
		}

		/// <summary>
		/// Gets the Exception Info to be writen to the Log.
		/// </summary>
		/// <param name="exception">Exception object.</param>
		/// <param name="showStack">True, show all the stack trace inf.</param>
		public static string FormattedMessage(Exception exception, bool showStack)
		{
			return InternalFormattedMessage(null, exception, Assembly.GetCallingAssembly(), showStack);
		}

		/// <summary>
		/// Gets the Exception Info to be writen to the Log.
		/// </summary>
		/// <param name="message">Additional exception info.</param>
		/// <param name="exception">Exception object.</param>
		public static string FormattedMessage(string message, Exception exception)
		{
			return InternalFormattedMessage(message, exception, Assembly.GetCallingAssembly());
		}

		/// <summary>
		/// Gets the Exception Info to be writen to the Log.
		/// </summary>
		/// <param name="message">Additional exception info.</param>
		/// <param name="exception">Exception object.</param>
		/// <param name="showStack">True, show all the stack trace inf.</param>
		public static string FormattedMessage(string message, Exception exception, bool showStack)
		{
			return InternalFormattedMessage(message, exception, Assembly.GetCallingAssembly(), showStack);
		}

		#endregion

		#endregion
	}
}