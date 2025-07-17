using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_RELEMPETAPA
    /// </summary>
    public partial class FtExtRelempetapaDTO : EntityBase
    {
        public int Emprcodi { get; set; }
        public int Ftetcodi { get; set; }
        public int Fetempcodi { get; set; } //Id
        public string Fetempusucreacion { get; set; }
        public DateTime? Fetempfeccreacion { get; set; }
        public string Fetempusumodificacion { get; set; }
        public DateTime? Fetempfecmodificacion { get; set; }
        public string Fetempestado { get; set; }
    }

    public partial class FtExtRelempetapaDTO
    {
        public string StrFetempfecmodificacion { get; set; }
        public string StrFetempfeccreacion { get; set; }
        
        public string Emprnomb { get; set; }
        public string Ftetnombre { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Ftprycodi { get; set; }
        public List<FtExtEtempdetpryDTO> ListaProyectos { get; set; } //usado para mostrar lista de proyectos al Editar o Ver
        public List<FtExtEtempdeteqDTO> ListaElementos { get; set; } //usado para mostrar lista de elementos al Editar o Ver

        public List<FtExtEtempdetpryDTO> ListaProyectosGuardar { get; set; } //usado para enviar info a BD
        public List<FtExtEtempdetpryDTO> ListaProyectosActualizar { get; set; } //usado para enviar info a BD

        public List<FtExtEtempdeteqDTO> ListaElementosGuardar { get; set; } //usado para enviar info a BD
        public List<FtExtEtempdeteqDTO> ListaElementosActualizar { get; set; } //usado para enviar info a BD
    }
}

