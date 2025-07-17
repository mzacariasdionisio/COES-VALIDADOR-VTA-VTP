using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_REVISION
    /// </summary>
    public class FtExtEnvioRevisionHelper : HelperBase
    {
        public FtExtEnvioRevisionHelper(): base(Consultas.FtExtEnvioRevisionSql)
        {
        }

        public FtExtEnvioRevisionDTO Create(IDataReader dr)
        {
            FtExtEnvioRevisionDTO entity = new FtExtEnvioRevisionDTO();

            int iFtrevcodi = dr.GetOrdinal(this.Ftrevcodi);
            if (!dr.IsDBNull(iFtrevcodi)) entity.Ftrevcodi = Convert.ToInt32(dr.GetValue(iFtrevcodi));

            int iFtrevhtmlobscoes = dr.GetOrdinal(this.Ftrevhtmlobscoes);
            if (!dr.IsDBNull(iFtrevhtmlobscoes)) entity.Ftrevhtmlobscoes = dr.GetString(iFtrevhtmlobscoes);

            int iFtrevhtmlrptaagente = dr.GetOrdinal(this.Ftrevhtmlrptaagente);
            if (!dr.IsDBNull(iFtrevhtmlrptaagente)) entity.Ftrevhtmlrptaagente = dr.GetString(iFtrevhtmlrptaagente);

            int iFtrevhtmlrptacoes = dr.GetOrdinal(this.Ftrevhtmlrptacoes);
            if (!dr.IsDBNull(iFtrevhtmlrptacoes)) entity.Ftrevhtmlrptacoes = dr.GetString(iFtrevhtmlrptacoes);

            int iFtrevestado = dr.GetOrdinal(this.Ftrevestado);
            if (!dr.IsDBNull(iFtrevestado)) entity.Ftrevestado = dr.GetString(iFtrevestado);

            return entity;
        }


        #region Mapeo de Campos

        public string Ftrevcodi = "FTREVCODI";
        public string Ftrevhtmlobscoes = "FTREVHTMLOBSCOES";
        public string Ftrevhtmlrptaagente = "FTREVHTMLRPTAAGENTE";
        public string Ftrevhtmlrptacoes = "FTREVHTMLRPTACOES";
        public string Ftrevestado = "FTREVESTADO";

        public string Ftedatcodi = "FTEDATCODI";
        public string Fevrqcodi = "FEVRQCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Fteeqcodi = "FTEEQCODI";
        public string Ftereqcodi = "FTEREQCODI";
        public string Ftitcodi = "FTITCODI";

        #endregion

        public string SqlListarPorDatos
        {
            get { return GetSqlXml("ListarPorDatos"); }
        }

        public string SqlListarPorRequisitos
        {
            get { return GetSqlXml("ListarPorRequisitos"); }
        }

        public string SqlListarPorModoOp
        {
            get { return GetSqlXml("ListarPorModoOp"); }
        }
        
    }
}
