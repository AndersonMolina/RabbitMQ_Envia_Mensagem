using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

//Utilizando padrão factory
var factory = new ConnectionFactory { HostName = "localhost" }; //aqui você coloca o endereço onde estiver o serviço do RabbitMQ, no nosso caso, no localhost.

//cria a conexão
using var connection = factory.CreateConnection();

//criamos um canal para a transmissão das mensagens.
//Podemos criar diversos canais para serem utilizados na mesma conexão, com finalidades diferentes
using var canal = connection.CreateModel();

//Criamos uma fila para receber a mensagem que enviaremos
canal.QueueDeclare(queue: "Olá",
                   durable: false,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null);

//é exibida uma mensagem no console para que seja digitado um texto a ser enviado
Console.WriteLine(value: "Digite sua mensagem e aperte <ENTER>");

while (true)
{
    var message = string.Empty;
    string texto = Console.ReadLine();

    if (texto == "")
        break;

    //Forçando dados

    if (texto == "The Shining")
    {
        var pedido = new pedido() { Id = 1, Livro = "The Shining", Categoria = "Terror/Suspense", Autor = "Stephen King", Valor = 99.90 };
        message = JsonSerializer.Serialize(value: pedido);

    }
    else if (texto == "Fortaleza Digital") 
    {
        var pedido = new pedido() { Id = 2, Livro = "Fortaleza Digital", Categoria = "Suspense", Autor = "Dan Brown", Valor = 88.80 };
        message = JsonSerializer.Serialize(value: pedido);

    }
    else
    {
        message = "Qualquer outro livro";
    }

    //Encodamos a mensagem em UTF8 para enviar
    var body = Encoding.UTF8.GetBytes(s: message);

    //publicaremos a mensagem
    canal.BasicPublish(exchange: string.Empty,
                       routingKey: "Olá", // para qual fila enviaremos
                       basicProperties: null,
                       body: body); // a mensagem que estamos enviando

    //Apenas para confirmar que a mensagem foi enviada
    Console.WriteLine(value: $" [OK] Enviado - Mensagem: {message}");
}

//Classe para montar os dados a serem enviados
class pedido
{
    public int Id {  get; set; }    
    public string Livro { get; set; }
    public string Categoria { get; set; }
    public string Autor {  get; set; }  
    public double Valor { get; set; }  


}

