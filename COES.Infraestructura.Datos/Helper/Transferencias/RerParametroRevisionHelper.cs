using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_PARAMETRO_REVISION
    /// </summary>
    public class RerParametroRevisionHelper : HelperBase
    {
        public RerParametroRevisionHelper() : base(Consultas.RerParametroRevisionSql)
        {
        }

        public RerParametroRevisionDTO Create(IDataReader dr)
        {
            RerParametroRevisionDTO entity = new RerParametroRevisionDTO();

            int iRerprecodi = dr.GetOrdinal(this.Rerprecodi);
            if (!dr.IsDBNull(iRerprecodi)) entity.Rerprecodi = Convert.ToInt32(dr.GetValue(iRerprecodi));

            int iRerpprcodi = dr.GetOrdinal(this.Rerpprcodi);
            if (!dr.IsDBNull(iRerpprcodi)) entity.Rerpprcodi = Convert.ToInt32(dr.GetValue(iRerpprcodi));

            int iPerinombre = dr.GetOrdinal(this.Perinombre);
            if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

            int iRecanombre = dr.GetOrdinal(this.Recanombre);
            if (!dr.IsDBNull(iRecanombre)) entity.Recanombre = dr.GetString(iRecanombre);

            int iRerpretipo = dr.GetOrdinal(this.Rerpretipo);
            if (!dr.IsDBNull(iRerpretipo)) entity.Rerpretipo = dr.GetString(iRerpretipo);

            int iRerpreusucreacion = dr.GetOrdinal(this.Rerpreusucreacion);
            if (!dr.IsDBNull(iRerpreusucreacion)) entity.Rerpreusucreacion = dr.GetString(iRerpreusucreacion);

            int iRerprefeccreacion = dr.GetOrdinal(this.Rerprefeccreacion);
            if (!dr.IsDBNull(iRerprefeccreacion)) entity.Rerprefeccreacion = dr.GetDateTime(iRerprefeccreacion);

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecacodi = dr.GetOrdinal(this.Recacodi);
            if (!dr.IsDBNull(iRecacodi)) entity.Recacodi = Convert.ToInt32(dr.GetValue(iRecacodi));
            return entity;
        }

        #region Mapeo de Campos
        public string Rerprecodi = "RERPRECODI";
        public string Rerpprcodi = "RERPPRCODI";
        public string Perinombre = "PERINOMBRE";
        public string Recanombre = "RECANOMBRE";
        public string Rerpretipo = "RERPRETIPO";
        public string Rerpreusucreacion = "RERPREUSUCREACION";
        public string Rerprefeccreacion = "RERPREFECCREACION";
        public string Pericodi = "PERICODI";
        public string Recacodi = "RECACODI";
        #endregion

        public string SqlListByRerpprcodiByTipo
        {
            get { return base.GetSqlXml("ListByRerpprcodiByTipo"); }
        }
        public string SqlDeleteAllByRerpprcodi
        {
            get { return base.GetSqlXml("DeleteAllByRerpprcodi"); }
        }
        
    }
}

