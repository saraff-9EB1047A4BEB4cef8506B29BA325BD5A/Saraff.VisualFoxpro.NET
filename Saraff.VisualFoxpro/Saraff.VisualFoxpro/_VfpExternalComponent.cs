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
using Saraff.VisualFoxpro.Core;

namespace Saraff.VisualFoxpro {

    public abstract class _VfpExternalComponent:Component {

        protected _VfpExternalComponent() {
        }

        internal T Create<T>() where T:_VfpExternalComponent,new() {
            return new T();
        }

        protected virtual object InvokeFormCore(string name, object[] args) {
            return this.InvokeCore(new VfpExternalFormEventArgs(name, args));
        }

        protected virtual void InvokeProgramCore(string name, object[] args) {
            this.InvokeCore(new VfpExternalProgramEventArgs(name, args));
        }
 
        protected virtual object InvokeObjectCore(string className, string instanceName, object[] args) {
            return this.InvokeCore(new VfpExternalObjectEventArgs(className, instanceName, args));
        }

        protected virtual object InvokeMethodCore(string objectName, string methodName, object[] args) {
            return this.InvokeCore(new VfpExternalMethodEventArgs(objectName, methodName, args));
        }

        protected virtual object GetPropertyCore(string objectName, string propertyName) {
            return this.InvokeCore(new VfpExternalPropertyEventArgs(objectName, propertyName, "Get", null));
        }

        protected virtual void SetPropertyCore(string objectName, string propertyName, object value) {
            this.InvokeCore(new VfpExternalPropertyEventArgs(objectName, propertyName, "Set", value));
        }

        protected virtual object InvokeCodeCore(string vfpCode, object[] args) {
            return this.InvokeCore(new VfpExternalCodeEventArgs(vfpCode, args));
        }

        private object InvokeCore(VfpExternalEventArgs args) {
            var _proxy=this.GetService(typeof(IProxy)) as IProxy;
            if(_proxy==null) {
                throw new InvalidOperationException("Не удалось получить ссылку на экземпляр VfpEventHandlerProxy.");
            }
            _proxy.Send(args);
            return args.ReturnValue;
        }

        #region EventArgs

        private class VfpExternalEventArgs:EventArgs {
            private object[] _args;

            protected VfpExternalEventArgs(string name, string type, object[] args) {
                this.ComponentName=name;
                this.ComponentType=type;
                if(args.Length>16) {
                    throw new ArgumentException("Слишком много параметров.", "args");
                }
                this._args=args;
            }

            /// <summary>
            /// Возвращает наименование компонента приложения.
            /// </summary>
            public string ComponentName {
                get;
                private set;
            }

            /// <summary>
            /// Возвращает тип компонента приложения.
            /// </summary>
            public string ComponentType {
                get;
                private set;
            }

            /// <summary>
            /// Возвращает значение, возвращенное компонентом приложения.
            /// </summary>
            public object ReturnValue {
                get;
                set;
            }

            /// <summary>
            /// Возвращает количество аргументов.
            /// </summary>
            public int ArgsCount {
                get {
                    return this._args.Length;
                }
            }

            #region Аргументы

            /// <summary>
            /// Возвращает 1-ый аргумент.
            /// </summary>
            public object Arg1 {
                get {
                    return this._args.Length<1?null:this._args[0];
                }
            }

            /// <summary>
            /// Возвращает 2-ый аргумент.
            /// </summary>
            public object Arg2 {
                get {
                    return this._args.Length<2?null:this._args[1];
                }
            }

            /// <summary>
            /// Возвращает 3-ый аргумент.
            /// </summary>
            public object Arg3 {
                get {
                    return this._args.Length<3?null:this._args[2];
                }
            }

            /// <summary>
            /// Возвращает 4-ый аргумент.
            /// </summary>
            public object Arg4 {
                get {
                    return this._args.Length<4?null:this._args[3];
                }
            }

