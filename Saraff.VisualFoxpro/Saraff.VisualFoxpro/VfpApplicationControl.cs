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
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using Saraff.AxHost;
using Saraff.VisualFoxpro.Core;
using Saraff.VisualFoxpro.IoC;
using _IoC = Saraff.IoC;

namespace Saraff.VisualFoxpro {

    public class VfpApplicationControl:ApplicationControl {
        private VfpEventHandlerProxy _eventHandlerProxy;
        private _IoC.ServiceContainer _services=new _IoC.ServiceContainer();
        private ProxyService _proxy;

        public VfpApplicationControl() {

            #region Подписываемся на события, отмеченные аттрибутом VfpProcessedAttribute

            foreach(EventInfo _event in this.GetType().GetEvents(BindingFlags.Public|BindingFlags.Instance)) {
                if(_event.GetCustomAttributes(typeof(VfpProcessedAttribute), false).Length>0) {
                    _EventDispatcher.AddEventDispatcher(this, _event, this._DefaultEventHandler);
                }
            }

            #endregion

            this._services.Bind<_IoC.IConfiguration, _Configuration>();
            this._services.Bind<IInstanceFactory>(this._services.CreateInstance<_InstanceFactory>(x => x("container", this._services)));
            this._services.Bind<IBinder>(this._services.CreateInstance<_Binder>(x => x("container", this._services)));
            this._services.Bind<IProxy>(this._proxy = this._services.CreateInstance<ProxyService>(x => x("handler", new EventHandler(this._ExternalInvokeHandler))));
            this._services.Bind(typeof(IWin32Window),this);

            this.Externals=new Dictionary<Type, _VfpExternalComponent>();

            #region Добавляем требуемые внешние компоненты

            foreach(VfpExternalRequiredAttribute _attr in this.GetType().GetCustomAttributes(typeof(VfpExternalRequiredAttribute), false)) {
                this.Externals.Add(_attr.Type, this._services.CreateInstance(_attr.Type) as _VfpExternalComponent);
            }

            #endregion

            this._services.Load(this.GetType().Assembly);
        }

        [ApplicationProcessed]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void VfpSetEventHandler_23469B5EFE5C4EB5B250FD5DF5C7869D(object handler) {
            try {
                this.EventHandlerProxy=VfpEventHandlerProxy.Create(handler as AxHostComponent);
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        protected override void Construct(ReadOnlyCollection<object> args) {
            try {
                this.ComponentParameters=args;
                this._DelayUntilReboot();
            } catch {
            }
            base.Construct(args);
        }

        protected override void Dispose(bool disposing) {
            if(disposing&&this._services!=null) {
                foreach(IDisposable _component in this._services.Components) {
                    if(_component is _VfpExternalComponent) {
                        _component.Dispose();
                    }
                }
                foreach(var _item in this._services.Components) {
                    if(_item is Control && (_item as Control).Parent != null) {
                        this._services.Remove(_item as Control);
                    }
                }
                this._services.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Returns an object that represents a service provided by the System.ComponentModel.Component
        /// or by its System.ComponentModel.Container.
        /// </summary>
        /// <param name="service">A service provided by the System.ComponentModel.Component.</param>
        /// <returns>
        /// An System.Object that represents a service provided by the System.ComponentModel.Component,
        /// or null if the System.ComponentModel.Component does not provide the specified
        /// service.
        /// </returns>
        protected override object GetService(Type service) {
            foreach(ServiceRequiredAttribute _attr in this.GetType().GetCustomAttributes(typeof(ServiceRequiredAttribute),true)) {
                for(var _provider = this._services as IServiceProvider; _provider != null && _attr.Service == service;) {
                    return _provider.GetService(service);
                }
            }
            return base.GetService(service);
        }

        protected virtual void OnEventHandlerProxyChanged(EventArgs e) {
            this._proxy.OnProxyChanged(EventArgs.Empty);
        }

        protected void ErrorMessageBox(Exception ex) {
            try {
                for(var _handler = (this._services as IServiceProvider).GetService(typeof(IErrorHandler)) as IErrorHandler; _handler != null;) {
                    _handler.Invoke(ex);
                    return;
                }
            } catch(Exception _ex) {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}============={0}{В процессе обработки ошибки произошло исключение.}{0}{1}: {2}{0}{3}{0}============={0}", Environment.NewLine,_ex.GetType().Name,_ex.Message,_ex.StackTrace));
            }
            this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(ex));
        }

        protected void ErrorMessageBox(string message) {
            this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(message, "Error"));
        }

        protected void WarningMessageBox(string message) {
            this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(message, "Warning"));
        }

        private void OnErrorHandlerRequired(ErrorHandlerRequiredEventArgs e) {
            if(this.VfpErrorHandlerRequired_B96C8831F80941689F8F05DC019E453F!=null) {
                this.VfpErrorHandlerRequired_B96C8831F80941689F8F05DC019E453F(this, e);
            }
        }

