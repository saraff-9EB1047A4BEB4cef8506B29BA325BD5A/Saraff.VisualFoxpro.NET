using System;
using System.Collections.Generic;
using System.Text;

namespace Saraff.VisualFoxpro {

    public interface IErrorHandler {

        void Invoke(Exception ex);
    }
}
