/* Этот файл является частью библиотеки Saraff.VisualFoxpro.NET
 * © SARAFF SOFTWARE (Кирножицкий Андрей), 2015.
 * Saraff.VisualFoxpro.NET - свободная программа: вы можете перераспространять ее и/или
 * изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
 * в каком она была опубликована Фондом свободного программного обеспечения;
 * либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
 * версии.
 * Saraff.VisualFoxpro.NET распространяется в надежде, что она будет полезной,
 * но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
 * или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
 * общественной лицензии GNU.
 * Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
 * вместе с этой программой. Если это не так, см.
 * <http://www.gnu.org/licenses/>.)
 * 
 * This file is part of Saraff.VisualFoxpro.NET.
 * © SARAFF SOFTWARE (Kirnazhytski Andrei), 2014.
 * Saraff.VisualFoxpro.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * Saraff.VisualFoxpro.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public License
 * along with Saraff.VisualFoxpro.NET. If not, see <http://www.gnu.org/licenses/>.
 * 
 * PLEASE SEND EMAIL TO:  vfp@saraff.ru.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using Saraff.AxHost;

namespace Saraff.VisualFoxpro.Odbc {

    public class OdbcApplicationControl:VfpApplicationControl {
        private IoC.ServiceContainer _services;

        public OdbcApplicationControl() {
        }

        [ApplicationProcessed]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void VfpToolButtonClick_9CACAA37F68546C68EC0578463BCDFB5(int button) {
            try {
                this.OnToolButtonClick(new ApplicationToolButtonEventArgs((VfpToolButons)button));
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        protected override void OnEventHandlerProxyChanged(EventArgs e) {
            base.OnEventHandlerProxyChanged(e);

            #region Настройка подключения

            var _args=new ConnectionRequiredEventArgs();
            this.OnConnectionRequired(_args);
            this.Connection=new VfpOdbcConnection(_args.ConnectionString);
            this.Connection.Open();

            #endregion

            this._services.Bind(typeof(IVfpOdbcConnection), this.Connection);
            this.OnDataLoad(EventArgs.Empty);
        }

        protected override void Dispose(bool disposing) {
            if(disposing&&this.Connection!=null) {
                this.Connection.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnServiceContainerInit(IoC.ServiceContainer container) {
            this._services=container;
            base.OnServiceContainerInit(container);
        }

        protected virtual void OnDataLoad(EventArgs e) {
            if(this.DataLoad!=null) {
                this.DataLoad(this, e);
            }
        }

        protected virtual void OnToolButtonClick(ApplicationToolButtonEventArgs e) {
        }

        protected void SetToolBarButtonsState(VfpToolButons buttons, bool isEnable) {
            this.OnToolButonsStateChangeRequired(new VfpToolButonsStateChangeRequiredEventArgs(buttons, isEnable));
        }

        private void OnConnectionRequired(ConnectionRequiredEventArgs e) {
            if(this.VfpConnectionRequired_ED37CBB9BB88499CA2D2633826835D7C!=null) {
                this.VfpConnectionRequired_ED37CBB9BB88499CA2D2633826835D7C(this, e);
            }
        }

        private void OnToolButonsStateChangeRequired(VfpToolButonsStateChangeRequiredEventArgs e) {
            if(this.VfpToolButonsStateChangeRequired_0C9678D80B9045AFA077AF524727F4B0!=null) {
                this.VfpToolButonsStateChangeRequired_0C9678D80B9045AFA077AF524727F4B0(this, e);
            }
        }

        #region Свойства

        protected VfpOdbcConnection Connection {
            get;
            private set;
        }

        protected string ConnectionString {
            get {
                return this.GetType().GetCustomAttributes(typeof(ConnectionInfoAttribute), false).Length>0?this.Connection.ConnectionString:null;
            }
        }

        #endregion

        #region Obsolete

        [Obsolete("Необходимо использовать this.Connection.CreateCommand()")]
        protected OdbcCommand CreateCommand() {
            return this.Connection.CreateCommand();
        }

        [Obsolete("Необходимо использовать this.Connection.Commit() или this.Connection.Rollback()")]
        protected void CommitChanges(bool isCommit) {
            if(isCommit) {
                this.Connection.Commit();
            } else {
                this.Connection.Rollback();
            }
        }

        #endregion

        #region События

        [VfpProcessed]
        public event EventHandler VfpConnectionRequired_ED37CBB9BB88499CA2D2633826835D7C;

        [VfpProcessed]
        public event EventHandler VfpToolButonsStateChangeRequired_0C9678D80B9045AFA077AF524727F4B0;

        public event EventHandler DataLoad;

        #endregion

        #region Вложенные типы

        public sealed class VfpToolButonsStateChangeRequiredEventArgs:EventArgs {

            public VfpToolButonsStateChangeRequiredEventArgs(VfpToolButons buttons, bool isEnable) {
                this.Buttons=buttons;
                this.IsEnable=isEnable;
            }

            public VfpToolButons Buttons {
                get;
                private set;
            }

            public bool IsEnable {
                get;
                private set;
            }
        }

        public sealed class ApplicationToolButtonEventArgs:EventArgs {

            internal ApplicationToolButtonEventArgs(VfpToolButons button) {
                this.ApplicationToolButton=button;
            }

            public VfpToolButons ApplicationToolButton {
                get;
                private set;
            }
        }

        #endregion
    }
}
