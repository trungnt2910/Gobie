﻿namespace Gobie.Models
{
    public static class Config
    {
        static Config()
        {
            var db = ImmutableDictionary.CreateBuilder<string, string>();
            db.Add("GobieClassGenerator", "global::Gobie.GobieClassGeneratorAttribute");
            db.Add("Gobie.GobieClassGenerator", "global::Gobie.GobieClassGeneratorAttribute");
            db.Add("global::Gobie.GobieClassGenerator", "global::Gobie.GobieClassGeneratorAttribute");
            db.Add("GobieFieldGenerator", "global::Gobie.GobieFieldGeneratorAttribute");
            db.Add("Gobie.GobieFieldGenerator", "global::Gobie.GobieFieldGeneratorAttribute");
            db.Add("GobieGlobalGenerator", "global::Gobie.GobieAssemblyGeneratorAttribute");
            db.Add("Gobie.GobieGlobalGenerator", "global::Gobie.GobieAssemblyGeneratorAttribute");
            GenToAttribute = db.ToImmutable();
        }

        public static ImmutableDictionary<string, string> GenToAttribute { get; }
    }
}
