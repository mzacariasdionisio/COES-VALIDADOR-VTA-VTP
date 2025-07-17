using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_HISEMPENTIDAD_DET
    /// </summary>
    public partial class SiHisempentidadDetDTO : EntityBase
    {
        public int Hempedcodi { get; set; }
        public int Hempencodi { get; set; }
        public int Emprcodi { get; set; }
        public DateTime Hempedfecha { get; set; }
        public int Hempedvalorid { get; set; }
        public string Hempedvalorestado { get; set; }
    }

    public partial class SiHisempentidadDetDTO
    {
        public string EmprnombOrigen { get; set; }
        public string Nombre { get; set; }
        public string Nombre2 { get; set; }
        public string EstadoActual { get; set; }
    }
}
