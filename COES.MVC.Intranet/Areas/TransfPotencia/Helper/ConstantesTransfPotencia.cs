using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.TransfPotencia;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TransfPotencia.Helper
{
    public class ConstantesTransfPotencia
    {
        //RUTAS
        public const string ReporteDirectorio = "ReporteTransferencia";

        //MENSAJES
        public const string MensajeOkInsertarReistro = "La información ha sido correctamente registrada";
        public const string MensajeOkEditarReistro = "La información ha sido correctamente actualizada";
        public const string MensajeErrorGrabarReistro = "Se ha producido un error al grabar la información";
        public const string MensajeSoles = "Importes Expresados en Nuevos Soles (S/.)";

        public const int Origlectcodi = 27;
        public const int nColumnTabla1 = 9;
        public const int LectCodiPrie01 = 201;
    }
}
