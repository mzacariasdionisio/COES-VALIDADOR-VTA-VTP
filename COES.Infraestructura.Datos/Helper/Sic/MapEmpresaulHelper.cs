using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MAP_EMPRESAUL
    /// </summary>
    public class MapEmpresaulHelper : HelperBase
    {
        public MapEmpresaulHelper(): base(Consultas.MapEmpresaulSql)
        {
        }

        public MapEmpresaulDTO Create(IDataReader dr)
        {
            MapEmpresaulDTO entity = new MapEmpresaulDTO();

            int iEmpulcodi = dr.GetOrdinal(this.Empulcodi);
            if (!dr.IsDBNull(iEmpulcodi)) entity.Empulcodi = Convert.ToInt32(dr.GetValue(iEmpulcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEmpulfecha = dr.GetOrdinal(this.Empulfecha);
            if (!dr.IsDBNull(iEmpulfecha)) entity.Empulfecha = dr.GetDateTime(iEmpulfecha);

            int iEmpuldesv = dr.GetOrdinal(this.Empuldesv);
            if (!dr.IsDBNull(iEmpuldesv)) entity.Empuldesv = Convert.ToDecimal(dr.GetValue(iEmpuldesv));

            int iEmpulprog = dr.GetOrdinal(this.Empulprog);
            if (!dr.IsDBNull(iEmpulprog)) entity.Empulprog = Convert.ToDecimal(dr.GetValue(iEmpulprog));

            int iEmpulejec = dr.GetOrdinal(this.Empulejec);
            if (!dr.IsDBNull(iEmpulejec)) entity.Empulejec = Convert.ToDecimal(dr.GetValue(iEmpulejec));

            int iTipoccodi = dr.GetOrdinal(this.Tipoccodi);
            if (!dr.IsDBNull(iTipoccodi)) entity.Tipoccodi = Convert.ToInt32(dr.GetValue(iTipoccodi));

            int iVermcodi = dr.GetOrdinal(this.Vermcodi);
            if (!dr.IsDBNull(iVermcodi)) entity.Vermcodi = Convert.ToInt32(dr.GetValue(iVermcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Empulcodi = "EMPULCODI";
        public string Empulfecha = "EMPULFECHA";
        public string Empuldesv = "EMPULDESV";
        public string Empulprog = "EMPULPROG";
        public string Empulejec = "EMPULEJEC";
        public string Tipoccodi = "TIPOCCODI";
        public string Vermcodi = "VERMCODI";
        public string Emprcodi = "EMPRCODI";
        public string Ptomedicodi = "PTOMEDICODI";

        public string Equitension = "EQUITENSION";
        public string Equicodi = "EQUICODI";
        public string Equiabrev = "EQUIABREV";
        public string Areanomb = "AREANOMB";
        public string Emprnomb = "EMPRNOMB";
        public string Tipocdesc = "TIPOCDESC";
        public string Barrcodi = "BARRCODI";


        #endregion
    }
}
