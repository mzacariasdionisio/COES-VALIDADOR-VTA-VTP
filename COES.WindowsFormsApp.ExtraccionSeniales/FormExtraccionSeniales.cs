using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COES.WindowsFormsApp.ExtraccionSeniales
{
    public partial class FormExtraccionSeniales : Form
    {
        private int strPadRightLength = 50;
        private string programaUbicacion;
        private string programaComandoEjecucion;
        private string bat;
        private string logFileName;

        public FormExtraccionSeniales()
        {
            InitializeComponent();
        }

        private void buttonIniciarExtraccionSenales_Click(object sender, EventArgs e)
        {
            buttonIniciarExtraccionSenales.Enabled = false;
            textBoxEstadoInicial();
            logFileName = String.Format(ConfigurationManager.AppSettings["LogFileName"], DateTime.Now.ToString("_MM_dd_yyyy_HH_mm_ss"));
            DateTime FechaInicio = dateTimePickerDesde.Value;
            DateTime FechaFin = dateTimePickerHasta.Value;
            DateTime fechaOperar = FechaInicio;
            textBoxExtraccionSenalDetalle.AppendText(Environment.NewLine);

            while ((FechaFin - fechaOperar).TotalDays >= 0)
            {
                DateTime operacionInicia = DateTime.Now;
                DateTime operacionFinaliza = DateTime.Now;
                textBoxExtraccionSenalDetalle.AppendText("Extracción para la fecha => " + fechaOperar.ToString("dd/MM/yyyy") + "\t" + Environment.NewLine);
                textBoxExtraccionSenalDetalle.AppendText("Iniciando extracción (" + operacionInicia.ToString("dd/MM/yyyy HH:mm:ss") + ")..." + Environment.NewLine);
                log(string.Format("Extracción para la fecha {0} (Inicio => {1}) ", fechaOperar.ToString("dd/MM/yyyy"), operacionInicia.ToString("dd/MM/yyyy HH:mm:ss")));
                procesar(fechaOperar);
                operacionFinaliza = DateTime.Now;
                textBoxExtraccionSenalDetalle.AppendText("Culminando extracción (" + operacionFinaliza.ToString("dd/MM/yyyy HH:mm:ss") + ")..." + Environment.NewLine);
                log(string.Format("Extracción para la fecha {0} (fin => {1}) ", fechaOperar.ToString("dd/MM/yyyy"), operacionFinaliza.ToString("dd/MM/yyyy HH:mm:ss")));
                textBoxExtraccionSenalDetalle.AppendText(Environment.NewLine);
                log(Environment.NewLine);
                fechaOperar = fechaOperar.AddDays(1);
            }

            buttonIniciarExtraccionSenales.Enabled = true;
        }

        private void FormExtraccionSenales_Load(object sender, EventArgs e)
        {
            programaUbicacion = ConfigurationManager.AppSettings["ProgramaUbicacion"];
            programaComandoEjecucion = ConfigurationManager.AppSettings["ProgramaComandoEjecucion"];
            bat = ConfigurationManager.AppSettings["Bat"];
            textBoxEstadoInicial();
        }

        private void textBoxEstadoInicial()
        {
            textBoxExtraccionSenalDetalle.Text = "Proceso de extracción de señales" + Environment.NewLine;
            textBoxExtraccionSenalDetalle.AppendText("------------------------------------------------------" + Environment.NewLine);
        }

        private void log(string texto)
        {
            File.AppendAllText(logFileName, texto + Environment.NewLine);
        }

        private void procesar(DateTime fecha)
        {
            string comando = string.Format(programaComandoEjecucion, programaUbicacion, fecha.ToString("yyyy-MM-dd"));

            if (File.Exists(bat))
                File.Delete(bat);

            File.AppendAllText(bat, "cd " + programaUbicacion + Environment.NewLine);
            File.AppendAllText(bat, comando);
            log(comando);
            textBoxExtraccionSenalDetalle.AppendText(comando + Environment.NewLine);
            try
            {
                Process.Start(new ProcessStartInfo(bat, "")).WaitForExit();
            }
            catch(Exception ex)
            {
                textBoxExtraccionSenalDetalle.AppendText(ex.ToString() + Environment.NewLine);
                log(ex.ToString());
            }
        }
    }
}
