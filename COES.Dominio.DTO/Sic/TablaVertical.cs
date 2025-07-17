using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class TablaVertical : EntityBase
    {
            public int Ptomedicodi { get; set; }
            public string Ptomedielenomb { get; set; }
            public string Equinomb { get; set; }
            public string Emprnomb{ get; set;}
            public int Anio { get; set; }
            public decimal? M1 { get; set; }
            public decimal? M2 { get; set; }
            public decimal? M3 { get; set; }
            public decimal? M4 { get; set; }
            public decimal? M5 { get; set; }
            public decimal? M6 { get; set; }
            public decimal? M7 { get; set; }
            public decimal? M8 { get; set; }
            public decimal? M9 { get; set; }
            public decimal? M10 { get; set; }
            public decimal? M11 { get; set; }
            public decimal? M12 { get; set; }
            public decimal? minimo { get; set; }
            public decimal? maximo { get; set; }
            public decimal? promedio { get; set; }
            public decimal? desv { get; set; }
            public string Caudal { get; set; }


    }
}
