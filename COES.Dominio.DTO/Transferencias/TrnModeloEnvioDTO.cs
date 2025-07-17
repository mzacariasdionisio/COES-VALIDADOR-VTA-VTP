using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_MODELO_ENVIO
    /// </summary>
    public class TrnModeloEnvioDTO : EntityBase
    {
        public int Modenvcodi { get; set; }
        public int Pericodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Modenvversion { get; set; }
        public string Modenvusuario { get; set; }
        public DateTime? Modenvfecenvio { get; set; }
        public string Modenvestado { get; set; }
        public string Modenvextension { get; set; }
        public string Modendfile { get; set; }
        public string Perinomb { get; set; }
        public string Versionnomb { get; set; }
        public int Trnmodcodi { get; set; }
        public string Trnmodnombre { get; set; }
    }
}
