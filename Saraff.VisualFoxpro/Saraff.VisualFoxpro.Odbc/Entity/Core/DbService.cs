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
using System.ComponentModel;
using System.Data;
using System.Text;
using Saraff.VisualFoxpro.Common;
using Saraff.VisualFoxpro.Common.Collections;
using Saraff.VisualFoxpro.Odbc;
using IoC = Saraff.VisualFoxpro.IoC;

namespace Saraff.VisualFoxpro.Odbc.Entity.Core {

    public abstract class DbService<T> : Component, IDbService<T> where T : class {

        private TResult _Execute<TResult>(string query, IEnumerable<object> args, Func<IDbCommand, TResult> callback = null) {
            try {
                using(var _command = this.Connection.CreateCommand()) {
                    _command.CommandText = query;
                    EnumerableUtil.Each(args ?? new object[0], x => {
                        var _out = x as OutDbArg;
                        if(_out != null) {
                            _command.Parameters.Add(_out.DataParameter);
                        } else {
                            _command.Parameters.AddWithValue("?", x ?? DBNull.Value);
                        }
                    });
                    return callback != null ? callback.Invoke(_command) : default(TResult);
                }
            } finally {
                this.Listener?.Execute(query, EnumerableUtil.ToArray(args ?? new object[0]));
            }
        }

        protected int Execute(string query, params object[] args) {
            return this._Execute(query, args, x => x.ExecuteNonQuery());
        }

        protected T ToEntity(string query, params object[] args) {
            return this._Execute(query, args, x => {
                using(var _reader = x.ExecuteReader()) {
                    if(_reader.Read()) {
                        return this.ToEntity(_reader);
                    }
                    return default(T);
                }
            });
        }

        protected IEnumerable<T> ToEnumerable(string query, object[] args = null, params Common.Action<T>[] rowCallbacks) {
            return this._Execute(query, args, x => {
                using(var _reader = x.ExecuteReader()) {
                    return this.ToEnumerable(_reader, rowCallbacks);
                }
            });
        }

        #region IDbService<T>

        public object GetObjectFromId(params object[] id) {
            return this.GetFromId(id);
        }

        public T GetFromId(params object[] id) {
            EnumerableUtil.Each(
                EnumerableUtil.Where(id, x => x == null || x.Equals(DBNull.Value)),
                x => {
                    throw new InvalidOperationException($"Значение ключа для {typeof(T).FullName} не может быть null.");
                });

            var _res = this.ToEntity(this.SqlQuery, id);
            if(_res == null) {
                var _key = string.Empty;
                EnumerableUtil.Each(id, x => _key += $" {x};");
                throw new InvalidOperationException($"Не удалось найти данные для заданного ключа ({typeof(T).FullName} where id ={_key}).");
            }
            return _res;
        }

        public abstract T ToEntity(IDataReader reader);

        public IEnumerable<T> ToEnumerable(IDataReader reader, params Common.Action<T>[] rowCallbacks) {
            var _result = new List<T>();
            while(reader.Read()) {
                var _item = this.ToEntity(reader);
                EnumerableUtil.Each(rowCallbacks, x => x(_item));
                _result.Add(_item);
            }
            return _result;
        }

        #endregion

        protected abstract string SqlQuery {
            get;
        }

        [IoC.ServiceRequired]
        public IVfpOdbcConnection Connection {
            get; set;
        }

        [IoC.ServiceRequired]
        public IQueryListener Listener {
            get;
            set;
        }
    }

    internal sealed class OutDbArg {

        private OutDbArg() {
        }

        public static OutDbArg Create(int size, Type type) {
            var _result = new OutDbArg {
                DataParameter = new System.Data.Odbc.OdbcParameter() { ParameterName = "?", Direction = ParameterDirection.Output, Size = size }
            };

            if(type == typeof(DateTime)) {
                _result.DataParameter.DbType = DbType.DateTime;
            }

            return _result;
        }

        public IDataParameter DataParameter {
            get;
            private set;
        }

        public object Value {
            get {
                return this.DataParameter?.Value;
            }
        }

        public override string ToString() {
            return $"[OUT]: {this.Value ?? "null"}";
        }
    }
}
