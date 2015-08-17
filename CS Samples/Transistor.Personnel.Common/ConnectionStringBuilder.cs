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
using Saraff.VisualFoxpro;

namespace Transistor.Personnel.Common {

    [ApplicationComponent]
    public sealed class ConnectionStringBuilder:VfpApplicationComponent {
        private OdbcConnectionStringBuilder _builder=new OdbcConnectionStringBuilder();

        [ApplicationProcessed]
        public void Add(string keyword,string value) {
            try {
                this._builder.Add(keyword, value);
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        [ApplicationProcessed]
        public void Clear() {
            try {
                this._builder.Clear();
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        [ApplicationProcessed]
        public string GetConnectionString() {
            try {
                return this._builder.ConnectionString;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
            return string.Empty;
        }

        [ApplicationProcessed]
        public void SetConnectionString(string value) {
            try {
                this._builder.ConnectionString=value;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        [ApplicationProcessed]
        public bool ContainsKey(string keyword) {
            try {
                return this._builder.ContainsKey(keyword);
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
            return false;
        }

        [ApplicationProcessed]
        public int GetCount() {
            try {
                return this._builder.Count;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
            return 0;
        }

        [ApplicationProcessed]
        public string GetDriver() {
            try {
                return this._builder.Driver;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
            return string.Empty;
        }

        [ApplicationProcessed]
        public void SetDriver(string value) {
            try {
                this._builder.Driver=value;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        [ApplicationProcessed]
        public string GetDsn() {
            try {
                return this._builder.Dsn;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
            return string.Empty;
        }

        [ApplicationProcessed]
        public void SetDsn(string value) {
            try {
                this._builder.Dsn=value;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        [ApplicationProcessed]
        public void Remove(string keyword) {
            try {
                this._builder.Remove(keyword);
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        [ApplicationProcessed]
        public string GetValue(string keyword) {
            try {
                return this._builder[keyword] as string;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
            return string.Empty;
        }

        [ApplicationProcessed]
        public void SetValue(string keyword,string value) {
            try {
                this._builder[keyword]=value;
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }
    }
}
