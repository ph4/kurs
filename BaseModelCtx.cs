using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kurs
{
    partial class Dns2Entities
    {
        //Singleton stuff
        private static Dns2Entities _context;
        
        /// <summary>
        /// Attempts to get existing or instantiates database context
        /// </summary>
        /// <returns>Database context</returns>
        public static Dns2Entities GetContext()
        {
            if (_context == null) _context = new Dns2Entities();
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error initializing, no db connection?");
                return null;
            }
            return _context;
        }

    }
}