        private void _DelayUntilReboot() {
            var _dir=new DirectoryInfo(Path.GetDirectoryName(this.GetType().Assembly.Location));
            foreach(var _file in _dir.GetFiles()) {
                Win32.MoveFileEx(_file.FullName, null, Win32.MoveFileFlags.DelayUntilReboot);
            }
            Win32.MoveFileEx(_dir.FullName, null, Win32.MoveFileFlags.DelayUntilReboot);
        }

        #region Свойства

        private VfpEventHandlerProxy EventHandlerProxy {
            get {
                return this._eventHandlerProxy;
            }
            set {
                this._eventHandlerProxy=value;
                this.OnEventHandlerProxyChanged(EventArgs.Empty);
            }
        }

        protected ReadOnlyCollection<object> ComponentParameters {
            get;
            private set;
        }

        protected Dictionary<Type, _VfpExternalComponent> Externals {
            get;
            private set;
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обработчик по умолчанию для событий пользовательского элемента управления.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void _DefaultEventHandler(object sender, _DispatchEventArgs e) {
            if(this.EventHandlerProxy!=null) {
                this.EventHandlerProxy.Send(new VfpEventHandler.VfpEventArgs(new AxHost.EventDescriptor(e.EventName, e.EventArgs)));
            }
        }

        /// <summary>
        /// Обработчик вызовов от компонентов, управляющих внешними элементами.
        /// </summary>
        /// <param name="sender">Источник.</param>
        /// <param name="args">Аргументы.</param>
        private void _ExternalInvokeHandler(object sender, EventArgs args) {
            if(this.VfpExternalRequired_E579B5882EA4411CA4D5DE57E72D0F3B!=null) {
                this.VfpExternalRequired_E579B5882EA4411CA4D5DE57E72D0F3B(this, args);
            }
        }

        #endregion

        #region События

        [VfpProcessed]
        [Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler VfpExternalRequired_E579B5882EA4411CA4D5DE57E72D0F3B;

        [VfpProcessed]
        [Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler VfpErrorHandlerRequired_B96C8831F80941689F8F05DC019E453F;

        #endregion

        #region Вложенные типы

        /// <summary>
        /// Используется для подписки на события пользовательского элемента управления.
        /// </summary>
        [ComVisible(false)]
        private sealed class _EventDispatcher {
            private string _event_name;
            private EventHandler<_DispatchEventArgs> _handler;

            /// <summary>
            /// Initializes a new instance of the <see cref="_EventDispatcher"/> class.
            /// </summary>
            /// <param name="target">Ссылка на целевой объект.</param>
            /// <param name="eventInfo">Описание события.</param>
            /// <param name="handler">Обработчик.</param>
            private _EventDispatcher(object target, EventInfo eventInfo, EventHandler<_DispatchEventArgs> handler) {
                this._event_name=eventInfo.Name;
                this._handler=handler;
                eventInfo.AddEventHandler(target, new EventHandler(this._Dispatch));
            }

            /// <summary>
            /// Обработчик события.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            private void _Dispatch(object sender, EventArgs e) {
                this._handler(this, new _DispatchEventArgs(this._event_name, sender, e));
            }

            /// <summary>
            /// Добавляет обработчик события.
            /// </summary>
            /// <param name="target">Ссылка на целевой объект.</param>
            /// <param name="eventInfo">Описание события.</param>
            /// <param name="handler">Обработчик.</param>
            /// <returns>Instance of the <see cref="_EventDispatcher"/> class.</returns>
            public static _EventDispatcher AddEventDispatcher(object target, EventInfo eventInfo, EventHandler<_DispatchEventArgs> handler) {
                return new _EventDispatcher(target, eventInfo, handler);
            }
        }

        /// <summary>
        /// Класс аргументов диспетчера событий.
        /// </summary>
        [ComVisible(false)]
        private sealed class _DispatchEventArgs:EventArgs {

            /// <summary>
            /// Initializes a new instance of the <see cref="_DispatchEventArgs"/> class.
            /// </summary>
            /// <param name="eventName">Name of the event.</param>
            /// <param name="sender">The sender.</param>
            /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
            public _DispatchEventArgs(string eventName, object sender, EventArgs eventArgs) {
                this.EventName=eventName;
                this.Sender=sender;
                this.EventArgs=eventArgs;
            }

            /// <summary>
            /// Gets the name of the event.
            /// </summary>
            /// <value>The name of the event.</value>
            public string EventName {
                get;
                private set;
            }

            /// <summary>
            /// Gets the sender.
            /// </summary>
            /// <value>The sender.</value>
            public object Sender {
                get;
                private set;
            }

            /// <summary>
            /// Gets the event args.
            /// </summary>
            /// <value>The event args.</value>
            public EventArgs EventArgs {
                get;
                private set;
            }
        }

        #endregion
    }
}
