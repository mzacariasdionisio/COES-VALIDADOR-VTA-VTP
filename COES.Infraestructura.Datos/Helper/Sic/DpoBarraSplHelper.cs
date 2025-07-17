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
    public class DpoBarraSplHelper : HelperBase
    {
        public DpoBarraSplHelper() : base(Consultas.DpoBarraSplSql)
        {
        }

        public DpoBarraSplDTO Create(IDataReader dr)
        {
            DpoBarraSplDTO entity = new DpoBarraSplDTO();

            int iBarsplcodi = dr.GetOrdinal(this.Barsplcodi);
            if (!dr.IsDBNull(iBarsplcodi)) entity.Barsplcodi = Convert.ToInt32(dr.GetValue(iBarsplcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iGrupoabrev = dr.GetOrdinal(this.Grupoabrev);
            if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

            int iBarsplestado = dr.GetOrdinal(this.Grupoabrev);
            if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

            int iBarsplusucreacion = dr.GetOrdinal(this.Barsplusucreacion);
            if (!dr.IsDBNull(iBarsplusucreacion)) entity.Barsplusucreacion = dr.GetString(iBarsplusucreacion);

            int iBarsplfeccreacion = dr.GetOrdinal(this.Barsplfeccreacion);
            if (!dr.IsDBNull(iBarsplfeccreacion)) entity.Barsplfeccreacion = dr.GetDateTime(iBarsplfeccreacion);

            return entity;
        }


        #region Mapeo de Campos
        public string Barsplcodi = "BARSPLCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Grupoabrev = "GRUPOABREV";
        public string Barsplestado = "BARSPLESTADO";
        public string Barsplusucreacion = "BARSPLUSUCREACION";
        public string Barsplfeccreacion = "BARSPLFECCREACION";
        #endregion

        public string SqlUpdateEstado
        {
            get { return GetSqlXml("UpdateEstado"); }
        }
        public string SqlListBarrasSPLByGrupo
        {
            get { return GetSqlXml("ListBarrasSPLByGrupo"); }
        }
    }
}
