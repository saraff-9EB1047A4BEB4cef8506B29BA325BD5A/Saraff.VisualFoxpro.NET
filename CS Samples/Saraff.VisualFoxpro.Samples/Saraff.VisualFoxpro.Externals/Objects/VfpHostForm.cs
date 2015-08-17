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
using System.Drawing;
using Saraff.VisualFoxpro.Core;
using Saraff.AxHost;
using System.IO;
using System.Drawing.Imaging;

namespace Saraff.VisualFoxpro.Externals.Objects {

    public sealed class VfpHostForm:VfpExternalObject {

        public void Close() {
            this.InvokeMethod("ExitFromForm");
        }

        public void Close(object result) {
            this.SetPropertyValue("ReturnValue", result);
            this.Close();
        }

        public void Hide() {
            this.InvokeMethod("Hide");
        }

        public void SetStatusMessage(string message) {
            this.InvokeCodeCore(string.Format("SET MESSAGE TO \"{0}\"", message), new object[0]);
        }

        public bool MaxButton {
            get {
                return (bool)this.GetPropertyValue("MaxButton");
            }
            set {
                this.SetPropertyValue("MaxButton", value);
            }
        }

        public bool MinButton {
            get {
                return (bool)this.GetPropertyValue("MinButton");
            }
            set {
                this.SetPropertyValue("MinButton", value);
            }
        }

        public int Width {
            get {
                return (int)this.GetPropertyValue("Width");
            }
            set {
                this.SetPropertyValue("Width", value);
            }
        }

        public int Height {
            get {
                return (int)this.GetPropertyValue("Height");
            }
            set {
                this.SetPropertyValue("Height", value);
            }
        }

        public Size Size {
            get {
                return new Size(this.Width, this.Height);
            }
            set {
                this.Width=value.Width;
                this.Height=value.Height;
            }
        }

        public string Title {
            get {
                return (string)this.GetPropertyValue("Caption");
            }
            set {
                this.SetPropertyValue("Caption", value);
            }
        }

        public Icon Icon {
            get {
                return new Icon((string)this.GetPropertyValue("Icon"));
            }
            set {
                var _path=String.Empty;
                using(var _stream=System.IO.File.Create(_path=System.IO.Path.GetTempFileName())) {
                    value.Save(_stream);
                }
                Core.Win32.MoveFileEx(_path,null,Core.Win32.MoveFileFlags.DelayUntilReboot);
                this.SetPropertyValue("Icon",_path);
            }
        }

        public object this[string name] {
            get {
                return this.GetPropertyValue(name);
            }
            set {
                this.SetPropertyValue(name, value);
            }
        }

        protected override string ClassName {
            get {
                throw new NotSupportedException();
            }
        }

        protected override string Instance {
            get {
                return "this.Parent";
            }
        }

        public override System.ComponentModel.ISite Site {
            get {
                return base.Site;
            }
            set {
                base.Site=value;

                var _proxy=this.GetService(typeof(IProxy)) as IProxy;
                if(_proxy!=null) {
                    _proxy.ProxyChanged+=_ProxyChanged;
                }
            }
        }

        private void _ProxyChanged(object sender, EventArgs e) {
            var _control=this.GetService(typeof(VfpApplicationControl)) as VfpApplicationControl;
            if(_control!=null) {
                foreach(ApplicationControlAttribute _attr in _control.GetType().GetCustomAttributes(typeof(ApplicationControlAttribute), false)) {
                    this.Width=_attr.Width;
                    this.Height=_attr.Height;
                    this.Title=_attr.Title;
                    break;
                }
            }
        }
    }
}
