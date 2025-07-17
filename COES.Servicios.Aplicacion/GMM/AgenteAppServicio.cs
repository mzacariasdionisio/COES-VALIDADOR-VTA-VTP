using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.GMM
{
    /// <summary>
    /// Clases con métodos del módulo GMME - Agentes
    /// </summary>
    public class AgenteAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AgenteAppServicio));


        /// <summary>
        /// Inserta un registro de la tabla ...
        /// </summary>
        public GmmEmpresaDTO Save(GmmEmpresaDTO entity)
        {
            try
            {
                //using (TransactionScope trx =)
                //{
                int _empgcodi = FactorySic.GetGmmAgentesRepository().Save(entity);
                if (_empgcodi > 0)
                {
                    entity.EMPGCODI = _empgcodi;
                    //GmmGarantiaDTO oGarantia = new GmmGarantiaDTO();
                    //oGarantia.EMPGCODI = _empgcodi;
                    //FactorySic.GetGmmGarantiaRepository().Save(oGarantia);

                    // insertar el log del  estado
                    GmmEstadoEmpresaDTO oEstadoEmpresa = new GmmEstadoEmpresaDTO();
                    oEstadoEmpresa.ESTFECREGISTRO = DateTime.Now;
                    oEstadoEmpresa.ESTESTADO = entity.EMPGESTADO;
                    oEstadoEmpresa.ESTUSUEDICION = entity.EMPGUSUCREACION;
                    oEstadoEmpresa.EMPGCODI = _empgcodi;
                    FactorySic.GetGmmEstadoEmpresaRepository().Save(oEstadoEmpresa);

                    // Crear la casilla del trienio
                    GmmTrienioDTO oTrienio = new GmmTrienioDTO();
                    oTrienio.EMPGCODI = _empgcodi;
                    oTrienio.TRINUMINC = 0;
                    oTrienio.TRISECUENCIA = 1;
                    oTrienio.TRIFECINICIO = DateTime.Now;
                    oTrienio.TRIFECLIMITE = DateTime.Now.AddDays(1080);
                    FactorySic.GetGmmTrienioRepository().Save(oTrienio);

                }
                //}

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return entity;
        }

        /// <summary>
        /// Actualiza un registro de la tabla ...
        /// </summary>
        public GmmEmpresaDTO Update(GmmEmpresaDTO entity)
        {
            try
            {
                FactorySic.GetGmmAgentesRepository().Update(entity);
                // insertar el log del  estado si es que este ha cambiado
                if (entity.flagcambioEstado)
                {
                    GmmEstadoEmpresaDTO oEstadoEmpresa = new GmmEstadoEmpresaDTO();
                    oEstadoEmpresa.ESTFECREGISTRO = DateTime.Now;
                    oEstadoEmpresa.ESTESTADO = entity.EMPGESTADO;
                    oEstadoEmpresa.ESTUSUEDICION = entity.EMPGUSUCREACION;
                    oEstadoEmpresa.EMPGCODI = entity.EMPGCODI;
                    FactorySic.GetGmmEstadoEmpresaRepository().Save(oEstadoEmpresa);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
            return entity;
        }

        /// <summary>
        /// Elimina un registro de la tabla ...
        /// </summary>
        public bool Delete(GmmEmpresaDTO entity)
        {
            try
            {
                return FactorySic.GetGmmAgentesRepository().Delete(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ...
        /// </summary>
        public GmmEmpresaDTO GetById(int empgcodi)
        {
            return FactorySic.GetGmmAgentesRepository().GetById(empgcodi);
        }

        /// <summary>
        /// Permite obtener los datos de una empresa para edición
        /// </summary>
        public GmmEmpresaDTO GetByIdEdit(int empgcodi)
        {
            return FactorySic.GetGmmAgentesRepository().GetByIdEdit(empgcodi);
        }

        /// <summary>
        /// ListarAgentes
        /// </summary>
        /// <param name="razonSocial"></param>
        /// <param name="documento"></param>
        /// <param name="tipoParticipante"></param>
        /// <param name="tipoModalidad"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <param name="estado"></param>
        /// <param name="dosMasIncumplimientos"></param>
        /// <returns></returns>
        public List<GmmEmpresaDTO> ListarAgentes(string razonSocial, string documento, string tipoParticipante, string tipoModalidad,
            string fecIni, string fecFin, string estado, bool dosMasIncumplimientos)
        {
            return FactorySic.GetGmmAgentesRepository().ListarFiltroAgentes(razonSocial, documento, tipoParticipante, tipoModalidad,
             fecIni, fecFin, estado, dosMasIncumplimientos);
        }
        public List<GmmEmpresaDTO> ListarModalidades(int empgcodi)
        {
            return FactorySic.GetGmmAgentesRepository().ListarModalidades(empgcodi);
        }
        public List<GmmEmpresaDTO> ListarEstados(int empgcodi)
        {
            return FactorySic.GetGmmAgentesRepository().ListarEstados(empgcodi);
        }
        public List<GmmEmpresaDTO> ListarIncumplimientos(int empgcodi)
        {
            return FactorySic.GetGmmAgentesRepository().ListarIncumplimientos(empgcodi);
        }

        public List<GmmEmpresaDTO> ListarAgentesSelect(string razonSocial)
        {
            return FactorySic.GetGmmAgentesRepository().ListarAgentes(razonSocial);
        }

        /// <summary>
        /// Permite listar las empresas para rechazo carga por Tipo y Estado
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="tipoEmpresa"></param>
        /// <param name="estadoRegistro"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresasRechazoCarga(string empresa, int tipoEmpresa, string estadoRegistro)
        {
            return FactorySic.GetSiEmpresaRepository().ListaEmpresasRechazoCarga(empresa, tipoEmpresa, estadoRegistro);
        }

        public List<GmmEmpresaDTO> ListarMaestroEmpresa(String razonsocial, string estadoRegistro)
        {
            return FactorySic.GetGmmAgentesRepository().ListarMaestroEmpresas(razonsocial, estadoRegistro);
        }

        public List<GmmEmpresaDTO> ListarEmpresasParticipantes(String razonsocial, string estadoRegistro)
        {
            return FactorySic.GetGmmAgentesRepository().ListarEmpresasParticipantes(razonsocial, estadoRegistro);
        }

        #region Calculo

        public List<GmmEmpresaDTO> listarAgentesCalculo(int anio, int mes, int formatotrimestre, int formatomes, int tipoEnergia)
        {
            return FactorySic.GetGmmAgentesRepository().ListarAgentesParaCalculo(anio, mes, formatotrimestre, formatomes, tipoEnergia);

        }

        

        public List<GmmEmpresaDTO> listarAgentesEntregaCalculo(int anio, int mes, int formatotrimestre, int formatomes, int tipoEnergia)
        {
            return FactorySic.GetGmmAgentesRepository().ListarAgentesEntregaParaCalculo(anio, mes, formatotrimestre, formatomes, tipoEnergia);

        }



        #endregion

        #region Mantenimiento Garantia - Modalidad
        public GmmGarantiaDTO ObtieneGarantiaById(int empgcodi)
        {
            return FactorySic.GetGmmAgentesRepository().ObtieneGarantiaById(empgcodi);
        }
        #endregion

    }
}
