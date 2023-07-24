using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Infra.Data.Utils;
using AgendaWeb.Presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AgendaWeb.Presentation.Controllers
{
    public class AccountController : Controller
    {
        //atributo
        private readonly IUsuarioRepository _usuarioRepository;

        public AccountController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    //procurando o usuario no banco de dados atraves do email e senha
                    var usuario = _usuarioRepository
                        .GetByEmailESenha(model.Email, CriptografiaUtils.GetMD5(model.Senha));

                    //verificando se o usuário foi encontrado 
                    if (usuario != null)
                    {
                        TempData["MensagemSucesso"] = $"Bem vindo {usuario.Nome}";

                        //gravar os dados usúario autenticado m sessão
                        var userIdentityModel = new UserIdentityModel
                        {
                            Id = usuario.Id,
                            Nome = usuario.Nome,
                            Email = usuario.Email,
                            DataInclusao = usuario.DataInclusao,
                            DataHoraAcesso = DateTime.Now
                        };

                        //converter o objeto em json
                        var json = JsonConvert.SerializeObject(userIdentityModel);
                        HttpContext.Session.SetString("usuario", json);

                        #region criando a permissão de acesso do usuário

                        var autorizacao = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, usuario.Id.ToString()) },
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        var claimPrincipal = new ClaimsPrincipal(autorizacao);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);

                        #endregion


                        //redirecionando para a página inicial do projeto
                        return RedirectToAction("Index", "Home"); //Home/Index
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Acesso negado. Usuário ou senha invalido";
                    }

                }
                catch (Exception e)
                {
                    TempData["MessagemErro"] = e.Message;
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //veriicando se o Email informado já esá cadastrado no banco de dados
                    if (_usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["MensagemErro"] = $"O email informado já está cadastrado. Tente outro.";
                    }
                    else
                    {
                        var usuario = new Usuario();

                        usuario.Id = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = CriptografiaUtils.GetMD5(model.Senha);
                        usuario.DataInclusao = DateTime.Now;

                        _usuarioRepository.Create(usuario); //cadastrando o usário

                        TempData["MensagemSucesso"] = "usuário cadastrado com sucesso";
                        ModelState.Clear(); //Limpar os campos do formulario

                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View();
        }

        [Authorize]
        public IActionResult UserData()
        {
            return View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            //apagar os dados do usuário da sessão
            HttpContext.Session.Remove("usuario");

            //apagar a permissão dada ao usuário autenticado
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //redirecionar o usuário para a página inicial
            return RedirectToAction("Login");
        }
    }
}
