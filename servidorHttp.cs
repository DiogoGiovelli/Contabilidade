using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class ServidorHttp
{

        //objeto TcpListenert é responsavel por ficar escuntando um porta do computador a espera de qualquer tipo de solitação TCP
        private TcpListener Controlador {get;set;}
     // porta que vai ser escultada
        private int Porta { get; set; }

     // contador 
        private int QtdRequests {get ; set; }
     
     
     //construtor
     //
        public ServidorHttp (int porta = 8080)
        {
            this.Porta = porta;
            try
            {
                  //criando um novo opjeto TcpListener que vai escutar no ip("127.0.0.1") e na porta(this.Porta) passadas
                  this.Controlador = new TcpListener( IPAddress.Parse("127.0.0.1"), this.Porta);
                  //inicia a escuta
                  this.Controlador.Start();
                  Console.WriteLine($" O Servidor está rodando, acesse http://{this.Controlador}:{this.Porta}/");

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
              Task task =Task.Run(() => ProcessarRequests(conexao, this.QtdRequests));
           }

        }

        private void ProcessarRequests(Socket conexao, int numeroRequest)
        {
           if (conexao.Connect)
           {
           byte[] bytesRequisicao = new byte[1024];
           conexao.Receive(bytesRequisicao,bytesRequisicao.Length,0);
           string 
           }

        }




}




