using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_ASIGNACIONRESERVA
    /// </summary>
    public class VcrAsignacionreservaDTO : EntityBase
    {
        public int Vcrarcodi { get; set; }
        public int Vcrecacodi { get; set; }
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public DateTime Vcrarfecha { get; set; }
        public decimal Vcrarrapbf { get; set; }
        public decimal Vcrarprbf { get; set; }
        public decimal Vcrarrama { get; set; }
        public decimal Vcrarmpa { get; set; }
        public decimal Vcrarasignreserva { get; set; }
        public string Vcrarusucreacion { get; set; }
        public DateTime Vcrarfeccreacion { get; set; }
        //20181105
        public decimal Vcrarramaursra { get; set; }
        //202010
        public decimal Vcrarrapbfbajar { get; set; }
        public decimal Vcrarprbfbajar { get; set; }
        public decimal Vcrarramabajar { get; set; }
        public decimal Vcrarmpabajar { get; set; }
        public decimal Vcrarramaursrabajar { get; set; }
    }

    public class VcrEveRsfhoraDTO : EntityBase
    {
        public int Rsfhorcodi { get; set; }
        public string Rsfhorindman { get; set; }
        public string Rsfhorindaut { get; set; }
        public string Rsfhorcomentario { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public DateTime? Rsfhorfecha { get; set; }
        public DateTime? Rsfhorinicio { get; set; }
        public DateTime? Rsfhorfin { get; set; }
        public decimal? Rsfhormaximo { get; set; }
        public string Ursnomb { get; set; }
        public decimal Valorautomatico { get; set; }
        public int Indicador { get; set; }

        public decimal Rsfdetsub { get; set; }

        public decimal Rsfdetbaj { get; set; }

    }

}