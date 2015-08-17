﻿/* Этот файл является частью библиотеки Saraff.VisualFoxpro.NET
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
using System.IO;
using Saraff.AxHost;
using Saraff.VisualFoxpro.Core;

namespace Saraff.VisualFoxpro {

    public class VfpApplicationComponent:ApplicationComponent {
        private VfpContainer _container;

        public VfpApplicationComponent() {
            this._container=new VfpContainer(this._ExternalInvokeHandler);
            this.Externals=new Dictionary<Type, _VfpExternalComponent>();

            #region Добавляем требуемые внешние компоненты

            foreach(VfpExternalRequiredAttribute _attr in this.GetType().GetCustomAttributes(typeof(VfpExternalRequiredAttribute), false)) {
                var _ext=Activator.CreateInstance(_attr.Type, true) as _VfpExternalComponent;
                this._container.Add(_ext);
                this.Externals.Add(_attr.Type, _ext);
            }

            #endregion
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
            if(disposing&&this._container!=null) {
                this._container.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void ErrorMessageBox(Exception ex) {
            this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(ex));
        }

        protected void ErrorMessageBox(string message) {
            this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(message, "Error"));
        }

        protected void WarningMessageBox(string message) {
            this.OnErrorHandlerRequired(new ErrorHandlerRequiredEventArgs(message, "Warning"));
        }

        private void OnErrorHandlerRequired(ErrorHandlerRequiredEventArgs e) {
            if(this.VfpErrorHandlerRequired_14173D436F344779B521DC61F955F7BD!=null) {
                this.VfpErrorHandlerRequired_14173D436F344779B521DC61F955F7BD(this, e);
            }
        }

        /// <summary>
        /// Обработчик вызовов от компонентов, управляющих внешними элементами.
        /// </summary>
        /// <param name="sender">Источник.</param>
        /// <param name="args">Аргументы.</param>
        private void _ExternalInvokeHandler(object sender, EventArgs args) {
            if(this.VfpExternalRequired_6BD50AADD7FD4919A200FBC48E9CC28A!=null) {
                this.VfpExternalRequired_6BD50AADD7FD4919A200FBC48E9CC28A(this, args);
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

        protected ReadOnlyCollection<object> ComponentParameters {
            get;
            private set;
        }

        protected Dictionary<Type, _VfpExternalComponent> Externals {
            get;
            private set;
        }

        #endregion

        #region События

        [ApplicationProcessed]
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public event EventHandler VfpExternalRequired_6BD50AADD7FD4919A200FBC48E9CC28A;

        [ApplicationProcessed]
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public event EventHandler VfpErrorHandlerRequired_14173D436F344779B521DC61F955F7BD;

        #endregion
    }
}
