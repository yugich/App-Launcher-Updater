using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace AppLauncherUpdater
{
    partial class LauncherForm
    {
        private ProgressBar progressBar;
        private Label statusLabel;
        private Button exitButton;
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            AutoScaleMode = AutoScaleMode.Font;
            Text = "App Launcher";
            Size = new Size(600, 400); // Define um tamanho padrão para o formulário

            InitializeProgressBar();
            InitializeStatusLabel();
            InitializeExitButton();

            Controls.Add(progressBar);
            Controls.Add(statusLabel);
            Controls.Add(exitButton);

            // Personalizações adicionais do formulário
            BackColor = Color.LightGray; // Altera a cor de fundo do formulário
        }

        private void InitializeProgressBar()
        {
            progressBar = new ProgressBar
            {
                Dock = DockStyle.Bottom,
                Height = 20 // Define a altura da barra de progresso
            };
        }

        private void InitializeStatusLabel()
        {
            statusLabel = new Label
            {
                AutoSize = true,
                Location = new Point(10, 10), // Define a posição do label
                Text = "Verificando atualizações...",
                Font = new Font("Arial", 10), // Define a fonte e o tamanho
                ForeColor = Color.Black // Define a cor do texto
            };
        }

        private void InitializeExitButton()
        {
            exitButton = new Button
            {
                Text = "Sair",
                Location = new Point(500, 350), // Define a posição do botão
                Size = new Size(75, 23) // Define o tamanho do botão
                
            };

            exitButton.Click += (sender, e) => Application.Exit();
        }

        #endregion

        //string extractionPath = "./App/"; // Caminho de extração
        string apiPath = "Set API Path Here";
        private async void CheckForUpdatesAsync()
        {
            string localVersionFile = $"{extractionPath}version.txt"; // Caminho do seu arquivo de versão local
            

            // Verifica se o diretório './App' existe, senão cria
            if (!Directory.Exists(Path.GetDirectoryName(localVersionFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localVersionFile));
            }

            // Verifica se o arquivo version.txt existe
            if (!File.Exists(localVersionFile))
            {
                // Cria o arquivo version.txt com a versão padrão "0.0"
                Directory.CreateDirectory(Path.GetDirectoryName(localVersionFile)); // Cria o diretório ./App se não existir
                File.WriteAllText(localVersionFile, "0.0");
            }

            var url = $"{apiPath}/latest-version"; // URL para verificar a versão mais recente

            try
            {
                string localVersion = File.ReadAllText(localVersionFile);
                string latestVersion = await GetLatestVersion(url);

                if (localVersion != latestVersion)
                {
                    statusLabel.Text = "Uma nova versão está disponível. Iniciando download...";
                    await DownloadAndUpdate(latestVersion);
                }
                else
                {
                    statusLabel.Text = "Você está na versão mais recente.";
                    StartApplication();
                }
            }
            catch (Exception ex)
            {
                StartApplication();
                statusLabel.Text = $"Erro: {ex.Message}";
            }
        }


        private static async Task<string> GetLatestVersion(string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private async Task DownloadAndUpdate(string latestVersion)
        {
            string downloadUrl = $"{apiPath}/storage"; // URL para baixar a versão mais recente
            string zipFilePath = "./Update.zip"; // Caminho temporário para o arquivo ZIP baixado

            var response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
            var totalBytes = response.Content.Headers.ContentLength ?? -1L;
            var canReportProgress = totalBytes != -1;

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var totalRead = 0L;
                var buffer = new byte[8192];
                var isMoreToRead = true;
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                do
                {
                    var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (read == 0)
                    {
                        isMoreToRead = false;
                        continue;
                    }

                    await fileStream.WriteAsync(buffer, 0, read);
                    totalRead += read;

                    if (canReportProgress)
                    {
                        var progress = (int)((totalRead * 100) / totalBytes);
                        progressBar.Value = progress;
                        var speed = (totalRead / 1024d / 1024d) / stopwatch.Elapsed.TotalSeconds; // Velocidade em MB/s
                        statusLabel.Text = $"Baixando atualização... {progress}% ({totalRead / 1024d / 1024d:0.00} MB baixados a {speed:0.00} MB/s)";
                    }
                }
                while (isMoreToRead);

                stopwatch.Stop();
            }

            // Atualiza o status para a extração de arquivos
            statusLabel.Text = "Extraindo arquivos...";
            ExtractAndReplaceFiles(zipFilePath);

            // Atualiza o arquivo de versão
            File.WriteAllText($"{extractionPath}version.txt", latestVersion);

            // Executa o App.exe
            StartApplication(); // Altere para o caminho correto, se necessário
        }


        private void ExtractAndReplaceFiles(string zipFilePath)
        {
            
            System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractionPath, true);
            File.Delete(zipFilePath);

        }

        private void StartApplication(string executableName)
        {
            try
            {
                // Inicia o aplicativo
                Process.Start($"{extractionPath}{executableName}");
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao iniciar o aplicativo: {ex.Message}");
            }
        }

        private void StartApplication()
        {
            try
            {
                string exeNameFile = $"{extractionPath}exeName.txt";
                // Verifica se o arquivo version.txt existe
                if (!File.Exists(exeNameFile))
                {
                    // Cria o arquivo version.txt com a versão padrão "0.0"
                    Directory.CreateDirectory(Path.GetDirectoryName(exeNameFile)); // Cria o diretório ./App se não existir
                    File.WriteAllText(exeNameFile, "app.exe");
                }


                // Lê o caminho do executável do arquivo path.txt
                string executablePath = $"{extractionPath}{File.ReadAllText(exeNameFile)}";// File.ReadAllText("./path.txt").Trim();

                // Inicia o aplicativo
                Process.Start(executablePath);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao iniciar o aplicativo: {ex.Message}");
            }
        }
    }
}