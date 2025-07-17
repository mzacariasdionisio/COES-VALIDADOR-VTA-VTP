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
    public class DpoBarraHelper : HelperBase
    {
        public DpoBarraHelper() : base(Consultas.DpoBarraSql)
        {
        }

        public DpoBarraDTO Create(IDataReader dr)
        {
            DpoBarraDTO entity = new DpoBarraDTO();

            int iDpobarcodiexcel = dr.GetOrdinal(this.Dpobarcodiexcel);
            if (!dr.IsDBNull(iDpobarcodiexcel)) entity.Dpobarcodiexcel = dr.GetString(iDpobarcodiexcel);

            int iDpobarnombre = dr.GetOrdinal(this.Dpobarnombre);
            if (!dr.IsDBNull(iDpobarnombre)) entity.Dpobarnombre = dr.GetString(iDpobarnombre);

            int iDpobartension = dr.GetOrdinal(this.Dpobartension);
            if (!dr.IsDBNull(iDpobartension)) entity.Dpobartension = dr.GetDecimal(iDpobartension);

            int iDpobarusucreacion = dr.GetOrdinal(this.Dpobarusucreacion);
            if (!dr.IsDBNull(iDpobarusucreacion)) entity.Dpobarusucreacion = dr.GetString(iDpobarusucreacion);

            int iDpobarfeccreacion = dr.GetOrdinal(this.Dpobarfeccreacion);
            if (!dr.IsDBNull(iDpobarfeccreacion)) entity.Dpobarfeccreacion = dr.GetDateTime(iDpobarfeccreacion);

            return entity;
        }


        #region Mapeo de Campos
        public string Dpobarcodi = "DPOBARCODI";
        public string Dpobarcodiexcel = "DPOBARCODIEXCEL";
        public string Dpobarnombre = "DPOBARNOMBRE";
        public string Dpobartension = "DPOBARTENSION";
        public string Dpobarusucreacion = "DPOBARUSUCREACION";
        public string Dpobarfeccreacion = "DPOBARFECCREACION";
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
