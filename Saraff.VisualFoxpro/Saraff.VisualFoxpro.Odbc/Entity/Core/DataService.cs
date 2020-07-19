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
using System.Data;
using System.Reflection;
using System.Text;
using Saraff.VisualFoxpro.Common;
using Saraff.VisualFoxpro.Common.Collections;
using IoC = Saraff.VisualFoxpro.IoC;

namespace Saraff.VisualFoxpro.Odbc.Entity.Core {

    public class DataService<T> : DbService<T>, IDataService<T> where T : class {
        private string _table = null;
        private IEnumerable<_Column> _columns = null;

        #region IDataService

        public T Add(T value) {
            var _cols = EnumerableUtil.ToArray(EnumerableUtil.OrderBy(this._Columns, x => x.IsKey ? x.KeyInfo.Order : -1));

            var _columnSet = string.Empty;
            EnumerableUtil.Each(_cols, x => _columnSet += $"{x.Name}, ");

            var _valueSet = string.Empty;
            var _in = new List<_Column>();
            var _out = new List<_Column>();

            EnumerableUtil.Each(_cols, x => {
                if(x.IsKey) {
                    switch(x.KeyInfo.Method) {
                        case DatabaseGeneratedMethod.MaxInc:
                            _valueSet += $"(SELECT NVL(MAX({x.Name})+1,1) FROM {this._Table}), ";
                            _out.Add(x);
                            break;
                        case DatabaseGeneratedMethod.Select:
                            _valueSet += $"({x.KeyInfo.Expresion}), ";
                            _out.Add(x);
                            break;
                        case DatabaseGeneratedMethod.Seq:
                            _valueSet += $"{x.KeyInfo.Expresion}.NEXTVAL, ";
                            _out.Add(x);
                            break;
                        default:
                            _valueSet += "?, ";
                            _in.Add(x);
                            _out.Add(x);
                            break;
                    }
                } else {
                    _valueSet += "?, ";
                    _in.Add(x);
                }
            });

            var _outSet1 = string.Empty;
            var _outSet2 = string.Empty;
            EnumerableUtil.Each(_out, x => {
                _outSet1 += $"{x.Name}, ";
                _outSet2 += $"?, ";
            });

            var _args =
                EnumerableUtil.ToArray(
                    EnumerableUtil.UnionAll(
                        EnumerableUtil.Select(_in, x => x.GetValue(value)),
                        EnumerableUtil.Select(_out, x => OutDbArg.Create(x.KeyInfo.Size, x.PropertyInfo.PropertyType) as object)));

            this.Execute($@"
                INSERT INTO {this._Table}
                    ({_columnSet.TrimEnd(' ', ',')})
                VALUES ({_valueSet.TrimEnd(' ', ',')})
                RETURNING {_outSet1.TrimEnd(' ', ',')} INTO {_outSet2.TrimEnd(' ', ',')}", _args);

            return this.GetFromId( 
                EnumerableUtil.ToArray(
                    EnumerableUtil.Select(
                        EnumerableUtil.Where(_args, x => x is OutDbArg),
                        x => (x as OutDbArg).Value)));
        }

        public T Edit(T value) {
            var _cols = EnumerableUtil.ToArray(EnumerableUtil.OrderBy(this._Columns, x => x.IsKey ? x.KeyInfo.Order : -1));

            var _predicate = string.Empty;
            EnumerableUtil.Each(EnumerableUtil.Where(_cols, x => x.IsKey), x => _predicate += $"AND {x.Name}=? ");

            var _columnSet = string.Empty;
            EnumerableUtil.Each(EnumerableUtil.Where(_cols, x => !x.IsKey), x => _columnSet += $"{x.Name}=?, ");


            this.Execute(
                $"UPDATE {this._Table} SET {_columnSet.TrimEnd(' ', ',')} WHERE 1=1 {_predicate}", 
                EnumerableUtil.ToArray(
                    EnumerableUtil.Select(_cols, x => x.GetValue(value))));

            return this.GetFromId(
                EnumerableUtil.ToArray(
                    EnumerableUtil.Select(
                        EnumerableUtil.Where(_cols, x => x.IsKey),
                        x => x.GetValue(value))));
        }

        public int Remove(T value) {
            var _keys = 
                EnumerableUtil.ToArray(
                    EnumerableUtil.OrderBy(
                        EnumerableUtil.Where(this._Columns, x => x.IsKey), 
                        x => x.KeyInfo.Order));

            var _predicate = string.Empty;
            EnumerableUtil.Each(_keys, x => _predicate += $"AND {x.Name}=? ");

            return this.Execute(
                $"DELETE FROM {this._Table} WHERE 1=1 {_predicate}", 
                EnumerableUtil.ToArray(
                    EnumerableUtil.Select(_keys, x => x.GetValue(value))));
        }

        public int Remove(params object[] keys) {
            var _keys = new _ColumnWithValue[0];

            EnumerableUtil.Each(keys, y => {
                _keys = EnumerableUtil.ToArray(
                    EnumerableUtil.UnionAll(
                        _keys,
                        EnumerableUtil.Except(
                            EnumerableUtil.Join(
                                EnumerableUtil.Where(this._Columns, x => x.IsKey || x.IsForeignKey),
                                EnumerableUtil.Where(_Column.GetColumns(y.GetType()), x => x.IsKey),
                                x => x.Alias ?? x.Name,
                                x => x.Alias ?? x.Name,
                                (o, i) => new _ColumnWithValue(o, i, y)),
                            _keys,
                            EnumerableUtil.CreateComparer<_ColumnWithValue>((a, b) => a.EntityColumn.Name == b.EntityColumn.Name))));
            });

            var _predicate = string.Empty;
            EnumerableUtil.Each(_keys, x => _predicate += $"AND {x.EntityColumn.Name}=? ");

            return this.Execute(
                $"DELETE FROM {this._Table} WHERE 1=1 {_predicate}",
                EnumerableUtil.ToArray(
                    EnumerableUtil.Select(_keys, x => x.ForeignValue)));
        }

        public IEnumerable<T> Get() {
            return this.ToEnumerable(this._SqlSelect);
        }

        public IEnumerable<T> Get(params object[] keys) {
            var _cols = new _ColumnWithValue[0];

            EnumerableUtil.Each(keys, y => {
                _cols = EnumerableUtil.ToArray(
                    EnumerableUtil.UnionAll(
                        _cols,
                        EnumerableUtil.Except(
                            EnumerableUtil.Join(
                                EnumerableUtil.Where(this._Columns, x => x.IsForeignKey),
                                EnumerableUtil.Where(_Column.GetColumns(y.GetType()), x => x.IsKey),
                                x => x.Alias ?? x.Name,
                                x => x.Alias ?? x.Name,
                                (o, i) => new _ColumnWithValue(o, i, y)),
                            _cols,
                            EnumerableUtil.CreateComparer<_ColumnWithValue>((a, b) => a.EntityColumn.Name == b.EntityColumn.Name))));
            });

            var _predicate = string.Empty;
            EnumerableUtil.Each(_cols, x => _predicate += $"AND {x.EntityColumn.Name}=? ");

            return this.ToEnumerable(
                this._SqlSelect + _predicate,
                EnumerableUtil.ToArray(
                    EnumerableUtil.Select(_cols, x => x.ForeignValue)));
        }

        public T Remodel(object obj) => this.ToEntity(
            this.SqlQuery,
            EnumerableUtil.ToArray(
                EnumerableUtil.Join(
                    EnumerableUtil.OrderBy(
                        EnumerableUtil.Where(this._Columns, x => x.IsKey),
                        x => x.KeyInfo.Order),
                    EnumerableUtil.Distinct(
                        _Column.GetColumns(obj.GetType()),
                        EnumerableUtil.CreateComparer<_Column>((a, b) => (a.Alias ?? a.Name) == (b.Alias ?? b.Name))),
                    x => x.Alias ?? x.Name,
                    x => x.Alias ?? x.Name,
                    (o, i) => i.GetValue(obj))));

        #endregion

        #region _DbService

        public override T ToEntity(IDataReader reader) {
            var _result = this.Factory.CreateInstance<T>();

            EnumerableUtil.Each(this._Columns, x => {
                if(!reader[x.Name].Equals(DBNull.Value)) {
                    if(x.PropertyInfo.PropertyType.IsGenericType && x.PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                        x.PropertyInfo.SetValue(_result, Convert.ChangeType(reader[x.Name], x.PropertyInfo.PropertyType.GetGenericArguments()[0]), null);
                    } else {
                        x.PropertyInfo.SetValue(_result, Convert.ChangeType(reader[x.Name], x.PropertyInfo.PropertyType), null);
                    }
                } else {
                    if(x.PropertyInfo.PropertyType.IsValueType && (!x.PropertyInfo.PropertyType.IsGenericType || x.PropertyInfo.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>))) {
                        throw new InvalidOperationException($"Свойству {x.PropertyInfo.Name} в типе {x.PropertyInfo.ReflectedType.FullName} невозможно присвоить значение null.");
                    }
                }
            });

            EnumerableUtil.Each(
                EnumerableUtil.Distinct(
                    EnumerableUtil.Where(this._Columns, x => x.IsForeignKey && x.ForeignKeyInfo.PropertyInfo != null),
                    EnumerableUtil.CreateComparer<_Column>((x, y) => x.Name.Equals(y.Name))),
                x => {
                    if(x.ForeignKeyInfo.PropertyInfo.PropertyType.IsSubclassOf(typeof(Delegate)) && 
                        x.ForeignKeyInfo.PropertyInfo.PropertyType.IsGenericType && 
                        x.ForeignKeyInfo.PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Func<>)) {

                        var _lazy = this.Factory.CreateInstance(
                            typeof(DataService<>._Lazy<>).MakeGenericType(typeof(T), x.ForeignKeyInfo.PropertyInfo.PropertyType.GetGenericArguments()[0]), 
                            y => y("id", x.ForeignKeyInfo.GetId(_result))) as _ILazyFactory;

                        x.ForeignKeyInfo.PropertyInfo.SetValue(_result,_lazy.Create(),null);

                    } else {
                        x.ForeignKeyInfo.PropertyInfo.SetValue(
                        _result,
                        (this.GetService(typeof(IDbService<>).MakeGenericType(x.ForeignKeyInfo.PropertyInfo.PropertyType)) as IDbService)?.GetObjectFromId(x.ForeignKeyInfo.GetId(_result)),
                        null);
                    }
                });

