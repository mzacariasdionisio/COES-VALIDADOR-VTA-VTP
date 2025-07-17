using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ReportesMedicion.Helper
{
    public class ConstantesReportesMedicion
    {
        public const string PtoCalculadoSiCodigo = "S";
        public const string PtoCalculadoSiDescrip = "Sí";
        public const string PtoCalculadoNoDescrip = "No";

        public const int EstadoReporptomedActivo = 1;
        public const int EstadoReporptomedInactivo = 0;
        public const string EstadoReporptomedActivoDescrip = "Activo";
        public const string EstadoReporptomedInactivoDescrip = "Inactivo";

        public const string EstadoActivoLetra = "A";
        public const string EstadoInactivoLetra = "B";

        public const int LectcodiEjeHis = 6;

        //Tipo Dato (H)
        public const int DatoPromedio = 1; //suma entre 2,4
        public const int DatoHorario = 2; //valor que cae en la hora

        //Validacion
        public const int ValidarCeroSi = 1;
        public const int ValidarCeroNo = 2;

        //Tipo Dato
        public const int TotalSumatoria = 1;
        public const int TotalPromedio = 2;

        //Recursivo
        public const int MaximoNivelRecursivo = 5;

        //Fuente de Datos
        public const int FuenteMediciones = 1;
        public const int FuenteScada = 2;
        public const string FormatosHidrologia = "129,133,100,101,102,103,104,141,131,144,145,105,142,146,135,136,137,138,132,139,140,130,131,133,134,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122,123,124,125,126,127,128,143,131,139,131,132,133,134,135,136,137,138,139,130,140,150,148,149,151";

        //Manual de usuario SGI
        public const string ArchivoManualUsuarioIntranetSGI = "Manual_Usuario_SGI_v1.2.pdf";
        public const string ModuloManualUsuarioSGI = "Manuales de Usuario\\";

        //constantes para fileServer SGI
        public const string FolderRaizSGIModuloManual = "Migración SGOCOES Desktop a Intranet\\";
    }
}
