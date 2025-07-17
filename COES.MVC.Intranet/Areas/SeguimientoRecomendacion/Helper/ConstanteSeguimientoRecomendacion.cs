using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Helper
{
    public class ConstanteSeguimientoRecomendacion
    {
        

        public const string SesionFechaInicioAseg = "SesionFechaInicioAseg";
        public const string SesionFechaFinAseg = "SesionFechaFinAseg";

        public const string SesionFamiliaAseg = "SesionFamiliaAseg";
        public const string SesionTipoEmpresaAseg = "SesionTipoEmpresaAseg";
        public const string SesionEmpresaAseg = "SesionEmpresaAseg";

        public const string SesionConRecomendacionAseg = "SesionConRecomendacionAseg";
        public const string SesionDetRecomendacionAseg = "SesionDetRecomendacionAseg";

        //pageSize listas
        public const int PageSizeRecomendacion = 50;
        public const int PageSizeComentario = 20;


        //reporte
        public const string SesionFechaInicioRepAseg = "SesionFechaInicioRepAseg";
        public const string SesionFechaFinRepAseg = "SesionFechaFinRepAseg";
        public const string SesionFamiliaRepAseg = "SesionFamiliaAseg";
        public const string SesionTipoEmpresaRepAseg = "SesionTipoEmpresaAseg";
        public const string SesionEmpresaRepAseg = "SesionEmpresaAseg";

        public const string SesionIdRecom = "SesionIdRecom";
        public const string SesionIdComent = "SesionIdComent";


        public const string SesionIdEstado = "SesionIdEstado";
        public const string SesionIdCriticidad = "SesionIdCriticidad";

        //fecha inicio. días de diferencia
        public const int DesfaseFecha = -180;

        //estado atendido
        public const int EstadoAtendido = 3;
        
        //ruta base de seguimiento de recomendaciones
        public const string PathSeguimientoRecomendaciones="SeguimRecom\\";

        //código de rol de aseguramiento
        public const int RolAseguramiento = 217;


        public const int DiaNotificacion = 7;
        

    
    }
}