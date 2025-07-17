using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_RELACION_OD_MO
    /// </summary>
    public class SmaRelacionOdMoHelper : HelperBase
    {
        public SmaRelacionOdMoHelper()
            : base(Consultas.SmaRelacionOdMoSql)
        {
        }

        public SmaRelacionOdMoDTO Create(IDataReader dr)
        {
            SmaRelacionOdMoDTO entity = new SmaRelacionOdMoDTO();

            int iOdmousucreacion = dr.GetOrdinal(this.Odmousucreacion);
            if (!dr.IsDBNull(iOdmousucreacion)) entity.Odmousucreacion = dr.GetString(iOdmousucreacion);

            int iOdmofeccreacion = dr.GetOrdinal(this.Odmofeccreacion);
            if (!dr.IsDBNull(iOdmofeccreacion)) entity.Odmofeccreacion = dr.GetDateTime(iOdmofeccreacion);

            int iOdmousumodificacion = dr.GetOrdinal(this.Odmousumodificacion);
            if (!dr.IsDBNull(iOdmousumodificacion)) entity.Odmousumodificacion = dr.GetString(iOdmousumodificacion);

            int iOdmofecmodificacion = dr.GetOrdinal(this.Odmofecmodificacion);
            if (!dr.IsDBNull(iOdmofecmodificacion)) entity.Odmofecmodificacion = dr.GetDateTime(iOdmofecmodificacion);

            int iOfdecodi = dr.GetOrdinal(this.Ofdecodi);
            if (!dr.IsDBNull(iOfdecodi)) entity.Ofdecodi = Convert.ToInt32(dr.GetValue(iOfdecodi));

            int iOdmocodi = dr.GetOrdinal(this.Odmocodi);
            if (!dr.IsDBNull(iOdmocodi)) entity.Odmocodi = Convert.ToInt32(dr.GetValue(iOdmocodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iOdmopotmaxofer = dr.GetOrdinal(this.Odmopotmaxofer);
            if (!dr.IsDBNull(iOdmopotmaxofer)) entity.Odmopotmaxofer = dr.GetDecimal(iOdmopotmaxofer);

            int iOdmobndcalificada = dr.GetOrdinal(this.Odmobndcalificada);
            if (!dr.IsDBNull(iOdmobndcalificada)) entity.Odmobndcalificada = dr.GetDecimal(iOdmobndcalificada);

            int iOdmobnddisponible = dr.GetOrdinal(this.Odmobnddisponible);
            if (!dr.IsDBNull(iOdmobnddisponible)) entity.Odmobnddisponible = dr.GetDecimal(iOdmobnddisponible);

            int iOdmoestado = dr.GetOrdinal(this.Odmoestado);
            if (!dr.IsDBNull(iOdmoestado)) entity.Odmoestado = dr.GetString(iOdmoestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Odmousucreacion = "ODMOUSUCREACION";
        public string Odmofeccreacion = "ODMOFECCREACION";
        public string Odmousumodificacion = "ODMOUSUMODIFICACION";
        public string Odmofecmodificacion = "ODMOFECMODIFICACION";
        public string Ofdecodi = "OFDECODI";
        public string Odmocodi = "ODMOCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Odmopotmaxofer = "ODMOPOTMAXOFER";
        public string Odmobndcalificada = "ODMOBNDCALIFICADA";
        public string Odmobnddisponible = "ODMOBNDDISPONIBLE";
        public string Odmoestado = "ODMOESTADO";

        #endregion
    }
}
