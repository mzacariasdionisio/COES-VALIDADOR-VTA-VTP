using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_PROCESO_CONTACTO
    /// </summary>
    public class WbProcesoContactoDTO : EntityBase
    {
        public int Contaccodi { get; set; }
        public int Procesocodi { get; set; }
        public string Comicousucreacion { get; set; }
        public string Comicousumodificacion { get; set; }
        public DateTime? Comicofeccreacion { get; set; }
        public DateTime? Comicofecmodificacion { get; set; }
        //añadidos
        public string Descomite { get; set; }
        public int Indicador { get; set; }
    }
}
