﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace Barbearia.API.DTO
{
    public class ClienteDTO
    {
        public int ClienteID { get; set; }
        [Required]
        public string? Nome { get; set; }
        [Required]
        public string? Telefone { get; set; }
        [EmailAddress(ErrorMessage = "Favor digitar um email válido!")]
        public string? Email { get; set; }

        [Required]
        [DisplayName("Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

    }
}
