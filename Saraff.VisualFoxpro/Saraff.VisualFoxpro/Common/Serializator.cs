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
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Saraff.VisualFoxpro.Common {

    public sealed class Serializator {

        /// <summary>
        /// Сериализует объект в массив байт.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Массив байт.</returns>
        public static byte[] SerializeToArray(object obj) {
            if(obj==null) {
                throw new ArgumentNullException("","Сериализуемый объект не должен быть равным null.");
            }
            using(var _stream=new MemoryStream()) {
                new BinaryFormatter().Serialize(_stream,obj);
                return _stream.ToArray();
            }
        }

        /// <summary>
        /// Десериализует объект из массив байт.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Массив байт.</param>
        /// <returns>Объект.</returns>
        public static T DeserializeFromArray<T>(byte[] obj) where T:class {
            return Serializator.DeserializeFromArray(obj) as T;
        }

        /// <summary>
        /// Десериализует объект из массив байт.
        /// </summary>
        /// <param name="obj">Массив байт.</param>
        /// <returns>Объект.</returns>
        public static object DeserializeFromArray(byte[] obj) {
            if(obj == null) {
                throw new ArgumentNullException("", "Десериализуемый массив не может быть null.");
            }
            if(obj.Length == 0) {
                throw new ArgumentException("Длинна десереализуемого массвива не может быть равна 0.");
            }
            using(var _stream = new MemoryStream(obj)) {
                AppDomain.CurrentDomain.AssemblyResolve += Serializator.currentDomain_AssemblyResolve;
                try {
                    return new BinaryFormatter().Deserialize(_stream);
                } finally {
                    AppDomain.CurrentDomain.AssemblyResolve -= currentDomain_AssemblyResolve;
                }
            }
        }

        /// <summary>
        /// Создаёт копию объекта.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Копируемый объект.</param>
        /// <returns>Копия объекта.</returns>
        public static T Copy<T>(T obj) where T:class {
            if(obj==null) {
                throw new ArgumentNullException("","Копируемый объект не может быть null.");
            }
            return Serializator.DeserializeFromArray<T>(Serializator.SerializeToArray(obj));
        }

        private static Assembly currentDomain_AssemblyResolve(object sender,ResolveEventArgs args) {
            return Assembly.Load(args.Name);
        }
    }
}
