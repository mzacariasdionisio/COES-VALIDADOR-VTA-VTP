using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ETEMPDETPRYEQ
    /// </summary>
    public class FtExtEtempdetpryeqHelper : HelperBase
    {
        public FtExtEtempdetpryeqHelper() : base(Consultas.FtExtEtempdetpryeqSql)
        {
        }

        public FtExtEtempdetpryeqDTO Create(IDataReader dr)
        {
            FtExtEtempdetpryeqDTO entity = new FtExtEtempdetpryeqDTO();

            int iFeeprycodi = dr.GetOrdinal(this.Feeprycodi);
            if (!dr.IsDBNull(iFeeprycodi)) entity.Feeprycodi = Convert.ToInt32(dr.GetValue(iFeeprycodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iFeepeqcodi = dr.GetOrdinal(this.Feepeqcodi);
            if (!dr.IsDBNull(iFeepeqcodi)) entity.Feepeqcodi = Convert.ToInt32(dr.GetValue(iFeepeqcodi));

            int iFeepeqflagotraetapa = dr.GetOrdinal(this.Feepeqflagotraetapa);
            if (!dr.IsDBNull(iFeepeqflagotraetapa)) entity.Feepeqflagotraetapa = dr.GetString(iFeepeqflagotraetapa);

            int iFeepeqflagsistema = dr.GetOrdinal(this.Feepeqflagsistema);
            if (!dr.IsDBNull(iFeepeqflagsistema)) entity.Feepeqflagsistema = dr.GetString(iFeepeqflagsistema);

            int iFeepequsucreacion = dr.GetOrdinal(this.Feepequsucreacion);
            if (!dr.IsDBNull(iFeepequsucreacion)) entity.Feepequsucreacion = dr.GetString(iFeepequsucreacion);

            int iFeepeqfeccreacion = dr.GetOrdinal(this.Feepeqfeccreacion);
            if (!dr.IsDBNull(iFeepeqfeccreacion)) entity.Feepeqfeccreacion = dr.GetDateTime(iFeepeqfeccreacion);

            int iFeepequsumodificacion = dr.GetOrdinal(this.Feepequsumodificacion);
            if (!dr.IsDBNull(iFeepequsumodificacion)) entity.Feepequsumodificacion = dr.GetString(iFeepequsumodificacion);

            int iFeepeqfecmodificacion = dr.GetOrdinal(this.Feepeqfecmodificacion);
            if (!dr.IsDBNull(iFeepeqfecmodificacion)) entity.Feepeqfecmodificacion = dr.GetDateTime(iFeepeqfecmodificacion);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iFeepeqestado = dr.GetOrdinal(this.Feepeqestado);
            if (!dr.IsDBNull(iFeepeqestado)) entity.Feepeqestado = dr.GetString(iFeepeqestado); 

            return entity;
        }


        #region Mapeo de Campos

        public string Feeprycodi = "FEEPRYCODI";
        public string Equicodi = "EQUICODI";
        public string Feepeqcodi = "FEEPEQCODI";
        public string Feepeqflagotraetapa = "FEEPEQFLAGOTRAETAPA";
        public string Feepeqflagsistema = "FEEPEQFLAGSISTEMA"; 
        public string Feepequsucreacion = "FEEPEQUSUCREACION";
        public string Feepeqfeccreacion = "FEEPEQFECCREACION";
        public string Feepequsumodificacion = "FEEPEQUSUMODIFICACION";
        public string Feepeqfecmodificacion = "FEEPEQFECMODIFICACION";
        public string Grupocodi = "GRUPOCODI";
        public string Feepeqestado = "FEEPEQESTADO";
        

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

        public string SqlListarPorRelProyectoEtapaEmpresa
        {
            get { return base.GetSqlXml("ListarPorRelProyectoEtapaEmpresa"); }
        }

        public string SqlGetByProyectoYEquipo
        {
            get { return base.GetSqlXml("GetByProyectoYEquipo"); }
        }

        public string SqlGetByProyectoYGrupo
        {
            get { return base.GetSqlXml("GetByProyectoYGrupo"); }
        }

        public string SqlGetByProyectoEquipoEmpresaEtapa
        {
            get { return base.GetSqlXml("GetByProyectoEquipoEmpresaEtapa"); }
        }

        public string SqlGetByProyectoGrupoEmpresaEtapa
        {
            get { return base.GetSqlXml("GetByProyectoGrupoEmpresaEtapa"); }
        }

        public string SqlListarDetallesPorRelEmpresaEtapaProyecto
        {
            get { return base.GetSqlXml("ListarDetallesPorRelEmpresaEtapaProyecto"); }
        }
        


    }
}
