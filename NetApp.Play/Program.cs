using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NetApp.Play
{
    // Contravariant interface.
    // 父类可以赋给子类
    interface IContravariant<in A> 
    {
        string whoareyou();
    }

    // Implementing contravariant interface.
    class Sample<A> : IContravariant<A> where A : class, new()
    {
        private List<A> _as = new List<A>();

        public Sample()
        {
            for (int n = 0; n < 3; n++)
            {
                _as.Add(new A());
            }
        }

        public string whoareyou()
        {
            foreach (var c in _as)
            {

            }
            return typeof(A).Name;
        }
    }

    class A
    {
        public virtual string Name => "A";
    }

    class B : A
    {
        public override string Name => "B";
        public string CName => "A_B";
    }

    class Program
    {
        static void Main(string[] args)
        {
            IContravariant<A> parent = new Sample<A>();
            IContravariant<B> child = new Sample<B>();

            var ob = parent.whoareyou();
            var st = child.whoareyou();
            // You can assign iobj to istr because
            // the IContravariant interface is contravariant.
            child = parent;
            //iobj = istr;

            var ost = child.whoareyou();

            Book.LearnExpression learnExpression = new Book.LearnExpression();
            learnExpression.Hello();
        }
    }
}