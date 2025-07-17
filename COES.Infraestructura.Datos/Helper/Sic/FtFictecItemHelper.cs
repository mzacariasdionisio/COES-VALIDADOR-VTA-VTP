using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_FICTECITEM
    /// </summary>
    public class FtFictecItemHelper : HelperBase
    {
        public FtFictecItemHelper()
            : base(Consultas.FtFictecItemSql)
        {
        }

        public FtFictecItemDTO Create(IDataReader dr)
        {
            FtFictecItemDTO entity = new FtFictecItemDTO();

            int iFtitcodi = dr.GetOrdinal(this.Ftitcodi);
            if (!dr.IsDBNull(iFtitcodi)) entity.Ftitcodi = Convert.ToInt32(dr.GetValue(iFtitcodi));

            int iFteqcodi = dr.GetOrdinal(this.Fteqcodi);
            if (!dr.IsDBNull(iFteqcodi)) entity.Fteqcodi = Convert.ToInt32(dr.GetValue(iFteqcodi));

            int iPropcodi = dr.GetOrdinal(this.Propcodi);
            if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

            int iConcepcodi = dr.GetOrdinal(this.Concepcodi);
            if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

            int iFtpropcodi = dr.GetOrdinal(this.Ftpropcodi);
            if (!dr.IsDBNull(iFtpropcodi)) entity.Ftpropcodi = Convert.ToInt32(dr.GetValue(iFtpropcodi));

            int iFtitorden = dr.GetOrdinal(this.Ftitorden);
            if (!dr.IsDBNull(iFtitorden)) entity.Ftitorden = Convert.ToInt32(dr.GetValue(iFtitorden));

            int iFtitusucreacion = dr.GetOrdinal(this.Ftitusucreacion);
            if (!dr.IsDBNull(iFtitusucreacion)) entity.Ftitusucreacion = dr.GetString(iFtitusucreacion);

            int iFtitusumodificacion = dr.GetOrdinal(this.Ftitusumodificacion);
            if (!dr.IsDBNull(iFtitusumodificacion)) entity.Ftitusumodificacion = dr.GetString(iFtitusumodificacion);

            int iFtitfecmodificacion = dr.GetOrdinal(this.Ftitfecmodificacion);
            if (!dr.IsDBNull(iFtitfecmodificacion)) entity.Ftitfecmodificacion = dr.GetDateTime(iFtitfecmodificacion);

            int iFtitactivo = dr.GetOrdinal(this.Ftitactivo);
            if (!dr.IsDBNull(iFtitactivo)) entity.Ftitactivo = Convert.ToInt32(dr.GetValue(iFtitactivo));

            int iFtitnombre = dr.GetOrdinal(this.Ftitnombre);
            if (!dr.IsDBNull(iFtitnombre)) entity.Ftitnombre = dr.GetString(iFtitnombre);

            int iFtitdet = dr.GetOrdinal(this.Ftitdet);
            if (!dr.IsDBNull(iFtitdet)) entity.Ftitdet = Convert.ToInt32(dr.GetValue(iFtitdet));

            int iFtitfeccreacion = dr.GetOrdinal(this.Ftitfeccreacion);
            if (!dr.IsDBNull(iFtitfeccreacion)) entity.Ftitfeccreacion = dr.GetDateTime(iFtitfeccreacion);

            int iFtitpadre = dr.GetOrdinal(this.Ftitpadre);
            if (!dr.IsDBNull(iFtitpadre)) entity.Ftitpadre = Convert.ToInt32(dr.GetValue(iFtitpadre));

            int iFtitorientacion = dr.GetOrdinal(this.Ftitorientacion);
            if (!dr.IsDBNull(iFtitorientacion)) entity.Ftitorientacion = dr.GetString(iFtitorientacion);

            int iFtittipo = dr.GetOrdinal(this.Ftittipoitem);
            if (!dr.IsDBNull(iFtittipo)) entity.Ftittipoitem = Convert.ToInt32(dr.GetValue(iFtittipo));

            int iFtittipoprop = dr.GetOrdinal(this.Ftittipoprop);
            if (!dr.IsDBNull(iFtittipoprop)) entity.Ftittipoprop = Convert.ToInt32(dr.GetValue(iFtittipoprop));

            return entity;
        }

        #region Mapeo de Campos

        public string Ftitcodi = "FTITCODI";
        public string Fteqcodi = "FTEQCODI";
        public string Propcodi = "PROPCODI";
        public string Concepcodi = "CONCEPCODI";
        public string Ftpropcodi = "FTPROPCODI";
        public string Ftitorden = "FTITORDEN";
        public string Ftitusucreacion = "FTITUSUCREACION";
        public string Ftitusumodificacion = "FTITUSUMODIFICACION";
        public string Ftitfecmodificacion = "FTITFECMODIFICACION";
        public string Ftitactivo = "FTITACTIVO";
        public string Ftitnombre = "FTITNOMBRE";
        public string Ftitdet = "FTITDET";
        public string Ftitfeccreacion = "FTITFECCREACION";
        public string Ftitpadre = "FTITPADRE";
        public string Ftitorientacion = "FTITORIENTACION";
        public string Ftittipoitem = "FTITTIPOITEM";
        public string Ftittipoprop = "FTITTIPOPROP";

        public string Propnomb = "PROPNOMB";
        public string Propunidad = "PROPUNIDAD";
        public string Proptipo = "PROPTIPO";
        public string Propfile = "PROPFILE";
        public string Concepdesc = "CONCEPDESC";
        public string Concepunid = "CONCEPUNID";
        public string Conceptipo = "CONCEPTIPO";
        public string Ftproptipo = "FTPROPTIPO";
        public string Ftpropunidad = "FTPROPUNIDAD";
        public string Ftpropnomb = "FTPROPNOMB";

        public string Concepflagcolor = "CONCEPFLAGCOLOR";
        public string Propflagcolor = "PROPFLAGCOLOR";

        #endregion

        #region Mapeo de Consultas

        public string SqlListarItemsByFichaTecnica
        {
            get { return base.GetSqlXml("ListarItemsByFichaTecnica"); }
        }

        public string SqlListarPorIds
        {
            get { return base.GetSqlXml("ListarPorIds"); }
        }
        

        #endregion
    }
}
