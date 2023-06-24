using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numbers
{
    public abstract class RealNumberMid : RealNumber
    {
        protected abstract override double CastToDouble();
        protected abstract override RealNumber Division(RealNumber b);
        protected abstract override RealNumber Exponentiation(RealNumber b);
        protected abstract override RealNumber Product(RealNumber b);
        protected abstract override RealNumber Subtraction(RealNumber b);
        protected abstract override RealNumber Sum(RealNumber b);
        protected abstract override RealNumber TrySum(RealNumber b);
        public abstract override string ToString();
        public override bool Equals(RealNumber r)
        {
            return base.Equals(r);
        }
        public abstract override IEnumerator<RealNumber> GetEnumerator();
    }
}
