using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_TIPOAREA
    /// </summary>
    public class EqTipoareaHelper : HelperBase
    {
        public EqTipoareaHelper(): base(Consultas.EqTipoareaSql)
        {
        }

        public EqTipoareaDTO Create(IDataReader dr)
        {
            EqTipoareaDTO entity = new EqTipoareaDTO();

            int iTareacodi = dr.GetOrdinal(this.Tareacodi);
            if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

            int iTareaabrev = dr.GetOrdinal(this.Tareaabrev);
            if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

            int iTareanomb = dr.GetOrdinal(this.Tareanomb);
            if (!dr.IsDBNull(iTareanomb)) entity.Tareanomb = dr.GetString(iTareanomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Tareacodi = "TAREACODI";
        public string Tareaabrev = "TAREAABREV";
        public string Tareanomb = "TAREANOMB";

        #endregion
    }
}

