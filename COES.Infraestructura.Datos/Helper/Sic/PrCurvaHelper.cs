using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla PR_CURVA
    /// </summary>
    public class PrCurvaHelper : HelperBase
    {
        public PrCurvaHelper()
            : base(Consultas.PrCurvaSql)
        {
        }

        public PrCurvaDTO Create(IDataReader dr)
        {
            PrCurvaDTO entity = new PrCurvaDTO();

            int iCurvCodi = dr.GetOrdinal(this.CurvCodi);
            if (!dr.IsDBNull(iCurvCodi)) entity.Curvcodi = Convert.ToInt32(dr.GetValue(iCurvCodi));

            int iCurvNombre = dr.GetOrdinal(this.CurvNombre);
            if (!dr.IsDBNull(iCurvNombre)) entity.Curvnombre = dr.GetString(iCurvNombre);

            int iCurvEstado = dr.GetOrdinal(this.CurvEstado);
            if (!dr.IsDBNull(iCurvEstado)) entity.Curvestado = dr.GetString(iCurvEstado);


            int iCurvusucreacion = dr.GetOrdinal(this.CurvUsuCreacion);
            if (!dr.IsDBNull(iCurvusucreacion)) entity.Curvusucreacion = dr.GetString(iCurvusucreacion);

            int iCurvfeccreacion = dr.GetOrdinal(this.CurvFecCreacion);
            if (!dr.IsDBNull(iCurvfeccreacion)) entity.Curvfeccreacion = dr.GetDateTime(iCurvfeccreacion);

            int iCurvusumodificacion = dr.GetOrdinal(this.CurvUsuModificacion);
            if (!dr.IsDBNull(iCurvusumodificacion)) entity.Curvusumodificacion = dr.GetString(iCurvusumodificacion);

            int iCurvfecmodificacion = dr.GetOrdinal(this.CurvFecModificacion);
            if (!dr.IsDBNull(iCurvfecmodificacion)) entity.Curvfecmodificacion = dr.GetDateTime(iCurvfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string CurvCodi = "CURVCODI";
        public string CurvNombre = "CURVNOMBRE";
        public string CurvEstado = "CURVESTADO";
        public string CurvUsuCreacion = "CURVUSUCREACION";
        public string CurvFecCreacion = "CURVFECCREACION";
        public string CurvUsuModificacion = "CURVUSUMODIFICACION";
        public string CurvFecModificacion = "CURVFECMODIFICACION";

        public string GrupoCodi = "GRUPOCODI";
        #endregion

        public string sqlListCurva
        {
            get { return base.GetSqlXml("ListCurva"); }
        }

        public string SqlAddDetail
        {
            get { return base.GetSqlXml("AddDetail"); }
        }

        public string SqlDeleteDetail
        {
            get { return base.GetSqlXml("DeleteDetail"); }
        }

        public string SqlUpdatePrincipal
        {
            get { return base.GetSqlXml("UpdatePrincipal"); }
        }


    }
}
