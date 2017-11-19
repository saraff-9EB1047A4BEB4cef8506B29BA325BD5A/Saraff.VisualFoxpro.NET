using System;
using System.Collections.Generic;
using System.Text;

namespace Saraff.VisualFoxpro.IoC {

    public interface IBinder {

        void Bind(Type service,object obj);

        void Bind(Type service,object obj,bool addToContainer);

        void Bind(Type service,Type obj);

        void Bind<TService>(object obj);

        void Bind<TService>(object obj,bool addToContainer);

        void Bind<TService, T>();
    }
}
