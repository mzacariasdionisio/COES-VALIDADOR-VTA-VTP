using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_URS_MODO_OPERACION
    /// </summary>
    public class SmaUserEmpresaHelper : HelperBase
    {
        public SmaUserEmpresaHelper(): base(Consultas.SmaUserEmpresaSql)
        {
        }

        public SmaUserEmpresaDTO Create(IDataReader dr)
        {
            SmaUserEmpresaDTO entity = new SmaUserEmpresaDTO();

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iUsername = dr.GetOrdinal(this.Username);
            if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            return entity;
        }


        #region Mapeo de Campos

        public string Usercode = "USERCODE";
        public string Emprcodi = "EMPRCODI";

        public string Username = "USERNAME";
        public string Emprnomb = "EMPRNOMB";

        #endregion


        public string SqlListEmpresa
        {
            get { return GetSqlXml("ListEmpresa"); }
        }

    }
}
