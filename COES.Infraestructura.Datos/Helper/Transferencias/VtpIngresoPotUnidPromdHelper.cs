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
    public class VtpIngresoPotUnidPromdHelper : HelperBase
    {
        public VtpIngresoPotUnidPromdHelper() : base(Consultas.VtpIngresoPotUnidPromdSql)
        {
        }

        public VtpIngresoPotUnidPromdDTO Create(IDataReader dr)
        {
            VtpIngresoPotUnidPromdDTO entity = new VtpIngresoPotUnidPromdDTO();

            int iInpuprcodi = dr.GetOrdinal(this.Inpuprcodi);
            if (!dr.IsDBNull(iInpuprcodi)) entity.Inpuprcodi = Convert.ToInt32(dr.GetValue(iInpuprcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iInpuprimportepromd = dr.GetOrdinal(this.Inpuprimportepromd);
            if (!dr.IsDBNull(iInpuprimportepromd)) entity.Inpuprimportepromd = dr.GetDecimal(iInpuprimportepromd);

            int iInpuprusucreacion = dr.GetOrdinal(this.Inpuprusucreacion);
            if (!dr.IsDBNull(iInpuprusucreacion)) entity.Inpuprusucreacion = dr.GetString(iInpuprusucreacion);

            int iInpuprfeccreacion = dr.GetOrdinal(this.Inpuprfeccreacion);
            if (!dr.IsDBNull(iInpuprfeccreacion)) entity.Inpuprfeccreacion = dr.GetDateTime(iInpuprfeccreacion);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iInpuprunidadnomb = dr.GetOrdinal(this.Inpuprunidadnomb);
            if (!dr.IsDBNull(iInpuprunidadnomb)) entity.Inpuprunidadnomb = dr.GetString(iInpuprunidadnomb);

            int iInpuprficticio = dr.GetOrdinal(this.Inpuprficticio);
            if (!dr.IsDBNull(iInpuprficticio)) entity.Inpuprficticio = Convert.ToInt32(dr.GetValue(iInpuprficticio));

            return entity;
        }

        #region Mapeo de Campos

        public string Inpuprcodi = "INPUPRCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Inpuprimportepromd = "INPUPRIMPORTEPROMD";
        public string Inpuprusucreacion = "INPUPRUSUCREACION";
        public string Inpuprfeccreacion = "INPUPRFECCREACION";
        public string Grupocodi = "GRUPOCODI";
        public string Inpuprunidadnomb = "INPUPRUNIDADNOMB";
        public string Inpuprficticio = "INPUPRFICTICIO";
        //Para los nombres
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Recpotnombre = "RECPOTNOMBRE";
        public string Perinombre = "PERINOMBRE";
        public string Perianio = "PERIANIO";
        public string Perimes = "PERIMES";
        public string Perianiomes = "PERIANIOMES";
        public string Recanombre = "RECANOMBRE";

        public string Pericodiini = "periinicio";
        public string Pericodifin = "perifin";
        public string Recpotini = "recpotinicio";
        public string Recpotfin = "recpotfin";
        
        #endregion

        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlListEmpresa
        {
            get { return base.GetSqlXml("ListEmpresa"); }
        }

        public string SqlListEmpresaCentral
        {
            get { return base.GetSqlXml("ListEmpresaCentral"); }
        }

        public string SqlGetIngresoPotUnidPromdByComparative
        {
            get { return base.GetSqlXml("GetIngresoPotUnidPromdByComparative"); }
        }

        public string SqlGetIngresoPotUnidPromdByComparativeUnique
        {
            get { return base.GetSqlXml("GetIngresoPotUnidPromdByComparativeUnique"); }
        }

        public string SqlGetIngresoPotUnidPromdByCompHist
        {
            get { return base.GetSqlXml("GetIngresoPotUnidPromdByCompHist"); }
        }

        public string SqlGetIngresoPotUnidPromdByCompHistUnique
        {
            get { return base.GetSqlXml("GetIngresoPotUnidPromdByCompHistUnique"); }
        }

        public string SqlListEmpresaCentralUnidad
        {
            get { return base.GetSqlXml("ListEmpresaCentralUnidad"); }
        }
        //CU21
        public string SqlGetByCentral
        {
            get { return base.GetSqlXml("GetByCentral"); }
        }
        public string SqlGetByCentralSumUnidades
        {
            get { return base.GetSqlXml("GetByCentralSumUnidades"); }
        }
    }
}
