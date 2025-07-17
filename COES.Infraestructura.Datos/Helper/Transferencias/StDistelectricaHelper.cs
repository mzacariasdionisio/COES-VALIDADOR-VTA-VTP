using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_DISTELECTRICA
    /// </summary>
    public class StDistelectricaHelper : HelperBase
    {
        public StDistelectricaHelper(): base(Consultas.StDistelectricaSql)
        {
        }

        public StDistelectricaDTO Create(IDataReader dr)
        {
            StDistelectricaDTO entity = new StDistelectricaDTO();

            int iDstelecodi = dr.GetOrdinal(this.Dstelecodi);
            if (!dr.IsDBNull(iDstelecodi)) entity.Dstelecodi = Convert.ToInt32(dr.GetValue(iDstelecodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iDsteleusucreacion = dr.GetOrdinal(this.Dsteleusucreacion);
            if (!dr.IsDBNull(iDsteleusucreacion)) entity.Dsteleusucreacion = dr.GetString(iDsteleusucreacion);

            int iDstelefeccreacion = dr.GetOrdinal(this.Dstelefeccreacion);
            if (!dr.IsDBNull(iDstelefeccreacion)) entity.Dstelefeccreacion = dr.GetDateTime(iDstelefeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Dstelecodi = "DSTELECODI";
        public string Barrcodi = "BARRCODI";
        public string Strecacodi = "STRECACODI";
        public string Dsteleusucreacion = "DSTELEUSUCREACION";
        public string Dstelefeccreacion = "DSTELEFECCREACION";
        //ATRIBUTOS PARA CONSULTAS
        //public string Sistrnnombre = "SISTRNNOMBRE";
        public string Barrnombre = "BARRNOMBRE";
        #endregion

        public string SqlGetByCriteriaVersion
        {
            get { return base.GetSqlXml("GetByCriteriaVersion"); }
        }

    }
}
