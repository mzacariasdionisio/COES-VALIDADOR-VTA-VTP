using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_BARRA
    /// </summary>
    public class StBarraHelper : HelperBase
    {
        public StBarraHelper(): base(Consultas.StBarraSql)
        {
        }

        public StBarraDTO Create(IDataReader dr)
        {
            StBarraDTO entity = new StBarraDTO();

            int iStbarrcodi = dr.GetOrdinal(this.Stbarrcodi);
            if (!dr.IsDBNull(iStbarrcodi)) entity.Stbarrcodi = Convert.ToInt32(dr.GetValue(iStbarrcodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iStbarrusucreacion = dr.GetOrdinal(this.Stbarrusucreacion);
            if (!dr.IsDBNull(iStbarrusucreacion)) entity.Stbarrusucreacion = dr.GetString(iStbarrusucreacion);

            int iStbarrfeccreacion = dr.GetOrdinal(this.Stbarrfeccreacion);
            if (!dr.IsDBNull(iStbarrfeccreacion)) entity.Stbarrfeccreacion = dr.GetDateTime(iStbarrfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Stbarrcodi = "STBARRCODI";
        public string Strecacodi = "STRECACODI";
        public string Barrcodi = "BARRCODI";
        public string Stbarrusucreacion = "STBARRUSUCREACION";
        public string Stbarrfeccreacion = "STBARRFECCREACION";
        //atributos de consulta
        public string Barrnomb = "BARRNOMB";
        #endregion

        ////para consultas
        //public string SqlGetBySisBarrNombre
        //{
        //    get { return base.GetSqlXml("GetBySisBarrNombre"); }
        //}

        public string SqlDeleteDstEleDet
        {
            get { return base.GetSqlXml("DeleteDstEleDet"); }
        }

        public string SqlListByStBarraVersion
        {
            get { return base.GetSqlXml("ListByStBarraVersion"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }


        
    }
}
