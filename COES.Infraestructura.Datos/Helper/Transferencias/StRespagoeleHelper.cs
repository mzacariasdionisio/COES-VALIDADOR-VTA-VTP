using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_RESPAGOELE
    /// </summary>
    public class StRespagoeleHelper : HelperBase
    {
        public StRespagoeleHelper() : base(Consultas.StRespagoeleSql)
        {
        }

        public StRespagoeleDTO Create(IDataReader dr)
        {
            StRespagoeleDTO entity = new StRespagoeleDTO();

            int iRespaecodi = dr.GetOrdinal(this.Respaecodi);
            if (!dr.IsDBNull(iRespaecodi)) entity.Respaecodi = Convert.ToInt32(dr.GetValue(iRespaecodi));

            int iRespagcodi = dr.GetOrdinal(this.Respagcodi);
            if (!dr.IsDBNull(iRespagcodi)) entity.Respagcodi = Convert.ToInt32(dr.GetValue(iRespagcodi));

            int iStcompcodi = dr.GetOrdinal(this.Stcompcodi);
            if (!dr.IsDBNull(iStcompcodi)) entity.Stcompcodi = Convert.ToInt32(dr.GetValue(iStcompcodi));

            int iRespaecodelemento = dr.GetOrdinal(this.Respaecodelemento);
            if (!dr.IsDBNull(iRespaecodelemento)) entity.Respaecodelemento = dr.GetString(iRespaecodelemento);

            int iRespaevalor = dr.GetOrdinal(this.Respaevalor);
            if (!dr.IsDBNull(iRespaevalor)) entity.Respaevalor = Convert.ToInt32(dr.GetValue(iRespaevalor));

            return entity;
        }


        #region Mapeo de Campos

        public string Respaecodi = "RESPAECODI";
        public string Respagcodi = "RESPAGCODI";
        public string Stcompcodi = "STCOMPCODI";
        public string Respaecodelemento = "RESPAECODELEMENTO";
        public string Respaevalor = "RESPAEVALOR";
        //variables para consulta
        public string Strecacodi = "STRECACODI";

        #endregion

        public string SqlListStRespagElePorID
        {
            get { return base.GetSqlXml("ListStRespagElePorID"); }
        }

        public string SqlDeleteStRespagoEleVersion
        {
            get { return base.GetSqlXml("DeleteStRespagoEleVersion"); }
        }

    }
}
