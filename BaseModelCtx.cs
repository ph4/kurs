using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kurs
{
    partial class Dns2Entities
    {
        //Singleton stuff
        private static Dns2Entities _context;

        public static Dns2Entities GetContext()
        {
            if (_context == null) _context = new Dns2Entities();
            return _context;
        }

    }
}
