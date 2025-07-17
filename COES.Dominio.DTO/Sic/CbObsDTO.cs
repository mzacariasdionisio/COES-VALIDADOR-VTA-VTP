using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_OBS
    /// </summary>
    public partial class CbObsDTO : EntityBase
    {
        public int Cbobscodi { get; set; }
        public int Cbevdacodi { get; set; }
        public string Cbobshtml { get; set; }
    }

    public partial class CbObsDTO
    {
        public int Equicodi { get; set; }
        public List<CbObsxarchivoDTO> ListaArchivoXObs { get; set; } = new List<CbObsxarchivoDTO>();

        public int Ccombcodi { get; set; }
        public string Ccombnombre { get; set; }
        public string Cbevdavalor { get; set; }
    }
}
