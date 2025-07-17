using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SIO_DATOPRIE
    /// </summary>
    public interface ISioDatoprieRepository
    {
        int Save(SioDatoprieDTO entity);
        void Update(SioDatoprieDTO entity);
        void Delete(int dpriecodi);
        SioDatoprieDTO GetById(int dpriecodi);
        List<SioDatoprieDTO> List();
        List<SioDatoprieDTO> GetByCriteria();

        List<SioDatoprieDTO> GetByCabpricodi(string idEmpresa, string cabpricodi);

        ///Tabla 03
        List<SioDatoprieDTO> GetListaDifusionCostoMarginal(string cabprie, DateTime dfecIniMes, DateTime dfecFinMes, string idBarra, string Tensiones, string idAreas);

        //// Tabla 04
        List<SioDatoprieDTO> GetCostoVariableByFiltro(string cabprie, DateTime dfecIniMes, DateTime dfecFinMes, string modoOpe, string tipoCombustible, string precioComb, string tipoCostoVar);

        // Tabla 07
        List<SioDatoprieDTO> GetListaByCabpricodi(string idEmpresa, string cabpricodi);

        // Tabla 08
        List<SioDatoprieDTO> GetDifusionTransfPotencia(string idEmpresa, int cabpricodi);

        List<SioDatoprieDTO> GetListaDifusionEnergPrie(int lectcodi, DateTime dfecha, string familia);
        List<SioDatoprieDTO> GetListaDifusionEnergPrieByFiltro(int lectcodi, DateTime dfecha, string familia, string idEmpresa, string tipoGene, string recenerg);
        ///Fin Tabla 26
        ///Tabla 27
        List<SioDatoprieDTO> GetByEmpLectPtomedFechaOrden(string idEmpresa, int lectcodi, string ptomedicodi, DateTime dfecha, string orden);

        int ValidarDataPorCodigoCabecera(int Dpriecodi);
        IDataReader GetDataReader(int Cabpricodi);
        int BorrarDataPorCodigoCabecera(int Cabpricodi);

        int Save(SioDatoprieDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(DateTime periodo, int tpriecodi, IDbConnection connection, IDbTransaction transaction);

        List<SioDatoprieDTO> GetSioDatosprieByCriteria(int cabpricodi, string equicodi, string grupocodi, string barrcodi, string emprcodi);


        #region SIOSEIN-PRIE-2021
        List<SioDatoprieDTO> GetReporteRR05ByOsinergcodi(string osinergcodi);
        List<SioDatoprieDTO> GetReporteR05MDTByOsinergcodi(string osinergcodi);
        List<SioDatoprieDTO> GetReporteR05IEyR05MDE(string osinergcodi, string medifecha);
        List<SioDatoprieDTO> GetByCabpricodi2(string idEmpresa, string cabpricodi);
        
        SioDatoprieDTO ObtenerMeMedicion48(string osinergCodi);
        SioDatoprieDTO ObtenerMeMedicion24(string osinergCodi);
        SioDatoprieDTO ObtenerMeMedicionxIntervalo(string osinergCodi);
        #endregion
    }
}
