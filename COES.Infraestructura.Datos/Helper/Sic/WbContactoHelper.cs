using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_CONTACTO
    /// </summary>
    public class WbContactoHelper : HelperBase
    {
        public WbContactoHelper(): base(Consultas.WbContactoSql)
        {
        }

        public WbContactoDTO Create(IDataReader dr)
        {
            WbContactoDTO entity = new WbContactoDTO();

            int iContaccodi = dr.GetOrdinal(this.Contaccodi);
            if (!dr.IsDBNull(iContaccodi)) entity.Contaccodi = Convert.ToInt32(dr.GetValue(iContaccodi));

            int iContacnombre = dr.GetOrdinal(this.Contacnombre);
            if (!dr.IsDBNull(iContacnombre)) entity.Contacnombre = dr.GetString(iContacnombre);

            int iContacapellido = dr.GetOrdinal(this.Contacapellido);
            if (!dr.IsDBNull(iContacapellido)) entity.Contacapellido = dr.GetString(iContacapellido);

            int iContacemail = dr.GetOrdinal(this.Contacemail);
            if (!dr.IsDBNull(iContacemail)) entity.Contacemail = dr.GetString(iContacemail);

            int iContaccargo = dr.GetOrdinal(this.Contaccargo);
            if (!dr.IsDBNull(iContaccargo)) entity.Contaccargo = dr.GetString(iContaccargo);

            int iContacempresa = dr.GetOrdinal(this.Contacempresa);
            if (!dr.IsDBNull(iContacempresa)) entity.Contacempresa = dr.GetString(iContacempresa);

            int iContactelefono = dr.GetOrdinal(this.Contactelefono);
            if (!dr.IsDBNull(iContactelefono)) entity.Contactelefono = dr.GetString(iContactelefono);

            int iContacmovil = dr.GetOrdinal(this.Contacmovil);
            if (!dr.IsDBNull(iContacmovil)) entity.Contacmovil = dr.GetString(iContacmovil);

            int iContaccomentario = dr.GetOrdinal(this.Contaccomentario);
            if (!dr.IsDBNull(iContaccomentario)) entity.Contaccomentario = dr.GetString(iContaccomentario);

            int iContacarea = dr.GetOrdinal(this.Contacarea);
            if (!dr.IsDBNull(iContacarea)) entity.Contacarea = dr.GetString(iContacarea);

            int iContacestado = dr.GetOrdinal(this.Contacestado);
            if (!dr.IsDBNull(iContacestado)) entity.Contacestado = dr.GetString(iContacestado);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Contaccodi = "CONTACCODI";
        public string Contacnombre = "CONTACNOMBRE";
        public string Contacapellido = "CONTACAPELLIDO";
        public string Contacemail = "CONTACEMAIL";
        public string Contaccargo = "CONTACCARGO";
        public string Contacempresa = "CONTACEMPRESA";
        public string Contactelefono = "CONTACTELEFONO";
        public string Contacmovil = "CONTACMOVIL";
        public string Contaccomentario = "CONTACCOMENTARIO";
        public string Contacarea = "CONTACAREA";
        public string Contacestado = "CONTACESTADO";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Emprdire = "EMPRDIRE";
        public string ContacDoc = "CONTACDOC";
        public string ContacFecRegistro = "CONTACFECREGISTRO";
        public string Fuente = "FUENTE";
        public string Userreplegal = "USERREPLEGAL";
        public string Usercontacto = "USERCONTACTO";
        public string Tipoemprnomb = "TIPOEMPRDESC";
        public string Emprcoes = "EMPRCOES";
        public string Emprnombcomercial = "EMPRNOMBRECOMERCIAL";

        public string SqlObtenerEmpresasContacto
        {
            get { return base.GetSqlXml("ObtenerEmpresasContacto"); }
        }       

        #endregion
    }
}
