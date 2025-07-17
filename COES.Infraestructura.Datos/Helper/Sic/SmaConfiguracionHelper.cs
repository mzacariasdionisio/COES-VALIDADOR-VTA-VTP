using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_CONFIGURACION
    /// </summary>
    public class SmaConfiguracionHelper : HelperBase
    {
        public SmaConfiguracionHelper(): base(Consultas.SmaConfiguracionSql)
        {
        }

        public SmaConfiguracionDTO Create(IDataReader dr)
        {
            SmaConfiguracionDTO entity = new SmaConfiguracionDTO();

            int iConfsmcorrelativo = dr.GetOrdinal(this.Confsmcorrelativo);
            if (!dr.IsDBNull(iConfsmcorrelativo)) entity.Confsmcorrelativo = Convert.ToInt32(dr.GetValue(iConfsmcorrelativo));

            int iConfsmparametro = dr.GetOrdinal(this.Confsmparametro);
            if (!dr.IsDBNull(iConfsmparametro)) entity.Confsmparametro = dr.GetString(iConfsmparametro);

            int iConfsmvalor = dr.GetOrdinal(this.Confsmvalor);
            if (!dr.IsDBNull(iConfsmvalor)) entity.Confsmvalor = dr.GetString(iConfsmvalor);

            int iConfsmatributo = dr.GetOrdinal(this.Confsmatributo);
            if (!dr.IsDBNull(iConfsmatributo)) entity.Confsmatributo = dr.GetString(iConfsmatributo);

            int iConfsmusucreacion = dr.GetOrdinal(this.Confsmusucreacion);
            if (!dr.IsDBNull(iConfsmusucreacion)) entity.Confsmusucreacion = dr.GetString(iConfsmusucreacion);

            int iConfsmfeccreacion = dr.GetOrdinal(this.Confsmfeccreacion);
            if (!dr.IsDBNull(iConfsmfeccreacion)) entity.Confsmfeccreacion = dr.GetDateTime(iConfsmfeccreacion);

            int iConfsmusumodificacion = dr.GetOrdinal(this.Confsmusumodificacion);
            if (!dr.IsDBNull(iConfsmusumodificacion)) entity.Confsmusumodificacion = dr.GetString(iConfsmusumodificacion);

            int iConfsmfecmodificacion = dr.GetOrdinal(this.Confsmfecmodificacion);
            if (!dr.IsDBNull(iConfsmfecmodificacion)) entity.Confsmfecmodificacion = dr.GetDateTime(iConfsmfecmodificacion);

            int iConfsmestado = dr.GetOrdinal(this.Confsmestado);
            if (!dr.IsDBNull(iConfsmestado)) entity.Confsmestado = dr.GetString(iConfsmestado);

            return entity;
        }

        public SmaConfiguracionDTO CreateAtributos(IDataReader dr)
        {
            SmaConfiguracionDTO entity = new SmaConfiguracionDTO();

            int iConfsmparametro = dr.GetOrdinal(this.Confsmparametro);
            if (!dr.IsDBNull(iConfsmparametro)) entity.Confsmparametro = dr.GetString(iConfsmparametro);

            int iConfsmestado = dr.GetOrdinal(this.Confsmestado);
            if (!dr.IsDBNull(iConfsmestado)) entity.Confsmestado = dr.GetString(iConfsmestado);

            return entity;
        }

        #region Mapeo de Campos

        public string Confsmcorrelativo = "CONFSMCORRELATIVO";
        public string Confsmatributo = "CONFSMATRIBUTO";
        public string Confsmparametro = "CONFSMPARAMETRO";
        public string Confsmvalor = "CONFSMVALOR";
        public string Confsmusucreacion = "CONFSMUSUCREACION";
        public string Confsmfeccreacion = "CONFSMFECCREACION";
        public string Confsmusumodificacion = "CONFSMUSUMODIFICACION";
        public string Confsmfecmodificacion = "CONFSMFECMODIFICACION";
        public string Confsmestado = "CONFSMESTADO";

        #endregion

        public string SqlGetValorxID
        {
            get { return base.GetSqlXml("GetValorxID"); }
        }

        public string SqlGetValor
        {
            get { return base.GetSqlXml("GetValor"); }
        }
        public string SqlGetByAtributo
        {
            get { return base.GetSqlXml("GetByAtributo"); }
        }

    }
}
