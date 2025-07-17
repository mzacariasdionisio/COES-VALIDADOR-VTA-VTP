using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_INGRESO_POTENCIA
    /// </summary>
    public class VtpIngresoPotenciaHelper : HelperBase
    {
        public VtpIngresoPotenciaHelper() : base(Consultas.VtpIngresoPotenciaSql)
        {
        }

        public VtpIngresoPotenciaDTO Create(IDataReader dr)
        {
            VtpIngresoPotenciaDTO entity = new VtpIngresoPotenciaDTO();

            int iPotipcodi = dr.GetOrdinal(this.Potipcodi);
            if (!dr.IsDBNull(iPotipcodi)) entity.Potipcodi = Convert.ToInt32(dr.GetValue(iPotipcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPotipimporte = dr.GetOrdinal(this.Potipimporte);
            if (!dr.IsDBNull(iPotipimporte)) entity.Potipimporte = dr.GetDecimal(iPotipimporte);

            int iPotipporcentaje = dr.GetOrdinal(this.Potipporcentaje);
            if (!dr.IsDBNull(iPotipporcentaje)) entity.Potipporcentaje = dr.GetDecimal(iPotipporcentaje);

            int iPotipusucreacion = dr.GetOrdinal(this.Potipusucreacion);
            if (!dr.IsDBNull(iPotipusucreacion)) entity.Potipusucreacion = dr.GetString(iPotipusucreacion);

            int iPotipfeccreacion = dr.GetOrdinal(this.Potipfeccreacion);
            if (!dr.IsDBNull(iPotipfeccreacion)) entity.Potipfeccreacion = dr.GetDateTime(iPotipfeccreacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Potipcodi = "POTIPCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Potipimporte = "POTIPIMPORTE";
        public string Potipporcentaje = "POTIPPORCENTAJE";
        public string Potipusucreacion = "POTIPUSUCREACION";
        public string Potipfeccreacion = "POTIPFECCREACION";
        public string Potipusumodificacion = "POTIPUSUMODIFICACION";
        public string Potipfecmodificacion = "POTIPFECMODIFICACION";
        //Atributos adicionales para usarlos en consultas
        public string Emprnomb = "EMPRNOMB";

        //ASSETEC 20190627: muestra si la empresa tiene un saldo asignado de otro periodo
        public string Potipsaldoanterior = "POTIPSALDOANTERIOR";

        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListEmpresa
        {
            get { return base.GetSqlXml("ListEmpresa"); }
        }
    }
}
