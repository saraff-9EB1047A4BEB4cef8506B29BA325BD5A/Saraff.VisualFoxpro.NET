using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using IoC=Saraff.VisualFoxpro.IoC;
using Saraff.VisualFoxpro.Odbc;
using Saraff.VisualFoxpro.Samples.ComponentModel;

namespace Saraff.VisualFoxpro.Samples.Core {

    internal class SampleServiceComponent:Component,ISampleService {

        #region ISampleService

        public DateTime GetCurrentDbDate() {
            using(var _command=this.Connection.CreateCommand()) {
                _command.CommandText="SELECT SYSDATE FROM DUAL";
                return (DateTime)_command.ExecuteScalar();
            }
        }

        #endregion

        [IoC.ServiceRequired]
        public IVfpOdbcConnection Connection {
            get;
            set;
        }
    }
}
