using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_DOCDIANOHABIL
    /// </summary>
    public class EpoDocDiaNoHabilHelper : HelperBase
    {
        public EpoDocDiaNoHabilHelper() : base(Consultas.EpoDocDiaNoHabilSql)
        {
        }

        public string SqlObtenerDiasNoHabiles
        {
            get { return base.GetSqlXml("List"); }
        }

        public EpoDocDiaNoHabilDTO Create(IDataReader dr)
        {
            EpoDocDiaNoHabilDTO entity = new EpoDocDiaNoHabilDTO();

            int iDiaNHCODI = dr.GetOrdinal(this.DiaNHCODI);
            if (!dr.IsDBNull(iDiaNHCODI)) entity.DIANHCODI = Convert.ToInt32(dr.GetValue(iDiaNHCODI));
            int iDiaNHFechaIni = dr.GetOrdinal(this.DiaNHFechaIni);
            if (!dr.IsDBNull(iDiaNHFechaIni)) entity.DIANHFECHAINI = Convert.ToDateTime(dr.GetValue(iDiaNHFechaIni));
            int iDiaNHFechaFin = dr.GetOrdinal(this.DiaNHFechaFin);
            if (!dr.IsDBNull(iDiaNHFechaFin)) entity.DIANHFECHAFIN = Convert.ToDateTime(dr.GetValue(iDiaNHFechaFin));

            return entity;
        }


        #region Mapeo de Campos
        public string DiaNHCODI = "DIANHCODI";
        public string DiaNHFechaIni = "DIANHFECHAINI";
        public string DiaNHFechaFin = "DIANHFECHAFIN";
        #endregion

    }
}
