// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 02/03/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class VceCfgDatCalculoRepository : RepositoryBase, IVceCfgDatcalculoRepository
    {

        public VceCfgDatCalculoRepository(string strConn)
            : base(strConn)
        {
        }

        VceCfgDatCalculoHelper helper = new VceCfgDatCalculoHelper();


        public void Save(Dominio.DTO.Sic.VceCfgDatCalculoDTO entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Dominio.DTO.Sic.VceCfgDatCalculoDTO entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DateTime crdcgfecmod, int grupocodi, int pecacodi)
        {
            throw new NotImplementedException();
        }

        public Dominio.DTO.Sic.VceCfgDatCalculoDTO GetById(int fenergcodi, int concepcodi, string cfgdccamponomb)
        {
            throw new NotImplementedException();
        }

        public List<Dominio.DTO.Sic.VceCfgDatCalculoDTO> List()
        {
            throw new NotImplementedException();
        }

        public List<Dominio.DTO.Sic.VceCfgDatCalculoDTO> GetByCriteria()
        {
            throw new NotImplementedException();
        }

        public List<VceCfgDatCalculoDTO> ObtenerCfgsCampo(string cfgdctipoval)
        {
            List<VceCfgDatCalculoDTO> entities = new List<VceCfgDatCalculoDTO>();

            string queryString = string.Format(helper.SqlGetCfgsCamp, cfgdctipoval);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

    }
}
