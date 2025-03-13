using FluentValidation;
using TechChallenge.Domain.RegionalAggregate;
using TechChallenge.UseCase.Interfaces;
using UseCase.Interfaces;

namespace TechChallenge.UseCase.ContatoUseCase.Adicionar
{
    public class AdicionarContatoUseCase : IAdicionarContatoUseCase
    {
        private readonly IValidator<AdicionarContatoDto> _validator;
        private readonly IMessagePublisher _messagePublisher;

        public AdicionarContatoUseCase(IValidator<AdicionarContatoDto> validator, IMessagePublisher messagePublisher)
        {
            _validator = validator;
            _messagePublisher = messagePublisher;
        }

        public ContatoAdicionadoDto Adicionar(AdicionarContatoDto adicionarContatoDto)
        {
            var validacao = _validator.Validate(adicionarContatoDto);
            if (!validacao.IsValid)
            {
                string mensagemValidacao = string.Empty;
                foreach (var item in validacao.Errors)
                {
                    mensagemValidacao = string.Concat(mensagemValidacao, item.ErrorMessage, ", ");
                }

                throw new Exception(mensagemValidacao.Remove(mensagemValidacao.Length - 2));
            }

            var contato = Contato.Criar(adicionarContatoDto.Nome, adicionarContatoDto.Telefone, adicionarContatoDto.Email, adicionarContatoDto.RegionalId);

            _messagePublisher.PublishAsync(contato);            

            return new ContatoAdicionadoDto
            {
                Id = contato.Id,
            };
        }
    }
}
