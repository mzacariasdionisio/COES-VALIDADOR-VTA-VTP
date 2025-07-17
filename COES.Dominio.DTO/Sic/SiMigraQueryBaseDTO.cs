using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MIGRAQUERYBASE
    /// </summary>
    public partial class SiMigraquerybaseDTO : EntityBase
    {
        public int Miqubacodi { get; set; }
        public int? Miqplacodi { get; set; }
        public string Miqubaquery { get; set; }
        public string Miqubamensaje { get; set; }
        public int Miqubaflag { get; set; }
        public string Miqubanomtabla { get; set; }
        public string Miqubastr { get; set; }
        public int Miqubaactivo { get; set; }
        public string Miqubaflagtbladicional { get; set; }
        public string Miqubausucreacion { get; set; }
        public DateTime Miqubafeccreacion { get; set; }
    }

    public partial class SiMigraquerybaseDTO
    {
        public string Miqplanomb { get; set; }
        public int Moxtopcodi { get; set; }

        public bool TieneTmopercodiDupl { get; set; }
        public bool TieneTmopercodiEqNoEmp { get; set; }
        public bool TieneTmopercodiRs { get; set; }
        public bool TieneTmopercodiFs { get; set; }
        public bool TieneTmopercodiEqAEmp { get; set; }
        public bool TieneFiltroTipoOp { get; set; }

        public string MiqubaflagDesc { get; set; }
        public string MiqubafeccreacionDesc { get; set; }

        public SiMigraqueryplantillaDTO Plantilla { get; set; }
        public List<SiMigraqueryparametroDTO> ListaParametroValor { get; set; } = new List<SiMigraqueryparametroDTO>();
        public List<SiMigraqueryxtipooperacionDTO> ListaTipoOpValor { get; set; } = new List<SiMigraqueryxtipooperacionDTO>();
    }
}
