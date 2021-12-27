﻿using Newtonsoft.Json;
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
        private CancellationTokenSource _cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
            apiURL = "http://localhost:54952";
            httpClient = new HttpClient();
        }

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(3));

            //Thread.Sleep(5000);
            loadingGif.Visible = true;
            pgProcesamiento.Visible = true;
            var reportarProgreso = new Progress<int>(ReportarProgresoTarjetas);
            //await Esperar();
            //var nombre = txtInput.Text;

            
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                //var saludo = await ObtenerSaludo(nombre);
                //MessageBox.Show(saludo);
                var tarjetas = await ObtenerTarjetasDeCredito(20, _cancellationTokenSource.Token);
                await ProcesarTarjetas(tarjetas, reportarProgreso, _cancellationTokenSource.Token);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(TaskCanceledException ex)
            {
                MessageBox.Show($"La operación ha sido cancelada {ex.Message}");
            }

            MessageBox.Show($"Operación finalizada en {stopWatch.ElapsedMilliseconds / 1000.0} segundos");

            loadingGif.Visible = false;
            pgProcesamiento.Visible = false;
            pgProcesamiento.Value = 0;
        }

        private void ReportarProgresoTarjetas(int porcentaje)
        {
            pgProcesamiento.Value = porcentaje;
        }

        private async Task ProcesarTarjetas(List<string> tarjetas, IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("procesando");
            using var semaforo = new SemaphoreSlim(2);

            var tareas = new List<Task<HttpResponseMessage>>();
            //var indice = 0;

            tareas = tarjetas.Select(async tarjeta =>
            {
                var json = JsonConvert.SerializeObject(tarjeta);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await semaforo.WaitAsync();
                try
                {
                    var tareaInterna = await httpClient.PostAsync($"{apiURL}/tarjetas", content, cancellationToken);

                    //if (progress != null)
                    //{
                    //    indice++;
                    //    var porcentaje = (double)indice / tarjetas.Count;
                    //    porcentaje = porcentaje * 100;
                    //    var porcentajeInt = (int)Math.Round(porcentaje, 0);
                    //    progress.Report(porcentajeInt);
                    //}

                    return tareaInterna;
                }
                finally
                {
                    semaforo.Release();
                }
            }).ToList();

            


            var respuestasTareas =  Task.WhenAll(tareas);

            if (progress != null)
            {
                while (await Task.WhenAny(respuestasTareas, Task.Delay(1000)) != respuestasTareas)
                {
                    var tareasCompletadas = tareas.Where(x => x.IsCompleted).Count();
                    var porcentaje = (double)tareasCompletadas / tarjetas.Count;
                    porcentaje = porcentaje * 100;
                    var porcentajeInt = (int)Math.Round(porcentaje, 0);
                    progress.Report(porcentajeInt);
                }
            }

            var respuestas = await respuestasTareas;

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

        private async Task<List<string>> ObtenerTarjetasDeCredito(int cantidadDeTarjetas, CancellationToken cancellationToken = default)
        {

            return await Task.Run(() =>
            {
                var tarjetas = new List<string>();

                for (int i = 0; i < cantidadDeTarjetas; i++)
                {
                    //await Task.Delay(1000);
                    //0000000000000001
                    //0000000000000002
                    tarjetas.Add(i.ToString().PadLeft(16, '0'));

                    Console.WriteLine($"Han sido generadas {tarjetas.Count} tarjetas");

                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
