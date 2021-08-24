using Instrumentation.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Un4seen.Bass;

namespace Instrumentation
{
    public partial class Form1 : System.Windows.Forms.Form
    {

        /// <summary>
        /// Частота дескритизации
        /// </summary>
        private static int SF = 44100;
        /// <summary>
        /// Состояние инициализации
        /// </summary>
        public static bool InitDefaultDevices;
        /// <summary>
        /// Канал
        /// </summary>
        public static int Stream;
        /// <summary>
        /// Громкость
        /// </summary>
        public static int Volume = 100;

        /// <summary>
        /// Инициализация Bass.dll
        /// </summary>
        /// <param name="sf"></param>
        /// <returns></returns>
        private static bool InitBass(int sf)
        {
            if (!InitDefaultDevices)
                InitDefaultDevices = Bass.BASS_Init(-1, sf, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            return InitDefaultDevices;
        }

        /// <summary>
        /// Вопроизведение
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="vol"></param>
        public static void Play(string filename, int vol)
        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();
                if (InitBass(SF))
                {

                    Stream = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT);
                    if (Stream != 0)
                    {
                        Volume = vol;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);
                    }

                }
            }
            else
                Bass.BASS_ChannelPlay(Stream, false);


            //MessageBox.Show("Вы выбрали не композицию!", "Сообщение");
        }

        public static void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
        }
        /// <summary>
        /// Пауза
        /// </summary>
        public static void Pause()
        {
            if (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(Stream);
        }
        /// <summary>
        /// Получаем длительность канал в секундах
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static int GetTimeOfStream(int stream)
        {
            long TimeBytes = Bass.BASS_ChannelGetLength(stream);
            double Time = Bass.BASS_ChannelBytes2Seconds(stream, TimeBytes);
            return (int)Time;
        }

        /// <summary>
        /// Полученик текущей позиции в секундах
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static int GetPostOfStream(int stream)
        {
            long pos = Bass.BASS_ChannelGetPosition(stream);
            int posSec = (int)Bass.BASS_ChannelBytes2Seconds(stream, pos);
            return posSec;
        }

        public static void SetPosOfScroll(int stream, int pos)
        {
            Bass.BASS_ChannelSetPosition(stream, (double)pos);
        }
        /// <summary>
        /// Установка громкости
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="vol"></param>
        public static void SetVolumeToStream(int stream, int vol)
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);

        }

        static List<string> pathSoundList = new List<string>();
        static bool bplay { get; set; } = true;
        static int cont { get; set; } = 0;

        static int selectIndex { get; set; } = 0;



        /// <summary>
        /// Метод считывающий имена, количество и тип файлов указанных в каталоге 
        /// </summary>
        public void readTypeFiles(string path)
        {
            listBox1.Items.Clear();
            if (pathSoundList != null)
            {
                pathSoundList.Clear(); //Очищает список
            }


            foreach (string file in Directory.EnumerateFiles(path))//путь
            {
                pathSoundList.Add(file); //Добавляет путь каждого файла в список

                string a = file.Replace(path, "");
                if (a.EndsWith("wav"))
                {
                    a = a.Replace(".wav", "");
                }
                a = a.Replace("-", "    ");
                char[] numbering = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
                a = a.Replace("-", "    ");
                a = a.TrimStart(numbering);
                if (a.StartsWith("    "))
                {
                    a = a.TrimStart();
                    a = a.TrimStart(numbering);
                    a = a.Insert(0, "    ");
                }                
                listBox1.Items.Add(a);
                /*
                char[] numbering_replace = { '-', ' ' };
                a = a.Replace(numbering_replace, "   ");
                listBox1.Items.Add(a);
                */
            }
        }

        public Form1()
        {
            InitializeComponent();

            FormSplash sf = new FormSplash();
            sf.ShowDialog();

            //прячем панель управления
            button1.Visible = false;
            btnStop.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            soundTrackBar.Visible = false;
            pictureBox2.Visible = false;
            volumeTrackBar.Visible = false;

            //таблица 1 - Смычковые струнные инструменты
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 1)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 6)
                    {
                        richTextBox1.Text = "Используется главный приём игры — áрко, при котором звукоизвлечение совершается ведением волоса смычка по струне. Этот приём игры используется по умолчанию при отсутствии указаний на другой основной приём — пиццикато, коль леньо, коль леньо тратто";
                        if (listBox1.SelectedIndex < 1)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 2)
                        {
                            pictureBox1.Image = Resources._01_1__Скрипка;

                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._01_2__Скрипка__альт__виолончель;

                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._01_3__Скрипка__альт__виолончель;

                        }
                        else if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = Resources._01_4__Виолончель__контрабас;

                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._01_5__Контрабас;

                        }
                    }
                }
            };
            //таблица 2 - Скрипка
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 2)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._201__Диапазон;

                    }
                    else if (listBox1.SelectedIndex < 6)
                    {
                        //2
                        richTextBox1.Text = "Каждая струна звучит в своём низком регистре, поскольку левая рука находится в низкой позиции\r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 2)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._202_1__На_первой_струне;

                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._202_2__На_второй_струне;

                        }
                        else if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = Resources._202_3__На_третьей_струне;

                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._202_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 10)
                    {
                        //3
                        richTextBox1.Text = "Первая струна звучит в своём низком регистре (низкая позиция левой руки), вторая — в среднем (средняя позиция), третья — в высоком (высокая позиция)\r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 7)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 8)
                        {
                            pictureBox1.Image = Resources._203_1__На_первой_струне;

                        }
                        else if (listBox1.SelectedIndex < 9)
                        {
                            pictureBox1.Image = Resources._203_2__На_второй_струне;

                        }
                        else if (listBox1.SelectedIndex < 10)
                        {
                            pictureBox1.Image = Resources._203_3__На_третьей_струне;

                        }

                    }
                    else if (listBox1.SelectedIndex < 14)
                    {
                        //4
                        richTextBox1.Text = "Вторая струна звучит в своём низком регистре (низкая позиция левой руки), третья — в среднем (средняя позиция), четвёртая — в высоком (высокая позиция)\r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 11)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 12)
                        {
                            pictureBox1.Image = Resources._204_1__На_второй_струне;

                        }
                        else if (listBox1.SelectedIndex < 13)
                        {
                            pictureBox1.Image = Resources._204_2__На_третьей_струне;

                        }
                        else if (listBox1.SelectedIndex < 14)
                        {
                            pictureBox1.Image = Resources._204_3__На_четвёртой_струне;

                        }
                    }
                    else if (listBox1.SelectedIndex < 17)
                    {
                        //5
                        richTextBox1.Text = "Открытой называется струна, не прижатая к грифу; закрытой — прижатая \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 15)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 16)
                        {
                            pictureBox1.Image = Resources._205_1__На_открытой_первой_струне;

                        }
                        else if (listBox1.SelectedIndex < 17)
                        {
                            pictureBox1.Image = Resources._205_2__На_закрытой_второй_струне;

                        }
                    }
                    else if (listBox1.SelectedIndex < 21)
                    {
                        //6
                        richTextBox1.Text = "Открытой называется струна, не прижатая к грифу; закрытой — прижатая \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 18)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 19)
                        {
                            pictureBox1.Image = Resources._206_1__На_открытой_второй_струне;

                        }
                        else if (listBox1.SelectedIndex < 20)
                        {
                            pictureBox1.Image = Resources._206_2__На_закрытой_третьей_струне;

                        }
                        else if (listBox1.SelectedIndex < 21)
                        {
                            pictureBox1.Image = Resources._206_3__На_закрытой_четвёртой_струне;

                        }
                    }
                    else if (listBox1.SelectedIndex < 26)
                    {
                        //7                        
                        richTextBox1.Text = "Сурди́на — это приспособление в форме гребешка, шайбы или бабочки, надеваемое на подставку или приставляемое к ней со стороны подгрифника. Наиболее крупные сурдины из металла или резины в форме гребешка называются глушителями и используются в основном в бытовых, а не художественных целях" +
                                            "\r\rКак правило, композиторы не уточняют в нотах материал сурдины, хотя от него зависит итоговое звучание. Установка сурдины обозначается con sord., снятие — senza sord. или via sord." +
                                            "\r\rПриём игры — арко";
                        pictureBox1.Image = Resources._207__Сурдины;
                        if (listBox1.SelectedIndex < 22)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }

                    }
                    else if (listBox1.SelectedIndex < 30)
                    {
                        //8
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._208__Разные_зоны_струны;
                        if (listBox1.SelectedIndex < 27)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }


                    }
                    else if (listBox1.SelectedIndex < 36)
                    {
                        //9
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._209__Особые_зоны_струны;
                        if (listBox1.SelectedIndex < 31)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }

                    }
                    else if (listBox1.SelectedIndex < 37)
                    {
                        //10
                        richTextBox1.Text = "При флаутáндо смычок очень легко касается струны и ведётся с высокой скоростью \r\rЗдесь флаутандо исполняется на основе арко";
                        pictureBox1.Image = Resources._210__Переход_к_флаутандо;

                    }
                    else if (listBox1.SelectedIndex < 44)
                    {
                        //11
                        richTextBox1.Text = "Пережим противоположен флаутандо \r\rЗдесь пережим исполняется на основе арко";
                        if (listBox1.SelectedIndex < 38)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 39)
                        {
                            pictureBox1.Image = Resources._211_1__Пережим_усиливающийся;
                        }
                        else if (listBox1.SelectedIndex < 40)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 41)
                        {
                            pictureBox1.Image = Resources._211_3__Пережим___sul_tasto;
                        }
                        else if (listBox1.SelectedIndex < 42)
                        {
                            pictureBox1.Image = Resources._211_4__Пережим___sul_ponticello;
                        }
                        else if (listBox1.SelectedIndex < 43)
                        {
                            pictureBox1.Image = Resources._211_5__Пережим___sub_ponticello;
                        }
                        else if (listBox1.SelectedIndex < 44)
                        {
                            pictureBox1.Image = null;
                        }

                    }
                    else if (listBox1.SelectedIndex < 54)
                    {
                        //12
                        richTextBox1.Text = "Вибрáто образуется при периодических колебаниях звука по высоте, громкости и тембру. Один из параметров всегда преобладает, у струнных инструментов — высота. На смычковых инструментах вибрато исполняется колебательным движением левой руки или её части, которое передаётся пальцу, прижимающему струну \r\rЗдесь вибрато исполняется на основе арко";
                        if (listBox1.SelectedIndex < 45)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 46)
                        {
                            pictureBox1.Image = Resources._212_1__Без_вибрато;
                        }
                        else if (listBox1.SelectedIndex < 47)
                        {
                            pictureBox1.Image = Resources._212_2__Вибрато_обычное;
                        }
                        else if (listBox1.SelectedIndex < 48)
                        {
                            pictureBox1.Image = Resources._212_3__Вибрато_медленное;
                        }
                        else if (listBox1.SelectedIndex < 49)
                        {
                            pictureBox1.Image = Resources._212_4__Вибрато_быстрое;
                        }
                        else if (listBox1.SelectedIndex < 50)
                        {
                            pictureBox1.Image = Resources._212_5__Вибрато_сильное_и_медленное;

                        }
                        else if (listBox1.SelectedIndex < 51)
                        {
                            pictureBox1.Image = Resources._212_6__Вибрато_ускоряющееся;

                        }
                        else if (listBox1.SelectedIndex < 52)
                        {
                            pictureBox1.Image = Resources._212_7__Вибрато_замедляющееся;

                        }
                        else if (listBox1.SelectedIndex < 53)
                        {
                            pictureBox1.Image = Resources._212_8__Вибрато_усиливающееся;

                        }
                        else if (listBox1.SelectedIndex < 54)
                        {
                            pictureBox1.Image = Resources._212_9__Вибрато_не_сразу;

                        }
                    }
                    else if (listBox1.SelectedIndex < 58)
                    {
                        //13
                        if (listBox1.SelectedIndex < 55)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 56)
                        {
                            richTextBox1.Text = "Исполняется периодическими движениями пальца левой руки, в то время как смычок ведётся в одном направлении по одной и той же струне \r\rЗдесь тремоло исполняется на основе арко";
                            pictureBox1.Image = Resources._213_1__Тремоло_левой_рукой;

                        }
                        else if (listBox1.SelectedIndex < 57)
                        {
                            richTextBox1.Text = "Исполняется чередованием направления ведения смычка, в то время как пальцы левой руки прижимают необходимые струны и остаются неподвижными \r\rЗдесь тремоло исполняется на основе арко";
                            pictureBox1.Image = Resources._213_2__Тремоло_смычком__По_одной_и_той_же_струне;

                        }
                        else if (listBox1.SelectedIndex < 58)
                        {
                            richTextBox1.Text = "Исполняется чередованием направления ведения смычка, в то время как пальцы левой руки прижимают необходимые струны и остаются неподвижными \r\rЗдесь тремоло исполняется на основе арко";
                            pictureBox1.Image = Resources._213_3__Тремоло_смычком__С_чередованием_струн;

                        }
                    }
                    else if (listBox1.SelectedIndex < 63)
                    {
                        //14
                        richTextBox1.Text = "Флажолéт — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с арко. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон \r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д.";
                        if (listBox1.SelectedIndex < 59)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 60)
                        {
                            pictureBox1.Image = Resources._214_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 61)
                        {
                            pictureBox1.Image = Resources._214_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 62)
                        {
                            pictureBox1.Image = Resources._214_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 63)
                        {
                            pictureBox1.Image = Resources._214_4__На_четвертой_струне;

                        }
                    }
                    else if (listBox1.SelectedIndex < 64)
                    {
                        //15
                        richTextBox1.Text = "Глиссандо выполняется с помощью передвижения пальца левой руки вдоль струны \r\rЗдесь глиссандо и флажолеты исполняются на основе арко";
                        pictureBox1.Image = Resources._215__Глиссандо_натуральными_флажолетами_на_каждой_струне;


                    }
                    else if (listBox1.SelectedIndex < 67)
                    {
                        //16
                        richTextBox1.Text = "Натуральный флажолет исполняется на открытой струне, искусственный — на закрытой: один палец прижимает струну, а другой одновременно к ней прикасается \r\rЗдесь флажолет исполняется на основе арко";
                        if (listBox1.SelectedIndex < 65)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 66)
                        {
                            pictureBox1.Image = Resources._216_1__Натуральный_флажолет;
                        }
                        else if (listBox1.SelectedIndex < 67)
                        {
                            pictureBox1.Image = Resources._216_2__Искусственный_флажолет;
                        }

                    }
                    else if (listBox1.SelectedIndex < 68)
                    {
                        //17
                        richTextBox1.Text = "Здесь флажолеты исполняются на основе арко";
                        pictureBox1.Image = Resources._217__Хроматическая_гамма_искусственными_флажолетами;
                    }
                    else if (listBox1.SelectedIndex < 69)
                    {
                        //18
                        richTextBox1.Text = "По умолчанию глиссандо выполняется с помощью передвижения пальца левой руки вдоль прижимаемой им струны \r\rПриём игры — арко";
                        pictureBox1.Image = Resources._218__Глиссандо_искусственными_флажолетами;
                    }
                    else if (listBox1.SelectedIndex < 80)
                    {
                        //19
                        if (listBox1.SelectedIndex < 70)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 71)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_1__Длинное_легато;
                        }
                        else if (listBox1.SelectedIndex < 72)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_2__Короткое_легато;
                        }
                        else if (listBox1.SelectedIndex < 73)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая, смягчённая. Между звуками есть микропауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_3__Короткое_портато;
                        }
                        else if (listBox1.SelectedIndex < 74)
                        {
                            richTextBox1.Text = "Атака звуков отчётливая. Между звуками нет паузы. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_4__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 75)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая или акцентированная. Между звуками есть микропауза. Смычок не отрывается от струн.Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_5__Маркато;
                        }
                        else if (listBox1.SelectedIndex < 76)
                        {
                            richTextBox1.Text = "Атака звуков острая и акцентированная. Между звуками есть пауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_6__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 77)
                        {
                            richTextBox1.Text = "Атака звуков очень острая. Между звуками есть пауза. Смычок отскакивает от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_7__Спиккато;
                        }
                        else if (listBox1.SelectedIndex < 78)
                        {
                            richTextBox1.Text = "Атака звуков твёрдая. Между звуками есть пауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_8__Стаккато;
                        }
                        else if (listBox1.SelectedIndex < 79)
                        {
                            richTextBox1.Text = "Атака звуков плотная, твёрдая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_9__Всё_вниз;
                        }
                        else if (listBox1.SelectedIndex < 80)
                        {
                            richTextBox1.Text = "Атака звуков острая, лёгкая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._219_10__Всё_вверх;
                        }
                    }
                    else if (listBox1.SelectedIndex < 91)
                    {
                        //20
                        if (listBox1.SelectedIndex < 81)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 82)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_1__Длинное_легато;
                        }
                        else if (listBox1.SelectedIndex < 83)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_2__Короткое_легато;
                        }
                        else if (listBox1.SelectedIndex < 84)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая, смягчённая. Между звуками есть микропауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_3__Короткое_портато;
                        }
                        else if (listBox1.SelectedIndex < 85)
                        {
                            richTextBox1.Text = "Атака звуков отчётливая. Между звуками нет паузы. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_4__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 86)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая или акцентированная. Между звуками есть микропауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_5__Маркато;
                        }
                        else if (listBox1.SelectedIndex < 87)
                        {
                            richTextBox1.Text = "Атака звуков острая и акцентированная. Между звуками есть пауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_6__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 88)
                        {
                            richTextBox1.Text = "Атака звуков очень острая. Между звуками есть пауза. Смычок отскакивает от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_7__Спиккато;
                        }
                        else if (listBox1.SelectedIndex < 89)
                        {
                            richTextBox1.Text = "Атака звуков твёрдая. Между звуками есть пауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_8__Стаккато___копия;
                        }
                        else if (listBox1.SelectedIndex < 90)
                        {
                            richTextBox1.Text = "Атака звуков плотная, твёрдая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_9__Всё_вниз;
                        }
                        else if (listBox1.SelectedIndex < 91)
                        {
                            richTextBox1.Text = "Атака звуков острая, лёгкая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._220_10__Всё_вверх;
                        }
                    }
                    else if (listBox1.SelectedIndex < 92)
                    {
                        //21
                        richTextBox1.Text = "Рикошет исполняется броском смычка на струну таким образом, чтобы тот подпрыгнул несколько раз. Количество звуков контролируется \r\rЗдесь рикошет исполняется на основе арко";
                        pictureBox1.Image = Resources._221__Рикошет;
                    }
                    else if (listBox1.SelectedIndex < 95)
                    {
                        //22
                        richTextBox1.Text = "Пиццикáто — щипок струны. По умолчанию осуществляется пальцем правой руки";
                        pictureBox1.Image = Resources._222__Пиццикато;
                        if (listBox1.SelectedIndex < 93)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 98)
                    {
                        //23
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 96)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 97)
                        {
                            pictureBox1.Image = Resources._223_1__Пиццикато_правой_рукой;
                        }
                        else if (listBox1.SelectedIndex < 98)
                        {
                            pictureBox1.Image = Resources._223_2__Пиццикато_левой_рукой;
                        }
                    }
                    else if (listBox1.SelectedIndex < 99)
                    {
                        //24
                        richTextBox1.Text = "Бáртоковское пиццикато — щипок струны с такой силой, чтобы та, возвращаясь, ударилась о гриф";
                        pictureBox1.Image = Resources._224__Бартоковское_пиццикато;
                    }
                    else if (listBox1.SelectedIndex < 100)
                    {
                        //25
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                        button1.Visible = false;
                        btnStop.Visible = false;
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        soundTrackBar.Visible = false;
                        pictureBox2.Visible = false;
                        volumeTrackBar.Visible = false;
                    }
                    else if (listBox1.SelectedIndex < 105)
                    {
                        //26
                        richTextBox1.Text = "Здесь флажолеты исполняются на основе пиццикато";
                        if (listBox1.SelectedIndex < 101)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 102)
                        {
                            pictureBox1.Image = Resources._226_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 103)
                        {
                            pictureBox1.Image = Resources._226_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 104)
                        {
                            pictureBox1.Image = Resources._226_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 105)
                        {
                            pictureBox1.Image = Resources._226_4__На_четвёртой_струне;
                        }

                    }
                    else if (listBox1.SelectedIndex < 106)
                    {
                        //27
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                        button1.Visible = false;
                        btnStop.Visible = false;
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        soundTrackBar.Visible = false;
                        pictureBox2.Visible = false;
                        volumeTrackBar.Visible = false;
                    }
                    else if (listBox1.SelectedIndex < 107)
                    {
                        //28
                        richTextBox1.Text = "Коль лéньо — удар древка смычка по струне";
                        pictureBox1.Image = Resources._228__Коль_леньо__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 108)
                    {
                        //29
                        richTextBox1.Text = "Коль леньо трáтто — ведение древка смычка по струне";
                        pictureBox1.Image = Resources._229__Коль_леньо_тратто__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 109)
                    {
                        //30
                        richTextBox1.Text = null;
                        pictureBox1.Image = Resources._230__Коль_леньо_тратто___пережим;
                    }
                    else if (listBox1.SelectedIndex < 114)
                    {
                        //31
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                        if (listBox1.SelectedIndex < 110)
                        {
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                            pictureBox1.Image = Resources._231__Коль_леньо_тратто___разные_зоны_струны;
                        }
                        else if (listBox1.SelectedIndex < 111)
                        {
                            pictureBox1.Image = Resources._231_1__Ordinario;
                        }
                        else if (listBox1.SelectedIndex < 112)
                        {
                            pictureBox1.Image = Resources._231_2__Sul_tasto;
                        }
                        else if (listBox1.SelectedIndex < 113)
                        {
                            pictureBox1.Image = Resources._231_3__Sul_ponticello;
                        }
                    }
                    else if (listBox1.SelectedIndex < 123)
                    {
                        //32
                        if (listBox1.SelectedIndex < 115)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 116)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._232_1__Длинное_легато;
                        }
                        else if (listBox1.SelectedIndex < 117)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._232_2__Короткое_легато;
                        }
                        else if (listBox1.SelectedIndex < 118)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая, смягчённая. Между звуками есть микропауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._232_3__Короткое_портато;
                        }
                        else if (listBox1.SelectedIndex < 119)
                        {
                            richTextBox1.Text = "Атака звуков отчётливая. Между звуками нет паузы. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._232_4__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 120)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая или акцентированная. Между звуками есть микропауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._232_5__Маркато;
                        }
                        else if (listBox1.SelectedIndex < 121)
                        {
                            richTextBox1.Text = "Атака звуков острая и акцентированная. Между звуками есть пауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._232_6__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 122)
                        {
                            richTextBox1.Text = "Атака звуков твёрдая. Между звуками есть пауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._232_7__Стаккато;
                        }
                        else if (listBox1.SelectedIndex < 123)
                        {
                            richTextBox1.Text = "Атака звуков плотная, твёрдая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._232_8__Всё_вниз;
                        }

                    }
                    else if (listBox1.SelectedIndex < 124)
                    {
                        //33
                        richTextBox1.Text = "Рикошет исполняется броском смычка на струну таким образом, чтобы тот подпрыгнул несколько раз. Количество звуков контролируется \r\rЗдесь рикошет исполняется на основе коль леньо ";
                        pictureBox1.Image = Resources._233__Коль_леньо___рикошет;
                    }
                    else if (listBox1.SelectedIndex < 125)
                    {
                        //34
                        richTextBox1.Text = "По умолчанию глиссандо выполняется с помощью передвижения пальца левой руки вдоль прижимаемой им струны";
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 126)
                    {
                        //35
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 127)
                    {
                        //36
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                }
            };
            //таблица 3 - Альт
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 3)
                {
                    richTextBox1.Text = null;
                    pictureBox1.Image = null;
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._301__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 6)
                    {
                        //2
                        richTextBox1.Text = "Каждая струна звучит в своём низком регистре, поскольку левая рука находится в низкой позиции \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 2)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._302_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._302_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = Resources._302_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._302_4__На_четвёртой_струне;
                        }


                    }
                    else if (listBox1.SelectedIndex < 9)
                    {
                        //3
                        pictureBox1.Image = null;
                        richTextBox1.Text = "Первая струна звучит в своём низком регистре (низкая позиция левой руки), вторая — в среднем (средняя позиция) \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 7)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 8)
                        {
                            pictureBox1.Image = Resources._303_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 9)
                        {
                            pictureBox1.Image = Resources._303_2__На_второй_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 12)
                    {
                        //4
                        richTextBox1.Text = "Вторая струна звучит в своём низком регистре (низкая позиция левой руки), третья — в среднем (средняя позиция) \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 10)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 11)
                        {
                            pictureBox1.Image = Resources._304_1__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 12)
                        {
                            pictureBox1.Image = Resources._304_2__На_третьей_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 15)
                    {
                        //5
                        richTextBox1.Text = "Третья струна звучит в своём низко-среднем регистре (низкая и средняя позиции левой руки), четвёртая — в высоком (высокая позиция) \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 13)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 14)
                        {
                            pictureBox1.Image = Resources._305_1__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 15)
                        {
                            pictureBox1.Image = Resources._305_2__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 21)
                    {
                        //6
                        richTextBox1.Text = "Сурдина — это приспособление в форме гребешка, шайбы или бабочки, надеваемое на подставку или приставляемое к ней со стороны подгрифника. Наиболее крупные сурдины из металла или резины в форме гребешка называются глушителями и используются в основном в бытовых, а не художественных целях \r\rКак правило, композиторы не уточняют в нотах материал сурдины, хотя от него во многом зависит итоговое звучание. Установка сурдины обозначается con sord., снятие — senza sord. или via sord. \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 16)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 21)
                        {
                            pictureBox1.Image = Resources._306__Сурдины;
                        }

                    }
                    else if (listBox1.SelectedIndex < 25)
                    {
                        //7
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._307__Разные_зоны_струны;
                        if (listBox1.SelectedIndex < 22)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 31)
                    {
                        //8
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._308__Особые_зоны_струны;
                        if (listBox1.SelectedIndex < 26)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }

                    }
                    else if (listBox1.SelectedIndex < 32)
                    {
                        //9
                        richTextBox1.Text = "При флаутандо смычок очень легко касается струны и ведётся с высокой скоростью \r\rОсновной приём игры — арко";
                        pictureBox1.Image = Resources._309__Переход_к_флаутандо;
                    }
                    else if (listBox1.SelectedIndex < 37)
                    {
                        //10
                        richTextBox1.Text = "Пережим противоположен флаутандо \r\rЗдесь пережим исполняется на основе арко";
                        if (listBox1.SelectedIndex < 33)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 34)
                        {
                            pictureBox1.Image = Resources._310_1__Пережим_усиливающийся;
                        }
                        else if (listBox1.SelectedIndex < 35)
                        {
                            pictureBox1.Image = Resources._310_2__Пережим___sul_tasto;
                        }
                        else if (listBox1.SelectedIndex < 36)
                        {
                            pictureBox1.Image = Resources._310_3__Пережим___sul_ponticello;
                        }
                        else if (listBox1.SelectedIndex < 37)
                        {
                            pictureBox1.Image = Resources._310_4__Пережим___sub_ponticello;
                        }
                    }
                    else if (listBox1.SelectedIndex < 42)
                    {
                        //11
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с арко. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон \r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д. \r\rНатуральный флажолет исполняется на открытой струне";
                        if (listBox1.SelectedIndex < 38)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 39)
                        {
                            pictureBox1.Image = Resources._311_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 40)
                        {
                            pictureBox1.Image = Resources._311_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 41)
                        {
                            pictureBox1.Image = Resources._311_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 42)
                        {
                            pictureBox1.Image = Resources._311_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 47)
                    {
                        //12
                        richTextBox1.Text = "Глиссандо выполняется с помощью передвижения пальца левой руки вдоль струны \r\rЗдесь глиссандо и флажолеты исполняются на основе арко";
                        pictureBox1.Image = Resources._312__Глиссандо_натуральными_флажолетами;
                        if (listBox1.SelectedIndex < 43)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }

                    }
                    else if (listBox1.SelectedIndex < 58)
                    {
                        //13
                        if (listBox1.SelectedIndex < 48)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 49)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_1__Длинное_легато;
                        }
                        else if (listBox1.SelectedIndex < 50)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_2__Короткое_легато;
                        }
                        else if (listBox1.SelectedIndex < 51)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая, смягчённая. Между звуками есть микропауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_3__Короткое_портато;
                        }
                        else if (listBox1.SelectedIndex < 52)
                        {
                            richTextBox1.Text = "Атака звуков отчётливая. Между звуками нет паузы. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх)\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_4__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 53)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая или акцентированная.Между звуками есть микропауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_5__Маркато;
                        }
                        else if (listBox1.SelectedIndex < 54)
                        {
                            richTextBox1.Text = "Атака звуков острая и акцентированная. Между звуками есть пауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_6__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 55)
                        {
                            richTextBox1.Text = "Атака звуков очень острая. Между звуками есть пауза. Смычок отскакивает от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх)\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_7__Спиккато;
                        }
                        else if (listBox1.SelectedIndex < 56)
                        {
                            richTextBox1.Text = "Атака звуков твёрдая. Между звуками есть пауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_8__Стаккато;
                        }
                        else if (listBox1.SelectedIndex < 57)
                        {
                            richTextBox1.Text = "Атака звуков плотная, твёрдая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_9__Всё_вниз;
                        }
                        else if (listBox1.SelectedIndex < 58)
                        {
                            richTextBox1.Text = "Атака звуков острая, лёгкая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._313_10__Всё_вверх;
                        }
                    }
                    else if (listBox1.SelectedIndex < 59)
                    {
                        //14
                        richTextBox1.Text = "Рикошет исполняется броском смычка на струну таким образом, чтобы тот подпрыгнул несколько раз. Количество звуков контролируется \r\rЗдесь рикошет исполняется на основе арко";
                        pictureBox1.Image = Resources._314__Рикошет;
                    }
                    else if (listBox1.SelectedIndex < 63)
                    {
                        //15
                        if (listBox1.SelectedIndex < 60)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 61)
                        {
                            richTextBox1.Text = "Пиццикато — щипок струны. По умолчанию осуществляется пальцем правой руки";
                            pictureBox1.Image = Resources._315_1__Пиццикато__Диапазон;
                        }
                        else if (listBox1.SelectedIndex < 62)
                        {
                            richTextBox1.Text = "Бартоковское пиццикато — щипок струны с такой силой, чтобы та, возвращаясь, ударилась о гриф";
                            pictureBox1.Image = Resources._315_2__Бартоковское_пиццикато;
                        }
                    }
                    else if (listBox1.SelectedIndex < 68)
                    {
                        //16
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 64)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 65)
                        {
                            pictureBox1.Image = Resources._316_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 66)
                        {
                            pictureBox1.Image = Resources._316_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 67)
                        {
                            pictureBox1.Image = Resources._316_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 68)
                        {
                            pictureBox1.Image = Resources._316_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 69)
                    {
                        //17
                        richTextBox1.Text = "Коль леньо — удар древка смычка по струне";
                        pictureBox1.Image = Resources._317__Коль_леньо__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 70)
                    {
                        //18
                        richTextBox1.Text = "Коль леньо тратто — ведение древка смычка по струне";
                        pictureBox1.Image = Resources._318__Коль_леньо_тратто;
                    }
                    else if (listBox1.SelectedIndex < 76)
                    {
                        //19
                        if (listBox1.SelectedIndex < 71)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 72)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._319_1__Короткое_легато;
                        }
                        else if (listBox1.SelectedIndex < 73)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая, смягчённая. Между звуками есть микропауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._319_2__Короткое_портато;
                        }
                        else if (listBox1.SelectedIndex < 74)
                        {
                            richTextBox1.Text = "Атака звуков отчётливая. Между звуками нет паузы. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._319_3__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 75)
                        {
                            richTextBox1.Text = "Атака звуков острая и акцентированная. Между звуками есть пауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._319_4__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 76)
                        {
                            richTextBox1.Text = "Атака звуков плотная, твёрдая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._319_5__Всё_вниз;
                        }
                    }
                }
            };
            //таблица 4 - Виолончель
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 4)
                {
                    richTextBox1.Text = null;
                    pictureBox1.Image = null;
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._401__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 6)
                    {
                        //2
                        richTextBox1.Text = "Каждая струна звучит в своём низком регистре, поскольку левая рука находится в низкой позиции \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 2)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._402_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._402_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = Resources._402_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._402_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 9)
                    {
                        //3
                        richTextBox1.Text = "Первая струна звучит в своём низком регистре (низкая позиция левой руки), вторая — в среднем (средняя позиция) \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 7)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 8)
                        {
                            pictureBox1.Image = Resources._403_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 9)
                        {
                            pictureBox1.Image = Resources._403_2__На_второй_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 12)
                    {
                        //4
                        richTextBox1.Text = "Вторая струна звучит в своём низком регистре (низкая позиция левой руки), третья — в среднем и высоком (средняя и высокая позиции) \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 10)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 11)
                        {
                            pictureBox1.Image = Resources._404_1__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 12)
                        {
                            pictureBox1.Image = Resources._404_2__На_третьей_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 15)
                    {
                        //5
                        richTextBox1.Text = "Третья струна звучит в своём низком регистре (низкая позиция левой руки), четвёртая — в среднем и высоком (средняя и высокая позиции) \r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 13)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 14)
                        {
                            pictureBox1.Image = Resources._405_1__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 15)
                        {
                            pictureBox1.Image = Resources._405_2__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 19)
                    {
                        //6
                        richTextBox1.Text = "Сурдина — это приспособление в форме гребешка, шайбы или бабочки, надеваемое на подставку или приставляемое к ней со стороны подгрифника. Наиболее крупные сурдины из металла или резины в форме гребешка называются глушителями и используются в основном в бытовых, а не художественных целях\r\rКак правило, композиторы не уточняют в нотах материал сурдины, хотя от него во многом зависит итоговое звучание. Установка сурдины обозначается con sord., снятие — senza sord. или via sord.\r\rПриём игры — арко";
                        pictureBox1.Image = Resources._406__Сурдины;
                        if (listBox1.SelectedIndex < 16)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }

                    }
                    else if (listBox1.SelectedIndex < 23)
                    {
                        //7
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._407__Разные_зоны_струны;
                        if (listBox1.SelectedIndex < 20)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }

                    }
                    else if (listBox1.SelectedIndex < 29)
                    {
                        //8
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._408__Особые_зоны_струны;
                        if (listBox1.SelectedIndex < 24)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }

                    }
                    else if (listBox1.SelectedIndex < 30)
                    {
                        //9
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._409__От_колодки_к_кончику_смычка;
                    }
                    else if (listBox1.SelectedIndex < 31)
                    {
                        //10
                        richTextBox1.Text = "При флаутандо смычок очень легко касается струны и ведётся с высокой скоростью  \r\rОсновной приём игры — арко";
                        pictureBox1.Image = Resources._410__Переход_к_флаутандо;
                    }
                    else if (listBox1.SelectedIndex < 32)
                    {
                        //11
                        richTextBox1.Text = "Пережим противоположен флаутандо \r\rЗдесь пережим исполняется на основе арко";
                        pictureBox1.Image = Resources._411__Пережим_усиливающийся;
                    }
                    else if (listBox1.SelectedIndex < 33)
                    {
                        //12
                        richTextBox1.Text = "По умолчанию глиссандо выполняется с помощью передвижения пальца левой руки вдоль прижимаемой им струны \r\rЗдесь глиссандо исполняется на основе арко";
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 38)
                    {
                        //13
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с арко. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон \r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д.";
                        if (listBox1.SelectedIndex < 34)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 35)
                        {
                            pictureBox1.Image = Resources._413_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 36)
                        {
                            pictureBox1.Image = Resources._413_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 37)
                        {
                            pictureBox1.Image = Resources._413_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 38)
                        {
                            pictureBox1.Image = Resources._413_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 43)
                    {
                        //14
                        richTextBox1.Text = "По умолчанию глиссандо выполняется с помощью передвижения пальца левой руки вдоль прижимаемой им струны \r\rЗдесь глиссандо и флажолеты исполняются на основе арко";
                        pictureBox1.Image = Resources._414__Глиссандо_натуральными_флажолетами;
                        if (listBox1.SelectedIndex < 39)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }


                    }
                    else if (listBox1.SelectedIndex < 46)
                    {
                        //15
                        richTextBox1.Text = "Натуральный флажолет исполняется на открытой струне, искусственный — на закрытой: один палец прижимает струну, а другой одновременно к ней прикасается \r\rЗдесь флажолет исполняется на основе арко";
                        if (listBox1.SelectedIndex < 44)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 45)
                        {
                            pictureBox1.Image = Resources._415_1__Натуральный_флажолет;
                        }
                        else if (listBox1.SelectedIndex < 46)
                        {
                            pictureBox1.Image = Resources._415_2__Искусственный_флажолет;
                        }
                    }
                    else if (listBox1.SelectedIndex < 55)
                    {
                        //16
                        if (listBox1.SelectedIndex < 47)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 48)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._416_1__Длинное_легато;
                        }
                        else if (listBox1.SelectedIndex < 49)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._416_2__Короткое_легато;
                        }
                        else if (listBox1.SelectedIndex < 50)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая, смягчённая. Между звуками есть микропауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._416_3__Короткое_портато;
                        }
                        else if (listBox1.SelectedIndex < 51)
                        {
                            richTextBox1.Text = "Атака звуков отчётливая. Между звуками нет паузы. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._416_4__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 52)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая или акцентированная. Между звуками есть микропауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._416_5__Маркато;
                        }
                        else if (listBox1.SelectedIndex < 53)
                        {
                            richTextBox1.Text = "Атака звуков острая и акцентированная. Между звуками есть пауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._416_6__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 54)
                        {
                            richTextBox1.Text = "Атака звуков очень острая. Между звуками есть пауза. Смычок отскакивает от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._416_7__Спиккато;
                        }
                        else if (listBox1.SelectedIndex < 55)
                        {
                            richTextBox1.Text = "Атака звуков плотная, твёрдая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._416_8__Всё_вниз;
                        }
                    }
                    else if (listBox1.SelectedIndex < 56)
                    {
                        //17
                        richTextBox1.Text = "Рикошет исполняется броском смычка на струну таким образом, чтобы тот подпрыгнул несколько раз. Количество звуков контролируется \r\rПриём игры — арко";
                        pictureBox1.Image = Resources._417__Рикошет;
                    }
                    else if (listBox1.SelectedIndex < 61)
                    {
                        //18
                        if (listBox1.SelectedIndex < 57)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 58)
                        {
                            pictureBox1.Image = Resources._418_1__Пиццикато__Диапазон;
                            richTextBox1.Text = "Пиццикато — щипок струны. По умолчанию осуществляется пальцем правой руки";
                        }
                        else if (listBox1.SelectedIndex < 59)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 60)
                        {
                            pictureBox1.Image = Resources._418_3__Бартоковское_пиццикато;
                            richTextBox1.Text = "Бартоковское пиццикато — щипок струны с такой силой, чтобы та, возвращаясь, ударилась о гриф";
                        }
                        else if (listBox1.SelectedIndex < 61)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;

                        }
                    }
                    else if (listBox1.SelectedIndex < 62)
                    {
                        //19
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 67)
                    {
                        //20
                        richTextBox1.Text = "Здесь флажолеты исполняются на основе пиццикато";
                        if (listBox1.SelectedIndex < 63)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 64)
                        {
                            pictureBox1.Image = Resources._420_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 65)
                        {
                            pictureBox1.Image = Resources._420_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 66)
                        {
                            pictureBox1.Image = Resources._420_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 67)
                        {
                            pictureBox1.Image = Resources._420_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 68)
                    {
                        //21
                        richTextBox1.Text = "Коль леньо — удар древка смычка по струне";
                        pictureBox1.Image = Resources._421__Коль_леньо__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 72)
                    {
                        //22
                        richTextBox1.Text = "Коль леньо тратто — ведение древка смычка по струне";
                        if (listBox1.SelectedIndex < 69)
                        {
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                            pictureBox1.Image = Resources._422__Коль_леньо_тратто___разные_зоны_струны;
                        }
                        else if (listBox1.SelectedIndex < 70)
                        {
                            pictureBox1.Image = Resources._422_1__Ordinario;
                        }
                        else if (listBox1.SelectedIndex < 71)
                        {
                            pictureBox1.Image = Resources._422_2__Sul_tasto;
                        }
                        else if (listBox1.SelectedIndex < 72)
                        {
                            pictureBox1.Image = Resources._422_3__Sul_ponticello;
                        }
                    }
                    else if (listBox1.SelectedIndex < 81)
                    {
                        //23
                        if (listBox1.SelectedIndex < 73)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 74)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._423_1__Короткое_легато;
                        }
                        else if (listBox1.SelectedIndex < 75)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая, смягчённая. Между звуками есть микропауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._423_2__Портато;
                        }
                        else if (listBox1.SelectedIndex < 76)
                        {
                            richTextBox1.Text = "Атака звуков отчётливая. Между звуками нет паузы. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._423_3__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 77)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая или акцентированная. Между звуками есть микропауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._423_4__Маркато;
                        }
                        else if (listBox1.SelectedIndex < 78)
                        {
                            richTextBox1.Text = "Атака звуков острая и акцентированная. Между звуками есть пауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._423_5__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 79)
                        {
                            richTextBox1.Text = "Атака звуков очень острая. Между звуками есть пауза. Смычок отскакивает от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх) \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._423_6__Спиккато;
                        }
                        else if (listBox1.SelectedIndex < 80)
                        {
                            richTextBox1.Text = "Атака звуков твёрдая. Между звуками есть пауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._423_7__Стаккато;
                        }
                        else if (listBox1.SelectedIndex < 81)
                        {
                            richTextBox1.Text = "Атака звуков плотная, твёрдая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — коль леньо тратто";
                            pictureBox1.Image = Resources._423_8__Всё_вниз;
                        }
                    }
                    else if (listBox1.SelectedIndex < 82)
                    {
                        //24
                        richTextBox1.Text = "Рикошет исполняется броском смычка на струну таким образом, чтобы тот подпрыгнул несколько раз. Количество звуков контролируется \r\rЗдесь рикошет исполняется на основе коль леньо ";
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 87)
                    {
                        //25
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;

                    }
                    else if (listBox1.SelectedIndex < 92)
                    {
                        //26
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;

                    }
                    else if (listBox1.SelectedIndex < 98)
                    {
                        //27
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;

                    }
                }                
            };
            //таблица 5 - Контрабас
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 5)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._501__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 6)
                    {
                        //2
                        richTextBox1.Text = "Каждая струна звучит в своём низком регистре, поскольку левая рука находится в низкой позиции\r\rПриём игры — арко";
                        if (listBox1.SelectedIndex < 2)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._502_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._502_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = Resources._502_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._502_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 10)
                    {
                        //3
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._503__Разные_зоны_струны;
                        if (listBox1.SelectedIndex < 7)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 16)
                    {
                        //4
                        richTextBox1.Text = "Приём игры — арко";
                        pictureBox1.Image = Resources._504__Особые_зоны_струны;
                        if (listBox1.SelectedIndex < 11)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }

                    }
                    else if (listBox1.SelectedIndex < 17)
                    {
                        //5
                        richTextBox1.Text = "При флаутандо смычок очень легко касается струны и ведётся с высокой скоростью  \r\rЗдесь флаутандо исполняется на основе арко";
                        pictureBox1.Image = Resources._505__Переход_к_флаутандо;
                    }
                    else if (listBox1.SelectedIndex < 22)
                    {
                        //6
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с арко. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон. \r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона(и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона(и звучанию струны на дуодециму выше) и т. д.";
                        if (listBox1.SelectedIndex < 18)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 19)
                        {
                            pictureBox1.Image = Resources._506_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 20)
                        {
                            pictureBox1.Image = Resources._506_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 21)
                        {
                            pictureBox1.Image = Resources._506_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 22)
                        {
                            pictureBox1.Image = Resources._506_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 27)
                    {
                        //7
                        richTextBox1.Text = "Глиссандо выполняется с помощью передвижения пальца левой руки вдоль струны \r\rЗдесь глиссандо и флажолеты исполняются на основе арко";
                        pictureBox1.Image = Resources._507__Глиссандо_натуральными_флажолетами;
                        if (listBox1.SelectedIndex < 23)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 28)
                    {
                        //8
                        richTextBox1.Text = "Здесь флажолеты исполняются на основе арко";
                        pictureBox1.Image = Resources._508__Хроматическая_гамма_искусственными_флажолетами;
                    }
                    else if (listBox1.SelectedIndex < 38)
                    {
                        //9
                        if (listBox1.SelectedIndex < 29)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 30)
                        {
                            richTextBox1.Text = "Атака звуков мягкая, сглаженная. Между звуками нет паузы. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_1__Легато;
                        }
                        else if (listBox1.SelectedIndex < 31)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая, смягчённая. Между звуками есть микропауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_2__Портато;
                        }
                        else if (listBox1.SelectedIndex < 32)
                        {
                            richTextBox1.Text = "Атака звуков отчётливая. Между звуками нет паузы. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх)\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_3__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 33)
                        {
                            richTextBox1.Text = "Атака звуков подчёркнутая или акцентированная. Между звуками есть микропауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх)\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_4__Маркато;
                        }
                        else if (listBox1.SelectedIndex < 34)
                        {
                            richTextBox1.Text = "Атака звуков острая и акцентированная. Между звуками есть пауза. Смычок не отрывается от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх)\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_5__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 35)
                        {
                            richTextBox1.Text = "Атака звуков очень острая. Между звуками есть пауза. Смычок отскакивает от струн. Каждый следующий звук исполняется другим направлением ведения смычка (поочерёдно вниз и вверх)\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_6__Спиккато;
                        }
                        else if (listBox1.SelectedIndex < 36)
                        {
                            richTextBox1.Text = "Атака звуков твёрдая. Между звуками есть пауза. Смычок не отрывается от струн. Несколько звуков исполняется на одном направлении ведения смычка\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_7__Стаккато;
                        }
                        else if (listBox1.SelectedIndex < 37)
                        {
                            richTextBox1.Text = "Атака звуков плотная, твёрдая. Между звуками есть пауза. Смычок отрывается от струн\r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_8__Всё_вниз;
                        }
                        else if (listBox1.SelectedIndex < 38)
                        {
                            richTextBox1.Text = "Атака звуков острая, лёгкая. Между звуками есть пауза. Смычок отрывается от струн \r\rПриём игры — арко";
                            pictureBox1.Image = Resources._509_9__Всё_вверх;
                        }
                    }
                    else if (listBox1.SelectedIndex < 43)
                    {
                        //10
                        if (listBox1.SelectedIndex < 39)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 40)
                        {
                            richTextBox1.Text = "Пиццикато — щипок струны. По умолчанию осуществляется пальцем правой руки";
                            pictureBox1.Image = Resources._510_1__Пиццикато__Диапазон;
                        }
                        else if (listBox1.SelectedIndex < 41)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 42)
                        {
                            richTextBox1.Text = "Бартоковское пиццикато — щипок струны с такой силой, чтобы та, возвращаясь, ударилась о гриф";
                            pictureBox1.Image = Resources._510_3__Бартоковское_пиццикато;
                        }
                        else if (listBox1.SelectedIndex < 43)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 48)
                    {
                        //11
                        richTextBox1.Text = "Здесь флажолеты исполняются на основе пиццикато";
                        if (listBox1.SelectedIndex < 44)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 45)
                        {
                            pictureBox1.Image = Resources._511_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 46)
                        {
                            pictureBox1.Image = Resources._511_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 47)
                        {
                            pictureBox1.Image = Resources._511_3__На_третьей_струне;
                        }
                        else if (listBox1.SelectedIndex < 48)
                        {
                            pictureBox1.Image = Resources._511_4__На_четвёртой_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 49)
                    {
                        //12
                        richTextBox1.Text = "Коль леньо — удар древка смычка по струне";
                        pictureBox1.Image = Resources._512__Коль_леньо;
                    }
                    else if (listBox1.SelectedIndex < 50)
                    {
                        //13
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                }
            };
            //таблица 6 - Классическая гитара
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 6)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "Используется главный приём игры — щипок струны пальцем";
                        pictureBox1.Image = Resources._601__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 2)
                    {
                        //2
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 6)
                    {
                        //3
                        richTextBox1.Text = "Первая струна звучит в своём низком регистре (низкая позиция левой руки), каждая следующая — в более высоком";
                        if (listBox1.SelectedIndex < 3)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._603_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = Resources._603_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._603_3__На_третьей_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 11)
                    {
                        //4
                        richTextBox1.Text = null;
                        pictureBox1.Image = Resources._604__Разные_зоны_струны;
                        if (listBox1.SelectedIndex < 7)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 15)
                    {
                        //5
                        richTextBox1.Text = "Вибрато образуется при периодических колебаниях звука по высоте, громкости и тембру. Один из параметров всегда преобладает, у струнных инструментов — высота. На гитаре вибрато исполняется колебательным движением левой руки или её части, которое передаётся пальцу, прижимающему струну";
                        if (listBox1.SelectedIndex < 12)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 13)
                        {
                            pictureBox1.Image = Resources._605_1__Без_вибрато;
                        }
                        else if (listBox1.SelectedIndex < 14)
                        {
                            pictureBox1.Image = Resources._605_2__Вибрато_обычное;
                        }
                        else if (listBox1.SelectedIndex < 15)
                        {
                            pictureBox1.Image = Resources._605_3__Вибрато_быстрое;
                        }
                    }
                    else if (listBox1.SelectedIndex < 16)
                    {
                        //6
                        richTextBox1.Text = "По умолчанию глиссандо выполняется с помощью передвижения пальца левой руки вдоль прижимаемой им струны";
                        pictureBox1.Image = Resources._606__Глиссандо;
                    }
                    else if (listBox1.SelectedIndex < 17)
                    {
                        //7
                        richTextBox1.Text = "Бенд — приём игры, при котором левая рука подтягивает или опускает прижимаемую колеблющуюся струну перпендикулярно грифу. Образуется гладкое глиссандо вверх до полутона";
                        pictureBox1.Image = Resources._607__Бенд;
                    }
                    else if (listBox1.SelectedIndex < 23)
                    {
                        //8
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с щипком. Сразу после щипка прикасающийся палец должен прервать контакт со струной. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон \r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д.";
                        if (listBox1.SelectedIndex < 18)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 19)
                        {
                            pictureBox1.Image = Resources._608_1__Октавные;
                        }
                        else if (listBox1.SelectedIndex < 20)
                        {
                            pictureBox1.Image = Resources._608_2__Квинтовые;
                        }
                        else if (listBox1.SelectedIndex < 21)
                        {
                            pictureBox1.Image = Resources._608_3__Квартовые;
                        }
                        else if (listBox1.SelectedIndex < 22)
                        {
                            pictureBox1.Image = Resources._608_4__Большетерцовые;
                        }
                        else if (listBox1.SelectedIndex < 23)
                        {
                            pictureBox1.Image = null;
                        }
                    }
                    else if (listBox1.SelectedIndex < 26)
                    {
                        //9
                        richTextBox1.Text = "Натуральный флажолет исполняется на открытой струне, искусственный — на закрытой: один палец прижимает струну, а другой одновременно к ней прикасается ";
                        if (listBox1.SelectedIndex < 24)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 25)
                        {
                            pictureBox1.Image = Resources._609_1__Три_натуральных_флажолета;
                        }
                        else if (listBox1.SelectedIndex < 26)
                        {
                            pictureBox1.Image = Resources._609_2__Три_искусственных_флажолета;
                        }
                    }
                    else if (listBox1.SelectedIndex < 27)
                    {
                        //10
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 28)
                    {
                        //11
                        richTextBox1.Text = "Пиццикато — щипок, производимый свободными пальцами правой руки в то время, как ребро её ладони приложено к струнам у подставки";
                        pictureBox1.Image = Resources._611__Пиццикато;
                    }
                    else if (listBox1.SelectedIndex < 29)
                    {
                        //12
                        richTextBox1.Text = "Бартоковское пиццикато — щипок струны с такой силой, чтобы та, возвращаясь, ударилась о гриф";
                        pictureBox1.Image = Resources._612__Бартоковское_пиццикато;
                    }
                    else if (listBox1.SelectedIndex < 30)
                    {
                        //13
                        richTextBox1.Text = "Техническое легато — приём игры, при котором первый звук извлекается правой рукой, а последующие — только левой. Связка нот, требующая подобного исполнения, пишется под лигой";
                        pictureBox1.Image = Resources._613__Техническое_легато;
                    }
                    else if (listBox1.SelectedIndex < 31)
                    {
                        //14
                        richTextBox1.Text = "Расгеáдо — резкий удар какоголибо пальца правой руки вверх или вниз по всем или нескольким соседним струнам одновременно";
                        pictureBox1.Image = Resources._614__Расгеадо;
                    }
                    else if (listBox1.SelectedIndex < 32)
                    {
                        //15
                        richTextBox1.Text = "Расгеадо — резкий удар какоголибо пальца правой руки вверх или вниз по всем или нескольким соседним струнам одновременно";
                        pictureBox1.Image = Resources._615__Расгеадо_по_скрещённым_струнам;
                    }
                    else if (listBox1.SelectedIndex < 33)
                    {
                        //16
                        richTextBox1.Text = "Тамбури́н ― удар боковой части большого пальца правой руки по всем или нескольким соседним струнам у подставки";
                        pictureBox1.Image = Resources._616__Тамбурин;
                    }
                    else if (listBox1.SelectedIndex < 34)
                    {
                        //17
                        richTextBox1.Text = null;
                        pictureBox1.Image = Resources._617__Тремоло_как_на_балалайке;
                    }
                    else if (listBox1.SelectedIndex < 37)
                    {
                        //18
                        richTextBox1.Text = null;
                        pictureBox1.Image = Resources._618_2__Щипок_ногтем;
                        if (listBox1.SelectedIndex < 35)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 36)
                        {
                            pictureBox1.Image = null;
                        }
                    }
                    else if (listBox1.SelectedIndex < 38)
                    {
                        //19
                        richTextBox1.Text = "Струна приглушается полуприжатием её к грифу левой рукой";
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 42)
                    {
                        //20
                        richTextBox1.Text = "Гóльпе ― удар пальцем не по струне";
                        pictureBox1.Image = null;
                        if (listBox1.SelectedIndex < 39)
                        {
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                }
            };
            //таблица 7 - Малая домра
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 7)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "Приём игры — щипок/удар медиатором, или нон тремоло. Это один из двух главных приёмов игры наравне с тремоло медиатором";
                        pictureBox1.Image = Resources._7101__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 5)
                    {
                        //2
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 2)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._7102_1__Одинарные_удары_вниз;
                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._7102_2__Одинарные_удары_вверх;
                        }
                        else if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = Resources._7102_3__Переменные_удары;
                        }
                    }
                    else if (listBox1.SelectedIndex < 8)
                    {
                        //3
                        richTextBox1.Text = "Тремоло исполняется быстрым чередованием ударов правой руки вниз-вверх по одной, двум или трём струнам сразу. По умолчанию на всех домрах звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 6)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 7)
                        {
                            pictureBox1.Image = Resources._7103_1__Тремоло_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 8)
                        {
                            pictureBox1.Image = Resources._7103_2__Тремоло_пальцем;
                        }
                    }
                    else if (listBox1.SelectedIndex < 12)
                    {
                        //4
                        richTextBox1.Text = "Каждая струна звучит в своём низком регистре, поскольку левая рука находится в низкой позиции \r\rПриём игры — тремоло медиатором";
                        if (listBox1.SelectedIndex < 9)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 10)
                        {
                            pictureBox1.Image = Resources._7104_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 11)
                        {
                            pictureBox1.Image = Resources._7104_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 12)
                        {
                            pictureBox1.Image = Resources._7104_3__На_третьей_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 16)
                    {
                        //5
                        richTextBox1.Text = "Первая струна звучит в своём низком регистре (низкая позиция левой руки), каждая следующая — в более высоком \r\rПриём игры — тремоло медиатором";
                        if (listBox1.SelectedIndex < 13)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 14)
                        {
                            pictureBox1.Image = Resources._7105_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 15)
                        {
                            pictureBox1.Image = Resources._7105_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 16)
                        {
                            pictureBox1.Image = Resources._7105_3__На_третьей_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 21)
                    {
                        //6
                        richTextBox1.Text = "Приём игры — тремоло медиатором";
                        pictureBox1.Image = Resources._7106__Разные_зоны_струны;
                        if (listBox1.SelectedIndex < 17)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 24)
                    {
                        //7
                        richTextBox1.Text = null;
                        pictureBox1.Image = Resources._7107__Sub_ponticello_медиатором_и_пальцем;
                        if (listBox1.SelectedIndex < 22)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 25)
                    {
                        //8
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;

                    }
                    else if (listBox1.SelectedIndex < 28)
                    {
                        //9
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 26)
                        {                            
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 27)
                        {
                            richTextBox1.Text = "Вибрато образуется при периодических колебаниях звука по высоте, громкости и тембру. Один из параметров всегда преобладает, у струнных инструментов — высота. \r\rНа балалайке вибрато достигается следующим способом: ребро правой ладони прикладывается к нужной струне за подставкой и с помощью покачиваний осуществляет периодические нажимы на подставку и струну";
                            pictureBox1.Image = Resources._7109_1__Вибрато_как_на_балалайке;
                        }
                        else if (listBox1.SelectedIndex < 28)
                        {
                            richTextBox1.Text = "Вибрато образуется при периодических колебаниях звука по высоте, громкости и тембру. Один из параметров всегда преобладает, у струнных инструментов — высота.";
                            pictureBox1.Image = null;
                        }
                    }
                    else if (listBox1.SelectedIndex < 31)
                    {
                        //10
                        richTextBox1.Text = "По умолчанию глиссандо выполняется с помощью передвижения пальца левой руки вдоль прижимаемой им струны";
                        if (listBox1.SelectedIndex < 29)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 30)
                        {
                            pictureBox1.Image = Resources._7110_1__Глиссандо___удар_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 31)
                        {
                            pictureBox1.Image = Resources._7110_2__Глиссандо___тремоло_медиатором;
                        }
                    }
                    else if (listBox1.SelectedIndex < 35)
                    {
                        //11
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с ударом медиатора. Сразу после звукоизвлечения прикасающийся палец должен прервать контакт со струной. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон \r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д. \r\rНатуральный флажолет исполняется на открытой струне ";
                        if (listBox1.SelectedIndex < 32)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 33)
                        {
                            pictureBox1.Image = Resources._7111_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 34)
                        {
                            pictureBox1.Image = Resources._7111_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 35)
                        {
                            pictureBox1.Image = Resources._7111_3__На_третьей_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 39)
                    {
                        //12
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с щипком. Сразу после щипка прикасающийся палец должен прервать контакт со струной. В результате извлекается какойлибо обертон. Кроме того, флажолетом называется сам извлекаемый обертон \r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д.\r\rНатуральный флажолет исполняется на открытой струне ";
                        if (listBox1.SelectedIndex < 36)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 37)
                        {
                            pictureBox1.Image = Resources._7112_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 38)
                        {
                            pictureBox1.Image = Resources._7112_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 39)
                        {
                            pictureBox1.Image = Resources._7112_3__На_третьей_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 43)
                    {
                        //13
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с тремоло медиатором. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон\r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д.\r\rНатуральный флажолет исполняется на открытой струне ";
                        if (listBox1.SelectedIndex < 40)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 41)
                        {
                            pictureBox1.Image = Resources._7113_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 42)
                        {
                            pictureBox1.Image = Resources._7113_2__На_второй_струне;
                        }
                        else if (listBox1.SelectedIndex < 43)
                        {
                            pictureBox1.Image = Resources._7113_3__На_третьей_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 44)
                    {
                        //14
                        richTextBox1.Text = "Искусственный флажолет исполняется на закрытой струне: один палец прижимает струну, а другой одновременно к ней прикасается ";
                        pictureBox1.Image = Resources._7114__Искусственные_флажолеты_медиатором_на_второй_струне;
                    }
                    else if (listBox1.SelectedIndex < 48)
                    {
                        //15
                        richTextBox1.Text = "Пиццикато — щипок струны пальцем";
                        if (listBox1.SelectedIndex < 45)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 46)
                        {
                            pictureBox1.Image = Resources._7115_1__Пиццикато_большим_пальцем;
                        }
                        else if (listBox1.SelectedIndex < 47)
                        {
                            pictureBox1.Image = Resources._7115_2__Пиццикато_средним_пальцем;
                        }
                        else if (listBox1.SelectedIndex < 48)
                        {
                            pictureBox1.Image = Resources._7115_3__Пиццикато_левой_рукой___тремоло_медиатором;
                        }
                    }
                    else if (listBox1.SelectedIndex < 51)
                    {
                        //16
                        richTextBox1.Text = "Большая дробь на домре — быстрая последовательность ударов вниз по всем струнам пятым, четвёртым, третьим пальцами и медиатором. При обратной дроби совершаются удары вверх третьим, четвёртым и пятым пальцами";
                        if (listBox1.SelectedIndex < 49)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 50)
                        {
                            pictureBox1.Image = Resources._7116_1__Большая_дробь;
                        }
                        else if (listBox1.SelectedIndex < 51)
                        {
                            pictureBox1.Image = Resources._7116_2__Обратная_дробь;
                        }
                    }
                    else if (listBox1.SelectedIndex < 54)
                    {
                        //17
                        richTextBox1.Text = "Струна приглушается полуприжатием её к грифу левой рукой";
                        if (listBox1.SelectedIndex < 52)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 53)
                        {
                            pictureBox1.Image = Resources._7117_1__Ordinario;
                        }
                        else if (listBox1.SelectedIndex < 54)
                        {
                            pictureBox1.Image = Resources._7117_2__Sul_ponticello;
                        }
                    }
                    else if (listBox1.SelectedIndex < 58)
                    {
                        //18
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 55)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 56)
                        {
                            pictureBox1.Image = Resources._7118_1__Легато;
                        }
                        else if (listBox1.SelectedIndex < 57)
                        {
                            pictureBox1.Image = Resources._7118_2__Нон_легато;
                        }
                        else if (listBox1.SelectedIndex < 58)
                        {
                            pictureBox1.Image = Resources._7118_3__Мартле;
                        }
                    }
                    else if (listBox1.SelectedIndex < 64)
                    {
                        //19
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 59)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 60)
                        {
                            pictureBox1.Image = Resources._7119_1__Легато;
                        }
                        else if (listBox1.SelectedIndex < 61)
                        {
                            pictureBox1.Image = Resources._7119_2__Деташе;
                        }
                        else if (listBox1.SelectedIndex < 62)
                        {
                            pictureBox1.Image = Resources._7119_3__Нон_легато;
                        }
                        else if (listBox1.SelectedIndex < 63)
                        {
                            pictureBox1.Image = Resources._7119_4__Мартле;
                        }
                        else if (listBox1.SelectedIndex < 64)
                        {
                            pictureBox1.Image = Resources._7119_5__Стаккато;
                        }
                    }
                    else if (listBox1.SelectedIndex < 65)
                    {
                        //20
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                }
            };
            //таблица 8 - Домра пикколо
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 8)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "По умолчанию на всех домрах звук извлекается медиатором";
                        pictureBox1.Image = Resources._7201__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 2)
                    {
                        //2
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                        button1.Visible = false;
                        btnStop.Visible = false;
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        soundTrackBar.Visible = false;
                        pictureBox2.Visible = false;
                        volumeTrackBar.Visible = false;
                    }
                    else if (listBox1.SelectedIndex < 4)
                    {
                        //3-4
                        richTextBox1.Text = "По умолчанию на всех домрах звук извлекается медиатором";
                        pictureBox1.Image = Resources._7202__Разные_зоны_струны;
                    }
                }
            };
            //таблица 9 - Альтовая домра
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 9)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "По умолчанию на всех домрах звук извлекается медиатором";
                        pictureBox1.Image = Resources._7301__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 2)
                    {
                        //2
                        richTextBox1.Text = null;
                        pictureBox1.Image = Resources._7302__Тремоло_медиатором;
                    }
                    else if (listBox1.SelectedIndex < 3)
                    {
                        //3
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                        button1.Visible = false;
                        btnStop.Visible = false;
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        soundTrackBar.Visible = false;
                        pictureBox2.Visible = false;
                        volumeTrackBar.Visible = false;
                    }
                    else if (listBox1.SelectedIndex < 7)
                    {
                        //4-7
                        richTextBox1.Text = "По умолчанию на всех домрах звук извлекается медиатором";
                        pictureBox1.Image = Resources._7303__Разные_зоны_струны;
                    }
                }
            };
            //таблица 10 - Басовая домра
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 10)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "По умолчанию на всех домрах звук извлекается медиатором";
                        pictureBox1.Image = Resources._7401__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 2)
                    {
                        //2
                        richTextBox1.Text = null;
                        pictureBox1.Image = Resources._7402__Тремоло_медиатором;
                    }
                    else if (listBox1.SelectedIndex < 3)
                    {
                        //3
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                        button1.Visible = false;
                        btnStop.Visible = false;
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        soundTrackBar.Visible = false;
                        pictureBox2.Visible = false;
                        volumeTrackBar.Visible = false;
                    }
                    else if (listBox1.SelectedIndex < 7)
                    {
                        //4-7
                        richTextBox1.Text = "По умолчанию на всех домрах звук извлекается медиатором";
                        pictureBox1.Image = Resources._7403__Разные_зоны_струны;
                    }

                }
            };
            //таблица 11 - Балалайка прима
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 11)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "По умолчанию на всех домрах звук извлекается медиатором";
                        pictureBox1.Image = Resources._8101__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 2)
                    {
                        //2
                        richTextBox1.Text = "Бря́цание — удар указательного пальца правой руки вниз или вверх по всем струнам одновременно";
                        pictureBox1.Image = Resources._8102__Бряцание;
                    }
                    else if (listBox1.SelectedIndex < 3)
                    {
                        //3
                        richTextBox1.Text = "Бряцание — удар указательного пальца правой руки вниз или вверх по всем струнам одновременно \r\rСтруна приглушается полуприжатием её к грифу левой рукой";
                        pictureBox1.Image = Resources._8103__Бряцание_по_приглушённым_струнам;
                    }
                    else if (listBox1.SelectedIndex < 4)
                    {
                        //4
                        richTextBox1.Text = "Тремоло выполняется быстрым чередованием ударов вниз и вверх указательным пальцем правой руки";
                        pictureBox1.Image = Resources._8104__Тремоло;
                    }
                    else if (listBox1.SelectedIndex < 9)
                    {
                        //5
                        if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._8105_1__Пиццикато_большим_пальцем_правой_руки;
                            richTextBox1.Text = null;
                        }
                        else if (listBox1.SelectedIndex < 7)
                        {
                            pictureBox1.Image = Resources._8105_2__Двойное_пиццикато;
                            richTextBox1.Text = "Двойное пиццикато образуется из последовательности ударов по одной струне: большим пальцем правой руки вниз и указательным пальцем вверх";
                        }
                        else if (listBox1.SelectedIndex < 8)
                        {
                            pictureBox1.Image = Resources._8105_3__Одинарное_пиццикато__переменные_удары_;
                            richTextBox1.Text = "Одинарное пиццикато образуется из последовательности ударов указательным пальцем правой руки вниз и вверх по одной струне";
                        }
                        else if (listBox1.SelectedIndex < 9)
                        {
                            pictureBox1.Image = Resources._8105_4__Подцеп;
                            richTextBox1.Text = "Подцéп — щипок струны, осуществляемый восходящим движением указательного или среднего пальца правой руки";
                        }
                    }
                    else if (listBox1.SelectedIndex < 10)
                    {
                        //6
                        richTextBox1.Text = "Гитарный приём — это такой приём, при котором звук извлекается любыми пальцами правой руки при относительной пассивности кисти и предплечья.";
                        pictureBox1.Image = Resources._8106__Гитарный_приём;
                    }
                    else if (listBox1.SelectedIndex < 11)
                    {
                        //7
                        richTextBox1.Text = "Sub ponticello — зона струны за подставкой";
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 15)
                    {
                        //8
                        richTextBox1.Text = "Малая дробь на балалайке — быстрая последовательность ударов вниз по всем струнам четырьмя пальцами. Большая дробь исполняется с добавлением удара большого пальца. При обратной дроби совершаются удары вверх четырьмя пальцами, кроме большого";
                        if (listBox1.SelectedIndex < 12)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 13)
                        {
                            pictureBox1.Image = Resources._8108_1__Малая_дробь;
                        }
                        else if (listBox1.SelectedIndex < 14)
                        {
                            pictureBox1.Image = Resources._8108_2__Большая_дробь;
                        }
                        else if (listBox1.SelectedIndex < 15)
                        {
                            pictureBox1.Image = Resources._8108_3__Обратная_дробь;
                        }

                    }
                    else if (listBox1.SelectedIndex < 19)
                    {
                        //9
                        richTextBox1.Text = "Вибрато образуется при периодических колебаниях звука по высоте, громкости и тембру. Один из параметров всегда преобладает, у струнных инструментов — высота";
                        if (listBox1.SelectedIndex < 16)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 17)
                        {
                            pictureBox1.Image = Resources._8109_1__Вибрато_обычное;
                        }
                        else if (listBox1.SelectedIndex < 18)
                        {
                            pictureBox1.Image = Resources._8109_2__Вибрато_медленное;
                        }
                        else if (listBox1.SelectedIndex < 19)
                        {
                            pictureBox1.Image = Resources._8109_3__Вибрато_быстрое;
                        }
                    }
                    else if (listBox1.SelectedIndex < 22)
                    {
                        //10
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с щипком/ударом. Сразу после звукоизвлечения прикасающийся палец должен прервать контакт со струной. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон\r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д.\r\rНатуральный флажолет исполняется на открытой струне";
                        if (listBox1.SelectedIndex < 20)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 21)
                        {
                            pictureBox1.Image = Resources._8110_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 22)
                        {
                            pictureBox1.Image = Resources._8110_2__На_второй_струне;
                        }
                    }
                    else if (listBox1.SelectedIndex < 23)
                    {
                        //11
                        richTextBox1.Text = "Искусственный флажолет исполняется на закрытой струне: один палец прижимает струну, а другой одновременно к ней прикасается";
                        pictureBox1.Image = Resources._8111__Искусственные_флажолеты_на_первой_струне_от_основного_тона_до_второй_октавы;
                    }
                    else if (listBox1.SelectedIndex < 27)
                    {
                        //12
                        richTextBox1.Text = "Техническое легато — приём игры, при котором первый звук извлекается правой рукой, а последующие — только левой. Связка нот, требующая подобного исполнения, пишется под лигой. Ноты, исполняемые только левой рукой, отмечаются знаком +";
                        if (listBox1.SelectedIndex < 24)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 25)
                        {
                            pictureBox1.Image = Resources._8112__Восходящее_техническое_легато;
                        }
                        else if (listBox1.SelectedIndex < 26)
                        {
                            pictureBox1.Image = Resources._8112_2_Восходящее_техническое_легато;
                        }
                        else if (listBox1.SelectedIndex < 27)
                        {
                            pictureBox1.Image = Resources._8112_3_Восходящее_техническое_легато;
                        }
                    }
                    else if (listBox1.SelectedIndex < 31)
                    {
                        //13
                        richTextBox1.Text = "Техническое легато — приём игры, при котором первый звук извлекается правой рукой, а последующие — только левой. Связка нот, требующая подобного исполнения, пишется под лигой. Ноты, исполняемые только левой рукой, отмечаются знаком +";
                        if (listBox1.SelectedIndex < 28)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 29)
                        {
                            pictureBox1.Image = Resources._8113__Нисходящее_техническое_легато;
                        }
                        else if (listBox1.SelectedIndex < 30)
                        {
                            pictureBox1.Image = Resources._8113_2_Нисходящее_техническое_легато;
                        }
                        else if (listBox1.SelectedIndex < 31)
                        {
                            pictureBox1.Image = Resources._8113_3_Нисходящее_техническое_легато;
                        }
                    }
                    else if (listBox1.SelectedIndex < 32)
                    {
                        //14
                        richTextBox1.Text = "Техническое легато — приём игры, при котором первый звук извлекается правой рукой, а последующие — только левой. Связка нот, требующая подобного исполнения, пишется под лигой. Ноты, исполняемые только левой рукой, отмечаются знаком +";
                        pictureBox1.Image = Resources._8114__Техническое_легато__Мелодия;
                    }
                    else if (listBox1.SelectedIndex < 37)
                    {
                        //15
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                        if (listBox1.SelectedIndex < 33)
                        {
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 38)
                    {
                        //16
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                }

            };
            //таблица 12 - Балалайка секунда
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 12)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "По умолчанию на балалайке секунде звук извлекается медиатором";
                        pictureBox1.Image = Resources._8201__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 4)
                    {
                        //2
                        richTextBox1.Text = "По умолчанию на балалайке секунде звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 2)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._8202_1__Удар_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._8202_2__Удар_большим_пальцем;
                        }
                    }
                    else if (listBox1.SelectedIndex < 7)
                    {
                        //3
                        richTextBox1.Text = "По умолчанию на балалайке секунде звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 5)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._8203_1__Тремоло_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 7)
                        {
                            pictureBox1.Image = Resources._8203_2__Тремоло_пальцем;
                        }
                    }
                    else if (listBox1.SelectedIndex < 10)
                    {
                        //4
                        richTextBox1.Text = "По умолчанию на балалайке секунде звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 8)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 9)
                        {
                            pictureBox1.Image = Resources._8204_1__Восходящее_арпеджиато_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 10)
                        {
                            pictureBox1.Image = Resources._8204_2__Восходящее_арпеджиато_большим_пальцем;
                        }
                    }

                };
            };
            //таблица 13 - Балалайка альт
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 13)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "По умолчанию на балалайке альте звук извлекается медиатором";
                        pictureBox1.Image = Resources._8301__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 4)
                    {
                        //2
                        richTextBox1.Text = "По умолчанию на балалайке альте звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 2)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._8302_1__Удар_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._8302_2__Удар_большим_пальцем;
                        }
                    }
                    else if (listBox1.SelectedIndex < 7)
                    {
                        //3
                        richTextBox1.Text = "По умолчанию на балалайке альте звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 5)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._8303_1__Тремоло_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 7)
                        {
                            pictureBox1.Image = Resources._8303_2__Тремоло_пальцем;
                        }
                    }
                    else if (listBox1.SelectedIndex < 10)
                    {
                        //4
                        richTextBox1.Text = "По умолчанию на балалайке альте звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 8)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 9)
                        {
                            pictureBox1.Image = Resources._8304_1__Восходящее_арпеджиато_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 10)
                        {
                            pictureBox1.Image = Resources._8304_2__Восходящее_арпеджиато_большим_пальцем;
                        }
                    }
                    else if (listBox1.SelectedIndex < 13)
                    {
                        //5
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с щипком. Сразу после щипка прикасающийся палец должен прервать контакт со струной. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон\r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше) и т. д.\r\rНатуральный флажолет исполняется на открытой струне";
                        if (listBox1.SelectedIndex < 11)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 12)
                        {
                            pictureBox1.Image = Resources._8305_1__На_первой_струне;
                        }
                        else if (listBox1.SelectedIndex < 13)
                        {
                            pictureBox1.Image = Resources._8305_2__На_второй_струне;
                        }
                    }

                };
            };
            //таблица 14 - Балалайка контрабас
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 14)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "По умолчанию на балалайке контрабасе звук извлекается медиатором";
                        pictureBox1.Image = Resources._8401__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 2)
                    {
                        //2
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 3)
                    {
                        //3
                        richTextBox1.Text = null;
                        pictureBox1.Image = Resources._8403__Тремоло_медиатором;
                    }
                    else if (listBox1.SelectedIndex < 6)
                    {
                        //4
                        richTextBox1.Text = "По умолчанию на балалайке контрабасе звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 4)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 5)
                        {
                            pictureBox1.Image = Resources._8404_1__Восходящее_арпеджиато_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._8404_2__Восходящее_арпеджиато_большим_пальцем;
                        }
                    }

                };
            };
            //таблица 15 - Арфа
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 15)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "Используется главный приём игры — пиццикато, или щипок струны пальцем";
                        pictureBox1.Image = Resources._901__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 4)
                    {
                        //2
                        richTextBox1.Text = "Существует два способа исполнения аккорда: арпеджиато или без арпеджиато. Предполагается, что звуки аккорда, записанного в нотах без волнистой линии, извлекаются одновременно. Однако арфисты нередко применяют арпеджиато по своему усмотрению";
                        pictureBox1.Image = Resources._902__Без_арпеджиато_и_с_арпеджиато__Хиндемит__Соната_для_арфы;
                        if (listBox1.SelectedIndex < 2)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 9)
                    {
                        //3
                        richTextBox1.Text = "Глиссандо исполняется проведением одного и того же пальца по всем струнам последовательно в указанном диапазоне";
                        pictureBox1.Image = Resources._903__Глиссандо;
                        if (listBox1.SelectedIndex < 5)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 12)
                    {
                        //4
                        richTextBox1.Text = "Глиссандо исполняется проведением одного и того же пальца по всем струнам последовательно в указанном диапазоне";
                        pictureBox1.Image = null;
                        if (listBox1.SelectedIndex < 10)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 11)
                        {
                            pictureBox1.Image = Resources._904__Глиссандо_разной_плотности;
                        }
                    }
                    else if (listBox1.SelectedIndex < 15)
                    {
                        //5
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с щипком. Сразу после щипка прикасающийся палец должен прервать контакт со струной. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон\r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше). Прикосновение в одной из точек деления на три равных отрезка приводит к извлечению квинтового обертона (и звучанию струны на дуодециму выше)";
                        if (listBox1.SelectedIndex < 13)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 14)
                        {
                            pictureBox1.Image = Resources._905_1__Натуральные_октавные_флажолеты_на_струнах_си__ля_соль_второй_октавы;
                        }
                        else if (listBox1.SelectedIndex < 15)
                        {
                            pictureBox1.Image = Resources._905_2__Натуральные_октавные_и_квинтовые_флажолеты_на_струнах_фа_второй_октавы___си_контроктавы;
                        }
                    }
                    else if (listBox1.SelectedIndex < 21)
                    {
                        //6
                        richTextBox1.Text = "Здесь тремоло исполняется по принципу «нота/созвучие — одна рука, следующая нота/созвучие — другая рука и т. д.»";
                        pictureBox1.Image = null;
                        if (listBox1.SelectedIndex < 16)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;

                        }
                    }
                    else if (listBox1.SelectedIndex < 22)
                    {
                        //7
                        richTextBox1.Text = "Тремоло Эола исполняется пальцами двух рук, совершающими быстрыми поочерёдные движения вперёд и назад по струнам в указанном диапазоне";
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 26)
                    {
                        //8
                        richTextBox1.Text = "Педальное глиссандо предполагает извлечение первого звука с помощью рук, а последующих — лишь с помощью смены позиции педали";
                        pictureBox1.Image = Resources._908__Педальное_глиссандо;
                        if (listBox1.SelectedIndex < 23)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 29)
                    {
                        //9
                        if (listBox1.SelectedIndex < 27)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 28)
                        {
                            pictureBox1.Image = null;
                            richTextBox1.Text = "По умолчанию звук извлекается примерно посередине длины струны";
                        }
                        else if (listBox1.SelectedIndex < 29)
                        {
                            pictureBox1.Image = Resources._909_2__Звукоизвлечение_ближе_к_деке;
                            richTextBox1.Text = "Дéка находится в нижней части арфы";
                        }
                    }
                    else if (listBox1.SelectedIndex < 33)
                    {
                        //10
                        richTextBox1.Text = "По умолчанию на арфе звук извлекается подушечкой пальца, а не ногтем";
                        if (listBox1.SelectedIndex < 30)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 31)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 32)
                        {
                            pictureBox1.Image = Resources._910_2__Звукоизвлечение_ногтем;
                        }
                        else if (listBox1.SelectedIndex < 33)
                        {
                            pictureBox1.Image = Resources._910_3__Звукоизвлечение_ногтем_ближе_к_деке;
                        }
                    }
                    else if (listBox1.SelectedIndex < 34)
                    {
                        //11
                        richTextBox1.Text = "Удар ладонью по струнам в указанном диапазоне";
                        pictureBox1.Image = Resources._911__Эффект_гонга;
                    }
                    else if (listBox1.SelectedIndex < 35)
                    {
                        //12
                        richTextBox1.Text = "Резкое и сильное скольжение пальцем левой руки от одной металлической струны к другой, в результате чего струны бьются друг о друга и продолжают звучать";
                        pictureBox1.Image = Resources._912__Эффект_грома;
                    }
                    else if (listBox1.SelectedIndex < 36)
                    {
                        //13
                        richTextBox1.Text = "Так арфистка В. Дулова назвала звучание струны, испытывающей сильное надавливание в своей нижней или верхней части пальцем свободной руки. Струны, требующие надавливания, отмечаются в нотах дополнительными ромбами.";
                        pictureBox1.Image = Resources._913__Эффект_ксилофона;
                    }
                    else if (listBox1.SelectedIndex < 43)
                    {
                        //14
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                        if (listBox1.SelectedIndex < 37)
                        {
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                    }
                    else if (listBox1.SelectedIndex < 44)
                    {
                        //15
                        pictureBox1.Image = null;
                        richTextBox1.Text = "Установлена полоска бумаги «змейкой» между струн";
                    }
                }

            };
            //таблица 16 - Клавишные гусли
            listBox1.SelectedIndexChanged += (s, a) =>
            {
                if (cont == 16)
                {
                    button1.Visible = true;
                    btnStop.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    label3.Visible = true;
                    soundTrackBar.Visible = true;
                    pictureBox2.Visible = true;
                    volumeTrackBar.Visible = true;

                    if (listBox1.SelectedIndex < 1)
                    {
                        //1
                        richTextBox1.Text = "По умолчанию на клавишных гуслях звук извлекается медиатором";
                        pictureBox1.Image = Resources._1001__Диапазон;
                    }
                    else if (listBox1.SelectedIndex < 4)
                    {
                        //2
                        richTextBox1.Text = "Арпеджиато — главный приём игры. По умолчанию осуществляется проведением медиатора по всем струнам последовательно в указанном диапазоне, при этом звучит лишь часть отыгрываемых струн — та, что освобождена от демпферов";
                        if (listBox1.SelectedIndex < 2)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 3)
                        {
                            pictureBox1.Image = Resources._1002_1__Восходящее_арпеджиато;
                        }
                        else if (listBox1.SelectedIndex < 4)
                        {
                            pictureBox1.Image = Resources._1002_2__Нисходящее_арпеджиато;
                        }
                    }
                    else if (listBox1.SelectedIndex < 7)
                    {
                        //3
                        richTextBox1.Text = "По умолчанию на клавишных гуслях звук извлекается медиатором";
                        if (listBox1.SelectedIndex < 5)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 6)
                        {
                            pictureBox1.Image = Resources._1003_1__Арпеджиато_медиатором;
                        }
                        else if (listBox1.SelectedIndex < 7)
                        {
                            pictureBox1.Image = Resources._1003_2__Арпеджиато_пальцем;
                        }
                    }
                    else if (listBox1.SelectedIndex < 10)
                    {
                        //4
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 8)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 9)
                        {
                            pictureBox1.Image = Resources._1004_1__Удары_медиатором_вверх;
                        }
                        else if (listBox1.SelectedIndex < 10)
                        {
                            pictureBox1.Image = Resources._1004_2__Удары_медиатором_вниз;
                        }
                    }
                    else if (listBox1.SelectedIndex < 13)
                    {
                        //5
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 11)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 12)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 13)
                        {
                            pictureBox1.Image = Resources._1005_2__Арпеджиато___щипок_пальцем;
                        }
                    }
                    else if (listBox1.SelectedIndex < 14)
                    {
                        //6
                        richTextBox1.Text = "Глиссандо исполняется проведением медиатора по всем струнам последовательно в указанном диапазоне. При этом звучат все отыгрываемые струны, это достигается нажатием рычага или всех клавиш";
                        pictureBox1.Image = Resources._1006__Глиссандо_медиатором;
                    }
                    else if (listBox1.SelectedIndex < 17)
                    {
                        //7
                        richTextBox1.Text = "Все струны заглушены демпферами, клавиши не нажаты";
                        if (listBox1.SelectedIndex < 15)
                        {
                            richTextBox1.Text = null;
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 16)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 17)
                        {
                            pictureBox1.Image = null;
                        }
                    }
                    else if (listBox1.SelectedIndex < 18)
                    {
                        //8
                        richTextBox1.Text = null;
                        pictureBox1.Image = null;
                    }
                    else if (listBox1.SelectedIndex < 22)
                    {
                        //9
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 19)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 20)
                        {
                            pictureBox1.Image = Resources._1009_1__Звукоизвлечение_медиатором__мелодия_;
                        }
                        else if (listBox1.SelectedIndex < 21)
                        {
                            pictureBox1.Image = Resources._1009_2__Звукоизвлечение_пальцами__мелодия____копия;
                        }
                        else if (listBox1.SelectedIndex < 22)
                        {
                            pictureBox1.Image = Resources._1009_3__Звукоизвлечение_пальцами__аккорды_;
                        }
                    }
                    else if (listBox1.SelectedIndex < 25)
                    {
                        //10
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 23)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 24)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 25)
                        {
                            pictureBox1.Image = Resources._1010_2__Sul_ponticello;
                        }
                    }
                    else if (listBox1.SelectedIndex < 26)
                    {
                        //11
                        richTextBox1.Text = "Флажолет — это приём игры, который исполняется лёгким прикосновением пальца к струне в точке её деления на несколько равных отрезков. Прикосновение само по себе не производит звукоизвлечение и в данном случае осуществляется вместе с щипком/ударом. Сразу после звукоизвлечения прикасающийся палец должен прервать контакт со струной. В результате извлекается какой-либо обертон. Кроме того, флажолетом называется сам извлекаемый обертон\r\rПрикосновение к струне в точке её деления на два равных отрезка приводит к извлечению октавного обертона (и звучанию струны на октаву выше)";
                        pictureBox1.Image = Resources._1011__Натуральные_октавные_флажолеты;
                    }
                    else if (listBox1.SelectedIndex < 29)
                    {
                        //12
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 27)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 28)
                        {
                            pictureBox1.Image = Resources._1012_1__Удар_ладонью_по_струнам;
                        }
                        else if (listBox1.SelectedIndex < 29)
                        {
                            pictureBox1.Image = null;
                        }
                    }
                    else if (listBox1.SelectedIndex < 34)
                    {
                        //13
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 30)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 31)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 32)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 33)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 34)
                        {
                            pictureBox1.Image = null;
                        }
                    }
                    else if (listBox1.SelectedIndex < 37)
                    {
                        //14
                        richTextBox1.Text = null;
                        if (listBox1.SelectedIndex < 35)
                        {
                            pictureBox1.Image = null;
                            button1.Visible = false;
                            btnStop.Visible = false;
                            label1.Visible = false;
                            label2.Visible = false;
                            label3.Visible = false;
                            soundTrackBar.Visible = false;
                            pictureBox2.Visible = false;
                            volumeTrackBar.Visible = false;
                        }
                        else if (listBox1.SelectedIndex < 36)
                        {
                            pictureBox1.Image = null;
                        }
                        else if (listBox1.SelectedIndex < 37)
                        {
                            pictureBox1.Image = null;
                        }
                    }
                }
            };
        }



        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            richTextBox1.Text = null;
            pictureBox1.Image = null;

            button1.Visible = false;
            btnStop.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            soundTrackBar.Visible = false;
            pictureBox2.Visible = false;
            volumeTrackBar.Visible = false;

            if (treeView1.SelectedNode.Name == "Node0SmoothedSI")
            {
                readTypeFiles(@"sound\01. Смычковые струнные инструменты\");
                cont = 1;
            }
            else if (treeView1.SelectedNode.Name == "Node1Violin")
            {
                readTypeFiles(@"sound\02. Скрипка\");
                cont = 2;
            }
            else if (treeView1.SelectedNode.Name == "Node2Alto")
            {
                readTypeFiles(@"sound\03. Альт\");
                cont = 3;
            }
            else if (treeView1.SelectedNode.Name == "Node3Cello")
            {
                readTypeFiles(@"sound\04. Виолончель\");
                cont = 4;
            }
            else if (treeView1.SelectedNode.Name == "Node4Contrabass")
            {
                readTypeFiles(@"sound\05. Контрабас (четырёхструнный)\");
                cont = 5;
            }
            else if (treeView1.SelectedNode.Name == "Node5ClassicalGuitar")
            {
                readTypeFiles(@"sound\06. Классическая гитара\");
                cont = 6;
            }
            else if (treeView1.SelectedNode.Name == "Node61SmallDomra")
            {
                readTypeFiles(@"sound\07. Домры\01. Малая домра\");
                cont = 7;
            }
            else if (treeView1.SelectedNode.Name == "Node62DomraPiccolo")
            {
                readTypeFiles(@"sound\07. Домры\02. Домра пикколо\");
                cont = 8;
            }
            else if (treeView1.SelectedNode.Name == "Node63AltoDomra")
            {
                readTypeFiles(@"sound\07. Домры\03. Альтовая домра\");
                cont = 9;
            }
            else if (treeView1.SelectedNode.Name == "Node64BassDomra")
            {
                readTypeFiles(@"sound\07. Домры\04. Басовая домра\");
                cont = 10;
            }
            else if (treeView1.SelectedNode.Name == "Node71BalalaikaPrima")
            {
                readTypeFiles(@"sound\08. Балалайки\01. Балалайка прима\");
                cont = 11;
            }
            else if (treeView1.SelectedNode.Name == "Node72BalalaikaSecond")
            {
                readTypeFiles(@"sound\08. Балалайки\02. Балалайка секунда\");
                cont = 12;
            }
            else if (treeView1.SelectedNode.Name == "Node73BalalaikaViola")
            {
                readTypeFiles(@"sound\08. Балалайки\03. Балалайка альт\");
                cont = 13;
            }
            else if (treeView1.SelectedNode.Name == "Node74BalalaikaContrabass")
            {
                readTypeFiles(@"sound\08. Балалайки\04. Балалайка контрабас\");
                cont = 14;
            }
            else if (treeView1.SelectedNode.Name == "Node8Harp")
            {
                readTypeFiles(@"sound\09. Арфа\");
                cont = 15;
            }
            else if (treeView1.SelectedNode.Name == "Node9KeyboardHarp")
            {
                readTypeFiles(@"sound\10. Клавишные гусли\");
                cont = 16;
            }
            else if (treeView1.SelectedNode.Name == "Node0")
            {
                readTypeFiles(@"sound\");
            }
            else if (treeView1.SelectedNode.Name == "Node1")
            {
                readTypeFiles(@"sound\");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (bplay == true)
                {

                    
                    Play(pathSoundList[selectIndex].ToString(), Volume);
                    if (pathSoundList[selectIndex].ToString().EndsWith("wav"))
                    {
                        button1.BackgroundImage = Resources.PauseBtn;
                        label2.Text = TimeSpan.FromSeconds(GetPostOfStream(Stream)).ToString();
                        label3.Text = TimeSpan.FromSeconds(GetTimeOfStream(Stream)).ToString();
                        soundTrackBar.Maximum = GetTimeOfStream(Stream);
                        soundTrackBar.Value = GetPostOfStream(Stream);
                        timer1.Enabled = true;

                        bplay = false;
                    }
                    else
                    {
                        MessageBox.Show("Вы выбрали не композицию!", "Сообщение");
                    }

                }
                else
                {
                    button1.BackgroundImage = Resources.playBtn;
                    Pause();

                    bplay = true;
                }
            }
            catch
            {

            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = TimeSpan.FromSeconds(GetPostOfStream(Stream)).ToString();
            soundTrackBar.Value = GetPostOfStream(Stream);
        }

        private void soundTrackBar_Scroll(object sender, EventArgs e)
        {
            SetPosOfScroll(Stream, soundTrackBar.Value);
        }

        private void volumeTrackBar_Scroll(object sender, EventArgs e)
        {
            SetVolumeToStream(Stream, volumeTrackBar.Value);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                Stop();
                bplay = true;
                button1.BackgroundImage = Resources.playBtn;
                timer1.Enabled = false;
                soundTrackBar.Value = 0;
                label2.Text = "00:00:00";
            }
            catch
            {
                MessageBox.Show("Вы выбрали не композицию!", "Сообщение");
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            InfoForm infoForm = new InfoForm();
            if ((Application.OpenForms["InfoForm"] as InfoForm) != null)
            {
                //Form is already open                
            }
            else
            {
                // Form is not open
                infoForm.Show();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ViewPicture viewPicture = new ViewPicture(pictureBox1.Image);
            if ((Application.OpenForms["ViewPicture"] as ViewPicture) != null)
            {
                //Form is already open                
            }
            else
            {
                // Form is not open
                viewPicture.Show();
            }            
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            selectIndex = listBox1.SelectedIndex;
            label1.Text = listBox1.SelectedItem.ToString();
            button1.BackgroundImage = Resources.playBtn;
            Stop();
            bplay = true;
            timer1.Enabled = false;

            label2.Text = "00:00:00";
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem != null)
            {
                try
                {

                    Stop();

                    button1.BackgroundImage = Resources.PauseBtn;

                    Play(pathSoundList[selectIndex].ToString(), Volume);
                    label2.Text = TimeSpan.FromSeconds(GetPostOfStream(Stream)).ToString();
                    label3.Text = TimeSpan.FromSeconds(GetTimeOfStream(Stream)).ToString();
                    soundTrackBar.Maximum = GetTimeOfStream(Stream);
                    soundTrackBar.Value = GetPostOfStream(Stream);
                    timer1.Enabled = true;

                    bplay = false;

                }
                catch
                {
                    MessageBox.Show("Вы выбрали не композицию!", "Сообщение");
                }
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {
            InfoForm infoForm = new InfoForm();
            infoForm.Show();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var absolutePath = System.IO.Path.Combine(Application.StartupPath, @"sound");
            System.Diagnostics.Process.Start("explorer", absolutePath);
        }
    }
}
