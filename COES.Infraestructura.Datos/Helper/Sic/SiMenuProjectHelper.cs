using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_MENU_PROJECT
    /// </summary>
    public class SiMenuProjectHelper : HelperBase
    {
        public SiMenuProjectHelper(): base(Consultas.SiMenuProjectSql)
        {
        }

        public SiMenuProjectDTO Create(IDataReader dr)
        {
            SiMenuProjectDTO entity = new SiMenuProjectDTO();

            int iMprojcodi = dr.GetOrdinal(this.Mprojcodi);
            if (!dr.IsDBNull(iMprojcodi)) entity.Mprojcodi = Convert.ToInt32(dr.GetValue(iMprojcodi));

            int iMprojdescripcion = dr.GetOrdinal(this.Mprojdescripcion);
            if (!dr.IsDBNull(iMprojdescripcion)) entity.Mprojdescripcion = dr.GetString(iMprojdescripcion);

            return entity;
        }


        #region Mapeo de Campos

        public string Mprojcodi = "MPROJCODI";
        public string Mprojdescripcion = "MPROJDESCRIPCION";

        #endregion
    }
}
