using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgoCoesDesktopReporteFrecuencia
{
    internal class DataGps
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }

        public string getInfo()
        {
            string cadena = String.Format("Codigo => {0}" + Environment.NewLine, Codigo);
            cadena += String.Format("Descripcion => {0}" + Environment.NewLine, Descripcion);
            cadena += String.Format("FechaInicial => {0}" + Environment.NewLine, FechaInicial);
            cadena += String.Format("FechaFinal => {0}" + Environment.NewLine, FechaFinal);
            return cadena;
        }
    }

    internal class DataPorSegundo
    {
        public string FechaHora { get; set; }
        public int IntervaloPorPosicion { get; set; }
        public string Periodo { get; set; }
        public string Dia { get; set; }
        public int IntervaloPorTiempo { get; set; }
        public string Hora { get; set; }
        public double Frecuencia { get; set; }

        public string getInfo()
        {
            string cadena = String.Format("FechaHora => {0}" + Environment.NewLine, FechaHora);
            cadena += String.Format("IntervaloPorPosicion => {0}" + Environment.NewLine, IntervaloPorPosicion);
            cadena += String.Format("Periodo => {0}" + Environment.NewLine, Periodo);
            cadena += String.Format("Dia => {0}" + Environment.NewLine, Dia);
            cadena += String.Format("IntervaloPorTiempo => {0}" + Environment.NewLine, IntervaloPorTiempo);
            cadena += String.Format("Hora => {0}" + Environment.NewLine, Hora);
            cadena += String.Format("Frecuencia => {0}" + Environment.NewLine, Frecuencia);
            return cadena;
        }
    }

    internal class DataPorMinuto
    {
        public string FechaHora { get; set; }
        public double Frecuencia { get; set; }
        public double Subita { get; set; }
        public double Desviacion { get; set; }
        public double Maximo { get; set; }
        public double Minimo { get; set; }
        public string Tension { get; set; }
        public string SegDisp { get; set; }

        public string getInfo()
        {
            string cadena = String.Format("FechaHora => {0}" + Environment.NewLine, FechaHora);
            cadena += String.Format("Frecuencia => {0}" + Environment.NewLine, Frecuencia);
            cadena += String.Format("Subita => {0}" + Environment.NewLine, Subita);
            cadena += String.Format("Desviacion => {0}" + Environment.NewLine, Desviacion);
            cadena += String.Format("Maximo => {0}" + Environment.NewLine, Maximo);
            cadena += String.Format("Minimo => {0}" + Environment.NewLine, Minimo);
            cadena += String.Format("Tension => {0}" + Environment.NewLine, Tension);
            cadena += String.Format("SegDisp => {0}" + Environment.NewLine, SegDisp);
            return cadena;
        }
    }

    internal class DataIndicadores
    {
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public double Valor { get; set; }
        public string Tipo { get; set; }

        public string getInfo()
        {
            string cadena = String.Format("Fecha => {0}" + Environment.NewLine, Fecha);
            cadena += String.Format("Hora => {0}" + Environment.NewLine, Hora);
            cadena += String.Format("Valor => {0}" + Environment.NewLine, Valor);
            cadena += String.Format("Tipo => {0}" + Environment.NewLine, Tipo);
            return cadena;
        }
    }
}
