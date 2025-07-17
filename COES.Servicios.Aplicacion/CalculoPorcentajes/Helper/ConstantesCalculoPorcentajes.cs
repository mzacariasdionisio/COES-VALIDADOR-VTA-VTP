using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CalculoPorcentajes.Helper
{
    public class ConstantesCalculoPorcentajes
    {
        //RUTAS
        public const string ReporteDirectorio = "ReporteTransferencia";
        public const string RepositorioDirectorio = "RepositorioAporteIntegrantes";
        //NOMBRE DE ARCHIVOS
        public const string SDDP_DURACION = "Duracion.xlsx";
        public const string SDDP_RENOVABLES = "Renovables.xlsx";
        public const string SDDP_TERMICAS = "Termicas.xlsx";
        public const string SDDP_HIDRAULICAS = "Hidraulicas.xlsx";
        public const string SDDP_COSTOMARGINAL = "CostoMarginal.xlsx";
        public const string SDDP_PARAMETROS = "Parametros.xlsx";
        public const string SDDP_RESULTADOSCMG = "ResultadosCMG.xlsx";
        public const string SDDP_RESULTADOSENERGIA = "ResultadosEnergia.xlsx";
        //MENSAJES
        public const string MensajeOkInsertarReistro = "La información ha sido correctamente registrada";
        public const string MensajeOkEditarReistro = "La información ha sido correctamente actualizada";
        public const string MensajeErrorGrabarReistro = "Se ha producido un error al grabar la información";
        public const string MensajeSoles = "Importes Expresados en Soles (S/)";
        //TIPOSDEEMPRESA
        public const int TipoEmprCodiTransmisor = 1;
        public const int TipoEmprCodiDistribuidor = 2;
        public const int TipoEmprCodiGenerador = 3;
        public const int TipoEmprCodiUsuarioLibre = 4;
        //MODULOS
        public const int IdModuloCalculoPorcentajes = 11;
        //FORMATOS
        public const int IdFormatoEjecutadaDistribuidores = 87;
        public const int IdFormatoProyectadaDistribuidores = 88;
        public const int IdFormatoProyectadaUsuariosLibres = 89;
        public const int IdFormatoEjecutadaTransmisores = 90;
        public const int IdFormatoProyectadaTransmisores = 91;
        //LECTURAS
        public const int IdLecturaEjecutadaDistribuidores = 216;
        public const int IdLecturaProyectadaDistribuidores = 217;
        public const int IdLecturaProyectadaUsuariosLibres = 218;
        public const int IdLecturaEjecutadaTransmisores = 219;
        public const int IdLecturaProyectadaTransmisores = 220;
        //ORIGENLECTURA
        public const int IdOrigenLectura = 30;
    }

}
