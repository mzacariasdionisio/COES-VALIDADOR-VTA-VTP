using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DPO_CASO
    /// </summary>
    [Serializable]
    public class DpoCasoDTO : EntityBase
    {
        public int Dpocsocodi { get; set; }
        public string Dpocsocnombre { get; set; }
        public string Areaabrev { get; set; }
        public string Dpocsousucreacion { get; set; }
        public DateTime Dpocsofeccreacion { get; set; }
        public string Dpocsousumodificacion { get; set; }
        public DateTime Dposcofecmodificacion { get; set; }

        // Adicionales
        public string StrDpocsofeccreacion { get; set; }
        public string StrDpocsousumodificacion { get; set; }
    }

    #region Filtros
    public class DpoNombreCasoDTO : EntityBase
    {
        public string Nombre { get; set; }
    }

    public class DpoUsuarioDTO : EntityBase
    {
        public string Usuario { get; set; }
    }
    #endregion
}
