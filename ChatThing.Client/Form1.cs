using ChatThing.Lib.Constants;
using ChatThing.Lib.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Security.Cryptography.Xml;

namespace ChatThing.Client
{
    public partial class Form1 : Form
    {
        private HubConnection _connection;
        private Guid _id = Guid.NewGuid();
        private string _name;
        private Dictionary<Guid, ClientModel> _clients = new();
        private Aes _aes;
        private RSACryptoServiceProvider _rsa = new();
        public Form1()
        {
            InitializeComponent();
            _aes = Aes.Create();
            _name = _id.ToString().Substring(0, 7);
            _connection = new HubConnectionBuilder().WithUrl("https://localhost:7219/hub").Build();
            _connection.On<string, byte[], DateTime, Guid>(HubConstants.RECEIVE_MESSAGE, ReceiveMessage);
            _connection.On<ClientModel>(HubConstants.RECEIVE_NEW_USER_INFO, ReceiveInfo);
            _connection.On<ConnectionModel>(HubConstants.GET_AES, GetAes);
            _connection.On<ConnectionModel>(HubConstants.ACK_AES, AckAes);
            _connection.StartAsync();
            Start();
        }

        private async void Start()
        {
            RSAParameters publicKey = _rsa.ExportParameters(false);
            Debug($"{_id} connecting");
            await _connection.InvokeAsync(HubConstants.CONNECT, new ClientModel() { Name = _name, Id = _id, SomeThing = publicKey.Modulus, OtherThing = publicKey.Exponent});
        }
        private void ReceiveMessage(string name, byte[] message, DateTime time, Guid id)
        {
            if(id == _id)
            {
                return;
            }
            if (_tbText.InvokeRequired)
            {
                _tbText.Invoke(() => { ReceiveMessage(name, message, time, id); });
            }
            else
            {
                string decrypted = null;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = _clients[id].SomeThing;
                    aesAlg.IV = _clients[id].OtherThing;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(message))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                decrypted= srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                _tbText.Text += $"{name} at {time.ToString("T")}\r\n";
                _tbText.Text += $"{decrypted}\r\n";
            }
        }
        private void Debug(string message)
        {
            if (_tbDebug.InvokeRequired)
            {
                _tbDebug.Invoke(() => { Debug(message); });
            }
            else
            {
                _tbDebug.Text += $"> {message}\r\n";
            }
        }
        private async void ReceiveInfo(ClientModel model)
        {
            Debug($"{_id} receiving info from {model.Id}");
            _clients.Add(model.Id, model);
            RSAParameters par = new();
            par.Exponent = model.OtherThing;
            par.Modulus = model.SomeThing;
            _rsa.ImportParameters(par);
            ConnectionModel modelC = new();
            modelC.EncryptedKey = _rsa.Encrypt(_aes.Key, false);
            modelC.IV = _aes.IV;
            modelC.SenderId = _id;
            modelC.ReceiverId = model.Id;
            await _connection.InvokeAsync(HubConstants.SEND_AES, modelC);
            Debug($"{_id} send their Aes to {model.Id}");
            //at this point that fella has this fella's Aes
        }
        private async void GetAes(ConnectionModel model)
        {
            if(model.ReceiverId == _id)
            {
                Debug($"{_id} got Aes from {model.SenderId}");
                var decryptedKey = _rsa.Decrypt(model.EncryptedKey, false);
                _clients.Add(model.SenderId, new ClientModel() {Id = model.SenderId, SomeThing = decryptedKey, OtherThing = model.IV });
                ICryptoTransform encryptor = _aes.CreateEncryptor(decryptedKey, model.IV);
                byte[] encrypted;
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(_aes.Key, 0, _aes.Key.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                }
                ConnectionModel con = new();
                con.SenderId = _id;
                con.ReceiverId = model.SenderId;
                con.EncryptedKey = encrypted;
                con.IV = _aes.IV;
                _connection.InvokeAsync(HubConstants.HANDSHAKE_AES, con);
                Debug($"{_id} sent Aes to {model.SenderId}");
            }
        }
        private async void AckAes(ConnectionModel model)
        {
            if(model.ReceiverId == _id)
            {
                Debug($"{_id} received Aes from {model.SenderId}");
                byte[] decrypted;
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = _aes.Key;
                    aesAlg.IV = _aes.IV;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream())
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                        {
                            csDecrypt.Write(model.EncryptedKey, 0, model.EncryptedKey.Length);
                            csDecrypt.FlushFinalBlock();
                            decrypted = msDecrypt.ToArray();
                        }
                    }
                }
                _clients[model.SenderId].SomeThing = decrypted;
                _clients[model.SenderId].OtherThing = model.IV;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(_tbName.Text))
            {
                _tbName.Text = _name;
            }
            ICryptoTransform encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);
            byte[] encrypted;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(_tbMessage.Text);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            await _connection.InvokeAsync(HubConstants.SEND_MESSAGE, _tbName.Text, encrypted, DateTime.UtcNow, _id);
        }

        private void _tbName_TextChanged(object sender, EventArgs e)
        {
            _name = _tbName.Text;
        }
    }
}