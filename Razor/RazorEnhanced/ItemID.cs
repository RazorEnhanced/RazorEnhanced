namespace RazorEnhanced
{
	public struct ItemID
	{
		private Assistant.ItemID m_AssistantItemID;

		internal ItemID(Assistant.ItemID itemID)
		{
			m_AssistantItemID = itemID;
		}

		public ItemID(int id)
		{
			m_AssistantItemID = new Assistant.ItemID((ushort)id);
		}

		public int Value { get { return m_AssistantItemID.Value; } }

		public override string ToString()
		{
			return m_AssistantItemID.ToString();
		}

		public Ultima.ItemData ItemData { get { return m_AssistantItemID.ItemData; } }
	}
}