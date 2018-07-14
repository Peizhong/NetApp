using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Play.Book
{
    class Chap17
    {
        public unsafe int GetIntNumber()
        {
            int  x= 10;
            int b = 100;
            int* px,pb;
            px = &x;
            pb = &b;
            return x;
        }
    }
}
