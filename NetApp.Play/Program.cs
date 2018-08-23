using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.Play
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = new Utils.MirgrateAvmt();
            var cs = m.GetClassifies();
        }
    }
}