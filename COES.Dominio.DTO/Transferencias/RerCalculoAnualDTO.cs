using System;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RER_CALCULO_ANUAL
    /// </summary>
    public class RerCalculoAnualDTO : EntityBase
    {
        public int Rercacodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public int Reravcodi { get; set; }
        public decimal Rercaippi { get; set; }
        public decimal Rercaippo { get; set; }
        public decimal Rercataradjbase { get; set; }
        public decimal Rercafaccorreccion { get; set; }
        public decimal Rercafacactanterior { get; set; }
        public decimal Rercafacactualizacion { get; set; }
        public decimal Rercataradj { get; set; }
        public string Rercacomment { get; set; }
        public string Rercausucreacion { get; set; }
        public DateTime Rercafeccreacion { get; set; }

        //Additional
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public int Reravaniotarif { get; set; }
        public string Reravversion { get; set; }
    }
}

