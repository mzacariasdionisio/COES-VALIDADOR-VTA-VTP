using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_FICTECXTIPOEQUIPO
    /// </summary>
    public class FtFictecXTipoEquipoHelper : HelperBase
    {
        public FtFictecXTipoEquipoHelper()
            : base(Consultas.FtFictecXTipoEquipoSql)
        {
        }

        public FtFictecXTipoEquipoDTO Create(IDataReader dr)
        {
            FtFictecXTipoEquipoDTO entity = new FtFictecXTipoEquipoDTO();

            int iFteqcodi = dr.GetOrdinal(this.Fteqcodi);
            if (!dr.IsDBNull(iFteqcodi)) entity.Fteqcodi = Convert.ToInt32(dr.GetValue(iFteqcodi));

            int iFteqnombre = dr.GetOrdinal(this.Fteqnombre);
            if (!dr.IsDBNull(iFteqnombre)) entity.Fteqnombre = dr.GetString(iFteqnombre);

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iCatecodi = dr.GetOrdinal(this.Catecodi);
            if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

            int iFtequsucreacion = dr.GetOrdinal(this.Ftequsucreacion);
            if (!dr.IsDBNull(iFtequsucreacion)) entity.Ftequsucreacion = dr.GetString(iFtequsucreacion);

            int iFtequsumodificacion = dr.GetOrdinal(this.Ftequsumodificacion);
            if (!dr.IsDBNull(iFtequsumodificacion)) entity.Ftequsumodificacion = dr.GetString(iFtequsumodificacion);

            int iFteqfecmodificacion = dr.GetOrdinal(this.Fteqfecmodificacion);
            if (!dr.IsDBNull(iFteqfecmodificacion)) entity.Fteqfecmodificacion = dr.GetDateTime(iFteqfecmodificacion);

            int iFteqfeccreacion = dr.GetOrdinal(this.Fteqfeccreacion);
            if (!dr.IsDBNull(iFteqfeccreacion)) entity.Fteqfeccreacion = dr.GetDateTime(iFteqfeccreacion);

            int iFteqpadre = dr.GetOrdinal(this.Fteqpadre);
            if (!dr.IsDBNull(iFteqpadre)) entity.Fteqpadre = Convert.ToInt32(dr.GetValue(iFteqpadre));

            int iFteqestado = dr.GetOrdinal(this.Fteqestado);
            if (!dr.IsDBNull(iFteqestado)) entity.Fteqestado = dr.GetString(iFteqestado);

            int iFteqflagext = dr.GetOrdinal(this.Fteqflagext);
            if (!dr.IsDBNull(iFteqflagext)) entity.Fteqflagext = Convert.ToInt32(dr.GetValue(iFteqflagext));

            int iFteqfecvigenciaext = dr.GetOrdinal(this.Fteqfecvigenciaext);
            if (!dr.IsDBNull(iFteqfecvigenciaext)) entity.Fteqfecvigenciaext = dr.GetDateTime(iFteqfecvigenciaext);

            int iFteqflagmostrarcoment = dr.GetOrdinal(this.Fteqflagmostrarcoment);
            if (!dr.IsDBNull(iFteqflagmostrarcoment)) entity.Fteqflagmostrarcoment = Convert.ToInt32(dr.GetValue(iFteqflagmostrarcoment));

            int iFteqflagmostrarsust = dr.GetOrdinal(this.Fteqflagmostrarsust);
            if (!dr.IsDBNull(iFteqflagmostrarsust)) entity.Fteqflagmostrarsust = Convert.ToInt32(dr.GetValue(iFteqflagmostrarsust));

            int iFteqflagmostrarfech = dr.GetOrdinal(this.Fteqflagmostrarfech);
            if (!dr.IsDBNull(iFteqflagmostrarfech)) entity.Fteqflagmostrarfech = Convert.ToInt32(dr.GetValue(iFteqflagmostrarfech));

            int iFtequsumodificacionasig = dr.GetOrdinal(this.Ftequsumodificacionasig);
            if (!dr.IsDBNull(iFtequsumodificacionasig)) entity.Ftequsumodificacionasig = dr.GetString(iFtequsumodificacionasig);

            int iFteqfecmodificacionasig = dr.GetOrdinal(this.Fteqfecmodificacionasig);
            if (!dr.IsDBNull(iFteqfecmodificacionasig)) entity.Fteqfecmodificacionasig = dr.GetDateTime(iFteqfecmodificacionasig);

            return entity;
        }

        #region Mapeo de Campos

        public string Fteqcodi = "FTEQCODI";
        public string Fteqnombre = "FTEQNOMBRE";
        public string Famcodi = "FAMCODI";
        public string Catecodi = "CATECODI";
        public string Ftequsucreacion = "FTEQUSUCREACION";
        public string Ftequsumodificacion = "FTEQUSUMODIFICACION";
        public string Fteqfecmodificacion = "FTEQFECMODIFICACION";
        public string Fteqfeccreacion = "FTEQFECCREACION";
        public string Fteqpadre = "FTEQPADRE";
        public string Fteqestado = "FTEQESTADO";

        public string Famnomb = "FAMNOMB";
        public string Catenomb = "CATENOMB";
        public string Famnombpadre = "FAMNOMBPADRE";
        public string Catenombpadre = "CATENOMBPADRE";
        public string Famcodipadre = "FAMCODIPADRE";
        public string Catecodipadre = "CATECODIPADRE";
        public string Fteqnombrepadre = "FTEQNOMBREPADRE";
        public string Fteqflagext = "FTEQFLAGEXT";
        public string Fteqfecvigenciaext = "FTEQFECVIGENCIAEXT";
        public string Fteqflagmostrarcoment = "FTEQFLAGMOSTRARCOMENT";
        public string Fteqflagmostrarsust = "FTEQFLAGMOSTRARSUST";
        public string Fteqflagmostrarfech = "FTEQFLAGMOSTRARFECH";
        public string Ftequsumodificacionasig = "FTEQUSUMODIFICACIONASIG";
        public string Fteqfecmodificacionasig = "FTEQFECMODIFICACIONASIG";
        #endregion

        #region Mapeo de Consultas

        public string SqlListByFteccodi
        {
            get { return base.GetSqlXml("ListByFteccodi"); }
        }

        public string SqlListAllByFteccodi
        {
            get { return base.GetSqlXml("ListAllByFteccodi"); }
        }

        public string SqlListByFteqpadre
        {
            get { return base.GetSqlXml("ListByFteqpadre"); }
        }

        #endregion

    }
}
