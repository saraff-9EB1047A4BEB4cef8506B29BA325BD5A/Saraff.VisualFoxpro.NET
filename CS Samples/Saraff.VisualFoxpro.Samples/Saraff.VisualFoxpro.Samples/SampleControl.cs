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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Saraff.AxHost;
using Saraff.VisualFoxpro.Odbc;
using Forms=Saraff.VisualFoxpro.Externals.Forms;
using Classes=Saraff.VisualFoxpro.Externals.Classes;
using Objects=Saraff.VisualFoxpro.Externals.Objects;
using Code=Saraff.VisualFoxpro.Externals.Code;
using CustomCode=Saraff.VisualFoxpro.Externals.CustomCode;

namespace Saraff.VisualFoxpro.Samples {

    [ApplicationControl(Title="Sample Control",Width=300,Height=500)]
    [VfpExternalRequired(typeof(Forms.str))]
    [VfpExternalRequired(typeof(Classes.bcFrmSpisok))]
    [VfpExternalRequired(typeof(Code.GetInterval))]
    [VfpExternalRequired(typeof(CustomCode.Script1))]
    [VfpExternalRequired(typeof(Objects.VfpApplicationParams))]
    [VfpExternalRequired(typeof(Objects.VfpHostForm))]
    [VfpExternalRequired(typeof(SampleControlExternalForm))]
    public partial class SampleControl:OdbcApplicationControl {

        public SampleControl() {
            InitializeComponent();
        }

        protected override void OnDataLoad(EventArgs e) {
            base.OnDataLoad(e);
            try {
                this.SetToolBarButtonsState(VfpToolButons.AddRecordButton|VfpToolButons.SaveButton, true);
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        protected override void OnToolButtonClick(ApplicationToolButtonEventArgs e) {
            base.OnToolButtonClick(e);
            try {
                switch(e.ApplicationToolButton) {
                    case VfpToolButons.AddRecordButton:
                        this.SetToolBarButtonsState(VfpToolButons.RemoveRecordButton|VfpToolButons.PrintButton, true);
                        this.SetToolBarButtonsState(VfpToolButons.SaveButton, false);
                        break;
                    case VfpToolButons.PrintButton:
                        int _res;
                        ((SampleControlExternalForm)this.Externals[typeof(SampleControlExternalForm)]).Invoke(out _res, 1, 2, 3, "A");
                        break;
                }
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        private void OnTestEvent(EventArgs e) {
            if(this.TestEvent!=null) {
                this.TestEvent(this, e);
            }
        }

        [ApplicationProcessed]
        public event EventHandler TestEvent;

        private void button1_Click(object sender, EventArgs e) {
            try {
                //int _res;
                //if(((Forms.str)this.Externals[typeof(Forms.str)]).Invoke(out _res)) {
                //}

                var _sp=(Classes.bcFrmSpisok)this.Externals[typeof(Classes.bcFrmSpisok)];
                _sp.Create();
                _sp.Show();

                //var _toolbar=(Objects.ToolBar)this.Externals[typeof(Objects.ToolBar)];
                //_toolbar.CopyButton=true;
                //_toolbar.Refresh();

                //((Code.GetInterval)this.Externals[typeof(Code.GetInterval)]).Invoke(new DateTime(2010, 1, 1), DateTime.Today, 3, false, true, true, false);

                //((CustomCode.Script1)this.Externals[typeof(CustomCode.Script1)]).Invoke(new object());

                //var _val=((Objects.VfpApplicationParams)this.Externals[typeof(Objects.VfpApplicationParams)])["P_WDVUZ"];

                //((Objects.VfpHostForm)this.Externals[typeof(Objects.VfpHostForm)]).Width+=100;

                //((Objects.VfpHostForm)this.Externals[typeof(Objects.VfpHostForm)]).Close();

                //((Objects.VfpHostForm)this.Externals[typeof(Objects.VfpHostForm)]).SetStatusMessage("OK!");
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }
    }
}