            return _result;
        }

        protected override string SqlQuery {
            get {
                var _predicate = string.Empty;
                EnumerableUtil.Each(
                    EnumerableUtil.OrderBy(
                        EnumerableUtil.Where(this._Columns, x => x.IsKey),
                        x => x.KeyInfo.Order),
                    x => _predicate += $"AND {x.Name}=? ");

                return this._SqlSelect + _predicate;
            }
        }

        #endregion

        [IoC.ServiceRequired]
        public IoC.IInstanceFactory Factory {
            get;
            set;
        }

        private string _SqlSelect {
            get {
                var _columnList = string.Empty;
                EnumerableUtil.Each(this._Columns, x => _columnList += $"{x.Name}, ");
                return $@"SELECT {_columnList.TrimEnd(' ', ',')} FROM {this._Table} WHERE 1=1 ";
            }
        }

        private string _Table {
            get {
                if(this._table == null) {
                    this._table=(EnumerableUtil.FirstOrDefault(typeof(T).GetCustomAttributes(typeof(TableAttribute), false)) as TableAttribute)?.Name;
                }
                return this._table;
            }
        }

        private IEnumerable<_Column> _Columns {
            get {
                if(this._columns == null) {
                    this._columns = EnumerableUtil.ToList(_Column.GetColumns(typeof(T)));
                }
                return this._columns;
            }
        }

