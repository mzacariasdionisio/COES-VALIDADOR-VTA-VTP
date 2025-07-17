using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_FICTECXTIPOEQUIPO
    /// </summary>
    public class FtFictecXTipoEquipoDTO : EntityBase
    {
        public int Fteqcodi { get; set; }
        public int? Fteqpadre { get; set; }
        public string Fteqnombre { get; set; }
        public int? Famcodi { get; set; }
        public int? Catecodi { get; set; }
        public string Ftequsucreacion { get; set; }
        public string Ftequsumodificacion { get; set; }
        public DateTime? Fteqfecmodificacion { get; set; }
        public DateTime? Fteqfeccreacion { get; set; }
        public string Fteqestado { get; set; }
        public int? Fteqflagext { get; set; }
        public DateTime? Fteqfecvigenciaext { get; set; }
        public int? Fteqflagmostrarcoment { get; set; }
        public int? Fteqflagmostrarsust { get; set; }
        public int? Fteqflagmostrarfech { get; set; }
        public string Ftequsumodificacionasig { get; set; }
        public DateTime? Fteqfecmodificacionasig { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string FteqestadoDesc { get; set; }
        public string FteqfecmodificacionDesc { get; set; }
        public string FteqfecmodificacionasigDesc { get; set; }
        public string FteqfeccreacionDesc { get; set; }
        public string Famnomb { get; set; }
        public string Catenomb { get; set; }
        public int Origen { get; set; }
        public string OrigenDesc { get; set; }
        public int OrigenTipo { get; set; }
        public string OrigenTipoDesc { get; set; }
        public string Titulo { get; set; }

        public string Fteqnombrepadre { get; set; }
        public int OrigenPadre { get; set; }
        public string OrigenPadreDesc { get; set; }
        public int OrigenPadreTipo { get; set; }
        public string OrigenPadreTipoDesc { get; set; }
        public string Famnombpadre { get; set; }
        public string Catenombpadre { get; set; }
        public int? Famcodipadre { get; set; }
        public int? Catecodipadre { get; set; }

        public int Ftecprincipal { get; set; }
        public string FtecprincipalDesc { get; set; }

        #region Ficha tecnica 2023
        public string EstadoActualExtranetDesc { get; set; }
        public string EstadoActualExtranet { get; set; }
        public string FlagVisibilidadExt { get; set; }
        public string FechaVigenciaExt { get; set; }
        public bool EsVigente { get; set; }

        #endregion
    }

    public class NotificacionFT
    {
        public int Fteqcodi { get; set; }
        public string Fteqnombre { get; set; }
        public string FteqnombreNew { get; set; }
        public string Fteqestado { get; set; }
        public string FteqestadoNew { get; set; }
        public string Ftequsumodificacion { get; set; }
        public DateTime? Fteqfecmodificacion { get; set; }
    }
}
