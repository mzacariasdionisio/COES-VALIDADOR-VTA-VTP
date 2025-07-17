using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_PROPRECURSO
    /// </summary>
    public class MpProprecursoHelper : HelperBase
    {
        public MpProprecursoHelper(): base(Consultas.MpProprecursoSql)
        {
        }

        public MpProprecursoDTO Create(IDataReader dr)
        {
            MpProprecursoDTO entity = new MpProprecursoDTO();

            int iMtopcodi = dr.GetOrdinal(this.Mtopcodi);
            if (!dr.IsDBNull(iMtopcodi)) entity.Mtopcodi = Convert.ToInt32(dr.GetValue(iMtopcodi));

            int iMrecurcodi = dr.GetOrdinal(this.Mrecurcodi);
            if (!dr.IsDBNull(iMrecurcodi)) entity.Mrecurcodi = Convert.ToInt32(dr.GetValue(iMrecurcodi));

            int iMpropcodi = dr.GetOrdinal(this.Mpropcodi);
            if (!dr.IsDBNull(iMpropcodi)) entity.Mpropcodi = Convert.ToInt32(dr.GetValue(iMpropcodi));

            int iMprvalfecvig = dr.GetOrdinal(this.Mprvalfecvig);
            if (!dr.IsDBNull(iMprvalfecvig)) entity.Mprvalfecvig = dr.GetDateTime(iMprvalfecvig);

            int iMprvalvalor = dr.GetOrdinal(this.Mprvalvalor);
            if (!dr.IsDBNull(iMprvalvalor)) entity.Mprvalvalor = dr.GetString(iMprvalvalor);

            return entity;
        }


        #region Mapeo de Campos

        public string Mtopcodi = "MTOPCODI";
        public string Mrecurcodi = "MRECURCODI";
        public string Mpropcodi = "MPROPCODI";
        public string Mprvalfecvig = "MPRVALFECVIG";
        public string Mprvalvalor = "MPRVALVALOR";

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
