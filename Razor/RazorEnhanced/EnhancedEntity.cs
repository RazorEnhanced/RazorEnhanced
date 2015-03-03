using System;
using System.IO;
using System.Collections.Generic;
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
			m_Serial = new Serial(entity.Serial);
			m_UOEntity = entity;
		}

		public int Serial { get { return m_Serial.Value; } }

		public virtual Point3D Position { get { return new RazorEnhanced.Point3D(m_UOEntity.Position); } }

		public bool Deleted
		{
			get
			{
				return m_UOEntity.Deleted;
			}
		}

		public virtual int Hue
		{
			get { return m_UOEntity.Hue; }
		}
	}
}
