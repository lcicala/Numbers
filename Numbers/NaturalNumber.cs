using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numbers
{
    public class NaturalNumber : RealNumberMid
    {
        private long _value;

        public NaturalNumber(long value)
        {
            _value = value;
            _multiplier = null;
        }
        public override bool Equals(RealNumber r)
        {
            if(r is NaturalNumber n)
                return _value == n._value;
            return base.Equals(r);
        }

        public override IEnumerator<RealNumber> GetEnumerator()
        {
            yield return this;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        protected override double CastToDouble()
        {
            return _value;
        }

        protected override RealNumber Division(RealNumber b)
        {
            if(b is NaturalNumber n)
            {
                var GCD = Operation.GCD(this._value, n._value);
                var num = _value/ GCD;
                var den = n._value / GCD;
                if (den != 1)
                    return new Fraction(num, den);
                else
                    return new NaturalNumber(num);
            }
            if (b != 1)
                return new Fraction(this, b);
            else
                return this;
        }

        protected override RealNumber Exponentiation(RealNumber b)
        {
            if (b is NaturalNumber n)
                return (long)Math.Pow(_value, n._value);
            if (b != 1 && b != 0)
            {
                if (b is Fraction f)
                {
                    var s = new Fraction(this, 1);
                    return s ^ f;
                }
                else
                    return new Radical(this, b, 1);
            }
            else if (b != 0)
                return this;
            else
                return 1;
        }

        protected override RealNumber Product(RealNumber b)
        {
            if (b is NaturalNumber n)
            {
                return new NaturalNumber(_value * n._value);
            }
            else if (b is Constant c)
            {
                return c * this;
            }
            else if (b is Radical r)
            {
                if (r.Exponent != 1)
                    return new Radical(r.Base, r.Exponent, r.Multiplier * this);
                else
                    return r.Base * r.Multiplier * this;
            }
            else if (b is Fraction f)
            {
                if (f.Denominator != 1)
                    return new Fraction(f.Numerator * this, f.Denominator);
                else
                    return f.Numerator * this;
            }
            else
                return CreateInstance(this, b);
        }

        protected override RealNumber Subtraction(RealNumber b)
        {
            return this + ((-1) * b);
        }

        protected override RealNumber Sum(RealNumber b)
        {
            if (b is NaturalNumber n)
            {
                return new NaturalNumber(_value + n._value);
            }
            else if (b is Constant c)
            {
                return c + this;
            }
            else if (b is Radical r)
            {
                if (r.Exponent != 1)
                    return CreateInstance(1, this, r);
                else
                    return r.Base * r.Multiplier + this;
            }
            else if (b is Fraction f)
            {
                if (f.Denominator != 1)
                    return new Fraction(f.Numerator + this * f.Denominator, f.Denominator);
                else
                    return f.Numerator + this;
            }
            else
                return CreateInstance(1, this, b);
        }

        protected override RealNumber TrySum(RealNumber b)
        {
            if (b is NaturalNumber n)
            {
                return Sum(n);
            }
            else if (b is Constant c)
            {
                return null;
            }
            else if (b is Radical r)
            {
                if (r.Exponent != 1)
                    return null;
                else
                    return TrySum(r.Base * r.Multiplier);
            }
            else if (b is Fraction f)
            {
                return null;
            }
            else
                return null;
        }

        public static explicit operator long(NaturalNumber naturalNumber) => naturalNumber._value;
    }
}
