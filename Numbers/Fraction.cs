using System;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Numbers
{
    public class Fraction : RealNumberMid /*IEquatable<Fraction>, IEquatable<double>, IEquatable<int>, IComparable<Fraction>*/
    {
        private RealNumber _numerator;
        private RealNumber _denominator;

        public RealNumber Numerator { get { return _numerator; } }
        public RealNumber Denominator { get { return _denominator; } }

        public Fraction(RealNumber numerator, RealNumber denominator)
        {
            _numerator = numerator;
            _denominator = denominator;
            Simplify();
        }

        public Fraction(double d)
        {
            int exp = 0;
            while (Math.Abs(d * Math.Pow(10, exp) - Math.Floor(d * Math.Pow(10, exp))) > 0)
            {
                exp++;
            }
            _numerator = new NaturalNumber((long)(d * Math.Pow(10, exp)));
            _denominator = new NaturalNumber((long)Math.Pow(10, exp));
            Simplify();
        }

        private void Simplify()
        {
            if (_numerator is NaturalNumber nn && _denominator is NaturalNumber nd)
            {
                var GCD = Operation.GCD((int)nn, (int)nd);
                _numerator /= GCD;
                _denominator /= GCD;
            }
        }

        public override string ToString()
        {
            if (Denominator == 1)
                return Numerator.ToString();
            return string.Format("{0}/{1}", _numerator, _denominator);
        }

        public override bool Equals(RealNumber other)
        {
            if(other is Fraction f)
                return Numerator == f.Numerator && Denominator == f.Denominator;
            else
                return base.Equals(other);
        }

        public override IEnumerator<RealNumber> GetEnumerator()
        {
            yield return this;
        }

        //public bool Equals(double other)
        //{
        //    var ot = new Fraction(other);
        //    return ot.Equals(this);
        //}

        //public bool Equals(int other)
        //{
        //    var ot = new Fraction(other);
        //    return ot.Equals(this);
        //}

        public int CompareTo(Fraction other)
        {
            if (other.Equals(this))
                return 0;
            //return Numerator * other.Denominator > other.Numerator * Denominator ? 1 : -1;
            return 0;
        }

        //public static bool operator >(Fraction a, Fraction b)
        //{
        //    return a.CompareTo(b) > 0;
        //}

        //public static bool operator <(Fraction a, Fraction b)
        //{
        //    return a.CompareTo(b) < 0;
        //}

        //public static bool operator ==(Fraction a, Fraction b)
        //{
        //    return a.Equals(b);
        //}

        //public static bool operator !=(Fraction a, Fraction b)
        //{
        //    return !a.Equals(b);
        //}

        protected override double CastToDouble()
        {
            return (double)Numerator / (double)Denominator * (double)_multiplier;
        }

        protected override RealNumber Product(RealNumber b)
        {
            if (Denominator == 1)
                return Numerator * b;
            if (b is NaturalNumber n)
            {
                return new Fraction(Numerator * n, Denominator);
            }
            else if (b is Constant c)
            {
                return new Fraction(Numerator * c, Denominator);
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
                if (f.Denominator * Denominator != 1)
                    return new Fraction(f.Numerator * Numerator, f.Denominator * Denominator);
                else
                    return f.Numerator * Numerator;
            }
            else
                return CreateInstance(this, b);
        }

        protected override RealNumber Division(RealNumber b)
        {
            if (Denominator == 1)
                return Numerator / b;
            if (b is NaturalNumber n)
            {
                return new Fraction(Numerator, Denominator * n);
            }
            else if (b is Constant c)
            {
                return new Fraction(Numerator, Denominator * c);
            }
            else if (b is Radical r)
            {
                if (r.Exponent != 1)
                    return new Radical(r.Base, r.Exponent, r.Multiplier / this);
                else
                    return (r.Base * r.Multiplier) / this;
            }
            else if (b is Fraction f)
            {
                if (f.Numerator != 0)
                    return this * new Fraction(f.Denominator, f.Numerator);
                else
                    throw new ArithmeticException();
            }
            else
                return CreateInstance(this, new Fraction(1, b));
        }

        protected override RealNumber Exponentiation(RealNumber b)
        {
            throw new NotImplementedException();
        }

        protected override RealNumber Subtraction(RealNumber b)
        {
            throw new NotImplementedException();
        }

        protected override RealNumber Sum(RealNumber b)
        {
            if (Denominator == 1)
                return Numerator + b;
            if (b is NaturalNumber n)
            {
                return new Fraction(Numerator + n * Denominator, Denominator);
            }
            else if (b is Constant c)
            {
                return new Fraction(Numerator + c * Denominator, Denominator);
            }
            else if (b is Radical r)
            {
                if (r.Exponent != 1)
                    return CreateInstance(1, this, r);
                else
                    return (r.Base * r.Multiplier) + this;
            }
            else if (b is Fraction f)
            {
               return new Fraction(Numerator * f.Denominator + f.Numerator * Denominator, Denominator * f.Denominator);
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
                return Sum(c);
            }
            else if (b is Radical r)
            {
                if (r.Exponent != 1)
                    return null;
                else
                    return Sum(r);
            }
            else if (b is Fraction f)
            {
                return Sum(f);
            }
            else
                return null;
        }

        //public static implicit operator Fraction(double d)
        //{
        //    return new Fraction(d);
        //}

        //public static implicit operator Fraction(int d)
        //{
        //    return new Fraction(d);
        //}

        //public static Fraction operator +(Fraction f1, Fraction f2)
        //{
        //    return new Fraction(f1.Numerator
        //        * f2.Denominator + f2.Numerator * f1.Denominator, f1.Denominator * f2.Denominator);
        //}

        //public static Fraction operator -(Fraction f1, Fraction f2)
        //{
        //    return new Fraction(f1.Numerator
        //        * f2.Denominator - f2.Numerator * f1.Denominator, f1.Denominator * f2.Denominator);
        //}

        //public static Fraction operator *(Fraction f1, Fraction f2)
        //{
        //    return new Fraction(f1.Numerator * f2.Numerator, f1.Denominator * f2.Denominator);
        //}

        //public static Fraction operator /(Fraction f1, Fraction f2)
        //{
        //    return new Fraction(f1.Numerator * f2.Denominator, f1.Denominator * f2.Numerator);
        //}

        //public static bool operator <=(Fraction left, Fraction right)
        //{
        //    return left.CompareTo(right) <= 0;
        //}

        //public static bool operator >=(Fraction left, Fraction right)
        //{
        //    return left.CompareTo(right) >= 0;
        //}
    }
}