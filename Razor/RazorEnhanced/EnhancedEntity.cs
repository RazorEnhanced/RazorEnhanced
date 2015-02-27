using System;
using System.IO;
using System.Collections.Generic;
using Assistant;

namespace RazorEnhanced
{
	public class EnhancedEntity
	{	
		private  UOEntity m_UOEntity;

		internal EnhancedEntity(UOEntity entity)
		{
			m_UOEntity = entity;
		}

		internal Serial Serial { get { return m_UOEntity.Serial; } }

		public virtual Point3D Position { get { return new RazorEnhanced.Point3D(m_UOEntity.Position); } }


		public bool Deleted
		{
			get
			{
				return m_UOEntity.Deleted;
			}
		}

		public Dictionary<ushort, ushort> ContextMenu
		{
			get { return m_UOEntity.ContextMenu; }
		}

		public virtual ushort Hue
		{
			get{ return m_UOEntity.Hue; }
			set{ m_UOEntity.Hue = value; }
		}
	}
}
