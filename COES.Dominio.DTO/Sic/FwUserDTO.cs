using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FW_USER
    /// </summary>
    public class FwUserDTO : EntityBase
    {
        public int Usercode { get; set; }
        public int? Areacode { get; set; }
        public string Username { get; set; }
        public string Userpass { get; set; }
        public int? Userconn { get; set; }
        public int? Usermaxconn { get; set; }
        public string Userlogin { get; set; }
        public int? Uservalidate { get; set; }
        public int? Usercheck { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Userstate { get; set; }
        public string Empresas { get; set; }
        public DateTime? Userfcreacion { get; set; }
        public DateTime? Userfactivacion { get; set; }
        public DateTime? Userfbaja { get; set; }
        public string Usertlf { get; set; }
        public string Motivocontacto { get; set; }
        public string Usercargo { get; set; }
        public string Arealaboral { get; set; }
        public string Useremail { get; set; }
        public int? Emprcodi { get; set; }
        public string Usersolicitud { get; set; }
        public string Userindreprleg { get; set; }
        public string Userucreacion { get; set; }
        public string Userad { get; set; }
        public string Usermovil { get; set; }
        public int? Userflagpermiso { get; set; }
        public string Userdoc { get; set; }
        public DateTime? Userfecregistro { get; set; }
    }
}
