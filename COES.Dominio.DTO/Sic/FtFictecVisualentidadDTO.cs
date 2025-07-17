using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_FICTEC_VISUALENTIDAD
    /// </summary>
    public class FtFictecVisualentidadDTO : EntityBase
    {
        public int Ftvercodi { get; set; }
        public string Ftverusucreacion { get; set; }
        public string Ftverocultoportal { get; set; }
        public DateTime? Ftverfecmodificacion { get; set; }
        public DateTime? Ftverfeccreacion { get; set; }
        public string Ftverusumodificacion { get; set; }
        public int Fteqcodi { get; set; }
        public int? Ftvercodisicoes { get; set; }
        public string Ftvertipoentidad { get; set; }
        public string Ftverocultoextranet { get; set; }
        public string Ftverocultointranet { get; set; }
    }

    public class NotificacionEqVisualizacion
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public string Empresa { get; set; }
        public string Ubicacion { get; set; }
        public string Oculto { get; set; }
        public string OcultoNew { get; set; }
        public string Usuario { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
