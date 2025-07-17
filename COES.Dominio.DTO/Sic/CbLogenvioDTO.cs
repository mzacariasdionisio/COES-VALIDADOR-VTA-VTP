using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_LOGENVIO
    /// </summary>
    public partial class CbLogenvioDTO : EntityBase
    {
        public int Logenvcodi { get; set; }
        public int Estenvcodi { get; set; }
        public int Cbenvcodi { get; set; }
        public string Logenvusucreacion { get; set; }
        public DateTime Logenvfeccreacion { get; set; }
        public string Logenvobservacion { get; set; }
        public int? Logenvplazo { get; set; }
    }

    public partial class CbLogenvioDTO
    {
        public string LogenvfeccreacionDesc { get; set; }
        public string Estenvnomb { get; set; }

        #region region CCGAS.PR31
        public string Logenvusurecepcion { get; set; }
        public string Logenvusulectura { get; set; }
        public DateTime? Logenvfecrecepcion { get; set; }
        public DateTime? Logenvfeclectura { get; set; }
        public string LogenvfecrecepcionDesc { get; set; }
        public string LogenvfeclecturaDesc { get; set; }
        public string Emprnomb { get; set; }
        #endregion
    }
}
