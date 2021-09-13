using MyInventory.Core.DB;
using MyInventory.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyInventory.Core
{
    public static class AppSettings
    {
        public static Profile CurrentProfile { get; set; }
        public static string AdminPassword { get; } = "0000";
    }
}
