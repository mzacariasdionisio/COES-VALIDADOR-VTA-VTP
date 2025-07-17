using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_FICHATECNICA
    /// </summary>
    public class FtFichaTecnicaHelper : HelperBase
    {
        public FtFichaTecnicaHelper()
            : base(Consultas.FtFichaTecnicaSql)
        {
        }

        public FtFichaTecnicaDTO Create(IDataReader dr)
        {
            FtFichaTecnicaDTO entity = new FtFichaTecnicaDTO();

            int iFteccodi = dr.GetOrdinal(this.Fteccodi);
            if (!dr.IsDBNull(iFteccodi)) entity.Fteccodi = Convert.ToInt32(dr.GetValue(iFteccodi));

            int iFtecnombre = dr.GetOrdinal(this.Ftecnombre);
            if (!dr.IsDBNull(iFtecnombre)) entity.Ftecnombre = dr.GetString(iFtecnombre);

            int iFtecprincipal = dr.GetOrdinal(this.Ftecprincipal);
            if (!dr.IsDBNull(iFtecprincipal)) entity.Ftecprincipal = Convert.ToInt32(dr.GetValue(iFtecprincipal));

            int iFtecestado = dr.GetOrdinal(this.Ftecestado);
            if (!dr.IsDBNull(iFtecestado)) entity.Ftecestado = dr.GetString(iFtecestado);

            int iFtecusucreacion = dr.GetOrdinal(this.Ftecusucreacion);
            if (!dr.IsDBNull(iFtecusucreacion)) entity.Ftecusucreacion = dr.GetString(iFtecusucreacion);

            int iFtecusumodificacion = dr.GetOrdinal(this.Ftecusumodificacion);
            if (!dr.IsDBNull(iFtecusumodificacion)) entity.Ftecusumodificacion = dr.GetString(iFtecusumodificacion);

            int iFtecfecmodificacion = dr.GetOrdinal(this.Ftecfecmodificacion);
            if (!dr.IsDBNull(iFtecfecmodificacion)) entity.Ftecfecmodificacion = dr.GetDateTime(iFtecfecmodificacion);

            int iFtecfeccreacion = dr.GetOrdinal(this.Ftecfeccreacion);
            if (!dr.IsDBNull(iFtecfeccreacion)) entity.Ftecfeccreacion = dr.GetDateTime(iFtecfeccreacion);

            int iFtecambiente = dr.GetOrdinal(this.Ftecambiente);
            if (!dr.IsDBNull(iFtecambiente)) entity.Ftecambiente = Convert.ToInt32(dr.GetValue(iFtecambiente));

            return entity;
        }

        #region Mapeo de Campos

        public string Fteccodi = "FTECCODI";
        public string Ftecnombre = "FTECNOMBRE";
        public string Ftecprincipal = "FTECPRINCIPAL";
        public string Ftecestado = "FTECESTADO";
        public string Ftecusucreacion = "FTECUSUCREACION";
        public string Ftecusumodificacion = "FTECUSUMODIFICACION";
        public string Ftecfecmodificacion = "FTECFECMODIFICACION";
        public string Ftecfeccreacion = "FTECFECCREACION";
        public string Ftecambiente = "FTECAMBIENTE";

        #endregion

        #region Mapeo de Consultas

        public string SqlGetFichaMaestraPrincipal
        {
            get { return base.GetSqlXml("GetFichaMaestraPrincipal"); }
        }

        #endregion

    }
}
