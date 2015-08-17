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

namespace Saraff.VisualFoxpro {

    public abstract class VfpExternalObject:_VfpExternalComponent {

        protected VfpExternalObject() {
            this.Instance=string.Format("m_clr_{0}",Guid.NewGuid().ToString("N"));
        }

        public virtual void Create(params object[] args) {
            this.InvokeObjectCore(this.ClassName, this.Instance, args);
        }

        protected virtual object InvokeMethod(string methodName, params object[] args) {
            return this.InvokeMethodCore(this.Instance, methodName, args);
        }

        protected virtual object GetPropertyValue(string propertyName) {
            return this.GetPropertyCore(this.Instance, propertyName);
        }

        protected virtual void SetPropertyValue(string propertyName, object value) {
            this.SetPropertyCore(this.Instance, propertyName, value);
        }

        private void Release() {
            this.InvokeMethodCore(this.Instance, "RELEASE_DB5D35D29C4C46C0B5EC0450FDA3351B", new object[0]);
        }

        protected override void Dispose(bool disposing) {
            if(disposing) {
                try {
                    this.Release();
                } catch {
                }
            }
            base.Dispose(disposing);
        }

        protected abstract string ClassName {
            get;
        }

        protected virtual string Instance {
            get;
            private set;
        }
    }
}
