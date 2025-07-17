using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_AJUSTE
    /// </summary>
    public class CaiAjusteHelper : HelperBase
    {
        public CaiAjusteHelper(): base(Consultas.CaiAjusteSql)
        {
        }

        public CaiAjusteDTO Create(IDataReader dr)
        {
            CaiAjusteDTO entity = new CaiAjusteDTO();

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iCaiprscodi = dr.GetOrdinal(this.Caiprscodi);
            if (!dr.IsDBNull(iCaiprscodi)) entity.Caiprscodi = Convert.ToInt32(dr.GetValue(iCaiprscodi));

            int iCaiajanio = dr.GetOrdinal(this.Caiajanio);
            if (!dr.IsDBNull(iCaiajanio)) entity.Caiajanio = Convert.ToInt32(dr.GetValue(iCaiajanio));

            int iCaiajmes = dr.GetOrdinal(this.Caiajmes);
            if (!dr.IsDBNull(iCaiajmes)) entity.Caiajmes = Convert.ToInt32(dr.GetValue(iCaiajmes));

            int iCaiajnombre = dr.GetOrdinal(this.Caiajnombre);
            if (!dr.IsDBNull(iCaiajnombre)) entity.Caiajnombre = dr.GetString(iCaiajnombre);

            int iCaiajusucreacion = dr.GetOrdinal(this.Caiajusucreacion);
            if (!dr.IsDBNull(iCaiajusucreacion)) entity.Caiajusucreacion = dr.GetString(iCaiajusucreacion);

            int iCaiajfeccreacion = dr.GetOrdinal(this.Caiajfeccreacion);
            if (!dr.IsDBNull(iCaiajfeccreacion)) entity.Caiajfeccreacion = dr.GetDateTime(iCaiajfeccreacion);

            int iCaiajusumodificacion = dr.GetOrdinal(this.Caiajusumodificacion);
            if (!dr.IsDBNull(iCaiajusumodificacion)) entity.Caiajusumodificacion = dr.GetString(iCaiajusumodificacion);

            int iCaiajfecmodificacion = dr.GetOrdinal(this.Caiajfecmodificacion);
            if (!dr.IsDBNull(iCaiajfecmodificacion)) entity.Caiajfecmodificacion = dr.GetDateTime(iCaiajfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caiajcodi = "CAIAJCODI";
        public string Caiprscodi = "CAIPRSCODI";
        public string Caiajanio = "CAIAJANIO";
        public string Caiajmes = "CAIAJMES";
        public string Caiajnombre = "CAIAJNOMBRE";
        public string Caiajusucreacion = "CAIAJUSUCREACION";
        public string Caiajfeccreacion = "CAIAJFECCREACION";
        public string Caiajusumodificacion = "CAIAJUSUMODIFICACION";
        public string Caiajfecmodificacion = "CAIAJFECMODIFICACION";

        #endregion

        public string SqlListByCaiPrscodi
        {
            get { return base.GetSqlXml("ListByCaiPrscodi"); }
        }

        public string SqlGetByIdAnterior
        {
            get { return base.GetSqlXml("GetByIdAnterior"); }
        }
    }
}
