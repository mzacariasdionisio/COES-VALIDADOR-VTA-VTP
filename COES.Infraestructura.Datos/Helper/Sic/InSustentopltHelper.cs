using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_SUSTENTOPLT
    /// </summary>
    public class InSustentopltHelper : HelperBase
    {
        public InSustentopltHelper() : base(Consultas.InSustentopltSql)
        {
        }

        public InSustentopltDTO Create(IDataReader dr)
        {
            InSustentopltDTO entity = new InSustentopltDTO();

            int iInpstcodi = dr.GetOrdinal(this.Inpstcodi);
            if (!dr.IsDBNull(iInpstcodi)) entity.Inpstcodi = Convert.ToInt32(dr.GetValue(iInpstcodi));

            int iInpsttipo = dr.GetOrdinal(this.Inpsttipo);
            if (!dr.IsDBNull(iInpsttipo)) entity.Inpsttipo = Convert.ToInt32(dr.GetValue(iInpsttipo));

            int iInpstnombre = dr.GetOrdinal(this.Inpstnombre);
            if (!dr.IsDBNull(iInpstnombre)) entity.Inpstnombre = dr.GetString(iInpstnombre);

            int iInpstestado = dr.GetOrdinal(this.Inpstestado);
            if (!dr.IsDBNull(iInpstestado)) entity.Inpstestado = dr.GetString(iInpstestado);

            int iInpstusumodificacion = dr.GetOrdinal(this.Inpstusumodificacion);
            if (!dr.IsDBNull(iInpstusumodificacion)) entity.Inpstusumodificacion = dr.GetString(iInpstusumodificacion);

            int iInpstfecmodificacion = dr.GetOrdinal(this.Inpstfecmodificacion);
            if (!dr.IsDBNull(iInpstfecmodificacion)) entity.Inpstfecmodificacion = dr.GetDateTime(iInpstfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Inpstcodi = "INPSTCODI";
        public string Inpsttipo = "INPSTTIPO";
        public string Inpstnombre = "INPSTNOMBRE";
        public string Inpstestado = "INPSTESTADO";
        public string Inpstusumodificacion = "INPSTUSUMODIFICACION";
        public string Inpstfecmodificacion = "INPSTFECMODIFICACION";

        #endregion

        public string SqlUpdateEstado
        {
            get { return GetSqlXml("UpdateEstado"); }
        }

        public string SqlGetVigenteByTipo
        {
            get { return GetSqlXml("GetVigenteByTipo"); }
        }
    }
}
