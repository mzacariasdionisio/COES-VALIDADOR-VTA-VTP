using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla Eq_equipo, Eq_area, Si_empresa 
    /// </summary>
    public class SiListarAreaSorteoHelper : HelperBase
    {
        public SiListarAreaSorteoHelper(): base(Consultas.SiListarAreaSorteoSql)
        {
        }

        public SiListarAreasDTO Create(IDataReader dr)
        {
            SiListarAreasDTO entity = new SiListarAreasDTO();

            int iequicodi = dr.GetOrdinal(this.equicodi);
            if (!dr.IsDBNull(iequicodi)) entity.equicodi = Convert.ToInt32(dr.GetValue(iequicodi));

            int iemprnomb = dr.GetOrdinal(this.emprnomb);
            if (!dr.IsDBNull(iemprnomb)) entity.emprnomb = dr.GetString(iemprnomb);

            int iareanomb = dr.GetOrdinal(this.areanomb);
            if (!dr.IsDBNull(iareanomb)) entity.areanomb = dr.GetString(iareanomb);

            int iequiabrev = dr.GetOrdinal(this.equiabrev);
            if (!dr.IsDBNull(iequiabrev)) entity.equiabrev = dr.GetString(iequiabrev);

            int iequipadre = dr.GetOrdinal(this.equipadre);
            if (!dr.IsDBNull(iequipadre)) entity.equipadre = Convert.ToInt32(dr.GetValue(iequipadre));
            return entity;
        }


        #region Mapeo de Campos

        public string equicodi = "equicodi";
        public string emprnomb = "emprnomb";
        public string areanomb = "areanomb";
        public string equiabrev = "equiabrev";
        public string equipadre = "equipadre";
        #endregion
    }
}
