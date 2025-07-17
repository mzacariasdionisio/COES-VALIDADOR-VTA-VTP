using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_EQUIUNIDBARR
    /// </summary>
    public class CaiEquiunidbarrHelper : HelperBase
    {
        public CaiEquiunidbarrHelper(): base(Consultas.CaiEquiunidbarrSql)
        {
        }

        public CaiEquiunidbarrDTO Create(IDataReader dr)
        {
            CaiEquiunidbarrDTO entity = new CaiEquiunidbarrDTO();

            int iCaiunbcodi = dr.GetOrdinal(this.Caiunbcodi);
            if (!dr.IsDBNull(iCaiunbcodi)) entity.Caiunbcodi = Convert.ToInt32(dr.GetValue(iCaiunbcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicen = dr.GetOrdinal(this.Equicodicen);
            if (!dr.IsDBNull(iEquicodicen)) entity.Equicodicen = Convert.ToInt32(dr.GetValue(iEquicodicen));

            int iEquicodiuni = dr.GetOrdinal(this.Equicodiuni);
            if (!dr.IsDBNull(iEquicodiuni)) entity.Equicodiuni = Convert.ToInt32(dr.GetValue(iEquicodiuni));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iCaiunbfecvigencia = dr.GetOrdinal(this.Caiunbfecvigencia);
            if (!dr.IsDBNull(iCaiunbfecvigencia)) entity.Caiunbfecvigencia = dr.GetDateTime(iCaiunbfecvigencia);

            int iCaiunbusucreacion = dr.GetOrdinal(this.Caiunbusucreacion);
            if (!dr.IsDBNull(iCaiunbusucreacion)) entity.Caiunbusucreacion = dr.GetString(iCaiunbusucreacion);

            int iCaiunbfeccreacion = dr.GetOrdinal(this.Caiunbfeccreacion);
            if (!dr.IsDBNull(iCaiunbfeccreacion)) entity.Caiunbfeccreacion = dr.GetDateTime(iCaiunbfeccreacion);

            int iCaiunbusumodificacion = dr.GetOrdinal(this.Caiunbusumodificacion);
            if (!dr.IsDBNull(iCaiunbusumodificacion)) entity.Caiunbusumodificacion = dr.GetString(iCaiunbusumodificacion);

            int iCaiunbfecmodificacion = dr.GetOrdinal(this.Caiunbfecmodificacion);
            if (!dr.IsDBNull(iCaiunbfecmodificacion)) entity.Caiunbfecmodificacion = dr.GetDateTime(iCaiunbfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caiunbcodi = "CAIUNBCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicen = "EQUICODICEN";
        public string Equicodiuni = "EQUICODIUNI";
        public string Barrcodi = "BARRCODI";
        public string Caiunbbarra = "CAIUNBBARRA";
        public string Caiunbfecvigencia = "CAIUNBFECVIGENCIA";
        public string Caiunbusucreacion = "CAIUNBUSUCREACION";
        public string Caiunbfeccreacion = "CAIUNBFECCREACION";
        public string Caiunbusumodificacion = "CAIUNBUSUMODIFICACION";
        public string Caiunbfecmodificacion = "CAIUNBFECMODIFICACION";

        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinombcen = "EQUINOMBCEN";
        public string Equinombuni = "EQUINOMBUNI";
        public string Ptomedielenomb = "PTOMEDIELENOMB";
        public string Barrnombre = "BARRNOMBRE";
        #endregion

        public string GetByIdBarrcodi
        {
            get { return GetSqlXml("GetByIdBarrcodi"); }
        }

        public string GetByEquicodiUNI
        {
            get { return GetSqlXml("GetByEquicodiUNI"); }
        }
    }
}
