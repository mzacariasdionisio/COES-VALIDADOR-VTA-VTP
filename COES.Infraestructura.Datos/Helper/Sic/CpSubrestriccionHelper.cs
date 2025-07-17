using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using COES.Infraestructura.Core.Ado;
using COES.Base.Core;
using System.Data;
using COES.Dominio.DTO.Sic;

//using COES.Infraestructura.Datos.CortoPlazo.Modelo.Sql;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_SUBRESTRICCION
    /// </summary>
    public class CpSubrestriccionHelper : HelperBase
    {
        public CpSubrestriccionHelper(): base(Consultas.CpSubrestriccionSql)
        {
        }

        public CpSubrestriccionDTO Create(IDataReader dr)
        {
            CpSubrestriccionDTO entity = new CpSubrestriccionDTO();

            int iSrestriccodi = dr.GetOrdinal(this.Srestriccodi);
            if (!dr.IsDBNull(iSrestriccodi)) entity.Srestcodi = Convert.ToInt16(dr.GetValue(iSrestriccodi));

            int iRestriccodi = dr.GetOrdinal(this.Restriccodi);
            if (!dr.IsDBNull(iRestriccodi)) entity.Restriccodi = Convert.ToInt16(dr.GetValue(iRestriccodi));

            int iSrestricnombre = dr.GetOrdinal(this.Srestricnombre);
            if (!dr.IsDBNull(iSrestricnombre)) entity.Srestnombre = dr.GetString(iSrestricnombre);

            int iSrestricunidad = dr.GetOrdinal(this.Srestricunidad);
            if (!dr.IsDBNull(iSrestricunidad)) entity.Srestunidad = dr.GetString(iSrestricunidad);

            int iActivo = dr.GetOrdinal(this.Activo);
            if (!dr.IsDBNull(iActivo)) entity.Srestactivo = Convert.ToInt32(dr.GetValue(iActivo));

            int iSrestricnombregams = dr.GetOrdinal(this.Srestricnombregams);
            if (!dr.IsDBNull(iSrestricnombregams)) entity.Srestnombregams = dr.GetString(iSrestricnombregams);

            return entity;
        }


        #region Mapeo de Campos

        public string Srestriccodi = "SRESTCODI";
        public string Restriccodi = "RESTRICCODI";
        public string Srestricnombre = "SRESTNOMBRE";
        public string Srestricunidad = "SRESTUNIDAD";
        public string Activo = "SRESTACTIVO";
        public string Srestricnombregams = "SRESTNOMBREGAMS";


        #endregion

    }
}
