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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Saraff.AxHost;
using Saraff.VisualFoxpro.Odbc;

namespace Transistor.Personnel.Common {

    /// <summary>
    /// Отображает фотографию работника.
    /// </summary>
    [ApplicationControl]
    public partial class EmployeePhoto:OdbcApplicationControl {

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeePhoto"/> class.
        /// </summary>
        public EmployeePhoto() {
            InitializeComponent();
        }

        /// <summary>
        /// Отображает фотографию для указанного табельного номера.
        /// </summary>
        /// <param name="tabn">Табельный номер.</param>
        [ApplicationProcessed]
        public void ShowPhoto(int tabn) {
            try {
                using(var _command=this.Connection.CreateCommand()) {
                    _command.CommandText=@"SELECT ... FROM ... WHERE TABN=?";
                    _command.Parameters.AddWithValue("?",tabn);
                    var _data=_command.ExecuteScalar() as byte[];
                    if(_data!=null&&_data.Length>0) {
                        using(MemoryStream _stream=new MemoryStream(_data)) {
                            this._SetImage(Image.FromStream(_stream));
                        }
                    }
                }
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        /// <summary>
        /// Сбрасывает изображение.
        /// </summary>
        [ApplicationProcessed]
        public void ResetPhoto() {
            try {
                this._SetImage(null);
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        /// <summary>
        /// Устанавливает зображение.
        /// </summary>
        /// <param name="image">Изображение.</param>
        private void _SetImage(Image image) {
            if(this.pictureBox1.Image!=null) {
                this.pictureBox1.Image.Dispose();
            }
            this.pictureBox1.Image=image;
        }

        /// <summary>
        /// Handles the Click event of the copyToClipboardToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void copyToClipboardToolStripMenuItem_Click(object sender,EventArgs e) {
            try {
                if(this.pictureBox1.Image!=null) {
                    Clipboard.SetImage(this.pictureBox1.Image);
                }
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }
    }
}
