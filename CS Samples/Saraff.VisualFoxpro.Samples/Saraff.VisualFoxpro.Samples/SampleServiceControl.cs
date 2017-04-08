using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Saraff.AxHost;
using IoC=Saraff.VisualFoxpro.IoC;
using Saraff.VisualFoxpro.Odbc;
using Objects=Saraff.VisualFoxpro.Externals.Objects;

namespace Saraff.VisualFoxpro.Samples {

    [ApplicationControl(Title="Sample Control",Width=300,Height=500)]
    [VfpExternalRequired(typeof(Objects.VfpHostForm))]
    [IoC.ServiceRequired(Service=typeof(ComponentModel.ISampleService))]
    public sealed partial class SampleServiceControl:OdbcApplicationControl {

        public SampleServiceControl() {
            this.InitializeComponent();
        }

        protected override void OnDataLoad(EventArgs e) {
            base.OnDataLoad(e);
            try {
                this.textBox1.Text=this.SampleService.GetCurrentDbDate().ToString();
            } catch(Exception ex) {
                this.ErrorMessageBox(ex);
            }
        }

        private ComponentModel.ISampleService SampleService {
            get {
                return this.GetService(typeof(ComponentModel.ISampleService)) as ComponentModel.ISampleService;
            }
        }
    }
}
