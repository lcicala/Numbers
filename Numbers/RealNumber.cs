using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Numbers
{
    public class RealNumber : IEnumerable<RealNumber>, IEquatable<RealNumber>, ICloneable
    {
        private List<RealNumber> _numbers;

        protected RealNumber _multiplier;
        protected RealNumber(RealNumber multiplier, params RealNumber[] numbers)
        {
            _numbers = numbers?.ToList();
            _multiplier = multiplier;
            //if (_multiplier is not null && _multiplier != 1)
            //{
            //    foreach (var number in _numbers)
            //    {
            //        if (number is not NaturalNumber)
            //        {
            //            if (number._multiplier is not null && number._multiplier != 1)
            //                number._multiplier *= _multiplier;
            //            else
            //                number._multiplier = _multiplier;
            //        }
            //    }
            //}
        }

        protected RealNumber() { }

        protected RealNumber CreateInstance(RealNumber multiplier, params RealNumber[] numbers)
        {
            return new RealNumber(multiplier, numbers);
        }

        public virtual IEnumerator<RealNumber> GetEnumerator()
        {
            //foreach (RealNumber number in _numbers)
            //{
            //    if (number is not NaturalNumber)
            //    {
            //        if (number._multiplier is null || number._multiplier == 1 && _multiplier is not null)
            //            number._multiplier = _multiplier;
            //        yield return number;
            //    }
            //    else
            //        yield return number;
            //}
            return _numbers?.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var ret = string.Empty;
            foreach (RealNumber number in _numbers)
            {
                if(number is not null && number != 0)
                    ret += number.ToString() + " + ";
            }
            ret = ret.Trim(' ', '+');
            if (ret == String.Empty)
                return "0";
            if (_multiplier is not null && _multiplier != 1)
            {
                if (ret != "1")
                    ret = $"({_multiplier.ToString()}) * ({ret})";
                else
                    ret = _multiplier.ToString();
            }
            if (ret == String.Empty)
                ret = "0";
            return ret;
        }

        protected virtual RealNumber Sum(RealNumber b)
        {
            IEnumerable<RealNumber> numbers = this;
            foreach (var n in this)
            {
                foreach (var n2 in b)
                {
                    RealNumber trysum;
                    if (n is Constant c && n2 is Constant c2)
                        trysum = (c).TrySum(c2);
                    else if (n is Constant cc)
                        trysum = (cc).TrySum(n2);
                    else if (n2 is Constant cr2)
                        trysum = (n).TrySum(cr2);
                    else
                        trysum = n?.TrySum(b) ?? null;
                    if (trysum is not null)
                    {
                        numbers = numbers.Except(n).Concat(trysum);
                        return new RealNumber(1, numbers.ToArray());
                    }
                }
            }
            if(b is not RealNumberMid)
            {
                var c = b.Select(x => { var y = x.Clone() as RealNumber; y._multiplier *= b._multiplier; return y; });
                numbers = numbers.Where(x => x is not null).Concat(c);
            }
            else
                numbers = numbers.Where(x => x is not null).Concat(b);
            return new RealNumber(1, numbers.ToArray());
        }

        protected virtual RealNumber TrySum(RealNumber b)
        {
            if (b is null)
                return this;
            IEnumerable<RealNumber> numbers = new List<RealNumber>();
            if (this._numbers is null)
                return null;
            foreach (var n in this)
            {
                foreach (var n2 in b)
                {
                    RealNumber trysum;
                    if (n is Constant c && n2 is Constant c2)
                        trysum = (c * _multiplier).TrySum(c2 * b._multiplier);
                    else if(n is Constant cc)
                        trysum = (cc * _multiplier).TrySum(n2);
                    else if(n2 is Constant cr2)
                        trysum = (n).TrySum(cr2 * b._multiplier);
                    else
                        trysum = n?.TrySum(b);
                    if (trysum is not null)
                    {
                        numbers = this.Except(n).Concat(trysum);
                        return new RealNumber(1, numbers.ToArray());
                    }
                }
            }
            return null;
        }

        protected virtual RealNumber Product(RealNumber b)
        {
            List<RealNumber> numbers = new List<RealNumber>();
            foreach(var n in this)
            {
                foreach (var n2 in b)
                {
                    numbers.Add(n.Product(n2));
                }
            }
            //return new RealNumber(1, numbers.ToArray());
            return new RealNumber(1, numbers.Aggregate((x, y) => x.Sum(y)).ToArray());
        }
        protected virtual RealNumber Subtraction(RealNumber b)
        {
            return Sum((-1) * b);
        }
        protected virtual RealNumber Division(RealNumber b) => throw new NotImplementedException();
        protected virtual RealNumber Exponentiation(RealNumber b)
        {
            if (b != 1 && b != 0)
                return new Radical(this, b, _multiplier);
            else if (b != 0)
                return this;
            else
                return 1;
        }

        protected virtual double CastToDouble()
        {
            double ret = 0;
            if (this._numbers is null)
                return 0;
            foreach(var n in this)
            {
                if(n is not null)
                    ret += n.CastToDouble();
            }
            ret *= _multiplier.CastToDouble();
            return ret;
        }

        public virtual bool Equals(RealNumber other)
        {
            if(other is null)
                return false;
            return (double)this == (double)other;
        }

        public object Clone()
        {
            return new RealNumber(_multiplier, _numbers?.ToArray());
        }

        public static bool operator ==(RealNumber lhs, RealNumber rhs)
        {
            return lhs?.Equals(rhs) ?? (rhs is null);
        }

        public static bool operator !=(RealNumber lhs, RealNumber rhs)
        {
            return !(lhs == rhs);
        }

        public static RealNumber operator +(RealNumber a, RealNumber b)
        {
            return a?.Sum(b) ?? b;
        }

        public static RealNumber operator *(RealNumber a, RealNumber b)
        {
            return a?.Product(b);
        }

        public static RealNumber operator -(RealNumber a, RealNumber b)
        {
            return a.Subtraction(b);
        }

        public static RealNumber operator /(RealNumber a, RealNumber b)
        {
            return a.Division(b);
        }

        public static RealNumber operator ^(RealNumber a, RealNumber b)
        {
            return a.Exponentiation(b);
        }

        public static implicit operator RealNumber(double d) => new Fraction(d);
        public static implicit operator RealNumber(int d) => new NaturalNumber(d);
        public static implicit operator RealNumber(long d) => new NaturalNumber(d);

        public static explicit operator double(RealNumber r)
        {
            return r?.CastToDouble() ?? 1;
        }
    }
}
