using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_NODO_CONTINUO
    /// </summary>
    public partial class CpNodoContinuoDTO : EntityBase, ICloneable
    {
        public int Cpnodocodi { get; set; }
        public int Cparbcodi { get; set; }

        public string Cpnodoestado { get; set; } //C: creado, E: ejecutado, N: no ejecutado
        public string Cpnodoconverge { get; set; } //C: converge. D: diverge.

        public string Cpnodoflagcondterm { get; set; }
        public string Cpnodoflagconcompr { get; set; }
        public string Cpnodoflagsincompr { get; set; }
        public string Cpnodoflagrer { get; set; }

        public int Cpnodonumero { get; set; }
        public string Cpnodocarpeta { get; set; }

        public DateTime? Cpnodofeciniproceso { get; set; }
        public DateTime? Cpnodofecfinproceso { get; set; }
        public string Cpnodomsjproceso { get; set; }

        public int? Cpnodoidtopguardado { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class CpNodoContinuoDTO
    {
        public int Nivel { get; set; }
        public int OrdenXNivel { get; set; }
        public int NodoPadre { get; set; }
        public List<int> ListaHijo { get; set; } = new List<int>();
        
        public int Topcodi { get; set; }
        public int Topiniciohora { get; set; }
        public string Topnombre { get; set; }

        public List<CpNodoDetalleDTO> ListaNodosDetalle { get; set; } = new List<CpNodoDetalleDTO>();
        public decimal? CostoOperacion { get; set; }
        public decimal? CostoRacionamiento { get; set; }
        public List<MeMedicion48DTO> ListaCMarginales { get; set; } = new List<MeMedicion48DTO>();

    }
}
