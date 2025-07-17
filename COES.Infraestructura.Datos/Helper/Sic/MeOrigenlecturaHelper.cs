using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_ORIGENLECTURA
    /// </summary>
    public class MeOrigenlecturaHelper : HelperBase
    {
        public MeOrigenlecturaHelper() : base(Consultas.MeOrigenlecturaSql)
        {
        }

        public MeOrigenlecturaDTO Create(IDataReader dr)
        {
            MeOrigenlecturaDTO entity = new MeOrigenlecturaDTO();

            int iOriglectnombre = dr.GetOrdinal(this.Origlectnombre);
            if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

            int iOriglectcodi = dr.GetOrdinal(this.Origlectcodi);
            if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Origlectnombre = "ORIGLECTNOMBRE";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";

        #endregion

        #region Mapeo de Querys

        public string SqlListByEmprcodi
        {
            get { return base.GetSqlXml("ListByEmprcodi"); }
        }

        #endregion
    }
}
