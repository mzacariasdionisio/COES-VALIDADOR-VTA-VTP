using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_FICTEC_VISUALENTIDAD
    /// </summary>
    public class FtFictecVisualentidadHelper : HelperBase
    {
        public FtFictecVisualentidadHelper() : base(Consultas.FtFictecVisualentidadSql)
        {
        }

        public FtFictecVisualentidadDTO Create(IDataReader dr)
        {
            FtFictecVisualentidadDTO entity = new FtFictecVisualentidadDTO();

            int iFtvercodi = dr.GetOrdinal(this.Ftvercodi);
            if (!dr.IsDBNull(iFtvercodi)) entity.Ftvercodi = Convert.ToInt32(dr.GetValue(iFtvercodi));

            int iFtverusucreacion = dr.GetOrdinal(this.Ftverusucreacion);
            if (!dr.IsDBNull(iFtverusucreacion)) entity.Ftverusucreacion = dr.GetString(iFtverusucreacion);

            int iFtverocultoportal = dr.GetOrdinal(this.Ftverocultoportal);
            if (!dr.IsDBNull(iFtverocultoportal)) entity.Ftverocultoportal = dr.GetString(iFtverocultoportal);

            int iFtverfecmodificacion = dr.GetOrdinal(this.Ftverfecmodificacion);
            if (!dr.IsDBNull(iFtverfecmodificacion)) entity.Ftverfecmodificacion = dr.GetDateTime(iFtverfecmodificacion);

            int iFtverfeccreacion = dr.GetOrdinal(this.Ftverfeccreacion);
            if (!dr.IsDBNull(iFtverfeccreacion)) entity.Ftverfeccreacion = dr.GetDateTime(iFtverfeccreacion);

            int iFtverusumodificacion = dr.GetOrdinal(this.Ftverusumodificacion);
            if (!dr.IsDBNull(iFtverusumodificacion)) entity.Ftverusumodificacion = dr.GetString(iFtverusumodificacion);

            int iFteqcodi = dr.GetOrdinal(this.Fteqcodi);
            if (!dr.IsDBNull(iFteqcodi)) entity.Fteqcodi = Convert.ToInt32(dr.GetValue(iFteqcodi));

            int iFtvercodisicoes = dr.GetOrdinal(this.Ftvercodisicoes);
            if (!dr.IsDBNull(iFtvercodisicoes)) entity.Ftvercodisicoes = Convert.ToInt32(dr.GetValue(iFtvercodisicoes));

            int iFtvertipoentidad = dr.GetOrdinal(this.Ftvertipoentidad);
            if (!dr.IsDBNull(iFtvertipoentidad)) entity.Ftvertipoentidad = dr.GetString(iFtvertipoentidad);

            int iFtverocultoextranet = dr.GetOrdinal(this.Ftverocultoextranet);
            if (!dr.IsDBNull(iFtverocultoextranet)) entity.Ftverocultoextranet = dr.GetString(iFtverocultoextranet);

            int iFtverocultointranet = dr.GetOrdinal(this.Ftverocultointranet);
            if (!dr.IsDBNull(iFtverocultointranet)) entity.Ftverocultointranet = dr.GetString(iFtverocultointranet);

            return entity;
        }


        #region Mapeo de Campos

        public string Ftvercodi = "FTVERCODI";
        public string Ftverusucreacion = "FTVERUSUCREACION";
        public string Ftverocultoportal = "FTVEROCULTOPORTAL";
        public string Ftverfecmodificacion = "FTVERFECMODIFICACION";
        public string Ftverfeccreacion = "FTVERFECCREACION";
        public string Ftverusumodificacion = "FTVERUSUMODIFICACION";
        public string Fteqcodi = "FTEQCODI";
        public string Ftvercodisicoes = "FTVERCODISICOES";
        public string Ftvertipoentidad = "FTVERTIPOENTIDAD";
        public string Ftverocultoextranet = "FTVEROCULTOEXTRANET";
        public string Ftverocultointranet = "FTVEROCULTOINTRANET";

        #endregion
    }
}
