using CotaPedido.Entidades;
using CotaPedido.Dominio.Interface;
using System.Collections.Generic;

namespace CotaPedido.Infra.IRepositorios
{
    public interface IRepositorioSubGrupo : IRepository<SubGrupo>
    {
        IEnumerable<SubGrupo> GetListOfGrupo(int idGrupo);
        SubGrupo Get(string subGrupo);
    }
}
