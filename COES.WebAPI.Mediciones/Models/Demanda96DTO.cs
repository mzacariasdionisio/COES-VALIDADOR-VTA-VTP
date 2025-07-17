using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using COES.Base.Core;

namespace COES.WebAPI.Mediciones.Models
{

    /// <summary>
    /// Demanda o Generación cada 30 minutos por día
    /// </summary>  
    public class Demanda96DTO
    {
        
        /// <summary>
        /// Código del punto de medición
        /// </summary>
        public DateTime PTOMEDICODI { get; set; }
        /// <summary>
        /// Primer intervalo de cuarto de hora 00:15
        /// </summary>
        public decimal? H1 { get; set; }
        /// <summary>
        /// Segundo intervalo de cuarto de hora 00:30
        /// </summary>
        public decimal? H2 { get; set; }
        /// <summary>
        /// Tercer intervalo de cuarto de hora 00:45
        /// </summary>
        public decimal? H3 { get; set; }
        /// <summary>
        /// Cuarto intervalo de cuarto de hora 01:00
        /// </summary>
        public decimal? H4 { get; set; }
        /// <summary>
        /// Quinto intervalo de cuarto de hora 01:15
        /// </summary>
        public decimal? H5 { get; set; }
        /// <summary>
        /// Sexto intervalo de cuarto de hora 01:30
        /// </summary>
        public decimal? H6 { get; set; }
        /// <summary>
        /// Séptimo intervalo de cuarto de hora 01:45
        /// </summary>
        public decimal? H7 { get; set; }
        /// <summary>
        /// Octavo intervalo de cuarto de hora 02:00
        /// </summary>
        public decimal? H8 { get; set; }
        /// <summary>
        /// Noveno intervalo de cuarto de hora 02:15
        /// </summary>
        public decimal? H9 { get; set; }
        /// <summary>
        /// Décimo intervalo de cuarto de hora 02:30
        /// </summary>
        public decimal? H10 { get; set; }
        /// <summary>
        /// Décimo primer intervalo de cuarto de hora 02:45
        /// </summary>
        public decimal? H11 { get; set; }
        /// <summary>
        /// Décimo segundo intervalo de cuarto de hora 03:00
        /// </summary>
        public decimal? H12 { get; set; }
        /// <summary>
        /// Décimo tercero intervalo de cuarto de hora 03:15
        /// </summary>
        public decimal? H13 { get; set; }
        /// <summary>
        /// Décimo cuarto intervalo de cuarto de hora 03:30
        /// </summary>
        public decimal? H14 { get; set; }
        /// <summary>
        /// Décimo quinto intervalo de cuarto de hora 03:45
        /// </summary>
        public decimal? H15 { get; set; }
        /// <summary>
        /// Décimo sexto intervalo de cuarto de hora 04:00
        /// </summary>
        public decimal? H16 { get; set; }
        /// <summary>
        /// Décimo séptimo intervalo de cuarto de hora 04:15
        /// </summary>
        public decimal? H17 { get; set; }
        /// <summary>
        /// Décimo octavo intervalo de cuarto de hora 04:30
        /// </summary>
        public decimal? H18 { get; set; }
        /// <summary>
        /// Décimo noveno intervalo de cuarto de hora 04:45
        /// </summary>
        public decimal? H19 { get; set; }
        /// <summary>
        /// vigésimo intervalo de cuarto de hora 05:00
        /// </summary>
        public decimal? H20 { get; set; }
        /// <summary>
        /// vigésimo primer intervalo de cuarto de hora 05:15
        /// </summary>
        public decimal? H21 { get; set; }
        /// <summary>
        /// vigésimo segundo intervalo de cuarto de hora 05:30
        /// </summary>
        public decimal? H22 { get; set; }
        /// <summary>
        /// vigésimo tercero intervalo de cuarto de hora 05:45
        /// </summary> 
        public decimal? H23 { get; set; }
        /// <summary>
        /// vigésimo cuarto intervalo de cuarto de hora 06:00
        /// </summary> 
        public decimal? H24 { get; set; }
        /// <summary>
        /// vigésimo quinto intervalo de cuarto de hora 06:15
        /// </summary>
        public decimal? H25 { get; set; }
        /// <summary>
        /// vigésimo sexto intervalo de cuarto de hora 06:30
        /// </summary>
        public decimal? H26 { get; set; }
        /// <summary>
        /// vigésimo séptimo intervalo de cuarto de hora 06:45
        /// </summary>
        public decimal? H27 { get; set; }
        /// <summary>
        /// vigésimo octavo intervalo de cuarto de hora 07:00
        /// </summary>
        public decimal? H28 { get; set; }
        /// <summary>
        /// vigésimo noveno intervalo de cuarto de hora 07:15
        /// </summary>
        public decimal? H29 { get; set; }
        /// <summary>
        /// trigésimo intervalo de cuarto de hora 07:30
        /// </summary>
        public decimal? H30 { get; set; }
        /// <summary>
        /// trigésimo primer intervalo de cuarto de hora 07:45
        /// </summary>
        public decimal? H31 { get; set; }
        /// <summary>
        /// trigésimo segundo intervalo de cuarto de hora 08:00
        /// </summary>
        public decimal? H32 { get; set; }
        /// <summary>
        /// trigésimo tercero intervalo de cuarto de hora 08:15
        /// </summary>
        public decimal? H33 { get; set; }
        /// <summary>
        /// trigésimo cuarto intervalo de cuarto de hora 08:30
        /// </summary>
        public decimal? H34 { get; set; }
        /// <summary>
        /// trigésimo quinto intervalo de cuarto de hora 08:45
        /// </summary>
        public decimal? H35 { get; set; }
        /// <summary>
        /// trigésimo sexto intervalo de cuarto de hora 09:00
        /// </summary>
        public decimal? H36 { get; set; }
        /// <summary>
        /// trigésimo séptimo intervalo de cuarto de hora 09:15
        /// </summary>
        public decimal? H37 { get; set; }
        /// <summary>
        /// trigésimo octavo intervalo de cuarto de hora 09:30
        /// </summary>
        public decimal? H38 { get; set; }
        /// <summary>
        /// trigésimo noveno intervalo de cuarto de hora 09:45
        /// </summary>
        public decimal? H39 { get; set; }
        /// <summary>
        /// cuadragésimo intervalo de cuarto de hora 10:00
        /// </summary>
        public decimal? H40 { get; set; }
        /// <summary>
        /// cuadragésimo primer intervalo de cuarto de hora 10:15
        /// </summary>
        public decimal? H41 { get; set; }
        /// <summary>
        /// cuadragésimo segundo intervalo de cuarto de hora 10:30
        /// </summary>
        public decimal? H42 { get; set; }
        /// <summary>
        /// cuadragésimo tercer intervalo de cuarto de hora 10:45
        /// </summary>
        public decimal? H43 { get; set; }
        /// <summary>
        /// cuadragésimo cuarto intervalo de cuarto de hora 11:00
        /// </summary>
        public decimal? H44 { get; set; }
        /// <summary>
        /// cuadragésimo quinto intervalo de cuarto de hora 11:15
        /// </summary>
        public decimal? H45 { get; set; }
        /// <summary>
        /// cuadragésimo sexto intervalo de cuarto de hora 11:30
        /// </summary>
        public decimal? H46 { get; set; }
        /// <summary>
        /// cuadragésimo séptmo intervalo de cuarto de  hora 11:45
        /// </summary>
        public decimal? H47 { get; set; }
        /// <summary>
        /// cuadragésimo octavo intervalo de cuarto de hora 12:00
        /// </summary>
        public decimal? H48 { get; set; }
        /// <summary>
        /// cuadragésimo noveno intervalo de cuarto de hora 12:15
        /// </summary>
        public decimal? H49 { get; set; }
        /// <summary>
        /// quincuagésimo intervalo de cuarto de hora 12:30
        /// </summary>
        public decimal? H50 { get; set; }
        /// <summary>
        /// quincuagésimo primer intervalo de cuarto de hora 12:45
        /// </summary>
        public decimal? H51 { get; set; }
        /// <summary>
        /// quincuagésimo segundo intervalo de cuarto de hora 13:00
        /// </summary>
        public decimal? H52 { get; set; }
        /// <summary>
        /// quincuagésimo tercer intervalo de cuarto de hora 13:15
        /// </summary>
        public decimal? H53 { get; set; }
        /// <summary>
        /// quincuagésimo cuarto intervalo de cuarto de hora 13:30
        /// </summary>
        public decimal? H54 { get; set; }
        /// <summary>
        /// quincuagésimo quinto intervalo de cuarto de hora 13:45
        /// </summary>
        public decimal? H55 { get; set; }
        /// <summary>
        /// quincuagésimo sexto intervalo de cuarto de hora 14:00
        /// </summary>
        public decimal? H56 { get; set; }
        /// <summary>
        /// quincuagésimo séptimo intervalo de cuarto de hora 14:15
        /// </summary>
        public decimal? H57 { get; set; }
        /// <summary>
        /// quincuagésimo octavo intervalo de cuarto de hora 14:30
        /// </summary>
        public decimal? H58 { get; set; }
        /// <summary>
        /// quincuagésimo noveno intervalo de cuarto de hora 14:45
        /// </summary>
        public decimal? H59 { get; set; }
        /// <summary>
        /// sexagésimo intervalo de cuarto de hora 15:00
        /// </summary>
        public decimal? H60 { get; set; }
        /// <summary>
        /// sexagésimo primer intervalo de cuarto de hora 15:15
        /// </summary>
        public decimal? H61 { get; set; }
        /// <summary>
        /// sexagésimo segundo intervalo de cuarto de hora 15:30
        /// </summary>
        public decimal? H62 { get; set; }
        /// <summary>
        /// sexagésimo tercer intervalo de cuarto de hora 15:45
        /// </summary>
        public decimal? H63 { get; set; }
        /// <summary>
        /// sexagésimo cuarto intervalo de cuarto de hora 16:00
        /// </summary>
        public decimal? H64 { get; set; }
        /// <summary>
        /// sexagésimo quinto intervalo de cuarto de hora 16:15
        /// </summary>
        public decimal? H65 { get; set; }
        /// <summary>
        /// sexagésimo sexto intervalo de cuarto de hora 16:30
        /// </summary>
        public decimal? H66 { get; set; }
        /// <summary>
        /// sexagésimo séptimo intervalo de cuarto de hora 16:45
        /// </summary>
        public decimal? H67 { get; set; }
        /// <summary>
        /// sexagésimo octavo intervalo de cuarto de hora 17:00
        /// </summary>
        public decimal? H68 { get; set; }
        /// <summary>
        /// sexagésimo noveno intervalo de cuarto de hora 17:15
        /// </summary>
        public decimal? H69 { get; set; }
        /// <summary>
        /// septuagésimo intervalo de cuarto de hora 17:30
        /// </summary>
        public decimal? H70 { get; set; }
        /// <summary>
        /// septuagésimo primer intervalo de cuarto de hora 17:45
        /// </summary>
        public decimal? H71 { get; set; }
        /// <summary>
        /// septuagésimo segundo intervalo de cuarto de hora 18:00
        /// </summary>
        public decimal? H72 { get; set; }
        /// <summary>
        /// septuagésimo tercer intervalo de cuarto de hora 18:15
        /// </summary>
        public decimal? H73 { get; set; }
        /// <summary>
        /// septuagésimo cuarto intervalo de cuarto de hora 18:30
        /// </summary>
        public decimal? H74 { get; set; }
        /// <summary>
        /// septuagésimo quinto intervalo de cuarto de hora 18:45
        /// </summary>
        public decimal? H75 { get; set; }
        /// <summary>
        /// septuagésimo sexto intervalo de cuarto de hora 19:00
        /// </summary>
        public decimal? H76 { get; set; }
        /// <summary>
        /// septuagésimo séptimo intervalo de cuarto de hora 19:15
        /// </summary>
        public decimal? H77 { get; set; }
        /// <summary>
        /// septuagésimo octavo intervalo de cuarto de hora 19:30
        /// </summary>
        public decimal? H78 { get; set; }
        /// <summary>
        /// septuagésimo noveno intervalo de cuarto de hora 19:45
        /// </summary>
        public decimal? H79 { get; set; }
        /// <summary>
        /// octogésimo intervalo de cuarto de hora 20:00
        /// </summary>        
        public decimal? H80 { get; set; }
        /// <summary>
        /// octogésimo primer intervalo de cuarto de hora 20:15
        /// </summary>
        public decimal? H81 { get; set; }
        /// <summary>
        /// octogésimo segundo intervalo de cuarto de hora 20:30
        /// </summary>
        public decimal? H82 { get; set; }
        /// <summary>
        /// octogésimo tercer intervalo de cuarto de hora 20:45
        /// </summary>
        public decimal? H83 { get; set; }
        /// <summary>
        /// octogésimo cuarto intervalo de cuarto de hora 21:00
        /// </summary>
        public decimal? H84 { get; set; }
        /// <summary>
        /// octogésimo quinto intervalo de cuarto de hora 21:15
        /// </summary>
        public decimal? H85 { get; set; }
        /// <summary>
        /// octogésimo sexto intervalo de cuarto de hora 21:30
        /// </summary>
        public decimal? H86 { get; set; }
        /// <summary>
        /// octogésimo séptimo intervalo de cuarto de hora 21:45
        /// </summary>
        public decimal? H87 { get; set; }
        /// <summary>
        /// octogésimo octavo intervalo de cuarto de hora 22:00
        /// </summary>
        public decimal? H88 { get; set; }
        /// <summary>
        /// octogésimo noveno intervalo de cuarto de hora 22:15
        /// </summary>
        public decimal? H89 { get; set; }
        /// <summary>
        /// nonagésimo intervalo de cuarto de hora 22:30
        /// </summary>
        public decimal? H90 { get; set; }
        /// <summary>
        /// nonagésimo primer intervalo de cuarto de hora 22:45
        /// </summary>
        public decimal? H91 { get; set; }
        /// <summary>
        /// nonagésimo segundo intervalo de cuarto de hora 23:00
        /// </summary>
        public decimal? H92 { get; set; }
        /// <summary>
        /// nonagésimo tercer intervalo de cuarto de hora 23:15
        /// </summary>
        public decimal? H93 { get; set; }
        /// <summary>
        /// nonagésimo cuarto intervalo de cuarto de hora 23:30
        /// </summary>
        public decimal? H94 { get; set; }
        /// <summary>
        /// nonagésimo quinto intervalo de cuarto de hora 23:45
        /// </summary>
        public decimal? H95 { get; set; }
        /// <summary>
        /// nonagésimo sexto intervalo de cuarto de hora 00:00
        /// </summary>
        public decimal? H96 { get; set; }
        ///<summary>
        ///tipo de informacion (activa/reactiva)
        /// </summary>
        public string Tipoinfocodidesc { get; set; }

    }
}