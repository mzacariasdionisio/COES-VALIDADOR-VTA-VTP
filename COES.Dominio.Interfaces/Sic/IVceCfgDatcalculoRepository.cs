// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 03/03/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCE_CFGDATCALCULO
    /// </summary>
    public interface IVceCfgDatcalculoRepository
    {
        void Save(VceCfgDatCalculoDTO entity);

        void Update(VceCfgDatCalculoDTO entity);

        void Delete(DateTime crdcgfecmod, int grupocodi, int pecacodi);

        VceCfgDatCalculoDTO GetById(int fenergcodi, int concepcodi, string cfgdccamponomb);

        List<VceCfgDatCalculoDTO> List();

        List<VceCfgDatCalculoDTO> GetByCriteria();

        List<VceCfgDatCalculoDTO> ObtenerCfgsCampo(string cfgdctipoval);

    }
}
