﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Saraff.VisualFoxpro.IoC {

    public interface IListener {

        object OnInvoking(MethodBase method, object instance, object[] parameters);

        object OnInvoked(MethodBase method, object instance, object result);

        Exception OnCatch(MethodBase method, object instance, Exception ex);
    }
}
