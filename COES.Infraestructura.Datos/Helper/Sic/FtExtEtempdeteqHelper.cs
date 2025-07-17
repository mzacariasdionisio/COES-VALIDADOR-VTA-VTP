using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ETEMPDETEQ
    /// </summary>
    public class FtExtEtempdeteqHelper : HelperBase
    {
        public FtExtEtempdeteqHelper() : base(Consultas.FtExtEtempdeteqSql)
        {
        }

        public FtExtEtempdeteqDTO Create(IDataReader dr)
        {
            FtExtEtempdeteqDTO entity = new FtExtEtempdeteqDTO();

            int iFetempcodi = dr.GetOrdinal(this.Fetempcodi);
            if (!dr.IsDBNull(iFetempcodi)) entity.Fetempcodi = Convert.ToInt32(dr.GetValue(iFetempcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iFeeeqcodi = dr.GetOrdinal(this.Feeeqcodi);
            if (!dr.IsDBNull(iFeeeqcodi)) entity.Feeeqcodi = Convert.ToInt32(dr.GetValue(iFeeeqcodi));

            int iFeeeqflagotraetapa = dr.GetOrdinal(this.Feeeqflagotraetapa);
            if (!dr.IsDBNull(iFeeeqflagotraetapa)) entity.Feeeqflagotraetapa = dr.GetString(iFeeeqflagotraetapa);

            int iFeeeqflagsistema = dr.GetOrdinal(this.Feeeqflagsistema);
            if (!dr.IsDBNull(iFeeeqflagsistema)) entity.Feeeqflagsistema = dr.GetString(iFeeeqflagsistema);

            int iFeeequsucreacion = dr.GetOrdinal(this.Feeequsucreacion);
            if (!dr.IsDBNull(iFeeequsucreacion)) entity.Feeequsucreacion = dr.GetString(iFeeequsucreacion);

            int iFeeeqfeccreacion = dr.GetOrdinal(this.Feeeqfeccreacion);
            if (!dr.IsDBNull(iFeeeqfeccreacion)) entity.Feeeqfeccreacion = dr.GetDateTime(iFeeeqfeccreacion);

            int iFeeequsumodificacion = dr.GetOrdinal(this.Feeequsumodificacion);
            if (!dr.IsDBNull(iFeeequsumodificacion)) entity.Feeequsumodificacion = dr.GetString(iFeeequsumodificacion);

            int iFeeeqfecmodificacion = dr.GetOrdinal(this.Feeeqfecmodificacion);
            if (!dr.IsDBNull(iFeeeqfecmodificacion)) entity.Feeeqfecmodificacion = dr.GetDateTime(iFeeeqfecmodificacion);

            int iFeeeqestado = dr.GetOrdinal(this.Feeeqestado);
            if (!dr.IsDBNull(iFeeeqestado)) entity.Feeeqestado = dr.GetString(iFeeeqestado);

            int iFeeeqflagcentral = dr.GetOrdinal(this.Feeeqflagcentral);
            if (!dr.IsDBNull(iFeeeqflagcentral)) entity.Feeeqflagcentral = dr.GetString(iFeeeqflagcentral);

            return entity;
        }

        #region Mapeo de Campos

        public string Fetempcodi = "FETEMPCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Equicodi = "EQUICODI";
        public string Feeeqcodi = "FEEEQCODI";
        public string Feeeqflagotraetapa = "FEEEQFLAGOTRAETAPA";
        public string Feeeqflagsistema = "FEEEQFLAGSISTEMA";
        public string Feeequsucreacion = "FEEEQUSUCREACION";
        public string Feeeqfeccreacion = "FEEEQFECCREACION";
        public string Feeequsumodificacion = "FEEEQUSUMODIFICACION";
        public string Feeeqfecmodificacion = "FEEEQFECMODIFICACION";
        public string Feeeqestado = "FEEEQESTADO";
        public string Feeeqflagcentral = "FEEEQFLAGCENTRAL";

        public string Emprcodi = "EMPRCODI";
        public string Areacodi = "AREACODI";
        public string Famcodi = "FAMCODI";
        public string Catecodi = "CATECODI";
        public string Equinomb = "EQUINOMB";
        public string Areanomb = "AREANOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Famnomb = "FAMNOMB";
        public string Catenomb = "CATENOMB";
        

        #endregion

        public string SqlGetByElementoEquipoEmpresaEtapa
        {
            get { return base.GetSqlXml("GetByElementoEquipoEmpresaEtapa"); }
        }

        public string SqlGetByElementoGrupoEmpresaEtapa
        {
            get { return base.GetSqlXml("GetByElementoGrupoEmpresaEtapa"); }
        }

        public string SqlGetByEmpresaEtapa
        {
            get { return base.GetSqlXml("GetByEmpresaEtapa"); }
        }

        public string SqlListarPorRelEmpresaEtapa
        {
            get { return base.GetSqlXml("ListarPorRelEmpresaEtapa"); }
        }


    }
}
