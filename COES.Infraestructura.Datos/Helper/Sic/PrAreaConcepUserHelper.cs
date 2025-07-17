using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_AREACONCEPUSER
    /// </summary>
    public class PrAreaConcepUserHelper : HelperBase
    {
        public PrAreaConcepUserHelper()
            : base(Consultas.PrAreaConcepUserSql)
        {
        }

        public PrAreaConcepUserDTO Create(IDataReader dr)
        {
            PrAreaConcepUserDTO entity = new PrAreaConcepUserDTO();

            int iAconuscodi = dr.GetOrdinal(this.Aconuscodi);
            if (!dr.IsDBNull(iAconuscodi)) entity.Aconuscodi = Convert.ToInt32(dr.GetValue(iAconuscodi));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iArconcodi = dr.GetOrdinal(this.Arconcodi);
            if (!dr.IsDBNull(iArconcodi)) entity.Arconcodi = Convert.ToInt32(dr.GetValue(iArconcodi));

            int iAconusactivo = dr.GetOrdinal(this.Aconusactivo);
            if (!dr.IsDBNull(iAconusactivo)) entity.Aconusactivo = Convert.ToInt32(dr.GetValue(iAconusactivo));

            int iAconususucreacion = dr.GetOrdinal(this.Aconususucreacion);
            if (!dr.IsDBNull(iAconususucreacion)) entity.Aconususucreacion = dr.GetString(iAconususucreacion);

            int iAconusfeccreacion = dr.GetOrdinal(this.Aconusfeccreacion);
            if (!dr.IsDBNull(iAconusfeccreacion)) entity.Aconusfeccreacion = dr.GetDateTime(iAconusfeccreacion);

            int iAconususumodificacion = dr.GetOrdinal(this.Aconususumodificacion);
            if (!dr.IsDBNull(iAconususumodificacion)) entity.Aconususumodificacion = dr.GetString(iAconususumodificacion);

            int iAconusfecmodificacion = dr.GetOrdinal(this.Aconusfecmodificacion);
            if (!dr.IsDBNull(iAconusfecmodificacion)) entity.Aconusfecmodificacion = dr.GetDateTime(iAconusfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Aconuscodi = "ACONUSCODI";
        public string Usercode = "USERCODE";
        public string Arconcodi = "ARCONCODI";
        public string Aconusactivo = "ACONUSACTIVO";
        public string Aconususucreacion = "ACONUSUSUCREACION";
        public string Aconusfeccreacion = "ACONUSFECCREACION";
        public string Aconususumodificacion = "ACONUSUSUMODIFICACION";
        public string Aconusfecmodificacion = "ACONUSFECMODIFICACION";

        public string Concepcodi = "CONCEPCODI";
        #endregion

        #region Mapeo de Consultas

        public string SqlListarConcepcodiByUsercode
        {
            get { return base.GetSqlXml("ListarConcepcodiByUsercode"); }
        }

        #endregion
    }
}
