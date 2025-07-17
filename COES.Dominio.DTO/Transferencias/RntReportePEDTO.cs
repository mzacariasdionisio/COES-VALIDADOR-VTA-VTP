using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RNT_REG_PUNTO_ENTREGA
    /// </summary>
    [XmlRoot("RNT_REPORTEPE")]
    public class RntReportePEDTO : EntityBase
    {

        // Datos Punto Entrega
        public int RepID { get; set; }
        public string RepCliente { get; set; }
        public string RepBarra { get; set; }
        public string RepNivelTension { get; set; }
        public decimal RepEnergiaSemestral { get; set; }
        public string RepTipo { get; set; }
        public int RepExonerado { get; set; }
        public int RepNi { get; set; }
        public int RepKi { get; set; }
        public string RepTEFecInicio { get; set; }
        public string RepTEFecFin { get; set; }
        public string RepTPFecInicio { get; set; }
        public string RepTPFecFin { get; set; }
        public string RepRIEmpresa { get; set; }
        public string RepRIPorcentaje { get; set; }
        public string RepRIIEmpresa { get; set; }
        public string RepRIIPorcentaje { get; set; }
        public string RepRIIIEmpresa { get; set; }
        public string RepRIIIPorcentaje { get; set; }
        public string RepCausaInterrupcion { get; set; }
        public decimal RepEi { get; set; }
        public decimal RepResarcimiento { get; set; }
        public bool[] arrEsValido { get; set; } 


    }


}
