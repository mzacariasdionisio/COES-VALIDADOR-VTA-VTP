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
    /// Clase que contiene el mapeo de la tabla VTP_CODIGO_RETIRO_SOL_DET
    /// </summary>
    public class CodigoRetiroDetalleHelper : HelperBase
    {

        public CodigoRetiroDetalleHelper() : base(Consultas.CodigoRetiroDetalleSql)
        {
        }
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public CodigoRetiroDetalleDTO Create(IDataReader dr)
        {
            CodigoRetiroDetalleDTO entity = new CodigoRetiroDetalleDTO();

            int i = -1;

            if (columnsExist(this.CoresdCodi, dr))
            {
                 i = dr.GetOrdinal(this.CoresdCodi);
                if (!dr.IsDBNull(i)) entity.CoresdcCodi = dr.GetInt32(i);
            }

            i = -1;
            if (columnsExist(this.BarrCodi, dr))
            {
                i= dr.GetOrdinal(this.BarrCodi);
                if (!dr.IsDBNull(i)) entity.BarrCodiSum = dr.GetInt32(i);
            }
            i = -1;
            if (columnsExist(this.BarrNombre, dr))
            {
                i = dr.GetOrdinal(this.BarrNombre);
                if (!dr.IsDBNull(i)) entity.BarrNombreSuministro = dr.GetString(i);
            }

            i = -1;
            if (columnsExist(this.CoresdReg, dr))
            {
                i = dr.GetOrdinal(this.CoresdReg);
                if (!dr.IsDBNull(i)) entity.coresdRegistros = dr.GetInt32(i);
            }


            return entity;
        }

        #region Mapeo de Campos
        public string CoresoCodi = "CORESOCODI";
        public string CoresdCodi = "CORESDCODI";
        public string BarrCodi = "BARRCODI";
        public string BarrNombre = "BARRNOMBRE";
        public string CoresdReg = "CORESDREG";



        #endregion

        public string SqlListarCodigoRetiroDetalle
        {
            get { return base.GetSqlXml("ListarCodigoRetiroDetalle"); }
        }



    }

}
