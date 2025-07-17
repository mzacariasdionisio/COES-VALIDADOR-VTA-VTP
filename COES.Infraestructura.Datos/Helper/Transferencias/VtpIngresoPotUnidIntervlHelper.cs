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
    public class VtpIngresoPotUnidIntervlHelper : HelperBase
    {
        public VtpIngresoPotUnidIntervlHelper() : base(Consultas.VtpIngresoPotUnidIntervlSql)
        {
        }

        public VtpIngresoPotUnidIntervlDTO Create(IDataReader dr)
        {
            VtpIngresoPotUnidIntervlDTO entity = new VtpIngresoPotUnidIntervlDTO();

            int iInpuincodi = dr.GetOrdinal(this.Inpuincodi);
            if (!dr.IsDBNull(iInpuincodi)) entity.Inpuincodi = Convert.ToInt32(dr.GetValue(iInpuincodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iIpefrcodi = dr.GetOrdinal(this.Ipefrcodi);
            if (!dr.IsDBNull(iIpefrcodi)) entity.Ipefrcodi = Convert.ToInt32(dr.GetValue(iIpefrcodi));

            int iInpuinintervalo = dr.GetOrdinal(this.Inpuinintervalo);
            if (!dr.IsDBNull(iInpuinintervalo)) entity.Inpuinintervalo = Convert.ToInt32(dr.GetValue(iInpuinintervalo));

            int iInpuindia = dr.GetOrdinal(this.Inpuindia);
            if (!dr.IsDBNull(iInpuindia)) entity.Inpuindia = Convert.ToInt32(dr.GetValue(iInpuindia));

            int iInpuinimporte = dr.GetOrdinal(this.Inpuinimporte);
            if (!dr.IsDBNull(iInpuinimporte)) entity.Inpuinimporte = dr.GetDecimal(iInpuinimporte);

            int iInpuinusucreacion = dr.GetOrdinal(this.Inpuinusucreacion);
            if (!dr.IsDBNull(iInpuinusucreacion)) entity.Inpuinusucreacion = dr.GetString(iInpuinusucreacion);

            int iInpuinfeccreacion = dr.GetOrdinal(this.Inpuinfeccreacion);
            if (!dr.IsDBNull(iInpuinfeccreacion)) entity.Inpuinfeccreacion = dr.GetDateTime(iInpuinfeccreacion);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iInpuinunidadnomb = dr.GetOrdinal(this.Inpuinunidadnomb);
            if (!dr.IsDBNull(iInpuinunidadnomb)) entity.Inpuinunidadnomb = dr.GetString(iInpuinunidadnomb);

            int iInpuinficticio = dr.GetOrdinal(this.Inpuinficticio);
            if (!dr.IsDBNull(iInpuinficticio)) entity.Inpuinficticio = Convert.ToInt32(dr.GetValue(iInpuinficticio));

            return entity;
        }
        
        #region Mapeo de Campos

        public string Inpuincodi = "INPUINCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Ipefrcodi = "IPEFRCODI";
        public string Inpuinintervalo = "INPUININTERVALO";
        public string Inpuindia = "INPUINDIA";
        public string Inpuinimporte = "INPUINIMPORTE";
        public string Inpuinusucreacion = "INPUINUSUCREACION";
        public string Inpuinfeccreacion = "INPUINFECCREACION";
        public string Grupocodi = "GRUPOCODI";
        public string Inpuinunidadnomb = "INPUINUNIDADNOMB";
        public string Inpuinficticio = "INPUINFICTICIO";
        //MAPEA ATRIBUTOS ADIONALES USADOS EN VISTAS Y CONSULTAS
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";

        #endregion

        public string SqlListSumIntervl
        {
            get { return base.GetSqlXml("ListSumIntervl"); }
        }

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListSumIntervlEmpresa 
        {
            get { return base.GetSqlXml("ListSumIntervlEmpresa"); }
        }
    }
}
