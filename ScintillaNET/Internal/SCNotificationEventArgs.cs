#region Using Directives

using System;

#endregion Using Directives

namespace ScintillaNET.Internal
{
	//
	// EventArgs for the SCNotification event
	//
	internal sealed class SCNotificationEventArgs : EventArgs
	{
		#region Fields

		private NativeMethods.SCNotification _notification;

		#endregion Fields

		#region Propeties

		public NativeMethods.SCNotification Notification
		{
			get
			{
				return _notification;
			}
		}

		#endregion Propeties

		#region Constructors

		public SCNotificationEventArgs(NativeMethods.SCNotification notification)
		{
			_notification = notification;
		}

		#endregion Constructors
	}
}