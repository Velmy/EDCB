using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using CtrlCmdCLI;
using CtrlCmdCLI.Def;
using EpgTimer.EpgView;

namespace EpgTimer.EpgView
{
    /// <summary>
    /// ServiceView.xaml の相互作用ロジック
    /// </summary>
    public partial class ServiceView : UserControl
    {
        Dictionary<UInt64, EpgServiceItem> serviceList = new Dictionary<ulong,EpgServiceItem>();

        public ServiceView()
        {
            InitializeComponent();
        }

        public void ClearInfo()
        {
            services.Children.Clear();
        }

        public void SetService(Dictionary<UInt64, EpgServiceItem> servicelist)
        {
            serviceList = servicelist;
            services.Children.Clear();
            double totalWidth = 0;
            foreach (var pair in serviceList)
            {
                EpgServiceItem info = pair.Value;
                UInt64 key = pair.Key;
                if (info.GroupID != key) continue;
                TextBlock item = new TextBlock();
                item.Text = info.service_name;
                if (info.remote_control_key_id != 0)
                {
                    item.Text += "\r\n" + info.remote_control_key_id.ToString();
                }
                else
                {
                    item.Text += "\r\n" + info.network_name + " " + info.SID.ToString();
                }
                item.Width = info.GroupWidth - 2;
                item.Height = 40 - 2;
                item.Margin = new Thickness(1, 1, 1, 1);
                item.Background = CommonManager.Instance.CustServiceColor;
                item.Foreground = Brushes.White;
                item.TextAlignment = TextAlignment.Center;
                item.FontSize = 12;
                item.MouseEnter += new MouseEventHandler(base_MouseEnter);
                item.DataContext = info;
                Canvas.SetLeft(item, info.LeftPos);
                services.Children.Add(item);
                totalWidth += info.GroupWidth;
            }
            canvas_service.Width = totalWidth;
            canvas_service.Height = 40;
        }

        void base_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(TextBlock))
            {
                popupService.Children.Clear();

                EpgServiceItem info = ((TextBlock)sender).DataContext as EpgServiceItem;

                info = serviceList[info.GroupID];
                popupService.Width = info.GroupWidth;
                Canvas.SetLeft(popupService, info.LeftPos);
                Canvas.SetTop(popupService, 0);
                for (;;)
                {
                    TextBlock item = new TextBlock();

                    item.Text = info.service_name;
                    if (info.Width == popupService.Width)
                    {
                        item.Text += "\r\n" + info.network_name + " " + info.SID.ToString();
                    }
                    else
                    {
                        item.Text += "\r\n" + info.SID.ToString();
                    }
                    item.Width = info.Width - 2;
                    item.Height = 40 - 2;
                    item.Margin = new Thickness(1, 1, 1, 1);
                    item.Background = CommonManager.Instance.CustServiceColor;
                    item.Foreground = Brushes.White;
                    item.TextAlignment = TextAlignment.Center;
                    item.FontSize = 12;
                    item.MouseLeftButtonDown += new MouseButtonEventHandler(item_MouseLeftButtonDown);
                    item.MouseEnter += item_MouseEnter;
                    item.MouseLeave += item_MouseLeave;
                    item.DataContext = info;
                    popupService.Children.Add(item);

                    if (info.GroupNext == 0) break;
                    info = serviceList[info.GroupNext];
                }
                popupService.Visibility = System.Windows.Visibility.Visible;
            }
        }

        void item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (sender.GetType() == typeof(TextBlock))
                {
                    TextBlock item = sender as TextBlock;
                    EpgServiceItem serviceInfo = item.DataContext as EpgServiceItem;
                    CommonManager.Instance.TVTestCtrl.SetLiveCh(serviceInfo.ONID, serviceInfo.TSID, serviceInfo.SID);
                }
            }
        }

        void item_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(TextBlock))
            {
                TextBlock item = sender as TextBlock;
                item.Background = CommonManager.Instance.CustServiceHighlightColor;
            }
        }

        void item_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(TextBlock))
            {
                TextBlock item = sender as TextBlock;
                item.Background = CommonManager.Instance.CustServiceColor;
            }
        }

        private void scrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            popupService.Children.Clear();
            popupService.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
