using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_MENSAJE
    /// </summary>
    public class MeMensajeDTO : EntityBase
    {
        public int Msjcodi { get; set; }
        public string Msjdescripcion { get; set; }
        public int Emprcodi { get; set; }
        public int Formatcodi { get; set; }
        public string Msjusucreacion { get; set; }
        public DateTime? Msjfeccreacion { get; set; }
        public DateTime? Msjfecperiodo { get; set; }
        public string Msjestado { get; set; }
        public string Msjusumodificacion { get; set; }
        public DateTime? Msjfecmodificacion { get; set; }
        

        public string Emprnomb { get; set; }
        public string Formatnombre { get; set; }
        public string MsjestadoDesc { get; set; }
        public string Remitente { get; set; }
        public string MsjfeccreacionDesc { get; set; }
        public string EmpresaRemitente { get; set; }
        public string MsjfecmodificacionDesc { get; set; }
        public bool EsRemitenteAgente { get; set; }
        public bool EsLeido { get; set; }

        public List<string> ListaArchivo { get; set; } = new List<string>();
    }
}
