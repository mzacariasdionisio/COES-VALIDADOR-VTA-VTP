using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_NODO_CONCEPTO
    /// </summary>
    public class CpNodoConceptoHelper : HelperBase
    {
        public CpNodoConceptoHelper(): base(Consultas.CpNodoConceptoSql)
        {
        }

        public CpNodoConceptoDTO Create(IDataReader dr)
        {
            CpNodoConceptoDTO entity = new CpNodoConceptoDTO();

            int iCpnconcodi = dr.GetOrdinal(this.Cpnconcodi);
            if (!dr.IsDBNull(iCpnconcodi)) entity.Cpnconcodi = Convert.ToInt32(dr.GetValue(iCpnconcodi));

            int iCpncontipo = dr.GetOrdinal(this.Cpncontipo);
            if (!dr.IsDBNull(iCpncontipo)) entity.Cpncontipo = dr.GetString(iCpncontipo);

            int iCpnconnombre = dr.GetOrdinal(this.Cpnconnombre);
            if (!dr.IsDBNull(iCpnconnombre)) entity.Cpnconnombre = dr.GetString(iCpnconnombre);

            int iCpnconunidad = dr.GetOrdinal(this.Cpnconunidad);
            if (!dr.IsDBNull(iCpnconunidad)) entity.Cpnconunidad = dr.GetString(iCpnconunidad);

            int iCpnconestado = dr.GetOrdinal(this.Cpnconestado);
            if (!dr.IsDBNull(iCpnconestado)) entity.Cpnconestado = Convert.ToInt32(dr.GetValue(iCpnconestado));

            return entity;
        }


        #region Mapeo de Campos

        public string Cpnconcodi = "CPNCONCODI";
        public string Cpncontipo = "CPNCONTIPO";
        public string Cpnconnombre = "CPNCONNOMBRE";
        public string Cpnconunidad = "CPNCONUNIDAD";
        public string Cpnconestado = "CPNCONESTADO";

        #endregion
    }
}
