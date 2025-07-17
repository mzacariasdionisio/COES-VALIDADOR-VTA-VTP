using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class DpoVersionRelacionHelper : HelperBase
    {
        public DpoVersionRelacionHelper() : base(Consultas.DpoVersionRelacionSql)
        {
        }

        public DpoVersionRelacionDTO Create(IDataReader dr)
        {
            DpoVersionRelacionDTO entity = new DpoVersionRelacionDTO();

            int iDposplcodi = dr.GetOrdinal(this.Dposplcodi);
            if (!dr.IsDBNull(iDposplcodi)) entity.Dposplcodi = Convert.ToInt32(dr.GetValue(iDposplcodi));

            int iDposplnombre = dr.GetOrdinal(this.Dposplnombre);
            if (!dr.IsDBNull(iDposplnombre)) entity.Dposplnombre = dr.GetString(iDposplnombre);

            int iDposplusucreacion = dr.GetOrdinal(this.Dposplusucreacion);
            if (!dr.IsDBNull(iDposplusucreacion)) entity.Dposplusucreacion = dr.GetString(iDposplusucreacion);

            int iDposplfeccreacion = dr.GetOrdinal(this.Dposplfeccreacion);
            if (!dr.IsDBNull(iDposplfeccreacion)) entity.Dposplfeccreacion = dr.GetDateTime(iDposplfeccreacion);

            int iDposplusumodificacion = dr.GetOrdinal(this.Dposplusumodificacion);
            if (!dr.IsDBNull(iDposplusumodificacion)) entity.Dposplusumodificacion = dr.GetString(iDposplusumodificacion);

            int iDposplfecmodificacion = dr.GetOrdinal(this.Dposplfecmodificacion);
            if (!dr.IsDBNull(iDposplfecmodificacion)) entity.Dposplfecmodificacion = dr.GetDateTime(iDposplfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos
        public string Dposplcodi = "DPOSPLCODI";
        public string Dposplnombre = "DPOSPLNOMBRE";
        public string Dposplusucreacion = "DPOSPLUSUCREACION";
        public string Dposplfeccreacion = "DPOSPLFECCREACION";
        public string Dposplusumodificacion = "DPOSPLUSUMODIFICACION";
        public string Dposplfecmodificacion = "DPOSPLFECMODIFICACION";
        #endregion

        //public string SqlGetByAnhio
        //{
        //    get { return GetSqlXml("GetByAnhio"); }
        //}
    }
}
