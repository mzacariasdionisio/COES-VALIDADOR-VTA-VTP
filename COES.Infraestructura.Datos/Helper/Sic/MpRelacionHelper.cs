using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_RELACION
    /// </summary>
    public class MpRelacionHelper : HelperBase
    {
        public MpRelacionHelper(): base(Consultas.MpRelacionSql)
        {
        }

        public MpRelacionDTO Create(IDataReader dr)
        {
            MpRelacionDTO entity = new MpRelacionDTO();

            int iMtopcodi = dr.GetOrdinal(this.Mtopcodi);
            if (!dr.IsDBNull(iMtopcodi)) entity.Mtopcodi = Convert.ToInt32(dr.GetValue(iMtopcodi));

            int iMtrelcodi = dr.GetOrdinal(this.Mtrelcodi);
            if (!dr.IsDBNull(iMtrelcodi)) entity.Mtrelcodi = Convert.ToInt32(dr.GetValue(iMtrelcodi));

            int iMrecurcodi1 = dr.GetOrdinal(this.Mrecurcodi1);
            if (!dr.IsDBNull(iMrecurcodi1)) entity.Mrecurcodi1 = Convert.ToInt32(dr.GetValue(iMrecurcodi1));

            int iMrecurcodi2 = dr.GetOrdinal(this.Mrecurcodi2);
            if (!dr.IsDBNull(iMrecurcodi2)) entity.Mrecurcodi2 = Convert.ToInt32(dr.GetValue(iMrecurcodi2));

            int iMrelvalor = dr.GetOrdinal(this.Mrelvalor);
            if (!dr.IsDBNull(iMrelvalor)) entity.Mrelvalor = dr.GetDecimal(iMrelvalor);

            int iMrelusumodificacion = dr.GetOrdinal(this.Mrelusumodificacion);
            if (!dr.IsDBNull(iMrelusumodificacion)) entity.Mrelusumodificacion = dr.GetString(iMrelusumodificacion);

            int iMrelfecmodificacion = dr.GetOrdinal(this.Mrelfecmodificacion);
            if (!dr.IsDBNull(iMrelfecmodificacion)) entity.Mrelfecmodificacion = dr.GetDateTime(iMrelfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Mtopcodi = "MTOPCODI";
        public string Mtrelcodi = "MTRELCODI";
        public string Mrecurcodi1 = "MRECURCODI1";
        public string Mrecurcodi2 = "MRECURCODI2";
        public string Mrelvalor = "MRELVALOR";
        public string Mrelusumodificacion = "MRELUSUMODIFICACION";
        public string Mrelfecmodificacion = "MRELFECMODIFICACION";

        #endregion

        public string SqlListarByTopologia
        {
            get { return base.GetSqlXml("ListarByTopologia"); }
        }

        public string SqlListarByTopologiaYRecurso
        {
            get { return base.GetSqlXml("ListarByTopologiaYRecurso"); }
        }
        
    }
}
