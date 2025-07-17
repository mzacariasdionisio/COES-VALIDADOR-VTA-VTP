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
    public class Demanda48 
    {
        
        /// <summary>
        /// Fecha de medición
        /// </summary>
        public DateTime Medifecha { get; set; }
        ///// <summary>
        ///// Tipo info codigo
        ///// </summary>
        //public int Tipoinfocodi { get; set; }
        ///// <summary>
        ///// Tipo info descripción
        ///// </summary>
        //public string Tipoinfodesc { get; set; }
        /// <summary>
        /// Código del punto de medición
        /// </summary>
        public int Ptomedicodi { get; set; }              
        /// <summary>
        /// Código del equipo
        /// </summary>
        public int CodigoEquipo { get; set; }
        /// <summary>
        /// Nombre del equipo
        /// </summary>
        public string NombreEquipo { get; set; }
        /// <summary>
        /// Codigo de la empresa
        /// </summary>
        public int Emprcodi { get; set; }
        /// <summary>
        /// Nombre de la empresa
        /// </summary>
        public string Emprnomb { get; set; }        
        /// <summary>
        /// Tensión del equipo
        /// </summary>
        public decimal? Equitension { get; set; }
        /// <summary>
        /// Area del código
        /// </summary>
        public int CodigoUbicacion { get; set; }
        /// <summary>
        /// Área del nombre
        /// </summary>
        public string NombreUbicacion { get; set; }
        /// <summary>
        /// Primer intervalo de media hora 00:30
        /// </summary>
        public decimal? H1 { get; set; }
        /// <summary>
        /// Segundo intervalo de media hora 01:00
        /// </summary>
        public decimal? H2 { get; set; }
        /// <summary>
        /// Tercer intervalo de media hora 01:30
        /// </summary>
        public decimal? H3 { get; set; }
        /// <summary>
        /// Cuarto intervalo de media hora 02:00
        /// </summary>
        public decimal? H4 { get; set; }
        /// <summary>
        /// Quinto intervalo de media hora 02:30
        /// </summary>
        public decimal? H5 { get; set; }
        /// <summary>
        /// Sexto intervalo de media hora 03:00
        /// </summary>
        public decimal? H6 { get; set; }
        /// <summary>
        /// Séptimo intervalo de media hora 03:30
        /// </summary>
        public decimal? H7 { get; set; }
        /// <summary>
        /// Octavo intervalo de media hora 04:00
        /// </summary>
        public decimal? H8 { get; set; }
        /// <summary>
        /// Noveno intervalo de media hora 04:30
        /// </summary>
        public decimal? H9 { get; set; }
        /// <summary>
        /// Décimo intervalo de media hora 05:00
        /// </summary>
        public decimal? H10 { get; set; }
        /// <summary>
        /// Décimo primer intervalo de media hora 05:30
        /// </summary>
        public decimal? H11 { get; set; }
        /// <summary>
        /// Décimo segundo intervalo de media hora 06:00
        /// </summary>
        public decimal? H12 { get; set; }
        /// <summary>
        /// Décimo tercero intervalo de media hora 06:30
        /// </summary>
        public decimal? H13 { get; set; }
        /// <summary>
        /// Décimo cuarto intervalo de media hora 07:00
        /// </summary>
        public decimal? H14 { get; set; }
        /// <summary>
        /// Décimo quinto intervalo de media hora 07:30
        /// </summary>
        public decimal? H15 { get; set; }
        /// <summary>
        /// Décimo sexto intervalo de media hora 08:00
        /// </summary>
        public decimal? H16 { get; set; }
        /// <summary>
        /// Décimo séptimo intervalo de media hora 08:30
        /// </summary>
        public decimal? H17 { get; set; }
        /// <summary>
        /// Décimo octavo intervalo de media hora 09:00
        /// </summary>
        public decimal? H18 { get; set; }
        /// <summary>
        /// Décimo noveno intervalo de media hora 09:30
        /// </summary>
        public decimal? H19 { get; set; }
        /// <summary>
        /// vigésimo intervalo de media hora 10:00
        /// </summary>
        public decimal? H20 { get; set; }
        /// <summary>
        /// vigésimo primer intervalo de media hora 10:30
        /// </summary>
        public decimal? H21 { get; set; }
        /// <summary>
        /// vigésimo segundo intervalo de media hora 11:00
        /// </summary> 
        public decimal? H22 { get; set; }
        /// <summary>
        /// vigésimo tercero intervalo de media hora 11:30
        /// </summary> 
        public decimal? H23 { get; set; }
        /// <summary>
        /// vigésimo cuarto intervalo de media hora 12:00
        /// </summary>
        public decimal? H24 { get; set; }
        /// <summary>
        /// vigésimo quinto intervalo de media hora 12:30
        /// </summary>
        public decimal? H25 { get; set; }
        /// <summary>
        /// vigésimo sexto intervalo de media hora 13:00
        /// </summary>
        public decimal? H26 { get; set; }
        /// <summary>
        /// vigésimo séptimo intervalo de media hora 13:30
        /// </summary>
        public decimal? H27 { get; set; }
        /// <summary>
        /// vigésimo octavo intervalo de media hora 14:00
        /// </summary>
        public decimal? H28 { get; set; }
        /// <summary>
        /// trigésimo noveno intervalo de media hora 14:30
        /// </summary>
        public decimal? H29 { get; set; }
        /// <summary>
        /// trigésimo intervalo de media hora 15:00
        /// </summary>
        public decimal? H30 { get; set; }
        /// <summary>
        /// trigésimo primer intervalo de media hora 15:30
        /// </summary>
        public decimal? H31 { get; set; }
        /// <summary>
        /// trigésimo segundo intervalo de media hora 16:00
        /// </summary>
        public decimal? H32 { get; set; }
        /// <summary>
        /// trigésimo tercero intervalo de media hora 16:30
        /// </summary>
        public decimal? H33 { get; set; }
        /// <summary>
        /// trigésimo cuarto intervalo de media hora 17:00
        /// </summary>
        public decimal? H34 { get; set; }
        /// <summary>
        /// trigésimo quinto intervalo de media hora 17:30
        /// </summary>
        public decimal? H35 { get; set; }
        /// <summary>
        /// trigésimo sexto intervalo de media hora 18:00
        /// </summary>
        public decimal? H36 { get; set; }
        /// <summary>
        /// trigésimo séptimo intervalo de media hora 18:30
        /// </summary>
        public decimal? H37 { get; set; }
        /// <summary>
        /// trigésimo octavo intervalo de media hora 19:00
        /// </summary>
        public decimal? H38 { get; set; }
        /// <summary>
        /// trigésimo noveno intervalo de media hora 19:30
        /// </summary>
        public decimal? H39 { get; set; }
        /// <summary>
        /// cuadragésimo intervalo de media hora 20:00
        /// </summary>
        public decimal? H40 { get; set; }
        /// <summary>
        /// cuadragésimo primer intervalo de media hora 20:30
        /// </summary>
        public decimal? H41 { get; set; }
        /// <summary>
        /// cuadragésimo segundo intervalo de media hora 21:00
        /// </summary>
        public decimal? H42 { get; set; }
        /// <summary>
        /// cuadragésimo tercer intervalo de media hora 21:30
        /// </summary>
        public decimal? H43 { get; set; }
        /// <summary>
        /// cuadragésimo cuarto intervalo de media hora 22:00
        /// </summary>
        public decimal? H44 { get; set; }
        /// <summary>
        /// cuadragésimo quinto intervalo de media hora 22:30
        /// </summary>
        public decimal? H45 { get; set; }
        /// <summary>
        /// cuadragésimo sexto intervalo de media hora 23:00
        /// </summary>
        public decimal? H46 { get; set; }
        /// <summary>
        /// cuadragésimo séptmo intervalo de media hora 23:30
        /// </summary>
        public decimal? H47 { get; set; }
        /// <summary>
        /// cuadragésimo octavo intervalo de media hora 00:00
        /// </summary>
        public decimal? H48 { get; set; }
        /// <summary>
        /// Unidad de medida
        /// </summary>
        public string Tipoinfoabrev { get; set; }
        /// <summary>
        /// Estado del primer intervalo de media hora 00:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T1 { get; set; }
        /// <summary>
        /// Estado del segundo intervalo de media hora 01:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T2 { get; set; }
        /// <summary>
        /// Estado del Tercer intervalo de media hora 01:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T3 { get; set; }
        /// <summary>
        /// Estado del Cuarto intervalo de media hora 02:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T4 { get; set; }
        /// <summary>
        /// Estado del Quinto intervalo de media hora 02:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T5 { get; set; }
        /// <summary>
        /// Estado del Sexto intervalo de media hora 03:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T6 { get; set; }
        /// <summary>
        /// Estado del Séptimo intervalo de media hora 03:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T7 { get; set; }
        /// <summary>
        /// Estado del Octavo intervalo de media hora 04:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T8 { get; set; }
        /// <summary>
        /// Estado del Noveno intervalo de media hora 04:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T9 { get; set; }
        /// <summary>
        /// Estado del Décimo intervalo de media hora 05:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T10 { get; set; }
        /// <summary>
        /// Estado del Décimo primer intervalo de media hora 05:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T11 { get; set; }
        /// <summary>
        /// Estado del Décimo segundo intervalo de media hora 06:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T12 { get; set; }
        /// <summary>
        /// Estado del Décimo tercero intervalo de media hora 06:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T13 { get; set; }
        /// <summary>
        /// Estado del Décimo cuarto intervalo de media hora 07:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T14 { get; set; }
        /// <summary>
        /// Estado del Décimo quinto intervalo de media hora 07:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T15 { get; set; }
        /// <summary>
        /// Estado del Décimo sexto intervalo de media hora 08:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T16 { get; set; }
        /// <summary>
        /// Estado del Décimo séptimo intervalo de media hora 08:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T17 { get; set; }
        /// <summary>
        /// Estado del Décimo octavo intervalo de media hora 09:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T18 { get; set; }
        /// <summary>
        /// Estado del Décimo noveno intervalo de media hora 09:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T19 { get; set; }
        /// <summary>
        /// Estado del vigésimo intervalo de media hora 10:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T20 { get; set; }
        /// <summary>
        /// Estado del vigésimo primer intervalo de media hora 10:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T21 { get; set; }
        /// <summary>
        /// Estado del vigésimo segundo intervalo de media hora 11:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary> 
        public decimal? T22 { get; set; }
        /// <summary>
        /// Estado del vigésimo tercero intervalo de media hora 11:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary> 
        public decimal? T23 { get; set; }
        /// <summary>
        /// Estado del vigésimo cuarto intervalo de media hora 12:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T24 { get; set; }
        /// <summary>
        /// Estado del vigésimo quinto intervalo de media hora 12:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T25 { get; set; }
        /// <summary>
        /// Estado del vigésimo sexto intervalo de media hora 13:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T26 { get; set; }
        /// <summary>
        /// Estado del vigésimo séptimo intervalo de media hora 13:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T27 { get; set; }
        /// <summary>
        /// Estado del vigésimo octavo intervalo de media hora 14:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T28 { get; set; }
        /// <summary>
        /// Estado del trigésimo noveno intervalo de media hora 14:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T29 { get; set; }
        /// <summary>
        /// Estado del trigésimo intervalo de media hora 15:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T30 { get; set; }
        /// <summary>
        /// Estado del trigésimo primer intervalo de media hora 15:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T31 { get; set; }
        /// <summary>
        /// Estado del trigésimo segundo intervalo de media hora 16:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T32 { get; set; }
        /// <summary>
        /// Estado del trigésimo tercero intervalo de media hora 16:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T33 { get; set; }
        /// <summary>
        /// Estado del trigésimo cuarto intervalo de media hora 17:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T34 { get; set; }
        /// <summary>
        /// Estado del trigésimo quinto intervalo de media hora 17:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T35 { get; set; }
        /// <summary>
        /// Estado del trigésimo sexto intervalo de media hora 18:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T36 { get; set; }
        /// <summary>
        /// Estado del trigésimo séptimo intervalo de media hora 18:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T37 { get; set; }
        /// <summary>
        /// Estado del trigésimo octavo intervalo de media hora 19:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T38 { get; set; }
        /// <summary>
        /// Estado del trigésimo noveno intervalo de media hora 19:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T39 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo intervalo de media hora 20:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T40 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo primer intervalo de media hora 20:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T41 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo segundo intervalo de media hora 21:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T42 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo tercer intervalo de media hora 21:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T43 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo cuarto intervalo de media hora 22:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T44 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo quinto intervalo de media hora 22:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T45 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo sexto intervalo de media hora 23:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T46 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo séptmo intervalo de media hora 23:30. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T47 { get; set; }
        /// <summary>
        /// Estado del cuadragésimo octavo intervalo de media hora 00:00. Estado 0: Valor numérico por defecto. Estado 1: Valor extraído del Scada. Estado 2: Valor extraído del TNA. Estado 3: Valor extraído del Programa diario (Yupana). Estado 4: Valor sin modificación.
        /// </summary>
        public decimal? T48 { get; set; }
    }
}