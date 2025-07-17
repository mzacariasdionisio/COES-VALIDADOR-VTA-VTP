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
    public class CodigoRetiroEquivalenciaDetalleHelper : HelperBase
    {
        public CodigoRetiroEquivalenciaDetalleHelper() : base(Consultas.CodigoRetiroEquivalenciaDetalleSql)
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

        private object valorReturn(IDataReader dr, string sColumna)
        {
            object resultado = null;
            int iIndex;
            if (columnsExist(sColumna, dr))
            {
                iIndex = dr.GetOrdinal(sColumna);
                if (!dr.IsDBNull(iIndex)) resultado = dr.GetValue(iIndex);
            }
            return resultado;
        }

        public CodigoRetiroRelacionDetalleDTO Create(IDataReader dr)
        {
            CodigoRetiroRelacionDetalleDTO entity = new CodigoRetiroRelacionDetalleDTO();
            entity.Retrelcodi = Convert.ToInt32(valorReturn(dr, this.Retrelcodi) ?? 0);
            entity.Rerldtcodi = Convert.ToInt32(valorReturn(dr, this.Rerldtcodi) ?? 0);
            entity.Tipocontratovtea = Convert.ToString(valorReturn(dr, this.Tipocontratovtea));
            entity.Tipousuariovtea = Convert.ToString(valorReturn(dr, this.Tipousuariovtea) );
            entity.Codigovtea = Convert.ToString(valorReturn(dr, this.Codigovtea));

            entity.Tipocontratovtp = Convert.ToString(valorReturn(dr, this.Tipocontratovtp) ?? entity.Tipocontratovtea);
            entity.Tipousuariovtp = Convert.ToString(valorReturn(dr, this.Tipousuariovtp) ?? entity.Tipousuariovtea);
            entity.Codigovtp = Convert.ToString(valorReturn(dr, this.Codigovtp) ?? "");



            return entity;
        }
        #region Mapeo de Campos

        public string Retrelcodi = "RETRELCODI";
        public string Rerldtcodi = "RERLDTCODI";

        public string Genemprcodivtea = "GENEMPRCODIVTEA";
        public string Cliemprcodivtea = "CLIEMPRCODIVTEA";
        public string Tipconcodivtea = "TIPCONCODIVTEA";
        public string Tipusuvtea = "TIPUSUVTEA";
        public string Barrcodivtea = "BARRCODIVTEA";
        public string Coresocodvtea = "CORESOCODVTEA";

        public string Genemprcodivtp = "GENEMPRCODIVTP";
        public string Cliemprcodivtp = "CLIEMPRCODIVTP";
        public string Tipconcodivtp = "TIPCONCODIVTP";
        public string Tipusuvtp = "TIPUSUVTP";
        public string Barrcodivtp = "BARRCODIVTP";
        public string Coresocodvtp = "CORESOCODVTP";

        public string Rerldtestado = "RERLDESTADO";
        public string Rerldtusucreacion = "RERDRUSUCREACION";
        public string Rerldtfeccreacion = "RERDTFECCREACION";

        //para la vista
        public string Genemprnombvtea = "GENEMPRNOMBVTEA";
        public string Cliemprnombvtea = "CLIEMPRNOMBVTEA";
        public string Tipocontratovtea = "TIPOCONTRATOVTEA";
        public string Tipousuariovtea = "TIPOUSUARIOVTEA";
        public string Barrnombvtea = "BARRNOMBVTEA";
        public string Codigovtea = "CODIGOVTEA";

        public string Genemprnombvtp = "GENEMPRNOMBVTP";
        public string Cliemprnombvtp = "CLIEMPRNOMBVTP";
        public string Tipocontratovtp = "TIPOCONTRATOVTP";
        public string Tipousuariovtp = "TIPOUSUARIOVTP";
        public string Barrnombvtp = "BARRNOMBVTP";
        public string Codigovtp = "CODIGOVTP";

        #endregion


        #region xml
        public string SqlListarRelacionDetalleCodigoRetiros
        {
            get { return base.GetSqlXml("ListarRelacionDetalleCodigoRetiros"); }
        }

        public string SqlListarRelacionDetalle
        {
            get { return base.GetSqlXml("ListarRelacionDetalle"); }
        }

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlExisteVTEA
        {
            get { return base.GetSqlXml("ExisteVTEA"); }
        }

        public string SqlExisteVTP
        {
            get { return base.GetSqlXml("ExisteVTP"); }
        }

        #endregion

    }
}
