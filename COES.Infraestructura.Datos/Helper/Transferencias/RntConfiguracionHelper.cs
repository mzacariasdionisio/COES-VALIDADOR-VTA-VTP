using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RNT_CONFIGURACION
    /// </summary>
    public class RntConfiguracionHelper : HelperBase
    {
        public RntConfiguracionHelper()
            : base(Consultas.RntConfiguracionSql)
        {

        }

        public RntConfiguracionDTO Create(IDataReader dr)
        {
            RntConfiguracionDTO entity = new RntConfiguracionDTO();

            int iConfatributo = dr.GetOrdinal(this.Confatributo);
            if (!dr.IsDBNull(iConfatributo)) entity.ConfAtributo = dr.GetString(iConfatributo);

            int iConfparametro = dr.GetOrdinal(this.Confparametro);
            if (!dr.IsDBNull(iConfparametro)) entity.ConfParametro = dr.GetString(iConfparametro);

            int iConfvalor = dr.GetOrdinal(this.Confvalor);
            if (!dr.IsDBNull(iConfvalor)) entity.ConfValor = dr.GetString(iConfvalor);

            int iConfestado = dr.GetOrdinal(this.Confestado);
            if (!dr.IsDBNull(iConfestado)) entity.ConfEstado = Convert.ToInt32(dr.GetValue(iConfestado));

            int iConfigcodi = dr.GetOrdinal(this.Configcodi);
            if (!dr.IsDBNull(iConfigcodi)) entity.ConfigCodi = Convert.ToInt32(dr.GetValue(iConfigcodi));

            int iConfusuariocreacion = dr.GetOrdinal(this.Confusuariocreacion);
            if (!dr.IsDBNull(iConfusuariocreacion)) entity.ConfUsuarioCreacion = dr.GetString(iConfusuariocreacion);

            int iConffechacreacion = dr.GetOrdinal(this.Conffechacreacion);
            if (!dr.IsDBNull(iConffechacreacion)) entity.ConfFechaCreacion = dr.GetDateTime(iConffechacreacion);

            int iConfusuarioupdate = dr.GetOrdinal(this.Confusuarioupdate);
            if (!dr.IsDBNull(iConfusuarioupdate)) entity.ConfUsuarioUpdate = dr.GetString(iConfusuarioupdate);

            int iConffechaupdate = dr.GetOrdinal(this.Conffechaupdate);
            if (!dr.IsDBNull(iConffechaupdate)) entity.ConfFechaUpdate = dr.GetDateTime(iConffechaupdate);

            return entity;
        }

        public RntConfiguracionDTO ListCombo(IDataReader dr)
        {
            RntConfiguracionDTO entity = new RntConfiguracionDTO();

            int iConfparametro = dr.GetOrdinal(this.Confparametro);
            if (!dr.IsDBNull(iConfparametro)) entity.ConfParametro = dr.GetString(iConfparametro);

            return entity;
        }


        #region Mapeo de Campos

        public string Confatributo = "CONFATRIBUTO";
        public string Confparametro = "CONFPARAMETRO";
        public string Confvalor = "CONFVALOR";
        public string Confestado = "CONFESTADO";
        public string Configcodi = "CONFCODI";
        public string Confusuariocreacion = "CONFUSUARIOCREACION";
        public string Conffechacreacion = "CONFFECHACREACION";
        public string Confusuarioupdate = "CONFUSUARIOUPDATE";
        public string Conffechaupdate = "CONFFECHAUPDATE";

        #endregion

        public string SqlListNivelTension
        {
            get { return base.GetSqlXml("ListNivelTension"); }
        }
        public string SqlListComboNivelTension
        {
            get { return base.GetSqlXml("ListComboNivelTension"); }
        }
        public string SqlListComboConfiguracion
        {
            get { return base.GetSqlXml("ListComboConfiguracion"); }
        }
        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListParametroRep
        {
            get { return base.GetSqlXml("ListParametroRep"); }
        }

        public string SqlGetListParametroRep
        {
            get { return base.GetSqlXml("GetListParametroRep"); }
        }

        public string SqlGetComboParametro
        {
            get { return base.GetSqlXml("GetComboParametro"); }
        }

    }
}
