using System;
using System.Collections.Generic;
using System.Text;

namespace ModelAndDal.Model.Users
{
    public delegate void UserBalanceNotification(User u, decimal amount);
}
