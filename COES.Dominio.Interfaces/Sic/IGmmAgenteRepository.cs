using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla GMME_EMPRESA
    /// </summary>
    public interface IGmmAgenteRepository
    {
        int Save(GmmEmpresaDTO entity);
        void Update(GmmEmpresaDTO entity);
        bool Delete(GmmEmpresaDTO entity);
        GmmEmpresaDTO GetById(int empgcodi);
        GmmEmpresaDTO GetByIdEdit(int empgcodi);
        List<GmmEmpresaDTO> ListarFiltroAgentes(string razonSocial, string documento, string tipoParticipante, string tipoModalidad,
            string fecIni, string fecFin, string estado, bool dosMasIncumplimientos);
        List<GmmEmpresaDTO> ListarModalidades(int empgcodi);
        List<GmmEmpresaDTO> ListarEstados(int empgcodi);
        List<GmmEmpresaDTO> ListarIncumplimientos(int empgcodi);

        List<GmmEmpresaDTO> ListarAgentes(string razonsocial);
        List<GmmEmpresaDTO> ListarMaestroEmpresas(string razonsocial, string estadoRegistro);
        List<GmmEmpresaDTO> ListarEmpresasParticipantes(string razonsocial, string estadoRegistro);
        List<GmmEmpresaDTO> ListarMaestroEmpresasCliente(string razonsocial, string estadoRegistro);
        List<GmmEmpresaDTO> ListarAgentesParaCalculo(int anio, int mes, int formatotrimestre, int formatomes, int tipoEnergia);  
        List<GmmEmpresaDTO> ListarAgentesEntregaParaCalculo(int anio, int mes, int formatotrimestre, int formatomes, int tipoEnergia);
        GmmGarantiaDTO ObtieneGarantiaById(int garacodi);

    }
}
