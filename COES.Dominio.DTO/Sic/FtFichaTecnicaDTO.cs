using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_FICHATECNICA
    /// </summary>
    public class FtFichaTecnicaDTO : EntityBase
    {
        public int Fteccodi { get; set; }
        public string Ftecnombre { get; set; }
        public string Ftecestado { get; set; }
        public string Ftecusucreacion { get; set; }
        public int Ftecprincipal { get; set; }
        public string Ftecusumodificacion { get; set; }
        public DateTime? Ftecfecmodificacion { get; set; }
        public DateTime? Ftecfeccreacion { get; set; }
        public int? Ftecambiente { get; set; }

        public string FtecprincipalDesc { get; set; }
        public string FtecfecmodificacionDesc { get; set; }
        public string FtecfeccreacionDesc { get; set; }
        public string FtecestadoDesc { get; set; }
        public string FtecambienteDesc { get; set; }
    }

    public class NotificacionFM
    {
        public int Fteccodi { get; set; }
        public string Ftecnombre { get; set; }
        public string FtecnombreNew { get; set; }
        public string Ftecprincipal { get; set; }
        public string FtecprincipalNew { get; set; }
        public string Ftequsumodificacion { get; set; }
        public DateTime? Fteqfecmodificacion { get; set; }
        public int Ambiente { get; set; }
    }
}
