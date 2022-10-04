using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;

using System.Windows.Threading;


namespace Planetario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public String dir_sistema = @"C:\Users\Jefferson\Documents\Visual Studio 2015\Projects\Planetario\Planetario";
        //public String dir_sistema = Directory.GetCurrentDirectory();

        public string dirSons;
        public String somInicio;
        public String somErro;
        public String somAcerto;

        //Verificar quantos kinects estão conectados
        KinectSensorChooser meuKinect;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            dirSons = dir_sistema + @"\som\";
            somErro = dirSons + "inicio.wav";
            //somInicio = @"E:\jefferson\OneDrive\FATEC\FATEC Projeto HAE\Curso Kinect\Projetos\Planetario\Planetario\som\inicio.wav";
            somErro = dirSons + "erro.wav";
            //somErro = @"E:\jefferson\OneDrive\FATEC\FATEC Projeto HAE\Curso Kinect\Projetos\Planetario\Planetario\som\erro.wav";
            somAcerto = dirSons + "acerto.wav";
            //somAcerto = @"E:\jefferson\OneDrive\FATEC\FATEC Projeto HAE\Curso Kinect\Projetos\Planetario\Planetario\som\acerto.wav";
            

            //tratando a conexão e desconexão do kinect
            meuKinect = new KinectSensorChooser();
            //vai enviar a mensagem
            meuKinect.KinectChanged += meuKinect_KinectChanged;
            //se estiver conectado vai mandar para variavel
            sensorChooserUI.KinectSensorChooser = meuKinect;
            //vai iniciar o kinect
            meuKinect.Start();

            //som
            System.Media.SoundPlayer MeuPlayer = new System.Media.SoundPlayer(somInicio);
            MeuPlayer.PlayLooping();
        }

        //verificar conexão
        void meuKinect_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            bool error = true;

            if (e.OldSensor == null)
            {
                try
                {
                    e.OldSensor.DepthStream.Disable();
                    e.OldSensor.SkeletonStream.Disable();
                }
                catch (Exception)
                {
                    error = true;
                }
            }

            if (e.NewSensor == null)
               return;

            try
            {
                //kinect xbox360
                e.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                e.NewSensor.SkeletonStream.Enable();

                try
                {
                    //kinect windows
                    e.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    e.NewSensor.DepthStream.Range = DepthRange.Near;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                }
                catch (InvalidOperationException)
                {
                    e.NewSensor.DepthStream.Range = DepthRange.Default;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                }
            }
            catch (InvalidOperationException)
            {
                error = true;
            }

            //configurar a região que vamos controlar com o cursor
            ZonaCursor.KinectSensor = e.NewSensor;

        }


        //PLACAR
        int erro, acerto;

        private void acertou()
        {
            acerto++;
            System.Media.SoundPlayer MeuPlayer = new System.Media.SoundPlayer(somAcerto);
            MeuPlayer.Play();

        }

        private void errou()
        {
            erro++;
            System.Media.SoundPlayer MeuPlayer = new System.Media.SoundPlayer(somErro);
            MeuPlayer.Play();

        }


        private void placar()
        {
            lblAcerto.Content = acerto;
            lblErro.Content = erro;
        }


        //FASE
        int contFase;

        public void alterarTextoFase()
        {
            if (contFase == 1)
            {
                fase.Content = "FASE 1";
                descricaoFase.Content = "COLOQUE OS NOMES NOS";
                descricaoFase2.Content = "PLANETAS";

            }
            if (contFase == 2)
            {
                fase.Content = "FASE 2";
                descricaoFase.Content = "COLOQUE OS PLANETAS NA";
                descricaoFase2.Content = "SEQUÊNCIA CORRETA";
            }
            if (contFase == 3)
            {
                fase.Content = "FASE 3";
                descricaoFase.Content = "RESPONDA AS PERGUNTAS";
                descricaoFase2.Content = "SOBRE OS PLANETAS";
            }

            if (contFase == 4)
            {
                iniciarPlacar.Margin = new Thickness(0, 0, 0, 200);
                btnProximo.Margin = new Thickness(0, 800, 0, 0);
                fase.Content = "PLACAR";
                descricaoFase.Content = "ACERTOS: " + acerto;
                descricaoFase2.Content = "ERROS: " + erro;
            }
        }

        private void alterarTela()
        {
            inicio.Margin = new Thickness(2000, 0, 0, 0);
            primeiraFase.Margin = new Thickness(2000, 0, 0, 0);
            segundaFase.Margin = new Thickness(2000, 0, 0, 0);
            terceiraFase.Margin = new Thickness(2000, 0, 0, 0);
            transicao.Margin = new Thickness(2000, 0, 0, 0);
        }

        private void btnProximo_Click(object sender, RoutedEventArgs e)
        {
            if (contFase == 1)
            {

                System.Media.SoundPlayer MeuPlayer = new System.Media.SoundPlayer(somAcerto);
                MeuPlayer.Play();

                alterarTela();
                primeiraFase.Margin = new Thickness(0, 0, 0, 0);
                iniciarPlacar.Margin = new Thickness(0, 0, 0, 0);
            }

            if (contFase == 2)
            {
                alterarTela();
                segundaFase.Margin = new Thickness(0, 0, 0, 0);
            }

            if (contFase == 3)
            {
                alterarTela();
                terceiraFase.Margin = new Thickness(0, 0, 0, 0);

                iniciarPerguntas();
            }


        }


        //TELA INICIAL

        //botao JOGAR
        private void btnJogar_Click(object sender, RoutedEventArgs e)
        {

            contFase = 1;
            alterarTextoFase();
            alterarTela();
            transicao.Margin = new Thickness(0, 0, 0, 0);
           
           


        }

        private void btnJogar_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage img_jogar = new BitmapImage(new Uri("img/inicio/inicio.png", UriKind.Relative));
            imgPainel.Source = img_jogar;
        }


        private void btnAjuda_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage img_ajuda = new BitmapImage(new Uri("img/inicio/ajuda.png", UriKind.Relative));
            imgPainel.Source = img_ajuda;
        }


        //PRIMEIRA FASE ==========================================

        int jupiter1 = 0;
        int marte1 = 0;
        int mercurio1 = 0;
        int netuno1 = 0;
        int saturno1 = 0;
        int terra1 = 0;
        int urano1 = 0;
        int venus1 = 0;


        BitmapImage imgNome = new BitmapImage(new Uri("img/nomes/nome.png", UriKind.Relative));




        //sumir
        private void sumirBtn()
        {
            btnMercurio1.Margin = new Thickness(0, 500, 0, 0);
            btnVenus1.Margin = new Thickness(0, 500, 0, 0);
            btnTerra1.Margin = new Thickness(0, 500, 0, 0);
            btnMarte1.Margin = new Thickness(0, 500, 0, 0);
            btnJupiter1.Margin = new Thickness(0, 500, 0, 0);
            btnSaturno1.Margin = new Thickness(0, 500, 0, 0);
            btnUrano1.Margin = new Thickness(0, 500, 0, 0);
            btnnetuno1.Margin = new Thickness(0, 500, 0, 0);

            //aparece nome Selecionado
            planetaSelecionado.Margin = new Thickness(0, 50, 0, 0);
            //fundo
            BitmapImage img = new BitmapImage(new Uri("img/inicio/lousa.png", UriKind.Relative));
            fundoGrid.Source = img;


        }

        private void aparecerBtn()
        {
            planetaSelecionado.Margin = new Thickness(0, 500, 0, 0);
            BitmapImage img = new BitmapImage(new Uri("img/fase1/borda_painel.png", UriKind.Relative));
            fundoGrid.Source = img;


            if (jupiter1 == 0)
            {
                btnJupiter1.Margin = new Thickness(0, 0, 700, 140);
            }
            if (marte1 == 0)
            {
                btnMarte1.Margin = new Thickness(0, 0, 225, 140);
            }
            if (saturno1 == 0)
            {
                btnSaturno1.Margin = new Thickness(240, 0, 0, 140);
            }
            if (urano1 == 0)
            {
                btnUrano1.Margin = new Thickness(700, 0, 0, 140);
            }
            if (mercurio1 == 0)
            {
                btnMercurio1.Margin = new Thickness(0, 70, 700, 0);
            }
            if (netuno1 == 0)
            {
                btnnetuno1.Margin = new Thickness(0, 70, 225, 0);
            }
            if (terra1 == 0)
            {
                btnTerra1.Margin = new Thickness(240, 70, 0, 0);
            }
            if (venus1 == 0)
            {
                btnVenus1.Margin = new Thickness(700, 70, 0, 0);
            }

        }


        private void btnAjuda_Click(object sender, RoutedEventArgs e)
        {

        }

        //concluir fase
        private void fimPrimereiraFase()
        {
            if (jupiter1 == 2 && marte1 == 2 && mercurio1 == 2 && netuno1 == 2 && saturno1 == 2 && terra1 == 2 && urano1 == 2 && venus1 == 2)
            {
                planetaSelecionado.Content = "PARABÉNS";

                sumirBtn();

                btnProximaTela.Margin = new Thickness(700, 0, 0, 0);

                contFase = 2;


            }
        }


        private void aparecerNomeFase()
        {
            btnNomeMercurio.Visibility = Visibility.Visible;
            btnNomeVenus.Visibility = Visibility.Visible;
            btnNomeTerra.Visibility = Visibility.Visible;
            btnNomeMarte.Visibility = Visibility.Visible;
            btnNomeJupiter.Visibility = Visibility.Visible;
            btnNomeSaturno.Visibility = Visibility.Visible;
            btnNomeUrano.Visibility = Visibility.Visible;
            btnNomenetuno.Visibility = Visibility.Visible;
        }

        //BOTÕES

        private void btnJupiter1_Click(object sender, RoutedEventArgs e)
        {
            aparecerNomeFase();

            planetaSelecionado.Content = "JÚPITER";

            jupiter1 = 1;

            sumirBtn();

        }

        private void btnNomeJupiter_Click(object sender, RoutedEventArgs e)
        {
            if (jupiter1 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/nomes/jupiter.png", UriKind.Relative));
                imgNomeJupiter.Source = img;

                imgJupiter1.Source = imgNome;

                btnJupiter1.IsEnabled = false;

                jupiter1 = 2;

                acertou();

                aparecerBtn();

                fimPrimereiraFase();
            }
            if (jupiter1 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnMarte1_Click(object sender, RoutedEventArgs e)
        {
            aparecerNomeFase();

            planetaSelecionado.Content = "MARTE";
            marte1 = 1;

            sumirBtn();
        }

        private void btnNomeMarte_Click(object sender, RoutedEventArgs e)
        {
            if (marte1 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/nomes/marte.png", UriKind.Relative));
                imgNomeMarte.Source = img;

                imgMarte1.Source = imgNome;

                btnMarte1.IsEnabled = false;

                marte1 = 2;

                acertou();

                aparecerBtn();
                fimPrimereiraFase();
            }
            if (marte1 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnSaturno1_Click(object sender, RoutedEventArgs e)
        {
            aparecerNomeFase();
            planetaSelecionado.Content = "SATURNO";
            saturno1 = 1;

            sumirBtn();
        }

        private void btnNomeSaturno_Click(object sender, RoutedEventArgs e)
        {
            if (saturno1 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/nomes/saturno.png", UriKind.Relative));
                imgNomeSaturno.Source = img;

                imgSaturno1.Source = imgNome;

                btnSaturno1.IsEnabled = false;

                saturno1 = 2;
                acertou();

                aparecerBtn();
                fimPrimereiraFase();
            }
            if (saturno1 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnUrano1_Click(object sender, RoutedEventArgs e)
        {
            aparecerNomeFase();
            planetaSelecionado.Content = "URANO";

            urano1 = 1;

            sumirBtn();
        }

        private void btnNomeUrano_Click(object sender, RoutedEventArgs e)
        {
            if (urano1 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/nomes/urano.png", UriKind.Relative));
                imgNomeUrano.Source = img;

                imgUrano1.Source = imgNome;

                btnUrano1.IsEnabled = false;

                urano1 = 2;
                acertou();

                aparecerBtn();
                fimPrimereiraFase();
            }
            if (urano1 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnMercurio1_Click(object sender, RoutedEventArgs e)
        {
            aparecerNomeFase();
            planetaSelecionado.Content = "MERCÚRIO";
            mercurio1 = 1;

            sumirBtn();
        }

        private void btnNomeMercurio_Click(object sender, RoutedEventArgs e)
        {
            if (mercurio1 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/nomes/mercurio.png", UriKind.Relative));
                imgNomeMercurio.Source = img;

                imgMercurio1.Source = imgNome;

                btnMercurio1.IsEnabled = false;

                mercurio1 = 2;
                acertou();

                aparecerBtn();
                fimPrimereiraFase();
            }
            if (mercurio1 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnnetuno1_Click(object sender, RoutedEventArgs e)
        {
            aparecerNomeFase();
            planetaSelecionado.Content = "NETUNO";
            netuno1 = 1;

            sumirBtn();

        }

        private void btnNomenetuno_Click(object sender, RoutedEventArgs e)
        {
            if (netuno1 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/nomes/netuno.png", UriKind.Relative));
                imgNomenetuno.Source = img;

                imgnetuno1.Source = imgNome;

                btnnetuno1.IsEnabled = false;

                netuno1 = 2;
                acertou();

                aparecerBtn();
                fimPrimereiraFase();
            }
            if (netuno1 == 0)
            {
                errou();
            }

            placar();

        }

        private void btnTerra1_Click(object sender, RoutedEventArgs e)
        {
            aparecerNomeFase();
            planetaSelecionado.Content = "TERRA";
            terra1 = 1;

            sumirBtn();
        }

        private void btnNomeTerra_Click(object sender, RoutedEventArgs e)
        {
            if (terra1 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/nomes/terra.png", UriKind.Relative));
                imgNomeTerra.Source = img;

                imgTerra1.Source = imgNome;

                btnTerra1.IsEnabled = false;

                terra1 = 2;
                acertou();

                aparecerBtn();
                fimPrimereiraFase();
            }
            if (terra1 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnVenus1_Click(object sender, RoutedEventArgs e)
        {
            aparecerNomeFase();
            planetaSelecionado.Content = "VÊNUS";
            venus1 = 1;

            sumirBtn();
        }

        private void btnNomeVenus_Click(object sender, RoutedEventArgs e)
        {
            if (venus1 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/nomes/venus.png", UriKind.Relative));
                imgNomeVenus.Source = img;

                imgVenus1.Source = imgNome;

                btnVenus1.IsEnabled = false;

                venus1 = 2;
                acertou();

                aparecerBtn();
                fimPrimereiraFase();
            }
            if (venus1 == 0)
            {
                errou();
            }

            placar();
        }





        //SEGUNDA FASE ==========================================

        int jupiter2 = 0;
        int marte2 = 0;
        int mercurio2 = 0;
        int netuno2 = 0;
        int saturno2 = 0;
        int terra2 = 0;
        int urano2 = 0;
        int venus2 = 0;

        BitmapImage imgF2 = new BitmapImage(new Uri("img/fase1/borda_planeta.png", UriKind.Relative));


        //concluir fase
        private void fimSegundaFase()
        {
            if (jupiter2 == 2 && marte2 == 2 && mercurio2 == 2 && netuno2 == 2 && saturno2 == 2 && terra2 == 2 && urano2 == 2 && venus2 == 2)
            {
                planetaSelecionado2.Content = "PARABÉNS";

                sumirPlaneta();

                btnProximaTela2.Margin = new Thickness(700, 0, 0, 0);
                imgPlanetaSelecionado2.Margin = new Thickness(0, 500, 0, 0);
                planetaSelecionado2.Margin = new Thickness(180, 50, 0, 0);


                contFase = 2;


            }
        }


        //sumir tela 2
        private void sumirPlaneta()
        {
            btnSelMercurio2.Margin = new Thickness(0, 500, 0, 0);
            btnSelVenus2.Margin = new Thickness(0, 500, 0, 0);
            btnSelTerra2.Margin = new Thickness(0, 500, 0, 0);
            btnSelMarte2.Margin = new Thickness(0, 500, 0, 0);
            btnSelJupiter2.Margin = new Thickness(0, 500, 0, 0);
            btnSelSaturno2.Margin = new Thickness(0, 500, 0, 0);
            btnSelUrano2.Margin = new Thickness(0, 500, 0, 0);
            btnSelnetuno2.Margin = new Thickness(0, 500, 0, 0);

            //aparece nome Selecionado
            planetaSelecionado2.Margin = new Thickness(280, 50, 0, 0);
            imgPlanetaSelecionado2.Margin = new Thickness(0, 0, 700, 0);

            //fundo
            BitmapImage img = new BitmapImage(new Uri("img/inicio/lousa.png", UriKind.Relative));
            fundoGrid2.Source = img;


        }

        private void aparecerPlaneta()
        {
            planetaSelecionado2.Margin = new Thickness(0, 500, 0, 0);
            imgPlanetaSelecionado2.Margin = new Thickness(0, 500, 0, 0);

            BitmapImage img = new BitmapImage(new Uri("img/fase1/borda_painel.png", UriKind.Relative));
            fundoGrid2.Source = img;


            if (jupiter2 == 0)
            {
                btnSelJupiter2.Margin = new Thickness(0, 110, 750, 0);
            }
            if (marte2 == 0)
            {
                btnSelMarte2.Margin = new Thickness(0, 110, 350, 0);
            }
            if (saturno2 == 0)
            {
                btnSelSaturno2.Margin = new Thickness(0, 0, 350, 130);
            }
            if (urano2 == 0)
            {
                btnSelUrano2.Margin = new Thickness(150, 0, 0, 130);
            }
            if (mercurio2 == 0)
            {
                btnSelMercurio2.Margin = new Thickness(650, 0, 0, 130);
            }
            if (netuno2 == 0)
            {
                btnSelnetuno2.Margin = new Thickness(150, 110, 0, 0);
            }
            if (terra2 == 0)
            {
                btnSelTerra2.Margin = new Thickness(0, 0, 750, 130);
            }
            if (venus2 == 0)
            {
                btnSelVenus2.Margin = new Thickness(650, 110, 0, 0);
            }

        }


        public void aparecerEspacoBtn()
        {
            btnMercurio2.Visibility = Visibility.Visible;
            btnVenus2.Visibility = Visibility.Visible;
            btnTerra2.Visibility = Visibility.Visible;
            btnMarte2.Visibility = Visibility.Visible;
            btnJupiter2.Visibility = Visibility.Visible;
            btnSaturno2.Visibility = Visibility.Visible;
            btnUrano2.Visibility = Visibility.Visible;
            btnnetuno2.Visibility = Visibility.Visible;
        }


        //botoes

        private void btnSelTerra2_Click(object sender, RoutedEventArgs e)
        {
            aparecerEspacoBtn();
            sumirPlaneta();
            terra2 = 1;

            planetaSelecionado2.Content = "TERRA";

            BitmapImage img = new BitmapImage(new Uri("img/planetas/terra.png", UriKind.Relative));
            imgPlanetaSelecionado2.Source = img;



        }

        private void btnTerra2_Click(object sender, RoutedEventArgs e)
        {
            if (terra2 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/planetas/terra.png", UriKind.Relative));
                imgTerra2.Source = img;

                imgSelTerra2.Source = imgF2;

                terra2 = 2;
                acertou();

                aparecerPlaneta();
                fimSegundaFase();
            }
            if (terra2 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnSelSaturno2_Click(object sender, RoutedEventArgs e)
        {
            aparecerEspacoBtn();
            sumirPlaneta();
            saturno2 = 1;
            planetaSelecionado2.Content = "SATURNO";

            BitmapImage img = new BitmapImage(new Uri("img/planetas/saturno.png", UriKind.Relative));
            imgPlanetaSelecionado2.Source = img;

        }

        private void btnSaturno2_Click(object sender, RoutedEventArgs e)
        {
            if (saturno2 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/planetas/saturno.png", UriKind.Relative));
                imgSaturno2.Source = img;

                imgSelSaturno2.Source = imgF2;

                acertou();
                saturno2 = 2;

                aparecerPlaneta();
                fimSegundaFase();
            }
            if (saturno2 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnSelUrano2_Click(object sender, RoutedEventArgs e)
        {
            aparecerEspacoBtn();
            sumirPlaneta();
            urano2 = 1;
            planetaSelecionado2.Content = "URANO";

            BitmapImage img = new BitmapImage(new Uri("img/planetas/urano.png", UriKind.Relative));
            imgPlanetaSelecionado2.Source = img;


        }

        private void btnUrano2_Click(object sender, RoutedEventArgs e)
        {
            if (urano2 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/planetas/urano.png", UriKind.Relative));
                imgUrano2.Source = img;

                imgSelUrano2.Source = imgF2;

                urano2 = 2;
                acertou();
                aparecerPlaneta();
                fimSegundaFase();
            }
            if (urano2 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnSelMercurio2_Click(object sender, RoutedEventArgs e)
        {
            aparecerEspacoBtn();
            sumirPlaneta();
            mercurio2 = 1;
            planetaSelecionado2.Content = "MERCÚRIO";
            BitmapImage img = new BitmapImage(new Uri("img/planetas/mercurio.png", UriKind.Relative));
            imgPlanetaSelecionado2.Source = img;

        }

        private void btnMercurio2_Click(object sender, RoutedEventArgs e)
        {
            if (mercurio2 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/planetas/mercurio.png", UriKind.Relative));
                imgMercurio2.Source = img;

                imgSelMercurio2.Source = imgF2;

                mercurio2 = 2;
                aparecerPlaneta();
                acertou();

                fimSegundaFase();
            }
            if (mercurio2 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnSelJupiter2_Click(object sender, RoutedEventArgs e)
        {
            aparecerEspacoBtn();
            sumirPlaneta();
            jupiter2 = 1;

            planetaSelecionado2.Content = "JÚPITER";

            BitmapImage img = new BitmapImage(new Uri("img/planetas/jupiter.png", UriKind.Relative));
            imgPlanetaSelecionado2.Source = img;



        }

        private void btnJupiter2_Click(object sender, RoutedEventArgs e)
        {
            if (jupiter2 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/planetas/jupiter.png", UriKind.Relative));
                imgJupiter2.Source = img;

                imgSelJupiter2.Source = imgF2;

                jupiter2 = 2;
                acertou();
                aparecerPlaneta();
                fimSegundaFase();
            }
            if (jupiter2 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnSelMarte2_Click(object sender, RoutedEventArgs e)
        {
            aparecerEspacoBtn();
            sumirPlaneta();
            marte2 = 1;

            planetaSelecionado2.Content = "MARTE";

            BitmapImage img = new BitmapImage(new Uri("img/planetas/marte.png", UriKind.Relative));
            imgPlanetaSelecionado2.Source = img;



        }

        private void btnMarte2_Click(object sender, RoutedEventArgs e)
        {
            if (marte2 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/planetas/marte.png", UriKind.Relative));
                imgMarte2.Source = img;

                imgSelMarte2.Source = imgF2;

                marte2 = 2;
                acertou();
                aparecerPlaneta();
                fimSegundaFase();
            }
            if (marte2 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnSelnetuno2_Click(object sender, RoutedEventArgs e)
        {
            aparecerEspacoBtn();
            sumirPlaneta();
            netuno2 = 1;

            planetaSelecionado2.Content = "NETUNO";

            BitmapImage img = new BitmapImage(new Uri("img/planetas/netuno.png", UriKind.Relative));
            imgPlanetaSelecionado2.Source = img;



        }

        private void btnnetuno2_Click(object sender, RoutedEventArgs e)
        {
            if (netuno2 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/planetas/netuno.png", UriKind.Relative));
                imgnetuno2.Source = img;

                imgSelnetuno2.Source = imgF2;

                netuno2 = 2;
                acertou();
                aparecerPlaneta();
                fimSegundaFase();

            }
            if (netuno2 == 0)
            {
                errou();
            }

            placar();
        }

        private void btnSelVenus2_Click(object sender, RoutedEventArgs e)
        {
            aparecerEspacoBtn();
            sumirPlaneta();
            venus2 = 1;

            planetaSelecionado2.Content = "VÊNUS";

            BitmapImage img = new BitmapImage(new Uri("img/planetas/venus.png", UriKind.Relative));
            imgPlanetaSelecionado2.Source = img;



        }

        private void btnVenus2_Click(object sender, RoutedEventArgs e)
        {
            if (venus2 == 1)
            {
                BitmapImage img = new BitmapImage(new Uri("img/planetas/venus.png", UriKind.Relative));
                imgVenus2.Source = img;

                imgSelVenus2.Source = imgF2;

                venus2 = 2;
                acertou();
                aparecerPlaneta();
                fimSegundaFase();
            }
            if (venus2 == 0)
            {
                errou();
            }

            placar();
        }



        //FASE 3

        //PERGUNTAS

        int pergunta = 1;

        int resposta = 0;

        int opcao1 = 1;
        int opcao2 = 1;
        int opcao3 = 1;


        public void mudarOpcao()
        {
            opcao1++;
            opcao2++;
            opcao3++;
        }

        public void iniciarPerguntas()
        {
            switch (pergunta)
            {
                case 1:
                    lblPergunta.Content = "QUANTOS PLANETAS EXISTEM ENTRE A TERRA E O SOL?";
                    btnOpcao1.Content = "UM";
                    btnOpcao2.Content = "DOIS";
                    btnOpcao3.Content = "TRÊS";

                    opcao1 = 1;
                    opcao2 = 1;
                    opcao3 = 1;

                    if (resposta == 1)
                    {
                        lblPergunta.Content = "RESPOSTA CORRETA!";
                        resposta = 0;

                        mudarOpcao();

                        esconderBtnPergunta();
                    }


                    break;

                case 2:
                    lblPergunta.Content = "QUANTOS PLANETAS EXISTEM ENTRE SATURNO E O SOL?";
                    btnOpcao1.Content = "CINCO";
                    btnOpcao2.Content = "DOIS";
                    btnOpcao3.Content = "TRÊS";

                    opcao1 = 2;
                    opcao2 = 2;
                    opcao3 = 2;

                    if (resposta == 1)
                    {
                        lblPergunta.Content = "RESPOSTA CORRETA!";
                        resposta = 0;

                        mudarOpcao();
                        esconderBtnPergunta();
                    }

                    break;

                case 3:
                    lblPergunta.Content = "QUAL É O NOME DO PRIMEIRO PLANETA?";
                    btnOpcao1.Content = "NETUNO";
                    btnOpcao2.Content = "VÊNUS";
                    btnOpcao3.Content = "MERCÚRIO";

                    opcao1 = 3;
                    opcao2 = 3;
                    opcao3 = 3;

                    if (resposta == 1)
                    {
                        lblPergunta.Content = "RESPOSTA CORRETA!";
                        resposta = 0;

                        mudarOpcao();
                        esconderBtnPergunta();
                    }


                    break;

                case 4:
                    lblPergunta.Content = "QUAL É O NOME DO MAIOR PLANETA?";
                    btnOpcao1.Content = "SOL";
                    btnOpcao2.Content = "JUPÍTER";
                    btnOpcao3.Content = "SATURNO";

                    opcao1 = 4;
                    opcao2 = 4;
                    opcao3 = 4;

                    if (resposta == 1)
                    {
                        lblPergunta.Content = "RESPOSTA CORRETA!";
                        resposta = 0;

                        mudarOpcao();
                        esconderBtnPergunta();
                    }



                    break;

                case 5:
                    lblPergunta.Content = "EXISTEM QUANTOS PLANETAS NO SISTEMA SOLAR?";
                    btnOpcao1.Content = "NOVE";
                    btnOpcao2.Content = "SETE";
                    btnOpcao3.Content = "OITO";

                    opcao1 = 5;
                    opcao2 = 5;
                    opcao3 = 5;

                    if (resposta == 1)
                    {
                        lblPergunta.Content = "RESPOSTA CORRETA!";
                        resposta = 0;

                        mudarOpcao();
                        esconderBtnPergunta();
                    }

                    break;

            }
            placar();

            if (pergunta > 5)
            {
                contFase = 4;
                alterarTextoFase();
                transicao.Margin = new Thickness(0, 0, 0, 0);
                terceiraFase.Margin = new Thickness(2000, 0, 0, 0);

               
                btnFinalizar.Margin = new Thickness(0, 300, 0, 0);

            }



        }

        public void esconderBtnPergunta()
        {
            btnOpcao1.Margin = new Thickness(0, 800, 650, 0);
            btnOpcao2.Margin = new Thickness(0, 800, 0, 0);
            btnOpcao3.Margin = new Thickness(650, 800, 0, 0);

            btnProximaPergunta.Margin = new Thickness(0, 400, 0, 0);

        }

        public void mostrarBtnPergunta()
        {
            btnOpcao1.Margin = new Thickness(0, 400, 650, 0);
            btnOpcao2.Margin = new Thickness(0, 400, 0, 0);
            btnOpcao3.Margin = new Thickness(650, 400, 0, 0);

            btnProximaPergunta.Margin = new Thickness(0, 800, 0, 0);

        }


        private void iniciar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOpcao1_Click(object sender, RoutedEventArgs e)
        {
            switch (opcao1)
            {
                case 1:
                    resposta = 0;
                    errou();
                    break;
                case 2:
                    resposta = 1;
                    acertou();

                    break;
                case 3:
                    resposta = 0;
                    errou();
                    break;
                case 4:
                    resposta = 0;
                    errou();
                    break;

                case 5:
                    resposta = 0;
                    errou();
                    break;


            }
            iniciarPerguntas();



        }

        private void btnOpcao2_Click(object sender, RoutedEventArgs e)
        {
            switch (opcao2)
            {
                case 1:
                    resposta = 1;
                    acertou();

                    break;
                case 2:
                    resposta = 0;
                    errou();
                    break;
                case 3:
                    resposta = 0;
                    errou();
                    break;
                case 4:
                    resposta = 1;
                    acertou();
                    break;
                case 5:
                    resposta = 0;
                    errou();
                    break;


            }
            iniciarPerguntas();


        }

        private void btnOpcao3_Click(object sender, RoutedEventArgs e)
        {
            switch (opcao3)
            {
                case 1:
                    resposta = 0;
                    errou();
                    break;
                case 2:
                    resposta = 0;
                    errou();
                    break;

                case 3:
                    resposta = 1;
                    acertou();
                    break;
                case 4:
                    resposta = 0;
                    errou();
                    break;
                case 5:
                    resposta = 1;
                    acertou();
                    break;
            }
            iniciarPerguntas();


        }


        private void btnProximaPergunta_Click(object sender, RoutedEventArgs e)
        {

            pergunta++;
            mostrarBtnPergunta();
            iniciarPerguntas();

        }

        private void btnProximaTela_Click(object sender, RoutedEventArgs e)
        {
            alterarTextoFase();
            alterarTela();
            transicao.Margin = new Thickness(0, 0, 0, 0);
        }

        private void btnProximaTela_Click_1(object sender, RoutedEventArgs e)
        {
            contFase = 2;
            alterarTextoFase();
            alterarTela();
            transicao.Margin = new Thickness(0, 0, 0, 0);
        }

        private void btnProximaTela2_Click(object sender, RoutedEventArgs e)
        {
            contFase = 3;
            alterarTextoFase();
            alterarTela();
            transicao.Margin = new Thickness(0, 0, 0, 0);
        }

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnReiniciar_Click(object sender, RoutedEventArgs e)
        {
            erro = 0;
            acerto = 0;
            contFase = 0;

             jupiter1 = 0;
             marte1 = 0;
             mercurio1 = 0;
             netuno1 = 0;
             saturno1 = 0;
             terra1 = 0;
             urano1 = 0;
             venus1 = 0;

              jupiter2 = 0;
              marte2 = 0;
              mercurio2 = 0;
              netuno2 = 0;
              saturno2 = 0;
              terra2 = 0;
              urano2 = 0;
              venus2 = 0;

               pergunta = 1;

               resposta = 0;

               opcao1 = 1;
               opcao2 = 1;
               opcao3 = 1;

               inicio.Margin = new Thickness(0, 0, 0, 0);
               transicao.Margin = new Thickness(2000, 0, 0, 0);
        }





    }
}
