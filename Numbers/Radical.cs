using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Numbers
{
    public class Radical : RealNumberMid /*: IEquatable<Radical>, IEquatable<double>, IEquatable<int>, IComparable<Radical>*/
    {
        private RealNumber _base;
        private RealNumber _exponent;

        public RealNumber Multiplier => _multiplier;
        public RealNumber Base { get { return _base; } }
        public RealNumber Exponent { get { return _exponent; } }

        public override IEnumerator<RealNumber> GetEnumerator()
        {
            yield return this;
        }

        public Radical(RealNumber b, RealNumber exp, RealNumber multiplier = null)
        {
            _base = b;
            _exponent = exp;
            if (multiplier == null)
            {
                _multiplier = 1;
            }
            else
                _multiplier = multiplier;
            //if (_base is Fraction bf && _exponent is Fraction ef)
            //{
            //    var primeFactorsGrouped = Operation.GetPrimeFactors((int)Math.Pow(bf.Numerator, ef.Numerator)).GroupBy(x => x);
            //    var newPrimeFactorsNumerator = new List<int>();
            //    var primeFactorsMultiplierNumerator = new List<int>();
            //    var newPrimeFactorsDenominator = new List<int>();
            //    var primeFactorsMultiplierDenominator = new List<int>();
            //    primeFactorsMultiplierDenominator.Add(1);
            //    primeFactorsMultiplierNumerator.Add(1);
            //    newPrimeFactorsDenominator.Add(1);
            //    newPrimeFactorsNumerator.Add(1);
            //    foreach (var group in primeFactorsGrouped)
            //    {
            //        newPrimeFactorsNumerator.AddRange(group.Take(group.Count() % (int)ef.Denominator));
            //        primeFactorsMultiplierNumerator.AddRange(group.Take(group.Count() / (int)ef.Denominator));
            //    }
            //    primeFactorsGrouped = Operation.GetPrimeFactors((int)Math.Pow(bf.Denominator, ef.Numerator)).GroupBy(x => x);
            //    foreach (var group in primeFactorsGrouped)
            //    {
            //        newPrimeFactorsDenominator.AddRange(group.Take(group.Count() % (int)ef.Denominator));
            //        primeFactorsMultiplierDenominator.AddRange(group.Take(group.Count() / (int)ef.Denominator));
            //    }
            //    _exponent = new Fraction(1, ef.Denominator);
            //    _base = new Fraction(newPrimeFactorsNumerator.Aggregate((x, y) => x * y), newPrimeFactorsDenominator.Aggregate((x, y) => x * y));
            //    _multiplier *= (new Fraction(primeFactorsMultiplierNumerator.Aggregate((x, y) => x * y), primeFactorsMultiplierDenominator.Aggregate((x, y) => x * y)));
            //}
        }

        public override string ToString()
        {
            if (Exponent == 1)
                return (Base * Multiplier).ToString();
            if (Base == 1)
                return Multiplier.ToString();
            if (Base == 0 || Multiplier == 0)
                return 0.ToString();
            if (Multiplier == 1)
                return string.Format("({0})^({1})", Base, Exponent);
            return string.Format("{2}*({0})^({1})", Base, Exponent, Multiplier);
        }

        public bool Equals(Radical other)
        {
            return Base == other.Base && Multiplier == other.Multiplier && Exponent == other.Exponent;
        }

        public bool Equals(double other)
        {
            var ot = new Radical(other, 1);
            return ot.Equals(this);
        }

        public bool Equals(int other)
        {
            var ot = new Radical(other, 1);
            return ot.Equals(this);
        }

        public int CompareTo(Radical other)
        {
            if (other.Equals(this))
                return 0;
            return (double)this > (double)other ? 1 : -1;
        }

        protected override RealNumber Sum(RealNumber b)
        {
            if (Exponent == 1)
                return Base * Multiplier + b;
            if(b is Radical r)
            {
                if (Base == r.Base && Exponent == r.Exponent)
                    return new Radical(Base, Exponent, Multiplier + r.Multiplier);
            }
            if(b.Multiplier is Radical rm)
            {
                if (Base == rm.Base && Exponent == rm.Exponent)
                {
                    var s = b.Clone() as RealNumber;
                    s.Multiplier = 1;
                    return new Radical(Base, Exponent, Multiplier + (rm.Multiplier*s));
                }
                else
                    return CreateInstance(1, this, rm);
            }
            return CreateInstance(1, this, b);
        }

        protected override double CastToDouble()
        {
            return Math.Pow((double)Base, (double)Exponent) * (double)Multiplier;
        }

        //protected override RealNumber Product(RealNumber b)
        //{
        //    if (b is Radical r)
        //    {
        //        var commonExponentDenominator = Operation.LCM(Exponent.Denominator, r.Exponent.Denominator);
        //        var thisNewExponentNumerator = Exponent.Numerator * commonExponentDenominator / Exponent.Denominator;
        //        var otherNewExponentNumerator = r.Exponent.Numerator * commonExponentDenominator / r.Exponent.Denominator;
        //        var newBase = new Fraction((int)(Math.Pow(Base.Numerator, thisNewExponentNumerator) * Math.Pow(r.Base.Numerator, otherNewExponentNumerator)),
        //            (int)(Math.Pow(Base.Denominator, thisNewExponentNumerator) * Math.Pow(r.Base.Denominator, otherNewExponentNumerator)));
        //        var newExponent = new Fraction(1, commonExponentDenominator);
        //        var newMultiplier = Multiplier * r.Multiplier;
        //        return new Radical(newBase, newExponent, newMultiplier);
        //    }
        //    else
        //    {
        //        List<RealNumber> l = new List<RealNumber>();
        //        foreach (var n in b)
        //        {
        //            l.Add(Product(n));
        //        }
        //        return CreateInstance(l.ToArray());
        //    }
        //}

        protected override RealNumber Product(RealNumber b)
        {
            if (Exponent == 1)
                return Base * Multiplier * b;
            if (b is NaturalNumber n)
            {
                if(Base == n)
                    return new Radical(Base, Exponent + 1, Multiplier);
                else
                    return new Radical(Base, Exponent, Multiplier * n);
            }
            else if (b is Constant c)
            {
                if(Base == c)
                    return new Radical(Base, Exponent+1, Multiplier);
                else
                    return new Radical(Base, Exponent, Multiplier * c);
            }
            else if (b is Radical r)
            {
                if (Base == r.Base)
                {
                    var exp = Exponent + r.Exponent;
                    if(exp != 1)
                        return new Radical(Base, Exponent + r.Exponent, Multiplier * r.Multiplier);
                    else
                        return Base * Multiplier * r.Multiplier;
                }
                if (Exponent == r.Exponent)
                    return new Radical(Base * r.Base, Exponent, Multiplier * r.Multiplier);
                if (Exponent is Fraction thisExponent)
                {
                    return new Radical(Base * (r.Base ^ (thisExponent.Denominator * r.Exponent)), Exponent, Multiplier * r.Multiplier);
                }
                else if(r.Exponent is Fraction rExponent)
                {
                    return new Radical(r.Base * (Base ^ (rExponent.Denominator * Exponent)), r.Exponent, Multiplier * r.Multiplier);
                }
                else
                    return CreateInstance(this, r);
            }
            else if (b is Fraction f)
            {
                return CreateInstance(f, this);
            }
            else
                return CreateInstance(this, b);
        }

        protected override RealNumber Subtraction(RealNumber b)
        {
            return Sum((-1) * b);
        }

        protected override RealNumber Division(RealNumber b)
        {
            throw new NotImplementedException();
        }

        protected override RealNumber Exponentiation(RealNumber b)
        {
            var exp = Exponent * b;
            if (exp == 1)
                return Base * (Multiplier ^ b);
            return new Radical(Base, exp, Multiplier);
        }

        protected override RealNumber TrySum(RealNumber b)
        {
            if (b is Radical r)
            {
                if (Base == r.Base && Exponent == r.Exponent || (Exponent == 1 && r.Exponent == 1))
                    return Sum(r);
            }
            if(b.Multiplier is Radical rm)
            {
                if (Base == rm.Base && Exponent == rm.Exponent || (Exponent == 1 && rm.Exponent == 1))
                    return Sum(b);
            }
            return null;
        }

        public override object Clone()
        {
            return new Radical(Base, Exponent, Multiplier);
        }

        //public static bool operator >(Radical a, Radical b)
        //{
        //    return a.CompareTo(b) > 0;
        //}

        //public static bool operator <(Radical a, Radical b)
        //{
        //    return a.CompareTo(b) < 0;
        //}

        //public static bool operator ==(Radical a, Radical b)
        //{
        //    return a.Equals(b);
        //}

        //public static bool operator !=(Radical a, Radical b)
        //{
        //    return !a.Equals(b);
        //}

        //public static explicit operator double(Radical radical)
        //{
        //    return Math.Pow((double)radical.Base, (double)radical.Exponent)*(double)radical.Multiplier;
        //}

        //public static implicit operator Radical(double d)
        //{
        //    return new Radical(d, 1);
        //}

        //public static implicit operator Radical(int d)
        //{
        //    return new Radical(d, 1);
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