        private sealed class _Column {

            public string Name {
                get;
                set;
            }

            public string Alias {
                get;
                set;
            }

            public bool IsKey {
                get {
                    return this.KeyInfo != null;
                }
            }

            public bool IsForeignKey {
                get {
                    return this.ForeignKeyInfo != null;
                }
            }

            public PropertyInfo PropertyInfo {
                get;
                set;
            }

            public _Key KeyInfo {
                get;
                set;
            }

            public _ForeignKey ForeignKeyInfo {
                get;
                set;
            }

            public object GetValue(object entity) {
                if(this.IsForeignKey && this.ForeignKeyInfo.PropertyInfo != null && !this.ForeignKeyInfo.PropertyInfo.PropertyType.IsSubclassOf(typeof(Delegate))) {
                    var _col = EnumerableUtil.FirstOrDefault(_Column.GetColumns(this.ForeignKeyInfo.PropertyInfo.PropertyType), x => (x.Alias ?? x.Name) == (this.Alias ?? this.Name));
                    if(_col != null) {
                        var _foreignEntity = this.ForeignKeyInfo.PropertyInfo.GetValue(entity, null);
                        if(_foreignEntity != null) {
                            return _col.GetValue(_foreignEntity);
                        }
                    }
                }
                return this.PropertyInfo.GetValue(entity, null);
            }

