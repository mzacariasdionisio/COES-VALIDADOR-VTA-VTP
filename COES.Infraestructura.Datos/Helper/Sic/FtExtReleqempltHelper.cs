using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_RELEQEMPLT
    /// </summary>
    public class FtExtReleqempltHelper : HelperBase
    {
        public FtExtReleqempltHelper() : base(Consultas.FtExtReleqempltSql)
        {
        }

        public FtExtReleqempltDTO Create(IDataReader dr)
        {
            FtExtReleqempltDTO entity = new FtExtReleqempltDTO();

            int iFtreqecodi = dr.GetOrdinal(this.Ftreqecodi);
            if (!dr.IsDBNull(iFtreqecodi)) entity.Ftreqecodi = Convert.ToInt32(dr.GetValue(iFtreqecodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iFtreqeestado = dr.GetOrdinal(this.Ftreqeestado);
            if (!dr.IsDBNull(iFtreqeestado)) entity.Ftreqeestado = Convert.ToInt32(dr.GetValue(iFtreqeestado));

            int iFtreqeusucreacion = dr.GetOrdinal(this.Ftreqeusucreacion);
            if (!dr.IsDBNull(iFtreqeusucreacion)) entity.Ftreqeusucreacion = dr.GetString(iFtreqeusucreacion);

            int iFtreqefeccreacion = dr.GetOrdinal(this.Ftreqefeccreacion);
            if (!dr.IsDBNull(iFtreqefeccreacion)) entity.Ftreqefeccreacion = dr.GetDateTime(iFtreqefeccreacion);

            int iFtreqeusumodificacion = dr.GetOrdinal(this.Ftreqeusumodificacion);
            if (!dr.IsDBNull(iFtreqeusumodificacion)) entity.Ftreqeusumodificacion = dr.GetString(iFtreqeusumodificacion);

            int iFtreqefecmodificacion = dr.GetOrdinal(this.Ftreqefecmodificacion);
            if (!dr.IsDBNull(iFtreqefecmodificacion)) entity.Ftreqefecmodificacion = dr.GetDateTime(iFtreqefecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ftreqecodi = "FTREQECODI";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";
        public string Ftreqeestado = "FTREQEESTADO";
        public string Ftreqeusucreacion = "FTREQEUSUCREACION";
        public string Ftreqefeccreacion = "FTREQEFECCREACION";
        public string Ftreqeusumodificacion = "FTREQEUSUMODIFICACION";
        public string Ftreqefecmodificacion = "FTREQEFECMODIFICACION";
        public string Emprnomb = "EMPRNOMB";
        #endregion

        public string SqlListarPorEquipo
        {
            get { return base.GetSqlXml("ListarPorEquipo"); }
        }
    }
}
