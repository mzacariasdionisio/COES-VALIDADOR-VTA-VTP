using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_TRANSMISOR
    /// </summary>
    public class StTransmisorHelper : HelperBase
    {
        public StTransmisorHelper(): base(Consultas.StTransmisorSql)
        {
        }

        public StTransmisorDTO Create(IDataReader dr)
        {
            StTransmisorDTO entity = new StTransmisorDTO();

            int iStranscodi = dr.GetOrdinal(this.Stranscodi);
            if (!dr.IsDBNull(iStranscodi)) entity.Stranscodi = Convert.ToInt32(dr.GetValue(iStranscodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iStransusucreacion = dr.GetOrdinal(this.Stransusucreacion);
            if (!dr.IsDBNull(iStransusucreacion)) entity.Stransusucreacion = dr.GetString(iStransusucreacion);

            int iStransfeccreacion = dr.GetOrdinal(this.Stransfeccreacion);
            if (!dr.IsDBNull(iStransfeccreacion)) entity.Stransfeccreacion = dr.GetDateTime(iStransfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Stranscodi = "STRANSCODI";
        public string Strecacodi = "STRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Stransusucreacion = "STRANSUSUCREACION";
        public string Stransfeccreacion = "STRANSFECCREACION";
        //atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        #endregion

        public string SqlListByStTransmisorVersion
        {
            get { return base.GetSqlXml("ListByStTransmisorVersion"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }
        
        
    }
}
