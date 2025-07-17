using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General.Helper
{
    /// <summary>
    /// Constantes utilizadas en el Portal Web
    /// </summary>
    public class ConstantesPortal
    {           

        public const string PrefijoFileNotaTecnica = "NTDE_{0}.{1}";
        public const string PrefijoFileNotaTecnicaDet = "NTDEDET_{0}_{1}.{2}";
        public const int ArchivoPrincipal = 1;
        public const int ArchivoCarta = 2;
        public const int ArchivoAntecendente = 3;
        public const string TipoEventoPublicacion = "1";
        public const string TipoEventoReunion = "3";
        public const string TipoEventoVencimiento = "2";
        public const string IconoEventoPublicacion = "icon-report.png?v=1";
        public const string ColorEventoPublicacion = "#2ECC71";
        public const string TextoEventoPublicacion = "Publicación del COES";
        public const string IconoEventoReunion = "icon-meeting.png?v=1";
        public const string ColorEventoReunion = "#F1C40F";
        public const string TextoEventoReunion = "Reunión";
        public const string IconoEventoVencimiento = "icon-clock.png?v=1";
        public const string ColorEventoVencimiento = "#E74C3C";
        public const string TextoEventoVencimiento = "Vence el plazo para entrega de información al COES";
    }
}
