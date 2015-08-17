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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Saraff.AxHost;
using Saraff.VisualFoxpro;
using Saraff.VisualFoxpro.Odbc;
using Obj=Saraff.VisualFoxpro.Externals.Objects;

namespace Transistor.Personnel.Common {

    /// <summary>
    /// Компонент VersionControl.
    /// </summary>
    [ApplicationControl(Title="Информация о системе")]
    [VfpExternalRequired(typeof(Obj.VfpHostForm))]
    public partial class VersionControl:OdbcApplicationControl {

        /// <summary>
        /// Конструктор контрола.
        /// </summary>
        public VersionControl() {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the <see cref="E:DataLoad"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnDataLoad(EventArgs e) {
            base.OnDataLoad(e);
            try {
                this._SetVersionInfo();
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        /// <summary>
        /// Присваивает версии CLR и СУБД соответствующим textBox.
        /// </summary>
        private void _SetVersionInfo() {
            using(var _command=this.Connection.CreateCommand()) {
                _command.CommandText="...";
                this.dbVersionTextBox.Text=_command.ExecuteScalar().ToString();
                this.CLRVersionTextBox.Text=string.Format(
                    "Версия CLR: {1}{0}"+
                    "Версия ОС: {2}{0}"+
                    "Текущий каталог: {3}{0}"+
                    "Системный каталог: {6}{0}"+
                    "Имя машины: {4}{0}"+
                    "Количество процессоров: {5}{0}"+
                    "Имя пользователя: {7}{0}"+
                    "Рабочий набор памяти: {8:N0} байт",
                    Environment.NewLine,
                    Environment.Version,
                    Environment.OSVersion,
                    Environment.CurrentDirectory,
                    Environment.MachineName,
                    Environment.ProcessorCount,
                    Environment.SystemDirectory,
                    Environment.UserName,
                    Environment.WorkingSet);
            }
        }

        private Obj.VfpHostForm Host {
            get {
                return this.Externals[typeof(Obj.VfpHostForm)] as Obj.VfpHostForm;
            }
        }
    }
}
