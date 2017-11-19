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
using System.ComponentModel;
using System.Data.Odbc;
using IoC = Saraff.VisualFoxpro.IoC;

namespace Saraff.VisualFoxpro.Odbc {

    public sealed class VfpOdbcConnection:Component,IVfpOdbcConnection,IDisposable {
        private OdbcConnection _connection;

        [IoC.ServiceRequired]
        public VfpOdbcConnection(string connectionString) {
            this._connection=new OdbcConnection(this.ConnectionString=connectionString);
        }

        public void Open() {
            this._connection.Open();
            this.Transaction=this._connection.BeginTransaction();
        }

        public void Close() {
            this._connection.Close();
        }

        public OdbcCommand CreateCommand() {
            var _command=this._connection.CreateCommand();
            _command.Transaction=this.Transaction;
            return _command;
        }

        public void Commit() {
            this.Transaction.Commit();
            this.Transaction=this._connection.BeginTransaction();
        }

        public void Rollback() {
            this.Transaction.Rollback();
            this.Transaction=this._connection.BeginTransaction();
        }

        protected override void Dispose(bool disposing) {
            if(disposing&&this._connection!=null) {
                this._connection.Dispose();
            }
            base.Dispose(disposing);
        }

        public OdbcTransaction Transaction {
            get;
            private set;
        }

        internal string ConnectionString {
            get;
            private set;
        }
    }

    public interface IVfpOdbcConnection {

        OdbcCommand CreateCommand();
    }
}
