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
    public class CodigoRetiroRelacionEquivalenciaHelper : HelperBase
    {
        public CodigoRetiroRelacionEquivalenciaHelper() : base(Consultas.CodigoRetiroRelacionEquivalenciasSql)
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



        public CodigoRetiroRelacionDTO Create(IDataReader dr)
        {
            CodigoRetiroRelacionDTO entity = new CodigoRetiroRelacionDTO();

            entity.RetrelCodi = Convert.ToInt32(valorReturn(dr, this.RetRelCodi) ?? 0);

            entity.EmpresaVTEA = Convert.ToString(valorReturn(dr, this.EmpResaVTEA) ?? 0);
            entity.ClienteVTEA = Convert.ToString(valorReturn(dr, this.ClienteVTEA) ?? 0);
            entity.Barrtrans = Convert.ToString(valorReturn(dr, this.BarrTrans) ?? 0);

            entity.EmpresaVTP = Convert.ToString(valorReturn(dr, this.EmpresaVTP) ?? 0);
            entity.ClienteVTP = Convert.ToString(valorReturn(dr, this.ClienteVTP) ?? 0);
            entity.Barrsum = Convert.ToString(valorReturn(dr, this.BarrSum) ?? 0);

            return entity;

        }

        public CodigoRetiroRelacionDetalleDTO CreateDetalle(IDataReader dr)
        {
            CodigoRetiroRelacionDetalleDTO entity = new CodigoRetiroRelacionDetalleDTO();
            entity.Retrelcodi = Convert.ToInt32(valorReturn(dr, this.Retrelcodi) ?? 0);
            entity.Rerldtcodi = Convert.ToInt32(valorReturn(dr, this.Rerldtcodi) ?? 0);
            entity.Tipocontratovtea = Convert.ToString(valorReturn(dr, this.Tipocontratovtea));
            entity.Tipousuariovtea = Convert.ToString(valorReturn(dr, this.Tipousuariovtea));
            entity.Codigovtea = Convert.ToString(valorReturn(dr, this.Codigovtea));
            entity.Tipocontratovtp = Convert.ToString(valorReturn(dr, this.Tipocontratovtp) ?? entity.Tipocontratovtea);
            entity.Tipousuariovtp = Convert.ToString(valorReturn(dr, this.Tipousuariovtp) ?? entity.Tipousuariovtea);
            entity.Codigovtp = Convert.ToString(valorReturn(dr, this.Codigovtp) ?? "");

            return entity;
        }


        #region Mapeo de Campos

        public string RetRelCodi = "RETRELCODI";
        public string RetrelVari = "RETRELVARI";
        public string RetelEstado = "RETELESTADO";
        public string EmpResaVTEA = "EMPRESAVTEA";
        public string ClienteVTEA = "CLIENTEVTEA";
        public string BarrTrans = "BARRTRANS";
        public string RetrelUsuCreacion = "RETRELUSUCREACION";
        public string RetrelFecCreacion = "RETRELFECCREACION";
        public string RetrelUsuModificacion = "RETRELUSUMODIFICACION";
        public string RetrelFecModificacion = "RETRELFECMODIFICACION";

        public string EmpresaVTP = "EMPRESAVTP";
        public string ClienteVTP = "CLIENTEVTP";
        public string BarrSum = "BARRSUM";

        public string NroPagina = "NROPAGINA";
        public string PageSize = "PAGESIZE";

        //filtro
        public string Genemprcodi = "GENEMPRCODI";
        public string Cliemprcodi = "CLIEMPRCODI";
        public string Barrcoditra = "BARRCODITRA";
        public string Barrcodisum = "BARRCODISUM";
        public string Tipconcodi = "TIPCONCODI";
        public string Tipusucodi = "TIPUSUCODI";
        public string Codigo = "CODIGO";



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

        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Emprcodi = "EMPRCODI";



        #endregion

        #region xml
        public string SqlListarRelacionCodigoRetiros
        {
            get { return base.GetSqlXml("ListarRelacionCodigoRetiros"); }
        }
        public string SqlListarRelacionCodigoRetirosPorCodigo
        {
            get { return base.GetSqlXml("ListarRelacionCodigoRetirosPorCodigo"); }
        }
        public string SqlTotalRecordsRelacionCodigoRetiros
        {
            get { return base.GetSqlXml("TotalRecordsRelacionCodigoRetiros"); }
        }

        public string SqlGetPoteCoincidenteByCodigoVtp
        {
            get { return base.GetSqlXml("GetPoteCoincidenteByCodigoVtp"); }
        }

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlCGetById
        {
            get { return base.GetSqlXml("GetById"); }
        }

        #endregion
    }





}
