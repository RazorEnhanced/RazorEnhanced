#region Using Directives

using System.Collections.Generic;

#endregion Using Directives

namespace ScintillaNET.Configuration
{
	public class CommandBindingConfigList : List<CommandBindingConfig>
	{
		#region Fields

		private bool? _allowDuplicateBindings;
		private bool? _inherit;

		#endregion Fields

		#region Properties

		public bool? AllowDuplicateBindings
		{
			get
			{
				return _allowDuplicateBindings;
			}
			set
			{
				_allowDuplicateBindings = value;
			}
		}

		public bool? Inherit
		{
			get
			{
				return _inherit;
			}
			set
			{
				_inherit = value;
			}
		}

		#endregion Properties
	}
}