// Copyright (c) 2025 Otto
// Лицензия: MIT (см. LICENSE)

using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace Пробел
{
    internal partial class FormSpace : Form
    {
        private readonly System.Windows.Forms.Timer? clipboardTimer; // Таймер для мониторинга буфера обмена
        private string lastClipboardText = string.Empty;             // Хранит последний текст из буфера обмена для сравнения
        private bool isUpdatingOutput = false;                       // Флаг, предотвращающий рекурсивное обновление в OutputTextBox
        private bool shouldShow = true;                              // Флаг, определяющий, показывать ли форму при запуске

        // Таймер для отложенного копирования (что бы не сбивался курсор при руссном редактировании в полях ввода и вывода)
        private readonly System.Windows.Forms.Timer copyTimer = new() { Interval = 250 };   // Задержка 250 мс

        // Путь к файлу конфига в %TEMP% пользователя
        private readonly string configPath = Path.Combine(Path.GetTempPath(), "config_Пробел.txt");

        // Конструктор формы: инициализация компонентов, событий и меню трея
        internal FormSpace()
        {
            InitializeComponent();

            // По умолчанию скрываем из панели задач
            this.ShowInTaskbar = false;

            // События формы и контролов
            this.Resize += FormSpace_Resize; // Отслеживаем сворачивание/разворачивание
            NotifyIcon1.MouseClick += NotifyIcon1_MouseClick; // Клик по иконке в трее
            CheckBoxCopy.CheckedChanged += CheckBoxCopy_CheckedChanged; // Переключение автокопирования

            // Инициализация обработчика для таймера отложенного копирования
            copyTimer.Tick += CopyTimer_Tick;

            // Создаем контекстное меню для notifyIcon
            var trayMenu = new ContextMenuStrip();

            // Пункт "Скопировать"
            var miCopy = new ToolStripMenuItem("Скопировать")
            {
                Image = Properties.Resources.copyIcon // Иконка из ресурсов
            };
            miCopy.Click += (s, e) => ButtonCopy_Click(s, e);
            trayMenu.Items.Add(miCopy);

            // Пункт "Обновлять буфер"
            var miToggleCopy = new ToolStripMenuItem("Обновлять буфер")
            {
                Checked = CheckBoxCopy.Checked,
                CheckOnClick = true
            };
            miToggleCopy.Click += (s, e) => { CheckBoxCopy.Checked = miToggleCopy.Checked; };
            trayMenu.Items.Add(miToggleCopy);

            // Пункт "Стереть буфер"
            var miClear = new ToolStripMenuItem("Стереть буфер")
            {
                Image = Properties.Resources.clearIcon // Иконка из ресурсов
            };
            miClear.Click += (s, e) =>
            {
                ButtonEraseBuf_Click(s, e);
                UpdateTrayIndicator();
            };
            trayMenu.Items.Add(miClear);

            trayMenu.Items.Add(new ToolStripSeparator());

            // Пункт "Выход"
            var miExit = new ToolStripMenuItem("Выход")
            {
                Image = Properties.Resources.exitIcon // Иконка из ресурсов
            };
            miExit.Click += (s, e) => Application.Exit();
            trayMenu.Items.Add(miExit);

            NotifyIcon1.ContextMenuStrip = trayMenu;
            NotifyIcon1.Visible = true; // иконка всегда видима

            LoadConfig(); // читаем настройки из файла

            try
            {
                // Делам панель индикатора круглой
                PanelIndicator.Width = PanelIndicator.Height;
                var path = new GraphicsPath();
                path.AddEllipse(0, 0, PanelIndicator.Width - 1, PanelIndicator.Height - 1);
                PanelIndicator.Region = new Region(path);
                PanelIndicator.BackColor = Color.Green;

                // Запуск таймера мониторинга буфера обмена
                clipboardTimer = new System.Windows.Forms.Timer { Interval = 200 }; // Задержка 200 мс
                clipboardTimer.Tick += ClipboardTimer_Tick;
                clipboardTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось запустить мониторинг буфера: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик таймера для отложенного копирования
        private void CopyTimer_Tick(object? sender, EventArgs e)
        {
            // Останавливаем таймер, чтобы не повторять копирование
            copyTimer.Stop();

            try
            {
                var text = OutputTextBox.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    // Отключаем монитор буфера перед своей записью
                    clipboardTimer?.Stop();

                    // Копируем текст в буфер
                    Clipboard.SetText(text);

                    // Обновляем lastClipboardText, чтобы монитор не затирал InputTextBox
                    lastClipboardText = text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось скопировать в буфер: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                // Возвращаем оба таймера в рабочее состояние
                clipboardTimer?.Start();
            }
        }

        // Ежесекундный тик таймера для обновления индикатора буфера
        private void ClipboardTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                bool hasText = Clipboard.ContainsText();
                PanelIndicator.BackColor = hasText ? Color.Red : Color.Green;
                UpdateTrayIndicator(); // обновляем цвет в меню трея

                if (hasText)
                {
                    var txt = Clipboard.GetText() ?? string.Empty;
                    if (txt != lastClipboardText)
                    {
                        lastClipboardText = txt;
                        InputTextBox.Text = txt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Обработчик изменения текста в поле "Исходный текст": форматирование и автокопирование
        private void InputTextBox_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                var formatted = FormatText(InputTextBox.Text ?? string.Empty);
                isUpdatingOutput = true;
                OutputTextBox.Text = formatted;
                isUpdatingOutput = false;

                // Запускаем таймер для отложенного копирования
                if (CheckBoxCopy.Checked && !string.IsNullOrEmpty(formatted))
                {
                    copyTimer.Stop(); // Сбрасываем, если уже запущен
                    copyTimer.Start(); // Запускаем копирование с задержкой
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при форматировании: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик изменения текста в выходном поле: автокопирование
        private void OutputTextBox_TextChanged(object sender, EventArgs e)
        {
            if (CheckBoxCopy.Checked && !isUpdatingOutput)
            {
                var text = OutputTextBox.Text ?? string.Empty;
                if (!string.IsNullOrEmpty(text))
                {
                    // Запускаем таймер для отложенного копирования
                    try
                    {
                        copyTimer.Stop(); // Сбрасываем, если уже запущен
                        copyTimer.Start(); // Запускаем копирование с задержкой
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                        $"Не удалось скопировать в буфер после редактирования: {ex.Message}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // Обработчик кнопки "Очистить поля": очищает оба TextBox
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            InputTextBox.Clear();
            OutputTextBox.Clear();
        }

        // Обработчик клика по кнопке "Копировать"
        private void ButtonCopy_Click(object? sender, EventArgs e)
        {
            try
            {
                var text = OutputTextBox.Text;
                if (string.IsNullOrEmpty(text)) return;

                // Отключаем монитор
                clipboardTimer?.Stop();

                // Копируем
                Clipboard.SetText(text);

                // Сразу же обновляем lastClipboardText,
                // чтобы при следующем тике не затирать InputTextBox
                lastClipboardText = text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось скопировать в буфер: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                // Включаем монитор обратно
                clipboardTimer?.Start();
            }
        }


        // Очистка буфера обмена и обновление индикатора
        private void ButtonEraseBuf_Click(object? sender, EventArgs e)
        {
            try
            {
                Clipboard.Clear();
                lastClipboardText = string.Empty;
                PanelIndicator.BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось очистить буфер: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Форматирует строку: убирает лишние пробелы и табуляции
        private static string FormatText(string input)
        {
            var lines = input.Split(["\r\n", "\n"], StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                line = SpaceRegex().Replace(line, " ");
                lines[i] = line;
            }
            return string.Join(Environment.NewLine, lines);
        }

        [GeneratedRegex("[ \t]+", RegexOptions.Compiled)]
        private static partial Regex SpaceRegex();

        // Обработчик переключения автокопирования: сохраняет и сразу копирует существующий текст
        private void CheckBoxCopy_CheckedChanged(object? sender, EventArgs e)
        {
            try
            {
                if (CheckBoxCopy.Checked && !string.IsNullOrEmpty(OutputTextBox.Text))
                {
                    // Отключаем монитор буфера
                    clipboardTimer?.Stop();

                    // Копируем отформатированный текст
                    Clipboard.SetText(OutputTextBox.Text);

                    // Обновляем lastClipboardText, чтобы таймер не перезаписывал InputTextBox
                    lastClipboardText = OutputTextBox.Text;
                }
                // Синхронизируем пункт меню в трее
                if (NotifyIcon1.ContextMenuStrip is ContextMenuStrip cs && cs.Items[1] is ToolStripMenuItem mi)
                {
                    mi.Checked = CheckBoxCopy.Checked;
                }
                // Сохраняем состояние
                SaveConfig(WindowState != FormWindowState.Minimized);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось скопировать в буфер: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                // Включаем монитор буфера обратно
                clipboardTimer?.Start();
            }
        }

        // Обработчик сворачивания/разворачивания формы
        private void FormSpace_Resize(object? sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                // Скрываем форму и оставляем иконку в трее без всплывающих окон
                Hide();
                SaveConfig(false);
            }
            else
            {
                // При разворачивании возвращаем на панель задач
                ShowInTaskbar = true;
                SaveConfig(true);
            }
        }

        // Клик по иконке в трее: левой кнопкой восстанавливаем форму
        private void NotifyIcon1_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Show();
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                SaveConfig(true);
            }
        }

        // Обновляет цвет пункта "Стереть буфер" в меню трея
        private void UpdateTrayIndicator()
        {
            bool hasText = Clipboard.ContainsText();
            if (NotifyIcon1.ContextMenuStrip is ContextMenuStrip cs)
            {
                // Пункт "Скопировать"
                if (cs.Items[0] is ToolStripMenuItem miCopy)
                {
                    miCopy.Enabled = hasText;
                    miCopy.ForeColor = hasText ? Color.Green : SystemColors.ControlText;
                }

                // Пункт "Стереть буфер"
                if (cs.Items[2] is ToolStripMenuItem miClear)
                {
                    miClear.Enabled = hasText;
                    miClear.ForeColor = hasText ? Color.Red : SystemColors.ControlText;
                }
            }
        }

        // Загрузка конфига
        private void LoadConfig()
        {
            bool buffer = false, show = true;
            if (File.Exists(configPath))
            {
                foreach (var line in File.ReadAllLines(configPath))
                {
                    var p = line.Split('=', 2);
                    if (p.Length != 2) continue;
                    var key = p[0].Trim().ToLowerInvariant();
                    var val = p[1].Trim().ToLowerInvariant();
                    if (key == "buffer" && bool.TryParse(val, out var b)) buffer = b;
                    if (key == "show" && bool.TryParse(val, out var s)) show = s;
                }
            }
            CheckBoxCopy.Checked = buffer;
            shouldShow = show; // Устанавливаем флаг видимости
        }

        // Сохранение конфига
        private void SaveConfig(bool show)
        {
            var lines = new[]
            {
                $"buffer={CheckBoxCopy.Checked.ToString().ToLowerInvariant()}",
                $"show={show.ToString().ToLowerInvariant()}"
            };
            File.WriteAllLines(configPath, lines);
        }

        // Ссылка на автора
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://gitflic.ru/project/otto/probel",
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }

        // Предотвращение отображения формы при запуске в свёрнутом режиме
        protected override void SetVisibleCore(bool value)
        {
            if (!IsHandleCreated && !shouldShow)
            {
                CreateHandle(); // Создаём дескриптор формы, чтобы она могла обрабатывать сообщения
                value = false; // Предотвращаем отображение формы
            }
            base.SetVisibleCore(value);
        }

        // Останавливает таймеры при закрытии формы и освобождает ресурсы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                clipboardTimer?.Stop();
                clipboardTimer?.Dispose();
                copyTimer?.Stop();
                copyTimer?.Dispose();
            }
            catch { }
            base.OnFormClosing(e);
        }
    }
}