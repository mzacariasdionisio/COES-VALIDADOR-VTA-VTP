using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ABI_FACTORPLANTA
    /// </summary>
    public class AbiFactorplantaHelper : HelperBase
    {
        public AbiFactorplantaHelper() : base(Consultas.AbiFactorplantaSql)
        {
        }

        public AbiFactorplantaDTO Create(IDataReader dr)
        {
            AbiFactorplantaDTO entity = new AbiFactorplantaDTO();

            int iFpfecmodificacion = dr.GetOrdinal(this.Fpfecmodificacion);
            if (!dr.IsDBNull(iFpfecmodificacion)) entity.Fpfecmodificacion = dr.GetDateTime(iFpfecmodificacion);

            int iFpusumodificacion = dr.GetOrdinal(this.Fpusumodificacion);
            if (!dr.IsDBNull(iFpusumodificacion)) entity.Fpusumodificacion = dr.GetString(iFpusumodificacion);

            int iFptipogenerrer = dr.GetOrdinal(this.Fptipogenerrer);
            if (!dr.IsDBNull(iFptipogenerrer)) entity.Fptipogenerrer = dr.GetString(iFptipogenerrer);

            int iFpintegrante = dr.GetOrdinal(this.Fpintegrante);
            if (!dr.IsDBNull(iFpintegrante)) entity.Fpintegrante = dr.GetString(iFpintegrante);

            int iFpenergia = dr.GetOrdinal(this.Fpenergia);
            if (!dr.IsDBNull(iFpenergia)) entity.Fpenergia = dr.GetDecimal(iFpenergia);

            int iFppe = dr.GetOrdinal(this.Fppe);
            if (!dr.IsDBNull(iFppe)) entity.Fppe = dr.GetDecimal(iFppe);

            int iFpvalormes = dr.GetOrdinal(this.Fpvalormes);
            if (!dr.IsDBNull(iFpvalormes)) entity.Fpvalormes = dr.GetDecimal(iFpvalormes);

            int iFpvalor = dr.GetOrdinal(this.Fpvalor);
            if (!dr.IsDBNull(iFpvalor)) entity.Fpvalor = dr.GetDecimal(iFpvalor);

            int iFpfechaperiodo = dr.GetOrdinal(this.Fpfechaperiodo);
            if (!dr.IsDBNull(iFpfechaperiodo)) entity.Fpfechaperiodo = dr.GetDateTime(iFpfechaperiodo);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iTgenercodi = dr.GetOrdinal(this.Tgenercodi);
            if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

            int iFpcodi = dr.GetOrdinal(this.Fpcodi);
            if (!dr.IsDBNull(iFpcodi)) entity.Fpcodi = Convert.ToInt32(dr.GetValue(iFpcodi));

            return entity;
        }

        #region Mapeo de Campos

        public string Fpfecmodificacion = "FPFECMODIFICACION";
        public string Fpusumodificacion = "FPUSUMODIFICACION";
        public string Fptipogenerrer = "FPTIPOGENERRER";
        public string Fpintegrante = "FPINTEGRANTE";
        public string Fpenergia = "FPENERGIA";
        public string Fppe = "FPPE";
        public string Fpvalormes = "FPVALORMES";
        public string Fpvalor = "FPVALOR";
        public string Fpfechaperiodo = "FPFECHAPERIODO";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Tgenercodi = "TGENERCODI";
        public string Fpcodi = "FPCODI";

        #endregion

        public string SqlDeleteByMes
        {
            get { return base.GetSqlXml("DeleteByMes"); }
        }
    }
}
