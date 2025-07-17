using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTP_PEAJE_EGRESO_MINFO
    /// </summary> 
    public interface IVtpPeajeEgresoMinfoRepository
    {
        int Save(VtpPeajeEgresoMinfoDTO entity);
        void Update(VtpPeajeEgresoMinfoDTO entity);
        void Delete(int pegrmicodi);
        VtpPeajeEgresoMinfoDTO GetById(int pegrmicodi);
        List<VtpPeajeEgresoMinfoDTO> List(int pericodi, int recpotcodi);
        List<VtpPeajeEgresoMinfoDTO> GetByCriteria(int pericodi, int recpotcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);
        List<VtpPeajeEgresoMinfoDTO> ListCabecera(int pericodi, int recpotcodi, int recacodi);
        List<VtpPeajeEgresoMinfoDTO> ListEmpresa(int pericodi, int recpotcodi, int emprcodi);
        List<VtpPeajeEgresoMinfoDTO> GetByCriteriaVista(int pericodi, int recpotcodi, int emprcodi, int cliemprcodi, int barrcodi, int barrcodifco, string pegrmitipousuario, string pegrmilicitacion, string pegrmicalidad, string pegrmicalidad2);
        List<VtpPeajeEgresoMinfoDTO> ListPotenciaValor(int pericodi, int recpotcodi);
        List<VtpPeajeEgresoMinfoDTO> GetByCriteriaInfoFaltante(int pericodi, int recpotcodi, int pericodianterior, int recpotcodianterior);
        List<VtpPeajeEgresoMinfoDTO> ListCabeceraRecalculo(int pericodi, int recpotcodi);
        List<VtpPeajeEgresoMinfoDTO> ListEmpresaRecalculo(int pericodi, int recpotcodi, int emprcodi);
        List<CodigoConsolidadoDTO> ListarCodigosVTP(int emprCodi);
        List<VtpPeajeEgresoMinfoDTO> ListarCodigosByCriteria(int emprcodi, int cliemprcodi, int barrcodi, string pegrmitipousuario, int pericodi, string pgrmilicitacion);
        List<VtpPeajeEgresoMinfoDTO> GetByCriteriaVistaNuevo(int pericodi, int recpotcodi, int emprcodi, int cliemprcodi, int barrcodi, string pegrmitipousuario, string pegrmilicitacion, string pegrmicalidad, string pegrmicalidad2);
    }
}