            public static IEnumerable<_Column> GetColumns(Type type) {
                return
                    EnumerableUtil.Where(
                        EnumerableUtil.Select(type.GetProperties(), x => {
                            var _column = EnumerableUtil.FirstOrDefault(x.GetCustomAttributes(typeof(ColumnAttribute), false)) as ColumnAttribute;
                            var _key = EnumerableUtil.FirstOrDefault(x.GetCustomAttributes(typeof(KeyAttribute), false)) as KeyAttribute;
                            var _foreignKey = EnumerableUtil.FirstOrDefault(x.GetCustomAttributes(typeof(ForeignKeyAttribute), false)) as ForeignKeyAttribute;
                            if(_column != null) {
                                return new _Column {
                                    Name = _column.Name,
                                    Alias = _column.Alias,
                                    PropertyInfo = x,
                                    KeyInfo = _key != null ? new _Key { Method = _key.DatabaseGenerated, Expresion = _key.Expresion, Size = _key.Size, Order = _key.Order } : null,
                                    ForeignKeyInfo = _foreignKey != null ? new _ForeignKey { Name = _foreignKey.Name, PropertyInfo = EnumerableUtil.FirstOrDefault(typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public), p => p.Name == _foreignKey.Name) } : null
                                };
                            }
                            return null;
                        }),
                        x => x != null);
            }
        }

        private sealed class _ColumnWithValue {

            public _ColumnWithValue(_Column entityColumn, _Column foreignColumn, object foreignEntity) {
                this.EntityColumn = entityColumn;
                this.ForeignColumn = foreignColumn;
                this.ForeignEntity = foreignEntity;
            }

            public _Column EntityColumn {
                get;
                private set;
            }

            public _Column ForeignColumn {
                get;
                private set;
            }

            public object ForeignEntity {
                get;
                private set;
            }

            public object ForeignValue {
                get {
                    return this.ForeignColumn.GetValue(this.ForeignEntity);
                }
            }
        }

        private sealed class _Key {

            public DatabaseGeneratedMethod Method {
                get;
                set;
            }

            public string Expresion {
                get;
                set;
            }

            public int Size {
                get;
                set;
            }

            public int Order {
                get;
                set;
            }
        }

        private sealed class _ForeignKey {

            public string Name {
                get;
                set;
            }

            public PropertyInfo PropertyInfo {
                get;
                set;
            }

            public object[] GetId(object entity) {
                return 
                    EnumerableUtil.ToArray(
                        EnumerableUtil.Join(
                            EnumerableUtil.OrderBy(
                                EnumerableUtil.Where(
                                    _Column.GetColumns(this.PropertyInfo.PropertyType.IsSubclassOf(typeof(Delegate)) ? this.PropertyInfo.PropertyType.GetGenericArguments()[0] : this.PropertyInfo.PropertyType),
                                    x => x.IsKey),
                                x => x.KeyInfo.Order),
                            EnumerableUtil.ToList(_Column.GetColumns(this.PropertyInfo.ReflectedType)),
                            x => x.Alias ?? x.Name,
                            x => x.Alias ?? x.Name,
                            (o, i) => i.GetValue(entity)));
            }
        }

        private interface _ILazyFactory {

            Delegate Create();
        }

        [Serializable]
        private sealed class _Lazy<TValue> : _ILazyFactory where TValue : class {
            private object[] _id;
            [NonSerialized]
            private IDbService<TValue> _service;
            private TValue _value;

            [IoC.ServiceRequired]
            public _Lazy(object[] id, IDbService<TValue> service) {
                this._id = id;
                this._service = service;
            }

            TValue Get() {
                if(this._id != null && this._id.Length > 0) {
                    return this._value ?? (this._value = this._service?.GetFromId(this._id));
                }
                return null;
            }

            #region _ILazyFactory

            public Delegate Create() {
                return new Func<TValue>(this.Get);
            }

            #endregion
        }
    }
}
