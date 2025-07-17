using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{

    public partial class SiMensajeDTO : EntityBase
    {
        public int? Bandcodi { get; set; }
        public int Emprcodi { get; set; }
        public int? Estmsgcodi { get; set; }
        public int? Fdatcodi { get; set; }
        public int Formatcodi { get; set; }
        public int Modcodi { get; set; }
        public int? Tmsgcodi { get; set; }

        public int Msgcodi { get; set; }
        public DateTime? Msgfecha { get; set; }
        public string Msgcontenido { get; set; }
        public DateTime? Msgfechaperiodo { get; set; }
        public string Msgto { get; set; }
        public string Msgcc { get; set; }
        public string Msgbcc { get; set; }
        public string Msgfrom { get; set; }
        public string Msgfromname { get; set; }
        public string Msgasunto { get; set; }
        public int Msgflagadj { get; set; }
        public string Msgestado { get; set; }
        public int? Msgtipo { get; set; }

        public DateTime Msgfeccreacion { get; set; }
        public string Msgusucreacion { get; set; }
        public DateTime? Msgfecmodificacion { get; set; }
        public string Msgusumodificacion { get; set; }
    }

    public partial class SiMensajeDTO
    {
        #region SIOSEIN
        public int Carcodi { get; set; }
        #endregion

        public string Msgarchivos { get; set; }
        public string Emprnomb { get; set; }

        public int Intercodi { get; set; }
        public int Progrcodi { get; set; }

        public string Remitente { get; set; }
        public string EmpresaRemitente { get; set; }
        public bool EsRemitenteAgente { get; set; }
        public bool EsLeido { get; set; }
        public string MsgestadoDesc { get; set; }

        public string MsgfeccreacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public bool EsVisibleLectura { get; set; }
        public string UsuarioLectura { get; set; }
        public string FechaDescLectura { get; set; }
        public int? Intercodivigente { get; set; }
        public string Programacion { get; set; }

        public List<InArchivoDTO> ListaArchivo { get; set; }
        public string Msglectura { get; set; }

    }
}
