using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla tr_cargaarchxml_sp7
    /// </summary>
    public class TrCargaarchxmlSp7DTO : EntityBase
    {
        public int CarCodi { get; set; }
        public DateTime CarFecha { get; set; }
        public int CarCantidad { get; set; }
        public string CarUsuario { get; set; }
        public string CarNombreXML { get; set; }
        public int? CarTipo { get; set; }
        public string CarTipoNombre { get; set; }
    }
}
