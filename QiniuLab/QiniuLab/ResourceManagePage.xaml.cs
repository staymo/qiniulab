﻿using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Storage.Model;
using Qiniu.Util;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QiniuLab
{
    /// <summary>
    /// Interaction logic for ResourceManagePage.xaml
    /// </summary>
    public partial class ResourceManagePage : Page
    {
        private string jsonResultTemplate;
        private BucketManager bucketManager;
        public ResourceManagePage()
        {
            InitializeComponent();
        }

        private void init()
        {
            using (StreamReader sr = new StreamReader("Template/JsonFormat.html"))
            {
                jsonResultTemplate = sr.ReadToEnd();
            }

            Mac mac = new Mac(QiniuLab.AppSettings.Default.ACCESS_KEY,
                    QiniuLab.AppSettings.Default.SECRET_KEY);
            this.bucketManager = new BucketManager(mac);
        }

        private void StatButton_Click(object sender, RoutedEventArgs e)
        {
            string statBucket = this.StatBucketTextBox.Text.Trim();
            string statKey = this.StatKeyTextBox.Text.Trim();
            if (statBucket.Length == 0 || statKey.Length == 0)
            {
                return;
            }
            #region FIX_STAT_ZONE_CONFIG
            try
            {
                Qiniu.Common.ZoneInfo zoneInfo = new Qiniu.Common.ZoneInfo();
                zoneInfo.ConfigZone(AppSettings.Default.ACCESS_KEY, statBucket);
                this.StatResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.StatResponseTextBox.Text = "配置出错，请检查您的输入(如scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_STAT_ZONE_CONFIG
            Task.Factory.StartNew(() =>
            {        
                StatResult statResult = bucketManager.stat(statBucket, statKey);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.StatResponseTextBox.Text = statResult.Response;
                    this.StatResponseInfoTextBox.Text = statResult.ResponseInfo.ToString();
                    if (!string.IsNullOrEmpty(statResult.Response))
                    {
                        string formattedResponse = this.jsonResultTemplate.Replace("#{RESPONSE}#", statResult.Response);
                        this.StatFormatResponseWebBrowser.NavigateToString(formattedResponse);
                    }
                    else
                    {
                        this.StatFormatResponseWebBrowser.NavigateToString("NO RESPONSE");
                    }
                }));
            });
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            string srcBucket = this.CopySrcBucketTextBox.Text.Trim();
            string srcKey = this.CopySrcKeyTextBox.Text.Trim();
            string destBucket = this.CopyDestBucketTextBox.Text.Trim();
            string destKey = this.CopyDestKeyTextBox.Text.Trim();
            if (srcBucket.Length == 0 || srcKey.Length == 0 ||
                destBucket.Length == 0 || destKey.Length == 0)
            {
                return;
            }
            #region FIX_COPY_ZONE_CONFIG
            try
            {
                Qiniu.Common.ZoneInfo zoneInfo = new Qiniu.Common.ZoneInfo();
                zoneInfo.ConfigZone(AppSettings.Default.ACCESS_KEY, srcBucket); // ???
                this.CopyResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.CopyResponseTextBox.Text = "配置出错，请检查您的输入(如scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_COPY_ZONE_CONFIG
            Task.Factory.StartNew(() =>
            {
               HttpResult copyResult = this.bucketManager.copy(srcBucket, srcKey, destBucket, destKey);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.CopyResponseTextBox.Text = copyResult.Response;
                    this.CopyResponseInfoTextBox.Text = copyResult.ResponseInfo.ToString();
                    if (!string.IsNullOrEmpty(copyResult.Response))
                    {
                        string formattedResponse = this.jsonResultTemplate.Replace("#{RESPONSE}#", copyResult.Response);
                        this.CopyFormatResponseWebBrowser.NavigateToString(formattedResponse);
                    }
                    else
                    {
                        this.CopyFormatResponseWebBrowser.NavigateToString("NO RESPONSE");
                    }
                }));
            });
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            string srcBucket = this.MoveSrcBucketTextBox.Text.Trim();
            string srcKey = this.MoveSrcKeyTextBox.Text.Trim();
            string destBucket = this.MoveDestBucketTextBox.Text.Trim();
            string destKey = this.MoveDestKeyTextBox.Text.Trim();
            if (srcBucket.Length == 0 || srcKey.Length == 0 ||
                destBucket.Length == 0 || destKey.Length == 0)
            {
                return;
            }
            #region FIX_MOVE_ZONE_CONFIG
            try
            {
                Qiniu.Common.ZoneInfo zoneInfo = new Qiniu.Common.ZoneInfo();
                zoneInfo.ConfigZone(AppSettings.Default.ACCESS_KEY, srcBucket); // ???
                this.MoveResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.MoveResponseTextBox.Text = "配置出错，请检查您的输入(如scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_MOVE_ZONE_CONFIG
            Task.Factory.StartNew(() =>
            {
                HttpResult moveResult = this.bucketManager.move(srcBucket, srcKey, destBucket, destKey);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.MoveResponseTextBox.Text = moveResult.Response;
                    this.MoveResponseInfoTextBox.Text = moveResult.ResponseInfo.ToString();
                    if (!string.IsNullOrEmpty(moveResult.Response))
                    {
                        string formattedResponse = this.jsonResultTemplate.Replace("#{RESPONSE}#", moveResult.Response);
                        this.MoveFormatResponseWebBrowser.NavigateToString(formattedResponse);
                    }
                    else
                    {
                        this.MoveFormatResponseWebBrowser.NavigateToString("NO RESPONSE");
                    }
                }));
            });
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string deleteBucket = this.DeleteBucketTextBox.Text.Trim();
            string deleteKey = this.DeleteKeyTextBox.Text.Trim();
            if (deleteBucket.Length == 0 || deleteKey.Length == 0)
            {
                return;
            }
            #region FIX_DEL_ZONE_CONFIG
            try
            {
                Qiniu.Common.ZoneInfo zoneInfo = new Qiniu.Common.ZoneInfo();
                zoneInfo.ConfigZone(AppSettings.Default.ACCESS_KEY, deleteBucket);
                this.DeleteResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.DeleteResponseTextBox.Text = "配置出错，请检查您的输入(如scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_DEL_ZONE_CONFIG
            Task.Factory.StartNew(() =>
            {
                HttpResult deleteResult = this.bucketManager.delete(deleteBucket, deleteKey);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.DeleteResponseTextBox.Text = deleteResult.Response;
                    this.DeleteResponseInfoTextBox.Text = deleteResult.ResponseInfo.ToString();
                    if (!string.IsNullOrEmpty(deleteResult.Response))
                    {
                        string formattedResponse = this.jsonResultTemplate.Replace("#{RESPONSE}#", deleteResult.Response);
                        this.DeleteFormatResponseWebBrowser.NavigateToString(formattedResponse);
                    }
                    else
                    {
                        this.DeleteFormatResponseWebBrowser.NavigateToString("NO RESPONSE");
                    }
                }));
            });
        }

        private void ChgmButton_Click(object sender, RoutedEventArgs e)
        {
            string chgmBucket = this.ChgmBucketTextBox.Text.Trim();
            string chgmKey = this.ChgmKeyTextBox.Text.Trim();
            string chgmMimeType = this.ChgmMimeTypeTextBox.Text.Trim();
            if (chgmBucket.Length == 0 || chgmKey.Length == 0 || chgmMimeType.Length == 0)
            {
                return;
            }
            #region FIX_CHGM_ZONE_CONFIG
            try
            {
                Qiniu.Common.ZoneInfo zoneInfo = new Qiniu.Common.ZoneInfo();
                zoneInfo.ConfigZone(AppSettings.Default.ACCESS_KEY, chgmBucket);
                this.ChgmResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.ChgmResponseTextBox.Text = "配置出错，请检查您的输入(如scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_CHGM_ZONE_CONFIG
            Task.Factory.StartNew(() =>
            {
                HttpResult chgmResult = this.bucketManager.chgm(chgmBucket, chgmKey, chgmMimeType);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.ChgmResponseTextBox.Text = chgmResult.Response;
                    this.ChgmResponseInfoTextBox.Text = chgmResult.ResponseInfo.ToString();
                    if (!string.IsNullOrEmpty(chgmResult.Response))
                    {
                        string formattedResponse = this.jsonResultTemplate.Replace("#{RESPONSE}#", chgmResult.Response);
                        this.ChgmFormatResponseWebBrowser.NavigateToString(formattedResponse);
                    }
                    else
                    {
                        this.ChgmFormatResponseWebBrowser.NavigateToString("NO RESPONSE");
                    }
                }));
            });
        }

        private void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            string url = this.FetchUrlTextBox.Text.Trim();
            string bucket = this.FetchBucketTextBox.Text.Trim();
            string key = this.FetchKeyTextBox.Text.Trim();
            if (url.Length == 0 || bucket.Length == 0)
            {
                return;
            }
            if (key.Length == 0)
            {
                key = null;
            }
            #region FIX_FETCH_ZONE_CONFIG
            try
            {
                Qiniu.Common.ZoneInfo zoneInfo = new Qiniu.Common.ZoneInfo();
                zoneInfo.ConfigZone(AppSettings.Default.ACCESS_KEY, bucket);
                this.FetchResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.FetchResponseTextBox.Text = "配置出错，请检查您的输入(如scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_FETCH_ZONE_CONFIG
            Task.Factory.StartNew(() =>
            {
                HttpResult fetchResult = this.bucketManager.fetch(url, bucket, key);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.FetchResponseTextBox.Text = fetchResult.Response;
                    this.FetchResponseInfoTextBox.Text = fetchResult.ResponseInfo.ToString();
                    if (!string.IsNullOrEmpty(fetchResult.Response))
                    {
                        string formattedResponse = this.jsonResultTemplate.Replace("#{RESPONSE}#", fetchResult.Response);
                        this.FetchFormatResponseWebBrowser.NavigateToString(formattedResponse);
                    }
                    else
                    {
                        this.FetchFormatResponseWebBrowser.NavigateToString("NO RESPONSE");
                    }
                }));
            });
        }

        private void PrefetchButton_Click(object sender, RoutedEventArgs e)
        {
            string prefetchBucket = this.PrefetchBucketTextBox.Text.Trim();
            string prefetchKey = this.PrefetchKeyTextBox.Text.Trim();
            if (prefetchBucket.Length == 0 || prefetchKey.Length == 0)
            {
                return;
            }
            #region FIX_PREF_ZONE_CONFIG
            try
            {
                Qiniu.Common.ZoneInfo zoneInfo = new Qiniu.Common.ZoneInfo();
                zoneInfo.ConfigZone(AppSettings.Default.ACCESS_KEY, prefetchBucket);
                this.PrefetchResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.PrefetchResponseTextBox.Text = "配置出错，请检查您的输入(如scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_PREF_ZONE_CONFIG
            Task.Factory.StartNew(() =>
            {
                HttpResult prefetchResult = this.bucketManager.prefetch(prefetchBucket, prefetchKey);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.PrefetchResponseTextBox.Text = prefetchResult.Response;
                    this.PrefetchResponseInfoTextBox.Text = prefetchResult.ResponseInfo.ToString();
                    if (!string.IsNullOrEmpty(prefetchResult.Response))
                    {
                        string formattedResponse = this.jsonResultTemplate.Replace("#{RESPONSE}#", prefetchResult.Response);
                        this.PrefetchFormatResponseWebBrowser.NavigateToString(formattedResponse);
                    }
                    else
                    {
                        this.PrefetchFormatResponseWebBrowser.NavigateToString("NO RESPONSE");
                    }
                }));
            });
        }

        private void BatchButton_Click(object sender, RoutedEventArgs e)
        {
            //
            // 在SDK的BucketManager中,batch(string[] ops)存在问题,等待修复
            // (BUG原因: '/','='被替换为%2F,%3D)
            //    
            // 而直接使用batch(string ops)则没有问题,因此暂时使用此方法
            //
            // NOTES by fengyh 2016-08-17 16:20 
            //
            string ops = this.BatchOpsTextBox.Text.Trim(new char[] { ' ', '\r', '\n' }); // 移除前导/末尾空白/换行
            if (ops.Length == 0)
            {
                return;
            }
            string ops1 = ops.Replace(" ", ""); // 清除内部空白
            string ops2 = ops1.Replace("\r\n\r\n", "\r\n"); // 清除内部空行
            string ops3 = ops2.Replace("\r\n\r\n", "\r\n"); // 清除单个回车造成的换行
            string opsa = "op=" + ops3.Replace("\r\n","&op=");
            Task.Factory.StartNew(() =>
           {
               HttpResult batchResult = this.bucketManager.batch(opsa);
               Dispatcher.BeginInvoke((Action)(() =>
               {
                   this.BatchResponseTextBox.Text = batchResult.Response;
                   this.BatchResponseInfoTextBox.Text = batchResult.ResponseInfo.ToString();
                   if (!string.IsNullOrEmpty(batchResult.Response))
                   {
                       string formattedResponse = this.jsonResultTemplate.Replace("#{RESPONSE}#", batchResult.Response);
                       this.BatchFormatResponseWebBrowser.NavigateToString(formattedResponse);
                   }
                   else
                   {
                       this.BatchFormatResponseWebBrowser.NavigateToString("NO RESPONSE");
                   }
               }));
           });
        }

        private void StatCmdButton_Click(object sender, RoutedEventArgs e)
        {
            string statBucket = this.StatBucketTextBox.Text.Trim();
            string statKey = this.StatKeyTextBox.Text.Trim();
            string op =this.bucketManager.statOp(statBucket, statKey);
            this.StatCmdResultTextBox.Text = op;
        }

        private void CopyCmdButton_Click(object sender, RoutedEventArgs e)
        {
            string srcBucket = this.CopySrcBucketTextBox.Text.Trim();
            string srcKey = this.CopySrcKeyTextBox.Text.Trim();
            string destBucket = this.CopyDestBucketTextBox.Text.Trim();
            string destKey = this.CopyDestKeyTextBox.Text.Trim();
            bool force = (bool)this.CopyForceOverwrite.IsChecked; // ADD 'force' 2016-08-17 16:17
            string op = this.bucketManager.copyOp(srcBucket, srcKey, destBucket, destKey,force); // 'force'
            this.CopyCmdResultTextBox.Text = op;
        }

        private void MoveCmdButton_Click(object sender, RoutedEventArgs e)
        {
            string srcBucket = this.MoveSrcBucketTextBox.Text.Trim();
            string srcKey = this.MoveSrcKeyTextBox.Text.Trim();
            string destBucket = this.MoveDestBucketTextBox.Text.Trim();
            string destKey = this.MoveDestKeyTextBox.Text.Trim();
            bool force = (bool)this.CopyForceOverwrite.IsChecked; // ADD 'force' 2016-08-17 16:17
            string op = this.bucketManager.moveOp(srcBucket, srcKey, destBucket, destKey,force); // 'force'
            this.MoveCmdResultTextBox.Text = op;
        }

        private void DeleteCmdButton_Click(object sender, RoutedEventArgs e)
        {
            string deleteBucket = this.DeleteBucketTextBox.Text.Trim();
            string deleteKey = this.DeleteKeyTextBox.Text.Trim();
            string op = this.bucketManager.deleteOp(deleteBucket, deleteKey);
            this.DeleteCmdResultTextBox.Text = op;
        }

        private void ChgmCmdButton_Click(object sender, RoutedEventArgs e)
        {
            string chgmBucket = this.ChgmBucketTextBox.Text.Trim();
            string chgmKey = this.ChgmKeyTextBox.Text.Trim();
            string chgmMimeType = this.ChgmMimeTypeTextBox.Text.Trim();
            string op =this.bucketManager.chgmOp(chgmBucket, chgmKey, chgmMimeType);
            this.ChgmCmdResultTextBox.Text = op;
        }

        private void OnLoaded_EventHandler(object sender, RoutedEventArgs e)
        {
            this.init();
        }
    }
}