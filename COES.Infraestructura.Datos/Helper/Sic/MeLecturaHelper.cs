using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_LECTURA
    /// </summary>
    public class MeLecturaHelper : HelperBase
    {
        public MeLecturaHelper(): base(Consultas.MeLecturaSql)
        {
        }

        public MeLecturaDTO Create(IDataReader dr)
        {
            MeLecturaDTO entity = new MeLecturaDTO();

            int iLectnro = dr.GetOrdinal(this.Lectnro);
            if (!dr.IsDBNull(iLectnro)) entity.Lectnro = Convert.ToInt32(dr.GetValue(iLectnro));

            int iLectnomb = dr.GetOrdinal(this.Lectnomb);
            if (!dr.IsDBNull(iLectnomb)) entity.Lectnomb = dr.GetString(iLectnomb);

            int iLectabrev = dr.GetOrdinal(this.Lectabrev);
            if (!dr.IsDBNull(iLectabrev)) entity.Lectabrev = dr.GetString(iLectabrev);

            int iOriglectcodi = dr.GetOrdinal(this.Origlectcodi);
            if (!dr.IsDBNull(iOriglectcodi)) entity.Origlectcodi = Convert.ToInt32(dr.GetValue(iOriglectcodi));

            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));

            int iLectperiodo = dr.GetOrdinal(this.Lectperiodo);
            if (!dr.IsDBNull(iLectperiodo)) entity.Lectperiodo = Convert.ToInt32(dr.GetValue(iLectperiodo));

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            return entity;
        }


        #region Mapeo de Campos

        public string Lectnro = "LECTNRO";
        public string Lectnomb = "LECTNOMB";
        public string Lectabrev = "LECTABREV";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Lectcodi = "LECTCODI";
        public string Lectperiodo = "LECTPERIODO";
        public string Areacode = "AREACODE";
        #endregion
    }
}
