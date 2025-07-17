using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_GENERADOR
    /// </summary>
    public class StGeneradorHelper : HelperBase
    {
        public StGeneradorHelper()
            : base(Consultas.StGeneradorSql)
        {
        }

        public StGeneradorDTO Create(IDataReader dr)
        {
            StGeneradorDTO entity = new StGeneradorDTO();

            int iStgenrcodi = dr.GetOrdinal(this.Stgenrcodi);
            if (!dr.IsDBNull(iStgenrcodi)) entity.Stgenrcodi = Convert.ToInt32(dr.GetValue(iStgenrcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iStgenrusucreacion = dr.GetOrdinal(this.Stgenrusucreacion);
            if (!dr.IsDBNull(iStgenrusucreacion)) entity.Stgenrusucreacion = dr.GetString(iStgenrusucreacion);

            int iStgenrfeccreacion = dr.GetOrdinal(this.Stgenrfeccreacion);
            if (!dr.IsDBNull(iStgenrfeccreacion)) entity.Stgenrfeccreacion = dr.GetDateTime(iStgenrfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Stgenrcodi = "STGENRCODI";
        public string Strecacodi = "STRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Stgenrusucreacion = "STGENRUSUCREACION";
        public string Stgenrfeccreacion = "STGENRFECCREACION";
        //variables de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Barrnombre = "BARRNOMBRE";
        #endregion

        public string SqlListByStGeneradorVersion
        {
            get { return base.GetSqlXml("ListByStGeneradorVersion"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }


        public string SqlListByStGeneradorReporte
        {
            get { return base.GetSqlXml("ListByStGeneradorReporte"); }
        }
    }
}
