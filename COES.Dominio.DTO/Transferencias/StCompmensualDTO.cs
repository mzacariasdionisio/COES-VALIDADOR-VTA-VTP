using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_COMPMENSUAL
    /// </summary>
    public class StCompmensualDTO : EntityBase
    {
        public int Cmpmencodi { get; set; } 
        public int Strecacodi { get; set; } 
        //public int Sistrncodi { get; set; } 
        public int Stcntgcodi { get; set; } 
        public string Cmpmenusucreacion { get; set; } 
        public DateTime Cmpmenfeccreacion { get; set; }

        //variables para consultas
        public string Sistrnnombre { get; set; }
        public string Equinomb { get; set; }
    }
}
