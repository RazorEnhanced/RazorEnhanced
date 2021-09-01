using System.Collections.Generic;
using System.IO;

namespace Assistant
{
	public class UOEntity
	{
		private Serial m_Serial;
		//private Point3D m_Pos;
		private ushort m_Hue;
		private bool m_Deleted;
		private readonly Dictionary<ushort, int> m_ContextMenu = new Dictionary<ushort, int>();
		protected ObjectPropertyList m_ObjPropList = null;

		internal ObjectPropertyList ObjPropList { get { return m_ObjPropList; } }


		internal UOEntity(Serial ser)
		{
			m_ObjPropList = new ObjectPropertyList(this);

			m_Serial = ser;
			m_Deleted = false;
		}

		internal Serial Serial { get { return m_Serial; } }

        private Point3D dont_use_pos;

        internal virtual Point3D Position
		{
            get { return dont_use_pos; }
			set
			{
                if (m_Serial == 0x2247A && dont_use_pos.Z != 48)
                {
                    // stop
                }

                if (value != dont_use_pos)
				{
					OnPositionChanging(value);
                    dont_use_pos = value;
				}
			}
		}

		internal bool Deleted
		{
			get
			{
				return m_Deleted;
			}
            set
            {
                m_Deleted = value;
            }

        }

        internal Dictionary<ushort, int> ContextMenu
		{
			get { return m_ContextMenu; }
		}

		internal virtual ushort Hue
		{
			get { return m_Hue; }
			set { m_Hue = value; }
		}

		internal virtual void Remove()
		{
			m_Deleted = true;
		}

		internal virtual void OnPositionChanging(Point3D newPos)
		{
		}

		public override int GetHashCode()
		{
			return m_Serial.GetHashCode();
		}

		virtual internal void ReadPropertyList(PacketReader p)
		{
			m_ObjPropList.Read(p);
		}
	}
}
