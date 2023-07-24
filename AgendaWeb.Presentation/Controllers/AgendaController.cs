using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Presentation.Models;
using AgendaWeb.Reports.Interfaces;
using AgendaWeb.Reports.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaWeb.Presentation.Controllers
{
    [Authorize]
    public class AgendaController : Controller
    {
        private readonly IEventoRepository _eventoRepository;

        public AgendaController(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost] //Annotation idica que o método seá executaTo no SUBMIT
        public IActionResult Cadastro(EventoCadastroViewModel model)
        {
            //verificando se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {

                try
                {
                    //ler o usuário autenticado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    var usuario = JsonConvert.DeserializeObject<UserIdentityModel>(json);

                    var evento = new Evento
                    {
                        Id = Guid.NewGuid(),
                        Nome = model.Nome,
                        Data = Convert.ToDateTime(model.Data),
                        Hora = TimeSpan.Parse(model.Hora),
                        Descricao = model.Descricao,
                        Prioridade = Convert.ToInt32(model.Prioridade),
                        DataInclusao = DateTime.Now,
                        DataAlteracao = DateTime.Now,
                        IdUsuario = usuario.Id //fereing key
                    };

                    //gravando no banco de dados
                    _eventoRepository.Create(evento);

                    TempData["MensagemSucesso"] = $"Evento {evento.Nome}, cadastrado com sucesso.";
                    ModelState.Clear(); //limpando os campos do formulário (model)
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }

            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formlário.";
            }

            return View();
        }

        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Consulta(EventoConsultaViewModel model)
        {
            //verificando se todos os campos da model passaram nas validações
            if (ModelState.IsValid)
            {
                try
                {
                    //converter as datas
                    var dataMin = Convert.ToDateTime(model.DataMin);
                    var dataMax = Convert.ToDateTime(model.DataMax);

                    //Verificando se a data de inicio e menor ou igual a datade fim
                    if (dataMin <= dataMax)
                    {
                        //ler o usuário autenticado na sessão
                        var json = HttpContext.Session.GetString("usuario");
                        var usuario = JsonConvert.DeserializeObject<UserIdentityModel>(json);

                        //realizando a consulta de eventos
                        model.Eventos = _eventoRepository.GetByDatas(dataMin, dataMax, model.Ativo, usuario.Id);

                        //verificando se algum evento foi abtido
                        if (model.Eventos.Count > 0)
                        {
                            TempData["mensagemSucesso"] = $"{model.Eventos.Count} evento(s) obtido(s) para pesquisa";
                        }
                        else
                        {
                            TempData["mensagemAlerta"] = "Nenhum evento foi encontrado para a pesquisa realizada.";
                        }
                    }
                    else
                    {
                        TempData["mensagemErro"] = "A data de início deve ser menor ou igual a data de término.";
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            //Voltar para a página
            return View(model);
        }

        public IActionResult Edicao(Guid id)
        {
            var model = new EventoEdicaoViewModel();

            try
            {
                //consultar o evento no banco de dados atraves do ID
                var evento = _eventoRepository.GetById(id);

                //preeencher os dados da classe model com as informações do evento
                model.Id = evento.Id;
                model.Nome = evento.Nome;
                model.Data = Convert.ToDateTime(evento.Data).ToString("yyyy-MM-dd");
                model.Hora = evento.Hora.ToString();
                model.Descricao = evento.Descricao;
                model.Prioridade = evento.Prioridade.ToString();
                model.Ativo = evento.Ativo;

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //enviando o model para a página
            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(EventoEdicaoViewModel model)
        {
            //verificando se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    //obtendo os dados do evento no banco de dados..
                    var evento = _eventoRepository.GetById(model.Id);

                    //ler o usuário autenticado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    var usuario = JsonConvert.DeserializeObject<UserIdentityModel>(json);

                    //modificar os dados do evento
                    evento.Nome = model.Nome;
                    evento.Data = Convert.ToDateTime(model.Data);
                    evento.Hora = TimeSpan.Parse(model.Hora);
                    evento.Descricao = model.Descricao;
                    evento.Prioridade = Convert.ToInt32(model.Prioridade);
                    evento.Ativo = model.Ativo;
                    evento.DataAlteracao = DateTime.Now;
                    evento.IdUsuario = usuario.Id;

                    //atualizando no banco de dados
                    _eventoRepository.Update(evento);

                    TempData["MensagemSucesso"] = $"Evento '{evento.Nome}', atualizado com sucesso";

                    return RedirectToAction("Consulta");
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário. ";
            }

            return View();
        }

        public IActionResult Exclusao(Guid id)
        {
            try
            {
                //buscar o evento no banco de dados 
                var evento = _eventoRepository.GetById(id);

                //excluindo o evento
                _eventoRepository.Delete(evento);

                TempData["MensagemSucesso"] = $"Evento '{evento.Nome}', excluído com sucesso";
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            //redirecionar de volta para página de consulta
            return RedirectToAction("Consulta");
        }

        public IActionResult Relatorio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Relatorio(EventoRelatorioViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //capturar as datas enviadas 
                    DateTime dataMin = Convert.ToDateTime(model.DataMin);
                    DateTime dataMax = Convert.ToDateTime(model.DataMax);

                    //ler o usuário autenticado na sessão
                    var json = HttpContext.Session.GetString("usuario");
                    var usuario = JsonConvert.DeserializeObject<UserIdentityModel>(json);

                    //consulta os eventos do banco atraves das datas
                    var eventos = _eventoRepository.GetByDatas(dataMin, dataMax, model.Ativo, usuario.Id);

                    //verificando sa algum evento foi obtido
                    if(eventos.Count > 0)
                    {
                        //criando um objeto para a interface..]
                        IEventoReportService eventoReportService = null; //vazio

                        //variaveis ara definir os parametros do download
                        var contentType = string.Empty; //MIME TYPE
                        var fileName = string.Empty;

                        
                        switch(model.Formato)
                        {
                            case 1://polimorfismo
                                eventoReportService = new EventoReportServicePdf();
                                contentType = "application/pdf";
                                fileName = $"eventos_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.pdf";
                                break;
                            case 2://polimorfismo
                                eventoReportService = new EventoReportServiceExcel();
                                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                fileName = $"eventos_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.xlsx";
                                break;
                        }

                        //gerando o arquivo do relatorio
                        var arquivo = eventoReportService.Create(dataMin, dataMax, eventos);

                        //DOWNLOADndo arquivo
                        return File(arquivo, contentType, fileName);
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Nenhum evento foi obtido para a pequisa informada.";
                    }
                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            return View();
        }
    }
}
