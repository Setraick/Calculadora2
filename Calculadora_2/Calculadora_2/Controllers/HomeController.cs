using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calculadora_2.Models;

namespace Calculadora_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// invocaçao inicial do nosso projeto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            //inicializar o primeiro valor do "visor"
            ViewBag.Visor = "0";

            return View();
        }

        /// <summary>
        /// Apresentaçao da calculadora
        /// </summary>
        /// <param name="visor">apresenta o numeros escritos na calculadora e o resultado das operaçoes realizadas</param>
        /// <param name="bt">recolhe a escolha do utilizador perante os diverso botoes da calculadora</param>
        /// <param name="primeiroOperando">assegura o efeito de memoria do HTTP guarda o primeiro operando necessario para as operaçoes</param>
        /// <param name="operador">assegura o efeito de memoria do HTTP guarda o operando necessario para as operaçoes aritmeticas</param>
        /// <param name="limpaVisor">especifica se o visor deve ser limpo ou nao</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string visor, string bt, string primeiroOperando, string operador, string limpaVisor)
        {
            //filtrar conteudo da variavel bt
            switch (bt)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    //processar os algarismos
                    if (visor == "0" || limpaVisor == "sim") visor = bt;
                    else visor += bt; // visor = visor + bt;

                    //ativar o serviço de memoria do HTTP
                    ViewBag.PrimeiroOperando = primeiroOperando;
                    ViewBag.Operador = operador;
                    //nao deve ser limpo
                    ViewBag.LimpaVisor = "nao";
                    break;

                case "+/-":
                    //processar a inversao do valor no visor
                    visor = Convert.ToDouble(visor) * -1 + "";
                    //outra hipotese seria o processamento de strings
                    //dentro do valor do visor -> visor.StartsWith().ToString().Substring().Length
                    //ativar o serviço de memoria do HTTP
                    ViewBag.PrimeiroOperando = primeiroOperando;
                    ViewBag.Operador = operador;
                    //nao deve ser limpo
                    ViewBag.LimpaVisor = "nao";
                    break;

                case ",":
                    //processar o separador da parte inteira da decimal
                    if (!visor.Contains(",")) visor += ",";
                    //ativar o serviço de memoria do HTTP
                    ViewBag.PrimeiroOperando = primeiroOperando;
                    ViewBag.Operador = operador;
                    //nao deve ser limpo
                    ViewBag.LimpaVisor = "nao";
                    break;

                case "+":
                case "-":
                case "x":
                case ":":
                case "=":
                    //processar os operadores
                    //
                    //-123,8 + 5 x

                    if (operador == null)
                    {
                        //é a primeira vez que se seleciona o operador
                        //ativa o serviço de memoria do HTTP
                        ViewBag.PrimeiroOperando = visor;
                        ViewBag.Operador = bt;
                        //dar ordem de limpar o visor
                        ViewBag.LimpaVisor = "sim";
                    }
                    else
                    {
                        //ja carreguei pelo menos uma segunda vez no sinal de operador
                        double auxPrimeiroOperando = Convert.ToDouble(primeiroOperando);
                        double auxSegundoOperando = Convert.ToDouble(visor);

                        //executar a operação 
                        switch (operador)
                        {
                            case "+":
                                visor = auxPrimeiroOperando + auxSegundoOperando + "";
                                break;
                            case "-":
                                visor = auxPrimeiroOperando - auxSegundoOperando + "";
                                break;
                            case "x":
                                visor = auxPrimeiroOperando * auxSegundoOperando + "";
                                break;
                            case ":":
                                visor = auxPrimeiroOperando / auxSegundoOperando + "";
                                break;
                        }
                        //ativar o serviço de memoria do HTTP
                        ViewBag.PrimeiroOperando = visor;
                        ViewBag.Operador = bt;
                        //limpar o ecra
                        ViewBag.LimpaVisor = "sim";

                    }
                    //trata do caso particular do sinla de "="
                    if (bt == "=")
                    {
                        //anular o efeito dos operadores
                        ViewBag.Operador = null;
                    }

                    break;

                case "c":
                    //ativar o serviço de memoria do HTTP
                    ViewBag.PrimeiroOperando = null;
                    ViewBag.Operador = null;
                    //limpar o ecra
                    ViewBag.LimpaVisor = "sim";
                    visor = "0";
                    break;



            }//switch(bt)

            //exportar os dados para o view
            ViewBag.Visor = visor;


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
