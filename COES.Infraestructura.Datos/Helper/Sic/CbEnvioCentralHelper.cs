using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_ENVIO_CENTRAL
    /// </summary>
    public class CbEnvioCentralHelper : HelperBase
    {
        public CbEnvioCentralHelper() : base(Consultas.CbEnvioCentralSql)
        {
        }

        public CbEnvioCentralDTO Create(IDataReader dr)
        {
            CbEnvioCentralDTO entity = new CbEnvioCentralDTO();

            int iCbcentcodi = dr.GetOrdinal(this.Cbcentcodi);
            if (!dr.IsDBNull(iCbcentcodi)) entity.Cbcentcodi = Convert.ToInt32(dr.GetValue(iCbcentcodi));

            int iCbcentestado = dr.GetOrdinal(this.Cbcentestado);
            if (!dr.IsDBNull(iCbcentestado)) entity.Cbcentestado = dr.GetString(iCbcentestado);

            int iCbvercodi = dr.GetOrdinal(this.Cbvercodi);
            if (!dr.IsDBNull(iCbvercodi)) entity.Cbvercodi = Convert.ToInt32(dr.GetValue(iCbvercodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Cbcentcodi = "CBCENTCODI";
        public string Cbcentestado = "CBCENTESTADO";
        public string Cbvercodi = "CBVERCODI";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Grupocodi = "GRUPOCODI";
        public string Fenergcodi = "FENERGCODI";
        public string Fenergnomb = "FENERGNOMB";

        public string CbenvfechaPeriodo = "CBENVFECHAPERIODO";
        public string Cbenvfecsolicitud = "CBENVFECSOLICITUD";

        #endregion

        public string SqlGetCentralesPorEnvio
        {
            get { return base.GetSqlXml("GetCentralesPorEnvio"); }
        }

        public string SqlGetCentralesConInfoEnviada
        {
            get { return base.GetSqlXml("GetCentralesConInfoEnviada"); }
        }

        public string SqlListarCentralUltimoEnvioXMes
        {
            get { return base.GetSqlXml("ListarCentralUltimoEnvioXMes"); }
        }

        public string SqlListarCentralUltimoEnvioXDato
        {
            get { return base.GetSqlXml("ListarCentralUltimoEnvioXDato"); }
        }

        public string SqlListarCentralNuevaUltimoEnvioXDato
        {
            get { return base.GetSqlXml("ListarCentralNuevaUltimoEnvioXDato"); }
        }

        public string SqlGetByEstadoYVersion
        {
            get { return base.GetSqlXml("GetByEstadoYVersion"); }
        }


        public string SqlListarCentralUltimoEnvioXDia
        {
            get { return base.GetSqlXml("ListarCentralUltimoEnvioXDia"); }
        }

        public string SqlListarCentralPorRangoMes
        {
            get { return base.GetSqlXml("ListarCentralPorRangoMes"); }
        }

        public string SqlListarPorIds
        {
            get { return base.GetSqlXml("ListarPorIds"); }
        }
    }
}
