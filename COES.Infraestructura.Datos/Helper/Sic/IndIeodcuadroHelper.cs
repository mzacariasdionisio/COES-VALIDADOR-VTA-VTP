using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_IEODCUADRO
    /// </summary>
    public class IndIeodcuadroHelper : HelperBase
    {
        public IndIeodcuadroHelper(): base(Consultas.IndIeodcuadroSql)
        {
        }

        public IndIeodcuadroDTO Create(IDataReader dr)
        {
            IndIeodcuadroDTO entity = new IndIeodcuadroDTO();

            int iIiccocodi = dr.GetOrdinal(this.Iiccocodi);
            if (!dr.IsDBNull(iIiccocodi)) entity.Iiccocodi = Convert.ToInt32(dr.GetValue(iIiccocodi));

            int iIiccotipoindisp = dr.GetOrdinal(this.Iiccotipoindisp);
            if (!dr.IsDBNull(iIiccotipoindisp)) entity.Iiccotipoindisp = dr.GetString(iIiccotipoindisp);

            int iIiccomw = dr.GetOrdinal(this.Iiccopr);
            if (!dr.IsDBNull(iIiccomw)) entity.Iiccopr = dr.GetDecimal(iIiccomw);

            int iIiccousucreacion = dr.GetOrdinal(this.Iiccousucreacion);
            if (!dr.IsDBNull(iIiccousucreacion)) entity.Iiccousucreacion = dr.GetString(iIiccousucreacion);

            int iIiccofeccreacion = dr.GetOrdinal(this.Iiccofeccreacion);
            if (!dr.IsDBNull(iIiccofeccreacion)) entity.Iiccofeccreacion = dr.GetDateTime(iIiccofeccreacion);

            int iIiccousumodificacion = dr.GetOrdinal(this.Iiccousumodificacion);
            if (!dr.IsDBNull(iIiccousumodificacion)) entity.Iiccousumodificacion = dr.GetString(iIiccousumodificacion);

            int iIiccofecmodificacion = dr.GetOrdinal(this.Iiccofecmodificacion);
            if (!dr.IsDBNull(iIiccofecmodificacion)) entity.Iiccofecmodificacion = dr.GetDateTime(iIiccofecmodificacion);

            int iIiccocomentario = dr.GetOrdinal(this.Iiccocomentario);
            if (!dr.IsDBNull(iIiccocomentario)) entity.Iiccocomentario = dr.GetString(iIiccocomentario);

            int iIccodi = dr.GetOrdinal(this.Iccodi);
            if (!dr.IsDBNull(iIccodi)) entity.Iccodi = Convert.ToInt32(dr.GetValue(iIccodi));

            int iIiccoestado = dr.GetOrdinal(this.Iiccoestado);
            if (!dr.IsDBNull(iIiccoestado)) entity.Iiccoestado = dr.GetString(iIiccoestado);

            return entity;
        }

        #region Mapeo de Campos

        public string Iiccocodi = "IICCOCODI";
        public string Iiccotipoindisp = "IICCOTIPOINDISP";
        public string Iiccopr = "IICCOPR";
        public string Iiccousucreacion = "IICCOUSUCREACION";
        public string Iiccofeccreacion = "IICCOFECCREACION";
        public string Iiccousumodificacion = "IICCOUSUMODIFICACION";
        public string Iiccofecmodificacion = "IICCOFECMODIFICACION";
        public string Iiccocomentario = "IICCOCOMENTARIO";
        public string Iccodi = "ICCODI";
        public string Iiccoestado = "IICCOESTADO";

        public string Ichorini = "ICHORINI";
        public string Ichorfin = "ICHORFIN";
        public string Icdescrip1 = "Icdescrip1";
        public string Icdescrip2 = "Icdescrip2";
        public string Icdescrip3 = "Icdescrip3";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Equicodi = "Equicodi";
        public string Equipadre = "Equipadre";
        public string Emprcodi = "Emprcodi";
        public string Equiabrev = "Equiabrev";
        public string Grupocodi = "Grupocodi";
        public string Emprnomb = "EMPRNOMB";
        public string Emprabrev = "EMPRABREV";
        public string Evenclasedesc = "EVENCLASEDESC";
        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Areadesc = "AREADESC";
        public string Famcodi = "FAMCODI";
        public string Famnomb = "FAMNOMB";
        public string Equinomb = "EQUINOMB";
        public string Evenclaseabrev = "EVENCLASEABREV";
        public string Famabrev = "FAMABREV";
        public string Central = "CENTRAL";
        public string Grupotipocogen = "GRUPOTIPOCOGEN";

        #endregion

        public string SqlListHistoricoByIccodi
        {
            get { return base.GetSqlXml("ListHistoricoByIccodi"); }
        }
    }
}
