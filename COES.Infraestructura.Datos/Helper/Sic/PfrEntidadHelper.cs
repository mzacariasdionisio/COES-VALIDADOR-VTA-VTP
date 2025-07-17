using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PFR_ENTIDAD
    /// </summary>
    public class PfrEntidadHelper : HelperBase
    {
        public PfrEntidadHelper(): base(Consultas.PfrEntidadSql)
        {
        }

        public PfrEntidadDTO Create(IDataReader dr)
        {
            PfrEntidadDTO entity = new PfrEntidadDTO();

            int iPfrentcodi = dr.GetOrdinal(this.Pfrentcodi);
            if (!dr.IsDBNull(iPfrentcodi)) entity.Pfrentcodi = Convert.ToInt32(dr.GetValue(iPfrentcodi));

            int iPfrentnomb = dr.GetOrdinal(this.Pfrentnomb);
            if (!dr.IsDBNull(iPfrentnomb)) entity.Pfrentnomb = dr.GetString(iPfrentnomb);

            int iPfrentid = dr.GetOrdinal(this.Pfrentid);
            if (!dr.IsDBNull(iPfrentid)) entity.Pfrentid = dr.GetString(iPfrentid);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iPfrentcodibarragams = dr.GetOrdinal(this.Pfrentcodibarragams);
            if (!dr.IsDBNull(iPfrentcodibarragams)) entity.Pfrentcodibarragams = Convert.ToInt32(dr.GetValue(iPfrentcodibarragams));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iEquipadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

            int iPfrcatcodi = dr.GetOrdinal(this.Pfrcatcodi);
            if (!dr.IsDBNull(iPfrcatcodi)) entity.Pfrcatcodi = Convert.ToInt32(dr.GetValue(iPfrcatcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iPfrentfeccreacion = dr.GetOrdinal(this.Pfrentfeccreacion);
            if (!dr.IsDBNull(iPfrentfeccreacion)) entity.Pfrentfeccreacion = dr.GetDateTime(iPfrentfeccreacion);

            int iPfrentcodibarragams2 = dr.GetOrdinal(this.Pfrentcodibarragams2);
            if (!dr.IsDBNull(iPfrentcodibarragams2)) entity.Pfrentcodibarragams2 = Convert.ToInt32(dr.GetValue(iPfrentcodibarragams2));

            int iPfrentestado = dr.GetOrdinal(this.Pfrentestado);
            if (!dr.IsDBNull(iPfrentestado)) entity.Pfrentestado = Convert.ToInt32(dr.GetValue(iPfrentestado));

            int iPfrentfecmodificacion = dr.GetOrdinal(this.Pfrentfecmodificacion);
            if (!dr.IsDBNull(iPfrentfecmodificacion)) entity.Pfrentfecmodificacion = dr.GetDateTime(iPfrentfecmodificacion);

            int iPfrentusucreacion = dr.GetOrdinal(this.Pfrentusucreacion);
            if (!dr.IsDBNull(iPfrentusucreacion)) entity.Pfrentusucreacion = dr.GetString(iPfrentusucreacion);

            int iPfrentusumodificacion = dr.GetOrdinal(this.Pfrentusumodificacion);
            if (!dr.IsDBNull(iPfrentusumodificacion)) entity.Pfrentusumodificacion = dr.GetString(iPfrentusumodificacion);

            int iPfrentficticio = dr.GetOrdinal(this.Pfrentficticio);
            if (!dr.IsDBNull(iPfrentficticio)) entity.Pfrentficticio = Convert.ToInt32(dr.GetValue(iPfrentficticio));

            int iPfrentunidadnomb = dr.GetOrdinal(this.Pfrentunidadnomb);
            if (!dr.IsDBNull(iPfrentunidadnomb)) entity.Pfrentunidadnomb = dr.GetString(iPfrentunidadnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Pfrentcodi = "PFRENTCODI";
        public string Pfrentnomb = "PFRENTNOMB";
        public string Pfrentid = "PFRENTID";
        public string Grupocodi = "GRUPOCODI";
        public string Pfrentcodibarragams = "PFRENTCODIBARRAGAMS";
        public string Barrcodi = "BARRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Pfrcatcodi = "PFRCATCODI";
        public string Equicodi = "EQUICODI";
        public string Pfrentfeccreacion = "PFRENTFECCREACION";
        public string Pfrentcodibarragams2 = "PFRENTCODIBARRAGAMS2";
        public string Pfrentestado = "PFRENTESTADO";
        public string Pfrentfecmodificacion = "PFRENTFECMODIFICACION";
        public string Pfrentusucreacion = "PFRENTUSUCREACION";
        public string Pfrentusumodificacion = "PFRENTUSUMODIFICACION";
        public string Pfrentficticio = "PFRENTFICTICIO";
        public string Pfrentunidadnomb = "PFRENTUNIDADNOMB";

        public string Idbarra1desc = "IDBARRA1DESC";
        public string Idbarra2desc = "IDBARRA2DESC";
        public string Idbarra1 = "IDBARRA1";
        public string Idbarra2 = "IDBARRA2";
        public string Barrnombre = "BARRNOMBRE";
        public string Equinomb = "EQUINOMB";

        #endregion
    }
}
