﻿using System.Dynamic;

namespace Fluxor.Blazor.Web.ReduxDevTools.Internal.CallbackObjects
{
	public class JumpToStateCallback : BaseCallbackObject<JumpToStatePayload>
	{
#pragma warning disable IDE1006 // Naming Styles
		public string state { get; set; }
#pragma warning restore IDE1006 // Naming Styles
	}
}
