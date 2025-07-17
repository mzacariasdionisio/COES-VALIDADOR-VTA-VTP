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
    /// Clase que contiene el mapeo de la vista VW_EQ_AREA
    /// </summary>
    public class AreaHelper : HelperBase
    {
        public AreaHelper() : base(Consultas.AreaSql)
        {
        }

        public string SqlListArea
        {
            get { return base.GetSqlXml("ListArea"); }
        }

        public string SqlListAreaProceso
        {
            get { return base.GetSqlXml("ListAreaProceso"); }
        }
        

        public AreaDTO Create(IDataReader dr)
        {
            AreaDTO entity = new AreaDTO();

            int iAreaCodi = dr.GetOrdinal(this.AreaCodi);
            if (!dr.IsDBNull(iAreaCodi)) entity.AreaCodi = dr.GetInt32(iAreaCodi);

            int iAreaNombre = dr.GetOrdinal(this.AreaNombre);
            if (!dr.IsDBNull(iAreaNombre)) entity.AreaNombre = dr.GetString(iAreaNombre);

            return entity;
        }
        #region Mapeo de Campos

        public string AreaCodi = "AREACODI";
        public string AreaNombre = "AREANOMB";

        #endregion

    }
}
