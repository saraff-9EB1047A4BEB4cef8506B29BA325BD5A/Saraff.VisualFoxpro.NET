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
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Saraff.VisualFoxpro.Common.Collections {

    public sealed class SortedBindingList<T> : BindingList<T>, IBindingListView {
        private PropertyDescriptor _sortPropertyCore = null;
        private ListSortDirection _sortDirectionCore = ListSortDirection.Ascending;
        private Collection<T> _collection = new Collection<T>();
        private PropertyDescriptorCollection _properties;
        private string _currentFilterExpression = string.Empty;
        private Dictionary<string, ConditionsType> _conditions;

        public SortedBindingList() : this(new List<T>()) {
        }

        public SortedBindingList(IEnumerable<T> enumerable) : this(new List<T>(enumerable)) {
        }

        public SortedBindingList(IList<T> list) : base(list) {
            foreach(var _val in list) {
                this._collection.Add(_val);
            }
            this._properties = ListBindingHelper.GetListItemProperties(typeof(T));
            this.ListChanged += this._listChanged;
        }

        private void _listChanged(object source, ListChangedEventArgs e) {
            switch(e.ListChangedType) {
                case ListChangedType.ItemAdded:
                    this._collection.Add(this[e.NewIndex]);
                    break;
                case ListChangedType.ItemChanged:
                    var _editedItem = this[e.NewIndex];
                    var _editedItemIndex = this._collection.IndexOf(_editedItem);
                    if(_editedItemIndex > -1) {
                        this._collection[_editedItemIndex] = _editedItem;
                    }
                    break;
            }
        }

        protected override void RemoveItem(int index) {
            var _deletedItem = this[index];
            base.RemoveItem(index);
            this._collection.Remove(_deletedItem);
        }

        protected override bool IsSortedCore {
            get {
                return this._sortPropertyCore != null;
            }
        }

        protected override bool SupportsSearchingCore {
            get {
                return true;
            }
        }

        protected override bool SupportsSortingCore {
            get {
                return true;
            }
        }

        protected override PropertyDescriptor SortPropertyCore {
            get {
                return this._sortPropertyCore;
            }
        }

        protected override ListSortDirection SortDirectionCore {
            get {
                return this._sortDirectionCore;
            }
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction) {
            List<T> _list = this.Items as List<T>;
            if(_list == null) {
                return;
            }
            _list.Sort(new Comparison<T>((T firstValue, T secondValue) => {
                return new Func<int>(() => {
                    object _first = firstValue == null ? null : prop.GetValue(firstValue);
                    object _second = secondValue == null ? null : prop.GetValue(secondValue);
                    if(_first == null) {
                        return (_second == null) ? 0 : -1; //nulls are equal
                    }
                    if(_second == null) {
                        return 1; //first has value, second doesn't
                    }
                    if(_first is IComparable) {
                        return ((IComparable)_first).CompareTo(_second);
                    }
                    throw new InvalidOperationException(string.Format("Тип {0} не реализует интерфейс {1}.", typeof(T).FullName, typeof(IComparable).FullName));
                })() * (direction == ListSortDirection.Descending ? -1 : 1);
            }));

            this._sortPropertyCore = prop;
            this._sortDirectionCore = direction;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, prop));//fire an event that the list has been changed.
        }

        protected override int FindCore(PropertyDescriptor prop, object key) {
            List<T> _list = this.Items as List<T>;
            if(_list == null) {
                throw new InvalidOperationException();
            }
            return _list.FindIndex(new Predicate<T>((T objectValue) => {
                object _value = objectValue == null ? null : prop.GetValue(objectValue);
                if(_value == null) {
                    return key == null; //nulls are equal
                }
                if(_value is IComparable) {
                    return ((IComparable)_value).CompareTo(key) == 0;
                }
                throw new InvalidOperationException(string.Format("Тип {0} не реализует интерфейс {1}.", typeof(T).FullName, typeof(IComparable).FullName));
            }));
        }

        protected override void RemoveSortCore() {
        }

        #region IBindingListView

        /// <summary>
        /// Gets or sets the filter to be used to exclude items from the collection of items returned by the data source
        /// </summary>
        /// <value></value>
        /// <returns>The string used to filter items out in the item collection returned by the data source. </returns>
        public string Filter {
            get {
                return _currentFilterExpression;
            }
            set {
                if(this._currentFilterExpression != value) {
                    this._AplyFilterCore(value);
                    this._currentFilterExpression = value;
                }
            }
        }

        /// <summary>
        /// Removes the current filter applied to the data source.
        /// </summary>
        public void RemoveFilter() {
            this.Filter = string.Empty;
        }

        /// <summary>
        /// Gets a value indicating whether the data source supports filtering.
        /// </summary>
        /// <value></value>
        /// <returns>true if the data source supports filtering; otherwise, false. </returns>
        public bool SupportsFiltering {
            get {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the data source supports advanced sorting.
        /// </summary>
        /// <value></value>
        /// <returns>true if the data source supports advanced sorting; otherwise, false. </returns>
        public bool SupportsAdvancedSorting {
            get {
                return false;
            }
        }

        #region Not Implemented

        /// <summary>
        /// Gets the collection of sort descriptions currently applied to the data source.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.ComponentModel.ListSortDescriptionCollection"/> currently applied to the data source.</returns>
        public ListSortDescriptionCollection SortDescriptions {
            get {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sorts the data source based on the given <see cref="T:System.ComponentModel.ListSortDescriptionCollection"/>.
        /// </summary>
        /// <param name="sorts">The <see cref="T:System.ComponentModel.ListSortDescriptionCollection"/> containing the sorts to apply to the data source.</param>
        public void ApplySort(ListSortDescriptionCollection sorts) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region Filter

        /// <summary>
        /// Преобразует строку с условиями для фильтрации в словарь, в котором ключом явялется свойство класса Т, а значением - ограничение для него.
        /// </summary>
        /// <param name="filterString">Строка содержащая условия фильтрации.</param>
        /// <returns>Словарь в котором ключом явялется свойство класса Т, а значением ограничение для него.</returns>
        private Dictionary<PropertyDescriptor, PropertiesOfCondition> _Parser(string filterString) {
            if(!String.IsNullOrEmpty(filterString)) {
                var _result = new Dictionary<PropertyDescriptor, PropertiesOfCondition>();
                foreach(var _element in filterString.Split(new string[] { "AND" }, StringSplitOptions.None)) {
                    if(_element != null) {
                        var _expr = _element.Split(new List<string>(this.DictionaryOfConditions.Keys).ToArray(), StringSplitOptions.None);
                        if(_expr.Length != 2) {
                            throw new InvalidOperationException("Ошибка в выражении фильтра.");
                        }
                        _result.Add(
                            this._properties.Find(_expr[0].Trim(), true),
                            new PropertiesOfCondition {
                                ConditionType = this.DictionaryOfConditions[_element.Substring(_expr[0].Length, _element.Length - _expr[0].Length - _expr[1].Length)],
                                ConditionValue = _expr[1].Trim()
                            });
                    } else {
                        throw new InvalidOperationException("Ошибка в выражении фильтра.");
                    }
                }
                return _result;
            }
            return null;
        }

        /// <summary>
        /// Определяет соответствует ли значение объекта класса T условию заданному в фильтре.
        /// </summary>
        /// <typeparam name="TValue">Тип свойства указанного в фильтре.</typeparam>
        /// <param name="value1">Значение заданное в фильтре.</param>
        /// <param name="value2">Значение свойства для текущего объекта.</param>
        /// <param name="condition">Тип сравнения.</param>
        /// <returns>true если соответсвует, false если нет.</returns>
        private bool _ComparerCore<TValue>(string value1, TValue value2, ConditionsType condition) where TValue : IComparable {
            var _comparigValue = (TValue)Convert.ChangeType(value1, typeof(TValue));
            switch(condition) {
                case ConditionsType.Equal:
                    return value2.CompareTo(_comparigValue) == 0;
                case ConditionsType.Less:
                    return value2.CompareTo(_comparigValue) < 0;
                case ConditionsType.More:
                    return value2.CompareTo(_comparigValue) > 0;
                case ConditionsType.NotEqual:
                    return value2.CompareTo(_comparigValue) != 0;
                case ConditionsType.Like:
                    return typeof(TValue) == typeof(string) && new Func<bool>(() => {
                        return new Regex(string.Format("^{0}$", _comparigValue.ToString().ToUpper().Replace("%", ".*"))).IsMatch(value2.ToString().ToUpper());
                    })();
                default:
                    return false;
            }
        }

        private bool _CompareCore(Type valueType, string value1, object value2, ConditionsType condition) {
            return (bool)typeof(SortedBindingList<T>).GetMethod("_ComparerCore", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new Type[] { valueType }).Invoke(this, new object[] { value1, value2, condition });
        }

        /// <summary>
        /// Отбирает строки соответсвующие фильтру.
        /// </summary>
        /// <param name="value">Строка описывающая фильтр.</param>
        private void _AplyFilterCore(string value) {
            var _dictionaries = this._Parser(value);
            this.Items.Clear();
            if(_dictionaries != null && _dictionaries.Count != 0) {
                foreach(var _item in this._collection) {
                    bool _isValid = true;
                    foreach(var _value in _dictionaries) {
                        if(!(_isValid = this._CompareCore(_value.Key.PropertyType, _value.Value.ConditionValue, _value.Key.GetValue(_item), _value.Value.ConditionType))) {
                            break;
                        }
                    }
                    if(_isValid) {
                        this.Items.Add(_item);
                    }
                }
            } else {
                foreach(var _val in this._collection) {
                    this.Items.Add(_val);
                }
            }
            if(this.SortPropertyCore != null) {
                this.ApplySortCore(this.SortPropertyCore, this.SortDirectionCore);
            }
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
        }

        /// <summary>
        /// Свойство возвращающее словарь, в качестве ключа которого используется строковое представление условия сравнения, 
        /// а в качестве значения - элемент перечисления ConditionsType.
        /// </summary>
        private Dictionary<string, ConditionsType> DictionaryOfConditions {
            get {
                if(_conditions == null) {
                    _conditions = new Dictionary<string, ConditionsType> {
                            {"=",ConditionsType.Equal},
                            {"<",ConditionsType.Less},
                            {">",ConditionsType.More},
                            {"!=",ConditionsType.NotEqual},
                            {"LIKE",ConditionsType.Like}
                        };
                }
                return _conditions;
            }
        }

        /// <summary>
        /// Перечисление, содержащиее список поддерживаемых условий для сравнения.
        /// </summary>
        private enum ConditionsType {
            More,
            Less,
            Equal,
            NotEqual,
            Like
        }

        /// <summary>
        /// Класс содержащий значение свойства из фильтра и условие по которому оно сравнивается со значениями свойств объекта класса Т.
        /// </summary>
        private sealed class PropertiesOfCondition {

            public string ConditionValue {
                get;
                set;
            }

            public ConditionsType ConditionType {
                get;
                set;
            }
        }

        #endregion
    }
}
