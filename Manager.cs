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
        static user _currentUser = null;
        public static user CurrentUser {
            get => _currentUser;
            set {
                _currentUser = value;
                NotifyUserChange(value);
            }
        }
        public delegate void UserChangeHandler(user user);
        public static event UserChangeHandler UserChangeEvent;
        private static void NotifyUserChange(user user)
        {
            UserChangeEvent?.Invoke(user);
        }
    }
}
