using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_CABECERA
    /// </summary>
    public class MeCabeceraHelper : HelperBase
    {
        public MeCabeceraHelper(): base(Consultas.MeCabeceraSql)
        {
        }

        public MeCabeceraDTO Create(IDataReader dr)
        {
            MeCabeceraDTO entity = new MeCabeceraDTO();

            int iCabcodi = dr.GetOrdinal(this.Cabcodi);
            if (!dr.IsDBNull(iCabcodi)) entity.Cabcodi = Convert.ToInt32(dr.GetValue(iCabcodi));

            int iCabquery = dr.GetOrdinal(this.Cabquery);
            if (!dr.IsDBNull(iCabquery)) entity.Cabquery = dr.GetString(iCabquery);

            int iCabfilas = dr.GetOrdinal(this.Cabfilas);
            if (!dr.IsDBNull(iCabfilas)) entity.Cabfilas = Convert.ToInt32(dr.GetValue(iCabfilas));

            int iCabcampodef = dr.GetOrdinal(this.Cabcampodef);
            if (!dr.IsDBNull(iCabcampodef)) entity.Cabcampodef = dr.GetString(iCabcampodef);

            int iCabcolumnas = dr.GetOrdinal(this.Cabcolumnas);
            if (!dr.IsDBNull(iCabcolumnas)) entity.Cabcolumnas = Convert.ToInt32(dr.GetValue(iCabcolumnas));

            int iCabdescrip = dr.GetOrdinal(this.Cabdescrip);
            if (!dr.IsDBNull(iCabdescrip)) entity.Cabdescrip = dr.GetString(iCabdescrip);

            int iCabfilasocultas = dr.GetOrdinal(this.Cabfilasocultas);
            if (!dr.IsDBNull(iCabfilasocultas)) entity.Cabfilasocultas = dr.GetString(iCabfilasocultas);

            return entity;
        }


        #region Mapeo de Campos

        public string Cabcodi = "CABCODI";
        public string Cabquery = "CABQUERY";
        public string Cabfilas = "CABFILAS";
        public string Cabcampodef = "CABCAMPODEF";
        public string Cabcolumnas = "CABCOLUMNAS";
        public string Cabdescrip = "CABDESCRIP";
        public string Cabfilasocultas = "CABFILASOCULTAS";

        #endregion
    }
}
