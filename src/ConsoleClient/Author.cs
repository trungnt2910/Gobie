﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public partial class Author
    {
        private const string EncapsulationTemplate = @"

namespace ConsoleClient
{
    public class {{Property}}Generated
    {
        private System.Collections.Generic.List<string> {{ field }} = new();

        /// Remarable that this is generated by gobie on the fly. Isn't it the coolest? And interesting
        public System.Collections.Generic.IEnumerable<string> {{ Property }} => {{ field }}.AsReadOnly();
    }
}
";

        [Gobie.GobieFieldGenerator(EncapsulationTemplate)]
        private List<string> books = new();

        [Gobie.GobieFieldGenerator(EncapsulationTemplate)]
        private List<string> books2 = new();

        private List<string> sdf = new();
    }
}
