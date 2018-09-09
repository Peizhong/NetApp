using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NetApp.Play
{
    // Contravariant interface.
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
            IContravariant<A> iobj = new Sample<A>();
            IContravariant<B> istr = new Sample<B>();

            var ob = iobj.whoareyou();
            var st = istr.whoareyou();
            // You can assign iobj to istr because
            // the IContravariant interface is contravariant.
            istr = iobj;
            //iobj = istr;

            var ost = istr.whoareyou();
            var m = new Utils.MirgrateAvmt();
            var cs = m.GetDevices();
        }
    }
}