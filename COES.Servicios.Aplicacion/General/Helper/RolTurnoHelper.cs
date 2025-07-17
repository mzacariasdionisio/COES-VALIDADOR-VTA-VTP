using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General.Helper
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class RolTurnoHelper
    {
        public const string RutaReporte = "Areas/RolTurnos/Reporte/";
        public const string FormatoCargaRolTurno = "FormatoCargaRolTurno.xlsx";
        public const string ArchivoImportacionRolTurno = "ImportacionRolTurno.xlsx";
        public const string ExportacionArchivo = "RolTurnoSPR_{0}.xlsx";
        public const string ModalidadRemota = "R";
        public const string ModalidadPresencial = "P";
        public const string MotivoErrorActividad = "Actividad no válida";
        public const string ColorPresencial = "FF000000";
        public const string ReportePDOPDI = "P";
        public const string ReporteDescanso = "D";

        /// <summary>
        /// Permite obtener el nombre del tipo de grupo
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static string ObtenerTipoGrupo(string tipo)
        {
            string tipoGrupo;
            switch (tipo)
            {
                case "P":
                    {
                        tipoGrupo = "Programadores";
                        break;
                    }
                case "E":
                    {
                        tipoGrupo = "Especialistas";
                        break;
                    }
                case "S":
                    {
                        tipoGrupo = "Subdirector";
                        break;
                    }
                case "O":
                    {
                        tipoGrupo = "Otros";
                        break;
                    }
                default:
                    {
                        tipoGrupo = "";
                        break;
                    }
            }

            return tipoGrupo;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
