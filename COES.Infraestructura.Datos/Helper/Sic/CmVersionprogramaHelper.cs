using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_VERSIONPROGRAMA
    /// </summary>
    public class CmVersionprogramaHelper : HelperBase
    {
        public CmVersionprogramaHelper(): base(Consultas.CmVersionprogramaSql)
        {
        }

        public CmVersionprogramaDTO Create(IDataReader dr)
        {
            CmVersionprogramaDTO entity = new CmVersionprogramaDTO();

            int iCmveprcodi = dr.GetOrdinal(this.Cmveprcodi);
            if (!dr.IsDBNull(iCmveprcodi)) entity.Cmveprcodi = Convert.ToInt32(dr.GetValue(iCmveprcodi));

            int iCmgncorrelativo = dr.GetOrdinal(this.Cmgncorrelativo);
            if (!dr.IsDBNull(iCmgncorrelativo)) entity.Cmgncorrelativo = Convert.ToInt32(dr.GetValue(iCmgncorrelativo));

            int iCmveprvalor = dr.GetOrdinal(this.Cmveprvalor);
            if (!dr.IsDBNull(iCmveprvalor)) entity.Cmveprvalor = dr.GetString(iCmveprvalor);

            int iCmveprtipoprograma = dr.GetOrdinal(this.Cmveprtipoprograma);
            if (!dr.IsDBNull(iCmveprtipoprograma)) entity.Cmveprtipoprograma = dr.GetString(iCmveprtipoprograma);

            int iCmveprtipoestimador = dr.GetOrdinal(this.Cmveprtipoestimador);
            if (!dr.IsDBNull(iCmveprtipoestimador)) entity.Cmveprtipoestimador = dr.GetString(iCmveprtipoestimador);

            int iCmveprtipocorrida = dr.GetOrdinal(this.Cmveprtipocorrida);
            if (!dr.IsDBNull(iCmveprtipocorrida)) entity.Cmveprtipocorrida = dr.GetString(iCmveprtipocorrida);

            int iTopcodi = dr.GetOrdinal(this.Topcodi);
            if (!dr.IsDBNull(iTopcodi)) entity.Topcodi = Convert.ToInt32(dr.GetValue(iTopcodi));

            int iCmveprversion = dr.GetOrdinal(this.Cmveprversion);
            if (!dr.IsDBNull(iCmveprversion)) entity.Cmveprversion = Convert.ToInt32(dr.GetValue(iCmveprversion));

            return entity;
        }


        #region Mapeo de Campos

        public string Cmveprcodi = "CMVEPRCODI";
        public string Cmgncorrelativo = "CMGNCORRELATIVO";
        public string Cmveprvalor = "CMVEPRVALOR";
        public string Cmveprtipoprograma = "CMVEPRTIPOPROGRAMA";
        public string Cmveprtipoestimador = "CMVEPRTIPOESTIMADOR";
        public string Cmveprtipocorrida = "CMVEPRTIPOCORRIDA";
        public string Topcodi = "TOPCODI";
        public string Cmveprversion = "CMVEPRVERSION";

        #endregion
    }
}
