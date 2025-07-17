using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_QN_ENVIO
    /// </summary>
    public class PmoQnEnvioHelper : HelperBase
    {
        public PmoQnEnvioHelper() : base(Consultas.PmoQnEnvioSql)
        {
        }

        public PmoQnEnvioDTO Create(IDataReader dr)
        {
            PmoQnEnvioDTO entity = new PmoQnEnvioDTO();

            int iQnbenvcodi = dr.GetOrdinal(this.Qnbenvcodi);
            if (!dr.IsDBNull(iQnbenvcodi)) entity.Qnbenvcodi = Convert.ToInt32(dr.GetValue(iQnbenvcodi));

            int iQnbenvanho = dr.GetOrdinal(this.Qnbenvanho);
            if (!dr.IsDBNull(iQnbenvanho)) entity.Qnbenvanho = Convert.ToInt32(dr.GetValue(iQnbenvanho));

            int iQnbenvnomb = dr.GetOrdinal(this.Qnbenvnomb);
            if (!dr.IsDBNull(iQnbenvnomb)) entity.Qnbenvnomb = dr.GetString(iQnbenvnomb);

            int iQnbenvestado = dr.GetOrdinal(this.Qnbenvestado);
            if (!dr.IsDBNull(iQnbenvestado)) entity.Qnbenvestado = Convert.ToInt32(dr.GetValue(iQnbenvestado));

            int iQnbenvversion = dr.GetOrdinal(this.Qnbenvversion);
            if (!dr.IsDBNull(iQnbenvversion)) entity.Qnbenvversion = Convert.ToInt32(dr.GetValue(iQnbenvversion));

            int iQnbenvfechaperiodo = dr.GetOrdinal(this.Qnbenvfechaperiodo);
            if (!dr.IsDBNull(iQnbenvfechaperiodo)) entity.Qnbenvfechaperiodo = dr.GetDateTime(iQnbenvfechaperiodo);

            int iQnbenvusucreacion = dr.GetOrdinal(this.Qnbenvusucreacion);
            if (!dr.IsDBNull(iQnbenvusucreacion)) entity.Qnbenvusucreacion = dr.GetString(iQnbenvusucreacion);

            int iQnbenvfeccreacion = dr.GetOrdinal(this.Qnbenvfeccreacion);
            if (!dr.IsDBNull(iQnbenvfeccreacion)) entity.Qnbenvfeccreacion = dr.GetDateTime(iQnbenvfeccreacion);

            int iQnbenvusumodificacion = dr.GetOrdinal(this.Qnbenvusumodificacion);
            if (!dr.IsDBNull(iQnbenvusumodificacion)) entity.Qnbenvusumodificacion = dr.GetString(iQnbenvusumodificacion);

            int iQnbenvfecmodificacion = dr.GetOrdinal(this.Qnbenvfecmodificacion);
            if (!dr.IsDBNull(iQnbenvfecmodificacion)) entity.Qnbenvfecmodificacion = dr.GetDateTime(iQnbenvfecmodificacion);

            int iQnlectcodi = dr.GetOrdinal(this.Qnlectcodi);
            if (!dr.IsDBNull(iQnlectcodi)) entity.Qnlectcodi = Convert.ToInt32(dr.GetValue(iQnlectcodi));

            int iQncfgecodi = dr.GetOrdinal(this.Qncfgecodi);
            if (!dr.IsDBNull(iQncfgecodi)) entity.Qncfgecodi = Convert.ToInt32(dr.GetValue(iQncfgecodi));

            int iQnbenvidentificador = dr.GetOrdinal(this.Qnbenvidentificador);
            if (!dr.IsDBNull(iQnbenvidentificador)) entity.Qnbenvidentificador = Convert.ToInt32(dr.GetValue(iQnbenvidentificador));

            int iQnbenvdeleted = dr.GetOrdinal(this.Qnbenvdeleted);
            if (!dr.IsDBNull(iQnbenvdeleted)) entity.Qnbenvdeleted = Convert.ToInt32(dr.GetValue(iQnbenvdeleted));

            int iQnbenvbase = dr.GetOrdinal(this.Qnbenvbase);
            if (!dr.IsDBNull(iQnbenvbase)) entity.Qnbenvbase = Convert.ToInt32(dr.GetValue(iQnbenvbase));

            return entity;
        }


        #region Mapeo de Campos

        public string Qnbenvcodi = "QNBENVCODI";
        public string Qnbenvanho = "QNBENVANHO";
        public string Qnbenvnomb = "QNBENVNOMB";
        public string Qnbenvestado = "QNBENVESTADO";
        public string Qnbenvversion = "QNBENVVERSION";
        public string Qnbenvfechaperiodo = "QNBENVFECHAPERIODO";
        public string Qnbenvusucreacion = "QNBENVUSUCREACION";
        public string Qnbenvfeccreacion = "QNBENVFECCREACION";
        public string Qnbenvusumodificacion = "QNBENVUSUMODIFICACION";
        public string Qnbenvfecmodificacion = "QNBENVFECMODIFICACION";
        public string Qnlectcodi = "QNLECTCODI";
        public string Qncfgecodi = "QNCFGECODI";
        public string Qnbenvidentificador = "QNBENVIDENTIFICADOR";
        public string Qnbenvdeleted = "QNBENVDELETED";
        public string Qnbenvbase = "QNBENVBASE";

        public string Qnlectnomb = "QNLECTNOMB";

        #endregion

        public string SqlUpdateEstadoBaja
        {
            get { return GetSqlXml("UpdateEstadoBaja"); }
        }
        public string SqlUpdateAprobar
        {
            get { return base.GetSqlXml("UpdateAprobar"); }
        }
        public string SqlUpdateOficial
        {
            get { return base.GetSqlXml("UpdateOficial"); }
        }
    }
}
