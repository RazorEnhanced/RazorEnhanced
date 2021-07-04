using Assistant;

namespace RazorEnhanced
{
	internal struct Serial
	{
		private Assistant.Serial m_AssistantSerial;

		internal Serial(Assistant.Serial serial)
		{
			m_AssistantSerial = serial;
		}

		public int Value { get { return (int)m_AssistantSerial.Value; } }
	}

	public class EnhancedEntity
	{
		private UOEntity m_UOEntity;
		private Serial m_Serial;

		internal EnhancedEntity(UOEntity entity)
		{
			if (entity == null)
				m_Serial = new Serial(0);
			else
				m_Serial = new Serial(entity.Serial);
			m_UOEntity = entity;
		}

		public int Serial { get { return m_Serial.Value; } }

		public virtual Point3D Position
		{
			get
			{
				if (m_UOEntity == null)
					return new RazorEnhanced.Point3D(0,0,0);
				else
					return new RazorEnhanced.Point3D(m_UOEntity.Position);

			}
		}

		public bool Deleted
		{
			get
			{
				if (m_UOEntity == null)
					return true;
				else
					return m_UOEntity.Deleted;
			}
		}

		public virtual int Hue
		{
			get
			{
				if (m_UOEntity == null)
					return 0;
				else
					return m_UOEntity.Hue;
			}
		}
	}
}