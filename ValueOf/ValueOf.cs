using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ValueOf
{
    public abstract class ValueOf<TValue, TOwner>: IComparable
        where TOwner: ValueOf<TValue, TOwner>
    {
        private readonly TValue _value;
        public TValue Value => _value;
        protected ValueOf(TValue value)
        {
            _value = value;
        }

        public abstract Task<bool> Validate(TValue value);
        public abstract Task<Exception> OnInValid(TValue value);

        protected virtual bool Equals(ValueOf<TValue, TOwner> other)
        {
            return _value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((ValueOf<TValue, TOwner>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TValue>.Default.GetHashCode(_value);
        }
        public override string ToString()
        {
            return _value.ToString();
        }

        public int CompareTo(object obj)
        {
            if (obj is TOwner other)
            {
                return CompareComponents(this.Value, other.Value);
            }

            return -1;
        }

        private int CompareComponents(object object1, object object2)
        {
            if (object1 is null && object2 is null)
                return 0;

            if (object1 is null)
                return -1;

            if (object2 is null)
                return 1;

            if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
                return comparable1.CompareTo(comparable2);

            return object1.Equals(object2) ? 0 : -1;
        }

    }
}
