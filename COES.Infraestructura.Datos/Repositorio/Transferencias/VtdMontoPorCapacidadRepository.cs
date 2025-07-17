using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VtdMontoPorCapacidad
    /// </summary>
    public class VtdMontoPorCapacidadRepository: RepositoryBase, IVtdMontoPorCapacidadRepository
    {
        public VtdMontoPorCapacidadRepository(string strConn): base(strConn)
        {
        }

        VtdMontoPorCapacidadHelper helper = new VtdMontoPorCapacidadHelper();
        //filtro por rango de fechas -Fit
        public VtdMontoPorCapacidadDTO GetListFullByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            VtdMontoPorCapacidadDTO entity = new VtdMontoPorCapacidadDTO();

            string sCommand = string.Format(helper.SqlListByDateRange, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha),emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                }
            }

            return entity;
        }
        public List<VtdMontoPorCapacidadDTO> GetListPageByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<VtdMontoPorCapacidadDTO> entitys = new List<VtdMontoPorCapacidadDTO>();

            string sCommand = string.Format(helper.SqlListPageByDateRange, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize,emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdMontoPorCapacidadDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
