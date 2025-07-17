using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Distribuidos.Contratos;

namespace COES.Servicios.Distribuidos.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class CostoMarginalServicio : ICostoMarginalServicio
    {
        public List<Dominio.DTO.Transferencias.CostoMarginalDTO> ListarCostosMarginales(int anio, int mes)
        {
            int periodo = Convert.ToInt32(anio.ToString() + "" + mes.ToString().PadLeft(2, '0'));
            int version = 1;
            PeriodoDTO entidadPeriodo = (new PeriodoAppServicio()).GetByAnioMes(periodo);
            List<RecalculoDTO> list = FactoryTransferencia.GetRecalculoRepository().List(entidadPeriodo.PeriCodi);
            if (list.Count > 0) version = list[0].RecaCodi;
            List<CostoMarginalDTO> listaCostoMarginal = (new CostoMarginalAppServicio()).ListCostoMarginalByBarraPeridoVersion(14, entidadPeriodo.PeriCodi, version); //Se usa la barra SANTAROSA220 de TRN_BARRA
            return listaCostoMarginal;
        }
    }
}