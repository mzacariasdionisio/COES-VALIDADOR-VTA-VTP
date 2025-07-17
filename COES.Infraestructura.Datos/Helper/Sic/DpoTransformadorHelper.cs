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
    public class DpoTransformadorHelper : HelperBase
    {
        public DpoTransformadorHelper() : base(Consultas.DpoTransformadorSql)
        {
        }

        public DpoTransformadorDTO Create(IDataReader dr)
        {
            DpoTransformadorDTO entity = new DpoTransformadorDTO();

            int iDpotnfcodi = dr.GetOrdinal(this.Dpotnfcodi);
            if (!dr.IsDBNull(iDpotnfcodi)) entity.Dpotnfcodi = Convert.ToInt32(dr.GetValue(iDpotnfcodi));

            int iDpotnfcodiexcel = dr.GetOrdinal(this.Dpotnfcodiexcel);
            if (!dr.IsDBNull(iDpotnfcodiexcel)) entity.Dpotnfcodiexcel = dr.GetString(iDpotnfcodiexcel);

            int iDposubnombre = dr.GetOrdinal(this.Dposubnombre);
            if (!dr.IsDBNull(iDposubnombre)) entity.Dposubnombre = dr.GetString(iDposubnombre);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iDpotnfusucreacion = dr.GetOrdinal(this.Dpotnfusucreacion);
            if (!dr.IsDBNull(iDpotnfusucreacion)) entity.Dpotnfusucreacion = dr.GetString(iDpotnfusucreacion);

            int iDpotnffeccreacion = dr.GetOrdinal(this.Dpotnffeccreacion);
            if (!dr.IsDBNull(iDpotnffeccreacion)) entity.Dpotnffeccreacion = dr.GetDateTime(iDpotnffeccreacion);

            return entity;
        }


        #region Mapeo de Campos
        public string Dpotnfcodi = "DPOTNFCODI";
        public string Dpotnfcodiexcel = "DPOTNFCODIEXCEL";
        public string Dposubnombre = "DPOSUBNOMBRE";
        public string Emprnomb = "EMPRNOMB";
        public string Dpotnfusucreacion = "DPOTNFUSUCREACION";
        public string Dpotnffeccreacion = "DPOTNFFECCREACION";
        public string Dpobarcodiexcel = "DPOBARCODIEXCEL";
        public string Dpobarnombre = "DPOBARNOMBRE";
        public string Dpobartension = "DPOBARTENSION";
        //Adicional
        public string Dposubcodi= "DPOSUBCODI";
        public string Dposubcodiexcel = "DPOSUBCODIEXCEL";
        #endregion

        public string SqlListTransformadorBySubestacion
        {
            get { return GetSqlXml("ListTransformadorBySubestacion"); }
        }
        public string SqlListTransformadorByList
        {
            get { return GetSqlXml("ListTransformadorByList"); }
        }
        public string SqlListTransformadorByListExcel
        {
            get { return GetSqlXml("ListTransformadorByListExcel"); }
        }
        public string SqlUpdateTransformadoresSirpit
        {
            get { return GetSqlXml("UpdateTransformadoresSirpit"); }
        }
    }
}
