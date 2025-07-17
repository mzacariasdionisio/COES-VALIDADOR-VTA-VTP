using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VCE_CFG_VALIDA_CONCEPTO
    /// </summary>
    public class VceCfgValidaConceptoRepository : RepositoryBase, IVceCfgValidaConceptoRepository
    {
        public VceCfgValidaConceptoRepository(string strConn)
            : base(strConn)
        {
        }

        VceCfgValidaConceptoHelper helper = new VceCfgValidaConceptoHelper();

        public VceCfgValidaConceptoDTO GetById(int crcvalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);


            dbProvider.AddInParameter(command, helper.Crcvalcodi, DbType.Int32, crcvalcodi);
            VceCfgValidaConceptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceCfgValidaConceptoDTO> ListValidaConceptos()
        {
            List<VceCfgValidaConceptoDTO> entitys = new List<VceCfgValidaConceptoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListConceptos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new VceCfgValidaConceptoDTO();

                    int iCrcvalcodi = dr.GetOrdinal(helper.Crcvalcodi);
                    if (!dr.IsDBNull(iCrcvalcodi)) entity.Crcvalcodi = Convert.ToInt32(dr.GetValue(iCrcvalcodi));

                    int iCrcvaldescripcion = dr.GetOrdinal(helper.Crcvaldescripcion);
                    if (!dr.IsDBNull(iCrcvaldescripcion)) entity.Crcvaldescripcion = dr.GetString(iCrcvaldescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
