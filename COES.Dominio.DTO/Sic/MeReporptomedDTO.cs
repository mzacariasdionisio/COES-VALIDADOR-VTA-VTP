using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_REPORPTOMED
    /// </summary>
    [Serializable]
    public partial class MeReporptomedDTO : EntityBase, ICloneable
    {
        public int Repptocodi { get; set; }
        public int Reporcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Repptoorden { get; set; }
        public int Repptoestado { get; set; }

        public int Repptotabmed { get; set; }
        public string Repptonomb { get; set; }
        public decimal? Relptofactor { get; set; }
        public decimal? Relptopotencia { get; set; }


        public int? Funptocodi { get; set; }
        public string Repptocolorcelda { get; set; }
        public string Repptoindcopiado { get; set; }

        public int? Repptoequivpto { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    public partial class MeReporptomedDTO
    {
        public string RepptotabmedDesc { get; set; }

        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public string Tipoptomedinomb { get; set; }
        public string Tipoinfoabrev { get; set; }
        public string Ptomedibarranomb { get; set; }
        public int Tipoptomedicodi { get; set; }
        public string Ptomedielenomb { get; set; }
        public string Ptomedidesc { get; set; }
        public int Famcodi { get; set; }
        public string Famnomb { get; set; }
        public string Famabrev { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Emprabrev { get; set; }
        public string Cuenca { get; set; }
        public string Equipopadre { get; set; }
        public int Equipadre { get; set; }
        public int Equicodi { get; set; }
        public string Equiabrev { get; set; }
        public string Valor1 { get; set; }
        public decimal? Valor2 { get; set; }
        public decimal? Valor3 { get; set; }
        public string Areanomb { get; set; }
        public string AreaOperativa { get; set; }
        public string Suministrador { get; set; }
        public string CodigoOsinergmin { get; set; }
        public string Unidad { get; set; }
        public string PtomediCalculado { get; set; }
        public string PtomediCalculadoDescrip { get; set; }
        public int PtomedicodiCalculado { get; set; }
        public string PtomedicodiCalculadoDescrip { get; set; }
        public string RepptoestadoDescrip { get; set; }
        public string Repornombre { get; set; }
        public int EmprcodiOrigen { get; set; }
        public string EmprabrevOrigen { get; set; }
        public string TipoRelacionnombre { get; set; }
        public int TipoRelacioncodi { get; set; }
        public int PtomedicodiOrigen { get; set; }
        public string PtomedibarranombOrigen { get; set; }
        public string PtomedielenombOrigen { get; set; }
        public string PtomedicodidescOrigen { get; set; }
        public decimal FactorOrigen { get; set; }
        public int EquicodiOrigen { get; set; }
        public string EquinombOrigen { get; set; }
        public decimal Relptocodi { get; set; }

        public int Areacodi { get; set; }
        public int Subestacioncodi { get; set; }
        public string Valor4 { get; set; }
        public string Valor5 { get; set; }
        public string Central { get; set; }
        public int Origlectcodi { get; set; }
        public string Origlectnombre { get; set; }
        public decimal? Meditotal { get; set; }
        public int Lectcodi { get; set; }
        public string Lectnomb { get; set; }
        public int? TipoinfocodiOrigen { get; set; }
        public int Tgenercodi { get; set; }
        public string EmprnombOrigen { get; set; }
        public int OrdenArea { get; set; }

        public string Funptofuncion { get; set; }
        public DateTime Medifecha { get; set; }
        public int NumeroSemana { get; set; }
        public string Osicodi { get; set; }
        public int? Codref { get; set; }

        public string DescPto { get; set; }
        public string RepptoequivptoDesc { get; set; }
    }
}
