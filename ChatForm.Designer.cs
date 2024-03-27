namespace ChatClient
{
    partial class ChatForm
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
            textBox_message = new TextBox();
            button_send = new Button();
            listView_messages = new ListView();
            button_connect = new Button();
            label_status = new Label();
            textBox_ip = new TextBox();
            textBox_port = new TextBox();
            _label_ip = new Label();
            checkBox_host = new CheckBox();
            checkBox_autohost = new CheckBox();
            SuspendLayout();
            // 
            // textBox_message
            // 
            textBox_message.Location = new Point(12, 677);
            textBox_message.Name = "textBox_message";
            textBox_message.Size = new Size(1072, 39);
            textBox_message.TabIndex = 0;
            textBox_message.KeyDown += textBox_message_KeyDown;
            // 
            // button_send
            // 
            button_send.Enabled = false;
            button_send.Location = new Point(1094, 675);
            button_send.Name = "button_send";
            button_send.Size = new Size(116, 41);
            button_send.TabIndex = 1;
            button_send.Text = "Send";
            button_send.UseVisualStyleBackColor = true;
            button_send.Click += button_send_Click;
            // 
            // listView_messages
            // 
            listView_messages.Location = new Point(12, 12);
            listView_messages.Name = "listView_messages";
            listView_messages.Size = new Size(1198, 596);
            listView_messages.TabIndex = 2;
            listView_messages.UseCompatibleStateImageBehavior = false;
            listView_messages.View = View.List;
            // 
            // button_connect
            // 
            button_connect.Location = new Point(12, 614);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(184, 57);
            button_connect.TabIndex = 3;
            button_connect.Text = "Connect";
            button_connect.UseVisualStyleBackColor = true;
            button_connect.Click += button_connect_Click;
            // 
            // label_status
            // 
            label_status.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label_status.AutoSize = true;
            label_status.Location = new Point(1037, 623);
            label_status.Name = "label_status";
            label_status.RightToLeft = RightToLeft.Yes;
            label_status.Size = new Size(173, 32);
            label_status.TabIndex = 4;
            label_status.Text = "Not connected";
            label_status.TextAlign = ContentAlignment.TopRight;
            // 
            // textBox_ip
            // 
            textBox_ip.Location = new Point(208, 623);
            textBox_ip.Name = "textBox_ip";
            textBox_ip.PlaceholderText = "IP Adress";
            textBox_ip.Size = new Size(178, 39);
            textBox_ip.TabIndex = 5;
            textBox_ip.Text = "127.0.0.1";
            textBox_ip.TextChanged += textBox_ip_TextChanged;
            // 
            // textBox_port
            // 
            textBox_port.Location = new Point(417, 623);
            textBox_port.Name = "textBox_port";
            textBox_port.PlaceholderText = "Port";
            textBox_port.Size = new Size(73, 39);
            textBox_port.TabIndex = 6;
            textBox_port.Text = "42069";
            textBox_port.TextChanged += textBox_port_TextChanged;
            // 
            // _label_ip
            // 
            _label_ip.AutoSize = true;
            _label_ip.Location = new Point(392, 626);
            _label_ip.Name = "_label_ip";
            _label_ip.Size = new Size(19, 32);
            _label_ip.TabIndex = 7;
            _label_ip.Text = ":";
            // 
            // checkBox_host
            // 
            checkBox_host.AutoSize = true;
            checkBox_host.Location = new Point(533, 626);
            checkBox_host.Name = "checkBox_host";
            checkBox_host.Size = new Size(95, 36);
            checkBox_host.TabIndex = 8;
            checkBox_host.Text = "Host";
            checkBox_host.UseVisualStyleBackColor = true;
            checkBox_host.CheckedChanged += checkBox_host_CheckedChanged;
            // 
            // checkBox_autohost
            // 
            checkBox_autohost.AutoSize = true;
            checkBox_autohost.Checked = true;
            checkBox_autohost.CheckState = CheckState.Checked;
            checkBox_autohost.Location = new Point(634, 627);
            checkBox_autohost.Name = "checkBox_autohost";
            checkBox_autohost.Size = new Size(97, 36);
            checkBox_autohost.TabIndex = 9;
            checkBox_autohost.Text = "Auto";
            checkBox_autohost.UseVisualStyleBackColor = true;
            checkBox_autohost.CheckedChanged += checkBox_autohost_CheckedChanged;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1222, 728);
            Controls.Add(checkBox_autohost);
            Controls.Add(checkBox_host);
            Controls.Add(_label_ip);
            Controls.Add(textBox_port);
            Controls.Add(textBox_ip);
            Controls.Add(label_status);
            Controls.Add(button_connect);
            Controls.Add(listView_messages);
            Controls.Add(button_send);
            Controls.Add(textBox_message);
            Name = "ChatForm";
            Text = "Chat - Stefan Strableg";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox_message;
        private Button button_send;
        private ListView listView_messages;
        private Button button_connect;
        private Label label_status;
        private TextBox textBox_ip;
        private TextBox textBox_port;
        private Label _label_ip;
        private CheckBox checkBox_host;
        private CheckBox checkBox_autohost;
    }
}