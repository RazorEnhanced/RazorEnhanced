using System.Collections.Generic;

namespace Assistant
{
    public class UOEntity
    {
        private Serial m_Serial;
        //private Point3D m_Pos;
        private ushort m_TypeID;
        private ushort m_Hue;
        private bool m_Deleted;
        private readonly Dictionary<ushort, int> m_ContextMenu = new();
        protected ObjectPropertyList m_ObjPropList = null;

        internal ObjectPropertyList ObjPropList { get { return m_ObjPropList; } }


        internal UOEntity(Serial ser)
        {
            m_ObjPropList = new ObjectPropertyList(this);

            m_Serial = ser;
            m_Deleted = false;
        }

        internal Serial Serial { get { return m_Serial.Value; } }
        internal Point3D entityPosition;

        //[PythonHidden]
        public virtual Point3D Position
        {
            get { return entityPosition; }
            set
            {
                if (value != entityPosition)
                {
                    OnPositionChanging(value);
                    entityPosition = value;
                }
            }
        }

        public bool Deleted
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

        internal ushort Body
        {
            get { return TypeID; }
        }

        internal virtual Assistant.TypeID TypeID
        {
            get { return m_TypeID; }
            set { m_TypeID = value; }
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
        public override bool Equals(object obj)
        {
            return Equals(obj as UOEntity);
        }

        public bool Equals(UOEntity entity)
        {
            return entity != null && Serial == entity.Serial;
        }


        virtual internal void ReadPropertyList(PacketReader p)
        {
            m_ObjPropList.Read(p);
        }
    }

    // All this weirdness is so that the Position gets refreshed every time its asked for
    // Otherwise python caches the position, and the value returned is always the same
    // Removed for now
    /*
    public class PlayerDynamicMetaObjectProvider : DynamicMetaObject
    {
        private UOEntity _entity;

        public PlayerDynamicMetaObjectProvider(UOEntity entity)
            : base(Expression.Constant(entity), BindingRestrictions.Empty, entity)
        {
            _entity = entity;
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            if (binder.Name == "Position")
            {
                Expression getPositionExpression = Expression.Property(
                    Expression.Constant(_entity),
                    typeof(Player).GetProperty("Position"));

                return new DynamicMetaObject(getPositionExpression, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            }

            return base.BindGetMember(binder);
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            if (binder.Name == "Position")
            {
                Expression setPositionExpression = Expression.Assign(
                    Expression.Property(Expression.Constant(_entity), "Position"),
                    Expression.Convert(value.Expression, typeof(Point3D)));

                return new DynamicMetaObject(setPositionExpression, BindingRestrictions.GetTypeRestriction(Expression, LimitType));
            }

            return base.BindSetMember(binder, value);
        }
    }
    */
}
