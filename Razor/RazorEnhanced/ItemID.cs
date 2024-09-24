namespace RazorEnhanced
{
    public struct ItemID
    {
        private Assistant.TypeID m_AssistantItemID;

        internal ItemID(Assistant.TypeID itemID)
        {
            m_AssistantItemID = itemID;
        }

        public ItemID(int id)
        {
            m_AssistantItemID = new Assistant.TypeID((ushort)id);
        }

        public int Value { get { return m_AssistantItemID.Value; } }

        public override string ToString()
        {
            return m_AssistantItemID.ToString();
        }

        public Ultima.ItemData ItemData { get { return m_AssistantItemID.ItemData; } }
    }
}