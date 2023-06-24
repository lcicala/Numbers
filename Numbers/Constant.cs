using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numbers
{
    public class Constant : RealNumberMid
    {
        private double _value;
        private string _name;

        public Constant(double value, RealNumber multiplier = null!, string name = null!)
        {
            _value = value;
            _name = name;
            _multiplier = multiplier ?? 1;
        }

        public override string ToString()
        {
            if(_multiplier == 1)
                return _name ?? _value.ToString();
            if(_multiplier is not null && _name is not null)
                return string.Concat(_multiplier?.ToString() ?? "", " * ", _name);
            else
                return _name ?? _value.ToString();
        }

        public static explicit operator double(Constant c)
        {
            return c._value;
        }

        public override IEnumerator<RealNumber> GetEnumerator()
        {
            yield return this;
        }

        protected override double CastToDouble()
        {
            if (_multiplier is null)
                return _value;
            return _value * (double)(_multiplier);
        }

        protected override RealNumber Sum(RealNumber b)
        {
            if (b is Constant c)
            {
                if (this._name == null && c._name == null)
                    return new Constant(_value + c._value);
                else if (_name == c._name)
                    return new Constant(_value, _multiplier + c._multiplier, _name);
                else if (_multiplier is Constant c1 && c._multiplier is Constant c2 && (c1._name == c._name && _name == c2._name))
                    return new Constant(_value, c1 + new Constant(c._value, 1, c._name), _name);
                else
                    return CreateInstance(1, this, c);
            }
            else
            {
                //List<RealNumber> l = new List<RealNumber>();
                //foreach (var n in b)
                //{
                //    l.Add(Sum(n));
                //}
                //return CreateInstance(l.ToArray());
                return CreateInstance(1, this, b);
            }
        }

        protected override RealNumber Product(RealNumber b)
        {
            if (b is Constant c)
            {
                if (this._name == null && c._name == null)
                    return new Constant(_value * c._value, _multiplier*c._multiplier);
                else if (_name == c._name)
                    return new Radical(this, 2, _multiplier*c._multiplier);
                else
                    return new Constant(_value, c, _name);
            }
            else if(b is NaturalNumber n)
            {
                return new Constant(_value, _multiplier * n, _name);
            }
            else
            {
                if(b is Radical)
                    return b * this;
                //List<RealNumber> l = new List<RealNumber>();
                //foreach (var n in b)
                //{
                //    l.Add(Sum(n));
                //}
                //return CreateInstance(l.ToArray());
                return CreateInstance(this, b);
            }
        }

        protected override RealNumber Division(RealNumber b)
        {
            return new Fraction(this, b);
        }

        protected override RealNumber Exponentiation(RealNumber b)
        {
            return new Radical(this, b, 1);
        }

        protected override RealNumber Subtraction(RealNumber b)
        {
            return Sum((-1) * b);
        }

        public override bool Equals(object? obj)
        {
            throw new NotImplementedException();
        }

        protected override RealNumber TrySum(RealNumber b)
        {
            if (b is Constant c)
            {
                if (this._name == null && c._name == null)
                    return Sum(c);
                else if (_name == c._name)
                    return Sum(c);
                else if (_multiplier is Constant c1 && c._multiplier is Constant c2 && (c1._name == c._name && _name == c2._name))
                    return Sum(c);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
    }
}
