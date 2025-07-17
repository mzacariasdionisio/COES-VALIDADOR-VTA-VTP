using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public partial class GmmDatInsumoDTO : EntityBase
    {
        /// <summary>
        /// Clase que mapea la tabla GMM_DATINSUMO
        /// </summary>
        ///         public int DATINSCODI { get; set; }
        public int DCALCODI { get; set; }
        public decimal DINSVALOR { get; set; }
        public decimal DCALVALOR { get; set; }
        public int EMPGCODI { get; set; }
        public int EMPRCODI { get; set; }
        public int PERICODI { get; set; }
        public string TINSCODI { get; set; }
        public int DINSANIO { get; set; }
        public string DINSMES { get; set; }
        public string DINSUSUCREACION { get; set; }
        public DateTime? DINSFECCREACION { get; set; }
        public string DINSUSUMODIFICACION { get; set; }
        public DateTime? DINSFECMODIFICACION { get; set; }

    }
    public partial class GmmDatInsumoDTO : EntityBase
    {
        // Tabla de resultados para el listado de valores insumos de tipo 
        // SC = Servicios complementarios
        public string LISTEMPRNOMB { get; set; }
        public int LISTEMPGCODI { get; set; }
        public decimal LISTDINSVALOR { get; set; }

        public decimal ENTREGAS { get; set; }
        public decimal SC { get; set; }
        public decimal INFLEX { get; set; }
        public decimal RECAU { get; set; }


    }
    public partial class GmmDatInsumoDTO : EntityBase
    {
        // Campo genérico para grabar el usuario de creación o modificación
        public string DINSUSUARIO { get; set; }
    }
}
