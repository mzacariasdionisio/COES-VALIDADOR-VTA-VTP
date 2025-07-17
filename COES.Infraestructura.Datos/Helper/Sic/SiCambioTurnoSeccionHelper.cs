using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_CAMBIO_TURNO_SECCION
    /// </summary>
    public class SiCambioTurnoSeccionHelper : HelperBase
    {
        public SiCambioTurnoSeccionHelper(): base(Consultas.SiCambioTurnoSeccionSql)
        {
        }

        public SiCambioTurnoSeccionDTO Create(IDataReader dr)
        {
            SiCambioTurnoSeccionDTO entity = new SiCambioTurnoSeccionDTO();

            int iCambioturnocodi = dr.GetOrdinal(this.Cambioturnocodi);
            if (!dr.IsDBNull(iCambioturnocodi)) entity.Cambioturnocodi = Convert.ToInt32(dr.GetValue(iCambioturnocodi));

            int iNroseccion = dr.GetOrdinal(this.Nroseccion);
            if (!dr.IsDBNull(iNroseccion)) entity.Nroseccion = Convert.ToInt32(dr.GetValue(iNroseccion));

            int iDescomentario = dr.GetOrdinal(this.Descomentario);
            if (!dr.IsDBNull(iDescomentario)) entity.Descomentario = dr.GetString(iDescomentario);

            int iSeccioncodi = dr.GetOrdinal(this.Seccioncodi);
            if (!dr.IsDBNull(iSeccioncodi)) entity.Seccioncodi = Convert.ToInt32(dr.GetValue(iSeccioncodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Cambioturnocodi = "CAMBIOTURNOCODI";
        public string Nroseccion = "NROSECCION";
        public string Descomentario = "DESCOMENTARIO";
        public string Seccioncodi = "SECCIONCODI";

        #endregion
    }
}
