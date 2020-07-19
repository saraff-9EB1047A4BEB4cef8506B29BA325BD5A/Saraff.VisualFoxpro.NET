using System;
using System.Collections.Generic;
using System.Text;

namespace Saraff.VisualFoxpro.IoC {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ProxyRequiredAttribute : Attribute {
    }
}
