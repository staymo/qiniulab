﻿using Qiniu.Processing;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace QiniuLab
{
    /// <summary>
    /// Interaction logic for ResourcePfopPage.xaml
    /// </summary>
    public partial class ResourcePfopPage : Page
    {
        private string prefopResultTemplate;
        public ResourcePfopPage()
        {
            InitializeComponent();
            this.init();
        }

        private void init()
        {
            using (StreamReader sr = new StreamReader("Template/JsonFormat.html"))
            {
                prefopResultTemplate = sr.ReadToEnd();
            }
        }

        private void PfopButton_Click(object sender, RoutedEventArgs e)
        {
            string bucket = this.BucketTextBox.Text.Trim();
            string key = this.KeyTextBox.Text.Trim();
            string fops = this.FopsTextBox.Text.Trim();
            if (bucket.Length == 0 || key.Length == 0 || fops.Length == 0)
            {
                return;
            }
            #region FIX_PFOP_ZONE_CONFIG
            try
            {
                Qiniu.Common.ZoneInfo zoneInfo = new Qiniu.Common.ZoneInfo();
                zoneInfo.ConfigZone(AppSettings.Default.ACCESS_KEY, bucket);
                this.PfopResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.PfopResponseTextBox.Text = "配置出错，请检查您的输入(如scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_PFOP_ZONE_CONFIG
            string pipeline = this.PipelineTextBox.Text.Trim();
            bool force = this.ForceCheckBox.IsChecked.Value;
            string notifyURL = this.NotifyURLTextBox.Text.Trim();

            Task.Factory.StartNew(() =>
            {
                Mac mac = new Mac(QiniuLab.AppSettings.Default.ACCESS_KEY,
                    QiniuLab.AppSettings.Default.SECRET_KEY);
                Pfop pfop = new Pfop(mac);
                PfopResult pfopResult = pfop.pfop(bucket, key, fops.Split(';'), pipeline, notifyURL, force);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (pfopResult.PersistentId != null)
                    {
                        this.PersistentIdTextBox.Text = pfopResult.PersistentId;
                    }

                    this.PfopResponseTextBox.Text = pfopResult.Response;
                    this.PfopResponseInfoTextBox.Text = pfopResult.ResponseInfo.ToString();
                }));
            });
        }

        private void PrefopButton_Click(object sender, RoutedEventArgs e)
        {
            string persistentId = this.PersistentIdTextBox.Text.Trim();
            if (persistentId.Length == 0)
            {
                return;
            }
            Task.Factory.StartNew(() =>
            {
                Prefop prefop = new Prefop(persistentId);
                PrefopResult result = prefop.prefop();
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.PrefopResponseTextBox.Text = result.Response;
                    this.PrefopResponseInfoTextBox.Text = result.ResponseInfo.ToString();
                    string formattedResponse = this.prefopResultTemplate.Replace("#{RESPONSE}#", result.Response);
                    this.PrefopFormatResponseWebBrowser.NavigateToString(formattedResponse);
                }));
            });
        }

        private void OnPersistentId_Changed(object sender, TextChangedEventArgs e)
        {
            this.PrefopResponseInfoTextBox.Text = "";
            this.PrefopResponseTextBox.Text = "";
        }
    }
}
