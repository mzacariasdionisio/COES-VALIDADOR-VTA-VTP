using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_DESTINATARIOMENSAJE
    /// </summary>
    public class InDestinatariomensajeDTO : EntityBase
    {
        public int Usercode { get; set; }
        public int Emprcodi { get; set; }
        public int Indemecodi { get; set; }
        public string Indemeestado { get; set; }
        public string Indememotivobaja { get; set; }
        public string Indemeusucreacion { get; set; }
        public DateTime? Indemefeccreacion { get; set; }
        public string Indemeusumodificacion { get; set; }
        public DateTime? Indemefecmodificacion { get; set; }
        public string Indemevigente { get; set; }

        public int? Evenclasecodi { get; set; }
        public int? Indmdeacceso { get; set; }
        public string Emprnomb { get; set; }
        public string Username { get; set; }
        public string Useremail { get; set; }
        public int Indmdecodi { get; set; }

        public List<InDestinatariomensajeDetDTO> ListaDetalle { get; set; }
        public int IdAnterior { get; set; }

    }

    public class InConfiguracionNotificacion
    {
        public int Emprcodi { get; set; }
        public int Usercodi { get; set; }
        public int Codigo { get; set; }
        public string Empresa { get; set; }
        public string Usuario { get; set; }
        public string Estado { get; set; }
        public string MotivoInactivo { get; set; }
        public int? IndDiario { get; set; }
        public int? IndSemanal { get; set; }
        public int? IndMensual { get; set; }
        public int? IndAnual { get; set; }
        public int? IndEjecutado { get; set; }
        public int Rowspan { get; set; }
        public string FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string Email { get; set; }

    }
}
