using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_AREA
    /// </summary>
    public class EqAreaDTO : EntityBase
    {
        public int Areacodi { get; set; }
        public int? Tareacodi { get; set; } 
        public string Areaabrev { get; set; } 
        public string Areanomb { get; set; }
        public int Areapadre { get; set; }
        public string Areaestado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioUpdate { get; set; }
        public DateTime FechaUpdate { get; set; }
        public int ANivelCodi { get; set; }         
        public string Tareanomb { get; set; }
        public string EstadoDesc { get; set; }

        #region PR5
        public int Orden { get; set; }
        public string Subestacion { get; set; }
        public int Emprcodi { get; set; }
        #endregion

        #region Zonas
        public string ANivelNomb { get; set; }
        #endregion

        //Assetec - PRODEM2 - 20200520
        public string Tareaabrev { get; set; }

        //GESPROTEC - 20241029
        #region GESPROTEC
        public string Zona { get; set; }
        public string Areanombenprotec { get; set; }
        public string Flagenprotec { get; set; }
        public int Epareacodi { get; set; }
        
        #endregion
    }
}
