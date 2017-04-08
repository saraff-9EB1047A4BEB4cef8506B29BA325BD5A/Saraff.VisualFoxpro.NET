using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Saraff.VisualFoxpro.IoC {

    /// <summary>
    /// IoC-контейнер.
    /// </summary>
    /// <seealso cref="System.ComponentModel.Container" />
    /// <seealso cref="System.IServiceProvider" />
    public sealed class ServiceContainer:Container,IServiceProvider {
        private Dictionary<Type,Type> _binding=new Dictionary<Type,Type>();
        private Dictionary<Type,object> _instances=new Dictionary<Type,object>();
        private Stack<Type> _stack=new Stack<Type>();

        /// <summary>
        /// Выполняет загрузку привязок из указанной сборки.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Load(Assembly assembly) {
            foreach(BindServiceAttribute _attr in assembly.GetCustomAttributes(typeof(BindServiceAttribute),false)) {
                this.Bind(_attr.Service,_attr.ObjectType);
            }
        }

        /// <summary>
        /// Binds the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="obj">The object.</param>
        /// <exception cref="ArgumentNullException">Отсутствует информация о связываемых типах.</exception>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public void Bind(Type service,Type obj) {
            if(service==null||obj==null) {
                throw new ArgumentNullException("Отсутствует информация о связываемых типах.");
            }
            if(!service.IsInterface) {
                throw new InvalidOperationException(string.Format("Тип \"{0}\" не является интерфейсом.",service.FullName));
            }
            if(obj.GetInterface((typeof(IComponent).FullName))==null) {
                throw new InvalidOperationException(string.Format("Тип \"{0}\" не является производным от \"{1}\".",obj.FullName,typeof(IComponent).FullName));
            }
            this._binding.Add(service,obj);
        }

        /// <summary>
        /// Binds the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="obj">The object.</param>
        public void Bind(Type service,object obj) {
            this.Bind(service,obj.GetType());
            this._instances.Add(service,obj);
        }

        /// <summary>
        /// Создает экземпляр указанного типа и осуществляет внедрение зависимостей.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <returns>Экземпляр указанного типа.</returns>
        public T CreateInstance<T>() where T : class {
            return this.CreateInstance(typeof(T)) as T;
        }

        /// <summary>
        /// Создает экземпляр указанного типа и осуществляет внедрение зависимостей.
        /// </summary>
        /// <param name="type">Тип.</param>
        /// <returns>Экземпляр указанного типа.</returns>
        public object CreateInstance(Type type) {
            try {
                return this._CreateInstanceCore(type);
            } finally {
                this._stack.Clear();
            }
        }

        private object _CreateInstanceCore(Type type) {
            if(this._stack.Contains(type)) {
                throw new InvalidOperationException(string.Format("IoC. Обнаружена циклическая зависимость в \"{0}\".",type.FullName));
            }
            this._stack.Push(type);
            try {
                var _inst = new _Func(() => {
                    foreach(var _ctor in type.GetConstructors()) {
                        if(_ctor.GetCustomAttributes(typeof(ServiceRequiredAttribute),false).Length > 0) {
                            var _args = new List<object>();
                            foreach(var _param in _ctor.GetParameters()) {
                                _args.Add(_param.ParameterType.IsInterface ? this.GetService(_param.ParameterType) : this._CreateInstanceCore(_param.ParameterType));
                            }
                            return Activator.CreateInstance(type,_args.ToArray());
                        }
                    }
                    if(type.GetConstructor(Type.EmptyTypes) != null) {
                        return Activator.CreateInstance(type);
                    }
                    throw new InvalidOperationException(string.Format("IoC. Не удалось найти подходящий конструктор для создания экземпляра \"{0}\".",type.FullName));
                })();
                foreach(var _prop in type.GetProperties()) {
                    if(_prop.PropertyType.IsInterface) {
                        foreach(var _attr in _prop.GetCustomAttributes(typeof(ServiceRequiredAttribute),false)) {
                            _prop.SetValue(_inst,this.GetService(_prop.PropertyType),null);
                        }
                    }
                }
                var _component = _inst as IComponent;
                if(_component != null) {
                    this.Add(_component);
                }
                return _inst;
            } finally {
                this._stack.Pop();
            }
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
            if(this._binding.ContainsKey(service)) {
                if(!this._instances.ContainsKey(service)) {
                    this._instances.Add(service,this._CreateInstanceCore(this._binding[service]));
                }
                return this._instances[service];
            }
            return base.GetService(service);
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
        object IServiceProvider.GetService(Type serviceType) {
            try {
                return this.GetService(serviceType);
            } finally {
                this._stack.Clear();
            }
        }

        private delegate object _Func();
    }
}
