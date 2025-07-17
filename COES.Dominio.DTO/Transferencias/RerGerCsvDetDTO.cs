using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_GERCSV_DET y que contiene la matriz temporal
    /// </summary>
    public class RerGerCsvDetDTO : EntityBase
    {
        public int Regedcodi { get; set; }
        public int Regercodi { get; set; }

        public int? Emprcodi { get; set; }
        public int? Equicodi { get; set; }
        public string Regedtipcsv { get; set; }
        public DateTime Regedfecha { get; set; }

        public decimal? Regedh1 { get; set; }
        public decimal? Regedh2 { get; set; }
        public decimal? Regedh3 { get; set; }
        public decimal? Regedh4 { get; set; }
        public decimal? Regedh5 { get; set; }
        public decimal? Regedh6 { get; set; }
        public decimal? Regedh7 { get; set; }
        public decimal? Regedh8 { get; set; }
        public decimal? Regedh9 { get; set; }
        public decimal? Regedh10 { get; set; }
        public decimal? Regedh11 { get; set; }
        public decimal? Regedh12 { get; set; }
        public decimal? Regedh13 { get; set; }
        public decimal? Regedh14 { get; set; }
        public decimal? Regedh15 { get; set; }
        public decimal? Regedh16 { get; set; }
        public decimal? Regedh17 { get; set; }
        public decimal? Regedh18 { get; set; }
        public decimal? Regedh19 { get; set; }
        public decimal? Regedh20 { get; set; }
        public decimal? Regedh21 { get; set; }
        public decimal? Regedh22 { get; set; }
        public decimal? Regedh23 { get; set; }
        public decimal? Regedh24 { get; set; }
        public decimal? Regedh25 { get; set; }
        public decimal? Regedh26 { get; set; }
        public decimal? Regedh27 { get; set; }
        public decimal? Regedh28 { get; set; }
        public decimal? Regedh29 { get; set; }
        public decimal? Regedh30 { get; set; }
        public decimal? Regedh31 { get; set; }
        public decimal? Regedh32 { get; set; }
        public decimal? Regedh33 { get; set; }
        public decimal? Regedh34 { get; set; }
        public decimal? Regedh35 { get; set; }
        public decimal? Regedh36 { get; set; }
        public decimal? Regedh37 { get; set; }
        public decimal? Regedh38 { get; set; }
        public decimal? Regedh39 { get; set; }
        public decimal? Regedh40 { get; set; }
        public decimal? Regedh41 { get; set; }
        public decimal? Regedh42 { get; set; }
        public decimal? Regedh43 { get; set; }
        public decimal? Regedh44 { get; set; }
        public decimal? Regedh45 { get; set; }
        public decimal? Regedh46 { get; set; }
        public decimal? Regedh47 { get; set; }
        public decimal? Regedh48 { get; set; }
        public decimal? Regedh49 { get; set; }
        public decimal? Regedh50 { get; set; }
        public decimal? Regedh51 { get; set; }
        public decimal? Regedh52 { get; set; }
        public decimal? Regedh53 { get; set; }
        public decimal? Regedh54 { get; set; }
        public decimal? Regedh55 { get; set; }
        public decimal? Regedh56 { get; set; }
        public decimal? Regedh57 { get; set; }
        public decimal? Regedh58 { get; set; }
        public decimal? Regedh59 { get; set; }
        public decimal? Regedh60 { get; set; }
        public decimal? Regedh61 { get; set; }
        public decimal? Regedh62 { get; set; }
        public decimal? Regedh63 { get; set; }
        public decimal? Regedh64 { get; set; }
        public decimal? Regedh65 { get; set; }
        public decimal? Regedh66 { get; set; }
        public decimal? Regedh67 { get; set; }
        public decimal? Regedh68 { get; set; }
        public decimal? Regedh69 { get; set; }
        public decimal? Regedh70 { get; set; }
        public decimal? Regedh71 { get; set; }
        public decimal? Regedh72 { get; set; }
        public decimal? Regedh73 { get; set; }
        public decimal? Regedh74 { get; set; }
        public decimal? Regedh75 { get; set; }
        public decimal? Regedh76 { get; set; }
        public decimal? Regedh77 { get; set; }
        public decimal? Regedh78 { get; set; }
        public decimal? Regedh79 { get; set; }
        public decimal? Regedh80 { get; set; }
        public decimal? Regedh81 { get; set; }
        public decimal? Regedh82 { get; set; }
        public decimal? Regedh83 { get; set; }
        public decimal? Regedh84 { get; set; }
        public decimal? Regedh85 { get; set; }
        public decimal? Regedh86 { get; set; }
        public decimal? Regedh87 { get; set; }
        public decimal? Regedh88 { get; set; }
        public decimal? Regedh89 { get; set; }
        public decimal? Regedh90 { get; set; }
        public decimal? Regedh91 { get; set; }
        public decimal? Regedh92 { get; set; }
        public decimal? Regedh93 { get; set; }
        public decimal? Regedh94 { get; set; }
        public decimal? Regedh95 { get; set; }
        public decimal? Regedh96 { get; set; }
        public string Regedusucreacion { get; set; }
        public DateTime Regedfeccreacion { get; set; }

        public string Equinomb { get; set; }
        public string Emprnomb { get; set; }
    }

    public class RerGerCsvDetTempDTO : EntityBase
    {
        public int Bloque { get; set; }
        public decimal? Dia1 { get; set; } // Sabado
        public decimal? Dia2 { get; set; } // Domingo
        public decimal? Dia3 { get; set; } // Lunes
        public decimal? Dia4 { get; set; } // Martes
        public decimal? Dia5 { get; set; } // Miercoles
        public decimal? Dia6 { get; set; } // Jueves
        public decimal? Dia7 { get; set; } // Viernes
    }
}

