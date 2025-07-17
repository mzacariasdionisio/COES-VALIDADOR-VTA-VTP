using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_COMPMENSUALELE
    /// </summary>
    public class StCompmensualeleHelper : HelperBase
    {
        public StCompmensualeleHelper()
            : base(Consultas.StCompmensualeleSql)
        {
        }

        public StCompmensualeleDTO Create(IDataReader dr)
        {
            StCompmensualeleDTO entity = new StCompmensualeleDTO();

            int iCmpmelcodi = dr.GetOrdinal(this.Cmpmelcodi);
            if (!dr.IsDBNull(iCmpmelcodi)) entity.Cmpmelcodi = Convert.ToInt32(dr.GetValue(iCmpmelcodi));

            int iCmpmencodi = dr.GetOrdinal(this.Cmpmencodi);
            if (!dr.IsDBNull(iCmpmencodi)) entity.Cmpmencodi = Convert.ToInt32(dr.GetValue(iCmpmencodi));

            int iStcompcodi = dr.GetOrdinal(this.Stcompcodi);
            if (!dr.IsDBNull(iStcompcodi)) entity.Stcompcodi = Convert.ToInt32(dr.GetValue(iStcompcodi));

            int iCmpmelcodelemento = dr.GetOrdinal(this.Cmpmelcodelemento);
            if (!dr.IsDBNull(iCmpmelcodelemento)) entity.Cmpmelcodelemento = dr.GetString(iCmpmelcodelemento);

            int iCmpmelvalor = dr.GetOrdinal(this.Cmpmelvalor);
            if (!dr.IsDBNull(iCmpmelvalor)) entity.Cmpmelvalor = dr.GetDecimal(iCmpmelvalor);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmpmelcodi = "CMPMELCODI";
        public string Cmpmencodi = "CMPMENCODI";
        public string Stcompcodi = "STCOMPCODI";
        public string Cmpmelcodelemento = "CMPMELCODELEMENTO";
        public string Cmpmelvalor = "CMPMELVALOR";
        // variables para consulta
        public string Strecacodi = "STRECACODI";
        public string Equinomb = "EQUINOMB";
        //public string Stcompcodelemento = "STCOMPCODELEMENTO";
        public string Stcntgcodi = "STCNTGCODI";

        public string SqlGetByIdStCompMensualEle
        {
            get { return base.GetSqlXml("GetByIdStCompMensualEle"); }
        }

        #endregion

        public string SqlListStCompMenElePorID
        {
            get { return base.GetSqlXml("ListStCompMenElePorID"); }
        }

        public string SqlDeleteStCompmensualEleVersion
        {
            get { return base.GetSqlXml("DeleteStCompmensualEleVersion"); }
        }

    }
}
