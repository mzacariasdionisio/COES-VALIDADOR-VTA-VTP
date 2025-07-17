using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnReduccionRedRepository
    {
        List<PrnReduccionRedDTO> ListByNombre();
        List<PrnReduccionRedDTO> ListByCPNivel(int version, string tipo);
        List<PrnReduccionRedDTO> ListPuntosAgrupacionesByBarra(string tipo);
        
        void DeleteReduccionRed(int reduccionred, int version);
        void Save(PrnReduccionRedDTO entity);
        void Update(PrnReduccionRedDTO entity);
        List<PrnReduccionRedDTO> GetModeloActivo();
        //14/03/2020
        void DeletePrnReduccionRedBarraVersion(int barracp, int barrapm, int version);
        List<PrnReduccionRedDTO> ListSumaBarraGaussPM(int version, string barraspm);
        //19032020

        //20200524
        List<PrnReduccionRedDTO> ListBarraCPPorArea(int areapadre);

    }
}
