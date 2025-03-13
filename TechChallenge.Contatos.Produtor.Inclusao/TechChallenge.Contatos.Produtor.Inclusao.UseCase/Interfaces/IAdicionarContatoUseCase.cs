using TechChallenge.UseCase.ContatoUseCase.Adicionar;

namespace TechChallenge.UseCase.Interfaces
{
    public interface IAdicionarContatoUseCase
    {
        ContatoAdicionadoDto Adicionar(AdicionarContatoDto input);
    }
}
