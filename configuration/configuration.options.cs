using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace DockerSecret
{
    public class Secret {
        public string Name { get; } = "";
        public Type Type { get; } = typeof(string);
        public Secret(string name) {
            Name = name;
        }
        public Secret(string name, Type type) {
            Name = name;
            Type = type;
        }
    }

    public class Options
    {
        public static readonly Options DEFAULT = new();

        public string? Location { get; private set;}
        public IList<Secret>? Secrets { get; }
        public bool Logging { get; private set;}


        public Options(string? location = ".",
                IList<Secret>? secrets = null,
                bool logging = false) {
            Location = location;
            Secrets = secrets ?? new List<Secret>();
            Logging = logging;
        }

        public Options FromLocation(string location = ".") {
            Location = location;
            return this;
        }
        public Options Load(string name) {
            (Secrets ?? new List<Secret>()).Add(new Secret(name));
            return this;
        }
        public Options LoadJson(string name) {
            (Secrets ?? new List<Secret>()).Add(new Secret(name, typeof(object)));
            return this;
        }
        public Options Log(bool logging = false) {
            Logging = logging;
            return this;
        }

    }
}