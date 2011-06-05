﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JabboServer
{
    public partial class Interface : Form
    {
        private bool IsStarted = false;

        private delegate void textLogger(string Text); // <<

        internal Interface()
        {
            InitializeComponent();
        }

        private void Interface_Load(object sender, EventArgs e)
        {
            this.Show();
        }

        private void Write(string Text)
        {
            if (Text != null)
            {
                this.logBox.AppendText(Text);
            }

            this.logBox.AppendText(Environment.NewLine);

            this.logBox.SelectionStart = this.logBox.Text.Length;
            this.logBox.ScrollToCaret();
            this.logBox.Refresh();
        }

        internal void WriteLine(string Text)
        {
            if (Visible)
            {
                textLogger Log = new textLogger(Write);
                this.Invoke(Log, new object[] { Text });
            }
        }

        public void Clear()
        {
            this.logBox.Clear();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (!IsStarted)
            {
                IsStarted = true;

                Engine.Initialize(this);

                startButton.Text = "Exit";
            }
            else
            {
                Engine.Dispose();
            }
        }

        private void informationButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show
                (
                "Server for the JavaScript Habbo Environment\n" +
                "Platform: C#.NET 4.0\n" +
                "Written by PEjump2, wichard & joopie\n" +
                "Copyright (C) 2011\n\n" +
                "",
                "About JabboServer",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
        }

        private void hotelalertButton_Click(object sender, EventArgs e)
        {
            if (IsStarted)
            {
                HotelalertBox HotelalertBox = new HotelalertBox();

                HotelalertBox.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("Please start the server first..", "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}