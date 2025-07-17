using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_CATEGORIA
    /// </summary>
    public class PrCategoriaHelper : HelperBase
    {
        public PrCategoriaHelper(): base(Consultas.PrCategoriaSql)
        {
        }

        public PrCategoriaDTO Create(IDataReader dr)
        {
            PrCategoriaDTO entity = new PrCategoriaDTO();

            int iCatecodi = dr.GetOrdinal(this.Catecodi);
            if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

            int iCateabrev = dr.GetOrdinal(this.Cateabrev);
            if (!dr.IsDBNull(iCateabrev)) entity.Cateabrev = dr.GetString(iCateabrev);

            int iCatenomb = dr.GetOrdinal(this.Catenomb);
            if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

            int iCatetipo = dr.GetOrdinal(this.Catetipo);
            if (!dr.IsDBNull(iCatetipo)) entity.Catetipo = dr.GetString(iCatetipo);

            return entity;
        }


        #region Mapeo de Campos

        public string Catecodi = "CATECODI";
        public string Cateabrev = "CATEABREV";
        public string Catenomb = "CATENOMB";
        public string Catetipo = "CATETIPO";

        #endregion

        public string SqlListByOriglectcodiYEmprcodi
        {
            get { return base.GetSqlXml("ListByOriglectcodiYEmprcodi"); }
        }
    }
}
