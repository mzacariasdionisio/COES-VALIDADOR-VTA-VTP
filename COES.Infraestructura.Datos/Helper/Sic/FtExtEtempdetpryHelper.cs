using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ETEMPDETPRY
    /// </summary>
    public class FtExtEtempdetpryHelper : HelperBase
    {
        public FtExtEtempdetpryHelper() : base(Consultas.FtExtEtempdetprySql)
        {
        }

        public FtExtEtempdetpryDTO Create(IDataReader dr)
        {
            FtExtEtempdetpryDTO entity = new FtExtEtempdetpryDTO();

            int iFetempcodi = dr.GetOrdinal(this.Fetempcodi);
            if (!dr.IsDBNull(iFetempcodi)) entity.Fetempcodi = Convert.ToInt32(dr.GetValue(iFetempcodi));

            int iFeeprycodi = dr.GetOrdinal(this.Feeprycodi);
            if (!dr.IsDBNull(iFeeprycodi)) entity.Feeprycodi = Convert.ToInt32(dr.GetValue(iFeeprycodi));

            int iFtprycodi = dr.GetOrdinal(this.Ftprycodi);
            if (!dr.IsDBNull(iFtprycodi)) entity.Ftprycodi = Convert.ToInt32(dr.GetValue(iFtprycodi));

            int iFeepryestado = dr.GetOrdinal(this.Feepryestado);
            if (!dr.IsDBNull(iFeepryestado)) entity.Feepryestado = dr.GetString(iFeepryestado);

            return entity;
        }

        #region Mapeo de Campos

        public string Fetempcodi = "FETEMPCODI";
        public string Feeprycodi = "FEEPRYCODI";
        public string Ftprycodi = "FTPRYCODI";
        public string Feepryestado = "FEEPRYESTADO";

        public string Emprnomb = "EMPRNOMB";
        public string Ftetnombre = "FTETNOMBRE";
        public string Emprcodi = "EMPRCODI";
        public string Ftetcodi = "FTETCODI";
        public string Ftprynombre = "FTPRYNOMBRE";

        #endregion

        public string SqlGetByEmpresaYEtapa
        {
            get { return GetSqlXml("GetByEmpresaYEtapa"); }
        }

        public string SqlListarPorRelEmpresaEtapa
        {
            get { return base.GetSqlXml("ListarPorRelEmpresaEtapa"); }
        }

        public string SqlGetByProyectos
        {
            get { return base.GetSqlXml("GetByProyectos"); }
        }
        

    }
}
