using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ProjectCars
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        public string Port {
            get {
                return textBoxPort.Text;
            }
            set {
                textBoxPort.Text = value;
            }
        }

        public string IPAddress
        {
            get
            {
                return textBoxIPAddress.Text;
            }
            set
            {
                textBoxIPAddress.Text = value;
            }
        }

        public ProjectCars.ConnectionType ConnectionType
        {
            get {
                ProjectCars.ConnectionType result = ProjectCars.ConnectionType.SharedMemory;
                if (radioButtonSharedMemory.Checked)
                {
                    result = ProjectCars.ConnectionType.SharedMemory;
                }
                else
                {
                    result = ProjectCars.ConnectionType.UDP;
                }

                return result;
            }
            set {
                if (value == ProjectCars.ConnectionType.SharedMemory)
                {
                    radioButtonSharedMemory.Checked = true;
                    radioButtonUDP.Checked = false;
                }
                else
                {
                    radioButtonSharedMemory.Checked = false;
                    radioButtonUDP.Checked = true;
                }
                UpdateForm();
            }
        }

        private void UpdateForm()
        {
            if (radioButtonSharedMemory.Checked)
            {
                labelNotes.Text = "Please ensure that the Shared Memory option within Project Cars has been enabled";
                textBoxIPAddress.Enabled = false;
                textBoxPort.Enabled = false;
            }
            else
            {
                labelNotes.Text = "Please ensure that the IP Address and Port are correctly configures. Note that you should not need to change the port number from 5606";
                textBoxIPAddress.Enabled = true;
                textBoxPort.Enabled = true;
            }
        }

        private bool ValidateForm()
        {
            bool result = true;
            if (IsPort(textBoxPort.Text))
            {
                if (IsIPAddress(textBoxIPAddress.Text))
                {

                }
                else
                {
                    MessageBox.Show("Please enter a valid IP Address", "IP Address is invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = false;
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid port number between 1 - 65535", "Port number is invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }
            return result;
        }

        private bool IsIPAddress(string value)
        {
            IPAddress address;
            bool result = false;

            if (System.Net.IPAddress.TryParse(value, out address))
            {
                result = true;
            }

            return result;
        }

        private bool IsPort(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            Regex numeric = new Regex(@"^[0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (numeric.IsMatch(value))
            {
                try
                {
                    if (Convert.ToInt32(value) < 65536)
                        return true;
                }
                catch (OverflowException)
                {
                }
            }

            return false;
        }
        
        private void radioButtonSharedMemory_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void radioButtonUDP_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
