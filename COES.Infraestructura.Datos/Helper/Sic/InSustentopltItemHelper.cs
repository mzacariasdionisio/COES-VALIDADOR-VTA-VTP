using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_SUSTENTOPLT_ITEM
    /// </summary>
    public class InSustentopltItemHelper : HelperBase
    {
        public InSustentopltItemHelper() : base(Consultas.InSustentopltItemSql)
        {
        }

        public InSustentopltItemDTO Create(IDataReader dr)
        {
            InSustentopltItemDTO entity = new InSustentopltItemDTO();

            int iInpsticodi = dr.GetOrdinal(this.Inpsticodi);
            if (!dr.IsDBNull(iInpsticodi)) entity.Inpsticodi = Convert.ToInt32(dr.GetValue(iInpsticodi));

            int iInpstidesc = dr.GetOrdinal(this.Inpstidesc);
            if (!dr.IsDBNull(iInpstidesc)) entity.Inpstidesc = dr.GetString(iInpstidesc);

            int iInpstcodi = dr.GetOrdinal(this.Inpstcodi);
            if (!dr.IsDBNull(iInpstcodi)) entity.Inpstcodi = Convert.ToInt32(dr.GetValue(iInpstcodi));

            int iInpstiorden = dr.GetOrdinal(this.Inpstiorden);
            if (!dr.IsDBNull(iInpstiorden)) entity.Inpstiorden = Convert.ToInt32(dr.GetValue(iInpstiorden));

            int iInpstitipo = dr.GetOrdinal(this.Inpstitipo);
            if (!dr.IsDBNull(iInpstitipo)) entity.Inpstitipo = Convert.ToInt32(dr.GetValue(iInpstitipo));

            return entity;
        }


        #region Mapeo de Campos

        public string Inpsticodi = "INPSTICODI";
        public string Inpstidesc = "INPSTIDESC";
        public string Inpstcodi = "INPSTCODI";
        public string Inpstiorden = "INPSTIORDEN";
        public string Inpstitipo = "INPSTITIPO";

        #endregion

        public string SqlUpdateOrden
        {
            get { return GetSqlXml("UpdateOrden"); }
        }
    }
}
