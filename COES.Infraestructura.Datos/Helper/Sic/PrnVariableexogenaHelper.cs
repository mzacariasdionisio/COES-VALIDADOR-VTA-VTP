using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnVariableexogenaHelper : HelperBase
    {
        public PrnVariableexogenaHelper()
            : base(Consultas.PrnVariablesexogenasSql)
        {
        }

        public PrnVariableexogenaDTO Create(IDataReader dr)
        {
            PrnVariableexogenaDTO entity = new PrnVariableexogenaDTO();

            int iVarexocodi = dr.GetOrdinal(this.Varexocodi);
            if (!dr.IsDBNull(iVarexocodi)) entity.Varexocodi = Convert.ToInt32(dr.GetValue(iVarexocodi));

            int iVarexonombre = dr.GetOrdinal(this.Varexonombre);
            if (!dr.IsDBNull(iVarexonombre)) entity.Varexonombre = dr.GetString(iVarexonombre);

            int iVarexousucreacion = dr.GetOrdinal(this.Varexousucreacion);
            if (!dr.IsDBNull(iVarexousucreacion)) entity.Varexousucreacion = dr.GetString(iVarexousucreacion);

            int iVarexofeccreacion = dr.GetOrdinal(this.Varexofeccreacion);
            if (!dr.IsDBNull(iVarexofeccreacion)) entity.Varexofeccreacion = dr.GetDateTime(iVarexofeccreacion);

            int iVarexousumodificacion = dr.GetOrdinal(this.Varexousumodificacion);
            if (!dr.IsDBNull(iVarexousumodificacion)) entity.Varexousumodificacion = dr.GetString(iVarexousumodificacion);

            int iVarexofecmodificacion = dr.GetOrdinal(this.Varexofecmodificacion);
            if (!dr.IsDBNull(iVarexofecmodificacion)) entity.Varexofecmodificacion = dr.GetDateTime(iVarexofecmodificacion);

            return entity;
        }

        #region Mapeo de los campos
        public string Varexocodi = "VAREXOCODI";
        public string Varexonombre = "VAREXONOMBRE";
        public string Varexousucreacion = "VAREXOUSUCREACION";
        public string Varexofeccreacion = "VAREXOFECCREACION";
        public string Varexousumodificacion = "VAREXOUSUMODIFICACION";
        public string Varexofecmodificacion = "VAREXOFECMODIFICACION";
        #endregion
    }
}
