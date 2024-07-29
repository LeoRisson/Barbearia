using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Barbearia.API.DTO
{
    public class ServicoDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome Serviço é Obrogatório")]
        [DisplayName("Nome Serviço")]
        public string? NomeServico { get; set; }
        [DisplayName("Descrição")]
        public string? Descricao { get; set; }

        [Range(1, 999, ErrorMessage = "A duração deve ser entre 1 e 999")]
        public int DuracaoMin { get; set; }

        [Range(0.01, 999.00, ErrorMessage = "O preço deve estar entre 0,01 e 999,00")]
        public double Preco { get; set; }
    }
}
