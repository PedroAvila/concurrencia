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
        private CancellationTokenSource _cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
            apiURL = "http://localhost:54952";
            httpClient = new HttpClient();
        }

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            //_cancellationTokenSource = new CancellationTokenSource();
            //_cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(3));

            //loadingGif.Visible = true;
            //pgProcesamiento.Visible = true;
            //var reportarProgreso = new Progress<int>(ReportarProgresoTarjetas);
            //await Esperar();
            //var nombre = txtInput.Text;


            //var stopWatch = new Stopwatch();
            //stopWatch.Start();

            //try
            //{
            //    var saludo = await ObtenerSaludo(nombre);
            //    MessageBox.Show(saludo);
            //    var tarjetas = await ObtenerTarjetasDeCredito(20, _cancellationTokenSource.Token);
            //    await ProcesarTarjetas(tarjetas, reportarProgreso, _cancellationTokenSource.Token);
            //}
            //catch (HttpRequestException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //catch (TaskCanceledException ex)
            //{
            //    MessageBox.Show($"La operación ha sido cancelada {ex.Message}");
            //}

            //MessageBox.Show($"Operación finalizada en {stopWatch.ElapsedMilliseconds / 1000.0} segundos");

            //loadingGif.Visible = false;
            //pgProcesamiento.Visible = false;
            //pgProcesamiento.Value = 0;

            //loadingGif.Visible = true;

            //Console.WriteLine($"hilo antes del await: {Thread.CurrentThread.ManagedThreadId}");
            //await Task.Delay(500);
            //Console.WriteLine($"hilo después del await: {Thread.CurrentThread.ManagedThreadId}");

            //await ObtenerSaludo("Pedro");
            //loadingGif.Visible = false;

            //CheckForIllegalCrossThreadCalls = true;

            //loadingGif.Visible = true;

            //btnCancelar.Text = "antes";
            //await Task.Delay(1000).ConfigureAwait(continueOnCapturedContext: false);
            //btnCancelar.Text = "después";

            //loadingGif.Visible = false;

            //loadingGif.Visible = true;

            //var reintentos = 3;
            //var tiempoEspera = 500;

            //for (int i = 0; i < reintentos; i++)
            //{
            //    try
            //    {
            //        // operacion
            //        break;
            //    }
            //    catch (Exception ex)
            //    {
            //        // loguear la exepcion
            //        await Task.Delay(tiempoEspera);
            //    }
            //}

            //await Reintentar(ProcesarSaludo);
            //{
            //using (var respuesta = await httpClient.GetAsync($"{apiURL}/saludos/Pedro"))
            //{
            //    respuesta.EnsureSuccessStatusCode();
            //    var contenido = await respuesta.Content.ReadAsStringAsync();
            //    Console.WriteLine(contenido);
            //}
            //});

            //try
            //{
            //    var contenido = await Reintentar(async () =>
            //    {
            //        using (var respuesta = await httpClient.GetAsync($"{apiURL}/saludos2/Pedro"))
            //        {
            //            respuesta.EnsureSuccessStatusCode();
            //            return await respuesta.Content.ReadAsStringAsync();

            //        }
            //    });
            //    Console.WriteLine(contenido);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("excepcion atrapada");
            //}


            //loadingGif.Visible = false;

            //loadingGif.Visible = true;

            //_cancellationTokenSource = new CancellationTokenSource();
            //var token = _cancellationTokenSource.Token;
            //var nombres = new string[] { "Felipe", "Claudia", "Antonio", "Alexandría" };

            ////var tareasHTTP = nombres.Select(x => ObtenerSaludo(x, token));
            ////var tarea = await Task.WhenAny(tareasHTTP);
            ////var contenido = await tarea;
            ////Console.WriteLine(contenido.ToUpper());
            ////_cancellationTokenSource.Cancel();

            ////var tareasHTTP = nombres.Select(x =>
            ////{
            ////    Func<CancellationToken, Task<string>> function = (ct) => ObtenerSaludo(x, ct);
            ////    return function;
            ////});

            ////var contenido = await EjecutarUno(tareasHTTP);
            ////Console.WriteLine(contenido);

            //var contenido = await  EjecutarUno(
            //    (ct) => ObtenerSaludo("Felipe", ct),
            //    (ct) => ObtenerAdios("Felipe", ct));

            //Console.WriteLine(contenido.ToUpper());

            //loadingGif.Visible = false;

            loadingGif.Visible = true;

            var tarea = EvaluarValor(txtInput.Text);
            Console.WriteLine("Inicio");
            Console.WriteLine($"Is Completed: {tarea.IsCompleted}");
            Console.WriteLine($"Is Canceled: {tarea.IsCanceled}");
            Console.WriteLine($"Is Faulted: {tarea.IsFaulted}");

            try
            {
                await tarea;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            Console.WriteLine("fin");
            Console.WriteLine("");

            loadingGif.Visible = false;
        }

        private Task EvaluarValor(string valor)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (valor == "1")
            {
                tcs.SetResult(null);
            }else if(valor == "2"){
                tcs.SetCanceled();
            }
            else
            {
                tcs.SetException(new ApplicationException($"valor inválido: {valor}"));
            }

            return tcs.Task;
        }

        private async Task<T> EjecutarUno<T>(IEnumerable<Func<CancellationToken, Task<T>>> funciones)
        {
            var cts = new CancellationTokenSource();
            var tareas = funciones.Select(funcion => funcion(cts.Token));
            var tarea = await Task.WhenAny(tareas);
            cts.Cancel();
            return await tarea;
        }

        private async Task<T> EjecutarUno<T>(params Func<CancellationToken, Task<T>>[] funciones)
        {
            var cts = new CancellationTokenSource();
            var tareas = funciones.Select(funcion => funcion(cts.Token));
            var tarea = await Task.WhenAny(tareas);
            cts.Cancel();
            return await tarea;
        }

        private async Task<string> ObtenerSaludo(string nombre, CancellationToken cancellationToken)
        {
            using (var respuesta = await httpClient.GetAsync($"{apiURL}/saludos/adios/{nombre}", cancellationToken))
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
                return contenido;
            }
        }

        private async Task<string> ObtenerAdios(string nombre, CancellationToken cancellationToken)
        {
            using (var respuesta = await httpClient.GetAsync($"{apiURL}/saludos/delay/{nombre}", cancellationToken))
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
                return contenido;
            }
        }

        private async Task ProcesarSaludo()
        {
            using (var respuesta = await httpClient.GetAsync($"{apiURL}/saludos2/Pedro"))
            {
                respuesta.EnsureSuccessStatusCode();
                var contenido = await respuesta.Content.ReadAsStringAsync();
                Console.WriteLine(contenido);
            }
        }

        private async Task Reintentar(Func<Task> f, int reintentos = 3, int tiempoEspera = 500)
        {
            for (int i = 0; i < reintentos; i++)
            {
                try
                {
                    await f();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.Delay(tiempoEspera);
                }
            }
        }

        private async Task<T> Reintentar<T>(Func<Task<T>> f, int reintentos = 3, int tiempoEspera = 500)
        {
            for (int i = 0; i < reintentos - 1; i++)
            {
                try
                {
                    return await f();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.Delay(tiempoEspera);
                }
            }

            return await f();
        }

        private void ReportarProgresoTarjetas(int porcentaje)
        {
            pgProcesamiento.Value = porcentaje;
        }

        private Task ProcesarTarjetasMock(List<string> tarjetas, IProgress<int> progress = null, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
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

        private Task<List<string>> ObtenerTarjetasDeCreditoMock(int cantidadDeTarjetas, CancellationToken cancellationToken = default)
        {
            var tarjetas = new List<string>();
            tarjetas.Add("0000000001");
            return Task.FromResult(tarjetas);
        }

        private Task ObtenerTareaConError()
        {
            return Task.FromException(new ApplicationException());
        }

        private Task ObtenerTareaCancelada()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            return Task.FromCanceled(_cancellationTokenSource.Token);
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
