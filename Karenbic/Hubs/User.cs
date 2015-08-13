using System;
using System.Collections.Generic;

namespace Karenbic.Hubs
{
    public class User
    {
        public int Id { set; get; }
        public string Name { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
}