using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_ANIOVERSION
    /// </summary>
    public class RerAnioVersionDTO : EntityBase
    {
        public int Reravcodi { get; set; }
        public string Reravversion { get; set; }
        public int Reravaniotarif { get; set; }
        public string Reravaniotarifdesc { get; set; }
        public decimal Reravinflacion { get; set; }
        public string Reravestado { get; set; }
        public string Reravusucreacion { get; set; }
        public DateTime Reravfeccreacion { get; set; }
        public string Reravusumodificacion { get; set; }
        public DateTime Reravfecmodificacion { get; set; }


        //Aditional
        public string Reravversiondesc { get; set; }
    }
}