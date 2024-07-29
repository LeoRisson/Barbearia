using Barbearia.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace Barbearia.API.DTO
{
    public class AgendamentoDTO
    {
        public int AgendamentoID { get; set; }
        [Required]
        public DateTime DataHora { get; set; }
        [DisplayName("Observações")]
        public string? Observacoes { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int ClienteID { get; set; }
        public Cliente cliente { get; set; }
        public virtual ICollection<Servico> Servicos { get; set; }
    }
}
