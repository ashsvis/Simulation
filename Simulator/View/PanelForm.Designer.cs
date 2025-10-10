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
            инструментыToolStripMenuItem = new ToolStripMenuItem();
            настройкиToolStripMenuItem = new ToolStripMenuItem();
            tsmiScreens = new ToolStripMenuItem();
            tsmiMultiScreensMode = new ToolStripMenuItem();
            tsmiOneScreenMode = new ToolStripMenuItem();
            tsmiLeftPanelVisible = new ToolStripMenuItem();
            tsmiRightPanelVisible = new ToolStripMenuItem();
            параметрыToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            tsmiRun = new ToolStripMenuItem();
            справкаToolStripMenuItem = new ToolStripMenuItem();
            опрограммеToolStripMenuItem = new ToolStripMenuItem();
            tsmiWindow = new ToolStripMenuItem();
            поГоризонталиToolStripMenuItem = new ToolStripMenuItem();
            воВертикалиToolStripMenuItem = new ToolStripMenuItem();
            каскадомToolStripMenuItem = new ToolStripMenuItem();
            упорядочитьСвернутыеToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            statusMainStrip = new StatusStrip();
            timerInterface = new System.Windows.Forms.Timer(components);
            panLeft = new Panel();
            tlpTools = new TableLayoutPanel();
            splitContainer1 = new SplitContainer();
            tcTools = new TabControl();
            tpModules = new TabPage();
            tvModules = new TreeView();
            cmModules = new ContextMenuStrip(components);
            tsmiRenameModule = new ToolStripMenuItem();
            tsmiModuleDublicate = new ToolStripMenuItem();
            tpEquipment = new TabPage();
            tvEquipment = new TreeView();
            cmEquipment = new ContextMenuStrip(components);
            tsmiRenameUnit = new ToolStripMenuItem();
            tsmiUnitDublicate = new ToolStripMenuItem();
            tbField = new TabPage();
            tvField = new TreeView();
            tabControl1 = new TabControl();
            tpProps = new TabPage();
            pgProps = new PropertyGrid();
            tvLibrary = new TreeView();
            panRight = new Panel();
            tlpLibrary = new TableLayoutPanel();
            tabControl3 = new TabControl();
            tabPage2 = new TabPage();
            tsToolLibrary = new ToolStrip();
            tsbAddBock = new ToolStripButton();
            tsbShowBlockForm = new ToolStripButton();
            tsbDeleteBlock = new ToolStripButton();
            toolStripCaption = new ToolStrip();
            tsbHostClose = new ToolStripButton();
            tslPanelCaption = new ToolStripLabel();
            tsbMinimize = new ToolStripButton();
            tsbScreenToRight = new ToolStripButton();
            tslScreenNumber = new ToolStripLabel();
            tsbScreenToLeft = new ToolStripButton();
            toolStripMain = new ToolStrip();
            tsbCreate = new ToolStripButton();
            tsbOpen = new ToolStripButton();
            tsbSave = new ToolStripButton();
            печатьToolStripButton = new ToolStripButton();
            toolStripSeparator7 = new ToolStripSeparator();
            tsbRun = new ToolStripButton();
            toolStripSeparator9 = new ToolStripSeparator();
            tsbAddModule = new ToolStripButton();
            tsbShowModuleForm = new ToolStripButton();
            tsbDeleteModule = new ToolStripButton();
            toolStripSeparator8 = new ToolStripSeparator();
            tsbCut = new ToolStripButton();
            tsbCopy = new ToolStripButton();
            tsbPaste = new ToolStripButton();
            splitterLeft = new Splitter();
            splitterRight = new Splitter();
            menuMainStrip.SuspendLayout();
            panLeft.SuspendLayout();
            tlpTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tcTools.SuspendLayout();
            tpModules.SuspendLayout();
            cmModules.SuspendLayout();
            tpEquipment.SuspendLayout();
            cmEquipment.SuspendLayout();
            tbField.SuspendLayout();
            tabControl1.SuspendLayout();
            tpProps.SuspendLayout();
            panRight.SuspendLayout();
            tlpLibrary.SuspendLayout();
            tabControl3.SuspendLayout();
            tabPage2.SuspendLayout();
            tsToolLibrary.SuspendLayout();
            toolStripCaption.SuspendLayout();
            toolStripMain.SuspendLayout();
            SuspendLayout();
            // 
            // menuMainStrip
            // 
            menuMainStrip.GripMargin = new Padding(0);
            menuMainStrip.Items.AddRange(new ToolStripItem[] { файлToolStripMenuItem, инструментыToolStripMenuItem, справкаToolStripMenuItem, tsmiWindow });
            menuMainStrip.Location = new Point(0, 25);
            menuMainStrip.MdiWindowListItem = tsmiWindow;
            menuMainStrip.Name = "menuMainStrip";
            menuMainStrip.Padding = new Padding(0);
            menuMainStrip.RenderMode = ToolStripRenderMode.System;
            menuMainStrip.Size = new Size(1002, 24);
            menuMainStrip.TabIndex = 1;
            menuMainStrip.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tsmiCreate, tsmiOpen, tsmiAddModule, toolStripSeparator, tsmiSave, tsmiSaveAs, toolStripSeparator1, tsmiPrint, tsmiPreview, toolStripSeparator2, tsmiExit });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new Size(48, 24);
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
            // инструментыToolStripMenuItem
            // 
            инструментыToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { настройкиToolStripMenuItem, параметрыToolStripMenuItem, toolStripMenuItem2, tsmiRun });
            инструментыToolStripMenuItem.Name = "инструментыToolStripMenuItem";
            инструментыToolStripMenuItem.Size = new Size(95, 24);
            инструментыToolStripMenuItem.Text = "&Инструменты";
            // 
            // настройкиToolStripMenuItem
            // 
            настройкиToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tsmiScreens, tsmiLeftPanelVisible, tsmiRightPanelVisible });
            настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            настройкиToolStripMenuItem.Size = new Size(138, 22);
            настройкиToolStripMenuItem.Text = "&Настройки";
            // 
            // tsmiScreens
            // 
            tsmiScreens.DropDownItems.AddRange(new ToolStripItem[] { tsmiMultiScreensMode, tsmiOneScreenMode });
            tsmiScreens.Name = "tsmiScreens";
            tsmiScreens.Size = new Size(156, 22);
            tsmiScreens.Text = "Экраны";
            // 
            // tsmiMultiScreensMode
            // 
            tsmiMultiScreensMode.Image = Properties.Resources.multiscreens;
            tsmiMultiScreensMode.Name = "tsmiMultiScreensMode";
            tsmiMultiScreensMode.Size = new Size(220, 22);
            tsmiMultiScreensMode.Text = "В мультиэкранный режим";
            tsmiMultiScreensMode.Click += tsmiMultiScreensMode_Click;
            // 
            // tsmiOneScreenMode
            // 
            tsmiOneScreenMode.Image = Properties.Resources.monoscreen;
            tsmiOneScreenMode.Name = "tsmiOneScreenMode";
            tsmiOneScreenMode.Size = new Size(220, 22);
            tsmiOneScreenMode.Text = "В одноэкранный режим";
            tsmiOneScreenMode.Click += tsmiOneScreenMode_Click;
            // 
            // tsmiLeftPanelVisible
            // 
            tsmiLeftPanelVisible.Checked = true;
            tsmiLeftPanelVisible.CheckState = CheckState.Checked;
            tsmiLeftPanelVisible.Name = "tsmiLeftPanelVisible";
            tsmiLeftPanelVisible.Size = new Size(156, 22);
            tsmiLeftPanelVisible.Text = " Левая панель";
            tsmiLeftPanelVisible.Click += tsmiLeftPanelVisible_Click;
            // 
            // tsmiRightPanelVisible
            // 
            tsmiRightPanelVisible.Checked = true;
            tsmiRightPanelVisible.CheckState = CheckState.Checked;
            tsmiRightPanelVisible.Name = "tsmiRightPanelVisible";
            tsmiRightPanelVisible.Size = new Size(156, 22);
            tsmiRightPanelVisible.Text = "Правая панель";
            tsmiRightPanelVisible.Click += tsmiRightPanelVisible_Click;
            // 
            // параметрыToolStripMenuItem
            // 
            параметрыToolStripMenuItem.Name = "параметрыToolStripMenuItem";
            параметрыToolStripMenuItem.Size = new Size(138, 22);
            параметрыToolStripMenuItem.Text = "&Параметры";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(135, 6);
            // 
            // tsmiRun
            // 
            tsmiRun.Name = "tsmiRun";
            tsmiRun.Size = new Size(138, 22);
            tsmiRun.Text = "Симуляция";
            tsmiRun.Click += tsbRun_Click;
            // 
            // справкаToolStripMenuItem
            // 
            справкаToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { опрограммеToolStripMenuItem });
            справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            справкаToolStripMenuItem.Size = new Size(65, 24);
            справкаToolStripMenuItem.Text = "&Справка";
            // 
            // опрограммеToolStripMenuItem
            // 
            опрограммеToolStripMenuItem.Name = "опрограммеToolStripMenuItem";
            опрограммеToolStripMenuItem.Size = new Size(158, 22);
            опрограммеToolStripMenuItem.Text = "&О программе…";
            // 
            // tsmiWindow
            // 
            tsmiWindow.DropDownItems.AddRange(new ToolStripItem[] { поГоризонталиToolStripMenuItem, воВертикалиToolStripMenuItem, каскадомToolStripMenuItem, упорядочитьСвернутыеToolStripMenuItem, toolStripMenuItem1 });
            tsmiWindow.Name = "tsmiWindow";
            tsmiWindow.Size = new Size(48, 24);
            tsmiWindow.Text = "&Окно";
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
            // panLeft
            // 
            panLeft.Controls.Add(tlpTools);
            panLeft.Dock = DockStyle.Left;
            panLeft.Location = new Point(0, 74);
            panLeft.Margin = new Padding(0);
            panLeft.Name = "panLeft";
            panLeft.Size = new Size(210, 431);
            panLeft.TabIndex = 4;
            // 
            // tlpTools
            // 
            tlpTools.ColumnCount = 2;
            tlpTools.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTools.ColumnStyles.Add(new ColumnStyle());
            tlpTools.Controls.Add(splitContainer1, 0, 1);
            tlpTools.Dock = DockStyle.Fill;
            tlpTools.Location = new Point(0, 0);
            tlpTools.Name = "tlpTools";
            tlpTools.RowCount = 2;
            tlpTools.RowStyles.Add(new RowStyle());
            tlpTools.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTools.Size = new Size(210, 431);
            tlpTools.TabIndex = 3;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 3);
            splitContainer1.Margin = new Padding(3, 3, 0, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tcTools);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl1);
            splitContainer1.Size = new Size(207, 425);
            splitContainer1.SplitterDistance = 207;
            splitContainer1.TabIndex = 2;
            // 
            // tcTools
            // 
            tcTools.Controls.Add(tpModules);
            tcTools.Controls.Add(tpEquipment);
            tcTools.Controls.Add(tbField);
            tcTools.Dock = DockStyle.Fill;
            tcTools.Location = new Point(0, 0);
            tcTools.Margin = new Padding(0);
            tcTools.Name = "tcTools";
            tcTools.Padding = new Point(0, 0);
            tcTools.SelectedIndex = 0;
            tcTools.Size = new Size(207, 207);
            tcTools.TabIndex = 2;
            // 
            // tpModules
            // 
            tpModules.Controls.Add(tvModules);
            tpModules.Location = new Point(4, 24);
            tpModules.Name = "tpModules";
            tpModules.Padding = new Padding(3);
            tpModules.Size = new Size(199, 179);
            tpModules.TabIndex = 1;
            tpModules.Text = "Задачи";
            tpModules.UseVisualStyleBackColor = true;
            // 
            // tvModules
            // 
            tvModules.ContextMenuStrip = cmModules;
            tvModules.Dock = DockStyle.Fill;
            tvModules.FullRowSelect = true;
            tvModules.HideSelection = false;
            tvModules.Location = new Point(3, 3);
            tvModules.Margin = new Padding(0);
            tvModules.Name = "tvModules";
            tvModules.Size = new Size(193, 173);
            tvModules.TabIndex = 2;
            tvModules.MouseDown += tvModules_MouseDown;
            // 
            // cmModules
            // 
            cmModules.Items.AddRange(new ToolStripItem[] { tsmiRenameModule, tsmiModuleDublicate });
            cmModules.Name = "cmModules";
            cmModules.Size = new Size(171, 48);
            cmModules.Opening += cmModules_Opening;
            // 
            // tsmiRenameModule
            // 
            tsmiRenameModule.Name = "tsmiRenameModule";
            tsmiRenameModule.Size = new Size(170, 22);
            tsmiRenameModule.Text = "Переименовать...";
            tsmiRenameModule.Click += tsmiRenameModule_Click;
            // 
            // tsmiModuleDublicate
            // 
            tsmiModuleDublicate.Name = "tsmiModuleDublicate";
            tsmiModuleDublicate.Size = new Size(170, 22);
            tsmiModuleDublicate.Text = "Дублировать";
            tsmiModuleDublicate.Click += tsmiModuleDublicate_Click;
            // 
            // tpEquipment
            // 
            tpEquipment.Controls.Add(tvEquipment);
            tpEquipment.Location = new Point(4, 24);
            tpEquipment.Name = "tpEquipment";
            tpEquipment.Padding = new Padding(3);
            tpEquipment.Size = new Size(199, 179);
            tpEquipment.TabIndex = 2;
            tpEquipment.Text = "Железо";
            tpEquipment.UseVisualStyleBackColor = true;
            // 
            // tvEquipment
            // 
            tvEquipment.ContextMenuStrip = cmEquipment;
            tvEquipment.Dock = DockStyle.Fill;
            tvEquipment.FullRowSelect = true;
            tvEquipment.HideSelection = false;
            tvEquipment.Location = new Point(3, 3);
            tvEquipment.Margin = new Padding(0);
            tvEquipment.Name = "tvEquipment";
            tvEquipment.Size = new Size(193, 173);
            tvEquipment.TabIndex = 3;
            tvEquipment.MouseDown += tvEquipment_MouseDown;
            // 
            // cmEquipment
            // 
            cmEquipment.Items.AddRange(new ToolStripItem[] { tsmiRenameUnit, tsmiUnitDublicate });
            cmEquipment.Name = "cmModules";
            cmEquipment.Size = new Size(171, 48);
            cmEquipment.Opening += cmEquipment_Opening;
            // 
            // tsmiRenameUnit
            // 
            tsmiRenameUnit.Name = "tsmiRenameUnit";
            tsmiRenameUnit.Size = new Size(170, 22);
            tsmiRenameUnit.Text = "Переименовать...";
            tsmiRenameUnit.Click += tsmiRenameUnit_Click;
            // 
            // tsmiUnitDublicate
            // 
            tsmiUnitDublicate.Name = "tsmiUnitDublicate";
            tsmiUnitDublicate.Size = new Size(170, 22);
            tsmiUnitDublicate.Text = "Дублировать";
            tsmiUnitDublicate.Click += tsmiUnitDublicate_Click;
            // 
            // tbField
            // 
            tbField.Controls.Add(tvField);
            tbField.Location = new Point(4, 24);
            tbField.Name = "tbField";
            tbField.Padding = new Padding(3);
            tbField.Size = new Size(199, 179);
            tbField.TabIndex = 3;
            tbField.Text = "Поле";
            tbField.UseVisualStyleBackColor = true;
            // 
            // tvField
            // 
            tvField.ContextMenuStrip = cmEquipment;
            tvField.Dock = DockStyle.Fill;
            tvField.FullRowSelect = true;
            tvField.HideSelection = false;
            tvField.Location = new Point(3, 3);
            tvField.Margin = new Padding(0);
            tvField.Name = "tvField";
            tvField.Size = new Size(193, 173);
            tvField.TabIndex = 4;
            tvField.MouseDown += tvField_MouseDown;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tpProps);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(0);
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new Point(0, 0);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(207, 214);
            tabControl1.TabIndex = 1;
            // 
            // tpProps
            // 
            tpProps.Controls.Add(pgProps);
            tpProps.Location = new Point(4, 24);
            tpProps.Name = "tpProps";
            tpProps.Padding = new Padding(3);
            tpProps.Size = new Size(199, 186);
            tpProps.TabIndex = 1;
            tpProps.Text = "Свойства";
            tpProps.UseVisualStyleBackColor = true;
            // 
            // pgProps
            // 
            pgProps.DisabledItemForeColor = Color.FromArgb(127, 255, 255, 255);
            pgProps.Dock = DockStyle.Fill;
            pgProps.Location = new Point(3, 3);
            pgProps.Margin = new Padding(3, 3, 0, 3);
            pgProps.Name = "pgProps";
            pgProps.PropertySort = PropertySort.Categorized;
            pgProps.Size = new Size(193, 180);
            pgProps.TabIndex = 0;
            pgProps.UseCompatibleTextRendering = true;
            pgProps.PropertyValueChanged += pgProps_PropertyValueChanged;
            // 
            // tvLibrary
            // 
            tvLibrary.Dock = DockStyle.Fill;
            tvLibrary.FullRowSelect = true;
            tvLibrary.HideSelection = false;
            tvLibrary.Location = new Point(3, 3);
            tvLibrary.Margin = new Padding(0);
            tvLibrary.Name = "tvLibrary";
            treeNode1.Name = "Узел0";
            treeNode1.Text = "Библиотека";
            tvLibrary.Nodes.AddRange(new TreeNode[] { treeNode1 });
            tvLibrary.Size = new Size(195, 397);
            tvLibrary.TabIndex = 1;
            tvLibrary.AfterSelect += tvLibrary_AfterSelect;
            tvLibrary.MouseDown += tvLibrary_MouseDown;
            // 
            // panRight
            // 
            panRight.Controls.Add(tlpLibrary);
            panRight.Dock = DockStyle.Right;
            panRight.Location = new Point(793, 74);
            panRight.Margin = new Padding(0);
            panRight.Name = "panRight";
            panRight.Size = new Size(209, 431);
            panRight.TabIndex = 8;
            // 
            // tlpLibrary
            // 
            tlpLibrary.ColumnCount = 2;
            tlpLibrary.ColumnStyles.Add(new ColumnStyle());
            tlpLibrary.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLibrary.Controls.Add(tabControl3, 1, 0);
            tlpLibrary.Dock = DockStyle.Fill;
            tlpLibrary.Location = new Point(0, 0);
            tlpLibrary.Name = "tlpLibrary";
            tlpLibrary.RowCount = 1;
            tlpLibrary.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpLibrary.Size = new Size(209, 431);
            tlpLibrary.TabIndex = 4;
            // 
            // tabControl3
            // 
            tabControl3.Controls.Add(tabPage2);
            tabControl3.Dock = DockStyle.Fill;
            tabControl3.Location = new Point(0, 0);
            tabControl3.Margin = new Padding(0);
            tabControl3.Name = "tabControl3";
            tabControl3.Padding = new Point(0, 0);
            tabControl3.SelectedIndex = 0;
            tabControl3.Size = new Size(209, 431);
            tabControl3.TabIndex = 1;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tvLibrary);
            tabPage2.Controls.Add(tsToolLibrary);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(201, 403);
            tabPage2.TabIndex = 0;
            tabPage2.Text = "Библиотека";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tsToolLibrary
            // 
            tsToolLibrary.GripStyle = ToolStripGripStyle.Hidden;
            tsToolLibrary.Items.AddRange(new ToolStripItem[] { tsbAddBock, tsbShowBlockForm, tsbDeleteBlock });
            tsToolLibrary.Location = new Point(3, 3);
            tsToolLibrary.Name = "tsToolLibrary";
            tsToolLibrary.Size = new Size(188, 25);
            tsToolLibrary.TabIndex = 2;
            tsToolLibrary.Text = "toolStrip1";
            tsToolLibrary.Visible = false;
            // 
            // tsbAddBock
            // 
            tsbAddBock.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddBock.Image = Properties.Resources.newitem;
            tsbAddBock.ImageTransparentColor = Color.Magenta;
            tsbAddBock.Name = "tsbAddBock";
            tsbAddBock.Size = new Size(23, 22);
            tsbAddBock.Text = "Добавить новый блок";
            tsbAddBock.Click += tsbAddBock_Click;
            // 
            // tsbShowBlockForm
            // 
            tsbShowBlockForm.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbShowBlockForm.Enabled = false;
            tsbShowBlockForm.Image = Properties.Resources.showform;
            tsbShowBlockForm.ImageTransparentColor = Color.Magenta;
            tsbShowBlockForm.Name = "tsbShowBlockForm";
            tsbShowBlockForm.Size = new Size(23, 22);
            tsbShowBlockForm.Text = "Показать содержимое блока";
            tsbShowBlockForm.Click += tsbShowBlockForm_Click;
            // 
            // tsbDeleteBlock
            // 
            tsbDeleteBlock.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDeleteBlock.Enabled = false;
            tsbDeleteBlock.Image = Properties.Resources.delitem;
            tsbDeleteBlock.ImageTransparentColor = Color.Magenta;
            tsbDeleteBlock.Name = "tsbDeleteBlock";
            tsbDeleteBlock.Size = new Size(23, 22);
            tsbDeleteBlock.Text = "Удалить выбранный блок";
            tsbDeleteBlock.Click += tsbDeleteBlock_Click;
            // 
            // toolStripCaption
            // 
            toolStripCaption.BackColor = SystemColors.ActiveCaption;
            toolStripCaption.GripMargin = new Padding(0);
            toolStripCaption.GripStyle = ToolStripGripStyle.Hidden;
            toolStripCaption.Items.AddRange(new ToolStripItem[] { tsbHostClose, tslPanelCaption, tsbMinimize, tsbScreenToRight, tslScreenNumber, tsbScreenToLeft });
            toolStripCaption.Location = new Point(0, 0);
            toolStripCaption.Name = "toolStripCaption";
            toolStripCaption.Padding = new Padding(0);
            toolStripCaption.RenderMode = ToolStripRenderMode.System;
            toolStripCaption.Size = new Size(1002, 25);
            toolStripCaption.TabIndex = 6;
            toolStripCaption.Text = "toolStrip1";
            // 
            // tsbHostClose
            // 
            tsbHostClose.Alignment = ToolStripItemAlignment.Right;
            tsbHostClose.BackColor = Color.Transparent;
            tsbHostClose.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbHostClose.Image = Properties.Resources.close;
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
            tslPanelCaption.Image = Properties.Resources.modeling;
            tslPanelCaption.Margin = new Padding(3, 1, 0, 2);
            tslPanelCaption.Name = "tslPanelCaption";
            tslPanelCaption.Size = new Size(224, 22);
            tslPanelCaption.Text = "Моделирование работы устройств";
            // 
            // tsbMinimize
            // 
            tsbMinimize.Alignment = ToolStripItemAlignment.Right;
            tsbMinimize.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbMinimize.Image = Properties.Resources.minimize;
            tsbMinimize.ImageTransparentColor = Color.Magenta;
            tsbMinimize.Name = "tsbMinimize";
            tsbMinimize.Size = new Size(23, 22);
            tsbMinimize.Text = "Свернуть";
            tsbMinimize.ToolTipText = "Свернуть окна приложения";
            tsbMinimize.Click += tsbMinimize_Click;
            // 
            // tsbScreenToRight
            // 
            tsbScreenToRight.Alignment = ToolStripItemAlignment.Right;
            tsbScreenToRight.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbScreenToRight.Image = Properties.Resources.greenright;
            tsbScreenToRight.ImageTransparentColor = Color.Magenta;
            tsbScreenToRight.Name = "tsbScreenToRight";
            tsbScreenToRight.Size = new Size(23, 22);
            tsbScreenToRight.Text = "Переместить правее";
            tsbScreenToRight.ToolTipText = "Переместить вправо";
            tsbScreenToRight.Click += tsmiScreenMoveRight_Click;
            // 
            // tslScreenNumber
            // 
            tslScreenNumber.Alignment = ToolStripItemAlignment.Right;
            tslScreenNumber.Font = new Font("Consolas", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            tslScreenNumber.Name = "tslScreenNumber";
            tslScreenNumber.Size = new Size(14, 22);
            tslScreenNumber.Text = "1";
            // 
            // tsbScreenToLeft
            // 
            tsbScreenToLeft.Alignment = ToolStripItemAlignment.Right;
            tsbScreenToLeft.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbScreenToLeft.Image = Properties.Resources.greenleft;
            tsbScreenToLeft.ImageTransparentColor = Color.Magenta;
            tsbScreenToLeft.Name = "tsbScreenToLeft";
            tsbScreenToLeft.Size = new Size(23, 22);
            tsbScreenToLeft.Text = "Переместить левее";
            tsbScreenToLeft.ToolTipText = "Переместить влево";
            tsbScreenToLeft.Click += tsmiScreenMoveLeft_Click;
            // 
            // toolStripMain
            // 
            toolStripMain.GripStyle = ToolStripGripStyle.Hidden;
            toolStripMain.Items.AddRange(new ToolStripItem[] { tsbCreate, tsbOpen, tsbSave, печатьToolStripButton, toolStripSeparator7, tsbRun, toolStripSeparator9, tsbAddModule, tsbShowModuleForm, tsbDeleteModule, toolStripSeparator8, tsbCut, tsbCopy, tsbPaste });
            toolStripMain.Location = new Point(0, 49);
            toolStripMain.Name = "toolStripMain";
            toolStripMain.RenderMode = ToolStripRenderMode.System;
            toolStripMain.Size = new Size(1002, 25);
            toolStripMain.TabIndex = 10;
            toolStripMain.Text = "toolStrip1";
            // 
            // tsbCreate
            // 
            tsbCreate.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCreate.Image = (Image)resources.GetObject("tsbCreate.Image");
            tsbCreate.ImageTransparentColor = Color.Magenta;
            tsbCreate.Name = "tsbCreate";
            tsbCreate.Size = new Size(23, 22);
            tsbCreate.Text = "&Создать";
            tsbCreate.Click += tsmiCreate_Click;
            // 
            // tsbOpen
            // 
            tsbOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbOpen.Image = (Image)resources.GetObject("tsbOpen.Image");
            tsbOpen.ImageTransparentColor = Color.Magenta;
            tsbOpen.Name = "tsbOpen";
            tsbOpen.Size = new Size(23, 22);
            tsbOpen.Text = "&Открыть";
            tsbOpen.Click += tsmiOpen_Click;
            // 
            // tsbSave
            // 
            tsbSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSave.Enabled = false;
            tsbSave.Image = (Image)resources.GetObject("tsbSave.Image");
            tsbSave.ImageTransparentColor = Color.Magenta;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new Size(23, 22);
            tsbSave.Text = "&Сохранить";
            tsbSave.Click += tsmiSave_Click;
            // 
            // печатьToolStripButton
            // 
            печатьToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            печатьToolStripButton.Enabled = false;
            печатьToolStripButton.Image = (Image)resources.GetObject("печатьToolStripButton.Image");
            печатьToolStripButton.ImageTransparentColor = Color.Magenta;
            печатьToolStripButton.Name = "печатьToolStripButton";
            печатьToolStripButton.Size = new Size(23, 22);
            печатьToolStripButton.Text = "&Печать";
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new Size(6, 25);
            // 
            // tsbRun
            // 
            tsbRun.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRun.Image = Properties.Resources.run;
            tsbRun.ImageTransparentColor = Color.Magenta;
            tsbRun.Name = "tsbRun";
            tsbRun.Size = new Size(23, 22);
            tsbRun.Text = "Пуск";
            tsbRun.Click += tsbRun_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new Size(6, 25);
            // 
            // tsbAddModule
            // 
            tsbAddModule.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddModule.Image = Properties.Resources.newitem;
            tsbAddModule.ImageTransparentColor = Color.Magenta;
            tsbAddModule.Name = "tsbAddModule";
            tsbAddModule.Size = new Size(23, 22);
            tsbAddModule.Text = "Добавить новый модуль";
            tsbAddModule.Click += tsmiAddModule_Click;
            // 
            // tsbShowModuleForm
            // 
            tsbShowModuleForm.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbShowModuleForm.Enabled = false;
            tsbShowModuleForm.Image = Properties.Resources.showform;
            tsbShowModuleForm.ImageTransparentColor = Color.Magenta;
            tsbShowModuleForm.Name = "tsbShowModuleForm";
            tsbShowModuleForm.Size = new Size(23, 22);
            tsbShowModuleForm.Text = "Показать содержимое модуля";
            tsbShowModuleForm.Click += tsbShowModuleForm_Click;
            // 
            // tsbDeleteModule
            // 
            tsbDeleteModule.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDeleteModule.Enabled = false;
            tsbDeleteModule.Image = Properties.Resources.delitem;
            tsbDeleteModule.ImageTransparentColor = Color.Magenta;
            tsbDeleteModule.Name = "tsbDeleteModule";
            tsbDeleteModule.Size = new Size(23, 22);
            tsbDeleteModule.Text = "Удалить выбранный модуль";
            tsbDeleteModule.Click += tsbDeleteModule_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new Size(6, 25);
            // 
            // tsbCut
            // 
            tsbCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCut.Enabled = false;
            tsbCut.Image = (Image)resources.GetObject("tsbCut.Image");
            tsbCut.ImageTransparentColor = Color.Magenta;
            tsbCut.Name = "tsbCut";
            tsbCut.Size = new Size(23, 22);
            tsbCut.Text = "Вы&резать";
            tsbCut.Click += tsbCut_Click;
            // 
            // tsbCopy
            // 
            tsbCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbCopy.Enabled = false;
            tsbCopy.Image = (Image)resources.GetObject("tsbCopy.Image");
            tsbCopy.ImageTransparentColor = Color.Magenta;
            tsbCopy.Name = "tsbCopy";
            tsbCopy.Size = new Size(23, 22);
            tsbCopy.Text = "&Копировать";
            tsbCopy.Click += tsbCopy_Click;
            // 
            // tsbPaste
            // 
            tsbPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbPaste.Enabled = false;
            tsbPaste.Image = (Image)resources.GetObject("tsbPaste.Image");
            tsbPaste.ImageTransparentColor = Color.Magenta;
            tsbPaste.Name = "tsbPaste";
            tsbPaste.Size = new Size(23, 22);
            tsbPaste.Text = "&Вставить";
            tsbPaste.Click += tsbPaste_Click;
            // 
            // splitterLeft
            // 
            splitterLeft.Cursor = Cursors.VSplit;
            splitterLeft.Location = new Point(210, 74);
            splitterLeft.MinSize = 0;
            splitterLeft.Name = "splitterLeft";
            splitterLeft.Size = new Size(3, 431);
            splitterLeft.TabIndex = 12;
            splitterLeft.TabStop = false;
            splitterLeft.SplitterMoved += splitterLeft_SplitterMoved;
            // 
            // splitterRight
            // 
            splitterRight.Cursor = Cursors.VSplit;
            splitterRight.Dock = DockStyle.Right;
            splitterRight.Location = new Point(790, 74);
            splitterRight.MinSize = 0;
            splitterRight.Name = "splitterRight";
            splitterRight.Size = new Size(3, 431);
            splitterRight.TabIndex = 13;
            splitterRight.TabStop = false;
            splitterRight.SplitterMoved += splitterRight_SplitterMoved;
            // 
            // PanelForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1002, 527);
            Controls.Add(splitterRight);
            Controls.Add(splitterLeft);
            Controls.Add(panRight);
            Controls.Add(panLeft);
            Controls.Add(statusMainStrip);
            Controls.Add(toolStripMain);
            Controls.Add(menuMainStrip);
            Controls.Add(toolStripCaption);
            FormBorderStyle = FormBorderStyle.None;
            IsMdiContainer = true;
            MainMenuStrip = menuMainStrip;
            Name = "PanelForm";
            StartPosition = FormStartPosition.Manual;
            Text = "Моделирование работы устройств";
            FormClosing += PanelForm_FormClosing;
            Load += PanelForm_Load;
            MdiChildActivate += PanelForm_MdiChildActivate;
            VisibleChanged += PanelForm_VisibleChanged;
            Enter += PanelForm_Enter;
            Leave += PanelForm_Leave;
            menuMainStrip.ResumeLayout(false);
            menuMainStrip.PerformLayout();
            panLeft.ResumeLayout(false);
            tlpTools.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tcTools.ResumeLayout(false);
            tpModules.ResumeLayout(false);
            cmModules.ResumeLayout(false);
            tpEquipment.ResumeLayout(false);
            cmEquipment.ResumeLayout(false);
            tbField.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tpProps.ResumeLayout(false);
            panRight.ResumeLayout(false);
            tlpLibrary.ResumeLayout(false);
            tabControl3.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tsToolLibrary.ResumeLayout(false);
            tsToolLibrary.PerformLayout();
            toolStripCaption.ResumeLayout(false);
            toolStripCaption.PerformLayout();
            toolStripMain.ResumeLayout(false);
            toolStripMain.PerformLayout();
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
        private ToolStripMenuItem инструментыToolStripMenuItem;
        private ToolStripMenuItem настройкиToolStripMenuItem;
        private ToolStripMenuItem параметрыToolStripMenuItem;
        private ToolStripMenuItem справкаToolStripMenuItem;
        private ToolStripMenuItem опрограммеToolStripMenuItem;
        private System.Windows.Forms.Timer timerInterface;
        private ToolStripMenuItem tsmiWindow;
        private ToolStripMenuItem поГоризонталиToolStripMenuItem;
        private ToolStripMenuItem воВертикалиToolStripMenuItem;
        private ToolStripMenuItem упорядочитьСвернутыеToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem каскадомToolStripMenuItem;
        private Panel panLeft;
        private Panel panRight;
        private TreeView tvLibrary;
        private SplitContainer splitContainer1;
        private PropertyGrid pgProps;
        private TableLayoutPanel tlpTools;
        private ToolStripButton tsbHostClose;
        private ToolStripLabel tslPanelCaption;
        private ToolStripMenuItem tsmiAddModule;
        private TabControl tabControl1;
        private TabPage tpProps;
        private TabControl tcTools;
        private TabPage tpModules;
        private TreeView tvModules;
        private ToolStripButton tsbMinimize;
        private ToolStripButton tsbScreenToRight;
        private ToolStripButton tsbScreenToLeft;
        private ToolStripMenuItem tsmiScreens;
        private ToolStripMenuItem tsmiMultiScreensMode;
        private ToolStripMenuItem tsmiOneScreenMode;
        private ToolStripLabel tslScreenNumber;
        private TableLayoutPanel tlpLibrary;
        private TabControl tabControl3;
        private TabPage tabPage2;
        private ToolStrip tsToolLibrary;
        private ToolStripButton tsbAddBock;
        private ToolStripButton tsbShowBlockForm;
        private ToolStripButton tsbDeleteBlock;
        private ContextMenuStrip cmModules;
        private ToolStripMenuItem tsmiModuleDublicate;
        private ToolStripMenuItem tsmiRenameModule;
        private TabPage tpEquipment;
        private ToolStripButton tsbAddModule;
        private ToolStripButton tsbShowModuleForm;
        private ToolStripButton tsbDeleteModule;
        private TreeView tvEquipment;
        private ContextMenuStrip cmEquipment;
        private ToolStripMenuItem tsmiRenameUnit;
        private ToolStripMenuItem tsmiUnitDublicate;
        private ToolStripButton tsbRun;
        private ToolStripMenuItem tsmiLeftPanelVisible;
        private ToolStripMenuItem tsmiRightPanelVisible;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem tsmiRun;
        private ToolStrip toolStripMain;
        private ToolStripButton tsbCreate;
        private ToolStripButton tsbOpen;
        private ToolStripButton tsbSave;
        private ToolStripButton печатьToolStripButton;
        private ToolStripButton tsbCut;
        private ToolStripButton tsbCopy;
        private ToolStripButton tsbPaste;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripSeparator toolStripSeparator8;
        private TabPage tbField;
        private TreeView tvField;
        private Splitter splitterLeft;
        private Splitter splitterRight;
    }
}
