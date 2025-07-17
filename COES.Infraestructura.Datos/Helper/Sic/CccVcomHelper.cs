using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CCC_VCOM
    /// </summary>
    public class CccVcomHelper : HelperBase
    {
        public CccVcomHelper() : base(Consultas.CccVcomSql)
        {
        }

        public CccVcomDTO Create(IDataReader dr)
        {
            CccVcomDTO entity = new CccVcomDTO();

            int iVcomcodi = dr.GetOrdinal(this.Vcomcodi);
            if (!dr.IsDBNull(iVcomcodi)) entity.Vcomcodi = Convert.ToInt32(dr.GetValue(iVcomcodi));

            int iCccvercodi = dr.GetOrdinal(this.Cccvercodi);
            if (!dr.IsDBNull(iCccvercodi)) entity.Cccvercodi = Convert.ToInt32(dr.GetValue(iCccvercodi));

            int iFenergcodi = dr.GetOrdinal(this.Fenergcodi);
            if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEqupadre = dr.GetOrdinal(this.Equipadre);
            if (!dr.IsDBNull(iEqupadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEqupadre));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iVcomvalor = dr.GetOrdinal(this.Vcomvalor);
            if (!dr.IsDBNull(iVcomvalor)) entity.Vcomvalor = dr.GetDecimal(iVcomvalor);

            int iVcomcodigomop = dr.GetOrdinal(this.Vcomcodigomop);
            if (!dr.IsDBNull(iVcomcodigomop)) entity.Vcomcodigomop = dr.GetString(iVcomcodigomop);

            int iVcomcodigotcomb = dr.GetOrdinal(this.Vcomcodigotcomb);
            if (!dr.IsDBNull(iVcomcodigotcomb)) entity.Vcomcodigotcomb = dr.GetString(iVcomcodigotcomb);

            int iTinfcoes = dr.GetOrdinal(this.Tinfcoes);
            if (!dr.IsDBNull(iTinfcoes)) entity.Tinfcoes = Convert.ToInt32(dr.GetValue(iTinfcoes));

            int iTinfosi = dr.GetOrdinal(this.Tinfosi);
            if (!dr.IsDBNull(iTinfosi)) entity.Tinfosi = Convert.ToInt32(dr.GetValue(iTinfosi));

            return entity;
        }

        #region Mapeo de Campos

        public string Vcomcodi = "VCOMCODI";
        public string Cccvercodi = "CCCVERCODI";
        public string Fenergcodi = "FENERGCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equipadre = "EQUIPADRE";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Vcomvalor = "VCOMVALOR";
        public string Vcomcodigomop = "VCOMCODIGOMOP";
        public string Vcomcodigotcomb = "VCOMCODIGOTCOMB";
        public string Tinfcoes = "TINFCOES";
        public string Tinfosi = "TINFOSI";

        public string Emprabrev = "EMPRABREV";
        public string Emprnomb = "EMPRNOMB";
        public string Central = "CENTRAL";
        public string Equinomb = "EQUINOMB";
        public string Gruponomb = "GRUPONOMB";
        public string Fenergnomb = "Fenergnomb";
        public string Tinfcoesabrev = "TINFCOESABREV";
        public string Tinfosiabrev = "TINFOSIABREV";

        #endregion
    }
}
