// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 21/03/2017
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

    public class VceArrparTipoOperaRepository : RepositoryBase, IVceArrparTipoOperaRepository
    {
        VceArrparTipoOperaHelper helper = new VceArrparTipoOperaHelper();

        public VceArrparTipoOperaRepository(string strConn)
            : base(strConn)
        {
        }

        public void Save(VceArrparTipoOperaDTO entity)
        {
            throw new NotImplementedException();
        }

        public void Update(VceArrparTipoOperaDTO entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(VceArrparTipoOperaDTO entity)
        {
            throw new NotImplementedException();
        }

        public List<VceArrparTipoOperaDTO> ListByType(string aptopcodi)
        {
            List<VceArrparTipoOperaDTO> entities = new List<VceArrparTipoOperaDTO>();

            string queryString = string.Format(helper.SqlListByType, aptopcodi);
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

        public int getConceptoVceArrparTipoOpera(string aptopcodi, string apstocodi)
        {
            string queryString = string.Format(helper.SqlGetConceptoVceArrparTipoOpera, aptopcodi, apstocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            int conceptoVceArrparTipoOpera = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iApstoconscombConcepto = dr.GetOrdinal("APSTOCONSCOMB_CONCEPTO");
                    if (!dr.IsDBNull(iApstoconscombConcepto)) conceptoVceArrparTipoOpera = dr.GetInt32(iApstoconscombConcepto);
                }
            }

            return conceptoVceArrparTipoOpera;
        }

    }

}
