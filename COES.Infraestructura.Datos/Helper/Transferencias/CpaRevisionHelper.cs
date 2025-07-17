using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CPA_REVISION
    /// </summary>
    public class CpaRevisionHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Cparcodi = "CPARCODI";
        public string Cpaapcodi = "CPAAPCODI";
        public string Cparrevision = "CPARREVISION";
        public string Cparcorrelativo = "CPARCORRELATIVO";
        public string Cparestado = "CPARESTADO";
        public string Cparultimo = "CPARULTIMO";
        public string Cparcmpmpo = "CPARCMPMPO";
        public string Cparusucreacion = "CPARUSUCREACION";
        public string Cparfeccreacion = "CPARFECCREACION";
        public string Cparusumodificacion = "CPARUSUMODIFICACION";
        public string Cparfecmodificacion = "CPARFECMODIFICACION";

        //Additional
        public string Cpaapanio = "CPAAPANIO";
        public string Cpaapajuste = "CPAAPAJUSTE";
        public string Cpaapanioejercicio = "CPAAPANIOEJERCICIO";
        #endregion

        public CpaRevisionHelper() : base(Consultas.CpaRevisionSql)
        {
        }

        public CpaRevisionDTO Create(IDataReader dr)
        {
            CpaRevisionDTO entity = new CpaRevisionDTO();

            int iCparcodi = dr.GetOrdinal(Cparcodi);
            if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

            int iCpaapcodi = dr.GetOrdinal(Cpaapcodi);
            if (!dr.IsDBNull(iCpaapcodi)) entity.Cpaapcodi = dr.GetInt32(iCpaapcodi);

            int iCparrevision = dr.GetOrdinal(Cparrevision);
            if (!dr.IsDBNull(iCparrevision)) entity.Cparrevision = dr.GetString(iCparrevision);

            int iCparcorrelativo = dr.GetOrdinal(Cparcorrelativo);
            if (!dr.IsDBNull(iCparcorrelativo)) entity.Cparcorrelativo = dr.GetInt32(iCparcorrelativo);

            int iCparestado = dr.GetOrdinal(Cparestado);
            if (!dr.IsDBNull(iCparestado)) entity.Cparestado = dr.GetString(iCparestado);

            int iCparultimo = dr.GetOrdinal(Cparultimo);
            if (!dr.IsDBNull(iCparultimo)) entity.Cparultimo = dr.GetString(iCparultimo);

            int iCparcmpmpo = dr.GetOrdinal(Cparcmpmpo);
            if (!dr.IsDBNull(iCparcmpmpo)) entity.Cparcmpmpo = dr.GetInt32(iCparcmpmpo);

            int iCparusucreacion = dr.GetOrdinal(Cparusucreacion);
            if (!dr.IsDBNull(iCparusucreacion)) entity.Cparusucreacion = dr.GetString(iCparusucreacion);

            int iCparfeccreacion = dr.GetOrdinal(Cparfeccreacion);
            if (!dr.IsDBNull(iCparfeccreacion)) entity.Cparfeccreacion = dr.GetDateTime(iCparfeccreacion);

            int iCparusumodificacion = dr.GetOrdinal(Cparusumodificacion);
            if (!dr.IsDBNull(iCparusumodificacion)) entity.Cparusumodificacion = dr.GetString(iCparusumodificacion);

            int iCparfecmodificacion = dr.GetOrdinal(Cparfecmodificacion);
            if (!dr.IsDBNull(iCparfecmodificacion)) entity.Cparfecmodificacion = dr.GetDateTime(iCparfecmodificacion);

            return entity;
        }

        public CpaRevisionDTO CreateById(IDataReader dr)
        {
            CpaRevisionDTO entity = Create(dr);

            int iCpaapanio = dr.GetOrdinal(Cpaapanio);
            if (!dr.IsDBNull(iCpaapanio)) entity.Cpaapanio = dr.GetInt32(iCpaapanio);

            int iCpaapajuste = dr.GetOrdinal(Cpaapajuste);
            if (!dr.IsDBNull(iCpaapajuste)) entity.Cpaapajuste = dr.GetString(iCpaapajuste);

            int iCpaapanioejercicio = dr.GetOrdinal(Cpaapanioejercicio);
            if (!dr.IsDBNull(iCpaapanioejercicio)) entity.Cpaapanioejercicio = dr.GetInt32(iCpaapanioejercicio);

            return entity;
        }

        public CpaRevisionDTO CreateByCriteria(IDataReader dr)
        {
            CpaRevisionDTO entity = CreateById(dr);
            return entity;
        }

        public string SqlUpdateUltimoByAnioByAjuste
        {
            get { return base.GetSqlXml("UpdateUltimoByAnioByAjuste"); }
        }

        public string SqlUpdateUltimoByCodi
        {
            get { return base.GetSqlXml("UpdateUltimoByCodi"); }
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateEstado"); }
        }

        public string SqlUpdateEstadoYCMgPMPO
        {
            get { return base.GetSqlXml("UpdateEstadoYCMgPMPO"); }
        }
    }
}

