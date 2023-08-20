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
        public Secret(string name) {
            Name = name;
        }
    }

    public class Options
    {
        public static readonly Options DEFAULT = new();

        public string? Location { get; private set;}
        public IList<Secret>? Secrets { get; }


        public Options(string? location = ".",
                IList<Secret>? secrets = null) {
            Location = location;
            Secrets = secrets ?? new List<Secret>();
        }

        public Options FromLocation(string location = ".") {
            Location = location;
            return this;
        }
        public Options Load(string name) {
            (Secrets ?? new List<Secret>()).Add(new Secret(name));
            return this;
        }

    }
}