namespace RazorEnhanced
{
	public class Property
	{
		private Assistant.ObjectPropertyList.OPLEntry m_OPLEntry;

		internal Property(Assistant.ObjectPropertyList.OPLEntry entry)
		{
			m_OPLEntry = entry;
		}

		public int Number { get { return m_OPLEntry.Number; } }
		public string Args { get { return m_OPLEntry.Args; } }

		public override string ToString()
		{
			return m_OPLEntry.ToString();
		}
	}
}