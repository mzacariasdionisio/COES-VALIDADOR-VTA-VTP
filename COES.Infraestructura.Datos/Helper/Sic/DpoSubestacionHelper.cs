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
    public class DpoSubestacionHelper : HelperBase
    {
        public DpoSubestacionHelper() : base(Consultas.DpoSubestacionSql)
        {
        }

        public DpoSubestacionDTO Create(IDataReader dr)
        {
            DpoSubestacionDTO entity = new DpoSubestacionDTO();

            int iDposubcodiexcel = dr.GetOrdinal(this.Dposubcodiexcel);
            if (!dr.IsDBNull(iDposubcodiexcel)) entity.Dposubcodiexcel = dr.GetString(iDposubcodiexcel);

            int iDposubnombre = dr.GetOrdinal(this.Dposubnombre);
            if (!dr.IsDBNull(iDposubnombre)) entity.Dposubnombre = dr.GetString(iDposubnombre);

            int iDposubusucreacion = dr.GetOrdinal(this.Dposubusucreacion);
            if (!dr.IsDBNull(iDposubusucreacion)) entity.Dposubusucreacion = dr.GetString(iDposubusucreacion);

            int iDposubfeccreacion = dr.GetOrdinal(this.Dposubfeccreacion);
            if (!dr.IsDBNull(iDposubfeccreacion)) entity.Dposubfeccreacion = dr.GetDateTime(iDposubfeccreacion);

            return entity;
        }


        #region Mapeo de Campos
        public string Dposubcodi = "DPOSUBCODI";
        public string Dposubcodiexcel = "DPOSUBCODIEXCEL";
        public string Dposubnombre = "DPOSUBNOMBRE";
        public string Dposubusucreacion = "DPOSUBUSUCREACION";
        public string Dposubfeccreacion = "DPOSUBFECCREACION";
        #endregion

        //public string SqlUpdateEstado
        //{
        //    get { return GetSqlXml("UpdateEstado"); }
        //}
        //public string SqlListBarrasSPLByGrupo
        //{
        //    get { return GetSqlXml("ListBarrasSPLByGrupo"); }
        //}
    }
}
