﻿using FractalRenderer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ComplexFractals
{
    public partial class Fractals : Form
    {
        string pluginSubdir;
        Type rendererType;
        AbstractRenderer fractalRenderer;

        int currentTask;

        private Dictionary<string, Dictionary<string, Type>> renderers;

        public Fractals()
        {
            InitializeComponent();

            pluginSubdir = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
        }

        private void Fractals_Shown(object sender, EventArgs e)
        {
            StartFSWatch();
            LoadRenderers();
        }


        private void StartFSWatch()
        {
            if (!Directory.Exists(pluginSubdir))
                Directory.CreateDirectory(pluginSubdir);

            fsWatch.Path = pluginSubdir;
            //fsWatch.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Security | NotifyFilters.Attributes; // Events that may change contents of file (or permissions to read)
            //fsWatch.IncludeSubdirectories = true;
            fsWatch.EnableRaisingEvents = true;
        }

        // TODO What if fswatcher raises events while we reload? 
        private void LoadRenderers()
        {
            renderers = new Dictionary<string, Dictionary<string, Type>>();
            DirectoryInfo di = new DirectoryInfo(pluginSubdir);
            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);

            progressBar1.Value = 0;
            progressBar1.Maximum = 1000 * files.Length;
            int loaded = 0;

            Assembly.Load(typeof(AbstractRenderer).Assembly.FullName);

            foreach (FileInfo fi in files)
            {
                string prefix = fi.DirectoryName.Remove(0, pluginSubdir.Length).TrimStart('/', '\\');
                if (!string.IsNullOrEmpty(prefix))
                    prefix += '/';

                try
                {
                    Assembly a = Assembly.LoadFile(fi.FullName);

                    progressBar1.Value = loaded * 1000 + 250;
                    Application.DoEvents();

                    Type[] types = a.GetTypes();
                    int typesLoaded = 0;
                    foreach (Type t in types)
                    {
                        if (!t.IsAbstract && t.IsSubclassOf(typeof(AbstractRenderer)))
                        {
                            FractalRendererAttribute info = t.GetCustomAttribute<FractalRendererAttribute>(true);
                            if (info != null)
                            {
                                string fractal = info.FractalName;
                                string renderer = prefix + info.RendererName;

                                AddRenderer(fractal, renderer, t);
                            }
                        }
                        typesLoaded++;

                        progressBar1.Value = loaded * 1000 + 250 + (typesLoaded * 750) / types.Length;
                        Application.DoEvents();
                    }
                }
                catch (BadImageFormatException bi)
                {
                    MessageBox.Show("Invalid plugin " + prefix + fi.Name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load plugin " + prefix + fi.Name);
                }
                finally
                {
                    loaded++;
                    progressBar1.Value = loaded * 1000;
                    Application.DoEvents();
                }
            }

            cbFractal.Items.Clear();
            cbFractal.Items.AddRange(renderers.Select(p => p.Key).ToArray());

            SetStatus("Refreshed renderers");
        }

        private void AddRenderer(string fractal, string renderer, Type t)
        {
            if (!renderers.ContainsKey(fractal))
                renderers.Add(fractal, new Dictionary<string, Type>());

            if (renderers[fractal].ContainsKey(renderer)) // Already got a renderer with this name :o
            {
                renderer = string.Format("{0} (conflict: {1})", renderer, t.FullName); // Change name of this to indicate conflig

                Type other = renderers[fractal][renderer];
                string oname = string.Format("{0} (conflict: {1})", renderer, other.FullName); // Change name of other to indicate conflict
                renderers[fractal].Remove(renderer);
                renderers[fractal].Add(oname, other);
            }

            renderers[fractal].Add(renderer, t);
        }


        private void btnRedraw_Click(object sender, EventArgs e)
        {
            RedrawFractal();
        }

        private void fsWatch_Changed(object sender, FileSystemEventArgs e)
        {
            LoadRenderers();
        }

        private void cbFractal_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbRenderer.Items.Clear();
            string key = cbFractal.SelectedItem.ToString();
            if (renderers.ContainsKey(key))
            {
                cbRenderer.Items.AddRange(renderers[key].Select(t => t.Key).ToArray());
            }
        }

        private void cbRenderer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string f = cbFractal.SelectedItem.ToString();
            string r = cbRenderer.SelectedItem.ToString();
            if (renderers.ContainsKey(f))
            {
                if (renderers[f].ContainsKey(r))
                {
                    rendererType = renderers[f][r];
                    ChangeRenderer();
                }
            }
        }

        private void SettingsChanged(SettingsWindow sender, bool redraw)
        {
            if (redraw)
            {
                RedrawPreview();
            }
        }


        private void ChangeRenderer()
        {
            fractalRenderer = Activator.CreateInstance(rendererType) as AbstractRenderer;

            if (gbSettings.Controls.Count > 0)
            {
                foreach (SettingsWindow w in gbSettings.Controls)
                    w.SettingsChanged -= SettingsChanged;
            }

            SettingsWindow settings = fractalRenderer.GetSettingsWindow();
            if (settings != null)
            {
                gbSettings.Controls.Add(settings);
                settings.SettingsChanged += SettingsChanged;
                settings.Dock = DockStyle.Fill;
            }

            RedrawPreview();
        }
        
        private void RedrawPreview()
        {
            if (fractalRenderer == null)
            {
                SetStatus("Renderer not selected yet");
                return;
            }

            SetStatus("Rendering preview");
            Bitmap preview = fractalRenderer.DrawPreview(pbPreview.Size);
            pbPreview.Image = preview;
            SetStatus("Ready");
        }

        private void RedrawFractal()
        {
            if (fractalRenderer == null)
            {
                SetStatus("Renderer not selected yet");
                return;
            }

            if (currentTask != 0)
            {
                SetStatus("Aborting current task");
                fractalRenderer.AbortRender(currentTask);
            }

            currentTask = fractalRenderer.StartRenderAsync(pbFractal.Size, renderComplete, renderProgress, renderAborted);
            if (currentTask != 0)
            {
                SetStatus("Started rendering fractal (task {0})", currentTask);
            }
            else
            {
                SetStatus("Task not started");
            }
        }

        private void renderProgress(int task, object user, float percentage)
        {
            if (currentTask == task)
            {
                Invoke(new Action(() =>
                {
                    progressBar1.Maximum = 1000;
                    progressBar1.Value = (int)(percentage * 1000);
                    SetStatus("Rendering... {0:P1}", percentage);
                }));
            }
        }

        private void renderComplete(int task, int retcode, object user, Bitmap result)
        {
            if (currentTask == task)
            {
                Invoke(new Action(() =>
                {
                    progressBar1.Value = 0;
                    pbFractal.Image = result;
                    pbPreview.Image = result;
                    SetStatus("Completed render");
                }));
            }
        }

        private void renderAborted(int task, int retcode, object user)
        {
            if (currentTask == task)
            {
                currentTask = 0;
                Invoke(new Action(() =>
                {
                    SetStatus("Aborted task");
                }));
            }
        }

        private void SetStatus(string format, params object[] args)
        {
            lblStatus.Text = string.Format(format, args);
        }

        private void Fractals_SizeChanged(object sender, EventArgs e)
        {
            RedrawPreview();
        }
    }
}