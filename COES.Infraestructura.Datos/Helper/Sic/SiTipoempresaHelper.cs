using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_TIPOEMPRESA
    /// </summary>
    public class SiTipoempresaHelper : HelperBase
    {
        public SiTipoempresaHelper(): base(Consultas.SiTipoempresaSql)
        {
        }

        public SiTipoempresaDTO Create(IDataReader dr)
        {
            SiTipoempresaDTO entity = new SiTipoempresaDTO();

            int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

            int iTipoemprdesc = dr.GetOrdinal(this.Tipoemprdesc);
            if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

            int iTipoemprabrev = dr.GetOrdinal(this.Tipoemprabrev);
            if (!dr.IsDBNull(iTipoemprabrev)) entity.Tipoemprabrev = dr.GetString(iTipoemprabrev);

            return entity;
        }


        #region Mapeo de Campos

        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Tipoemprdesc = "TIPOEMPRDESC";
        public string Tipoemprabrev = "TIPOEMPRABREV";


        public string SqlObtenerTiposEmpresaContacto
        {
            get { return base.GetSqlXml("ObtenerTiposEmpresaContacto"); }
        }

        #endregion
    }
}

