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
    /// Clase de acceso a datos de la tabla VtdMontoPorEnergia
    /// </summary>
    public class VtdMontoPorEnergiaRepository: RepositoryBase, IVtdMontoPorEnergiaRepository
    {
        public VtdMontoPorEnergiaRepository(string strConn): base(strConn)
        {
        }

        VtdMontoPorEnergiaHelper helper = new VtdMontoPorEnergiaHelper();
        //filtro por rango de fechas -Fit
        public List<VtdMontoPorEnergiaDTO> GetListFullByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            List<VtdMontoPorEnergiaDTO> entitys = new List<VtdMontoPorEnergiaDTO>();

            string sCommand = string.Format(helper.SqlListByDateRangeMontoPorEnergia, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha),emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdMontoPorEnergiaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<VtdMontoPorEnergiaDTO> GetListPageByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<VtdMontoPorEnergiaDTO> entitys = new List<VtdMontoPorEnergiaDTO>();

            string sCommand = string.Format(helper.SqlListPageByDateRangeMontoPorEnergia, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize,emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdMontoPorEnergiaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
