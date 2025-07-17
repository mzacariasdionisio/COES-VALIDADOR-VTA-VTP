using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_PROVISIONBASE
    /// </summary>
    public class VcrProvisionbaseHelper : HelperBase
    {
        public VcrProvisionbaseHelper(): base(Consultas.VcrProvisionbaseSql)
        {
        }

        public VcrProvisionbaseDTO Create(IDataReader dr)
        {
            VcrProvisionbaseDTO entity = new VcrProvisionbaseDTO();

            int iVcrpbcodi = dr.GetOrdinal(this.Vcrpbcodi);
            if (!dr.IsDBNull(iVcrpbcodi)) entity.Vcrpbcodi = Convert.ToInt32(dr.GetValue(iVcrpbcodi));

            int iVcrpbperiodoini = dr.GetOrdinal(this.Vcrpbperiodoini);
            if (!dr.IsDBNull(iVcrpbperiodoini)) entity.Vcrpbperiodoini = dr.GetDateTime(iVcrpbperiodoini);

            int iVcrpbperiodofin = dr.GetOrdinal(this.Vcrpbperiodofin);
            if (!dr.IsDBNull(iVcrpbperiodofin)) entity.Vcrpbperiodofin = dr.GetDateTime(iVcrpbperiodofin);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrpbpotenciabf = dr.GetOrdinal(this.Vcrpbpotenciabf);
            if (!dr.IsDBNull(iVcrpbpotenciabf)) entity.Vcrpbpotenciabf = dr.GetDecimal(iVcrpbpotenciabf);

            int iVcrpbpreciobf = dr.GetOrdinal(this.Vcrpbpreciobf);
            if (!dr.IsDBNull(iVcrpbpreciobf)) entity.Vcrpbpreciobf = dr.GetDecimal(iVcrpbpreciobf);

            int iVcrpbusucreacion = dr.GetOrdinal(this.Vcrpbusucreacion);
            if (!dr.IsDBNull(iVcrpbusucreacion)) entity.Vcrpbusucreacion = dr.GetString(iVcrpbusucreacion);

            int iVcrpbfeccreacion = dr.GetOrdinal(this.Vcrpbfeccreacion);
            if (!dr.IsDBNull(iVcrpbfeccreacion)) entity.Vcrpbfeccreacion = dr.GetDateTime(iVcrpbfeccreacion);

            int iVcrpbusumodificacion = dr.GetOrdinal(this.Vcrpbusumodificacion);
            if (!dr.IsDBNull(iVcrpbusumodificacion)) entity.Vcrpbusumodificacion = dr.GetString(iVcrpbusumodificacion);

            int iVcrpbfecmodificacion = dr.GetOrdinal(this.Vcrpbfecmodificacion);
            if (!dr.IsDBNull(iVcrpbfecmodificacion)) entity.Vcrpbfecmodificacion = dr.GetDateTime(iVcrpbfecmodificacion);
            //ASSETEC: 202010
            int iVcrpbpotenciabfb = dr.GetOrdinal(this.Vcrpbpotenciabfb);
            if (!dr.IsDBNull(iVcrpbpotenciabfb)) entity.Vcrpbpotenciabfb = dr.GetDecimal(iVcrpbpotenciabfb);

            int iVcrpbpreciobfb = dr.GetOrdinal(this.Vcrpbpreciobfb);
            if (!dr.IsDBNull(iVcrpbpreciobfb)) entity.Vcrpbpreciobfb = dr.GetDecimal(iVcrpbpreciobfb);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrpbcodi = "VCRPBCODI";
        public string Vcrpbperiodoini = "VCRPBPERIODOINI";
        public string Vcrpbperiodofin = "VCRPBPERIODOFIN";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrpbpotenciabf = "VCRPBPOTENCIABF";
        public string Vcrpbpreciobf = "VCRPBPRECIOBF";
        public string Vcrpbusucreacion = "VCRPBUSUCREACION";
        public string Vcrpbfeccreacion = "VCRPBFECCREACION";
        public string Vcrpbusumodificacion = "VCRPBUSUMODIFICACION";
        public string Vcrpbfecmodificacion = "VCRPBFECMODIFICACION";
        public string Vcrpbpotenciabfb = "VCRPBPOTENCIABFB";
        public string Vcrpbpreciobfb = "VCRPBPRECIOBFB";

        //Variables de CONSULTA
        public string Periodo = "PERIODO";
        public string Equinomb = "EQUINOMB";

        #endregion

        public string SqlGetByIdURS
        {
            get { return base.GetSqlXml("GetByIdURS"); }
        }

        public string SqlListIndex
        {
            get { return base.GetSqlXml("ListIndex"); }
        }

        public string SqlGetByIdView
        {
            get { return base.GetSqlXml("GetByIdView"); }
        }
    }
}
