using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_PLANTILLACORREO
    /// </summary>
    public class SiPlantillacorreoHelper : HelperBase
    {
        public SiPlantillacorreoHelper(): base(Consultas.SiPlantillacorreoSql)
        {
        }

        public SiPlantillacorreoDTO Create(IDataReader dr)
        {
            SiPlantillacorreoDTO entity = new SiPlantillacorreoDTO();

            int iPlantcodi = dr.GetOrdinal(this.Plantcodi);
            if (!dr.IsDBNull(iPlantcodi)) entity.Plantcodi = Convert.ToInt32(dr.GetValue(iPlantcodi));

            int iPlantcontenido = dr.GetOrdinal(this.Plantcontenido);
            if (!dr.IsDBNull(iPlantcontenido)) entity.Plantcontenido = dr.GetString(iPlantcontenido);

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iTpcorrcodi = dr.GetOrdinal(this.Tpcorrcodi);
            if (!dr.IsDBNull(iTpcorrcodi)) entity.Tpcorrcodi = Convert.ToInt32(dr.GetValue(iTpcorrcodi));

            int iPlantasunto = dr.GetOrdinal(this.Plantasunto);
            if (!dr.IsDBNull(iPlantasunto)) entity.Plantasunto = dr.GetString(iPlantasunto);

            int iPlantnomb = dr.GetOrdinal(this.Plantnomb);
            if (!dr.IsDBNull(iPlantnomb)) entity.Plantnomb = dr.GetString(iPlantnomb);

            int iPlantindhtml = dr.GetOrdinal(this.Plantindhtml);
            if (!dr.IsDBNull(iPlantindhtml)) entity.Plantindhtml = dr.GetString(iPlantindhtml);

            int iPlantindadjunto = dr.GetOrdinal(this.Plantindadjunto);
            if (!dr.IsDBNull(iPlantindadjunto)) entity.Plantindadjunto = dr.GetString(iPlantindadjunto);

            int iPlanticorreos = dr.GetOrdinal(this.Planticorreos);
            if (!dr.IsDBNull(iPlanticorreos)) entity.Planticorreos = dr.GetString(iPlanticorreos);

            int iPlanticorreosCc = dr.GetOrdinal(this.PlanticorreosCc);
            if (!dr.IsDBNull(iPlanticorreosCc)) entity.PlanticorreosCc = dr.GetString(iPlanticorreosCc);

            int iPlanticorreosBcc = dr.GetOrdinal(this.PlanticorreosBcc);
            if (!dr.IsDBNull(iPlanticorreosBcc)) entity.PlanticorreosBcc = dr.GetString(iPlanticorreosBcc);

            int iPlanticorreoFrom = dr.GetOrdinal(this.PlanticorreoFrom);
            if (!dr.IsDBNull(iPlanticorreoFrom)) entity.PlanticorreoFrom = dr.GetString(iPlanticorreoFrom);

            int iPlantlinkadjunto = dr.GetOrdinal(this.Plantlinkadjunto);
            if (!dr.IsDBNull(iPlantlinkadjunto)) entity.Plantlinkadjunto = dr.GetString(iPlantlinkadjunto);

            int iPlantfeccreacion = dr.GetOrdinal(this.Plantfeccreacion);
            if (!dr.IsDBNull(iPlantfeccreacion)) entity.Plantfeccreacion = dr.GetDateTime(iPlantfeccreacion);

            int iPlantfecmodificacion = dr.GetOrdinal(this.Plantfecmodificacion);
            if (!dr.IsDBNull(iPlantfecmodificacion)) entity.Plantfecmodificacion = dr.GetDateTime(iPlantfecmodificacion);

            int iPlantusucreacion= dr.GetOrdinal(this.Plantusucreacion);
            if (!dr.IsDBNull(iPlantusucreacion)) entity.Plantusucreacion = dr.GetString(iPlantusucreacion);

            int iPlantusumodificacion = dr.GetOrdinal(this.Plantusumodificacion);
            if (!dr.IsDBNull(iPlantusumodificacion)) entity.Plantusumodificacion = dr.GetString(iPlantusumodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Plantcodi = "PLANTCODI";
        public string Plantcontenido = "PLANTCONTENIDO";
        public string Modcodi = "MODCODI";
        public string Tpcorrcodi = "TPCORRCODI";
        public string Plantasunto = "PLANTASUNTO";
        public string Plantnomb = "PLANTNOMB";
        public string Plantindhtml = "PLANTINDHTML";
        public string Plantindadjunto = "PLANTINDADJUNTO";
        public string Planticorreos = "PLANTICORREOS";
        public string PlanticorreosCc = "PLANTICORREOSCC";
        public string PlanticorreosBcc = "PLANTICORREOSBCC";
        public string PlanticorreoFrom = "PLANTICORREOFROM";
        public string Plantlinkadjunto = "PLANTLINKADJUNTO";
        public string Plantfeccreacion = "PLANTFECCREACION";
        public string Plantfecmodificacion = "PLANTFECMODIFICACION";
        public string Plantusucreacion = "PLANTUSUCREACION";
        public string Plantusumodificacion = "PLANTUSUMODIFICACION";

        public string SqlObtenerPlantillaPorModulo
        {
            get { return base.GetSqlXml("ObtenerPlantillaPorModulo"); }
        }

        public string SqlActualizarPlantilla
        {
            get { return base.GetSqlXml("ActualizarPlantilla"); }
        }

        public string SqlListarPlantillas
        {
            get { return base.GetSqlXml("ListarPlantillas");  }
        }

        #endregion
    }
}
