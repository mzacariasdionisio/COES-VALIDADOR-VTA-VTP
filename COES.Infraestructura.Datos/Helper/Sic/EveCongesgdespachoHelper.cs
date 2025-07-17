using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_CONGESGDESPACHO
    /// </summary>
    public class EveCongesgdespachoHelper : HelperBase
    {
        public EveCongesgdespachoHelper()
            : base(Consultas.EveCongesgdespachoSql)
        {
        }

        public EveCongesgdespachoDTO Create(IDataReader dr)
        {
            EveCongesgdespachoDTO entity = new EveCongesgdespachoDTO();

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iIccodi = dr.GetOrdinal(this.Iccodi);
            if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

            int iCongdecodi = dr.GetOrdinal(this.Congdecodi);
            if (!dr.IsDBNull(iCongdecodi)) entity.Congdecodi = Convert.ToInt32(dr.GetValue(iCongdecodi));

            int iCongdefechaini = dr.GetOrdinal(this.Congdefechaini);
            if (!dr.IsDBNull(iCongdefechaini)) entity.Congdefechaini = dr.GetDateTime(iCongdefechaini);

            int iCongdefechafin = dr.GetOrdinal(this.Congdefechafin);
            if (!dr.IsDBNull(iCongdefechafin)) entity.Congdefechafin = dr.GetDateTime(iCongdefechafin);

            int iCongdeusucreacion = dr.GetOrdinal(this.Congdeusucreacion);
            if (!dr.IsDBNull(iCongdeusucreacion)) entity.Congdeusucreacion = dr.GetString(iCongdeusucreacion);

            int iCongdefeccreacion = dr.GetOrdinal(this.Congdefeccreacion);
            if (!dr.IsDBNull(iCongdefeccreacion)) entity.Congdefeccreacion = dr.GetDateTime(iCongdefeccreacion);

            int iCongdeusumodificacion = dr.GetOrdinal(this.Congdeusumodificacion);
            if (!dr.IsDBNull(iCongdeusumodificacion)) entity.Congdeusumodificacion = dr.GetString(iCongdeusumodificacion);

            int iCongdefecmodificacion = dr.GetOrdinal(this.Congdefecmodificacion);
            if (!dr.IsDBNull(iCongdefecmodificacion)) entity.Congdefecmodificacion = dr.GetDateTime(iCongdefecmodificacion);

            int iCongdeestado = dr.GetOrdinal(this.Congdeestado);
            if (!dr.IsDBNull(iCongdeestado)) entity.Congdeestado = Convert.ToInt32(dr.GetValue(iCongdeestado));

            return entity;
        }

        #region Mapeo de Campos

        public string Grupocodi = "GRUPOCODI";
        public string Iccodi = "ICCODI";
        public string Congdecodi = "CONGDECODI";
        public string Congdefechaini = "CONGDEFECHAINI";
        public string Congdefechafin = "CONGDEFECHAFIN";
        public string Congdeusucreacion = "CONGDEUSUCREACION";
        public string Congdefeccreacion = "CONGDEFECCREACION";
        public string Congdeusumodificacion = "CONGDEUSUMODIFICACION";
        public string Congdefecmodificacion = "CONGDEFECMODIFICACION";
        public string Congdeestado = "CONGDEESTADO";

        public string Gruponomb = "GRUPONOMB";
        public string Equicodi = "EQUICODI";
        public string Equiabrev = "EQUIABREV";
        public string Equinomb = "EQUINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Icdescrip2 = "ICDESCRIP2";
        public string Ichorini = "ICHORINI";
        public string Ichorfin = "ICHORFIN";
        public string Areanomb = "AREANOMB";
        public string Central = "CENTRAL";
        public string Grupopadre = "GRUPOPADRE";
        public string Catecodi = "CATECODI";
        #endregion

        public string SqlUpdateEstado
        {
            get { return GetSqlXml("UpdateEstado"); }
        }

        public string ObtenerListadoCongestion
        {
            get { return base.GetSqlXml("ObtenerListadoCongestion"); }
        }

    }
}
