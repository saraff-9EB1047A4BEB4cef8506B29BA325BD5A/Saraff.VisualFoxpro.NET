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
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using Saraff.AxHost;
using Saraff.VisualFoxpro.Common.Collections;
using Saraff.VisualFoxpro.Core;
using Saraff.VisualFoxpro.IoC;
using _IoC = Saraff.IoC;

namespace Saraff.VisualFoxpro {

    public class VfpApplicationComponent : ApplicationComponent {
        private _IoC.ServiceContainer _services = new _IoC.ServiceContainer();

        public VfpApplicationComponent() {
            this._services.Bind<_IoC.IConfiguration, _Configuration>();
            this._services.Bind<IInstanceFactory>(this._services.CreateInstance<_InstanceFactory>(x => x("container", this._services)));
            this._services.Bind<IBinder>(this._services.CreateInstance<_Binder>(x => x("container", this._services)));
            this._services.Bind<IProxy>(this._services.CreateInstance<ProxyService>(x => x("handler", new EventHandler(this._ExternalInvokeHandler))));

            this._services.Load(this.GetType().Assembly);

            this.Externals = new Dictionary<Type, _VfpExternalComponent>();

            #region Добавляем требуемые внешние компоненты

            foreach(VfpExternalRequiredAttribute _attr in this.GetType().GetCustomAttributes(typeof(VfpExternalRequiredAttribute), false)) {
                this.Externals.Add(_attr.Type, this._services.CreateInstance(_attr.Type) as _VfpExternalComponent);
            }

            #endregion

            #region Внедряем зависимости в открытые свойства

            EnumerableUtil.Each(
                EnumerableUtil.Where(
                    this.GetType().GetProperties(),
                    x => x.IsDefined(typeof(ServiceRequiredAttribute), false)),
                x => x.SetValue(this, this.GetServiceCore(x.PropertyType), null));

            #endregion

            #region Внедряем зависимости в закрытые свойства базовых типов

            for(var _type = this.GetType().BaseType; _type != null && _type != typeof(VfpApplicationControl).BaseType; _type = _type.BaseType) {
                EnumerableUtil.Each(
                    EnumerableUtil.Where(
                        _type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic),
                        x => x.IsDefined(typeof(ServiceRequiredAttribute), false)),
                    x => x.SetValue(this, this.GetServiceCore(x.PropertyType), null));
            }

            #endregion
        }

        protected override void Construct(ReadOnlyCollection<object> args) {
            try {
                this.ComponentParameters = args;
                this._CleanUp();
            } catch {
            }
            base.Construct(args);
        }

        protected override void Dispose(bool disposing) {
            if(disposing && this._services != null) {
                foreach(IDisposable _component in this._services.Components) {
                    if(_component is _VfpExternalComponent) {
                        _component.Dispose();
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
            foreach(ServiceRequiredAttribute _attr in this.GetType().GetCustomAttributes(typeof(ServiceRequiredAttribute), true)) {
                for(var _provider = this._services as IServiceProvider; _provider != null && (_attr.Service.IsGenericType && _attr.Service.GetGenericTypeDefinition() == typeof(IContextBinder<,>) ? _attr.Service.GetGenericArguments()[0] : _attr.Service) == (service.IsGenericType && service.GetGenericTypeDefinition() == typeof(IContextBinder<,>) ? service.GetGenericArguments()[0] : service);) {
                    if(service.IsGenericType && service.GetGenericTypeDefinition() == typeof(IContextBinder<,>)) { // в качестве контекста допустимо использовать только собственный тип
                        return service.GetGenericArguments()[1] != this.GetType() ? null : _provider.GetService(service);
                    }
                    return _provider.GetService(typeof(IContextBinder<,>).MakeGenericType(service, this.GetType())) ?? _provider.GetService(service); // если для текущего типа есть контекстная привязка, то используем ее.
                }
            }
            return base.GetService(service);
        }

        protected void ErrorMessageBox(Exception ex) {
            try {
                this.ErrorHandlerService?.Invoke().Invoke(ex);
            } catch(Exception _ex) {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}============={0}{{В процессе обработки ошибки произошло исключение.}}{0}{1}: {2}{0}{3}{0}============={0}", Environment.NewLine, _ex.GetType().Name, _ex.Message, _ex.StackTrace));
                this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(ex));
            }
        }

        protected void ErrorMessageBox(string message) => this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(message, "Error"));

