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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Saraff.VisualFoxpro.Common.Collections {

    public static class EnumerableUtil {

        public static IEnumerable<TResult> Cast<TResult>(IEnumerable source) where TResult : class {
            foreach(var _value in source) {
                yield return _value as TResult;
            }
            yield break;
        }

        public static TSource First<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate = null) {
            foreach(var _item in source) {
                if(predicate == null || predicate(_item)) {
                    return _item;
                }
            }
            throw new InvalidOperationException();
        }

        public static TSource FirstOrDefault<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate = null) {
            foreach(var _item in source) {
                if(predicate == null || predicate(_item)) {
                    return _item;
                }
            }
            return default(TSource);
        }

        public static TSource Last<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate = null) {
            var _result = new List<TSource>();
            foreach(var _item in source) {
                if(predicate == null || predicate(_item)) {
                    _result.Add(_item);
                }
            }
            if(_result.Count>0) {
                return _result[_result.Count - 1];
            }
            throw new InvalidOperationException();
        }

        public static TSource LastOrDefault<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate = null) {
            var _result = default(TSource);
            foreach(var _item in source) {
                if(predicate == null || predicate(_item)) {
                    _result = _item;
                }
            }
            return _result;
        }

        public static TSource ElementAt<TSource>(IEnumerable<TSource> source, int index) {
            var i = 0;
            foreach(var _item in source) {
                if(i == index) {
                    return _item;
                }
                i++;
            }
            throw new InvalidOperationException();
        }

        public static TSource ElementAtOrDefault<TSource>(IEnumerable<TSource> source, int index) {
            var i = 0;
            foreach(var _item in source) {
                if(i == index) {
                    return _item;
                }
                i++;
            }
            return default(TSource);
        }

        public static IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate) {
            foreach(var _item in source) {
                if(predicate(_item)) {
                    yield return _item;
                }
            }
            yield break;
        }

        public static int Count<TSource>(IEnumerable<TSource> source) {
            return EnumerableUtil.Count(source, x => true);
        }

        public static int Count<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate) {
            var _result = 0;
            foreach(var _item in source) {
                if(predicate(_item)) {
                    _result++;
                }
            }
            return _result;
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector) {
            foreach(var _item in source) {
                yield return selector(_item);
            }
            yield break;
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector) {
            foreach(var _item in source) {
                foreach(var _result in selector(_item)) {
                    yield return _result;
                }
            }
            yield break;
        }

        public static void Each<TSource>(IEnumerable<TSource> source, Action<TSource> action) {
            foreach(var _item in source) {
                action(_item);
            }
        }

        public static int Find<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate) {
            var _index = -1;

            EnumerableUtil.First(source, x => {
                _index++;
                return predicate(x);
            });

            return _index;
        }

        public static int Find<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate, int defaultValue) {
            var _index = -1;

            if(EnumerableUtil.FirstOrDefault(source, x => {
                _index++;
                return predicate(x);
            }) == null) {
                return defaultValue;
            }

            return _index;
        }

        public static TSource[] ToArray<TSource>(IEnumerable<TSource> source) {
            return EnumerableUtil.ToList(source).ToArray();
        }

        public static List<TSource> ToList<TSource>(IEnumerable<TSource> source) {
            return new List<TSource>(source);
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) where TKey : IComparable<TKey> {
            return EnumerableUtil.SelectMany(outer, o => EnumerableUtil.Select(
                    EnumerableUtil.Where(inner, i => outerKeySelector(o).CompareTo(innerKeySelector(i)) == 0), 
                    i => resultSelector(o, i)));
        }

        private static IEnumerable<TSource> OrderBy<TSource>(IEnumerable<TSource> source, ICollection<Comparison<TSource>> comparisons) {
            var _result = EnumerableUtil.ToList(source);
            _result.Sort((x, y) => {
                foreach(var _comparison in comparisons) {
                    var _order = _comparison(x, y);
                    if(_order != 0) {
                        return _order;
                    }
                }
                return 0;
            });
            return _result;
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey> {
            return new EnumerableUtil._OrderedEnumerable<TSource>(source, (x, y) => keySelector(x).CompareTo(keySelector(y)));
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey> {
            return source.CreateOrderedEnumerable((x, y) => keySelector(x).CompareTo(keySelector(y)));
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey> {
            return new EnumerableUtil._OrderedEnumerable<TSource>(source, (x, y) => keySelector(y).CompareTo(keySelector(x)));
        }

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey> {
            return source.CreateOrderedEnumerable((x, y) => keySelector(y).CompareTo(keySelector(x)));
        }

        public static IEnumerable<TSource> Except<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer = null) {
            return EnumerableUtil.Where(first, x => EnumerableUtil.FirstOrDefault(second, y => comparer != null ? comparer.Equals(x, y) : x.Equals(y)) == null);
        }

        public static IEnumerable<TSource> Union<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer = null) {
            return EnumerableUtil.Distinct(EnumerableUtil.UnionAll(first, second), comparer);
        }

        public static IEnumerable<TSource> UnionAll<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second) {
            foreach(var _item in first) {
                yield return _item;
            }
            foreach(var _item in second) {
                yield return _item;
            }
            yield break;
        }

        public static IEnumerable<TSource> Distinct<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer = null) {
            var _result = new List<TSource>();
            EnumerableUtil.Each(source, x => {
                if(EnumerableUtil.FirstOrDefault(_result, y => comparer != null ? comparer.Equals(x, y) : x.Equals(y)) == null) {
                    _result.Add(x);
                }
            });
            return _result;
        }

        public static IEnumerable<TSource> Take<TSource>(IEnumerable<TSource> source, int count) {
            var i = 0;
            foreach(var _item in source) {
                if(++i > count) {
                    break;
                }
                yield return _item;
            }
            yield break;
        }

        public static IEnumerable<TSource> Skip<TSource>(IEnumerable<TSource> source, int count) {
            var i = 0;
            foreach(var _item in source) {
                if(++i > count) {
                    yield return _item;
                }
            }
            yield break;
        }

        public static IEqualityComparer<T> CreateComparer<T>(Func<T, T, bool> equals = null, Func<T, int> hash = null) {
            return new _EqualityComparer<T>(equals, hash);
        }

        public interface IOrderedEnumerable<T> : IEnumerable<T> {

            IOrderedEnumerable<T> CreateOrderedEnumerable(Comparison<T> comparer);
        }

        private sealed class _OrderedEnumerable<T> : IOrderedEnumerable<T> {
            private IEnumerable<T> _source = null;
            private ICollection<Comparison<T>> _comparisons;

            private _OrderedEnumerable() {
            }

            public _OrderedEnumerable(IEnumerable<T> source, Comparison<T> comparison) {
                this._source = source;
                this._comparisons = new List<Comparison<T>> { comparison };
            }

            public IOrderedEnumerable<T> CreateOrderedEnumerable(Comparison<T> comparer) {
                var _comparisons = new List<Comparison<T>>(this._comparisons);
                _comparisons.Add(comparer);
                return new _OrderedEnumerable<T> { _source = this._source, _comparisons = _comparisons };
            }

            public IEnumerator<T> GetEnumerator() {
                return EnumerableUtil.OrderBy(this._source, this._comparisons).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return EnumerableUtil.OrderBy(this._source, this._comparisons).GetEnumerator();
            }
        }

        private sealed class _EqualityComparer<T> : IEqualityComparer<T> {
            public Func<T, T, bool> _equals = null;
            public Func<T, int> _hash = null;

            public _EqualityComparer(Func<T, T, bool> equals = null, Func<T, int> hash = null) {
                this._equals = equals;
                this._hash = hash;
            }

            public bool Equals(T x, T y) {
                return this._equals != null ? this._equals(x, y) : x.Equals(y);
            }

            public int GetHashCode(T obj) {
                return this._hash != null ? this._hash(obj) : obj.GetHashCode();
            }
        }
    }
}
