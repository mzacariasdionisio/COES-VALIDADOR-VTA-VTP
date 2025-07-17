using COES.Base.Core;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class ValorTransferenciaEmpresaRepository : RepositoryBase, IValorTransferenciaEmpresaRepository
    {
        public ValorTransferenciaEmpresaRepository(string strConn)
            : base(strConn)
        {
        }

        ValorTransferenciaEmpresaHelper helper = new ValorTransferenciaEmpresaHelper();

        public int Save(Dominio.DTO.Transferencias.ValorTransferenciaEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.VALTRANEMPCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, entity.EmpCodi);
            dbProvider.AddInParameter(command, helper.VALTRANEMPVERSION, DbType.Int32, entity.ValTranEmpVersion);
            dbProvider.AddInParameter(command, helper.VALTRANEMPTOTAL, DbType.Decimal, entity.ValTranEmpTotal);
            dbProvider.AddInParameter(command, helper.VTRANEUSERNAME, DbType.String, entity.ValtranUserName);
            dbProvider.AddInParameter(command, helper.VALTRANEMPFECINS, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(Dominio.DTO.Transferencias.ValorTransferenciaEmpresaDTO entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int pericodi, int version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.VALTRANEMPVERSION, DbType.Int32, version);

            dbProvider.ExecuteNonQuery(command);
        }

        public Dominio.DTO.Transferencias.ValorTransferenciaEmpresaDTO GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Dominio.DTO.Transferencias.ValorTransferenciaEmpresaDTO> List()
        {
            throw new NotImplementedException();
        }

        public List<Dominio.DTO.Transferencias.ValorTransferenciaEmpresaDTO> GetByCriteria(string nombre)
        {
            throw new NotImplementedException();
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

    }
}
