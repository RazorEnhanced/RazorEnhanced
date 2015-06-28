using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Assistant
{
    internal abstract class Agent
    {
        private static List<Agent> m_List = new List<Agent>();
        internal static List<Agent> List { get { return m_List; } }

        internal delegate void ItemCreatedEventHandler(Item item);
        internal delegate void MobileCreatedEventHandler(Mobile m);
        internal static event ItemCreatedEventHandler OnItemCreated;
        internal static event MobileCreatedEventHandler OnMobileCreated;

        internal static void InvokeMobileCreated(Mobile m)
        {
            if (OnMobileCreated != null)
                OnMobileCreated(m);
        }

        internal static void InvokeItemCreated(Item i)
        {
            if (OnItemCreated != null)
                OnItemCreated(i);
        }

        internal static void Add(Agent a)
        {
            m_List.Add(a);
        }

    }

}

