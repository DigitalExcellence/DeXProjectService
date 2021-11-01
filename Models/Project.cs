﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
    }
}
