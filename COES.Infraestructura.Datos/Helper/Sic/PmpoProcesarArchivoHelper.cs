using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo a la tabla Me_MedicionxIntervalo
    /// </summary>
    public class PmpoProcesarArchivoHelper : HelperBase
    {
        public PmpoProcesarArchivoHelper()
            : base(Consultas.PmpoProcesoArchivoSql)
        {
        }

        public PmpoConfiguracionDTO Create(IDataReader dr)
        {
            PmpoConfiguracionDTO entity = new PmpoConfiguracionDTO();

            int iConfiguracioncodi = dr.GetOrdinal(this.Confpmcodi);
            if (!dr.IsDBNull(iConfiguracioncodi)) entity.Confpmcodi = Convert.ToInt32(dr.GetValue(iConfiguracioncodi));

            int iConfpmatributo = dr.GetOrdinal(this.Confpmatributo);
            if (!dr.IsDBNull(iConfpmatributo)) entity.Confpmatributo = dr.GetString(iConfpmatributo);

            int iConfpmparametro = dr.GetOrdinal(this.Confpmparametro);
            if (!dr.IsDBNull(iConfpmparametro)) entity.Confpmparametro = dr.GetString(iConfpmparametro);

            int iConfpmvalor = dr.GetOrdinal(this.Confpmvalor);
            if (!dr.IsDBNull(iConfpmvalor)) entity.Confpmvalor = dr.GetString(iConfpmvalor);

            int iConfpmestado = dr.GetOrdinal(this.Confpmestado);
            if (!dr.IsDBNull(iConfpmestado)) entity.Confpmestado = dr.GetString(iConfpmestado);


            int iConfpmusucreacion = dr.GetOrdinal(this.Confpmusucreacion);
            if (!dr.IsDBNull(iConfpmusucreacion)) entity.Confpmusucreacion = dr.GetString(iConfpmusucreacion);

            int iConfpmfeccreacion = dr.GetOrdinal(this.Confpmfeccreacion);
            if (!dr.IsDBNull(iConfpmfeccreacion)) entity.Confpmfeccreacion = dr.GetDateTime(iConfpmfeccreacion);

            int iConfpmusumodificacion = dr.GetOrdinal(this.Confpmusumodificacion);
            if (!dr.IsDBNull(iConfpmusumodificacion)) entity.Confpmusumodificacion = dr.GetString(iConfpmusumodificacion);

            int iConfpmfecmodificacion = dr.GetOrdinal(this.Confpmfecmodificacion);
            if (!dr.IsDBNull(iConfpmfecmodificacion)) entity.Confpmfecmodificacion = dr.GetDateTime(iConfpmfecmodificacion);


            return entity;
        }

        #region Mapeo de Campos

        public string Confpmcodi = "CONFPMCODI";
        public string Confpmatributo = "CONFPMATRIBUTO";
        public string Confpmparametro = "CONFPMPARAMETRO";
        public string Confpmvalor = "CONFPMVALOR";
        public string Confpmestado = "CONFPMESTADO";
        public string Confpmusucreacion = "CONFPMUSUCREACION";
        public string Confpmfeccreacion = "CONFPMFECCREACION";
        public string Confpmusumodificacion = "CONFPMUSUMODIFICACION";
        public string Confpmfecmodificacion = "CONFPMFECMODIFICACION";

        #endregion

        public string SqlListAtributo
        {
            get { return base.GetSqlXml("ListAtributo"); }
        }
        public string SqlListValor
        {
            get { return base.GetSqlXml("ListValor"); }
        }

        public string SqlListValorActivo
        {
            get { return base.GetSqlXml("ListValorActivo"); }
        }
        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }
        public string SqlGetByParmanetroFech
        {
            get { return base.GetSqlXml("GetByParmanetroFech"); }
        }
    }
}