            /// <summary>
            /// Возвращает 5-ый аргумент.
            /// </summary>
            public object Arg5 {
                get {
                    return this._args.Length<5?null:this._args[4];
                }
            }

            /// <summary>
            /// Возвращает 6-ый аргумент.
            /// </summary>
            public object Arg6 {
                get {
                    return this._args.Length<6?null:this._args[5];
                }
            }

            /// <summary>
            /// Возвращает 7-ый аргумент.
            /// </summary>
            public object Arg7 {
                get {
                    return this._args.Length<7?null:this._args[6];
                }
            }

            /// <summary>
            /// Возвращает 8-ый аргумент.
            /// </summary>
            public object Arg8 {
                get {
                    return this._args.Length<8?null:this._args[7];
                }
            }

            /// <summary>
            /// Возвращает 9-ый аргумент.
            /// </summary>
            public object Arg9 {
                get {
                    return this._args.Length<9?null:this._args[8];
                }
            }

            /// <summary>
            /// Возвращает 10-ый аргумент.
            /// </summary>
            public object Arg10 {
                get {
                    return this._args.Length<10?null:this._args[9];
                }
            }

            /// <summary>
            /// Возвращает 11-ый аргумент.
            /// </summary>
            public object Arg11 {
                get {
                    return this._args.Length<11?null:this._args[10];
                }
            }

            /// <summary>
            /// Возвращает 12-ый аргумент.
            /// </summary>
            public object Arg12 {
                get {
                    return this._args.Length<12?null:this._args[11];
                }
            }

            /// <summary>
            /// Возвращает 13-ый аргумент.
            /// </summary>
            public object Arg13 {
                get {
                    return this._args.Length<13?null:this._args[12];
                }
            }

            /// <summary>
            /// Возвращает 14-ый аргумент.
            /// </summary>
            public object Arg14 {
                get {
                    return this._args.Length<14?null:this._args[13];
                }
            }

            /// <summary>
            /// Возвращает 15-ый аргумент.
            /// </summary>
            public object Arg15 {
                get {
                    return this._args.Length<15?null:this._args[14];
                }
            }

            /// <summary>
            /// Возвращает 16-ый аргумент.
            /// </summary>
            public object Arg16 {
                get {
                    return this._args.Length<16?null:this._args[15];
                }
            }

            #endregion
        }

        private sealed class VfpExternalFormEventArgs:VfpExternalEventArgs {

            public VfpExternalFormEventArgs(string name, object[] args) : base(name, "Form", args) {
            }
        }

        private sealed class VfpExternalProgramEventArgs:VfpExternalEventArgs {

            public VfpExternalProgramEventArgs(string name, object[] args) : base(name, "Program", args) {
            }
        }

        private sealed class VfpExternalObjectEventArgs:VfpExternalEventArgs {

            public VfpExternalObjectEventArgs(string className, string instanceName, object[] args) : base(className, "Object", args) {
                this.InstanceName=instanceName;
            }

            public string InstanceName {
                get;
                private set;
            }
        }

        private sealed class VfpExternalMethodEventArgs:VfpExternalEventArgs {

            public VfpExternalMethodEventArgs(string objectName, string methodName, object[] args) : base(objectName, "Method", args) {
                this.MethodName=methodName;
            }

            public string MethodName {
                get;
                private set;
            }
        }

        private sealed class VfpExternalPropertyEventArgs:VfpExternalEventArgs {

            public VfpExternalPropertyEventArgs(string objectName, string propertyName, string method, object value) : base(objectName, "Property", new object[] { value }) {
                this.PropertyName=propertyName;
                this.Method=method;
            }

            public string PropertyName {
                get;
                private set;
            }

            public string Method {
                get;
                private set;
            }
        }

        private sealed class VfpExternalCodeEventArgs:VfpExternalEventArgs {

            public VfpExternalCodeEventArgs(string vfpCode, object[] args) : base(string.Empty, "Code", args) {
                this.VfpCode=vfpCode;
            }

            public string VfpCode {
                get;
                private set;
            }
        }

        #endregion
    }
}
