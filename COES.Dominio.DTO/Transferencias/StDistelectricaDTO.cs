using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla ST_DISTELECTRICA
    /// </summary>
    public class StDistelectricaDTO : EntityBase
    {
        public int Dstelecodi { get; set; }
        public int Barrcodi { get; set; } 
        public int Strecacodi { get; set; } 
        public string Dsteleusucreacion { get; set; } 
        public DateTime Dstelefeccreacion { get; set; }
        //Atributo para consulta
        //public string Sistrnnombre { get; set; }
        public string Barrnombre { get; set; }

    }
}
