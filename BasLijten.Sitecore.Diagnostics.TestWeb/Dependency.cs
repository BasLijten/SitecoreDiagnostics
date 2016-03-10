using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace BasLijten.SC.Diagnostics.TestWeb
{
    public class Dependency
    {
        public Dependency()
        {

        }

        public void DoRun()
        {
            Random a = new Random();
            var i = a.Next(0, 50);
            Thread.Sleep(i * 100);
        }
    }
}