using System;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payments.Models;

namespace Payments.Controllers
{
    [ApiController]
    public class PaymentsController : Controller
    {
        private readonly Random rand = new Random();
        private readonly HttpClient httpClient;
        private readonly ConcurrentDictionary<string, StrongBox<int>> internalCounters;

        public PaymentsController(ConcurrentDictionary<string, StrongBox<int>> internalCounters)
        {
            this.httpClient = new HttpClient();
            this.internalCounters = internalCounters;
            this.internalCounters.GetOrAdd("authorize", new StrongBox<int>());
        }

        // POST pay
        [Route("pay/{orderNum}")]
        [HttpPost]
        public IActionResult Pay(string orderNum, Payment payment)
        {
            if (rand.NextDouble() < 0.01)
            {
                string url = $"{Request.Scheme}://{Request.Host.ToUriComponent()}/health";
                Task.Run(async () => await httpClient.GetAsync(url));
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(40, 10), 20)));

            if (string.IsNullOrWhiteSpace(orderNum))
            {
                throw new ArgumentException($"invalid order number: {orderNum}");
            }
            else if (string.IsNullOrWhiteSpace(payment.Name))
            {
                throw new ArgumentException($"invalid name: {payment.Name}");
            }
            else if (string.IsNullOrWhiteSpace(payment.CreditCardNum) || rand.NextDouble() < 0.01)
            {
                throw new ArgumentException($"invalid credit card number: {payment.CreditCardNum}");
            }

            
            IActionResult result = rand.NextDouble() < 0.5 ? FastPay() : ProcessPayment();
            Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(20, 5), 10)));

            var dbTask = SavePaymentAsync();
            var updateTask = UpdateAccountAsync();
            Task.Run(async () => await Task.WhenAll(dbTask, updateTask));
            Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(10, 2), 5)));

            return result;
        }

        private IActionResult FastPay()
        {
     
            {
                try
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(80, 20), 40)));
                    if (rand.NextDouble() < 0.001)
                    {
                        throw new NullReferenceException();
                    }
                    return Accepted("fast pay accepted");
                }
                catch (Exception e)
                {
                    LogException(e, "");
                    return StatusCode(StatusCodes.Status500InternalServerError, "fast pay failed");
                }
            }
        }

        private IActionResult ProcessPayment()
        {

            {
                try
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(25, 5), 15)));
                    if (rand.NextDouble() < 0.001)
                    {
                        throw new OutOfMemoryException();
                    }
                    if (!AuthorizePayment())
                    {
                        
                        return StatusCode(StatusCodes.Status500InternalServerError, "payment authorization failed");
                    }
                    Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(15, 3), 10)));
                    return FinishPayment();
                }
                catch (Exception e)
                {
                    LogException(e, "");
                    return StatusCode(StatusCodes.Status500InternalServerError, "payment processing failed");
                }
            }
        }

        private bool AuthorizePayment()
        {

            {
                try
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(100, 25), 50)));
                    if (Interlocked.Increment(ref internalCounters["authorize"].Value) % 5 == 0)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(15000));
                        throw new TimeoutException();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    LogException(e, "payment authorization timed out, please retry");
                    return false;
                }
            }
        }

        private IActionResult FinishPayment()
        {

            {
                try
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(60, 15), 30)));
                    if (rand.NextDouble() < 0.001)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                        throw new TimeoutException();
                    }
                    return Accepted("payment accepted");
                }
                catch (Exception e)
                {
                    LogException(e, "");
                    return StatusCode(StatusCodes.Status500InternalServerError, "finish payment failed");
                }
            }
        }

        private async Task UpdateAccountAsync()
        {

            {
                try
                {
                    double randDuration = rand.NextDouble() / 3;
                    await Task.Delay(TimeSpan.FromSeconds(1 + randDuration));
                    if (rand.NextDouble() < 0.001)
                    {
                        throw new OutOfMemoryException();
                    }
                }
                catch (Exception e)
                {
                    LogException(e, "");
                }
            }
        }

        private async Task SavePaymentAsync()
        {

            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(100, 25), 50)));
                    if (rand.NextDouble() < 0.05)
                    {
                        throw new TimeoutException();
                    }
                }
                catch (Exception e)
                {
                    LogException(e, "");
                }
            }
        }

        // GET health
        [Route("health")]
        [HttpGet]
        public async Task<IActionResult> GetHealthAsync()
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(20, 5), 10)));

            if (await CheckHealthAsync())
            {
              
                return Ok("healthy");
            }
           
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "unavailable");
        }

        private async Task<bool> CheckHealthAsync()
        {
          
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(20, 5), 10)));
                var taskDb = CheckDbHealthAsync();
                var taskAuth = CheckAuthHealthAsync();
                var results = await Task.WhenAll(taskDb, taskAuth);
                foreach (bool healthy in results)
                {
                    if (!healthy)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private async Task<bool> CheckDbHealthAsync()
        {
          
            {
                await Task.Delay(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(50, 10), 30)));
                if (rand.NextDouble() < 0.01)
                {
                   
                    return false;
                }
                return true;
            }
        }

        private async Task<bool> CheckAuthHealthAsync()
        {

            {
                await Task.Delay(TimeSpan.FromMilliseconds(Math.Max(RandomGauss(45, 10), 25)));
                if (rand.NextDouble() < 0.01)
                {
                    
                    return false;
                }
                return true;
            }
        }

        private double RandomGauss(double mean, double stdDev)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2);
            double randNormal =
                         mean + stdDev * randStdNormal;
            return randNormal;
        }

        private void LogException(Exception exception, string message)
        {

            return;
        }
    }
}