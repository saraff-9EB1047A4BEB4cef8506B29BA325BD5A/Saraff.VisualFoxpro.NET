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
using System.Collections.ObjectModel;
using System.Text;
using System.Data.Odbc;
using Saraff.AxHost;

namespace Saraff.VisualFoxpro.Odbc {

    [IoC.ServiceRequired(Service = typeof(IoC.IBinder))]
    [IoC.ServiceRequired(Service = typeof(IoC.IInstanceFactory))]
    public class OdbcApplicationComponent:VfpApplicationComponent {

        public OdbcApplicationComponent() {
        }

        protected override void Construct(ReadOnlyCollection<object> args) {
            base.Construct(args);

            #region Настройка подключения

            var _args=new ConnectionRequiredEventArgs();
            this.OnConnectionRequired(_args);
            this._Binder.Bind<IVfpOdbcConnection>(this.Connection=this._InstanceFactory.CreateInstance<VfpOdbcConnection>(x => x("connectionString", _args.ConnectionString)));
            this.Connection.Open();

            #endregion

            this.OnDataLoad(EventArgs.Empty);
        }

        protected virtual void OnDataLoad(EventArgs e) {
            if(this.DataLoad!=null) {
                this.DataLoad(this, e);
            }
        }

        private void OnConnectionRequired(ConnectionRequiredEventArgs e) {
            if(this.VfpConnectionRequired_448FCAB8513A4E5996E775521DC76FD5!=null) {
                this.VfpConnectionRequired_448FCAB8513A4E5996E775521DC76FD5(this, e);
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

        private IoC.IBinder _Binder {
            get {
                return this.GetService(typeof(IoC.IBinder)) as IoC.IBinder;
            }
        }

        private IoC.IInstanceFactory _InstanceFactory {
            get {
                return this.GetService(typeof(IoC.IInstanceFactory)) as IoC.IInstanceFactory;
            }
        }

        #endregion

        #region События

        [ApplicationProcessed]
        public event EventHandler VfpConnectionRequired_448FCAB8513A4E5996E775521DC76FD5;

        public event EventHandler DataLoad;

        #endregion
    }
}
