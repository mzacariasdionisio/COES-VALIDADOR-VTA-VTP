using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class PrnVersionHelper: HelperBase
    {
        public PrnVersionHelper() : base(Consultas.PrnVersionSql)
        {
        }

        public PrnVersionDTO Create(IDataReader dr)
        {
            PrnVersionDTO entity = new PrnVersionDTO();

            int iPrnvercodi = dr.GetOrdinal(this.Prnvercodi);
            if (!dr.IsDBNull(iPrnvercodi)) entity.Prnvercodi = Convert.ToInt32(dr.GetValue(iPrnvercodi));

            int iPrnvernomb = dr.GetOrdinal(this.Prnvernomb);
            if (!dr.IsDBNull(iPrnvernomb)) entity.Prnvernomb = dr.GetString(iPrnvernomb);

            int iPrnverestado = dr.GetOrdinal(this.Prnverestado);
            if (!dr.IsDBNull(iPrnverestado)) entity.Prnverestado = dr.GetString(iPrnverestado);
            
            int iPrnverusucreacion = dr.GetOrdinal(this.Prnverusucreacion);
            if (!dr.IsDBNull(iPrnverusucreacion)) entity.Prnverusucreacion = dr.GetString(iPrnverusucreacion);

            int iPrnverfeccreacion = dr.GetOrdinal(this.Prnverfeccreacion);
            if (!dr.IsDBNull(iPrnverfeccreacion)) entity.Prnverfeccreacion = dr.GetDateTime(iPrnverfeccreacion);

            int iPrnverusumodificacion = dr.GetOrdinal(this.Prnverusumodificacion);
            if (!dr.IsDBNull(iPrnverusumodificacion)) entity.Prnverusumodificacion = dr.GetString(iPrnverusumodificacion);

            int iPrnverfecmodificacion = dr.GetOrdinal(this.Prnverfecmodificacion);
            if (!dr.IsDBNull(iPrnverfecmodificacion)) entity.Prnverfecmodificacion = dr.GetDateTime(iPrnverfecmodificacion);

            return entity;
        }

        #region Mapeo de los campos

        public string Prnvercodi = "PRNVERCODI";
        public string Prnvernomb = "PRNVERNOMB";
        public string Prnverestado = "PRNVERESTADO";
        public string Prnverusucreacion = "PRNVERUSUCREACION";
        public string Prnverfeccreacion = "PRNVERFECCREACION";
        public string Prnverusumodificacion = "PRNVERUSUMODIFICACION";
        public string Prnverfecmodificacion = "PRNVERFECMODIFICACION";

        public string Prnredbarracp = "PRNREDBARRACP";
        public string Prnredbarrapm = "PRNREDBARRAPM";
        public string Prnredgauss = "PRNREDGAUSS";
        public string Prnredperdida = "PRNREDPERDIDA";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Prnredtipo = "PRNREDTIPO";
        #endregion

        #region Consultas a la BD

        public string SqlGetModeloActivo
        {
            get { return base.GetSqlXml("GetModeloActivo"); }
        }

        #endregion


        public string SqlUpdateAllVersionInactivo
        {
            get { return GetSqlXml("UpdateAllVersionInactivo"); }
        }

    }
}
