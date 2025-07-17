using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla NR_POTENCIACONSIGNA
    /// </summary>
    public class NrPotenciaconsignaHelper : HelperBase
    {
        public NrPotenciaconsignaHelper(): base(Consultas.NrPotenciaconsignaSql)
        {
        }

        public NrPotenciaconsignaDTO Create(IDataReader dr)
        {
            NrPotenciaconsignaDTO entity = new NrPotenciaconsignaDTO();

            int iNrpccodi = dr.GetOrdinal(this.Nrpccodi);
            if (!dr.IsDBNull(iNrpccodi)) entity.Nrpccodi = Convert.ToInt32(dr.GetValue(iNrpccodi));

            int iNrsmodcodi = dr.GetOrdinal(this.Nrsmodcodi);
            if (!dr.IsDBNull(iNrsmodcodi)) entity.Nrsmodcodi = Convert.ToInt32(dr.GetValue(iNrsmodcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iNrpcfecha = dr.GetOrdinal(this.Nrpcfecha);
            if (!dr.IsDBNull(iNrpcfecha)) entity.Nrpcfecha = dr.GetDateTime(iNrpcfecha);

            int iNrpceliminado = dr.GetOrdinal(this.Nrpceliminado);
            if (!dr.IsDBNull(iNrpceliminado)) entity.Nrpceliminado = dr.GetString(iNrpceliminado);

            int iNrpcusucreacion = dr.GetOrdinal(this.Nrpcusucreacion);
            if (!dr.IsDBNull(iNrpcusucreacion)) entity.Nrpcusucreacion = dr.GetString(iNrpcusucreacion);

            int iNrpcfeccreacion = dr.GetOrdinal(this.Nrpcfeccreacion);
            if (!dr.IsDBNull(iNrpcfeccreacion)) entity.Nrpcfeccreacion = dr.GetDateTime(iNrpcfeccreacion);

            int iNrpcusumodificacion = dr.GetOrdinal(this.Nrpcusumodificacion);
            if (!dr.IsDBNull(iNrpcusumodificacion)) entity.Nrpcusumodificacion = dr.GetString(iNrpcusumodificacion);

            int iNrpcfecmodificacion = dr.GetOrdinal(this.Nrpcfecmodificacion);
            if (!dr.IsDBNull(iNrpcfecmodificacion)) entity.Nrpcfecmodificacion = dr.GetDateTime(iNrpcfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Nrpccodi = "NRPCCODI";
        public string Nrsmodcodi = "NRSMODCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Nrpcfecha = "NRPCFECHA";
        public string Nrpceliminado = "NRPCELIMINADO";
        public string Nrpcusucreacion = "NRPCUSUCREACION";
        public string Nrpcfeccreacion = "NRPCFECCREACION";
        public string Nrpcusumodificacion = "NRPCUSUMODIFICACION";
        public string Nrpcfecmodificacion = "NRPCFECMODIFICACION";
        public string Nrsmodnombre = "NRSMODNOMBRE";
        public string Gruponomb = "GRUPONOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        #endregion
    }
}
