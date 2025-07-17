using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_FACTOR_VERSION_MMAYOR
    /// </summary>
    public partial class InFactorVersionMmayorDTO : EntityBase, ICloneable
    {
        public int Infmmcodi { get; set; }
        public int Infvercodi { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public string Infmmdescrip { get; set; }
        public DateTime Infmmfechaini { get; set; }
        public DateTime Infmmfechafin { get; set; }
        public string InfmmfechainiDesc { get; set; }
        public string InfmmfechafinDesc { get; set; }
        public decimal Infmmduracion { get; set; }
        public int Claprocodi { get; set; }
        public int Tipoevencodi { get; set; }
        public int Infmmhoja { get; set; }
        public string Infmmobspm { get; set; }
        public string Infmmorigen { get; set; }
        public string Infmmjustif { get; set; }
        public string Infmmobsps { get; set; }
        public string Infmmobspd { get; set; }
        public string Infmmobse { get; set; }

        public string Infmmusumodificacion { get; set; }
        public DateTime? Infmmfecmodificacion { get; set; }
        public string InfmmfecmodificacionDesc { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class InFactorVersionMmayorDTO 
    {
        public string Emprnomb { get; set; }
        public int Areacodi { get; set; }
        public string Areanomb { get; set; }
        public string Equiabrev { get; set; }
        public string Famabrev { get; set; }
        public string Tipoevendesc { get; set; }
        public string Clapronombre { get; set; }

        public string ObsIncumpl { get; set; }
        public string InfmmobseDesc { get; set; }
        public string InfmmobspmDesc { get; set; }
        public string InfmmobspsDesc { get; set; }
        public string InfmmobspdDesc { get; set; }
        public string InfmmorigenDesc { get; set; }
        public string InfmmobseDescMenor { get; set; }
        public string InfmmobseDescTr { get; set; }

        public List<int> Intercodis { get; set; }
        public List<InArchivoDTO> ListaArchivos{ get; set; }
        public int APE { get; set; }
        public int APNE { get; set; }
        public int AENP { get; set; }
        public int AEMAP { get; set; }

        public bool AccionEditar { get; set; }
        public int NroFila { get; set; }
        public int NroFilaSimilarCon { get; set; }
    }
}
