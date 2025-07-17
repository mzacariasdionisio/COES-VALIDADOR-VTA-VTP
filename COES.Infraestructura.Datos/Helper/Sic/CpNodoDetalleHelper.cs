using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_NODO_DETALLE
    /// </summary>
    public class CpNodoDetalleHelper : HelperBase
    {
        public CpNodoDetalleHelper(): base(Consultas.CpNodoDetalleSql)
        {
        }

        public CpNodoDetalleDTO Create(IDataReader dr)
        {
            CpNodoDetalleDTO entity = new CpNodoDetalleDTO();

            int iCpndetcodi = dr.GetOrdinal(this.Cpndetcodi);
            if (!dr.IsDBNull(iCpndetcodi)) entity.Cpndetcodi = Convert.ToInt32(dr.GetValue(iCpndetcodi));

            int iCpnconcodi = dr.GetOrdinal(this.Cpnconcodi);
            if (!dr.IsDBNull(iCpnconcodi)) entity.Cpnconcodi = Convert.ToInt32(dr.GetValue(iCpnconcodi));

            int iCpnodocodi = dr.GetOrdinal(this.Cpnodocodi);
            if (!dr.IsDBNull(iCpnodocodi)) entity.Cpnodocodi = Convert.ToInt32(dr.GetValue(iCpnodocodi));

            int iCpndetvalor = dr.GetOrdinal(this.Cpndetvalor);
            if (!dr.IsDBNull(iCpndetvalor)) entity.Cpndetvalor = dr.GetString(iCpndetvalor);

            return entity;
        }


        #region Mapeo de Campos

        public string Cpndetcodi = "CPNDETCODI";
        public string Cpnconcodi = "CPNCONCODI";
        public string Cpnodocodi = "CPNODOCODI";
        public string Cpndetvalor = "CPNDETVALOR";

        #endregion

        public string SqlListaPorNodo
        {
            get { return base.GetSqlXml("ListaPorNodo"); }
        }

        public string SqlListaPorArbol
        {
            get { return base.GetSqlXml("ListaPorArbol"); }
        }
    }
}
