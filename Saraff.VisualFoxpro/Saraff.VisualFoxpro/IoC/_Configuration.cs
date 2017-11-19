using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using _IoC = Saraff.IoC;

namespace Saraff.VisualFoxpro.IoC {

    internal sealed class _Configuration : Component, _IoC.IConfiguration {

        public Type BindServiceAttributeType {
            get {
                return typeof(BindServiceAttribute);
            }
        }

        public _IoC.BindServiceCallback BindServiceCallback {
            get {
                return (x, callback) => {
                    var _attr = x as BindServiceAttribute;
                    if(_attr != null) {
                        callback(_attr.Service, _attr.ObjectType);
                    }
                };
            }
        }

        public Type ContextBinderType {
            get {
                return typeof(IContextBinder<,>);
            }
        }

        public Type ServiceRequiredAttributeType {
            get {
                return typeof(ServiceRequiredAttribute);
            }
        }
    }
}
