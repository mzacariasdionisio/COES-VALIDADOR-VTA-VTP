using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    public partial class PmoConfIndispEquipoDTO : EntityBase, ICloneable
    {
        public int PmCindCodi { get; set; }
        public int Sddpcodi { get; set; }
        public int EquiCodi { get; set; }
        public int Grupocodimodo { get; set; }
        public decimal PmCindPorcentaje { get; set; }
        public string PmCindConJuntoEqp { get; set; }
        public string PmCindRelInversa { get; set; }

        public string PmCindEstRegistro { get; set; }
        public string PmCindUsuCreacion { get; set; }
        public DateTime? PmCindFecCreacion { get; set; }
        public string PmCindUsuModificacion { get; set; }
        public DateTime? PmCindFecModificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class PmoConfIndispEquipoDTO
    {
        public int GrupoCodi { get; set; }
        public string GrupoNomb { get; set; }

        public int EmprCodi { get; set; }
        public string Emprnomb { get; set; }

        public string EquiAbrev { get; set; }
        public int Equipadre { get; set; }
        public int FamCodi { get; set; }
        public string Famabrev { get; set; }
        public string AreaNomb { get; set; }
        public string TareaAbrev { get; set; }

        public int Tsddpcodi { get; set; }
        public int Sddpnum { get; set; }
        public string Sddpnomb { get; set; }

        public int Ptomedicodi { get; set; }
        public string Ptomedielenomb { get; set; }
        public string Ptomedidesc { get; set; }

        public int CateCodi { get; set; }
        public string Catenomb { get; set; }
        public string Grupotipo { get; set; }

        public string UsuarioMod { get; set; }
        public DateTime? FechaMod { get; set; }

        public string FechaModStr { get; set; }
        public bool TieneAlerta { get; set; }
        public string MensajeAlerta { get; set; }

        public List<string> ListaEvendescrip { get; set; } = new List<string>();

        public string Central { get; set; }
        public bool TieneCicloComb { get; set; }
        public bool TieneModoCicloSimple { get; set; }
        public bool TieneModoEspecial { get; set; }
        public decimal Pe { get; set; }

        public List<int> ListaEquicodiModo { get; set; } = new List<int>();
        public List<int> ListaEquicodi { get; set; } = new List<int>();
        public List<string> ListaEquiabrev { get; set; } = new List<string>();
        public string SListaEquicodi { get; set; } = "";
        public string SListaEquiabrev { get; set; } = "";
    }
}
