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

    public sealed class prikaz:VfpExternalForm<Saraff.VisualFoxpro.Externals.Forms.prikaz.Result> {

        public bool Invoke(out Result result) {
            return base.Invoke(out result,"R");
        }

        public bool Invoke(out Result result,string type) {
            return base.Invoke(out result,type);
        }

        protected override string FormName {
            get {
                return "prikaz";
            }
        }

        protected override bool IsSuccess(object value,out Result result) {
            result=new Result();
            if(value!=null&&!value.Equals("False")&&!value.Equals(DBNull.Value)) {
                string[] _temp=value.ToString().Split(',');
                result=new Result() {
                    OrderId=int.Parse(_temp[0]),
                    OrderType=char.Parse(_temp[1]),
                    OrderDate=DateTime.Parse(_temp[2])
                };
                return true;
            }
            return false;
        }

        public class Result {

            public int OrderId {
                get;
                set;
            }

            public char OrderType {
                get;
                set;
            }

            public DateTime OrderDate {
                get;
                set;
            }
        }
    }
}
