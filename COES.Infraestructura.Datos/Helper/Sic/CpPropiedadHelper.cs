using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using System.Data;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_PROPIEDAD
    /// </summary>
    public class CpPropiedadHelper : HelperBase
    {
        public CpPropiedadHelper(): base(Consultas.CpPropiedadSql)
        {
        }

        public CpPropiedadDTO Create(IDataReader dr)
        {
            CpPropiedadDTO entity = new CpPropiedadDTO();

            int iPropcodi = dr.GetOrdinal(this.Propcodi);
            if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

            int iPropabrev = dr.GetOrdinal(this.Propabrev);
            if (!dr.IsDBNull(iPropabrev)) entity.Propabrev = dr.GetString(iPropabrev);

            int iPropunidad = dr.GetOrdinal(this.Propunidad);
            if (!dr.IsDBNull(iPropunidad)) entity.Propunidad = dr.GetString(iPropunidad);

            int iCatcodi = dr.GetOrdinal(this.Catcodi);
            if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt32(dr.GetValue(iCatcodi));

            int iProptipo = dr.GetOrdinal(this.Proptipo);
            if (!dr.IsDBNull(iProptipo)) entity.Proptipo = dr.GetString(iProptipo);

            int iPropnombre = dr.GetOrdinal(this.Propnombre);
            if (!dr.IsDBNull(iPropnombre)) entity.Propnombre = dr.GetString(iPropnombre);

            int iProporden = dr.GetOrdinal(this.Proporden);
            if (!dr.IsDBNull(iProporden)) entity.Proporden = Convert.ToInt16(dr.GetValue(iProporden));

            int iPropcodisicoes = dr.GetOrdinal(this.Propcodisicoes);
            if (!dr.IsDBNull(iPropcodisicoes)) entity.Propcodisicoes = Convert.ToInt32(dr.GetValue(iPropcodisicoes));

            int iPropabrevgams = dr.GetOrdinal(this.Propabrevgams);
            if (!dr.IsDBNull(iPropabrevgams)) entity.Propabrevgams = dr.GetString(iPropabrevgams);

            return entity;
        }


        #region Mapeo de Campos

        public string Propcodi = "PROPCODI";
        public string Propabrev = "PROPABREV";
        public string Propunidad = "PROPUNIDAD";
        public string Catcodi = "CATCODI";
        public string Proptipo = "PROPTIPO";
        public string Propnombre = "PROPNOMBRE";
        public string Proporden = "PROPORDEN";
        public string Propcodisicoes = "PROPCODISICOES";
        public string Propabrevgams = "PROPABREVGAMS";


        #endregion
    }
}
