﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tasks.data
{
    public class User
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

    }
}
