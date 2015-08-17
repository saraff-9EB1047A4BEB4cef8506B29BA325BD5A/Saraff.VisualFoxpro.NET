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

namespace Saraff.VisualFoxpro.Externals.Forms {

    public sealed class poisk_fam:VfpExternalForm<int> {
        private static Dictionary<System.Reflection.MethodBase, string> _dictionary;
        private System.Reflection.MethodBase _dataKey;

        public override bool Invoke(out int result, params object[] args) {
            this._dataKey=new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            return base.Invoke(out result, args);
        }

        protected override string FormName {
            get {
                return "poisk_fam";
            }
        }

        protected override bool IsSuccess(object value, out int result) {
            var _result=value.ToString().Split(new string[] { ";" }, 2, StringSplitOptions.None);
            if(_result.Length>1&&poisk_fam.Dictionary.ContainsKey(this._dataKey)) {
                poisk_fam.Dictionary[this._dataKey]=_result[1];
            }
            return int.TryParse(_result[0], out result)&&result!=0;
        }

        private static Dictionary<System.Reflection.MethodBase, string> Dictionary {
            get {
                if(poisk_fam._dictionary==null) {
                    poisk_fam._dictionary=new Dictionary<System.Reflection.MethodBase, string>();
                }
                return poisk_fam._dictionary;
            }
        }

        private static string Context {
            get {
                var _key=new System.Diagnostics.StackTrace().GetFrame(2).GetMethod();
                if(!poisk_fam.Dictionary.ContainsKey(_key)) {
                    poisk_fam.Dictionary.Add(_key, string.Empty);
                }
                return poisk_fam.Dictionary[_key];
            }
        }
    }
}