        protected void WarningMessageBox(string message) => this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(message, "Warning"));

        private void OnErrorHandlerRequired(ErrorHandlerRequiredEventArgs e) => this.VfpErrorHandlerRequired_14173D436F344779B521DC61F955F7BD?.Invoke(this, e);

        /// <summary>
        /// Обработчик вызовов от компонентов, управляющих внешними элементами.
        /// </summary>
        /// <param name="sender">Источник.</param>
        /// <param name="args">Аргументы.</param>
        private void _ExternalInvokeHandler(object sender, EventArgs args) => this.VfpExternalRequired_6BD50AADD7FD4919A200FBC48E9CC28A?.Invoke(this, args);

        private void _CleanUp() {
            var _location = Path.GetDirectoryName(typeof(VfpApplicationComponent).Assembly.Location);
            var _directory = Path.GetFileName(_location);
            if(Path.GetDirectoryName(this.GetType().Assembly.Location) == _location && !File.Exists(Path.Combine(_location, _directory))) {
                EnumerableUtil.Each(
                    Directory.GetDirectories(Path.Combine(_location, "..")),
                    x => {
                        try {
                            if(Path.GetFileName(x) != _directory) {
                                Directory.Delete(x, true);
                            }
                        } catch(Exception ex) {
                            System.Diagnostics.Debug.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                        }
                    }
                );
                using(var _file = File.Create(Path.Combine(_location, _directory))) { // создаем служебный файл для предотвращения повторного удаления
                }
            }
        }

        private object GetServiceCore(Type type) {
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IContextBinder<,>)) {
                return null;
            }
            var _isLazy = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Lazy<>);
            var _callback = (Activator.CreateInstance(typeof(_Lazy<,>).MakeGenericType(this.GetType(), _isLazy ? type.GetGenericArguments()[0] : type), this._services) as _ILazy)?.CreateLazyService();
            return _isLazy ? _callback : _callback.DynamicInvoke();
        }

        #region Свойства

        protected ReadOnlyCollection<object> ComponentParameters {
            get;
            private set;
        }

        protected Dictionary<Type, _VfpExternalComponent> Externals {
            get;
            private set;
        }

        [IoC.ServiceRequired]
        private IoC.Lazy<IErrorHandler> ErrorHandlerService {
            get;
            set;
        }

        #endregion

        #region События

        [ApplicationProcessed]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler VfpExternalRequired_6BD50AADD7FD4919A200FBC48E9CC28A;

        [ApplicationProcessed]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler VfpErrorHandlerRequired_14173D436F344779B521DC61F955F7BD;

        #endregion

        private interface _ILazy {

            Delegate CreateLazyService();
        }

        private sealed class _Lazy<T, TResult> : _ILazy where TResult : class {

            public _Lazy(_IoC.ServiceContainer container) {
                this.Container = container;
            }

            public Delegate CreateLazyService() {
                if(typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(IContextBinder<,>)) {
                    return null;
                }
                return Delegate.CreateDelegate(typeof(Lazy<>).MakeGenericType(typeof(TResult)), this, new Lazy<TResult>(this.GetService).Method);
            }

            private TResult GetService() => (typeof(TResult).IsInterface ? (this.GetService<IContextBinder<TResult, T>>() ?? this.GetService<TResult>()) : this.Container.CreateInstance<TResult>()) as TResult;

            private _IoC.ServiceContainer Container {
                get;
                set;
            }

            private IServiceProvider ServiceProvider => this.Container as IServiceProvider;

            private TResult GetService<TService>() => this.ServiceProvider.GetService(typeof(TService)) as TResult;
        }
    }
}
