using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Saraff.VisualFoxpro.Core {

    internal sealed class ProxyService:Component, IProxy {
        private EventHandler _handler;

        public ProxyService(EventHandler handler) {
            this._handler=handler;
        }

        #region IProxy

        public event EventHandler ProxyChanged;

        public void Send(EventArgs args) {
            this._handler(this,args);
        }

        #endregion

        internal void OnProxyChanged(EventArgs e) {
            if(this.ProxyChanged != null) {
                this.ProxyChanged(this,e);
            }
        }
    }
}
