using System;
using System.Collections.Generic;
using System.Text;

namespace Saraff.VisualFoxpro.Odbc {

    [AttributeUsage(AttributeTargets.Class,AllowMultiple=true,Inherited=false)]
    public sealed class DbObjectRequiredAttribute:Attribute {

        public DbObjectRequiredAttribute(string owner, string name, DbAccessFlags flags) {
            this.Owner=owner;
            this.Name=name;
            this.Flags=flags;
        }

        public string Owner {
            get;
            private set;
        }

        public string Name {
            get;
            private set;
        }

        public DbAccessFlags Flags {
            get;
            private set;
        }
    }

    [Flags]
    public enum DbAccessFlags {
        Select=0x01,
        Insert=0x02,
        Update=0x04,
        Delete=0x08,
        Execute=0x10
    }
}
