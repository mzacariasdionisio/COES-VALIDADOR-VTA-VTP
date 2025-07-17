using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_USUARIO_URS
    /// </summary>
    public partial class SmaUsuarioUrsDTO : EntityBase
    {
        public int Uurscodi { get; set; }
        public int? Urscodi { get; set; }
        public string Uursusucreacion { get; set; }
        public string Uursusumodificacion { get; set; }
        public DateTime? Uursfecmodificacion { get; set; }
        public int? Usercode { get; set; }
        public string Uursestado { get; set; }
        public DateTime? Uursfeccreacion { get; set; }
    }

    public partial class SmaUsuarioUrsDTO
    {
        // JOIN
        public string Ursnomb { get; set; }
        public string Urstipo { get; set; }
        public int Grupocodi { get; set; }
        public string Grupotipo { get; set; }
        public string Gruponom { get; set; }
        public string Username { get; set; }
        public string Useremail { get; set; }
        public string Userlogin { get; set; }

    }
}
