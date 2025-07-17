using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnPrdTransversalRepository
    {

        void Save(PrnPrdTransversalDTO entity);
        List<PrnPrdTransversalDTO> List();
        List<PrnPrdTransversalDTO> GetRelacionesPorNombre(string Nombre);
        List<PrnPrdTransversalDTO> GetPerdidaPorBarraCP(int idBarra);

        //--------------------------------------------------------------------
        List<PrGrupoDTO> ListaBarrasPerdidasTransversales();
        List<PrnPrdTransversalDTO> ListPerdidasTransvBarraFormulas();
        //--------------------------------------------------------------------
        List<PrnPrdTransversalDTO> ListBarrasPerdidasTransversales(int Prdtrncodi);
        List<PrnPrdTransversalDTO> GetPerdidasTransversalesByNombre(string Prdtrnetqnomb);
        void DeleteRelacionesPerdidasTransv(string Prdtrnetqnomb);
        List<PrnPrdTransversalDTO> ObtenerPerdidaPorBarraCP(int grupocodi);
    }
}
