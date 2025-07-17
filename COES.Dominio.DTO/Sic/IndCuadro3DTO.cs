using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_CUADRO3
    /// </summary>
    public class IndCuadro3DTO : EntityBase
    {
        public int Cuadr3codi { get; set; }
        public decimal Cuadr3potlimite { get; set; }
        public string Cuadr3despotlimite { get; set; }
        public string Cuadr3usumodificacion { get; set; }
        public DateTime? Cuadr3fecmodificacion { get; set; }

        //Tabla temporal cuadro 3 Popup
        public int ID { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public string ModoOperOGrupoId { get; set; }
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public decimal Potencia { get; set; }
        public string ModoOperOGrupo { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int Equicodi { get; set; }

        //Unir grupo y modo operacion en un Select
        public int IdModoOpeOGrupo { get; set; }
        public string Tipo { get; set; }
        public string Valor { get; set; }

        public decimal FactorK { get; set; }
        public string Equilimpotgrupomodoper { get; set; }//PotGrupoModoOper
        //Reporte
        public int Indcuacodi { get; set; }
        public string Unidad { get; set; }
        public decimal PotenciaEfectiva { get; set; }
        public decimal PotenciaAsegurada { get; set; }
        public decimal PotenciaAsegurada2 { get; set; }
        public string PotenciaEfectivaDesc { get; set; }
        public string PotenciaAseguradaDesc { get; set; }
        public string TipoOrigen { get; set; }
        public string Cuadr3Electrico { get; set; }
    }
}
