using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_MAXDEMANDA
    /// </summary>
    public class CaiMaxdemandaHelper : HelperBase
    {
        public CaiMaxdemandaHelper(): base(Consultas.CaiMaxdemandaSql)
        {
        }

        public CaiMaxdemandaDTO Create(IDataReader dr)
        {
            CaiMaxdemandaDTO entity = new CaiMaxdemandaDTO();

            int iCaimdecodi = dr.GetOrdinal(this.Caimdecodi);
            if (!dr.IsDBNull(iCaimdecodi)) entity.Caimdecodi = Convert.ToInt32(dr.GetValue(iCaimdecodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iCaimdeaniomes = dr.GetOrdinal(this.Caimdeaniomes);
            if (!dr.IsDBNull(iCaimdeaniomes)) entity.Caimdeaniomes = Convert.ToInt32(dr.GetValue(iCaimdeaniomes));

            int iCaimdefechor = dr.GetOrdinal(this.Caimdefechor);
            if (!dr.IsDBNull(iCaimdefechor)) entity.Caimdefechor = dr.GetDateTime(iCaimdefechor);

            int iCaimdetipoinfo = dr.GetOrdinal(this.Caimdetipoinfo);
            if (!dr.IsDBNull(iCaimdetipoinfo)) entity.Caimdetipoinfo = dr.GetString(iCaimdetipoinfo);

            int iCaimdemaxdemanda = dr.GetOrdinal(this.Caimdemaxdemanda);
            if (!dr.IsDBNull(iCaimdemaxdemanda)) entity.Caimdemaxdemanda = dr.GetDecimal(iCaimdemaxdemanda);

            int iCaimdeusucreacion = dr.GetOrdinal(this.Caimdeusucreacion);
            if (!dr.IsDBNull(iCaimdeusucreacion)) entity.Caimdeusucreacion = dr.GetString(iCaimdeusucreacion);

            int iCaimdefeccreacion = dr.GetOrdinal(this.Caimdefeccreacion);
            if (!dr.IsDBNull(iCaimdefeccreacion)) entity.Caimdefeccreacion = dr.GetDateTime(iCaimdefeccreacion);

            int iCaimdeusumodificacion = dr.GetOrdinal(this.Caimdeusumodificacion);
            if (!dr.IsDBNull(iCaimdeusumodificacion)) entity.Caimdeusumodificacion = dr.GetString(iCaimdeusumodificacion);

            int iCaimdefecmodificacion = dr.GetOrdinal(this.Caimdefecmodificacion);
            if (!dr.IsDBNull(iCaimdefecmodificacion)) entity.Caimdefecmodificacion = dr.GetDateTime(iCaimdefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caimdecodi = "CAIMDECODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Caimdeaniomes = "CAIMDEANIOMES";
        public string Caimdefechor = "CAIMDEFECHOR";
        public string Caimdetipoinfo = "CAIMDETIPOINFO";
        public string Caimdemaxdemanda = "CAIMDEMAXDEMANDA";
        public string Caimdeusucreacion = "CAIMDEUSUCREACION";
        public string Caimdefeccreacion = "CAIMDEFECCREACION";
        public string Caimdeusumodificacion = "CAIMDEUSUMODIFICACION";
        public string Caimdefecmodificacion = "CAIMDEFECMODIFICACION";

        #endregion

        public string SqlDeleteEjecutada
        {
            get { return base.GetSqlXml("DeleteEjecutada"); }
        }



        public string SqlGetByCaimdeAnioMes
        {
            get { return base.GetSqlXml("GetByCaimdeAnioMes"); }
        }
    }
}
