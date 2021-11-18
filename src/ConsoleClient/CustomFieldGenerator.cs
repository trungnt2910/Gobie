﻿using Gobie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    public class CustomFieldGeneratorAttribute : GobieFieldGeneratorAttribute
    {
        [GobieTemplate]
        private const string GeneratorComment =
@"        // This used a generator from Gobie, no big deal
";

        [GobieTemplate]
        private const string EncapsulationTemplate =
@"        private System.Collections.Generic.List<string> {{ field }} = new();

        /// Remarable that this is generated by gobie on the fly. Isn't it the coolest? And interesting
        public System.Collections.Generic.IEnumerable<string> {{ Property }} => {{ field }}.AsReadOnly();
";

        [GobieTemplate]
        private const string GeneratorComment2 =
@"        // A third comment. tempalted field.
";

        public CustomFieldGeneratorAttribute()
                                    : base(false)
        {
        }
    }
}
