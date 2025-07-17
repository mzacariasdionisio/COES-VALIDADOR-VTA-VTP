using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ETEMPDETPRY
    /// </summary>
    public partial class FtExtEtempdetpryDTO : EntityBase
    {
        public int Fetempcodi { get; set; } 
        public int Feeprycodi { get; set; } //Id
        public int Ftprycodi { get; set; } 
        public string Feepryestado { get; set; } 
    }

    public partial class FtExtEtempdetpryDTO
    {
        public string Emprnomb { get; set; }
        public string Ftetnombre { get; set; }
        public int Emprcodi { get; set; }
        public int Ftetcodi { get; set; }
        public string Ftprynombre { get; set; }

        public string NombreEmpresa { get; set; }
        public string CodigoProy { get; set; }
        public string NombProyExt { get; set; }

        public List<FtExtEtempdetpryeqDTO> ListaCambiosGuardar { get; set; }
        public List<FtExtEtempdetpryeqDTO> ListaCambiosActualizar { get; set; }
    }
}
