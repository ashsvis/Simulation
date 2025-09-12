namespace Simulator
{
    partial class PanelForm
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelForm));
            TreeNode treeNode1 = new TreeNode("Библиотека");
            menuMainStrip = new MenuStrip();
            файлToolStripMenuItem = new ToolStripMenuItem();
            tsmiCreate = new ToolStripMenuItem();
            tsmiOpen = new ToolStripMenuItem();
            tsmiAddModule = new ToolStripMenuItem();
            toolStripSeparator = new ToolStripSeparator();
            tsmiSave = new ToolStripMenuItem();
            tsmiSaveAs = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmiPrint = new ToolStripMenuItem();
            tsmiPreview = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            tsmiExit = new ToolStripMenuItem();
            изменитьToolStripMenuItem = new ToolStripMenuItem();
            отменитьToolStripMenuItem = new ToolStripMenuItem();
            повторитьToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            вырезатьToolStripMenuItem = new ToolStripMenuItem();
            копироватьToolStripMenuItem = new ToolStripMenuItem();
            вставитьToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            выбратьвсеToolStripMenuItem = new ToolStripMenuItem();
            инструментыToolStripMenuItem = new ToolStripMenuItem();
            настройкиToolStripMenuItem = new ToolStripMenuItem();
            параметрыToolStripMenuItem = new ToolStripMenuItem();
            справкаToolStripMenuItem = new ToolStripMenuItem();
            содержимоеToolStripMenuItem = new ToolStripMenuItem();
            индексToolStripMenuItem = new ToolStripMenuItem();
            поискToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            опрограммеToolStripMenuItem = new ToolStripMenuItem();
            окноToolStripMenuItem = new ToolStripMenuItem();
            поГоризонталиToolStripMenuItem = new ToolStripMenuItem();
            воВертикалиToolStripMenuItem = new ToolStripMenuItem();
            каскадомToolStripMenuItem = new ToolStripMenuItem();
            упорядочитьСвернутыеToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            statusMainStrip = new StatusStrip();
            timerInterface = new System.Windows.Forms.Timer(components);
            timerSimulation = new System.Windows.Forms.Timer(components);
            panel1 = new Panel();
            tlpTools = new TableLayoutPanel();
            splitContainer1 = new SplitContainer();
            tcTools = new TabControl();
            tpModiles = new TabPage();
            tvModules = new TreeView();
            tpLibrary = new TabPage();
            tvLibrary = new TreeView();
            pgProps = new PropertyGrid();
            pnLeftSize = new Panel();
            toolStripCaption = new ToolStrip();
            tsbHostClose = new ToolStripButton();
            tslPanelCaption = new ToolStripLabel();
            menuMainStrip.SuspendLayout();
            panel1.SuspendLayout();
            tlpTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tcTools.SuspendLayout();
            tpModiles.SuspendLayout();
            tpLibrary.SuspendLayout();
            toolStripCaption.SuspendLayout();
            SuspendLayout();
            // 
            // menuMainStrip
            // 
            menuMainStrip.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem, изменитьToolStripMenuItem, инструментыToolStripMenuItem, справкаToolStripMenuItem, окноToolStripMenuItem });
            menuMainStrip.Location = new Point(0, 25);
            menuMainStrip.MdiWindowListItem = окноToolStripMenuItem;
            menuMainStrip.Name = "menuMainStrip";
            menuMainStrip.Size = new Size(1002, 24);
            menuMainStrip.TabIndex = 1;
            menuMainStrip.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tsmiCreate, tsmiOpen, tsmiAddModule, toolStripSeparator, tsmiSave, tsmiSaveAs, toolStripSeparator1, tsmiPrint, tsmiPreview, toolStripSeparator2, tsmiExit });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new Size(48, 20);
            файлToolStripMenuItem.Text = "&Файл";
            // 
            // tsmiCreate
            // 
            tsmiCreate.Image = (Image)resources.GetObject("tsmiCreate.Image");
            tsmiCreate.ImageTransparentColor = Color.Magenta;
            tsmiCreate.Name = "tsmiCreate";
            tsmiCreate.ShortcutKeys = Keys.Control | Keys.N;
            tsmiCreate.Size = new Size(233, 22);
            tsmiCreate.Text = "&Создать проект";
            tsmiCreate.Click += tsmiCreate_Click;
            // 
            // tsmiOpen
            // 
            tsmiOpen.Image = (Image)resources.GetObject("tsmiOpen.Image");
            tsmiOpen.ImageTransparentColor = Color.Magenta;
            tsmiOpen.Name = "tsmiOpen";
            tsmiOpen.ShortcutKeys = Keys.Control | Keys.O;
            tsmiOpen.Size = new Size(233, 22);
            tsmiOpen.Text = "&Открыть проект";
            tsmiOpen.Click += tsmiOpen_Click;
            // 
            // tsmiAddModule
            // 
            tsmiAddModule.Name = "tsmiAddModule";
            tsmiAddModule.Size = new Size(233, 22);
            tsmiAddModule.Text = "&Добавить модуль";
            tsmiAddModule.Click += tsmiAddModule_Click;
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(230, 6);
            // 
            // tsmiSave
            // 
            tsmiSave.Enabled = false;
            tsmiSave.Image = (Image)resources.GetObject("tsmiSave.Image");
            tsmiSave.ImageTransparentColor = Color.Magenta;
            tsmiSave.Name = "tsmiSave";
            tsmiSave.ShortcutKeys = Keys.Control | Keys.S;
            tsmiSave.Size = new Size(233, 22);
            tsmiSave.Text = "&Сохранить проект";
            tsmiSave.Click += tsmiSave_Click;
            // 
            // tsmiSaveAs
            // 
            tsmiSaveAs.Name = "tsmiSaveAs";
            tsmiSaveAs.Size = new Size(233, 22);
            tsmiSaveAs.Text = "Сохранить проект &как";
            tsmiSaveAs.Click += tsmiSaveAs_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(230, 6);
            // 
            // tsmiPrint
            // 
            tsmiPrint.Image = (Image)resources.GetObject("tsmiPrint.Image");
            tsmiPrint.ImageTransparentColor = Color.Magenta;
            tsmiPrint.Name = "tsmiPrint";
            tsmiPrint.ShortcutKeys = Keys.Control | Keys.P;
            tsmiPrint.Size = new Size(233, 22);
            tsmiPrint.Text = "&Печать";
            // 
            // tsmiPreview
            // 
            tsmiPreview.Image = (Image)resources.GetObject("tsmiPreview.Image");
            tsmiPreview.ImageTransparentColor = Color.Magenta;
            tsmiPreview.Name = "tsmiPreview";
            tsmiPreview.Size = new Size(233, 22);
            tsmiPreview.Text = "Предварительный про&смотр";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(230, 6);
            // 
            // tsmiExit
            // 
            tsmiExit.Name = "tsmiExit";
            tsmiExit.Size = new Size(233, 22);
            tsmiExit.Text = "Вы&ход";
            tsmiExit.Click += выходToolStripMenuItem_Click;
            // 
            // изменитьToolStripMenuItem
            // 
            изменитьToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { отменитьToolStripMenuItem, повторитьToolStripMenuItem, toolStripSeparator3, вырезатьToolStripMenuItem, копироватьToolStripMenuItem, вставитьToolStripMenuItem, toolStripSeparator4, выбратьвсеToolStripMenuItem });
            изменитьToolStripMenuItem.Name = "изменитьToolStripMenuItem";
            изменитьToolStripMenuItem.Size = new Size(73, 20);
            изменитьToolStripMenuItem.Text = "&Изменить";
            // 
            // отменитьToolStripMenuItem
            // 
            отменитьToolStripMenuItem.Name = "отменитьToolStripMenuItem";
            отменитьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            отменитьToolStripMenuItem.Size = new Size(181, 22);
            отменитьToolStripMenuItem.Text = "&Отменить";
            // 
            // повторитьToolStripMenuItem
            // 
            повторитьToolStripMenuItem.Name = "повторитьToolStripMenuItem";
            повторитьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            повторитьToolStripMenuItem.Size = new Size(181, 22);
            повторитьToolStripMenuItem.Text = "&Повторить";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(178, 6);
            // 
            // вырезатьToolStripMenuItem
            // 
            вырезатьToolStripMenuItem.Image = (Image)resources.GetObject("вырезатьToolStripMenuItem.Image");
            вырезатьToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            вырезатьToolStripMenuItem.Name = "вырезатьToolStripMenuItem";
            вырезатьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            вырезатьToolStripMenuItem.Size = new Size(181, 22);
            вырезатьToolStripMenuItem.Text = "В&ырезать";
            // 
            // копироватьToolStripMenuItem
            // 
            копироватьToolStripMenuItem.Image = (Image)resources.GetObject("копироватьToolStripMenuItem.Image");
            копироватьToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            копироватьToolStripMenuItem.Name = "копироватьToolStripMenuItem";
            копироватьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            копироватьToolStripMenuItem.Size = new Size(181, 22);
            копироватьToolStripMenuItem.Text = "&Копировать";
            // 
            // вставитьToolStripMenuItem
            // 
            вставитьToolStripMenuItem.Image = (Image)resources.GetObject("вставитьToolStripMenuItem.Image");
            вставитьToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            вставитьToolStripMenuItem.Name = "вставитьToolStripMenuItem";
            вставитьToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            вставитьToolStripMenuItem.Size = new Size(181, 22);
            вставитьToolStripMenuItem.Text = "&Вставить";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(178, 6);
            // 
            // выбратьвсеToolStripMenuItem
            // 
            выбратьвсеToolStripMenuItem.Name = "выбратьвсеToolStripMenuItem";
            выбратьвсеToolStripMenuItem.Size = new Size(181, 22);
            выбратьвсеToolStripMenuItem.Text = "Выбрать &все";
            // 
            // инструментыToolStripMenuItem
            // 
            инструментыToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { настройкиToolStripMenuItem, параметрыToolStripMenuItem });
            инструментыToolStripMenuItem.Name = "инструментыToolStripMenuItem";
            инструментыToolStripMenuItem.Size = new Size(95, 20);
            инструментыToolStripMenuItem.Text = "&Инструменты";
            // 
            // настройкиToolStripMenuItem
            // 
            настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            настройкиToolStripMenuItem.Size = new Size(138, 22);
            настройкиToolStripMenuItem.Text = "&Настройки";
            // 
            // параметрыToolStripMenuItem
            // 
            параметрыToolStripMenuItem.Name = "параметрыToolStripMenuItem";
            параметрыToolStripMenuItem.Size = new Size(138, 22);
            параметрыToolStripMenuItem.Text = "&Параметры";
            // 
            // справкаToolStripMenuItem
            // 
            справкаToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { содержимоеToolStripMenuItem, индексToolStripMenuItem, поискToolStripMenuItem, toolStripSeparator5, опрограммеToolStripMenuItem });
            справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            справкаToolStripMenuItem.Size = new Size(65, 20);
            справкаToolStripMenuItem.Text = "&Справка";
            // 
            // содержимоеToolStripMenuItem
            // 
            содержимоеToolStripMenuItem.Name = "содержимоеToolStripMenuItem";
            содержимоеToolStripMenuItem.Size = new Size(158, 22);
            содержимоеToolStripMenuItem.Text = "&Содержимое";
            // 
            // индексToolStripMenuItem
            // 
            индексToolStripMenuItem.Name = "индексToolStripMenuItem";
            индексToolStripMenuItem.Size = new Size(158, 22);
            индексToolStripMenuItem.Text = "&Индекс";
            // 
            // поискToolStripMenuItem
            // 
            поискToolStripMenuItem.Name = "поискToolStripMenuItem";
            поискToolStripMenuItem.Size = new Size(158, 22);
            поискToolStripMenuItem.Text = "&Поиск";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(155, 6);
            // 
            // опрограммеToolStripMenuItem
            // 
            опрограммеToolStripMenuItem.Name = "опрограммеToolStripMenuItem";
            опрограммеToolStripMenuItem.Size = new Size(158, 22);
            опрограммеToolStripMenuItem.Text = "&О программе…";
            // 
            // окноToolStripMenuItem
            // 
            окноToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { поГоризонталиToolStripMenuItem, воВертикалиToolStripMenuItem, каскадомToolStripMenuItem, упорядочитьСвернутыеToolStripMenuItem, toolStripMenuItem1 });
            окноToolStripMenuItem.Name = "окноToolStripMenuItem";
            окноToolStripMenuItem.Size = new Size(48, 20);
            окноToolStripMenuItem.Text = "&Окно";
            // 
            // поГоризонталиToolStripMenuItem
            // 
            поГоризонталиToolStripMenuItem.Name = "поГоризонталиToolStripMenuItem";
            поГоризонталиToolStripMenuItem.Size = new Size(207, 22);
            поГоризонталиToolStripMenuItem.Text = "По горизонтали";
            поГоризонталиToolStripMenuItem.Click += поГоризонталиToolStripMenuItem_Click;
            // 
            // воВертикалиToolStripMenuItem
            // 
            воВертикалиToolStripMenuItem.Name = "воВертикалиToolStripMenuItem";
            воВертикалиToolStripMenuItem.Size = new Size(207, 22);
            воВертикалиToolStripMenuItem.Text = "Во вертикали";
            воВертикалиToolStripMenuItem.Click += воВертикалиToolStripMenuItem_Click;
            // 
            // каскадомToolStripMenuItem
            // 
            каскадомToolStripMenuItem.Name = "каскадомToolStripMenuItem";
            каскадомToolStripMenuItem.Size = new Size(207, 22);
            каскадомToolStripMenuItem.Text = "Каскадом";
            каскадомToolStripMenuItem.Click += каскадомToolStripMenuItem_Click;
            // 
            // упорядочитьСвернутыеToolStripMenuItem
            // 
            упорядочитьСвернутыеToolStripMenuItem.Name = "упорядочитьСвернутыеToolStripMenuItem";
            упорядочитьСвернутыеToolStripMenuItem.Size = new Size(207, 22);
            упорядочитьСвернутыеToolStripMenuItem.Text = "Упорядочить свернутые";
            упорядочитьСвернутыеToolStripMenuItem.Click += упорядочитьСвернутыеToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(204, 6);
            // 
            // statusMainStrip
            // 
            statusMainStrip.Location = new Point(0, 505);
            statusMainStrip.Name = "statusMainStrip";
            statusMainStrip.Size = new Size(1002, 22);
            statusMainStrip.SizingGrip = false;
            statusMainStrip.TabIndex = 2;
            statusMainStrip.Text = "statusStrip1";
            // 
            // timerInterface
            // 
            timerInterface.Tick += timerInterface_Tick;
            // 
            // timerSimulation
            // 
            timerSimulation.Tick += timerSimulation_Tick;
            // 
            // panel1
            // 
            panel1.Controls.Add(tlpTools);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 49);
            panel1.Name = "panel1";
            panel1.Size = new Size(209, 456);
            panel1.TabIndex = 4;
            // 
            // tlpTools
            // 
            tlpTools.ColumnCount = 2;
            tlpTools.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTools.ColumnStyles.Add(new ColumnStyle());
            tlpTools.Controls.Add(splitContainer1, 0, 0);
            tlpTools.Controls.Add(pnLeftSize, 1, 0);
            tlpTools.Dock = DockStyle.Fill;
            tlpTools.Location = new Point(0, 0);
            tlpTools.Name = "tlpTools";
            tlpTools.RowCount = 1;
            tlpTools.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTools.Size = new Size(209, 456);
            tlpTools.TabIndex = 3;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tcTools);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pgProps);
            splitContainer1.Size = new Size(199, 450);
            splitContainer1.SplitterDistance = 221;
            splitContainer1.TabIndex = 2;
            // 
            // tcTools
            // 
            tcTools.Controls.Add(tpModiles);
            tcTools.Controls.Add(tpLibrary);
            tcTools.Dock = DockStyle.Fill;
            tcTools.Location = new Point(0, 0);
            tcTools.Name = "tcTools";
            tcTools.SelectedIndex = 0;
            tcTools.Size = new Size(199, 221);
            tcTools.TabIndex = 2;
            // 
            // tpModiles
            // 
            tpModiles.Controls.Add(tvModules);
            tpModiles.Location = new Point(4, 24);
            tpModiles.Name = "tpModiles";
            tpModiles.Padding = new Padding(3);
            tpModiles.Size = new Size(191, 193);
            tpModiles.TabIndex = 1;
            tpModiles.Text = "Модули";
            tpModiles.UseVisualStyleBackColor = true;
            // 
            // tvModules
            // 
            tvModules.BackColor = Color.FromArgb(64, 64, 64);
            tvModules.Dock = DockStyle.Fill;
            tvModules.ForeColor = SystemColors.Window;
            tvModules.FullRowSelect = true;
            tvModules.HideSelection = false;
            tvModules.LineColor = Color.WhiteSmoke;
            tvModules.Location = new Point(3, 3);
            tvModules.Name = "tvModules";
            tvModules.Size = new Size(185, 187);
            tvModules.TabIndex = 2;
            tvModules.DoubleClick += tvModules_DoubleClick;
            // 
            // tpLibrary
            // 
            tpLibrary.Controls.Add(tvLibrary);
            tpLibrary.Location = new Point(4, 24);
            tpLibrary.Name = "tpLibrary";
            tpLibrary.Padding = new Padding(3);
            tpLibrary.Size = new Size(191, 193);
            tpLibrary.TabIndex = 0;
            tpLibrary.Text = "Библиотека";
            tpLibrary.UseVisualStyleBackColor = true;
            // 
            // tvLibrary
            // 
            tvLibrary.BackColor = Color.FromArgb(64, 64, 64);
            tvLibrary.Dock = DockStyle.Fill;
            tvLibrary.ForeColor = SystemColors.Window;
            tvLibrary.FullRowSelect = true;
            tvLibrary.HideSelection = false;
            tvLibrary.LineColor = Color.WhiteSmoke;
            tvLibrary.Location = new Point(3, 3);
            tvLibrary.Name = "tvLibrary";
            treeNode1.Name = "Узел0";
            treeNode1.Text = "Библиотека";
            tvLibrary.Nodes.AddRange(new TreeNode[] { treeNode1 });
            tvLibrary.Size = new Size(185, 187);
            tvLibrary.TabIndex = 1;
            tvLibrary.MouseDown += tvLibrary_MouseDown;
            // 
            // pgProps
            // 
            pgProps.Dock = DockStyle.Fill;
            pgProps.Location = new Point(0, 0);
            pgProps.Name = "pgProps";
            pgProps.Size = new Size(199, 225);
            pgProps.TabIndex = 0;
            pgProps.PropertyValueChanged += pgProps_PropertyValueChanged;
            // 
            // pnLeftSize
            // 
            pnLeftSize.Cursor = Cursors.VSplit;
            pnLeftSize.Dock = DockStyle.Right;
            pnLeftSize.Location = new Point(205, 0);
            pnLeftSize.Margin = new Padding(0);
            pnLeftSize.Name = "pnLeftSize";
            pnLeftSize.Size = new Size(4, 456);
            pnLeftSize.TabIndex = 5;
            pnLeftSize.Paint += pnLeftSize_Paint;
            // 
            // toolStripCaption
            // 
            toolStripCaption.BackColor = SystemColors.ActiveCaption;
            toolStripCaption.GripStyle = ToolStripGripStyle.Hidden;
            toolStripCaption.Items.AddRange(new ToolStripItem[] { tsbHostClose, tslPanelCaption });
            toolStripCaption.Location = new Point(0, 0);
            toolStripCaption.Name = "toolStripCaption";
            toolStripCaption.Size = new Size(1002, 25);
            toolStripCaption.TabIndex = 6;
            toolStripCaption.Text = "toolStrip1";
            // 
            // tsbHostClose
            // 
            tsbHostClose.Alignment = ToolStripItemAlignment.Right;
            tsbHostClose.BackColor = Color.Firebrick;
            tsbHostClose.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbHostClose.Image = Properties.Resources.cancel;
            tsbHostClose.ImageTransparentColor = Color.Magenta;
            tsbHostClose.Name = "tsbHostClose";
            tsbHostClose.Size = new Size(23, 22);
            tsbHostClose.Text = "Закрыть приложение";
            tsbHostClose.Click += tsbHostClose_Click;
            // 
            // tslPanelCaption
            // 
            tslPanelCaption.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tslPanelCaption.ForeColor = SystemColors.ActiveCaptionText;
            tslPanelCaption.Image = Properties.Resources.system;
            tslPanelCaption.Margin = new Padding(3, 1, 0, 2);
            tslPanelCaption.Name = "tslPanelCaption";
            tslPanelCaption.Size = new Size(224, 22);
            tslPanelCaption.Text = "Моделирование работы устройств";
            // 
            // PanelForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1002, 527);
            Controls.Add(panel1);
            Controls.Add(statusMainStrip);
            Controls.Add(menuMainStrip);
            Controls.Add(toolStripCaption);
            FormBorderStyle = FormBorderStyle.None;
            IsMdiContainer = true;
            MainMenuStrip = menuMainStrip;
            Name = "PanelForm";
            StartPosition = FormStartPosition.Manual;
            Text = "Моделирование работы устройств";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            MdiChildActivate += PanelForm_MdiChildActivate;
            Enter += PanelForm_Enter;
            Leave += PanelForm_Leave;
            menuMainStrip.ResumeLayout(false);
            menuMainStrip.PerformLayout();
            panel1.ResumeLayout(false);
            tlpTools.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tcTools.ResumeLayout(false);
            tpModiles.ResumeLayout(false);
            tpLibrary.ResumeLayout(false);
            toolStripCaption.ResumeLayout(false);
            toolStripCaption.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStripCaption;
        private MenuStrip menuMainStrip;
        private StatusStrip statusMainStrip;
        private ToolStripMenuItem файлToolStripMenuItem;
        private ToolStripMenuItem tsmiCreate;
        private ToolStripMenuItem tsmiOpen;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem tsmiSave;
        private ToolStripMenuItem tsmiSaveAs;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiPrint;
        private ToolStripMenuItem tsmiPreview;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem tsmiExit;
        private ToolStripMenuItem изменитьToolStripMenuItem;
        private ToolStripMenuItem отменитьToolStripMenuItem;
        private ToolStripMenuItem повторитьToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem вырезатьToolStripMenuItem;
        private ToolStripMenuItem копироватьToolStripMenuItem;
        private ToolStripMenuItem вставитьToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem выбратьвсеToolStripMenuItem;
        private ToolStripMenuItem инструментыToolStripMenuItem;
        private ToolStripMenuItem настройкиToolStripMenuItem;
        private ToolStripMenuItem параметрыToolStripMenuItem;
        private ToolStripMenuItem справкаToolStripMenuItem;
        private ToolStripMenuItem содержимоеToolStripMenuItem;
        private ToolStripMenuItem индексToolStripMenuItem;
        private ToolStripMenuItem поискToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem опрограммеToolStripMenuItem;
        private System.Windows.Forms.Timer timerInterface;
        private ToolStripMenuItem окноToolStripMenuItem;
        private ToolStripMenuItem поГоризонталиToolStripMenuItem;
        private ToolStripMenuItem воВертикалиToolStripMenuItem;
        private ToolStripMenuItem упорядочитьСвернутыеToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem каскадомToolStripMenuItem;
        private System.Windows.Forms.Timer timerSimulation;
        private Panel panel1;
        private TreeView tvLibrary;
        private SplitContainer splitContainer1;
        private PropertyGrid pgProps;
        private TableLayoutPanel tlpTools;
        private Panel pnLeftSize;
        private ToolStripButton tsbHostClose;
        private ToolStripLabel tslPanelCaption;
        private TabControl tcTools;
        private TabPage tpLibrary;
        private TabPage tpModiles;
        private TreeView tvModules;
        private ToolStripMenuItem tsmiAddModule;
    }
}
