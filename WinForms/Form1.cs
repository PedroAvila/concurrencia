using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class Form1 : Form
    {
        private string apiURL;
        private HttpClient httpClient;
        public Form1()
        {
            InitializeComponent();
            apiURL = "http://localhost:54952";
            httpClient = new HttpClient();
        }

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(5000);
            loadingGif.Visible = true;
            //await Esperar();
            //var nombre = txtInput.Text;

            var tarjetas = await ObtenerTarjetasDeCredito(2500);
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                //var saludo = await ObtenerSaludo(nombre);
                //MessageBox.Show(saludo);
                await ProcesarTarjetas(tarjetas);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show($"Operación finalizada en {stopWatch.ElapsedMilliseconds / 1000.0} segundos");

            loadingGif.Visible = false;
        }

        private async Task ProcesarTarjetas(List<string> tarjetas)
        {
            using var semaforo = new SemaphoreSlim(1000);

            var tareas = new List<Task<HttpResponseMessage>>();

            tareas = tarjetas.Select(async tarjeta =>
            {
                var json = JsonConvert.SerializeObject(tarjeta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await semaforo.WaitAsync();
                try
                {
                    return await httpClient.PostAsync($"{apiURL}/tarjetas", content);
                }
                finally
                {
                    semaforo.Release();
                }
            }).ToList();


            var respuestas = await Task.WhenAll(tareas);
            var tarjetasRechazadas = new List<string>();

            foreach (var respuesta in respuestas)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var respuestaTarjeta = JsonConvert.DeserializeObject<RespuestaTarjeta>(contenido);
                if (!respuestaTarjeta.Aprobada)
                {
                    tarjetasRechazadas.Add(respuestaTarjeta.Tarjeta);
                }
            }

            foreach (var tarjeta in tarjetasRechazadas)
            {
                Console.WriteLine(tarjeta);
            }
        }

        private async Task<List<string>> ObtenerTarjetasDeCredito(int cantidadDeTarjetas)
        {

            return await Task.Run(() =>
            {
                var tarjetas = new List<string>();

                for (int i = 0; i < cantidadDeTarjetas; i++)
                {
                    //0000000000000001
                    //0000000000000002
                    tarjetas.Add(i.ToString().PadLeft(16, '0'));
                }

                return tarjetas;
            });
        }


        private async Task Esperar()
        {
            await Task.Delay(TimeSpan.FromSeconds(0));
        }

        private async Task<string> ObtenerSaludo(string nombre)
        {
            using (var respuesta = await httpClient.GetAsync($"{apiURL}/Saludos/{nombre}"))
            {
                respuesta.EnsureSuccessStatusCode();
                var saludo = await respuesta.Content.ReadAsStringAsync();
                return saludo;
            }
        }

    }
}
