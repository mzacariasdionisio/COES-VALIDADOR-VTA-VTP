using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.HidraulicoRPF
{
    public class RpfEnergiaPotenciaAppServicio : AppServicioBase
    {

        /// <summary>
        /// Inserta o actualiza un registro de empresas
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateRPF(RpfEnergiaPotenciaDTO entity)
        {
            try
            {
                int id = 0;


                FactorySic.GetRPFhidraulicoRepository().Update(entity);
                id = 1;

                return id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error updateapp");
                throw new Exception(ex.Message, ex);
            }
        }


        public int SaveRPF(RpfEnergiaPotenciaDTO entity)
        {
            try
            {
                int id = 0;


                FactorySic.GetRPFhidraulicoRepository().Save(entity);
                id = 1;

                return id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error updateapp");
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Elimina una empresa en base al id
        /// </summary>
        /// <param name="idEmpresa"></param>
        public void DeleteRPF(DateTime fecha)
        {
            try
            {
                Debug.WriteLine("deleterpf");
                Debug.WriteLine(fecha);
                FactorySic.GetRPFhidraulicoRepository().Delete(fecha);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener la empresa en base al id
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public RpfEnergiaPotenciaDTO GetByDateRPF(DateTime fecha)
        {
            return FactorySic.GetRPFhidraulicoRepository().GetById(fecha);
        }

        /// <summary>
        /// Permite listar todas las empresas
        /// </summary>
        /// <returns></returns>
        public List<RpfEnergiaPotenciaDTO> ListRPF(DateTime fechaini, DateTime fechafin)
        {
            return FactorySic.GetRPFhidraulicoRepository().List(fechaini, fechafin);
        }


    }
}
