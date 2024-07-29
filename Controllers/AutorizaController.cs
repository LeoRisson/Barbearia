using Barbearia.API.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Barbearia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutorizaController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManeger;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly IConfiguration _configuration;

        public AutorizaController(UserManager<IdentityUser> userManeger, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManeger = userManeger;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("CadastrarUsuario")]
        public async Task<IActionResult> RegisterUser([FromBody] UsuarioDTO Pobj_Usuario)
        {
            var user = new IdentityUser
            {
                UserName = Pobj_Usuario.Email,
                Email = Pobj_Usuario.Email,
                EmailConfirmed = true,
            };

            var result = await _userManeger.CreateAsync(user, Pobj_Usuario.Password);
            
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            return Ok(GeraToken(Pobj_Usuario));
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO Pobj_Usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }

            var result = await _signInManager.PasswordSignInAsync(
                                Pobj_Usuario.Email,
                                Pobj_Usuario.Password,
                                isPersistent: false,
                                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return Ok(GeraToken(Pobj_Usuario));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "E-Mail ou Senha Inválidos");
                return BadRequest(ModelState);
            }



        }


        private UsuarioToken GeraToken(UsuarioDTO Pobj_Usuario)
        {
            //define as Declarações do usuário
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, Pobj_Usuario.Email),
                new Claim("Barbearia", "usuariobarbarberia"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //gera uma chave com base em um algoritimo simetrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            //gera a assinatura digital do Token usando o algoritimo Hmac e a chave privada
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //tempo de expiração do Token
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            //classe que represanta um JWT Token e gera o token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credenciais);

            //retorna os dados com o token e as informações
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };
        }
    }
}
