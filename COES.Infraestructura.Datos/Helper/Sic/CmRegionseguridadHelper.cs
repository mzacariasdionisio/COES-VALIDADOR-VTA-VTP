using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_REGIONSEGURIDAD
    /// </summary>
    public class CmRegionseguridadHelper : HelperBase
    {
        public CmRegionseguridadHelper(): base(Consultas.CmRegionseguridadSql)
        {
        }

        public CmRegionseguridadDTO Create(IDataReader dr)
        {
            CmRegionseguridadDTO entity = new CmRegionseguridadDTO();

            int iRegsegcodi = dr.GetOrdinal(this.Regsegcodi);
            if (!dr.IsDBNull(iRegsegcodi)) entity.Regsegcodi = Convert.ToInt32(dr.GetValue(iRegsegcodi));

            int iRegsegnombre = dr.GetOrdinal(this.Regsegnombre);
            if (!dr.IsDBNull(iRegsegnombre)) entity.Regsegnombre = dr.GetString(iRegsegnombre);

            int iRegsegvalorm = dr.GetOrdinal(this.Regsegvalorm);
            if (!dr.IsDBNull(iRegsegvalorm)) entity.Regsegvalorm = dr.GetDecimal(iRegsegvalorm);

            int iRegsegdirec = dr.GetOrdinal(this.Regsegdirec);
            if (!dr.IsDBNull(iRegsegdirec)) entity.Regsegdirec = dr.GetString(iRegsegdirec);

            int iRegsegestado = dr.GetOrdinal(this.Regsegestado);
            if (!dr.IsDBNull(iRegsegestado)) entity.Regsegestado = dr.GetString(iRegsegestado);

            int iRegsegusucreacion = dr.GetOrdinal(this.Regsegusucreacion);
            if (!dr.IsDBNull(iRegsegusucreacion)) entity.Regsegusucreacion = dr.GetString(iRegsegusucreacion);

            int iRegsegfeccreacion = dr.GetOrdinal(this.Regsegfeccreacion);
            if (!dr.IsDBNull(iRegsegfeccreacion)) entity.Regsegfeccreacion = dr.GetDateTime(iRegsegfeccreacion);

            int iRegsegusumodificacion = dr.GetOrdinal(this.Regsegusumodificacion);
            if (!dr.IsDBNull(iRegsegusumodificacion)) entity.Regsegusumodificacion = dr.GetString(iRegsegusumodificacion);

            int iRegsegfecmodificacion = dr.GetOrdinal(this.Regsegfecmodificacion);
            if (!dr.IsDBNull(iRegsegfecmodificacion)) entity.Regsegfecmodificacion = dr.GetDateTime(iRegsegfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Regsegcodi = "REGSEGCODI";
        public string Regsegnombre = "REGSEGNOMBRE";
        public string Regsegvalorm = "REGSEGVALORM";
        public string Regsegdirec = "REGSEGDIREC";
        public string Regsegestado = "REGSEGESTADO";
        public string Regsegusucreacion = "REGSEGUSUCREACION";
        public string Regsegfeccreacion = "REGSEGFECCREACION";
        public string Regsegusumodificacion = "REGSEGUSUMODIFICACION";
        public string Regsegfecmodificacion = "REGSEGFECMODIFICACION";

        #endregion

        public string SqlGetByCriteriaCoordenada
        {
            get { return base.GetSqlXml("GetByCriteriaCoordenada"); }
        }

    }
}
