using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_POTLIM
    /// </summary>
    public class IndPotlimHelper : HelperBase
    {
        public IndPotlimHelper(): base(Consultas.IndPotlimSql)
        {
        }

        public IndPotlimDTO Create(IDataReader dr)
        {
            IndPotlimDTO entity = new IndPotlimDTO();

            int iPotlimcodi = dr.GetOrdinal(this.Potlimcodi);
            if (!dr.IsDBNull(iPotlimcodi)) entity.Potlimcodi = Convert.ToInt32(dr.GetValue(iPotlimcodi));

            int iPotlimmw = dr.GetOrdinal(this.Potlimmw);
            if (!dr.IsDBNull(iPotlimmw)) entity.Potlimmw = dr.GetDecimal(iPotlimmw);

            int iPotlimnombre = dr.GetOrdinal(this.Potlimnombre);
            if (!dr.IsDBNull(iPotlimnombre)) entity.Potlimnombre = dr.GetString(iPotlimnombre);

            int iPotlimusucreacion = dr.GetOrdinal(this.Potlimusucreacion);
            if (!dr.IsDBNull(iPotlimusucreacion)) entity.Potlimusucreacion = dr.GetString(iPotlimusucreacion);

            int iPotlimfeccreacion = dr.GetOrdinal(this.Potlimfeccreacion);
            if (!dr.IsDBNull(iPotlimfeccreacion)) entity.Potlimfeccreacion = dr.GetDateTime(iPotlimfeccreacion);

            int iPotlimusumodificacion = dr.GetOrdinal(this.Potlimusumodificacion);
            if (!dr.IsDBNull(iPotlimusumodificacion)) entity.Potlimusumodificacion = dr.GetString(iPotlimusumodificacion);

            int iPotlimfecmodificacion = dr.GetOrdinal(this.Potlimfecmodificacion);
            if (!dr.IsDBNull(iPotlimfecmodificacion)) entity.Potlimfecmodificacion = dr.GetDateTime(iPotlimfecmodificacion);

            int iPotlimfechaini = dr.GetOrdinal(this.Potlimfechaini);
            if (!dr.IsDBNull(iPotlimfechaini)) entity.Potlimfechaini = dr.GetDateTime(iPotlimfechaini);

            int iPotlimfechafin = dr.GetOrdinal(this.Potlimfechafin);
            if (!dr.IsDBNull(iPotlimfechafin)) entity.Potlimfechafin = dr.GetDateTime(iPotlimfechafin);

            int iPotlimestado = dr.GetOrdinal(this.Potlimestado);
            if (!dr.IsDBNull(iPotlimestado)) entity.Potlimestado = Convert.ToInt32(dr.GetValue(iPotlimestado));

            return entity;
        }
        
        public IndPotlimDTO CreateMap(IDataReader dr)
        {
            IndPotlimDTO entity = new IndPotlimDTO();

            int iPotlimcodi = dr.GetOrdinal(this.Potlimcodi);
            if (!dr.IsDBNull(iPotlimcodi)) entity.Potlimcodi = Convert.ToInt32(dr.GetValue(iPotlimcodi));

            int iPotlimmw = dr.GetOrdinal(this.Potlimmw);
            if (!dr.IsDBNull(iPotlimmw)) entity.Potlimmw = dr.GetDecimal(iPotlimmw);

            int iPotlimnombre = dr.GetOrdinal(this.Potlimnombre);
            if (!dr.IsDBNull(iPotlimnombre)) entity.Potlimnombre = dr.GetString(iPotlimnombre);

            int iPotlimfechaini = dr.GetOrdinal(this.Potlimfechaini);
            if (!dr.IsDBNull(iPotlimfechaini)) entity.Potlimfechaini = dr.GetDateTime(iPotlimfechaini);

            int iPotlimfechafin = dr.GetOrdinal(this.Potlimfechafin);
            if (!dr.IsDBNull(iPotlimfechafin)) entity.Potlimfechafin = dr.GetDateTime(iPotlimfechafin);

            int iPotlimestado = dr.GetOrdinal(this.Potlimestado);
            if (!dr.IsDBNull(iPotlimestado)) entity.Potlimestado = Convert.ToInt32(dr.GetValue(iPotlimestado));  
            
            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iEquinomb2 = dr.GetOrdinal(this.Equinomb2);
            if (!dr.IsDBNull(iEquinomb2)) entity.Equinomb2 = dr.GetString(iEquinomb2);

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
            
            int iEqulimpotefectiva = dr.GetOrdinal(this.Equlimpotefectiva);
            if (!dr.IsDBNull(iEqulimpotefectiva)) entity.Equlimpotefectiva = dr.GetDecimal(iEqulimpotefectiva);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Potlimcodi = "POTLIMCODI";
        public string Potlimmw = "POTLIMMW";
        public string Potlimnombre = "POTLIMNOMBRE";
        public string Potlimusucreacion = "POTLIMUSUCREACION";
        public string Potlimfeccreacion = "POTLIMFECCREACION";
        public string Potlimusumodificacion = "POTLIMUSUMODIFICACION";
        public string Potlimfecmodificacion = "POTLIMFECMODIFICACION";
        public string Potlimfechaini = "POTLIMFECHAINI";
        public string Potlimfechafin = "POTLIMFECHAFIN";
        public string Potlimestado = "POTLIMESTADO";

        public string Equinomb = "EQUINOMB";
        public string Equinomb2 = "EQUINOMB2";
        public string Gruponomb = "GRUPONOMB";
        public string Equlimpotefectiva = "EQULIMPOTEFECTIVA";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";

        #endregion

        public string SqlUpdatePartial => GetSqlXml("UpdatePartial");
        public string SqlUpdateEstado => GetSqlXml("UpdateEstado");
    }
}
