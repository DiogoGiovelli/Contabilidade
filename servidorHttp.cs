using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class ServidorHttp
{

   //objeto TcpListenert é responsavel por ficar escuntando um porta do computador a espera de qualquer tipo de solitação TCP
   private TcpListener Controlador {get;set;}
   // porta que vai ser escultada
   private int Porta { get; set; }

   public string HtmlTeste{ get; set;}

   // contador 
   private int QtdRequests {get ; set; }
     
     
   //construtor
   //
   public ServidorHttp (int porta = 8080)
      {
            this.Porta = porta;
            this.CriarHtmlTeste();
            try
            {
                  //criando um novo opjeto TcpListener que vai escutar no ip (ipHost) e na porta(this.Porta) passadas
                  string ipHost = "127.0.0.1";
                  this.Controlador = new TcpListener( IPAddress.Parse(ipHost), this.Porta);
                  //inicia a escuta
                  this.Controlador.Start();
                  Console.WriteLine($" O Servidor está rodando, acesse http://{ipHost}:{this.Porta}/");
                  //Chamando de forma assincrona o metodo AguardarRequests
                  Task servidorHttpTask = Task.Run(() => AguardarRequests());
                  //Aguardando a solicitação
                  servidorHttpTask.GetAwaiter().GetResult();

            }
            catch (Exception e)
            {
                  Console.WriteLine($"Erro ao iniciar servidor na porta {this.Porta}:\n{e.Message}");
            }
        }
         

   //metodo que vai aguardar novas requisições que vai gerar um objeto do tipo Socket 
   //Socket é um objeto comum pra comum para comunicação em rede
   //permite verificar e responder requisições 
   private async Task AguardarRequests()
        {
           while (true)
           {
              Socket conexao = await this.Controlador.AcceptSocketAsync();
              this.QtdRequests++;
              //Chamando de forma assincrona o metodo ProcessarRequests
              Task task =Task.Run(() => ProcessarRequests(conexao, this.QtdRequests));
           }

        }

   private void ProcessarRequests(Socket conexao, int numeroRequest)
        {
           Console.WriteLine($"Processando request #{numeroRequest}...\n");
           //ver se conexão está ativa
           if (conexao.Connected)
           {
               byte[] bytesRequisicao = new byte[1024];
               //armanzendando os dados da requisição que é um vetor de 1024 bytes
               conexao.Receive(bytesRequisicao,bytesRequisicao.Length,0);
               //convertendo o vetor em texto UTF8 e removendo os zeros desnecessarios
               string textoRequisicao = Encoding.UTF8.GetString(bytesRequisicao)
                                          .Replace((char)0, ' ').Trim();

               if (textoRequisicao.Length > 0)
               {
                  Console.WriteLine($"\n{textoRequisicao}\n");
                  //transfomando a pagina teste em uma sequencia de bytes
                  var bytesConteudo = Encoding.UTF8.GetBytes(this.HtmlTeste,0,this.HtmlTeste.Length);
                  //gerando um cabeçalho
                  var bytesCabecalho = GerarCabecalho("HTTP/1.1", "text/html;charset=utf-8",
                     "200", bytesConteudo.Length);
                  int bytesEnviados = conexao.Send(bytesCabecalho,bytesCabecalho.Length,0);
                  bytesEnviados = conexao.Send(bytesConteudo,bytesConteudo.Length,0);

                  //Fechando a conexão!!!
                  conexao.Close();
                  Console.WriteLine($"\n{bytesEnviados} bytes enviado em resposta a requisição #{numeroRequest}.");

               }

           }
            Console.WriteLine($"\nRequest {numeroRequest} finalizado. ");

      }

   public byte[] GerarCabecalho(string versaoHttp, string tipoMime, string codigoHttp, int qtdeBytes = 0)
   {
      StringBuilder texto = new StringBuilder();
      //criando o cabeçalho, ao final da lina é adicionado \n \r com a propriedade NewLine da classe Environment
      texto.Append($"{versaoHttp} {codigoHttp}{Environment.NewLine}");
      texto.Append($"Server: Servidor Contabilidade {Environment.NewLine}");
      texto.Append($"Content-Type: {tipoMime}{Environment.NewLine}");
      texto.Append($"Content-Length: {qtdeBytes}{Environment.NewLine}{Environment.NewLine}");
      return Encoding.UTF8.GetBytes(texto.ToString());

   }

   public void CriarHtmlTeste()
   {
      StringBuilder html = new StringBuilder();
      html.Append("<!DOCTYPE html><html lang\"pt-br\"<head><meta charset\"UTF-8\">");
      html.Append("<meta name \"viewport\" content\"width=device-width, initial-scale=1.0\">");
      html.Append("<title>CONTABILIDADE pagina teste</title>");
      html.Append("<body><h1>Está é uma pagina teste</h1></body></html>");
      this.HtmlTeste = html.ToString();
   }

}




