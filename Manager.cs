using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace kurs
{
    static class Manager
    {
        public static Frame MainFrame { get; set; }
        public static user CurrentUser { get; set; } = null;
        public delegate void UserChangeHandler(user user);
        public static event UserChangeHandler UserChangeEvent;
        public static void NotifyUserChange(user user)
        {
            UserChangeEvent?.Invoke(user);
        }
    }
}
