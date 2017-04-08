using System;
using System.Collections.Generic;
using System.Text;

namespace Saraff.VisualFoxpro.IoC {

    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property|AttributeTargets.Constructor, AllowMultiple=true, Inherited=false)]
    public sealed class ServiceRequiredAttribute:Attribute {

        public ServiceRequiredAttribute() {
        }

        public Type Service {
            get;
            set;
        }
    }
}
