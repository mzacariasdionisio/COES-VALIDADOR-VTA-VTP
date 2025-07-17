using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAQUERYPLANTILLA
    /// </summary>
    public partial class SiMigraqueryplantillaDTO : EntityBase
    {
        public int Miqplacodi { get; set; }
        public string Miqplanomb { get; set; }
        public string Miqpladesc { get; set; }
        public string Miqplacomentario { get; set; }
        public string Miqplausucreacion { get; set; }
        public DateTime Miqplafeccreacion { get; set; }
    }

    public partial class SiMigraqueryplantillaDTO
    {
        public bool TieneTmopercodiDupl { get; set; }
        public bool TieneTmopercodiEqNoEmp { get; set; }
        public bool TieneTmopercodiRs { get; set; }
        public bool TieneTmopercodiFs { get; set; }
        public bool TieneTmopercodiEqAEmp { get; set; }

        public string MiqplafeccreacionDesc { get; set; }

        public List<SiMigraqueryplantparamDTO> ListaParametro { get; set; } = new List<SiMigraqueryplantparamDTO>();
        public List<SiMigraqueryplanttopDTO> ListaTipoOp { get; set; } = new List<SiMigraqueryplanttopDTO>();
    }
}
