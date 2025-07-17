using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_SUSTENTO
    /// </summary>
    public class InSustentoHelper : HelperBase
    {
        public InSustentoHelper() : base(Consultas.InSustentoSql)
        {
        }

        public InSustentoDTO Create(IDataReader dr)
        {
            InSustentoDTO entity = new InSustentoDTO();

            int iInstcodi = dr.GetOrdinal(this.Instcodi);
            if (!dr.IsDBNull(iInstcodi)) entity.Instcodi = Convert.ToInt32(dr.GetValue(iInstcodi));

            int iInstestado = dr.GetOrdinal(this.Instestado);
            if (!dr.IsDBNull(iInstestado)) entity.Instestado = dr.GetString(iInstestado);

            int iInstusumodificacion = dr.GetOrdinal(this.Instusumodificacion);
            if (!dr.IsDBNull(iInstusumodificacion)) entity.Instusumodificacion = dr.GetString(iInstusumodificacion);

            int iInstfecmodificacion = dr.GetOrdinal(this.Instfecmodificacion);
            if (!dr.IsDBNull(iInstfecmodificacion)) entity.Instfecmodificacion = dr.GetDateTime(iInstfecmodificacion);

            int iIntercodi = dr.GetOrdinal(this.Intercodi);
            if (!dr.IsDBNull(iIntercodi)) entity.Intercodi = Convert.ToInt32(dr.GetValue(iIntercodi));

            int iInpstcodi = dr.GetOrdinal(this.Inpstcodi);
            if (!dr.IsDBNull(iInpstcodi)) entity.Inpstcodi = Convert.ToInt32(dr.GetValue(iInpstcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Instcodi = "INSTCODI";
        public string Instestado = "INSTESTADO";
        public string Instusumodificacion = "INSTUSUMODIFICACION";
        public string Instfecmodificacion = "INSTFECMODIFICACION";
        public string Intercodi = "INTERCODI";
        public string Inpstcodi = "INPSTCODI";

        public string Inpsttipo = "INPSTTIPO";

        #endregion

        public string SqlGetByIntercodi
        {
            get { return base.GetSqlXml("GetByIntercodi"); }
        }

    }
}
