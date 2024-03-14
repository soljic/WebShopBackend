using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Movies
{
    public class MovieServiceQueryRb : IHostedService
    {
        //private readonly IMessageQueue _messageQueue;
        //private readonly IHttpClient _httpClient;

        //public MovieService(IMessageQueue messageQueue, IHttpClient httpClient)
        //{
        //    _messageQueue = messageQueue;
        //    _httpClient = httpClient;
        //}

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_messageQueue.Subscribe<GetMoviesMessage>(async message =>
            //{
            //    var movies = await _httpClient.GetMoviesAsync();
            //    var resultMessage = new GetMoviesResultMessage { Movies = movies };
            //    await _messageQueue.PublishAsync(resultMessage);
            //});

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Ovdje možete dodati logiku za zaustavljanje usluge, ako je potrebno
            return Task.CompletedTask;
        }
    }
}
