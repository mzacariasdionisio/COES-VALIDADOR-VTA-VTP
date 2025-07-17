using System;
using System.Collections.Generic;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.DTO.Scada
{
    /// <summary>
    /// Clase que mapea la tabla ME_SCADA_SP7
    /// </summary>
    public class MeScadaSp7DTO : EntityBase
    {
        public int Canalcodi { get; set; } 
        public DateTime Medifecha { get; set; } 
        public decimal? Meditotal { get; set; } 
        public string Mediestado { get; set; } 
        public decimal? H1 { get; set; } 
        public decimal? H2 { get; set; } 
        public decimal? H3 { get; set; } 
        public decimal? H4 { get; set; } 
        public decimal? H5 { get; set; } 
        public decimal? H6 { get; set; } 
        public decimal? H7 { get; set; } 
        public decimal? H8 { get; set; } 
        public decimal? H9 { get; set; } 
        public decimal? H10 { get; set; } 
        public decimal? H11 { get; set; } 
        public decimal? H12 { get; set; } 
        public decimal? H13 { get; set; } 
        public decimal? H14 { get; set; } 
        public decimal? H15 { get; set; } 
        public decimal? H16 { get; set; } 
        public decimal? H17 { get; set; } 
        public decimal? H18 { get; set; } 
        public decimal? H19 { get; set; } 
        public decimal? H20 { get; set; } 
        public decimal? H21 { get; set; } 
        public decimal? H22 { get; set; } 
        public decimal? H23 { get; set; } 
        public decimal? H24 { get; set; } 
        public decimal? H25 { get; set; } 
        public decimal? H26 { get; set; } 
        public decimal? H27 { get; set; } 
        public decimal? H28 { get; set; } 
        public decimal? H29 { get; set; } 
        public decimal? H30 { get; set; } 
        public decimal? H31 { get; set; } 
        public decimal? H32 { get; set; } 
        public decimal? H33 { get; set; } 
        public decimal? H34 { get; set; } 
        public decimal? H35 { get; set; } 
        public decimal? H36 { get; set; } 
        public decimal? H37 { get; set; } 
        public decimal? H38 { get; set; } 
        public decimal? H39 { get; set; } 
        public decimal? H40 { get; set; } 
        public decimal? H41 { get; set; } 
        public decimal? H42 { get; set; } 
        public decimal? H43 { get; set; } 
        public decimal? H44 { get; set; } 
        public decimal? H45 { get; set; } 
        public decimal? H46 { get; set; } 
        public decimal? H47 { get; set; } 
        public decimal? H48 { get; set; } 
        public decimal? H49 { get; set; } 
        public decimal? H50 { get; set; } 
        public decimal? H51 { get; set; } 
        public decimal? H52 { get; set; } 
        public decimal? H53 { get; set; } 
        public decimal? H54 { get; set; } 
        public decimal? H55 { get; set; } 
        public decimal? H56 { get; set; } 
        public decimal? H57 { get; set; } 
        public decimal? H58 { get; set; } 
        public decimal? H59 { get; set; } 
        public decimal? H60 { get; set; } 
        public decimal? H61 { get; set; } 
        public decimal? H62 { get; set; } 
        public decimal? H63 { get; set; } 
        public decimal? H64 { get; set; } 
        public decimal? H65 { get; set; } 
        public decimal? H66 { get; set; } 
        public decimal? H67 { get; set; } 
        public decimal? H68 { get; set; } 
        public decimal? H69 { get; set; } 
        public decimal? H70 { get; set; } 
        public decimal? H71 { get; set; } 
        public decimal? H72 { get; set; } 
        public decimal? H73 { get; set; } 
        public decimal? H74 { get; set; } 
        public decimal? H75 { get; set; } 
        public decimal? H76 { get; set; } 
        public decimal? H77 { get; set; } 
        public decimal? H78 { get; set; } 
        public decimal? H79 { get; set; } 
        public decimal? H80 { get; set; } 
        public decimal? H81 { get; set; } 
        public decimal? H82 { get; set; } 
        public decimal? H83 { get; set; } 
        public decimal? H84 { get; set; } 
        public decimal? H85 { get; set; } 
        public decimal? H86 { get; set; } 
        public decimal? H87 { get; set; } 
        public decimal? H88 { get; set; } 
        public decimal? H89 { get; set; } 
        public decimal? H90 { get; set; } 
        public decimal? H91 { get; set; } 
        public decimal? H92 { get; set; } 
        public decimal? H93 { get; set; } 
        public decimal? H94 { get; set; } 
        public decimal? H95 { get; set; } 
        public decimal? H96 { get; set; } 
        public string Medicambio { get; set; } 
        public string Medscdusucreacion { get; set; } 
        public DateTime? Medscdfeccreacion { get; set; } 
        public string Medscdusumodificacion { get; set; } 
        public DateTime? Medscdfecmodificacion { get; set; } 

        public string Zonanomb { get; set; }
        public string Canalnomb { get; set; }
        public string Canalunidad { get; set; }
        public int Ptomedicodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Equicodi { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public int Hopcodi { get; set; }
        public string FechaIniDesc { get; set; }
        public string FechaFinDesc { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public EveHoraoperacionDTO HoraOperacion { get; set; }
        public int Tgenercodi { get; set; }
        public decimal Factor { get; set; }
        public int Canalcalidad { get; set; }
        public int Grupocodi { get; set; }
        public int Famcodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public int Equipadre { get; set; }

    }
}
