using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_ENERGIA
    /// </summary>
    public class StEnergiaHelper : HelperBase
    {
        public StEnergiaHelper(): base(Consultas.StEnergiaSql)
        {
        }

        public StEnergiaDTO Create(IDataReader dr)
        {
            StEnergiaDTO entity = new StEnergiaDTO();

            int iStenrgcodi = dr.GetOrdinal(this.Stenrgcodi);
            if (!dr.IsDBNull(iStenrgcodi)) entity.Stenrgcodi = Convert.ToInt32(dr.GetValue(iStenrgcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iStenrgrgia = dr.GetOrdinal(this.Stenrgrgia);
            if (!dr.IsDBNull(iStenrgrgia)) entity.Stenrgrgia = dr.GetDecimal(iStenrgrgia);

            int iStenrgusucreacion = dr.GetOrdinal(this.Stenrgusucreacion);
            if (!dr.IsDBNull(iStenrgusucreacion)) entity.Stenrgusucreacion = dr.GetString(iStenrgusucreacion);

            int iStenrgfeccreacion = dr.GetOrdinal(this.Stenrgfeccreacion);
            if (!dr.IsDBNull(iStenrgfeccreacion)) entity.Stenrgfeccreacion = dr.GetDateTime(iStenrgfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Stenrgcodi = "STENRGCODI";
        public string Strecacodi = "STRECACODI";
        public string Equicodi = "EQUICODI";
        public string Stenrgrgia = "STENRGRGIA";
        public string Stenrgusucreacion = "STENRGUSUCREACION";
        public string Stenrgfeccreacion = "STENRGFECCREACION";
        //atributos de consultas
        public string Equinomb = "EQUINOMB";
        public string Stcntgcodi = "STCNTGCODI";
        #endregion

        public string SqlGetByCentralCodi
        {
            get { return base.GetSqlXml("GetByCentralCodi"); }
        }

        public string SqlListByStEnergiaVersion
        {
            get { return base.GetSqlXml("ListByStEnergiaVersion"); }
        }
        
    }
}
