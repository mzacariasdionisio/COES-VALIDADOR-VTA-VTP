using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_SDDP_GENMARG
    /// </summary>
    public class CaiSddpGenmargHelper : HelperBase
    {
        public CaiSddpGenmargHelper(): base(Consultas.CaiSddpGenmargSql)
        {
        }

        public CaiSddpGenmargDTO Create(IDataReader dr)
        {
            CaiSddpGenmargDTO entity = new CaiSddpGenmargDTO();

            int iSddpgmcodi = dr.GetOrdinal(this.Sddpgmcodi);
            if (!dr.IsDBNull(iSddpgmcodi)) entity.Sddpgmcodi = Convert.ToInt32(dr.GetValue(iSddpgmcodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iSddpgmtipo = dr.GetOrdinal(this.Sddpgmtipo);
            if (!dr.IsDBNull(iSddpgmtipo)) entity.Sddpgmtipo = dr.GetString(iSddpgmtipo);

            int iSddpgmetapa = dr.GetOrdinal(this.Sddpgmetapa);
            if (!dr.IsDBNull(iSddpgmetapa)) entity.Sddpgmetapa = Convert.ToInt32(dr.GetValue(iSddpgmetapa));

            int iSddpgmserie = dr.GetOrdinal(this.Sddpgmserie);
            if (!dr.IsDBNull(iSddpgmserie)) entity.Sddpgmserie = Convert.ToInt32(dr.GetValue(iSddpgmserie));

            int iSddpgmbloque = dr.GetOrdinal(this.Sddpgmbloque);
            if (!dr.IsDBNull(iSddpgmbloque)) entity.Sddpgmbloque = Convert.ToInt32(dr.GetValue(iSddpgmbloque));

            int iSddpgmnombre = dr.GetOrdinal(this.Sddpgmnombre);
            if (!dr.IsDBNull(iSddpgmnombre)) entity.Sddpgmnombre = dr.GetString(iSddpgmnombre);
            
            int iSddpgmenergia = dr.GetOrdinal(this.Sddpgmenergia);
            if (!dr.IsDBNull(iSddpgmenergia)) entity.Sddpgmenergia = Convert.ToDecimal(dr.GetValue(iSddpgmenergia));

            int iSddpgmfecha = dr.GetOrdinal(this.Sddpgmfecha);
            if (!dr.IsDBNull(iSddpgmfecha)) entity.Sddpgmfecha = dr.GetDateTime(iSddpgmfecha);

            int iSddpgmusucreacion = dr.GetOrdinal(this.Sddpgmusucreacion);
            if (!dr.IsDBNull(iSddpgmusucreacion)) entity.Sddpgmusucreacion = dr.GetString(iSddpgmusucreacion);

            int iSddpgmfeccreacion = dr.GetOrdinal(this.Sddpgmfeccreacion);
            if (!dr.IsDBNull(iSddpgmfeccreacion)) entity.Sddpgmfeccreacion = dr.GetDateTime(iSddpgmfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Sddpgmcodi = "SDDPGMCODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Sddpgmtipo = "SDDPGMTIPO";
        public string Sddpgmetapa = "SDDPGMETAPA";
        public string Sddpgmserie = "SDDPGMSERIE";
        public string Sddpgmbloque = "SDDPGMBLOQUE";
        public string Sddpgmnombre = "SDDPGMNOMBRE";
        public string Sddpgmenergia = "SDDPGMENERGIA";        
        public string Sddpgmfecha = "SDDPGMFECHA";
        public string Sddpgmusucreacion = "SDDPGMUSUCREACION";
        public string Sddpgmfeccreacion = "SDDPGMFECCREACION";

        public string TableName = "CAI_SDDP_GENMARG";
        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string GetSumSddpGenmargByEtapaB1
        {
            get { return base.GetSqlXml("GetSumSddpGenmargByEtapaB1"); }
        }

        public string GetSumSddpGenmargByEtapaB2
        {
            get { return base.GetSqlXml("GetSumSddpGenmargByEtapaB2"); }
        }

        public string GetSumSddpGenmargByEtapaB3
        {
            get { return base.GetSqlXml("GetSumSddpGenmargByEtapaB3"); }
        }

        public string GetSumSddpGenmargByEtapaB4
        {
            get { return base.GetSqlXml("GetSumSddpGenmargByEtapaB4"); }
        }

        public string GetSumSddpGenmargByEtapaB5
        {
            get { return base.GetSqlXml("GetSumSddpGenmargByEtapaB5"); }
        }

        public string GetByCriteriaCaiSddpGenmargsBarrNoIns
        {
            get { return base.GetSqlXml("GetByCriteriaCaiSddpGenmargsBarrNoIns"); }
        }

        public string GetSumSddpGenmargByEtapa
        {
            get { return base.GetSqlXml("GetSumSddpGenmargByEtapa"); }
        }
    }
}
