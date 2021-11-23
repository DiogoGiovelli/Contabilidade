using System.Net.Sockets;

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
        this.Controlador = new TcpListener( IPAddress.Parse("127.0.0.1"), this.Porta);
        }
        catch
        {
            
        }
        }

}




