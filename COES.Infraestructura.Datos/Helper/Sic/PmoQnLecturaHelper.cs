using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PMO_QN_LECTURA
    /// </summary>
    public class PmoQnLecturaHelper : HelperBase
    {
        public PmoQnLecturaHelper() : base(Consultas.PmoQnLecturaSql)
        {
        }

        public PmoQnLecturaDTO Create(IDataReader dr)
        {
            PmoQnLecturaDTO entity = new PmoQnLecturaDTO();

            int iQnlectcodi = dr.GetOrdinal(this.Qnlectcodi);
            if (!dr.IsDBNull(iQnlectcodi)) entity.Qnlectcodi = Convert.ToInt32(dr.GetValue(iQnlectcodi));

            int iQnlectnomb = dr.GetOrdinal(this.Qnlectnomb);
            if (!dr.IsDBNull(iQnlectnomb)) entity.Qnlectnomb = dr.GetString(iQnlectnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Qnlectcodi = "QNLECTCODI";
        public string Qnlectnomb = "QNLECTNOMB";

        #endregion
    }
}
