using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class StockCombustibleAppServicio
    {
        /// <summary>
        /// Permite obtener el reporte de stock de combustible
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="empresas"></param>
        /// <param name="grupos"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> ObtenerConsultaStock(DateTime fechaInicio, DateTime fechaFin,
            int? idEmpresa, int? idGrupo, int? idTipoCombustible)
        {
            List<MeMedicion1DTO> estructura = this.ObtenerEstructura(idEmpresa, idGrupo, idTipoCombustible);
            List<MeMedicion1DTO> resultado = FactorySic.GetMeMedicion1Repository().GetByCriteria2(fechaInicio, fechaFin, idEmpresa, idGrupo, idTipoCombustible);
            List<MeMedicion1DTO> list = new List<MeMedicion1DTO>();

            int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

            for (int i = 0; i <= nroDias; i++ )
            {
                DateTime fecha = fechaInicio.AddDays(i);

                List<MeMedicion1DTO> subList = resultado.Where(x => x.Medifecha.Year == fecha.Year && x.Medifecha.Month == fecha.Month &&
                    x.Medifecha.Day == fecha.Day).ToList();                
                List<MeMedicion1DTO> noIncluido = estructura.Where(x => !subList.Any(y => x.Ptomedicodi == y.Ptomedicodi && x.Tipoinfocodi == y.Tipoinfocodi)).Distinct().ToList();

                foreach (MeMedicion1DTO entity in noIncluido)
                {
                    MeMedicion1DTO item = new MeMedicion1DTO();
                    item.Emprnomb = entity.Emprnomb;
                    item.Grupocodi = entity.Grupocodi;
                    item.Equicodi = entity.Equicodi;
                    item.Gruponomb = entity.Gruponomb;
                    item.H1 = entity.H1;
                    item.Nota = entity.Nota;
                    item.IndInformo = entity.IndInformo;
                    item.Lectcodi = entity.Lectcodi;
                    item.Medifecha = fecha;
                    item.Ptomedicodi = entity.Ptomedicodi;
                    item.Tipoinfocodi = entity.Tipoinfocodi;
                    item.Tipoinfodesc = entity.Tipoinfodesc;

                    list.Add(item);

                }

                list.AddRange(subList);
                //list.AddRange(noIncluido);
            }

            return list.OrderBy(x => x.Medifecha).ThenBy(x => x.Emprnomb).ThenBy(x => x.Gruponomb).ThenBy(x => x.Tipoinfodesc).ToList();
        }


        #region Assetec[IND.PR25.2022]
        /// <summary>
        /// Permite obtener el reporte de stock de combustible
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idGrupo"></param>
        /// <param name="idTipoCombustible"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> ObtenerConsultaStockIndisponibilidades(DateTime fechaInicio, DateTime fechaFin,
            int? idEmpresa, int? idGrupo, int? idTipoCombustible)
        {
            List<MeMedicion1DTO> estructura = this.ObtenerEstructura(idEmpresa, idGrupo, idTipoCombustible);
            //List<MeMedicion1DTO> resultado = FactorySic.GetMeMedicion1Repository().GetByCriteria2(fechaInicio, fechaFin, idEmpresa, idGrupo, idTipoCombustible);
            List<IndStockCombustibleDTO> resultado = FactorySic.GetIndStockCombustibleRepository().ListStockByAnioMes(fechaInicio.Year, fechaInicio.Month);
            List<MeMedicion1DTO> list = new List<MeMedicion1DTO>();

            int nroDias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

            for (int i = 0; i <= nroDias; i++)
            {
                DateTime fecha = fechaInicio.AddDays(i);

                //List<MeMedicion1DTO> subList = resultado.Where(x => x.Medifecha.Year == fecha.Year && x.Medifecha.Month == fecha.Month &&
                //    x.Medifecha.Day == fecha.Day).ToList();
                List<MeMedicion1DTO> noIncluido = estructura.Where(x => !resultado.Any(y => x.Ptomedicodi == y.Ptomedicodi && x.Tipoinfocodi == y.Tipoinfocodi)).Distinct().ToList();

                foreach (MeMedicion1DTO entity in noIncluido)
                {
                    MeMedicion1DTO item = new MeMedicion1DTO();
                    item.Equicodi = entity.Equicodi;
                    item.H1 = entity.H1;
                    item.Medifecha = fecha;
                    item.Ptomedicodi = entity.Ptomedicodi;
                    item.Tipoinfocodi = entity.Tipoinfocodi;
                    list.Add(item);
                }

                List<MeMedicion1DTO> temporal = new List<MeMedicion1DTO>();
                foreach (IndStockCombustibleDTO entity in resultado)
                {
                    MeMedicion1DTO item = new MeMedicion1DTO();
                    item.Equicodi = entity.Equicodicentral;
                    string cadena = entity.GetType().GetProperty("D" + (i + 1).ToString()).GetValue(entity, null).ToString();
                    decimal value;
                    bool isDecimal = decimal.TryParse(cadena, out value);
                    //item.H1 = decimal.Parse(entity.GetType().GetProperty("D" + (i + 1).ToString()).GetValue(entity, null).ToString());
                    item.H1 = value;
                    item.Medifecha = fecha;
                    item.Ptomedicodi = entity.Ptomedicodi;
                    item.Tipoinfocodi = entity.Tipoinfocodi;
                    temporal.Add(item);
                }

                list.AddRange(temporal);
                //list.AddRange(noIncluido);
            }

            return list.OrderBy(x => x.Medifecha).ThenBy(x => x.Emprnomb).ThenBy(x => x.Gruponomb).ThenBy(x => x.Tipoinfodesc).ToList();
        }
        #endregion

        /// <summary>
        /// Permite obtener las empresas del stock de combustible
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasStock()
        {
            return FactorySic.GetMeMedicion1Repository().ObtenerEmpresasStock();
        }

        /// <summary>
        /// Permite obtener los grupos del stock de combustible
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ObtenerGruposStock(int idEmpresa)
        {
            return FactorySic.GetMeMedicion1Repository().ObtenerGruposStock(idEmpresa);
        }

        /// <summary>
        /// Permite listar los tipos de combustibles
        /// </summary>
        /// <returns></returns>
        public List<MeMedicion1DTO> ObtenerTipoCombustible()
        {
            return FactorySic.GetMeMedicion1Repository().ObtenerTipoCombustible();
        }

        /// <summary>
        /// Permite obtener la estructura 
        /// </summary>
        /// <returns></returns>
        public List<MeMedicion1DTO> ObtenerEstructura(int? idEmpresa, int? idGrupo, int? idTipoCombustible)
        {
            return FactorySic.GetMeMedicion1Repository().ObtenerEstructura(idEmpresa, idGrupo, idTipoCombustible);
        }
    }
}
